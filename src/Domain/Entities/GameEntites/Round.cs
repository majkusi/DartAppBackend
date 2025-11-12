namespace DartAppClean.Domain.Entities.GameEntites
{
    public class Round : BaseAuditableEntity
    {
        public int GameId { get; set; }
        public int PlayerId { get; set; }
        public Game Game { get; set; } = null!;
        public int RoundNumber { get; set; } 
        public DateTime StartedAt { get; set; } = DateTime.UtcNow;
        public int Points { get; set; }
    }
}
