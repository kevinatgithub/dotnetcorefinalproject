using FinalProject.ApiModels.interfaces;
using FinalProject.attributes;

namespace FinalProject.ApiModels
{
    public class UpdateOrderModel : IHasItemId, IHasQuantity, IHasOrderStatus
    {
        public int ItemId { get; set; }
        [GreaterThanZero]
        public int Quantity { get; set; }
        [ValidOrderStatus]
        public string Status { get; set; }
    }
}
