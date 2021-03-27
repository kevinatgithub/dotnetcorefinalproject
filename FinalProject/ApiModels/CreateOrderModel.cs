using FinalProject.ApiModels.interfaces;
using FinalProject.attributes;

namespace FinalProject.ApiModels
{
    public class CreateOrderModel : IHasItemId, IHasQuantity
    {
        public int ItemId { get; set; }
        [GreaterThanZero]
        public int Quantity { get; set; }
    }
}
