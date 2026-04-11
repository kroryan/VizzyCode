# Juno Live Bridge Guide

This guide documents the VizzyCode live bridge for Juno: New Origins.

The bridge consists of two pieces:

- a Juno mod running inside the game process (HTTP server on `http://127.0.0.1:7842/`)
- VizzyCode desktop app and VS Code extension commands that call the mod over localhost

---

## Scene and Context Reference

Understanding which scene you are in is the most important factor when interpreting bridge data.

### Scene values

| `scene` value | When it appears |
| --- | --- |
| `menu` | Main menu, loading screen, or any non-game scene |
| `designer` | Ship designer — craft is being built, not simulated |
| `flight` | Flight scene — craft is launched and being simulated |

### Telemetry quality values

These appear in `GET /telemetry` and `GET /snapshot` responses.

| `quality` value | What it means |
| --- | --- |
| `no_craft` | No craft is loaded (menu, or designer without a craft) |
| `designer_static` | In the designer — structural data only, no physics |
| `flight_no_flightdata` | In flight but `ICraftFlightData` is not available (tracked/unloaded craft) |
| `flight_active_full` | In flight with full active physics data — use this for real telemetry |

---

## Endpoint Availability by Scene

This table shows what each endpoint can return in each scene.

| Endpoint | menu | designer | flight |
| --- | --- | --- | --- |
| `GET /status` | ✓ scene=menu | ✓ scene=designer | ✓ scene=flight |
| `GET /craft` | ✗ no craft | ✓ static structure | ✓ active structure |
| `GET /craft/all` | ✗ | ✗ | ✓ all loaded craft nodes |
| `GET /snapshot` | partial (no craft) | ✓ structure only | ✓ full |
| `GET /telemetry` | partial (no_craft) | partial (designer_static) | ✓ flight_active_full |
| `GET /telemetry/full` | partial | partial | ✓ with extended fields |
| `GET /parts/{id}` | ✗ | ✓ | ✓ |
| `GET /vizzy/{id}` | ✗ | ✓ read Vizzy XML | ✓ read Vizzy XML |
| `PUT /vizzy/{id}` | ✗ | ✓ write + reload | ✓ write live |
| `GET /stages` | ✗ | ✓ static staging | ✓ live staging |
| `POST /stages/activate` | ✗ | ✗ | ✓ live action |
| `GET /planets` | ✗ | ✗ | ✓ solar system tree |
| `GET /planets/{name}` | ✗ | ✗ | ✓ planet + atmosphere + orbit |
| `GET /universe/time` | ✗ | ✗ | ✓ universal + real time |
| `GET /biome` | ✗ | ✗ | ✓ biome at craft position |

---

## Telemetry Field Availability by Flight State

### Structural craft data — available in designer and flight

These fields come from `CraftData` and `Assembly`. They are always valid when a craft is loaded.

| Field | Designer | Launchpad / ground | Orbit | Landed away |
| --- | --- | --- | --- | --- |
| `craft.name` | ✓ | ✓ | ✓ | ✓ |
| `craft.partCount` | ✓ | ✓ | ✓ | ✓ |
| `craft.parts[].id / name / type` | ✓ | ✓ | ✓ | ✓ |
| `craft.parts[].hasVizzy` | ✓ | ✓ | ✓ | ✓ |
| `craft.parts[].activationStage/Group` | ✓ | ✓ | ✓ | ✓ |
| `craft.parts[].mass / dryMass / wetMass` | ✓ | ✓ | ✓ | ✓ |
| `craft.parts[].position / rotation` | ✓ (local) | ✓ | ✓ | ✓ |
| `craft.parts[].modifiers` | ✓ | ✓ | ✓ | ✓ |
| `stages.*` | ✓ static | ✓ | ✓ | ✓ |

### Flight telemetry — only available in flight scene with active FlightData

These fields come from `ICraftFlightData` and are only populated when `quality=flight_active_full`.

| Field | Designer | Launchpad | Ascent | Orbit | Landing |
| --- | --- | --- | --- | --- | --- |
| `altitudeASL` | 0 | ✓ | ✓ | ✓ | ✓ |
| `altitudeAGL` | 0 | ✓ (≈0) | ✓ | ✓ | ✓ |
| `altitudeTerrain` | 0 | ✓ (≈0) | ✓ | large | ✓ |
| `velocityMagnitude` | 0 | ✓ (≈0) | ✓ | ✓ | ✓ |
| `surfaceVelocityMagnitude` | 0 | ✓ | ✓ | ✓ | ✓ |
| `verticalSurfaceVelocity` | 0 | ✓ | ✓ | small | ✓ |
| `lateralSurfaceVelocity` | 0 | ✓ | ✓ | ✓ | ✓ |
| `accelerationMagnitude` | 0 | ✓ | ✓ | small | ✓ |
| `machNumber` | 0 | ✓ (0 on pad) | ✓ | 0 (no atm) | ✓ |
| `pitch / heading / bankAngle` | 0 | ✓ | ✓ | ✓ | ✓ |
| `angleOfAttack / sideSlip` | 0 | ✓ | ✓ | 0 (no atm) | ✓ |
| `grounded` | 0 | true | false | false | true |
| `inWater` | 0 | depends | false | false | depends |
| `currentMassKg / fuelMass` | 0 | ✓ | ✓ | ✓ | ✓ |
| `remainingFuelInStage` | 0 | ✓ | ✓ | ✓ | ✓ |
| `remainingBattery / Monopropellant` | 0 | ✓ | ✓ | ✓ | ✓ |
| `currentEngineThrustN / maxActiveEngineThrustN` | 0 | ✓ | ✓ | ✓ | ✓ |
| `gravityMagnitude` | 0 | ✓ | ✓ | ✓ | ✓ |
| `solarRadiationIntensity` | 0 | ✓ | ✓ | ✓ | ✓ |
| `parentPlanetOcclusion` | 0 | ✓ | ✓ | ✓ | ✓ |
| `position / velocity / acceleration` | 0 | ✓ | ✓ | ✓ | ✓ |
| `gravity vector` | 0 | ✓ | ✓ | ✓ | ✓ |
| `craftForward / craftUp / craftRight` | 0 | ✓ | ✓ | ✓ | ✓ |
| `orbit.*` | null | null | orbit appears > 70 km | ✓ | null |
| `performance.*` (deltaV, TWR, ISP) | null | ✓ | ✓ | ✓ | ✓ |
| `navTarget.*` | null | only if set | only if set | only if set | only if set |

> **Note on orbit data**: orbit fields only have meaningful values when the craft is on a Keplerian trajectory — roughly above 70 km for Droo. On the launchpad or during low-altitude flight, `orbit` may be null or contain zeroes.

### Extended telemetry — only in `GET /telemetry/full`

These fields are additional and only appear in the `/telemetry/full` response.

| Field | Designer | Launchpad | Orbit | Notes |
| --- | --- | --- | --- | --- |
| `latDeg / lonDeg` | 0 | ✓ | ✓ | Latitude/longitude in degrees |
| `angularVelocityMagnitude` | 0 | ✓ | ✓ | Rotation rate of craft (rad/s) |
| `angularVelocity` (vector) | 0 | ✓ | ✓ | Angular velocity vector |
| `dragAccelerationMagnitude` | 0 | ✓ | 0 in vacuum | Only meaningful in atmosphere |
| `north / east` (vectors) | 0 | ✓ | ✓ | Cardinal direction vectors in planet frame |
| `solarRadiationDirection` | 0 | ✓ | ✓ | Direction toward sun |
| `positionNormalized` | 0 | ✓ | ✓ | Unit position vector from planet center |
| `universalTime` | 0 | ✓ | ✓ | Time since universe creation (s) |
| `atmosphereSample.density` | 0 | ✓ | 0 in vacuum | kg/m³ at craft position |
| `atmosphereSample.pressure` | 0 | ✓ | 0 in vacuum | Pa at craft position |
| `atmosphereSample.temperature` | 0 | ✓ | 0 in vacuum | K at craft position |
| `atmosphereSample.speedOfSound` | 0 | ✓ | 0 in vacuum | m/s at craft position |

> **Use case — atmosphere entry**: `atmosphereSample.density` and `.pressure` are the correct fields for detecting atmospheric entry when writing Vizzy. More reliable than `machNumber > 0` alone.

---

## When to Use Each Data Source — Mission Phase Guide

This section answers the practical question: **which endpoints should you query, and when?**

It is intended for both human users writing Vizzy manually and for AI assistants generating Vizzy from a live bridge snapshot.

---

### Phase 0 — Designer (craft not yet launched)

The craft is being assembled. There is no simulation running. All physics values are zero.

**What is reliable:**
- `GET /craft` — full part list, mass, modifiers, staging, connection graph
- `GET /stages` — staging configuration, activation groups
- `GET /vizzy/{id}` — read existing Vizzy XML from any part
- `PUT /vizzy/{id}` — write Vizzy XML to a part before launch
- `GET /snapshot` — craft structure snapshot (quality = `designer_static`)

**What is misleading (do not use for logic):**
- Any field from `/telemetry` or `/telemetry/full` — all return zeros. This does **not** mean the craft is at altitude 0 on Droo. It means there is no simulation running.
- `orbit.*` — always null or zero in the designer.
- `performance.*` — zeros. Wait until the craft is in flight.

**Typical AI workflow in the designer:**

1. Call `GET /craft` → inspect parts, find which part has `hasVizzy: true`.
2. Call `GET /vizzy/{id}` for that part → read the existing program.
3. Generate or modify the Vizzy XML.
4. Call `PUT /vizzy/{id}` → inject the updated program.

Do **not** read `/telemetry` to decide anything about the craft's flight state while in the designer.

---

### Phase 1 — Launchpad / Pre-launch (craft is in flight scene, `grounded: true`)

The craft is sitting on the pad. Physics is active but the craft has not moved. This is the earliest moment where real telemetry becomes valid.

**What is reliable:**
- Everything from the designer phase above.
- `altitudeASL`, `altitudeAGL` — real values, typically 0–small number (height of the pad).
- `currentMassKg`, `fuelMass`, `remainingFuelInStage` — fuel tanks are full.
- `pitch`, `heading`, `bankAngle` — craft orientation on the pad.
- `gravityMagnitude` — valid surface gravity at the launch site.
- `performance.deltaV`, `performance.thrustToWeightRatio`, `performance.currentIsp` — these are meaningful before launch.
- `GET /telemetry/full` → `latDeg`, `lonDeg` — launch site coordinates.
- `GET /telemetry/full` → `atmosphereSample.*` — sea-level atmosphere (full pressure and density).
- `grounded: true` — confirms the craft is on the ground.

**What is not yet available:**
- `orbit.*` — null or zero. No orbit exists until the craft reaches a ballistic trajectory.
- `velocityMagnitude`, `surfaceVelocityMagnitude` — zero (craft is stationary).
- `machNumber` — zero (not moving).
- `GET /biome` — returns the biome at the launch site. Valid here.

**Typical use case:**

Use the launchpad snapshot to pre-calculate gravity turn parameters, TWR margin, and expected delta-v before the flight program starts.

---

### Phase 2 — Atmospheric Ascent (craft is climbing, still in atmosphere)

The craft has cleared the pad and is under thrust through the atmosphere.

**What is reliable and mission-critical:**
- `velocityMagnitude`, `surfaceVelocityMagnitude` — real velocity.
- `verticalSurfaceVelocity`, `lateralSurfaceVelocity` — vector decomposition for gravity turn logic.
- `machNumber` — supersonic detection. Use for max-q monitoring.
- `angleOfAttack`, `sideSlip` — aerodynamic attitude.
- `accelerationMagnitude` — net acceleration (thrust minus drag minus gravity).
- `currentEngineThrustN`, `maxActiveEngineThrustN` — engine output.
- `remainingFuelInStage` — fuel depletion for stage trigger logic.
- `altitudeASL`, `altitudeAGL`, `altitudeTerrain` — current altitude.
- `performance.remainingBurnTime` — estimated time to stage fuel depletion.

**Extended fields from `/telemetry/full` that matter in ascent:**
- `atmosphereSample.density` — real-time air density. Use for drag-aware calculations.
- `atmosphereSample.pressure` — atmospheric pressure at current altitude.
- `atmosphereSample.speedOfSound` — divide into `velocityMagnitude` for exact Mach number.
- `dragAccelerationMagnitude` — current drag deceleration. Use for net thrust calculation.
- `angularVelocity` — craft rotation rate. Use for stability control.
- `craftForward`, `craftUp`, `craftRight` — attitude vectors for alignment burns.

**What is not yet available:**
- `orbit.*` — typically null below ~70 km on Droo. May appear as partial data near the Kármán line, but values are not meaningful until a stable orbit is established.

**Typical use case:**

Query `/telemetry/full` for real-time drag and atmosphere data when generating a gravity-turn or max-q limiter Vizzy program.

---

### Phase 3 — Orbit (craft is on a stable Keplerian trajectory)

The craft is coasting without thrust in vacuum. This is when orbital mechanics data becomes available and reliable.

**What is reliable and mission-critical:**
- `orbit.apoapsisAltitude`, `orbit.periapsisAltitude` — current apoapsis/periapsis. Use for burn timing.
- `orbit.eccentricity` — 0 = circular, >0 = elliptical, 1 = escape.
- `orbit.inclination` — orbital plane angle vs equatorial.
- `orbit.period` — orbital period in seconds.
- `orbit.trueAnomaly` — current position in orbit (0 = periapsis, 180 = apoapsis).
- `orbit.apoapsisTime`, `orbit.periapsisTime` — seconds until next apoapsis/periapsis.
- `orbit.burnNodeDeltaV`, `orbit.hasBurnNodePoint` — active maneuver node data.
- `performance.deltaVStage` — remaining delta-v in current stage.
- `performance.thrustToWeightRatio` — TWR (useful for burn duration calculation).
- `GET /planets/{name}` → `currentOrbit.sphereOfInfluence` — SOI boundary for intercept planning.
- `GET /universe/time` → `universalTime` — use for synchronizing timed maneuvers.
- `GET /craft/all` — all other craft nodes in the simulation.

**Extended fields from `/telemetry/full` that matter in orbit:**
- `latDeg`, `lonDeg` — ground track position (changes each orbit due to planet rotation).
- `positionNormalized` — unit vector from planet center to craft.
- `north`, `east` — local cardinal direction vectors. Use for prograde/retrograde burn directions.
- `solarRadiationDirection` — vector toward the sun. Use for solar panel orientation logic.

**What is zero in orbit (expected):**
- `machNumber` — zero in vacuum (no atmosphere).
- `dragAccelerationMagnitude` — zero in vacuum.
- `atmosphereSample.*` — all zero in vacuum.
- `angleOfAttack`, `sideSlip` — not meaningful in vacuum.

**Typical AI workflow in orbit:**

1. Call `GET /telemetry` → confirm `quality: flight_active_full` and `orbit` is not null.
2. Read `orbit.apoapsisAltitude`, `orbit.periapsisAltitude`, `orbit.period`.
3. Call `GET /planets/{parentName}` → get SOI, gravity, surface data for the target planet.
4. Call `GET /universe/time` → get current `universalTime` for burn timing.
5. Generate the circularization or transfer burn Vizzy using the above values.

---

### Phase 4 — Landing / Re-entry

The craft is descending back through the atmosphere or approaching a surface.

**What is reliable and mission-critical:**
- `altitudeASL`, `altitudeAGL`, `altitudeTerrain` — triple altitude reference. `altitudeTerrain` is most useful for terrain avoidance.
- `verticalSurfaceVelocity` — negative = descending. Use for retro-burn timing.
- `surfaceVelocityMagnitude` — total ground-relative speed.
- `machNumber` — re-entry heating phase detection.
- `atmosphereSample.density`, `atmosphereSample.pressure` — atmosphere entry detection. Reliable threshold: density > 0.001 kg/m³.
- `dragAccelerationMagnitude` — aerobraking effectiveness.
- `grounded` — use as landing confirmation trigger.
- `GET /biome` — landing biome. Useful for science or contract verification.

**Typical use case:**

Query `/telemetry/full` during re-entry to trigger chute deployment when `atmosphereSample.pressure > X` and `altitudeAGL < Y`. More reliable than checking `machNumber` alone because a craft entering a thin atmosphere (Cylero, Luna) can have low Mach but real aerodynamic forces.

---

### Phase 5 — Multi-Craft / Rendezvous

Two or more craft are in flight at the same time.

**What is available:**
- `GET /craft/all` → list of all tracked craft nodes with IDs, positions, parent planets.
- `navTarget.*` in `/telemetry` → distance, type, and name of the current navigation target.
- `GET /telemetry` → `craftNodeId` — the node ID of the currently tracked craft.

**Typical use case:**

Call `/craft/all` to get the node ID of the target vessel. Cross-reference against `navTarget.name`. Then generate the docking or proximity approach Vizzy using those values.

---

### Quick Reference — "I am generating Vizzy for X, what should I query?"

| Goal | Required data | Endpoint |
| --- | --- | --- |
| Inspect part layout / modifiers | Part list, mass, staging | `GET /craft` |
| Write/update Vizzy on a part | Part ID, existing XML | `GET /craft` → `GET /vizzy/{id}` → `PUT /vizzy/{id}` |
| Gravity turn / ascent program | Atmosphere, mass, TWR | `GET /telemetry/full` (on pad or in flight) |
| Max-q limiter | machNumber, atm pressure | `GET /telemetry/full` (ascent) |
| Circularization burn | Orbit params, deltaV | `GET /telemetry` (orbit) |
| Hohmann transfer timing | Orbit params, planet orbit | `GET /telemetry` + `GET /planets/{target}` |
| Landing burn trigger | altitudeAGL, vertical velocity | `GET /telemetry` (descent) |
| Chute deployment | Atmosphere density, pressure | `GET /telemetry/full` (re-entry) |
| Science / biome check | Biome, lat/lon | `GET /biome` |
| Timed maneuver | Universal time | `GET /universe/time` |
| Rendezvous approach | All craft positions | `GET /craft/all` |
| Re-entry heat shield | atmosphereSample.density | `GET /telemetry/full` |
| Planet SOI planning | SOI, gravity, atmosphere | `GET /planets/{name}` |

---

### Note for AI Assistants

When a user asks you to generate a Vizzy program using live bridge data:

1. **Always check `quality` first.** If `quality` is not `flight_active_full`, do not use telemetry values for calculations. Use `GET /craft` for part structure only.
2. **Never treat designer zeros as real values.** `altitudeASL: 0`, `velocityMagnitude: 0` in the designer does not mean the craft is on Droo at sea level — it means the simulation is not running.
3. **Use `/telemetry/full` instead of `/telemetry`** when you need lat/lon, atmosphere sample, angular velocity, or drag.
4. **Use `/snapshot`** when you need a single consistent bundle of craft + staging + telemetry captured at the same moment.
5. **Orbit data is null below ~70 km.** If `orbit` is null or `apoapsisAltitude: 0`, the craft has not reached a stable ballistic trajectory yet.
6. **`performance.*` is valid on the pad and in flight.** Delta-v and TWR are available as soon as the craft is in the flight scene.

---

## Planet Telemetry — `GET /planets` and `GET /planets/{name}`

Planet data is only available in the **flight scene**. It requires the solar system tree to be loaded.

### `GET /planets`

Returns a flat list of all bodies in the current solar system.

```json
{
  "ok": true,
  "planets": [
    {
      "name": "Droo",
      "parent": "Sun",
      "radius": 600000,
      "mass": 5.97e24,
      "surfaceGravity": 9.81,
      "escapeVelocity": 11186,
      "hasAtmosphere": true,
      "hasWater": true,
      "childCount": 1
    }
  ]
}
```

### `GET /planets/{name}`

Returns full data for a single planet by name (case-insensitive).

**Static physical data** (always valid, same as in-game wiki values):

| Field | Description |
| --- | --- |
| `radius` | Planet radius in meters |
| `mass` | Planet mass in kg |
| `surfaceGravity` | Surface gravitational acceleration (m/s²) |
| `escapeVelocity` | Escape velocity from surface (m/s) |
| `angularVelocity` | Rotation rate in rad/s |
| `impactRadius` | Collision detection radius in meters |
| `hasWater` | Has ocean |
| `hasTerrainPhysics` | Has terrain collision |
| `maxTerrainElevation` | Estimated max terrain height above sea level (m) |
| `children[]` | Names of moons or child bodies |

**Atmosphere data** (under `atmosphere`):

| Field | Description |
| --- | --- |
| `hasAtmosphere` | Has physical atmosphere |
| `height` | Atmosphere ceiling in meters |
| `scaleHeight` | Exponential scale height (m) — used for density calculation |
| `surfaceAirDensity` | Air density at sea level (kg/m³) |
| `meanSurfaceTemperature` | Mean surface temperature in Kelvin |
| `meanSurfaceTemperatureDay` | Daytime surface temperature in Kelvin |
| `meanSurfaceTemperatureNight` | Nighttime surface temperature in Kelvin |
| `meanMassPerMolecule` | Molecular mass of atmosphere |
| `meanGamma` | Specific heat ratio γ (used for speed of sound) |
| `crushAltitude` | Altitude where atmospheric pressure crushes unprotected craft |

> **Use case — atmosphere planning**: before writing a re-entry Vizzy, query `GET /planets/Droo` and note `atmosphere.height` (entry threshold), `surfaceAirDensity` (drag reference at sea level), and `scaleHeight` (how fast density drops with altitude). These values feed directly into the `CraftProperty.AtmosphereDensity` Vizzy block.

**Current orbital position** (under `currentOrbit`, live — changes every frame):

| Field | Description |
| --- | --- |
| `solarPosition` | 3D position of the planet relative to the sun (m) |
| `solarVelocity` | Velocity relative to the sun (m/s) |
| `sphereOfInfluence` | SOI radius (m) — boundary where this body dominates gravity |
| `apoapsis / periapsis` | Orbital altitude extremes (m) |
| `eccentricity` | Orbital eccentricity |
| `inclination` | Orbital inclination (deg) |
| `period` | Orbital period (s) |
| `semiMajorAxis` | Semi-major axis (m) |
| `trueAnomaly` | Current position in orbit (deg) |

> **Use case — interplanetary transfer**: query `GET /planets/Luna` → `currentOrbit.trueAnomaly` and `solarPosition` to compute phase angle relative to your craft's current position before writing a Hohmann transfer Vizzy.

---

## Universe Time — `GET /universe/time`

Only available in the flight scene.

| Field | Description |
| --- | --- |
| `universalTime` | Time since the universe was created (s) — consistent across warp |
| `timeReal` | Real time elapsed since flight scene start (s) |
| `timeDelta` | Last frame delta time (s) |
| `timeMultiplier` | Current warp rate (1 = real time, >1 = warp) |
| `timeSinceLaunch` | Time since craft was launched (s) |

> Use `universalTime` for any Vizzy that needs deterministic time references across warp. `timeReal` resets each flight scene.

---

## All Craft Nodes — `GET /craft/all`

Only available in the flight scene. Returns a summary of every craft node currently tracked by the simulation.

| Field | Description |
| --- | --- |
| `nodeId` | Unique craft node ID |
| `name` | Craft name |
| `isPlayer` | Is this the player's active craft |
| `altitude` | Altitude ASL (m) |
| `altitudeAgl` | Altitude AGL (m) |
| `parentPlanet` | Name of the planet/body this craft is orbiting |
| `partCount` | Total part count |
| `hasCommandPod` | Has an active command pod |
| `latDeg / lonDeg` | Latitude and longitude in degrees |
| `surfaceVelocity` | Surface velocity vector |

> **Use case**: before writing a rendezvous or multi-craft Vizzy, query `/craft/all` to get the exact IDs and positions of all nearby craft.

---

## Biome — `GET /biome`

Only available in the flight scene. Returns the biome at the current craft surface position.

```json
{
  "ok": true,
  "biome": "Grasslands",
  "planet": "Droo",
  "latDeg": 0.5,
  "lonDeg": -156.3
}
```

> **Use case**: career contract verification — check if the craft is in the required biome before triggering a science event in Vizzy.

---

## Report Quality Reference

The Markdown reports generated by the standalone app and VS Code extension include a quality warning at the top when telemetry is not from an active flight.

| Report scenario | What you get |
| --- | --- |
| Snapshot in designer | Full craft structure, staging, static part data. Telemetry shows designer_static warning. |
| Snapshot in flight (on pad) | Full craft + staging + active telemetry. Orbit section may be null below 70 km. |
| Snapshot in flight (orbit) | Full craft + staging + telemetry + orbit. Performance section has active delta-v. |
| Telemetry report only | No craft/parts section. Use Craft Snapshot report for the full part listing. |
| No craft loaded | Report shows "no_craft" quality warning. No part data. |

---

## Endpoint Reference Summary

| Endpoint | Method | Available in | Returns |
| --- | --- | --- | --- |
| `/status` | GET | all | bridge alive, scene, craft name |
| `/craft` | GET | designer, flight | full craft structure |
| `/craft/all` | GET | flight | all tracked craft nodes |
| `/snapshot` | GET | designer, flight | craft + telemetry + stages bundle |
| `/telemetry` | GET | all | flight telemetry (quality-flagged) |
| `/telemetry/full` | GET | all | telemetry + lat/lon + atm + angular + solar |
| `/parts/{id}` | GET | designer, flight | single part full data |
| `/vizzy/{id}` | GET | designer, flight | raw Vizzy XML from part |
| `/vizzy/{id}` | PUT | designer, flight | write Vizzy XML to part |
| `/stages` | GET | designer, flight | staging + activation groups |
| `/stages/activate` | POST | flight | activate next stage |
| `/planets` | GET | flight | all planets summary list |
| `/planets/{name}` | GET | flight | planet static + atmosphere + live orbit |
| `/universe/time` | GET | flight | universal time, delta, warp rate |
| `/biome` | GET | flight | biome at craft position |

---

## Official Modding Basis

The bridge follows the normal Juno desktop modding model:

- Mods use the public `ModApi` surface — `ModApi.Common.Game`, `ICraftScript`, `ICraftFlightData`, `IPlanetData`, `IOrbitNode`, `IPlanetNode`, `IFlightState`, `IFlightScene`.
- The bridge deliberately avoids custom in-game UI. All functionality runs through the HTTP service.
- Main-thread execution is required for all Unity/game-object access. The bridge queues all game operations and flushes them from `MonoBehaviour.Update()`.

Useful official references:

- https://www.simplerockets.com/Forums/View/188748/How-to-make-mods
- https://www.simplerockets.com/r/ModApi
- https://www.simplerockets.com/Forums/View/32289/
- https://www.simplerockets.com/Forums/View/32308/

---

## Safety Rules

- Keep a craft backup before live-exporting Vizzy to the running game.
- Treat live export as a convenience layer, not as permanent storage.
- Do not skip the `*.vizzy.meta.json` sidecar for imported programs.
- Do not export to a different part unless you intentionally want to replace that part's Vizzy.
- Do not use `POST /stages/activate` during a mission-critical flight phase.
- Planet and telemetry data are read-only. There is no live flight control or physics injection API.

---

## Threading Model

The bridge receives HTTP requests on background threads. All game objects must be touched on Unity's main thread.

The mod queues operations and flushes them from `MonoBehaviour.Update()`. Any endpoint that accesses `Game.Instance`, craft scripts, part data, planet nodes, or flight state must run inside this queue.

---

## What The Bridge Is Not

- It does not execute arbitrary C# inside Juno.
- It does not make invalid Vizzy XML valid — always run export validation before sending XML.
- It does not run Vizzy programs standalone or headlessly. Vizzy requires a craft part in the flight scene.
- It does not control flight inputs, throttle, or navball heading. It is read-only for telemetry and planets, read-write for Vizzy XML only.
