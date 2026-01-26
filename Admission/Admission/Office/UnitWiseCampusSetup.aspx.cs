using Admission.App_Start;
using CommonUtility;
using DATAMANAGER;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Admission.Admission.Office
{
    public partial class UnitWiseCampusSetup : PageBase
    {
        private string _pageUrl = HttpContext.Current.Request.Url.AbsoluteUri;

        long uId = 0;
        string uRole = string.Empty;

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            uId = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId); //user primary key
            uRole = SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName);

            if (!IsPostBack)
            {
                LoadDDL();
            }

        }

        private void LoadDDL()
        {
            using(var db = new OfficeDataManager())
            {
                DDLHelper.Bind<DAL.AdmissionUnit>(ddlAdmUnit, db.AdmissionDB.AdmissionUnits.Where(c => c.IsActive == true).ToList(), "UnitName", "ID", EnumCollection.ListItemType.AdmissionUnit);
            }
        }

        private void LoadListView()
        {

        }

        protected void btnLoad_Click(object sender, EventArgs e)
        {
            long admUnitId = -1;
            admUnitId = Convert.ToInt64(ddlAdmUnit.SelectedValue);

            if(admUnitId > 0)
            {

                DAL.AdmissionSetup admSetup = null;
                try
                {
                    using (var db = new OfficeDataManager())
                    {
                        admSetup = db.AdmissionDB.AdmissionSetups
                            .Where(c => c.AdmissionUnitID == admUnitId && c.IsActive == true)
                            .FirstOrDefault();
                    }
                }
                catch (Exception)
                {

                    throw;
                }

                List<DAL.ProgramCampusPriority> list = null;
                if (admSetup != null)
                {
                    try
                    {
                        using (var db = new OfficeDataManager())
                        {
                            list = db.AdmissionDB.ProgramCampusPriorities
                                .Where(c => c.AcaCalID == admSetup.AcaCalID && c.AdmissionUnitID == admUnitId)
                                .ToList();
                        }
                    }
                    catch (Exception)
                    {
                        lblMessage.Text = "Error getting all campuses priorities.";
                        lblMessage.ForeColor = Color.Crimson;
                        messagePanel.CssClass = "alert alert-danger";
                    }
                    
                }

                if(list != null && list.Count > 0)
                {
                    gvProgCampusPrior.DataSource = list.OrderBy(c => c.ProgramDistrictPriorityId).ThenBy(x=> x.CampusID).ThenBy(x=> x.CampusPriority).ToList();
                }
                else
                {
                    gvProgCampusPrior.DataSource = null;
                }
                gvProgCampusPrior.DataBind();
                GridRebind();

            }//end if(admUnitId > 0)
        }

        private void GridRebind()
        {
            try
            {
                foreach(GridViewRow row in gvProgCampusPrior.Rows)
                {
                    DropDownList ddlPriority = (DropDownList)row.FindControl("ddlCampusPriority");
                    Label lblProgramDistrictPriorityId = (Label)row.FindControl("lblProgramDistrictPriorityId");
                    Label lblPriority = (Label)row.FindControl("hfCampusPriority");
                    Label lblVenueName = (Label)row.FindControl("lblVenueName");

                    ddlPriority.AppendDataBoundItems = true;
                    for (int priority = 1; priority <= 50; priority++)
                    {
                        ddlPriority.Items.Add(new ListItem(Convert.ToString(priority), Convert.ToString(priority)));
                    }
                    ddlPriority.DataBind();
                    ddlPriority.SelectedValue = lblPriority.Text;

                    long programDistrictPriorityId = Convert.ToInt64(lblProgramDistrictPriorityId.Text);
                    using (var db = new GeneralDataManager())
                    {
                        var districtName = db.AdmissionDB.ProgramDistrictPriorities.Where(x=> x.ID == programDistrictPriorityId).Select(x=> x.DistrictName).FirstOrDefault();
                        lblVenueName.Text = districtName;

                    }


                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected void btnGenerate_Click(object sender, EventArgs e)
        {
            long admUnitId = -1;
            admUnitId = Convert.ToInt64(ddlAdmUnit.SelectedValue);

            long progDistrictPriorId = -1;
            progDistrictPriorId = Convert.ToInt64(ddlDistrictSetup.SelectedValue);

            if (admUnitId > 0 && progDistrictPriorId > 0)
            {
                DAL.AdmissionSetup admSetup = null;
                try
                {
                    using (var db = new OfficeDataManager())
                    {
                        admSetup = db.AdmissionDB.AdmissionSetups
                            .Where(c => c.AdmissionUnitID == admUnitId && c.IsActive == true)
                            .FirstOrDefault();
                    }
                }
                catch (Exception ex)
                {
                    lblMessage.Text = "Error getting admission setup " + ex.Message + "; " + ex.InnerException.Message;
                    lblMessage.ForeColor = Color.Crimson;
                    messagePanel.CssClass = "alert alert-danger";
                    messagePanel.Visible = true;
                }

                DAL.ProgramDistrictPriority progDistrictPriorObj = null;
                if (admSetup != null)
                {
                    try
                    {
                        using (var db = new OfficeDataManager())
                        {
                            progDistrictPriorObj = db.AdmissionDB.ProgramDistrictPriorities.Find(progDistrictPriorId);
                        }
                    }
                    catch (Exception ex)
                    {
                        lblMessage.Text = "Error getting District Priorities " + ex.Message + "; " + ex.InnerException.Message;
                        lblMessage.ForeColor = Color.Crimson;
                        messagePanel.CssClass = "alert alert-danger";
                        messagePanel.Visible = true;
                    }
                }

                List<DAL.Campu> campusList = null;
                if (progDistrictPriorObj != null)
                {
                    try
                    {
                        using (var db = new OfficeDataManager())
                        {
                            campusList = db.AdmissionDB.Campus
                                .Where(c => c.DistrictSeatPlanSetupId == progDistrictPriorObj.DistrictSeatPlanSetupID)
                                .ToList();
                        }
                    }
                    catch (Exception ex)
                    {
                        lblMessage.Text = "Error getting buildings " + ex.Message + "; " + ex.InnerException.Message;
                        lblMessage.ForeColor = Color.Crimson;
                        messagePanel.CssClass = "alert alert-danger";
                        messagePanel.Visible = true;
                    }
                }

                List<DAL.ProgramCampusPriority> existingPrgCampusList = null;
                try
                {
                    using (var db = new OfficeDataManager())
                    {
                        existingPrgCampusList = db.AdmissionDB.ProgramCampusPriorities
                            .Where(c => c.AcaCalID == admSetup.AcaCalID && c.AdmissionUnitID == admUnitId && c.ProgramDistrictPriorityId == progDistrictPriorObj.ID).ToList();
                    }
                }
                catch (Exception ex)
                {
                    lblMessage.Text = "Error getting existing building priorities " + ex.Message + "; " + ex.InnerException.Message;
                    lblMessage.ForeColor = Color.Crimson;
                    messagePanel.CssClass = "alert alert-danger";
                    messagePanel.Visible = true;
                }
                if (existingPrgCampusList != null && existingPrgCampusList.Count() > 0)
                {
                    foreach (DAL.ProgramCampusPriority item in existingPrgCampusList)
                    {
                        try
                        {
                            using (var db = new OfficeDataManager())
                            {
                                db.Delete<DAL.ProgramCampusPriority>(item);
                            }
                        }
                        catch (Exception ex)
                        {
                            lblMessage.Text = "Error deleting existing buildings. " + ex.Message + "; " + ex.InnerException.Message;
                            lblMessage.ForeColor = Color.Crimson;
                            messagePanel.CssClass = "alert alert-danger";
                        }
                    }
                }


                if (campusList != null && campusList.Count() > 0)
                {
                    foreach (var item in campusList)
                    {
                        try
                        {
                            DAL.ProgramCampusPriority progCampusPrior = new DAL.ProgramCampusPriority();
                            progCampusPrior.AcaCalID = admSetup.AcaCalID;
                            progCampusPrior.AdmissionUnitID = admUnitId;
                            progCampusPrior.AdmissionUnitName = ddlAdmUnit.SelectedItem.Text;
                            progCampusPrior.CampusID = item.ID;
                            progCampusPrior.ProgramID = null;
                            progCampusPrior.ProgramName = null;
                            progCampusPrior.CampusName = item.CampusName;
                            progCampusPrior.CampusNumber = item.CampusNumber;
                            progCampusPrior.CampusPriority = item.CampusPriority;
                            progCampusPrior.CreatedBy = uId;
                            progCampusPrior.DateCreated = DateTime.Now;
                            progCampusPrior.ProgramDistrictPriorityId = progDistrictPriorId;

                            using (var db = new OfficeDataManager())
                            {
                                db.Insert<DAL.ProgramCampusPriority>(progCampusPrior);
                            }
                        }
                        catch (Exception)
                        {
                            lblMessage.Text = "Error generating campuses for faculty.";
                            lblMessage.ForeColor = Color.Crimson;
                            messagePanel.CssClass = "alert alert-danger";
                        }
                    }
                }


                #region N/A
                //List<DAL.Campu> campusList = null;
                //try
                //{
                //    using(var db = new OfficeDataManager())
                //    {
                //        campusList = db.AdmissionDB.Campus
                //            .Where(c => c.IsActive == true)
                //            .ToList();
                //    }
                //}
                //catch (Exception)
                //{
                //    lblMessage.Text = "Error getting all campuses.";
                //    lblMessage.ForeColor = Color.Crimson;
                //    messagePanel.CssClass = "alert alert-danger";
                //}

                //DAL.AdmissionSetup admSetup = null;
                //try
                //{
                //    using(var db = new OfficeDataManager())
                //    {
                //        admSetup = db.AdmissionDB.AdmissionSetups
                //            .Where(c => c.AdmissionUnitID == admUnitId && c.IsActive == true)
                //            .FirstOrDefault();
                //    }
                //}
                //catch (Exception)
                //{

                //    throw;
                //}

                //DAL.ProgramDistrictPriority progDistrictPriorObj = null;
                //if (admSetup != null)
                //{
                //    try
                //    {
                //        using (var db = new OfficeDataManager())
                //        {
                //            progDistrictPriorObj = db.AdmissionDB.ProgramDistrictPriorities.Find(progDistrictPriorId);
                //        }
                //    }
                //    catch (Exception ex)
                //    {
                //        lblMessage.Text = "Error getting District Priorities " + ex.Message + "; " + ex.InnerException.Message;
                //        lblMessage.ForeColor = Color.Crimson;
                //        messagePanel.CssClass = "alert alert-danger";
                //        messagePanel.Visible = true;
                //    }
                //}


                //if (campusList != null && admSetup != null)
                //{

                //    #region Delete exist setup by Faculty and Session
                //    List<DAL.ProgramCampusPriority> progCampPriorList = null;
                //    try
                //    {
                //        using (var db = new OfficeDataManager())
                //        {
                //            progCampPriorList = db.AdmissionDB.ProgramCampusPriorities
                //                .Where(c => c.AcaCalID == admSetup.AcaCalID && c.AdmissionUnitID == admUnitId).ToList();
                //        }
                //    }
                //    catch (Exception)
                //    {

                //        throw;
                //    }

                //    if (progCampPriorList != null) // programCampusPriority exist with same acaCalId and admUnitID
                //    {
                //        if (progCampPriorList.Count() > 0)
                //        {
                //            foreach (DAL.ProgramCampusPriority item in progCampPriorList)
                //            {
                //                try
                //                {
                //                    using (var db = new OfficeDataManager())
                //                    {
                //                        db.Delete<DAL.ProgramCampusPriority>(item);
                //                    }
                //                }
                //                catch (Exception)
                //                {
                //                    lblMessage.Text = "Error deleting existing campuses.";
                //                    lblMessage.ForeColor = Color.Crimson;
                //                    messagePanel.CssClass = "alert alert-danger";
                //                }
                //            }
                //        }
                //    } 
                //    #endregion


                //    if (campusList.Count() > 0)
                //    {
                //        foreach(var item in campusList)
                //        {
                //            try
                //            {
                //                DAL.ProgramCampusPriority progCampusPrior = new DAL.ProgramCampusPriority();
                //                progCampusPrior.AcaCalID = admSetup.AcaCalID;
                //                progCampusPrior.AdmissionUnitID = admUnitId;
                //                progCampusPrior.AdmissionUnitName = ddlAdmUnit.SelectedItem.Text;
                //                progCampusPrior.CampusID = item.ID;
                //                progCampusPrior.ProgramID = null;
                //                progCampusPrior.ProgramName = null;
                //                progCampusPrior.CampusName = item.CampusName;
                //                progCampusPrior.CampusNumber = item.CampusNumber;
                //                progCampusPrior.CampusPriority = item.CampusPriority;
                //                progCampusPrior.CreatedBy = uId;
                //                progCampusPrior.DateCreated = DateTime.Now;
                //                progCampusPrior.ProgramDistrictID = (int)progDistrictPriorId;

                //                using (var db = new OfficeDataManager())
                //                {
                //                    db.Insert<DAL.ProgramCampusPriority>(progCampusPrior);
                //                }
                //            }
                //            catch (Exception)
                //            {
                //                lblMessage.Text = "Error generating campuses for faculty.";
                //                lblMessage.ForeColor = Color.Crimson;
                //                messagePanel.CssClass = "alert alert-danger";
                //            }
                //        }

                //        lblMessage.Text = "Generated.";
                //        lblMessage.ForeColor = Color.Crimson;
                //        messagePanel.CssClass = "alert alert-success";
                //    } //end if (campusList.Count() > 0)
                //} 
                #endregion
            } //if(admUnitId > 0)
        }

        protected void lnkEdit_Click(object sender, EventArgs e)
        {
            LinkButton linkButton = new LinkButton();
            linkButton = (LinkButton)sender;
            long id = Convert.ToInt64(linkButton.CommandArgument);

            DAL.ProgramCampusPriority obj = null;
            if(id > 0)
            {
                try
                {
                    using(var db = new OfficeDataManager())
                    {
                        obj = db.AdmissionDB.ProgramCampusPriorities.Find(id);
                    }
                }
                catch (Exception)
                {
                    lblMessage.Text = "Error getting campuses for update.";
                    lblMessage.ForeColor = Color.Crimson;
                    messagePanel.CssClass = "alert alert-danger";
                }
            }

            if (obj != null)
            {
                DropDownList ddlPriority = linkButton.NamingContainer.FindControl("ddlCampusPriority") as DropDownList;
                obj.CampusPriority = Convert.ToInt32(ddlPriority.SelectedValue);
                try
                {
                    using (var db = new OfficeDataManager())
                    {
                        db.Update<DAL.ProgramCampusPriority>(obj);
                    }
                }
                catch (Exception)
                {
                    lblMessage.Text = "Error updating campuse priority.";
                    lblMessage.ForeColor = Color.Crimson;
                    messagePanel.CssClass = "alert alert-danger";
                }
            }
        }

        protected void ddlAdmUnit_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                long admUnitId = -1;
                admUnitId = Convert.ToInt64(ddlAdmUnit.SelectedValue);

                if (admUnitId > 0)
                {
                    DAL.AdmissionSetup admSetup = null;
                    try
                    {
                        using (var db = new OfficeDataManager())
                        {
                            admSetup = db.AdmissionDB.AdmissionSetups
                                .Where(c => c.AdmissionUnitID == admUnitId && c.IsActive == true)
                                .FirstOrDefault();
                        }
                    }
                    catch (Exception)
                    {

                        throw;
                    }

                    if (admSetup != null)
                    {
                        using (var db = new OfficeDataManager())
                        {
                            DDLHelper.Bind<DAL.ProgramDistrictPriority>(ddlDistrictSetup,
                                db.AdmissionDB.ProgramDistrictPriorities
                                    .Where(c => c.AdmissionUnitID == admUnitId && c.AcaCalID == admSetup.AcaCalID)
                                    .OrderBy(c => c.DistrictPriority).ToList(),
                                "DistrictName", "ID", EnumCollection.ListItemType.Select);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                
            }
        }
    }
}