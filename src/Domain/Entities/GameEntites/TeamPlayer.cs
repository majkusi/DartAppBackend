namespace DartAppClean.Domain.Entities.MatchEntites
{
    public class TeamPlayer
    {
        public int Id { get; private set; }
        public int MatchId { get; set; }
        public int TeamId { get; private set; }
        public required Match Match { get; set; }
        public Team? Team { get; set; }
        public string PlayerUsername { get; set; } = null!;
        public int IndividualScore { get; set; }
        public bool Winner { get; set; }

        public void ScorePoints(int points)
        {
            if (points < 0)
                throw new ArgumentException("Points cannot be negative.");

            if (points == IndividualScore)
            {
                IndividualScore = 0;
                Winner = true;
                Match.FinishMatch(PlayerUsername);
            }
            else if (points < IndividualScore)
            {
                IndividualScore -= points;
            }
        }

    }
}
