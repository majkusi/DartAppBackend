namespace DartAppClean.Domain.Entities.MatchEntites
{
    public class Round : BaseAuditableEntity
    {
        public int MatchId { get; set; }
        public int PlayerId { get; set; }
        public string PlayerUsername { get; set; } = null!;
        public Match Match { get; set; } = null!;
        public int RoundNumber { get; set; }
        public DateTime StartedAt { get; set; } = DateTime.UtcNow;
        public int Points { get; set; }



    }
}
