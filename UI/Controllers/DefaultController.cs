using MVC.Helper;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Transactions;
using System.Globalization;

namespace UI.Controllers
{
    public class DefaultController : Controller
    {
        public ActionResult Index()
        {
            return  Redirect("/login/login/index");
        //    FileStream fs = new FileStream(Server.MapPath("/6.xls"), FileMode.Open, FileAccess.ReadWrite, FileShare.None);//创建文件流
        //    DataTable dt = new XlsStreamToDT(fs, 15).Xls2DT();
        //    using (var scope = new TransactionScope())
        //    {
        //        for (int i = 1; i <= 16; i++)
        //        {
        //            int? department;
        //            string StuNum = dt.Rows[i][0].ToString();//获取学号
        //            string StuName = dt.Rows[i][1].ToString();//获取姓名
        //            string Gender = dt.Rows[i][2].ToString();//获取性别
        //            string QQ = dt.Rows[i][3].ToString();//获取QQ
        //            string TelephoneNumber = dt.Rows[i][4].ToString();//获取电话号码
        //            string Email = dt.Rows[i][5].ToString();//获取Email
        //            string HeadTeacher = dt.Rows[i][6].ToString();//获取班主任
        //            string Class = dt.Rows[i][7].ToString();//获取班级
        //            string Counselor = dt.Rows[i][8].ToString();//获取辅导员
        //            //string TechnicalGuideNumber = dt.Rows[i][11].ToString();//获取技术指导
        //            string DepartmentStr = dt.Rows[i][11].ToString();//获取部门编号
        //            //if (TechnicalGuideNumber == "无")
        //            //    TechnicalGuideNumber = null;
        //            if (DepartmentStr == "无")
        //                department = null;
        //            else
        //                department = Convert.ToInt32(DepartmentStr);
        //            string time = dt.Rows[i][9].ToString();//获取加入实验室时间
        //            DateTime JoinTime = DateTime.ParseExact(time, "yyyyMMdd", System.Globalization.CultureInfo.CurrentCulture);
        //            int TechnicalLevel = Convert.ToInt32(dt.Rows[i][14].ToString());//获取学号



        //            MODEL.T_MemberInformation member = new MODEL.T_MemberInformation()
        //            {
        //                StuNum = StuNum,
        //                StuName = StuName,
        //                Gender = Gender,
        //                QQNum = QQ,
        //                TelephoneNumber = TelephoneNumber,
        //                Email = Email,
        //                HeadTeacher = HeadTeacher,
        //                Class = Class,
        //                Counselor = Counselor,
        //                JoinTime = JoinTime,
        //                TechnicalGuideNumber = null,
        //                Department = department,
        //                TechnicalLevel = TechnicalLevel,
        //                LoginPwd = "123456",
        //                PhotoPath = "~/final.png",
        //                Major = "计算机"

        //            };
        //            OperateContext.Current.BLLSession.IMemberInformationBLL.Add(member);
        //            string Organization = dt.Rows[i][12].ToString();//组织编号
        //            if (Organization != "无")
        //            {
        //                string[] OrganizationStr = Organization.Split(new char[] { ';' });
        //                for (int j = 0; j < OrganizationStr.Length; j++)
        //                {
        //                    MODEL.T_OgnizationAct OrganizationAct = new MODEL.T_OgnizationAct() { OrganizationId = Convert.ToInt32(OrganizationStr[j]), RoleActor = StuNum, IsDel = false };
        //                    OperateContext.Current.BLLSession.IOgnizationActBLL.Add(OrganizationAct);
        //                }
        //            }

        //            string position = dt.Rows[i][13].ToString();
        //            if (position != "无")
        //            {
        //                if (position.Contains(";"))
        //                {
        //                    string[] positionStr = position.Split(new char[] { ';' });
        //                    for (int j = 0; j < positionStr.Length - 1; j++)
        //                    {
        //                        MODEL.T_RoleAct roleAct = new MODEL.T_RoleAct() { RoleId = Convert.ToInt32(positionStr[j]), RoleActor = StuNum, IsDel = false };
        //                        OperateContext.Current.BLLSession.IRoleActBLL.Add(roleAct);
        //                    }
        //                }
        //                else
        //                {
        //                    MODEL.T_RoleAct roleAct = new MODEL.T_RoleAct() { RoleId = Convert.ToInt32(position), RoleActor = StuNum, IsDel = false };
        //                    OperateContext.Current.BLLSession.IRoleActBLL.Add(roleAct);
        //                }
        //            }
        //        }
        //        scope.Complete();
        //    }
            //return View();
        }
    }
}
