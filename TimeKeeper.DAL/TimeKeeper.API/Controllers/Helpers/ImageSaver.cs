using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using TimeKeeper.DAL.Entities;

namespace TimeKeeper.API.Controllers.Helpers
{
    public static class ImageSaver
    {
        public static string ConvertAndSave(this BaseClass<int> entity)
        {
            var type = entity.GetType().Name;
            string path = HttpContext.Current.Server.MapPath("~/App_Data/" + type + "s");

            byte[] ImageBytes = null;
            string FileName = "";

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            switch (type)
            {
                case "Employee":
                    var emp = entity as Employee;
                    if (String.IsNullOrEmpty(emp.Image)) return "";
                    ImageBytes = Convert.FromBase64String(emp.Image);
                    FileName = emp.Id.ToString() + "-" + emp.FirstName + "-" + emp.LastName + ".jpg";
                    break;

                case "Customer":
                    var cust = entity as Customer;
                    if (String.IsNullOrEmpty(cust.Image)) return "";
                    ImageBytes = Convert.FromBase64String(cust.Image);
                    FileName = cust.Id + "-" + cust.Name + ".jpg";
                    break;

                default: return "";
            }

            string FullPath = Path.Combine(path, FileName);
            File.WriteAllBytes(FullPath, ImageBytes);

            return FullPath;
        }

        public static string ConvertToBase64(this BaseClass <int> entity)
        {
            var type = entity.GetType().Name.ToString();

            if(type.Contains("_"))
            type = type.Substring(0, type.IndexOf("_"));

            string base64Photo = "";

            if (type == "Employee")
            {
                var emp = entity as Employee;
                base64Photo = (File.Exists(emp.Image)) ? Convert.ToBase64String(File.ReadAllBytes(emp.Image)) : "";
            }
            else if (type == "Customer")
            {
                var cust = entity as Customer;
                base64Photo = (File.Exists(cust.Image)) ? Convert.ToBase64String(File.ReadAllBytes(cust.Image)) : "";
            }

            return base64Photo;
        }

    }
}