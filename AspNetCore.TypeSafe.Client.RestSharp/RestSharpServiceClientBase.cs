﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using AspnetCore.TypeSafe.Core;
using AspnetCore.TypeSafe.TestClient.Serializer;
using RestSharp;

namespace AspnetCore.TypeSafe.Client.RestSharp
{
    public abstract class RestSharpServiceClientBase : RestClient
    {
        protected RestSharpServiceClientBase(string url) : base(url)
        {
        }

        protected static object[] BuildParams(params object[] args)
        {
            return args;
        }

        protected RestRequest BuildRequest(IEnumerable<object> parameter, [CallerMemberName]string caller = null)
        {
            var restRequest = new RestRequest
            {
                JsonSerializer = NewtonsoftJsonSerializer.Default
            };

            var request = new ReflectionRequest(caller, parameter?.ToList());
            restRequest.AddJsonBody(request.Wrap());
            return restRequest;
        }

        protected async Task<T> PostAsync<T>(IRestRequest request)
        {
            var response = await ExecutePostTaskAsync<T>(request).ConfigureAwait(false);
            if (response.IsSuccessful)
            {
                return response.Data;
            }

            if (response.ErrorException != null)
            {
                throw new Exception(response.ErrorMessage, response.ErrorException);
            }

            throw new Exception(response.ErrorMessage);
        }
    }
}
