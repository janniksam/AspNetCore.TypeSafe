using System.Threading.Tasks;

namespace AspnetCore.TypeSafe.Test.Shared
{
    public interface IServiceInterface
    {
        Task<string> Foo(string name);

        Task<SumResponse> Bar(SumRequest request);
    }
}