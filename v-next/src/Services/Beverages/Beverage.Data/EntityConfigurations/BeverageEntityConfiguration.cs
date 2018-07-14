namespace BeerAppreciation.Beverage.Data.EntityConfigurations
{
    using Domain;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class BeverageEntityConfiguration : IEntityTypeConfiguration<Beverage>
    {
        public void Configure(EntityTypeBuilder<Beverage> builder)
        {
            builder.ToTable("Beverage");

            builder.HasKey(b => b.Id);

            builder.Property(b => b.Id)
                .ForSqlServerUseSequenceHiLo("beverage_hilo")
                .IsRequired();

            builder.Property(b => b.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(b => b.Description)
                .IsRequired(false);

            builder.Property(b => b.AlcoholPercent)
                .HasColumnType("decimal(5, 2)")
                .IsRequired(false);

            builder.Property(b => b.Volume)
                .IsRequired(false);

            builder.Property(b => b.Url)
                .IsRequired(false)
                .HasMaxLength(200);
        }
    }
}
