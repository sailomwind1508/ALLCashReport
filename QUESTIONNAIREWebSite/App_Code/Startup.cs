using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(QUESTIONNAIREWebSite.Startup))]
namespace QUESTIONNAIREWebSite
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
