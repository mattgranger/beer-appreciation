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
    [Table("Events", Schema = "BA")]
    public class Event : BaseEntityWithState<int>
    {
        [Required]
        public DateTime Date { get; set; }
        [StringLength(100)]
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        [StringLength(300)]
        public string Location { get; set; }
        public int DrinkingClubId { get; set; }
        public string OwnerId { get; set; }

        public DrinkingClub DrinkingClub { get; set; }
        public Appreciator Owner { get; set; }
        public ICollection<EventRegistration> Registrations { get; set; }
        public ICollection<EventBeverage> Beverages { get; set; }

        [NotMapped]
        public ICollection<Appreciator> Appreciators { get; set; }
    }
}
