using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homestead.Shared.SignalR
{
    public static class SignalR
    {
        private static string _url = "/comms";

        public static HubConnection Initialize(string localUrl)
        {

            return new HubConnectionBuilder()
                .WithUrl(string.Concat(localUrl, _url))
                .Build();


        }
    }

}
