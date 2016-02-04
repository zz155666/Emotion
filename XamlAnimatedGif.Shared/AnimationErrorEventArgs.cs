using System;
using Windows.UI.Xaml;

namespace XamlAnimatedGif
{
    public class AnimationErrorEventArgs : EventArgs
    {
        public AnimationErrorEventArgs(Exception exception, AnimationErrorKind kind)
        {
            Exception = exception;
            Kind = kind;
        }

        public Exception Exception { get; }

        public AnimationErrorKind Kind { get; }
    }

    public enum AnimationErrorKind
    {
        Loading,
        Rendering
    }
}
