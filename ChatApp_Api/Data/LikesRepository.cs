using ChatApp_Api.DTOs;
using ChatApp_Api.Entities;
using ChatApp_Api.Extensions;
using ChatApp_Api.Helpers;
using ChatApp_Api.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ChatApp_Api.Data
{
    public class LikesRepository : ILikesRepository
    {
        private readonly DataContext _context;

        public LikesRepository(DataContext context)
        {
            _context = context;
        }
        public async Task<UserLike> GetUserLike(int sourceUserId, int likedUserId)
        {
            return await _context.Likes.FindAsync(sourceUserId, likedUserId);
        }

        public async Task<PagedList<LikeDto>> GetUserLikes(LikeParams likeParams)
        {
            var users = _context.Users.OrderBy(u=> u.UserName).AsQueryable();
            var likes = _context.Likes.AsQueryable();
            if(likeParams.Predicate == "liked")
            {
                likes = likes.Where(like => like.SourceUserId == likeParams.UserId);
                users = likes.Select(like => like.LikedUser);
            }
            if(likeParams.Predicate == "likedBy")
            {
                likes = likes.Where(like => like.LikedUserId == likeParams.UserId);
                users = likes.Select(like => like.SourceUser);
            }
            var likeUsers = users.Select(user => new LikeDto
            {
                Username = user.UserName,
                KnownAs = user.KnownAs,
                Age = user.DateOfBirth.CalculateAge(),
                PhotoUrl = user.Photos.FirstOrDefault(x=> x.IsMain).Url,
                City = user.City,
                Id = user.Id,
            });
            return await PagedList<LikeDto>.CreateAsync(likeUsers, likeParams.PageNumber, likeParams.PageSize);
        }

        public async Task<AppUser> GetUserWithLikes(int userId)
        {
            return await _context.Users
                .Include(x=>x.LikedUsers)
                .FirstOrDefaultAsync(x=>x.Id == userId);
        }
    }
}
