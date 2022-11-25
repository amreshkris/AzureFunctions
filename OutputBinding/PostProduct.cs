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

namespace AzureFunctions.OutputBinding
{
    public static class PostTasks
    {
        //Output Binding - Create/Write new task in the database

        [FunctionName("PostProduct")]

        [OpenApiOperation(operationId: "Run", tags: new[] { "PostProduct" })]
        [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(List<Product>), Description = "Product to be created", Required = true)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/json", bodyType: typeof(string), Description = "The OK response")]

        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "PostProduct")] HttpRequest req,
            ILogger log,
            [Sql("dbo.Products",ConnectionStringSetting = "SqlConnectionString")]
                IAsyncCollector<Product> products)
        {
            log.LogInformation("Adding products");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var inputProducts = JsonConvert.DeserializeObject<List<Product>>(requestBody);

            foreach (var product in inputProducts)
            {
                //Add custom logic               
                await products.AddAsync(product);
                await products.FlushAsync();
            }


            return new OkObjectResult(products);
        }
    }
}
