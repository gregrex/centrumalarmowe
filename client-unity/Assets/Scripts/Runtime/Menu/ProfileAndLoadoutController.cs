using System.Collections.Generic;
using UnityEngine;

namespace Alarm112.Client.Menu
{
    public sealed class ProfileAndLoadoutController : MonoBehaviour
    {
        public int AccountLevel { get; private set; } = 1;
        public Dictionary<string, int> RoleMastery { get; } = new();

        public void ApplyDemoProgression(int accountLevel)
        {
            AccountLevel = accountLevel;
            RoleMastery["operator"] = 140;
            RoleMastery["dispatcher"] = 110;
            RoleMastery["coordinator"] = 75;
            RoleMastery["crisis_officer"] = 95;
            Debug.Log($"[ProfileAndLoadoutController] accountLevel={AccountLevel}");
        }
    }
}
