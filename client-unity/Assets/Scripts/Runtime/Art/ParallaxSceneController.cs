using UnityEngine;

namespace Alarm112.Client.Art
{
    public sealed class ParallaxSceneController : MonoBehaviour
    {
        [SerializeField] private string presetId = "parallax.city_night";
        public string PresetId => presetId;

        public void ApplyPreset(string value)
        {
            presetId = value;
            Debug.Log($"[ParallaxSceneController] preset => {presetId}");
        }
    }
}
