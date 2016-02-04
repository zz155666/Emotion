using Emotion.Core.HTTP;
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
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

//“空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409 上有介绍

namespace Emotion
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPagePCHot : Page
    {
        int _preSelectNavigation = -1;
        bool _ignoreNavigation = false;
        /// <summary>
        /// 构造方法
        /// </summary>
        public MainPagePCHot()
        {
            this.InitializeComponent();
            mainNavigationList.SelectedIndex = 1;
      //      ShowNavigationBar(App.AlwaysShowNavigation);
        }

        #region  事件处理程序
        /// <summary>
        /// 导航栏隐现
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ListBoxItem_Tapped(object sender, TappedRoutedEventArgs e)
        {
            ListBoxItem tapped_item = sender as ListBoxItem;
            if (tapped_item != null && tapped_item.Tag != null && tapped_item.Tag.ToString().Equals("0")) //汉堡按钮
            {
                mainSplitView.IsPaneOpen = !mainSplitView.IsPaneOpen;
            }
        }
        /// <summary>
        /// 导航
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void mainNavigationList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_ignoreNavigation)
            {
                _ignoreNavigation = false;
                return;
            }
            ListBoxItem tapped_item = mainNavigationList.SelectedItems[0] as ListBoxItem;
            if (tapped_item != null && tapped_item.Tag != null && tapped_item.Tag.ToString().Equals("1")) //最新
            {
                mainSplitView.IsPaneOpen = false;
                _preSelectNavigation = mainNavigationList.SelectedIndex;
                mainFrame.Navigate(typeof(HomePage));
            }
            if (tapped_item != null && tapped_item.Tag != null && tapped_item.Tag.ToString().Equals("2")) //热门
            {
                mainSplitView.IsPaneOpen = false;
                _preSelectNavigation = mainNavigationList.SelectedIndex;
        //        mainFrame.Navigate(typeof(NewsPage));
            }
            if (tapped_item != null && tapped_item.Tag != null && tapped_item.Tag.ToString().Equals("3")) //收藏
            {
                mainSplitView.IsPaneOpen = false;
                _preSelectNavigation = mainNavigationList.SelectedIndex;
          //      mainFrame.Navigate(typeof(RankingPage));
            }
            if (tapped_item != null && tapped_item.Tag != null && tapped_item.Tag.ToString().Equals("4")) //搜索
            {
                mainSplitView.IsPaneOpen = false;
                _preSelectNavigation = mainNavigationList.SelectedIndex;
        //        mainFrame.Navigate(typeof(FlashPage));
            }
            if (tapped_item != null && tapped_item.Tag != null && tapped_item.Tag.ToString().Equals("4")) //设置
            {
                mainSplitView.IsPaneOpen = false;
                _preSelectNavigation = mainNavigationList.SelectedIndex;
                //        mainFrame.Navigate(typeof(FlashPage));
            }
        }
        #endregion

        /// <summary>
        /// 设置主页面导航显示方式
        /// </summary>
        /// <param name="show"></param>
        public void ShowNavigationBar(bool show)
        {
            mainSplitView.DisplayMode = show ? SplitViewDisplayMode.CompactOverlay : SplitViewDisplayMode.Overlay;
        }
        /// <summary>
        /// 打开导航栏一次
        /// </summary>
        public void ShowNavigationBarOneTime()
        {
            mainSplitView.IsPaneOpen = true;
        }
    }
}
