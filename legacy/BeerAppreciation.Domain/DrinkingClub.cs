using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BeerAppreciation.Data.EF.Domain;

namespace BeerAppreciation.Domain
{
    [Table("DrinkingClubs", Schema = "BA")]
    public class DrinkingClub : BaseEntityWithState<int>
    {
        [StringLength(100)]
        [Required]
        [Index("IX_DrinkingClubName", 1, IsUnique = true)]
        public string Name { get; set; }
        public string Description { get; set; }
        [StringLength(200)]
        public string PasswordHash { get; set; }
        public bool IsPrivate { get; set; }

        public ICollection<Appreciator> Members { get; set; }
        public ICollection<Event> Events { get; set; }
    }
}
