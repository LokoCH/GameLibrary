using GamesLibrary.Domain.Abstractions;
using GamesLibrary.Domain.Constants;
using System.ComponentModel.DataAnnotations;

namespace GamesLibrary.Domain.Entities
{
    public class Creater : BaseEntity
    {
        [Required(ErrorMessage = "Название студии обязательно")]
        [StringLength(CreaterConstants.MAX_LENGTH_NAME, ErrorMessage = "Название не может быть длиннее {1}")]
        public string Name { get; set; } = string.Empty;

        public IEnumerable<Game>? Games { get; set; } = new List<Game>();
    }
}
