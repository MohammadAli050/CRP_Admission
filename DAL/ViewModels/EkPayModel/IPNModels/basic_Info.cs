using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.ViewModels.EkPayModel.IPNModels
{
    public class basic_Info
    {
        public string mer_reg_id { get; set; }
        public string ipn_info { get; set; }
        public string redirect_to { get; set; }
        public string dgtl_sign { get; set; }
        public string ord_desc { get; set; }
        public string remarks { get; set; }
    }
}
