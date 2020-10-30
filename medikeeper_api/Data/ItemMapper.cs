using medikeeper_api.Data.Entities;
using medikeeper_api.Models;

namespace medikeeper_api.Data
{
    public static class ItemMapper
    {
        public static Item ModelToEntity(ItemModel model)
        {
            return new Item()
            {
                Id = model.Id ?? 0,
                ExternalId = model.ExternalId,
                Name = model.Name,
                Cost = model.Cost
            };
        }

        public static ItemModel EntityToModel(Item entity)
        {
            return new ItemModel()
            {
                Id = entity.Id,
                ExternalId = entity.ExternalId,
                Name = entity.Name,
                Cost = entity.Cost
            };
        }
    }
}