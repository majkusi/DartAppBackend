namespace DartAppClean.Domain.Entities.GameEntites
{
    public class Game : BaseAuditableEntity
    {
        public GameTypesEnum? GameTypes { get; set; }
        public X01TypeEnum? X01TypeEnum { get; set; }
        public CricketTypeEnum? CricketTypeEnum { get; set; }
        public DateTime GameStartTime { get; set; } = DateTime.UtcNow;
        public ICollection<Team>? Teams { get; set; } = new List<Team>();
        public ICollection<Round>? Rounds { get; set; } = new List<Round>();

        public void AssignTeams(IList<string> players, bool teamsMode)
        {
            int teamNumber = 1;
            if (teamsMode)
            {
                for (int i = 0; i < players.Count; i += 2)
                {
                    var team = new Team
                    {
                        Game = this,
                        GameId = this.Id,
                        TeamNumber = teamNumber++
                    };

                    team.AddPlayer(players[i]);
                    team.AddPlayer(players[i + 1]);

                    Teams!.Add(team);
                }
            }
            else
            {
                for (int i = 0; i < players.Count; i++)
                {
                    var team = new Team
                    {
                        Game = this,
                        GameId = this.Id,
                        TeamNumber = teamNumber++
                    };
                    team.AddPlayer(players[i]);
                    Teams!.Add(team);
                }

            }

        }


    }
}
