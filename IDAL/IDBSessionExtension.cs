
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IDAL
{
	public partial interface IDBSession
    {
		IsdiagramDAL IsdiagramDAL{get;set;}
		IstemMessageDAL IstemMessageDAL{get;set;}
		IAbnormalDAL IAbnormalDAL{get;set;}
		IAnswerSheetDAL IAnswerSheetDAL{get;set;}
		IAnswerSheetCommentDAL IAnswerSheetCommentDAL{get;set;}
		IBriefAnswerSheetDAL IBriefAnswerSheetDAL{get;set;}
		IBriefScoreDAL IBriefScoreDAL{get;set;}
		IChoiceAnswerSheetDAL IChoiceAnswerSheetDAL{get;set;}
		IDepartmentDAL IDepartmentDAL{get;set;}
		IInterviewerInfoDAL IInterviewerInfoDAL{get;set;}
		IIsShowDAL IIsShowDAL{get;set;}
		IMemberInformationDAL IMemberInformationDAL{get;set;}
		IOgnizationActDAL IOgnizationActDAL{get;set;}
		IOnDutyDAL IOnDutyDAL{get;set;}
		IOrganizationDAL IOrganizationDAL{get;set;}
		IPaperDAL IPaperDAL{get;set;}
		IPaperQuestionDAL IPaperQuestionDAL{get;set;}
		IPermissionDAL IPermissionDAL{get;set;}
		IProjectInformationDAL IProjectInformationDAL{get;set;}
		IProjectParticipationDAL IProjectParticipationDAL{get;set;}
		IProjectTypeDAL IProjectTypeDAL{get;set;}
		IProjPhaseDAL IProjPhaseDAL{get;set;}
		IQuestionDAL IQuestionDAL{get;set;}
		IQuestionOptionDAL IQuestionOptionDAL{get;set;}
		IQuestionTypeDAL IQuestionTypeDAL{get;set;}
		IRoleDAL IRoleDAL{get;set;}
		IRoleActDAL IRoleActDAL{get;set;}
		IRolePermissionDAL IRolePermissionDAL{get;set;}
		ITaskInformationDAL ITaskInformationDAL{get;set;}
		ITaskParticipationDAL ITaskParticipationDAL{get;set;}
		ITaskTypeDAL ITaskTypeDAL{get;set;}
		ITeacherInfoDAL ITeacherInfoDAL{get;set;}
		ITechnicaLevelDAL ITechnicaLevelDAL{get;set;}
		Il_MessageDAL Il_MessageDAL{get;set;}
		Il_User_MessageDAL Il_User_MessageDAL{get;set;}
    }

}