using System.Data.Entity;

namespace EntityWebApi.Models
{
    public class AnyEntityDbInitializer : DropCreateDatabaseAlways<AnyEntityContext>
    {
        protected override void Seed(AnyEntityContext db)
        {
            db.AnyEntities.Add(new AnyEntity { Id = 1, Description = "Сущность 1" });
            db.AnyEntities.Add(new AnyEntity { Id = 2, Description = "Сущность 2" });
            db.AnyEntities.Add(new AnyEntity { Id = 3, Description = "Сущность 3" });

            base.Seed(db);
        }
    }
}