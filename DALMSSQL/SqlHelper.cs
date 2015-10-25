using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace DALMSSQL
{
    public class SqlHelper
    {
        //static string connstr = "server=.;database=heimablog;uid=sa;pwd=suncoder";

        #region connstr
        //2.WinForm项目，请添加对“System.Configuration”的引用
        //2.1.对配置文件connectionStrings节进行读取
        static string connstr = System.Configuration.ConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
        #endregion

        #region 查询数据库返回一张表 + static DataTable ExecuteDataTable(string sql, params SqlParameter[] parameters)
        /// <summary>
        /// 查询数据库返回一张表
        /// </summary>
        /// <param name="sql">查询字符串</param>
        /// <param name="parameters">查询所需要的参数</param>
        /// <returns>返回查询的结果的第一个结果集的表</returns>
        public static DataTable ExecuteDataTable(string sql, params SqlParameter[] parameters)
        {
            //创建一个零时的数据集，在内存里
            DataSet ds = new DataSet();
            //创建一个适配器对象，
            SqlDataAdapter adapter = new SqlDataAdapter(sql, connstr);
            //给适配器对象的查询命令对象添加参数
            adapter.SelectCommand.Parameters.AddRange(parameters);
            //try
            //{
            //    //填充数据集
            //    adapter.Fill(ds);
            //    return ds.Tables[0];
            //}
            //catch//如果捕捉错误，那么就返回null
            //{
            //    return null;
            //}
            adapter.Fill(ds);
            return ds.Tables[0];
        }
        #endregion

        //返回受影响行数
        public static int ExecuteNonQuery(string sql, params SqlParameter[] parameters)
        {
            using (SqlConnection conn = new SqlConnection(connstr))
            {
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddRange(parameters);
                    conn.Open();
                    return cmd.ExecuteNonQuery();
                }
            }
        }
        //返回查询结果的第一行第一列
        public static object ExecuteScalar(string sql, params SqlParameter[] parameters)
        {
            using (SqlConnection conn = new SqlConnection(connstr))
            {
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddRange(parameters);
                    conn.Open();
                    return cmd.ExecuteScalar();
                }
            }
        }

      
    }
}
