namespace DartAppClean.Domain.Entities.GameEntites
{
    public class Round
    {
        public int Id { get; set; }
        public int GameId { get; set; }
        public Game Game { get; set; } = null!;
        public int RoundNumber { get; set; } 
        public DateTime StartedAt { get; set; } = DateTime.UtcNow;
        public ICollection<RoundScore> Scores { get; set; } = new List<RoundScore>();
        public int CurrentScore { get; set; }
    }

}
