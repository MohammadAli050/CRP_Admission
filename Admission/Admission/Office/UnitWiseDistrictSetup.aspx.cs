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
    public partial class UnitWiseDistrictSetup : PageBase
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
            using (var db = new OfficeDataManager())
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

                List<DAL.ProgramDistrictPriority> list = null;
                if (admSetup != null)
                {
                    try
                    {
                        using (var db = new OfficeDataManager())
                        {
                            list = db.AdmissionDB.ProgramDistrictPriorities
                                .Where(c => c.AcaCalID == admSetup.AcaCalID && c.AdmissionUnitID == admUnitId)
                                .ToList();
                        }
                    }
                    catch (Exception)
                    {
                        lblMessage.Text = "Error getting all District priorities.";
                        lblMessage.ForeColor = Color.Crimson;
                        messagePanel.CssClass = "alert alert-danger";
                    }

                }

                if (list != null && list.Count > 0)
                {
                    gvProgCampusPrior.DataSource = list.OrderBy(c => c.DistrictPriority).ToList();
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
                foreach (GridViewRow row in gvProgCampusPrior.Rows)
                {
                    DropDownList ddlPriority = (DropDownList)row.FindControl("ddlDistrictPriority");
                    Label lblPriority = (Label)row.FindControl("hfCampusPriority");

                    ddlPriority.AppendDataBoundItems = true;
                    for (int priority = 1; priority <= 50; priority++)
                    {
                        ddlPriority.Items.Add(new ListItem(Convert.ToString(priority), Convert.ToString(priority)));
                    }
                    ddlPriority.DataBind();
                    ddlPriority.SelectedValue = lblPriority.Text;
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

            if (admUnitId > 0)
            {
                List<DAL.DistrictSeatPlanSetup> districtSeatPlanSetupList = null;
                try
                {
                    using (var db = new OfficeDataManager())
                    {
                        districtSeatPlanSetupList = db.AdmissionDB.DistrictSeatPlanSetups
                            .Where(c => c.IsActive == true)
                            .ToList();
                    }
                }
                catch (Exception)
                {
                    lblMessage.Text = "Error getting all campuses.";
                    lblMessage.ForeColor = Color.Crimson;
                    messagePanel.CssClass = "alert alert-danger";
                }

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


                if (districtSeatPlanSetupList != null && admSetup != null)
                {

                    #region Delete exist setup by Faculty and Session
                    List<DAL.ProgramDistrictPriority> progDistrictPriorList = null;
                    try
                    {
                        using (var db = new OfficeDataManager())
                        {
                            progDistrictPriorList = db.AdmissionDB.ProgramDistrictPriorities
                                .Where(c => c.AcaCalID == admSetup.AcaCalID && c.AdmissionUnitID == admUnitId).ToList();
                        }
                    }
                    catch (Exception)
                    {

                        throw;
                    }

                    if (progDistrictPriorList != null && progDistrictPriorList.Count() > 0) // programCampusPriority exist with same acaCalId and admUnitID
                    {
                        foreach (DAL.ProgramDistrictPriority item in progDistrictPriorList)
                        {
                            try
                            {
                                using (var db = new OfficeDataManager())
                                {
                                    db.Delete<DAL.ProgramDistrictPriority>(item);
                                }
                            }
                            catch (Exception)
                            {
                                lblMessage.Text = "Error deleting existing campuses.";
                                lblMessage.ForeColor = Color.Crimson;
                                messagePanel.CssClass = "alert alert-danger";
                            }
                        }
                    }
                    #endregion


                    if (districtSeatPlanSetupList != null && districtSeatPlanSetupList.Count() > 0)
                    {
                        foreach (var item in districtSeatPlanSetupList)
                        {
                            try
                            {
                                DAL.ProgramDistrictPriority progDistrictPrior = new DAL.ProgramDistrictPriority();
                                progDistrictPrior.AcaCalID = admSetup.AcaCalID;
                                progDistrictPrior.AdmissionUnitID = admUnitId;
                                progDistrictPrior.AdmissionUnitName = ddlAdmUnit.SelectedItem.Text;
                                progDistrictPrior.DistrictSeatPlanSetupID = item.ID;
                                progDistrictPrior.ProgramID = null;
                                progDistrictPrior.ProgramName = null;
                                progDistrictPrior.DistrictName = item.Attribute1;
                                progDistrictPrior.DistrictNumber = item.DistrictNumber;
                                progDistrictPrior.DistrictPriority = item.DistrictPriority;
                                progDistrictPrior.CreatedBy = uId;
                                progDistrictPrior.DateCreated = DateTime.Now;

                                using (var db = new OfficeDataManager())
                                {
                                    db.Insert<DAL.ProgramDistrictPriority>(progDistrictPrior);
                                }
                            }
                            catch (Exception)
                            {
                                lblMessage.Text = "Error generating campuses for faculty.";
                                lblMessage.ForeColor = Color.Crimson;
                                messagePanel.CssClass = "alert alert-danger";
                            }
                        }

                        lblMessage.Text = "Generated.";
                        lblMessage.ForeColor = Color.Crimson;
                        messagePanel.CssClass = "alert alert-success";
                    } //end if (campusList.Count() > 0)
                }
            } //if(admUnitId > 0)
        }

        protected void lnkEdit_Click(object sender, EventArgs e)
        {
            LinkButton linkButton = new LinkButton();
            linkButton = (LinkButton)sender;
            long id = Convert.ToInt64(linkButton.CommandArgument);

            DAL.ProgramDistrictPriority obj = null;
            if (id > 0)
            {
                try
                {
                    using (var db = new OfficeDataManager())
                    {
                        obj = db.AdmissionDB.ProgramDistrictPriorities.Find(id);
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
                DropDownList ddlPriority = linkButton.NamingContainer.FindControl("ddlDistrictPriority") as DropDownList;
                obj.DistrictPriority = Convert.ToInt32(ddlPriority.SelectedValue);
                try
                {
                    using (var db = new OfficeDataManager())
                    {
                        db.Update<DAL.ProgramDistrictPriority>(obj);
                    }
                }
                catch (Exception)
                {
                    lblMessage.Text = "Error updating District priority.";
                    lblMessage.ForeColor = Color.Crimson;
                    messagePanel.CssClass = "alert alert-danger";
                }
            }
        }
    }
}