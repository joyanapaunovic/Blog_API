using Blog.Application.DTO;
using Blog.Application.Exceptions;
using Blog.Application.UseCases;
using Blog.Application.UseCases.Commands.Blog_posts;
using Blog.Application.UseCases.DTO;
using Blog.DataAccess;
using Blog.Domain;
using Blog.Implementation.Validators;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Implementation.UseCases.Commands.EF.Blog_posts
{
    public class EFUpdateBlogPostCommand : EFUseCase, IUpdateBlogPostCommand
    {
        private readonly UpdateTagValidator _validatorUpdateTag;
        private readonly UpdateBlogPostValidator _validator;
        public EFUpdateBlogPostCommand(BlogContext context, 
                                       UpdateTagValidator validatorUpdateTag,
                                       UpdateBlogPostValidator validator) : base(context)
        {
            _validatorUpdateTag = validatorUpdateTag;
            _validator = validator;
        }

        public int Id => 14;

        public string Name => "Update a blog post";

        public string Description => "Updating a blog post using Entity Framework.";

        public void Execute(UpdateBlogPostDTO request)
        {
            // VALIDATION => blog post title, blog post content
            _validator.ValidateAndThrow(request);
            var blogPostToUpdate = Context.BlogPosts.Find(request.Id);
            if (blogPostToUpdate.UserId != request.UserId)
            {
                throw new InformationExceptionForUser("Dear user, you don't have a blog post with that id, " +
                "so we didn't complete the update. " +
                "Check again your blog posts and try again.");
            }
            

            // => CATEGORIES
            foreach (var categoryIds in request.Categories)
            {
                foreach (var singleCategoryIdForwaded in categoryIds.NewCategoryIdsToAdd)
                {
                    // Console.WriteLine("svaki pojedinacno prosledjen id kategorije: ponaosob -> " + singleCategoryIdInFollowingCollecting);
                    //var id = categoryIds.NewCategoryIdsToAdd[i];
                    var exists = Context.Categories.Any(x => x.Id == singleCategoryIdForwaded);
                    if (!exists)
                    {
                        throw new InformationExceptionForUser("Dear user, " +
                        "some of the category ids you have entered don't exist. " +
                        "Check out the categories and try again to finish updating.");
                    }
                    else
                    {
                        // uklanjanje postojecih veza sa odr. blog postom
                        var categories = Context.BlogPostsCategories.Where(x => x.BlogPostId == request.Id);
                        Context.BlogPostsCategories.RemoveRange(categories);
                        Context.SaveChanges();
                        var categoryId = singleCategoryIdForwaded;
                        // blog posts categories
                        var newBlogPostCategoryRelationship = new BlogPostCategory
                        {
                            BlogPostId = blogPostToUpdate.Id,
                            CategoryId = categoryId
                        };
                        Context.BlogPostsCategories.Add(newBlogPostCategoryRelationship);
                        Context.SaveChanges();
                    }
                }
            }

            // => TAGS
            foreach (var tag in request.Tags)
                {
                /* UKLANJANJE POSTOJECIH VEZA TAGOVA */
                var previousTagsToRemove = Context.BlogPostsTags.Where(x => x.BlogPostId == request.Id);
                Context.BlogPostsTags.RemoveRange(previousTagsToRemove);
                Context.SaveChanges();
                if (tag.TagName != null)
                {
                    // provera da li taj tag name postoji u bazi podataka
                    Tag existingTag = Context.Tags.Where(x => x.TagName == tag.TagName)
                                                  .FirstOrDefault();
                    /**
                    -> brisemo sve veze sa postojecim tagovima da bi korisnik setovao 
                    nove vrednosti u formi kolekcije integer-a (tagovi koji se vec nalaze u bazi)
                    -ili dodavanjem potpuno novog taga (tabela Tags)
                    -ili kombinacijom ova dva - sto znaci da moze poslati oba svojstva
                    -ako ne posalje onda samo ne upada u odredjeni if 
                    svojstva za prosledjivanje su: "TagName" i "NewTagIdsToAdd"
                    **/
                    /* KREIRANJE NOVOG TAGA - KORISNIK PROSLEDJUJE TagName */
                    if (existingTag == null)
                    {
                        // validacija
                        _validatorUpdateTag.ValidateAndThrow(tag);
                        // dodajemo novi tag
                        var newTag = new Tag
                        {
                            TagName = tag.TagName
                        };

                        Context.Tags.Add(newTag);
                        Context.SaveChanges();
                        // blog post tags - strani kljuc
                        var newBlogPostTag = new BlogPostTag
                        {
                            BlogPostId = blogPostToUpdate.Id,
                            TagId = newTag.Id
                        };
                        Context.BlogPostsTags.Add(newBlogPostTag);

                    }
                    // u slucaju da postoji vec tag name, obavestenje korisniku:
                        else
                        {
                            throw new InformationExceptionForUser("Dear user, the tag name you have tried to add already exists," +
                            " so check out the existing tags first and then try again.");
                        }
                    }
                    /* PROSLEDJIVANJE KOLEKCIJE POSTOJECIH ID-EVA TAGOVA 
                       * => KORISNIK PROSLEDJUJE NewTagIdsToAdd */
                    if (tag.NewTagIdsToAdd != null)
                    {
                        // da li postoje svi id-evi koji su prosledjeni u bazi podataka
                        foreach (var singleTagId in tag.NewTagIdsToAdd)
                        {
                            var ifContainsTheFollowingIds = Context.Tags.Any(x => x.Id == singleTagId);
                            
                            if (!ifContainsTheFollowingIds)
                            {
                                throw new InformationExceptionForUser("Dear user, some of the tag ids you have " +
                                    " entered" + " are not found in our database. " +
                                    "Check the existing tag ids and try again. " +
                                    "If instead you want to add a completely new tag, " +
                                    "please send us the TagName property.");
                            }
                            else
                            {
                                //Console.WriteLine("svaki prosledjeni id TAGA: ponaosob -> " + singleTagId);
                                var oneOfTheIdsEntered = singleTagId;
                                // UPIS U BAZU (BLOGPOSTSTAGS)
                                // blog posts tags
                                var newBlogPostTagsRelationship = new BlogPostTag
                                {
                                    BlogPostId = blogPostToUpdate.Id,
                                    TagId = oneOfTheIdsEntered
                                };
                                Context.BlogPostsTags.Add(newBlogPostTagsRelationship);
                                Context.SaveChanges();
                            }
                        }
                    }
                }

            // => FILES
            // FILES TABELA
            // POSTOJECE BLOG POST FILES VEZE
            var existingFiles = Context.BlogPostsFiles.Where(x => x.BlogPostId == request.Id);
            Context.BlogPostsFiles.RemoveRange(existingFiles);
            Context.SaveChanges();

            // FAJLOVI
            var fileIdsToDelete = Context.BlogPostsFiles.Where(x => x.BlogPostId == request.Id)
                                                       .Select(x => x.FileId)
                                                       .ToList();

            foreach (var fileIdToDelete in fileIdsToDelete)
            {
                var fileToDelete = Context.Files.FirstOrDefault(x => x.Id == fileIdToDelete);
                if (fileToDelete != null)
                {
                    Context.Files.Remove(fileToDelete);
                }
            }

            Context.SaveChanges();

            // NOVI FAJL
            foreach (var file in request.Files)
            {
                var newFile = new File
                {
                    Path = file
                };
                Context.Files.Add(newFile);
                Context.SaveChanges();

                var newBlogPostFile = new BlogPostFile
                {
                    BlogPostId = blogPostToUpdate.Id,
                    FileId = newFile.Id
                };
                Context.BlogPostsFiles.Add(newBlogPostFile);
            }
            blogPostToUpdate.BlogPostContent = request.BlogPostContent;
            blogPostToUpdate.BlogPostTitle = request.BlogPostTitle;
            blogPostToUpdate.User = Context.Users.Find(request.UserId);
            blogPostToUpdate.UpdatedAt = DateTime.UtcNow;
            blogPostToUpdate.UpdatedBy = blogPostToUpdate.User.Email;

            Context.SaveChanges();

        }
    }
}

