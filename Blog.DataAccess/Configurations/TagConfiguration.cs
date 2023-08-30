using Blog.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.DataAccess.Configurations
{
    public class TagConfiguration : EntityConfiguration<Tag>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<Tag> builder)
        {
            builder.Property(x => x.TagName).IsRequired().HasMaxLength(30);

            builder.HasIndex(x => x.TagName).IsUnique();

            // 1 tag - N posts
            builder.HasMany(x => x.BlogPosts)
                    .WithOne(x => x.Tag)
                    .HasForeignKey(x => x.TagId)
                    .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
