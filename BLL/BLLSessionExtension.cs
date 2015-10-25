
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IBLL;
using BLL;

namespace BLL
{
	public partial class BLLSession:IBLL.IBLLSession
    {
		#region 01 业务接口 IsysdiagramDAL
		IsdiagramBLL isdiagramBLL;
		public IsdiagramBLL IsdiagramBLL
		{
			get
			{
				if(isdiagramBLL==null)
					isdiagramBLL= new sysdiagram();
				return isdiagramBLL;
			}
			set
			{
				isdiagramBLL= value;
			}
		}
		#endregion

		#region 02 业务接口 ISystemMessageDAL
		IstemMessageBLL istemMessageBLL;
		public IstemMessageBLL IstemMessageBLL
		{
			get
			{
				if(istemMessageBLL==null)
					istemMessageBLL= new SystemMessage();
				return istemMessageBLL;
			}
			set
			{
				istemMessageBLL= value;
			}
		}
		#endregion

		#region 03 业务接口 IT_AbnormalDAL
		IAbnormalBLL iAbnormalBLL;
		public IAbnormalBLL IAbnormalBLL
		{
			get
			{
				if(iAbnormalBLL==null)
					iAbnormalBLL= new T_Abnormal();
				return iAbnormalBLL;
			}
			set
			{
				iAbnormalBLL= value;
			}
		}
		#endregion

		#region 04 业务接口 IT_AnswerSheetDAL
		IAnswerSheetBLL iAnswerSheetBLL;
		public IAnswerSheetBLL IAnswerSheetBLL
		{
			get
			{
				if(iAnswerSheetBLL==null)
					iAnswerSheetBLL= new T_AnswerSheet();
				return iAnswerSheetBLL;
			}
			set
			{
				iAnswerSheetBLL= value;
			}
		}
		#endregion

		#region 05 业务接口 IT_AnswerSheetCommentDAL
		IAnswerSheetCommentBLL iAnswerSheetCommentBLL;
		public IAnswerSheetCommentBLL IAnswerSheetCommentBLL
		{
			get
			{
				if(iAnswerSheetCommentBLL==null)
					iAnswerSheetCommentBLL= new T_AnswerSheetComment();
				return iAnswerSheetCommentBLL;
			}
			set
			{
				iAnswerSheetCommentBLL= value;
			}
		}
		#endregion

		#region 06 业务接口 IT_BriefAnswerSheetDAL
		IBriefAnswerSheetBLL iBriefAnswerSheetBLL;
		public IBriefAnswerSheetBLL IBriefAnswerSheetBLL
		{
			get
			{
				if(iBriefAnswerSheetBLL==null)
					iBriefAnswerSheetBLL= new T_BriefAnswerSheet();
				return iBriefAnswerSheetBLL;
			}
			set
			{
				iBriefAnswerSheetBLL= value;
			}
		}
		#endregion

		#region 07 业务接口 IT_BriefScoreDAL
		IBriefScoreBLL iBriefScoreBLL;
		public IBriefScoreBLL IBriefScoreBLL
		{
			get
			{
				if(iBriefScoreBLL==null)
					iBriefScoreBLL= new T_BriefScore();
				return iBriefScoreBLL;
			}
			set
			{
				iBriefScoreBLL= value;
			}
		}
		#endregion

		#region 08 业务接口 IT_ChoiceAnswerSheetDAL
		IChoiceAnswerSheetBLL iChoiceAnswerSheetBLL;
		public IChoiceAnswerSheetBLL IChoiceAnswerSheetBLL
		{
			get
			{
				if(iChoiceAnswerSheetBLL==null)
					iChoiceAnswerSheetBLL= new T_ChoiceAnswerSheet();
				return iChoiceAnswerSheetBLL;
			}
			set
			{
				iChoiceAnswerSheetBLL= value;
			}
		}
		#endregion

		#region 09 业务接口 IT_DepartmentDAL
		IDepartmentBLL iDepartmentBLL;
		public IDepartmentBLL IDepartmentBLL
		{
			get
			{
				if(iDepartmentBLL==null)
					iDepartmentBLL= new T_Department();
				return iDepartmentBLL;
			}
			set
			{
				iDepartmentBLL= value;
			}
		}
		#endregion

		#region 10 业务接口 IT_InterviewerInfoDAL
		IInterviewerInfoBLL iInterviewerInfoBLL;
		public IInterviewerInfoBLL IInterviewerInfoBLL
		{
			get
			{
				if(iInterviewerInfoBLL==null)
					iInterviewerInfoBLL= new T_InterviewerInfo();
				return iInterviewerInfoBLL;
			}
			set
			{
				iInterviewerInfoBLL= value;
			}
		}
		#endregion

		#region 11 业务接口 IT_IsShowDAL
		IIsShowBLL iIsShowBLL;
		public IIsShowBLL IIsShowBLL
		{
			get
			{
				if(iIsShowBLL==null)
					iIsShowBLL= new T_IsShow();
				return iIsShowBLL;
			}
			set
			{
				iIsShowBLL= value;
			}
		}
		#endregion

		#region 12 业务接口 IT_MemberInformationDAL
		IMemberInformationBLL iMemberInformationBLL;
		public IMemberInformationBLL IMemberInformationBLL
		{
			get
			{
				if(iMemberInformationBLL==null)
					iMemberInformationBLL= new T_MemberInformation();
				return iMemberInformationBLL;
			}
			set
			{
				iMemberInformationBLL= value;
			}
		}
		#endregion

		#region 13 业务接口 IT_OgnizationActDAL
		IOgnizationActBLL iOgnizationActBLL;
		public IOgnizationActBLL IOgnizationActBLL
		{
			get
			{
				if(iOgnizationActBLL==null)
					iOgnizationActBLL= new T_OgnizationAct();
				return iOgnizationActBLL;
			}
			set
			{
				iOgnizationActBLL= value;
			}
		}
		#endregion

		#region 14 业务接口 IT_OnDutyDAL
		IOnDutyBLL iOnDutyBLL;
		public IOnDutyBLL IOnDutyBLL
		{
			get
			{
				if(iOnDutyBLL==null)
					iOnDutyBLL= new T_OnDuty();
				return iOnDutyBLL;
			}
			set
			{
				iOnDutyBLL= value;
			}
		}
		#endregion

		#region 15 业务接口 IT_OrganizationDAL
		IOrganizationBLL iOrganizationBLL;
		public IOrganizationBLL IOrganizationBLL
		{
			get
			{
				if(iOrganizationBLL==null)
					iOrganizationBLL= new T_Organization();
				return iOrganizationBLL;
			}
			set
			{
				iOrganizationBLL= value;
			}
		}
		#endregion

		#region 16 业务接口 IT_PaperDAL
		IPaperBLL iPaperBLL;
		public IPaperBLL IPaperBLL
		{
			get
			{
				if(iPaperBLL==null)
					iPaperBLL= new T_Paper();
				return iPaperBLL;
			}
			set
			{
				iPaperBLL= value;
			}
		}
		#endregion

		#region 17 业务接口 IT_PaperQuestionDAL
		IPaperQuestionBLL iPaperQuestionBLL;
		public IPaperQuestionBLL IPaperQuestionBLL
		{
			get
			{
				if(iPaperQuestionBLL==null)
					iPaperQuestionBLL= new T_PaperQuestion();
				return iPaperQuestionBLL;
			}
			set
			{
				iPaperQuestionBLL= value;
			}
		}
		#endregion

		#region 18 业务接口 IT_PermissionDAL
		IPermissionBLL iPermissionBLL;
		public IPermissionBLL IPermissionBLL
		{
			get
			{
				if(iPermissionBLL==null)
					iPermissionBLL= new T_Permission();
				return iPermissionBLL;
			}
			set
			{
				iPermissionBLL= value;
			}
		}
		#endregion

		#region 19 业务接口 IT_ProjectInformationDAL
		IProjectInformationBLL iProjectInformationBLL;
		public IProjectInformationBLL IProjectInformationBLL
		{
			get
			{
				if(iProjectInformationBLL==null)
					iProjectInformationBLL= new T_ProjectInformation();
				return iProjectInformationBLL;
			}
			set
			{
				iProjectInformationBLL= value;
			}
		}
		#endregion

		#region 20 业务接口 IT_ProjectParticipationDAL
		IProjectParticipationBLL iProjectParticipationBLL;
		public IProjectParticipationBLL IProjectParticipationBLL
		{
			get
			{
				if(iProjectParticipationBLL==null)
					iProjectParticipationBLL= new T_ProjectParticipation();
				return iProjectParticipationBLL;
			}
			set
			{
				iProjectParticipationBLL= value;
			}
		}
		#endregion

		#region 21 业务接口 IT_ProjectTypeDAL
		IProjectTypeBLL iProjectTypeBLL;
		public IProjectTypeBLL IProjectTypeBLL
		{
			get
			{
				if(iProjectTypeBLL==null)
					iProjectTypeBLL= new T_ProjectType();
				return iProjectTypeBLL;
			}
			set
			{
				iProjectTypeBLL= value;
			}
		}
		#endregion

		#region 22 业务接口 IT_ProjPhaseDAL
		IProjPhaseBLL iProjPhaseBLL;
		public IProjPhaseBLL IProjPhaseBLL
		{
			get
			{
				if(iProjPhaseBLL==null)
					iProjPhaseBLL= new T_ProjPhase();
				return iProjPhaseBLL;
			}
			set
			{
				iProjPhaseBLL= value;
			}
		}
		#endregion

		#region 23 业务接口 IT_QuestionDAL
		IQuestionBLL iQuestionBLL;
		public IQuestionBLL IQuestionBLL
		{
			get
			{
				if(iQuestionBLL==null)
					iQuestionBLL= new T_Question();
				return iQuestionBLL;
			}
			set
			{
				iQuestionBLL= value;
			}
		}
		#endregion

		#region 24 业务接口 IT_QuestionOptionDAL
		IQuestionOptionBLL iQuestionOptionBLL;
		public IQuestionOptionBLL IQuestionOptionBLL
		{
			get
			{
				if(iQuestionOptionBLL==null)
					iQuestionOptionBLL= new T_QuestionOption();
				return iQuestionOptionBLL;
			}
			set
			{
				iQuestionOptionBLL= value;
			}
		}
		#endregion

		#region 25 业务接口 IT_QuestionTypeDAL
		IQuestionTypeBLL iQuestionTypeBLL;
		public IQuestionTypeBLL IQuestionTypeBLL
		{
			get
			{
				if(iQuestionTypeBLL==null)
					iQuestionTypeBLL= new T_QuestionType();
				return iQuestionTypeBLL;
			}
			set
			{
				iQuestionTypeBLL= value;
			}
		}
		#endregion

		#region 26 业务接口 IT_RoleDAL
		IRoleBLL iRoleBLL;
		public IRoleBLL IRoleBLL
		{
			get
			{
				if(iRoleBLL==null)
					iRoleBLL= new T_Role();
				return iRoleBLL;
			}
			set
			{
				iRoleBLL= value;
			}
		}
		#endregion

		#region 27 业务接口 IT_RoleActDAL
		IRoleActBLL iRoleActBLL;
		public IRoleActBLL IRoleActBLL
		{
			get
			{
				if(iRoleActBLL==null)
					iRoleActBLL= new T_RoleAct();
				return iRoleActBLL;
			}
			set
			{
				iRoleActBLL= value;
			}
		}
		#endregion

		#region 28 业务接口 IT_RolePermissionDAL
		IRolePermissionBLL iRolePermissionBLL;
		public IRolePermissionBLL IRolePermissionBLL
		{
			get
			{
				if(iRolePermissionBLL==null)
					iRolePermissionBLL= new T_RolePermission();
				return iRolePermissionBLL;
			}
			set
			{
				iRolePermissionBLL= value;
			}
		}
		#endregion

		#region 29 业务接口 IT_TaskInformationDAL
		ITaskInformationBLL iTaskInformationBLL;
		public ITaskInformationBLL ITaskInformationBLL
		{
			get
			{
				if(iTaskInformationBLL==null)
					iTaskInformationBLL= new T_TaskInformation();
				return iTaskInformationBLL;
			}
			set
			{
				iTaskInformationBLL= value;
			}
		}
		#endregion

		#region 30 业务接口 IT_TaskParticipationDAL
		ITaskParticipationBLL iTaskParticipationBLL;
		public ITaskParticipationBLL ITaskParticipationBLL
		{
			get
			{
				if(iTaskParticipationBLL==null)
					iTaskParticipationBLL= new T_TaskParticipation();
				return iTaskParticipationBLL;
			}
			set
			{
				iTaskParticipationBLL= value;
			}
		}
		#endregion

		#region 31 业务接口 IT_TaskTypeDAL
		ITaskTypeBLL iTaskTypeBLL;
		public ITaskTypeBLL ITaskTypeBLL
		{
			get
			{
				if(iTaskTypeBLL==null)
					iTaskTypeBLL= new T_TaskType();
				return iTaskTypeBLL;
			}
			set
			{
				iTaskTypeBLL= value;
			}
		}
		#endregion

		#region 32 业务接口 IT_TeacherInfoDAL
		ITeacherInfoBLL iTeacherInfoBLL;
		public ITeacherInfoBLL ITeacherInfoBLL
		{
			get
			{
				if(iTeacherInfoBLL==null)
					iTeacherInfoBLL= new T_TeacherInfo();
				return iTeacherInfoBLL;
			}
			set
			{
				iTeacherInfoBLL= value;
			}
		}
		#endregion

		#region 33 业务接口 IT_TechnicaLevelDAL
		ITechnicaLevelBLL iTechnicaLevelBLL;
		public ITechnicaLevelBLL ITechnicaLevelBLL
		{
			get
			{
				if(iTechnicaLevelBLL==null)
					iTechnicaLevelBLL= new T_TechnicaLevel();
				return iTechnicaLevelBLL;
			}
			set
			{
				iTechnicaLevelBLL= value;
			}
		}
		#endregion

		#region 34 业务接口 ITbl_MessageDAL
		Il_MessageBLL il_MessageBLL;
		public Il_MessageBLL Il_MessageBLL
		{
			get
			{
				if(il_MessageBLL==null)
					il_MessageBLL= new Tbl_Message();
				return il_MessageBLL;
			}
			set
			{
				il_MessageBLL= value;
			}
		}
		#endregion

		#region 35 业务接口 ITbl_User_MessageDAL
		Il_User_MessageBLL il_User_MessageBLL;
		public Il_User_MessageBLL Il_User_MessageBLL
		{
			get
			{
				if(il_User_MessageBLL==null)
					il_User_MessageBLL= new Tbl_User_Message();
				return il_User_MessageBLL;
			}
			set
			{
				il_User_MessageBLL= value;
			}
		}
		#endregion

    }

}