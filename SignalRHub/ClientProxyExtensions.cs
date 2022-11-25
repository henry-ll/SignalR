using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalRHub
{
	public static class ClientProxyExtensions
	{
		public static Task SendAsync(this IClientProxy clientProxy, string method, List<object>  arg1, CancellationToken cancellationToken = default(CancellationToken))
		{
			return clientProxy.SendCoreAsync(method, arg1.ToArray(), cancellationToken);
		}
	}
}
