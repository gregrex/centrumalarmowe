using System.ComponentModel.DataAnnotations;

namespace Alarm112.Contracts;

public sealed class SessionActionDto
{
    [Required(ErrorMessage = "SessionId is required.")]
    [StringLength(128, MinimumLength = 1, ErrorMessage = "SessionId must be 1-128 characters.")]
    [RegularExpression(@"^[a-zA-Z0-9\-_]+$", ErrorMessage = "SessionId contains invalid characters.")]
    public required string SessionId { get; set; }

    [Required(ErrorMessage = "ActorId is required.")]
    [StringLength(64, MinimumLength = 1, ErrorMessage = "ActorId must be 1-64 characters.")]
    public required string ActorId { get; set; }

    [Required(ErrorMessage = "Role is required.")]
    [AllowedValues("CallOperator", "Dispatcher", "OperationsCoordinator", "CrisisOfficer",
        ErrorMessage = "Role must be one of: CallOperator, Dispatcher, OperationsCoordinator, CrisisOfficer.")]
    public required string Role { get; set; }

    [Required(ErrorMessage = "ActionType is required.")]
    [AllowedValues("dispatch", "escalate", "resolve",
        ErrorMessage = "ActionType must be one of: dispatch, escalate, resolve.")]
    public required string ActionType { get; set; }

    [StringLength(1024, ErrorMessage = "PayloadJson must not exceed 1024 characters.")]
    public string? PayloadJson { get; set; }

    [Required(ErrorMessage = "CorrelationId is required.")]
    [StringLength(64, MinimumLength = 1, ErrorMessage = "CorrelationId must be 1-64 characters.")]
    public required string CorrelationId { get; set; }
}
