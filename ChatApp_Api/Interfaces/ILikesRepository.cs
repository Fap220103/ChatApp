using ChatApp_Api.DTOs;
using ChatApp_Api.Entities;
using ChatApp_Api.Helpers;

namespace ChatApp_Api.Interfaces
{
    public interface ILikesRepository
    {
        Task<UserLike> GetUserLike(int sourceUserId, int likedUserId);
        Task<AppUser> GetUserWithLikes(int userId);
        Task<PagedList<LikeDto>> GetUserLikes(LikeParams likeParams);

    }
}
