using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Holod.Models.Database.Configs
{
    public class PostConfig : IEntityTypeConfiguration<Post>
    {
        public void Configure(EntityTypeBuilder<Post> builder)
        {
            builder
                .HasOne(post => post.Stuff)
                .WithOne(stuff => stuff.Post)
                .HasForeignKey<Stuff>(stuff => stuff.PostId);
        }
    }
}
