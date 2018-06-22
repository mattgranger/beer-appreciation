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
    [Table("EventRegistrations", Schema = "BA")]
    public class EventRegistration : BaseEntityWithState<int>
    {
        [Required]
        public string AppreciatorId { get; set; }
        [Required]
        public int EventId { get; set; }
        [Required]
        public DateTime RegistrationDate { get; set; }
        public string Comments { get; set; }
        /// <summary>
        /// When set as a freeloader, beer ratings can occur even
        /// if the registrant has not bought along any beer.
        /// </summary>
        public bool Freeloader { get; set; }

        public virtual Event Event { get; set; }
        public virtual Appreciator Appreciator { get; set; }
        public ICollection<EventBeverage> Beverages { get; set; }
        public ICollection<Rating> Ratings { get; set; }
    }
}
