using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Toarnbeike.AppLibrary.Options;

public static class OptionsBuilderExtensions
{
    extension<TRoot>(OptionsBuilder<TRoot> builder) where TRoot : class
    {
        public OptionsBuilder<TRoot> AddNestedOptions<TNested>(Func<TRoot, TNested> selector, bool withAnnotations = true)
        where TNested : class
        {
            ArgumentNullException.ThrowIfNull(builder);
            ArgumentNullException.ThrowIfNull(selector);

            builder.Services.AddSingleton(sp =>
            {
                var root = sp.GetRequiredService<IOptions<TRoot>>().Value;
                return Microsoft.Extensions.Options.Options.Create(selector(root));
            });

            if (withAnnotations)
            {
                builder.Services.AddSingleton<IValidateOptions<TNested>, DataAnnotationsOptionsValidator<TNested>>();
            }

            return builder;
        }
    }
}