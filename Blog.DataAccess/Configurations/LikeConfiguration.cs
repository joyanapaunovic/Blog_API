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
    public class LikeConfiguration : EntityConfiguration<Like>
    {

        protected override void ConfigureEntity(EntityTypeBuilder<Like> builder)
        {
            // slika lajka i fiksna je kad god se doda neki lajk; 
            builder.Property(x => x.FileId).HasDefaultValue(3); 
            
            // 1 like - N posts
            builder.HasMany(x => x.BlogPosts)
                   .WithOne(x => x.Like)
                   .HasForeignKey(x => x.LikeId)
                   .OnDelete(DeleteBehavior.Restrict);
            // 1 like - N comments
            builder.HasMany(x => x.Comments)
                    .WithOne(x => x.Like)
                    .HasForeignKey(x => x.LikeId)
                    .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
