using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.OpenApi.Models;
using System.Net;
using bdotnet;
using AzureFunctions.Models;

namespace AzureFunctions.InputBinding
{
    public static class DeleteProduct
    {
        [FunctionName("DeleteProduct")]
        [OpenApiOperation(operationId: "Run", tags: new[] { "Delete Product" })]
        [OpenApiParameter(name: "id", In = ParameterLocation.Query, Required = true, Type = typeof(Guid), Description = "The id parameter")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/json", bodyType: typeof(string), Description = "The OK response")]

        public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "DeleteProduct")] HttpRequest req,
            [Sql("DeleteProduct", CommandType = System.Data.CommandType.StoredProcedure, 
                Parameters = "@Id={Query.id}", ConnectionStringSetting = "SqlConnectionString")] 
                IEnumerable<Product> products)
        {
            return new OkObjectResult(products);
        }
    }
}