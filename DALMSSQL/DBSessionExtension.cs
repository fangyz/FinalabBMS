
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IDAL;

namespace DALMSSQL
{
	public partial class DBSession:IDAL.IDBSession
    {
		#region 01 数据接口 IsdiagramDAL
		IsdiagramDAL isdiagramDAL;
		public IsdiagramDAL IsdiagramDAL
		{
			get
			{
				if(isdiagramDAL==null)
					isdiagramDAL= new sysdiagramDAL();
				return isdiagramDAL;
			}
			set
			{
				isdiagramDAL= value;
			}
		}
		#endregion

		#region 02 数据接口 IstemMessageDAL
		IstemMessageDAL istemMessageDAL;
		public IstemMessageDAL IstemMessageDAL
		{
			get
			{
				if(istemMessageDAL==null)
					istemMessageDAL= new SystemMessageDAL();
				return istemMessageDAL;
			}
			set
			{
				istemMessageDAL= value;
			}
		}
		#endregion

		#region 03 数据接口 IAbnormalDAL
		IAbnormalDAL iAbnormalDAL;
		public IAbnormalDAL IAbnormalDAL
		{
			get
			{
				if(iAbnormalDAL==null)
					iAbnormalDAL= new T_AbnormalDAL();
				return iAbnormalDAL;
			}
			set
			{
				iAbnormalDAL= value;
			}
		}
		#endregion

		#region 04 数据接口 IAnswerSheetDAL
		IAnswerSheetDAL iAnswerSheetDAL;
		public IAnswerSheetDAL IAnswerSheetDAL
		{
			get
			{
				if(iAnswerSheetDAL==null)
					iAnswerSheetDAL= new T_AnswerSheetDAL();
				return iAnswerSheetDAL;
			}
			set
			{
				iAnswerSheetDAL= value;
			}
		}
		#endregion

		#region 05 数据接口 IAnswerSheetCommentDAL
		IAnswerSheetCommentDAL iAnswerSheetCommentDAL;
		public IAnswerSheetCommentDAL IAnswerSheetCommentDAL
		{
			get
			{
				if(iAnswerSheetCommentDAL==null)
					iAnswerSheetCommentDAL= new T_AnswerSheetCommentDAL();
				return iAnswerSheetCommentDAL;
			}
			set
			{
				iAnswerSheetCommentDAL= value;
			}
		}
		#endregion

		#region 06 数据接口 IBriefAnswerSheetDAL
		IBriefAnswerSheetDAL iBriefAnswerSheetDAL;
		public IBriefAnswerSheetDAL IBriefAnswerSheetDAL
		{
			get
			{
				if(iBriefAnswerSheetDAL==null)
					iBriefAnswerSheetDAL= new T_BriefAnswerSheetDAL();
				return iBriefAnswerSheetDAL;
			}
			set
			{
				iBriefAnswerSheetDAL= value;
			}
		}
		#endregion

		#region 07 数据接口 IBriefScoreDAL
		IBriefScoreDAL iBriefScoreDAL;
		public IBriefScoreDAL IBriefScoreDAL
		{
			get
			{
				if(iBriefScoreDAL==null)
					iBriefScoreDAL= new T_BriefScoreDAL();
				return iBriefScoreDAL;
			}
			set
			{
				iBriefScoreDAL= value;
			}
		}
		#endregion

		#region 08 数据接口 IChoiceAnswerSheetDAL
		IChoiceAnswerSheetDAL iChoiceAnswerSheetDAL;
		public IChoiceAnswerSheetDAL IChoiceAnswerSheetDAL
		{
			get
			{
				if(iChoiceAnswerSheetDAL==null)
					iChoiceAnswerSheetDAL= new T_ChoiceAnswerSheetDAL();
				return iChoiceAnswerSheetDAL;
			}
			set
			{
				iChoiceAnswerSheetDAL= value;
			}
		}
		#endregion

		#region 09 数据接口 IDepartmentDAL
		IDepartmentDAL iDepartmentDAL;
		public IDepartmentDAL IDepartmentDAL
		{
			get
			{
				if(iDepartmentDAL==null)
					iDepartmentDAL= new T_DepartmentDAL();
				return iDepartmentDAL;
			}
			set
			{
				iDepartmentDAL= value;
			}
		}
		#endregion

		#region 10 数据接口 IInterviewerInfoDAL
		IInterviewerInfoDAL iInterviewerInfoDAL;
		public IInterviewerInfoDAL IInterviewerInfoDAL
		{
			get
			{
				if(iInterviewerInfoDAL==null)
					iInterviewerInfoDAL= new T_InterviewerInfoDAL();
				return iInterviewerInfoDAL;
			}
			set
			{
				iInterviewerInfoDAL= value;
			}
		}
		#endregion

		#region 11 数据接口 IIsShowDAL
		IIsShowDAL iIsShowDAL;
		public IIsShowDAL IIsShowDAL
		{
			get
			{
				if(iIsShowDAL==null)
					iIsShowDAL= new T_IsShowDAL();
				return iIsShowDAL;
			}
			set
			{
				iIsShowDAL= value;
			}
		}
		#endregion

		#region 12 数据接口 IMemberInformationDAL
		IMemberInformationDAL iMemberInformationDAL;
		public IMemberInformationDAL IMemberInformationDAL
		{
			get
			{
				if(iMemberInformationDAL==null)
					iMemberInformationDAL= new T_MemberInformationDAL();
				return iMemberInformationDAL;
			}
			set
			{
				iMemberInformationDAL= value;
			}
		}
		#endregion

		#region 13 数据接口 IOgnizationActDAL
		IOgnizationActDAL iOgnizationActDAL;
		public IOgnizationActDAL IOgnizationActDAL
		{
			get
			{
				if(iOgnizationActDAL==null)
					iOgnizationActDAL= new T_OgnizationActDAL();
				return iOgnizationActDAL;
			}
			set
			{
				iOgnizationActDAL= value;
			}
		}
		#endregion

		#region 14 数据接口 IOnDutyDAL
		IOnDutyDAL iOnDutyDAL;
		public IOnDutyDAL IOnDutyDAL
		{
			get
			{
				if(iOnDutyDAL==null)
					iOnDutyDAL= new T_OnDutyDAL();
				return iOnDutyDAL;
			}
			set
			{
				iOnDutyDAL= value;
			}
		}
		#endregion

		#region 15 数据接口 IOrganizationDAL
		IOrganizationDAL iOrganizationDAL;
		public IOrganizationDAL IOrganizationDAL
		{
			get
			{
				if(iOrganizationDAL==null)
					iOrganizationDAL= new T_OrganizationDAL();
				return iOrganizationDAL;
			}
			set
			{
				iOrganizationDAL= value;
			}
		}
		#endregion

		#region 16 数据接口 IPaperDAL
		IPaperDAL iPaperDAL;
		public IPaperDAL IPaperDAL
		{
			get
			{
				if(iPaperDAL==null)
					iPaperDAL= new T_PaperDAL();
				return iPaperDAL;
			}
			set
			{
				iPaperDAL= value;
			}
		}
		#endregion

		#region 17 数据接口 IPaperQuestionDAL
		IPaperQuestionDAL iPaperQuestionDAL;
		public IPaperQuestionDAL IPaperQuestionDAL
		{
			get
			{
				if(iPaperQuestionDAL==null)
					iPaperQuestionDAL= new T_PaperQuestionDAL();
				return iPaperQuestionDAL;
			}
			set
			{
				iPaperQuestionDAL= value;
			}
		}
		#endregion

		#region 18 数据接口 IPermissionDAL
		IPermissionDAL iPermissionDAL;
		public IPermissionDAL IPermissionDAL
		{
			get
			{
				if(iPermissionDAL==null)
					iPermissionDAL= new T_PermissionDAL();
				return iPermissionDAL;
			}
			set
			{
				iPermissionDAL= value;
			}
		}
		#endregion

		#region 19 数据接口 IProjectInformationDAL
		IProjectInformationDAL iProjectInformationDAL;
		public IProjectInformationDAL IProjectInformationDAL
		{
			get
			{
				if(iProjectInformationDAL==null)
					iProjectInformationDAL= new T_ProjectInformationDAL();
				return iProjectInformationDAL;
			}
			set
			{
				iProjectInformationDAL= value;
			}
		}
		#endregion

		#region 20 数据接口 IProjectParticipationDAL
		IProjectParticipationDAL iProjectParticipationDAL;
		public IProjectParticipationDAL IProjectParticipationDAL
		{
			get
			{
				if(iProjectParticipationDAL==null)
					iProjectParticipationDAL= new T_ProjectParticipationDAL();
				return iProjectParticipationDAL;
			}
			set
			{
				iProjectParticipationDAL= value;
			}
		}
		#endregion

		#region 21 数据接口 IProjectTypeDAL
		IProjectTypeDAL iProjectTypeDAL;
		public IProjectTypeDAL IProjectTypeDAL
		{
			get
			{
				if(iProjectTypeDAL==null)
					iProjectTypeDAL= new T_ProjectTypeDAL();
				return iProjectTypeDAL;
			}
			set
			{
				iProjectTypeDAL= value;
			}
		}
		#endregion

		#region 22 数据接口 IProjPhaseDAL
		IProjPhaseDAL iProjPhaseDAL;
		public IProjPhaseDAL IProjPhaseDAL
		{
			get
			{
				if(iProjPhaseDAL==null)
					iProjPhaseDAL= new T_ProjPhaseDAL();
				return iProjPhaseDAL;
			}
			set
			{
				iProjPhaseDAL= value;
			}
		}
		#endregion

		#region 23 数据接口 IQuestionDAL
		IQuestionDAL iQuestionDAL;
		public IQuestionDAL IQuestionDAL
		{
			get
			{
				if(iQuestionDAL==null)
					iQuestionDAL= new T_QuestionDAL();
				return iQuestionDAL;
			}
			set
			{
				iQuestionDAL= value;
			}
		}
		#endregion

		#region 24 数据接口 IQuestionOptionDAL
		IQuestionOptionDAL iQuestionOptionDAL;
		public IQuestionOptionDAL IQuestionOptionDAL
		{
			get
			{
				if(iQuestionOptionDAL==null)
					iQuestionOptionDAL= new T_QuestionOptionDAL();
				return iQuestionOptionDAL;
			}
			set
			{
				iQuestionOptionDAL= value;
			}
		}
		#endregion

		#region 25 数据接口 IQuestionTypeDAL
		IQuestionTypeDAL iQuestionTypeDAL;
		public IQuestionTypeDAL IQuestionTypeDAL
		{
			get
			{
				if(iQuestionTypeDAL==null)
					iQuestionTypeDAL= new T_QuestionTypeDAL();
				return iQuestionTypeDAL;
			}
			set
			{
				iQuestionTypeDAL= value;
			}
		}
		#endregion

		#region 26 数据接口 IRoleDAL
		IRoleDAL iRoleDAL;
		public IRoleDAL IRoleDAL
		{
			get
			{
				if(iRoleDAL==null)
					iRoleDAL= new T_RoleDAL();
				return iRoleDAL;
			}
			set
			{
				iRoleDAL= value;
			}
		}
		#endregion

		#region 27 数据接口 IRoleActDAL
		IRoleActDAL iRoleActDAL;
		public IRoleActDAL IRoleActDAL
		{
			get
			{
				if(iRoleActDAL==null)
					iRoleActDAL= new T_RoleActDAL();
				return iRoleActDAL;
			}
			set
			{
				iRoleActDAL= value;
			}
		}
		#endregion

		#region 28 数据接口 IRolePermissionDAL
		IRolePermissionDAL iRolePermissionDAL;
		public IRolePermissionDAL IRolePermissionDAL
		{
			get
			{
				if(iRolePermissionDAL==null)
					iRolePermissionDAL= new T_RolePermissionDAL();
				return iRolePermissionDAL;
			}
			set
			{
				iRolePermissionDAL= value;
			}
		}
		#endregion

		#region 29 数据接口 ITaskInformationDAL
		ITaskInformationDAL iTaskInformationDAL;
		public ITaskInformationDAL ITaskInformationDAL
		{
			get
			{
				if(iTaskInformationDAL==null)
					iTaskInformationDAL= new T_TaskInformationDAL();
				return iTaskInformationDAL;
			}
			set
			{
				iTaskInformationDAL= value;
			}
		}
		#endregion

		#region 30 数据接口 ITaskParticipationDAL
		ITaskParticipationDAL iTaskParticipationDAL;
		public ITaskParticipationDAL ITaskParticipationDAL
		{
			get
			{
				if(iTaskParticipationDAL==null)
					iTaskParticipationDAL= new T_TaskParticipationDAL();
				return iTaskParticipationDAL;
			}
			set
			{
				iTaskParticipationDAL= value;
			}
		}
		#endregion

		#region 31 数据接口 ITaskTypeDAL
		ITaskTypeDAL iTaskTypeDAL;
		public ITaskTypeDAL ITaskTypeDAL
		{
			get
			{
				if(iTaskTypeDAL==null)
					iTaskTypeDAL= new T_TaskTypeDAL();
				return iTaskTypeDAL;
			}
			set
			{
				iTaskTypeDAL= value;
			}
		}
		#endregion

		#region 32 数据接口 ITeacherInfoDAL
		ITeacherInfoDAL iTeacherInfoDAL;
		public ITeacherInfoDAL ITeacherInfoDAL
		{
			get
			{
				if(iTeacherInfoDAL==null)
					iTeacherInfoDAL= new T_TeacherInfoDAL();
				return iTeacherInfoDAL;
			}
			set
			{
				iTeacherInfoDAL= value;
			}
		}
		#endregion

		#region 33 数据接口 ITechnicaLevelDAL
		ITechnicaLevelDAL iTechnicaLevelDAL;
		public ITechnicaLevelDAL ITechnicaLevelDAL
		{
			get
			{
				if(iTechnicaLevelDAL==null)
					iTechnicaLevelDAL= new T_TechnicaLevelDAL();
				return iTechnicaLevelDAL;
			}
			set
			{
				iTechnicaLevelDAL= value;
			}
		}
		#endregion

		#region 34 数据接口 Il_MessageDAL
		Il_MessageDAL il_MessageDAL;
		public Il_MessageDAL Il_MessageDAL
		{
			get
			{
				if(il_MessageDAL==null)
					il_MessageDAL= new Tbl_MessageDAL();
				return il_MessageDAL;
			}
			set
			{
				il_MessageDAL= value;
			}
		}
		#endregion

		#region 35 数据接口 Il_User_MessageDAL
		Il_User_MessageDAL il_User_MessageDAL;
		public Il_User_MessageDAL Il_User_MessageDAL
		{
			get
			{
				if(il_User_MessageDAL==null)
					il_User_MessageDAL= new Tbl_User_MessageDAL();
				return il_User_MessageDAL;
			}
			set
			{
				il_User_MessageDAL= value;
			}
		}
		#endregion

    }

}