using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace WinterWorkShop.Cinema.Data
{
    [Table("user")]
    public class User
    {
        public Guid Id { get; set; }

        [Column("FirstName")]
        public string Name { get; set; }

        [Column("username")]
        public string UserName { get; set; }        

        [Column("Active")]
        public bool IsAdmin { get; set; }
    }
}
