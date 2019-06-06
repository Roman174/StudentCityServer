using Holod.Models.Database.Pass;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Holod.Models.Database.Configs
{
    public class PassConfig : IEntityTypeConfiguration<PassInfo>
    {
        public void Configure(EntityTypeBuilder<PassInfo> builder)
        {
            builder
                .HasOne(pass => pass.Resident)
                .WithOne(resident => resident.PassInfo)
                .HasForeignKey<PassInfo>(pass => pass.IdResident);
        }
    }
}