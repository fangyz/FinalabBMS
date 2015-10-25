using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace MVC.Helper
{
    public class GetAbnormal
    {
        /// <summary>
        /// 处理异常，将异常保存到数据库
        /// </summary>
        /// <param name="filterContext"></param>
        public void Abnormal(ExceptionContext filterContext)
        {
            MODEL.T_Abnormal abnormal = new MODEL.T_Abnormal();
            string stack = filterContext.Exception.StackTrace;
            string[] str = stack.Split('.');
            string area = str[0];
            string controller = str[1];
            string action = str[2];
            string[] str1 = action.Split('(');
            string reallyaction = str1[0];
            abnormal.Area = area;
            abnormal.Controller = controller;
            abnormal.ACtion = reallyaction;
            abnormal.Message = filterContext.Exception.Message;
            OperateContext.Current.BLLSession.IAbnormalBLL.Add(abnormal);
            //接下来在配置文件设置重定向
            //注意：customErrors要放在system.web下


            //string filePath = Server.MapPath("~/ExcelModel/Exception.txt");
            //FileInfo file = new FileInfo(filePath);
            //if (!file.Exists)
            //{
            //    file.Create().Close();
            //}
            //StreamWriter sw = System.IO.File.AppendText(filePath);
        }
    }
}
