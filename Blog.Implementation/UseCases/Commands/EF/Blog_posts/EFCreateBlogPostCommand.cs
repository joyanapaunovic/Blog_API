using Blog.Application.UseCases.Commands.Blog_posts;
using Blog.Application.UseCases.DTO;
using Blog.DataAccess;
using Blog.Domain;
using Blog.Implementation.Validators;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Blog.Implementation.UseCases.Commands.EF.Blog_posts
{
    public class EFCreateBlogPostCommand : EFUseCase, ICreateBlogPostCommand
    {
        private readonly CreateBlogPostValidator _validator;
        public EFCreateBlogPostCommand(BlogContext context, CreateBlogPostValidator validator) : base(context)
        {
            _validator = validator;
        }

        public int Id => 5;

        public string Name => "Create blog post";

        public string Description => "Creating a blog post using Entity Framework.";

        public void Execute(CreateBlogPostDTO request)
        {
            _validator.ValidateAndThrow(request);
            // NOVI BLOG POST
            var newBlogPost = new BlogPost
            {
                BlogPostTitle = request.BlogPostTitle,
                BlogPostContent = request.BlogPostContent,
                UserId = request.UserId
            };
            // Cuvanje
            Context.BlogPosts.Add(newBlogPost);
            Context.SaveChanges();
            var blogPostId = newBlogPost.Id;
            // M : N veze za tabele koje korisnik blog post-a moze da dodeli
            // KATEGORIJE - kolekcija integer-a
            foreach (var category in request.Categories)
            {
                var idCategory = category;
                var blogPostCategory = new BlogPostCategory
                {
                    BlogPostId = blogPostId,
                    CategoryId = idCategory
                };
                Context.BlogPostsCategories.Add(blogPostCategory);
            }
            // TAGOVI - kolekcija integer-a
            foreach (var tag in request.Tags)
            {
                    var tagId = tag;
                    var blogPostTag = new BlogPostTag
                    {
                        BlogPostId = blogPostId,
                        TagId = tagId
                    };
               
                Context.BlogPostsTags.Add(blogPostTag);

            }
            // FAJLOVI - kolekcija stirngova
            foreach(var file in request.Files)
            {
                //FILE TABELA - DODAVANJE IMENA FAJLA
                var filePath = file;
                var FilePath_tableFile = new File
                {
                    Path = filePath
                };
                //cuvanje da bi se dohvatio id fajla upravo dodatog
                Context.Files.Add(FilePath_tableFile);
                Context.SaveChanges();
                var fileId = FilePath_tableFile.Id;
                //DODAVANJE VEZE IZMEDJU BLOG POSTA I FAJLA - VEZIVNA TABELA
                var blogPostsFile = new BlogPostFile
                {
                    BlogPostId = blogPostId,
                    FileId = fileId
                };
                Context.BlogPostsFiles.Add(blogPostsFile);
            }

            Context.SaveChanges();

            }
        }
    }
