namespace Alarm112.Client.Runtime.UI.Reports;

public sealed class ReportProgressionController
{
    public string Grade { get; private set; } = "A";
    public int Stars { get; private set; } = 2;

    public void Bind(string grade, int stars)
    {
        Grade = grade;
        Stars = stars;
    }
}
