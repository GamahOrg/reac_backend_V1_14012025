namespace REAC.Domain.Shared.Enum;
public enum RequestStatus
{
    ///<summary>جديدة</summary>
    New = 1,
    ///<summary>مُستلمة</summary>
    Received = 2,
    ///<summary>تم تحديد ميعاد جلسة</summary>
    AsessionTimeHasBeenSet = 3,
    ///<summary>تم إعادة تحديد ميعاد جلسة</summary>
    AsessionTimeHasBeenReset = 4,
    ///<summary>تمت الجلسة</summary>
    TheSessionWasCompleted = 5,
    ///<summary>تم كتابة محضر الجلسة</summary>
    TheMinutesOfTheMeetingWereWritten = 6,
    ///<summary>تنازل عن الحق</summary>
    WaiverOfRight = 7,
    ///<summary>تنازل عن الدعوى</summary>
    WaiverOfTheLitigation = 8,
    ///<summary>عدم الصُلح</summary>
    ReconciliationWasNotMade = 9,
    ///<summary>حفظ وإغلاق</summary>
    SaveAndClose = 10,
    ///<summary> حفظ كمسودة</summary>
    SaveAsDraft = 11,
    ///<summary>مرفوضة</summary>
    Rejected = 12,
    ///<summary>معاده للتعديل</summary>
    SentBackForEdit = 13,
    ///<summary>تم التعديل</summary>
    Edited = 14,
    ///<summary>قيد المشارطة</summary>
    AgreementRequestHasBeenSent = 15,
    ///<summary>تم قبول محضر الصلح</summary>
    AcceptanceOfTheReconciliationReport = 16,
    ///<summary>تم رفض محضر الصلح</summary>
    RejectionOfTheReconciliationReport = 17,
    ///<summary>تم الصلح</summary>
    ReconciliationWasMade = 18,
    ///<summary>عدم الصُلح والانتقال للتحكيم</summary>
    ReconciliationWasNotMadeAndMoveToArbitration = 19,
    ///<summary>تم التحكيم</summary>
    JudgmentDecisionIssued = 20,
    ///<summary>بإنتظار التفسير</summary>
    WaitingForExplanation = 21,
    ///<summary>تم الرد</summary>
    ExplanationResponsed = 22,
    ///<summary>رفض طلب المشارطة</summary>
    AgreementRequestHasBeenRejected = 23,
    ///<summary>ملغاة</summary>
    Cancelled = 24,
}

