using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using EntityWebApi.Filters;
using EntityWebApi.Models;
using NHibernate;

namespace EntityWebApi.Controllers
{
    [ApiAuthorize]
    public class AnyEntitiesController : BaseApiController
    {
        public IList<AnyEntity> GetAnyEntities()
        {
            IList<AnyEntity> entities;
            using (ISession session = NHibernateSession.OpenSession())
            {
                entities = session.Query<AnyEntity>().ToList();
            }
            return entities;
        }


        [ResponseType(typeof(AnyEntity))]
        public IHttpActionResult GetAnyEntity(int id)
        {
            AnyEntity anyEntity;
            using (ISession session = NHibernateSession.OpenSession())
            {
                anyEntity = session.Query<AnyEntity>().FirstOrDefault(e => e.Id == id);
            }

            if (anyEntity == null)
            {
                return NotFound();
            }

            return Ok(anyEntity);
        }

        [ResponseType(typeof(void))]
        public IHttpActionResult PutAnyEntity(int id, AnyEntity anyEntity)
        {
            if (IsInRole("Admin"))
            {
                if (id != anyEntity.Id)
                {
                    return BadRequest();
                }

                try
                {
                    using (ISession session = NHibernateSession.OpenSession())
                    {
                        using (ITransaction transaction = session.BeginTransaction())
                        {
                            session.SaveOrUpdate(anyEntity);
                            transaction.Commit();
                        }
                    }
                }
                catch (Exception e)
                {
                    throw;
                }

                return StatusCode(HttpStatusCode.NoContent);
            }

            return Content(HttpStatusCode.Unauthorized, "Редактирование позволено только роли Admin");
        }

        [ResponseType(typeof(AnyEntity))]
        public IHttpActionResult PostAnyEntity(AnyEntity anyEntity)
        {
            if (IsInRole("Admin"))
            {
                try
                {
                    using (ISession session = NHibernateSession.OpenSession())
                    {
                        using (ITransaction transaction = session.BeginTransaction())
                        {
                            session.Save(anyEntity);
                            transaction.Commit();
                        }
                    }
                }
                catch (Exception e)
                {
                    throw;
                }

                return CreatedAtRoute("DefaultApi", new { id = anyEntity.Id }, anyEntity);
            }

            return Content(HttpStatusCode.Unauthorized, "Вставка позволена только роли Admin");
        }

        [ResponseType(typeof(AnyEntity))]
        public IHttpActionResult DeleteAnyEntity(int id)
        {
            if (IsInRole("Admin"))
            {
                AnyEntity anyEntity = null;
                try
                {
                    using (ISession session = NHibernateSession.OpenSession())
                    {
                        anyEntity = session.Get<AnyEntity>(id);
                        if (anyEntity == null)
                        {
                            return NotFound();
                        }

                        using (ITransaction transaction = session.BeginTransaction())
                        {
                            session.Delete(anyEntity);
                            transaction.Commit();
                        }
                    }
                }
                catch (Exception e)
                {
                    throw;
                }

                return Ok(anyEntity);
            }

            return Content(HttpStatusCode.Unauthorized, "Удаление позволено только роли Admin");
        }
    }
}