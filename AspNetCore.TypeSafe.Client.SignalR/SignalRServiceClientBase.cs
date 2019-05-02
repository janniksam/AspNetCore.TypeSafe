using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;

namespace AspNetCore.TypeSafe.Client.SignalR
{
    public abstract class SignalRServiceClientBase
    {
        protected readonly HubConnection Connection;

        protected SignalRServiceClientBase(HubConnection connection)
        {
            Connection = connection;
        }

        protected static object[] BuildParams(params object[] args)
        {
            return args;
        }

        protected async Task<T> InvokeAsync<T>(object[] args, [CallerMemberName]string methodName = null)
        {
            return await Connection.InvokeCoreAsync<T>(methodName, args).ConfigureAwait(false);
        }

        protected async Task InvokeAsync(object[] args, [CallerMemberName]string methodName = null)
        {
            await Connection.InvokeCoreAsync(methodName, args).ConfigureAwait(false);
        }

        protected async Task SendCoreAsync(object[] args, [CallerMemberName]string methodName = null)
        {
            await Connection.SendCoreAsync(methodName, args).ConfigureAwait(false);
        }
    }
}
