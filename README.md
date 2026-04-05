# VizzyCode

**Open, convert, and AI-edit Vizzy programs as C# code.**

VizzyCode is a companion tool for [VizzyBuilder](https://github.com/RayanRal/VizzyBuilder) that lets you:

- Open **any** Vizzy XML file (craft files or standalone programs)
- Automatically convert all Vizzy blocks to **editable C# VizzyBuilder API code**
- Chat with **Claude AI** to write, fix, and understand Vizzy programs
- Save the result as a `.cs` script ready to run in VizzyBuilder

![VizzyCode Screenshot](screenshot.png)

---

## Features

| Feature | Description |
|---------|-------------|
| 📂 Open Craft XML | Extracts all FlightPrograms from a craft file |
| 📂 Open Vizzy XML | Opens standalone Vizzy program XML files |
| 🔄 Full Conversion | Handles all 60+ block types (events, loops, math, vectors, lists, MFD widgets, custom instructions, etc.) |
| 🤖 Claude Chat | Ask Claude to write or modify Vizzy C# code — works with your **claude.ai subscription** or an API key |
| 💾 Save as .cs | One click to save the code for VizzyBuilder |
| 🌗 Dark/Light theme | |

---

## Requirements

- Windows 10/11 x64
- **.NET 9 Desktop Runtime** (download from [dotnet.microsoft.com](https://dotnet.microsoft.com/download/dotnet/9.0))  
  *Or use the self-contained build — no runtime needed.*

### For Claude AI chat (optional but recommended)
Either:
- **Claude Code** installed and logged in (`claude login`) → uses your **claude.ai subscription** for free
- Or an **Anthropic API key** (configure in chat panel ⚙ Settings)

---

## Build

```bash
git clone https://github.com/kroryan/VizzyCode.git
cd VizzyCode
dotnet build
dotnet run
```

### Self-contained (no runtime needed)
```bash
dotnet publish -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -o publish
```
The output `publish/VizzyCode.exe` runs on any Windows 10/11 x64 machine with no dependencies.

---

## Usage

1. **Open a file**: `File > Open Craft XML` or `File > Open Vizzy XML`
2. **Browse the structure** in the left tree panel (variables, events, custom instructions)
3. **Edit the C# code** in the center editor
4. **Ask Claude** in the right chat panel — e.g.:
   - *"Add a PID controller for pitch"*
   - *"Explain what this custom instruction does"*
   - *"Convert this to use a while loop instead"*
5. **Save**: `Ctrl+S` → saves as `.cs` to the VizzyBuilder Scripts folder

---

## How it works

Vizzy programs are stored as XML inside craft files or as standalone `.xml` files.  
VizzyCode parses that XML and maps every block type to the equivalent **VizzyBuilder C# API** call:

```xml
<!-- Vizzy XML -->
<While>
  <Comparison op="l">
    <CraftProperty property="Altitude.ASL" />
    <Constant text="70000" />
  </Comparison>
  <Instructions>
    <SetThrottle><Constant text="1" /></SetThrottle>
  </Instructions>
</While>
```

```csharp
// Generated C# (VizzyBuilder API)
using (new While((Vz.Craft.AltitudeASL() < 70000)))
{
    Vz.SetThrottle(1);
}
```

The generated code can be run directly in VizzyBuilder to regenerate the Vizzy XML.

---

## Credits

- **VizzyBuilder** by Rayan Ral — the C# API this tool generates code for  
  [github.com/RayanRal/VizzyBuilder](https://github.com/RayanRal/VizzyBuilder)
- **Juno: New Origins** — the game that uses Vizzy  
- **Claude** by Anthropic — AI assistant integration

---

## License

MIT
