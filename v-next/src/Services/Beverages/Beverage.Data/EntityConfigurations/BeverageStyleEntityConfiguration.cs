namespace BeerAppreciation.Beverage.Data.EntityConfigurations
{
    using Domain;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class BeverageStyleEntityConfiguration : IEntityTypeConfiguration<BeverageStyle>
    {
        public void Configure(EntityTypeBuilder<BeverageStyle> builder)
        {
            builder.ToTable("BeverageStyle");

            builder.HasKey(bt => bt.Id);

            builder.Property(bt => bt.Id)
                .ForSqlServerUseSequenceHiLo("beverage_style_hilo")
                .IsRequired();

            builder.Property(bt => bt.Name)
                .IsRequired()
                .HasMaxLength(100);
        }
    }
}
