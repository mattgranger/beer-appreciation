namespace Catalog.API.Infrastructure.EntityConfigurations
{
    using Domain;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class BeverageTypeEntityConfiguration : IEntityTypeConfiguration<BeverageType>
    {
        public void Configure(EntityTypeBuilder<BeverageType> builder)
        {
            builder.ToTable("BeverageType", "Catalog");

            builder.HasKey(bt => bt.Id);

            builder.Property(bt => bt.Id)
                .ForSqlServerUseSequenceHiLo("beverage_type_hilo")
                .IsRequired();

            builder.Property(bt => bt.Name)
                .IsRequired()
                .HasMaxLength(100);
        }
    }
}
