using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Data;//DataSet/DataTable/DataRow/DataCol
using System.Data.SqlClient;//SqlConnection/SqlCommand/SqlDataAdapter/SqlDataReader
using System.Reflection;

namespace MODEL
{
    /// <summary>
    /// 数据库操作帮助类
    /// </summary>
    public static class SQLHelper
    {
        //1.准备连接字符串
        //public static readonly string connStr = ConfigurationManager.AppSettings["connStr"];
        public static readonly string connStr = ConfigurationManager.ConnectionStrings["connStr"].ConnectionString;

        #region 2.执行查询多行语句 - 返回数据表 +static DataTable ExcuteDataTable(string strSelectCmd, params SqlParameter[] paras)
        /// <summary>
        /// 2.执行查询多行语句 - 返回数据表
        /// </summary>
        /// <param name="strSelectCmd"></param>
        /// <param name="paras"></param>
        /// <returns></returns>
        public static DataTable ExcuteDataTable(string strSelectCmd, params SqlParameter[] paras)
        {
            //1.创建连接通道
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                //2.创建适配器
                SqlDataAdapter da = new SqlDataAdapter(strSelectCmd, conn);
                //2.1设置查询命令的参数
                da.SelectCommand.Parameters.AddRange(paras);
                //3.数据表
                DataTable dt = new DataTable();
                //4.将数据查询并填充到数据表中
                da.Fill(dt);
                return dt;
            }
        }
        #endregion

        #region  2.0升级泛型版 ------ 执行查询多行语句 - 返回数据表
        /// <summary>
        /// 2.0升级泛型版 ------ 执行查询多行语句 - 返回数据表
        /// </summary>
        /// <typeparam name="T2">泛型类型</typeparam>
        /// <param name="strSelectCmd">查询sql语句</param>
        /// <param name="paras">查询参数</param>
        /// <returns>泛型集合</returns>
        public static List<T2> ExcuteList<T2>(string strSelectCmd, params SqlParameter[] paras)
        {
            //1.创建连接通道
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                //2.创建适配器
                SqlDataAdapter da = new SqlDataAdapter(strSelectCmd, conn);
                //2.1设置查询命令的参数
                da.SelectCommand.Parameters.AddRange(paras);
                //3.数据表
                DataTable dt = new DataTable();
                //4.将数据查询并填充到数据表中
                da.Fill(dt);
                //5.将DataTable转成泛型集合List<T2>
                if (dt.Rows.Count > 0)
                {
                    //6.创建泛型集合对象
                    List<T2> list = new List<T2>();
                    //7.遍历数据行，将行数据存入 实体对象中，并添加到 泛型集合中list
                    foreach (DataRow row in dt.Rows)
                    {
                        //留言：等学完反射后再讲~~~~！
                        //7.1先获得泛型的类型(里面包含该类的所有信息---有什么属性啊，有什么方法啊，什么字段啊....................)
                        Type t = typeof(T2);
                        //7.2根据类型创建该类型的对象
                        T2 model = (T2)Activator.CreateInstance(t);// new MODEL.Classes()
                        //7.3根据类型 获得 该类型的 所有属性定义
                        PropertyInfo[] properties = t.GetProperties();
                        //7.4遍历属性数组
                        foreach (PropertyInfo p in properties)
                        {
                            //7.4.1获得属性名，作为列名
                            string colName = p.Name;
                            //7.4.2根据列名 获得当前循环行对应列的值
                            object colValue = row[colName];
                            //7.4.3将 列值 赋给 model对象的p属性
                            //model.ID=colValue;
                            p.SetValue(model, colValue, null);
                        }
                        //7.5将装好 了行数据的 实体对象 添加到 泛型集合中 O了！！！
                        list.Add(model);
                    }
                    return list;
                }
            }
            return null;
        }
        #endregion

        #region 3.执行查询多行语句 - 返回数据读取器  +static SqlDataReader ExcuteDataReader(string strSelectCmd, params SqlParameter[] paras)
        /// <summary>
        /// 执行查询多行语句 - 返回数据读取器
        /// </summary>
        /// <param name="strSelectCmd"></param>
        /// <param name="paras"></param>
        /// <returns></returns>
        public static SqlDataReader ExcuteDataReader(string strSelectCmd, params SqlParameter[] paras)
        {
            SqlConnection conn = null;
            try
            {
                //1.创建连接通道
                conn = new SqlConnection(connStr);
                //2.创建命令对象
                SqlCommand cmd = new SqlCommand(strSelectCmd, conn);
                //3.添加命令参数
                cmd.Parameters.AddRange(paras);
                //4.打开连接
                conn.Open();
                //5.创建读取器（当关闭此读取器时，会自动关闭连接通道）
                SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);//当关闭此读取器时，会自动关闭连接通道
                //6.返回读取器
                return dr;
            }
            catch (Exception ex)
            {
                conn.Dispose();
                throw ex;
            }
        }
        #endregion

        #region 4.执行非查询语句（增删改） +static int ExcuteNonQuery(string strCmd, params SqlParameter[] paras)
        /// <summary>
        /// 执行非查询语句（增删改）
        /// </summary>
        /// <param name="strCmd"></param>
        /// <param name="paras"></param>
        /// <returns></returns>
        public static int ExcuteNonQuery(string strCmd, params SqlParameter[] paras)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                SqlCommand cmd = new SqlCommand(strCmd, conn);
                cmd.Parameters.AddRange(paras);
                conn.Open();
                return cmd.ExecuteNonQuery();
            }
        }
        #endregion

        #region 5.查询结果集里的第一个单元格的值（单个值）+static object ExcuteScalar(string strSelectCmd, params SqlParameter[] paras)
        /// <summary>
        /// 查询结果集里的第一个单元格的值（单个值）
        /// </summary>
        /// <param name="strSelectCmd"></param>
        /// <param name="paras"></param>
        /// <returns></returns>
        public static object ExcuteScalar(string strSelectCmd, params SqlParameter[] paras)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                SqlCommand cmd = new SqlCommand(strSelectCmd, conn);
                cmd.Parameters.AddRange(paras);
                conn.Open();
                return cmd.ExecuteScalar();
            }
        }
        #endregion

        #region 6.查询结果集里的第一个单元格的值（单个值）-- 泛型版本 + static T ExcuteScalar<T>(string strSelectCmd, params SqlParameter[] paras)
        /// <summary>
        /// 查询结果集里的第一个单元格的值（单个值）-- 泛型版本
        /// </summary>
        /// <typeparam name="T">类型参数</typeparam>
        /// <param name="strSelectCmd"></param>
        /// <param name="paras"></param>
        /// <returns></returns>
        public static T ExcuteScalar<T>(string strSelectCmd, params SqlParameter[] paras)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                SqlCommand cmd = new SqlCommand(strSelectCmd, conn);
                cmd.Parameters.AddRange(paras);
                conn.Open();
                object o = cmd.ExecuteScalar();
                return (T)Convert.ChangeType(o, typeof(T));
            }
        }
        #endregion


        #region 将数据表 转成对应 T2 类型的泛型集合对象
        /// <summary>
        /// 将数据表 转成对应 T2 类型的泛型集合对象
        /// </summary>
        /// <typeparam name="T2">泛型类型</typeparam>
        /// <returns>泛型集合</returns>
        public static List<T2> Table2List<T2>(DataTable dt)
        {
            //5.将DataTable转成泛型集合List<T2>
            if (dt.Rows.Count > 0)
            {
                //6.创建泛型集合对象
                List<T2> list = new List<T2>();
                //7.遍历数据行，将行数据存入 实体对象中，并添加到 泛型集合中list
                foreach (DataRow row in dt.Rows)
                {
                    //留言：等学完反射后再讲~~~~！
                    //7.1先获得泛型的类型(里面包含该类的所有信息---有什么属性啊，有什么方法啊，什么字段啊....................)
                    Type t = typeof(T2);
                    //7.2根据类型创建该类型的对象
                    T2 model = (T2)Activator.CreateInstance(t);// new MODEL.Classes()
                    //7.3根据类型 获得 该类型的 所有属性定义
                    PropertyInfo[] properties = t.GetProperties();
                    //7.4遍历属性数组
                    foreach (PropertyInfo p in properties)
                    {
                        //7.4.1获得属性名，作为列名
                        string colName = p.Name;
                        //7.4.2根据列名 获得当前循环行对应列的值
                        object colValue = row[colName];
                        //7.4.3将 列值 赋给 model对象的p属性
                        //model.ID=colValue;
                        p.SetValue(model, colValue, null);
                    }
                    //7.5将装好 了行数据的 实体对象 添加到 泛型集合中 O了！！！
                    list.Add(model);
                }
                return list;
            }
            return null;
        }
        #endregion

        #region 操作事务
        /// <summary>
        /// 操作事务
        /// </summary>
        /// <param name="strCmd">需要执行的一系列sql语句</param>
        /// <returns>执行成功返回true，失败返回false</returns>
        public static bool Transaction(string[] strCmd)
        {
            using (SqlConnection con = new SqlConnection(connStr))
            {
                con.Open();
                SqlTransaction myTran = con.BeginTransaction();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.Transaction = myTran;
                try
                {
                    foreach (string str in strCmd)
                    {
                        cmd.CommandText = str;
                        cmd.ExecuteNonQuery();
                    }
                    myTran.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    myTran.Rollback();
                    return false;
                }
            }
        } 
        #endregion

        //#region 通知接收事务操作
        ///// <summary>
        ///// 通知接收事务操作
        ///// </summary>
        ///// <param name="receive">通知接收人一系列学号</param>
        ///// <returns>操作成功返回true</returns>
        //public static bool ReceiveNoticeTrans(ReceiveNotice[] receive)
        //{
        //    using (SqlConnection con = new SqlConnection(connStr))
        //    {
        //        con.Open();
        //        SqlTransaction myTran = con.BeginTransaction();
        //        SqlCommand cmd = new SqlCommand();
        //        cmd.Connection = con;
        //        cmd.Transaction = myTran;
        //        try
        //        {
        //            foreach (ReceiveNotice rece in receive)
        //            {
        //                cmd.CommandText = "insert into T_NoticeReceive (NoticeId,NoticeReceiver,IsRead) values(@NoticeId,@NoticeReceiver,@IsRead)";
        //                cmd.Parameters.Add(new SqlParameter("@NoticeId", rece.NoticeId));
        //                cmd.Parameters.Add(new SqlParameter("@NoticeReceiver", rece.NoticeReceiver));
        //                cmd.Parameters.Add(new SqlParameter("@IsRead", false));
        //                cmd.ExecuteNonQuery();
        //                //移除cmd里面所有的参数
        //                cmd.Parameters.Clear();
        //            }
        //            myTran.Commit();
        //            return true;
        //        }
        //        catch (Exception ex)
        //        {
        //            myTran.Rollback();
        //            return false;
        //        }
        //    }
        //} 
        //#endregion
    }
}