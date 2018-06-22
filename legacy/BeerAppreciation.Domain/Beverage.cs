using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BeerAppreciation.Data.EF.Domain;

namespace BeerAppreciation.Domain
{
    [Table("Beverages", Schema = "BA")]
    public class Beverage : BaseEntityWithState<int>
    {
        [StringLength(100)]
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal? AlcoholPercent { get; set; }
        public int? Volume { get; set; }
        [StringLength(200)]
        public string Url { get; set; }
        [Required]
        public int BeverageStyleId { get; set; }
        [Required]
        public int BeverageTypeId { get; set; }
        [Required]
        public int ManufacturerId { get; set; }

        public BeverageStyle BeverageStyle { get; set; }
        public BeverageType BeverageType { get; set; }
        public Manufacturer Manufacturer { get; set; }
    }
}
