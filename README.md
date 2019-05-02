# AspNetCore.TypeSafe

AspNetCore.TypeSafe introduces a typesafe rest api to your ASP.NET Core projects.

[![Build status master](https://ci.appveyor.com/api/projects/status/6dlgq1a3lqgyp7yv?svg=true&passingText=master%20-%20passing&failingText=master%20-%20failing&pendingText=master%20-%20pending)](https://ci.appveyor.com/project/janniksam/aspnetcore-typesafe) 

## Components

### AspNetCore.TypeSafe.Core

[![NuGet version](https://badge.fury.io/nu/AspNetCore.TypeSafe.Core.svg)](https://badge.fury.io/nu/AspNetCore.TypeSafe.Core)

Core-Components of the AspNetCore.TypeSafe library.

### AspNetCore.TypeSafe.Server

[![NuGet version](https://badge.fury.io/nu/AspNetCore.TypeSafe.Server.svg)](https://badge.fury.io/nu/AspNetCore.TypeSafe.Server)

Server-Components of the AspNetCore.TypeSafe library.

### AspnetCore.TypeSafe.Client.RestSharp

[![NuGet version](https://badge.fury.io/nu/AspnetCore.TypeSafe.Client.RestSharp.svg)](https://badge.fury.io/nu/AspnetCore.TypeSafe.Client.RestSharp)

An exemplary client implementation of the AspNetCore.TypeSafe library using the RestSharp-client.

### AspnetCore.TypeSafe.Client.SignalR

[![NuGet version](https://badge.fury.io/nu/AspnetCore.TypeSafe.Client.SignalR.svg)](https://badge.fury.io/nu/AspnetCore.TypeSafe.Client.SignalR)

An exemplary client implementation of the AspNetCore.TypeSafe library with a SignalR-client.

## Example usage

### Server side implementation

#### Startup.cs (SignalR  doesn't need this step)

First of all you will have to enable this framework in your Startup.cs like shown below

```cs
public void ConfigureServices(IServiceCollection services)
{
    services.AddMvc()
            .AddJsonOptions(o => o.SerializerSettings.TypeNameHandling = TypeNameHandling.Auto)
            .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
          
    services.AddTypeSafe();
}
```

Please not, that it is mandatory to at least use the TypeNameHandling setting "Auto" to support the magic, that is happening in background.

#### Implementing your API-methods

Now you can define a service interface with all the methods your api should support:

```cs
public interface IServiceInterface
{
   Task<string> Foo(string name);
   Task<SumResponse> Bar(SumRequest request);
}
```

#### Add a controller to process each request (SignalR  doesn't need this step)

Finally, to implement the actual ServiceInterface, just implement the IServiceInterface in your Controller like this:

Please not that later on our whole communication will be done `Task<object> Post(TypeSafeRequestWrapper typeSafeRequest)` by invoking the Post-method. The ResolveProvider will do the magic and forward it to each individual implemention of our interface.

```cs
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
```

### Client side implementation via RestSharp

Basically, you just have to make use of the RestSharpServiceClientBase base class.

```cs
public class ServiceClient : RestSharpServiceClientBase<IServiceInterface>, IServiceInterface
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
```

This is just an example of how to do it on the client.
Feel free to implement your own client by taking a peek at what I've done in the `RestSharpServiceClientBase`.

## SignalR implementation

To make SignalR request typesafe, you can use implement the Hub like this. It should look very similar to the way you'd implement it normally.

### Hub implementation

```cs
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
```

### Client side implementation via RestSharp

Your client can use the examplary implementaton by `SignalRServiceClientBase`

```cs
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
```
