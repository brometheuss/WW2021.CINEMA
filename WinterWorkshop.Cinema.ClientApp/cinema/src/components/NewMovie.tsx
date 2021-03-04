import React, { useState } from "react";
import { withRouter } from "react-router-dom";
import { YearPicker } from "react-dropdown-date";
import { serviceConfig } from "./../appSettings";
import { NotificationManager } from "react-notifications";
import {
  FormGroup,
  FormControl,
  Button,
  Container,
  Row,
  Col,
  FormText,
} from "react-bootstrap";

interface IState {
  title: string;
  year: string;
  rating: number;
  current: boolean;
  titleError: string;
  yearError: string;
  submited: boolean;
}

const NewMovie: React.FC = (props: any) => {
  const [state, setState] = useState<IState>({
    title: "",
    year: "",
    rating: 1,
    current: true,
    titleError: "",
    yearError: "",
    submited: false,
  });

  const onInputChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { id, value } = e.target;
    setState({ ...state, [id]: value });
    validateData(id, value);
  };

  const handleYearChange = (year: string) => {
    setState({ ...state, year: year });
    validateData("year", year);
  };

  const handleSubmit = (e: React.FormEvent<HTMLFormElement>) => {
    e.preventDefault();
    const { title, year, rating, current } = state;

    setState({ ...state, submited: true });
    if (title && year && rating && current) {
      handleAddMovie();
    } else {
      setState({ ...state, submited: false });
    }
  };

  const validateData = (id: string, value: string) => {
    if (id === "title") {
      if (!value && value === "") {
        setState({
          ...state,
          titleError: "Please fill in movie title",
        });
      } else {
        setState({ ...state, titleError: "" });
      }
    }

    if (id === "year") {
      const yearNum = +value;
      if (!value || value === "" || yearNum < 1895 || yearNum > 2100) {
        setState({
          ...state,
          yearError: "Please chose valid year",
        });
      } else {
        setState({ ...state, yearError: "" });
      }
    }
  };

  const handleAddMovie = () => {
    const data = {
      Title: state.title,
      Year: +state.year,
      Rating: +state.rating,
      Current: state.current,
    };

    const requestOptions = {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
        Authorization: `Bearer ${localStorage.getItem("jwt")}`,
      },
      body: JSON.stringify(data),
    };

    fetch(`${serviceConfig.baseURL}/api/movies`, requestOptions)
      .then((response) => {
        if (!response.ok) {
          return Promise.reject(response);
        }
        return response.statusText;
      })
      .then((statusText) => {
        NotificationManager.success("Movie successfuly added!");
        props.history.push("/AllMovies");
      })
      .catch((response) => {
        NotificationManager.error(response.message || response.statusText);
        setState({ ...state, submited: false });
      });
  };

  return (
    <React.Fragment>
      <Container>
        <Row>
          <h1>New Movie Component</h1>
        </Row>
        <Row>
          <Col>
            <form onSubmit={handleSubmit}>
              <FormGroup>
                <FormControl
                  type="text"
                  placeholder="Enter movie title"
                  id="title"
                  value={state.title}
                  onChange={onInputChange}
                />
                <FormText className="text-danger">{state.titleError}</FormText>
              </FormGroup>
              <FormGroup>
                <FormControl
                  as="select"
                  id="rating"
                  value={state.rating.toString()}
                  placeholder="Chose movie rating"
                  onChange={onInputChange}
                >
                  <option value="1">1</option>
                  <option value="2">2</option>
                  <option value="3">3</option>
                  <option value="4">4</option>
                  <option value="5">5</option>
                  <option value="6">6</option>
                  <option value="7">7</option>
                  <option value="8">8</option>
                  <option value="9">9</option>
                  <option value="10">10</option>
                </FormControl>
              </FormGroup>
              <FormGroup>
                <FormControl
                  as="select"
                  id="current"
                  placeholder="Movie is:"
                  value={state.current?.toString()}
                  onChange={onInputChange}
                >
                  <option value="true">Current</option>
                  <option value="false">Not Current</option>
                </FormControl>
              </FormGroup>
              <FormGroup>
                <YearPicker
                  defaultValue={"Chose movie year"}
                  start={1890}
                  end={2120}
                  reverse
                  required={true}
                  disabled={false}
                  value={state.year}
                  onChange={(year: string) => {
                    handleYearChange(year);
                  }}
                  id="year"
                  name="year"
                  classes={"form-control"}
                ></YearPicker>
                <FormText className="text-danger">{state.yearError}</FormText>
              </FormGroup>
              <Button type="submit" disabled={state.submited} block>
                Add Movie
              </Button>
            </form>
          </Col>
        </Row>
      </Container>
    </React.Fragment>
  );
};

export default withRouter(NewMovie);
