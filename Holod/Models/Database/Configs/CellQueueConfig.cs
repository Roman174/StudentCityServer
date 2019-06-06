using Holod.Models.Database.QueueInfo;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Holod.Models.Database.Configs
{
    public class CellQueueConfig : IEntityTypeConfiguration<CellQueue>
    {
        public void Configure(EntityTypeBuilder<CellQueue> builder)
        {
            builder
                .HasOne(cell => cell.Resident)
                .WithOne(resident => resident.CellQueue)
                .HasForeignKey<CellQueue>(cell => cell.ResidentId);
        }
    }
}
