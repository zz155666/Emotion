using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Popups;
using Windows.ApplicationModel.DataTransfer;
using Emotion.Core.ViewModels;
using Emotion.Core.Data;
using Windows.Storage.Streams;
using Windows.Storage;
using System.Net;
using System.Threading.Tasks;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上提供

namespace Emotion
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class EmotionContentPage : Page
    {
        /// <summary>
        /// 当前查看的表情包
        /// </summary>
        private HomeItem _homeItem;
        private ImageContentList _iamgeContentList;
        public EmotionContentPage()
        {
            this.InitializeComponent();
            if (App.AlwaysShowNavigation)
            {
                Home.Visibility = Visibility.Collapsed;
            }
            
        }


        /// <summary>
        /// 页面加载
        /// </summary>
        /// <param name="e"></param>
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            object[] parameters = e.Parameter as object[];
            if (parameters != null)
            {
                if (parameters.Length == 1 && (parameters[0] as HomeItem) != null)
                {
                    _homeItem = parameters[0] as HomeItem;
                    ContextList.ItemsSource = _iamgeContentList = new ImageContentList(_homeItem.eId);
                    _iamgeContentList.DataLoaded += _list_blogs_DataLoaded;
                    _iamgeContentList.DataLoading += _list_blogs_DataLoading;
                    PageTitle.Text = _homeItem.eName;
                }
            }
        }
        private void _list_blogs_DataLoaded()
        {
            Loading.IsActive = false;
        }
        private void _list_blogs_DataLoading()
        {
            Loading.IsActive = true;
        }
        /// <summary>
        /// 点击后退
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Back_Click(object sender, RoutedEventArgs e)
        {
            if (this.Frame.CanGoBack)
            {
                this.Frame.GoBack();
            }
        }
        /// <summary>
        /// 点击标题栏上的刷新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            _iamgeContentList.DoRefresh(_homeItem.eId);
            await ContextList.LoadMoreItemsAsync();
        }
        /// <summary>
        /// 打开主菜单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Home_Click(object sender, RoutedEventArgs e)
        {
            ((Window.Current.Content as Frame).Content as MainPagePC).ShowNavigationBarOneTime();
        }

        private void ContextList_ItemClick(object sender, ItemClickEventArgs e)
        {
            var pp = e.ClickedItem;
            if (pp is ImageContent)
            {
                var g = pp as ImageContent;

                DataPackage dataPackage = new DataPackage();
                dataPackage.SetBitmap(RandomAccessStreamReference.CreateFromUri(new Uri(g.Url)));
                Clipboard.SetContent(dataPackage);
                ContentIamge.Source = new BitmapImage(new Uri(g.Url));
           //     var Task2 = Task.Factory.StartNew(async () =>
           //{
           //    WebRequest request = WebRequest.Create(g.Url);
           //    WebResponse response = await request.GetResponseAsync();
           //    IBuffer buffer = null;
           //    Stream stream = response.GetResponseStream();
           //    var inputstream = stream.AsInputStream();
           //    using (var dataReader = new DataReader(inputstream))
           //    {
           //        await dataReader.LoadAsync((uint)stream.Length);
           //        buffer = dataReader.DetachBuffer();
           //    }
           //    var iRandomAccessStream = new InMemoryRandomAccessStream();
           //    await iRandomAccessStream.WriteAsync(buffer);
           //});

            }
        }
    }
}
