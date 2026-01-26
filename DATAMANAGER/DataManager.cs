using DAL;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATAMANAGER
{
    public class AdmissionDataManager<T> : IDisposable where T : class
    {
        #region properties, constructors and defautl initializtion
        private Entities context = null;
        private bool disposed = false;

        public DbSet<T> DbSet { get; set; }

        public AdmissionDataManager()
        {
            context = new Entities();
            DbSet = context.Set<T>();
            context.Configuration.LazyLoadingEnabled = false;
            context.Configuration.ProxyCreationEnabled = false;
        }

        public AdmissionDataManager(Entities context)
        {
            DbSet = context.Set<T>();
        }
        #endregion

        public virtual async Task<List<T>> ToListAsync()
        {
            return await DbSet.ToListAsync();

        }

        public virtual List<T> ToList()
        {
            return DbSet.ToList();

        }

        public virtual async Task<T> FindAsync(int? id)
        {

            return await DbSet.FindAsync(id);
        }


        public virtual T Find(int? id)
        {
            return DbSet.Find(id);
        }


        public void Add(T entity)
        {
            DbSet.Add(entity);
            context.SaveChanges();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await context.SaveChangesAsync();

        }

        public int SaveChanges()
        {
            return context.SaveChanges();

        }

        public void Update(T entity)
        {
            context.Entry<T>(entity).State = EntityState.Modified;
            context.SaveChanges();
        }


        public void Delete(T entity)
        {

            DbSet.Remove(entity);
            context.SaveChanges();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
    //---------------------------------------------------------------------------------------------------

    public class GeneralDataManagerBase
    {
        //private bool disposed = false;
        //private CommonMasterEntities _CommonDB;
        private Entities _AdmissionDB;

        //public CommonMasterEntities CommonDB
        //{
        //    get
        //    {
        //        return _CommonDB;
        //    }
        //}

        public Entities AdmissionDB
        {
            get
            {
                return _AdmissionDB;
            }
        }

        //public GeneralDataManagerBase(CommonMasterEntities PmisDB)
        //{
        //    //_CommonDB = PmisDB;
        //}

        public GeneralDataManagerBase(Entities AdmissionDB)
        {
            _AdmissionDB = AdmissionDB;
        }

        //public GeneralDataManagerBase(PMISDBEntities PmisDB1, CommonMasterEntities PmisDB2)
        //{
        //    _PmisDB = PmisDB1;
        //    //_CommonDB = PmisDB2;
        //}


        public GeneralDataManagerBase()
        {
            _AdmissionDB = new Entities();
            //_CommonDB = new CommonMasterEntities();
        }
        //public void Dispose()
        //{
        //    _PmisDB.Dispose();
        //    _CommonDB.Dispose();
        //}
    }
    //---------------------------------------------------------------------------------------------------


    public class GeneralDataManager : GeneralDataManagerBase, IDisposable
    {
        private bool disposed = false;
        public GeneralDataManager(Entities AdmissionDB1)
            : base(AdmissionDB1)
        {
            //
        }

        public GeneralDataManager() : base()
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
            //if (typeof(T).Equals(typeof(PMIS_EMPLOYEE)))
            //{
            //    PmisDB.PMIS_EMPLOYEE.Add((PMIS_EMPLOYEE)obj);

            //}
            //using (var db = new PmisDataManager<T>())
            //{
            //    db.Add(obj);
            //    db.SaveChanges();
            //}

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
        // 3. 
        //

        #region DBBack

        public void SPCreateDatabaseBackUp(string filePathName)
        {
            AdmissionDB.Database.ExecuteSqlCommand(TransactionalBehavior.DoNotEnsureTransaction, @"EXEC [dbo].[SPDBBackupToDisk] @filePathName= N'" + filePathName + "' ");
        }

        public void SPCreateDatabaseLogBackUp(string filePathName)
        {
            AdmissionDB.Database.ExecuteSqlCommand(TransactionalBehavior.DoNotEnsureTransaction, @"EXEC [dbo].[SPDBBackupLogToDisk] @filePathName= N'" + filePathName + "' ");
        }

        #endregion

        #region bKash TRANSACTION STATUS CODE

        public List<DAL.BkashTrxStatusCode> GetAllBkashTrxStatusCode()
        {
            List<DAL.BkashTrxStatusCode> list = AdmissionDB.BkashTrxStatusCodes
                .Where(c => c.IsActive == true)
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

        public DAL.BkashTrxStatusCode GetBkashTrxStatusCodeByCode(string code, bool isActive)
        {
            DAL.BkashTrxStatusCode obj = AdmissionDB.BkashTrxStatusCodes
                .Where(c => c.Code == code && c.IsActive == isActive)
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

        #region MENU

        public List<DAL.Menu> GetAllMenu_AD()
        {
            List<DAL.Menu> list = AdmissionDB.Menus
                .Include(a => a.Menu2)
                .OrderBy(a => a.ParentMenuID)
                .ToList();

            return list;
        }

        public DAL.Menu GetMenuByMenuID_ND(long menuID)
        {
            DAL.Menu obj = AdmissionDB.Menus.Find(menuID);

            return obj;
        }

        #endregion

        #region RESULT DIVISION

        public DAL.ResultDivision GetResultDivisionByID(int resultDivisionId)
        {
            DAL.ResultDivision obj = AdmissionDB.ResultDivisions.Find(resultDivisionId);
            return obj;
        }

        #endregion

        #region EDUCATION BOARD

        public List<DAL.EducationBoard> GetAllEducationBoard_ND()
        {
            List<DAL.EducationBoard> list = AdmissionDB.EducationBoards.ToList();
            return list;
        }

        #endregion

        #region EDUCATION CATEGORY

        public List<DAL.EducationCategory> GetAllEducationCategory_ND()
        {
            List<DAL.EducationCategory> list = AdmissionDB.EducationCategories.ToList();
            return list;
        }

        #endregion

        #region EXAM TYPE

        public List<DAL.ExamType> GetAllExamType_ND()
        {
            List<DAL.ExamType> list = AdmissionDB.ExamTypes.ToList();
            return list;
        }

        #endregion

        #region GROUP OR SUBJECT

        public List<DAL.GroupOrSubject> GetAllGroupOrSubject_ND()
        {
            List<DAL.GroupOrSubject> list = AdmissionDB.GroupOrSubjects.ToList();
            return list;
        }

        #endregion

        #region RESULT DIVISION

        public List<DAL.ResultDivision> GetAllResultDivision_ND()
        {
            List<DAL.ResultDivision> list = AdmissionDB.ResultDivisions.Where(x=> x.IsActive == true).ToList();
            return list;
        }

        #endregion

        #region UNDERGRAD GRAD PROGRAMS

        public List<DAL.UndergradGradProgram> GetAllUndergradGradProgram_ND()
        {
            List<DAL.UndergradGradProgram> list = AdmissionDB.UndergradGradPrograms.ToList();
            return list;
        }

        #endregion

        #region DIVISION

        public List<DAL.Division> GetAllDivision_ND()
        {
            List<DAL.Division> list = AdmissionDB.Divisions.ToList();
            return list;
        }

        #endregion

        #region DISTRICT

        public List<DAL.District> GetAllDistrict_ND()
        {
            List<DAL.District> list = AdmissionDB.Districts.ToList();
            return list;
        }

        public List<DAL.District> GetAllDistrictByDivisionID_ND(int divisionId)
        {
            List<DAL.District> list = AdmissionDB.Districts
                .Where(a => a.DivisionID == divisionId)
                .ToList();
            return list;
        }

        #endregion

        #region QUOTA

        public DAL.Quota GetQuotaById(int id)
        {
            DAL.Quota obj = null;
            obj = AdmissionDB.Quotas.Find(id);
            if(obj != null)
            {
                return obj;
            }
            else
            {
                return null;
            }
        }

        public List<DAL.Quota> GetAllQuotasByActive(bool isActive)
        {
            List<DAL.Quota> list = null;
            list = AdmissionDB.Quotas.Where(c => c.IsActive == isActive).ToList();
            if(list != null)
            {
                return list;
            }
            else
            {
                return null;
            }
        }

        public List<DAL.Quota> GetAllQuotas()
        {
            List<DAL.Quota> list = null;
            list = AdmissionDB.Quotas.ToList();
            if (list != null)
            {
                return list;
            }
            else
            {
                return null;
            }
        }

        #endregion

    }



}
