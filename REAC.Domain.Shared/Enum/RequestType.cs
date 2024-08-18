namespace REAC.Domain.Shared.Enum
{
    public enum RequestType
    {
        ReformerTaskrequests = 1,
        ReformationSession = 2,
        ReformationReport = 3,
        ExtendReconciliationPeriodRequest = 4,
        StopFreeUpReformer = 5,

        ArbitratorTaskrequests = 6,
        ArbitrationSession = 7,
        ArbitrationReport = 8,
        ExtendArbitrationPeriodRequest = 9,
        StopFreeUpArbitrator = 10,

        LitigationRequest = 11,

        JoinRequestAsReformerRequest = 12,
        JoinRequestAsArbitratorRequest = 13,
    }
}
