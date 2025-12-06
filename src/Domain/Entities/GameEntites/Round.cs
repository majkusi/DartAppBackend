using DartAppClean.Domain.Entities.GameEntites;

namespace DartAppClean.Domain.Entities.MatchEntites
{
    public class Round : BaseAuditableEntity
    {
        public int MatchId { get; set; }
        public int PlayerId { get; set; }
        public string PlayerUsername { get; set; } = null!;
        public Match Game { get; set; } = null!;
        public int? RoundNumber { get; set; }
        public DateTime StartedAt { get; set; } = DateTime.UtcNow;
        public int Points { get; set; }

        private Round() { }

        private Round(int gameId, int roundNumber, int points, string playerUsername)
        {
            MatchId = gameId;
            RoundNumber = roundNumber;
            Points = points;
            PlayerUsername = playerUsername;
        }
        public static Round Create(int matchId, int roundNumber, int points, string playerUsername)
        {
            if (matchId < 0)
                throw new ArgumentException("Provided matchId is less to zero");
            if (string.IsNullOrEmpty(playerUsername))
                throw new ArgumentException("Provided playerUsername is null or empty");
            if (points < 0)
                throw new ArgumentException("Provided points are less than zero");
            var round = new Round(matchId, roundNumber, points, playerUsername);
            round.AddDomainEvent(new RoundCreatedEvent(round.MatchId, round.PlayerUsername, round.Points));

            return round;
        }
    }
}
