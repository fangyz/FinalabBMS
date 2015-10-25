using MODEL.ViewModel;
using MVC.Helper;
using P01MVCAjax.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web.Mvc;

namespace Permission
{
    public class PermissionController:Controller
    {
        //权限管理页面
        #region 1.1为权限首页获取数据，每一次分页的请求也是执行这个方法 ActionResult PerIndex()
        /// <summary>
        /// 为权限首页获取数据，每一次分页的请求也是执行这个方法
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        public ActionResult PerIndex()
        {
            //设置查询第几页的数据
            int pageNow = 1;
            if (Request.QueryString["page"] != null)
            {
                pageNow = Convert.ToInt32(Request.QueryString["page"]);
            }
            //接下来开始查询数据
            Expression<Func<MODEL.T_Permission, bool>> whereLambda;
                whereLambda = u => u.IsDelete == false;
                //根据lambda表达式和第几页拿到数据，
                PermissionMsg permissionMsg = PageData(whereLambda, pageNow);
                //将数据赋值给ViewBag
                ViewBag.listPer = permissionMsg.ListPer;
                ViewBag.totalRecord = permissionMsg.TotalRecord;
                ViewBag.pageNow = pageNow;
                int pageCount = permissionMsg.TotalRecord / 10;
                if (permissionMsg.TotalRecord % 10 != 0)
                {
                    pageCount = permissionMsg.TotalRecord / 10 + 1;
                }
                ViewBag.pageCount = pageCount;
                return View();
        } 
        #endregion
        #region 1.2获取分页数据，共同调用，GetPageData需要调用 ActionResult PageData(Expression<Func<MODEL.T_MemberInformation, bool>> whereLambda, int pageIndex)
        public PermissionMsg PageData(Expression<Func<MODEL.T_Permission, bool>> whereLambda, int pageIndex)
        {
            int totalRecord;
            int pageSize = 10;//页容量固定为10
            try//为什么异常没有捕捉到
            {
                var list = OperateContext.Current.BLLSession.IPermissionBLL.GetPagedList(pageIndex, pageSize,
                   whereLambda,u=>u.PerId, out totalRecord).ToList();
                PermissionMsg permissionMsg = new PermissionMsg()
                {
                    ListPer = list,
                    TotalRecord = totalRecord
                };
                return permissionMsg;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        #endregion

        //权限操作
        #region 2.1 得到权限操作页面,比如新增,编辑,查看 ActionResult HandlePermission()
        //得到权限操作页面,比如新增,编辑,查看
        public ActionResult HandlePermission()
        {
            int perId = 0;
            int operate = 0;//得到是什么操作：如果是1则为新增，2为编辑，3为查看
            if (Request.QueryString["operate"] != null)
            {
                operate = Convert.ToInt32(Request.QueryString["operate"]);
            }
            if (operate == 1)
            {
                ViewBag.opeName = "新增权限";
                ViewBag.operate = 1;
                MODEL.T_Permission per = new MODEL.T_Permission();
                ViewBag.perModel = per;
            }
            if (operate == 2)
            {
                perId = Convert.ToInt32(Request.QueryString["perid"]);
                //得到当前ID的权限信息
                var per = OperateContext.Current.BLLSession.IPermissionBLL.GetListBy(u => u.IsDelete == false && u.PerId == perId).First();
                MODEL.T_Permission perModel = per;
                ViewBag.perModel = perModel;
                ViewBag.opeName = "编辑权限";
                ViewBag.operate = 2;
            }
            if (operate == 3)
            {
                perId = Convert.ToInt32(Request.QueryString["perid"]);
                try
                {
                    //得到当前ID的权限信息
                    var per = OperateContext.Current.BLLSession.IPermissionBLL.GetListBy(u => u.IsDelete == false && u.PerId == perId).First();
                    MODEL.T_Permission perModel = per;
                    ViewBag.perModel = perModel;
                    ViewBag.opeName = "权限详细信息";
                    ViewBag.operate = 3;
                }
                catch
                {
                    return null;
                }
            }
            return View();
        } 
        #endregion
        #region 2.2 操作权限表中的数据 ActionResult EditPer()
        /// <summary>
        /// 操作权限表中的数据
        /// </summary>
        /// <returns></returns>

        public ActionResult OperatePer()
        {
            string id = Request.Form["perId"].ToString();
            int operate = 1;//1为新增，2为修改
            if (!string.IsNullOrEmpty(id)) 
            {
                operate = 2;
            }
            MODEL.T_Permission per = new MODEL.T_Permission();
            per.PerAddTime = Convert.ToDateTime(Request.Form["PerAddTime"]);
            per.PerName = Request.Form["PerName"].ToString();
            per.PerParent = Convert.ToInt32(Request.Form["PerParent"]);
            per.PerAreaName = Request.Form["PerAreaName"].ToString();
            per.PerController = Request.Form["PerController"].ToString();
            per.PerActionName = Request.Form["PerActionName"].ToString();
            per.PerFormMethod = Convert.ToInt32(Request.Form["PerFormMethod"]);
            int show= Convert.ToInt32(Request.Form["PerIsShow"]);
            if (show == 1) { per.PerIsShow = true; }
            else{per.PerIsShow=false;}
            per.PerIco = Request.Form["PerIco"].ToString();
            per.IsCommon = Convert.ToBoolean(Request.Form["IsCommon"]);
            per.perExplain = Request.Form["perExplain"].ToString();

            string[] proNames; 
            if (operate == 1)
            {
                if (OperateContext.Current.BLLSession.IPermissionBLL.Add(per) > 0)
                {
                    return Redirect("/Permission/Permission/PerIndex");
                }
            }
            if (operate == 2)
            {
                per.PerId=Convert.ToInt32(Request.Form["PerId"]);
                proNames = new string[] { "PerName", "PerParent", "PerAreaName", "PerController", "PerActionName", "PerFormMethod", "PerIsShow", "PerAddTime", 
                "PerIco","perExplain","IsCommon"};
                //成功或失败信息提示还没做    可以用返回script代码
                if (OperateContext.Current.BLLSession.IPermissionBLL.Modify(per, proNames) > 0)
                {
                    return Redirect("/Permission/Permission/PerIndex");
                }
            }
            return Redirect("/Permission/Permission/PerIndex");
        }
        #endregion
        #region 2.3根据权限ID删除权限 ActionResult DelPer()
        public ActionResult DelPer()
        {
            int perid = Convert.ToInt32(Request.Form["perid"]);
            //删除权限前，首先要删除角色里的这个权限
            OperateContext.Current.BLLSession.IRolePermissionBLL.DelBy(u => u.PerId == perid);
            //删除权限表里的数据
            if (OperateContext.Current.BLLSession.IPermissionBLL.DelBy(u => u.PerId == perid) > 0)
            {
                return OperateContext.Current.RedirectAjax("ok", "删除成功", null, null);
            }
            return OperateContext.Current.RedirectAjax("err", "删除失败", null, null);
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
