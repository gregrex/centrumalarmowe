namespace Alarm112.Client.Runtime.UI.Reports;

public sealed class ReportRoomPolishController
{
    public string VariantId { get; private set; } = "report.success";
    public string SceneId { get; private set; } = "report_room.clean_night";

    public void Bind(string variantId, string sceneId)
    {
        VariantId = variantId;
        SceneId = sceneId;
    }
}
