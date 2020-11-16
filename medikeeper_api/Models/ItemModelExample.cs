using Swashbuckle.Examples;
using System.Collections.Generic;

namespace medikeeper_api.Models
{
    public class ItemModelPostExample : IExamplesProvider
    {
        public object GetExamples()
        {
            return new ItemModel()
            {
                ExternalId = "001",
                Name = "Item 1",
                Cost = 10.00m
            };
        }
    }

    public class ItemModelGetExample : IExamplesProvider
    {
        public object GetExamples()
        {
            return new ItemModel()
            {
                Id = 1,
                ExternalId = "001",
                Name = "Item 1",
                Cost = 10.00m
            };
        }
    }

    public class ItemModelGetListExample : IExamplesProvider
    {
        public object GetExamples()
        {
            return new List<ItemModel>()
            {
                new ItemModel
                {
                    Id = 1,
                    ExternalId = "001",
                    Name = "Item 1",
                    Cost = 10.00m
                },

                new ItemModel
                {
                    Id = 2,
                    ExternalId = "003",
                    Name = "Item 3",
                    Cost = 50.00m
                }
            };
        }
    }
}
