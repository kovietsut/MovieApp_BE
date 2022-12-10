using Microsoft.AspNetCore.Mvc;

namespace VoteMovie.Services.Abstract
{
    public interface IMovieService
    {
        JsonResult GetList(int? pageNumber, int? pageSize, string? searchText);
    }
}
