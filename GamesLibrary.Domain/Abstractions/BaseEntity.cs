using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace GamesLibrary.Domain.Abstractions
{
    public class BaseEntity
    {
        [Required]
        public Guid Id { get; set; }

        public override string ToString()
        {
            return JsonSerializer.Serialize(this, GetType());
        }
    }
}
