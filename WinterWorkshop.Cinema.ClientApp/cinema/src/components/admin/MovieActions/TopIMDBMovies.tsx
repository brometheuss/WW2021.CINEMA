import React, { useEffect, useState } from "react";
import { withRouter } from "react-router";
import Spinner from "../../Spinner"

const TopIMDBMovies: React.FC = (props: any) => {
    const url = "https://www.imdb.com/title/";

    const [movies, setMovies] = useState({
        items: [
            {
                id: "",
                image: "",
                rank: "",
                title: "",
                imDbRating: ""
            },

        ],
        dataRecieved: false
    })


    useEffect(() => {
        getIMDBMovies();
    }, [])

    const getIMDBMovies = () => {
        const requestOptions = {
            method: "GET",
            headers: {
                "Content-Type": "application/json"
            },
        };

        fetch(`https://imdb-api.com/en/API/MostPopularMovies/k_fweq5i39`, requestOptions)
            .then((response) => {
                if (!response.ok) {
                    return Promise.reject(response);
                }
                return response.json();
            })
            .then((data) => {
                if (data) {
                    setMovies({
                        ...movies,
                        items: data.items,
                        dataRecieved: true
                    });
                }
            })
            .catch((response) => {
                //NotificationManager.error(response.message || response.statusText);
                // setState({ ...state, submitted: false });
                console.log(response);
            });
    };

    const fillPageWithMovies = () => {
        return movies.items.map((item) => {
            return (
                <>
                    {
                        (parseInt(item.rank) < 11) ?
                            <div className="card" key={item.id} style={{
                                width: '18rem'
                                , display: 'inline-block', margin: '10px'
                            }}>
                                <img className="card-img-top" src={item.image} alt={item.title} />
                                <div className="card-body">
                                    <h5 className="card-title">{item.imDbRating || "Unrated"} &nbsp;&nbsp; Rank: {item.rank}</h5>
                                    <p className="card-text">{item.title}</p>
                                    <a href={url + item.id} className="btn btn-primary">Click for more info</a>
                                </div>
                            </div> : ""

                    }
                </>
            )

        })

    }


    return (
        <>
            { movies.dataRecieved ? fillPageWithMovies() : <Spinner></Spinner>}
        </>
    )
}

export default withRouter(TopIMDBMovies);


