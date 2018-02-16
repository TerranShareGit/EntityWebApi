using System.Data.Entity;

namespace EntityWebApi.Models
{
    public class AnyEntityContext : DbContext
    {
        public DbSet<AnyEntity> AnyEntities { get; set; }
    }
}