namespace Alarm112.Contracts;

public sealed record LiveopsReviewPanelDto(
    string Title,
    IReadOnlyList<LiveopsReviewWidgetDto> Widgets,
    string ReviewState);
