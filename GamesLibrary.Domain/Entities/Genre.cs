using GamesLibrary.Domain.Abstractions;
using GamesLibrary.Domain.Constants;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace GamesLibrary.Domain.Entities
{
    public class Genre : BaseEntity
    {
        [Required(ErrorMessage = "Название жанра обязательно")]
        [StringLength(GenreConstants.MAX_LENGTH_NAME, ErrorMessage = "Название не может быть длиннее {1}")]
        public string Name { get; set; } = string.Empty;

        [JsonIgnore]
        public IEnumerable<Game> Games { get; set; } = new List<Game>();
    }
}
