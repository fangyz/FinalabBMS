
 using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace IDAL
{

	public partial interface IsdiagramDAL : IBaseDAL<MODEL.sysdiagram>
    {
    }

	public partial interface IstemMessageDAL : IBaseDAL<MODEL.SystemMessage>
    {
    }

	public partial interface IAbnormalDAL : IBaseDAL<MODEL.T_Abnormal>
    {
    }

	public partial interface IAnswerSheetDAL : IBaseDAL<MODEL.T_AnswerSheet>
    {
    }

	public partial interface IAnswerSheetCommentDAL : IBaseDAL<MODEL.T_AnswerSheetComment>
    {
    }

	public partial interface IBriefAnswerSheetDAL : IBaseDAL<MODEL.T_BriefAnswerSheet>
    {
    }

	public partial interface IBriefScoreDAL : IBaseDAL<MODEL.T_BriefScore>
    {
    }

	public partial interface IChoiceAnswerSheetDAL : IBaseDAL<MODEL.T_ChoiceAnswerSheet>
    {
    }

	public partial interface IDepartmentDAL : IBaseDAL<MODEL.T_Department>
    {
    }

	public partial interface IInterviewerInfoDAL : IBaseDAL<MODEL.T_InterviewerInfo>
    {
    }

	public partial interface IIsShowDAL : IBaseDAL<MODEL.T_IsShow>
    {
    }

	public partial interface IMemberInformationDAL : IBaseDAL<MODEL.T_MemberInformation>
    {
    }

	public partial interface IOgnizationActDAL : IBaseDAL<MODEL.T_OgnizationAct>
    {
    }

	public partial interface IOnDutyDAL : IBaseDAL<MODEL.T_OnDuty>
    {
    }

	public partial interface IOrganizationDAL : IBaseDAL<MODEL.T_Organization>
    {
    }

	public partial interface IPaperDAL : IBaseDAL<MODEL.T_Paper>
    {
    }

	public partial interface IPaperQuestionDAL : IBaseDAL<MODEL.T_PaperQuestion>
    {
    }

	public partial interface IPermissionDAL : IBaseDAL<MODEL.T_Permission>
    {
    }

	public partial interface IProjectInformationDAL : IBaseDAL<MODEL.T_ProjectInformation>
    {
    }

	public partial interface IProjectParticipationDAL : IBaseDAL<MODEL.T_ProjectParticipation>
    {
    }

	public partial interface IProjectTypeDAL : IBaseDAL<MODEL.T_ProjectType>
    {
    }

	public partial interface IProjPhaseDAL : IBaseDAL<MODEL.T_ProjPhase>
    {
    }

	public partial interface IQuestionDAL : IBaseDAL<MODEL.T_Question>
    {
    }

	public partial interface IQuestionOptionDAL : IBaseDAL<MODEL.T_QuestionOption>
    {
    }

	public partial interface IQuestionTypeDAL : IBaseDAL<MODEL.T_QuestionType>
    {
    }

	public partial interface IRoleDAL : IBaseDAL<MODEL.T_Role>
    {
    }

	public partial interface IRoleActDAL : IBaseDAL<MODEL.T_RoleAct>
    {
    }

	public partial interface IRolePermissionDAL : IBaseDAL<MODEL.T_RolePermission>
    {
    }

	public partial interface ITaskInformationDAL : IBaseDAL<MODEL.T_TaskInformation>
    {
    }

	public partial interface ITaskParticipationDAL : IBaseDAL<MODEL.T_TaskParticipation>
    {
    }

	public partial interface ITaskTypeDAL : IBaseDAL<MODEL.T_TaskType>
    {
    }

	public partial interface ITeacherInfoDAL : IBaseDAL<MODEL.T_TeacherInfo>
    {
    }

	public partial interface ITechnicaLevelDAL : IBaseDAL<MODEL.T_TechnicaLevel>
    {
    }

	public partial interface Il_MessageDAL : IBaseDAL<MODEL.Tbl_Message>
    {
    }

	public partial interface Il_User_MessageDAL : IBaseDAL<MODEL.Tbl_User_Message>
    {
    }


}