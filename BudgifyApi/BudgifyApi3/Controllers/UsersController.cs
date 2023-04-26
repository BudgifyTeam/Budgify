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

        [HttpGet("all", Name = "GetUsers")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IEnumerable<UserDto> GetUsers()
        {
            return new List<UserDto> {
            new UserDto {Token = "token", Username = "username"},
            new UserDto {Token = "token2", Username = "username2"},
            new UserDto {Token = "token3", Username = "username3"},
            new UserDto {Token = "token4", Username = "username4"}
            };

        }

        [HttpGet("Login", Name = "LoginUSer")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public Response<string> Login([FromBody] UserDto user)
        {
            return new Response<string>
            {
                message = "mensajes",
                code = true,
                data = new List<string> {
                    "dato1", "dato2"
                }
            };
        }

        [HttpPost("Register", Name = "RegisterUser")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<UserDto> RegisterUser([FromBody] UserDto user)
        {
            //Se creó y guardó
            //entonces se retorna el objeto creato con el endpoint get que corresponda.
            return CreatedAtRoute("LoginUSer", user);
        }

        [HttpPut("Modify", Name = "ModifyUser")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public Response<string> UpdateUser([FromBody] UserDto user)
        {
            return new Response<string>
            {
                message = "mensajes",
                code = true,
                data = new List<string> {
                    "dato1", "dato2"
                }
            };
        }
    }
}
