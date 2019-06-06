using Holod.Models.Database.QueueInfo;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Holod.Models.Database.Configs
{
    public class QueueConfig : IEntityTypeConfiguration<Queue>
    {
        public void Configure(EntityTypeBuilder<Queue> builder)
        {
            builder
                .HasMany(queue => queue.Cells)
                .WithOne(cell => cell.Queue)
                .HasForeignKey(cell => cell.QueueId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
