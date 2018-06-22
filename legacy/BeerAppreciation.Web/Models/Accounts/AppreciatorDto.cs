namespace BeerAppreciation.Web.Models.Accounts
{
    using System.Collections.Generic;
    using System.Linq;
    using Domain;
    using Microsoft.AspNet.Identity.EntityFramework;

    public class AppreciatorDto
    {
        public AppreciatorDto()
        {
        }

        public AppreciatorDto(Appreciator user, List<IdentityRole> securityRoles)
        {
            this.InitUser(user);
            this.InitRoles(user, securityRoles);
        }

        private void InitRoles(Appreciator user, List<IdentityRole> securityRoles)
        {
            var userRoleIds = user.Roles.Select(r => r.RoleId).ToList();
            if (!userRoleIds.Any())
            {
                return;
            }

            this.Roles = securityRoles
                .Where(sr => userRoleIds.Contains(sr.Id))
                .Select(sr => new SecurityRoleDto(sr)).ToList();
            foreach (var role in securityRoles)
            {
                if (userRoleIds.Contains(role.Id))
                {
                    
                }
            }
        }

        private void InitUser(Appreciator user)
        {
            this.Id = user.Id;
            this.DrinkingName = user.DrinkingName;
            this.UserName = user.UserName;
            this.DrinkingClubs = user.DrinkingClubs?.Select(dc => new DrinkingClubDto(dc)).ToList();
        }

        public string Id { get; set; }
        public string DrinkingName { get; set; }
        public string UserName { get; set; }
        public IList<DrinkingClubDto> DrinkingClubs { get; set; }
        public IList<SecurityRoleDto> Roles { get; set; }
    }
}