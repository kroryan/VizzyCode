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
  if (!stages) {
    vscode.window.showWarningMessage('Cannot reach the mod. Is Juno running?');
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
    await junoPost('/stages/activate', {});
    vscode.window.showInformationMessage('Stage activated!');
  }
}

// ── HTTP helpers (Juno bridge) ─────────────────────────────────────────────

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
