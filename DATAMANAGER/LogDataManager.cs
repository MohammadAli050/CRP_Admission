using DAL_Log;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATAMANAGER
{
    public class AdmissionLogDataManager<T> : IDisposable where T : class
    {
        #region properties, constructors and defautl initializtion
        private DAL_Log.EntitiesLog context = null;
        private bool disposed = false;

        public DbSet<T> DbSet { get; set; }

        public AdmissionLogDataManager()
        {
            context = new DAL_Log.EntitiesLog();
            DbSet = context.Set<T>();
            context.Configuration.LazyLoadingEnabled = false;
        }

        public AdmissionLogDataManager(DAL_Log.EntitiesLog context)
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
    //-----------------------------------------------------
    #region logdataManagerBase
    public class LogDataManagerBase
    {
        private DAL_Log.EntitiesLog _AdmissionLog;

        public DAL_Log.EntitiesLog AdmissionLog
        {
            get
            {
                return _AdmissionLog;
            }
        }

        public LogDataManagerBase(DAL_Log.EntitiesLog AdmissionLog)
        {
            _AdmissionLog = AdmissionLog;
        }

        public LogDataManagerBase()
        {
            _AdmissionLog = new DAL_Log.EntitiesLog();

        }

    }
    #endregion
    //---------------------------------------------------------------------------------------------------
    #region logDataManager
    public class LogDataManager : LogDataManagerBase, IDisposable
    {
        private bool disposed = false;
        public LogDataManager(DAL_Log.EntitiesLog AdmissionLog1)
            : base(AdmissionLog1)
        {
            //
        }

        public LogDataManager() : base()
        {

        }

        public void Update<T>(T obj) where T : class
        {
            AdmissionLog.Entry(obj).State = EntityState.Modified;
            AdmissionLog.SaveChanges();
        }

        public void Delete<T>(T obj) where T : class
        {
            AdmissionLog.Entry(obj).State = EntityState.Deleted;
            AdmissionLog.SaveChanges();
        }

        public void Insert<T>(T obj) where T : class
        {
            AdmissionLog.Entry<T>(obj).State = EntityState.Added;
            AdmissionLog.SaveChanges();
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
                    AdmissionLog.Dispose();
                    //CommonDB.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
        }
        #endregion
    }
    #endregion
}