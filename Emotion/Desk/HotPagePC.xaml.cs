﻿using System;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Emotion.Core.Data;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上提供

namespace Emotion.Desk
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class HotPagePc : Page
    {
        /// <summary>
        /// 首页表情列表
        /// </summary>
        private HomeItemList _list_blogs;
        /// <summary>
        /// 本类构造函数
        /// </summary>
        /// <param name="sign"></param>
        public HotPagePc( )
        {
            this.InitializeComponent();
            if (App.AlwaysShowNavigation)
            {
                Home.Visibility = Visibility.Collapsed;
            }
            BlogsListView.ItemsSource = _list_blogs = new HomeItemList(true);
            _list_blogs.DataLoaded += _list_blogs_DataLoaded;
            _list_blogs.DataLoading += _list_blogs_DataLoading;
        }
        /// <summary>
        /// 页面加载
        /// </summary>
        /// <param name="e"></param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            //没有加载参数
        }
        /// <summary>
        /// 点击博客条目 查看博客正文
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BlogsListView_ItemClick(object sender, ItemClickEventArgs e)
        {
          this.Frame.Navigate(typeof(EmotionContentPage), new object[] { e.ClickedItem });
        }
        /// <summary>
        /// 点击查看博主主页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HyperlinkButton_Click(object sender, RoutedEventArgs e)
        {
   //         this.Frame.Navigate(typeof(UserHome), new object[] { (sender as HyperlinkButton).Tag.ToString(), (sender as HyperlinkButton).Content });
        }

        /// <summary>
        /// 博客列表开始加载
        /// </summary>
        private void _list_blogs_DataLoading()
        {
            Loading.IsActive = true;
        }
        /// <summary>
        /// 博客列表加载完毕
        /// </summary>
        private void _list_blogs_DataLoaded()
        {
            Loading.IsActive = false;
            ListCount.Text = _list_blogs.TotalCount.ToString();
        }
        /// <summary>
        /// 刷新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            _list_blogs.DoRefresh();
            await BlogsListView.LoadMoreItemsAsync();
        }
        /// <summary>
        /// 点击推荐
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            await (new MessageDialog("点击查看全文再去推荐哟!")).ShowAsync();
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
    }
}
