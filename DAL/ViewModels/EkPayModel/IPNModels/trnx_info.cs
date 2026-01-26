using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.ViewModels.EkPayModel.IPNModels
{
    public class trnx_info
    {
        public string trnx_amt { get; set; }
        public string trnx_id { get; set; }
        public string mer_trnx_id { get; set; }
        public string curr { get; set; }
        public string pi_trnx_id { get; set; }
        public string pi_charge { get; set; }
        public string ekpay_charge { get; set; }
        public string pi_discount { get; set; }
        public string total_ser_chrg { get; set; }
        public string discount { get; set; }
        public string promo_discount { get; set; }
        public string total_pabl_amt { get; set; }
    }
}
