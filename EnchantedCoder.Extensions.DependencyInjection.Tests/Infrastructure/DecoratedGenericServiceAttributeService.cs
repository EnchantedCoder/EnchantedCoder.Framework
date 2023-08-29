using EnchantedCoder.Extensions.DependencyInjection.Abstractions;

namespace EnchantedCoder.Extensions.DependencyInjection.Tests.Infrastructure;

[Service<DecoratedGenericServiceAttributeService>(Profile = nameof(DecoratedGenericServiceAttributeService))]
public class DecoratedGenericServiceAttributeService
{
}
