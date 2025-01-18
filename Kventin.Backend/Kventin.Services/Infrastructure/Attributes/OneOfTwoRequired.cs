using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Kventin.Services.Infrastructure.Attributes
{
    public class OneOfTwoRequired(string property1, string property2) : ValidationAttribute
    {
        private readonly string _property1 = property1;
        private readonly string _property2 = property2;

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var property1Value = validationContext.ObjectType.GetProperty(_property1)?.GetValue(validationContext.ObjectInstance, null);
            var property2Value = validationContext.ObjectType.GetProperty(_property2)?.GetValue(validationContext.ObjectInstance, null);

            if ((property1Value == null && property2Value == null) || (property1Value != null && property2Value != null))
            {
                return new ValidationResult($"Должно быть передано только одно из двух полей: {_property1} или {_property2}");
            }

            return ValidationResult.Success;
        }
    }
}
