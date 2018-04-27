using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;
/// <summary>
/// Summary description for lang
/// </summary>
namespace IS.Sess
{
    /// <summary>
    /// LOGINED STAFF
    /// </summary>
    public class STAFF_INFO
    {
        public STAFF_INFO()
        {

        }
        /// <summary>
        /// Gán các thông tin cho đối tượng hiện tại khi khởi tạo đối tượng
        /// </summary>
        /// <param name="code">Mã quản lý của người đăng nhập</param>
        /// <param name="codeview">Mã để dùng đăng nhập - username</param>
        /// <param name="name">Tên đầy đủ người đăng nhập</param>
        /// <param name="departmentcode">Mã phòng ban mà nhân việc thuộc về</param>
        /// <param name="researchdepartmentcode">Mã đơn vị tham gia nghiên cứu khoa học</param>
        /// <param name="researchreport">Tình trạng báo cáo nghiên cứu khoa học</param>
        /// <param name="researchtype">Phan loại báo cáo nghiên cứu khoa học</param>
        /// <param name="degreecode">Mã học vị</param>
        /// <param name="leveltitlecode">Mã chức vụ</param>
        /// <param name="academictitecode">Mã học hàm</param>
        /// <param name="armyrankcode">Mã quân hàm</param>
        /// <param name="partyleveltitlecode">Mã chức vụ Đảng</param>
        /// <param name="changepass">Thay đổi mật khẩu hay không</param>
        /// <param name="logintime">Thời điểm đang nhập</param>
        /// <param name="type">Loại hình đăng nhập</param>
        public STAFF_INFO(string universitycode, string code, string codeview, string name, string departmentcode, string researchdepartmentcode, int researchreport, int researchtype
            , string degreecode, string leveltitlecode, string academictitecode, string armyrankcode, string partyleveltitlecode, int changepass, DateTime logintime, string type, string img, string deparmtmentname, string researchdepartmentname)
        {
            CODE = code;
            CODEVIEW = codeview;
            NAME = name;
            DEPARTMENTCODE = departmentcode;
            RESEARCHDEPARTMENTCODE = researchdepartmentcode;
            RESEARCHREPORT = researchreport;
            RESEARCHTYPE = researchtype;
            DEGREECODE = degreecode;
            LEVELTITLECODE = leveltitlecode;
            ACADEMICTITLECODE = academictitecode;
            ARMYRANKCODE = armyrankcode;
            PARTYLEVELTITLECODE = partyleveltitlecode;
            CHANGEPASS = changepass;
            LOGTIME = logintime;
            TYPE = type;
            UNIVERSITYCODE = universitycode;
            IMG = img;
            DEPARTMENTNAME = deparmtmentname;
            RESEARCHDEPARTMENTNAME = researchdepartmentname;
        }
        public string UNIVERSITYCODE;
        public string CODE;
        public string CODEVIEW;
        public string NAME;
        public string DEPARTMENTCODE;
        public string RESEARCHDEPARTMENTCODE;
        public int RESEARCHREPORT;
        public int RESEARCHTYPE;
        public string DEGREECODE;
        public string LEVELTITLECODE;
        public string ACADEMICTITLECODE;
        public string ARMYRANKCODE;
        public string PARTYLEVELTITLECODE;
        public int CHANGEPASS;
        public DateTime LOGTIME;
        public string TYPE;
        public string IMG;
        public string DEPARTMENTNAME;
        public string RESEARCHDEPARTMENTNAME;
    }
    /// <summary>
    /// manage all sesion information
    /// </summary>

    public class session : System.Web.UI.Page
    {
        public session()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        /// <summary>
        /// Thiết lập giá trị thông qua key
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public int setKey(string key, object value)
        {
            System.Web.HttpContext.Current.Session[key] = value;
            return 0;
        }
        /// <summary>
        /// Lấy giá trị key, trong trường hợp không tồn tại sẽ trả về defaultValue
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public object getKey(string key, object defaultValue)
        {
            if (System.Web.HttpContext.Current.Session[key] == null)
            {
                return defaultValue;
            }
            return System.Web.HttpContext.Current.Session[key];

        }

        
        #region 4 login
        public int login(STAFF_INFO staff)
        {
            System.Web.HttpContext.Current.Session["STAFF_INFO"] = staff;
            //assign for later
            return 0;
        }
 
        /// <summary>
        /// Remove login information
        /// </summary>
        /// <returns></returns>
        public int logout()
        {
            System.Web.HttpContext.Current.Session["STAFF_INFO"] = null;
            return 0;
        }
        /// <summary>
        /// Check the login status, 0 is logined
        /// </summary>
        /// <returns>-1: not yet, 0 is logged</returns>
        public int isLogin()
        {
            if (System.Web.HttpContext.Current.Session["STAFF_INFO"] == null)
            {
                return -1;
            }
            STAFF_INFO staff = (STAFF_INFO)System.Web.HttpContext.Current.Session["STAFF_INFO"];
            if (staff.CODE == "")
            {
                return -1;
            }
            return 0;
        }
        #endregion
        #region login and logined information
        /// <summary>
        /// Tên đăng nhập - username
        /// </summary>
        public string loginName
        {
            get
            {
                if (System.Web.HttpContext.Current.Session["STAFF_INFO"] == null)
                    return "";
                STAFF_INFO staff = (STAFF_INFO)System.Web.HttpContext.Current.Session["STAFF_INFO"];
                return staff.CODEVIEW;
            }
            //set
            //{
            //    if (System.Web.HttpContext.Current.Session["STAFF_INFO"] != null)
            //    {
            //        STAFF_INFO staff = (STAFF_INFO)System.Web.HttpContext.Current.Session["STAFF_INFO"];
            //        staff.CODEVIEW=value;
            //        System.Web.HttpContext.Current.Session["STAFF_INFO"] = staff;
            //    }
            //}
        }
        /// <summary>
        /// get the login full name
        /// </summary>
        public string loginFullName
        {
            get
            {
                if (System.Web.HttpContext.Current.Session["STAFF_INFO"] == null)
                    return "";
                STAFF_INFO staff = (STAFF_INFO)System.Web.HttpContext.Current.Session["STAFF_INFO"];
                return staff.NAME;
            }
            //set
            //{
            //    if (System.Web.HttpContext.Current.Session["STAFF_INFO"] != null)
            //    {
            //        STAFF_INFO staff = (STAFF_INFO)System.Web.HttpContext.Current.Session["STAFF_INFO"];
            //        staff.NAME = value;
            //        System.Web.HttpContext.Current.Session["STAFF_INFO"] = staff;
            //    }
            //}
        }
        /// <summary>
        /// ảnh của người đăng nhập hiện tại
        /// </summary>
        public string loginImg
        {
            get
            {
                if (System.Web.HttpContext.Current.Session["STAFF_INFO"] == null)
                    return "";
                STAFF_INFO staff = (STAFF_INFO)System.Web.HttpContext.Current.Session["STAFF_INFO"];
                return staff.IMG;
            }
            //set
            //{
            //    if (System.Web.HttpContext.Current.Session["STAFF_INFO"] != null)
            //    {
            //        STAFF_INFO staff = (STAFF_INFO)System.Web.HttpContext.Current.Session["STAFF_INFO"];
            //        staff.NAME = value;
            //        System.Web.HttpContext.Current.Session["STAFF_INFO"] = staff;
            //    }
            //}
        }
        /// <summary>
        /// code of staff
        /// </summary>
        public string loginCode
        {
            get
            {
                if (System.Web.HttpContext.Current.Session["STAFF_INFO"] == null)
                    return "";
                STAFF_INFO staff = (STAFF_INFO)System.Web.HttpContext.Current.Session["STAFF_INFO"];
                return staff.CODE;
            }
            //set
            //{
            //    if (System.Web.HttpContext.Current.Session["STAFF_INFO"] != null)
            //    {
            //        STAFF_INFO staff = (STAFF_INFO)System.Web.HttpContext.Current.Session["STAFF_INFO"];
            //        staff.NAME = value;
            //        System.Web.HttpContext.Current.Session["STAFF_INFO"] = staff;
            //    }
            //}
        }

        /// <summary>
        /// Nhân viên đang đăng nhập
        /// </summary>
        public STAFF_INFO STAFF
        {
            get
            {
                if (System.Web.HttpContext.Current.Session["STAFF_INFO"] == null)
                    return null;
                STAFF_INFO staff = (STAFF_INFO)System.Web.HttpContext.Current.Session["STAFF_INFO"];
                return staff;
            }
            set
            {
                if (System.Web.HttpContext.Current.Session["STAFF_INFO"] != null)
                {
                    STAFF_INFO staff = (STAFF_INFO)System.Web.HttpContext.Current.Session["STAFF_INFO"];
                    System.Web.HttpContext.Current.Session["STAFF_INFO"] = value;
                }
            }
        }


        #endregion

        public int func(string key)
        {
            return 15;
        }




    }
}