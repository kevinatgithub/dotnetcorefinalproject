namespace FinalProject.ApiModels
{
    public class CreateOrderModel : IHasItemId, IHasQuantity
    {
        public int ItemId { get; set; }
        public int Quantity { get; set; }
    }
}
