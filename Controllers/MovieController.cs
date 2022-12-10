using Microsoft.AspNetCore.Mvc;
using VoteMovie.Services.Abstract;

namespace VoteMovie.Controllers
{
    [ApiController]
    [Route("movie")]
    public class MovieController : GeneralController
    {
        private readonly IMovieService _iMovieService;

        public MovieController(IMovieService iMovieService)
        {
            _iMovieService = iMovieService;
        }

        [HttpGet("getList")]
        public JsonResult GetList(int? pageNumber, int? pageSize, string? searchText)
        {
            return _iMovieService.GetList(pageNumber, pageSize, searchText);
        }
    }
}
