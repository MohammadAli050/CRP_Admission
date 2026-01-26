using DAL;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATAMANAGER
{
    public class OfficeDataManager : GeneralDataManagerBase, IDisposable
    {
        private bool disposed = false;

        public OfficeDataManager(Entities AdmissionDB) : base(AdmissionDB)
        {

        }

        public OfficeDataManager() : base()
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


        #region ADMISSION SETUP

        public AdmissionSetup GetAdmissionSetupByID_ND(long admissionSetupID)
        {
            AdmissionSetup obj = AdmissionDB.AdmissionSetups.Find(admissionSetupID);

            return obj;
        }

        public AdmissionSetup GetAdmissionSetupByID_AD(long admissionSetupID)
        {
            AdmissionSetup obj = AdmissionDB.AdmissionSetups
                .Include(a => a.AdmissionUnit)
                .Include(a => a.AdmissionUnit.AdmissionUnitPrograms)
                .Where(a => a.ID == admissionSetupID)
                .FirstOrDefault();

            return obj;
        }

        public List<AdmissionSetup> GetAllAdmissionSetup_AD()
        {
            List<AdmissionSetup> list = AdmissionDB.AdmissionSetups
                .Include(s => s.AdmissionUnit)
                .Include(s => s.AdmissionUnit.AdmissionUnitPrograms)
                .ToList();

            return list;
        }

        public List<AdmissionSetup> GetAllActiveAdmissionSetup_AD(bool isActive)
        {
            List<AdmissionSetup> list = AdmissionDB.AdmissionSetups
                .Include(s => s.AdmissionUnit)
                .Include(s => s.AdmissionUnit.AdmissionUnitPrograms)
                .Where(s => s.IsActive == isActive)
                .ToList();

            return list;
        }

        public List<AdmissionSetup> GetAllAdmissionSetupByAdmUnitID(long admissionUnitID)
        {
            List<AdmissionSetup> list = AdmissionDB.AdmissionSetups
                .Include(a => a.AdmissionUnit)
                .Where(a => a.AdmissionUnitID == admissionUnitID)
                .ToList();

            return list;
        }

        public List<AdmissionSetup> GetAllAdmissionSetupByStoreID(int storeID)
        {
            List<AdmissionSetup> list = AdmissionDB.AdmissionSetups
                .Include(a => a.AdmissionUnit)
                .Where(a => a.StoreID == storeID)
                .ToList();

            return list;
        }

        public List<AdmissionSetup> GetAllActiveAdmissionSetupByEducationCategoryID(int eduCatId, bool isActive)
        {
            List<DAL.AdmissionSetup> list = AdmissionDB.AdmissionSetups
                .Where(c => c.EducationCategoryID == eduCatId && c.IsActive == isActive)
                .ToList();

            return list;
        }

        public List<CertificateAdmissionSetup> GetAllCertificateActiveAdmissionSetupByEducationCategoryID(int eduCatId, bool isActive)
        {
            List<DAL.CertificateAdmissionSetup> list = AdmissionDB.CertificateAdmissionSetups
                .Where(c => c.EducationCategoryID == eduCatId && c.IsActive == isActive)
                .ToList();

            return list;
        }

        public List<PostgraduateDiplomaAdmissionSetup> GetAllPostgraduateDiplomaAdmissionSetupByEducationCategoryID(int eduCatId, bool isActive)
        {
            List<DAL.PostgraduateDiplomaAdmissionSetup> list = AdmissionDB.PostgraduateDiplomaAdmissionSetups
                .Where(c => c.EducationCategoryID == eduCatId && c.IsActive == isActive)
                .ToList();

            return list;
        }

        #endregion

        #region ADMISSION UNIT

        public List<DAL.AdmissionUnit> GetAllAdmissionUnit()
        {
            List<DAL.AdmissionUnit> list = AdmissionDB.AdmissionUnits
                .Where(c => c.IsActive == true)
                .ToList();
            return list;
        }


        #endregion

        #region ADMISSION UNIT PROGRAMS

        public AdmissionUnitProgram GetAdmissionUnitProgramByID_AD(long admUnitProgID)
        {
            AdmissionUnitProgram obj = AdmissionDB.AdmissionUnitPrograms
                .Include(b => b.AdmissionUnit)
                .Where(a => a.ID == admUnitProgID)
                .FirstOrDefault();

            return obj;
        }

        public List<AdmissionUnitProgram> GetAllAdmissionUnitProgram_AD()
        {
            List<DAL.AdmissionUnitProgram> obj = AdmissionDB.AdmissionUnitPrograms
                .Include(c => c.AdmissionUnit)
                .Include(c => c.EducationCategory)
                .ToList();

            return obj;
        }

        public List<AdmissionUnitProgram> GetAllAdmissionUnitProgramByAdmUnitIDEducCatID_ND(long admUnitId, int EduCatId)
        {
            List<DAL.AdmissionUnitProgram> list = AdmissionDB.AdmissionUnitPrograms
                .Where(c => c.AdmissionUnitID == admUnitId && c.EducationCategoryID == EduCatId)
                .ToList();
            return list;
        }

        //this method is for one to one record, where admission is taken program wise.
        public long GetEducationCategoryByAdmissionUnitProgramID_ND(long admissionUnitProgramId)
        {
            long educationCategoryID = -1;
            educationCategoryID = AdmissionDB.AdmissionUnitPrograms
                .Where(c => c.ID == admissionUnitProgramId && c.IsActive == true)
                .Select(c => c.EducationCategoryID)
                .FirstOrDefault();
            if (educationCategoryID > 0)
                return educationCategoryID;
            else
                return -1;
        }


        //this method is for one to one record, where admission is taken faculty wise
        public long GetEducationCategoryByAdmissionUnitID_ND(long admissionUnitId)
        {
            long educationCategoryID = -1;
            educationCategoryID = AdmissionDB.AdmissionUnitPrograms
                .Where(c => c.AdmissionUnitID == admissionUnitId && c.IsActive == true)
                .Select(sel => sel.EducationCategoryID)
                .FirstOrDefault();
            if (educationCategoryID > 0)
            {
                return educationCategoryID;
            }
            else
            {
                return -1;
            }
        }

        #endregion

        #region GLOBAL ADMISSION SETTINGS

        public DAL.GlobalAdmissionSetting GetGAS()
        {
            DAL.GlobalAdmissionSetting obj = AdmissionDB.GlobalAdmissionSettings.Find(1);
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

        #region UNIT PROGRAM DETAILS

        public DAL.UnitProgramDetail GetUnitProgramDetailByID_AD(long unitProgramDetailID)
        {
            DAL.UnitProgramDetail obj = AdmissionDB.UnitProgramDetails
                .Include(p => p.AdmissionUnit)
                .Include(p => p.AdmissionUnit.AdmissionUnitPrograms)
                .Where(p => p.ID == unitProgramDetailID)
                .FirstOrDefault();

            return obj;
        }

        public List<DAL.UnitProgramDetail> GetAllUnitProgramDetail_AD()
        {
            List<DAL.UnitProgramDetail> list = AdmissionDB.UnitProgramDetails
                .Include(a => a.AdmissionUnit)
                .Include(a => a.AdmissionUnit.AdmissionUnitPrograms)
                .ToList();

            return list;
        }

        //public List<DAL.UnitProgramDetail> GetAllUnitProgramDetailsByAdmissionUnitID_AD(long admUnitID)
        //{
        //    List<DAL.UnitProgramDetail> list = AdmissionDB.UnitProgramDetails
        //        .Include(a => a.AdmissionUnit)
        //        .Include(a=>a.AdmissionUnit.AdmissionUnitPrograms)
        //        .Where(b => b.AdmissionUnitID == admUnitID)
        //        .ToList();

        //    return list;
        //}

        //public DAL.AdmissionSetup GetProgramInformation_AD(long admissionSetupId)
        //{
        //    DAL.AdmissionSetup obj = AdmissionDB.AdmissionSetups
        //        .Include(a => a.AdmissionUnit)
        //        .Include(a => a.AdmissionUnit.AdmissionUnitPrograms)
        //        .Include(a => a.AdmissionUnit.UnitProgramDetails)
        //        .Where(a => a.AdmissionUnitID == admissionSetupId)
        //        .FirstOrDefault();

        //    return obj;
        //}

        public DAL.UnitProgramDetail GetProgramDetailsByAdmissionUnitID_AD(long admissionUnitId)
        {
            DAL.UnitProgramDetail obj = AdmissionDB.UnitProgramDetails
                .Include(a => a.AdmissionUnit)
                .Include(a => a.AdmissionUnit.AdmissionUnitPrograms)
                .Include(a => a.AdmissionUnit.AdmissionSetups)
                .Where(a => a.AdmissionUnitID == admissionUnitId && a.AdmissionUnit.IsActive == true)
                .FirstOrDefault();

            return obj;
        }

        #endregion

        #region ROLE

        public List<DAL.Role> GetAllRole_ND()
        {
            List<DAL.Role> list = AdmissionDB.Roles
                .ToList();
            return list;
        }

        #endregion

        #region STORE

        public List<DAL.Store> GetAllStores()
        {
            List<DAL.Store> obj = AdmissionDB.Stores.ToList();

            return obj;
        }

        public List<DAL.Store> GetAllActiveSSLStore(bool isActive)
        {
            List<DAL.Store> list = null;
            list = AdmissionDB.Stores.Where(c => c.IsActive == true).OrderBy(c => c.StoreName).ToList();
            if (list != null)
            {
                return list;
            }
            else
            {
                return null;
            }

        }

        public DAL.Store GetActiveSSLStoreForMultiplepurchase(bool isActive, bool isMultipleAllowed)
        {
            return AdmissionDB.Stores.Where(c => c.IsActive == true && c.IsMultipleAllowed == true).FirstOrDefault();
        }
        #endregion

        #region STORE FOSTER
        public List<DAL.StoreFoster> GetAllActiveFPGStore(bool isActive)
        {
            List<DAL.StoreFoster> list = null;
            list = AdmissionDB.StoreFosters.Where(c => c.IsActive == true)
                .OrderBy(c => c.StoreName).ToList();
            if (list != null)
                return list;
            else
                return null;
        }

        public DAL.StoreFoster GetFPGStoreByID(int id)
        {
            return AdmissionDB.StoreFosters.Find(id);
        }

        public DAL.StoreFoster GetFPGStoreActiveMultiplePurchaseStore(bool isActive, bool isMultipleAllowed)
        {
            return AdmissionDB.StoreFosters
                .Where(c => c.IsActive == isActive && c.IsMultipleAllowed == isMultipleAllowed)
                .FirstOrDefault();
        }


        public List<DAL.StoreFoster> GetAllStoreFosters()
        {
            List<DAL.StoreFoster> obj = AdmissionDB.StoreFosters.ToList();

            return obj;
        }

        #endregion

        #region SYSTEM USER

        public List<DAL.SystemUser> GetAllSystemUser_ND()
        {
            List<DAL.SystemUser> list = AdmissionDB.SystemUsers
                .OrderBy(a => a.Username)
                .Where(a => a.IsActive == true && a.IsSA == false)
                .ToList();
            return list;
        }

        public List<DAL.SystemUser> GetAllSystemUser_AD()
        {
            List<DAL.SystemUser> list = AdmissionDB.SystemUsers
                .Include(a => a.SystemUserInRoles)
                .ToList();
            return list;
        }

        public DAL.SystemUser GetSystemUserByUsername_AD(string username)
        {
            DAL.SystemUser user = AdmissionDB.SystemUsers
                .Include(a => a.SystemUserInRoles)
                .Where(a => a.Username == username)
                .FirstOrDefault();
            return user;
        }

        public string GetSysterUserNameByID_ND(long userId)
        {
            string username = null;
            username = AdmissionDB.SystemUsers
                .Where(c => c.ID == userId)
                .Select(c => c.Username)
                .FirstOrDefault();
            if (username != null)
                return username;
            else
                return "";
        }

        public DAL.SystemUser GetSystemUserSuperAdminByID(long userId, bool isSuperAdmin)
        {
            DAL.SystemUser obj = null;
            obj = AdmissionDB.SystemUsers.Where(c => c.ID == userId && c.IsSA == isSuperAdmin).FirstOrDefault();
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

        #region SYSTEM USER IN ROLE

        public List<DAL.SystemUserInRole> GetAllSysUserInRole_AD()
        {
            List<DAL.SystemUserInRole> list = AdmissionDB.SystemUserInRoles
                .Include(a => a.SystemUser)
                .Include(a => a.Role)
                .OrderBy(a => a.SystemUser.Username)
                .ToList();
            return list;
        }

        public List<DAL.SystemUserInRole> GetAllSysUserInRole_ND()
        {
            List<DAL.SystemUserInRole> list = AdmissionDB.SystemUserInRoles.ToList();
            return list;
        }

        public DAL.SystemUserInRole GetSysUserInRoleByUserID_AD(long systemUserID)
        {
            DAL.SystemUserInRole obj = AdmissionDB.SystemUserInRoles
                .Include(a => a.SystemUser)
                .Include(a => a.Role)
                .Where(a => a.SystemUserID == systemUserID)
                .FirstOrDefault();
            return obj;
        }

        #endregion

        #region MENU

        public List<DAL.Menu> GetAllRootMenu()
        {
            List<DAL.Menu> list = AdmissionDB.Menus
                .Where(a => a.ParentMenuID == null)
                .OrderBy(a => a.Name)
                .ToList();
            return list;
        }

        public DAL.Menu GetMenuByID(long? menuId)
        {
            DAL.Menu obj = AdmissionDB.Menus
                .Where(a => a.ID == menuId)
                .FirstOrDefault();
            return obj;
        }

        public List<DAL.Menu> GetMenuByParentMenuID(long? parentMenuId)
        {
            List<DAL.Menu> list = AdmissionDB.Menus
                .Where(a => a.ParentMenuID != null && a.ParentMenuID == parentMenuId)
                .OrderBy(a => a.MenuOrder)
                .ToList();
            return list;
        }

        #endregion

        #region MENU IN ROLE

        public DAL.MenuInRole GetMenuInRoleByRoleIDMenuID(long? roleId, long? menuId)
        {
            DAL.MenuInRole obj = AdmissionDB.MenuInRoles
                .Where(a => a.RoleID == roleId && a.MenuID == menuId)
                .FirstOrDefault();
            return obj;
        }

        public DAL.MenuInRole GetParentMenuInRoleByMenuID(long? menuId)
        {
            DAL.MenuInRole obj = AdmissionDB.MenuInRoles
                .Include(a => a.Menu)
                .Include(a => a.Role)
                .Where(a => a.Menu.ParentMenuID == null && a.MenuID == menuId)
                .FirstOrDefault();
            return obj;
        }

        public List<DAL.MenuInRole> GetAllParentMenuInRoleByMenuID(long? menuId)
        {
            List<DAL.MenuInRole> list = AdmissionDB.MenuInRoles
                .Include(a => a.Menu)
                .Include(a => a.Role)
                .Where(a => a.Menu.ParentMenuID == null && a.MenuID == menuId)
                .ToList();
            return list;
        }

        public DAL.MenuInRole GetChildMenuInRoleByParentMenuID(long? parentMenuId, long? roleId)
        {
            DAL.MenuInRole obj = AdmissionDB.MenuInRoles
                .Where(a => a.RoleID == roleId && a.MenuID == parentMenuId)
                .FirstOrDefault();
            return obj;
        }

        public List<DAL.MenuInRole> GetAllChildMenuInRoleByParentMenuID(long? parentMenuId)
        {
            List<DAL.MenuInRole> list = AdmissionDB.MenuInRoles
                .Include(a => a.Menu)
                .Include(a => a.Role)
                .Where(a => a.Menu.ParentMenuID == parentMenuId)
                .ToList();
            return list;
        }

        //public List<DAL.MenuInRole> GetAllMenuInRoleByRoleName(string roleName)
        //{
        //    List<DAL.MenuInRole> list = AdmissionDB.MenuInRoles
        //        .Include(a=>a.Menu)
        //        .Include(a=>a.Role)
        //        .Where(a => a.Role.RoleName == roleName)
        //        .ToList();
        //    return list;
        //}

        #endregion

        #region NOTICE

        public List<DAL.Notice> GetAllNotice()
        {
            List<DAL.Notice> list = AdmissionDB.Notices
                .ToList();
            return list;
        }

        public List<DAL.Notice> GetTop5Notice()
        {
            List<DAL.Notice> list = AdmissionDB.Notices
                .OrderByDescending(a => a.NoticeDate)
                .Take(5)
                .ToList();
            return list;
        }

        #endregion


        #region District Seat Limit VM
        public List<DAL.ViewModels.DistrictSeatLimitVM> GetDistrictSeatLimitVM(int acaCalId)
        {
            List<DAL.ViewModels.DistrictSeatLimitVM> dslvmList = new List<DAL.ViewModels.DistrictSeatLimitVM>();
            try
            {
                var data = AdmissionDB.DistrictSeatLimits.Where(x=> x.IsActive == true && x.AcaCalId == acaCalId).ToList();
                if (data != null && data.Count > 0)
                {
                    foreach(var tData in data)
                    {
                        DAL.ViewModels.DistrictSeatLimitVM model = new DAL.ViewModels.DistrictSeatLimitVM();
                        model.ID = tData.ID;
                        model.DistrictId = (int)tData.DistrictId;

                        DAL.DistrictForSeatPlan dis = AdmissionDB.DistrictForSeatPlans.Where(x => x.ID == tData.DistrictId).FirstOrDefault();
                        if (dis != null)
                        {
                            model.DistrictName = dis.Name;
                        }
                        else
                        {
                            model.DistrictName = "";
                        }

                        #region N/A
                        //if (tData.DistrictId == 1)
                        //{
                        //    model.DistrictName = "Dhaka";
                        //}
                        //else if (tData.DistrictId == 2)
                        //{
                        //    model.DistrictName = "Chattogram";
                        //}
                        //else if (tData.DistrictId == 3)
                        //{
                        //    model.DistrictName = "Bogura";
                        //}
                        //else if (tData.DistrictId == 4)
                        //{
                        //    model.DistrictName = "Khulna";
                        //} 
                        #endregion


                        model.SeatLimit = (int)tData.SeatLimit;
                        model.SeatFillup = (int)tData.SeatFillup;

                        if (model.DistrictName != "")
                        {
                            dslvmList.Add(model);
                        }

                        model.AdmissionUnitId = (long)tData.AdmissionUnitId;
                        DAL.AdmissionUnit admUnit = null;
                        using (var db = new OfficeDataManager())
                        {
                            admUnit = db.AdmissionDB.AdmissionUnits.Where(x => x.ID == tData.AdmissionUnitId).FirstOrDefault();
                            if (admUnit != null)
                            {
                                model.AdmissionUnitName = admUnit.UnitName;
                            }
                        }
                    }

                    return dslvmList;
                }
                else
                {
                    return new List<DAL.ViewModels.DistrictSeatLimitVM>();
                }
                
            }
            catch (Exception ex)
            {
                return new List<DAL.ViewModels.DistrictSeatLimitVM>();
            }

            
        }

        #endregion
        
        // ### For Different Functions in the system.

    }
}
