using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Holod.Models.Database.Configs
{
    public class StuffConfig : IEntityTypeConfiguration<Stuff>
    {
        public void Configure(EntityTypeBuilder<Stuff> builder)
        {
            builder
                .HasOne(s => s.StudentCity)
                .WithMany(st => st.Stuffs)
                .HasForeignKey(s => s.StudentCityId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
