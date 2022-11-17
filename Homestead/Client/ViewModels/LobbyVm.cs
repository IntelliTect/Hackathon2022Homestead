using Homestead.Shared;
using System.Net.Http.Json;

namespace Homestead.Client.ViewModels
{
    public class LobbyVm
    {
        private BoardVm? _Board;
        public BoardVm Board => _Board ?? throw new NullReferenceException("No Board Found");
        public int PlayerNumber => _PlayerNumber;
        private int _PlayerNumber;
        public bool InGame { get; set; }
        public string PlayerId { get; set; } = ""; // This gets set by the index page



        public async Task<bool> JoinGame(string gameId, HttpClient http)
        {
            var result = await http.PostAsync($"Join/{gameId}/{PlayerId}", null);
            var startGameResult = await result.Content.ReadFromJsonAsync<StartGameDto>();
            if (startGameResult == null) throw new ArgumentException("Game not found");

            _PlayerNumber = startGameResult?.PlayerNumber ?? 0;
            if (_PlayerNumber > 0)
            {
                InGame = true;
                _Board = new BoardVm(startGameResult!.Game, PlayerNumber);
                return true;
            }
            InGame = false;
            return false;
        }

        public async Task<bool> CreateGame(HttpClient http)
        {
            try
            {
                var result = await http.PostAsync($"Create/{PlayerId}", null);
                var startGameResult = await result.Content.ReadFromJsonAsync<StartGameDto>();
                if (startGameResult == null) throw new ArgumentException("Game not found");
                _PlayerNumber = startGameResult.PlayerNumber;
                _Board = new BoardVm(startGameResult!.Game, PlayerNumber);
                InGame = true;
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Update the current board.
        /// </summary>
        /// <param name="game"></param>
        public void UpdateBoard(Game game)
        {
            Board.Update(game);
        }
    }
}
