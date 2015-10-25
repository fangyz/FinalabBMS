using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;
namespace Common
{
    public static class DataHelper
    {
        /// <summary>
        /// js 序列化器
        /// </summary>
        static JavaScriptSerializer jss = new JavaScriptSerializer();

        /// <summary>
        /// 将 对象 转成 json格式字符串
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string Obj2Json(object obj)
        {
            //把集合 转成 json 数组格式字符串
            return jss.Serialize(new { menus=obj});
        }

        /// <summary>
        /// 返回 MD5 加密字符串
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string MD5(string str)
        {
            return System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(str, "MD5");
        }
    }
}
