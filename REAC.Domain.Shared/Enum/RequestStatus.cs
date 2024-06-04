using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REAC.Domain.Shared.Enum
{
    public enum RequestStatus
    {
        RequestHasBeenCreated = 1,
        RequestHasBeenCreatedAndIsBeingStudied = 2,
        RequestIsUnderStudy = 3,
        RequestHasBeenAccepted = 4,
        RequestHasBeenRejected = 5,
        FeedbackHasBeenSentToTheClient = 6,
        FeedbackHasBeenProcessedAndTheRequestHasBeenSent = 7
    }
}
