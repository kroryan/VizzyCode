using System;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace VizzyCode
{
    internal static class CliProcessRunner
    {
        public static async Task<(int ExitCode, string StdOut, string StdErr, bool TimedOut)> RunAsync(
            ProcessStartInfo startInfo,
            TimeSpan inactivityTimeout,
            CancellationToken ct)
        {
            DebugLog.Write($"START file={startInfo.FileName} args={startInfo.Arguments} cwd={startInfo.WorkingDirectory}");
            using var process = Process.Start(startInfo);
            if (process == null)
                throw new InvalidOperationException("Unable to start process.");

            process.StandardInput.Close();

            var stdout = new StringBuilder();
            var stderr = new StringBuilder();
            DateTime lastActivityUtc = DateTime.UtcNow;

            Task stdoutTask = PumpReaderAsync(process.StandardOutput, stdout, () => lastActivityUtc = DateTime.UtcNow, ct);
            Task stderrTask = PumpReaderAsync(process.StandardError, stderr, () => lastActivityUtc = DateTime.UtcNow, ct);

            while (!process.HasExited)
            {
                ct.ThrowIfCancellationRequested();

                if (DateTime.UtcNow - lastActivityUtc > inactivityTimeout)
                {
                    try { process.Kill(true); } catch { }
                    await Task.WhenAll(stdoutTask, stderrTask);
                    DebugLog.Write($"TIMEOUT pid={process.Id} stdout_len={stdout.Length} stderr_len={stderr.Length}");
                    return (-1, stdout.ToString(), stderr.ToString(), true);
                }

                await Task.Delay(250, ct);
            }

            await Task.WhenAll(stdoutTask, stderrTask);
            DebugLog.Write($"EXIT pid={process.Id} code={process.ExitCode} stdout_len={stdout.Length} stderr_len={stderr.Length}");
            return (process.ExitCode, stdout.ToString(), stderr.ToString(), false);
        }

        private static async Task PumpReaderAsync(System.IO.StreamReader reader, StringBuilder buffer, Action onActivity, CancellationToken ct)
        {
            char[] chunk = new char[1024];
            while (!reader.EndOfStream)
            {
                int read = await reader.ReadAsync(chunk.AsMemory(0, chunk.Length), ct);
                if (read <= 0)
                    break;

                onActivity();
                buffer.Append(chunk, 0, read);
                DebugLog.Write($"STREAM chunk_len={read}");
            }
        }
    }
}
