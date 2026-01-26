using DATAMANAGER;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonUtility
{
    public static class Authorize
    {
        public static bool AuthenticateSuperAdmin(long userId)
        {
            DAL.SystemUser su = null;
            using (var db = new OfficeDataManager())
            {
                su = db.GetSystemUserSuperAdminByID(userId, true);
            }

            if(su != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool AuthenticateOfficeUser(long userId)
        {
            //to do: to be implemented
            return false;
        }

    }
}
