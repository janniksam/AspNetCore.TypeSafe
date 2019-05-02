using System;
using System.Diagnostics;
using System.Threading.Tasks;
using AspnetCore.TypeSafe.Test.Shared;
using AspnetCore.TypeSafe.TestClient;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Win32.SafeHandles;
using Newtonsoft.Json;

namespace AspNetCore.TypeSafe.TestClient
{
    class Program
    {
        static void Main(string[] args)
        {
            var client = new ServiceClient("http://localhost:50014/api/values");
            for (var i = 0; i < 10; i++)
            {
                var sw = new Stopwatch();
                sw.Start();

                var responseFoo = client.Foo("Jannik");
                var responseBar = client.Bar(new SumRequest { Number1 = 1, Number2 = 199 });
                Task.WhenAll(responseBar, responseFoo).Wait();

                sw.Stop();
                Console.WriteLine($"{sw.ElapsedMilliseconds} - {responseFoo.Result} - {responseBar.Result.SumResult}");
            }

            var connection = new HubConnectionBuilder()
                .WithUrl("http://localhost:50014/test")
                .AddJsonProtocol(p => p.PayloadSerializerSettings = new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.Auto
                })
                .Build();

            connection.StartAsync().Wait();

            var signalRClient = new SignalRServiceClient(connection);
            while (true)
            {
                var sw = new Stopwatch();
                sw.Start();

                var responseFoo = signalRClient.Foo("Jannik");
                var responseBar = signalRClient.Bar(new SumRequest { Number1 = 1, Number2 = 199 });
                Task.WhenAll(responseBar, responseFoo).Wait();

                sw.Stop();
                Console.WriteLine($"{sw.ElapsedMilliseconds} - {responseFoo.Result} - {responseBar.Result.SumResult}");
            }
        }
    }
}
