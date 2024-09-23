using AutoMapper;
using ChatApp_Api.Data;
using ChatApp_Api.DTOs;
using ChatApp_Api.Entities;
using ChatApp_Api.Extensions;
using ChatApp_Api.Helpers;
using ChatApp_Api.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ChatApp_Api.Controllers
{
    [Authorize]
    public class UsersController : BaseApiController
    {
   
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IPhotoService _photoService;
        private readonly DataContext _context;

        public UsersController(IUserRepository userRepository,
                                IMapper mapper,
                                IPhotoService photoService,
                                DataContext context)
        {
           
            _userRepository = userRepository;
            _mapper = mapper;
            _photoService = photoService;
            _context = context;
        }
        [HttpGet]
        public async Task<ActionResult<PagedList<MemberDto>>> GetUsers([FromQuery]UserParams userParams)
        {
            var user = await _userRepository.GetByUsernameAsync(User.GetUserName());

            userParams.CurrentUsername = user.UserName;
            if(string.IsNullOrEmpty(userParams.Gender))
            {
                userParams.Gender = user.Gender == "male" ? "female" : "male";
            }
            var users = await _userRepository.GetMembersAsync(userParams);
            Response.AddPaginationHeader(users.CurrentPage,users.PageSize,users.TotalCount,users.TotalPages);
            return Ok(users);
        }
        //[HttpGet("{username}")]

        //public async Task<ActionResult<MemberDto>> GetUserById(string username)
        //{
        //    var user = await _userRepository.GetByUsernameAsync(username);
        //    var userreturn = _mapper.Map<MemberDto>(user);
        //    return Ok(userreturn);

        //}
        [HttpGet("{username}", Name ="GetUser")]

        public async Task<ActionResult<MemberDto>> GetUserById(string username)
        {
            return await _userRepository.GetMemberAsync(username);

        }
        [HttpPut]
        public async Task<IActionResult> UpdateUser(MemberUpdateDto memberUpdateDto)
        {
            var user = await _userRepository.GetByUsernameAsync(User.GetUserName());

            _mapper.Map(memberUpdateDto, user);

            _userRepository.Update(user);
            if(await _userRepository.SaveAllAsync())
            {
                return NoContent();
            }
            return BadRequest("Failed to update");
        }
        [HttpPost("add-photo")]
        public async Task<IActionResult> AddPhoto(IFormFile file)
        {
            var user = await _userRepository.GetByUsernameAsync(User.GetUserName());
            var result = await _photoService.AddPhotoAsync(file);
            if(result.Error != null) { return BadRequest(result.Error.Message); }
            var photo = new Photo
            {
                Url = result.SecureUrl.AbsoluteUri,
                PublicId = result.PublicId,
            };
            if(user.Photos.Count == 0)
            {
                photo.IsMain = true;
            }
            user.Photos.Add(photo);
            if(await _userRepository.SaveAllAsync())
            {
                //return Ok(_mapper.Map<PhotoDto>(photo));
                return CreatedAtRoute("GetUser",new {username = user.UserName} ,_mapper.Map<PhotoDto>(photo));
            }
                
            return BadRequest("Problem adding photo");
        }
        [HttpPut("set-main-photo/{photoId}")]
        public async Task<IActionResult> SetMainPhoto(int photoId)
        {
            var user = await _userRepository.GetByUsernameAsync(User.GetUserName());
            var photo = user.Photos.FirstOrDefault(x => x.Id == photoId);
            if (photo.IsMain) return BadRequest("This is already your main photo");
            var currentMain = user.Photos.FirstOrDefault(x => x.IsMain);
            if (currentMain != null) 
            {
                currentMain.IsMain = false;
            }

            photo.IsMain = true;
            if(await _userRepository.SaveAllAsync() )
            {
                return NoContent();
            }
            return BadRequest("Failed change main photo");
        }
        [HttpDelete("delete-photo/{photoId}")]
        public async Task<IActionResult> DeletePhoto(int photoId)
        {
            var user = await _userRepository.GetByUsernameAsync(User.GetUserName());
            var photo = user.Photos.FirstOrDefault(y => y.Id == photoId);
            if (photo == null) return NotFound();
            if (photo.IsMain) return BadRequest("You can not delete your main photo");
            if (photo.PublicId != null)
            {
                var result = await _photoService.DeletePhotoAsync(photo.PublicId);
                if (result.Error != null) return BadRequest(result.Error.Message);

            }
            user.Photos.Remove(photo);
            if (await _userRepository.SaveAllAsync())
            {
                return Ok();
            }
            return BadRequest("Failed to delete the photo");

        }
    }
}
