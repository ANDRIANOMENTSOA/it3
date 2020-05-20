using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using System.IO;

namespace WebApplication1.Models
{
    public class ConnexionSQLServer
    {
        

        public class DbConnection
        {

            public string Path = "";  //  System.AppDomain.s + "\\Configfile.cnt";  DESKTOP-K56R42H
            public string SERVER = "DESKTOP-O0K2BQJ\\SA";
            public string DATABASE = "OnsiteBiatss_KK";
            public string USER = "sa";
            public string PASSWORD = "1234";
           // public string pbConnectionString = "Server=DESKTOP-O0K2BQJ\\SA;Database=OnsiteBiatss_KK;User Id=sa; Password=1234";
            //public string pbConnectionString = "Server=.\\RELATE;Database=OnsiteBiatss_KK;User Id=sa; Password=1234";
            //public string pbConnectionString = "Server=DESKTOP-7HJUR50;Database=OnsiteBiatss_KK;User Id=sa; Password=1234";
            //public string pbConnectionString = "Server=DESKTOP-K56R42H;Database=OnsiteBiatss_KK;User Id=sa; Password=1234";
            //public string pbConnectionString = "Server=DESKTOP-CGR76E3;Database=OnsiteBiatss_KK;User Id=sa; Password=1234";
           // public string pbConnectionString = "Server=DESKTOP-O0K2BQJ\\SA;Database=OnsiteBiatss_KK;User Id=sa; Password=1234";
            public string pbConnectionString = "Server=DESKTOP-Q821GFS;Database=OnsiteBiatss_KK;User Id=sa; Password=1234";
           // public string pbConnectionString = "Server=DESKTOP-54APPF8\\SYMPHONY2;Database=OnsiteBiatss_KK;User Id=sa; Password=1234";


            public SqlConnection conn;
            public string ConnectionString()
            {
                string ConnString = pbConnectionString;

                return ConnString;
            }
            public DataSet RetObjDS(string Sql)
            {
                DataSet ds = new DataSet();
                SqlConnection con = new SqlConnection(pbConnectionString);
                con.Open();
                SqlDataAdapter ada = new SqlDataAdapter(Sql, con);
                ada.Fill(ds);
                con.Close();
                return ds;
            }

            public void DocumentLog(string AWBCarrier, string AWBSerialNumber, string AWBDocSerialNumber, string User, string Message)
            {

                string dataNow = DateTime.Now.ToString("yyyy-MMM-dd HH:mm:ss");
                User = conn.WorkstationId.ToString();
                
                string Sql = "DECLARE @MaxRecId bigint;" + Environment.NewLine;
                Sql = Sql + "set @MaxRecId = (select iif(MAX(RecId) is null,1, MAX(RecId)+1) As MaxLineid from AWB.DocumentLog) ;";
                Sql = Sql + "INSERT INTO AWB.DocumentLog Values(";
                Sql = Sql + "@MaxRecId" + Environment.NewLine;
                Sql = Sql + ",'" + AWBCarrier.Trim() + "'," + Environment.NewLine;
                Sql = Sql + "'" + AWBSerialNumber.Trim() + "'," + Environment.NewLine;
                Sql = Sql + "'" + AWBDocSerialNumber.Trim() + "'," + Environment.NewLine;
                Sql = Sql + "'" + Message.Trim() + "'," + Environment.NewLine;
                Sql = Sql + "'" + User.Trim() + "'," + Environment.NewLine;
                Sql = Sql + "'" + dataNow.Trim() + "'" + Environment.NewLine;
                Sql = Sql + ")";
                bool UpdFlg = DbUpdate(Sql);
            }
            #region  DB Operation
            //=================================================================== 
            public SqlDataReader GetRecords(string agsql)
            {

                SqlDataReader myReader;
                myReader = null;
                if (pbConnectionString != "")
                {
                    //Tsql working
                    try
                    {
                        DbClose();


                        SqlCommand command;
                        conn = new SqlConnection(ConnectionString());
                        conn.Open();

                        command = new SqlCommand();
                        string SqlSelect = agsql;
                        command.Connection = conn;
                        command.CommandText = SqlSelect;
                        command = new SqlCommand(SqlSelect, conn);
                        // command.ExecuteNonQuery();
                        myReader = command.ExecuteReader();
                        while (conn.State == ConnectionState.Executing)
                        {
                            // stay here until query is not completed
                        }
                        //Do not colse from here
                        // conn.Close();

                    }
                    catch (Exception e)
                    {
                        //  SendMessage(e.Message);
                        string Msg = e.Message;
                        conn.Close();
                    }

                }
                return myReader;
            }
            public SqlDataReader GetData(string agsql)
            {
                SqlDataReader myReader;
                myReader = null;
                //Tsql working

                try
                {
                    DbClose();


                    SqlCommand command;
                    conn = new SqlConnection(ConnectionString());
                    conn.Open();

                    command = new SqlCommand();
                    string SqlSelect = agsql;
                    command.Connection = conn;
                    command.CommandText = SqlSelect;
                    command = new SqlCommand(SqlSelect, conn);
                    // command.ExecuteNonQuery();
                    myReader = command.ExecuteReader();
                    while (conn.State == ConnectionState.Executing)
                    {
                        // stay here until query is not completed
                    }
                    //Do not colse from here
                    // conn.Close();

                }
                catch (Exception e)
                {
                    //  SendMessage(e.Message);
                    string Msg = e.Message;
                    conn.Close();

                }
                return myReader;
            }


            public SqlDataReader Readers(string agsql)
            {
                SqlDataReader myReader;
                myReader = null;
                //Tsql working

                try
                {
                    SqlCommand command;
                    conn = new SqlConnection(ConnectionString());
                    conn.Open();

                    command = new SqlCommand();
                    string SqlSelect = agsql;
                    command.Connection = conn;
                    command.CommandText = SqlSelect;
                    command = new SqlCommand(SqlSelect, conn);
                    // command.ExecuteNonQuery();
                    myReader = command.ExecuteReader();
                    while (conn.State == ConnectionState.Executing)
                    {
                        // stay here until query is not completed
                    }
                    //Do not colse from here
                    // conn.Close();

                }
                catch (Exception e)
                {
                    //  SendMessage(e.Message);
                    string Msg = e.Message;
                    conn.Close();

                }
                return myReader;
            }


            public bool DbClose()
            {

                try
                {
                    conn.Close();
                    conn.Dispose();
                }
                catch { }



                try
                {
                    if (conn.State == ConnectionState.Open)
                    {
                        //opening the connection
                        conn.Close();
                        conn.Dispose();


                    }
                    return (true);
                }
                catch { return (false); }
            }
            public bool DbUpdate(string agSql)
            {


                // EXEC SP_CONFIGURE 'remote query timeout', 1800
                // reconfigure
                //EXEC sp_configure


                //EXEC SP_CONFIGURE 'show advanced options', 1
                // reconfigure
                //EXEC sp_configure


                //EXEC SP_CONFIGURE 'remote query timeout', 1800
                // reconfigure
                //EXEC sp_configure
                // DatabaseError = 0;
                bool RetVal = false;
                try
                {
                    // label25.Text = " UPDATING DATABASE....";
                    SqlCommand command;
                    conn = new SqlConnection(ConnectionString());
                    conn.Open();


                    command = new SqlCommand();
                    string SqlSelect = agSql;
                    command.Connection = conn;
                    command.CommandText = SqlSelect;
                    command.CommandTimeout = 1800;
                    command = new SqlCommand(SqlSelect, conn);
                    command.ExecuteNonQuery();
                    //myReader = command.ExecuteReader();
                    while (conn.State == ConnectionState.Executing)
                    {
                        // stay here until query is not completed
                    }
                    RetVal = true;
                    //Do not colse from here
                    conn.Close();
                    // DatabaseError = 0;
                    // label25.Text = "";
                }
                catch (Exception e)
                {
                    conn.Close();
                    // SendMessage(e.Message);
                    int Rtt = 0;
                    // label25.Text = "ERROR LOADING XML FILE INTO DATABASE :-" + e.Message;
                    Rtt = 0;
                    // Write Sql Statement To File --ERR
                    string msg = e.Message + Environment.NewLine;
                    msg = msg + e.StackTrace.ToString() + Environment.NewLine;
                    msg = msg + agSql + Environment.NewLine;

                    string dte = DateTime.Now.ToString("yyyyMMMdd HHssmm");
                    string path = "C:\\";
                    string path2 = path + "\\Temp";
                    string path3 = path2 + "\\" + dte.Trim() + ".XML.Err";
                    DirectoryInfo dir = new DirectoryInfo(path);
                    DirectoryInfo SYSTEMDATA = new DirectoryInfo(path2);
                    if (!SYSTEMDATA.Exists)
                    {
                        dir.CreateSubdirectory("TEMP");
                    }
                    string WkFile2 = path3;
                    System.IO.StreamWriter file2 = new System.IO.StreamWriter(WkFile2);
                    file2.WriteLine(msg);
                    file2.Close();
                    //label25.Text = "Error In Updating database. Error File:" + path3;
                    //DatabaseError = 1;
                    for (int i = 1; i < 6; i++) { System.Console.Beep(); }
                    Rtt++;
                }

                return RetVal;
            }



            public bool RetVal(string Sql)
            {
                bool RetVal = false;
                SqlConnection con = new SqlConnection(ConnectionString());

                con.Open();

                SqlCommand com = new SqlCommand(Sql, con);

                SqlDataReader read = com.ExecuteReader();
                if (read != null)
                {
                    if (read.HasRows)
                    {
                        read.Read();
                        RetVal = true;
                    }
                }
                read.Close();
                con.Close();
                return RetVal;
            }
            //==================================================================

            public bool Query(string Sql)
            {
                bool RetVal = false;
                SqlConnection con = new SqlConnection(ConnectionString());

                con.Open();

                SqlCommand com = new SqlCommand(Sql, con);

                SqlDataReader read = com.ExecuteReader();
                if (read != null)
                {
                    if (read.HasRows)
                    {
                        read.Read();
                        RetVal = true;
                    }
                }
                read.Close();
                con.Close();
                return RetVal;
            }

            public bool ExistQuery(string Sql)
            {
                bool RetVal = false;
                SqlConnection con = new SqlConnection(ConnectionString());


                con.Open();

                SqlCommand com = new SqlCommand(Sql, con);

                SqlDataReader read = com.ExecuteReader();
                if (read != null)
                {
                    if (read.HasRows)
                    {
                        read.Read();
                        RetVal = true;
                    }
                }
                read.Close();
                con.Close();
                return RetVal;
            }

            public bool GetValue(string Sql)
            {
                bool RetVal = false;
                SqlConnection con = new SqlConnection(ConnectionString());

                con.Open();

                SqlCommand com = new SqlCommand(Sql, con);

                SqlDataReader read = com.ExecuteReader();
                if (read != null)
                {
                    if (read.HasRows)
                    {
                        read.Read();
                        RetVal = true;
                    }
                }
                read.Close();
                con.Close();
                return RetVal;
            }
            //==================================================================
            #endregion
        }
    }
}