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
    public class CommentConfiguration : EntityConfiguration<Comment>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<Comment> builder)
        {
            builder.Property(x => x.CommentContent).IsRequired().HasMaxLength(300);

            // 1 parent comment - N children comments
            builder.HasMany(x => x.Children)
                    .WithOne(x => x.ParentComment)
                    .HasForeignKey(x => x.ParentCommentId)
                    .OnDelete(DeleteBehavior.Restrict);
            // LIKES
            builder.HasMany(x => x.Likes)
                   .WithOne(x => x.Comment)
                   .HasForeignKey(x => x.CommentId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
