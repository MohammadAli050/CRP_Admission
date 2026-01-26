using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.ViewModels.EkPayModel.IPNModels
{
    public class pi_det_info
    {
        public string pay_timestamp { get; set; }
        public string pi_name { get; set; }
        public string pi_type { get; set; }
        public string card_holder_name { get; set; }
        public string pi_number { get; set; }
        public string pi_gateway { get; set; }
    }
}
