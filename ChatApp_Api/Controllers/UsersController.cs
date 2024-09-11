using AutoMapper;
using ChatApp_Api.Data;
using ChatApp_Api.DTOs;
using ChatApp_Api.Entities;
using ChatApp_Api.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ChatApp_Api.Controllers
{
    [Authorize]
    public class UsersController : BaseApiController
    {
   
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UsersController(IUserRepository userRepository,IMapper mapper)
        {
           
            _userRepository = userRepository;
            _mapper = mapper;
        }
      
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers()
        {
            var users = await _userRepository.GetMembersAsync();
            return Ok(users);
        }
        //[HttpGet("{username}")]

        //public async Task<ActionResult<MemberDto>> GetUserById(string username)
        //{
        //    var user = await _userRepository.GetByUsernameAsync(username);
        //    var userreturn = _mapper.Map<MemberDto>(user);
        //    return Ok(userreturn);

        //}
        [HttpGet("{username}")]

        public async Task<ActionResult<MemberDto>> GetUserById(string username)
        {
            return await _userRepository.GetMemberAsync(username);

        }

    }
}
