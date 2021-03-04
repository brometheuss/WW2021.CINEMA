import React, { useState } from "react";
import { NotificationManager } from "react-notifications";
import { serviceConfig } from "../../appSettings";
import Projection from "./Projection";
import { Row } from "react-bootstrap";
import { IMovie } from "../../models";

interface IState {
  movies: IMovie[];
  submitted: boolean;
}

const AllProjectionsForCinema: React.FC = () => {
  const [state, setState] = useState<IState>({
    movies: [
      {
        id: "",
        bannerUrl: "",
        rating: 0,
        title: "",
        year: "",
      },
    ],
    submitted: false,
  });

  const getProjections = () => {
    const requestOptions = {
      method: "GET",
      headers: {
        "Content-Type": "application/json",
        Authorization: `Bearer ${localStorage.getItem("jwt")}`,
      },
    };

    fetch(`${serviceConfig.baseURL}/api/Movies/current`, requestOptions)
      .then((response) => {
        if (!response.ok) {
          return Promise.reject(response);
        }
        return response.json();
      })
      .then((data) => {
        NotificationManager.success("Successfuly fetched data");
        if (data) {
        }
      })
      .catch((response) => {
        NotificationManager.error(response.message || response.statusText);
        setState({ ...state, submitted: false });
      });
  };
  return (
    <div className="no-gutters">
      <Row className="no-gutters"></Row>
      <Row className="no-gutters set-overflow-y">
        <Projection></Projection>
        <Projection></Projection>
        <Projection></Projection>
        <Projection></Projection>
        <Projection></Projection>
      </Row>
    </div>
  );
};

export default AllProjectionsForCinema;
