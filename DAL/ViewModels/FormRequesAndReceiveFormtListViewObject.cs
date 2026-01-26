using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.ViewModels
{
    public class FormRequesAndReceiveFormtListViewObject
    {
        public long CandidateID { get; set; }
        public string Name { get; set; }
        //public Nullable<long> CandidateFormSerialID { get; set; }
        //public Nullable<long> FormSerial { get; set; }
        public Nullable<long> CandidatePaymentID { get; set; }
        public Nullable<long> PaymentId { get; set; }
        public string Mobile { get; set; }
        //public Nullable<long> AdmissionSetupID { get; set; }
        //public Nullable<long> AdmissionUnitID { get; set; }
        //public string UNIT { get; set; }
        public Nullable<System.DateTime> DateApplied { get; set; }
        public string IsPaid { get; set; }
        public Nullable<int> AcaCalId { get; set; }
        public string Email { get; set; }
    }
}
