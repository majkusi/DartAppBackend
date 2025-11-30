namespace DartAppClean.Application.Common.Models;
public class FinishedMatchDto
{

    public bool MatchFinished { get; set; }
    public string WinnerPlayer { get; set; } = String.Empty;

    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Domain.Entities.MatchEntites.Match, FinishedMatchDto>();
        }
    }
}
