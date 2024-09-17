﻿using ChatApp_Api.Extensions;
using ChatApp_Api.Helpers;
using ChatApp_Api.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChatApp_Api.Controllers
{
    [Authorize]
    public class LikesController : BaseApiController
    {
        private readonly IUserRepository _userRepository;
        private readonly ILikesRepository _likesRepository;

        public LikesController(IUserRepository userRepository,
                                ILikesRepository likesRepository)
        {
            _userRepository = userRepository;
            _likesRepository = likesRepository;
        }
        [HttpPost("{username}")]
        public async Task<IActionResult> AddLike(string username) 
        { 
            var sourceUserId = User.GetUserId();
            var likedUser = await _userRepository.GetByUsernameAsync(username);
            var sourceUser = await _likesRepository.GetUserWithLikes(sourceUserId);
            if(likedUser == null)  return NotFound();
            if (sourceUser.UserName == username) return BadRequest("You cannot like yourself");
            var userLike = await _likesRepository.GetUserLike(sourceUserId, likedUser.Id);
            if (userLike != null) return BadRequest("You already like this user");
            userLike = new Entities.UserLike
            {
                SourceUserId = sourceUserId,
                LikedUserId = likedUser.Id
            };
            sourceUser.LikedUsers.Add(userLike);
            if (await _userRepository.SaveAllAsync()) return Ok();
            return BadRequest("Failed to like user");

        }
        [HttpGet]
        public async Task<IActionResult> GetUserLikes([FromQuery]LikeParams likeParams)
        {
            likeParams.UserId = User.GetUserId();
            var users = await _likesRepository.GetUserLikes(likeParams);
            Response.AddPaginationHeader(users.CurrentPage, users.PageSize,users.TotalCount,users.TotalPages);
            return Ok(users);
        }
    }
}
