namespace DartAppClean.Domain.Entities.MatchEntites
{
    public class Match : BaseAuditableEntity
    {
        public MatchTypesEnum? MatchTypes { get; set; }
        public X01TypeEnum? X01TypeEnum { get; set; }
        public CricketTypeEnum? CricketTypeEnum { get; set; }
        public DateTime MatchStartTime { get; set; } = DateTime.UtcNow;
        public ICollection<Team>? Teams { get; set; } = new List<Team>();
        public ICollection<Round>? Rounds { get; set; } = new List<Round>();
        public string CurrentPlayer { get; set; } = String.Empty;
        public bool MatchFinished { get; set; } = false;
        public string WinnerPlayer { get; set; } = String.Empty;
        public void FinishMatch(string winnerUsername)
        {
            MatchFinished = true;
            WinnerPlayer = winnerUsername;
        }
        public void AssignTeams(IList<string> players, bool teamsMode, int score)
        {
            int teamNumber = 1;
            if (teamsMode)
            {
                for (int i = 0; i < players.Count; i += 2)
                {
                    var team = new Team
                    {
                        Match = this,
                        MatchId = this.Id,
                        TeamNumber = teamNumber++,
                        Score = score
                    };
                    Teams!.Add(team);
                    team.AddPlayer(players[i], score, team.MatchId);
                    team.AddPlayer(players[i + 1], score, team.MatchId);
                }
            }
            else
            {
                for (int i = 0; i < players.Count; i++)
                {
                    var team = new Team
                    {
                        Match = this,
                        MatchId = this.Id,
                        TeamNumber = teamNumber++
                    };

                    Teams!.Add(team);
                    team.AddPlayer(players[i], score, team.MatchId);
                }

            }

        }


    }
}
