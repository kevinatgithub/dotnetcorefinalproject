﻿namespace FinalProject.ApiModels
{
    public class CreateOrderModel : IHasItemId
    {
        public int ItemId { get; set; }
        public int Quantity { get; set; }
    }
}
