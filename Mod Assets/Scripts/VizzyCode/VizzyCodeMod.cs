using ModApi.Mods;
using UnityEngine;

namespace VizzyCodeMod
{
    /// <summary>
    /// Entry point for the VizzyCode mod.
    ///
    /// Exposes a local HTTP bridge on http://127.0.0.1:7842/ so that the
    /// VizzyCode desktop app and VS Code extension can read and write Vizzy
    /// programs in the running game without touching save files on disk.
    ///
    /// Pattern: private constructor + GetModInstance&lt;T&gt;() (official Jundroo pattern).
    /// Lifecycle: OnModInitialized() is called after the mod is fully loaded.
    /// </summary>
    public class VizzyCodeMod : GameMod
    {
        public const int BridgePort = 7842;
        public const string ModVersion = "0.0.61";

        private VizzyCodeMod() : base() { }
        public static VizzyCodeMod Instance { get; } = GetModInstance<VizzyCodeMod>();

        private VizzyBridge _bridge;

        protected override void OnModInitialized()
        {
            base.OnModInitialized();

            try
            {
                _bridge = new VizzyBridge(BridgePort);
                _bridge.Start();
                Debug.Log($"[VizzyCode] Mod initialized. HTTP bridge on http://127.0.0.1:{BridgePort}/");
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"[VizzyCode] Failed to start bridge: {ex}");
            }
        }

        private void OnDestroy()
        {
            try { _bridge?.Stop(); } catch { }
        }
    }
}
