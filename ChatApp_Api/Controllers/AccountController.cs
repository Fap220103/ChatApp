using AutoMapper;
using ChatApp_Api.Data;
using ChatApp_Api.DTOs;
using ChatApp_Api.Entities;
using ChatApp_Api.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace ChatApp_Api.Controllers
{

    public class AccountController : BaseApiController
    {
        private readonly DataContext _context;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;

        public AccountController(DataContext context,
                                ITokenService tokenService,
                                IMapper mapper)
        {
            _context = context;
            _tokenService = tokenService;
            _mapper = mapper;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            if(await UserExists(registerDto.Username))
            {
                return BadRequest("username is already exists");
            }
            var user = _mapper.Map<AppUser>(registerDto);
            using var hmac = new HMACSHA512();

            user.UserName = registerDto.Username.ToLower();
            user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password));
            user.PasswordSalt = hmac.Key;
            
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return Ok(new UserDto
            {
                Username = user.UserName,
                Token = _tokenService.CreateToken(user),
                KnownAs = user.KnownAs,
                Gender = user.Gender,
            });
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            var user = await _context.Users
                .Include(p=>p.Photos)
                .SingleOrDefaultAsync(u=>u.UserName == loginDto.Username);
            if(user == null)
            {
                return Unauthorized("Invalid username");
            }
            using var hmac = new HMACSHA512(user.PasswordSalt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));
            for(int i =0;i< computedHash.Length; i++)
            {
                if (computedHash[i] != user.PasswordHash[i]) 
                    return Unauthorized("Invalid Password");
            }        
            return Ok(new UserDto
            {
                Username = user.UserName,
                Token = _tokenService.CreateToken(user),
                PhotoUrl = user.Photos.FirstOrDefault(x=>x.IsMain)?.Url,
                KnownAs = user.KnownAs,
                Gender = user.Gender,
            });
        }
        private async Task<bool> UserExists(string username)
        {
            return await _context.Users.AnyAsync(x=>x.UserName == username.ToLower());
        }
    }
}
