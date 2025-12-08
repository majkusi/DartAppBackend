
using DartAppClean.Domain.Entities.GameEntites;

namespace DartAppClean.Domain.Entities.MatchEntites
{
    public class Team : BaseAuditableEntity
    {
        public int MatchId { get; set; }
        public required Match Match { get; set; }
        public int TeamNumber { get; set; }
        public int Score { get; set; }

        public ICollection<TeamPlayer> Players { get; set; } = new List<TeamPlayer>();

        public void AddPlayer(string username, int score, int matchId)
        {
            if (Match is null)
            {
                throw new InvalidOperationException("Team.Match must be set before adding players.");
            }

            if (MatchId != matchId)
            {
                throw new ArgumentException("Provided MatchId does not match Team.MatchId");
            }

            if (string.IsNullOrWhiteSpace(username))
            {
                throw new ArgumentException("Player username cannot be empty or whitespace.", nameof(username));
            }

            var normalized = username.Trim();

            Players.Add(new TeamPlayer
            {
                PlayerUsername = normalized,  
                IndividualScore = score,
                Team = this,
                Match = this.Match,
            });
        }
    }
}
