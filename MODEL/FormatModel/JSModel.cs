using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MODEL.FormatModel
{
    /// <summary>
    /// 统一的 JS格式类
    /// </summary>
    public static class JSModel
    {
        public static string ContentString(string msg, string nextUrl)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<script>");
            sb.Append("alert('" + msg + "');");
            if (nextUrl != null)
            {
                sb.Append("window.location='" + nextUrl + "'");
            }
            sb.Append("</script>");
            return sb.ToString();
        }

        public static string ContentString(string msg)
        {
            return ContentString(msg,null);
        }
    }
}
