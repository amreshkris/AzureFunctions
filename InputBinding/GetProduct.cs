using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.OpenApi.Models;
using System.Net;
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

        public static  IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "GetProduct/{id:guid?}")] 
             HttpRequest req,           
            ILogger log,
            [Sql(@"                    
                    IF (@id IS NULL or @id='')
                        select Top(100) * from Products
                    ELSE
                        select * from Products where Id = @Id
                 ",                     
                CommandType = System.Data.CommandType.Text,
                Parameters = "@id={id}",
                ConnectionStringSetting = "SqlConnectionString")]
                IEnumerable<Product> products)
        {
           return new OkObjectResult(products);
        }
    }
}
