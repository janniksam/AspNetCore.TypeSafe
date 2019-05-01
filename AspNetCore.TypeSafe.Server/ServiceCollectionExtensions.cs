using System;
using Microsoft.Extensions.DependencyInjection;

namespace AspnetCore.TypeSafe.Server
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddTypeSafe(this IServiceCollection collection, Func<TypeSafeOptions> optionsBuilder = null)
        {
            var options = optionsBuilder?.Invoke();
            if (options == null)
            {
                options = new TypeSafeOptions
                {
                    ResolveProvider = new ReflectionResolver()
                };
            }

            collection.AddSingleton(options.ResolveProvider);
            return collection;
        }
    }
}