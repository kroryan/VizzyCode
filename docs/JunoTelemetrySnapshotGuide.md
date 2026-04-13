# Juno Telemetry And Craft Snapshot Guide

This guide documents the expanded VizzyCode live bridge telemetry layer.

The purpose is practical authoring context. A human or AI agent should be able to inspect the craft that will receive a Vizzy program before changing the script, then capture runtime telemetry while the program is running to diagnose what failed.

## Research Basis

The expansion is based on:

- Juno's official desktop modding model, described in the official "How to make mods?" forum post: `https://www.simplerockets.com/Forums/View/188748/How-to-make-mods`.
- The official `ModApi` reference exposed at `https://www.simplerockets.com/r/ModApi`.
- Local ModTools XML documentation bundled in `Mod Assets/ModTools/Assemblies`.
- The public API shape documented for `ICraftScript`, `ICraftFlightData`, `ICraftNode`, `CraftData`, `Assembly`, `PartData`, `PartModifierData`, `ActivationStage`, and `ICommandPodScript`.

Important official API facts used by the implementation:

- `ICraftScript` exposes the active command pod, command pods, craft data, root part, mass, center of mass, gravity, surface velocity, physics state, and `FlightData`.
- `ICraftFlightData` exposes active-craft flight data such as altitude, velocity, acceleration, attitude, fuel, battery, thrust, gravity, atmosphere, solar radiation, orbit data, performance data, and nav-sphere target.
- `ICraftNode` exposes lower-level craft node metadata such as node id, loaded craft script, parent/reference data, part count, command pod presence, contact state, heading, mass, and coarse position/velocity.
- `CraftData.Assembly` exposes parts, bodies, and part connections.
- `PartData` exposes id, name, tag, type, activation stage/group, command pod id, enabled/activated state, mass, dry/wet mass, price, position, rotation, connections, and modifiers.
- `PartModifierData` exposes type id, name, id, input id, mass, price, scale, designer/inspector visibility, and associated activation groups.

## Why This Matters For VizzyCode

Vizzy authoring is not just syntax conversion.

Real Vizzy programs often depend on craft-specific facts:

- exact part names used by `Part.NameToID`
- which command pod controls the craft
- which parts have a Vizzy program
- activation group layout
- stage ordering
- engine/fuel/power modifiers
- wet and dry mass
- current orbit and body
- current velocity frame
- burn time, delta-v, and thrust-to-weight
- whether the craft is in designer, loaded flight, or only represented by coarse node data

Without this context, an AI may write valid-looking code that targets nonexistent parts, stages the wrong hardware, assumes the wrong body, or uses flight-only telemetry while the craft is still in the designer.

## Endpoints

All endpoints are local-only:

```text
http://127.0.0.1:7842/
```

### `GET /snapshot`

Returns the main AI/human context bundle.

It includes:

- bridge/mod version
- scene
- craft name
- capture timestamp
- full craft structure summary
- stage and activation group state
- telemetry snapshot when available
- notes describing data quality and limitations

Use this before asking an AI to write or repair a Vizzy mission for the current craft.

### Human/AI-readable reports

The raw `/snapshot` and `/telemetry` JSON payloads are the exact machine-readable source data, but they are not the best first document for humans or AI agents.

VizzyCode now provides explicit Markdown report exports:

- `Save Full Craft Report for Humans/AI`
- `Save Telemetry Report for Humans/AI`
- `VizzyCode: Save Full Juno Craft Report for Humans/AI`
- `VizzyCode: Save Juno Telemetry Report for Humans/AI`

These reports are the recommended readable context format. They summarize the raw bridge payload into sections for craft identity, telemetry quality, live telemetry, orbit, performance, nav target, stages, activation groups, Vizzy-capable parts, practical part roles, and all parts.

Use the report first. Keep the JSON beside it when exact values, vectors, or fields not listed in the report are needed.

For AI agents:

- If the agent can use the standalone app or local files, give it the saved `*.report.md`, the matching raw `*.json`, the `.vizzy.cs`, and all docs.
- If the agent cannot trigger VS Code commands directly, the user should generate the report from the app or extension first, then provide the report file.
- For conversion work, prefer the standalone `VizzyCode.Cli.exe` because it is directly callable by automation agents. The VS Code extension is mainly for interactive human use.
- Tell the AI that the report is the readable source of truth, while JSON is only the raw exact backup.

### `GET /telemetry`

Returns the current active-craft telemetry snapshot.

In the designer this is intentionally limited and marked as `designer_static`.

In flight, when `ICraftFlightData` is available, it is marked as `flight_active_full` and includes:

- altitude ASL/AGL/terrain
- velocity, surface velocity, vertical and lateral surface velocity
- acceleration and gravity
- Mach number
- attitude: pitch, heading, bank angle, angle of attack, sideslip
- mass, fuel, battery, monopropellant
- current and maximum active engine thrust
- RCS thrust
- grounded/in-water state
- solar radiation and parent-planet occlusion
- active orbit fields
- performance fields
- nav target metadata when exposed

### `GET /craft`

Returns an expanded static craft model.

It includes:

- craft name and XML version
- part count, body count, connection count
- root part id
- active command pod id
- local center of mass
- bodies
- part connections
- parts with expanded part metadata
- modifiers per part

The original fields `name`, `parts`, `id`, `name`, and `hasVizzy` remain present for backward compatibility.

### `GET /parts/{partId}`

Returns the same expanded metadata for a single part.

Use this when a Vizzy program references a specific part name or id and you need to inspect:

- exact part name
- part type
- activation stage/group
- command pod id
- body id
- mass and price
- connected part ids
- modifiers
- whether the part has `FlightProgramData`

### `GET /stages`

Returns:

- current stage
- number of stages
- activation group names
- activation group states
- stage grouping derived from part `ActivationStage`
- stage part ids and names

This makes it easier to understand what a stage action will affect before a Vizzy script activates it.

## Data Quality Flags

Every telemetry response includes quality fields.

Use them before trusting the numbers:

- `activeFullFlightData=true` means data came from the active craft's `ICraftFlightData`.
- `trackedFallback=true` means only lower-quality craft node/static data was available.
- `quality="designer_static"` means the craft structure is visible, but flight telemetry is not.
- `quality="flight_active_full"` means live active-craft telemetry is available.
- `quality="flight_no_flightdata"` means the craft exists in flight but full `FlightData` was not available.
- `quality="no_craft"` means no craft is loaded.

AI agents must not treat fallback/static data as full per-frame flight data.

## Recommended AI Workflow

Before authoring:

1. Start Juno with the VizzyCode mod enabled.
2. Load the craft in the designer.
3. Save a full craft report for humans/AI from the app or VS Code.
4. Optionally save the matching full snapshot JSON for exact raw fields.
5. Give the AI the report, the JSON when needed, `README.md`, all docs, the current `.vizzy.cs`, and any original working XML.
6. Ask the AI to target real part names, stages, activation groups, and available telemetry fields from the report.

During runtime debugging:

1. Export the Vizzy program to the running game.
2. Run the mission until it fails or reaches the suspect state.
3. Save a telemetry report for humans/AI.
4. Optionally save telemetry JSON for exact raw values.
5. Compare the Vizzy state assumptions against real altitude, apoapsis/periapsis, fuel, stage, target, mass, thrust, and body data.
6. Fix the script based on real craft behavior, not guessed craft behavior.

## Safe Boundaries

The bridge should keep these boundaries:

- Prefer public `ModApi` data first.
- Use reflection only for optional fields that may not exist in every Juno build.
- Never claim full flight data for non-active or unloaded crafts unless verified.
- Do not mutate craft structure from telemetry endpoints.
- Keep live actions separate from read-only diagnostics.
- Keep localhost binding at `127.0.0.1`.
- Include explicit quality flags whenever a value may be partial, stale, inferred, or unavailable.

## What To Add Next

The next useful additions are:

- `/crafts` for all tracked crafts, with loaded/unloaded and quality flags.
- `/parts/{id}/xml` to capture generated part XML for exact modifier/state inspection.
- `/program/log` if Vizzy runtime logs can be exposed safely.
- time-warp state and capability metadata.
- nav-target richer classification for craft/body/part/landmark/position targets.
- body constants for active parent body and selected body target when the runtime body object exposes them.
- repeated telemetry capture from the app/extension, producing a time series JSON for mission debugging.

Do not add hidden autopilot, target switching, or non-player full flight telemetry unless the exact API is verified in the installed Juno build.
