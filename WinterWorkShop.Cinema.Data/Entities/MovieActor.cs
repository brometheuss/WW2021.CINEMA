using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace WinterWorkShop.Cinema.Data.Entities
{
    [Table("movieActor")]
    public class MovieActor
    {
        public int MovieId { get; set; }
        public int ActorId { get; set; }
        public Movie Movie { get; set; }
        public Actor Actor { get; set; }

    }
}
