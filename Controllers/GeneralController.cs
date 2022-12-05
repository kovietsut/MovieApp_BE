using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using VoteMovie.Infrastructure.Utils;

namespace VoteMovie.Controllers
{
    public class GeneralController : ControllerBase
    {
        protected int GetCurrentUserId()
        {
            ClaimsPrincipal currentUser = this.User;
            var userId = currentUser.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);
            return userId.Value.ToSafeInt();
        }
    }
}
