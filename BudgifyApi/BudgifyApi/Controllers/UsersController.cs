using BudgifyBll;
using BudgifyDal;
using BudgifyModels;
using BudgifyModels.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace BudgifyApi3.Controllers
{
    /// <summary>
    /// Controller for managing users.
    /// </summary>
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

        /// <summary>
        /// Registers a new user.
        /// </summary>
        /// <param name="user">The user registration information.</param>
        /// <returns>The registered user.</returns>
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

        /// <summary>
        /// Logs in a user.
        /// </summary>
        /// <param name="user">The user login information.</param>
        /// <returns>The user session.</returns>
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

        /// <summary>
        /// Modifies a user's information.
        /// </summary>
        /// <param name="userid">The ID of the user to modify.</param>
        /// <param name="icon">The new user icon.</param>
        /// <param name="name">The new user name.</param>
        /// <param name="email">The new user email.</param>
        /// <param name="publicAccount">The new public account status.</param>
        /// <param name="token">The user token.</param>
        /// <returns>The modified user session.</returns>
        [HttpPut("ModifyUser", Name = "ModifyUser")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<SessionDto>> ModifyUser(int userid, string icon, string name, string email, Boolean publicAccount, string token)
        {
            Response<SessionDto> response = await userBll.ModifyUser(userid, icon, name, email, publicAccount, token);

            if (!response.code)
            {
                resError.message = response.message;
                resError.code = 0;

                return StatusCode(StatusCodes.Status400BadRequest, response);
            }
            return StatusCode(StatusCodes.Status200OK, response);
        }

        /// <summary>
        /// Deletes a user.
        /// </summary>
        /// <param name="userid">The ID of the user to delete.</param>
        /// <returns>The response indicating the result of the delete operation.</returns>
        [HttpGet("DeleteUser", Name = "DeleteUser")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Response<string>>> DeleteUser(int userid)
        {
            Response<string> response = await userBll.DeleteUser(userid);

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
