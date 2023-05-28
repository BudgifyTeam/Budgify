using BudgifyBll;
using BudgifyDal;
using BudgifyModels;
using BudgifyModels.Dto;
using Microsoft.AspNetCore.Mvc;

namespace BudgifyApi.Controllers
{
    /// <summary>
    /// API controller for managing pockets.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class PocketController : ControllerBase
    {
        private readonly PocketBll _PocketBll;
        private readonly ResponseError resError = new ResponseError();

        /// <summary>
        /// Initializes a new instance of the <see cref="PocketController"/> class.
        /// </summary>
        /// <param name="db">The database context.</param>
        public PocketController(AppDbContext db)
        {
            _PocketBll = new PocketBll(db);
        }

        /// <summary>
        /// Creates a new pocket for a user.
        /// </summary>
        /// <param name="userid">The ID of the user.</param>
        /// <param name="name">The name of the pocket.</param>
        /// <param name="icon">The icon of the pocket.</param>
        /// <param name="goal">The goal amount of the pocket.</param>
        /// <returns>An ActionResult containing the response with the created pocket.</returns>
        [HttpPost("CreatePocket", Name = "CreatePocket")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ResponsePocket>> CreatePocket(int userid, string name, string icon, double goal)
        {
            ResponsePocket response = await _PocketBll.CreatePocket(userid, name, icon, goal);

            if (!response.code)
            {
                resError.message = response.message;
                resError.code = 0;

                return StatusCode(StatusCodes.Status400BadRequest, response);
            }
            return StatusCode(StatusCodes.Status200OK, response);
        }

        /// <summary>
        /// Deletes a pocket.
        /// </summary>
        /// <param name="pocketid">The ID of the pocket to be deleted.</param>
        /// <param name="newPocket">The ID of the pocket to be set as the new pocket.</param>
        /// <returns>An ActionResult containing the response after deleting the pocket.</returns>
        [HttpGet("DeletePocket", Name = "DeletePocket")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ResponsePocket>> DeletePocket(int pocketid, int newPocket)
        {
            ResponsePocket response = await _PocketBll.DeletePocket(pocketid, newPocket);

            if (!response.code)
            {
                resError.message = response.message;
                resError.code = 0;

                return StatusCode(StatusCodes.Status400BadRequest, response);
            }
            return StatusCode(StatusCodes.Status200OK, response);
        }

        /// <summary>
        /// Modifies a pocket.
        /// </summary>
        /// <param name="pocket">The pocket object with updated information.</param>
        /// <param name="total">The total amount of the pocket.</param>
        /// <param name="icon">The icon of the pocket.</param>
        /// <param name="name">The name of the pocket.</param>
        /// <returns>An ActionResult containing the response after modifying the pocket.</returns>
        [HttpPut("ModifyPocket", Name = "ModifyPocket")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ResponsePocket>> ModifyPocket([FromBody] PocketDto pocket, double total, string icon, string name)
        {
            ResponsePocket response = await _PocketBll.ModifyPocket(pocket, total, icon, name);

            if (!response.code)
            {
                resError.message = response.message;
                resError.code = 0;

                return StatusCode(StatusCodes.Status400BadRequest, response);
            }
            return StatusCode(StatusCodes.Status200OK, response);
        }

        /// <summary>
        /// Retrieves pockets for a user.
        /// </summary>
        /// <param name="userid">The ID of the user.</param>
        /// <returns>An ActionResult containing the response with the list of pockets.</returns>
        [HttpGet("GetPockets", Name = "GetPockets")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<PocketDto>> GetPockets(int userid) { 
            ResponseList<PocketDto> response = _PocketBll.GetPockets(userid);

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
