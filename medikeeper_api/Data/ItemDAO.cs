using Dapper;
using medikeeper_api.Data.Entities;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace medikeeper_api.Data
{
    public class ItemDAO
    {
        private string ConnectionString { get; }
        public ItemDAO()
        {
            ConnectionString = ConfigurationManager.ConnectionStrings["itemDB"].ConnectionString;
        }

        public async Task<IEnumerable<Item>> GetItems()
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                var items = await connection.QueryAsync<Item>("SELECT * FROM Item");
                return items;
            }
        }

        public async Task<IEnumerable<Item>> GetItemsByName(string name)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                var items = await connection.QueryAsync<Item>(
                                $"SELECT * FROM Item WHERE Name = {name}");
                return items;
            }
        }

        public async Task<Item> GetItemById(int id)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                var item = await connection.QuerySingleOrDefaultAsync<Item>(
                                $"SELECT * FROM Item WHERE Id = {id}");
                return item;
            }
        }

        public async Task<IEnumerable<Item>> GetItemsByExternalId(string externalId)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                var items = await connection.QueryAsync<Item>(
                                $"SELECT * FROM Item WHERE ExternalId = {externalId}");
                return items;
            }
        }

        public async Task<int> CreateItem(Item item)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                var sql =  @"INSERT INTO Item (ExternalId, Name, Cost)
                             OUTPUT INSERTED.Id
                             Values (@ExternalId, @Name, @Cost)";
                var newId = await connection.QuerySingleAsync<int>(sql, item);
                return newId;
            }
        }

        public async Task<bool> UpdateItem(int id, Item item)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                var sql = "UPDATE Item SET Name = @Name, Cost = @Cost, ExternalId = @ExternalId WHERE Id = @Id";
                var updated = await connection.ExecuteAsync(sql, new { item.Name, item.Cost, item.ExternalId, id });
                return (updated != 0);
            }
        }

        public async Task<bool> DeleteItem(int id)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                var sql = "DELETE FROM Item WHERE Id = @id";
                var updated = await connection.ExecuteAsync(sql, new { id });
                return (updated != 0);
            }
        }

        public async Task<IEnumerable<Item>> GetItemsWithMaxPrices()
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                var items = await connection.QueryAsync<Item>("SELECT * FROM maxpricesbyitem_view");
                return items;
            }
        }

        public async Task<Item> GetItemWithMaxPriceByName(string name)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                var item = await connection.QuerySingleOrDefaultAsync<Item>(
                                $"SELECT * FROM maxpricesbyitem_view WHERE Name = {name}");
                return item;
            }
        }
    }
}