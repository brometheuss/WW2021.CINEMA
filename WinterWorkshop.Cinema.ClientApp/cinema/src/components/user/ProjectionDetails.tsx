import React, { useEffect, useState } from "react";
import { NotificationManager } from "react-notifications";
import { serviceConfig } from "../../appSettings";
import { Container, Row, Col, Card } from "react-bootstrap";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faShoppingCart, faCouch } from "@fortawesome/free-solid-svg-icons";
import { getUserName, getRole } from "../helpers/authCheck";
import {
  ICurrentReservationSeats,
  IMovie,
  IProjection,
  IReservedSeats,
  ISeats,
} from "../../models";

interface IState {
  projections: IProjection[];
  movie: IMovie;
  slicedTime: string;
  moviesId: string;
  auditId: string;
  maxRow: number;
  maxNumberOfRow: number;
  inc: number;
  seats: ISeats[];
  inciD: number;
  clicked: boolean;
  projectionId: string;
  currentReservationSeats: ICurrentReservationSeats[];
  reservedSeats: IReservedSeats[];
  userId: string;
  submitted: boolean;
}

const ProjectionDetails: React.FC = () => {
  const [state, setState] = useState<IState>({
    projections: [],
    movie: {
      id: "",
      bannerUrl: "",
      title: "",
      rating: 0,
      year: "",
    },
    slicedTime: "",
    moviesId: "",
    auditId: "",
    maxRow: 0,
    maxNumberOfRow: 0,
    inc: 0,
    seats: [
      {
        id: "",
        number: 0,
        row: 0,
      },
    ],
    inciD: 0,
    clicked: false,
    projectionId: "",
    currentReservationSeats: [
      {
        currentSeatId: "",
      },
    ],
    reservedSeats: [
      {
        seatId: "",
      },
    ],
    userId: "",
    submitted: false,
  });

  let allButtons: any;

  useEffect(() => {
    getMovie();
    getProjection();
    getUserByUsername();
  }, []);

  const getProjection = () => {
    var idFromUrl = window.location.pathname.split("/");
    var id = idFromUrl[3];

    const requestOptions = {
      method: "GET",
      headers: {
        "Content-Type": "application/json",
        Authorization: "Bearer " + localStorage.getItem("jwt"),
      },
    };

    fetch(
      `${serviceConfig.baseURL}/api/projections/byprojectionid/` + id,
      requestOptions
    )
      .then((response) => {
        if (!response.ok) {
          return Promise.reject(response);
        }
        return response.json();
      })
      .then((data) => {
        if (data) {
          setState({
            ...state,
            projections: data,
            auditId: data.auditoriumId,
            slicedTime: data.projectionTime.slice(11, 16),
            moviesId: data.movieId,
          });

          getReservedSeats(requestOptions, id);
          getSeatsForAuditorium(data.auditoriumId);
          getSeats(data.auditoriumId);
        }
      })
      .catch((response) => {
        NotificationManager.error(response.message || response.statusText);
        setState({ ...state, submitted: false });
      });
  };

  const getReservedSeats = (requestOptions: RequestInit, id: string) => {
    const reservedSeats = fetch(
      `${serviceConfig.baseURL}/api/reservations/getbyprojectionid/${id}`,
      requestOptions
    )
      .then((response) => {
        if (!response.ok) {
          return Promise.reject(response);
        }
        return response.json();
      })
      .then((data) => {
        if (data) {
          setState({
            ...state,
            reservedSeats: data,
          });
        }
      })
      .catch((response) => {
        NotificationManager.error(response.message || response.statusText);
        setState({ ...state, submitted: false });
      });
  };

  const getSeatsForAuditorium = (auditId: string) => {
    const requestOptions = {
      method: "GET",
      headers: {
        "Content-Type": "application/json",
        Authorization: `Bearer ${localStorage.getItem("jwt")}`,
      },
    };

    fetch(
      `${serviceConfig.baseURL}/api/seats/numberofseats/${auditId}`,
      requestOptions
    )
      .then((response) => {
        if (!response.ok) {
          return Promise.reject(response);
        }
        return response.json();
      })
      .then((data) => {
        if (data) {
          setState({
            ...state,
            seats: data,
            maxRow: data.maxRow,
            maxNumberOfRow: data.maxNumber,
          });
        }
      })
      .catch((response) => {
        NotificationManager.error(response.message || response.statusText);
        setState({ ...state, submitted: false });
      });
  };

  const getUserByUsername = () => {
    let ussName = getUserName();

    const requestOptions = {
      method: "GET",
      headers: {
        "Content-Type": "application/json",
        Authorization: `Bearer ${localStorage.getItem("jwt")}`,
      },
    };

    fetch(
      `${serviceConfig.baseURL}/api/users/byusername/${ussName}`,
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
          setState({ ...state, userId: data.id });
        }
      })
      .catch((response) => {
        NotificationManager.error(response.message || response.statusText);
        setState({ ...state, submitted: false });
      });
  };

  const getSeats = (auditId: string) => {
    const requestOptions = {
      method: "GET",
      headers: {
        "Content-Type": "application/json",
        Authorization: `Bearer ${localStorage.getItem("jwt")}`,
      },
    };

    fetch(
      `${serviceConfig.baseURL}/api/seats/byauditoriumid/${auditId}`,
      requestOptions
    )
      .then((response) => {
        if (!response.ok) {
          return Promise.reject(response);
        }
        return response.json();
      })
      .then((data) => {
        if (data) {
          setState({
            ...state,
            seats: data as ISeats[],
          });
        }
      })
      .catch((response) => {
        NotificationManager.error(response.message || response.statusText);
        setState({ ...state, submitted: false });
      });
  };

  const getMovie = () => {
    var id = window.location.pathname.split("/")[4];
    const requestOptions = {
      method: "GET",
      headers: {
        "Content-Type": "application/json",
        Authorization: `Bearer ${localStorage.getItem("jwt")}`,
      },
    };

    fetch(`${serviceConfig.baseURL}/api/movies/${id}`, requestOptions)
      .then((response) => {
        if (!response.ok) {
          return Promise.reject(response);
        }
        return response.json();
      })
      .then((data) => {
        if (data) {
          setState({
            ...state,
            movie: data as IMovie,
          });
        }
      })
      .catch((response) => {
        NotificationManager.error(response.message || response.statusText);
        setState({ ...state, submitted: false });
      });
  };

  const tryReservation = (
    e: React.MouseEvent<HTMLButtonElement, MouseEvent>
  ) => {
    e.preventDefault();
    const requestOptions = {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
        Authorization: `Bearer ${localStorage.getItem("jwt")}`,
      },
      body: "",
    };

    fetch(`${serviceConfig.baseURL}/api/levi9payment`, requestOptions)
      .then((response) => {
        if (!response.ok) {
          return Promise.reject(response);
        }
        return response.statusText;
      })
      .then((result) => {
        makeReservation(e);
      })
      .catch((response) => {
        NotificationManager.warning("Insufficient founds.");
        setState({ ...state, submitted: false });
      });
  };

  const makeReservation = (
    e: React.MouseEvent<HTMLButtonElement, MouseEvent>
  ) => {
    e.preventDefault();

    if (
      getRole() === "user" ||
      getRole() === "superUser" ||
      getRole() === "admin"
    ) {
      var idFromUrl = window.location.pathname.split("/");
      var projectionId = idFromUrl[3];

      const { currentReservationSeats } = state;

      const data = {
        projectionId: projectionId,
        seatIds: currentReservationSeats,
        userId: state.userId,
      };

      const requestOptions = {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
          Authorization: `Bearer ${localStorage.getItem("jwt")}`,
        },
        body: JSON.stringify(data),
      };

      fetch(`${serviceConfig.baseURL}/api/reservations`, requestOptions)
        .then((response) => {
          if (!response.ok) {
            return Promise.reject(response);
          }
          return response.statusText;
        })
        .then((result) => {
          NotificationManager.success(
            "Your reservation has been made successfully!"
          );
          setTimeout(() => {
            window.location.reload();
          }, 2000);
        })
        .catch((response) => {
          NotificationManager.error(response.message || response.statusText);
          setState({ ...state, submitted: false });
        });
    } else {
      NotificationManager.error("Please log in to make reservation.");
    }
  };

  const renderRows = (rows: number, seats: number) => {
    const rowsRendered: JSX.Element[] = [];
    if (state.seats.length > 0) {
      for (let i = 0; i < rows; i++) {
        const startingIndex = i * seats;
        const maxIndex = (i + 1) * seats;

        rowsRendered.push(
          <tr key={i}>{renderSeats(seats, i, startingIndex, maxIndex)}</tr>
        );
      }
    }
    return rowsRendered;
  };

  const checkIfSeatIsTaken = (currentSeatId: string) => {
    for (let i = 0; i < state.reservedSeats.length; i++) {
      if (state.reservedSeats[i].seatId === currentSeatId) {
        return true;
      }
    }
    return false;
  };

  const checkIfSeatIsCurrentlyReserved = (currentSeatId) => {
    return state.currentReservationSeats.includes(currentSeatId);
  };

  const getSeatByPosition = (row: number, number: number) => {
    for (let i = 0; i < state.seats.length; i++) {
      if (state.seats[i].number === number && state.seats[i].row === row) {
        return state.seats[i];
      }
    }
  };

  const getSeatById = (seatId: string) => {
    for (let i = 0; i < state.seats.length; i++) {
      if (state.seats[i].id === seatId) {
        return state.seats[i];
      }
    }
  };

  const getAllButtons = () => {
    if (!allButtons) {
      allButtons = document.getElementsByClassName("seat");
      for (let i = 0; i < allButtons.length; i++) {
        let seat = getSeatById(allButtons[i].value);
      }
    }
  };

  const markSeatAsGreenish = (seatId: string) => {
    getAllButtons();
    for (let i = 0; i < allButtons.length; i++) {
      if (seatId === allButtons[i].value) {
        allButtons[i].className = "seat nice-green-color";
      }
    }
  };

  const getButtonBySeatId = (seatId: string) => {
    getAllButtons();
    for (let i = 0; i < allButtons.length; i++) {
      if (seatId === allButtons[i].value) {
        return allButtons[i];
      }
    }
  };

  const markSeatAsBlue = (seatId: string) => {
    getAllButtons();
    for (let i = 0; i < allButtons.length; i++) {
      if (seatId === allButtons[i].value) {
        allButtons[i].className = "seat";
      }
    }
  };

  const markWholeRowSeatsAsBlue = () => {
    getAllButtons();
    for (let i = 0; i < allButtons.length; i++) {
      if (allButtons[i].className !== "seat seat-taken") {
        allButtons[i].className = "seat";
      }
    }
  };

  const renderSeats = (
    seats: number,
    row: number,
    startingIndex: number,
    maxIndex: number
  ) => {
    let renderedSeats: JSX.Element[] = [];
    let seatIndex = startingIndex;
    if (state.seats.length > 0) {
      for (let i = 0; i < seats; i++) {
        let currentSeatId = state.seats[seatIndex].id;
        let currentlyReserved =
          state.currentReservationSeats.filter(
            (seat) => seat.currentSeatId === currentSeatId
          ).length > 0;
        let seatTaken =
          state.reservedSeats.filter((seat) => seat.seatId === currentSeatId)
            .length > 0;

        renderedSeats.push(
          <button
            onClick={(e) => {
              let currentRow = row;
              let currentNumber = i;
              let currSeatId = currentSeatId;

              let leftSeatIsCurrentlyReserved = false;
              let leftSeatIsTaken = false;
              let rightSeatIsCurrentlyReserved = false;
              let rightSeatIsTaken = false;
              let leftSeatProperties = getSeatByPosition(
                currentRow + 1,
                currentNumber
              );
              let rightSeatProperties = getSeatByPosition(
                currentRow + 1,
                currentNumber + 2
              );
              let currentReservationSeats = state.currentReservationSeats;

              if (leftSeatProperties) {
                leftSeatIsCurrentlyReserved = checkIfSeatIsCurrentlyReserved(
                  leftSeatProperties.id
                );
                leftSeatIsTaken = checkIfSeatIsTaken(leftSeatProperties.id);
              }

              if (rightSeatProperties) {
                rightSeatIsCurrentlyReserved = checkIfSeatIsCurrentlyReserved(
                  rightSeatProperties.id
                );
                rightSeatIsTaken = checkIfSeatIsTaken(rightSeatProperties.id);
              }

              if (!checkIfSeatIsCurrentlyReserved(currSeatId)) {
                if (
                  state.currentReservationSeats.length !== 0 &&
                  getButtonBySeatId(currentSeatId).className !==
                    "seat nice-green-color"
                ) {
                  return;
                }
                if (
                  !leftSeatIsCurrentlyReserved &&
                  !leftSeatIsTaken &&
                  leftSeatProperties
                ) {
                  markSeatAsGreenish(leftSeatProperties.id);
                }
                if (
                  !rightSeatIsCurrentlyReserved &&
                  !rightSeatIsTaken &&
                  rightSeatProperties
                ) {
                  markSeatAsGreenish(rightSeatProperties.id);
                }
                if (
                  state.currentReservationSeats.includes({
                    currentSeatId: currentSeatId,
                  }) === false
                ) {
                  currentReservationSeats.push({
                    currentSeatId: currentSeatId,
                  });
                }
              } else {
                if (
                  leftSeatIsCurrentlyReserved &&
                  rightSeatIsCurrentlyReserved
                ) {
                  markWholeRowSeatsAsBlue();
                  currentReservationSeats = [];
                } else {
                  currentReservationSeats.splice(
                    currentReservationSeats.indexOf({
                      currentSeatId: currentSeatId,
                    }),
                    1
                  );
                  if (
                    !leftSeatIsCurrentlyReserved &&
                    !leftSeatIsTaken &&
                    leftSeatProperties
                  ) {
                    markSeatAsBlue(leftSeatProperties.id);
                  }
                  if (
                    !rightSeatIsCurrentlyReserved &&
                    !rightSeatIsTaken &&
                    rightSeatProperties
                  ) {
                    markSeatAsBlue(rightSeatProperties.id);
                  }

                  if (
                    leftSeatIsCurrentlyReserved ||
                    rightSeatIsCurrentlyReserved
                  ) {
                    setTimeout(() => {
                      markSeatAsGreenish(currentSeatId);
                    }, 150);
                  }
                }
                setState({
                  ...state,
                  currentReservationSeats: currentReservationSeats,
                });
              }
              setState({
                ...state,
                currentReservationSeats: currentReservationSeats,
              });
            }}
            className={
              seatTaken
                ? "seat seat-taken"
                : currentlyReserved
                ? "seat seat-current-reservation"
                : "seat"
            }
            value={currentSeatId}
            key={`row${row}-seat${i}`}
          >
            <FontAwesomeIcon className="fa-2x couch-icon" icon={faCouch} />
          </button>
        );
        if (seatIndex < maxIndex) {
          seatIndex += 1;
        }
      }
    }

    return renderedSeats;
  };

  const getRoundedRating = (rating: number) => {
    const result = Math.round(rating);
    return <span className="float-right">Rating: {result}/10</span>;
  };

  const fillTableWithData = () => {
    let auditorium = renderRows(state.maxNumberOfRow, state.maxRow);
    return (
      <Card.Body>
        <Card.Title>
          <span className="card-title-font">{state.movie.title}</span>
          <span className="float-right">
            {getRoundedRating(state.movie.rating)}
          </span>
        </Card.Title>
        <hr />
        <Card.Subtitle className="mb-2 text-muted">
          Year of production: {state.movie.year}
          <span className="float-right">
            Time of projection: {state.slicedTime}h
          </span>
        </Card.Subtitle>
        <hr />
        <Card.Text>
          <Row className="mt-2">
            <Col className="justify-content-center align-content-center">
              <form>
                <div className="payment">
                  <h4 className="text-center">Choose your seat(s):</h4>
                  <table className="payment-table">
                    <thead className="payment-table__head">
                      <tr className="payment-table__row">
                        <th className="payment-table__cell">Ulaznice</th>
                        <th className="payment-table__cell">Cena</th>
                        <th className="payment-table__cell">Ukupno</th>
                      </tr>
                    </thead>
                    <tbody className="payment-table__row">
                      <tr>
                        <td className="payment-table__cell">
                          <span>{state.currentReservationSeats.length}</span>
                        </td>
                        <td className="payment-table__cell">350,00 RSD</td>
                        <td className="payment-table__cell">
                          {state.currentReservationSeats.length * 350},00 RSD
                        </td>
                      </tr>
                    </tbody>
                  </table>
                  <button
                    onClick={(e) => tryReservation(e)}
                    className="btn-payment"
                  >
                    Confirm
                    <FontAwesomeIcon
                      className="text-primary mr-2 fa-1x btn-payment__icon"
                      icon={faShoppingCart}
                    />
                  </button>
                </div>
              </form>
              <div>
                <Row className="justify-content-center">
                  <table className="table-cinema-auditorium">
                    <tbody>{auditorium}</tbody>
                  </table>
                  <div className="text-center text-white font-weight-bold cinema-screen">
                    CINEMA SCREEN
                  </div>
                </Row>
              </div>
            </Col>
          </Row>
          <hr />
        </Card.Text>
      </Card.Body>
    );
  };

  const showTable = fillTableWithData();
  return (
    <Container>
      <Row className="justify-content-center">
        <Col>
          <Card className="mt-5 card-width">{showTable}</Card>
        </Col>
      </Row>
    </Container>
  );
};

export default ProjectionDetails;
