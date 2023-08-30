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
    public class CategoryConfiguration : EntityConfiguration<Category>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<Category> builder)
        {
            builder.Property(x => x.Title).IsRequired().HasMaxLength(40);

            builder.HasIndex(x => x.Title).IsUnique();

            // RELATIONSHIPS
            builder.HasMany(x => x.BlogPosts)
                    .WithOne(x => x.Category)
                    .HasForeignKey(x => x.CategoryId)
                    .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
