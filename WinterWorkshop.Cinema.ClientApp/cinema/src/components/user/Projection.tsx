import React, { useEffect, useState } from "react";
import { NotificationManager } from "react-notifications";
import { serviceConfig } from "../../appSettings";
import { withRouter } from "react-router-dom";
import { Container, Row, Col, Card, Button } from "react-bootstrap";
import "./../../index.css";
import { IAuditorium, IProjection, ICinema, IMovie } from "../../models";

interface IState {
  movies: IMovie[];
  cinemas: ICinema[];
  auditoriums: IAuditorium[];
  filteredAuditoriums: IAuditorium[];
  filteredMovies: IMovie[];
  filteredProjections: IProjection[];
  dateTime: string;
  id: string;
  current: boolean;
  tag: string;
  titleError: string;
  yearError: string;
  submitted: boolean;
  isLoading: boolean;
  selectedCinema: boolean;
  selectedAuditorium: boolean;
  selectedMovie: boolean;
  selectedDate: boolean;
  date: Date;
  cinemaId: string;
  auditoriumId: string;
  movieId: string;
  name: string;
}

const Projection: React.FC = (props: any) => {
  const [state, setState] = useState<IState>({
    movies: [
      {
        id: "",
        bannerUrl: "",
        title: "",
        rating: 0,
        year: "",
        projections: [
          {
            id: "",
            movieId: "",
            projectionTime: "",
            auditoriumName: "",
          },
        ],
      },
    ],
    cinemas: [{ id: "", name: "" }],
    auditoriums: [
      {
        id: "",
        name: "",
      },
    ],
    filteredAuditoriums: [
      {
        id: "",
        name: "",
      },
    ],
    filteredMovies: [
      {
        id: "",
        bannerUrl: "",
        title: "",
        rating: 0,
        year: "",
      },
    ],
    filteredProjections: [
      {
        id: "",
        movieId: "",
        projectionTime: "",
        bannerUrl: "",
        auditoriumName: "",
        movieTitle: "",
        movieRating: 0,
        movieYear: "",
      },
    ],
    cinemaId: "",
    auditoriumId: "",
    movieId: "",
    dateTime: "",
    id: "",
    name: "",
    current: false,
    tag: "",
    titleError: "",
    yearError: "",
    submitted: false,
    isLoading: true,
    selectedCinema: false,
    selectedAuditorium: false,
    selectedMovie: false,
    selectedDate: false,
    date: new Date(),
  });

  useEffect(() => {
    getCurrentMoviesAndProjections();
    getAllCinemas();
    getAllAuditoriums();
  }, []);

  const handleSubmit = (event: React.FormEvent<HTMLFormElement>) => {
    event.preventDefault();

    setState({ ...state, submitted: true });
    const { cinemaId, auditoriumId, movieId, dateTime } = state;

    if (cinemaId || auditoriumId || movieId || dateTime) {
      getCurrentFilteredMoviesAndProjections();
    } else {
      NotificationManager.error("Not found.");
      setState({ ...state, submitted: false });
    }
  };

  const getRoundedRating = (rating: number) => {
    const result = Math.round(rating);
    return <span className="float-right">Rating: {result}/10</span>;
  };

  const navigateToProjectionDetails = (id: string, movieId: string) => {
    props.history.push(`projectiondetails/${id}/${movieId}`);
  };

  const getCurrentMoviesAndProjections = () => {
    setState({ ...state, submitted: false });

    const requestOptions = {
      method: "GET",
      headers: {
        "Content-Type": "application/json",
        Authorization: `Bearer ${localStorage.getItem("jwt")}`,
      },
    };

    setState({ ...state, isLoading: true });
    fetch(
      `${serviceConfig.baseURL}/api/movies/currentMoviesAndProjections`,
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
          setState({ ...state, movies: data, isLoading: false });
        }
      })
      .catch((response) => {
        setState({ ...state, isLoading: false });
        NotificationManager.error(response.message || response.statusText);
      });
  };

  const getCurrentFilteredMoviesAndProjections = () => {
    const { cinemaId, auditoriumId, movieId, dateTime } = state;

    const requestOptions = {
      method: "GET",
      headers: {
        "Content-Type": "application/json",
        Authorization: `Bearer ${localStorage.getItem("jwt")}`,
      },
    };

    setState({ ...state, isLoading: true });
    let query = "";
    if (cinemaId) {
      query = `cinemaId=${cinemaId}`;
    }
    if (auditoriumId) {
      query += `${query.length ? "&" : ""}auditoriumId=${auditoriumId}`;
    }
    if (movieId) {
      query += `${query.length ? "&" : ""}movieId=${movieId}`;
    }
    if (dateTime) {
      query += `${query.length ? "&" : ""}dateTime=${dateTime}`;
    }
    if (query.length) {
      query = `?${query}`;
    }
    fetch(
      `${serviceConfig.baseURL}/api/projections/filter${query}`,
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
          let movies = state.movies;
          let filteredMovies = data;

          for (let i = 0; i < movies.length; i++) {
            for (let j = 0; j < filteredMovies.length; j++) {
              if (movies[i].id === data[j].movieId) {
                data[j].bannerUrl = movies[i].bannerUrl;
              }
            }
          }

          setState({ ...state, filteredProjections: data, isLoading: false });
        }
      })
      .catch((response) => {
        setState({ ...state, isLoading: false });
        NotificationManager.error(response.message || response.statusText);
      });
  };

  const getAllAuditoriums = () => {
    const requestOptions = {
      method: "GET",
      headers: {
        "Content-Type": "application/json",
        Authorization: `Bearer ${localStorage.getItem("jwt")}`,
      },
    };

    setState({ ...state, isLoading: true });
    fetch(`${serviceConfig.baseURL}/api/auditoriums/all`, requestOptions)
      .then((response) => {
        if (!response.ok) {
          return Promise.reject(response);
        }
        return response.json();
      })
      .then((data) => {
        if (data) {
          setState({ ...state, auditoriums: data, isLoading: false });
        }
      })
      .catch((response) => {
        NotificationManager.error(response.message || response.statusText);
        setState({ ...state, isLoading: false });
      });
  };

  const getAllCinemas = () => {
    const requestOptions = {
      method: "GET",
      headers: {
        "Content-Type": "application/json",
        Authorization: `Bearer ${localStorage.getItem("jwt")}`,
      },
    };

    setState({ ...state, isLoading: true });
    fetch(`${serviceConfig.baseURL}/api/Cinemas/all`, requestOptions)
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
            cinemas: data,
            isLoading: false,
          });
        }
      })
      .catch((response) => {
        NotificationManager.error(response.message || response.statusText);
        setState({ ...state, isLoading: false });
      });
  };

  const fillFilterWithMovies = () => {
    if (state.selectedAuditorium) {
      return state.filteredMovies.map((movie) => {
        return (
          <option value={movie.id} key={movie.id}>
            {movie.title}
          </option>
        );
      });
    } else {
      return state.movies.map((movie) => {
        return (
          <option value={movie.id} key={movie.id}>
            {movie.title}
          </option>
        );
      });
    }
  };

  const fillFilterWithCinemas = () => {
    return state.cinemas.map((cinema) => {
      return (
        <option value={cinema.id} key={cinema.id}>
          {cinema.name}
        </option>
      );
    });
  };

  const fillFilterWithAuditoriums = () => {
    if (state.selectedCinema) {
      return state.filteredAuditoriums.map((auditorium) => {
        return <option value={auditorium.id}>{auditorium.name}</option>;
      });
    } else {
      return state.auditoriums.map((auditorium) => {
        return (
          <option value={auditorium.id} key={auditorium.id}>
            {auditorium.name}
          </option>
        );
      });
    }
  };

  const fillTableWithData = () => {
    return state.movies.map((movie) => {
      const projectionButton = movie.projections?.map((projection) => {
        return (
          <Button
            key={projection.id}
            onClick={() => navigateToProjectionDetails(projection.id, movie.id)}
            className="btn-projection-time"
          >
            {projection.projectionTime.slice(11, 16)}h
          </Button>
        );
      });

      return (
        <Card.Body key={movie.id}>
          <div>
            <img className="img-style" src={movie.bannerUrl}></img>
          </div>
          <Card.Title>
            <span className="card-title-font">{movie.title}</span>
            {getRoundedRating(movie.rating)}
          </Card.Title>
          <hr />
          <Card.Subtitle className="mb-2 text-muted">
            Year of production: {movie.year}
          </Card.Subtitle>
          <hr />
          <Card.Text>
            <span className="mb-2 font-weight-bold">Projection times:</span>
          </Card.Text>
          {projectionButton}
        </Card.Body>
      );
    });
  };

  const fillTableWithFilteredProjections = () => {
    return state.filteredProjections.map((filteredProjection) => {
      return (
        <Card.Body>
          <div className="banner-img">
            <img className="img-style" src={filteredProjection.bannerUrl}></img>
          </div>
          <Card.Title>
            <span className="card-title-font">
              {filteredProjection.movieTitle} -{" "}
              {filteredProjection.auditoriumName}
            </span>
            {filteredProjection.movieRating &&
              getRoundedRating(filteredProjection.movieRating)}
          </Card.Title>
          <hr />
          <Card.Subtitle className="mb-2 text-muted">
            Year of production: {filteredProjection.movieYear}
          </Card.Subtitle>
          <hr />
          <Card.Text>
            <span className="mb-2 font-weight-bold">Projection times:</span>
          </Card.Text>
          <Button
            key={filteredProjection.id}
            onClick={() =>
              navigateToProjectionDetails(
                filteredProjection.id,
                filteredProjection.movieId
              )
            }
            className="btn-projection-time"
          >
            {filteredProjection.projectionTime.slice(11, 16)}h
          </Button>
        </Card.Body>
      );
    });
  };

  const getAuditoriumsBySelectedCinema = (selectedCinemaId: string) => {
    setState({ ...state, cinemaId: selectedCinemaId });

    const requestOptions = {
      method: "GET",
      headers: {
        "Content-Type": "application/json",
        Authorization: `Bearer ${localStorage.getItem("jwt")}`,
      },
    };

    setState({ ...state, isLoading: true });
    fetch(
      `${serviceConfig.baseURL}/api/Auditoriums/bycinemaid/${selectedCinemaId}`,
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
            filteredAuditoriums: data,
            isLoading: false,
            selectedCinema: true,
          });
        }
      })
      .catch((response) => {
        NotificationManager.error(response.message || response.statusText);
        setState({ ...state, isLoading: false });
      });
  };

  const getMoviesBySelectedAuditorium = (selectedAuditoriumId: string) => {
    setState({ ...state, auditoriumId: selectedAuditoriumId });
    const requestOptions = {
      method: "GET",
      headers: {
        "Content-Type": "application/json",
        Authorization: `Bearer ${localStorage.getItem("jwt")}`,
      },
    };

    setState({ ...state, isLoading: true });
    fetch(
      `${serviceConfig.baseURL}/api/Movies/byauditoriumid/${selectedAuditoriumId}`,
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
            filteredMovies: data,
            isLoading: false,
            selectedAuditorium: true,
          });
        }
      })
      .catch((response) => {
        NotificationManager.error(response.message || response.statusText);
        setState({ ...state, isLoading: false });
      });
  };

  const checkIfFiltered = () => {
    if (state.submitted) {
      return fillTableWithFilteredProjections();
    } else {
      return fillTableWithData();
    }
  };

  return (
    <Container>
      <h1 className="projections-title">Current projections</h1>
      <form
        id="name"
        name={state.name}
        onSubmit={handleSubmit}
        className="filter"
      >
        <span className="filter-heading">Filter by:</span>
        <select
          onChange={(e) => getAuditoriumsBySelectedCinema(e.target.value)}
          name="cinemaId"
          id="cinema"
          className="select-dropdown"
        >
          <option value="none">Cinema</option>
          {fillFilterWithCinemas()}
        </select>
        <select
          onChange={(e) => getMoviesBySelectedAuditorium(e.target.value)}
          name="auditoriumId"
          id="auditorium"
          className="select-dropdown"
          disabled
        >
          <option value="none">Auditorium</option>
          {fillFilterWithAuditoriums()}
        </select>
        <select
          onChange={(e) =>
            setState({ ...state, selectedMovie: true, movieId: e.target.value })
          }
          name="movieId"
          id="movie"
          className="select-dropdown"
          disabled
        >
          <option value="none">Movie</option>
          {fillFilterWithMovies()}
        </select>
        <input
          onChange={(e) =>
            setState({ ...state, selectedDate: true, dateTime: e.target.value })
          }
          name="dateTime"
          type="date"
          id="date"
          className="input-date select-dropdown"
          disabled
        />
        <button
          id="filter-button"
          className="btn-search"
          type="submit"
          onClick={() => setState({ ...state, submitted: true })}
        >
          Submit
        </button>
      </form>
      <Row className="justify-content-center">
        <Col>
          <Card className="card-width">{checkIfFiltered()}</Card>
        </Col>
      </Row>
    </Container>
  );
};

export default withRouter(Projection);
