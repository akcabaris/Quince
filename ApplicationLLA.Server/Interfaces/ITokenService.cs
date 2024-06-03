using ApplicationLLA.Server.Models;

namespace ApplicationLLA.Server.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(AppUser user, IList<string> userRoles);
    }
}
