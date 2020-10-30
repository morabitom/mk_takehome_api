using System.ComponentModel.DataAnnotations;

namespace medikeeper_api.Models
{
    public class ItemModel
    {
        public int? Id { get; set; }
        [Required]
        [StringLength(10)]
        public string ExternalId { get; set; }
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        [Required]
        [Range(typeof(decimal),"0", "1000000")]
        public decimal Cost { get; set; }
    }
}