using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;

namespace AspNetCore.TypeSafe.Client.SignalR
{
    public abstract class SignalRServiceClientBase
    {
        private readonly HubConnection m_connection;

        protected SignalRServiceClientBase(HubConnection connection)
        {
            m_connection = connection;
        }

        protected static object[] BuildParams(params object[] args)
        {
            return args;
        }

        protected async Task<T> InvokeAsync<T>(object[] args, [CallerMemberName]string methodName = null)
        {
            return await m_connection.InvokeCoreAsync<T>(methodName, args).ConfigureAwait(false);
        }

        protected async Task SendCoreAsync<T>(object[] args, [CallerMemberName]string methodName = null)
        {
            await m_connection.SendCoreAsync(methodName, args).ConfigureAwait(false);
        }
    }
}
