using Emotion.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;
using Windows.Foundation;
using Emotion.Core.HTTP;
using Emotion.Core.Common;

namespace Emotion.Core.Data
{
    /// <summary>
    /// 表情包列表
    /// </summary>
   public class HomeItemList : ObservableCollection<HomeItem>, ISupportIncrementalLoading
    {
        private bool _busy = false;
        private bool _has_more_items = false;
        private int _current_page = 1;
        /// <summary>
        /// 是否最热；
        /// </summary>
        private bool _isHot;
        /// <summary>
        /// 搜索关键字
        /// </summary>
        private string _eName;
        public event DataLoadingEventHandler DataLoading;
        public event DataLoadedEventHandler DataLoaded;
        public int TotalCount
        {
            get; set;
        }

        public bool HasMoreItems
        {
            get
            {
                if (_busy)
                    return false;
                else
                    return _has_more_items;
            }
            private set
            {
                _has_more_items = value;
            }
        }
        public HomeItemList(bool isHot=false,string eName="")
        {
            HasMoreItems = true;
            _isHot = isHot;
            _eName = eName;
        }

        public void DoRefresh()
        {
            _current_page = 1;
            TotalCount = 0;
            Clear();
            HasMoreItems = true;
        }

        public IAsyncOperation<LoadMoreItemsResult> LoadMoreItemsAsync(uint count)
        {
            return InnerLoadMoreItemsAsync(count).AsAsyncOperation();
        }
        private async Task<LoadMoreItemsResult> InnerLoadMoreItemsAsync(uint expectedCount)
        {
            _busy = true;
            var actualCount = 0;
            List<HomeItem> list = null;
            try
            {
                if (DataLoading != null)
                {
                    DataLoading();
                }
               
                list = await WebApi.GetRecentBiaoQingList(_current_page, _isHot, _eName);
            }
            catch (Exception ex)
            {
                HasMoreItems = false;
            }

            if (list != null && list.Any())
            {
                actualCount = list.Count;
                TotalCount += actualCount;
                _current_page++;
                HasMoreItems = true;
                list.ForEach((c) => { this.Add(c); });
            }
            else
            {
                HasMoreItems = false;
            }
            if (DataLoaded != null)
            {
                DataLoaded();
            }
            _busy = false;
            return new LoadMoreItemsResult
            {
                Count = (uint)actualCount
            };
        }
    }
}
