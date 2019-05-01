using System.Threading.Tasks;
using AspnetCore.TypeSafe.Core;
using AspnetCore.TypeSafe.Server;
using AspnetCore.TypeSafe.Test.Shared;
using Microsoft.AspNetCore.Mvc;

namespace AspnetCore.TypeSafe.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase, IServiceInterface
    {
        private readonly IResolveProvider m_resolveProvider;

        public ValuesController(IResolveProvider resolveProvider)
        {
            m_resolveProvider = resolveProvider;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return BadRequest();
        }

        [HttpPost]
        public async Task<object> Post(TypeSafeRequestWrapper typeSafeRequest)
        {
            // Here happens the "magic"
            var result = await m_resolveProvider.ResolveRequestAsync(this, typeSafeRequest?.Request);
            return result;
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
