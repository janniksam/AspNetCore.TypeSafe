using System.Threading.Tasks;
using AspnetCore.TypeSafe.Core;
using AspnetCore.TypeSafe.Server;
using AspnetCore.TypeSafe.Test.Shared;
using Microsoft.AspNetCore.SignalR;

namespace AspNetCore.TypeSafe.Web.Hubs
{
    public class MyTypeSafeHub : Hub, IServiceInterface
    {
        private readonly IResolveProvider m_resolveProvider;

        public MyTypeSafeHub(IResolveProvider resolveProvider)
        {
            m_resolveProvider = resolveProvider;
        }

        public Task<string> Foo(string name)
        {
            return Task.Run(() => $"Hello {name}");
        }

        public Task<SumResponse> Bar(SumRequest request)
        {
            return Task.FromResult(new SumResponse {SumResult = request.Number1 + request.Number2});
        }
    }
}