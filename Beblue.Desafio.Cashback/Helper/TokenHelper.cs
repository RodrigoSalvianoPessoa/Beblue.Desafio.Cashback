using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Security.Claims;

namespace Beblue.Desafio.Cashback.WebApi.Helper
{
    public static class TokenHelper
    {
        private const string Issuer = "http://wwww.trustsolutions.com.br";
        private const string SecretKey = "XCAP05H6LoKvbRRa/QkqLNMI7cOHguaRyHzyg7n5qEkGjQmtBhz4SzYh4Fqwjyi3KJHlSXKPwVu2+bXr6CtpgQ==";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="username"></param>
        /// <param name="expireMinutes"></param>
        /// <returns></returns>
        public static string GenerateToken(string username, int expireMinutes = 20)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();

                var now = DateTime.UtcNow;

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new[]
                    {
                        new Claim(ClaimTypes.Name, username)
                    }),
                    IssuedAt = DateTime.Now,
                    Issuer = Issuer,
                    Expires = now.AddMinutes(Convert.ToInt32(expireMinutes)),
                    SigningCredentials = GetCredentials()
                };

                var securityToken = tokenHandler.CreateToken(tokenDescriptor);
                var token = tokenHandler.WriteToken(securityToken);

                return token;
            }
            catch (Exception exception)
            {
                throw new HttpRequestException("Cannot create token", exception);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        public static void AddBearerTokenAuthorization(this IServiceCollection services)
        {
            var key = new SymmetricSecurityKey(Convert.FromBase64String(SecretKey));

            services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(jwtBearerOptions =>
                {
                    jwtBearerOptions.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = Issuer,
                        ValidateActor = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = key
                    };
                });

            services.AddAuthorization();
        }

        private static SigningCredentials GetCredentials()
        {
            var symmetricKey = Convert.FromBase64String(SecretKey);
            return new SigningCredentials(new SymmetricSecurityKey(symmetricKey), SecurityAlgorithms.HmacSha256Signature);
        }
        /// <summary>
        /// Convert hours to Minutes
        /// </summary>
        /// <param name="hours"></param>
        /// <returns></returns>
        public static int ToMinutes(int hours) => hours * 60;
        /// <summary>
        /// Convert hours to Seconds
        /// </summary>
        /// <param name="hours"></param>
        /// <returns></returns>
        public static int ToSeconds(int hours) => ToMinutes(hours) * 60;

    }
}
