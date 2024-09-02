using ChatApp_Api.Entities;

namespace ChatApp_Api.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(AppUser user);
    }
}
