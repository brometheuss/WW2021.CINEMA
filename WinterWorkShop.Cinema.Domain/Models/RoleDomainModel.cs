using System;
using System.Collections.Generic;
using System.Text;

namespace WinterWorkShop.Cinema.Domain.Models
{
    public class RoleDomainModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<UserDomainModel> Users { get; set; }
    }
}
