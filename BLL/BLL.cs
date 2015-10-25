 using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL
{
	public partial class sysdiagram : BaseBLL<MODEL.sysdiagram>,IBLL.IsdiagramBLL
    {
		public override void SetDAL()
		{
			idal = DBSession.IsdiagramDAL;
		}
    }

	public partial class SystemMessage : BaseBLL<MODEL.SystemMessage>,IBLL.IstemMessageBLL
    {
		public override void SetDAL()
		{
			idal = DBSession.IstemMessageDAL;
		}
    }

	public partial class T_Abnormal : BaseBLL<MODEL.T_Abnormal>,IBLL.IAbnormalBLL
    {
		public override void SetDAL()
		{
			idal = DBSession.IAbnormalDAL;
		}
    }

	public partial class T_AnswerSheet : BaseBLL<MODEL.T_AnswerSheet>,IBLL.IAnswerSheetBLL
    {
		public override void SetDAL()
		{
			idal = DBSession.IAnswerSheetDAL;
		}
    }

	public partial class T_AnswerSheetComment : BaseBLL<MODEL.T_AnswerSheetComment>,IBLL.IAnswerSheetCommentBLL
    {
		public override void SetDAL()
		{
			idal = DBSession.IAnswerSheetCommentDAL;
		}
    }

	public partial class T_BriefAnswerSheet : BaseBLL<MODEL.T_BriefAnswerSheet>,IBLL.IBriefAnswerSheetBLL
    {
		public override void SetDAL()
		{
			idal = DBSession.IBriefAnswerSheetDAL;
		}
    }

	public partial class T_BriefScore : BaseBLL<MODEL.T_BriefScore>,IBLL.IBriefScoreBLL
    {
		public override void SetDAL()
		{
			idal = DBSession.IBriefScoreDAL;
		}
    }

	public partial class T_ChoiceAnswerSheet : BaseBLL<MODEL.T_ChoiceAnswerSheet>,IBLL.IChoiceAnswerSheetBLL
    {
		public override void SetDAL()
		{
			idal = DBSession.IChoiceAnswerSheetDAL;
		}
    }

	public partial class T_Department : BaseBLL<MODEL.T_Department>,IBLL.IDepartmentBLL
    {
		public override void SetDAL()
		{
			idal = DBSession.IDepartmentDAL;
		}
    }

	public partial class T_InterviewerInfo : BaseBLL<MODEL.T_InterviewerInfo>,IBLL.IInterviewerInfoBLL
    {
		public override void SetDAL()
		{
			idal = DBSession.IInterviewerInfoDAL;
		}
    }

	public partial class T_IsShow : BaseBLL<MODEL.T_IsShow>,IBLL.IIsShowBLL
    {
		public override void SetDAL()
		{
			idal = DBSession.IIsShowDAL;
		}
    }

	public partial class T_MemberInformation : BaseBLL<MODEL.T_MemberInformation>,IBLL.IMemberInformationBLL
    {
		public override void SetDAL()
		{
			idal = DBSession.IMemberInformationDAL;
		}
    }

	public partial class T_OgnizationAct : BaseBLL<MODEL.T_OgnizationAct>,IBLL.IOgnizationActBLL
    {
		public override void SetDAL()
		{
			idal = DBSession.IOgnizationActDAL;
		}
    }

	public partial class T_OnDuty : BaseBLL<MODEL.T_OnDuty>,IBLL.IOnDutyBLL
    {
		public override void SetDAL()
		{
			idal = DBSession.IOnDutyDAL;
		}
    }

	public partial class T_Organization : BaseBLL<MODEL.T_Organization>,IBLL.IOrganizationBLL
    {
		public override void SetDAL()
		{
			idal = DBSession.IOrganizationDAL;
		}
    }

	public partial class T_Paper : BaseBLL<MODEL.T_Paper>,IBLL.IPaperBLL
    {
		public override void SetDAL()
		{
			idal = DBSession.IPaperDAL;
		}
    }

	public partial class T_PaperQuestion : BaseBLL<MODEL.T_PaperQuestion>,IBLL.IPaperQuestionBLL
    {
		public override void SetDAL()
		{
			idal = DBSession.IPaperQuestionDAL;
		}
    }

	public partial class T_Permission : BaseBLL<MODEL.T_Permission>,IBLL.IPermissionBLL
    {
		public override void SetDAL()
		{
			idal = DBSession.IPermissionDAL;
		}
    }

	public partial class T_ProjectInformation : BaseBLL<MODEL.T_ProjectInformation>,IBLL.IProjectInformationBLL
    {
		public override void SetDAL()
		{
			idal = DBSession.IProjectInformationDAL;
		}
    }

	public partial class T_ProjectParticipation : BaseBLL<MODEL.T_ProjectParticipation>,IBLL.IProjectParticipationBLL
    {
		public override void SetDAL()
		{
			idal = DBSession.IProjectParticipationDAL;
		}
    }

	public partial class T_ProjectType : BaseBLL<MODEL.T_ProjectType>,IBLL.IProjectTypeBLL
    {
		public override void SetDAL()
		{
			idal = DBSession.IProjectTypeDAL;
		}
    }

	public partial class T_ProjPhase : BaseBLL<MODEL.T_ProjPhase>,IBLL.IProjPhaseBLL
    {
		public override void SetDAL()
		{
			idal = DBSession.IProjPhaseDAL;
		}
    }

	public partial class T_Question : BaseBLL<MODEL.T_Question>,IBLL.IQuestionBLL
    {
		public override void SetDAL()
		{
			idal = DBSession.IQuestionDAL;
		}
    }

	public partial class T_QuestionOption : BaseBLL<MODEL.T_QuestionOption>,IBLL.IQuestionOptionBLL
    {
		public override void SetDAL()
		{
			idal = DBSession.IQuestionOptionDAL;
		}
    }

	public partial class T_QuestionType : BaseBLL<MODEL.T_QuestionType>,IBLL.IQuestionTypeBLL
    {
		public override void SetDAL()
		{
			idal = DBSession.IQuestionTypeDAL;
		}
    }

	public partial class T_Role : BaseBLL<MODEL.T_Role>,IBLL.IRoleBLL
    {
		public override void SetDAL()
		{
			idal = DBSession.IRoleDAL;
		}
    }

	public partial class T_RoleAct : BaseBLL<MODEL.T_RoleAct>,IBLL.IRoleActBLL
    {
		public override void SetDAL()
		{
			idal = DBSession.IRoleActDAL;
		}
    }

	public partial class T_RolePermission : BaseBLL<MODEL.T_RolePermission>,IBLL.IRolePermissionBLL
    {
		public override void SetDAL()
		{
			idal = DBSession.IRolePermissionDAL;
		}
    }

	public partial class T_TaskInformation : BaseBLL<MODEL.T_TaskInformation>,IBLL.ITaskInformationBLL
    {
		public override void SetDAL()
		{
			idal = DBSession.ITaskInformationDAL;
		}
    }

	public partial class T_TaskParticipation : BaseBLL<MODEL.T_TaskParticipation>,IBLL.ITaskParticipationBLL
    {
		public override void SetDAL()
		{
			idal = DBSession.ITaskParticipationDAL;
		}
    }

	public partial class T_TaskType : BaseBLL<MODEL.T_TaskType>,IBLL.ITaskTypeBLL
    {
		public override void SetDAL()
		{
			idal = DBSession.ITaskTypeDAL;
		}
    }

	public partial class T_TeacherInfo : BaseBLL<MODEL.T_TeacherInfo>,IBLL.ITeacherInfoBLL
    {
		public override void SetDAL()
		{
			idal = DBSession.ITeacherInfoDAL;
		}
    }

	public partial class T_TechnicaLevel : BaseBLL<MODEL.T_TechnicaLevel>,IBLL.ITechnicaLevelBLL
    {
		public override void SetDAL()
		{
			idal = DBSession.ITechnicaLevelDAL;
		}
    }

	public partial class Tbl_Message : BaseBLL<MODEL.Tbl_Message>,IBLL.Il_MessageBLL
    {
		public override void SetDAL()
		{
			idal = DBSession.Il_MessageDAL;
		}
    }

	public partial class Tbl_User_Message : BaseBLL<MODEL.Tbl_User_Message>,IBLL.Il_User_MessageBLL
    {
		public override void SetDAL()
		{
			idal = DBSession.Il_User_MessageDAL;
		}
    }

}