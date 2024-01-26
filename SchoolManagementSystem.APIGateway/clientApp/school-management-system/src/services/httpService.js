import axios from "axios";
import QueryString from "qs";
import { toast } from "react-toastify";
import { getJwt, setJwt, getUser } from "../utils/user";
import urls from "./../utils/urls.json";
import apiUrl from "../utils/api";

const refreshTokenUrl = apiUrl + urls.accountsRoute + "refreshAccessToken";

axios.interceptors.response.use(null, async (error) => {
  const status = error.response && error.response.status;
  const result = () => Promise.reject(error);

  const unExpectedError = status < 400 || status >= 500;
  if (unExpectedError) {
    console.log(`Logging the error: ${error}`);
    toast.error("An unexpected error occurred.");
    return result();
  }

  if (status === 403) {
    console.log(`Logging: forbidden to access ${error.request.responseURL}`);
    toast.error("You don't have privilege to perform this action.");
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

  try {
    await refreshAccessToken(jwt);
    return axios(originalRequest);
  } catch (exception) {
    logoutApp();
  }

  return result();
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
  if (response && response.status === 200) {
    setJwt(response.data);
    return response.data;
  }
};

export default httpService;