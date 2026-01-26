using DATAMANAGER;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Admission.Admission.Admin
{
    public partial class CandidateFormSl : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        private void LoadListView()
        {
            using(var db = new CandidateDataManager())
            {
                if (!string.IsNullOrEmpty(txtCandidateId.Text.Trim()) && string.IsNullOrEmpty(txtFormSl.Text.Trim()))
                {
                    List<DAL.CandidateFormSl> list = db.GetAllCandidateFormSlByCandID_AD(Int64.Parse(txtCandidateId.Text.Trim()));
                    if (list.Any())
                    {
                        lvFormSl.DataSource = list;
                    }
                    else
                    {
                        lvFormSl.DataSource = null;
                    }
                }
                else if (!string.IsNullOrEmpty(txtFormSl.Text.Trim()) && string.IsNullOrEmpty(txtCandidateId.Text.Trim()))
                {
                    List<DAL.CandidateFormSl> list = db.GetAllCandidateFormSlByFormSl_AD(Int64.Parse(txtFormSl.Text.Trim()));
                    if (list.Any())
                    {
                        lvFormSl.DataSource = list;
                    }
                    else
                    {
                        lvFormSl.DataSource = null;
                    }
                }
                else if(string.IsNullOrEmpty(txtFormSl.Text.Trim()) && string.IsNullOrEmpty(txtCandidateId.Text.Trim()))
                {
                    lvFormSl.DataSource = null;
                }
                lvFormSl.DataBind();
            }
        }

        private List<DAL.CandidateFormSl> GetCandidateFormSl()
        {
            List<DAL.CandidateFormSl> list = new List<DAL.CandidateFormSl>();
            using (var db = new CandidateDataManager())
            {
                if (!string.IsNullOrEmpty(txtCandidateId.Text.Trim()))
                {
                    list = db.GetAllCandidateFormSlByCandID_ND(Int64.Parse(txtCandidateId.Text.Trim()));
                    if (list.Any())
                    {
                        list = list.ToList(); ;
                    }
                    else
                    {
                        list = null;
                    }
                }
                else if (!string.IsNullOrEmpty(txtFormSl.Text.Trim()))
                {
                    list = db.GetAllCandidateFormSlByFormSl_ND(Int64.Parse(txtFormSl.Text.Trim()));
                    if (list.Any())
                    {
                        list = list.ToList(); ;
                    }
                    else
                    {
                        list = null;
                    }
                }
            }
            return list;
        }

        protected void lvFormSl_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ListViewDataItem currentItem = (ListViewDataItem)e.Item;
                DAL.CandidateFormSl cFormSl = (DAL.CandidateFormSl)((ListViewDataItem)(e.Item)).DataItem;

                Label lblSerial = (Label)currentItem.FindControl("lblSerial");
                HiddenField hfFsLId = (HiddenField)currentItem.FindControl("hfFsLId");
                Label lblName = (Label)currentItem.FindControl("lblName");
                Label lblCandidateId = (Label)currentItem.FindControl("lblCandidateId");
                HiddenField hfCandidateId = (HiddenField)currentItem.FindControl("hfCandidateId");
                Label lblAcaCal = (Label)currentItem.FindControl("lblAcaCal");
                Label lblFormSerial = (Label)currentItem.FindControl("lblFormSerial");
                Label lblPaymentId = (Label)currentItem.FindControl("lblPaymentId");
                HiddenField hfPaymentId = (HiddenField)currentItem.FindControl("hfPaymentId");

                LinkButton lnkEdit = (LinkButton)currentItem.FindControl("lnkEdit");
                LinkButton lnkUpdate = (LinkButton)currentItem.FindControl("lnkUpdate");
                LinkButton lnkCancel = (LinkButton)currentItem.FindControl("lnkCancel");

                lblSerial.Text = (e.Item.DataItemIndex + 1).ToString();
                hfFsLId.Value = cFormSl.ID.ToString();
                lblCandidateId.Text = cFormSl.CandidateID.ToString();
                long candidateId = cFormSl.CandidateID;
                using(var dbBasicInfo = new CandidateDataManager())
                {
                    //long candidateId = Int64.Parse(hfCandidateId.Value);
                    DAL.BasicInfo basicInfoObj = new DAL.BasicInfo();
                    basicInfoObj = dbBasicInfo.AdmissionDB.BasicInfoes.Find(candidateId);
                    if(basicInfoObj.ID > 0)
                    {
                        Label lblNameNew = new Label();
                        lblNameNew.Text = basicInfoObj.FirstName;
                        lblName = lblNameNew;
                    }
                    else
                    {
                        lblName.Text = "N/A";
                    }
                }
                lblCandidateId.Text = cFormSl.CandidateID.ToString();
                lblAcaCal.Text = cFormSl.AcaCalID.ToString();
                lblFormSerial.Text = cFormSl.FormSerial.ToString();
                lblPaymentId.Text = cFormSl.CandidatePaymentID.ToString();
                long paymentId = cFormSl.CandidatePaymentID;
                using (var dbCPayment = new CandidateDataManager())
                {
                    //long paymentId = Int64.Parse(hfPaymentId.Value);
                    DAL.CandidatePayment cPaymentObj = new DAL.CandidatePayment();
                    cPaymentObj = dbCPayment.AdmissionDB.CandidatePayments.Find(paymentId);
                    if (cPaymentObj.ID > 0)
                    {
                        Label lblPaymentIdNew = new Label();
                        lblPaymentIdNew.Text = cPaymentObj.PaymentId.ToString();
                        lblPaymentId = lblPaymentIdNew;
                    }
                    else
                    {
                        lblPaymentId.Text = "N/A";
                    }
                }


            }
        }

        protected void lvFormSl_ItemEditing(object sender, ListViewEditEventArgs e)
        {
            lvFormSl.EditIndex = e.NewEditIndex;
            //lvFormSl.DataSource = GetCandidateFormSl();
            //lvFormSl.DataBind();
            LoadListView();
        }

        protected void lvFormSl_ItemUpdating(object sender, ListViewUpdateEventArgs e)
        {
            try
            {
                ListViewItem item = lvFormSl.Items[e.ItemIndex];

                HiddenField _hfFormSerialId = (HiddenField)item.FindControl("hfFsLId");
                long cFormSlId = Convert.ToInt64(_hfFormSerialId);
                TextBox txtFormSerial = (TextBox)item.FindControl("txtFormSerial");

                using(var db = new CandidateDataManager())
                {
                    DAL.CandidateFormSl obj = db.GetCandidateFormSlByID_ND(cFormSlId);
                    if (obj != null)
                    {
                        obj.FormSerial = Convert.ToInt64(txtFormSerial);
                        obj.Attribute1 = "modified date: " + DateTime.Now;
                        using (var dbUpdate = new CandidateDataManager())
                        {
                            dbUpdate.Update<DAL.CandidateFormSl>(obj);
                            lblMessage.Text = "Successful";
                            lblMessage.CssClass = "alert alert-success";
                            lblMessage.Visible = true;

                            //todo write log.
                        }
                    }
                }

            }
            catch(Exception ex)
            {
                lblMessage.Text = "Unsuccessful";
                lblMessage.CssClass = "alert alert-danger";
                lblMessage.Visible = true;
            }
        }

        protected void lvFormSl_ItemCanceling(object sender, ListViewCancelEventArgs e)
        {
            lvFormSl.EditIndex = -1;
            LoadListView();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            LoadListView();
        }
    }
}