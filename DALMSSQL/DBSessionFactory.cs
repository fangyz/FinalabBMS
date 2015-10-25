using IDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace DALMSSQL
{
    public class DBSessionFactory:IDBSessionFactory
    {
        public IDBSession GetDBSession()
        {
            //从当前线程中 获取 DBContext 数据仓储 对象
            IDAL.IDBSession dbSesion = CallContext.GetData(typeof(DBSessionFactory).Name) as DBSession;
            if (dbSesion == null)
            {
                dbSesion = new DBSession();
                CallContext.SetData(typeof(DBSessionFactory).Name, dbSesion);
            }
            return dbSesion;
        }
    }
}
