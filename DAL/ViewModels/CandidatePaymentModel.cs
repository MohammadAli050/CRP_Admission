using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.ViewModels
{
    public class CandidatePaymentModel
    {
        public Nullable<long> PaymentId { get; set; }
        public string StudentName { get; set; }
        public string Session { get; set; }
        public Nullable<System.DateTime> FormAppliedDate { get; set; }
        public bool IsPaid { get; set; }
        public string PaymentUpdateStatus { get; set; }
    }
}
