using System.ComponentModel.DataAnnotations;

namespace Alarm112.Contracts;

public sealed record DispatchCommandDto(
    [property: Required(ErrorMessage = "IncidentId is required.")]
    [property: StringLength(64, MinimumLength = 1)]
    [property: RegularExpression(@"^[a-zA-Z0-9\-_\.]+$", ErrorMessage = "IncidentId contains invalid characters.")]
    string IncidentId,

    [property: Required(ErrorMessage = "UnitId is required.")]
    [property: StringLength(64, MinimumLength = 1)]
    [property: RegularExpression(@"^[a-zA-Z0-9\-_\.]+$", ErrorMessage = "UnitId contains invalid characters.")]
    string UnitId,

    [property: Required(ErrorMessage = "ActionId is required.")]
    [property: StringLength(64, MinimumLength = 1)]
    string ActionId,

    [property: Required(ErrorMessage = "ActorRole is required.")]
    [property: AllowedValues("CallOperator", "Dispatcher", "OperationsCoordinator", "CrisisOfficer",
        ErrorMessage = "ActorRole must be one of: CallOperator, Dispatcher, OperationsCoordinator, CrisisOfficer.")]
    string ActorRole,

    bool IsBot);
