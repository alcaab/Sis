using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace Desyco.Dms.Application.Common.Validations;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public class MinCollectionLengthAttribute(int minLength) : ValidationAttribute
{
    public override string FormatErrorMessage(string name)
    {
        return string.Format(ErrorMessageString, name, minLength);
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        switch (value)
        {
            case null:
                break;
            case ICollection collection:
                {
                    if (collection.Count < minLength)
                    {
                        return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
                    }

                    break;
                }
            default:
                throw new InvalidOperationException($"The {nameof(MinCollectionLengthAttribute)} must be used on a type that implements {nameof(ICollection)}.");
        }

        return ValidationResult.Success;
    }
}
