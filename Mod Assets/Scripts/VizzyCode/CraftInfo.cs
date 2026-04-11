using System.Collections.Generic;

namespace VizzyCodeMod
{
    // ── Data transfer objects (serialized to/from JSON) ────────────────────────

    public class StatusResponse
    {
        public bool ok = true;
        public string modVersion = VizzyCodeMod.ModVersion;
        public string scene;          // "designer" | "flight" | "menu"
        public string craftName;      // null when no craft loaded
    }

    public class CraftInfoResponse
    {
        public string name;
        public List<PartInfo> parts = new();
    }

    public class PartInfo
    {
        public int id;
        public string name;
        public bool hasVizzy;
    }

    public class VizzyResponse
    {
        public int partId;
        public string partName;
        public string xml;            // full <Program>...</Program> XML string
        public bool ok;
        public string error;
    }

    public class SetVizzyRequest
    {
        public string xml;            // <Program>...</Program> XML
    }

    public class StagesResponse
    {
        public int currentStage;
        public int numStages;
        public List<string> activationGroupNames = new();
        public List<bool> activationGroupStates = new();
    }

    public class ErrorResponse
    {
        public bool ok = false;
        public string error;
    }
}
