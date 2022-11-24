using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

namespace Company.Function
{
    public static class GetProducts
    {      
        [FunctionName("GetProducts")]
        public static IActionResult Run(
                [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "GetAllProducts")] HttpRequest req,
                [Sql("SELECT * FROM [dbo].[Products]",
            CommandType = System.Data.CommandType.Text,
            ConnectionStringSetting = "SqlConnectionString")] IEnumerable<Object> result,
                ILogger log)
        {           
            return new OkObjectResult(result);
        }
    }
}
