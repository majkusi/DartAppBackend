namespace DartAppClean.Domain.Entities.GameEntites
{
    public class TeamPlayer
    {
        public int Id { get; private set; }
        public int MatchId { get; set; }
        public int TeamId { get; private set; }
        public required Match Match { get; set; }
        public Team? Team { get; set; }
        public required string PlayerUsername { get; set; } = null!;
        public int IndividualScore { get; set; }
        public bool Winner { get; set; }
        public int Order { get; set; }
        public float? Average { get; set; }
        public int CricketMarksCounter { get; set; }
        public ICollection<int> PointsPerRound { get; set; } = new List<int>();
        public ICollection<List<int>> CricketPointsPerRound { get; set; } = new List<List<int>>();
        public void ScorePointsX01(int points)
        {
            if (points < 0)
                throw new ArgumentException("Points cannot be negative.");

            if (points == IndividualScore)
            {
                IndividualScore = 0;
                Winner = true;
            }
            else if (points < IndividualScore)
            {
                PointsPerRound.Add(points);
                IndividualScore -= points;
                Average = PointsPerRound.Sum() / PointsPerRound.Count();
            }
        }
        public void ScorePointsCricket(List<int> points)
        {
            if (points.Count == 0)
                throw new ArgumentException("Points cannot be empty");
            if (points.Count > 9)
                throw new ArgumentException("Maximum marks per round is 9!");

            CricketPointsPerRound.Add(points);
            CricketMarksCounter += points.Count();
            Average = CricketMarksCounter / CricketPointsPerRound.Count();
        }

    }
}
