using System.Security.Claims;

namespace ApplicationLLA.Server.Extensions
{
    public static class ClaimsExtensions
    {
        public static string GetUserMail(this ClaimsPrincipal user)
        {
            return user.Claims.SingleOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
        }
    }
}
