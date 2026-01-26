using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.ViewModels
{
    public class BuildingVM
    {
        public int BuildingID { get; set; }
        public int CampusID { get; set; }

        public int BuildingPriority { get; set; }
        public string BuildingWithCampusName { get; set; }
    }
}
