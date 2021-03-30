export interface IProjection {
  id: string;
  projectionTime: string;
  movieId: string;
  auditoriumName: string;
  bannerUrl?: string;
  movieTitle?: string;
  movieRating?: number;
  movieYear?: string;
}

export interface IMovie {
  id: string;
  title: string;
  rating: number;
  year: string;
  hasOscar?: boolean;
  bannerUrl?: string;
  current?: boolean;
  projections?: IProjection[];
}

export interface ICinema {
  id: string;
  name: string;
}

export interface IAuditorium {
  id: string;
  name: string;
  cinemaId?: string;
}

export interface ISeats {
  id: string;
  number: number;
  row: number;
}

export interface ICurrentReservationSeats {
  id: string;
}

export interface IReservedSeats {
  id: string;
}

export interface IUser {
  id: string;
  firstName: string;
  lastName: string;
  points: string;
}

export interface IReservation {
  projectionId: string;
  projectionTime?: string;
  auditoriumName?: string;
  movieTitle?: string;
}

export interface ITag {
  name: string;
}
