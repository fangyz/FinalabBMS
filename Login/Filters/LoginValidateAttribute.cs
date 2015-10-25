using MVC.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Login.Filters
{
    /// <summary>
    /// 用户身份验证 过滤器
    /// </summary>
    public class LoginValidateAttribute:System.Web.Mvc.AuthorizeAttribute
    {
        #region 1.0 验证方法 - 在 ActionExcuting过滤器之前执行
        /// <summary>
        /// 验证方法 - 在 ActionExcuting过滤器之前执行，Authorization是最早执行的
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnAuthorization(System.Web.Mvc.AuthorizationContext filterContext)
        {
            //1.如果请求的 Admin 区域里的 控制器类和方法，那么就要验证权限 && filterContext.RouteData.DataTokens["area"].ToString().ToLower() == "Login".ToLower()
            if (filterContext.RouteData.DataTokens.Keys.Contains("area")//当前请求匹配的 路由对象中 是否 有 area区域
               )
            {
                //2.检查 被请求的 方法 和 控制器是否有 Skip 标签，如果有，则不验证；如果没有，则验证
                if (!filterContext.ActionDescriptor.IsDefined(typeof(Common.Attributes.SkipAttribute), false) &&
                    !filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(Common.Attributes.SkipAttribute), false))
                {
                    #region 1.验证用户是否登陆(Session && Cookie)
                    //1.验证用户是否登陆(Session && Cookie)
                    if (!OperateContext.Current.IsLogin())
                    {
                        filterContext.Result = OperateContext.Current.Redirect(true, filterContext.ActionDescriptor);
                    }
                    #endregion

                    #region 2.验证登陆用户 是否有访问该页面的权限
                    else
                    {
                        //2.获取 登陆用户权限
                        string strAreaName = filterContext.RouteData.DataTokens["area"].ToString().ToLower();
                        string strContrllerName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName.ToLower();
                        string strActionName = filterContext.ActionDescriptor.ActionName.ToLower();
                        string strHttpMethod = filterContext.HttpContext.Request.HttpMethod;

                        if (!OperateContext.Current.HasPemission(strAreaName, strContrllerName, strActionName, strHttpMethod))
                        {
                            filterContext.Result = OperateContext.Current.Redirect(false,filterContext.ActionDescriptor);
                        }
                    }
                    #endregion
                }

            }
        } 
        #endregion
    }
}
