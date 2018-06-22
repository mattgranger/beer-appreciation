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
    [Table("Manufacturers", Schema = "BA")]
    public class Manufacturer : BaseEntityWithState<int>
    {
        [StringLength(100)]
        [Required]
        [Index("IX_ManufacturerName", 1, IsUnique = true)]
        public string Name { get; set; }
        public string Description { get; set; }
        [StringLength(100)]
        public string Country { get; set; }

        public ICollection<Beverage> Beverages { get; set; }
    }
}
