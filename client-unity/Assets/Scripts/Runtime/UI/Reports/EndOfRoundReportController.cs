
using UnityEngine;

namespace Alarm112.Client.Runtime.UI.Reports;

public sealed class EndOfRoundReportController : MonoBehaviour
{
    [SerializeField] private string grade = "A";
    [SerializeField] private int pressurePeak = 61;

    public void Bind(string newGrade, int newPressurePeak)
    {
        grade = newGrade;
        pressurePeak = newPressurePeak;
        Debug.Log($"[EndOfRoundReport] grade={grade} pressurePeak={pressurePeak}");
    }
}
