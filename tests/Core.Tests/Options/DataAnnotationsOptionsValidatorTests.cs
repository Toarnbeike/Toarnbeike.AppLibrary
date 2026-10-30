using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.ComponentModel.DataAnnotations;

namespace Toarnbeike.AppLibrary.Options;

public class DataAnnotationsOptionsValidatorTests
{
    [Test]
    public void Validate_ValidOptions_ReturnsSuccessForAnyName()
    {
        using var provider = CreateProvider<AnnotatedOptions>();
        var validator = provider.GetRequiredService<IValidateOptions<AnnotatedOptions>>();
        var options = new AnnotatedOptions { Name = "valid", Count = 5 };

        validator.Validate(null, options).Succeeded.ShouldBeTrue();
        validator.Validate("named", options).Succeeded.ShouldBeTrue();
    }

    [Test]
    public void Validate_InvalidProperties_ReturnsAllAnnotationMessages()
    {
        using var provider = CreateProvider<AnnotatedOptions>();
        var validator = provider.GetRequiredService<IValidateOptions<AnnotatedOptions>>();

        var result = validator.Validate(null, new AnnotatedOptions { Count = 0 });

        result.Failed.ShouldBeTrue();
        result.Failures.ShouldNotBeNull();
        result.Failures.Count().ShouldBe(2);
        result.Failures.ShouldContain("Name is required.");
        result.Failures.ShouldContain("Count is out of range.");
    }

    [Test]
    public void Validate_ObjectValidationWithoutMessage_ReturnsFallbackMessage()
    {
        using var provider = CreateProvider<OptionsWithObjectValidation>();
        var validator = provider.GetRequiredService<IValidateOptions<OptionsWithObjectValidation>>();

        var result = validator.Validate("named", new OptionsWithObjectValidation());

        result.Failed.ShouldBeTrue();
        result.Failures.ShouldNotBeNull();
        result.Failures.ShouldHaveSingleItem().ShouldBe("Invalid configuration.");
    }

    [Test]
    public void Validate_NullOptions_ThrowsArgumentNullException()
    {
        using var provider = CreateProvider<AnnotatedOptions>();
        var validator = provider.GetRequiredService<IValidateOptions<AnnotatedOptions>>();

        Should.Throw<ArgumentNullException>(() => validator.Validate(null, null!));
    }

    private static ServiceProvider CreateProvider<TNested>() where TNested : class
    {
        var services = new ServiceCollection();
        services.AddOptions<RootOptions<TNested>>().AddNestedOptions(root => root.Nested);
        return services.BuildServiceProvider();
    }

    public class RootOptions<TNested> where TNested : class
    {
        public TNested Nested { get; set; } = null!;
    }

    public class AnnotatedOptions
    {
        [Required(ErrorMessage = "Name is required.")]
        public string? Name { get; set; }

        [Range(1, 10, ErrorMessage = "Count is out of range.")]
        public int Count { get; set; }
    }

    public class OptionsWithObjectValidation : IValidatableObject
    {
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            yield return new ValidationResult(null);
        }
    }
}
