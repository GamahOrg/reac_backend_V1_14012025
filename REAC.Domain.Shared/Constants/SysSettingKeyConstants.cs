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



    }
}
