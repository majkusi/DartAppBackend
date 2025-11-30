namespace DartAppClean.Domain.Entities.MatchEntites
{
    public class Team : BaseAuditableEntity
    {
        public int MatchId { get; set; }
        public required Match Match { get; set; }
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
                Match = this.Match,
            });
        }

    }
}
