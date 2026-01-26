using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.ViewModels
{
    public class DistrictSeatLimitVM
    {
        public int ID { get; set; }
        public long AdmissionUnitId { get; set; }
        public string AdmissionUnitName { get; set; }
        public int DistrictId { get; set; }        
        public string DistrictName { get; set; }
        public int SeatLimit { get; set; }
        public int SeatFillup{ get; set; }
    }
}
