using ChatApp_Api.Entities;

namespace ChatApp_Api.Interfaces
{
    public interface ITokenService
    {
        Task<string> CreateToken(AppUser user);
    }
}
