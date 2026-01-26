using Admission.App_Start;
using DATAMANAGER;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Admission.Admission.Admin
{
    public partial class BackupDb : PageBase
    {
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (!IsPostBack)
            {
                LoadGridView();
            }
        }

        public class FileInformation
        {
            public string FileName { get; set; }
            public string FilePath { get; set; }
            public string DateModified { get; set; }
            public string FileSize { get; set; }
        }

        private void LoadGridView()
        {
            //string[] filePaths = Directory.GetFiles(Server.MapPath("~/Backup/"));
            DirectoryInfo info = new DirectoryInfo(Server.MapPath("~/Backup/"));
            FileInfo[] fileInfos = info.GetFiles().ToArray();

            List<FileInformation> files = new List<FileInformation>();
            foreach (FileInfo filePath in fileInfos)
            {
                //files.Add(new ListItem(Path.GetFileName(filePath), filePath));
                FileInformation _file = new FileInformation(); ;
                _file.FileName = filePath.Name;
                _file.FilePath = filePath.FullName;
                _file.DateModified = filePath.LastAccessTime.ToString("dd/MM/yyyy:HH-mm-ss");
                _file.FileSize = (((filePath.Length) / 1024f ) / 1024f ).ToString() + " MB";

                files.Add(_file);
            }

            if(files.Count() > 0)
            {
                GridView1.DataSource = files;
            }
            else
            {
                GridView1.DataSource = null;
            }
            GridView1.DataBind();
        }

        protected void btnCreateBack_Click(object sender, EventArgs e)
        {

            string filePath = Server.MapPath("~/Backup/");
            //E:\Deploy_Zone\BUP_Admission    <--server path

            //string filePath = "F:/WorkStation/esc_BUP_Admission_V2/Admission/Backup/";
            //string filePath = "E:/Deploy_Zone/BUP_Admission/Backup/";
            string fileName = "BUP_Admission_V2_" + DateTime.Now.ToString("yyyy_dd_MM_HH_mm_ss") + ".bak";

            try
            {
                using (var db = new GeneralDataManager())
                {
                    db.SPCreateDatabaseBackUp(filePath + fileName);
                    lblMessage.Text = "success";
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = ex.Message + "; " + ex.InnerException.Message;
            }
            LoadGridView();
        }

        protected void btnCreateBackLog_Click(object sender, EventArgs e)
        {
            string filePath = Server.MapPath("~/Backup/");
            //E:\Deploy_Zone\BUP_Admission    <--server path
            
            //string filePath = "F:/WorkStation/esc_BUP_Admission_V2/Admission/Backup/";
            //string filePath = "E:/Deploy_Zone/BUP_Admission/Backup/";
            string fileName = "BUP_Admission_V2_LOG_" + DateTime.Now.ToString("yyyy_dd_MM_HH_mm_ss") + ".bak";

            try
            {
                using (var db = new GeneralDataManager())
                {
                    db.SPCreateDatabaseLogBackUp(filePath + fileName);
                    lblMessage.Text = "success";
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = ex.Message + "; " + ex.InnerException.Message;
            }
            LoadGridView();
        }

        protected void lnkDownload_Click(object sender, EventArgs e)
        {
            string filePath = (sender as LinkButton).CommandArgument;
            Response.ContentType = ContentType;
            Response.AppendHeader("Content-Disposition", "attachment; filename=" + Path.GetFileName(filePath));
            Response.WriteFile(filePath);
            Response.End();
        }

        protected void lnkDelete_Click(object sender, EventArgs e)
        {
            string filePath = (sender as LinkButton).CommandArgument;
            File.Delete(filePath);

            lblMessageGv.Text = "File deleted";
            LoadGridView();
        }

    }
}