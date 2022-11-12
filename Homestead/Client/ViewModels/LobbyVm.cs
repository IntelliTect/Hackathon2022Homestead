using Homestead.Shared;
using System.Net.Http.Json;

namespace Homestead.Client.ViewModels
{
    public class LobbyVm : ILobbyVm
    {
        public BoardVm? Board { get; internal set; }
        public int PlayerNumber => _PlayerNumber;
        public int _PlayerNumber;
        public bool InGame { get; set; }


        public async Task<bool> JoinGame(string gameId, HttpClient http)
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
        
        public async Task<bool> CreateGame(HttpClient http)
        {
            try
            {
                var game = await http.GetFromJsonAsync<Game>("Create");
                _PlayerNumber = 1;
                InGame = true;
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
    }
}
