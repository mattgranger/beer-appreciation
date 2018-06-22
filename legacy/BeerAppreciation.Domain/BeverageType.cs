namespace BeerAppreciation.Domain
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Data.EF.Domain;

    [Table("BeverageTypes", Schema = "BA")]
    public class BeverageType : BaseEntityWithState<int>
    {
        [StringLength(100)]
        [Required]
        [Index("IX_BeverageTypeName", 1, IsUnique = true)]
        public string Name { get; set; }
        public string Description { get; set; }

        public ICollection<BeverageStyle> Styles { get; set; } 
    }
}
