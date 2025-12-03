using DartAppClean.Domain.Entities.GameEntites;

namespace DartAppClean.Domain.Entities.MatchEntites
{
    public class Round : BaseAuditableEntity
    {
        public int GameId { get; set; }
        public int PlayerId { get; set; }
        public string PlayerUsername { get; set; } = null!;
        public Match Game { get; set; } = null!;
        public int RoundNumber { get; set; }
        public DateTime StartedAt { get; set; } = DateTime.UtcNow;
        public int Points { get; set; }

        private Round() { }

        private Round(int gameId, int roundNumber, int points, string playerUsername)
        {
            GameId = gameId;
            RoundNumber = roundNumber;
            Points = points;
            PlayerUsername = playerUsername;
        }
        public static Round Create(int gameId, int roundNumber, int points, string playerUsername)
        {
            var round = new Round(gameId, roundNumber, points, playerUsername);
            round.AddDomainEvent(new RoundCreatedEvent(round.GameId, round.PlayerUsername, round.Points));
            return round;
        }



    }
}
