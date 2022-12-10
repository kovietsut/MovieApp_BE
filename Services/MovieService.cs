using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VoteMovie.Entity;
using VoteMovie.Model.Config;
using VoteMovie.Repositories.Abstract;
using VoteMovie.Services.Abstract;

namespace VoteMovie.Services
{
    public class MovieService : IMovieService
    {
        private readonly IUnitOfWork _unitOfWork;
        public MovieService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public JsonResult GetList(int? pageNumber, int? pageSize, string? searchText)
        {
            if (pageNumber == null)
            {
                pageNumber = 1;
            }

            if (pageSize == null)
            {
                pageSize = 10;
            }

            var data = _unitOfWork.RepositoryCRUD<Movie>()
                .GetAll(x => x.IsEnabled == true)
                .OrderByDescending(x => x.Id)
                .Select(x => new
                {
                    x.Id,
                    x.Title,
                    x.ImageContent,
                    likes = _unitOfWork.RepositoryCRUD<Favorite>().Set().Count(y => y.MovieId == x.Id && y.TypeOfFavorite == "Like")
                });
            var listData = data.Skip(((int)pageNumber - 1) * (int)pageSize)
                .Take((int)pageSize).OrderByDescending(x => x.Id).ToList();
            return JsonUtil.Success(listData, dataCount: data.Count());
        }
    }
}
