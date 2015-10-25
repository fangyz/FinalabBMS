using MODEL.DTO;
using MODEL.ViewModel;
using MVC.Helper;
using P01MVCAjax.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Globalization;
using System.Transactions;
using System.Data;

namespace PersonalManger
{
    /// <summary>
    /// 展示人员信息和修改人员信息
    /// </summary>
    public class CheckMemberController : Controller
    {
        //公用方法
        #region 0.1通过SecreShowId得到私密属性是否公开 public void GetSecret(int secretId, ref int familyTel, ref int address, ref int phone)
        public void GetSecret(int secretId, ref int familyTel, ref int address, ref int phone)
        {
            for (int num1 = 0; num1 <= 1; num1++)
            {
                for (int num2 = 0; num2 <= 1; num2++)
                {
                    for (int num3 = 0; num3 <= 1; num3++)
                    {
                        if (num1 * 4 + 2 * num2 + num3 == secretId)
                        {
                            familyTel = num1;
                            address = num2;
                            phone = num3;
                        }
                    }
                }
            }
        }
        #endregion
        #region 0.2确定年级 public void GetYear(out string dtone, out string dttwo, out string dtthree, out string dtfour)
        public void GetYear(out string dtone, out string dttwo, out string dtthree, out string dtfour)
        {
            DateTime dt = DateTime.Now;
            if (dt.Month >= 9)
            {
                dtone = dt.Year.ToString();
                dttwo = (dt.Year - 1).ToString();
                dtthree = (dt.Year - 2).ToString();
                dtfour = (dt.Year - 3).ToString();
            }
            else
            {
                dtone = (dt.Year - 1).ToString();
                dttwo = (dt.Year - 2).ToString();
                dtthree = (dt.Year - 3).ToString();
                dtfour = (dt.Year - 4).ToString();
            }
        } 
        #endregion

        //个人信息 模块
        #region 1.1点击个人信息链接，个人信息展示页面    ViewResult  PersonPage()
        public ViewResult PersonPage()
        {
            string stunum =Request.QueryString["StuNum"];//这个链接是从成员信息过来的
            string backurl = Request.QueryString["backurl"];
            if (string.IsNullOrEmpty(backurl))
            {
                ViewBag.backurl = "fromperson";
            }
            else
            {
                if (backurl == "fromindex")
                {
                    ViewBag.backurl = "fromindex";
                }
                if (backurl == "fromrole")
                {
                    ViewBag.backurl = "fromrole";
                }
            }
            if (string.IsNullOrEmpty(stunum))
            {
                stunum = OperateContext.Current.Usr.StuNum;
            }
            /*判断每个人是否具有修改的权限*/
            string IsEdit;
            //如果是管理员或者是自己本身则具有修改权限
            if (OperateContext.Current.HasPemission("PersonalManger", "CheckMember", "AdminEdit", "post")||
                stunum.Equals(OperateContext.Current.Usr.StuNum))
            {
                IsEdit = "True";
                ViewBag.IsEdit = IsEdit;
            }
            else
            {
                IsEdit = "False";
                ViewBag.IsEdit = IsEdit;
            }
            //拿到此用户信息的对象
            MODEL.T_MemberInformation member = OperateContext.Current.BLLSession.IMemberInformationBLL.GetListBy(u => u.StuNum == stunum).FirstOrDefault();
            ViewBag.member = member;
            //设置私密信息
            int familyTel = 0;
            int address = 0;
            int phone = 0;
            GetSecret(member.SecretShow, ref familyTel, ref address, ref phone);
            ViewBag.familyTel = familyTel;
            ViewBag.address = address;
            ViewBag.phone = phone;
            //获取该人担任的职务
            string RoleString = "";
            List<string> role = member.T_RoleAct.Select(u => u.T_Role.RoleName).ToList();
            for (int i = 0; i < role.Count; i++)
            {
                if (i > 2)
                {
                    RoleString = RoleString + "...";
                    break;
                }
                RoleString = RoleString + " " + role[i];
            }
            ViewBag.RoleString = RoleString;
            return View();
        } 
        #endregion
        #region 1.2点击编辑按钮，修改个人的基本信息+public ViewResult PageEdit()
        [Common.Attributes.Skip]
        public ViewResult PageEdit()
        {
            //得到返回的页面
            string backurl = Request.QueryString["backurl"];
            if (!string.IsNullOrEmpty(backurl))
            {
                if (backurl == "fromindex")
                {
                    ViewBag.backurl = "/PersonalManger/CheckMember/Index";
                }
                if (backurl == "fromrole")
                {
                    ViewBag.backurl = "/Permission/Role/RoleIndex";
                }
                if (backurl == "fromperson")
                {
                    ViewBag.backurl = "/PersonalManger/CheckMember/PersonPage";
                }
            }
            else
            {
                ViewBag.backurl = "/PersonalManger/CheckMember/PersonPage";
            }
            string stunum = Request["StuNum"];//得到要修改学生的学号
            string user = OperateContext.Current.Usr.StuNum;//拿到登入用户的学号
            if (user == "FinalAdmin")//注意：这里是直接用登入者账号是否等于数据库的系统维护人员的账号的。注意！！！
            {
                int UserRoleId;
                ViewBag.user = "admin";
                //这里是得到要修改成员的角色
                UserRoleId = OperateContext.Current.BLLSession.IRoleActBLL.GetListBy(u => u.RoleActor == stunum).First().RoleId;
                List<MODEL.T_Role> listRole=OperateContext.Current.BLLSession.IRoleBLL.GetListBy(u => u.IsDelete == false).ToList();
                ViewBag.listRole = listRole;
                ViewBag.UserRoleId = UserRoleId;
            }
            else
            {
                ViewBag.user = "notAdmin";
            }
            //确定年级
            string dtone;
            string dttwo;
            string dtthree;
            string dtfour;
            GetYear(out  dtone, out  dttwo, out  dtthree, out  dtfour);
            ViewBag.IsShow = 1;
            string  datetime = stunum.Substring(0, 4).ToString();
            //通过年级来设置学习顾问，组织等信息；大二和大三成员只能看到自己的学习顾问,管理员也无法修改
            #region 1初始化下拉框的信息
            List<MODEL.T_Department> dep = OperateContext.Current.BLLSession.IDepartmentBLL.GetListBy(u => u.DepartmentId > 0);
            List<MODEL.T_TechnicaLevel> techLeval = OperateContext.Current.BLLSession.ITechnicaLevelBLL.GetListBy(u => u.TechLevelId > 0);
            List<MODEL.T_MemberInformation> StudyGuide = OperateContext.Current.BLLSession.IMemberInformationBLL.GetListBy(u =>(u.T_RoleAct.Select(p => p.RoleId).Contains(Position.StudyMember) || u.T_RoleAct.Select(p => p.RoleId).Contains(Position.StudyLeader)));
            List<MODEL.T_Organization> organization = OperateContext.Current.BLLSession.IOrganizationBLL.GetListBy(u => u.OrganizationId > 0);
            //选择信息赋值,初始化部门，组织，技术水平下拉框
            ViewBag.dep = dep;
            ViewBag.techLeval = techLeval;
            ViewBag.StudyGuide = StudyGuide;
            ViewBag.organization = organization;
            #endregion
            #region 2个人信息赋初始值
            //成员的基本信息
            MODEL.T_MemberInformation member = OperateContext.Current.BLLSession.IMemberInformationBLL.GetListBy(u => u.StuNum == stunum).FirstOrDefault();
            if (OperateContext.Current.HasPemission("PersonalManger", "CheckMember", "AdminEdit", "3"))
            {
                ViewBag.HasPer = true;
                ViewBag.urlfix = "AdminEdit";
            }
            else
            {
                ViewBag.urlfix = "Edit";
            }
            ViewBag.member = member;
            int familyTel = 0;         //设置私密信息
            int address = 0;
            int phone = 0;
            GetSecret(member.SecretShow, ref familyTel, ref address, ref phone);
            ViewBag.familyTel = familyTel;
            ViewBag.address = address;
            ViewBag.phone = phone;
            string dtime = "";       //为生日赋初始值，生日在数据库是可为空类型，直接赋值报错
            if (member.Birthday != null)
            {
                dtime = member.Birthday.ToString().Substring(0, 10);
            }
            ViewBag.dtime = dtime;
            ViewBag.IsOne = 0;    //判断成员是不是大一成员
            if (dtone == datetime)
            {
                ViewBag.IsOne = 1;
            }
            //为专业下拉框设值
            string[] Professional = { "计算机科学技术", "网络工程", "通信工程", "软件工程", "设计艺术", "市场营销", "其他" };
            ViewBag.Professional = Professional;
            int count = Professional.Count();
            ViewBag.count = count;
            #endregion
            #region 3成员自己的实验室信息赋初始值
            ViewBag.StuGuide = "";
            ViewBag.org = "";
            ViewBag.depName = "";
            try
            {
                ViewBag.StuGuide = OperateContext.Current.BLLSession.IMemberInformationBLL.GetListBy(u => u.StuNum == member.StudyGuideNumber).First().StuName;
                int orgId = OperateContext.Current.BLLSession.IOgnizationActBLL.GetListBy(o => o.RoleActor == member.StuNum).First().OrganizationId;
                string orgName = OperateContext.Current.BLLSession.IOrganizationBLL.GetListBy(u => u.OrganizationId == orgId).First().OrganizationName;
                ViewBag.orgName = orgName;
                ViewBag.orgId = orgId;
                ViewBag.depName = OperateContext.Current.BLLSession.IDepartmentBLL.GetListBy(u => u.DepartmentId == member.Department).First().DepartmentName;
            }
            catch (Exception ex) { }
            #endregion 
            return View();
        }
        #endregion
        #region  1.3点击保存按钮，保存修改后的数据   AdminEdit(MODEL.T_MemberInformation member)
        /// <summary>
        /// 管理员修改方法，可以修改所有信息
        /// </summary>
        /// <param name="member"></param>
        /// <returns></returns>
        public ActionResult AdminEdit(MODEL.T_MemberInformation member)
        {
            int oldRoleId = 0;
            try
            {
                oldRoleId=Convert.ToInt32(Request.Form["oldRoleId"]);
                //查看角色属性
                int roleId = 0;
                OperateContext.Current.BLLSession.IRoleActBLL.DelBy(u => u.RoleId == oldRoleId && u.RoleActor == member.StuNum);
                object o = Request.Form["role"];
                if (Request.Form["role"] != null)
                {
                    roleId = Convert.ToInt32(Request.Form["role"]);
                    MODEL.T_RoleAct roleAct = new MODEL.T_RoleAct();
                    roleAct.RoleId = roleId;
                    roleAct.RoleActor = member.StuNum;
                    OperateContext.Current.BLLSession.IRoleActBLL.Add(roleAct);
                }
            }
            catch
            {

            }
            //得到修改SecretShow属性
            int ShowFPhone = 2;
            int ShowAddress = 2;
            int ShowPhone = 2;
            int IsShow;
            ShowAddress = Convert.ToInt32(Request.Form["ShowAddress"]);//设置部分信息是否可以看到
            ShowFPhone = Convert.ToInt32(Request.Form["ShowFPhone"]);
            ShowPhone = Convert.ToInt32(Request.Form["ShowPhone"]);
            IsShow = 4 * ShowFPhone + 2 * ShowAddress + ShowPhone;//得到信息公开数据
            member.SecretShow = IsShow;
            if (ModelState.IsValid)
            {
                //EF修改主键一定要加
                string[] proNames = new string[] {"StuNum","StuName", "Gender", "QQNum","Email","Birthday" , "Class", "Major","Counselor","HeadTeacher",
                        "UndergraduateTutor", "TelephoneNumber", "HomPhoneNumber", "FamilyAddress", "Department", "TechnicalLevel", "StudyGuideNumber", "Sign", "OtheInfor", "SecretShow"};
                 int IsSuccess= OperateContext.Current.BLLSession.IMemberInformationBLL.Modify(member, proNames);
                if (IsSuccess > 0)
                    return Content("<script>alert('修改成功');window.location='/PersonalManger/CheckMember/PersonPage?StuNum=" + member.StuNum + "'</script>");
                else
                {
                    return Content("<script>alert('修改失败');window.location='/PersonalManger/CheckMember/PageEdit?StuNum=" + member.StuNum + "'</script>");
                }
            }
            else
            {
                return Content("<script>alert('修改失败');window.location='/PersonalManger/CheckMember/PageEdit?StuNum=" + member.StuNum + "'</script>");
            }

        }
        #endregion
        #region 1.4普通成员编辑的自己的信息+public ActionResult Edit(MODEL.T_MemberInformation member)
        /// <summary>
        /// 普通成员编辑的自己的信息
        /// </summary>
        /// <param name="member"></param>
        /// <returns></returns>
        [Common.Attributes.Skip]
        public ActionResult Edit(MODEL.T_MemberInformation member)
        {
            int ShowFPhone = 2;
            int ShowAddress = 2;
            int ShowPhone = 2;
            int IsShow;
            ShowAddress = Convert.ToInt32(Request.Form["ShowAddress"]);//设置部分信息是否可以看到
            ShowFPhone = Convert.ToInt32(Request.Form["ShowFPhone"]);
            ShowPhone = Convert.ToInt32(Request.Form["ShowPhone"]);
            IsShow = 4 * ShowFPhone + 2 * ShowAddress + ShowPhone;//得到信息公开数据
            member.SecretShow = IsShow;
            try
            {
                if (ModelState.IsValid)
                {
                    /*EF修改主键一定要加*/
                    string[] proNames = new string[] { "StuNum", "StuName", "Gender", "Email", "LoginPwd", "Class", "Major", "Counselor", "HeadTeacher", "UndergraduateTutor", "TelephoneNumber","Birthday",
                        "HomPhoneNumber","FamilyAddress","Sign","OtheInfor","SecretShow"};
                    OperateContext.Current.BLLSession.IMemberInformationBLL.Modify(member, proNames);
                    return Content("<script>alert('修改成功');window.location='/PersonalManger/CheckMember/PersonPage?StuNum=" + member.StuNum + "'</script>");
                }
                else
                {
                    return Content("<script>alert('修改成功');window.location='/PersonalManger/CheckMember/PageEdit?StuNum=" + member.StuNum + "'</script>");
                }
            }
            catch (Exception ex)
            {
                return Content("<script>alert('修改成功');window.location='/PersonalManger/CheckMember/PageEdit?StuNum=" + member.StuNum + "'</script>");
            }
        }
        #endregion
        #region 1.5上传头像+public ActionResult UpLoadImg(HttpPostedFileBase UpLoadImg)
        /// <summary>
        /// 上传头像
        /// </summary>
        /// <param name="UpLoadImg"></param>
        /// <returns></returns>
        [Common.Attributes.Skip]
        public ActionResult UpLoadImg()
        {
            HttpPostedFileBase head = Request.Files["head"];
            string StuNum = Request["StuNum"];
            string fileName = head.FileName;
            string ext = Path.GetExtension(fileName).ToLower();
            if (!ext.Equals(".gif") && !ext.Equals(".jpg") && !ext.Equals(".png") && !ext.Equals(".bmp"))
            {

                return Content("<script>alert('您上传的文件格式不正确！上传格式有(.gif、.jpg、.png、.bmp)')</script>");
            }
            else if (head.ContentLength > 1048576 * 5)
            {
                return Content("<script>alert('内容最大为5M')</script>");
            }
            else
            {
                try
                {
                    head.SaveAs(Server.MapPath("~/HeadImg/" + fileName));
                    string ImagePath = "../../HeadImg/" + fileName;
                    MODEL.T_MemberInformation user = new MODEL.T_MemberInformation() { StuNum = StuNum, PhotoPath = ImagePath };
                    OperateContext.Current.BLLSession.IMemberInformationBLL.Modify(user, new string[] { "PhotoPath" });
                    return Content("<script>alert('修改成功')</script>");
                }
                catch (Exception ex)
                {
                    return Content("<script>alert('修改失败')</script>");
                }
            }
        }
        #endregion

        //成员信息 模块
        #region 2.1点击成员信息链接 返回成员信息查看列表页面，编辑信息页面   public ActionResult Index()
        public ActionResult Index()
        {
            string backurl = Request.QueryString["backurl"];
            if (string.IsNullOrEmpty(backurl))
            {
                ViewBag.backurl = "fromindex";
            }
            else
            {
                ViewBag.backurl = "fromrole";
            }
            /*判断每个人是否具有修改的权限*/
            string IsEdit;
            if (OperateContext.Current.HasPemission("PersonalManger", "CheckMember", "AdminEdit", "post"))
            {
                IsEdit = "True";
                ViewBag.IsEdit = IsEdit;
            }
            else
            {
                IsEdit = "False";
                ViewBag.IsEdit = IsEdit;
            }
            //初始化部门
            List<MODEL.T_Department> depa = new List<MODEL.T_Department>();
            depa=OperateContext.Current.BLLSession.IDepartmentBLL.GetListBy(u => u.IsDelete == false).ToList();
            ViewBag.depa = depa;
            return View();
        }
        #endregion
        #region 2.2成员分页，获取数据+public ActionResult GetPageData(FormCollection form)
        public ActionResult GetPageData(FormCollection form)
        {
            //确定年级
            DateTime dt = DateTime.Now;
            string dtone;
            string dttwo;
            string dtthree;
            string dtfour;
            GetYear(out  dtone, out  dttwo, out  dtthree, out  dtfour);
            //声明查询的变量
            Expression<Func<MODEL.T_MemberInformation, bool>> whereLambda;
            string dataBy = form["dataBy"];//模糊查寻的条件
            int pageIndex = Convert.ToInt32(form["pageIndex"]);
            if (!string.IsNullOrEmpty(dataBy))
            {
                string[] search = dataBy.Split(new char[] { ',' });
                //分离3个参数
                string dtime = "";
                int? depId = Convert.ToInt32(search[0]);//拿到部门id
                int year = Convert.ToInt32(search[1]);
                if (year != 6)
                {
                    if (year == 1) { dtime = dtone; }
                    if (year == 2) { dtime = dttwo; }
                    if (year == 3) { dtime = dtthree; }
                    if (year == 4) { dtime = dtfour; }
                }
                string numOrName = search[2];
                //动态添加是否删除，部门，年级lambda表达式
                BinaryExpression queryDel;
                BinaryExpression queryAdmin;
                BinaryExpression queryDep;
                BinaryExpression query;
                ParameterExpression parameter = Expression.Parameter(typeof(MODEL.T_MemberInformation), "u");
                //初始化参数一，为是否删除加上条件
                ConstantExpression condel = Expression.Constant(false);
                MemberExpression member = Expression.PropertyOrField(parameter, "IsDelete");
                queryDel = Expression.Equal(member, condel);
                //设置不为admin
                ConstantExpression conNotAdmin = Expression.Constant(false);
                MemberExpression mem = Expression.PropertyOrField(parameter, "IsAdmin");
                queryAdmin = Expression.Equal(mem, conNotAdmin);
                queryDel = Expression.And(queryDel, queryAdmin);
                //初始化参数二：判断部门
                if (depId != 1)
                {
                    ConstantExpression condep = Expression.Constant(depId);
                    MemberExpression member1 = Expression.PropertyOrField(parameter, "Department");
                    queryDep = Expression.Equal(member1, condep);
                    query = Expression.And(queryDel, queryDep);
                }
                else
                {
                    query = queryDel;
                }
                //动态添加年级
                if (year != 6)
                {
                    var con = Expression.Constant(dtime);
                    Expression expression = Expression.Constant(false);
                    Expression right = Expression.Call(Expression.Property(parameter, typeof(MODEL.T_MemberInformation).GetProperty("StuNum")),
                        typeof(string).GetMethod("Contains", new Type[] { typeof(string) }), con);
                    query = Expression.And(query, right);
                }
                whereLambda = Expression.Lambda<Func<MODEL.T_MemberInformation, bool>>(query, parameter);
                //当输入学号或姓名时，只需根据这个条件查找
                if (!string.IsNullOrEmpty(numOrName))
                {
                    whereLambda = u => u.IsDelete == false && (u.StuNum.Contains(numOrName) || u.StuName.Contains(numOrName)) && u.StuNum != "FinalAdmin";
                }
            }
            else
            {
                whereLambda = u => u.IsDelete == false && u.IsAdmin == false;
            }
            try
            {
                return PageData(whereLambda, pageIndex);
            }
            catch (Exception ex)
            {
                return   OperateContext.Current.RedirectAjax("err", null, null, null);
            }
        }
        #endregion
        #region 2.3根据id删除成员  ActionResult DeleteBy(FormCollection form)
        /// <summary>
        /// 根据id删除成员
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        public ActionResult DeleteBy(FormCollection form)
        {
            string stuNum = form["stuNum"];
            MODEL.T_MemberInformation model = new MODEL.T_MemberInformation();
            model.IsDelete = true;
            int delCount = OperateContext.Current.BLLSession.IMemberInformationBLL.ModifyBy(model, u => u.StuNum == stuNum, "IsDelete");
            if (delCount > 0)
            {
                return OperateContext.Current.RedirectAjax("ok", "删除成功", null, null);
            }
            else
            {
                return OperateContext.Current.RedirectAjax("err", "删除失败", null, null);
            }
        }
        #endregion
        #region 2.4获取分页数据，共同调用，PageData需要调用 ActionResult PageData(Expression<Func<MODEL.T_MemberInformation, bool>> whereLambda, int pageIndex)
        public ActionResult PageData(Expression<Func<MODEL.T_MemberInformation, bool>> whereLambda, int pageIndex)
        {
            int totalRecord;
            int pageSize = 10;//页容量固定死为10
            try//为什么异常没有捕捉到
            {
                var list = OperateContext.Current.BLLSession.IMemberInformationBLL.GetPagedList(pageIndex, pageSize,
                   whereLambda, u => u.StuNum, out totalRecord).Select(u
                     => new MemberInformationDTO()
                     {
                         StuNum = u.StuNum,
                         StuName = u.StuName,
                         Major = u.Major,
                         TelephoneNumber = u.TelephoneNumber,
                         Year = (Convert.ToInt32(DateTime.Now.Year) - Convert.ToInt32(u.StuNum.Substring(0, 4))).ToString() + "年级",
                         Department = u.T_Department == null ? "无" : u.T_Department.DepartmentName,
                     });

                JsonModel json;
                PageModel pageModel = new PageModel()
                {
                    TotalRecord = totalRecord,
                    data = list
                };

                json = new JsonModel()
                {
                    Data = pageModel,
                    BackUrl = "",
                    Statu = "ok",
                    Msg = "成功"
                };

                JsonResult jr = new JsonResult();
                jr.Data = json;
                jr.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
                return jr;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        #endregion
        #region 2.5导出部分数据到excel FileResult  ExportMsg()
        public ActionResult   ExportMsg()
        {
            List<String> list = new List<String>();
            List<String> listName = new List<String>();
            #region 初始化数据
            //得到选中项 这是比较笨的方法，有机会要改进哈
            if (Request.Form["StuNum"] != null) { list.Add("StuNum"); listName.Add("学号"); }
            if (Request.Form["StuName"] != null) { list.Add("StuName"); listName.Add("姓名"); }
            if (Request.Form["Gender"] != null) { list.Add("Gender"); listName.Add("性别"); }
            if (Request.Form["QQNum"] != null) { list.Add("QQNum"); listName.Add("QQ号"); }
            if (Request.Form["Email"] != null) { list.Add("Email"); listName.Add("电子邮箱"); }
            if (Request.Form["Birthday"] != null) { list.Add("Birthday"); listName.Add("生日"); }
            if (Request.Form["Class"] != null) { list.Add("Class"); listName.Add("班级"); }
            if (Request.Form["TelephoneNumber"] != null) { list.Add("TelephoneNumber"); listName.Add("电话号码"); }
            if (Request.Form["Department"] != null) { list.Add("Department"); listName.Add("部门"); }
            if (Request.Form["StudyGuideNumber"] != null) { list.Add("StudyGuideNumber"); listName.Add("指导学长学姐"); }
            if (Request.Form["JoinTime"] != null) { list.Add("JoinTime"); listName.Add("加入时间"); }
            if (Request.Form["Major"] != null) { list.Add("Major"); listName.Add("主修"); }
            if (Request.Form["Counseloer"] != null) { list.Add("Counselor"); listName.Add("辅导员"); }
            if (Request.Form["HeadTeacher"] != null) { list.Add("HeadTeacher"); listName.Add("班主任"); }
            if (Request.Form["UndergraduateTutor"] != null) { list.Add("UndergraduateTutor"); listName.Add("毕业导师"); }
            if (Request.Form["HomPhoneNumber"] != null) { list.Add("HomPhoneNumber"); listName.Add("家庭电话"); }
            if (Request.Form["FamilyAddress"] != null) { list.Add("FamilyAddress"); listName.Add("家庭地址"); }
            if (Request.Form["Sign"] != null) { list.Add("Sign"); listName.Add("个性签名"); }
            int depart=0;
            if (!string.IsNullOrEmpty((Request.Form["depart"])))
            {
                depart = Convert.ToInt32(Request.Form["depart"]);
            }
            if (list.Count == 0)
            {
               return  Content("<script>alert('您没有选择任何项喔~~！');window.location='/PersonalManger/CheckMember/index'</script>");
            }
            DataTable dt = OperateContext.Current.BLLSession.IMemberInformationBLL.GetPartData(list,depart);
            //如果选择了部门那么导出时要将数字转为部门名
            int count = 0;
            //如果选择的指导学长学姐要将学号转换为姓名,下面变量是的得到导出的学号在list列表里的顺序
            int guideCount=0;
            if (Request.Form["Department"] != null)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i] == "Department") { count = i; }
                }
            }
            if (Request.Form["StudyGuideNumber"] != null)
            {
                for (int i = 0; i<list.Count; i++)
                {
                    if (list[i] == "StudyGuideNumber") { guideCount = i; }
                }
            }
            #endregion
            NPOI.HSSF.UserModel.HSSFWorkbook workbook= new NPOI.HSSF.UserModel.HSSFWorkbook();
            NPOI.HSSF.UserModel.HSSFSheet sheet = workbook.CreateSheet("第一页") as NPOI.HSSF.UserModel.HSSFSheet;
            NPOI.HSSF.UserModel.HSSFRow hr = sheet.CreateRow(0) as NPOI.HSSF.UserModel.HSSFRow;
            for(int i=0;i<listName.Count;i++)
            {
                hr.CreateCell(i).SetCellValue(listName[i]);
            }
            for (int rowIndex = 0; rowIndex < dt.Rows.Count; rowIndex++)
            {
                NPOI.HSSF.UserModel.HSSFRow hro = sheet.CreateRow(rowIndex + 1) as NPOI.HSSF.UserModel.HSSFRow;
                for (int colIndex = 0; colIndex < listName.Count; colIndex++)
                {
                    string str="";
                    if (colIndex == count&&count!=0)
                    {
                        if (dt.Rows[rowIndex][colIndex].ToString() == "10001") { str = "NET应用开发部"; }
                        if (dt.Rows[rowIndex][colIndex].ToString() == "10002") { str = "安卓应用开发部"; }
                        if (dt.Rows[rowIndex][colIndex].ToString() == "10003") { str = "硬件编程技术部"; }
                        if (dt.Rows[rowIndex][colIndex].ToString() == "10004") { str = "系统编程技术部"; }
                        if (dt.Rows[rowIndex][colIndex].ToString() == "10007") { str = "暂未加入部门"; }
                        hro.CreateCell(colIndex).SetCellValue(str);
                    }
                    else
                    {
                        if (colIndex == guideCount && guideCount != 0)
                        {
                            string num = dt.Rows[rowIndex][colIndex].ToString();
                            if (!string.IsNullOrEmpty(num))
                            {
                                string name = OperateContext.Current.BLLSession.IMemberInformationBLL.GetListBy(u => u.StuNum == num).FirstOrDefault().StuName;
                                hro.CreateCell(colIndex).SetCellValue(name);
                            }
                        }
                        else
                        {
                            hro.CreateCell(colIndex).SetCellValue(dt.Rows[rowIndex][colIndex].ToString());
                        }
                    } 
                }
            }
            MemoryStream ms = new MemoryStream();
            workbook.Write(ms);
            ms.Seek(0, SeekOrigin.Begin);
            return File(ms, "application/vnd.ms-excel");
        }
        #endregion



        //新增成员 模块
        #region 3.1新增人员首页  AddStu()
        public ActionResult AddStu()
        {
            return View();
        }
        #endregion
        #region 3.2将成员信息增加到数据库中 AddStuData(MODEL.T_MemberInformation member)
        //将成员信息新增到数据库中
        public ActionResult AddStuData()
        {
            MODEL.T_MemberInformation member = new MODEL.T_MemberInformation();
            member.StuName = Request.Form["StuName"];
            member.StuNum= Request.Form["num"];
            member.LoginPwd= Request.Form["pwd"];
            member.Gender = Request.Form["gender"];
            
            member.QQNum = "1";
            member.Email = "1@qq.com";
            member.PhotoPath = "../../HeadImg/final.png";
            member.JoinTime = DateTime.Now;
            member.Department = 10007;
            member.TechnicalLevel = 10003;
            member.IsAdmin = false;
            member.IsDelete = false;
            member.SecretShow = 7;
            int isOk = OperateContext.Current.BLLSession.IMemberInformationBLL.Add(member);
            if (isOk > 0)
            {
                return Content("<script>alert('新增成功');window.location='/PersonalManger/CheckMember/Index';</script>");
            }
            else
            {
                return Content("<script>alert('新增失败');window.location='/PersonalManger/CheckMember/AddStu'</script>");
            }
        }
        #endregion
        #region 3.3从excel表里批量新增到数据库里
        //批量新增
        [HttpPost]
        public ActionResult BatchAdd(HttpPostedFileBase filebase)
        {
            HttpPostedFileBase file = Request.Files["files"];
            string FileName;
            string savePath;
            if (file == null || file.ContentLength <= 0)
            {
                return Content("<script>alert('文件不能为空！');window.location='/PersonalManger/CheckMember/AddStu';</script>");
            }
            else
            {
                string filename = Path.GetFileName(file.FileName);
                int filesize = file.ContentLength;//获取上传文件的大小单位为字节byte
                string fileEx = Path.GetExtension(filename);//获取上传文件的扩展名
                string NoFileName = Path.GetFileNameWithoutExtension(filename);//获取无扩展文件名
                int MaxSize = 8000 * 1024;//定义上传文件的最大空间大小为8M
                string FileType = ".xls,.xlsx";//定义上传文件的类型字符串
                FileName = NoFileName+ fileEx;
                if (!FileType.Contains(fileEx))
                {
                    return Content("<script>alert('文件类型不对，只能导入xls和xlsx格式的文件！');window.location='/PersonalManger/CheckMember/AddStu';</script>");
                }
                if (filesize >= MaxSize)
                {
                    return Content("<script>alert('上传文件超过8M，请小于8M！');window.location='/PersonalManger/CheckMember/AddStu';</script>");
                }
                string path = AppDomain.CurrentDomain.BaseDirectory + "Excel\\";
                savePath = Path.Combine(path, FileName);
                file.SaveAs(savePath);
            }
            FileStream fs = new FileStream(savePath,FileMode.Open, FileAccess.ReadWrite, FileShare.None);//创建文件流
            DataTable dt = new XlsStreamToDT(fs, 15).Xls2DT();
            using (var scope = new TransactionScope())
            {
                MODEL.T_MemberInformation member;
                for (int i = 1; i <dt.Rows.Count; i++)
                {
                    member = new MODEL.T_MemberInformation();
                    member.StuNum = dt.Rows[i][0].ToString();//获取学号
                    member.StuName = dt.Rows[i][1].ToString();//获取姓名
                    member.LoginPwd = dt.Rows[i][2].ToString();//获取初始密码
                    member.QQNum = "1"; 
                    member.Email = "1@qq.com";
                    member.PhotoPath = "../../HeadImg/final.png";
                    member.JoinTime = DateTime.Now;
                    member.Department = 10007;
                    member.TechnicalLevel = 10003;
                    member.IsAdmin = false;
                    member.IsDelete = false;
                    member.SecretShow = 7;
                    OperateContext.Current.BLLSession.IMemberInformationBLL.Add(member);
                    MODEL.T_OgnizationAct OrganizationAct = new MODEL.T_OgnizationAct() { OrganizationId =10004 , RoleActor =member.StuNum,IsDel = false };
                    OperateContext.Current.BLLSession.IOgnizationActBLL.Add(OrganizationAct);
                    //对每一个新成员赋予实习生的角色
                    MODEL.T_RoleAct ra = new MODEL.T_RoleAct();
                    ra.RoleId = 10009;
                    ra.RoleActor = member.StuNum;
                    ra.IsDel = false;
                    OperateContext.Current.BLLSession.IRoleActBLL.Add(ra);
                }
                scope.Complete();
            }
            //删除掉存储在网站目录下的文件
            //事实上，调用File.Delete并没有删除文件，它只是让操作系统认为文件不存在，
            //文件在磁盘上的空间被标记成空的，以便用于再次使用。但是文件的数据没有被移除，您可以非常容易恢复。
            //被删除的文件直到相应的空间被重写才会真消失，这也许要很长时间。
            if (System.IO.File.Exists(savePath))
            {
                System.IO.File.Delete(savePath);
            }
            return Content("<script>alert('批量新增成功！');window.location='/PersonalManger/CheckMember/Index';</script>");
        }
        #endregion
        #region 3.4下载录入表格
        [Common.Attributes.Skip]
        [HttpPost]
        //下载数据录入表格
        public FileResult GetExcel()
        {
            var path =Server.MapPath("~/") +"ExcelModel\\Final.xls";
            return File(path,"application/vnd.ms.excel","final.xls");
        }
        #endregion


        //修改密码
        public ActionResult EditPwd()
        {
            string pwd = OperateContext.Current.Usr.LoginPwd;
            ViewBag.pwd = pwd;
            return View();
        }
        //点击保存修改信息
        [Common.Attributes.Skip]
        public ActionResult SavePwd()
        {
            string pwd = Request.Form["pwd"];
            MODEL.T_MemberInformation mem=new MODEL.T_MemberInformation();
            mem=OperateContext.Current.Usr;
            mem.LoginPwd=pwd;
            OperateContext.Current.BLLSession.IMemberInformationBLL.Modify(mem, "LoginPwd");
            return Content("<script>alert('修改成功！');window.location='/PersonalManger/CheckMember/EditPwd'</script>");
        }

        //处理异常
        protected override void OnException(ExceptionContext filterContext)
        {
            GetAbnormal ab = new GetAbnormal();
            ab.Abnormal(filterContext);
            base.OnException(filterContext);
        }
    }


}