
using Blog.Api.Core;
using Blog.API.Core;
using Blog.Application.UseCases.Commands.Blog_posts;
using Blog.Application.UseCases.Commands.Categories;
using Blog.Application.UseCases.Commands.Comments;
using Blog.Application.UseCases.Commands.Likes;
using Blog.Application.UseCases.Commands.Tags;
using Blog.Application.UseCases.Commands.UseCase;
using Blog.Application.UseCases.Commands.Users;
using Blog.Application.UseCases.Queries;
using Blog.DataAccess;
using Blog.Domain;
using Blog.Implementation.UseCases.Commands.EF.Blog_posts;
using Blog.Implementation.UseCases.Commands.EF.Categories;
using Blog.Implementation.UseCases.Commands.EF.Comments;
using Blog.Implementation.UseCases.Commands.EF.Likes;
using Blog.Implementation.UseCases.Commands.EF.Tags;
using Blog.Implementation.UseCases.Commands.EF.UseCase;
using Blog.Implementation.UseCases.Commands.EF.Users;
using Blog.Implementation.UseCases.Queries.EF;
using Blog.Implementation.Validators;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.API.Extensions
{
    public static class ContainerExtension
    {
        /* => JWT */
        public static void AddJwt(this IServiceCollection services, AppSettings settings)
        {
            services.AddTransient(x =>
            {
                var context = x.GetService<BlogContext>();
                var settings = x.GetService<AppSettings>();

                return new JwtManager(context, settings.JwtSettings);

            });
            #region AUTHENTICATION
            services.AddAuthentication(options =>
            {
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(cfg =>
            {
                cfg.RequireHttpsMetadata = false;
                cfg.SaveToken = true;
                cfg.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = settings.JwtSettings.Issuer,
                    ValidateIssuer = true,
                    ValidAudience = "Any",
                    ValidateAudience = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(settings.JwtSettings.SecretKey)),
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };
            });
            #endregion
        }

        /* => USE CASES */
        public static void AddUseCases(this IServiceCollection services)
        {

            #region COMMANDS
                /* ====================================================================*/
                // ADD CATEGORIES
                services.AddTransient<ICreateCategoryCommand, EFCreateCategoryCommand>();
                // DELETE CATEGORIES
                services.AddTransient<IDeleteCategoryCommand, EFDeleteCategoryCommand>();
                // UPDATE CATEGORY
                services.AddTransient<IUpdateCategoryCommand, EFUpdateCategoryCommand>();
                /* ====================================================================*/
                // REGISTER USERS - INSERT
                services.AddTransient<IRegisterUserCommand, EFRegisterUserCommand>();
                // UPDATE USER DATA 
                services.AddTransient<IUpdateUserDataCommand, EFUpdateUserDataCommand>();
                /* ====================================================================*/
                // CREATE BLOG POST
                services.AddTransient<ICreateBlogPostCommand, EFCreateBlogPostCommand>();
                // UPDATE BLOG POST
                services.AddTransient<IUpdateBlogPostCommand, EFUpdateBlogPostCommand>();
                // DELETE BLOG POST
                services.AddTransient<IDeleteBlogPostCommand, EFDeleteBlogPostCommand>();
                /* ====================================================================*/
                // CREATE TAG
                services.AddTransient<ICreateTagCommand, EFCreateTagCommand>();
                // DELETE TAG
                services.AddTransient<IDeleteTagCommand, EFDeleteTagCommand>();
                // UPDATE TAG
                services.AddTransient<IUpdateTagCommand, EFUpdateTagCommand>();
                /* ====================================================================*/
                // CREATE COMMENT
                services.AddTransient<ICreateCommentCommand, EFCreateCommentCommand>();
                // UPDATE COMMENT
                services.AddTransient<IUpdateCommentCommand, EFUpdateCommentCommand>();
                // DELETE COMMENT 
                services.AddTransient<IDeleteCommentCommand, EFDeleteCommentCommand>();
                /* ====================================================================*/
                // CREATE LIKES ON COMMENTS OR BLOG POSTS
                services.AddTransient<ICreateLikesForCommentsOrBlogPostsCommand, EFCreateLikesForCommentsOrBlogPostsCommand>();
                // DELETE LIKES ON COMMENTS
                services.AddTransient<IDeleteLikeForCommentsCommand, EFDeleteLikeForCommentCommand>();
                // UPDATE USER USE CASES
                services.AddTransient<IUpdateUserUseCasesCommand, EFUpdateUserUseCasesCommand>();
                // delete user 
                //services.AddTransient<IDeleteUserCommand, EFDeleteUserCommand>();
            #endregion


            #region QUERIES
            // SEARCH CATEGORIES 
            services.AddTransient<IGetCategoriesQuery, EFSearchCategoriesQuery>();
                // SEARCH BLOG POSTS (BLOG POST TITLES - THEMES)
                services.AddTransient<IGetBlogPostsQuery, EFSearchBlogPostsQuery>();
                // SHOW ONE BLOG POST 
                services.AddTransient<IGetOneBlogPostQuery, EFShowOneBlogPostQuery>();
                // SEARCH TAGS
                services.AddTransient<IGetTagsQuery, EFSearchTagsQuery>();
                // SEARCH USERS
                services.AddTransient<IGetUsersQuery, EFSearchUsersQuery>();
                // GET COMMENTS - pretraga sa query string-om gde se prosledjuje blogPostId
                services.AddTransient<IGetCommentsQuery, EFSearchCommentsQuery>();
            #endregion


            #region VALIDATORS
                // => CREATE CATEGORY VALIDATOR
                services.AddTransient<CreateCategoryValidator>();
                // => REGISTER USER VALIDATOR
                services.AddTransient<RegisterUsersValidator>();
                // => UPDATE BLOG POST VALIDATOR
                services.AddTransient<UpdateBlogPostValidator>();
                // => CREATE BLOG POST VALIDATOR
                services.AddTransient<CreateBlogPostValidator>();
                // => CREATE TAG VALIDATOR 
                services.AddTransient<CreateTagValidator>();
                // => UPDATE TAG VALIDATOR
                services.AddTransient<UpdateTagValidator>();
                // => CREATE COMMENT VALIDATOR
                services.AddTransient<CreateCommentValidator>();
                // => UPDATE USE CASE VALIDATOR
                services.AddTransient<UpdateUserUseCasesValidator>();
            #endregion

        }
        /* APPLICATION USER */
        public static void AddApplicationUser(this IServiceCollection services)
        {
            // IApplicationUser
            services.AddTransient<IApplicationUser>(x =>
            {
                //  PRISTUP HTTP-u -> dohvatanje tokena (payload-a)
                var accessor = x.GetService<IHttpContextAccessor>();
                var header = accessor.HttpContext.Request.Headers["Authorization"];


                // user je skup claim-ova
                // P A Y L O A D
                var claims = accessor.HttpContext.User;

                if (claims == null || claims.FindFirst("UserId") == null)
                {
                    return new AnonymousUser();
                }

                var actor = new JwtUser
                {
                    Email = claims.FindFirst("Email").Value,
                    Id = Int32.Parse(claims.FindFirst("UserId").Value),
                    Identity = claims.FindFirst("Email").Value,
                    UseCaseAllowedIds = JsonConvert.DeserializeObject<List<int>>(claims.FindFirst("UseCases").Value)
                };

                return actor;

            });
        }
    }
}
