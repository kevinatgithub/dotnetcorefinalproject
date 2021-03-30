using DomainModels;
using FinalProject.ApiModels.interfaces;
using System;
using System.ComponentModel.DataAnnotations;

namespace FinalProject.Attributes
{
    public class ValidOrderStatus : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var model = (IHasOrderStatus)validationContext.ObjectInstance;

            if (!Enum.IsDefined(typeof(OrderStatus), model.Status))
                return new ValidationResult("Order Status should either be 'PLACED', 'COMPLETED', or 'CANCELED'");

            return ValidationResult.Success;
        }
    }
}
