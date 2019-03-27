using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;

namespace GTA.VTS.CustomersOrders.BLL
{
    /// /// <summary>
    /// 描述:读取Excel表格中数据
    /// 作者：叶振东
    /// 日期：2010-02-23
    /// </summary>
    public class OrderSQLHelper
    {
        /// <summary>
        /// 读取Excel表格数据,并返回DataSet集合
        /// </summary>
        /// <param name="Path">Excel表格文件路径</param>
        /// <returns>读取到的文件集合</returns>
        public DataSet dataSet(string Path)
        {
            string strCon = " Provider = Microsoft.Jet.OLEDB.4.0 ; Data Source = " + Path +
                               ";Extended Properties=Excel 8.0";
            OleDbConnection myConn = new OleDbConnection(strCon);
            string strCom = " SELECT * FROM [Sheet1$] ";
            myConn.Open();
            DataSet myDataSet = new DataSet();
            OleDbDataAdapter myCommand = new OleDbDataAdapter(strCom, myConn);
            myDataSet = new DataSet();
            myCommand.Fill(myDataSet, "[Sheet1$]");
            myConn.Close();
            return myDataSet;
        }
    }
}
