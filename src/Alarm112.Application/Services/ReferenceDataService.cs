using Alarm112.Application.Interfaces;
using Alarm112.Contracts;

namespace Alarm112.Application.Services;

public sealed class ReferenceDataService : IReferenceDataService
{
    public Task<ReferenceDataDto> GetReferenceDataAsync(CancellationToken cancellationToken)
    {
        IReadOnlyCollection<DictionaryEntryDto> entries =
        [
            new("role.operator", "role", "Operator zgłoszeń", 1),
            new("role.dispatcher", "role", "Dyspozytor jednostek", 2),
            new("role.coordinator", "role", "Koordynator operacyjny", 3),
            new("role.crisis_officer", "role", "Oficer kryzysowy", 4),
            new("severity.low", "severity", "Niski", 1),
            new("severity.medium", "severity", "Średni", 2),
            new("severity.high", "severity", "Wysoki", 3),
            new("severity.critical", "severity", "Krytyczny", 4),
            new("unit.ambulance.basic", "unit", "Karetka podstawowa", 1),
            new("unit.fire.engine", "unit", "Wóz gaśniczy", 2),
            new("unit.police.patrol", "unit", "Patrol policji", 3)
        ];

        IReadOnlyDictionary<string, string> texts = new Dictionary<string, string>
        {
            ["hud.alert.critical"] = "ALERT KRYTYCZNY",
            ["hud.bot.active"] = "BOT aktywny",
            ["session.status.connected"] = "Połączono",
            ["ui.screen.lobby.title"] = "Lobby sesji",
            ["ui.button.quick_play"] = "Szybka gra",
            ["ui.button.ready"] = "Gotowy",
            ["ui.button.start"] = "Start",
            ["ui.button.take_over"] = "Przejmij rolę"
        };

        return Task.FromResult(new ReferenceDataDto(entries, texts));
    }
}
