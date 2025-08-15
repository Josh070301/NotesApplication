using System.Data.Entity;
using NotesApplication.Models;

namespace NotesApplication.Models
{
    public class ApplicationDbContext : DbContext
    {
        static ApplicationDbContext()
        {
            // Set the database initializer to create the database if it doesn't exist
            Database.SetInitializer(new DatabaseInitializer());
        }
        
        public ApplicationDbContext() : base("NotesDbConnection")
        {
            // Force initialization
            Database.Initialize(false);
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Note> Notes { get; set; }
        
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // Configure relationships and constraints here
            modelBuilder.Entity<Note>()
                .HasRequired(n => n.User)
                .WithMany()
                .HasForeignKey(n => n.UserId)
                .WillCascadeOnDelete(true);
                
            base.OnModelCreating(modelBuilder);
        }
    }
}