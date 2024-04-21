using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(project_masterdetails_crud.Startup))]
namespace project_masterdetails_crud
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
