]using Agenda.Host.Models;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Agenda.Host.Services
{
    public class AuthService
    {
        private static Dictionary<Guid, string> TicketsRole = new Dictionary<Guid, string>
        {
            [Guid.Parse("free")] = "free",
            [Guid.Parse("paid")] = "paid",
        };

        public static string GenerateToken(User user, List<Ticket> tickets, string secret)
        {
            var role = TicketsRole[tickets.OrderByDescending(t => t.Price).FirstOrDefault().TicketKey];

            if (user.Email == "admin@admin.com")
            {
                role = "admin";
            }

            var identity = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.FullName),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Hash, user.Hash),
                    new Claim(ClaimTypes.Role, role)
                });

            if (tickets.Any(t => t.Description.ToUpper().Contains("PREMIUM")))
            {
                identity.AddClaim(new Claim("stream_user", user.Username));
                identity.AddClaim(new Claim("stream_pass", user.Password));
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = identity,
                Expires = DateTime.UtcNow.AddHours(24),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
