using DartAppClean.Domain.Entities.GameEntites;

namespace DartAppClean.Domain.Entities.MatchEntites
{
    public class Team : BaseAuditableEntity
    {
        public int GameId { get; set; }
        public required Game Game { get; set; }
        public int TeamNumber { get; set; }
        public int Score { get; set; }

        public ICollection<TeamPlayer> Players { get; set; } = new List<TeamPlayer>();

        public void AddPlayer(string username, int score, int MatchId)
        {
            if (string.IsNullOrWhiteSpace(username))
                throw new Exception("Player username cannot be empty.");

            Players.Add(new TeamPlayer
            {
                PlayerUsername = username,
                IndividualScore = score,
                Team = this,
                Game = this.Game,
            });
        }

    }
}
