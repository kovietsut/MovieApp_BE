using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VoteMovie.Model;
using VoteMovie.Services.Abstract;

namespace VoteMovie.Controllers
{
    [ApiController]
    [Route("user")]
    public class UserController : GeneralController
    {
        private readonly IAccountService _iAccountService;

        public UserController(IAccountService iAccountService)
        {
            _iAccountService = iAccountService;
        }


        [AllowAnonymous]
        [HttpPost("signIn")]
        public async Task<JsonResult> SignIn([FromBody] LoginModel model)
        {
            return await _iAccountService.SignIn(model);
        }

        [AllowAnonymous]
        [HttpPost("signUp")]
        public JsonResult SignUp([FromBody] AccountModel model)
        {
            return _iAccountService.SignUp(model);
        }
    }
}
