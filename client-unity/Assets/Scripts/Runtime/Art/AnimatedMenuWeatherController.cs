using UnityEngine;

namespace Alarm112.ClientUnity.Runtime.Art;

public sealed class AnimatedMenuWeatherController : MonoBehaviour
{
    [SerializeField] private string weatherPreset = "day_clear";
    [SerializeField] private bool reducedMotion;

    public string WeatherPreset => weatherPreset;

    public void ApplyPreset(string presetId)
    {
        weatherPreset = presetId;
    }

    public void SetReducedMotion(bool value)
    {
        reducedMotion = value;
    }
}
