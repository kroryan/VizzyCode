using UnityEngine;
using System;

namespace VizzyCodeMod
{
    /// <summary>
    /// Tiny MonoBehaviour that lives for the lifetime of the game and lets
    /// background threads post work back onto Unity's main thread.
    /// </summary>
    public class VizzyCodeUpdater : MonoBehaviour
    {
        private static VizzyCodeUpdater _instance;

        public event Action OnUpdate;

        public static VizzyCodeUpdater EnsureExists()
        {
            if (_instance != null) return _instance;

            var go = new GameObject("[VizzyCode]") { hideFlags = HideFlags.HideAndDontSave };
            DontDestroyOnLoad(go);
            _instance = go.AddComponent<VizzyCodeUpdater>();
            return _instance;
        }

        private void Update()
        {
            OnUpdate?.Invoke();
        }

        private void OnDestroy()
        {
            _instance = null;
        }
    }
}
