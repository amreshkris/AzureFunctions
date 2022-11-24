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
using AzureFunctions.Models;

namespace bdotnet
{
    public static class UpdateTask
    {
        //Output Binding
        [FunctionName("UpdateProduct")]

        [OpenApiOperation(operationId: "Run", tags: new[] { "UpdateProduct" })]
        [OpenApiParameter(name: "id", In = ParameterLocation.Query, Required = true, Type = typeof(Guid), Description = "The id parameter")]
        [OpenApiParameter(name: "name", In = ParameterLocation.Query, Required = true, Type = typeof(string), Description = "The Name to update")]
        [OpenApiParameter(name: "updatedprice", In = ParameterLocation.Query, Required = false, Type = typeof(decimal), Description = "Updated Price")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/json", bodyType: typeof(string), Description = "The OK response")]

         public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "UpdateProduct")]        
            HttpRequest req,
            [Sql("dbo.Products",ConnectionStringSetting = "SqlConnectionString")] 
            out Product product)
        {
            bool isPriceConversionSuccess = Decimal.TryParse(req.Query["updatedprice"], out decimal updatedPrice);
            product = new Product
            {                
                Id = Guid.Parse(req.Query["id"]),
                Name = req.Query["name"],
                UpdatedPrice = isPriceConversionSuccess ?  updatedPrice : 0.0m          
            };
            
            return new OkObjectResult(product);          
        }
    }
}