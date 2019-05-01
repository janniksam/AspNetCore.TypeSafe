using System.Threading.Tasks;
using AspnetCore.TypeSafe.Core;

namespace AspnetCore.TypeSafe.Server
{
    public interface IResolveProvider
    {
        Task<object> ResolveRequestAsync<T>(T target, ITypeSafeRequest request) where T : class;
    }
}