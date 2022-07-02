using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace bdotnet
{
    public static class GetTasks
    {
        [FunctionName("GetTasks")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] 
            HttpRequest req,
            ILogger log,
            [Sql(@"  select Id, [order], title, url, completed from dbo.ToDo
                     where @Id = Id", CommandType = System.Data.CommandType.Text,
                    Parameters = "@Id={Query.id}", 
                    ConnectionStringSetting = "SqlConnectionString")] IAsyncEnumerable<ToDoTask> toDos)
        {
            return await Task.FromResult(new OkObjectResult(toDos));
        }
    }
}
