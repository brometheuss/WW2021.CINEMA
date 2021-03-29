import React, { useEffect, useState } from "react";
import { NotificationManager } from "react-notifications";
import { serviceConfig } from "../../../appSettings";
import { Row, Table } from "react-bootstrap";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faEdit, faTrash } from "@fortawesome/free-solid-svg-icons";
import Spinner from "../../Spinner";
import { ICinema } from "../../../models";

interface IState {
  cinemas: ICinema[];
  isLoading: boolean;
}

const ShowAllCinemas: React.FC = (props: any) => {
  const [state, setState] = useState<IState>({
    cinemas: [
      {
        id: "",
        name: "",
      },
    ],
    isLoading: true,
  });

  useEffect(() => {
    getCinemas();
  }, []);

  const getCinemas = () => {
    const requestOptions = {
      method: "GET",
      headers: {
        "Content-Type": "application/json",
        Authorization: `Bearer ${localStorage.getItem("jwt")}`,
      },
    };

    setState({ ...state, isLoading: true });
    fetch(`${serviceConfig.baseURL}/api/Cinemas`, requestOptions)
      .then((response) => {
        if (!response.ok) {
          return Promise.reject(response);
        }
        return response.json();
      })
      .then((data) => {
        if (data) {
          setState({ ...state, cinemas: data, isLoading: false });
        }
      })
      .catch((response) => {
        NotificationManager.error(response.message || response.statusText);
        setState({ ...state, isLoading: false });
      });
  };

  const removeCinema = (id: string) => {
    const requestOptions = {
      method: "DELETE",
      headers: {
        "Content-Type": "application/json",
        Authorization: `Bearer ${localStorage.getItem("jwt")}`,
      },
    };

    fetch(`${serviceConfig.baseURL}/api/cinemas/${id}`, requestOptions)
      .then((response) => {
        if (!response.ok) {
          NotificationManager.error("Cinema has projections and cannot be deleted.");
          return;
        }
        NotificationManager.success("Successfully deleted cinema.");
        return response.json();
      })

      .catch((response) => {
        NotificationManager.error(response.message || response.statusText);
        setState({ ...state, isLoading: false });
      });
    setTimeout(() => window.location.reload(), 1000);
  };

  const fillTableWithDaata = () => {
    return state.cinemas.map((cinema) => {
      return (
        <tr key={cinema.id}>
          <td width="45%">{cinema.id}</td>
          <td width="45%">{cinema.name}</td>
          <td
            width="5%"
            className="text-center cursor-pointer"
            onClick={() => editCinema(cinema.id)}
          >
            <FontAwesomeIcon className="text-info mr-2 fa-1x" icon={faEdit} />
          </td>
          <td
            width="5%"
            className="text-center cursor-pointer"
            onClick={() => removeCinema(cinema.id)}
          >
            <FontAwesomeIcon
              className="text-danger mr-2 fa-1x"
              icon={faTrash}
            />
          </td>
        </tr>
      );
    });
  };

  const editCinema = (id: string) => {
    // to be implemented
    props.history.push(`editcinema/${id}`);
  };

  const rowsData = fillTableWithDaata();
  const table = (
    <Table striped bordered hover size="sm" variant="dark">
      <thead>
        <tr>
          <th>Id</th>
          <th>Name</th>
          <th></th>
          <th></th>
        </tr>
      </thead>
      <tbody>{rowsData}</tbody>
    </Table>
  );
  const showTable = state.isLoading ? <Spinner></Spinner> : table;

  return (
    <React.Fragment>
      <Row className="no-gutters pt-2">
        <h1 className="form-header form-heading">All Cinemas</h1>
      </Row>
      <Row className="no-gutters pr-5 pl-5">{showTable}</Row>
    </React.Fragment>
  );
};

export default ShowAllCinemas;
