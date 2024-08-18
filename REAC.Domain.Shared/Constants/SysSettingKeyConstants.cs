namespace REAC.Domain.Shared.Constants
{
    public class SysSettingKeyConstants
    {
        public const string SecurityApiUrl = "http://localhost:7144/"; // http://localhost:7144/

        #region Integration Keys

        public const string MC_Key = "MC";
        public const string MOJ_Key = "MOJ";
        public const string GEE_Key = "GEE";
        public const string Nafath_Key = "Nafath";
        public const string NafathCheck_Key = "NafathCheck";

        #endregion

        #region Login GEE Integration keys

        public const string GEE_LoginURL = "GEE_LoginURL";
        public const string GEE_grant_type = "GEE_grant_type";
        public const string GEE_client_secret = "GEE_client_secret";
        public const string GEE_client_id = "GEE_client_id";

        #endregion

        #region MOJ Integration keys

        public const string MOJ_URL = "MOJ_URL";
        public const int MOJControlId = 82007;

        #endregion

        #region MC Integration keys

        public const string MC_GetCRInformationByCRNumber_URL = "MC_GetCRInformationByCRNumber_URL";
        public const string MC_GetCRInformationByCROwners_URL = "MC_GetCRInformationByCROwners_URL";

        #endregion

        #region SMS Integration keys

        public const string Disable_SMS_Integration = "Disable_SMS_Integration";
        public const string SMS_URL = "SMS_URL";
        public const string SMS_Notification_Type = "SMS_Notification_Type";
        public const string Litigation_Request_Max_Session_ReScheduleTime = "Litigation_Request_Max_Session_ReScheduleTime";


        #endregion

        #region Nafath Integration keys

        public const string Nafath_URL = "Nafath_URL";
        public const string Nafath_Action = "Nafath_Action";
        public const string Nafath_Service = "Nafath_Service";

        #endregion

        #region Nafath Check keys

        public const string NafathCheck_Action = "NafathCheck_Action";
        public const string NafathCheck_URL = "NafathCheck_URL";

        #endregion

        #region Get User Info

        public const string NafathGetUserInfo_URL = "NafathGetUserInfo_URL";

        #endregion

        #region Nafath Response Status keys

        public const string NafathStatus_COMPLETED = "COMPLETED";
        public const string NafathStatus_WAITING = "WAITING";
        public const string NafathStatus_REJECTED = "REJECTED";
        public const string NafathStatus_EXPIRED = "EXPIRED";

        #endregion







        public const string Reconciliation_Period = "Reconciliation.Period";
        public const string Reconciliation_Request_Study_Period = "Reconciliation.Request.Study.Period";
        public const string Reconciliation_Request_Process_Notes_Period = "Reconciliation.Request.Process.Notes.Period";
        public const string Reconciliation_Reformer_Approve_Mission_Period = "Reconciliation.Reformer.Approve.Mission.Period";
        public const string Reconciliation_LitigationParties_Response_Period = "Reconciliation.LitigationParties.Response.Period";
        public const string Reconciliation_Report_Creation = "Reconciliation.Report.Creation";


        #region systemNotificationMessages


        public const string Citizen_AddBankAccount = "Citizen.AddBankAccount";
        public const string Citizen_EditBankAccount ="Citizen.EditBankAccount";
        public const string Reformer_JoiningRequest ="Reformer.JoiningRequest";
        public const string Arbitrator_JoiningRequest ="Arbitrator.JoiningRequest";
        public const string Reformer_AcceptJoiningRequest ="Reformer.AcceptJoiningRequest";
        public const string Arbitrator_AcceptJoiningRequest ="Arbitrator.AcceptJoiningRequest";
        public const string Arbitrator_JoiningRequest_Updated = "Arbitrator.JoiningRequest.Updated";
        public const string Reformer_JoiningRequest_Updated = "Reformer.JoiningRequest.Updated";
        public const string Arbitrator_UpdateJoiningRequest_WithFees = "Arbitrator.UpdateJoiningRequest.WithFees";
        public const string Reformer_UpdateJoiningRequest_WithFees = "Reformer.UpdateJoiningRequest.WithFees";
        public const string Arbitrator_UpdateJoiningRequest = "Arbitrator.UpdateJoiningRequest.NoFees";
        public const string Reformer_UpdateJoiningRequest = "Reformer.UpdateJoiningRequest.NoFees";
        public const string Reformer_AcceptJoiningRequest_NoFees ="Reformer.AcceptJoiningRequest.NoFees";
        public const string Arbitrator_AcceptJoiningRequest_NoFees ="Arbitrator.AcceptJoiningRequest.NoFees";
        public const string Reformer_AcceptJoiningRequest_WaitingForFees ="Reformer.AcceptJoiningRequest.WaitingForFees";
        public const string Arbitrator_AcceptJoiningRequest_WaitingForFees ="Arbitrator.AcceptJoiningRequest.WaitingForFees";
        public const string Reformer_RejectJoiningRequest ="Reformer.RejectJoiningRequest";
        public const string Arbitrator_RejectJoiningRequest ="Arbitrator.RejectJoiningRequest";
        public const string Reformer_Activate ="Reformer.Activate";
        public const string Arbitrator_Activate ="Arbitrator.Activate";
        public const string Reformer_DeActivate ="Reformer.DeActivate";
        public const string Arbitrator_DeActivate ="Arbitrator.DeActivate";
        public const string Arbitration_Arbitraor_Assigned ="Arbitration.Arbitraor.Assigned";
        public const string Arbitration_ArbitraorChairman_Assigned ="Arbitration.ArbitraorChairman.Assigned";
        public const string Arbitration_ArbitraorAssigned_SendToChairman ="Arbitration.ArbitraorAssigned.SendToChairman";
        public const string Reconciliation_Reconciliator_AcceptTask ="Reconciliation.Reconciliator.AcceptTask";
        public const string Arbitration_Arbitraor_AcceptTask ="Arbitration.Arbitraor.AcceptTask";
        public const string Reconciliation_Reconciliator_RejectTask = "Reconciliation.Reconciliator.RejectTask";
        public const string Arbitration_Arbitraor_RejectTask ="Arbitration.Arbitraor.RejectTask";
        public const string Reconciliation_ReSendTask ="Reconciliation.ReSendTask";
        public const string ArbitraorRecusal_AddRequest ="ArbitraorRecusal.AddRequest";
        public const string ArbitraorRecusal_AddRequest_Approve ="ArbitraorRecusal.AddRequest.Approve";
        public const string ArbitraorRecusal_AddRequest_Reject ="ArbitraorRecusal.AddRequest.Reject";
        public const string ArbitraorRecusal_InformArbitrator_Reject ="ArbitraorRecusal.InformArbitrator.Reject";
        public const string ArbitraorRecusal_InformArbitratorChairman_Rejected ="ArbitraorRecusal.InformArbitratorChairman.Rejected";
        public const string ArbitratorsChairmanNominationList_Added ="ArbitratorsChairmanNominationList.Added";
        public const string ArbitratorsNominationList_Added_FirstParty ="ArbitratorsNominationList.Added.FirstParty";
        public const string ArbitratorsNominationList_Added_SecondParty ="ArbitratorsNominationList.Added.SecondParty";
        public const string ArbitratorsNominationList_AssignedAndSent ="ArbitratorsNominationList.AssignedAndSent";
        public const string ArbitratorsChairmanNominationListReply_Added ="ArbitratorsChairmanNominationListReply.Added";
        public const string Arbitration_ExplanationReport_Added ="Arbitration.ExplanationReport.Added";
        public const string Arbitration_ExplanationReport_RepliedOn ="Arbitration.ExplanationReport.RepliedOn";
        public const string Arbitration_ExtendRequest_Added ="Arbitration.ExtendRequest.Added";
        public const string Arbitration_ExtendRequest_Accepted ="Arbitration.ExtendRequest.Accepted";
        public const string Arbitration_ExtendRequest_Rejected ="Arbitration.ExtendRequest.Rejected";
        public const string Arbitrators_Count_Added ="Arbitrators.Count.Added";
        public const string Arbitrators_Count_Added_AfterAgreement ="Arbitrators.Count.Added.AfterAgreement";
        public const string Arbitrators_Count_FromBothSides ="Arbitrators.Count.FromBothSides";
        public const string ArbitratorSelectionDuration_Added ="ArbitratorSelectionDuration.Added";
        public const string ArbitrationReport_Added_FirstParty ="ArbitrationReport.Added.FirstParty";
        public const string ArbitrationReport_Added_SecondParty ="ArbitrationReport.Added.SecondParty";
        public const string ArbitrationSession_Added_FirstParty ="ArbitrationSession.Added.FirstParty";
        public const string ArbitrationSession_Added_SecondParty ="ArbitrationSession.Added.SecondParty";
        public const string ExcuteWorkflow = "ExcuteWorkflow";
        public const string ProcessWorkflow_Accepted ="ProcessWorkflow.Accepted";
        public const string ProcessWorkflow_Reject ="ProcessWorkflow.Reject";
        public const string ProcessWorkflow_SentBackForEdit ="ProcessWorkflow.SentBackForEdit";
        public const string ProcessWorkflow_Edited ="ProcessWorkflow.Edited";
        public const string Reconciliation_Cancelled ="Reconciliation.Cancelled";
        public const string Reconciliation_ReAssigned ="Reconciliation.ReAssigned";
        public const string LitigationRequest_Decision_WaiverOfRight ="LitigationRequest.Decision.WaiverOfRight";
        public const string LitigationRequest_Decision_ReconciliationWasMade ="LitigationRequest.Decision.ReconciliationWasMade";
        public const string LitigationRequest_Decision_WaiverOfTheLitigation ="LitigationRequest.Decision.WaiverOfTheLitigation";
        public const string LitigationRequest_Decision_ReconciliationWasNotMadeAndMoveToArbitration ="LitigationRequest.Decision.ReconciliationWasNotMadeAndMoveToArbitration";
        public const string LitigationRequest_Decision_ReconciliationWasNotMade ="LitigationRequest.Decision.ReconciliationWasNotMade";
        public const string Reconciliation_ExtendRequest_Added ="Reconciliation.ExtendRequest.Added";
        public const string Reconciliation_ExtendRequest_Accepted ="Reconciliation.ExtendRequest.Accepted";
        public const string Reconciliation_ExtendRequest_Rejected ="Reconciliation.ExtendRequest.Rejected";
        public const string ReconciliationSession_set ="ReconciliationSession.set";
        public const string ReconciliationSession_Reset ="ReconciliationSession.Reset";

        #endregion


    }
}
