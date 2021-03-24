using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModels
{
    public class Item
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public int Stocks { get; set; }
        [Column(TypeName = "decimal(7,2)")]
        public decimal UnitPrice { get; set; }
    }
}
