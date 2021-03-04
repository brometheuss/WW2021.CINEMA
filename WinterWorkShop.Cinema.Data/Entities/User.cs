using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using WinterWorkShop.Cinema.Data.Entities;

namespace WinterWorkShop.Cinema.Data
{
    [Table("user")]
    public class User
    {
        public Guid Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }
        public int Points { get; set; }

        [Column("userName")]
        public string UserName { get; set; }

        public int RoleId { get; set; }

        public virtual Role Role { get; set; }
    }
}
