﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using WinterWorkShop.Cinema.Data.Entities;

namespace WinterWorkShop.Cinema.Data
{
    public class CinemaContext: DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Projection> Projections { get; set; }
        public DbSet<Cinema> Cinemas { get; set; }
        public DbSet<Auditorium> Auditoriums { get; set; }
        public DbSet<Seat> Seats { get; set; }
        public DbSet<Role> Role { get; set; }
        public DbSet<Actor> Actors { get; set; } 

        public CinemaContext(DbContextOptions options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            /// <summary>
            /// Seat -> Auditorium relation
            /// </summary>
            /// <returns></returns>
            modelBuilder.Entity<Seat>()
                .HasOne(x => x.Auditorium)
                .WithMany(x => x.Seats)
                .HasForeignKey(x => x.AuditoriumId)
                .IsRequired();

            /// <summary>
            /// Auditorium -> Seat relation
            /// </summary>
            /// <returns></returns>
            modelBuilder.Entity<Auditorium>()
                .HasMany(x => x.Seats)
                .WithOne(x => x.Auditorium)
                .IsRequired();


            /// <summary>
            /// Cinema -> Auditorium relation
            /// </summary>
            /// <returns></returns>
            modelBuilder.Entity<Cinema>()
                .HasMany(x => x.Auditoriums)
                .WithOne(x => x.Cinema)
                .IsRequired();
            
            /// <summary>
            /// Auditorium -> Cinema relation
            /// </summary>
            /// <returns></returns>
            modelBuilder.Entity<Auditorium>()
                .HasOne(x => x.Cinema)
                .WithMany(x => x.Auditoriums)
                .HasForeignKey(x => x.CinemaId)
                .IsRequired();


            /// <summary>
            /// Auditorium -> Projection relation
            /// </summary>
            /// <returns></returns>
            modelBuilder.Entity<Auditorium>()               
               .HasMany(x => x.Projections)
               .WithOne(x => x.Auditorium)
               .IsRequired();

            /// <summary>
            /// Projection -> Auditorium relation
            /// </summary>
            /// <returns></returns>
            modelBuilder.Entity<Projection>()
                .HasOne(x => x.Auditorium)
                .WithMany(x => x.Projections)
                .HasForeignKey(x => x.AuditoriumId)
                .IsRequired();


            /// <summary>
            /// Projection -> Movie relation
            /// </summary>
            /// <returns></returns>
            modelBuilder.Entity<Projection>()
                .HasOne(x => x.Movie)
                .WithMany(x => x.Projections)
                .HasForeignKey(x => x.MovieId)
                .IsRequired();

            /// <summary>
            /// Movie -> Projection relation
            /// </summary>
            /// <returns></returns>
            modelBuilder.Entity<Movie>()
                .HasMany(x => x.Projections)
                .WithOne(x => x.Movie)
                .IsRequired();

            //Composite key 
            modelBuilder.Entity<MovieActor>()
                .HasKey(ma => new { ma.MovieId, ma.ActorId });
        }
    }
}
