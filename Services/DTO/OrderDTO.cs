﻿using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Services.DTO
{
    public class OrderDTO
    {
        public int? Id { get; set; }
        public int ItemId { get; set; }
        public int Quantity { get; set; }
        [Column(TypeName = "decimal(7,2)")]
        public decimal TotalPrice { get; set; }
        public string Status { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
