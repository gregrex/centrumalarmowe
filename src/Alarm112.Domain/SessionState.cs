namespace Alarm112.Domain;

public enum SessionState
{
    Draft = 0,
    Lobby = 1,
    Countdown = 2,
    Active = 3,
    Recovery = 4,
    Summary = 5,
    Archived = 6
}
