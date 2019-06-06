using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Holod.Models.Database.Configs
{
    public class HostelConfig : IEntityTypeConfiguration<Hostel>
    {
        public void Configure(EntityTypeBuilder<Hostel> builder)
        {
            builder
                .HasMany(h => h.Stuffs)
                .WithOne(s => s.Hostel)
                .HasForeignKey(s => s.HostelId)
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .HasMany(h => h.Residents)
                .WithOne(r => r.Hostel)
                .HasForeignKey(r => r.HostelId)
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .HasOne(h => h.StudentCity)
                .WithMany(st => st.Hostels)
                .HasForeignKey(h => h.StudentCityId)
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .HasMany(h => h.Queues)
                .WithOne(q => q.Hostel)
                .HasForeignKey(q => q.HostelId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
