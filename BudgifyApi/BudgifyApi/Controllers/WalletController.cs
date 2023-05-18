using BudgifyBll;
using BudgifyDal;
using BudgifyModels;
using BudgifyModels.Dto;
using Microsoft.AspNetCore.Mvc;

namespace BudgifyApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalletController : ControllerBase
    {
        private readonly WalletBll _walletBll;
        private readonly ResponseError resError = new ResponseError();

        public WalletController(AppDbContext db)
        {
            _walletBll = new WalletBll(db);
        }
        [HttpPost("CreateWallet", Name = "CreateWallet")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ResponseWallet>> CreateWallet(int userid, string name, string icon)
        {
            ResponseWallet response = await _walletBll.CreateWallet(userid, name, icon);

            if (!response.code)
            {
                resError.message = response.message;
                resError.code = 0;

                return StatusCode(StatusCodes.Status400BadRequest, response);
            }
            return StatusCode(StatusCodes.Status200OK, response);
        }

        [HttpGet("DeleteWallet", Name = "DeleteWallet")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ResponseWallet>> DeleteWallet(int walletid)
        {
            ResponseWallet response = await _walletBll.DeleteWallet(walletid);

            if (!response.code)
            {
                resError.message = response.message;
                resError.code = 0;

                return StatusCode(StatusCodes.Status400BadRequest, response);
            }
            return StatusCode(StatusCodes.Status200OK, response);
        }

        [HttpPut("ModifyWallet", Name = "ModifyWallet")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ResponseWallet>> ModifyWallet([FromBody] WalletDto wallet, double total, string icon, string name)
        {
            ResponseWallet response = await _walletBll.ModifyWallet(wallet, total, icon, name);

            if (!response.code)
            {
                resError.message = response.message;
                resError.code = 0;

                return StatusCode(StatusCodes.Status400BadRequest, response);
            }
            return StatusCode(StatusCodes.Status200OK, response);
        }

        [HttpGet("GetWallets", Name = "GetWallets")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<WalletDto>> GetWallets(int userid) { 
            ResponseList<WalletDto> response = _walletBll.GetWallets(userid);

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
