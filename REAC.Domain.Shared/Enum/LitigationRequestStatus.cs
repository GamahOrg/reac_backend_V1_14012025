namespace REAC.Domain.Shared.Enum;
public enum LitigationRequestStatus
{
    ///<summary>جديدة</summary>
    New = 1,
    ///<summary>مُستلمة</summary>
    Received = 2,
    ///<summary>تم تحديد ميعاد جلسة</summary>
    AsessionTimeHasBeenSet = 3,
    ///<summary>تمت الجلسة</summary>
    TheSessionWasCompleted = 4,
    ///<summary>تم كتابة محضر الجلسة</summary>
    TheMinutesOfTheMeetingWereWritten = 5,
    ///<summary>تنازل عن الحق</summary>
    WaiverOfRight = 6,
    ///<summary>تنازل عن الدعوى</summary>
    WaiverOfTheLawsuit = 7,
    ///<summary>عدم الصُلح</summary>
    LackOfReconciliation = 8,
    ///<summary>حفظ وإغلاق</summary>
    SaveAndClose = 9,
    ///<summary> حفظ كمسودة</summary>
    SaveAsDraft = 10,
    ///<summary>مرفوضة</summary>
    Rejected = 11,
    ///<summary>معاده للتعديل</summary>
    SentBackForEdit = 12,
    ///<summary>تم التعديل</summary>
    Edited = 13,
    ///<summary>قيد المشارطة</summary>
    CharterRequestSent = 14


}

