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
    public class FileConfiguration : EntityConfiguration<File>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<File> builder)
        {
            builder.Property(x => x.Path).IsRequired().HasMaxLength(100);

            // 1 file - N posts
            builder.HasMany(x => x.BlogPosts)
                    .WithOne(x => x.File)
                    .HasForeignKey(x => x.FileId)
                    .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
