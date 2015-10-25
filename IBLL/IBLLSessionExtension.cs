
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IBLL
{
	public partial interface IBLLSession
    {
		IsdiagramBLL IsdiagramBLL{get;set;}
		IstemMessageBLL IstemMessageBLL{get;set;}
		IAbnormalBLL IAbnormalBLL{get;set;}
		IAnswerSheetBLL IAnswerSheetBLL{get;set;}
		IAnswerSheetCommentBLL IAnswerSheetCommentBLL{get;set;}
		IBriefAnswerSheetBLL IBriefAnswerSheetBLL{get;set;}
		IBriefScoreBLL IBriefScoreBLL{get;set;}
		IChoiceAnswerSheetBLL IChoiceAnswerSheetBLL{get;set;}
		IDepartmentBLL IDepartmentBLL{get;set;}
		IInterviewerInfoBLL IInterviewerInfoBLL{get;set;}
		IIsShowBLL IIsShowBLL{get;set;}
		IMemberInformationBLL IMemberInformationBLL{get;set;}
		IOgnizationActBLL IOgnizationActBLL{get;set;}
		IOnDutyBLL IOnDutyBLL{get;set;}
		IOrganizationBLL IOrganizationBLL{get;set;}
		IPaperBLL IPaperBLL{get;set;}
		IPaperQuestionBLL IPaperQuestionBLL{get;set;}
		IPermissionBLL IPermissionBLL{get;set;}
		IProjectInformationBLL IProjectInformationBLL{get;set;}
		IProjectParticipationBLL IProjectParticipationBLL{get;set;}
		IProjectTypeBLL IProjectTypeBLL{get;set;}
		IProjPhaseBLL IProjPhaseBLL{get;set;}
		IQuestionBLL IQuestionBLL{get;set;}
		IQuestionOptionBLL IQuestionOptionBLL{get;set;}
		IQuestionTypeBLL IQuestionTypeBLL{get;set;}
		IRoleBLL IRoleBLL{get;set;}
		IRoleActBLL IRoleActBLL{get;set;}
		IRolePermissionBLL IRolePermissionBLL{get;set;}
		ITaskInformationBLL ITaskInformationBLL{get;set;}
		ITaskParticipationBLL ITaskParticipationBLL{get;set;}
		ITaskTypeBLL ITaskTypeBLL{get;set;}
		ITeacherInfoBLL ITeacherInfoBLL{get;set;}
		ITechnicaLevelBLL ITechnicaLevelBLL{get;set;}
		Il_MessageBLL Il_MessageBLL{get;set;}
		Il_User_MessageBLL Il_User_MessageBLL{get;set;}
    }

}