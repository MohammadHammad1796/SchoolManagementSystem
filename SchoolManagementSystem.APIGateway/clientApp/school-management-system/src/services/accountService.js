import httpService from "./httpService";
import urls from "../utils/urls.json";
import apiUrl from "../utils/api";
import { getJwt } from "../utils/user";

class AccountService {
  static #apiUrl = apiUrl + urls.accountsRoute;
  static #registerApiUrl = this.#apiUrl + "register/";
  static #loginApiUrl = this.#apiUrl + "login/";
  static #logoutApiUrl = this.#apiUrl + "logout/";
  static #refreshAccessTokenApiUrl = this.#apiUrl + "refreshAccessToken/";

  static registerAsync = async (data) => {
    return await httpService.post(this.#registerApiUrl, data);
  };

  static loginAsync = async (data) => {
    return await httpService.post(this.#loginApiUrl, data);
  };

  static logoutAsync = async () => {
    return await httpService.get(this.#logoutApiUrl, {
      errorConfig: { handleError: false },
    });
  };

  static refreshAccessTokenAsync = async () => {
    const jwt = getJwt();
    return await httpService.post(this.#refreshAccessTokenApiUrl, {
      accessToken: jwt.accessToken,
      refreshToken: jwt.refreshToken,
    });
  };
}

export default AccountService;
