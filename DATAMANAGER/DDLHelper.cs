using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;

namespace DATAMANAGER
{
    public static class DDLHelper
    {
        public static void Bind<T>(DropDownList ddl, List<T> items, string nameProperty, string valueProperty)
        {
            ddl.DataSource = items;
            ddl.DataTextField = nameProperty;
            ddl.DataValueField = valueProperty;
            ddl.DataBind();
        }

        public static void Bind<T>(DropDownList ddl, List<T> items, string nameProperty, string valueProperty, bool sorted)
        {
            //DropDownList ddl = new DropDownList();
            if (sorted)
                items = items.ToList<T>();
            //end
            ddl.DataSource = items;
            ddl.DataTextField = nameProperty;
            ddl.DataValueField = valueProperty;
            ddl.DataBind();
            //return ddl;
        }

        public static void Bind<T>(DropDownList ddl, List<T> items, string nameProperty, string valueProperty, ListItemCollection extraItems, int hasSelectedValue)
        {
            if (hasSelectedValue == 1)
            {
                foreach (ListItem item in extraItems)
                {
                    ddl.Items.Insert(0, item);
                }
                Bind<T>(ddl, items, nameProperty, valueProperty);
            }
            else
            {
                Bind<T>(ddl, items, nameProperty, valueProperty);
                foreach (ListItem item in extraItems)
                {
                    ddl.Items.Insert(0, item);
                }
            }
        }

        public static void Bind<T>(DropDownList ddl, List<T> items, string nameProperty, string valueProperty, ListItemCollection extraItems)
        {

            Bind<T>(ddl, items, nameProperty, valueProperty);
            foreach (ListItem item in extraItems)
            {
                ddl.Items.Insert(0, item);
            }
        }
        public static void Bind<T>(DropDownList ddl, List<T> items, string nameProperty, string valueProperty, ListItemCollection extraItems, bool sorted)
        {
            Bind<T>(ddl, items, nameProperty, valueProperty, sorted);
            foreach (ListItem item in extraItems)
            {
                ddl.Items.Insert(0, item);
            }
        }
        public static void Bind(DropDownList ddl, ListItemCollection list)
        {
            foreach (ListItem item in list)
            {
                ddl.Items.Add(item);
            }
        }
        public static void Bind(DropDownList ddl, ListItemCollection list, EnumCollection.ListItemType ddlType)
        {
            Bind(ddl, list);
            foreach (ListItem item in GetExtraItems(ddlType))
            {
                ddl.Items.Insert(0, item);
            }
        }

        private static ListItemCollection GetExtraItems(EnumCollection.ListItemType ddltype)
        {
            ListItemCollection extraItems = new ListItemCollection();
            switch (ddltype)
            {
                case EnumCollection.ListItemType.AdmissionUnit:
                    extraItems.Add(new ListItem("--Select Faculty--", "-1"));
                    break;
                //case EnumCollection.ListItemType.Class: //for Class                   
                //    extraItems.Add(new ListItem("Select Class", "-1"));
                //    //Bind<T>(ddl, items, nameProperty, valueProperty, extraItems);
                //    break;
                case EnumCollection.ListItemType.Select:
                    extraItems.Add(new ListItem("--Select--", "-1"));
                    break;
                case EnumCollection.ListItemType.All:
                    extraItems.Add(new ListItem("--All--", "-1"));
                    break;
                case EnumCollection.ListItemType.SelectAll:
                    extraItems.Add(new ListItem("--Select All--", "0"));
                    break;
                case EnumCollection.ListItemType.Batch:
                    extraItems.Add(new ListItem("--Select Batch--", "-1"));
                    break;
                case EnumCollection.ListItemType.Session:
                    extraItems.Add(new ListItem("--Select Session--", "-1"));
                    break;
                case EnumCollection.ListItemType.Program:
                    extraItems.Add(new ListItem("--Select Program--", "-1"));
                    break;
                case EnumCollection.ListItemType.Gender:
                    extraItems.Add(new ListItem("--Select Gender--", "-1"));
                    break;
                case EnumCollection.ListItemType.District:
                    extraItems.Add(new ListItem("--Select District--", "-1"));
                    break;
                case EnumCollection.ListItemType.Division:
                    extraItems.Add(new ListItem("--Select Division--", "-1"));
                    break;
                case EnumCollection.ListItemType.EducationBoard:
                    extraItems.Add(new ListItem("--Select Board--", "-1"));
                    break;
                case EnumCollection.ListItemType.EducationCategory:
                    extraItems.Add(new ListItem("--Select Education Category--", "-1"));
                    break;
                case EnumCollection.ListItemType.GroupOrSubject:
                    extraItems.Add(new ListItem("--Select Group Or Subject--", "-1"));
                    break;
                case EnumCollection.ListItemType.Nationality:
                    extraItems.Add(new ListItem("--Select Nationality--", "-1"));
                    break;
                case EnumCollection.ListItemType.Country:
                    extraItems.Add(new ListItem("--Select Country--", "-1"));
                    break;
                case EnumCollection.ListItemType.Language:
                    extraItems.Add(new ListItem("--Select Language--", "-1"));
                    break;
                case EnumCollection.ListItemType.MotherTongue:
                    extraItems.Add(new ListItem("--Select Mother Tongue--", "-1"));
                    break;
                case EnumCollection.ListItemType.MaritalStatus:
                    extraItems.Add(new ListItem("--Select Marital Status--", "-1"));
                    break;
                case EnumCollection.ListItemType.BloodGroup:
                    extraItems.Add(new ListItem("--Select Blood Group--", "-1"));
                    break;
                case EnumCollection.ListItemType.Religion:
                    extraItems.Add(new ListItem("--Select Religion--", "-1"));
                    break;
                case EnumCollection.ListItemType.Quota:
                    extraItems.Add(new ListItem("--Select Quota--", "-1"));
                    break;
                case EnumCollection.ListItemType.QuotaType:
                    extraItems.Add(new ListItem("--Select Quota Type--", "-1"));
                    break;
                case EnumCollection.ListItemType.ExamType:
                    extraItems.Add(new ListItem("--Select Exam Type--", "-1"));
                    break;
                case EnumCollection.ListItemType.EducationMedium:
                    extraItems.Add(new ListItem("--Select Medium--", "-1"));
                    break;
            }
            return extraItems;
        }

        public static void Bind<T>(DropDownList ddl, List<T> items, string nameProperty, string valueProperty, EnumCollection.ListItemType ddltype)
        {
            Bind<T>(ddl, items, nameProperty, valueProperty, GetExtraItems(ddltype));

        }

        public static void Bind<T>(DropDownList ddl, List<T> items, string nameProperty, string valueProperty, EnumCollection.ListItemType ddltype, bool sorted)
        {
            Bind<T>(ddl, items, nameProperty, valueProperty, GetExtraItems(ddltype), sorted);
        }
    }
}
