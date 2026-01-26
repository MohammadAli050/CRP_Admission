using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.ViewModels
{
    public class SeatPlanDeleteProgramRoomPriorityObject
    {
        public long ID { get; set; }
        public string AdmUnitName { get; set; }
        public long AdmUnitID { get; set; }
        public int AcaCalID { get; set; }
        public string StartRoll { get; set; }
        public string EndRoll { get; set; }
        public string BuildingName { get; set; }
        public string RoomName { get; set; }
        public int Priority { get; set; }
    }
}
