using Microsoft.IdentityModel.Tokens;
using Microsoft.Owin;
using Microsoft.Owin.Security.Jwt;
using Owin;
using System;
using System.Configuration;
using System.Text;
using NotesApplication.Helpers;

namespace NotesApplication
{
    public partial class Startup
    {
        // Get JWT secret from environment variables or fallback to web.config
        private static string Secret = EnvHelper.GetEnvironmentVariable("JWT_SECRET") ?? "";
        
        public void ConfigureAuth(IAppBuilder app)
        {
            var issuer = "NotesApplication";
            var audience = "NotesApplicationUsers";
            var key = Encoding.ASCII.GetBytes(Secret);
            
            // Configure JWT authentication
            app.UseJwtBearerAuthentication(
                new JwtBearerAuthenticationOptions
                {
                    AuthenticationMode = Microsoft.Owin.Security.AuthenticationMode.Active,
                    AllowedAudiences = new[] { audience },
                    IssuerSecurityKeyProviders = new[] 
                    {
                        new SymmetricKeyIssuerSecurityKeyProvider(issuer, key)
                    }
                });
        }
    }
}