using Admission.App_Start;
using Newtonsoft.Json;
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
    public partial class MaintenanceSetup : PageBase
    {
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (!IsPostBack)
            {
                string path = Server.MapPath("~/ApplicationOfflineData/");
                using (StreamReader reader = new StreamReader(path + "maintainNotice.json"))
                {
                    string json = reader.ReadToEnd();
                    dynamic jsonObject = JsonConvert.DeserializeObject<dynamic>(json);

                    int id = Int32.Parse(jsonObject["ID"].ToString());
                    bool isActive = jsonObject["IsActive"];
                    string notice = jsonObject["Notice"].ToString();

                    if (isActive)
                    {
                        lblIsActive.Text = "Yes";
                        lblIsActive.ForeColor = Color.Green;
                    }
                    else
                    {
                        lblIsActive.Text = "No";
                        lblIsActive.ForeColor = Color.Crimson;
                    }
                    chbxShowMaintenanceNotice.Checked = isActive;
                    txtMaintenanceNotice.Text = notice;
                }
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            //DAL.NonDbObjects.MaintainNotice maintainObj = new DAL.NonDbObjects.MaintainNotice();
            //maintainObj.ID = 1;
            //maintainObj.IsActive = chbxShowMaintenanceNotice.Checked;
            //maintainObj.Notice = txtMaintenanceNotice.Text;

            string path = Server.MapPath("~/ApplicationOfflineData/");
            //using (StreamWriter file = File.CreateText(path + "maintainNotice.json"))
            //{
            //    JsonSerializer serializer = new JsonSerializer();
            //    serializer.Serialize(file, maintainObj);
            //}
            string json = string.Empty;
            dynamic jsonObject;
            using (StreamReader reader = new StreamReader(path + "maintainNotice.json"))
            {
                json = reader.ReadToEnd();
                jsonObject = JsonConvert.DeserializeObject<dynamic>(json);
            }

            jsonObject["ID"] = 1;
            jsonObject["IsActive"] = chbxShowMaintenanceNotice.Checked;
            jsonObject["Notice"] = txtMaintenanceNotice.Text;

            using (StreamWriter file = File.CreateText(path + "maintainNotice.json"))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(file, jsonObject);
            }
        }
    }
}