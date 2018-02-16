using NHibernate;
using NHibernate.Cfg;
using System.Web;

namespace EntityWebApi.Models
{
    public class NHibernateSession
    {
        public static ISession OpenSession()
        {
            var configuration = new Configuration();
            var configurationPath = HttpContext.Current.Server.MapPath(@"~\Models\hibernate.cfg.xml");
            configuration.Configure(configurationPath);
            var bookConfigurationFile = HttpContext.Current.Server.MapPath(@"~\Models\AnyEntity.hbm.xml");
            configuration.AddFile(bookConfigurationFile);
            ISessionFactory sessionFactory = configuration.BuildSessionFactory();
            return sessionFactory.OpenSession();
        }
    }
}