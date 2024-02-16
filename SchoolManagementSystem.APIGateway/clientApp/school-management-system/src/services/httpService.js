import axios from "axios";
import QueryString from "qs";
import { toast } from "react-toastify";
import { getJwt, setJwt, getUser } from "../utils/user";
import urls from "./../utils/urls.json";
import apiUrl from "../utils/api";

let requestsNeedRefreshToken = [];
let isRefreshTokenInProgress = false;
const refreshTokenUrl = apiUrl + urls.accountsRoute + "refreshAccessToken";

const processRequestsNeedRefreshTokenQueue = ({ error = null } = {}) => {
  requestsNeedRefreshToken.forEach((promise) => {
    if (error) promise.reject(error);
    else promise.resolve();
  });

  requestsNeedRefreshToken = [];
};

axios.interceptors.response.use(null, (error) => {
  const status = error.response && error.response.status;
  const result = () => Promise.reject(error);
  const isDevelopment = process.env.NODE_ENV === "development";

  const networkError = error.code === "ERR_NETWORK";
  if (networkError) {
    if (isDevelopment) {
      toast.error(error.message);
      console.log(`Logging the network error: ${error}`);
    } else {
      toast.error("An unexpected error occurred.");
      // Logging the error some place;
    }

    return result();
  }

  const unExpectedError = status < 400 || status >= 500;
  if (unExpectedError) {
    if (isDevelopment) {
      toast.error(error.message);
      console.log(`Logging the error: ${error}`);
    } else {
      toast.error("Network error occurred.");
      // Logging the error some place;
    }
    return result();
  }

  if (status === 403) {
    if (isDevelopment) {
      toast.error(error.message);
      console.log(`Logging: forbidden to access ${error}`);
    } else {
      toast.error("You don't have privilege to perform this action.");
      // Logging the error some place;
    }
    return result();
  }

  if (status !== 401) return result();

  const path = window.location.pathname;
  if (
    path.toLowerCase().startsWith("/logout") ||
    path.toLowerCase().startsWith("/login")
  )
    return result();

  const originalRequest = error.config;
  if (originalRequest.url === refreshTokenUrl) {
    logoutApp(path);
    return result();
  }

  const jwt = getJwt();
  const shouldRefresh = isTokenValidToRefresh(jwt);
  if (!shouldRefresh) {
    logoutApp(path);
    return result();
  }

  if (isRefreshTokenInProgress)
    return addOriginalRequestToQueue(originalRequest);

  return startRefreshTokenProcess({ jwt, originalRequest });
});

axios.interceptors.request.use((config) => {
  config.paramsSerializer = {
    serialize: (params) =>
      QueryString.stringify(params, {
        arrayFormat: "brackets",
        allowDots: true,
      }),
  };

  let jwt = getJwt();
  if (jwt) config.headers.Authorization = `Bearer ${jwt.accessToken}`;

  return config;
});

const logoutApp = (path) => {
  setJwt();
  localStorage.setItem("loadMessage", "Session expired!");
  window.location.pathname = path || "/";
};

const httpService = {
  get: axios.get,
  post: axios.post,
  put: axios.put,
  delete: axios.delete,
};

const isTokenValidToRefresh = (jwt) => {
  const user = getUser();
  const tokenExpired = user?.exp <= Date.now() / 1000;
  if (!tokenExpired) {
    setJwt();
    return false;
  }

  const refreshTokenExpired = new Date(jwt.refreshExpireAt) <= Date.now();
  if (refreshTokenExpired) {
    setJwt();
    return false;
  }

  return true;
};

const refreshAccessToken = async (jwt) => {
  const response = await axios.post(refreshTokenUrl, {
    accessToken: jwt.accessToken,
    refreshToken: jwt.refreshToken,
  });
  if (response && response.status === 200) setJwt(response.data);
};

const addOriginalRequestToQueue = (originalRequest) => {
  return new Promise((resolve, reject) => {
    requestsNeedRefreshToken.push({ resolve, reject });
  })
    .then(() => {
      return axios(originalRequest);
    })
    .catch((error) => {
      return Promise.reject(error);
    });
};

const startRefreshTokenProcess = ({ jwt, originalRequest }) => {
  isRefreshTokenInProgress = true;

  return new Promise((resolve, reject) => {
    refreshAccessToken(jwt)
      .then(() => {
        processRequestsNeedRefreshTokenQueue();
        resolve(axios(originalRequest));
      })
      .catch((error) => {
        processRequestsNeedRefreshTokenQueue({ error });
        reject(error);
        logoutApp();
      })
      .finally(() => {
        isRefreshTokenInProgress = false;
      });
  });
};

export default httpService;
