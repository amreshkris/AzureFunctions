using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
namespace AzureFunctions.InputBinding
{
    public static class DeleteTask
    {
        [FunctionName("DeleteTask")]
        public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "DeleteTask")] HttpRequest req,
            [Sql("DeleteToDo", CommandType = System.Data.CommandType.StoredProcedure, 
                Parameters = "@Id={Query.id}", ConnectionStringSetting = "SqlConnectionString")] 
                IEnumerable<Task> tasks)
        {
            return new OkObjectResult(tasks);
        }
    }
}