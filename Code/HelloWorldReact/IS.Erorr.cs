using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
namespace IS.Base
{
    /// <summary>
    /// Lớp phụ trách lưu lỗi của các thực thi cuối cùng, hỗ trợ cho quá trình debug khi lập trình
    /// </summary>
    public class __error
    {
        /// <summary>
        /// Thông báo lỗi
        /// </summary>
        public string Message = "";
        /// <summary>
        /// Dữ liệu lỗi
        /// </summary>
        public System.Collections.IDictionary Data = null;
        /// <summary>
        /// Nguồn lỗi
        /// </summary>
        public string Source = "";
        /// <summary>
        /// mã lỗi
        /// </summary>
        public int HashCode = 0;
        /// <summary>
        /// Thiế lập thông tin lỗi dựa trên các thông số tương ứng
        /// </summary>
        /// <param name="pSource"></param>
        /// <param name="pMessage"></param>
        /// <param name="pHashCode"></param>
        /// <param name="pData"></param>
        public void setError(string pSource, string pMessage, int pHashCode, System.Collections.IDictionary pData)
        {
            Message = pMessage;
            Source = pSource;
            HashCode = pHashCode;
            Data = pData;
        }
        /// <summary>
        /// Xóa bỏ thông tin lỗi hiện tại có
        /// </summary>
        public void clearError()
        {
            Message = "";
            Source = "";
            HashCode = 0;
            Data = null;
        }
        /// <summary>
        /// Thiết lập thông tin lỗi dựa trên Exception
        /// </summary>
        /// <param name="e">Thông tin cần đưa vào dựa trên bắt lỗi hiện tại</param>
        public void setError(Exception e)
        {
            Message = e.Message;
            Source = e.Source;
            HashCode = e.GetHashCode();
            Data = e.Data;

        }

    }
    /// <summary>
    /// Thiết lập thông số lỗi trên toàn bộ hệ thống, tương tự như trong mô tả của <see cref="__error"/>
    /// </summary>
    public static class er
    {

        public static string Message = "";
        public static System.Collections.IDictionary Data = null;
        public static string Source = "";
        public static int HashCode = 0;
        public static void setError(string pSource, string pMessage, int pHashCode, System.Collections.IDictionary pData)
        {
            Message = pMessage;
            Source = pSource;
            HashCode = pHashCode;
            Data = pData;
        }
        public static void clearError()
        {
            Message = "";
            Source = "";
            HashCode = 0;
            Data = null;
        }
        /// <summary>
        /// Thiết lập thông tin lỗi dựa trên Exception
        /// </summary>
        /// <param name="e">Thông tin cần đưa vào dựa trên bắt lỗi hiện tại</param>
        public static void setError(Exception e)
        {
            Message = e.Message;
            Source = e.Source;
            HashCode = e.GetHashCode();
            Data = e.Data;

        }
        /// <summary>
        /// Thiết lập lỗi thông qua lỗi cục bộ
        /// </summary>
        /// <param name="_er"></param>
        public static void setError(__error _er)
        {
            Message = _er.Message;
            Source = _er.Source;
            HashCode = _er.HashCode;
            Data = _er.Data;

        }
    }
    /// <summary>
    /// Thực hiện quá trình ghi log lỗi. Thực hiện cho các phiên bản ứng dụng desktop
    /// </summary>
    public class Errorlog
    {
        public static string fileName = "error.log";
        public static void raiseError(string source, string function)
        {
            StreamWriter SW;
            string s;
            try
            {
                if (File.Exists(Errorlog.fileName))
                {
                    SW = File.AppendText(Errorlog.fileName);
                }
                else
                {
                    SW = File.CreateText(Errorlog.fileName);
                }
                s = DateTime.Now.ToString("yyyy:mm:dd - HH:MM:ss");
                s += "|" + source + "|" + function + "|" + er.Message + "|" + er.Source + "|" + er.HashCode.ToString();
                SW.WriteLine(s);
                SW.Close();
            }
            catch
            {
            }

        }

        public static void raiseError(string source, string function, string Message, string errorSource, int HashCode)
        {
            StreamWriter SW;
            string s;
            try
            {
                if (File.Exists(Errorlog.fileName))
                {
                    SW = File.AppendText(Errorlog.fileName);
                }
                else
                {
                    SW = File.CreateText(Errorlog.fileName);
                }
                s = DateTime.Now.ToString("yyyy:mm:dd - HH:MM:ss");
                s += "|" + source + "|" + function + "|" + Message + "|" + errorSource + "|" + HashCode.ToString();
                SW.WriteLine(s);
                SW.Close();
            }
            catch
            {
            }
        }
        public static void raiseError(string source, string function, string Message, string errorSource, int HashCode, string thesource)
        {
            StreamWriter SW;
            string s;
            try
            {
                if (File.Exists(Errorlog.fileName))
                {
                    SW = File.AppendText(Errorlog.fileName);
                }
                else
                {
                    SW = File.CreateText(Errorlog.fileName);
                }
                s = DateTime.Now.ToString("yyyy:mm:dd - HH:MM:ss");
                s += "|" + source + "|" + function + "|" + Message + "|" + errorSource + "|" + HashCode.ToString() + "|" + thesource;
                SW.WriteLine(s);
                SW.Close();
            }
            catch
            {
            }
        }
        public static void raiseError(string source, string function, Exception e, string theSource)
        {
            StreamWriter SW;
            string s;
            try
            {
                if (File.Exists(Errorlog.fileName))
                {
                    SW = File.AppendText(Errorlog.fileName);
                }
                else
                {
                    SW = File.CreateText(Errorlog.fileName);
                }
                s = DateTime.Now.ToString("yyyy:mm:dd - HH:MM:ss");
                s += "|" + source + "|" + function + "|" + e.Message + "|" + e.Source + "|" + e.GetHashCode().ToString() + "|" + theSource;
                SW.WriteLine(s);
                SW.Close();
            }
            catch
            {
            }
        }
        public static void raiseError(string source, string function, Exception e)
        {
            StreamWriter SW;
            string s;
            try
            {
                if (File.Exists(Errorlog.fileName))
                {
                    SW = File.AppendText(Errorlog.fileName);
                }
                else
                {
                    SW = File.CreateText(Errorlog.fileName);
                }
                s = DateTime.Now.ToString("yyyy:mm:dd - HH:MM:ss");
                s += "|" + source + "|" + function + "|" + e.Message + "|" + e.Source + "|" + e.GetHashCode().ToString();
                SW.WriteLine(s);
                SW.Close();
            }
            catch
            {
            }
        }
        //public static void InsertLogError(string shortMess, string fullMess)
        //{

        //}
    }
}