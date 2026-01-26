using DAL_Log;
using DATAMANAGER;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonUtility
{
    public static class LogWriter
    {
        public static void AccessLogWriter(DAL_Log.AccessLog aLog)
        {
            try
            {
                using (var dbAccessLogInsert = new LogDataManager())
                {
                    dbAccessLogInsert.Insert<DAL_Log.AccessLog>(aLog);
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        public static void DataLogWriter(DAL_Log.DataLog dLog)
        {
            try
            {
                using (var dbDataLogInsert = new LogDataManager())
                {
                    dbDataLogInsert.Insert<DAL_Log.DataLog>(dLog);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        public static void SmsLog(DAL_Log.SmsLog sLog)
        {
            try
            {
                using (var dbSmsLogInsert = new LogDataManager())
                {
                    dbSmsLogInsert.Insert<DAL_Log.SmsLog>(sLog);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        public static void EmailLog(DAL_Log.EmailLog eLog)
        {
            try
            {
                using (var dbEmailLogInsert = new LogDataManager())
                {
                    dbEmailLogInsert.Insert<DAL_Log.EmailLog>(eLog);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }


        public static void CandidateByPassLog(DAL_Log.CandidateByPassLog cbpLog)
        {
            try
            {
                using (var dbEmailLogInsert = new LogDataManager())
                {
                    dbEmailLogInsert.Insert<DAL_Log.CandidateByPassLog>(cbpLog);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }


    }
}
