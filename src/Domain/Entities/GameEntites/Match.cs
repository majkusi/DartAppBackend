using DartAppClean.Domain.Entities.MatchEntites;

namespace DartAppClean.Domain.Entities.GameEntites
{
    public class Match : BaseAuditableEntity
    {
        public GameTypesEnum? GameTypes { get; set; }
        public X01TypeEnum? X01TypeEnum { get; set; }
        public CricketTypeEnum? CricketTypeEnum { get; set; }
        public DateTime GameStartTime { get; set; } = DateTime.UtcNow;
        public ICollection<Team> Teams { get; set; } = new List<Team>();
        public ICollection<Round>? Rounds { get; set; } = new List<Round>();
        public string CurrentPlayer { get; set; } = String.Empty;
        public bool GameFinished { get; set; } = false;
        public string WinnerUsername { get; set; } = String.Empty;
        public List<string> TurnOrder { get; set; } = new List<string>();

        private Match() { }
        private Match(GameTypesEnum gameType, X01TypeEnum? x01Type)
        {
            GameTypes = gameType;
            X01TypeEnum = x01Type;
        }

        public static Match Create(GameTypesEnum gameType, X01TypeEnum? x01Type)
        {
            var match = new Match(gameType, x01Type);
            match.AddDomainEvent(new MatchCreatedEvent(match));
            return match;
        }

        public void FinishMatch(string winnerUsername)
        {
            GameFinished = true;
            WinnerUsername = winnerUsername;
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
                        Game = this,
                        GameId = this.Id,
                        TeamNumber = teamNumber++,
                        Score = score
                    };

                    Teams!.Add(team);

                    team.AddPlayer(players[i], score, team.GameId);
                    var player1 = team.Players.Where(p => p.PlayerUsername == players[i]).FirstOrDefault();


                    if (player1 == null) throw new Exception("Player is null ");
                    player1.Order = 1;
                    CurrentPlayer = player1.PlayerUsername;
                    if (i + 1 < players.Count)
                    {
                        team.AddPlayer(players[i + 1], score, team.GameId);
                        var player2 = team.Players.Where(p => p.PlayerUsername == players[i + 1]).FirstOrDefault();
                        if (player2 == null) throw new Exception("Player 2 is null");
                        player2.Order = 2;
                    }

                }

                var orderedTeams = Teams
                    .OrderBy(t => t.TeamNumber)
                    .Select(t => t.Players
                        .OrderBy(p => players.IndexOf(p.PlayerUsername))
                        .Select(p => p.PlayerUsername)
                        .ToList())
                    .ToList();

                var maxPlayers = orderedTeams.Max(t => t.Count);
                TurnOrder.Clear();

                for (int i = 0; i < maxPlayers; i++)
                {
                    foreach (var teamPlayers in orderedTeams)
                    {
                        if (i < teamPlayers.Count)
                        {
                            TurnOrder.Add(teamPlayers[i]);
                        }
                    }
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

                    Teams!.Add(team);
                    team.AddPlayer(players[i], score, team.GameId);
                }
                TurnOrder = players.ToList();
                CurrentPlayer = players[0];
            }
        }



    }
}
