using REAC.Domain.Shared.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REAC.Domain.Shared.Helpers
{
    public class RequestCode   // رقم الدعوي
    {
        public static string GenerateRequestCode(int litigationRequestContractTypeId, int litigationCount) 
        {
            string code = "";
            switch (litigationRequestContractTypeId)
            {
                case (int)LitigationRequestContractType.Rent:
                    code = "R0000" + (litigationCount + 1) + " - " + DateTime.Now.Year;
                    break;

                case (int)LitigationRequestContractType.Properties:
                    code = "O0000 " + (litigationCount + 1) + " - " + DateTime.Now.Year;
                    break;

                case (int)LitigationRequestContractType.Wafi:
                    code = "W0000 " + (litigationCount + 1) + " - " + DateTime.Now.Year;
                    break;

                case (int)LitigationRequestContractType.RealEstateBrokerage:
                    code = "B0000 " + (litigationCount + 1) + " - " + DateTime.Now.Year;
                    break;

                case (int)LitigationRequestContractType.OtherServices:
                    code = "N0000 " + (litigationCount + 1) + " - " + DateTime.Now.Year;
                    break;
            }

            return code;
        }
    }
}
