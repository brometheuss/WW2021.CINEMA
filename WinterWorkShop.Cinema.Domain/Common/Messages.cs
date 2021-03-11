﻿namespace WinterWorkShop.Cinema.Domain.Common
{
    public static class Messages
    {
        #region Users

        #endregion

        #region Payments
        public const string PAYMENT_CREATION_ERROR = "Connection error, occured while creating new payment, please try again";
        #endregion

        #region Auditoriums
        public const string AUDITORIUM_GET_ALL_AUDITORIUMS_ERROR = "Error occured while getting all auditoriums, please try again.";
        public const string AUDITORIUM_PROPERTY_NAME_NOT_VALID = "The auditorium Name cannot be longer than 50 characters.";
        public const string AUDITORIUM_PROPERTY_SEATROWSNUMBER_NOT_VALID = "The auditorium number of seats rows must be between 1-20.";
        public const string AUDITORIUM_PROPERTY_SEATNUMBER_NOT_VALID = "The auditorium number of seats number must be between 1-20.";
        public const string AUDITORIUM_CREATION_ERROR = "Error occured while creating new auditorium, please try again.";
        public const string AUDITORIUM_SEATS_CREATION_ERROR = "Error occured while creating seats for auditorium, please try again.";
        public const string AUDITORIUM_SAME_NAME = "Cannot create new auditorium, auditorium with same name alredy exist.";
        public const string AUDITORIUM_INVALID_CINEMAID = "Cannot create new auditorium, auditorium with given cinemaId does not exist.";
        #endregion

        #region Cinemas
        public const string CINEMA_GET_ALL_CINEMAS_ERROR = "Error occured while getting all cinemas, please try again";
        public const string CINEMA_NAME_NOT_VALID = "Cinema cannot be longer than 50 characters";
        public const string CINEMA_DOES_NOT_EXIST = "Cinema does not exist";
        public const string CINEMA_CREATION_ERROR = "Error occured while creating new cinema, please try again.";
        #endregion

        #region Movies        
        public const string MOVIE_DOES_NOT_EXIST = "Movie does not exist.";
        public const string MOVIE_PROPERTIE_TITLE_NOT_VALID = "The movie title cannot be longer than 50 characters.";
        public const string MOVIE_PROPERTIE_YEAR_NOT_VALID = "The movie year must be between 1895-2100.";
        public const string MOVIE_PROPERTIE_RATING_NOT_VALID = "The movie rating must be between 1-10.";
        public const string MOVIE_CREATION_ERROR = "Error occured while creating new movie, please try again.";
        public const string MOVIE_GET_ALL_CURRENT_MOVIES_ERROR = "Error occured while getting current movies, please try again.";
        public const string MOVIE_GET_BY_ID = "Error occured while getting movie by Id, please try again.";
        public const string MOVIE_GET_ALL_MOVIES_ERROR = "Error occured while getting all movies, please try again.";
        public const string MOVIE_CREATION_ERROR_HASOSCAR_REQUIRED = "Oscar field is required.";
        public const string MOVIE_DEACTIVATION_ERROR = "Cannot deactivate movie which has future projections";

        #endregion

        #region Projections
        public const string PROJECTION_GET_ALL_PROJECTIONS_ERROR = "Error occured while getting all projections, please try again.";
        public const string PROJECTION_CREATION_ERROR = "Error occured while creating new projection, please try again.";
        public const string PROJECTIONS_AT_SAME_TIME = "Cannot create new projection, there are projections at same time alredy.";
        public const string PROJECTION_IN_PAST = "Projection time cannot be in past.";
        public const string PROJECTION_DOES_NOT_EXIST = "Projection does not exist.";
        #endregion

        #region Seats
        public const string SEAT_GET_ALL_SEATS_ERROR = "Error occured while getting all seats, please try again.";
        #endregion

        #region User
        public const string USER_NOT_FOUND = "User does not exist.";
        #endregion

        #region City
        public const string CITY_NOT_FOUND = "City does not exist";
        #endregion

        #region Reservation
        public const string RESERVATION_NOT_FOUND = "Reservation not found";
        #endregion

        #region Actor
        public const string ACTOR_NOT_FOUND = "Actor not found";
        #endregion
    }
}
