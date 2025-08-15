using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(NotesApplication.Startup))]

namespace NotesApplication
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}