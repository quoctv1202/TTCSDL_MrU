using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml;
using System.Configuration;

namespace IS.Config
{
    public static class AppConfig//: System.Web.UI.Page
    {
        /// <summary>
        /// Kết nối đến cơ sở dữ liệu trong hệ thống, được quy định trong file .config tương ứng
        /// </summary>
        /// <returns></returns>
        public static string connectionString()
        {
            string con = ConfigurationSettings.AppSettings["connectionString"].ToString();

            return con;
        }
        /// <summary>
        /// Tiêu đề chính trên banner
        /// </summary>
        public static string MasterTitle
        {
            get
            {
                string con = "Chương trình minh họa";
                return con;
            }

        }

        public static string baseDirectory;
        public static string extension
        {
            get
            {
                return ".jsx";//".min.js";//
            }
        }

    }
}
