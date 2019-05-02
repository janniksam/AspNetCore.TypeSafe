using System.Threading.Tasks;
using AspnetCore.TypeSafe.Client.RestSharp;
using AspnetCore.TypeSafe.Test.Shared;

namespace AspnetCore.TypeSafe.TestClient
{
    public class ServiceClient : RestSharpServiceClientBase, IServiceInterface
    {
        public ServiceClient(string url) : base(url)
        {
        }

        public async Task<string> Foo(string name)
        {
            var request = BuildRequest(BuildParams(name));
            return await PostAsync<string>(request).ConfigureAwait(false);
        }

        public async Task<SumResponse> Bar(SumRequest sumRequest)
        {
            var request = BuildRequest(BuildParams(sumRequest));
            return await PostAsync<SumResponse>(request).ConfigureAwait(false);
        }
    }
}