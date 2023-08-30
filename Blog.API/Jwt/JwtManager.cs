using Blog.DataAccess;
using Blog.API.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Newtonsoft.Json;

namespace Blog.Api.Core
{
    public class JwtManager
    {
        private readonly BlogContext _context;
        private readonly JwtSettings _settings;

        public JwtManager(BlogContext context, JwtSettings settings)
        {
            _settings = settings;
            _context = context;
        }

        public string MakeToken(string email, string password)
        {
            // provera kredencijala
            var user = _context.Users.Include(x => x.UseCases).FirstOrDefault(x => x.Email == email);

            if (user == null)
            {
                throw new UnauthorizedAccessException();
            }

            var valid = BCrypt.Net.BCrypt.Verify(password, user.Password);

            if (!valid)
            {
                throw new UnauthorizedAccessException();
            }

            //var useCases = _context.UserUseCases.Where(x => x.UserId == user.Id).Select(x => x.UseCaseId);

            var author = new JwtUser
            {
                Id = user.Id,
                UseCaseAllowedIds = user.UseCases.Select(x => x.UseCaseId).ToList(),
                Identity = user.Email,
                Email = user.Email
            };
            // srednji deo tokena
            var claims = new List<Claim> // Jti : "",
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iss, _settings.Issuer),
                new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64, _settings.Issuer),
                new Claim("UserId", author.Id.ToString(), ClaimValueTypes.String, _settings.Issuer),
                new Claim("UseCases", JsonConvert.SerializeObject(author.UseCaseAllowedIds)),
                new Claim("Email", user.Email),
            };
            // zakljucavanje i cuvanje tokena
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.SecretKey));

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var now = DateTime.UtcNow;
            var token = new JwtSecurityToken(
                issuer: _settings.Issuer,
                audience: "Any",
                claims: claims,
                notBefore: now,
                expires: now.AddMinutes(_settings.Minutes), // u odnosu na sadasnji trenutak, koliko ce trajati token -> za sekunde - now.AddSeconds(100) npr.
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
