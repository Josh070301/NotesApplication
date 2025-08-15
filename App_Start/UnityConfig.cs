using NotesApplication.Interfaces;
using NotesApplication.Models;
using NotesApplication.Repositories;
using System.Web.Mvc;
using Unity;
using Unity.Mvc5;

namespace NotesApplication
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
            var container = new UnityContainer();
            
            // Register your types here
            container.RegisterType<ApplicationDbContext>();
            container.RegisterType<IUserRepository, UserRepository>();
            container.RegisterType<INoteRepository, NoteRepository>();
            
            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}