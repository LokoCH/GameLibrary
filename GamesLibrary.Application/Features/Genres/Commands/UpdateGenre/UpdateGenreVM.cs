using System.Text.Json;

namespace GamesLibrary.Application.Features.Genres.Commands.UpdateGenre
{
    public class UpdateGenreVM
    {
        public UpdateGenreDTO OldGenre { get; set; } = null!;
        public UpdateGenreDTO NewGenre { get; set; } = null!;

        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}
