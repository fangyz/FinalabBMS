using IDAL;
using MODEL;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace DALMSSQL
{
   public class DBContextFactory:IDBContextFactory
    {
        public DbContext GetDbContext()
        {
            //从当前线程中 获取 EF上下文对象
            DbContext dbContext = CallContext.GetData(typeof(DBContextFactory).Name) as DbContext;
            if (dbContext == null)
            {
                dbContext = new FinalEntities();
                dbContext.Configuration.ValidateOnSaveEnabled = false;
                //将新创建的 ef上下文对象 存入线程
                CallContext.SetData(typeof(DBContextFactory).Name, dbContext);
            }
            return dbContext;
        }
    }
}
