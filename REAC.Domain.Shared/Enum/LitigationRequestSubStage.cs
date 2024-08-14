namespace REAC.Domain.Shared.Enum
{
    public enum LitigationRequestSubStage
    {
        AddSession = 0,
        LitigationRequestReportAdded = 1,
        LitigationRequestPartiesDecisionAdded = 2,
        ArbitratorSelectionPeriod = 3,
        SetArbitratorsNumber = 4,
        PrepareArbitratorsListAndSendTheNominationRequest = 5,
        SetReformerFirstTime = 6,
        LitigationRequestReformerDecisionAdded = 7,


        ArbitratorSelectionDuration = 8,
        LitigationRequestAddArbitratorCount = 9,

        LitigationRequestAddArbitratorListNomination = 10,
        LitigationRequestAddArbitratorListNominationDone = 11,

        LitigationRequestAddArbitratorChairmanListNomination = 12,
        LitigationRequestAddArbitratorChairmanListNominationDone = 13,

        LitigationRequestAddArbitrationSession = 14,

        JudgmentDecisionIssuing = 15,
        JudgmentDecisionIssued = 16,



    }
}
