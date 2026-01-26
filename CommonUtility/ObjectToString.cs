using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonUtility
{
    public class ObjectToString
    {
        public static string ConvertToString(Object obj)
        {
            var props = obj.GetType().GetProperties();
            var sb = new StringBuilder();
            foreach (var p in props)
            {
                sb.AppendLine(p.Name + ": " + p.GetValue(obj, null) + ";");
            }
            return sb.ToString();
        }



        //public static string ConvertObjectToString(object obj, string converteTypeName)
        //{

        //    string result = "";

        //    if (converteTypeName == "basicinfo")
        //    {
        //        try
        //        {
        //            //var props = obj.GetType().GetProperties();
        //            //var pi = typeof(T).GetProperty(obj.);

        //            DAL.BasicInfo bi = (DAL.BasicInfo)obj;
                    
        //            result += "FirstName: " + bi.FirstName.ToString();
        //            result += "Mobile: " + bi.Mobile.ToString();
        //            result += "SMSPhone: " + bi.SMSPhone.ToString();
        //            result += "Email: " + bi.Email.ToString();
        //            result += "DateOfBirth: " + bi.DateOfBirth.ToString();
        //            result += "PlaceOfBirth: " + bi.PlaceOfBirth.ToString();
        //            result += "BirthRegistrationNo: " + (!string.IsNullOrEmpty(bi.BirthRegistrationNo) ? bi.BirthRegistrationNo.ToString() : "");
        //            result += "NationalityID: " + (!string.IsNullOrEmpty(bi.NationalityID.ToString()) ? bi.NationalityID.ToString() : "");
        //        }
        //        catch (Exception ex)
        //        {
                    
        //        }
        //    }

        //    return result;

        //}



    }
}
