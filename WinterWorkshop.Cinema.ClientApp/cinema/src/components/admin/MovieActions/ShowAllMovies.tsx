import React, { useEffect, useState } from "react";
import { NotificationManager } from "react-notifications";
import { serviceConfig } from "../../../appSettings";
import { Row, Table } from "react-bootstrap";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import {
  faEdit,
  faTrash,
  faInfoCircle,
  faLightbulb,
} from "@fortawesome/free-solid-svg-icons";
import Spinner from "../../Spinner";
import "./../../../index.css";
import {
  isAdmin,
  isSuperUser,
  isUser,
  isGuest,
} from "./../../helpers/authCheck";
import { ToastContainer, toast } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";
import { IMovie, ITag } from "../../../models";

interface IState {
  movies: IMovie[];
  tags: ITag[];
  title: string;
  year: string;
  id: string;
  rating: number;
  ratingBiggerThan: string;
  ratingLowerThan: string;
  yearBiggerThan: string;
  yearLowerThan: string;
  current: boolean;
  tag: string;
  listOfTags: string[];
  titleError: string;
  yearError: string;
  submitted: boolean;
  isLoading: boolean;
}

const ShowAllMovies: React.FC = (props: any) => {
  const [state, setState] = useState<IState>({
    movies: [
      {
        id: "",
        bannerUrl: "",
        title: "",
        year: "",
        current: false,
        rating: 0,
      },
    ],
    tags: [
      {
        name: "",
      },
    ],
    title: "",
    year: "",
    id: "",
    ratingBiggerThan: "",
    ratingLowerThan: "",
    yearBiggerThan: "",
    yearLowerThan: "",
    rating: 0,
    current: false,
    tag: "",
    listOfTags: [""],
    titleError: "",
    yearError: "",
    submitted: false,
    isLoading: true,
  });

  toast.configure();

  let userShouldSeeWholeTable;
  const shouldUserSeeWholeTable = () => {
    if (userShouldSeeWholeTable === undefined) {
      userShouldSeeWholeTable = !isGuest() && !isUser();
    }
    return userShouldSeeWholeTable;
  };

  useEffect(() => {
    getProjections();
  }, []);

  const getMovie = (movieId: string) => {
    const requestOptions = {
      method: "GET",
      headers: {
        "Content-Type": "application/json",
        Authorization: `Bearer ${localStorage.getItem("jwt")}`,
      },
    };

    fetch(`${serviceConfig.baseURL}/api/movies/${movieId}`, requestOptions)
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
            title: data.title,
            year: data.year,
            rating: Math.round(data.rating),
            current: data.current === true,
            id: data.id,
          });
        }
      })
      .catch((response) => {
        NotificationManager.error(response.message || response.statusText);
        setState({ ...state, submitted: false });
      });
  };

  const changeCurrent = (
    e: React.MouseEvent<HTMLTableDataCellElement, MouseEvent>,
    id: string
  ) => {
    e.preventDefault();
    const requestOptions = {
      method: "PATCH",
      headers: {
        "Content-Type": "application/json",
        Authorization: `Bearer ${localStorage.getItem("jwt")}`,
      },
    };

    fetch(
      `${serviceConfig.baseURL}/api/movies/changecurrent/${id}`,
      requestOptions
    )
      .then((response) => {
        if (!response.ok) {
          return Promise.reject(response);
        }
        return response.statusText;
      })
      .then((result) => {
        NotificationManager.success(
          "Successfuly changed current status for movie!"
        );
        const newState = state.movies.filter((movie) => {
          return movie.id !== id;
        });
        setState({ ...state, movies: newState });
        setTimeout(() => {
          window.location.reload();
        }, 2000);
      })
      .catch(() => {
        NotificationManager.error("This movie has projections!");
        setState({ ...state, submitted: false });
      });
  };

  const getProjections = () => {
    if (isAdmin() === true || isSuperUser() === true) {
      const requestOptions = {
        method: "GET",
        headers: {
          "Content-Type": "application/json",
          Authorization: `Bearer ${localStorage.getItem("jwt")}`,
        },
      };

      setState({ ...state, isLoading: true });
      fetch(`${serviceConfig.baseURL}/api/movies/all`, requestOptions)
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
          setState({ ...state, submitted: false });
        });
    } else {
      const requestOptions = {
        method: "GET",
        headers: {
          "Content-Type": "application/json",
          Authorization: `Bearer ${localStorage.getItem("jwt")}`,
        },
      };

      setState({ ...state, isLoading: true });
      fetch(`${serviceConfig.baseURL}/api/movies/current`, requestOptions)
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
          setState({ ...state, submitted: false });
        });
    }
  };

  const removeMovie = (id: string) => {
    const requestOptions = {
      method: "DELETE",
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
        return response.statusText;
      })
      .then((result) => {
        NotificationManager.success("Successfuly removed movie!");
        const newState = state.movies.filter((movie) => {
          return movie.id !== id;
        });
        setState({ ...state, movies: newState });
      })
      .catch((response) => {
        NotificationManager.error(response.message || response.statusText);
        setState({ ...state, submitted: false });
      });
  };

  const getTagsByMovieId = (
    e: React.MouseEvent<HTMLTableDataCellElement, MouseEvent>,
    movieId: string
  ) => {
    e.preventDefault();

    const requestOptions = {
      method: "GET",
      headers: {
        "Content-Type": "application/json",
        Authorization: `Bearer ${localStorage.getItem("jwt")}`,
      },
    };

    fetch(
      `${serviceConfig.baseURL}/api/tags/getbymovieid/${movieId}`,
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
          setState({ ...state, tags: data });
          showTags();
        }
      })
      .catch((response) => {
        NotificationManager.error(response.message || response.statusText);
        setState({ ...state, submitted: false });
      });
  };

  const showTags = () => {
    setState({ ...state, listOfTags: [] });
    state.tags.map((tag) => {
      state.listOfTags.push(tag.name);
    });
    var list = " | ";
    for (var i = 0; i < state.listOfTags.length; i++) {
      list += state.listOfTags[i] + " | ";
    }
    toast.info(list, {
      position: toast.POSITION.BOTTOM_CENTER,
      className: "toast-class",
    });
  };

  const fillTableWithDaata = () => {
    return state.movies.map((movie) => {
      return (
        <tr key={movie.id}>
          <td
            className="text-center cursor-pointer"
            onClick={(
              e: React.MouseEvent<HTMLTableDataCellElement, MouseEvent>
            ) => getTagsByMovieId(e, movie.id)}
          >
            <FontAwesomeIcon
              className="text-info mr-2 fa-1x"
              icon={faInfoCircle}
            />
          </td>
          <td>{movie.title}</td>
          <td>{movie.year}</td>
          <td>{Math.round(movie.rating)}/10</td>

          {shouldUserSeeWholeTable() && <td>{movie.current ? "Yes" : "No"}</td>}
          {shouldUserSeeWholeTable() && (
            <td
              className="text-center cursor-pointer"
              onClick={() => editMovie(movie.id)}
            >
              <FontAwesomeIcon className="text-info mr-2 fa-1x" icon={faEdit} />
            </td>
          )}
          {shouldUserSeeWholeTable() && (
            <td
              className="text-center cursor-pointer"
              onClick={() => removeMovie(movie.id)}
            >
              <FontAwesomeIcon
                className="text-danger mr-2 fa-1x"
                icon={faTrash}
              />
            </td>
          )}
          {shouldUserSeeWholeTable() && (
            <td
              className="text-center cursor-pointer"
              onClick={(
                e: React.MouseEvent<HTMLTableDataCellElement, MouseEvent>
              ) => changeCurrent(e, movie.id)}
            >
              <FontAwesomeIcon
                className={
                  movie.current
                    ? "text-warning mr-2 fa-1x"
                    : "text-secondary mr-2 fa-1x"
                }
                icon={faLightbulb}
              />
            </td>
          )}
        </tr>
      );
    });
  };

  const editMovie = (id: string) => {
    props.history.push(`editmovie/${id}`);
  };

  const handleChange = (e) => {
    const { id, value } = e.target;
    setState({ ...state, [id]: value });
  };

  const handleSubmit = (e: React.FormEvent<HTMLFormElement>) => {
    e.preventDefault();

    // var data = new FormData(e.currentTarget);

    // var queryParts: string[] = [];
    // var entries = data.entries();

    // for (var pair of entries) {
    //   queryParts.push(
    //     `${encodeURIComponent(pair[0])}=${encodeURIComponent(
    //       pair[1].toString()
    //     )}`
    //   );
    // }
    // var query = queryParts.join("&");
    // var loc = window.location;
    // var url = `${loc.protocol}//${loc.host}${loc.pathname}?${query}`;

    // let tag = url.split("=")[1];

    // setState({ ...state, submitted: true });
    // if (tag) {
    //   searchMovie(tag);
    // } else {
    //   NotificationManager.error(
    //     "Please type type something in search bar to search for movies."
    //   );
    //   setState({ ...state, submitted: false });
    // }
    // if (state.title !== "" || state.ratingLowerThan !== "11" || state.ratingBiggerThan !== "0") {
    //   console.log("Ne pozovi", state.title, state.ratingBiggerThan, state.ratingLowerThan);
    //   searchMovie();
    // }

    searchMovie();
  };

  const searchMovie = () => {
    const requestOptions = {
      method: "GET",
      headers: {
        "Content-Type": "application/json",
        Authorization: `Bearer ${localStorage.getItem("jwt")}`,
      },
    };

    setState({ ...state, isLoading: true });
    fetch(`${serviceConfig.baseURL}/api/Movies/bytag?ratingbiggerthan=${state.ratingBiggerThan === "" ? "0" : state.ratingBiggerThan}&ratinglowerthan=${state.ratingLowerThan === "" ? "11" : state.ratingLowerThan}&yearbiggerthan=${state.yearBiggerThan === "" ? "1894" : state.yearBiggerThan}&yearlowerthan=${state.yearLowerThan === "" ? "2100" : state.yearLowerThan}&title=${state.title}`, requestOptions)
      .then((response) => {
        if (!response.ok) {
          return Promise.reject(response);
        }
        return response.json();
      })
      .then((data) => {
        if (data) {
          setState({ ...state, movies: data, isLoading: false });
        } else {
          const { title } = state;
          const requestOptions = {
            method: "GET",
            headers: {
              "Content-Type": "application/json",
              Authorization: `Bearer ${localStorage.getItem("jwt")}`,
            },
          };

          setState({ ...state, isLoading: true });
          fetch(
            `${serviceConfig.baseURL}/api/Movies/bytitle/${title}`,
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
            });
        }
      })
      .catch((response) => {
        setState({ ...state, isLoading: false });
        NotificationManager.error("Movie doesn't exists.");
        setState({ ...state, submitted: false });
      });
  };

  let title;
  let inputValue1;

  const rowsData = fillTableWithDaata();
  const table = (
    <Table striped bordered hover size="sm" variant="dark">
      <thead>
        <tr>
          <th>Tags</th>
          <th>Title</th>
          <th>Year</th>
          <th>Rating</th>
          {shouldUserSeeWholeTable() && <th>Is Current</th>}
          {shouldUserSeeWholeTable() && <th></th>}
          {shouldUserSeeWholeTable() && <th></th>}
        </tr>
      </thead>
      <tbody>{rowsData}</tbody>
    </Table>
  );
  const showTable = state.isLoading ? <Spinner></Spinner> : table;

  return (
    <React.Fragment>
      <Row className="no-gutters pt-2">
        <h1 className="form-header form-heading">All Movies</h1>
      </Row>
      <Row>
        <form
          onSubmit={(e: React.FormEvent<HTMLFormElement>) => handleSubmit(e)}
          className="form-inline search-field md-form mr-auto mb-4 search-form search-form-second"
        >
          <input
            className="mr-sm-2 search-bar"
            id="tag"
            type="text"
            placeholder="Rating Bigger Than"
            name="ratingbiggerthan"
            value={state.ratingBiggerThan}
            aria-label="Search"
            onChange={(e) => setState({ ...state, ratingBiggerThan: e.target.value })}
          />

          <input
            className="mr-sm-2 search-bar"
            id="tag"
            type="text"
            placeholder="Rating Lower Than"
            name="ratinglowerthan"
            value={state.ratingLowerThan}
            aria-label="Search"
            onChange={(e) => setState({ ...state, ratingLowerThan: e.target.value })}
          />

          <input
            className="mr-sm-2 search-bar"
            id="tag"
            type="text"
            placeholder="Title"
            name="title"
            value={state.title}
            aria-label="Search"
            onChange={(e) => setState({ ...state, title: e.target.value })}
          />

          <input
            className="mr-sm-2 search-bar"
            id="tag"
            style={{
              width: '196px'
            }}
            type="text"
            placeholder="Year Bigger Than"
            name="yearbiggerthan"
            value={state.yearBiggerThan}
            aria-label="Search"
            onChange={(e) => setState({ ...state, yearBiggerThan: e.target.value })}
          />
          <input
            className="mr-sm-2 search-bar"
            id="tag"
            style={{
              width: '196px'
            }}
            type="text"
            placeholder="Year Lower Than"
            name="yearlowerthan"
            value={state.yearLowerThan}
            aria-label="Search"
            onChange={(e) => setState({ ...state, yearLowerThan: e.target.value })}
          />

          <button className="btn-search" type="submit"
            style={{
              position: 'absolute',
              right: '50px',
              top: '130px'
            }}>
            Search
          </button>
          <button className="btn-search"
            style={{
              position: 'absolute',
              right: '50px',
              top: '100px',
              height: '28px',
              width: '65px',
              display: 'flex',
              alignItems: 'center',
              justifyContent: 'center'
            }} type="button" onClick={getProjections}>Reset</button>
        </form>

      </Row>
      <Row className="no-gutters pr-5 pl-5">{showTable}</Row>
    </React.Fragment>
  );
};

export default ShowAllMovies;
