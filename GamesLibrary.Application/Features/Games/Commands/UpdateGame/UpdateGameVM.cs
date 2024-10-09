using System.Text.Json;

namespace GamesLibrary.Application.Features.Games.Commands.UpdateGame
{
    public class UpdateGameVM
    {
        public UpdateGameDTO OldGame { get; set; } = null!;
        public UpdateGameDTO NewGame { get; set; } = null!;

        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}
