import jwtDecode from "jwt-decode";

const tokenKey = "token";

export function getUser() {
  let jwt = localStorage.getItem(tokenKey);
  let user = null;
  if (jwt) {
    const tokenObject = JSON.parse(jwt);
    user = jwtDecode(tokenObject.accessToken);

    user.roles = user.roles.split(",").map((role) => role);
  }
  return user;
}

export function setJwt(token) {
  token
    ? localStorage.setItem(tokenKey, JSON.stringify(token))
    : localStorage.removeItem(tokenKey);
}

export function getJwt() {
  let jwt = localStorage.getItem(tokenKey);
  return jwt ? JSON.parse(jwt) : null;
}
