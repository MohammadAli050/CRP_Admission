using DATAMANAGER;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonUtility
{
    public class CommonLogic
    {
        public static string AdmissionLoginId()
        {
            string loginId = string.Empty;

            loginId = UtilityManager.LoginId("C-", 6, "");

            using(var db = new CandidateDataManager())
            {
                DAL.CandidateUser candidateUser = db.GetCandidateUserByUsername_ND(loginId);
                if (candidateUser != null)
                {
                    loginId = AdmissionLoginId();
                }
            }

            return loginId;
        }

        public static string AdmissionPassword()
        {
            string password = string.Empty;

            password = UtilityManager.UserPassword("", 7, "");

            return password;
        }
        
    }
}
