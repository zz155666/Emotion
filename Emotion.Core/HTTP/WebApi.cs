using Emotion.Core.Common;
using Emotion.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Json;

namespace Emotion.Core.HTTP
{
  public static class WebApi
    {
      /// <summary>
      /// 测试服务器地址
      /// </summary>
      // private static string home = "http://182.92.101.144:8080/ibiaoqing/";
       /// <summary>
       /// 正式服务器地址
       /// </summary>
      private static string home = "http://123.57.155.230:8080/ibiaoqing/";
        private static string biaoqinglist = home + "admin/expre/listBy.do";
        private static string imageList = home + "admin/expre/getByeId.do";
      /// <summary>
      /// 获取表情包列表
      /// </summary>
      /// <param name="currentpage">当前页数</param>
      /// <returns></returns>
        public async static Task<List<HomeItem>> GetRecentBiaoQingList(int currentpage)
        {
            List<HomeItem> home_item = new List<HomeItem>();
            string url = biaoqinglist + "?status=Y&status1=B&pageNumber="+ currentpage;
            string json = await BaseService.SendGetRequest(url);
            string jsonstring = "{\"json\": " + json + "}";
            JsonObject jsonObject = JsonObject.Parse(jsonstring);
            JsonArray jsonarray = jsonObject.GetNamedArray("json");
            var pp = jsonarray[0];
            if (jsonarray[0].GetString().Equals("OK"))
            {
                foreach (var p in jsonarray[2].GetArray())
                {
                    JsonObject jsonprivate = p.GetObject() as JsonObject;
                    HomeItem home = CopyToT<HomeItem>(jsonprivate, new HomeItem());
                    home_item.Add(home);
                }
                JsonArray count = jsonarray[1].GetArray();
                if(count!=null&& count.Count > 2)
                {
                    CurrUserLogin.NewListCount = Convert.ToInt32(count[1].GetNumber());
                }
            }
            return home_item;
        }
      /// <summary>
      /// 获取表情包内图片列表
      /// </summary>
      /// <param name="eid">表情包ID</param>
      /// <returns></returns>
        public async static Task<List<ImageContent>> GetReCentIamgeList(int eid)
        {
            List<ImageContent> Imagelist = new List<ImageContent>();
            string url = imageList + "?eId=" + eid;
            string json = await BaseService.SendGetRequest(url);
            string jsonstring = "{\"json\": " + json + "}";
            JsonObject jsonObject = JsonObject.Parse(jsonstring);
            JsonArray jsonarray = jsonObject.GetNamedArray("json");
            var pp = jsonarray[0];
            if (jsonarray[0].GetString().Equals("OK"))
            {
                foreach (var p in jsonarray[2].GetArray())
                {
                    JsonObject jsonprivate = p.GetObject() as JsonObject;
                    ImageContent home = CopyToT<ImageContent>(jsonprivate, new ImageContent());
                    Imagelist.Add(home);
                }
                //JsonArray count = jsonarray[1].GetArray();
                //if (count != null && count.Count > 2)
                //{
                //    CurrUserLogin.NewListCount = Convert.ToInt32(count[1].GetNumber());
                //}
            }
            return Imagelist;
        }
        /// <summary>
        /// 利用反射获取json对象的属性
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="newobj"></param>
        /// <returns></returns>
        private static T CopyToT<T>(JsonObject obj, object newobj)
        {
            T t = default(T);
            if (newobj == null)
            {
                return t;
            }
            t = (T)newobj;
            if (obj == null)
            {
                return t;
            }
            PropertyInfo[] propertyInfos = t.GetType().GetProperties();//得到原对象所有属性
            foreach (PropertyInfo pi in propertyInfos)//循环对象属性
            {
                string name = pi.Name;
                try
                {
                    var pp = pi.PropertyType.Name;
                    object valuenew;
                    switch (pp)
                    {
                        case "String":
                            pi.SetValue(newobj, obj.GetNamedString(name));
                            break;
                        case "Int32":
                            try {
                                valuenew = Convert.ToInt32(obj.GetNamedNumber(name));
                            }
                            catch (Exception e)
                            {
                                valuenew = 1;
                            }
                            pi.SetValue(newobj, valuenew);
                            break;
                    }
                }
                catch (Exception e)
                {
                    //
                }
               
            }
                return t;
        }
        /// <summary>
        /// 使用反射将一个对象的值赋值给另一个对象
        /// </summary>
        /// <param name="obj">原对象</param>
        /// <param name="newobj">新的对象</param>
        /// <returns></returns>
        public static T CopyToT<T>(object obj, object newobj)
        {
            T t = default(T);
            if (newobj == null)
            {
                return t;
            }
            t = (T)newobj;
            if (obj == null)
            {
                return t;
            }

            PropertyInfo[] propertyInfos = obj.GetType().GetProperties();//得到原对象所有属性
            Type ty = t.GetType();
            if (propertyInfos.Length < 0)
            {
                return t;
            }
            foreach (PropertyInfo pi in propertyInfos)//循环对象属性
            {
                string name = pi.Name;
                object value = pi.GetValue(obj, null);
                BindingFlags flag = BindingFlags.Public | BindingFlags.IgnoreCase | BindingFlags.Instance;//忽略属性名称大小写
                var p = ty.GetProperty(name, flag);//根据原对象属性名称得到新对象属性
                if (p != null)
                {
                    p.SetValue(t, value, null);//赋值
                }
            }
            return t;
        }
    }
}
