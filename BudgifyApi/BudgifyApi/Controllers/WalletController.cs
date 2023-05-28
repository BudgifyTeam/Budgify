using BudgifyBll;
using BudgifyDal;
using BudgifyModels;
using BudgifyModels.Dto;
using Microsoft.AspNetCore.Mvc;

namespace BudgifyApi.Controllers
{
    /// <summary>
    /// Represents the controller for managing wallets.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class WalletController : ControllerBase
    {
        private readonly WalletBll _walletBll;
        private readonly ResponseError resError = new ResponseError();

        /// <summary>
        /// Initializes a new instance of the <see cref="WalletController"/> class.
        /// </summary>
        /// <param name="db">The application database context.</param>
        public WalletController(AppDbContext db)
        {
            _walletBll = new WalletBll(db);
        }

        /// <summary>
        /// Creates a new wallet.
        /// </summary>
        /// <param name="userid">The ID of the user.</param>
        /// <param name="name">The name of the wallet.</param>
        /// <param name="icon">The icon of the wallet.</param>
        /// <returns>The action result containing the created wallet.</returns>
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

        /// <summary>
        /// Endpoint to delete a wallet.
        /// </summary>
        /// <param name="walletid">The ID of the wallet to delete.</param>
        /// <param name="newWallet">The ID of the new wallet to assign the expenses.</param>
        /// <returns>The action result containing the response.</returns>
        [HttpGet("DeleteWallet", Name = "DeleteWallet")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ResponseWallet>> DeleteWallet(int walletid, int newWallet)
        {
            ResponseWallet response = await _walletBll.DeleteWallet(walletid, newWallet);

            if (!response.code)
            {
                resError.message = response.message;
                resError.code = 0;

                return StatusCode(StatusCodes.Status400BadRequest, response);
            }
            return StatusCode(StatusCodes.Status200OK, response);
        }

        /// <summary>
        /// Updates a wallet.
        /// </summary>
        /// <param name="wallet">The wallet data to be modified.</param>
        /// <param name="total">The new total amount of the wallet.</param>
        /// <param name="icon">The new icon of the wallet.</param>
        /// <param name="name">The new name of the wallet.</param>
        /// <returns>The action result containing the response.</returns>
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

        /// <summary>
        /// Retrieves the wallets for a user.
        /// </summary>
        /// <param name="userid">The ID of the user.</param>
        /// <returns>The action result containing the list of wallets.</returns>
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
