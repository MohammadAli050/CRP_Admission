using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.ViewModels.EkPayModel
{
    public class ek_pay
    {
        public virtual mer_info mer_info { get; set; }

        public string req_timestamp { get; set; }

        public virtual feed_uri feed_uri { get; set; }

        public virtual cust_info cust_info { get; set; }

        public virtual trns_info trns_info { get; set; }

        public virtual ipn_info ipn_info { get; set; }

        public string mac_addr { get; set; }
    }
}
