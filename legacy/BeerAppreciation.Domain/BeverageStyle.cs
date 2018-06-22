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
    [Table("BeverageStyles", Schema = "BA")]
    public class BeverageStyle : BaseEntityWithState<int>
    {
        [StringLength(100)]
        [Required]
        [Index("IX_BeverageStyleName", 1, IsUnique = true)]
        public string Name { get; set; }
        public string Description { get; set; }
        public int? ParentId { get; set; }
        public int BeverageTypeId { get; set; }

        public BeverageStyle Parent { get; set; }
        public BeverageType BeverageType { get; set; }

        public ICollection<Beverage> Beverages { get; set; } 
    }
}
