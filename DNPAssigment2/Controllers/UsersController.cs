using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DNPAssigment1.Login;
using DNPAssigment1.Models;
using DNPAssigment1.Persistance;
using Microsoft.AspNetCore.Mvc;

namespace DNPAssigment2.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private IUserService iUserService;

        public UsersController(IUserService iuser)
        {
            iUserService = iuser;
        }

        [HttpPost]
        public async Task<ActionResult<bool>> Login([FromBody] User loggingUser)
        {
            try
            {
                var response = await iUserService.ValidateUser(loggingUser.Username, loggingUser.Password);
                return Ok(response);
            }
            catch (Exception e)
            {
                return StatusCode(400, e.Message);
            }
            
        }
    }
}