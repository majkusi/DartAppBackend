namespace DartAppClean.Domain.Entities.GameEntites
{
    public class RoundScore
    {
        public int Id { get; set; }
        public int RoundId { get; set; }
        public Round Round { get; set; } = null!;
        public int TeamPlayerId { get; set; }
        public TeamPlayer TeamPlayer { get; set; } = null!;
        public int Points { get; set; }
    }
}
