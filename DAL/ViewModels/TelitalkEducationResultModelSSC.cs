using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.ViewModels
{
    public class TelitalkEducationResultModelSSC
    {
        public int responseCode { get; set; }
        public string responseDesc { get; set; }
        public string board { get; set; }
        public string rollNo { get; set; }
        public string passYear { get; set; }
        public string name { get; set; }
        public string father { get; set; }
        public string mother { get; set; }
        public string regNo { get; set; }
        public string gender { get; set; }
        public string result { get; set; }
        public string gpa { get; set; }
        public string gpaExc4th { get; set; }
        public string studGroup { get; set; }
        public string eiin { get; set; }
        public string dob { get; set; }
        public string totalObtMark { get; set; }
        public string totalExc4TH { get; set; }
        public string iName { get; set; }
        public string cCode { get; set; }
        public string cName { get; set; }
        public string thana { get; set; }
        public string sub4thCode { get; set; }
        public List<TelitalkEducationSubjectModel> subject { get; set; }
    }
}
