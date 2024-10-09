using System.Text.Json;

namespace GamesLibrary.Application.Features.Creaters.Commands.UpdateCreater
{
    public class UpdateCreaterVM
    {
        public UpdateCreaterDTO OldCreater { get; set; } = null!;
        public UpdateCreaterDTO NewCreater { get; set; } = null!;

        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}
