const vscode = require('vscode');
const cp = require('child_process');
const fs = require('fs');
const path = require('path');

let extensionContext = null;

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
    })
  );

  const statusBar = vscode.window.createStatusBarItem(vscode.StatusBarAlignment.Left, 100);
  statusBar.name = 'VizzyCode';
  statusBar.text = 'VizzyCode';
  statusBar.tooltip = 'VizzyCode extension is active';
  statusBar.command = 'vizzycode.importXmlToCode';
  statusBar.show();
  context.subscriptions.push(statusBar);
}

function deactivate() {}

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
  if (/\.vizzy\.cs$/i.test(output)) {
    output = output.replace(/\.vizzy\.cs$/i, '.xml');
  } else {
    output = output.replace(/\.cs$/i, '.xml');
  }

  await runCli(['export', input.fsPath, '-o', output], path.dirname(input.fsPath));
  await maybeOpen(output);
}

async function runRoundTrip(uri) {
  const input = await resolveInputUri(uri, 'xml');
  if (!input) return;

  const dir = path.dirname(input.fsPath);
  const stem = path.basename(input.fsPath, path.extname(input.fsPath));
  const output = path.join(dir, `${stem}.roundtrip.xml`);
  const code = path.join(dir, `${stem}.roundtrip.vizzy.cs`);
  await runCli(['roundtrip', input.fsPath, '-o', output, '-c', code], dir);
  await maybeOpen(code);
}

async function resolveInputUri(uri, kind) {
  if (uri && uri.fsPath && fs.existsSync(uri.fsPath)) return uri;

  const active = vscode.window.activeTextEditor?.document?.uri;
  if (active) {
    if (fs.existsSync(active.fsPath)) {
      if (kind === 'xml' && active.fsPath.toLowerCase().endsWith('.xml')) return active;
      if (kind === 'code' && active.fsPath.toLowerCase().endsWith('.cs')) return active;
    }
  }

  const filters = kind === 'xml'
    ? { 'Vizzy XML': ['xml'] }
    : { 'Vizzy Code': ['cs'] };

  const picked = await vscode.window.showOpenDialog({
    canSelectMany: false,
    filters
  });

  return picked && picked.length > 0 ? picked[0] : null;
}

async function runCli(args, cwd) {
  const cliPath = await resolveCliPath();
  if (!cliPath) {
    throw new Error('VizzyCode CLI not found. Build or publish VizzyCode.Cli, or configure vizzycode.cliPath.');
  }

  const output = await new Promise((resolve, reject) => {
    cp.execFile(cliPath, args, { cwd }, (error, stdout, stderr) => {
      if (error) {
        reject(new Error((stderr || stdout || error.message).trim()));
        return;
      }
      resolve((stdout || '').trim());
    });
  });

  if (output) {
    vscode.window.showInformationMessage(String(output));
  }
}

async function safeRun(action) {
  try {
    await action();
  } catch (error) {
    const message = String(error && error.message ? error.message : error);
    vscode.window.showErrorMessage(message);
  }
}

async function resolveCliPath() {
  if (extensionContext) {
    const bundledCandidates = [
      path.join(extensionContext.extensionPath, 'bin', 'win-x64', 'VizzyCode.Cli.exe'),
      path.join(extensionContext.extensionPath, 'bin', 'win-x64', 'VizzyCode.Cli')
    ];

    for (const candidate of bundledCandidates) {
      if (fs.existsSync(candidate)) return candidate;
    }
  }

  const configured = vscode.workspace.getConfiguration('vizzycode').get('cliPath');
  if (configured && fs.existsSync(configured)) return configured;

  const roots = vscode.workspace.workspaceFolders || [];
  for (const root of roots) {
    const base = root.uri.fsPath;
    const candidates = [
      path.join(base, 'publish_standalone_win64', 'VizzyCode.Cli.exe'),
      path.join(base, 'VizzyCode.Cli', 'bin', 'Debug', 'net9.0', 'VizzyCode.Cli.exe'),
      path.join(base, 'VizzyCode.Cli', 'bin', 'Release', 'net9.0', 'VizzyCode.Cli.exe'),
      path.join(base, 'VizzyCode.Cli', 'bin', 'Debug', 'net9.0', 'VizzyCode.Cli'),
      path.join(base, 'VizzyCode.Cli', 'bin', 'Release', 'net9.0', 'VizzyCode.Cli')
    ];

    for (const candidate of candidates) {
      if (fs.existsSync(candidate)) return candidate;
    }
  }

  return null;
}

async function maybeOpen(filePath) {
  const shouldOpen = vscode.workspace.getConfiguration('vizzycode').get('autoOpenResults', true);
  if (!shouldOpen) return;

  const doc = await vscode.workspace.openTextDocument(filePath);
  await vscode.window.showTextDocument(doc, { preview: false });
}

module.exports = {
  activate,
  deactivate
};
