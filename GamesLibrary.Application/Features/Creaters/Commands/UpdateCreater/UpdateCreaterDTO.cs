using System.Text.Json;

namespace GamesLibrary.Application.Features.Creaters.Commands.UpdateCreater
{
    public class UpdateCreaterDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}
