using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(YogaFitnessClub.Startup))]
namespace YogaFitnessClub
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
