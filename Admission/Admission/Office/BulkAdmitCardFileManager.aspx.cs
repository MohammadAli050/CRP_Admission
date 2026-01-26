using Admission.App_Start;
using CommonUtility;
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
    public partial class BulkAdmitCardFileManager : PageBase
    {
        private string _pageUrl = HttpContext.Current.Request.Url.AbsoluteUri;

        long uId = 0;
        string uRole = string.Empty;

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            uId = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId); //user primary key
            uRole = SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName);

            if (uRole.ToLower() == "candidate")
            {

                SessionSGD.DeleteFromSession(SessionName.Common_UserId);
                SessionSGD.DeleteFromSession(SessionName.Common_LoginID);
                SessionSGD.DeleteFromSession(SessionName.Common_RedirectPage);
                SessionSGD.DeleteFromSession(SessionName.Common_RoleName);
                SessionSGD.DeleteFromSession(SessionName.Common_UserG);
                Response.Redirect("~/Admission/Home.aspx", false);
            }

            if (!IsPostBack)
            {
                LoadGridView();
            }
        }

        public class FileSystemItemObj
        {
            public string DirectoryName { get; set; }
            public string FullName { get; set; }
            public long? Size { get; set; }
            public DateTime CreationTime { get; set; }
            public bool IsFolder { get; set; }
        }

        private void LoadGridView()
        {
            //string[] filePaths = Directory.GetFiles(Server.MapPath("~/Backup/"));
            DirectoryInfo info = new DirectoryInfo(Server.MapPath("~/ApplicationDocs/BulkAdmitCard/"));

            DirectoryInfo[] directoryInfos = info.GetDirectories();

            List<FileSystemItemObj> fsItems = new List<FileSystemItemObj>();

            foreach (DirectoryInfo directoryPath in directoryInfos)
            {
                FileSystemItemObj fsItemObj = new FileSystemItemObj();
                fsItemObj.FullName = directoryPath.FullName;
                fsItemObj.DirectoryName = directoryPath.Name;
                fsItemObj.Size = null;
                fsItemObj.CreationTime = directoryPath.CreationTime;
                fsItemObj.IsFolder = true;

                fsItems.Add(fsItemObj);
            }

            if (fsItems.Count() > 0)
            {
                GridView1.DataSource = fsItems;
            }
            else
            {
                GridView1.DataSource = null;
            }
            GridView1.DataBind();
        }

        protected void lnkDownload_Click(object sender, EventArgs e)
        {
            try
            {
                string dirName = (sender as LinkButton).CommandArgument;
                string defaultPath = "~/ApplicationDocs/BulkAdmitCard/";
                string fullPath = defaultPath + dirName + "/";

                DirectoryInfo info = new DirectoryInfo(Server.MapPath(fullPath));
                FileInfo[] files = info.GetFiles().OrderBy(x => x.Name).ToArray();

                int StartSL = 0, EndSL = 0;
                try
                {
                    if (!string.IsNullOrEmpty(txtMinTextRoll.Text))
                        StartSL = Convert.ToInt32(txtMinTextRoll.Text.Trim());

                    if (!string.IsNullOrEmpty(txtMaxTextRoll.Text))
                        EndSL = Convert.ToInt32(txtMaxTextRoll.Text.Trim());
                }
                catch (Exception ex)
                {
                }

                if (files.Count() > 0)
                {
                    Response.Clear();
                    Response.ContentType = "application/zip";
                    Response.AddHeader("content-disposition", "filename=" + dirName + ".zip");
                    using (ZipFile zip = new ZipFile())
                    {
                        int i = 1;
                        foreach (var item in files)
                        {
                            if (StartSL > 0 && EndSL > 0 && EndSL >= StartSL)
                            {
                                if(i>=StartSL && i<=EndSL)
                                {
                                    string fileName = item.Name;
                                    zip.AddFile(item.FullName, dirName);
                                }
                            }
                            else
                            {
                                string fileName = item.Name;
                                zip.AddFile(item.FullName, dirName);
                            }
                            i++;

                        }
                        zip.Save(Response.OutputStream);

                    }
                    HttpCookie cookie = new HttpCookie("ExcelDownloadFlag");
                    cookie.Value = "Flag";
                    cookie.Expires = DateTime.Now.AddDays(1);
                    Response.AppendCookie(cookie);
                    Response.Flush();
                }
                else
                {
                    lblMessageGv.Text = "No file(s) to download.";
                    lblMessageGv.ForeColor = Color.Crimson;
                }


            }
            catch (Exception ex)
            {
                #region LOG /*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-/*-
                try
                {
                    DAL_Log.DataLog dLog = new DAL_Log.DataLog();
                    dLog.EventName = "BulkAdmitCardDownload_Exception";
                    dLog.PageName = "BulkAdmitCardFileManager.aspx";
                    dLog.OldData = "";
                    dLog.NewData = "Exception: " + ex.Message.ToString() + "; InnerException: " + ex.InnerException.Message.ToString();
                    dLog.UserId = uId;
                    dLog.DateCreated = DateTime.Now;
                    dLog.SessionInformation = SessionSGD.GetObjFromSession<long>(SessionName.Common_UserId).ToString() + "; " +
                        SessionSGD.GetObjFromSession<string>(SessionName.Common_RoleName) + "; UID: " + uId; ;
                    LogWriter.DataLogWriter(dLog);
                }
                catch (Exception ex2)
                {
                }
                #endregion
            }


        }

        protected void lnkDelete_Click(object sender, EventArgs e)
        {
            string dirName = (sender as LinkButton).CommandArgument;
            string defaultPath = "~/ApplicationDocs/BulkAdmitCard/";
            string fullPath = defaultPath + dirName + "/";

            DirectoryInfo info = new DirectoryInfo(Server.MapPath(fullPath));
            //string[] files = info.GetFiles(Server.MapPath(fullPath), "*", SearchOption.AllDirectories);

            //if (files.Count() > 0)
            //{
            //    foreach(string file in files)
            //    {
            //        File.Delete(fullPath + file.Name);
            //    }
            //}
            //else
            //{
            //    lblMessageGv.Text = "No file(s) to download.";
            //    lblMessageGv.ForeColor = Color.Crimson;
            //}

            //if (Directory.Exists(Server.MapPath(fullPath)))
            //{
            //    Directory.Delete(Server.MapPath(fullPath));
            //}


        }
    }
}