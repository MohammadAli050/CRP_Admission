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


namespace Admission.Admission.PostgraduateDiploma.PDCandidateInfo
{
    public partial class CandApplicationPriority : PageBase
    {
        long uId = -1;
        string uRole = string.Empty;
        long cId = -1;

        int noOfPrograms = -1;

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            uId = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId); //systemUser primary key
            uRole = SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName);

            if ((string.IsNullOrEmpty(Request.QueryString["val"])))
            {
                Response.Redirect("~/Admission/Message.aspx?message=Illegal Page Request&type=warning", false);
            }
            else
            {
                cId = Int64.Parse(Request.QueryString["val"].ToString());
                //cId = Int64.Parse(Decrypt.DecryptString(Request.QueryString["val"].ToString()));

                hrefAppAdditional.NavigateUrl = "CandApplicationAdditional.aspx?val=" + Request.QueryString["val"].ToString();
                hrefAppAddress.NavigateUrl = "CandApplicationAddress.aspx?val=" + Request.QueryString["val"].ToString();
                //hrefAppAttachment.NavigateUrl = "CandApplicationAttachment.aspx?val=" + Request.QueryString["val"].ToString();
                hrefAppBasic.NavigateUrl = "CandApplicationBasic.aspx?val=" + Request.QueryString["val"].ToString();
                hrefAppEducation.NavigateUrl = "CandApplicationEducation.aspx?val=" + Request.QueryString["val"].ToString();
                //hrefAppFinGuar.NavigateUrl = "CandApplicationFinGuarantor.aspx?val=" + Request.QueryString["val"].ToString();
                hrefAppPriority.NavigateUrl = "CandApplicationPriority.aspx?val=" + Request.QueryString["val"].ToString();
                hrefAppRelation.NavigateUrl = "CandApplicationRelation.aspx?val=" + Request.QueryString["val"].ToString();
            }

            if (!IsPostBack)
            {
                if (cId > 0)
                {
                    //GenerateCandidatePriority();
                    LoadListView(cId);
                }
                else
                {
                    Response.Redirect("~/Admission/Login.aspx");
                }
            }
        }


        private void LoadListView(long cId)
        {

            //long cId = -1;
            bool isUndergradCandidate = true;

            //if (uId > 0)
            //{
            //    try
            //    {
            //        using (var db = new CandidateDataManager())
            //        {
            //            cId = db.GetCandidateIdByUserID_ND(uId);
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        Panel_GridView.Visible = false;
            //        Panel_Master.Visible = true;
            //        lblMessage_Masters.Text = "Error: Unable to get candidate.";
            //        lblMessage_Masters.ForeColor = Color.Crimson;
            //        return;
            //    }
            //}

            List<DAL.SPGetCandidatePriorityList_Result> candidatePriorityList = null;
            List<DAL.SPAdmissionUnitProgramsByCandidateId_Result> programsAppliedList = null;

            if (cId > 0)
            {
                try
                {
                    using (var db = new CandidateDataManager())
                    {
                        //SPGetCandidatePriorityList(candidateId, IsPaidCandidate, EducationCategoryID)

                        programsAppliedList = db.AdmissionDB.SPAdmissionUnitProgramsByCandidateId(cId, true).ToList();

                        if (programsAppliedList.Count() > 0)
                        {
                            foreach (var item in programsAppliedList)
                            {
                                if (item.EducationCategoryID == 6)
                                {
                                    isUndergradCandidate = false;
                                }
                            }
                        }

                        if (isUndergradCandidate == true)
                        {
                            candidatePriorityList = db.AdmissionDB.SPGetCandidatePriorityList(cId, true, 4).ToList();
                        }
                        else if (isUndergradCandidate == false)
                        {
                            candidatePriorityList = db.AdmissionDB.SPGetCandidatePriorityList(cId, true, 6).ToList();
                        }

                    }
                }
                catch (Exception ex)
                {
                    Panel_GridView.Visible = false;
                    Panel_Master.Visible = true;
                    lblMessage_Masters.Text = "Error: Unable to get candidate choice list.";
                    lblMessage_Masters.ForeColor = Color.Crimson;
                    return;
                }
            }

            if (candidatePriorityList.Count() > 0)
            {
                noOfPrograms = candidatePriorityList.Count();
                gvProgramPriority.DataSource = candidatePriorityList;
                gvProgramPriority.DataBind();
            }
            else
            {
                gvProgramPriority.DataSource = null;
                gvProgramPriority.DataBind();
            }

        }


        protected void gvProgramPriority_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DropDownList ddlPriority = (e.Row.FindControl("ddlPriority") as DropDownList);
                if (noOfPrograms > 0)
                {
                    //List<int> priorities = new List<int>();
                    ddlPriority.Items.Add(new ListItem("Select", "-1"));
                    for (int i = 1; i <= noOfPrograms; i++)
                    {
                        //priorities.Add(i);
                        ddlPriority.Items.Add(new ListItem(i.ToString(), i.ToString()));
                    }
                }

                //Label lblProgPriorId = (Label)e.Row.FindControl("lblProgPriorId");

                //long progPriorIdLong = -1;

                //if (!string.IsNullOrEmpty(lblProgPriorId.Text))
                //{
                //    progPriorIdLong = Convert.ToInt64(lblProgPriorId.Text);
                //}
                //else
                //{
                //    progPriorIdLong = -1;
                //}

                //DAL.ProgramPriority progPriorObj = null;
                //if (progPriorIdLong > 0)
                //{
                //    try
                //    {
                //        using (var db = new CandidateDataManager())
                //        {
                //            progPriorObj = db.AdmissionDB.ProgramPriorities.Find(progPriorIdLong);
                //        }
                //    }
                //    catch (Exception ex)
                //    {

                //    }
                //}

                Label lblProgPriorPriority = (Label)e.Row.FindControl("lblProgPriorPriority");
                int priority = -1;
                if (string.IsNullOrEmpty(lblProgPriorPriority.Text))
                {
                    //priority = Convert.ToInt32(progPriorObj.Priority);
                    priority = -1;
                }
                else
                {
                    priority = Convert.ToInt32(lblProgPriorPriority.Text);
                }

                ddlPriority.SelectedValue = priority.ToString();
                //if (priority > 0)
                //{
                //    ddlPriority.SelectedValue = priority.ToString();
                //}
                //else
                //{
                //    ddlPriority.SelectedValue = priority.ToString();
                //}

                Button btnSave = (e.Row.FindControl("btnSaveIndividual") as Button);

                btnSave.CommandName = "SaveIndividualPriority";
                btnSave.CommandArgument = e.Row.DataItemIndex.ToString();
            }
        }

        protected void gvProgramPriority_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "SaveIndividualPriority")
            {
                int index = Convert.ToInt32(e.CommandArgument.ToString());

                long cId = -1;

                if (uId > 0)
                {
                    try
                    {
                        using (var db = new CandidateDataManager())
                        {
                            cId = db.GetCandidateIdByUserID_ND(uId);
                        }
                    }
                    catch (Exception ex)
                    {
                        Panel_Master.Visible = true;
                        lblMessage_Masters.Text = "Error: Unable to get candidate.";
                        lblMessage_Masters.ForeColor = Color.Crimson;
                        return;
                    }
                }

                //DAL.SPGetCandidatePriorityList_Result objABC = null;

                DropDownList ddlPriority = (gvProgramPriority.Rows[index].FindControl("ddlPriority") as DropDownList);
                Label lblDdlPriorityMsg = (Label)gvProgramPriority.Rows[index].FindControl("lblDdlPriorityMsg");

                Label lblProgPriorId = (Label)gvProgramPriority.Rows[index].FindControl("lblProgPriorId");
                Label lblProgPriorCandidateId = (Label)gvProgramPriority.Rows[index].FindControl("lblProgPriorCandidateId");
                Label lblProgPriorAdmUnitID = (Label)gvProgramPriority.Rows[index].FindControl("lblProgPriorAdmUnitID");
                Label lblProgPriorAdmUnitProgID = (Label)gvProgramPriority.Rows[index].FindControl("lblProgPriorAdmUnitProgID");
                Label lblProgPriorAdmSetupID = (Label)gvProgramPriority.Rows[index].FindControl("lblProgPriorAdmSetupID");
                Label lblProgPriorBatchID = (Label)gvProgramPriority.Rows[index].FindControl("lblProgPriorBatchID");
                Label lblProgPriorAcaCalID = (Label)gvProgramPriority.Rows[index].FindControl("lblProgPriorAcaCalID");
                Label lblProgPriorProgramID = (Label)gvProgramPriority.Rows[index].FindControl("lblProgPriorProgramID");
                Label lblProgPriorProgramName = (Label)gvProgramPriority.Rows[index].FindControl("lblProgPriorProgramName");
                Label lblProgPriorShortName = (Label)gvProgramPriority.Rows[index].FindControl("lblProgPriorShortName");
                Label lblProgPriorPriority = (Label)gvProgramPriority.Rows[index].FindControl("lblProgPriorPriority");

                Label lblAdmSetupID = (Label)gvProgramPriority.Rows[index].FindControl("lblAdmSetupID");
                Label lblAdmSetupAcaCalID = (Label)gvProgramPriority.Rows[index].FindControl("lblAdmSetupAcaCalID");

                Label lblAdmUnitID = (Label)gvProgramPriority.Rows[index].FindControl("lblAdmUnitID");

                Label lblAdmUnitProgID = (Label)gvProgramPriority.Rows[index].FindControl("lblAdmUnitProgID");
                Label lblAdmUnitProgAcaCalID = (Label)gvProgramPriority.Rows[index].FindControl("lblAdmUnitProgAcaCalID");
                Label lblAdmUnitProgAdmUnitID = (Label)gvProgramPriority.Rows[index].FindControl("lblAdmUnitProgAdmUnitID");
                Label lblAdmUnitProgProgramID = (Label)gvProgramPriority.Rows[index].FindControl("lblAdmUnitProgProgramID");
                Label lblAdmUnitProgEduCatId = (Label)gvProgramPriority.Rows[index].FindControl("lblAdmUnitProgEduCatId");
                Label lblAdmUnitProgBatchID = (Label)gvProgramPriority.Rows[index].FindControl("lblAdmUnitProgBatchID");
                Label lblAdmUnitProgBatchName = (Label)gvProgramPriority.Rows[index].FindControl("lblAdmUnitProgBatchName");
                Label lblAdmUnitProgSemesterCode = (Label)gvProgramPriority.Rows[index].FindControl("lblAdmUnitProgSemesterCode");
                Label lblAdmUnitProgBatchCode = (Label)gvProgramPriority.Rows[index].FindControl("lblAdmUnitProgBatchCode");
                Label lblAdmUnitProgProgramCode = (Label)gvProgramPriority.Rows[index].FindControl("lblAdmUnitProgProgramCode");
                Label lblAdmUnitProgProgramName = (Label)gvProgramPriority.Rows[index].FindControl("lblAdmUnitProgProgramName");
                Label lblAdmUnitProgShortCode = (Label)gvProgramPriority.Rows[index].FindControl("lblAdmUnitProgShortCode");

                int selectedPriority = -1;
                selectedPriority = Convert.ToInt32(ddlPriority.SelectedValue);
                if (selectedPriority < 0)
                {
                    Panel_Master.Visible = false;

                    //lblMessage.Text = "Please select a priority/choice";
                    //Panel_Message.CssClass = "alert alert-warning";
                    //lblMessage.Focus();
                    lblDdlPriorityMsg.Text = "Please select a priority/choice";
                    lblDdlPriorityMsg.ForeColor = Color.Crimson;
                    lblDdlPriorityMsg.Focus();
                    return;
                }


                long ProgPriorID = -1;
                if (!string.IsNullOrEmpty(lblProgPriorId.Text))
                {
                    ProgPriorID = Convert.ToInt64(lblProgPriorId.Text);
                }
                else
                {
                    ProgPriorID = -1;
                } //end if-else if (!string.IsNullOrEmpty(lblProgPriorId.Text))

                if (ProgPriorID > 0) //ProgramPriority Exist...Update existing.
                {
                    DAL.ProgramPriority existingProgPriorObj = null;

                    try
                    {
                        using (var db = new CandidateDataManager())
                        {
                            existingProgPriorObj = db.AdmissionDB.ProgramPriorities.Find(ProgPriorID);
                        }
                    }
                    catch (Exception ex)
                    {
                        Panel_Master.Visible = true;
                        lblMessage_Masters.Text = "Error: Unable to get existing candidate priority/choice for update.";
                        lblMessage_Masters.ForeColor = Color.Crimson;
                        return;
                    }

                    if (existingProgPriorObj != null)
                    {
                        int priorityChoiceUpdate = Convert.ToInt32(ddlPriority.SelectedValue);

                        DAL.ProgramPriority updateProgPriorObj = new DAL.ProgramPriority();

                        updateProgPriorObj = existingProgPriorObj; //assign the existing object to the object that will be used to update the existing object.

                        updateProgPriorObj.AcaCalID = Convert.ToInt32(lblAdmSetupAcaCalID.Text);
                        updateProgPriorObj.AdmissionSetupID = Convert.ToInt64(lblAdmSetupID.Text);
                        updateProgPriorObj.AdmissionUnitID = Convert.ToInt64(lblAdmUnitID.Text);
                        updateProgPriorObj.AdmissionUnitProgramID = Convert.ToInt64(lblAdmUnitProgID.Text);
                        updateProgPriorObj.BatchID = null;
                        //updateProgPriorObj.CandidateID = existingProgPriorObj.CandidateID;
                        //updateProgPriorObj.CreatedBy = existingProgPriorObj.CreatedBy;
                        //updateProgPriorObj.DateCreated = existingProgPriorObj.DateCreated;
                        updateProgPriorObj.ModifiedBy = cId;
                        updateProgPriorObj.DateModified = DateTime.Now;
                        updateProgPriorObj.Priority = priorityChoiceUpdate;
                        updateProgPriorObj.ProgramID = Convert.ToInt32(lblAdmUnitProgProgramID.Text);
                        updateProgPriorObj.ProgramName = lblAdmUnitProgProgramName.Text;
                        updateProgPriorObj.ShortName = lblAdmUnitProgShortCode.Text;

                        try
                        {
                            using (var dbUpdate = new CandidateDataManager())
                            {
                                dbUpdate.Update<DAL.ProgramPriority>(updateProgPriorObj);
                            }
                            Panel_Master.Visible = true;
                            lblMessage_Masters.Text = "Candidate program priority saved.";
                            lblMessage_Masters.ForeColor = Color.Green;
                        }
                        catch (Exception ex)
                        {
                            Panel_Master.Visible = true;
                            lblMessage_Masters.Text = "Error: Unable to update existing candidate priority/choice.";
                            lblMessage_Masters.ForeColor = Color.Crimson;
                            return;
                        }
                    }

                }
                else //ProgramPriority Does Not Exist... Insert new
                {
                    int priorityChoiceInsert = Convert.ToInt32(ddlPriority.SelectedValue);

                    DAL.ProgramPriority candidatePriorityObj = new DAL.ProgramPriority();

                    candidatePriorityObj.AcaCalID = Convert.ToInt32(lblAdmSetupAcaCalID.Text);
                    candidatePriorityObj.AdmissionSetupID = Convert.ToInt64(lblAdmSetupID.Text);
                    candidatePriorityObj.AdmissionUnitID = Convert.ToInt64(lblAdmUnitID.Text);
                    candidatePriorityObj.AdmissionUnitProgramID = Convert.ToInt64(lblAdmUnitProgID.Text);
                    candidatePriorityObj.BatchID = null;
                    candidatePriorityObj.CandidateID = cId;
                    candidatePriorityObj.CreatedBy = cId;
                    candidatePriorityObj.DateCreated = DateTime.Now;
                    candidatePriorityObj.ModifiedBy = null;
                    candidatePriorityObj.DateModified = null;
                    candidatePriorityObj.Priority = priorityChoiceInsert;
                    candidatePriorityObj.ProgramID = Convert.ToInt32(lblAdmUnitProgProgramID.Text);
                    candidatePriorityObj.ProgramName = lblAdmUnitProgProgramName.Text;
                    candidatePriorityObj.ShortName = lblAdmUnitProgShortCode.Text;

                    DAL.ProgramPriority priorityObjectExist = null;

                    try
                    {
                        using (var db = new CandidateDataManager())
                        {
                            priorityObjectExist = db.AdmissionDB.ProgramPriorities
                                .Where(c => c.CandidateID == candidatePriorityObj.CandidateID &&
                                    c.Priority == candidatePriorityObj.Priority)
                                .FirstOrDefault();

                            // * DO NOT DELETE THE CODE BELOW ************************************
                            //priorityObjectExist = db.AdmissionDB.ProgramPriorities
                            //    .Where(c => c.CandidateID == candidatePriorityObj.CandidateID &&
                            //        c.AcaCalID == candidatePriorityObj.AcaCalID &&
                            //        c.AdmissionSetupID == candidatePriorityObj.AdmissionSetupID &&
                            //        c.AdmissionUnitID == candidatePriorityObj.AdmissionUnitID &&
                            //        c.AdmissionUnitProgramID == candidatePriorityObj.AdmissionUnitProgramID &&
                            //        c.ProgramID == candidatePriorityObj.ProgramID &&
                            //        c.Priority == candidatePriorityObj.Priority)
                            //    .FirstOrDefault();
                            // *******************************************************************
                        }
                    }
                    catch (Exception ex)
                    {

                        //to do...something here.

                    }// end try-catch

                    if (priorityObjectExist != null)
                    {
                        Panel_Master.Visible = true;
                        lblMessage_Masters.Text = "Another program with same priority exists. Please select a different priority value.";
                        lblMessage_Masters.ForeColor = Color.Crimson;
                        return;
                    }
                    else
                    {
                        try
                        {
                            using (var dbInsert = new CandidateDataManager())
                            {
                                dbInsert.Insert<DAL.ProgramPriority>(candidatePriorityObj);
                            }
                            Panel_Master.Visible = true;
                            lblMessage_Masters.Text = "Candidate program priority saved. [" + candidatePriorityObj.ProgramName + "]";
                            lblMessage_Masters.ForeColor = Color.Green;
                        }
                        catch (Exception ex)
                        {
                            Panel_Master.Visible = true;
                            lblMessage_Masters.Text = "Error saving candidate program priority.";
                            lblMessage_Masters.ForeColor = Color.Crimson;
                            return;
                        } // end try-catch
                    } // end if-else if(priorityObjectExist != null)
                }

            }
        }

        protected void btnSave_Priority_Click(object sender, EventArgs e)
        {
            //Nothing to do here...
        }

        //protected void btnNext_Click(object sender, EventArgs e)
        //{
        //    //Response.Redirect("ApplicationEducation.aspx", false);
        //}








    }
}
