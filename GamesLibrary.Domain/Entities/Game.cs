using GamesLibrary.Domain.Abstractions;
using GamesLibrary.Domain.Constants;
using System.ComponentModel.DataAnnotations;

namespace GamesLibrary.Domain.Entities
{
    public class Game : BaseEntity
    {
        [Required(ErrorMessage = "Название игры обязательно")]
        [StringLength(GameConstants.MAX_LENGTH_NAME, ErrorMessage = "Название не может быть длиннее {1}")]
        public string Name { get; set; } = string.Empty;

        [Required]
        public Guid CreaterId { get;set; }
        public Creater Creater { get; set; } = null!;

        public IEnumerable<Genre> Genres { get; set; } = new List<Genre>();
       
    }
}
