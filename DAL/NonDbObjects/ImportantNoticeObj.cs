using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.NonDbObjects
{
    public class ImportantNoticeObj
    {
        public string Title { get; set; }
        public DateTime NoticeDate { get; set; }
        public string Details { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public long CreatedBy { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
