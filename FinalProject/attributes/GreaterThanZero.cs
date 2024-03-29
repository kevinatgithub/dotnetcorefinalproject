﻿using System.ComponentModel.DataAnnotations;

namespace FinalProject.Attributes
{
    public class GreaterThanZero : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var quantity = int.Parse(value.ToString());
            if (quantity <= 0)
                return new ValidationResult($"{validationContext.MemberName} value should be greater than zero (0)");

            return ValidationResult.Success;
        }
    }
}
