using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.ViewModels.EkPayModel.IPNModels
{
    public class EkPayIPNModel
    {
        public string secure_token { get; set; }
        public string msg_code { get; set; }
        public string msg_det { get; set; }
        public string req_timestamp { get; set; }

        public virtual basic_Info basic_Info { get; set;}
        public virtual cust_info cust_info { get; set; }
        public virtual trnx_info trnx_info { get; set; }
        public virtual pi_det_info pi_det_info { get; set; }
    }
}
