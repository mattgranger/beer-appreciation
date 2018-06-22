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
    [Table("Ratings", Schema = "BA")]
    public class Rating : BaseEntityWithState<int>
    {
        [Required]
        public int EventRegistrationId { get; set; }
        [Range(0.0, 10.0)]
        public decimal Score { get; set; }
        public DateTime SubmittedDate { get; set; }
        public string Comments { get; set; }
        [Required]
        public int EventBeverageId { get; set; }

        public EventRegistration EventRegistration { get; set; }
        public EventBeverage EventBeverage { get; set; }
    }
}
