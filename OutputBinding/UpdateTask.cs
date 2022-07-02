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
    public static class UpdateTask
    {
        //Output Binding
        [FunctionName("UpdateTask")]
         public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "upsert-todotasks")]        
            HttpRequest req,
            [Sql("dbo.Tasks",ConnectionStringSetting = "SqlConnectionString")] 
            out Task toDoItem)
        {   
            Boolean.TryParse(req.Query["completed"], out bool status);  
            toDoItem = new Task
            {
                Title = req.Query["title"], 
                Id = Guid.Parse(req.Query["id"]),
                Url = $"?id={req.Query["id"]}",
                Completed = status
            };
            return new OkObjectResult(toDoItem);          
        }
    }
}