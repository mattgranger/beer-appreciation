using Microsoft.AspNet.Identity.EntityFramework;

namespace BeerAppreciation.Web.Models.Accounts
{
    public class SecurityRoleDto
    {
        public SecurityRoleDto()
        {
        }

        public SecurityRoleDto(IdentityRole sr)
        {
            this.Id = sr.Id;
            this.Name = sr.Name;
        }

        public string Id { get; set; }
        public string Name { get; set; }
    }
}