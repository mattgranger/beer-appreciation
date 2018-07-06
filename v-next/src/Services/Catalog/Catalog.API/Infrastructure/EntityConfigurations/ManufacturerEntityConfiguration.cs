namespace Catalog.API.Infrastructure.EntityConfigurations
{
    using Domain;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class ManufacturerEntityConfiguration : IEntityTypeConfiguration<Manufacturer>
    {
        public void Configure(EntityTypeBuilder<Manufacturer> builder)
        {
            builder.ToTable("Manufacturer", "Catalog");

            builder.HasKey(bt => bt.Id);

            builder.Property(bt => bt.Id)
                .ForSqlServerUseSequenceHiLo("manufacturer_hilo")
                .IsRequired();

            builder.Property(bt => bt.Name)
                .IsRequired()
                .HasMaxLength(100);
        }
    }
}
