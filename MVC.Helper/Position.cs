using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC.Helper
{
    public  class Position
    {
        public static readonly int President = 10001;//总裁
        public static readonly int Minister = 10002;//部长
        public static readonly int StudyLeader = 10003;//顾问团团长
        public static readonly int StudyMember = 10004;//顾问团团员
        public static readonly int PlanLeader = 10005;//策划组组长
        public static readonly int PlanMmember = 10006;//策划组成员
        public static readonly int  Financial = 10007;//财务主管
        public static readonly int  Student= 10009;//实习成员
        public static readonly int FullMember = 10010;//正式成员
    }

    public class Department
    {
        public static readonly int NET = 10001;//.NET
        public static readonly int JAVA = 10002;//JAVA
        public static readonly int HardWare = 10003;//硬件编程
        public static readonly int System = 10004;//系统编程部
        public static readonly int Design = 10005;//设计
        public static readonly int Scheme = 10007;//策划
    }

    public class TechnicalLevel
    {
        public static readonly int  TechBackbone = 10001;//技术骨干
        public static readonly int EliteProgram = 10002;//项目精英
        public static readonly int  FullMember = 10004;//正式成员
        public static readonly int Technician = 10005;//技术人员
        public static readonly int Student = 10003;//实习生
    }

    public class Entry
    {
        public static readonly int EntryId= 5;//录入权限Id
        public static readonly int EntryPresident = 7;//录入总裁权限ID
        public static readonly int EntryMinister = 8;//录入部长权限ID
        public static readonly int EntryStudyLeader = 9;//录入学习顾问团团长权限ID
        public static readonly int EntryStudyMember = 10;//录入学习顾问团成员权限ID
        public static readonly int EntryPlanLeadert = 11;//录入活动策划组组长权限ID
        public static readonly int EntryPlanMmember = 12;//录入活动策划组成员权限ID
        public static readonly int EntryFinancial = 13;//录入财务主管权限ID
        public static readonly int EntryMember= 14;//录入正式成员权限ID
        public static readonly int EntryDepartment = 46;//录入部门成员权限ID
    }

    public class WeekCount
    {
        public static readonly int weekCount = 18;//设置一个学期值日表的周数为16周
    }

}
