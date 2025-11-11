namespace DartAppClean.Domain.Entities.GameEntites
{
    public class Team : BaseAuditableEntity
    {
        public int GameId { get; set; }
        public  Game? Game { get; set; } 
        public int TeamNumber { get; set; }
        public int Score { get; set; }
       
        public ICollection<TeamPlayer> Players { get; set; } = new List<TeamPlayer>();
    }
}
