using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using AspnetCore.TypeSafe.Client.RestSharp;
using AspnetCore.TypeSafe.Core;
using AspnetCore.TypeSafe.Test.Shared;
using AspnetCore.TypeSafe.TestClient;
using AspnetCore.TypeSafe.TestClient.Serializer;
using RestSharp;

namespace TestClient
{
    class Program
    {
        static void Main(string[] args)
        {
            var client = new ServiceClient("http://localhost:50014/api/values");
            while (true)
            {
                var sw = new Stopwatch();
                sw.Start();

                var responseFoo = client.Foo("Jannik");
                var responseBar = client.Bar(new SumRequest {Number1 = 1, Number2 = 199});
                Task.WhenAll(responseBar, responseFoo).Wait();

                sw.Stop();
                Console.WriteLine($"{sw.ElapsedMilliseconds} - {responseFoo.Result} - {responseBar.Result.SumResult}");
            }
            
        }
    }
}
