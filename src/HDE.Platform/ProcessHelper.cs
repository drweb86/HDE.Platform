using System.Diagnostics;
using System.Text;

namespace HDE.Platform
{
    static class ProcessHelper
    {
        public static void Execute(
            string executable,
            string args,
            string workingDirectory,

            out string stdOutput,
            out string stdError,
            out int returnCode)
        {
            var stdOutputBuilder = new StringBuilder();
            var stdErrorBuilder = new StringBuilder();

            var process = new Process();
            process.StartInfo.FileName = executable;
            process.StartInfo.WorkingDirectory = workingDirectory;
            process.StartInfo.Arguments = args;
            process.StartInfo.UseShellExecute = false;

            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.RedirectStandardOutput = true;

            process.OutputDataReceived += (s, a) => stdOutputBuilder.Append(a.Data);
            process.ErrorDataReceived += (s, a) => stdErrorBuilder.Append(a.Data);

            process.Start();

            process.BeginErrorReadLine();
            process.BeginOutputReadLine();

            process.WaitForExit();

            stdOutput = stdOutputBuilder.ToString();
            stdError = stdErrorBuilder.ToString();
            returnCode = process.ExitCode;
        }
    }
}