namespace DartAppClean.Domain.Entities.GameEntites
{
    public class Team
    {
        public int Id { get; set; }
        public int GameId { get; set; }
        public Game Game { get; set; } = null!;
        public int TeamNumber { get; set; }
        public int Score { get; set; }
        public ICollection<TeamPlayer> Players { get; set; } = new List<TeamPlayer>();
    }
}
