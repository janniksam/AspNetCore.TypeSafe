using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AspnetCore.TypeSafe.Core;

namespace AspnetCore.TypeSafe.Server
{
    public class ReflectionResolver : IResolveProvider
    {
        public async Task<object> ResolveRequestAsync<T>(T target, ITypeSafeRequest request) where T : class
        {
            if (!(request is ReflectionRequest reflectionRequest))
            {
                throw new ArgumentException(
                    $"Expected type {typeof(ReflectionRequest)}. Given request was {request.GetType()}");
            }

            return await CallAsync(target, reflectionRequest);
        }

        private static async Task<object> CallAsync<T>(T target, ReflectionRequest request)
        {
            var method = typeof(T).GetMethod(request.MethodToInvoke);
            if (method == null)
            {
                throw new ArgumentOutOfRangeException(nameof(request),
                    $"The method {nameof(request.MethodToInvoke)} could not be resolved.");
            }

            var parameters = request.Parameters?.ToArray();

            var task = (Task)method.Invoke(target, parameters);
            await task.ConfigureAwait(false);

            var resultProperty = task.GetType().GetProperty("Result");
            if (resultProperty == null)
            {
                throw new ArgumentOutOfRangeException(nameof(request),
                    $"The method {nameof(request.MethodToInvoke)} does not return a Task.");
            }
            return resultProperty.GetValue(task);
        }
    }
}
