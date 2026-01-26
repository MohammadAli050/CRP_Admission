using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.ViewModels.EkPayModel
{
    public class ResponseEkPay
    {
        public int ResponseCode { get; set; }
        public string ResponseStatus { get; set; }
        public string ResponseMessage { get; set; }
        public object ResponseData { get; set; }
    }
}
