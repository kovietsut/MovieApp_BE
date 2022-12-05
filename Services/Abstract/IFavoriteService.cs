using Microsoft.AspNetCore.Mvc;
using VoteMovie.Model;

namespace VoteMovie.Services.Abstract
{
    public interface IFavoriteService
    {
        JsonResult CountLikePost(int movieId);
        JsonResult CountDisLikePost(int movieId);
        JsonResult LikePost(FavoriteModel model);
        JsonResult UnlikePost(DislikeModel model);
        JsonResult DisLikePost(FavoriteModel model);
        JsonResult UnDislikePost(DislikeModel model);
    }
}
