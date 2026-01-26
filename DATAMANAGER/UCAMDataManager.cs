using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATAMANAGER
{
    public class UCAMDataManager<T> : IDisposable where T : class
    {
        #region properties, constructors and defautl initializtion
        private DAL_UCAM.UCAMDbEntities context = null;
        private bool disposed = false;

        public DbSet<T> DbSet { get; set; }

        public UCAMDataManager()
        {
            context = new DAL_UCAM.UCAMDbEntities();
            DbSet = context.Set<T>();
            context.Configuration.LazyLoadingEnabled = false;
        }

        public UCAMDataManager(DAL_UCAM.UCAMDbEntities context)
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

    #region UCAMdataManagerBase
    public class UCAMDataManagerBase
    {
        private DAL_UCAM.UCAMDbEntities _UCAMDb;

        public DAL_UCAM.UCAMDbEntities UCAMDb
        {
            get
            {
                return _UCAMDb;
            }
        }

        public UCAMDataManagerBase(DAL_UCAM.UCAMDbEntities AdmissionLog)
        {
            _UCAMDb = UCAMDb;
        }

        public UCAMDataManagerBase()
        {
            _UCAMDb = new DAL_UCAM.UCAMDbEntities();
        }
    }
    #endregion

    #region UCAMDataManager
    public class UCAMDataManager : UCAMDataManagerBase, IDisposable
    {
        private bool disposed = false;
        public UCAMDataManager(DAL_UCAM.UCAMDbEntities UCAMDb1)
            : base(UCAMDb1)
        {
            //
        }

        public UCAMDataManager() : base()
        {

        }

        public void Update<T>(T obj) where T : class
        {
            UCAMDb.Entry(obj).State = EntityState.Modified;
            UCAMDb.SaveChanges();
        }

        public void Delete<T>(T obj) where T : class
        {
            UCAMDb.Entry(obj).State = EntityState.Deleted;
            UCAMDb.SaveChanges();
        }

        public void Insert<T>(T obj) where T : class
        {
            UCAMDb.Entry<T>(obj).State = EntityState.Added;
            UCAMDb.SaveChanges();
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
                    UCAMDb.Dispose();
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
