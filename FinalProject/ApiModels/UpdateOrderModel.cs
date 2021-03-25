﻿using System.ComponentModel.DataAnnotations.Schema;

namespace FinalProject.ApiModels
{
    public class UpdateOrderModel
    {
        public int ItemId { get; set; }
        public int Quantity { get; set; }
        public string Status { get; set; }
    }
}