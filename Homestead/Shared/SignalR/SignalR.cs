using Microsoft.AspNetCore.SignalR.Client;

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
