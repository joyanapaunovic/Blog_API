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
    public class UserConfiguration : EntityConfiguration<User>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<User> builder)
        {
            builder.Property(x => x.FirstName).IsRequired().HasMaxLength(30);
            builder.Property(x => x.LastName).IsRequired().HasMaxLength(30);
            builder.Property(x => x.Email).IsRequired().HasMaxLength(50);
            builder.Property(x => x.Password).IsRequired().HasMaxLength(500);

            builder.HasIndex(x => x.FirstName);
            builder.HasIndex(x => x.LastName);
            builder.HasIndex(x => x.Email).IsUnique();

            /* RELATIONSHIPS */
            // 1 user : N posts
            builder.HasMany(x => x.BlogPosts)
                .WithOne(x => x.User)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Restrict);
            // 1 user : N comments
            builder.HasMany(x => x.Comments)
                .WithOne(x => x.User)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Restrict);
            // 1 user : N likes
            builder.HasMany(x => x.Likes)
                    .WithOne(x => x.User)
                    .HasForeignKey(x => x.UserId)
                    .OnDelete(DeleteBehavior.Restrict); ;
            
            builder.HasMany(x => x.UseCases)
                    .WithOne(x => x.User)
                    .HasForeignKey(x => x.UserId);
        }
    }
}
