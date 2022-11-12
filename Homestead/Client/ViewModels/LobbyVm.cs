using Homestead.Shared;
using System.Net.Http.Json;

namespace Homestead.Client.ViewModels
{
    public class LobbyVm : ILobbyVm
    {
        public BoardVm? Board { get; set; }
        public int PlayerNumber => _PlayerNumber;
        private int _PlayerNumber;
        public bool InGame { get; set; }


        public async Task<bool> JoinGame(string gameId, HttpClient http)
        {
            var result = await http.PostAsync($"Join/{gameId}", null);
            var startGameResult = await result.Content.ReadFromJsonAsync<StartGameDto>();
            if (startGameResult == null) throw new ArgumentException("Game not found");
            
            _PlayerNumber = startGameResult?.PlayerId ?? 0;
            if (_PlayerNumber > 0)
            {
                InGame = true;
                Board = new BoardVm(startGameResult!.Game, PlayerNumber);
                return true;
            }
            InGame = false;
            return false;
        }
        
        public async Task<bool> CreateGame(HttpClient http)
        {
            try
            {
                var result = await http.GetFromJsonAsync<StartGameDto>("Create");
                if (result == null) throw new ArgumentException("Game not found");
                _PlayerNumber = result.PlayerId;
                Board = new BoardVm(result!.Game, PlayerNumber);
                InGame = true;
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public void UpdateBoard(Game game)
        {
            if (Board == null)
            {
                Board = new BoardVm(game, PlayerNumber);
            }
            else
            {
                Board.Update(game);
            }
        }
    }
}
