using Emotion.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Emotion.Core.Common
{
   /// <summary>
   /// 用于存放系统全局变量
   /// </summary>
   public class CurrUserLogin
    {
        private static CurrUserLogin _instance = null;
        /// <summary>
        /// 取得实例
        /// </summary>
        /// <returns>实例</returns>
        public static CurrUserLogin GetInstance()
        {
            if (_instance == null)
            {
                _instance = new CurrUserLogin();
            }
            return _instance;
        }
        private int _newListCount = 999;
        /// <summary>
        /// 最新的表情包总页数
        /// </summary>
        public static int NewListCount
        {
            get { return GetInstance()._newListCount; }
            set { GetInstance()._newListCount = value; }
        }
    }
}
