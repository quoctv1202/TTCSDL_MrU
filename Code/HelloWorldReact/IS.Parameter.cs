using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
namespace IS.Base
{

    public class spParam
    {
        public string name;
        public SqlDbType type;
        public Object data;
        public ParameterDirection direction = ParameterDirection.Input;
        public int size = 0;
        public int searchtype = 0;
        /// <summary>
        /// Khởi tạo một tham số
        /// </summary>
        /// <param name="sName">Tên tham số không có ký tự @</param>
        /// <param name="oType">Kiểu dữ liệu</param>
        /// <param name="oData">Dữ liệu của tham số</param>
        public spParam(string sName, SqlDbType oType, Object oData, int isearchtype)
        {
            //for input init
            name = sName;
            type = oType;
            searchtype = isearchtype;
            if ((type == SqlDbType.DateTime && oData.ToString() == ""))
            {
                data = null;
            }
            else
            {
                data = oData;
            }
        }
    }
}