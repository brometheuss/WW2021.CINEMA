import React, { useEffect, useState } from "react";
import { withRouter } from "react-router-dom";
import { serviceConfig } from "./../appSettings";
import { NotificationManager } from "react-notifications";
import { Container, Row, Col, Table, Button } from "react-bootstrap";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faEdit, faTrash } from "@fortawesome/free-solid-svg-icons";
import { IMovie } from "../models";

interface IState {
  movies: IMovie[];
}

const AllMovies: React.FC = (props: any) => {
  const [state, setState] = useState<IState>({
    movies: [
      {
        id: "",
        bannerUrl: "",
        title: "",
        year: "",
        rating: 0,
      },
    ],
  });

  useEffect(() => {
    handleFetchData();
  }, []);

  const handleFetchData = () => {
    const requestParams = {
      method: "GET",
      headers: {
        "Content-Type": "application/json",
        Authorization: `Bearer ${localStorage.getItem("jwt")}`,
      },
    };

    fetch(`${serviceConfig.baseURL}/api/movies/current`, requestParams)
      .then((response) => {
        if (!response.ok) {
          return Promise.reject(response);
        }
        return response.json();
      })
      .then((response) => {
        setState({ ...state, movies: response });
      })
      .catch((response) => {
        NotificationManager.error(response.message || response.statusText);
      });
  };

  const handleOnEdit = (id: string) => {
    props.history.push(`UpdateMovie/${id}`);
  };

  const handleOnAdd = () => {
    props.history.push("NewMovie");
  };

  const handleOnDelete = (id: string) => {
    const requestData = {
      method: "DELETE",
      headers: {
        "Content-Type": "application/json",
        Authorization: `Bearer ${localStorage.getItem("jwt")}`,
      },
    };

    fetch(`${serviceConfig.baseURL}/api/movies/${id}`, requestData)
      .then((response) => {
        if (!response.ok) {
          return Promise.reject(response);
        }
        return response.text;
      })
      .then((responseText) => {
        NotificationManager.success("Movie successfuly removed");
        const newState = state.movies.filter((movie) => {
          return movie.id !== id;
        });
        setState({ ...state, movies: newState });
      })
      .catch((response) => {
        NotificationManager.error(response.message || response.statusText);
      });
  };

  const renderRows = () => {
    return state.movies.map((movie) => {
      return (
        <tr key={movie.id}>
          <td>{movie.id}</td>
          <td>{movie.title}</td>
          <td>{movie.year}</td>
          <td>{Math.round(movie.rating)}</td>
          <td onClick={() => handleOnEdit(movie.id)}>
            <FontAwesomeIcon
              className="text-info mr-2 fa-1x"
              icon={faEdit}
            ></FontAwesomeIcon>
          </td>
          <td onClick={() => handleOnDelete(movie.id)}>
            <FontAwesomeIcon
              className="text-danger mr-2 fa-1x"
              icon={faTrash}
            ></FontAwesomeIcon>
          </td>
        </tr>
      );
    });
  };

  return (
    <React.Fragment>
      <Container>
        <Row>
          <h1>All Current Movies</h1>
        </Row>
        <Row>
          <Col>
            <Table striped bordered hover size="sm" variant="dark">
              <thead>
                <tr>
                  <th>Id</th>
                  <th>Title</th>
                  <th>Year</th>
                  <th>Rating</th>
                  <th></th>
                  <th></th>
                </tr>
              </thead>
              <tbody>{renderRows()}</tbody>
            </Table>
          </Col>
        </Row>
        <Row>
          <Button block onClick={() => handleOnAdd()}>
            Add Movie
          </Button>
        </Row>
      </Container>
    </React.Fragment>
  );
};

export default withRouter(AllMovies);
