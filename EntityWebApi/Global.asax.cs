using EntityWebApi.Models;
using System.Data.Entity;
using System.Web.Http;

namespace EntityWebApi
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            Database.SetInitializer(new AnyEntityDbInitializer());

            GlobalConfiguration.Configure(WebApiConfig.Register);
        }
    }
}
