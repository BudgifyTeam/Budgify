using BudgifyBll;
using BudgifyDal;
using BudgifyModels;
using BudgifyModels.Dto;
using Microsoft.AspNetCore.Mvc;

namespace BudgifyApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PocketController : ControllerBase
    {
        private readonly PocketBll _PocketBll;
        private readonly ResponseError resError = new ResponseError();

        public PocketController(AppDbContext db)
        {
            _PocketBll = new PocketBll(db);
        }
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
