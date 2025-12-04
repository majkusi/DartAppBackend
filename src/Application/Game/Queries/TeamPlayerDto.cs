namespace DartAppClean.Application.Game.Queries;
public class TeamPlayerDto
{
    public string PlayerUsername { get; set; } = null!;
    public int IndividualScore { get; set; }
    public bool Winner { get; set; }
    public int Order { get; set; }
}
