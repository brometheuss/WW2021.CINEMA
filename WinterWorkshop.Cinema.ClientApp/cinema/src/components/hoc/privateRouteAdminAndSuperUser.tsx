import React, { useEffect } from "react";
import { Route, Redirect } from "react-router-dom";
import { NotificationManager } from "react-notifications";
import * as authCheck from "../helpers/authCheck";

export const PrivateRouteAdminAndSuperUser = ({
  component: Component,
  ...rest
}) => {
  useEffect(() => {
    if (!authCheck.isAdmin() && !authCheck.isSuperUser()) {
      NotificationManager.error("You shall not pass!");
    }
  });

  return (
    <Route
      {...rest}
      render={(props) =>
        localStorage.getItem("jwt") &&
        (authCheck.isAdmin() || authCheck.isSuperUser()) ? (
          <Component {...props} />
        ) : (
          <Redirect to={{ pathname: "/", state: { from: props.location } }} />
        )
      }
    />
  );
};
