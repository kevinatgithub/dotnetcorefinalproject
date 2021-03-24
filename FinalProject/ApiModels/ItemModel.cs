using System.ComponentModel.DataAnnotations;

namespace FinalProject.ApiModels
{
    public class ItemModel
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
        [Required]
        public int Stocks { get; set; }
        [Required]
        public decimal UnitPrice { get; set; }
    }
}
