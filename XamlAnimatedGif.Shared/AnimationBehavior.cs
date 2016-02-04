using XamlAnimatedGif.Decoding;
using System;
using System.IO;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Media;

namespace XamlAnimatedGif
{
    public static class AnimationBehavior
    {

        public static Uri GetSourceUri(Image image)
        {
            return (Uri)image.GetValue(SourceUriProperty);
        }

        public static void SetSourceUri(Image image, Uri value)
        {
            image.SetValue(SourceUriProperty, value);
        }

        public static readonly DependencyProperty SourceUriProperty =
            DependencyProperty.RegisterAttached(
              "SourceUri",
              typeof(Uri),
              typeof(AnimationBehavior),
              new PropertyMetadata(
                null,
                SourceChanged));

        #region SourceStream

        public static Stream GetSourceStream(DependencyObject obj)
        {
            return (Stream)obj.GetValue(SourceStreamProperty);
        }

        public static void SetSourceStream(DependencyObject obj, Stream value)
        {
            obj.SetValue(SourceStreamProperty, value);
        }

        public static readonly DependencyProperty SourceStreamProperty =
            DependencyProperty.RegisterAttached(
                "SourceStream",
                typeof(Stream),
                typeof(AnimationBehavior),
                new PropertyMetadata(
                    null,
                    SourceChanged));

        #endregion

        #region RepeatBehavior

        public static RepeatBehavior GetRepeatBehavior(DependencyObject obj)
        {
            return (RepeatBehavior)obj.GetValue(RepeatBehaviorProperty);
        }

        public static void SetRepeatBehavior(DependencyObject obj, RepeatBehavior value)
        {
            obj.SetValue(RepeatBehaviorProperty, value);
        }

        public static readonly DependencyProperty RepeatBehaviorProperty =
            DependencyProperty.RegisterAttached(
              "RepeatBehavior",
              typeof(RepeatBehavior),
              typeof(AnimationBehavior),
              new PropertyMetadata(
                default(RepeatBehavior),
                SourceChanged));

        #endregion

        #region AutoStart

        public static bool GetAutoStart(DependencyObject obj)
        {
            return (bool)obj.GetValue(AutoStartProperty);
        }

        public static void SetAutoStart(DependencyObject obj, bool value)
        {
            obj.SetValue(AutoStartProperty, value);
        }

        public static readonly DependencyProperty AutoStartProperty =
            DependencyProperty.RegisterAttached(
                "AutoStart",
                typeof(bool),
                typeof(AnimationBehavior),
                new PropertyMetadata(true));

        #endregion

        #region AnimateInDesignMode


        public static bool GetAnimateInDesignMode(DependencyObject obj)
        {
            return (bool)obj.GetValue(AnimateInDesignModeProperty);
        }

        public static void SetAnimateInDesignMode(DependencyObject obj, bool value)
        {
            obj.SetValue(AnimateInDesignModeProperty, value);
        }

        public static readonly DependencyProperty AnimateInDesignModeProperty =
            DependencyProperty.RegisterAttached(
                "AnimateInDesignMode",
                typeof(bool),
                typeof(AnimationBehavior),
                new PropertyMetadata(
                    false,
                    AnimateInDesignModeChanged));

        #endregion

        #region Animator

        public static Animator GetAnimator(DependencyObject obj)
        {
            return (Animator) obj.GetValue(AnimatorProperty);
        }

        private static void SetAnimator(DependencyObject obj, Animator value)
        {
            obj.SetValue(AnimatorProperty, value);
        }

        public static readonly DependencyProperty AnimatorProperty =
            DependencyProperty.RegisterAttached(
                "Animator",
                typeof (Animator),
                typeof (AnimationBehavior),
                new PropertyMetadata(null));

        #endregion

        #region Error


        // WinRT doesn't support custom attached events, use a normal CLR event instead
        public static event EventHandler<AnimationErrorEventArgs> Error;

        internal static void OnError(Image image, Exception exception, AnimationErrorKind kind)
        {
            Error?.Invoke(image, new AnimationErrorEventArgs(exception, kind));
        }

        #endregion

        #region DownloadProgress

        // WinRT doesn't support custom attached events, use a normal CLR event instead
        public static event EventHandler<DownloadProgressEventArgs> DownloadProgress;

        internal static void OnDownloadProgress(Image image, int downloadPercentage)
        {
            DownloadProgress?.Invoke(image, new DownloadProgressEventArgs(downloadPercentage));
        }
        #endregion

        #region Loaded


        // WinRT doesn't support custom attached events, use a normal CLR event instead
        public static event EventHandler Loaded;

        private static void OnLoaded(Image sender)
        {
            Loaded?.Invoke(sender, EventArgs.Empty);
        }

        #endregion


        #region Private attached properties

        private static int GetSeqNum(DependencyObject obj)
        {
            return (int)obj.GetValue(SeqNumProperty);
        }

        private static void SetSeqNum(DependencyObject obj, int value)
        {
            obj.SetValue(SeqNumProperty, value);
        }

        private static readonly DependencyProperty SeqNumProperty =
            DependencyProperty.RegisterAttached("SeqNum", typeof(int), typeof(AnimationBehavior), new PropertyMetadata(0));

        #endregion

        private static void SourceChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            var image = o as Image;
            if (image == null)
                return;

            InitAnimation(image);
        }

        private static void AnimateInDesignModeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var image = d as Image;
            if (image == null)
                return;

            InitAnimation(image);
        }

        private static bool CheckDesignMode(Image image, Uri sourceUri, Stream sourceStream)
        {
            if (IsInDesignMode(image) && !GetAnimateInDesignMode(image))
            {
                var bmp = new BitmapImage();
                if (sourceStream != null)
                {
                    bmp.SetSource(sourceStream.AsRandomAccessStream());
                }
                else if (sourceUri != null)
                {
                    bmp.UriSource = sourceUri;
                }
                image.Source = bmp;
                return false;
            }
            return true;
        }

        private static void InitAnimation(Image image)
        {
            if (IsLoaded(image))
            {
                image.Unloaded += Image_Unloaded;
            }
            else
            {
                image.Loaded += Image_Loaded;
                return;
            }

            int seqNum = GetSeqNum(image) + 1;
            SetSeqNum(image, seqNum);

            image.Source = null;
            ClearAnimatorCore(image);
            
            try
            {
                var stream = GetSourceStream(image);
                if (stream != null)
                {
                    InitAnimationAsync(image, stream, GetRepeatBehavior(image), seqNum);
                    return;
                }

                var uri = GetAbsoluteUri(image);
                if (uri != null)
                {
                    InitAnimationAsync(image, uri, GetRepeatBehavior(image), seqNum);
                }
            }
            catch (Exception ex)
            {
                OnError(image, ex, AnimationErrorKind.Loading);
            }
        }

        private static void Image_Loaded(object sender, RoutedEventArgs e)
        {
            var image = (Image) sender;
            image.Loaded -= Image_Loaded;
            InitAnimation(image);
        }

        private static void Image_Unloaded(object sender, RoutedEventArgs e)
        {
            var image = (Image) sender;
            image.Unloaded -= Image_Unloaded;
            image.Loaded += Image_Loaded;
            ClearAnimatorCore(image);
        }

        private static bool IsLoaded(FrameworkElement element)
        {
            return VisualTreeHelper.GetParent(element) != null;
        }

        private static Uri GetAbsoluteUri(Image image)
        {
            var uri = GetSourceUri(image);
            if (uri == null)
                return null;
            if (!uri.IsAbsoluteUri)
            {
                var baseUri = image.BaseUri;
                if (baseUri != null)
                {
                    uri = new Uri(baseUri, uri);
                }
                else
                {
                    throw new InvalidOperationException("Relative URI can't be resolved");
                }
            }
            return uri;
        }

        private static async void InitAnimationAsync(Image image, Uri sourceUri, RepeatBehavior repeatBehavior, int seqNum)
        {
            if (!CheckDesignMode(image, sourceUri, null))
                return;

            try
            {
                var animator = await Animator.CreateAsync(image, sourceUri, repeatBehavior);
                // Check that the source hasn't changed while we were loading the animation
                if (GetSeqNum(image) != seqNum)
                {
                    animator.Dispose();
                    return;
                }
                await SetAnimatorCoreAsync(image, animator);
                OnLoaded(image);
            }
            catch (InvalidSignatureException)
            {
                image.Source = new BitmapImage(sourceUri);
                OnLoaded(image);
            }
            catch(Exception ex)
            {
                OnError(image, ex, AnimationErrorKind.Loading);
            }
        }

        private static async void InitAnimationAsync(Image image, Stream stream, RepeatBehavior repeatBehavior, int seqNum)
        {
            if (!CheckDesignMode(image, null, stream))
                return;

            try
            {
                var animator = await Animator.CreateAsync(image, stream, repeatBehavior);
                await SetAnimatorCoreAsync(image, animator);
                // Check that the source hasn't changed while we were loading the animation
                if (GetSeqNum(image) != seqNum)
                {
                    animator.Dispose();
                    return;
                }
                OnLoaded(image);
            }
            catch (InvalidSignatureException)
            {
                var bmp = new BitmapImage();
                bmp.SetSource(stream.AsRandomAccessStream());
                image.Source = bmp;
                OnLoaded(image);
            }
            catch(Exception ex)
            {
                OnError(image, ex, AnimationErrorKind.Loading);
            }
        }

        private static async Task SetAnimatorCoreAsync(Image image, Animator animator)
        {
            SetAnimator(image, animator);
            image.Source = animator.Bitmap;
            if (GetAutoStart(image))
                animator.Play();
            else
                await animator.ShowFirstFrameAsync();
        }

        private static void ClearAnimatorCore(Image image)
        {
            var animator = GetAnimator(image);
            if (animator == null)
                return;

            animator.Dispose();
            SetAnimator(image, null);
        }

        // ReSharper disable once UnusedParameter.Local (used in WPF)
        private static bool IsInDesignMode(DependencyObject obj)
        {
            return DesignMode.DesignModeEnabled;

        }
    }
}
