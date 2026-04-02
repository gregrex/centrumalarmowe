using UnityEngine;

namespace Alarm112.ClientUnity.Runtime.UI.Reports;

public sealed class PostRoundReportController : MonoBehaviour
{
    [SerializeField] private string gradeId = "B";
    [SerializeField] private int score = 82;
    [SerializeField] private int stars = 2;

    public string GradeId => gradeId;
    public int Score => score;
    public int Stars => stars;

    public void Configure(string newGradeId, int newScore, int newStars)
    {
        gradeId = newGradeId;
        score = newScore;
        stars = newStars;
    }
}
