using DatingAPI.Models;

namespace DatingAPI.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(AppUser user);
    }
}
