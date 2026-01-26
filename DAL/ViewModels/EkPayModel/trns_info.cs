using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.ViewModels.EkPayModel
{
    public class trns_info
    {
        public string trnx_id { get; set; }
        public string trnx_amt { get; set; }
        public string trnx_currency { get; set; }
        public string ord_id { get; set; }
        public string ord_det { get; set; }
    }
}
