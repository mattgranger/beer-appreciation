using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace BeerAppreciation.Domain
{
    using Data.EF.Domain;

    [Table("Appreciators", Schema = "BA")]
    public class Appreciator : IdentityUser, IEntityWithState<string>
    {
        [StringLength(100)]
        public string DrinkingName { get; set; }

        public ICollection<DrinkingClub> DrinkingClubs { get; set; }

        public ICollection<EventRegistration> Registrations { get; set; } 

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<Appreciator> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            userIdentity.AddClaim(new Claim("DrinkingName", this.DrinkingName));
            userIdentity.AddClaim(new Claim("UserId", this.Id));
            userIdentity.AddClaim(new Claim("Roles", this.GetRoleString()));

            return userIdentity;
        }

        private string GetRoleString()
        {
            if (this.Roles == null || !this.Roles.Any())
            {
                return string.Empty;
            }

            return string.Join("|", this.Roles.Select(r => r.RoleId));
        }

        /// <summary>
        /// Gets or sets the state of the entity.
        /// </summary>
        [NotMapped]
        public State State { get; set; }
    }
}
