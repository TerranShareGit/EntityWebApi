using AnyEntityClient;
using System;
using System.Threading.Tasks;

namespace EntityHttpClient
{
    class Program
    {
        static void Main()
        {
            RunAsync().GetAwaiter().GetResult();
        }

        static async Task RunAsync()
        {
            WebApiHelper webApiHelper = new WebApiHelper("admin@mail.com", "Admin-1");
            WebApiHelper webApiHelperForUser = new WebApiHelper("user@mail.com", "User-1");

            try
            {
                //Create AnyEntity
                AnyEntity entity = new AnyEntity
                {
                    Id = "100",
                    Description = "HttpClient Entity"
                };

                AnyEntity entityUser = new AnyEntity
                {
                    Id = "100",
                    Description = "HttpClient Entity User"
                };

                var url = await webApiHelper.CreateAnyEntityAsync(entity);
                Console.WriteLine($"Создание от админа {url}");
                entity = await webApiHelper.GetAnyEntityAsync(url.PathAndQuery);
                webApiHelper.ShowAnyEntity(entity);
                Console.WriteLine();

                try
                {
                    var urlUser = await webApiHelperForUser.CreateAnyEntityAsync(entityUser);
                    Console.WriteLine($"Создание от пользователя {urlUser}");
                    entityUser = await webApiHelperForUser.GetAnyEntityAsync(url.PathAndQuery);
                    webApiHelperForUser.ShowAnyEntity(entityUser);
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Создание от пользователя: {e.Message}");
                }
                Console.WriteLine();


                //Update AnyEntity
                Console.WriteLine("Обновление от админа");
                entity.Description = "Updating HttpClient Entity";
                await webApiHelper.UpdateAnyEntityAsync(entity);
                entity = await webApiHelper.GetAnyEntityAsync(url.PathAndQuery);
                webApiHelper.ShowAnyEntity(entity);
                Console.WriteLine();

                Console.WriteLine("Обновление от пользователя");
                entityUser.Description = "Updating HttpClient Entity";
                try
                {
                    await webApiHelperForUser.UpdateAnyEntityAsync(entityUser);
                    entityUser = await webApiHelperForUser.GetAnyEntityAsync(url.PathAndQuery);
                    webApiHelperForUser.ShowAnyEntity(entityUser);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                Console.WriteLine();


                //Delete AnyEntity
                var statusCode = await webApiHelper.DeleteAnyEntityAsync(entity.Id);
                Console.WriteLine($"Удаление от админа (HTTP Status = {(int)statusCode})");

                try
                {
                    var statusCodeUser = await webApiHelperForUser.DeleteAnyEntityAsync(entityUser.Id);
                    Console.WriteLine($"Удаление от пользователя (HTTP Status = {(int)statusCodeUser})");
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Удаление от пользователя: {e.Message}"); ;
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            Console.ReadLine();
        }
    }
}