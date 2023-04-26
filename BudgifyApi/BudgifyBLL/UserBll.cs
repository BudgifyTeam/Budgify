using BudgifyModels;
using BudgifyDal;

namespace BudgifyBll
{
    public class UserBll
    {
        //private readonly UserDal userDal;
        public Response<string> Register() { 
            Response<string> response = new Response<string>();
            try {
                string result = "hola";
                List<string> resultList = new List<string>();
                if (result != "error")
                {
                    response.message = result;
                    resultList.Add("sampple");
                    response.data = resultList;
                    response.code = true;
                }
                else {
                    response.message = "Error al registrar al usuario";
                    response.code = false;
                }
            }
            catch (Exception ex)
            {
                response.message = ex.Message;
                response.code = false;
            }
            return response;
        }
    }
}
