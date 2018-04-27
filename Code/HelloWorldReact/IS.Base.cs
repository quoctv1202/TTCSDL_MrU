// ***********************************************************************
// Assembly         : IS.Base
// Author           : uyennm
// Created          : 06-23-2012
//
// Last Modified By : uyennm
// Last Modified On : 08-06-2012
// ***********************************************************************
// <copyright file="DBBase.cs" company="">
//     . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace IS.Base
{
    /// <summary>
    /// Class DBBase, phụ trách tất cả các thao tác với cơ sở dữ liệu. Lớp này không được sử dụng trực tiếp mà sẽ được thực hiện qua các lớp *_BUS như trong <see cref="BusinessController"/>
    /// </summary>
    public class DBBase
    {
        /// <summary>
        /// 
        /// </summary>
        private String pConnectionString;
        /// <summary>
        /// 
        /// </summary>
        private SqlConnection conn;
        /// <summary>
        /// 
        /// </summary>
        private SqlTransaction tran = null;
        /// <summary>
        /// 
        /// </summary>
        private bool isOpen = false;
        private bool isTran = false;
        public __error _er = new __error();
        //public DBBase()
        //{
        //    conn = new SqlConnection();
        //}
        /// <summary>
        /// Initializes a new instance of the <see cref="DBBase" /> class.
        /// </summary>
        /// <param name="sConnectionString">The s connection string.</param>
        public DBBase(string sConnectionString)
        {
            pConnectionString = sConnectionString;
            conn = new SqlConnection(pConnectionString);
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="DBBase" /> class.
        /// </summary>
        /// <param name="server">The server.</param>
        /// <param name="data">The data.</param>
        /// <param name="user">The user.</param>
        /// <param name="password">The password.</param>
        public DBBase(string server, string data, string user, string password)
        {
            SqlConnectionStringBuilder a = new SqlConnectionStringBuilder();
            a.DataSource = server;
            a.UserID = user;
            a.Password = password;
            a.InitialCatalog = data;
            pConnectionString = a.ConnectionString;
            conn = new SqlConnection(pConnectionString);
            a = null;

        }
        /// <summary>
        /// Initializes a new instance of the <see cref="DBBase" /> class.
        /// </summary>
        /// <param name="server">The server.</param>
        /// <param name="data">The data.</param>
        public DBBase(string server, string data)
        {
            //for windows authentication
            SqlConnectionStringBuilder a = new SqlConnectionStringBuilder();
            a.DataSource = server;
            a.InitialCatalog = data;
            a.IntegratedSecurity = true;
            pConnectionString = a.ConnectionString;
            conn = new SqlConnection(pConnectionString);
            a = null;

        }
        /// <summary>
        /// Sets the connection.
        /// </summary>
        /// <param name="_conn">The _conn.</param>
        public void setConnection(SqlConnection _conn)
        {
            this.conn = _conn;
            isOpen = false;//don't close it when dispose this object
        }
        /// <summary>
        /// Finalizes an instance of the <see cref="DBBase" /> class.
        /// </summary>
        ~DBBase()
        {
            if (isOpen)
            {
                if (conn.State != System.Data.ConnectionState.Closed)
                {
                    try
                    {
                        conn.Close();
                    }
                    catch (Exception e)
                    {
                        _er.setError(e);
                        er.setError(e);
                    }
                }

                conn.Dispose();
            }
        }
        /// <summary>
        /// Gets the connection.
        /// </summary>
        /// <value>The connection.</value>
        public SqlConnection Connection
        {
            get { return conn; }
        }
        /// <summary>
        /// Gets or sets the connection string.
        /// </summary>
        /// <value>The connection string.</value>
        public string ConnectionString
        {
            get { return pConnectionString; }
            set
            {
                pConnectionString = value;
                conn.ConnectionString = pConnectionString;
            }
        }
        /// <summary>
        /// Builds the connection string.
        /// </summary>
        /// <param name="server">The server.</param>
        /// <param name="data">The data.</param>
        public void BuildConnectionString(string server, string data)
        {
            SqlConnectionStringBuilder a = new SqlConnectionStringBuilder();
            a.DataSource = server;
            a.InitialCatalog = data;
            a.IntegratedSecurity = true;
            pConnectionString = a.ConnectionString;
            conn.ConnectionString = pConnectionString;
            a = null;

        }
        /// <summary>
        /// Builds the connection string.
        /// </summary>
        /// <param name="server">The server.</param>
        /// <param name="data">The data.</param>
        /// <param name="user">The user.</param>
        /// <param name="password">The password.</param>
        public void BuildConnectionString(string server, string data, string user, string password)
        {
            SqlConnectionStringBuilder a = new SqlConnectionStringBuilder();
            a.DataSource = server;
            a.UserID = user;
            a.Password = password;
            a.InitialCatalog = data;
            pConnectionString = a.ConnectionString;
            conn.ConnectionString = pConnectionString;
            a = null;

        }
        /// <summary>
        /// Gets a value indicating whether this <see cref="DBBase" /> is connected.
        /// </summary>
        /// <value><c>true</c> if connected; otherwise, <c>false</c>.</value>
        public bool Connected
        {
            get { return (conn.State == ConnectionState.Open); }
        }
        //kết nối đến server
        /// <summary>
        /// Opens this instance.
        /// </summary>
        /// <returns></returns>
        public int Open()
        {
            if (conn.State == ConnectionState.Broken || conn.State ==ConnectionState.Connecting)
                conn.Close();
            try
            {
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                    isOpen = true;
                }
                return 0;
            }
            catch (Exception e)
            {
                _er.setError(e);
                er.setError(e);
                return -1;
            }
        }
        //đóng kết nối đến server
        /// <summary>
        /// Closes this instance.
        /// </summary>
        public void Close()
        {
            if (isOpen)
            {
                try
                {
                    if (conn.State != ConnectionState.Closed)
                        conn.Close();
                }
                catch (Exception e)
                {
                    _er.setError(e);
                    er.setError(e);
                    string error = e.Message;
                }
            }

        }
        /// <summary>
        /// Gán transaction trong SQL cho đối tượng này để đảm bảo nhiều đối tượng có thể thực hiện được cùng một phiên dữ liệu
        /// </summary>
        /// <param name="pTran"></param>
        public void setTran(SqlTransaction pTran)
        {
            tran = pTran;
            isTran = false;
        }

        //Thực hiện một command đã có sẵn, với kết nối hiện hiện tại
        /// <summary>
        /// Does the command.
        /// </summary>
        /// <param name="cm">The cm.</param>
        /// <returns>Negative value: error, else number of rows affected</returns>
        public int doCommand(ref SqlCommand cm)
        {
            if (!Connected)
            {
                Open();
                if (!Connected)
                {
                    return -1;
                }
            }
            cm.Connection = conn;
            cm.Transaction = tran;
            try
            {
                cm.CommandTimeout = 300;
                int i = cm.ExecuteNonQuery();
                return 0;
            }
            catch (Exception e)
            {
                _er.setError(e);
                er.setError(e);
                return -2;
            }
        }
        //Thực hiện một câu lệnh SQL
        /// <summary>
        /// Does the SQL.
        /// </summary>
        /// <param name="SQL">The SQL.</param>
        /// <returns>Negative value: error, else number of rows affected</returns>
        public int doSQL(string SQL)
        {
            if (!Connected)
            {
                Open();
                if (!Connected)
                {
                    return -1;
                }
            }
            SqlCommand cm = new SqlCommand(SQL);
            cm.Connection = conn;
            cm.CommandType = CommandType.Text;
            cm.Transaction = tran;
            try
            {
                int i = cm.ExecuteNonQuery();
                return 0;
            }
            catch (Exception e)
            {
                _er.setError(e);
                er.setError(e);
                return -2;
            }

        }
        //Lấy dữ liệu thông qua một câu lệnh SQL
        /// <summary>
        /// Gets the SQL.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <param name="sTableName">Name of the s table.</param>
        /// <param name="SQL">The SQL.</param>
        /// <returns>-1: error, else number of record returned</returns>
        public int getSQL(ref DataSet ds, string sTableName, string SQL)
        {
            if (!Connected)
            {
                Open();
                if (!Connected)
                {
                    return -1;
                }
            }
            SqlCommand cm = new SqlCommand(SQL);
            cm.Connection = conn;
            cm.CommandType = System.Data.CommandType.Text;
            cm.Transaction = tran;
            SqlDataAdapter da = new SqlDataAdapter(cm);
            try
            {
                return da.Fill(ds, sTableName);
            }
            catch (Exception e)
            {
                _er.setError(e);
                er.setError(e);
                return -2;
            }

        }

        //Lấy dữ liệu về thông qua DataReader và một câu lệnh SQL
        /// <summary>
        /// Gets the SQL.
        /// </summary>
        /// <param name="SQL">The SQL.</param>
        /// <returns>null for error, else the result</returns>
        public SqlDataReader getSQL(string SQL)
        {
            if (!Connected)
            {
                Open();
                if (!Connected)
                {
                    return null;
                }
            }
            SqlCommand cm = new SqlCommand(SQL);
            cm.Connection = conn;
            cm.CommandType = System.Data.CommandType.Text;
            try
            {
                return cm.ExecuteReader();
            }
            catch (Exception e)
            {
                _er.setError(e);
                er.setError(e);
                return null;
            }

        }
        //Lấy dữ liệu về thông qua datatable và một câu lệnh SQL
        /// <summary>
        /// Gets the SQL.
        /// </summary>
        /// <param name="SQL">The SQL.</param>
        /// <returns>null for error, else the result</returns>
        public DataTable getSQLTable(string SQL)
        {
            if (!Connected)
            {
                Open();
                if (!Connected)
                {
                    return null;
                }
            }
            SqlCommand cm = new SqlCommand(SQL);
            cm.Connection = conn;
            cm.CommandType = System.Data.CommandType.Text;
            SqlDataAdapter da = new SqlDataAdapter();
            DataTable dt = new DataTable();
            da.SelectCommand = cm;
            try
            {
                da.Fill(dt);
                cm.Dispose();
                da.Dispose();
                return dt;
            }
            catch (Exception e)
            {
                _er.setError(e);
                er.setError(e);
                cm.Dispose();
                dt.Dispose();
                da.Dispose();
                return null;
            }

        }

        //Lấy dữ liệu thông qua một command
        /// <summary>
        /// Gets the command.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <param name="sTableName">Name of the s table.</param>
        /// <param name="cm">The cm.</param>
        /// <returns></returns>
        public int getCommand(ref System.Data.DataSet ds, string sTableName, SqlCommand cm)
        {
            SqlDataAdapter da = new SqlDataAdapter();
            if (!Connected)
            {
                Open();
                if (!Connected)
                {
                    return -1;
                }
            }
            cm.Connection = conn;
            cm.Transaction = tran;
            da.SelectCommand = cm;
            try
            {
                if (ds.Tables.Contains(sTableName))
                {
                    ds.Tables.Remove(sTableName);
                }
                return da.Fill(ds, sTableName);
            }
            catch (Exception e)
            {
                _er.setError(e);
                er.setError(e);
                return -2;
            }
        }

        //Kiểu lấy về là datareader thông qua một command
        /// <summary>
        /// Gets the command.
        /// </summary>
        /// <param name="cm">The cm.</param>
        /// <returns></returns>
        public SqlDataReader getCommand(SqlCommand cm)
        {
            if (!Connected)
            {
                Open();
                if (!Connected)
                {
                    return null;
                }
            }
            cm.Transaction = tran;
            cm.Connection = conn;
            try
            {
                return  cm.ExecuteReader();
            }
            catch (Exception e)
            {
                _er.setError(e);
                er.setError(e);
                return null;
            }
        }
        //Kiểu lấy về là datareader thông qua một command
        /// <summary>
        /// Gets the command table.
        /// </summary>
        /// <param name="cm">The cm.</param>
        /// <returns></returns>
        public DataTable getCommandTable(SqlCommand cm)
        {
            if (!Connected)
            {
                Open();
                if (!Connected)
                {
                    return null;
                }
            }
            cm.Transaction = tran;
            cm.Connection = conn;
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cm;
            try
            {
                da.Fill(dt) ;
                da.Dispose();
                return dt;
            }
            catch (Exception e)
            {
                _er.setError(e);
                er.setError(e);
                dt.Dispose();
                da.Dispose();
                return null;
            }
        }

        /// <summary>
        /// Create the transaction for current connection.
        /// </summary>
        /// <returns></returns>
        public int beginTran()
        {
            if (tran != null)
            {
                return -1;
            }
            if (Open() != 0)
            {
                return -2;
            }
            try
            {
                tran = conn.BeginTransaction();
                isTran = true;
            }
            catch (Exception e)
            {
                _er.setError(e);
                er.setError(e);
                return -3;
            }
            return 0;
        }
        /// <summary>
        /// Commits the current transaction.
        /// </summary>
        /// <returns></returns>
        public int commit()
        {
            if (tran == null)
            {
                return -1;
            }
            if (isTran)
            {
                try
                {
                    tran.Commit();
                    tran = null;
                    isTran = false;
                }
                catch (Exception e)
                {
                    _er.setError(e);
                    er.setError(e);
                    tran = null;
                    isTran = false;
                    return -3;
                }
            }
            return 0;
        }
        /// <summary>
        /// Rollbace the current transaction.
        /// </summary>
        /// <returns></returns>
        public int rollback()
        {
            if (tran == null)
            {
                return -1;
            }
            if (isTran)
            {
                try
                {
                    tran.Rollback();
                    tran = null;
                    isTran = false;
                }
                catch (Exception e)
                {
                    tran = null;
                    isTran = false;
                    _er.setError(e);
                    er.setError(e);
                    return -3;
                }

            }
            return 0;
        }
        /// <summary>
        /// get current transation
        /// </summary>
        /// <returns></returns>
        public SqlTransaction getTran()
        {
            return tran;
        }
        /// <summary>
        /// Gets the page.
        /// </summary>
        /// <param name="sql">The SQL.</param>
        /// <param name="startRecord">The start record.</param>
        /// <param name="rowPerPage">The row per page.</param>
        /// <returns></returns>
        public DataTable  getPage(string sql, int startRecord, int rowPerPage)
        {
            SqlDataAdapter da = new SqlDataAdapter();
            if (!Connected)
            {
                Open();
                if (!Connected)
                {
                    return null;
                }
            }
            SqlCommand cm = new SqlCommand(sql);
            cm.Connection = conn;
            cm.Transaction = tran;
            da.SelectCommand = cm;
            DataSet ds = new DataSet();
            int ret;
            try
            {
                ret =da.Fill(ds, startRecord,rowPerPage,"table1");
            }
            catch (Exception e)
            {
                _er.setError(e);
                er.setError(e);
                return null;
            }
            DataTable dt = ds.Tables["table1"].Clone();
            cm.Dispose();
            ds.Dispose();
            da.Dispose();
            return dt;
        }
        /// <summary>
        /// Gets the page.
        /// </summary>
        /// <param name="cm">The cm.</param>
        /// <param name="startRecord">The start record.</param>
        /// <param name="rowPerPage">The row per page.</param>
        /// <returns></returns>
        public DataTable getPage(ref SqlCommand cm, int startRecord, int rowPerPage)
        {
            SqlDataAdapter da = new SqlDataAdapter();
            if (!Connected)
            {
                Open();
                if (!Connected)
                {
                    return null;
                }
            }
            cm.Connection = conn;
            cm.Transaction = tran;
            da.SelectCommand = cm;
            DataSet ds = new DataSet();
            int ret;
            try
            {
                ret = da.Fill(ds, startRecord, rowPerPage, "table1");
            }
            catch (Exception e)
            {
                _er.setError(e);
                er.setError(e);
                return null;
            }
            DataTable dt = ds.Tables["table1"].Clone();
            cm.Dispose();
            ds.Dispose();
            da.Dispose();
            return dt;
        }
        #region old version
        /// <summary>
        /// Get dataset from sql command
        /// </summary>
        /// <param name="ds">Dataset to hold the data</param>
        /// <param name="sTableName">The table hold data</param>
        /// <param name="cm">Command to get data</param>
        /// <returns>Return >=0 is ok, else negative value</returns>
        public int getStoreProcedure(ref System.Data.DataSet ds, string sTableName, SqlCommand cm)
        {
            SqlDataAdapter da = new SqlDataAdapter();
            if (!Connected)
            {
                Open();
                if (!Connected)
                {
                    return -1;
                }
            }
            cm.Connection = conn;
            cm.Transaction = tran;
            da.SelectCommand = cm;
            try
            {
                if (ds.Tables.Contains(sTableName))
                {
                    ds.Tables.Remove(sTableName);
                }
                return da.Fill(ds, sTableName);
            }
            catch (Exception e)
            {
                _er.setError(e);
                er.setError(e);
                return -2;
            }
        }

        //Kiểu lấy về là datareader thông qua một command
        /// <summary>
        /// Get data to sql datareader
        /// </summary>
        /// <param name="cm">Sql command to get data</param>
        /// <returns>Return positive ok, else negative</returns>
        public SqlDataReader getStoreProcedure(SqlCommand cm)
        {
            if (!Connected)
            {
                Open();
                if (!Connected)
                {
                    return null;
                }
            }
            cm.Transaction = tran;
            cm.Connection = conn;
            try
            {
                int i = cm.ExecuteNonQuery();
                return null;
            }
            catch (Exception e)
            {
                _er.setError(e);
                er.setError(e);
                return null;
            }
        }

        //Lấy dữ liệu và kiểm tra được trạng thái của command
        /// <summary>
        /// Gets the store procedure.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <param name="sTableName">Name of the s table.</param>
        /// <param name="cm">The cm.</param>
        /// <returns>
        /// Negative value means error, else the number of records
        /// </returns>
        public int getStoreProcedure(ref System.Data.DataSet ds, string sTableName, ref SqlCommand cm)
        {
            SqlDataAdapter da = new SqlDataAdapter();
            if (!Connected)
            {
                Open();
                if (!Connected)
                {
                    return -1;
                }
            }
            cm.Connection = conn;
            cm.Transaction = tran;
            da.SelectCommand = cm;
            try
            {
                if (ds.Tables.Contains(sTableName))
                {
                    ds.Tables.Remove(sTableName);
                }
                return da.Fill(ds, sTableName);
            }
            catch (Exception e)
            {
                _er.setError(e);
                er.setError(e);
                return -2;
            }
            return 0;
        }

        //Lấy dữ liệu và kiểm tra được trạng thái của command
        /// <summary>
        /// Gets the store procedure.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <param name="cm">The cm.</param>
        /// <returns>
        /// Negative value means error, else the number of records
        /// </returns>
        public int getStoreProcedure(ref System.Data.DataSet ds, ref SqlCommand cm)
        {
            SqlDataAdapter da = new SqlDataAdapter();
            if (!Connected)
            {
                Open();
                if (!Connected)
                {
                    return -1;
                }
            }
            cm.Connection = conn;
            cm.Transaction = tran;
            da.SelectCommand = cm;
            try
            {
                return da.Fill(ds);
            }
            catch (Exception e)
            {
                _er.setError(e);
                er.setError(e);
                return -2;
            }
            return 0;
        }

        //Lấy dữ liệu thông qua một stored procedure và danh sách các tham số đầu vào được lưu trong mảng
        /// <summary>
        /// Gets the store procedure.
        /// </summary>
        /// <param name="SPName">Name of the SP.</param>
        /// <param name="Parameters">The parameters.</param>
        /// <param name="ds">The ds.</param>
        /// <returns>Negative value means error, else the number of records</returns>
        public int getStoreProcedure(string SPName, ref ArrayList Parameters, ref DataSet ds)
        {
            //            DataSet dsResult = new DataSet();
            if (!Connected)
            {
                Open();
                if (!Connected)
                {
                    return -1;
                }
            }
            SqlCommand cm = new SqlCommand(SPName, conn);
            cm.CommandType = CommandType.StoredProcedure;
            cm.Transaction = tran;
            if (Parameters != null)
            {
                for (int i = 0; i < Parameters.Count; i++)
                {
                    cm.Parameters.Add(Parameters[i]);
                }
            }

            SqlDataAdapter da = new SqlDataAdapter(cm);
            try
            {
                return da.Fill(ds);
            }
            catch (Exception e)
            {
                _er.setError(e);
                er.setError(e);
                return -2;
            }
        }

        //Lấy dữ liệu thông qua một stored procedure và danh sách các tham số đầu vào được lưu trong mảng
        /// <summary>
        /// Gets the store procedure.
        /// </summary>
        /// <param name="SPName">Name of the SP.</param>
        /// <param name="Parameters">The parameters.</param>
        /// <param name="ds">The ds.</param>
        /// <param name="sTableName">Name of the s table.</param>
        /// <returns>Negative value means error, else the number of records</returns>
        public int getStoreProcedure(string SPName, ref ArrayList Parameters, ref DataSet ds, string sTableName)
        {
            //            DataSet dsResult = new DataSet();
            if (!Connected)
            {
                Open();
                if (!Connected)
                {
                    return -1;
                }
            }
            SqlCommand cm = new SqlCommand(SPName, conn);
            cm.CommandType = CommandType.StoredProcedure;
            cm.Transaction = tran;
            if (Parameters != null)
            {
                for (int i = 0; i < Parameters.Count; i++)
                {
                    cm.Parameters.Add(Parameters[i]);
                }
            }

            SqlDataAdapter da = new SqlDataAdapter(cm);
            try
            {
                return da.Fill(ds, sTableName);
            }
            catch (Exception e)
            {
                _er.setError(e);
                er.setError(e);
                return -2;
            }
        }
        //Chỉ thực hiện stored procedure, update, insert, delete
        /// <summary>
        /// Does the store procedure.
        /// </summary>
        /// <param name="SPName">Name of the SP.</param>
        /// <param name="Parameters">The parameters.</param>
        /// <returns>Negative value: error, else number of rows affected</returns>
        public int doStoreProcedure(string SPName, ref ArrayList Parameters)
        {
            //            DataSet dsResult = new DataSet();
            if (!Connected)
            {
                Open();
                if (!Connected)
                {
                    return -1;
                }
            }
            SqlCommand cm = new SqlCommand(SPName, conn);
            cm.CommandType = CommandType.StoredProcedure;
            cm.Transaction = tran;
            if (Parameters != null)
            {
                for (int i = 0; i < Parameters.Count; i++)
                {
                    cm.Parameters.Add(Parameters[i]);
                }
            }

            try
            {
                return cm.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                _er.setError(e);
                er.setError(e);
                return -2;
            }
        }
        //Thực hiện một stored procedure, chỉ có một tham số trả về, và tham số này phải là cuối cùng trong dãy tham số
        /// <summary>
        /// Does the store procedure.
        /// </summary>
        /// <param name="spName">Name of the sp.</param>
        /// <param name="retPara">The ret para.</param>
        /// <param name="para">The para.</param>
        /// <returns>Negative value: error, else number of rows affected</returns>
        public int doStoreProcedure(string spName, ref spParam retPara, params spParam[] para)
        {
            int i;
            if (!Connected)
            {
                Open();
                if (!Connected)
                {
                    return -1;
                }
            }
            SqlCommand cm = conn.CreateCommand();
            cm.Connection = conn;
            cm.CommandType = CommandType.StoredProcedure;
            cm.CommandText = spName;
            cm.Transaction = tran;
            for (i = 0; i < para.Length; i++)
            {
                cm.Parameters.Add(para[i].name, para[i].type);
                if ((para[i].direction == ParameterDirection.Input) || para[i].direction == ParameterDirection.InputOutput)
                {
                    cm.Parameters[para[i].name].Value = para[i].data;
                }
                if (para[i].size > 0)
                {
                    cm.Parameters[para[i].name].Size = para[i].size;
                }
                cm.Parameters[para[i].name].Direction = para[i].direction;
            }
            try
            {
                cm.Parameters.Add(retPara.name, retPara.type, retPara.size);
                cm.Parameters[retPara.name].Direction = retPara.direction;
                i = cm.ExecuteNonQuery();
                retPara.data = cm.Parameters[retPara.name].Value;
                return 0;
            }
            catch (Exception e)
            {
                _er.setError(e);
                er.setError(e);
                return -1;
            }
        }
        //delete, insert, update
        /// <summary>doStoreProcedure
        /// Does the store procedure.
        /// </summary>
        /// <param name="spName">Name of the sp.</param>
        /// <param name="para">The para.</param>
        /// <returns>Negative value: error, else number of rows affected</returns>
        public int doStoreProcedure(string spName, params spParam[] para)
        {
            int i;
            if (!Connected)
            {
                Open();
                if (!Connected)
                {
                    return -1;
                }
            }
            SqlCommand cm = conn.CreateCommand();
            cm.Connection = conn;
            cm.CommandType = CommandType.StoredProcedure;
            cm.CommandText = spName;
            cm.Transaction = tran;
            for (i = 0; i < para.Length; i++)
            {
                cm.Parameters.Add(para[i].name, para[i].type);
                if ((para[i].direction == ParameterDirection.Input) || para[i].direction == ParameterDirection.InputOutput)
                {
                    cm.Parameters[para[i].name].Value = para[i].data;
                }
                if (para[i].size > 0)
                {
                    cm.Parameters[para[i].name].Size = para[i].size;
                }
                cm.Parameters[para[i].name].Direction = para[i].direction;
            }
            try
            {
                i = cm.ExecuteNonQuery();
                return 0;
            }
            catch (Exception e)
            {
                _er.setError(e);
                er.setError(e);
                return -1;
            }

        }
        //Lấy dữ liệu đưa về bảng
        /// <summary>
        /// Gets the store procedure.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <param name="tbName">Name of the tb.</param>
        /// <param name="storePocedureName">Name of the store pocedure.</param>
        /// <param name="para">The para.</param>
        /// <returns>
        /// negative value means error, else number of records
        /// </returns>
        public int getStoreProcedure(ref DataSet ds, string tbName, string storePocedureName, params spParam[] para)
        {
            int i;
            if (!Connected)
            {
                Open();
                if (!Connected)
                {
                    return -1;
                }
            }
            SqlDataAdapter da = new SqlDataAdapter();
            SqlCommand cm = conn.CreateCommand();
            cm.Connection = conn;
            cm.CommandType = CommandType.StoredProcedure;
            cm.CommandText = storePocedureName;
            cm.Transaction = tran;
            for (i = 0; i < para.Length; i++)
            {
                cm.Parameters.Add(para[i].name, para[i].type);
                if ((para[i].direction == ParameterDirection.Input) || para[i].direction == ParameterDirection.InputOutput)
                {
                    cm.Parameters[para[i].name].Value = para[i].data;
                }
                if (para[i].size > 0)
                {
                    cm.Parameters[para[i].name].Size = para[i].size;
                }
                cm.Parameters[para[i].name].Direction = para[i].direction;
            }
            try
            {
                if (ds.Tables.Contains(tbName))
                {
                    ds.Tables.Remove(tbName);
                }
                da.SelectCommand = cm;
                i = da.Fill(ds, tbName);
                return i;
            }
            catch (Exception e)
            {
                _er.setError(e);
                er.setError(e);
                return -2;
            }

        }

        /// <summary>
        /// Gets the data set.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <param name="sSQL">The s SQL.</param>
        /// <param name="sName">Name of the s.</param>
        /// <returns>
        /// negative value : error, else return number of rows returned
        /// </returns>
        public int getDataSet(ref DataSet ds, string sSQL, string sName)
        {
            //string sSQL: SQL command to get data
            //string sName: Name of table
            //Return: new data set hold the table
            SqlDataAdapter adapt = new SqlDataAdapter();
            if (!Connected)
            {
                Open();
                if (!Connected)
                {
                    return -1;
                }
            }
            adapt = new SqlDataAdapter();
            adapt.SelectCommand = new SqlCommand(sSQL, conn, tran);
            try
            {
                if (ds.Tables.Contains(sName))
                {
                    ds.Tables.Remove(sName);
                }
                return adapt.Fill(ds, sName);
            }
            catch (Exception e)
            {
                _er.setError(e);
                er.setError(e);
                return -1;
            }
            return 0;
        }

        /// <summary>
        /// Function get Code from stored procedure [genCode] - Created by VuLM - 03.10.2011
        /// </summary>
        /// <param name="strTableName">Table Name</param>
        /// <param name="strField">Name of code field</param>
        /// <param name="strLang">Portal language</param>
        /// <returns>Code</returns>
        public string getGenCodeValue(string strTableName, string strField, string strLang)
        {
            if (!Connected)
            {
                Open();
                if (!Connected)
                {
                    return "-1";
                }
            }
            SqlCommand cm = new SqlCommand();
            cm.Connection = conn;
            cm.CommandType = CommandType.StoredProcedure;
            cm.Transaction = tran;
            cm.CommandText = "genCode";

            cm.Parameters.Add("@code", SqlDbType.NVarChar).Value = "";
            cm.Parameters.Add("@table", SqlDbType.NVarChar).Value = strTableName;
            cm.Parameters.Add("@fieldname", SqlDbType.VarChar).Value = strField;
            cm.Parameters.Add("@lang", SqlDbType.VarChar).Value = strLang;
            cm.Parameters.Add("@e", SqlDbType.VarChar).Value = "";

            cm.Parameters["@code"].Direction = ParameterDirection.Input;
            cm.Parameters["@table"].Direction = ParameterDirection.Input;
            cm.Parameters["@fieldname"].Direction = ParameterDirection.Input;
            cm.Parameters["@lang"].Direction = ParameterDirection.Input;
            cm.Parameters["@e"].Direction = ParameterDirection.InputOutput;
            cm.Parameters["@e"].Size = 10;


            cm.ExecuteNonQuery();

            return Convert.ToString(cm.Parameters["@e"].Value);
        }
        //Lấy dữ liệu thông qua stored và datareader
        /// <summary>
        /// Get data to data reader
        /// </summary>
        /// <param name="storePocedureName"></param>
        /// <param name="para"></param>
        /// <returns></returns>
        public SqlDataReader getDataReader(string storePocedureName, params spParam[] para)
        {
            int i;
            if (!Connected)
            {
                Open();
                if (!Connected)
                {
                    return null;
                }
            }
            SqlCommand cm = conn.CreateCommand();
            cm.Connection = conn;
            cm.CommandType = CommandType.StoredProcedure;
            cm.CommandText = storePocedureName;
            cm.Transaction = tran;
            for (i = 0; i < para.Length; i++)
            {
                cm.Parameters.Add(para[i].name, para[i].type);
                if ((para[i].direction == ParameterDirection.Input) || para[i].direction == ParameterDirection.InputOutput)
                {
                    cm.Parameters[para[i].name].Value = para[i].data;
                }
                if (para[i].size > 0)
                {
                    cm.Parameters[para[i].name].Size = para[i].size;
                }
                cm.Parameters[para[i].name].Direction = para[i].direction;
            }
            try
            {
                return cm.ExecuteReader();
            }
            catch (Exception e)
            {
                _er.setError(e);
                er.setError(e);
                return null;
            }

        }

        #endregion

    }
}
