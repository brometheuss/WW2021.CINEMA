using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WinterWorkShop.Cinema.Domain.Common;
using WinterWorkShop.Cinema.Domain.Interfaces;
using WinterWorkShop.Cinema.Domain.Models;

namespace WinterWorkShop.Cinema.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RolesController : ControllerBase
    {
        private readonly IRoleService _roleService;

        public RolesController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RoleDomainModel>>> GetAsync()
        {
            var roles = await _roleService.GetAllAsync();

            if(roles == null)
            {
                roles = new List<RoleDomainModel>();
            }

            return Ok(roles);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<RoleDomainModel>> GetByIdAsync(int id)
        {
            var role = await _roleService.GetByIdAsync(id);

            if(role == null)
            {
                return NotFound(Messages.ROLE_DOES_NOT_EXIST);
            }

            return Ok(role);
        }
    }
}
