using Microsoft.Extensions.Options;
using System.ComponentModel.DataAnnotations;

namespace Toarnbeike.AppLibrary.Options;

/// <summary>
/// Validates options instances by evaluating data annotation attributes and producing options validation results.
/// </summary>
/// <remarks>Runs object validation for all properties and reports any validation error messages as configuration failures.</remarks>
/// <typeparam name="T">The options type to validate.</typeparam>
internal sealed class DataAnnotationsOptionsValidator<T> : IValidateOptions<T>
    where T : class
{
    /// <inheritdoc />
    public ValidateOptionsResult Validate(string? name, T options)    {
        var context = new ValidationContext(options);
        var results = new List<ValidationResult>();

        return Validator.TryValidateObject(options, context, results, validateAllProperties: true)
            ? ValidateOptionsResult.Success
            : ValidateOptionsResult.Fail(results.Select(x => x.ErrorMessage ?? "Invalid configuration."));
    }
}