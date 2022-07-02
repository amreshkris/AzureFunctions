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
    public static class PostTasks
    {
        //Output Binding - Create/Write new task in the database
        
        [FunctionName("PostTasks")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous,"post", Route = "PostTasks")] HttpRequest req,
            ILogger log,
            [Sql("dbo.Tasks",ConnectionStringSetting = "SqlConnectionString")] 
                IAsyncCollector<Task> toDoItems)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            Task todoTask = JsonConvert.DeserializeObject<Task>(requestBody);

            todoTask.Id = Guid.NewGuid();
            todoTask.Url =  Environment.GetEnvironmentVariable("ToDoUri")+"?id="+todoTask.Id.ToString();
            if (todoTask.Completed == null)
            {
                todoTask.Completed = false;
            }
            await toDoItems.AddAsync(todoTask);
            await toDoItems.FlushAsync();
            
            return new OkObjectResult(todoTask);
        }
    }
}
