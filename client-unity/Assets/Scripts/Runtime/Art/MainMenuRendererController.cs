using UnityEngine;

namespace Alarm112.Client.Art
{
    public sealed class MainMenuRendererController : MonoBehaviour
    {
        [SerializeField] private string sceneVariantId = "scene.menu.city_night.default";
        [SerializeField] private string heroObjectId = "hero.vehicle.ambulance.alpha";

        public void ApplyRendererBundle(string variantId, string heroId)
        {
            sceneVariantId = variantId;
            heroObjectId = heroId;
            Debug.Log($"[MainMenuRendererController] variant={sceneVariantId}, hero={heroObjectId}");
        }
    }
}
