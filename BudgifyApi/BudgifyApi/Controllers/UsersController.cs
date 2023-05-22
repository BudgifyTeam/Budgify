using BudgifyBll;
using BudgifyDal;
using BudgifyModels;
using BudgifyModels.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace BudgifyApi3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserBll userBll;
        private readonly ResponseError resError = new ResponseError();

        public UsersController(AppDbContext db)
        {
            userBll = new UserBll(db);
        }

        [HttpPost("Register", Name = "RegisterUser")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UserRegister>> RegisterUser([FromBody] UserRegister user)
        {
            Response<user> response = await userBll.Register(user);

            if (!response.code)
            {
                resError.message = response.message;
                resError.code = 0;

                return StatusCode(StatusCodes.Status400BadRequest, response);
            }
            return StatusCode(StatusCodes.Status200OK, response);
        }

        [HttpPost("Login", Name = "LoginUser")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<SessionDto> LoginUser([FromBody] UserLogin user)
        {
            Response<SessionDto> response = userBll.Login(user);

            if (!response.code)
            {
                resError.message = response.message;
                resError.code = 0;

                return StatusCode(StatusCodes.Status400BadRequest, response);
            }
            return StatusCode(StatusCodes.Status200OK, response);
        }

        [HttpPut("ModifyUser", Name = "ModifyUser")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<SessionDto> ModifyUser([FromBody] user user, string icon, string name, string email, Boolean publicAccount, string token)
        {
            Response<SessionDto> response = userBll.ModifyUser(user, icon, name, email, publicAccount, token);

            if (!response.code)
            {
                resError.message = response.message;
                resError.code = 0;

                return StatusCode(StatusCodes.Status400BadRequest, response);
            }
            return StatusCode(StatusCodes.Status200OK, response);
        }

        [HttpGet("DeleteUser", Name = "DeleteUser")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Response<string>> DeleteUser(int userid)
        {
            Response<string> response = userBll.DeleteUser(userid);

            if (!response.code)
            {
                resError.message = response.message;
                resError.code = 0;

                return StatusCode(StatusCodes.Status400BadRequest, response);
            }
            return StatusCode(StatusCodes.Status200OK, response);
        }
    }
}
