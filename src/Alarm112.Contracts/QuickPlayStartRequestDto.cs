using System.ComponentModel.DataAnnotations;

namespace Alarm112.Contracts;

public sealed class QuickPlayStartRequestDto
{
    [Required(ErrorMessage = "ScenarioId is required.")]
    [StringLength(64, MinimumLength = 1, ErrorMessage = "ScenarioId must be 1-64 characters.")]
    [RegularExpression(@"^[a-zA-Z0-9\-_]+$", ErrorMessage = "ScenarioId contains invalid characters.")]
    public required string ScenarioId { get; set; }

    [Required(ErrorMessage = "Difficulty is required.")]
    [AllowedValues("easy", "normal", "hard",
        ErrorMessage = "Difficulty must be one of: easy, normal, hard.")]
    public required string Difficulty { get; set; }

    [Required(ErrorMessage = "PreferredRole is required.")]
    [AllowedValues("CallOperator", "Dispatcher", "OperationsCoordinator", "CrisisOfficer",
        ErrorMessage = "PreferredRole must be one of: CallOperator, Dispatcher, OperationsCoordinator, CrisisOfficer.")]
    public required string PreferredRole { get; set; }

    public bool AutoFillBots { get; set; } = true;
}
