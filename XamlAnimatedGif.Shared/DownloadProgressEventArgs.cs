using System.Windows;

namespace XamlAnimatedGif
{

    public class DownloadProgressEventArgs : System.EventArgs
    {
        public int Progress { get; set; }

        public DownloadProgressEventArgs(int progress)
        {
            Progress = progress;
        }
    }
}
