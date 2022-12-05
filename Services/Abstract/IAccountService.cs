using Microsoft.AspNetCore.Mvc;
using VoteMovie.Model;

namespace VoteMovie.Services.Abstract
{
    public interface IAccountService
    {
        JsonResult SignUp(AccountModel model);
        Task<dynamic> SignIn(LoginModel model);
    }
}
