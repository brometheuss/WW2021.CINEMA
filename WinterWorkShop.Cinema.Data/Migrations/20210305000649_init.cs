using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WinterWorkShop.Cinema.Data.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "actor",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_actor", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "cinema",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    CityId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cinema", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "movie",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Title = table.Column<string>(nullable: true),
                    Year = table.Column<int>(nullable: false),
                    Rating = table.Column<double>(nullable: true),
                    Current = table.Column<bool>(nullable: false),
                    HasOscar = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_movie", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "role",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_role", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "auditorium",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    cinemaId = table.Column<int>(nullable: false),
                    AuditoriumName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_auditorium", x => x.Id);
                    table.ForeignKey(
                        name: "FK_auditorium_cinema_cinemaId",
                        column: x => x.cinemaId,
                        principalTable: "cinema",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "movieActor",
                columns: table => new
                {
                    MovieId = table.Column<Guid>(nullable: false),
                    ActorId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_movieActor", x => new { x.MovieId, x.ActorId });
                    table.ForeignKey(
                        name: "FK_movieActor_actor_ActorId",
                        column: x => x.ActorId,
                        principalTable: "actor",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_movieActor_movie_MovieId",
                        column: x => x.MovieId,
                        principalTable: "movie",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    Points = table.Column<int>(nullable: false),
                    userName = table.Column<string>(nullable: true),
                    IsAdmin = table.Column<bool>(nullable: false),
                    RoleId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user", x => x.Id);
                    table.ForeignKey(
                        name: "FK_user_role_RoleId",
                        column: x => x.RoleId,
                        principalTable: "role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "projection",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    auditorium_id = table.Column<int>(nullable: false),
                    DateTime = table.Column<DateTime>(nullable: false),
                    MovieId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_projection", x => x.Id);
                    table.ForeignKey(
                        name: "FK_projection_auditorium_auditorium_id",
                        column: x => x.auditorium_id,
                        principalTable: "auditorium",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_projection_movie_MovieId",
                        column: x => x.MovieId,
                        principalTable: "movie",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "seat",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    auditorium_id = table.Column<int>(nullable: false),
                    Row = table.Column<int>(nullable: false),
                    Number = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_seat", x => x.Id);
                    table.ForeignKey(
                        name: "FK_seat_auditorium_auditorium_id",
                        column: x => x.auditorium_id,
                        principalTable: "auditorium",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "reservation",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    UserId = table.Column<int>(nullable: false),
                    ProjectionId = table.Column<int>(nullable: false),
                    UserId1 = table.Column<Guid>(nullable: true),
                    ProjectionId1 = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_reservation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_reservation_projection_ProjectionId1",
                        column: x => x.ProjectionId1,
                        principalTable: "projection",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_reservation_user_UserId1",
                        column: x => x.UserId1,
                        principalTable: "user",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "reservationSeat",
                columns: table => new
                {
                    ReservationId = table.Column<Guid>(nullable: false),
                    SeatId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_reservationSeat", x => new { x.ReservationId, x.SeatId });
                    table.ForeignKey(
                        name: "FK_reservationSeat_reservation_ReservationId",
                        column: x => x.ReservationId,
                        principalTable: "reservation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_reservationSeat_seat_SeatId",
                        column: x => x.SeatId,
                        principalTable: "seat",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_auditorium_cinemaId",
                table: "auditorium",
                column: "cinemaId");

            migrationBuilder.CreateIndex(
                name: "IX_movieActor_ActorId",
                table: "movieActor",
                column: "ActorId");

            migrationBuilder.CreateIndex(
                name: "IX_projection_auditorium_id",
                table: "projection",
                column: "auditorium_id");

            migrationBuilder.CreateIndex(
                name: "IX_projection_MovieId",
                table: "projection",
                column: "MovieId");

            migrationBuilder.CreateIndex(
                name: "IX_reservation_ProjectionId1",
                table: "reservation",
                column: "ProjectionId1");

            migrationBuilder.CreateIndex(
                name: "IX_reservation_UserId1",
                table: "reservation",
                column: "UserId1");

            migrationBuilder.CreateIndex(
                name: "IX_reservationSeat_SeatId",
                table: "reservationSeat",
                column: "SeatId");

            migrationBuilder.CreateIndex(
                name: "IX_seat_auditorium_id",
                table: "seat",
                column: "auditorium_id");

            migrationBuilder.CreateIndex(
                name: "IX_user_RoleId",
                table: "user",
                column: "RoleId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "movieActor");

            migrationBuilder.DropTable(
                name: "reservationSeat");

            migrationBuilder.DropTable(
                name: "actor");

            migrationBuilder.DropTable(
                name: "reservation");

            migrationBuilder.DropTable(
                name: "seat");

            migrationBuilder.DropTable(
                name: "projection");

            migrationBuilder.DropTable(
                name: "user");

            migrationBuilder.DropTable(
                name: "auditorium");

            migrationBuilder.DropTable(
                name: "movie");

            migrationBuilder.DropTable(
                name: "role");

            migrationBuilder.DropTable(
                name: "cinema");
        }
    }
}
