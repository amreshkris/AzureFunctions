using System;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;

namespace AzureFunctions.Models
{
    public class Product
    {
        [OpenApiProperty(Description = "Id", Nullable = true)]
        public Guid Id { get; set; }

        public string Name { get; set; }

        public int Number { get; set; }

        public int CategoryId { get; set; }

        public decimal Price { get; set; }

        public decimal UpdatedPrice { get; set; }
    }
}