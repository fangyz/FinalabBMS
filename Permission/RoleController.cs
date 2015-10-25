using MODEL.ViewModel;
using MVC.Helper;
using P01MVCAjax.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Permission
{
    public class RoleController:Controller
    {
        //角色管理模块
        #region 1.1得到各位角色的首页
        public ActionResult RoleIndex()
        {
            List<MODEL.T_Role> role = OperateContext.Current.BLLSession.IRoleBLL.GetListBy(u => u.IsDelete == false).ToList();
            JsonModel json;
            json = new JsonModel()
            {
                Data = role,
                BackUrl = "",
                Statu = "ok",
                Msg = "成功"
            };
            JsonResult jr = new JsonResult();
            jr.Data = json;
            jr.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            ViewBag.roleData = role;
            ViewBag.count = 1;
            return View();
        } 
        #endregion
        #region 1.2编辑和新增角色
        public ActionResult EditRole()
        {
            string roleName = Request.Form["role"].ToString();
            int roleid = Convert.ToInt32(Request.Form["roleid"]);
            string hidden = Request.Form["hidden"].ToString();
            if (hidden == "1")//说明这是新增
            {
                MODEL.T_Role role = new MODEL.T_Role();
                role.IsDelete = false;
                role.RoleId = roleid;
                role.RoleName = roleName;
                OperateContext.Current.BLLSession.IRoleBLL.Add(role);
            }
            else//说明这是修改
            {
                //MODEL.T_Role role = OperateContext.Current.BLLSession.IRoleBLL.GetListBy(u => u.RoleId == roleid).FirstOrDefault();
                MODEL.T_Role role = new MODEL.T_Role();
                role.RoleId = roleid;
                role.RoleName = roleName;
                role.IsDelete = false;
                OperateContext.Current.BLLSession.IRoleBLL.Modify(role, "RoleName");
            }
            return Redirect("/Permission/Role/RoleIndex");
        } 
        #endregion
        #region 1.3删除角色
        public ActionResult DeleteById(FormCollection form)//很奇怪，我没有验证是否有权限，可是如果不加skip不行！！！
        {
            int roleId = Convert.ToInt32(form["roleId"]);
            string roleName = form["roleName"];
            MODEL.T_Role role = new MODEL.T_Role();
            role.RoleId = roleId;
            role.RoleName = roleName;
            role.IsDelete = true;
            // OperateContext.Current.BLLSession.IRoleBLL.DelBy(u => u.RoleId == roleId);
            int delCount = OperateContext.Current.BLLSession.IRoleBLL.Modify(role, "IsDelete");
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

        //为角色分配权限模块
        #region 2.1得到设置角色权限的页面 ActionResult SetRolePer()
        public ActionResult SetRolePer()
        {
            //得到要分配权限的角色Id
            int roleid = Convert.ToInt32(Request["roleId"]);
            //首先要得到这个角色的权限
            var listRolePer = OperateContext.Current.BLLSession.IRolePermissionBLL.GetPermissionByRoled(roleid);
            //接下来得到所有权限
            var listAllPer = OperateContext.Current.BLLSession.IPermissionBLL.GetListBy(u => u.IsDelete == false && u.PerId != 19 && u.PerId != 20 && u.PerId != 21).ToList();
            //接下来得到所有父权限，这个父权限是第一级的父权限
            var listParentPer = (from p in listAllPer where p.PerParent == 0 select p).ToList();
            ViewBag.roleId = roleid;
            //接下来是生成树形复选框
            MODEL.ViewModel.RolePermissionTree rpt = new RolePermissionTree();
            rpt.UserPer = listRolePer;
            rpt.AllPer = listAllPer;
            rpt.ParentPer = listParentPer;
            return View(rpt);
        } 
        #endregion
        #region 2.1得到角色编辑后的权限 ActionResult SetPer()
        public ActionResult SetPer()
        {
            //方式一：简单方式，把旧权限删除掉，然后再把新权限加入
            //方式二：原来的所有权限，然后得到新权限，将2个集合相同的删除后，剩下的就是要添加进去的
            //采用方式一，首先将这个角色的旧权限删除掉
            try
            {
                //得到要分配权限的角色Id
                int roleid = Convert.ToInt32(Request.Form["roleId"]);
                //删除这个角色的所有权限
                OperateContext.Current.BLLSession.IRolePermissionBLL.DelBy(u => u.RoleId == roleid);
                object perid;
                //接下来得到所有权限
                var listAllPer = OperateContext.Current.BLLSession.IPermissionBLL.GetListBy(u =>u.IsDelete == false && u.PerId != 19 && u.PerId != 20 && u.PerId != 21).ToList();
                for (int i = 0; i < listAllPer.Count; i++)
                {
                    string perIdName = listAllPer[i].PerId.ToString();
                    //只要不为空，则说明你选中了
                    perid = Request.Form[perIdName];
                    if (perid!=null)
                    {
                        //现在为这个角色添加新的权限到数据库
                        MODEL.T_RolePermission rp = new MODEL.T_RolePermission();
                        rp.RoleId = roleid;
                        rp.PerId = Convert.ToInt32(perid);
                        rp.AddTime = DateTime.Now;
                        rp.IsDel = false;
                        OperateContext.Current.BLLSession.IRolePermissionBLL.Add(rp);
                
                    }
                }
            }
            catch(Exception ex)
            {
                return RedirectToAction("RoleIndex");
            }
            return RedirectToAction("RoleIndex");

        }
        #endregion


        //清空数据
        #region 3.1清空数据首页
        public ActionResult ReallyDelMsg()
        {
            return View();
        } 
        #endregion
        #region 3.2真真的删除成员
        public ActionResult DelStu()
        {
            //这里删除时还要先删除有主外键约束的
            //OperateContext.Current.BLLSession.IRoleActBLL.DelBy(u)
            if (OperateContext.Current.BLLSession.IMemberInformationBLL.DelBy(u => u.IsDelete == true) > 0)
            {
                return Content("<script>alert('删除成功！');window.location='/Permission/Role/ReallyDelMsg';</script>");
            }
            return Content("<script>alert('删除失败！');window.location='/Permission/Role/ReallyDelMsg';</script>");
        } 
        #endregion


        #region 处理异常 protected override void OnException(ExceptionContext filterContext)
        protected override void OnException(ExceptionContext filterContext)
        {
            GetAbnormal ab = new GetAbnormal();
            ab.Abnormal(filterContext);
            base.OnException(filterContext);
        }
        #endregion

      


    }
}
