namespace DartAppClean.Domain.Entities.GameEntites
{
    public class Game : BaseAuditableEntity
    {    
        public GameTypesEnum GameTypes { get; set; }
        public X01TypeEnum? X01TypeEnum { get; set; }
        public CricketTypeEnum CricketTypeEnum { get; set; }
        public DateTime GameStartTime { get; set; } = DateTime.UtcNow;
        public ICollection<Team>? Teams { get; set; } = new List<Team>();
        public ICollection<Round>? Rounds { get; set; } = new List<Round>();
    }
}
