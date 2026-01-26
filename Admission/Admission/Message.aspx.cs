using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Admission.Admission
{
    public partial class Message : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if ((!string.IsNullOrEmpty(Request.QueryString["message"])) && (!string.IsNullOrEmpty(Request.QueryString["type"])))
                {
                    string message = Request.QueryString["message"].ToString();
                    //TYPE:
                    //general   --
                    //success   -- green
                    //info      -- blue
                    //warning   -- yellow
                    //danger    -- red
                    string type = Request.QueryString["type"].ToString();
                    if (type == "success")
                    {
                        lblMessage.Text = "Dear Student, Congratulations! " + "<br/>" +
                                            "You have successfully completed your Application." + "<br/>" +
                                            "Please Login again to collect Your Admit Card." + "<br/>" +
                                            "BUP."; ;
                        messagePanel.CssClass = "alert alert-success";
                    }
                    else if (type == "info")
                    {
                        lblMessage.Text = message;
                        messagePanel.CssClass = "alert alert-info";
                    }
                    else if (type == "warning")
                    {
                        lblMessage.Text = message;
                        messagePanel.CssClass = "alert alert-warning";
                    }
                    else if (type == "danger")
                    {
                        lblMessage.Text = message;
                        messagePanel.CssClass = "alert alert-danger";
                    }
                    //lblMessage.Text = message;
                    messagePanel.Visible = true;
                }
                else
                {
                    messagePanel.Visible = false;
                }
            }
        }
    }
}