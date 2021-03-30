import React, { useEffect, useState } from "react";
import { NotificationManager } from "react-notifications";
import { serviceConfig } from "../../appSettings";
import { Row } from "react-bootstrap";
import { getUserName, getRole } from "../helpers/authCheck";
import { withRouter } from "react-router";
import { IProjection, IUser, IReservation } from "../../models";

interface IState {
  user: IUser;
  reservations: IReservation[];
  projection: IProjection[];
  submitted: boolean;
}

const UserProfile: React.FC = () => {
  const [state, setState] = useState<IState>({
    user: {
      id: "",
      firstName: "",
      lastName: "",
      points: "",
    },
    reservations: [
      {
        projectionId: "",
        projectionTime: "",

      },
    ],
    projection: [],
    submitted: false,
  });

  useEffect(() => {
    getUserByUsername();
  }, [state.user.id]);

  const getUserByUsername = () => {
    let userName = getUserName();

    const requestOptions = {
      method: "GET",
      headers: {
        "Content-Type": "application/json",
        Authorization: `Bearer ${localStorage.getItem("jwt")}`,
      },
    };

    fetch(
      `${serviceConfig.baseURL}/api/users/byusername/${userName}`,
      requestOptions
    )
      .then((response) => {
        if (!response.ok) {
          return;
        }
        return response.json();
      })
      .then((data) => {
        if (data) {
          setState(prevState => ({ ...prevState, user: data }));
          console.log('USER ID');
          getReservationsByUserId(state.user.id);
          console.log('USER ID');
        }
      })
      .catch((response) => {
        NotificationManager.error(response.message || response.statusText);
        setState({ ...state, submitted: false });
      });
  };

  const getReservationsByUserId = (userId: string) => {
    const requestOptions = {
      method: "GET",
      headers: {
        "Content-Type": "application/json",
        Authorization: `Bearer ${localStorage.getItem("jwt")}`,
      },
    };

    fetch(
      `${serviceConfig.baseURL}/api/reservations/byuserid/${userId}`,
      requestOptions
    )
      .then((response) => {
        if (!response.ok) {
          return;
        }
        return response.json();
      })
      .then((data) => {
        if (data) {
          setState(prevState => ({ ...prevState, reservations: data }));
          state.reservations.map((reservation) => {
            getProjectionById(reservation.projectionId);
          });
        }
      })
      .catch((response) => {
        NotificationManager.error(response.message || response.statusText);
        setState({ ...state, submitted: false });
      });
  };

  const getProjectionById = (projectionId: string) => {
    const requestOptions = {
      method: "GET",
      headers: {
        "Content-Type": "application/json",
        Authorization: `Bearer ${localStorage.getItem("jwt")}`,
      },
    };

    fetch(
      `${serviceConfig.baseURL}/api/projections/byprojectionid/${projectionId}`,
      requestOptions
    )
      .then((response) => {
        if (!response.ok) {
          return;
        }
        return response.json();
      })
      .then((data) => {
        if (data) {
          setState({ ...state, projection: data });
        }
      })
      .catch((response) => {
        NotificationManager.error(response.message || response.statusText);
        setState({ ...state, submitted: false });
      });
  };

  return (
    <React.Fragment>
      <Row className="no-gutters pt-2">
        {state.user.firstName !== "" && state.user.lastName !== "" ? <h1 className="form-header form-heading">
          Hello, {state.user.firstName}!
        </h1> : ""}
      </Row>
      <Row className="no-gutters pr-5 pl-5">
        <div className="card mb-3 user-info-container">
          <div className="row no-gutters">
            <div className="col-md-4">
              <img
                src="https://encrypted-tbn0.gstatic.com/images?q=tbn%3AANd9GcS8tVjlY8BQfSZg9SoudTWMCR6eHXpi-QHhQDUYSyjFmHYOTyyp"
                className="avatar-img"
                alt="..."
              />
            </div>
            <div className="col-md-8">
              <div className="card-body">
                <h5 className="card-title">User details:</h5>
                {state.user.firstName !== "" && state.user.lastName !== "" ? <> <p className="card-text">
                  <strong>Full name:</strong>{" "}
                  {`${state.user.firstName} ${state.user.lastName}`}
                </p>
                  <p className="card-text">
                    <strong>Bonus points: </strong> {state.user.points}
                  </p>
                  <p className="card-text">
                    <strong>Status: </strong> {getRole()}
                  </p> </> : <strong>Please Login</strong>}
              </div>
            </div>
          </div>
        </div>
        <div>
          <ul>
            <li className="list-group-item list-group-item-primary"><strong>Your Reservations: </strong></li>
            {state.reservations.map((reservation) => {
              return (
                <li className="list-group-item">{reservation.projectionTime} - {reservation.movieTitle}, {reservation.auditoriumName}</li>
              )
            })}
          </ul>
        </div>
      </Row>
    </React.Fragment>
  );
};

export default withRouter(UserProfile);
