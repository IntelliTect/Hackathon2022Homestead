using Homestead.Shared;
using System.Net.Http.Json;

namespace Homestead.Client.ViewModels
{
    // TODO: Change to an injectable with DI as a singleton.
    public static class LobbyVm
    {
        public static BoardVm? Board { get; internal set; }
        public static int PlayerNumber => _PlayerNumber;
        public static int _PlayerNumber;
        public static bool InGame { get; set; }


        public static async Task<bool> JoinGame(string gameId, HttpClient http)
        {
            var result = await http.PostAsync($"Join/{gameId}", null);
            string playerNum = await result.Content.ReadAsStringAsync();
            if (int.TryParse(playerNum, out _PlayerNumber))
            {
                if (PlayerNumber > 0)
                {
                    InGame = true;
                    return true;
                }
            }
            InGame = false;
            return false;
        }
        
        public static async Task<bool> CreateGame(HttpClient http)
        {
            var game = await http.GetFromJsonAsync<Game>("Create");
            _PlayerNumber = 1;
            InGame = true;
            return true;

            // TODO: return false if this fails.
        }


    }
}
