using System;
using System.ComponentModel.DataAnnotations;

namespace VoltaPetsAPI.Helpers
{
    public class FechaAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if(DateTime.TryParse(Convert.ToString(value), out DateTime fecha))
            {
                return ValidationResult.Success;
            }

            return new ValidationResult(ErrorMessage);
        }

    }
}
