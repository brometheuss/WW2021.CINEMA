import React, { useEffect, useState } from "react";
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
import { IMovie } from "../models";

interface IState {
  movie: IMovie;
  titleError: string;
  yearError: string;
  submited: boolean;
}

const UpdateMovie: React.FC = (props: any) => {
  const { id } = props.match.params;

  const [state, setState] = useState<IState>({
    movie: {
      id: "",
      title: "",
      year: "",
      rating: 1,
      current: true,
    },
    titleError: "",
    yearError: "",
    submited: false,
  });

  useEffect(() => {
    handleFetchMovie(id);
  }, []);

  const onInputChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { id, value } = e.target;
    setState({ ...state, [id]: value });
    validateData(id, value);
  };

  const handleYearChange = (year: string) => {
    setState({ ...state, movie: { ...state.movie, year: year } });
    validateData("year", year);
  };

  const handleSubmit = (e: React.FormEvent<HTMLFormElement>) => {
    e.preventDefault();
    const { title, year, rating, current } = state.movie;

    setState({ ...state, submited: true });
    if (title && year && rating && current) {
      handleUpdateMovie();
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

  const handleFetchMovie = (id: string) => {
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
      .then((response) => {
        setState({
          ...state,
          movie: {
            title: response.title,
            year: response.year,
            rating: response.rating,
            current: response.current,
            id: response.id,
          },
        });
      })
      .catch((response) => {
        NotificationManager.error(response.message || response.statusText);
        setState({ ...state, submited: false });
      });
  };

  const handleUpdateMovie = () => {
    const data = {
      Title: state.movie.title,
      Year: +state.movie.year,
      Rating: +state.movie.rating,
      Current: state.movie.current,
    };

    const requestOptions = {
      method: "PUT",
      headers: {
        "Content-Type": "application/json",
        Authorization: `Bearer ${localStorage.getItem("jwt")}`,
      },
      body: JSON.stringify(data),
    };

    fetch(
      `${serviceConfig.baseURL}/api/movies/${state.movie.id}`,
      requestOptions
    )
      .then((response) => {
        if (!response.ok) {
          return Promise.reject(response);
        }
        return response.statusText;
      })
      .then((statusText) => {
        NotificationManager.success("Movie successfuly updated!");
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
          <h1>Update Movie Component</h1>
        </Row>
        <Row>
          <Col>
            <form onSubmit={handleSubmit}>
              <FormGroup>
                <FormControl
                  type="text"
                  placeholder="Enter movie title"
                  id="title"
                  value={state.movie.title}
                  onChange={onInputChange}
                />
                <FormText className="text-danger">{state.movie.title}</FormText>
              </FormGroup>
              <FormGroup>
                <FormControl
                  as="select"
                  id="rating"
                  value={state.movie.rating.toString()}
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
                  value={state.movie.current?.toString()}
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
                  value={state.movie.year}
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
                Update Movie
              </Button>
            </form>
          </Col>
        </Row>
      </Container>
    </React.Fragment>
  );
};

export default withRouter(UpdateMovie);
