namespace DartAppClean.Domain.Entities.GameEntites
{
    public class TeamPlayer
    {
        public int Id { get; private set; }
        public int TeamId { get; private set; }
        public Team? Team { get; private set; }
        public string PlayerUsername { get;  set; } = null!;
        public int IndividualScore { get;  set; }

        public void ScorePoints(int points)
        {
            IndividualScore += points;
        }
    }
}
