using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Holod.Models.Database.Configs
{
    public class PostConfig : IEntityTypeConfiguration<Post>
    {
        public void Configure(EntityTypeBuilder<Post> builder)
        {
            builder
                .HasMany(post => post.Stuffs)
                .WithOne(stuff => stuff.Post)
                .HasForeignKey(stuff => stuff.PostId);
        }
    }
}
