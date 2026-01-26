using Admission.App_Start;
using Ionic.Zip;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Admission.Admission.Office
{
    public partial class BackupCandidateDocuments : PageBase
    {
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (!IsPostBack)
            {
                LoadDDL();
            }
        }

        private void LoadDDL()
        {
            ddlSelectDir.Items.Clear();
            ddlSelectDir.Items.Add(new ListItem("Select", "-1"));
            ddlSelectDir.Items.Add(new ListItem("Candidate Photo", "1"));
            ddlSelectDir.Items.Add(new ListItem("Candidate Signature", "2"));
            ddlSelectDir.Items.Add(new ListItem("Financial Guarantor Signature", "3"));
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string candidatePhotoPath = "~/Upload/Candidate/Photo/";
            string candidateSignaturePath = "~/Upload/Candidate/Signature/";
            string candidateFinGuarSignPath = "~/Upload/FinancialGuarantor/Signature/";

            lblMessage.Text = "Please Wait...";
            lblMessage.ForeColor = Color.Crimson;

            if (ddlSelectDir.SelectedValue == "1")
            {
                //if no date range is selected, get the entire directory
                if (string.IsNullOrEmpty(txtDateFrom.Text.Trim()) && string.IsNullOrEmpty(txtDateTo.Text.Trim()))
                {
                    Response.Clear();
                    Response.ContentType = "application/zip";
                    Response.AddHeader("content-disposition", "filename=" + "CandidatePhoto_All.zip");
                    using (ZipFile zip = new ZipFile())
                    {
                        zip.AddDirectory(Server.MapPath(candidatePhotoPath));
                        zip.Save(Response.OutputStream);
                    }
                }
                //else get the files with created date in the given date range.
                else
                {
                    DateTime dateFrom = DateTime.Parse(txtDateFrom.Text.Trim());
                    DateTime dateTo = DateTime.Parse(txtDateTo.Text.Trim());

                    DirectoryInfo info = new DirectoryInfo(Server.MapPath(candidatePhotoPath));
                    FileInfo[] files = info.GetFiles().Where(d => d.CreationTime.Date >= dateFrom.Date && d.CreationTime.Date <= dateTo.Date).ToArray();

                    lblMessage.Text = files.Count().ToString();

                    if (files.Count() > 0)
                    {
                        Response.Clear();
                        Response.ContentType = "application/zip";
                        Response.AddHeader("content-disposition", "filename=" + "CandidatePhoto_From_" + txtDateFrom.Text.Trim() + "_To_" + txtDateTo.Text.Trim() + ".zip");
                        using (ZipFile zip = new ZipFile())
                        {
                            foreach (var item in files)
                            {
                                string fileName = item.Name;
                                //zip.AddFile(Server.MapPath(candidatePhotoPath) + fileName);
                                zip.AddFile(item.FullName, "Photo");
                            }
                            zip.Save(Response.OutputStream);
                        }
                    }
                    else
                    {
                        lblMessage.Text = "No file(s) to download.";
                        lblMessage.ForeColor = Color.Crimson;
                    }
                }
            } //end if (CandidatePhoto - Selected)
            else if (ddlSelectDir.SelectedValue == "2")
            {
                //if no date range is selected, get the entire directory
                if (string.IsNullOrEmpty(txtDateFrom.Text.Trim()) && string.IsNullOrEmpty(txtDateTo.Text.Trim()))
                {
                    Response.Clear();
                    Response.ContentType = "application/zip";
                    Response.AddHeader("content-disposition", "filename=" + "CandidateSignature_All.zip");
                    using (ZipFile zip = new ZipFile())
                    {
                        zip.AddDirectory(Server.MapPath(candidateSignaturePath));
                        zip.Save(Response.OutputStream);
                    }
                }
                //else get the files with created date in the given date range.
                else
                {
                    DateTime dateFrom = DateTime.Parse(txtDateFrom.Text.Trim());
                    DateTime dateTo = DateTime.Parse(txtDateTo.Text.Trim());

                    DirectoryInfo info = new DirectoryInfo(Server.MapPath(candidateSignaturePath));
                    FileInfo[] files = info.GetFiles().Where(d => d.CreationTime.Date >= dateFrom.Date && d.CreationTime.Date <= dateTo.Date).ToArray();

                    lblMessage.Text = files.Count().ToString();

                    if (files.Count() > 0)
                    {
                        Response.Clear();
                        Response.ContentType = "application/zip";
                        Response.AddHeader("content-disposition", "filename=" + "CandidateSign_From_" + txtDateFrom.Text.Trim() + "_To_" + txtDateTo.Text.Trim() + ".zip");
                        using (ZipFile zip = new ZipFile())
                        {
                            foreach (var item in files)
                            {
                                string fileName = item.Name;
                                zip.AddFile(item.FullName, "Signature");
                            }
                            zip.Save(Response.OutputStream);
                        }
                    }
                    else
                    {
                        lblMessage.Text = "No file(s) to download.";
                        lblMessage.ForeColor = Color.Crimson;
                    }
                }
            } //end if (CandidateSignature - Selected)
            else if (ddlSelectDir.SelectedValue == "1")
            {
                //if no date range is selected, get the entire directory
                if (string.IsNullOrEmpty(txtDateFrom.Text.Trim()) && string.IsNullOrEmpty(txtDateTo.Text.Trim()))
                {
                    Response.Clear();
                    Response.ContentType = "application/zip";
                    Response.AddHeader("content-disposition", "filename=" + "FinancialGuarantorSign_All.zip");
                    using (ZipFile zip = new ZipFile())
                    {
                        zip.AddDirectory(Server.MapPath(candidateFinGuarSignPath));
                        zip.Save(Response.OutputStream);
                    }
                }
                //else get the files with created date in the given date range.
                else
                {
                    DateTime dateFrom = DateTime.Parse(txtDateFrom.Text.Trim());
                    DateTime dateTo = DateTime.Parse(txtDateTo.Text.Trim());

                    DirectoryInfo info = new DirectoryInfo(Server.MapPath(candidateFinGuarSignPath));
                    FileInfo[] files = info.GetFiles().Where(d => d.CreationTime.Date >= dateFrom.Date && d.CreationTime.Date <= dateTo.Date).ToArray();

                    lblMessage.Text = files.Count().ToString();

                    if (files.Count() > 0)
                    {
                        Response.Clear();
                        Response.ContentType = "application/zip";
                        Response.AddHeader("content-disposition", "filename=" + "FinancialGuarantorSign_From_" + txtDateFrom.Text.Trim() + "_To_" + txtDateTo.Text.Trim() + ".zip");
                        using (ZipFile zip = new ZipFile())
                        {
                            foreach (var item in files)
                            {
                                string fileName = item.Name;
                                zip.AddFile(item.FullName, "FinancialGuarantorSign");
                            }
                            zip.Save(Response.OutputStream);
                        }
                    }
                    else
                    {
                        lblMessage.Text = "No file(s) to download.";
                        lblMessage.ForeColor = Color.Crimson;
                    }
                }
            } //end if (FinancialGuarantorSignature - Selected)
        }
    }
}