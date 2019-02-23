using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Gestion_repos.Startup))]
namespace Gestion_repos
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
