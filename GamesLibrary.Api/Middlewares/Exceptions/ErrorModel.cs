using System.Text.Json;

namespace GamesLibrary.Api.Middlewares.Exceptions
{
    public class ErrorModel
    {
        public int StatusCode { get; internal set; }
        public string Message { get; internal set; } = string.Empty;
        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}
