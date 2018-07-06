namespace Catalog.API.Infrastructure.EntityConfigurations
{
    using Domain;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class BeverageStyleEntityConfiguration : IEntityTypeConfiguration<BeverageStyle>
    {
        public void Configure(EntityTypeBuilder<BeverageStyle> builder)
        {
            builder.ToTable("BeverageStyle", "Catalog");

            builder.HasKey(bt => bt.Id);

            builder.Property(bt => bt.Id)
                .ForSqlServerUseSequenceHiLo("beverage_style_hilo")
                .IsRequired();

            builder.Property(bt => bt.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.HasOne(b => b.Parent)
                .WithMany()
                .HasForeignKey(b => b.ParentId);

            builder.HasOne(b => b.BeverageType)
                .WithMany()
                .HasForeignKey(b => b.BeverageTypeId);
        }
    }
}
