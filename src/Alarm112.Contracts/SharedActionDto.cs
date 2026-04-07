using System.ComponentModel.DataAnnotations;

namespace Alarm112.Contracts;

public sealed record SharedActionDto(
    [property: Required(ErrorMessage = "SharedActionId is required.")]
    [property: StringLength(128, MinimumLength = 1, ErrorMessage = "SharedActionId must be 1-128 characters.")]
    [property: RegularExpression(@"^[a-zA-Z0-9\-_]+$", ErrorMessage = "SharedActionId contains invalid characters.")]
    string SharedActionId,

    [property: Required(ErrorMessage = "IncidentId is required.")]
    [property: StringLength(64, MinimumLength = 1, ErrorMessage = "IncidentId must be 1-64 characters.")]
    [property: RegularExpression(@"^[a-zA-Z0-9\-_]+$", ErrorMessage = "IncidentId contains invalid characters.")]
    string IncidentId,

    [property: Required(ErrorMessage = "ActionType is required.")]
    [property: StringLength(64, MinimumLength = 1, ErrorMessage = "ActionType must be 1-64 characters.")]
    string ActionType,

    [property: Required(ErrorMessage = "RequestedByRole is required.")]
    [property: AllowedValues("CallOperator", "Dispatcher", "OperationsCoordinator", "CrisisOfficer",
        ErrorMessage = "RequestedByRole must be one of: CallOperator, Dispatcher, OperationsCoordinator, CrisisOfficer.")]
    string RequestedByRole,

    IReadOnlyCollection<string> RequiredRoles,

    [property: Range(1, 300, ErrorMessage = "TimeoutSeconds must be between 1 and 300.")]
    int TimeoutSeconds,

    bool AllowBotAssist);
