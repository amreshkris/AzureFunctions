using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using AzureFunctions.Models;
using Microsoft.Azure.WebJobs.Extensions.Sql;
using System.Text.Json;

namespace AzureFunctions.Trigger
{
    public static class ProductTrigger
    {
        //Output Binding
        [FunctionName("ProductTrigger")]

        public static void Run(
            [SqlTrigger("[dbo].[Products]", ConnectionStringSetting = "SqlConnectionString")]                    
            IReadOnlyList<SqlChange<Product>> changes,
            ILogger logger)
        {           
            foreach (SqlChange<Product> change in changes)
            {
                Product product = change.Item;
                logger.LogInformation($"Change operation: {change.Operation}");
                logger.LogInformation($"Product : {JsonSerializer.Serialize(product)}");
            }
        }
    }
}