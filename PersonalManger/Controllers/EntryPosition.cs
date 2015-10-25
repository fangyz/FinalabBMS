using MODEL.DTO;
using MODEL.ViewModel;
using MVC.Helper;
using P01MVCAjax.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace PersonalManger
{
    public class EntryPositionController : Controller
    {
        //人事管理 录入职位模块
        #region 1.1 得到录入职位的主页面 ActionResult EntryChoose()
        public ActionResult EntryChoose()
        {
            List<MODEL.T_Permission> UsrPermission=new List<MODEL.T_Permission>();
            List<MODEL.T_Permission> EntryPer = new List<MODEL.T_Permission>();
            //1.拿到当前登入者的信息
            if (Session["ainfo"] != null)
            {
                //userId即为用户学号
                string userId = Session["StuNum"].ToString();
                var user = OperateContext.Current.BLLSession.IMemberInformationBLL.GetListBy(u => u.StuNum == userId).FirstOrDefault();
                UsrPermission = OperateContext.Current.GetUserPermission(user.StuNum);
            }
            //2.根据权限集合得到此用户可以录入哪些角色
            if (UsrPermission.Count > 0)
            {
                foreach (MODEL.T_Permission per in UsrPermission)
                {
                    if (per.PerParent == Entry.EntryId)
                    {
                        EntryPer.Add(per);
                    }
                }
            }
            //3.得到用户可以录入的选项
            ViewBag.role = EntryPer;
            return View();
        }
        #endregion

        #region 1.2得到录入具体某个职位的页面  ActionResult EntryIndex()
        public ActionResult EntryIndex()
        {
            //接收要录入职位的权限Id
            int perId = Convert.ToInt32(Request.QueryString["perId"]);
            List<MODEL.T_MemberInformation> mem = new List<MODEL.T_MemberInformation>();
            //得到录入这个职位的候选者并验证权限
            object o=GetEntryData(perId);
            if (o == null)
            {
                return Content("<script>alert('您没有权限录入!')</script>");
            }
            else
            {
                mem = GetEntryData(perId);
            }
            if (perId == 46) { ViewBag.dep = 1; }
            //得到可以担任这个职位的候选者
            ViewBag.mem = mem;
            ViewBag.perid = perId;
            return View();
        }
        #endregion

        #region 1.3根据不同的条件获取不同的职位可以担任的人   GetEntryData(FormCollection form)
        public List<MODEL.T_MemberInformation> GetEntryData(int perId)
        {
            //确定职位候选人的年级
            DateTime dt = DateTime.Now;
            string dtone;
            string dttwo;
            if (dt.Month >= 9)
            {
                dtone =dt.Year.ToString();
                dttwo=(dt.Year-1).ToString();
            }
            else
            {
                dtone = (dt.Year - 1).ToString();
                dttwo=(dt.Year - 2).ToString();
            }
            //查找条件
            Expression<Func<MODEL.T_MemberInformation, bool>> filter = null;
            if (perId == Entry.EntryPresident)//录入总裁
            {
                filter = u => u.IsDelete == false && (u.StuNum.Contains(dttwo)) && (u.TechnicalLevel == TechnicalLevel.EliteProgram) && (u.T_RoleAct.Select(p => p.RoleId).Contains(Position.President) == false);
            }
            if (perId == Entry.EntryMinister)//录入部长
            {
                filter = u => u.IsDelete == false && (u.StuNum.Contains(dttwo)) && (u.T_RoleAct.Select(p => p.RoleId).Contains(Position.Minister) == false && u.TechnicalLevel == TechnicalLevel.FullMember);
            }
            if (perId == Entry.EntryStudyLeader)//录入学习顾问团团长
            {
                filter = u => u.IsDelete == false && (u.StuNum.Contains(dttwo)) && (u.TechnicalLevel == TechnicalLevel.FullMember) && (u.T_RoleAct.Select(p => p.RoleId).Contains(Position.StudyLeader) == false);
            }
            if (perId == Entry.EntryStudyMember)//录入学习顾问团成员
            {
                filter = u => u.IsDelete == false && (u.StuNum.Contains(dttwo)) && (u.TechnicalLevel == TechnicalLevel.FullMember) && (u.T_RoleAct.Select(p => p.RoleId).Contains(Position.StudyMember) == false);
            }
            if (perId == Entry.EntryPlanLeadert)//录入活动策划组组长
            {
                filter = u => u.IsDelete == false && (u.StuNum.Contains(dtone)) && (u.T_RoleAct.Select(p => p.RoleId).Contains(Position.PlanLeader) == false);
            }
            if (perId == Entry.EntryPlanMmember)//录入活动策划组组员
            {
                filter = u => u.IsDelete == false && (u.StuNum.Contains(dtone)) && (u.T_RoleAct.Select(p => p.RoleId).Contains(Position.PlanMmember) == false); ;
            }
            if (perId == Entry.EntryFinancial)//录入财务主管
            {
                filter = u => u.IsDelete == false && (u.StuNum.Contains(dttwo)) && (u.TechnicalLevel == TechnicalLevel.FullMember);
            }
            if (perId == Entry.EntryDepartment)//录入部门成员
            {
                filter = u => u.IsDelete == false && (u.StuNum.Contains(dtone) || u.StuNum.Contains(dttwo)) && (u.TechnicalLevel == TechnicalLevel.FullMember);
            }
            if (perId == Entry.EntryMember)//录入正式成员
            {
                filter = u => u.IsDelete == false && (u.StuNum.Contains(dtone)||u.StuNum.Contains(dttwo)) && (u.TechnicalLevel == TechnicalLevel.Student);
            }
            List<MODEL.T_MemberInformation> memlist = new List<MODEL.T_MemberInformation>();
            memlist = OperateContext.Current.BLLSession.IMemberInformationBLL.GetListBy(filter).ToList();
            return memlist;
        }
        #endregion

       

        //开始录入数据
        #region 2.1得到选中的数据  ActionResult GetCheckedData()
        [Common.Attributes.Skip]
        public ActionResult GetCheckedData()
        {
            DateTime dt = DateTime.Now;
            string dtthree;
            if (dt.Month >= 9)
            {
                dtthree = (dt.Year-2).ToString();
            }
            else
            {
                dtthree = (dt.Year - 3).ToString();
            }
            //如果是录入部门成员，需要得到录入的部门ID
            int dep = Convert.ToInt32(Request.Form["dep"]);
            int perId = Convert.ToInt32(Request.Form["entryWhat"]);
            if (Request.Form["entryWhat"] == null)
            {
                return Content("<script>alert('请您选择录入信息');window.location='/PersonalManger/EntryPosition/EntryChoose'</script>");
            }
            //得到选中的人
            List<MODEL.T_MemberInformation> mem = GetEntryData(perId);
            List<MODEL.T_MemberInformation> modifyMem = new List<MODEL.T_MemberInformation>();
            for (int i = 0; i < mem.Count; i++)
            {
                if (Request.Form[mem[i].StuNum] != null)
                {
                    modifyMem.Add(mem[i]);
                }
            }
            //将选中的人录入
            if (EntryData(perId, modifyMem, dep, dtthree))
            {
                return Content("<script>alert('录入成功！');window.location='/PersonalManger/EntryPosition/EntryChoose'</script>");
            }
            return Content("<script>alert('录入失败,请您重新录入');window.location='/PersonalManger/EntryPosition/EntryChoose'</script>");
        } 
        #endregion

        #region 2.2录入数据到数据库里 EntryData(int perId, List<MODEL.T_MemberInformation> modifyMem, int dep, string dtThree)
        public bool EntryData(int perId, List<MODEL.T_MemberInformation> modifyMem, int dep, string dtThree)
        {
            List<MODEL.T_RoleAct> roleActList=new List<MODEL.T_RoleAct>();
            if (perId == Entry.EntryPresident)//录入总裁
            {
                //删除担任这个职务的上一届成员
                roleActList = OperateContext.Current.BLLSession.IRoleActBLL.GetListBy(u => u.RoleId == 10001&&u.RoleActor.Contains(dtThree)==true).ToList();
                OperateContext.Current.BLLSession.IRoleActBLL.DelBy(u => u.RoleId == Position.President && u.RoleActor.Contains(dtThree) == true);
                SetNewRole(roleActList);
                //开始录入
                if (OperatePosition(modifyMem, Position.President))
                {
                    return true;
                }
            }
            if (perId == Entry.EntryMinister)//录入部长
            {
                //删除担任这个职务的上一届成员
                roleActList = OperateContext.Current.BLLSession.IRoleActBLL.GetListBy(u => u.RoleId == Position.Minister && u.RoleActor.Contains(dtThree) == true).ToList();
                OperateContext.Current.BLLSession.IRoleActBLL.DelBy(u => u.RoleId == Position.Minister && u.RoleActor.Contains(dtThree) == true);
                SetNewRole(roleActList);
                //开始录入
                if (OperatePosition(modifyMem, Position.Minister))
                {
                    return true;
                }
            }
            if (perId == Entry.EntryStudyLeader)//录入学习顾问团团长
            {
                //删除担任这个职位的上一届成员
                roleActList = OperateContext.Current.BLLSession.IRoleActBLL.GetListBy(u => u.RoleId == Position.StudyLeader && u.RoleActor.Contains(dtThree) == true).ToList();
                OperateContext.Current.BLLSession.IRoleActBLL.DelBy(u => u.RoleId == Position.StudyLeader && u.RoleActor.Contains(dtThree) == true);
                SetNewRole(roleActList);
                if (OperatePosition(modifyMem, Position.StudyLeader))
                {
                    return true;
                }
            }
            if (perId == Entry.EntryStudyMember)//录入学习顾问团成员
            {
                //删除担任这个职位的上一届成员
                roleActList = OperateContext.Current.BLLSession.IRoleActBLL.GetListBy(u => u.RoleId == Position.StudyMember && u.RoleActor.Contains(dtThree) == true).ToList();
                OperateContext.Current.BLLSession.IRoleActBLL.DelBy(u => u.RoleId == Position.StudyMember && u.RoleActor.Contains(dtThree) == true);
                SetNewRole(roleActList);        
                if (OperatePosition(modifyMem, Position.StudyMember))
                {
                    return true;
                }
            }
            if (perId == Entry.EntryPlanLeadert)//录入活动策划组组长
            {
                string dttwo = (Convert.ToInt32(dtThree) - 1).ToString();
                //删除担任这个职位的上一届成员
                roleActList = OperateContext.Current.BLLSession.IRoleActBLL.GetListBy(u => u.RoleId == Position.StudyMember && u.RoleActor.Contains(dttwo) == true).ToList();
                OperateContext.Current.BLLSession.IRoleActBLL.DelBy(u => u.RoleId == Position.PlanLeader && u.RoleActor.Contains(dttwo) == true);
                SetNewRole(roleActList);       
                //开始录入
                if (OperatePosition(modifyMem, Position.PlanLeader))
                {
                    return true;
                }
            }
            if (perId == Entry.EntryPlanMmember)//录入活动策划组组员
            {
                string dttwo = (Convert.ToInt32(dtThree) - 1).ToString();
                //删除担任这个职位的上一届成员
                roleActList = OperateContext.Current.BLLSession.IRoleActBLL.GetListBy(u => u.RoleId == Position.StudyMember && u.RoleActor.Contains(dttwo) == true).ToList();
                OperateContext.Current.BLLSession.IRoleActBLL.DelBy(u => u.RoleId == Position.PlanMmember && u.RoleActor.Contains(dttwo) == true);
                SetNewRole(roleActList);
                //开始录入
                if (OperatePosition(modifyMem, Position.PlanMmember))
                {
                    return true;
                }
            }
            if (perId == Entry.EntryFinancial)//录入财务主管
            {
                //删除担任这个职位的上一届成员
                roleActList = OperateContext.Current.BLLSession.IRoleActBLL.GetListBy(u => u.RoleId == Position.Financial).ToList();
                OperateContext.Current.BLLSession.IRoleActBLL.DelBy(u => u.RoleId == Position.Financial);
                SetNewRole(roleActList);
                //开始录入
                if (OperatePosition(modifyMem, Position.Financial))
                {
                    return true;
                }
            }
            if (perId == Entry.EntryDepartment)//录入部门成员
            {
                //开始录入
                foreach (MODEL.T_MemberInformation mem in modifyMem)
                {
                    //分配这个成员的部门，dep为部门编号
                    mem.Department = dep;
                    if (OperateContext.Current.BLLSession.IMemberInformationBLL.Modify(mem, "Department")>0)
                    {
                        return true;
                    }
                }
            }
            if (perId == Entry.EntryMember)//录入正式成员
            {
                //还要修改技术水平里的数据
                foreach (MODEL.T_MemberInformation mem in modifyMem)
                {
                    mem.TechnicalLevel = TechnicalLevel.FullMember;
                    string[] pro =new string[]{"StuNum","TechnicalLevel"};
                    OperateContext.Current.BLLSession.IMemberInformationBLL.Modify(mem, pro);
                }
                if (OperatePosition(modifyMem, Position.FullMember))
                {
                    return true;
                }
            }
            return false;
        } 
        #endregion

        #region 2.3公用方法，将职位录入到数据库里+bool OperatePosition(List<MODEL.T_MemberInformation> memList, int roleId)
        public bool OperatePosition(List<MODEL.T_MemberInformation> memList, int roleId)
        {
            MODEL.T_RoleAct roleAct;
            try
            {
                int count = 0;
                foreach (MODEL.T_MemberInformation mem in memList)
                {
                    //开始录入
                    roleAct = new MODEL.T_RoleAct();
                    roleAct.RoleId = roleId;
                    roleAct.RoleActor = mem.StuNum;
                    roleAct.IsDel = false;
                    if (OperateContext.Current.BLLSession.IRoleActBLL.Add(roleAct) > 0)
                    {
                        count++;
                    }
                }
                if (count > 0)
                    return true;
                else
                    return false;
            }
            catch
            {
                return false;
            }
        } 
        #endregion

        #region 2.4为卸任职务的成员指定正式成员角色
        public void SetNewRole(List<MODEL.T_RoleAct> roleAct)
        {
            if (roleAct != null)
            {
                foreach (MODEL.T_RoleAct role in roleAct)
                {
                    role.RoleId = Position.FullMember;
                    OperateContext.Current.BLLSession.IRoleActBLL.Add(role);
                }
            }
        } 
        #endregion




        //删除职位
        #region 4.1删除成员页面 ActionResult DelIndex()
        [Common.Attributes.Skip]
        public ActionResult DelIndex()
        {
            int perId = Convert.ToInt32(Request.QueryString["perId"]);
            //得到录入这个职位的担任者
            List<MODEL.T_MemberInformation> mem = GetDelData(perId);
            //判断这是删除什么
            ViewBag.mem = mem;
            ViewBag.perid = perId;
            return View();
        } 
        #endregion

        #region 4.2根据不同的条件获取这个职位的成员 GetDelData(FormCollection form)
        /// <summary>
        /// 根据不同的条件获取不同的职位数据
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        [Common.Attributes.Skip]
        public List<MODEL.T_MemberInformation> GetDelData(int perId)
        {
            //确定职位候选人的年级
            DateTime dt = DateTime.Now;
            string dtone;
            string dttwo;
            if (dt.Month >= 9)
            {
                dtone = dt.Year.ToString();
                dttwo = (dt.Year - 1).ToString();
            }
            else
            {
                dtone = (dt.Year - 1).ToString();
                dttwo = (dt.Year - 2).ToString();
            }
            //查找条件
            Expression<Func<MODEL.T_MemberInformation, bool>> filter = null;
            if (perId ==Entry.EntryPresident)//查看总裁
            {
                filter = u => u.IsDelete == false&& (u.T_RoleAct.Select(p => p.RoleId).Contains(Position.President) == true);
            }
            if (perId == Entry.EntryMinister)//查看部长
            {
                filter = u => u.IsDelete == false && (u.T_RoleAct.Select(p => p.RoleId).Contains(Position.Minister) == true);
            }
            if (perId == Entry.EntryStudyLeader)//查看学习顾问团团长
            {
                filter = u => u.IsDelete == false && (u.T_RoleAct.Select(p => p.RoleId).Contains(Position.StudyLeader) == true);
            }
            if (perId == Entry.EntryStudyMember)//查看学习顾问团成员
            {
                filter = u => u.IsDelete == false && (u.T_RoleAct.Select(p => p.RoleId).Contains(Position.StudyMember) == true);
            }
            if (perId == Entry.EntryPlanLeadert)//查看活动策划组组长
            {
                filter = u => u.IsDelete == false && (u.T_RoleAct.Select(p => p.RoleId).Contains(Position.PlanLeader) == true);
            }
            if (perId == Entry.EntryPlanMmember)//查看活动策划组组员
            {
                filter = u => u.IsDelete == false && (u.T_RoleAct.Select(p => p.RoleId).Contains(Position.PlanMmember) == true);
            }
            if (perId == Entry.EntryFinancial)//查看财务主管
            {
                filter = u => u.IsDelete == false && (u.T_RoleAct.Select(p => p.RoleId).Contains(Position.Financial) == true); ;
            }
            List<MODEL.T_MemberInformation> memlist = OperateContext.Current.BLLSession.IMemberInformationBLL.GetListBy(filter).ToList();
            return memlist;
        }
        #endregion

        #region 4.3得到选中要删除的数据  ActionResult GetDelChecked()
        [Common.Attributes.Skip]
        public ActionResult GetDelChecked()
        {
            DateTime dt = DateTime.Now;
            string dtthree;
            if (dt.Month >= 9)
            {
                dtthree = (dt.Year - 2).ToString();
            }
            else
            {
                dtthree = (dt.Year - 3).ToString();
            }
            int perId = Convert.ToInt32(Request.Form["delWhat"]);
            //得到这个职务的担任者
            List<MODEL.T_MemberInformation> mem = GetDelData(perId);
            List<MODEL.T_MemberInformation> modifyMem = new List<MODEL.T_MemberInformation>();
            for (int i = 0; i < mem.Count; i++)
            {
                if (Request.Form[mem[i].StuNum] != null)
                {
                    modifyMem.Add(mem[i]);
                }
            }
            //将选中的人删除
            if (DelData(perId, modifyMem,dtthree))
            {
                return Content("<script>alert('删除成功！');window.location='/PersonalManger/EntryPosition/EntryChoose'</script>");
            }
            return Content("<script>alert('删除失败,请您重新删除');window.location='/PersonalManger/EntryPosition/EntryChoose'</script>");
        }
        #endregion

        #region 4.4开始删除数据 DelData(int perId, List<MODEL.T_MemberInformation> modifyMem,string dtThree)
        public bool DelData(int perId, List<MODEL.T_MemberInformation> modifyMem,string dtThree)
        {
            if (perId == Entry.EntryPresident)//删除总裁
            {
                if (DelPosition(modifyMem,Position.President))
                {
                    return true;
                }
            }
            if (perId == Entry.EntryMinister)//删除部长
            {
                if (DelPosition(modifyMem, Position.Minister))
                {
                    return true;
                }
            }
            if (perId == Entry.EntryStudyLeader)//删除学习顾问团团长
            {
                if (DelPosition(modifyMem, Position.StudyLeader))
                {
                    return true;
                }
            }
            if (perId == Entry.EntryStudyMember)//删除学习顾问团成员
            {
                if (DelPosition(modifyMem, Position.StudyMember))
                {
                    return true;
                }
            }
            if (perId == Entry.EntryPlanLeadert)//删除活动策划组组长
            {
                if (DelPosition(modifyMem, Position.PlanLeader))
                {
                    return true;
                }
            }
            if (perId == Entry.EntryPlanMmember)//删除活动策划组组员
            {
                if (DelPosition(modifyMem, Position.PlanMmember))
                {
                    return true;
                }
            }
            if (perId == Entry.EntryFinancial)//删除财务主管
            {
                if (DelPosition(modifyMem, Position.Financial))
                {
                    return true;
                }
            }  
            return false;
        }
        #endregion

        #region 4.5将职位从数据库里删除+bool DelPosition(List<MODEL.T_MemberInformation> memList, int roleId)
        public bool DelPosition(List<MODEL.T_MemberInformation> memList, int roleId)
        {
            MODEL.T_RoleAct roleAct;
                int count = 0;
                foreach (MODEL.T_MemberInformation mem in memList)
                {
                    //开始删除数据
                    roleAct = new MODEL.T_RoleAct();
                    roleAct.RoleId = roleId;
                    roleAct.RoleActor = mem.StuNum;
                    roleAct.IsDel = false;
                    if (OperateContext.Current.BLLSession.IRoleActBLL.Del(roleAct) > 0)
                    {
                        count++;
                    }
                }
                if (count > 0)
                    return true;
                else
                    return false;
        }
        #endregion

        #region 处理异常
        protected override void OnException(ExceptionContext filterContext)
        {
            GetAbnormal ab = new GetAbnormal();
            ab.Abnormal(filterContext);
            base.OnException(filterContext);
        }
       #endregion



    }
}
