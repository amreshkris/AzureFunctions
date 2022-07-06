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
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.OpenApi.Models;
using System.Net;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using AzureFunctions.Models;

namespace AzureFunctions.InputBinding
{
    public static class GetProducts
    {
        //Input Binding - Read data from the database
        //Select the task that matches with the given id 
        [FunctionName("GetProduct")]

        [OpenApiOperation(operationId: "Run", tags: new[] { "GetProduct" })]
        [OpenApiParameter(name: "id", In = ParameterLocation.Query, Required = true, Type = typeof(Guid), Description = "The id parameter")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/json", bodyType: typeof(string), Description = "The OK response")]

        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "GetProduct")]
            HttpRequest req,
            ILogger log,
            [Sql(@"  select * from dbo.Products
                     where @Id = Id", CommandType = System.Data.CommandType.Text,
                    Parameters = "@Id={Query.id}",
                    ConnectionStringSetting = "SqlConnectionString")]
                    IAsyncEnumerable<Product> products)
        {
            return await System.Threading.Tasks.Task.FromResult
            (new OkObjectResult(products));
        }
    }
}
