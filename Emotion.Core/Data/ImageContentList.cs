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
    /// 表情包内详细的图片列表
    /// </summary>
   public class ImageContentList : ObservableCollection<ImageContent>, ISupportIncrementalLoading
    {
        private bool _busy = false;
        private bool _has_more_items = false;
        private int _eid;
        public event DataLoadingEventHandler DataLoading;
        public event DataLoadedEventHandler DataLoaded;

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
        public ImageContentList(int eid)
        {
            _eid = eid;
            HasMoreItems = true;
        }

        public void DoRefresh(int eid)
        {
            _eid = eid;
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
            List<ImageContent> list = null;
            try
            {
                if (DataLoading != null)
                {
                    DataLoading();
                }
                    list = await WebApi.GetReCentIamgeList(_eid);
            }
            catch (Exception ex)
            {
                HasMoreItems = false;
            }

            if (list != null && list.Any())
            {
                actualCount = list.Count;
                HasMoreItems = false;
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
