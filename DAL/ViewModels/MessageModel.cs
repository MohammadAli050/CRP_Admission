using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.ViewModels
{
    public class MessageModel
    {
        //Code:
        // 1 = Success
        // 2 = Fail

        public int MessageCode { get; set; }
        public string MessageBody { get; set; }
        public bool MessageBoolean { get; set; }
    }
}
