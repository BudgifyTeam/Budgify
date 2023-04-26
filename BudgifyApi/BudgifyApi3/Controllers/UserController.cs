using BudgifyModels;
using BudgifyBll;
using System.Web.Http;
using System.Web;
using System.Net.Http;
using System.Net;

namespace BudgifyApi.Controllers
{
    public class UserController: ApiController
    {
        private readonly UserBll userBll = new UserBll();
        private readonly ResponseError resError = new ResponseError();

        [HttpPost]
        [Route("api/user/register")]
        public HttpResponseMessage Register() 
        {
            Response<string> response = userBll.Register();

            if (!response.code)
            {
                resError.message = response.message;
                resError.code = 0;

                return Request.CreateResponse(HttpStatusCode.BadRequest, resError);
            }
            return Request.CreateResponse(HttpStatusCode.OK, response);
        }
    }


}
