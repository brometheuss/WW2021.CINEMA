import React, { useEffect } from "react";
import { Route, Redirect } from "react-router-dom";
import { NotificationManager } from "react-notifications";
import * as authCheck from "../helpers/authCheck";

export const PrivateRouteAdminSuperUserAndUser = ({
  component: Component,
  ...rest
}) => {
  useEffect(() => {
    if (authCheck.isSuperUser() || authCheck.isAdmin() || authCheck.isUser()) {
    } else {
      NotificationManager.error("Please log in.");
    }
  });

  return (
    <Route
      {...rest}
      render={(props) =>
        localStorage.getItem("jwt") &&
        (authCheck.isSuperUser() ||
          authCheck.isAdmin() ||
          authCheck.isUser()) ? (
          <Component {...props} />
        ) : (
          <Redirect to={{ pathname: "/", state: { from: props.location } }} />
        )
      }
    />
  );
};
