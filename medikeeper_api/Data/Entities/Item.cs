namespace medikeeper_api.Data.Entities
{
    public class Item
    {
        public int Id { get; set; }
        public string ExternalId { get; set; }
        public string Name { get; set; }
        public decimal Cost { get; set; }
    }
}