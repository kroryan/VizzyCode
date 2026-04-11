const vscode = require('vscode');
const cp = require('child_process');
const fs = require('fs');
const path = require('path');
const http = require('http');

const JUNO_PORT = 7842;
const JUNO_BASE = `http://127.0.0.1:${JUNO_PORT}`;

let extensionContext = null;
let statusBar = null;        // main status bar item
let junoStatusBar = null;    // Juno connection indicator

function activate(context) {
  extensionContext = context;
  vscode.commands.executeCommand('setContext', 'vizzycode.active', true);

  context.subscriptions.push(
    vscode.commands.registerCommand('vizzycode.importXmlToCode', async (uri) => {
      await safeRun(() => runImport(uri));
    }),
    vscode.commands.registerCommand('vizzycode.exportCodeToXml', async (uri) => {
      await safeRun(() => runExport(uri));
    }),
    vscode.commands.registerCommand('vizzycode.roundTripXml', async (uri) => {
      await safeRun(() => runRoundTrip(uri));
    }),

    // ── Juno commands ───────────────────────────────────────────────────────
    vscode.commands.registerCommand('vizzycode.junoConnect', async () => {
      await safeRun(() => junoConnect());
    }),
    vscode.commands.registerCommand('vizzycode.junoImportFromGame', async (uri) => {
      await safeRun(() => junoImportFromGame(uri));
    }),
    vscode.commands.registerCommand('vizzycode.junoExportToGame', async (uri) => {
      await safeRun(() => junoExportToGame(uri));
    }),
    vscode.commands.registerCommand('vizzycode.junoBrowseParts', async () => {
      await safeRun(() => junoBrowseParts());
    }),
    vscode.commands.registerCommand('vizzycode.junoViewStages', async () => {
      await safeRun(() => junoViewStages());
    }),
    vscode.commands.registerCommand('vizzycode.junoSaveTelemetry', async () => {
      await safeRun(() => junoSaveJson('telemetry', '/telemetry'));
    }),
    vscode.commands.registerCommand('vizzycode.junoSaveSnapshot', async () => {
      await safeRun(() => junoSaveJson('snapshot', '/snapshot'));
    }),
    vscode.commands.registerCommand('vizzycode.junoSaveTelemetryReport', async () => {
      await safeRun(() => junoSaveReport('telemetry', '/telemetry'));
    }),
    vscode.commands.registerCommand('vizzycode.junoSaveSnapshotReport', async () => {
      await safeRun(() => junoSaveReport('snapshot', '/snapshot'));
    })
  );

  // Main status bar item
  statusBar = vscode.window.createStatusBarItem(vscode.StatusBarAlignment.Left, 100);
  statusBar.name    = 'VizzyCode';
  statusBar.text    = '$(code) VizzyCode';
  statusBar.tooltip = 'VizzyCode — import/export Vizzy';
  statusBar.command = 'vizzycode.importXmlToCode';
  statusBar.show();
  context.subscriptions.push(statusBar);

  // Juno status bar item
  junoStatusBar = vscode.window.createStatusBarItem(vscode.StatusBarAlignment.Left, 99);
  junoStatusBar.name    = 'VizzyCode Juno';
  junoStatusBar.text    = '$(circle-slash) Juno';
  junoStatusBar.tooltip = 'Click to connect to Juno: New Origins';
  junoStatusBar.command = 'vizzycode.junoConnect';
  junoStatusBar.show();
  context.subscriptions.push(junoStatusBar);

  // Auto-check connection on startup (silent)
  junoGet('/status').then(setJunoStatusBar).catch(() => {});
}

function deactivate() {}

// ── VizzyCode CLI commands ─────────────────────────────────────────────────

async function runImport(uri) {
  const input = await resolveInputUri(uri, 'xml');
  if (!input) return;
  const output = input.fsPath.replace(/\.xml$/i, '.vizzy.cs');
  await runCli(['import', input.fsPath, '-o', output], path.dirname(input.fsPath));
  await maybeOpen(output);
}

async function runExport(uri) {
  const input = await resolveInputUri(uri, 'code');
  if (!input) return;
  let output = input.fsPath;
  if (/\.vizzy\.cs$/i.test(output)) output = output.replace(/\.vizzy\.cs$/i, '.xml');
  else output = output.replace(/\.cs$/i, '.xml');
  await runCli(['export', input.fsPath, '-o', output], path.dirname(input.fsPath));
  await maybeOpen(output);
}

async function runRoundTrip(uri) {
  const input = await resolveInputUri(uri, 'xml');
  if (!input) return;
  const dir   = path.dirname(input.fsPath);
  const stem  = path.basename(input.fsPath, path.extname(input.fsPath));
  const output = path.join(dir, `${stem}.roundtrip.xml`);
  const code   = path.join(dir, `${stem}.roundtrip.vizzy.cs`);
  await runCli(['roundtrip', input.fsPath, '-o', output, '-c', code], dir);
  await maybeOpen(code);
}

// ── Juno integration commands ──────────────────────────────────────────────

async function junoConnect() {
  const status = await junoGet('/status');
  setJunoStatusBar(status);
  if (!status) {
    vscode.window.showWarningMessage(
      `Cannot reach VizzyCode mod at ${JUNO_BASE}. ` +
      'Make sure Juno: New Origins is running with the mod installed.'
    );
    return;
  }
  const scene = status.scene || 'unknown';
  const craft = status.craftName ? ` · ${status.craftName}` : '';
  vscode.window.showInformationMessage(
    `Connected to Juno! Scene: ${scene}${craft}  (mod v${status.modVersion})`
  );
}

async function junoImportFromGame(uri) {
  // 1. Ask the user which part to import (show quick pick from /craft)
  const part = await pickVizzyPart();
  if (!part) return;

  // 2. Fetch the Vizzy XML from the game
  const resp = await junoGet(`/vizzy/${part.id}`);
  if (!resp || !resp.ok) {
    const err = resp ? resp.error : 'Connection failed';
    vscode.window.showErrorMessage(`Import failed: ${err}`);
    return;
  }

  // 3. Determine where to save the .vizzy.cs file
  const workspaceFolder = vscode.workspace.workspaceFolders?.[0]?.uri.fsPath;
  const defaultDir = workspaceFolder || require('os').homedir();
  const safeName = (part.name || `part_${part.id}`).replace(/[^a-zA-Z0-9_\-. ]/g, '_');

  const saveUri = await vscode.window.showSaveDialog({
    defaultUri: vscode.Uri.file(path.join(defaultDir, `${safeName}.vizzy.cs`)),
    filters: { 'Vizzy Code': ['vizzy.cs', 'cs'] }
  });
  if (!saveUri) return;

  // 4. Save raw XML first, then run CLI import to get the .vizzy.cs
  const tmpXml = saveUri.fsPath.replace(/\.vizzy\.cs$/i, '.xml');
  fs.writeFileSync(tmpXml, resp.xml, 'utf8');

  await runCli(['import', tmpXml, '-o', saveUri.fsPath], path.dirname(saveUri.fsPath));
  await maybeOpen(saveUri.fsPath);

  vscode.window.showInformationMessage(
    `Imported Vizzy from Juno — part: ${part.name} (id ${part.id})`
  );
}

async function junoExportToGame(uri) {
  // 1. Resolve .cs / .vizzy.cs file to export
  const input = await resolveInputUri(uri, 'code');
  if (!input) return;

  // 2. Convert to XML using CLI
  const tmpXml = input.fsPath.replace(/\.vizzy\.cs$/i, '._juno_export.xml')
                              .replace(/\.cs$/i, '._juno_export.xml');
  await runCli(['export', input.fsPath, '-o', tmpXml], path.dirname(input.fsPath));

  if (!fs.existsSync(tmpXml)) {
    vscode.window.showErrorMessage('Export failed: XML file was not created.');
    return;
  }

  const xml = fs.readFileSync(tmpXml, 'utf8');
  fs.unlinkSync(tmpXml);   // clean up temp file

  // 3. Pick the part to push to
  const part = await pickVizzyPart();
  if (!part) return;

  // 4. PUT to the game
  const result = await junoPut(`/vizzy/${part.id}`, { xml });
  if (!result || !result.ok) {
    const err = result ? result.error : 'Connection failed';
    vscode.window.showErrorMessage(`Export to Juno failed: ${err}`);
    return;
  }

  setJunoStatusBar({ scene: 'active', craftName: `Exported → ${result.partName}` });
  vscode.window.showInformationMessage(
    `Vizzy exported to Juno — part: ${result.partName} (id ${result.partId})`
  );
}

async function junoBrowseParts() {
  const craft = await junoGet('/craft');
  if (!craft) {
    vscode.window.showWarningMessage('Cannot reach the mod. Is Juno running?');
    return;
  }

  const parts = (craft.parts || []).filter(p => p.hasVizzy);
  if (parts.length === 0) {
    vscode.window.showInformationMessage('No parts with Vizzy programs in the current craft.');
    return;
  }

  const items = parts.map(p => ({
    label:       `$(circuit-board) ${p.name}`,
    description: `id: ${p.id}`,
    id:          p.id,
    name:        p.name
  }));

  const picked = await vscode.window.showQuickPick(items, {
    placeHolder: `${craft.name} — pick a part to import`
  });
  if (!picked) return;

  // Trigger import for the selected part
  const resp = await junoGet(`/vizzy/${picked.id}`);
  if (!resp || !resp.ok) {
    vscode.window.showErrorMessage(`Could not fetch Vizzy for part ${picked.name}: ${resp?.error}`);
    return;
  }

  const workspaceFolder = vscode.workspace.workspaceFolders?.[0]?.uri.fsPath;
  const defaultDir = workspaceFolder || require('os').homedir();
  const safeName   = picked.name.replace(/[^a-zA-Z0-9_\-. ]/g, '_');

  const saveUri = await vscode.window.showSaveDialog({
    defaultUri: vscode.Uri.file(path.join(defaultDir, `${safeName}.vizzy.cs`)),
    filters:    { 'Vizzy Code': ['vizzy.cs', 'cs'] }
  });
  if (!saveUri) return;

  const tmpXml = saveUri.fsPath.replace(/\.vizzy\.cs$/i, '.xml');
  fs.writeFileSync(tmpXml, resp.xml, 'utf8');
  await runCli(['import', tmpXml, '-o', saveUri.fsPath], path.dirname(saveUri.fsPath));
  await maybeOpen(saveUri.fsPath);
}

async function junoViewStages() {
  const stages = await junoGet('/stages');
  if (!stages || stages.ok === false) {
    vscode.window.showWarningMessage(stages?.error || 'Cannot reach the mod. Is Juno running?');
    return;
  }

  const names  = stages.activationGroupNames  || [];
  const states = stages.activationGroupStates || [];
  const lines  = [`Stage: ${stages.currentStage} / ${stages.numStages}`, ''];
  for (let i = 0; i < names.length; i++) {
    lines.push(`[${states[i] ? 'ON ' : 'OFF'}]  ${names[i] || `Group ${i}`}`);
  }

  const result = await vscode.window.showInformationMessage(
    lines.join('\n'),
    'Activate Next Stage', 'Close'
  );
  if (result === 'Activate Next Stage') {
    const activated = await junoPost('/stages/activate', {});
    if (!activated || activated.ok === false) {
      vscode.window.showWarningMessage(activated?.error || 'Could not activate the next stage.');
      return;
    }
    vscode.window.showInformationMessage('Stage activated!');
  }
}

// ── HTTP helpers (Juno bridge) ─────────────────────────────────────────────

async function junoSaveJson(kind, endpoint) {
  const payload = await junoGet(endpoint);
  if (!payload) {
    vscode.window.showWarningMessage(`Cannot read Juno ${kind}. Is Juno running with the VizzyCode mod?`);
    return;
  }

  const workspaceFolder = vscode.workspace.workspaceFolders?.[0]?.uri.fsPath;
  const defaultDir = workspaceFolder || require('os').homedir();
  const craftName = payload.craftName || payload.craft?.name || 'JunoCraft';
  const safeName = String(craftName).replace(/[^a-zA-Z0-9_\-. ]/g, '_').trim() || 'JunoCraft';

  const saveUri = await vscode.window.showSaveDialog({
    defaultUri: vscode.Uri.file(path.join(defaultDir, `${safeName}.${kind}.json`)),
    filters: { 'JSON': ['json'] }
  });
  if (!saveUri) return;

  fs.writeFileSync(saveUri.fsPath, JSON.stringify(payload, null, 2), 'utf8');
  await maybeOpen(saveUri.fsPath);
  vscode.window.showInformationMessage(
    `Saved Juno ${kind} JSON. Give it to humans or AI agents as craft context before editing Vizzy.`
  );
}

async function junoSaveReport(kind, endpoint) {
  const payload = await junoGet(endpoint);
  if (!payload) {
    vscode.window.showWarningMessage(`Cannot read Juno ${kind}. Is Juno running with the VizzyCode mod?`);
    return;
  }

  const workspaceFolder = vscode.workspace.workspaceFolders?.[0]?.uri.fsPath;
  const defaultDir = workspaceFolder || require('os').homedir();
  const craftName = payload.craftName || payload.craft?.name || payload.telemetry?.craftName || 'JunoCraft';
  const safeName = String(craftName).replace(/[^a-zA-Z0-9_\-. ]/g, '_').trim() || 'JunoCraft';

  const saveUri = await vscode.window.showSaveDialog({
    defaultUri: vscode.Uri.file(path.join(defaultDir, `${safeName}.${kind}.report.md`)),
    filters: { 'Markdown': ['md'], 'Text': ['txt'] }
  });
  if (!saveUri) return;

  fs.writeFileSync(saveUri.fsPath, buildJunoMarkdownReport(payload, kind), 'utf8');
  await maybeOpen(saveUri.fsPath);
  vscode.window.showInformationMessage(
    `Saved Juno ${kind} report. This is the recommended human/AI-readable craft context.`
  );
}

function buildJunoMarkdownReport(payload, kind) {
  const craft = payload.craft || payload;
  const telemetry = payload.telemetry || payload;
  const stages = payload.stages || {};
  const parts = Array.isArray(craft.parts) ? craft.parts : [];
  const titleKind = kind === 'telemetry' ? 'Telemetry' : 'Craft Snapshot';
  const craftName = payload.craftName || craft.name || telemetry.craftName || 'Unknown craft';
  const lines = [];

  lines.push(`# Juno ${titleKind} Report`, '');
  lines.push('This is the human/AI-readable view of the VizzyCode Juno bridge data. Keep the raw JSON when exact machine-readable data is needed, but give this report to humans or AI agents first.', '');

  section(lines, 'Summary');
  bullet(lines, 'Craft', craftName);
  bullet(lines, 'Scene', payload.scene || telemetry.scene);
  bullet(lines, 'Mod version', payload.modVersion);
  bullet(lines, 'Telemetry quality', telemetry.quality);
  bullet(lines, 'Active full flight data', boolText(telemetry.activeFullFlightData));
  bullet(lines, 'Tracked fallback', boolText(telemetry.trackedFallback));
  bullet(lines, 'Has command pod', boolText(telemetry.hasCommandPod));
  bullet(lines, 'Physics enabled', boolText(telemetry.physicsEnabled));
  bullet(lines, 'Part count', valueText(craft.partCount));
  bullet(lines, 'Body count', valueText(craft.bodyCount));
  bullet(lines, 'Current stage', stageSummary(stages));
  lines.push('');

  section(lines, 'AI Authoring Notes');
  lines.push('- Use exact part names from this report when calling `Vz.PartNameToID(...)` or targeting part properties.');
  lines.push('- Prefer activation group and stage data from this report over guessing from craft appearance.');
  lines.push('- Treat `designer_static`, `flight_no_flightdata`, and `trackedFallback=true` as limited-quality data.');
  lines.push('- If the Vizzy script depends on orbit, velocity, fuel, or target state, capture this report again in flight.');
  lines.push('- If a needed part is missing here, the script should not assume it exists.', '');

  section(lines, 'Live Telemetry');
  table(lines, [
    ['Altitude ASL', unit(telemetry.altitudeASL, 'm')],
    ['Altitude AGL', unit(telemetry.altitudeAGL, 'm')],
    ['Terrain altitude', unit(telemetry.altitudeTerrain, 'm')],
    ['Velocity', unit(telemetry.velocityMagnitude, 'm/s')],
    ['Surface velocity', unit(telemetry.surfaceVelocityMagnitude, 'm/s')],
    ['Vertical speed', unit(telemetry.verticalSurfaceVelocity, 'm/s')],
    ['Lateral speed', unit(telemetry.lateralSurfaceVelocity, 'm/s')],
    ['Acceleration', unit(telemetry.accelerationMagnitude, 'm/s^2')],
    ['Pitch', unit(telemetry.pitch, 'deg')],
    ['Heading', unit(telemetry.heading, 'deg')],
    ['Bank', unit(telemetry.bankAngle, 'deg')],
    ['Grounded', boolText(telemetry.grounded)],
    ['Mass', unit(telemetry.currentMassKg, 'kg')],
    ['Fuel mass', unit(telemetry.fuelMass, 'kg')],
    ['Current thrust', unit(telemetry.currentEngineThrustN, 'N')],
    ['Max active thrust', unit(telemetry.maxActiveEngineThrustN, 'N')]
  ]);
  lines.push('');

  if (telemetry.orbit) {
    section(lines, 'Orbit');
    table(lines, [
      ['Parent', telemetry.orbit.parent],
      ['Apoapsis altitude', unit(telemetry.orbit.apoapsisAltitude, 'm')],
      ['Periapsis altitude', unit(telemetry.orbit.periapsisAltitude, 'm')],
      ['Time to apoapsis', unit(telemetry.orbit.apoapsisTime, 's')],
      ['Time to periapsis', unit(telemetry.orbit.periapsisTime, 's')],
      ['Eccentricity', valueText(telemetry.orbit.eccentricity)],
      ['Inclination', unit(telemetry.orbit.inclination, 'deg')],
      ['Period', unit(telemetry.orbit.period, 's')],
      ['Burn node delta-v', unit(telemetry.orbit.burnNodeDeltaV, 'm/s')],
      ['Has burn node', boolText(telemetry.orbit.hasBurnNodePoint)]
    ]);
    lines.push('');
  }

  if (telemetry.performance) {
    section(lines, 'Performance');
    table(lines, [
      ['Current ISP', unit(telemetry.performance.currentIsp, 's')],
      ['Stage delta-v', unit(telemetry.performance.deltaVStage, 'm/s')],
      ['All-stage fuel', valueText(telemetry.performance.fuelAllStagesPercentage)],
      ['Remaining burn time', unit(telemetry.performance.remainingBurnTime, 's')],
      ['TWR', valueText(telemetry.performance.thrustToWeightRatio)]
    ]);
    lines.push('');
  }

  if (telemetry.navTarget) {
    section(lines, 'Nav Target');
    table(lines, [
      ['Exists', boolText(telemetry.navTarget.exists)],
      ['Type', telemetry.navTarget.type],
      ['Name', telemetry.navTarget.name],
      ['Body', telemetry.navTarget.bodyName],
      ['Raw type', telemetry.navTarget.rawType]
    ]);
    lines.push('');
  }

  section(lines, 'Stages And Activation Groups');
  bullet(lines, 'Current stage', valueText(stages.currentStage));
  bullet(lines, 'Stage count', valueText(stages.numStages));
  lines.push('');
  const names = Array.isArray(stages.activationGroupNames) ? stages.activationGroupNames : [];
  const states = Array.isArray(stages.activationGroupStates) ? stages.activationGroupStates : [];
  if (names.length) {
    lines.push('| Group | Name | State |', '| --- | --- | --- |');
    names.forEach((name, i) => lines.push(`| ${i} | ${md(name)} | ${states[i] === true ? 'ON' : states[i] === false ? 'OFF' : 'unknown'} |`));
    lines.push('');
  }
  const stageList = Array.isArray(stages.stages) ? stages.stages : [];
  if (stageList.length) {
    lines.push('| Stage | Current | Part count | Part names |', '| --- | --- | --- | --- |');
    stageList.forEach(stage => {
      const partNames = Array.isArray(stage.partNames) ? stage.partNames.slice(0, 12).join(', ') : '';
      lines.push(`| ${valueText(stage.stage)} | ${boolText(stage.isCurrent)} | ${valueText(stage.partCount)} | ${md(partNames)} |`);
    });
    lines.push('');
  }

  if (parts.length) {
    section(lines, 'Vizzy-Capable Parts');
    const vizzy = parts.filter(p => p.hasVizzy);
    if (!vizzy.length) lines.push('No Vizzy-capable parts were reported.');
    else {
      lines.push('| ID | Name | Type | Stage | Activation Group |', '| --- | --- | --- | --- | --- |');
      vizzy.forEach(p => lines.push(`| ${valueText(p.id)} | ${md(p.name)} | ${md(p.partType)} | ${valueText(p.activationStage)} | ${valueText(p.activationGroup)} |`));
    }
    lines.push('');

    section(lines, 'Parts By Practical Role');
    const byRole = new Map();
    parts.forEach(p => {
      const role = classifyPart(p);
      if (!byRole.has(role)) byRole.set(role, []);
      byRole.get(role).push(p);
    });
    [...byRole.keys()].sort().forEach(role => {
      const namesForRole = byRole.get(role).slice(0, 20).map(p => `${p.name || '(unnamed)'} #${p.id} stage ${p.activationStage}`).join('; ');
      lines.push(`- **${md(role)}**: ${md(namesForRole)}`);
    });
    lines.push('');

    section(lines, 'All Parts');
    lines.push('| ID | Name | Type | Role | Stage | AG | Mass | Modifiers |', '| --- | --- | --- | --- | --- | --- | --- | --- |');
    parts.slice().sort((a, b) => Number(a.id || 0) - Number(b.id || 0)).forEach(p => {
      const modCount = Array.isArray(p.modifiers) ? p.modifiers.length : 0;
      lines.push(`| ${valueText(p.id)} | ${md(p.name)} | ${md(p.partType)} | ${md(classifyPart(p))} | ${valueText(p.activationStage)} | ${valueText(p.activationGroup)} | ${unit(p.mass, 'kg')} | ${modCount} |`);
    });
    lines.push('');
  }

  if (Array.isArray(payload.notes) && payload.notes.length) {
    section(lines, 'Bridge Notes');
    payload.notes.forEach(note => lines.push(`- ${note}`));
    lines.push('');
  }

  section(lines, 'Raw Data');
  lines.push('The original bridge payload should be saved next to this report as JSON when exact values, vectors, or unlisted fields are required.');
  return lines.join('\n');
}

function classifyPart(part) {
  const mods = Array.isArray(part.modifiers) ? part.modifiers : [];
  const haystack = [
    part.name, part.partType,
    ...mods.flatMap(m => [m.typeName, m.name, m.typeId])
  ].filter(Boolean).join(' ').toLowerCase();
  if (haystack.includes('flightprogram') || part.hasVizzy) return 'Vizzy / flight computer';
  if (haystack.includes('commandpod') || haystack.includes('command pod')) return 'Command pod';
  if (haystack.includes('rocketengine') || haystack.includes('engine')) return 'Engine';
  if (haystack.includes('fueltank') || haystack.includes('fuel tank') || haystack.includes('propellant')) return 'Fuel';
  if (haystack.includes('parachute')) return 'Parachute';
  if (haystack.includes('solar')) return 'Solar / power';
  if (haystack.includes('battery')) return 'Battery / power';
  if (haystack.includes('gyro') || haystack.includes('reaction wheel')) return 'Attitude control';
  if (haystack.includes('rcs') || haystack.includes('nozzle')) return 'RCS';
  if (haystack.includes('landingleg') || haystack.includes('landing leg') || haystack.includes('leg')) return 'Landing gear';
  if (haystack.includes('wheel')) return 'Wheel';
  if (haystack.includes('docking')) return 'Docking';
  if (haystack.includes('separator') || haystack.includes('decoupler')) return 'Separation';
  if (haystack.includes('wing') || haystack.includes('fin') || haystack.includes('controlsurface')) return 'Aero / control surface';
  return 'Other';
}

function section(lines, title) {
  lines.push(`## ${title}`, '');
}

function bullet(lines, label, value) {
  if (value === undefined || value === null || value === '') return;
  lines.push(`- **${label}:** ${value}`);
}

function table(lines, rows) {
  lines.push('| Field | Value |', '| --- | --- |');
  rows.forEach(([label, value]) => {
    if (value !== undefined && value !== null && value !== '') lines.push(`| ${md(label)} | ${md(value)} |`);
  });
}

function stageSummary(stages) {
  if (!stages || (stages.currentStage === undefined && stages.numStages === undefined)) return '';
  return `${valueText(stages.currentStage)} / ${valueText(stages.numStages)}`;
}

function valueText(value) {
  if (value === undefined || value === null) return '';
  if (typeof value === 'number') return Number.isInteger(value) ? String(value) : value.toFixed(3).replace(/\.?0+$/, '');
  return String(value);
}

function boolText(value) {
  return typeof value === 'boolean' ? String(value) : '';
}

function unit(value, suffix) {
  const text = valueText(value);
  return text ? `${text} ${suffix}` : '';
}

function md(value) {
  return valueText(value).replace(/\|/g, '\\|').replace(/[\r\n]+/g, ' ');
}

function junoGet(endpoint) {
  return new Promise((resolve) => {
    const req = http.get(JUNO_BASE + endpoint, { timeout: 3000 }, (res) => {
      let data = '';
      res.on('data', c => data += c);
      res.on('end', () => { try { resolve(JSON.parse(data)); } catch { resolve(null); } });
    });
    req.on('error', () => resolve(null));
    req.on('timeout', () => { req.destroy(); resolve(null); });
  });
}

function junoPut(endpoint, body) {
  return new Promise((resolve) => {
    const payload = JSON.stringify(body);
    const options = {
      hostname: '127.0.0.1', port: JUNO_PORT,
      path: endpoint, method: 'PUT',
      headers: { 'Content-Type': 'application/json', 'Content-Length': Buffer.byteLength(payload) },
      timeout: 5000
    };
    const req = http.request(options, (res) => {
      let data = '';
      res.on('data', c => data += c);
      res.on('end', () => { try { resolve(JSON.parse(data)); } catch { resolve(null); } });
    });
    req.on('error', () => resolve(null));
    req.on('timeout', () => { req.destroy(); resolve(null); });
    req.write(payload);
    req.end();
  });
}

function junoPost(endpoint, body) {
  return new Promise((resolve) => {
    const payload = JSON.stringify(body);
    const options = {
      hostname: '127.0.0.1', port: JUNO_PORT,
      path: endpoint, method: 'POST',
      headers: { 'Content-Type': 'application/json', 'Content-Length': Buffer.byteLength(payload) },
      timeout: 5000
    };
    const req = http.request(options, (res) => {
      let data = '';
      res.on('data', c => data += c);
      res.on('end', () => { try { resolve(JSON.parse(data)); } catch { resolve(null); } });
    });
    req.on('error', () => resolve(null));
    req.on('timeout', () => { req.destroy(); resolve(null); });
    req.write(payload);
    req.end();
  });
}

async function pickVizzyPart() {
  const craft = await junoGet('/craft');
  if (!craft) {
    vscode.window.showWarningMessage('Cannot reach the mod. Is Juno running?');
    return null;
  }
  const parts = (craft.parts || []).filter(p => p.hasVizzy);
  if (parts.length === 0) {
    vscode.window.showInformationMessage('No Vizzy parts in the current craft.');
    return null;
  }
  if (parts.length === 1) return parts[0];

  const items = parts.map(p => ({
    label: `$(circuit-board) ${p.name}`, description: `id: ${p.id}`,
    id: p.id, name: p.name
  }));
  const picked = await vscode.window.showQuickPick(items, { placeHolder: 'Select a part' });
  return picked ? { id: picked.id, name: picked.name } : null;
}

function setJunoStatusBar(status) {
  if (!junoStatusBar) return;
  if (!status) {
    junoStatusBar.text    = '$(circle-slash) Juno';
    junoStatusBar.tooltip = 'Juno: Not connected — click to check';
    junoStatusBar.color   = undefined;
    return;
  }
  const scene = status.scene || '?';
  const craft = status.craftName ? ` · ${status.craftName}` : '';
  junoStatusBar.text    = `$(rocket) Juno: ${scene}${craft}`;
  junoStatusBar.tooltip = `Connected to Juno — mod v${status.modVersion || '?'}`;
  junoStatusBar.color   = new vscode.ThemeColor('statusBarItem.prominentForeground');
}

// ── Shared helpers ─────────────────────────────────────────────────────────

async function resolveInputUri(uri, kind) {
  if (uri && uri.fsPath && fs.existsSync(uri.fsPath)) return uri;

  const active = vscode.window.activeTextEditor?.document?.uri;
  if (active && fs.existsSync(active.fsPath)) {
    if (kind === 'xml'  && active.fsPath.toLowerCase().endsWith('.xml')) return active;
    if (kind === 'code' && active.fsPath.toLowerCase().endsWith('.cs'))  return active;
  }

  const filters = kind === 'xml'
    ? { 'Vizzy XML': ['xml'] }
    : { 'Vizzy Code': ['cs'] };

  const picked = await vscode.window.showOpenDialog({ canSelectMany: false, filters });
  return picked && picked.length > 0 ? picked[0] : null;
}

async function runCli(args, cwd) {
  const cliPath = await resolveCliPath();
  if (!cliPath) {
    throw new Error(
      'VizzyCode CLI not found. Build or publish VizzyCode.Cli, or set vizzycode.cliPath.'
    );
  }
  const output = await new Promise((resolve, reject) => {
    cp.execFile(cliPath, args, { cwd }, (error, stdout, stderr) => {
      if (error) { reject(new Error((stderr || stdout || error.message).trim())); return; }
      resolve((stdout || '').trim());
    });
  });
  if (output) vscode.window.showInformationMessage(String(output));
}

async function safeRun(action) {
  try { await action(); }
  catch (error) {
    vscode.window.showErrorMessage(String(error && error.message ? error.message : error));
  }
}

async function resolveCliPath() {
  if (extensionContext) {
    const candidates = [
      path.join(extensionContext.extensionPath, 'bin', 'win-x64', 'VizzyCode.Cli.exe'),
      path.join(extensionContext.extensionPath, 'bin', 'win-x64', 'VizzyCode.Cli')
    ];
    for (const c of candidates) if (fs.existsSync(c)) return c;
  }

  const configured = vscode.workspace.getConfiguration('vizzycode').get('cliPath');
  if (configured && fs.existsSync(configured)) return configured;

  for (const root of (vscode.workspace.workspaceFolders || [])) {
    const base = root.uri.fsPath;
    const candidates = [
      path.join(base, 'publish_standalone_win64', 'VizzyCode.Cli.exe'),
      path.join(base, 'VizzyCode.Cli', 'bin', 'Debug',   'net9.0', 'VizzyCode.Cli.exe'),
      path.join(base, 'VizzyCode.Cli', 'bin', 'Release', 'net9.0', 'VizzyCode.Cli.exe'),
      path.join(base, 'VizzyCode.Cli', 'bin', 'Debug',   'net9.0', 'VizzyCode.Cli'),
      path.join(base, 'VizzyCode.Cli', 'bin', 'Release', 'net9.0', 'VizzyCode.Cli')
    ];
    for (const c of candidates) if (fs.existsSync(c)) return c;
  }
  return null;
}

async function maybeOpen(filePath) {
  if (!vscode.workspace.getConfiguration('vizzycode').get('autoOpenResults', true)) return;
  const doc = await vscode.workspace.openTextDocument(filePath);
  await vscode.window.showTextDocument(doc, { preview: false });
}

module.exports = { activate, deactivate };
