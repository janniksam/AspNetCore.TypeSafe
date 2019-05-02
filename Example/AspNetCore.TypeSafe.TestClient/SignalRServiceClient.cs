using System.Threading.Tasks;
using AspnetCore.TypeSafe.Test.Shared;
using AspNetCore.TypeSafe.Client.SignalR;
using Microsoft.AspNetCore.SignalR.Client;

namespace AspNetCore.TypeSafe.TestClient
{
    public class SignalRServiceClient : SignalRServiceClientBase, IServiceInterface
    {
        public SignalRServiceClient(HubConnection connection) : base(connection)
        {
        }

        public async Task<string> Foo(string name)
        {
            return await InvokeAsync<string>(BuildParams(name)).ConfigureAwait(false);
        }

        public async Task<SumResponse> Bar(SumRequest request)
        {
            return await InvokeAsync<SumResponse>(BuildParams(request)).ConfigureAwait(false);
        }
    }
}