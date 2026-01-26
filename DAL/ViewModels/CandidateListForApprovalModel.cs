using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.ViewModels
{
    public class CandidateListForApprovalModel
    {
        //public long cFormID { get; set; }
        public long FormSerial { get; set; }
        //public long cPaymentID { get; set; }
        public Nullable<long> PaymentId { get; set; }
        //public Nullable<bool> IsPaid { get; set; }
        public long candidateID { get; set; }
        public string candidateName { get; set; }
        public string candidateSmsPhone { get; set; }
        //public string candidatePhoto { get; set; }
        //public string candidateSign { get; set; }
        public string quota { get; set; }
        public long admSetupAdmUnitID { get; set; }
        public long admSetupID { get; set; }
        public int admSetupAcaCalId { get; set; }
        //public Nullable<long> sscExamID { get; set; }
        //public Nullable<long> sscExamDetailsID { get; set; }
        //public Nullable<int> sscEduBoardID { get; set; }
        public string sscEduBoard { get; set; }
        //public string sscInstitute { get; set; }
        //public string sscRollNo { get; set; }
        //public Nullable<int> sscGroupOrSubjectID { get; set; }
        public string sscGroupOrSub { get; set; }
        public string sscResult { get; set; }
        //public Nullable<decimal> sscResultGpaW4s { get; set; }
        //public Nullable<decimal> sscMarks { get; set; }
        public Nullable<int> sscPassingYear { get; set; }
        //public Nullable<long> hscExamID { get; set; }
        //public Nullable<long> hscExamDetailsID { get; set; }
        //public Nullable<int> hscEduBoardID { get; set; }
        public string hscEduBoard { get; set; }
        //public string hscInstitute { get; set; }
        //public string hscRollNo { get; set; }
        //public Nullable<int> hscGroupOrSubjectID { get; set; }
        public string hscGroupOrSub { get; set; }
        public string hscResult { get; set; }
        //public Nullable<decimal> hscResultGpaW4s { get; set; }
        //public Nullable<decimal> hscMarks { get; set; }
        public Nullable<int> hscPassingYear { get; set; }
        //public Nullable<long> undExamID { get; set; }
        //public Nullable<long> undExamDetailsID { get; set; }
        //public string undInstitute { get; set; }
        //public Nullable<int> undGroupOrSubjectID { get; set; }
        //public string undGroupOrSub { get; set; }
        //public string undResult { get; set; }
        //public string undProgram { get; set; }
        //public Nullable<int> undPassingYear { get; set; }
        //public Nullable<long> grdExamID { get; set; }
        //public Nullable<long> grdExamDetailsID { get; set; }
        //public string grdInstitute { get; set; }
        //public Nullable<int> grdGroupOrSubjectID { get; set; }
        //public string grdGroupOrSub { get; set; }
        //public string grdResult { get; set; }
        //public string grdProgram { get; set; }
        //public Nullable<int> grdPassingYear { get; set; }
        public bool IsApproved { get; set; }
        public bool IsFinalSubmit { get; set; }
    }
}
