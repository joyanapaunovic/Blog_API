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
    public class BlogPostConfiguration : EntityConfiguration<BlogPost>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<BlogPost> builder)
        {
          
                builder.Property(x => x.BlogPostTitle).IsRequired().HasMaxLength(30);
                builder.Property(x => x.BlogPostContent).IsRequired();

                builder.HasIndex(x => x.BlogPostTitle);

                // 1 post => N categories
                builder.HasMany(x => x.Categories)
                        .WithOne(x => x.BlogPost)
                        .HasForeignKey(x => x.BlogPostId)
                        .OnDelete(DeleteBehavior.Restrict);
                // 1 post => N tags
                builder.HasMany(x => x.Tags)
                        .WithOne(x => x.BlogPost)
                        .HasForeignKey(x => x.BlogPostId)
                        .OnDelete(DeleteBehavior.Restrict);
                // 1 post => N files
                builder.HasMany(x => x.Files)
                       .WithOne(x => x.BlogPost)
                       .HasForeignKey(x => x.BlogPostId)
                       .OnDelete(DeleteBehavior.Restrict);
                // 1 post => N likes
                builder.HasMany(x => x.Likes)
                       .WithOne(x => x.BlogPost)
                       .HasForeignKey(x => x.BlogPostId)
                       .OnDelete(DeleteBehavior.Restrict);
                // 1 post => N comments
                builder.HasMany(x => x.Comments)
                       .WithOne(x => x.BlogPost)
                       .HasForeignKey(x => x.BlogPostId)
                       .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
