import React, { useEffect, useState } from "react";
import { withRouter } from "react-router-dom";
import { Link } from "react-router-dom";
import { Navbar, Nav, Form, FormControl, Button } from "react-bootstrap";
import { NotificationManager } from "react-notifications";
import { serviceConfig } from "../appSettings";
import {
  getTokenExp,
  isGuest,
  getUserName,
} from "../../src/components/helpers/authCheck";

interface IState {
  username: string;
  submitted: boolean;
  token: boolean;
  shouldHide: boolean;
}

const Header: React.FC = (props: any) => {
  const [state, setState] = useState<IState>({
    username: "",
    submitted: false,
    token: false,
    shouldHide: true,
  });

  useEffect(() => {
    setTimeout(() => {
      if (localStorage.getItem("userLoggedIn") !== null) {
        hideLoginButtonElement();
      } else {
        hideLogoutButtonElement();
      }
    }, 500);
  }, []);

  let shouldDisplayUserProfile = false;
  const shouldShowUserProfile = () => {
    if (shouldDisplayUserProfile === undefined) {
      shouldDisplayUserProfile = !isGuest();
    }
    return shouldDisplayUserProfile;
  };

  useEffect(() => {
    let tokenExp = getTokenExp();
    let currentTimestamp = +new Date();
    if (!tokenExp || tokenExp * 1000 < currentTimestamp) {
      getTokenForGuest();
    }
  }, []);

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { id, value } = e.target;
    setState({ ...state, [id]: value });
  };

  const handleSubmit = (e: React.FormEvent<HTMLFormElement>) => {
    e.preventDefault();

    setState({ ...state, submitted: true });
    const { username } = state;
    if (username) {
      login();
    } else {
      setState({ ...state, submitted: false });
    }
  };

  const handleSubmitLogout = (e: React.FormEvent<HTMLFormElement>) => {
    e.preventDefault();
    localStorage.removeItem("userLoggedIn");
    setState({ ...state, submitted: true });
    setState({ ...state, token: false });
    getTokenForGuest();
  };

  const hideLoginButtonElement = () => {
    let loginButton = document.getElementById("login");
    if (loginButton) {
      loginButton.style.display = "none";
    }
    let logoutButton = document.getElementById("logout");
    if (logoutButton) {
      logoutButton.style.display = "block";
    }
    document.getElementById("username")!.style.display = "none";
  };

  const hideLogoutButtonElement = () => {
    let loginButton = document.getElementById("login");

    if (loginButton) {
      loginButton.style.display = "block";
    }
    let logoutButton = document.getElementById("logout");
    if (logoutButton) {
      logoutButton.style.display = "none";
    }
    document.getElementById("username")!.style.display = "block";
  };

  const login = () => {
    localStorage.setItem("userLoggedIn", "true");

    const requestOptions = {
      method: "GET",
      headers: {
        "Content-Type": "application/json",
        Authorization: `Bearer ${localStorage.getItem("jwt")}`,
      },
    };

    fetch(
      `${serviceConfig.baseURL}/api/users/byusername/${state.username}`,
      requestOptions
    )
      .then((response) => {
        if (!response.ok) {
          return Promise.reject(response);
        }
        return response.json();
      })
      .then((data) => {
        setState({ ...state, token: true });
        var isGuest = false;
        if (data.userName) {
          setState({ ...state, shouldHide: false });
          if (!data.isAdmin && !data.isSuperUser && !data.isUser) {
            isGuest = true;
          }
          getToken(data.isAdmin, data.isSuperUser, data.isUser, isGuest);
          NotificationManager.success(`Welcome, ${data.firstName}!`);
        }
      })
      .catch((response) => {
        NotificationManager.error("Username does not exists.");
        setState({ ...state, submitted: false });
      });
  };

  const getToken = (
    IsAdmin: boolean,
    isSuperUser: boolean,
    isUser: boolean,
    isGuest: boolean
  ) => {
    const requestOptions = {
      method: "GET",
    };
    fetch(
      `${serviceConfig.baseURL}/get-token?name=${state.username}&admin=${IsAdmin}&superUser=${isSuperUser}&user=${isUser}&guest=${isGuest}`,
      requestOptions
    )
      .then((response) => {
        if (!response.ok) {
          return Promise.reject(response);
        }
        return response.json();
      })
      .then((data) => {
        if (data.token) {
          localStorage.setItem("jwt", data.token);
          setTimeout(() => {
            window.location.reload();
          }, 500);
        }
      })
      .catch((response) => {
        NotificationManager.error(response.message || response.statusText);
        setState({ ...state, submitted: false });
      });
  };

  const getTokenForGuest = () => {
    const requestOptions = {
      method: "GET",
    };
    fetch(`${serviceConfig.baseURL}/get-token?guest=true`, requestOptions)
      .then((response) => {
        if (!response.ok) {
          return Promise.reject(response);
        }
        return response.json();
      })
      .then((data) => {
        setState({ ...state, shouldHide: true });
        if (data.token) {
          localStorage.setItem("jwt", data.token);
          window.location.reload();
        }
      })
      .catch((response) => {
        NotificationManager.error(response.message || response.statusText);
        setState({ ...state, submitted: false });
      });
    state.token = true;
  };

  const redirectToUserPage = () => {
    props.history.push(`userprofile`);
  };

  const refreshPage = () => {
    window.location.reload(true);
  };

  return (
    <Navbar bg="dark" expand="lg">
      <Navbar.Brand className="text-info font-weight-bold text-capitalize">
        <Link className="text-decoration-none" to="/dashboard/Projection">
          Cinema 9
        </Link>
        <span style={{ color: 'blue' }}> | </span>
        <Link className="text-decoration-none" to="/userprofile">
          Profile
        </Link>
      </Navbar.Brand>
      <Navbar.Toggle aria-controls="basic-navbar-nav" className="text-white" />
      <Navbar.Collapse id="basic-navbar-nav" className="text-white">
        <Nav className="mr-auto text-white"></Nav>
        <Form
          inline
          onSubmit={(e: React.FormEvent<HTMLFormElement>) => handleSubmit(e)}
        >
          <FormControl
            type="text"
            placeholder="Username"
            id="username"
            value={state.username}
            onChange={handleChange}
            className="mr-sm-2"
          />
          <Button type="submit" variant="outline-success" id="login">
            Login
          </Button>
        </Form>
        {shouldShowUserProfile() && (
          <Button
            style={{ backgroundColor: "transparent", marginRight: "10px" }}
            onClick={redirectToUserPage}
          >
            {getUserName()}
          </Button>
        )}
        <Form
          inline
          onSubmit={(e: React.FormEvent<HTMLFormElement>) =>
            handleSubmitLogout(e)
          }
        >
          <Button type="submit" variant="outline-danger" id="logout">
            Logout
          </Button>
        </Form>
      </Navbar.Collapse>
    </Navbar>
  );
};

export default withRouter(Header);
