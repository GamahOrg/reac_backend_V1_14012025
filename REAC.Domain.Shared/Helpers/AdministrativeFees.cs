using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REAC.Domain.Shared.Helpers
{
    public class AdministrativeFees // الرسوم الادارية
    {
        public static int GetFees(int LitigationRequestAmount) // مبلغ المنازعه
        {
            int administrativeFees = 0;
          
            switch (LitigationRequestAmount)
            {
                case int n when (n >= 0 && n <= 10000):   //لو الدعوة لو مش علي مبلغ مالي يبقي دا هيدفع دي اقل رسوم
                    administrativeFees = 400;
                    break;

                case int n when (n >= 10001 && n <= 20000):
                    administrativeFees = 600;
                    break;

                //case int n when (n >= 20001 && n <= 30000):
                //    administrativeFees = 500;
                //    break;

                //case int n when (n >= 30001 && n <= 50000):
                //    administrativeFees = 600;
                //    break;

                //case int n when (n >= 50001 && n <= 100000):
                //    administrativeFees = 2000;
                //    break;

                //case int n when (n >= 100001 && n <= 200000):
                //    administrativeFees = 2500;
                //    break;

            }

            return administrativeFees;
        }
    }
}