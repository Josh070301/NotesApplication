using Microsoft.IdentityModel.Tokens;
using Microsoft.Owin;
using Microsoft.Owin.Security.Jwt;
using Owin;
using System;
using System.Configuration;
using System.Text;

namespace NotesApplication
{
    public partial class Startup
    {
        private static string Secret = ConfigurationManager.AppSettings["JwtSecret"] ?? "TestJWTSecretKey";
        
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