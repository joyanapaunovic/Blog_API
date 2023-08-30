using Blog.DataAccess.Configurations;
using Blog.Domain;
using Microsoft.EntityFrameworkCore;
using System;

namespace Blog.DataAccess
{
    public class BlogContext : DbContext
    {
        public BlogContext(DbContextOptions<BlogContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(this.GetType().Assembly);
            // M:N tables
            modelBuilder.Entity<BlogPostCategory>().HasKey(x => new { x.BlogPostId, x.CategoryId });
            modelBuilder.Entity<BlogPostFile>().HasKey(x => new { x.BlogPostId, x.FileId });
            modelBuilder.Entity<BlogPostLike>().HasKey(x => new { x.BlogPostId, x.LikeId });
            modelBuilder.Entity<BlogPostTag>().HasKey(x => new { x.BlogPostId, x.TagId });
            modelBuilder.Entity<UserUseCase>().HasKey(x => new { x.UserId, x.UseCaseId });
            modelBuilder.Entity<CommentLike>().HasKey(x => new { x.CommentId, x.LikeId });
            // USE CASE LOGS
            modelBuilder.Entity<UseCaseLogs>().HasKey(x => x.Id);
            modelBuilder.Entity<UseCaseLogs>().Property(x => x.UseCaseName).IsRequired().HasMaxLength(400);
            modelBuilder.Entity<UseCaseLogs>().Property(x => x.UserId).IsRequired();
            modelBuilder.Entity<UseCaseLogs>().Property(x => x.ExecutionTime).IsRequired();
            modelBuilder.Entity<UseCaseLogs>().Property(x => x.IsAuthorized).IsRequired();

            modelBuilder.Entity<UseCaseLogs>().HasIndex(x => x.UseCaseName);
            modelBuilder.Entity<UseCaseLogs>().HasIndex(x => x.Id);

            base.OnModelCreating(modelBuilder);
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseSqlServer("Data Source=DESKTOP-CIO2VEQ;Initial Catalog=blog;Integrated Security=True");
        // => DB SETS
        public DbSet<BlogPost> BlogPosts { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<File> Files { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Like> Likes { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<CommentLike> CommentsLikes { get; set; }
        public DbSet<BlogPostCategory> BlogPostsCategories { get; set; }
        public DbSet<BlogPostTag> BlogPostsTags { get; set; }
        public DbSet<BlogPostFile> BlogPostsFiles { get; set; }
        public DbSet<BlogPostLike> BlogPostsLikes { get; set; }
        public DbSet<UserUseCase> UserUseCases { get; set; }
        
        public DbSet<UseCaseLogs> UseCaseLogs { get; set; }


    }
}
