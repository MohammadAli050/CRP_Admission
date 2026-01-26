using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.ViewModels
{
    public class ApproveCandidateGridViewObject
    {

        public long CandidateID { get; set; }
        public long PaymentID { get; set; }
        public long  FormSL { get; set; }
        public string PhotoUrl { get; set; }
        public string Name { get; set; }
        public string Mobile { get; set; }
        public decimal SSCGpa { get; set; }
        public string SSCBoard { get; set; }
        public string SSCGroup { get; set; }
        public string SSCYear { get; set; }
        public decimal HSCGpa { get; set; }
        public string HSCBoard { get; set; }
        public string HSCGroup { get; set; }
        public string HSCYear { get; set; }

        public decimal SscPlusHsc { get; set; }

        public decimal UndgrdGpa { get; set; }
        public string UndgrdProgram { get; set; }
        public string UndgrdInstitute { get; set; }
        public string UndgrdGroup { get; set; }
        public string UndgrdYear { get; set; }
        public decimal GradGpa { get; set; }
        public string GradProgram { get; set; }
        public string GradInstitute { get; set; }
        public string GradGroup { get; set; }
        public string GradYear { get; set; }

        public string Quota { get; set; }
        

        public int ProgramID { get; set; }
        public int AcaCalID { get; set; }
    }
}
