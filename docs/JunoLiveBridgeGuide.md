# Juno Live Bridge Guide

This guide documents the optional VizzyCode live bridge for Juno: New Origins.

The bridge is split into two pieces:

- a Juno mod running inside the game
- VizzyCode desktop app / VS Code extension commands that call the mod over localhost

The goal is to avoid the slow manual loop of exporting a Vizzy XML file, importing it into Juno, reopening the Vizzy editor, and repeating. With the bridge, VizzyCode can read and write the Vizzy program stored on a part in the currently loaded craft.

## Official Modding Basis

The bridge follows the normal Juno desktop modding model:

- Juno supports PC mods through the official mod tools and Unity workflow.
- Mods should prefer the public `ModApi` surface when possible.
- The official API reference exposes `ModApi.Common.Game`, craft interfaces such as `ICraftScript`, and part APIs such as `PartData`.
- Official UI tutorials show that mods can integrate into Juno scenes, but the current VizzyCode bridge deliberately avoids custom in-game UI because the bridge only needs a local service.

Useful official references:

- https://www.simplerockets.com/Forums/View/188748/How-to-make-mods
- https://www.simplerockets.com/r/ModApi
- https://www.simplerockets.com/Forums/View/32289/
- https://www.simplerockets.com/Forums/View/32308/

## Runtime Shape

The mod starts a local HTTP listener inside the game process:

```text
http://127.0.0.1:7842/
```

The listener is intentionally bound to `127.0.0.1`, not a LAN address. It is meant for local tools on the same machine only.

The desktop app and VS Code extension use this bridge. They do not need to read or patch craft save files directly when the game is running.

## Supported Endpoints

### `GET /status`

Returns whether the bridge is alive and what Juno scene is currently active.

Typical response:

```json
{
  "ok": true,
  "modVersion": "0.1.0",
  "scene": "designer",
  "craftName": "Example Craft"
}
```

Expected `scene` values:

- `designer`
- `flight`
- `menu`

Use this endpoint before any live import/export command.

### `GET /craft`

Returns the currently loaded craft name and part list.

Each part includes:

- `id`
- `name`
- `hasVizzy`

Only parts with `hasVizzy=true` can be used by the import/export Vizzy endpoints.

### `GET /vizzy/{partId}`

Returns the full `<Program>...</Program>` XML stored in a part's `FlightProgramData`.

This is the live equivalent of importing a Vizzy program from XML. VizzyCode then converts that XML into the same clean `.vizzy.cs` view used by file imports.

### `PUT /vizzy/{partId}`

Writes a full `<Program>...</Program>` XML string back into a part's `FlightProgramData`.

VizzyCode must still run export validation before calling this endpoint. The bridge writes what it is given; it is not a replacement for exporter validation.

Request body:

```json
{
  "xml": "<Program name=\"Example\">...</Program>"
}
```

### `GET /stages`

Returns current staging and activation group metadata for the active command pod:

- `currentStage`
- `numStages`
- `activationGroupNames`
- `activationGroupStates`

This is useful for debugging craft state from VizzyCode without opening extra in-game panels.

### `POST /stages/activate`

Activates the next stage through the active command pod, then returns the updated stage metadata.

Use carefully. This is a live game action.

## Desktop App Workflow

1. Start Juno with the VizzyCode mod enabled.
2. Load a craft in the designer or flight scene.
3. Open the VizzyCode desktop app.
4. Use `Juno > Connect / Check Status`.
5. Use `Juno > Browse Craft Parts...`.
6. Select a part with a Vizzy program.
7. Import the program, edit the generated code, then export back to the same part.

The app validates generated XML before sending it back to the game. If validation fails, fix the script first.

## VS Code Workflow

1. Install the VizzyCode Tools extension.
2. Start Juno with the VizzyCode mod enabled.
3. Run `VizzyCode: Connect to Juno`.
4. Run `VizzyCode: Import Vizzy from Running Game`.
5. Choose the part to import.
6. Edit the generated `.vizzy.cs`.
7. Run `VizzyCode: Export Vizzy to Running Game`.
8. Choose the destination part.

The VS Code extension uses the bundled `VizzyCode.Cli`, so the same XML validation rules apply.

## Safety Rules

- Always keep a craft backup before exporting directly into the running game.
- Treat live export as a convenience layer, not as permanent storage.
- Do not skip the generated `*.vizzy.meta.json` sidecar for imported programs.
- Do not export to a different part unless you intentionally want to replace that part's Vizzy program.
- Do not use `POST /stages/activate` while a real mission is in a fragile state.

## Threading Model

The bridge receives HTTP requests on background threads, but Juno and Unity craft objects must be read or modified on Unity's main thread.

The mod therefore queues craft operations and flushes them from a tiny `MonoBehaviour.Update()` loop.

Safe main-thread operations include:

- reading the current designer craft
- reading the current flight craft
- scanning craft parts
- reading/writing `FlightProgramData.FlightProgramXml`
- reading active command pod stage metadata
- activating the next stage

Future bridge endpoints should follow the same rule. If an endpoint touches `Game.Instance`, craft scripts, parts, command pods, or Unity objects, run it on the main thread.

## What The Bridge Is Not

The bridge does not run arbitrary C# inside Juno.

VizzyCode still produces Vizzy XML. The mod only moves that XML between the running game and external tools.

The bridge also does not make unsupported Vizzy XML valid. If the exporter creates a bad block shape, Juno can still reject it. That is why the desktop app, CLI, and VS Code extension all run export validation before saving or sending XML.

## Current Tested Behavior

The bridge has been smoke-tested while Juno was running in the designer:

- `GET /status` returned `scene=designer`
- `GET /craft` returned the current craft name and part list
- VizzyCode detected at least one part with `hasVizzy=true`

This confirms the bridge can see the active designer craft and can identify Vizzy-bearing parts.

## Future Expansion

Reasonable future additions:

- active craft telemetry snapshot
- active craft central-body id/name
- active craft body constants when available from the same runtime body object
- richer target metadata
- loaded/unloaded flags for tracked crafts
- craft structure summaries for loaded crafts
- explicit telemetry quality flags

Risky or unsupported claims:

- full real-time `FlightData` for every non-active craft
- guaranteed hidden autopilot mode introspection
- guaranteed programmatic nav-target switching
- built-in closest-approach or SOI prediction APIs unless verified in the installed game build

When adding new endpoints, document whether each field is official API data, reflected runtime data, or an inference.
