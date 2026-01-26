using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.ViewModels.EkPayModel
{
    public class SuccessResponse
    {
        public string secure_token { get; set; }
        public string token_exp_time { get; set; }
        public string msg_code { get; set; }
        public string msg_det { get; set; }
        public string ack_timestamp { get; set; }
    }
}
