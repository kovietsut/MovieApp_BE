using Microsoft.AspNetCore.Mvc;
using VoteMovie.Entity;
using VoteMovie.Model;
using VoteMovie.Model.Config;
using VoteMovie.Repositories.Abstract;
using VoteMovie.Services.Abstract;

namespace VoteMovie.Services
{
    public class FavoriteService : IFavoriteService
    {
        private readonly IUnitOfWork _unitOfWork;

        public FavoriteService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public JsonResult CountLikePost(int movieId)
        {
            try
            {
                if (movieId == null) return JsonUtil.Error("Movie Id cannot be null");
                var list = _unitOfWork.RepositoryCRUD<Favorite>()
                    .GetAll(x => x.MovieId == movieId && x.TypeOfFavorite == "Like");
                return JsonUtil.Success(list.Count());
            }
            catch (Exception e)
            {
                return JsonUtil.Error("Something went wrong");
            }
        }

        public JsonResult CountDisLikePost(int movieId)
        {
            try
            {
                if (movieId == null) return JsonUtil.Error("Movie Id cannot be null");
                var list = _unitOfWork.RepositoryCRUD<Favorite>()
                    .GetAll(x => x.MovieId == movieId && x.TypeOfFavorite == "Dislike");
                return JsonUtil.Success(list.Count());
            }
            catch (Exception e)
            {
                return JsonUtil.Error("Something went wrong");
            }
        }

        public JsonResult LikePost(FavoriteModel model)
        {
            try
            {
                if (model.MovieId == null) return JsonUtil.Error("Movie Id cannot be null");
                if (model.UserId == null) return JsonUtil.Error("User Id cannot be null");
                var checkMovie = _unitOfWork.RepositoryCRUD<Favorite>()
                    .Any(x => x.Id == model.MovieId && x.UserId == model.UserId);
                if (checkMovie) return JsonUtil.Error("Movie is liked");
                var favorite = new Favorite()
                {
                    IsEnabled = true,
                    UserId = model.UserId,
                    MovieId = model.MovieId,
                    TypeOfFavorite = "Like"
                };
                _unitOfWork.RepositoryCRUD<Favorite>().Insert(favorite);
                _unitOfWork.Commit();
                return JsonUtil.Success(favorite.Id);
            }
            catch (Exception e)
            {
                return JsonUtil.Error("Something went wrong");
            }
        }

        public JsonResult UnlikePost(DislikeModel model)
        {
            try
            {
                var favorite = _unitOfWork.RepositoryCRUD<Favorite>().GetSingle(x => x.Id == model.FavoriteId);
                _unitOfWork.RepositoryCRUD<Favorite>().Delete(favorite.Id);
                _unitOfWork.Commit();
                return JsonUtil.Success(favorite.Id);
            }
            catch (Exception e)
            {
                return JsonUtil.Error("Something went wrong");
            }
        }

        public JsonResult DisLikePost(FavoriteModel model)
        {
            try
            {
                if (model.MovieId == null) return JsonUtil.Error("Movie Id cannot be null");
                if (model.UserId == null) return JsonUtil.Error("User Id cannot be null");
                var checkMovie = _unitOfWork.RepositoryCRUD<Favorite>()
                    .Any(x => x.Id == model.MovieId && x.UserId == model.UserId);
                if (checkMovie) return JsonUtil.Error("Movie is disliked");
                var favorite = new Favorite()
                {
                    IsEnabled = true,
                    UserId = model.UserId,
                    MovieId = model.MovieId,
                    TypeOfFavorite = "Dislike"
                };
                _unitOfWork.RepositoryCRUD<Favorite>().Insert(favorite);
                _unitOfWork.Commit();
                return JsonUtil.Success(favorite.Id);
            }
            catch (Exception e)
            {
                return JsonUtil.Error("Something went wrong");
            }
        }

        public JsonResult UnDislikePost(DislikeModel model)
        {
            try
            {
                var favorite = _unitOfWork.RepositoryCRUD<Favorite>().GetSingle(x => x.Id == model.FavoriteId);
                _unitOfWork.RepositoryCRUD<Favorite>().Delete(favorite.Id);
                _unitOfWork.Commit();
                return JsonUtil.Success(favorite.Id);
            }
            catch (Exception e)
            {
                return JsonUtil.Error("Something went wrong");
            }
        }
    }
}