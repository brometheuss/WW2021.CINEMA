export const getRole = () => {
  const token = localStorage.getItem("jwt");

  if (!token) {
    return false;
  }

  var jwtDecoder = require("jwt-decode");
  const decodedToken = jwtDecoder(token);

  let role =
    decodedToken[
      "http://schemas.microsoft.com/ws/2008/06/identity/claims/role"
    ];

  return role;
};

export const getUserName = () => {
  let decodedToken = getDecodedToken();
  if (!decodedToken) {
    return;
  }

  return decodedToken.sub;
};

export const getDecodedToken = () => {
  var jwtDecoder = require("jwt-decode");
  const token = localStorage.getItem("jwt");

  if (!token) {
    return false;
  }
  return jwtDecoder(token);
};

export const getTokenExp = () => {
  let token = getDecodedToken();
  if (token) {
    return token.exp;
  }
};

export const isAdmin = () => {
  return getRole() === "admin";
};
export const isSuperUser = () => {
  return getRole() === "superUser";
};

export const isUser = () => {
  return getRole() === "user";
};

export const isGuest = () => {
  return getRole() === "guest";
};
