﻿namespace GamesLibrary.Application.Features.Games.Commands.UpdateGame
{
    public class UpdateGameDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public Guid CreaterId { get; set; }
        public IEnumerable<UpdateGameGenresDTO> Genres { get; set; } = null!;
    }

    public class UpdateGameGenresDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;

    }
}
