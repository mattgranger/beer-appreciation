namespace Catalog.API.Infrastructure.EntityConfigurations
{
    using Domain;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class BeverageEntityConfiguration : IEntityTypeConfiguration<Beverage>
    {
        public void Configure(EntityTypeBuilder<Beverage> builder)
        {
            builder.ToTable("Beverage", "Catalog");

            builder.Property(b => b.Id)
                .ForSqlServerUseSequenceHiLo("beverage_hilo")
                .IsRequired();

            builder.Property(b => b.Name)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(b => b.Description)
                .IsRequired(false);

            builder.Property(b => b.AlcoholPercent)
                .IsRequired(false);

            builder.Property(b => b.Volume)
                .IsRequired(false);

            builder.Property(b => b.Url)
                .IsRequired(false)
                .HasMaxLength(200);

            builder.HasOne(b => b.BeverageStyle)
                .WithMany()
                .HasForeignKey(b => b.BeverageStyleId);

            builder.HasOne(b => b.BeverageType)
                .WithMany()
                .HasForeignKey(b => b.BeverageTypeId);

            builder.HasOne(b => b.Manufacturer)
                .WithMany()
                .HasForeignKey(b => b.ManufacturerId);
        }
    }
}
