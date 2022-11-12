using Homestead.Shared;

namespace Homestead.Client.ViewModels
{
    public interface ILobbyVm
    {
        Task<bool> JoinGame(string gameId, HttpClient http);
        Task<bool> CreateGame(HttpClient http);
        void UpdateBoard(Game game);
    }
}
