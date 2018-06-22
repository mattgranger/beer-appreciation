using System.Linq;
using System.Security.Claims;
using System.Security.Principal;

namespace BeerAppreciation.Core.Extensions
{
    public static class GenericPrincipalExtensions
    {
        public static string DrinkingName(this IPrincipal user)
        {
            if (!user.Identity.IsAuthenticated) return "";
            var claimsIdentity = user.Identity as ClaimsIdentity;
            if (claimsIdentity == null) return "";
            foreach (var claim in claimsIdentity.Claims.Where(claim => claim.Type == "DrinkingName"))
            {
                return claim.Value;
            }
            return "";
        }

        public static string UserId(this IPrincipal user)
        {
            if (!user.Identity.IsAuthenticated) return "";
            var claimsIdentity = user.Identity as ClaimsIdentity;
            if (claimsIdentity == null) return "";
            foreach (var claim in claimsIdentity.Claims.Where(claim => claim.Type == "UserId"))
            {
                return claim.Value;
            }
            return "";
        }
    }
}
