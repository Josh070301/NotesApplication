using System;
using System.Data.Entity;
using System.Security.Cryptography;
using System.Text;

namespace NotesApplication.Models
{
    public class DatabaseInitializer : CreateDatabaseIfNotExists<ApplicationDbContext>
    {
        protected override void Seed(ApplicationDbContext context)
        {
            // Add a default admin user
            var adminUser = new User
            {
                Username = "admin",
                Email = "admin@example.com",
                PasswordHash = HashPassword("Admin@123"),
                CreatedAt = DateTime.Now
            };
            
            context.Users.Add(adminUser);
            
            // Add some sample notes
            context.Notes.Add(new Note
            {
                Title = "Welcome to Notes App",
                Content = "This is your first note. You can edit or delete it, or create new notes!",
                CreatedAt = DateTime.Now,
                User = adminUser
            });
            
            context.Notes.Add(new Note
            {
                Title = "How to use the app",
                Content = "Create, edit and organize your notes easily with our simple interface.",
                CreatedAt = DateTime.Now,
                User = adminUser
            });
            
            context.SaveChanges();
            
            base.Seed(context);
        }
        
        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            }
        }
    }
}