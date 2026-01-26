using DAL;
using DAL.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATAMANAGER
{
    public class CandidateDataManager : GeneralDataManager, IDisposable
    {
        private bool disposed = false;

        public CandidateDataManager(Entities AdmissionDB) : base(AdmissionDB)
        {

        }

        public CandidateDataManager() : base()
        {

        }

        public void Update<T>(T obj) where T : class
        {
            AdmissionDB.Entry(obj).State = EntityState.Modified;
            AdmissionDB.SaveChanges();
        }

        public void Delete<T>(T obj) where T : class
        {
            AdmissionDB.Entry(obj).State = EntityState.Deleted;
            AdmissionDB.SaveChanges();
        }

        public void Insert<T>(T obj) where T : class
        {
            AdmissionDB.Entry<T>(obj).State = EntityState.Added;
            AdmissionDB.SaveChanges();
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                    AdmissionDB.Dispose();
                    //CommonDB.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~GeneralDataManager() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }

        #endregion

        //-------------------------------------------------------------------------------------------------------------
        // Notes:-
        // 1. Methods ending with _ND means that 'No Dependencies'. No eager loading is done.
        // 2. Methods ending with _AD means 'All Dependencies'. Eagerly loaded all data.
        // 3. Methods ending with _JOIN means a join operation is made instead of eager loading.
        //



        #region ADDRESS

        public List<DAL.Address> GetAllAddressByCandidateID_ND(long candidateId)
        {
            List<DAL.Address> list = AdmissionDB.Addresses
                .Where(a => a.CandidateID == candidateId)
                .ToList();
            if (list.Count() > 0)
                return list;
            else
                return null;
        }

        public List<DAL.Address> GetAllAddressByCandidateID_AD(long candidateId)
        {
            List<DAL.Address> list = AdmissionDB.Addresses
                .Include(gt => gt.AddressDetail)
                .Where(a => a.CandidateID == candidateId)
                .ToList();
            if (list.Count() > 0)
                return list;
            else
                return null;
        }

        public DAL.Address GetAddressByCandidateIDAddressTypeID_ND(long candidateId, int addressTypeId)
        {
            DAL.Address obj = AdmissionDB.Addresses
                .Where(c => c.CandidateID == candidateId && c.AddressTypeID == addressTypeId)
                .FirstOrDefault();
            if (obj != null)
                return obj;
            else
                return null;
        }

        public DAL.Address GetAddressByCandidateIDAddressTypeID_AD(long candidateId, int addressTypeId)
        {
            DAL.Address obj = AdmissionDB.Addresses
                .Include(gt => gt.AddressDetail)
                .Where(c => c.CandidateID == candidateId && c.AddressTypeID == addressTypeId)
                .FirstOrDefault();
            if (obj != null)
                return obj;
            else
                return null;
        }

        #endregion

        #region ADDRESS DETAILS

        public DAL.AddressDetail GetAddressDetailByID_ND(long id)
        {
            DAL.AddressDetail obj = AdmissionDB.AddressDetails.Find(id);
            if (obj != null)
                return obj;
            else
                return null;
        }

        #endregion

        #region BASIC INFO

        public DAL.BasicInfo GetCandidateBasicInfoByID_ND(long candidateId)
        {
            DAL.BasicInfo obj = AdmissionDB.BasicInfoes.Find(candidateId);
            if (obj != null)
                return obj;
            else
                return null;
        }

        public DAL.BasicInfo GetCandidateBasicInfoByID_AD(long candidateId)
        {
            DAL.BasicInfo obj = AdmissionDB.BasicInfoes
                .Include(a => a.Country)
                .Include(a => a.MaritalStatu)
                .Include(a => a.Religion)
                .Include(a => a.Quota)
                .Include(a => a.BloodGroup)
                .Include(a => a.Gender)
                .Include(a => a.Language)
                .Where(x => x.ID == candidateId)
                .FirstOrDefault();

            if (obj != null)
                return obj;
            else
                return null;
        }

        public DAL.BasicInfo GetCandidateBasicInfoByUserID_ND(long userID)
        {
            DAL.BasicInfo obj = AdmissionDB.BasicInfoes
                .Where(a => a.CandidateUserID == userID)
                .FirstOrDefault();
            return obj;
        }

        public long GetCandidateIdByUserID_ND(long userID)
        {
            long cId = -1;
            cId = AdmissionDB.BasicInfoes
                .Where(c => c.CandidateUserID == userID)
                .Select(c => c.ID)
                .FirstOrDefault();

            if (cId > 0)
            {
                return cId;
            }
            else
            {
                return -1;
            }
        }

        #endregion

        #region EXAM

        public List<DAL.Exam> GetAllExamByCandidateID_ND(long candidateId)
        {
            List<DAL.Exam> list = AdmissionDB.Exams
                .Where(a => a.CandidateID == candidateId)
                .ToList();
            return list;
        }

        public List<DAL.Exam> GetAllExamByCandidateID_AD(long candidateId)
        {
            List<DAL.Exam> list = AdmissionDB.Exams
                .Include(a => a.ExamDetail)
                .Where(a => a.CandidateID == candidateId)
                .ToList();
            if (list.Count() > 0)
            {
                return list;
            }
            else
            {
                return null;
            }
        }

        /*
         *ID	ExamTypeName	Code	EducationCategory_ID
            1	Secondary School Certificate	SSC	2
            2	Higher Secondary School Certificate	HSC	3
            3	Undergraduate	Bachelor's degree	4
            4	Graduate	Master's degree	6
            5	O Level	O Level	2
            6	Dakhil	Dakhil	2
            7	A Level	A Level	3
            8	Alim	Alim	3
            9	Diploma	Diploma	3
            10	Pass	Pass	5
            11	N/A	N/A	1 
         */

        public DAL.Exam GetSecondaryExamByCandidateID_AD(long candidateId)
        {
            DAL.Exam obj = AdmissionDB.Exams
                .Include(a => a.ExamDetail)
                .Where(a => a.CandidateID == candidateId && (a.ExamTypeID == 1 || a.ExamTypeID == 5 || a.ExamTypeID == 6 || a.ExamTypeID == 12 || a.ExamTypeID == 14))
                .FirstOrDefault();
            if (obj != null)
                return obj;
            else
                return null;
        }

        public DAL.Exam GetHigherSecdExamByCandidateID_AD(long candidateId)
        {
            DAL.Exam obj = AdmissionDB.Exams
                .Include(a => a.ExamDetail)
                .Where(a => a.CandidateID == candidateId && (a.ExamTypeID == 2 || a.ExamTypeID == 7 || a.ExamTypeID == 8 || a.ExamTypeID == 9 || a.ExamTypeID == 13 || a.ExamTypeID == 15))
                .FirstOrDefault();
            if (obj != null)
                return obj;
            else
                return null;
        }

        public DAL.Exam GetUndergradExamByCandidateID_AD(long candidateId)
        {
            DAL.Exam obj = AdmissionDB.Exams
                .Include(a => a.ExamDetail)
                .Where(a => a.CandidateID == candidateId && (a.ExamTypeID == 3 || a.ExamTypeID == 10))
                .FirstOrDefault();
            if (obj != null)
                return obj;
            else
                return null;
        }

        public DAL.Exam GetGradExamByCandidateID_AD(long candidateId)
        {
            DAL.Exam obj = AdmissionDB.Exams
                .Include(a => a.ExamDetail)
                .Where(a => a.CandidateID == candidateId && a.ExamTypeID == 4)
                .FirstOrDefault();
            if (obj != null)
                return obj;
            else
                return null;
        }



        //---------Certificate Exam-------------

        public DAL.CertificateExam GetCertificateSecondaryExamByCandidateID_AD(long candidateId)
        {
            DAL.CertificateExam obj = new DAL.CertificateExam();
            obj = AdmissionDB.CertificateExams.Where(a => a.CandidateID == candidateId && (a.ExamTypeID == 1 || a.ExamTypeID == 5 || a.ExamTypeID == 6 || a.ExamTypeID == 12)).FirstOrDefault();
            if (obj != null)
                return obj;
            else
                return null;
        }

        public DAL.CertificateExam GetCertificateHigherSecdExamByCandidateID_AD(long candidateId)
        {
            DAL.CertificateExam obj = new DAL.CertificateExam();
            obj = AdmissionDB.CertificateExams.Where(a => a.CandidateID == candidateId && (a.ExamTypeID == 2 || a.ExamTypeID == 7 || a.ExamTypeID == 8 || a.ExamTypeID == 9 || a.ExamTypeID == 13)).FirstOrDefault();
            if (obj != null)
                return obj;
            else
                return null;
        }

        public DAL.CertificateExam GetCertificateUndergradExamByCandidateID_AD(long candidateId)
        {
            DAL.CertificateExam obj = new DAL.CertificateExam();
            obj = AdmissionDB.CertificateExams.Where(a => a.CandidateID == candidateId && (a.ExamTypeID == 3 || a.ExamTypeID == 10)).FirstOrDefault();
            if (obj != null)
                return obj;
            else
                return null;
        }

        public DAL.CertificateExam GetCertificateGradExamByCandidateID_AD(long candidateId)
        {
            DAL.CertificateExam obj = new DAL.CertificateExam();
            obj = AdmissionDB.CertificateExams.Where(a => a.CandidateID == candidateId && a.ExamTypeID == 4).FirstOrDefault();
            if (obj != null)
                return obj;
            else
                return null;
        }

        //---------End Certificate Exam-------------


        //---------PostgraduateDiploma Exam-------------

        public DAL.PostgraduateDiplomaExam GetPostgraduateDiplomaSecondaryExamByCandidateID_AD(long candidateId)
        {
            DAL.PostgraduateDiplomaExam obj = new DAL.PostgraduateDiplomaExam();
            obj = AdmissionDB.PostgraduateDiplomaExams.Where(a => a.CandidateID == candidateId && (a.ExamTypeID == 1 || a.ExamTypeID == 5 || a.ExamTypeID == 6)).FirstOrDefault();
            if (obj != null)
                return obj;
            else
                return null;
        }

        public DAL.PostgraduateDiplomaExam GetPostgraduateDiplomaHigherSecdExamByCandidateID_AD(long candidateId)
        {
            DAL.PostgraduateDiplomaExam obj = new DAL.PostgraduateDiplomaExam();
            obj = AdmissionDB.PostgraduateDiplomaExams.Where(a => a.CandidateID == candidateId && (a.ExamTypeID == 2 || a.ExamTypeID == 7 || a.ExamTypeID == 8 || a.ExamTypeID == 9)).FirstOrDefault();
            if (obj != null)
                return obj;
            else
                return null;
        }

        public DAL.PostgraduateDiplomaExam GetPostgraduateDiplomaUndergradExamByCandidateID_AD(long candidateId)
        {
            DAL.PostgraduateDiplomaExam obj = new DAL.PostgraduateDiplomaExam();
            obj = AdmissionDB.PostgraduateDiplomaExams.Where(a => a.CandidateID == candidateId && (a.ExamTypeID == 3 || a.ExamTypeID == 10)).FirstOrDefault();
            if (obj != null)
                return obj;
            else
                return null;
        }

        public DAL.PostgraduateDiplomaExam GetPostgraduateDiplomaGradExamByCandidateID_AD(long candidateId)
        {
            DAL.PostgraduateDiplomaExam obj = new DAL.PostgraduateDiplomaExam();
            obj = AdmissionDB.PostgraduateDiplomaExams.Where(a => a.CandidateID == candidateId && a.ExamTypeID == 4).FirstOrDefault();
            if (obj != null)
                return obj;
            else
                return null;
        }

        //---------End PostgraduateDiploma Exam-------------



        #endregion

        #region EXAM DETAILS

        public DAL.ExamDetail GetExamDetailByID_ND(long id)
        {
            DAL.ExamDetail obj = AdmissionDB.ExamDetails.Find(id);
            return obj;
        }

        #endregion

        #region ADDITIONAL INFO

        public DAL.AdditionalInfo GetAdditionalInfoByCandidateID_ND(long candidateId)
        {
            DAL.AdditionalInfo obj = AdmissionDB.AdditionalInfoes
                .Where(a => a.CandidateID == candidateId)
                .FirstOrDefault();
            if (obj != null)
            {
                return obj;
            }
            else
            {
                return null;
            }
        }


        #endregion

        #region DOCUMENT

        public List<DAL.Document> GetAllDocumentByCandidateID_ND(long candidateID)
        {
            List<DAL.Document> list = AdmissionDB.Documents
                .Where(c => c.CandidateID == candidateID)
                .ToList();

            if (list.Count > 0)
            {
                return list;
            }
            else
            {
                return null;
            }
        }

        public DAL.Document GetDocumentByCandidateIDDocumentTypeID_ND(long candidateID, int documentTypeID)
        {
            DAL.Document obj = AdmissionDB.Documents
                .Where(c => c.CandidateID == candidateID && c.DocumentTypeID == documentTypeID)
                .FirstOrDefault();
            if (obj != null)
            {
                return obj;
            }
            else
            {
                return null;
            }
        }

        public List<DAL.Document> GetAllDocumentByCandidateID_AD(long candidateID)
        {
            List<DAL.Document> list = AdmissionDB.Documents
                .Include(c => c.DocumentDetail)
                .Where(c => c.CandidateID == candidateID)
                .ToList();

            if (list.Count > 0)
            {
                return list;
            }
            else
            {
                return null;
            }
        }

        public DAL.Document GetDocumentByCandidateIDDocumentTypeID_AD(long candidateID, int documentTypeID)
        {
            DAL.Document obj = AdmissionDB.Documents
                .Include(c => c.DocumentDetail)
                .Where(c => c.CandidateID == candidateID && c.DocumentTypeID == documentTypeID)
                .FirstOrDefault();

            if (obj != null)
            {
                return obj;
            }
            else
            {
                return null;
            }
        }

        #endregion

        #region CANDIDATE DOCUMENT

        public DAL.DocumentDetail GetDocumentDetailByID_ND(long ID)
        {
            DAL.DocumentDetail obj = AdmissionDB.DocumentDetails.Find(ID);
            if (obj != null)
            {
                return obj;
            }
            else
            {
                return null;
            }
        }

        #endregion

        #region CANDIDATE FORM SERIAL

        public DAL.CandidateFormSl GetCandidateFormSlByID_ND(long candidateFormSlID)
        {
            DAL.CandidateFormSl obj = AdmissionDB.CandidateFormSls.Find(candidateFormSlID);
            return obj;
        }

        public DAL.CandidateFormSl GetCandidateFormSlByCandID_AD(long candidateID)
        {
            DAL.CandidateFormSl obj = AdmissionDB.CandidateFormSls
                .Include(a => a.AdmissionSetup)
                .Include(a => a.AdmissionSetup.AdmissionUnit)
                .Include(a => a.AdmissionSetup.AdmissionUnit.AdmissionUnitPrograms)
                .Include(a => a.BasicInfo)
                .Include(a => a.CandidatePayment)
                .Where(x => x.CandidateID == candidateID)
                .FirstOrDefault();
            if (obj != null)
                return obj;
            else
                return null;
        }

        public List<DAL.CandidateFormSl> GetAllCandidateFormSlByCandID_AD(long candidateID)
        {
            List<DAL.CandidateFormSl> list = AdmissionDB.CandidateFormSls
                .Include(a => a.AdmissionSetup)
                .Include(a => a.AdmissionSetup.AdmissionUnit)
                .Include(a => a.AdmissionSetup.AdmissionUnit.AdmissionUnitPrograms)
                .Include(a => a.BasicInfo)
                .Include(a => a.CandidatePayment)
                .Where(x => x.CandidateID == candidateID)
                .ToList();
            if (list.Count() > 0)
                return list;
            else
                return null;
        }

        public List<DAL.CandidateFormSl> GetAllCandidateFormSlByCandIDIsPaid_AD(long candidateID, bool isPaid)
        {
            List<DAL.CandidateFormSl> list = AdmissionDB.CandidateFormSls
                .Include(a => a.AdmissionSetup)
                .Include(a => a.AdmissionSetup.AdmissionUnit)
                .Include(a => a.AdmissionSetup.AdmissionUnit.AdmissionUnitPrograms)
                .Include(a => a.BasicInfo)
                .Include(a => a.CandidatePayment)
                .Where(x => x.CandidateID == candidateID && x.CandidatePayment.IsPaid == isPaid)
                .ToList();
            if (list.Count() > 0)
                return list;
            else
                return null;
        }

        public DAL.CandidateFormSl GetCandidateFormSlByFormSl_AD(long formSerial)
        {
            DAL.CandidateFormSl obj = AdmissionDB.CandidateFormSls
                .Include(a => a.AdmissionSetup)
                .Include(a => a.AdmissionSetup.AdmissionUnit)
                .Include(a => a.AdmissionSetup.AdmissionUnit.AdmissionUnitPrograms)
                .Include(a => a.BasicInfo)
                .Include(a => a.CandidatePayment)
                .Where(x => x.FormSerial == formSerial)
                .FirstOrDefault();
            if (obj != null)
            {
                return obj;
            }
            else
            {
                return null;
            }
        }

        public List<DAL.CandidateFormSl> GetAllCandidateFormSlByFormSl_AD(long formSerial)
        {
            List<DAL.CandidateFormSl> list = AdmissionDB.CandidateFormSls
                .Include(a => a.AdmissionSetup)
                .Include(a => a.AdmissionSetup.AdmissionUnit)
                .Include(a => a.AdmissionSetup.AdmissionUnit.AdmissionUnitPrograms)
                .Include(a => a.BasicInfo)
                .Include(a => a.CandidatePayment)
                .Where(x => x.FormSerial == formSerial)
                .ToList();

            return list;
        }

        public List<DAL.CandidateFormSl> GetAllCandidateFormSlByFormSl_ND(long formSerial)
        {
            List<DAL.CandidateFormSl> list = AdmissionDB.CandidateFormSls
                .Where(x => x.FormSerial == formSerial)
                .ToList();

            return list;
        }

        public DAL.CandidateFormSl GetCandidateFormSlByCandID_ND(long candidateID)
        {
            DAL.CandidateFormSl obj = AdmissionDB.CandidateFormSls
                .Where(x => x.CandidateID == candidateID)
                .FirstOrDefault();

            if (obj != null)
                return obj;
            else
                return null;
        }

        public List<DAL.CandidateFormSl> GetAllCandidateFormSlByCandID_ND(long candidateID)
        {
            List<DAL.CandidateFormSl> list = AdmissionDB.CandidateFormSls
                .Where(x => x.CandidateID == candidateID)
                .ToList();

            return list;
        }

        #endregion

        #region CANDIDATE PAYMENT

        public DAL.CandidatePayment GetCandidatePaymentByID_ND(long candidatePaymentID)
        {
            DAL.CandidatePayment obj = AdmissionDB.CandidatePayments.Find(candidatePaymentID);
            if (obj != null)
                return obj;
            else
                return null;
        }

        public DAL.CandidatePayment GetCandidatePaymentByCandidateID(long candidateID)
        {
            DAL.CandidatePayment obj = AdmissionDB.CandidatePayments
                .Include(a => a.BasicInfo)
                .Include(a => a.CandidateFormSls)
                .Where(a => a.CandidateID == candidateID)
                .FirstOrDefault();
            if (obj != null)
            {
                return obj;
            }
            else
            {
                return null;
            }
        }

        public DAL.CandidatePayment GetCandidatePaymentByID_AD(long candidatePaymentID)
        {
            DAL.CandidatePayment obj = AdmissionDB.CandidatePayments
                .Include(a => a.BasicInfo)
                .Include(a => a.CandidateFormSls)
                .Where(a => a.ID == candidatePaymentID)
                .FirstOrDefault();
            if (obj != null)
            {
                return obj;
            }
            else
            {
                return null;
            }
        }


        public DAL.CandidatePayment GetCandidatePaymentByPaymentID_AD(long candidatePaymentID)
        {
            DAL.CandidatePayment obj = AdmissionDB.CandidatePayments
                .Include(a => a.BasicInfo)
                .Include(a => a.CandidateFormSls)
                .Where(a => a.PaymentId == candidatePaymentID)
                .FirstOrDefault();
            if (obj != null)
            {
                return obj;
            }
            else
            {
                return null;
            }
        }

        public List<DAL.CandidatePayment> GetAllCandidatePaymentByCandidateID_AD(long candidateId, bool isPaid)
        {
            List<DAL.CandidatePayment> list = AdmissionDB.CandidatePayments
                .Include(gt => gt.CandidateFormSls)
                .Include(gt => gt.BasicInfo)
                .Where(c => c.CandidateID == candidateId && c.IsPaid == isPaid)
                .ToList();
            if (list.Count() > 0) { return list; }
            else return null;
        }

        public List<DAL.CandidatePayment> GetAllCandidatePaymentByCandidateID_ND(long candidateId, bool isPaid)
        {
            List<DAL.CandidatePayment> list = AdmissionDB.CandidatePayments
                .Where(c => c.CandidateID == candidateId && c.IsPaid == isPaid)
                .ToList();
            if (list.Count() > 0) { return list; }
            else return null;
        }

        public List<FormRequestListViewObject> GetCandidatePaymentTopN(int topNValue, bool isPaid)
        {
            var list = (from candPayment in AdmissionDB.CandidatePayments
                        join candFormSl in AdmissionDB.CandidateFormSls on candPayment.ID equals candFormSl.CandidatePaymentID
                        join candidate in AdmissionDB.BasicInfoes on candFormSl.CandidateID equals candidate.ID
                        join admSetup in AdmissionDB.AdmissionSetups on candFormSl.AdmissionSetupID equals admSetup.ID
                        join admUnit in AdmissionDB.AdmissionUnits on admSetup.AdmissionUnitID equals admUnit.ID
                        where candPayment.IsPaid == isPaid
                        select new FormRequestListViewObject
                        {
                            CandidateID = candidate.ID,
                            Name = candidate.FirstName,
                            CandidateFormSerialID = candFormSl.ID,
                            FormSerial = candFormSl.FormSerial,
                            CandidatePaymentID = candPayment.ID,
                            PaymentId = candPayment.PaymentId,
                            IsPaid = candPayment.IsPaid == true ? "Yes" : "No",
                            Mobile = candidate.SMSPhone,
                            AdmissionSetupID = admSetup.ID,
                            AdmissionUnitID = admUnit.ID,
                            UNIT = admUnit.UnitName,
                            DateApplied = candidate.DateCreated
                        }).Take(topNValue).ToList();
            if (list.Count() > 0)
            {
                return list.OrderByDescending(c => c.CandidateID).ToList();
            }
            else
            {
                return null;
            }
        }

        #endregion

        #region CANDIDATE USER

        public DAL.CandidateUser GetCandidateUserByUsername_ND(string username)
        {
            DAL.CandidateUser obj = AdmissionDB.CandidateUsers
                .Where(a => a.UsernameLoginId == username && a.IsActive == true)
                .FirstOrDefault();

            return obj;
        }

        public DAL.CertificateCandidateUser GetCertificateCandidateUserByUsername_ND(string username)
        {
            DAL.CertificateCandidateUser obj = AdmissionDB.CertificateCandidateUsers
                .Where(a => a.UsernameLoginId == username)
                .FirstOrDefault();

            return obj;
        }
        public DAL.PostgraduateDiplomaCandidateUser GetPostgraduateDiplomaCandidateUserByUsername_ND(string username)
        {
            DAL.PostgraduateDiplomaCandidateUser obj = AdmissionDB.PostgraduateDiplomaCandidateUsers
                .Where(a => a.UsernameLoginId == username)
                .FirstOrDefault();

            return obj;
        }

        public DAL.CandidateUser GetCandidateUserByID_ND(long userId)
        {
            DAL.CandidateUser obj = AdmissionDB.CandidateUsers.Find(userId);
            return obj;
        }

        public dynamic GetCandidateUserTopN_JOIN(int takeNo)
        {
            var list = (from candidateUser in AdmissionDB.CandidateUsers
                        join candidate in AdmissionDB.BasicInfoes on candidateUser.ID equals candidate.CandidateUserID
                        join candidatePayment in AdmissionDB.CandidatePayments on candidate.ID equals candidatePayment.CandidateID
                        where candidatePayment.IsPaid == true
                        select new
                        {
                            FirstName = candidate.FirstName,
                            UserName = candidateUser.UsernameLoginId,
                            Mobile = candidate.SMSPhone,
                            PaymentID = candidatePayment.PaymentId,
                            Password = candidateUser.Password,
                            DateCreated = candidateUser.DateCreated
                        }).Take(takeNo).ToList();

            if (list.Count() > 0)
            {
                return list.OrderByDescending(c => c.DateCreated).ToList();
            }
            else
            {
                return null;
            }
        }

        public dynamic GetCandidateUserByPaymentId_JOIN(long? paymentId, string mobile)
        {
            var list = (from candidateUser in AdmissionDB.CandidateUsers
                        join candidate in AdmissionDB.BasicInfoes on candidateUser.ID equals candidate.CandidateUserID
                        join candidatePayment in AdmissionDB.CandidatePayments on candidate.ID equals candidatePayment.CandidateID
                        where candidatePayment.IsPaid == true && (candidatePayment.PaymentId == paymentId || candidate.SMSPhone == mobile)
                        select new
                        {
                            FirstName = candidate.FirstName,
                            UserName = candidateUser.UsernameLoginId,
                            Mobile = candidate.SMSPhone,
                            PaymentID = candidatePayment.PaymentId,
                            Password = candidateUser.Password,
                            DateCreated = candidateUser.DateCreated
                        }).ToList();

            if (list.Count() > 0)
            {
                return list.OrderByDescending(c => c.DateCreated).ToList();
            }
            else
            {
                return null;
            }
        }

        public string GetSysterUserNameByID_ND(long userId)
        {
            string username = null;
            username = AdmissionDB.CandidateUsers
                .Where(c => c.ID == userId)
                .Select(c => c.UsernameLoginId)
                .FirstOrDefault();
            if (username != null)
                return username;
            else
                return "";
        }

        #endregion

        #region EXTRACURRICULAR ACTIVITY

        public DAL.ExtraCurricularActivity GetExtraCurricularActivityByCandidateIDSequenceNo_ND(long candidateId, string sequenceNo)
        {
            DAL.ExtraCurricularActivity obj = AdmissionDB.ExtraCurricularActivities
                .Where(c => c.CandidateID == candidateId && c.Attribute1 == sequenceNo)
                .FirstOrDefault();
            if (obj != null)
                return obj;
            else
                return null;
        }

        public List<DAL.ExtraCurricularActivity> GetAllExtraCurricularActivityByCandidateID_ND(long candidateId)
        {
            List<DAL.ExtraCurricularActivity> list = AdmissionDB.ExtraCurricularActivities
                .Where(c => c.CandidateID == candidateId)
                .ToList();
            if (list.Count() > 0)
                return list;
            else
                return null;
        }

        #endregion

        #region FINANCIAL GUARANTOR

        public DAL.FinancialGuarantorInfo GetFinancialGuarantorByCandidateID_ND(long candidateId)
        {
            DAL.FinancialGuarantorInfo obj = AdmissionDB.FinancialGuarantorInfoes
                .Where(c => c.CandidateID == candidateId)
                .FirstOrDefault();

            if (obj != null)
                return obj;
            else
                return null;
        }

        #endregion

        #region WORK EXPERIENCE

        public DAL.WorkExperience GetWorkExperienceByCandidateID_ND(long candidateId)
        {
            DAL.WorkExperience obj = AdmissionDB.WorkExperiences
                .Where(c => c.CandidateID == candidateId)
                .FirstOrDefault();
            if (obj != null)
                return obj;
            else
                return null;
        }

        #endregion

        #region RELATION

        public List<DAL.Relation> GetAllRelationByCandidateID_ND(long candidateId)
        {
            List<DAL.Relation> list = new List<Relation>();
            list = AdmissionDB.Relations
                .Where(a => a.CandidateID == candidateId)
                .ToList();
            if (list.Count() > 0)
            {
                return list;
            }
            else
            {
                return null;
            }
        }

        public List<DAL.Relation> GetAllRelationByCandidateID_AD(long candidateId)
        {
            List<DAL.Relation> list = new List<Relation>();
            list = AdmissionDB.Relations
                .Include(a => a.BasicInfo)
                .Include(a => a.RelationDetail)
                .Include(a => a.RelationType)
                .Where(a => a.CandidateID == candidateId)
                .ToList();

            if (list.Count() > 0)
                return list;
            else
                return null;
        }

        public DAL.Relation GetRelationByCandidateIDRelationTypeID_AD(long candidateId, int relationTypeId)
        {
            DAL.Relation obj = AdmissionDB.Relations
                .Include(a => a.RelationDetail)
                .Include(a => a.RelationType)
                .Where(a => a.CandidateID == candidateId && a.RelationTypeID == relationTypeId)
                .FirstOrDefault();
            if (obj != null)
                return obj;
            else
                return null;
        }

        #endregion

        #region RELATION DETAILS

        public DAL.RelationDetail GetRelationDetailByID_ND(long id)
        {
            DAL.RelationDetail obj = AdmissionDB.RelationDetails.Find(id);
            if (obj != null)
                return obj;
            else
                return null;
        }

        #endregion



        //public DAL.CandidatePayment GetCandidatePaymentByCandidateID(long candidateID)
        //{
        //    DAL.CandidatePayment obj = AdmissionDB.CandidatePayments
        //        .Include(a => a.BasicInfo)
        //        .Include(a => a.CandidateFormSls)
        //        .Where(a => a.CandidateID == candidateID)
        //        .FirstOrDefault();
        //    if (obj != null)
        //    {
        //        return obj;
        //    }
        //    else
        //    {
        //        return null;
        //    }
        //}


        #region Is Candidate Has ExamType SSC HSC
        public bool IsCandidateHasExamTypeSSCHSC(long candidateID)
        {
            bool hasExamTypeSSCHSC = false;

            List<DAL.Exam> examList = AdmissionDB.Exams.Where(x => x.CandidateID == candidateID).ToList();

            if (examList != null && examList.Count > 0)
            {

                if (examList.Where(x => x.ExamTypeID == 1).FirstOrDefault() != null &&
                    examList.Where(x => x.ExamTypeID == 2).FirstOrDefault() != null)
                {
                    hasExamTypeSSCHSC = true;
                }

                return hasExamTypeSSCHSC;
            }
            else
            {
                return hasExamTypeSSCHSC;
            }
        }
        #endregion

        #region Candidate Education Category ID
        public int GetCandidateEducationCategoryID(long candidateID)
        {
            int educationCategoryId = -1;
            DAL.CandidateFormSl obj = AdmissionDB.CandidateFormSls
                .Include(a => a.AdmissionSetup)
                .Where(x => x.CandidateID == candidateID)
                .FirstOrDefault();
            if (obj != null)
            {
                educationCategoryId = obj.AdmissionSetup.EducationCategoryID;
                return educationCategoryId;
            }                
            else
            {
                return educationCategoryId;
            }
                
        }
        #endregion


    }
}
