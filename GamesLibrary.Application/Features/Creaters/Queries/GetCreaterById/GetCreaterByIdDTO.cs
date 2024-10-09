namespace GamesLibrary.Application.Features.Creaters.Queries.GetCreaterById
{
    public class GetCreaterByIdDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}