using Microsoft.AspNetCore.Mvc;
using VoteMovie.Model;
using VoteMovie.Services.Abstract;

namespace VoteMovie.Controllers;

[ApiController]
[Route("favorite")]
public class FavoriteController : GeneralController
{
    private readonly IFavoriteService _iFavoriteService;

    public FavoriteController(IFavoriteService iFavoriteService)
    {
        _iFavoriteService = iFavoriteService;
    }
    
    [HttpGet("countLikePost")]
    public JsonResult CountLikePost(int movieId)
    {
        return _iFavoriteService.CountLikePost(movieId);
    }
    
    [HttpGet("countDisLikePost")]
    public JsonResult CountDisLikePost(int movieId)
    {
        return _iFavoriteService.CountDisLikePost(movieId);
    }

    [HttpPost("likePost")]
    public JsonResult LikePost([FromBody] FavoriteModel model)
    {
        model.UserId = GetCurrentUserId();
        return _iFavoriteService.LikePost(model);
    }
    
    [HttpPost("disLikePost")]
    public JsonResult DisLikePost([FromBody] FavoriteModel model)
    {
        model.UserId = GetCurrentUserId();
        return _iFavoriteService.DisLikePost(model);
    }
    
    [HttpDelete("unlikePost")]
    public JsonResult UnlikePost([FromBody] DislikeModel model)
    {
        return _iFavoriteService.UnlikePost(model);
    }
    
    [HttpDelete("unDislikePost")]
    public JsonResult UnDislikePost([FromBody] DislikeModel model)
    {
        return _iFavoriteService.UnDislikePost(model);
    }
}