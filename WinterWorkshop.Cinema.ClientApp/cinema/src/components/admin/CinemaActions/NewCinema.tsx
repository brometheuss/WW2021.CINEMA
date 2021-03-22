import React, { useState } from "react";
import { withRouter } from "react-router-dom";
import {
  FormGroup,
  FormControl,
  Button,
  Container,
  Row,
  Col,
  FormText,
} from "react-bootstrap";
import { NotificationManager } from "react-notifications";
import { serviceConfig } from "../../../appSettings";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faCouch } from "@fortawesome/free-solid-svg-icons";

interface IState {
  name: string;
  nameError: string;
  auditName: string;
  seatRows: number;
  numberOfSeats: number;
  auditNameError: string;
  seatRowsError: string;
  numOfSeatsError: string;
  submitted: boolean;
  canSubmit: boolean;
  cityName: string;
}

const NewCinema: React.FC = (props: any) => {
  const [state, setState] = useState<IState>({
    name: "",
    nameError: "",
    auditName: "",
    seatRows: 0,
    numberOfSeats: 0,
    cityName: "",
    auditNameError: "",
    seatRowsError: "",
    numOfSeatsError: "",
    submitted: false,
    canSubmit: true,
  });

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { id, value } = e.target;
    setState({ ...state, [id]: value });
    //validate(id, value);
  };

  const validate = (id: string, value: string) => {
    if (id === "name") {
      if (value === "") {
        setState({
          ...state,
          nameError: "Fill in cinema name",
          canSubmit: false,
        });
      } else {
        setState({ ...state, nameError: "", canSubmit: true });
      }
    }
    if (id === "auditName") {
      if (value === "") {
        setState({
          ...state,
          auditNameError: "Fill in auditorium name",
          canSubmit: false,
        });
      } else {
        setState({ ...state, auditNameError: "", canSubmit: true });
      }
    } else if (id === "numberOfSeats") {
      const seatsNum = +value;
      if (seatsNum > 20 || seatsNum < 1) {
        setState({
          ...state,
          numOfSeatsError: "Seats number can be in between 1 and 20.",
          canSubmit: false,
        });
      } else {
        setState({ ...state, numOfSeatsError: "", canSubmit: true });
      }
    } else if (id === "seatRows") {
      const seatsNum = +value;
      if (seatsNum > 20 || seatsNum < 1) {
        setState({
          ...state,
          seatRowsError: "Seats number can be in between 1 and 20.",
          canSubmit: false,
        });
      } else {
        setState({ ...state, seatRowsError: "", canSubmit: true });
      }
    }
  };

  const handleSubmit = (e: React.FormEvent<HTMLFormElement>) => {
    e.preventDefault();

    setState({ ...state, submitted: true });
    if ((state.cityName === "Zrenjanin" || state.cityName === "Novi Sad" || state.cityName === "Beograd") && state.name !== "") {
      addCinema();
    }
    else {
      NotificationManager.error("Cinema inserting rules: City name: 50 characters max, must be Zrenjanin, Novi Sad or Beograd. Name: 50 characters max. Seat Rows: from 1 to 20. Number of seats: from 1 to 20. Auditorium Name: 50 characters max. ");
      setState({ ...state, submitted: false });
    }
  };

  const addCinema = () => {
    const data = {
      Name: state.name,
      numberOfSeats: +state.numberOfSeats,
      seatRows: +state.seatRows,
      auditName: state.auditName,
      cityName: state.cityName
    };

    const requestOptions = {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
        Authorization: `Bearer ${localStorage.getItem("jwt")}`,
      },
      body: JSON.stringify(data),
    };

    fetch(`${serviceConfig.baseURL}/api/cinemas`, requestOptions)
      .then((response) => {
        if (!response.ok) {
          return Promise.reject(response);
        }
        return response.statusText;
      })
      .then((result) => {
        NotificationManager.success("Successfuly added cinema!");
        props.history.push(`AllCinemas`);
      })
      .catch((response) => {
        NotificationManager.error(response.message || response.statusText);
        setState({ ...state, submitted: false });
      });
  };

  const renderRows = (rows: number, seats: number) => {
    const rowsRendered: JSX.Element[] = [];
    for (let i = 0; i < rows; i++) {
      rowsRendered.push(<tr key={i}>{renderSeats(seats, i)}</tr>);
    }
    return rowsRendered;
  };

  const renderSeats = (seats: number, row: React.Key) => {
    let renderedSeats: JSX.Element[] = [];
    for (let i = 0; i < seats; i++) {
      renderedSeats.push(
        <td id="test" className="rendering-seats" key={`row${row}-seat${i}`}>
          <FontAwesomeIcon className="fa-2x couch-icon" icon={faCouch} />
        </td>
      );
    }
    return renderedSeats;
  };

  return (
    <Container>
      <Row>
        <Col>
          <h1 className="form-header">Add New Cinema</h1>
          <form onSubmit={handleSubmit}>
            <FormGroup>
              <FormControl
                id="cityName"
                type="text"
                placeholder="City Name"
                value={state.cityName}
                className="add-new-form"
                onChange={handleChange}
              />
              <FormControl
                id="name"
                type="text"
                placeholder="Cinema Name"
                value={state.name}
                className="add-new-form"
                onChange={handleChange}
              />
              <FormText className="text-danger">{state.nameError}</FormText>
              <h1 className="form-header">Add Auditorium For Cinema</h1>
              <FormControl
                id="auditName"
                type="text"
                placeholder="Auditorium Name"
                value={state.auditName}
                onChange={handleChange}
                className="add-new-form"
              />
              <FormText className="text-danger">
                {state.auditNameError}
              </FormText>
              <FormControl
                id="seatRows"
                className="add-new-form"
                type="number"
                placeholder="Number Of Rows"
                value={state.seatRows.toString()}
                onChange={handleChange}
              />
              <FormText className="text-danger">{state.seatRowsError}</FormText>
              <FormControl
                id="numberOfSeats"
                className="add-new-form"
                type="number"
                placeholder="Number Of Seats"
                value={state.numberOfSeats.toString()}
                onChange={handleChange}
                max="36"
              />
              <FormText className="text-danger">
                {state.numOfSeatsError}
              </FormText>
            </FormGroup>
            <Button
              className="btn-add-new"
              type="submit"
              disabled={state.submitted || !state.canSubmit}
              block
            >
              Add
            </Button>
          </form>
        </Col>
      </Row>
      <Row className="mt-2">
        <Col className="justify-content-center align-content-center">
          <h1 className="form-header">Auditorium Preview</h1>
          <div>
            <Row className="justify-content-center">
              <table className="table-cinema-auditorium">
                <tbody>{renderRows(state.seatRows, state.numberOfSeats)}</tbody>
              </table>
            </Row>
            <Row className="justify-content-center mb-4">
              <div className="text-center text-white font-weight-bold cinema-screen">
                CINEMA SCREEN
              </div>
            </Row>
          </div>
        </Col>
      </Row>
    </Container>
  );
};

export default withRouter(NewCinema);
