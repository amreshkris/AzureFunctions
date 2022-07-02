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
        //Input Binding - Read data from the database
        //Select the task that matches with the given id 
        [FunctionName("GetTasks")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "GetTasks")] 
            HttpRequest req,
            ILogger log,
            [Sql(@"  select Id, [order], title, url, completed from dbo.Tasks
                     where @Id = Id", CommandType = System.Data.CommandType.Text,
                    Parameters = "@Id={Query.id}",
                    ConnectionStringSetting = "SqlConnectionString")] 
                    IAsyncEnumerable<Task> tasks)
        {
            return await System.Threading.Tasks.Task.FromResult
            (new OkObjectResult(tasks));
        }
    }
}
