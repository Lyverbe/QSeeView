using System;
using System.ComponentModel;
using System.Diagnostics;

namespace QSeeView.Tools
{
    public class VideoConverter
    {
        public event EventHandler<int> ConversionDone;

        public bool IsConverting { get; private set; }

        public void Convert(string inputFileName)
        {
            var process = new Process();
            process.StartInfo.FileName = App.Settings.FfmpegPath;
            // For full conversion versus quick header change
            //process.StartInfo.Arguments = $"-y -r 24 -i \"{App.Settings.DownloadFolder}\\{inputFileName}.dav\" -preset fast -b:v 1000k -c libx264 \"{App.Settings.DownloadFolder}\\{inputFileName}.avi\"";
            process.StartInfo.Arguments = $"-y -f dhav -i \"{App.Settings.DownloadFolder}\\{inputFileName}.dav\" -vcodec copy \"{App.Settings.DownloadFolder}\\{inputFileName}.avi\"";
            process.StartInfo.UseShellExecute = true;
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            process.EnableRaisingEvents = true;
            process.Exited += (s, e) => ConversionDone?.Invoke(this, process.ExitCode);

#if false   // For debugging ffmpeg
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            var sb = new System.Text.StringBuilder();
            process.OutputDataReceived += (s, e) => sb.AppendLine(e.Data);
            process.ErrorDataReceived += (s, e) => sb.AppendLine(e.Data);
#endif

            try
            {
                IsConverting = process.Start();
#if false   // For debugging ffmpeg
                process.BeginOutputReadLine();
                process.BeginErrorReadLine();
                process.WaitForExit();
                var _x = sb.ToString();
#endif
            }
            catch (Win32Exception exception)
            {
                throw exception;
            }
        }
    }
}
