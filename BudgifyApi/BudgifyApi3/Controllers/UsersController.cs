using BudgifyModels;
using BudgifyModels.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BudgifyApi3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        [HttpGet]
        public IEnumerable<UserDto> GetUsers()
        {
            return new List<UserDto> { 
            new UserDto {Token = "token", Username = "username"},
            new UserDto {Token = "token2", Username = "username2"},
            new UserDto {Token = "token3", Username = "username3"},
            new UserDto {Token = "token4", Username = "username4"}
            };

        }

        [HttpGet("login")]
        public Response<string> Login(UserDto user)
        {
            return new Response<string> {
                message = "mensajes",
                code = true,
                data = new List<string> { 
                    "dato1", "dato2"
                }
            };
        }
    }
}
