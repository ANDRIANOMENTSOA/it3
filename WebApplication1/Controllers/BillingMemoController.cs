using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core.Common.EntitySql;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using WebApplication1.Models;
using System.Globalization;
using System.IO;

namespace WebApplication1.Controllers
{
    public class BillingMemoController : Controller
    {

        //public string pbConnectionString = "Server=.\\RELATE;Database=OnsiteBiatss_KK;User Id=sa; Password=1234";
        //public string pbConnectionString = "Server=FANO-PC;Database=OnsiteBiatss_KK;User Id=so; Integrated Security=True";
         public string pbConnectionString = "Server=DESKTOP-Q821GFS;Database=OnsiteBiatss_KK;User Id=sa; Password=1234";
        //public string pbConnectionString = "Server=DESKTOP-7HJUR50;Database=OnsiteBiatss_KK;User Id=sa; Password=1234";
        //public string pbConnectionString = "Server=DESKTOP-CGR76E3;Database=OnsiteBiatss_KK;User Id=sa; Password=1234";
        //public string pbConnectionString = "Server=DESKTOP-K56R42H;Database=OnsiteBiatss_KK;User Id=sa; Password=1234";
        //public string pbConnectionString = "Server=DESKTOP-O0K2BQJ\\SA;Database=OnsiteBiatss_KK;User Id=sa; Password=1234";
        //public string pbConnectionString = "Server=DESKTOP-54APPF8\\SYMPHONY2;Database=OnsiteBiatss_KK;User Id=sa; Password=1234";
       // public string pbConnectionString = "Server=DESKTOP-DORH05Q;Database=OnsiteBiatss_KK;User Id=sa; Password=1234";

        ConnexionSQLServer.DbConnection dbconnect = new ConnexionSQLServer.DbConnection();

        private string _Doc = "";
        private string _Cp = "";

        private string PVCreditMemoNumber = "";
        private string PVInvoiceNumber = "";
        decimal zTot = 0;
        private string cboTo;

        private bool DbUpdate(string agSql)
        {
            SqlConnection conn = new SqlConnection(pbConnectionString);
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
                //label25.Text = " UPDATING DATABASE....";
                SqlCommand command;
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
                    // stay here until query is not compstringed
                }
                RetVal = true;
                //Do not colse from here
                conn.Close();
                // DatabaseError = 0;
                //  label25.Text = "";
            }
            catch (Exception e)
            {
                // SendMessage(e.Message);
                int Rtt = 0;
                // label25.Text = "ERROR LOADING XML FILE INTO DATABASE :-" + e.Message;

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
                // DatabaseError = 1;
                for (int i = 1; i < 6; i++) { System.Console.Beep(); }

            }

            return RetVal;
        }


        //Harentsoa
        //GET: /AirlineDetails/{id}
        public ActionResult AirlineDetailsFrom()
       {
            string ID = Request["fromValue"];

            String sql = "SELECT * FROM [Ref].[Airlines] WHERE [AirlineID] = '" + ID + "'";

            SqlConnection con = new SqlConnection(pbConnectionString);
            DataSet ds = new DataSet();
            SqlDataAdapter ada = new SqlDataAdapter(sql, con);

            ada.Fill(ds);
            int ligne = ds.Tables[0].Rows.Count;
            string[,] data = new string[15, ligne];
            int i = 0;
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                data[1, i] = dr[ds.Tables[0].Columns[1].ColumnName].ToString();
                    
                ViewBag.AirlineID = ID;
                ViewBag.AirlineCode = dr[ds.Tables[0].Columns[0].ColumnName].ToString();
                ViewBag.AirlineName = dr[ds.Tables[0].Columns[2].ColumnName].ToString();
                ViewBag.AirlineZone = dr[ds.Tables[0].Columns[3].ColumnName].ToString();
                ViewBag.AirlineStatus = dr[ds.Tables[0].Columns[4].ColumnName].ToString();
                ViewBag.AirlineCategory = dr[ds.Tables[0].Columns[5].ColumnName].ToString();
                ViewBag.AirlineAdressL1 = dr[ds.Tables[0].Columns[6].ColumnName].ToString();
                ViewBag.AirlineAdressL2 = dr[ds.Tables[0].Columns[7].ColumnName].ToString();
                ViewBag.AirlineAdressL3 = dr[ds.Tables[0].Columns[8].ColumnName].ToString();
                ViewBag.AirlineCountryName = dr[ds.Tables[0].Columns[9].ColumnName].ToString();
                ViewBag.AirlineCountryCode = dr[ds.Tables[0].Columns[10].ColumnName].ToString();
                ViewBag.AirlineCityName = dr[ds.Tables[0].Columns[11].ColumnName].ToString();
                ViewBag.AirlinePostalCode = dr[ds.Tables[0].Columns[12].ColumnName].ToString();
                ViewBag.AirlineICHMember = dr[ds.Tables[0].Columns[13].ColumnName].ToString();
                ViewBag.AirlineSettlementMethod = dr[ds.Tables[0].Columns[14].ColumnName].ToString();
            }

            con.Close();
            return PartialView("_AirlineDetails", ID);  
       }

        //Harentsoa
        //GET: /AirlineDetails/{id}
        public ActionResult AirlineDetailsTo()
        {
            string ID = Request["toValue"];

            String sql = "SELECT * FROM [Ref].[Airlines] WHERE [AirlineID] = '" + ID + "'";

            SqlConnection con = new SqlConnection(pbConnectionString);
            DataSet ds = new DataSet();
            SqlDataAdapter ada = new SqlDataAdapter(sql, con);

            ada.Fill(ds);
            int ligne = ds.Tables[0].Rows.Count;
            string[,] data = new string[15, ligne];
            int i = 0;
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                data[1, i] = dr[ds.Tables[0].Columns[1].ColumnName].ToString();

                ViewBag.AirlineID = ID;
                ViewBag.AirlineCode = dr[ds.Tables[0].Columns[0].ColumnName].ToString();
                ViewBag.AirlineName = dr[ds.Tables[0].Columns[2].ColumnName].ToString();
                ViewBag.AirlineZone = dr[ds.Tables[0].Columns[3].ColumnName].ToString();
                ViewBag.AirlineStatus = dr[ds.Tables[0].Columns[4].ColumnName].ToString();
                ViewBag.AirlineCategory = dr[ds.Tables[0].Columns[5].ColumnName].ToString();
                ViewBag.AirlineAdressL1 = dr[ds.Tables[0].Columns[6].ColumnName].ToString();
                ViewBag.AirlineAdressL2 = dr[ds.Tables[0].Columns[7].ColumnName].ToString();
                ViewBag.AirlineAdressL3 = dr[ds.Tables[0].Columns[8].ColumnName].ToString();
                ViewBag.AirlineCountryName = dr[ds.Tables[0].Columns[9].ColumnName].ToString();
                ViewBag.AirlineCountryCode = dr[ds.Tables[0].Columns[10].ColumnName].ToString();
                ViewBag.AirlineCityName = dr[ds.Tables[0].Columns[11].ColumnName].ToString();
                ViewBag.AirlinePostalCode = dr[ds.Tables[0].Columns[12].ColumnName].ToString();
                ViewBag.AirlineICHMember = dr[ds.Tables[0].Columns[13].ColumnName].ToString();
                ViewBag.AirlineSettlementMethod = dr[ds.Tables[0].Columns[14].ColumnName].ToString();
            }

            con.Close();
            return PartialView("_AirlineDetails", ID);
        }

        // Harentsoa
        // Get BilledAirline
        public ActionResult BilledAirline()
        {
            string airlineCode = Request["toValueForBilledAirline"];

            string Sql = "SELECT[AirlineID] +'-'+[AirlineName] as BilledAirline FROM[Ref].[Airlines]  where Airlinecode ='" + airlineCode + "'";

            SqlConnection con = new SqlConnection(pbConnectionString);
            DataSet ds = new DataSet();
            SqlDataAdapter ada = new SqlDataAdapter(Sql, con);

            ada.Fill(ds);
            con.Close();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {          
                ViewBag.nameFromAirlineName = dr[ds.Tables[0].Columns[0].ColumnName].ToString();
            }
           
            return PartialView("_BilledAirline", airlineCode);

        }

        //Harentsoa
        //GET: /DocDetailsSearch/{ID}
        [HttpGet]
        public ActionResult GetDocDetails()
        {

            string cpnNo = Request["CpnNo"];

            string sql = " SELECT DISTINCT CouponNumber, CouponStatus, UsageOriginCode, UsageDestinationCode,Carrier, FareBasisTicketDesignator, ReservationBookingDesignator" + Environment.NewLine;
            sql = sql + " ,UsageFlightNumber,UsageAirline, UsageDate, IsOAL" + Environment.NewLine;
            sql = sql + "FROM [Pax].[SalesDocumentCoupon]" + Environment.NewLine;
            sql = sql + " WHERE CouponNumber = '" + cpnNo + "'";
           


            SqlConnection con = new SqlConnection(pbConnectionString);
            DataSet ds = new DataSet();
            SqlDataAdapter ada = new SqlDataAdapter(sql, con);

            ada.Fill(ds);  
            con.Close();

            int ligne = ds.Tables[0].Rows.Count;
            string[,] data = new string[11, ligne];
           
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                //data[1, i] = dr[ds.Tables[0].Columns[1].ColumnName].ToString();

                ViewBag.CouponNumber = dr[ds.Tables[0].Columns[0].ColumnName].ToString();
                ViewBag.CouponStatus = dr[ds.Tables[0].Columns[1].ColumnName].ToString();
                ViewBag.UsageOriginCode = dr[ds.Tables[0].Columns[2].ColumnName].ToString();
                ViewBag.UsageDestinationCode = dr[ds.Tables[0].Columns[3].ColumnName].ToString();
                ViewBag.Carrier = dr[ds.Tables[0].Columns[4].ColumnName].ToString();
                ViewBag.FareBasisTicketDesignator = dr[ds.Tables[0].Columns[5].ColumnName].ToString();
                ViewBag.ReservationBookingDesignator = dr[ds.Tables[0].Columns[6].ColumnName].ToString();
                ViewBag.UsageFlightNumber = dr[ds.Tables[0].Columns[7].ColumnName].ToString();
                ViewBag.UsageAirline = dr[ds.Tables[0].Columns[8].ColumnName].ToString();
                ViewBag.UsageDate = dr[ds.Tables[0].Columns[9].ColumnName].ToString();
                ViewBag.IsOAL = dr[ds.Tables[0].Columns[10].ColumnName].ToString();
            }

            return PartialView("_GetDocDetails", cpnNo);
        }

        // Convert Date
        public string ConvertDateProcess(string date)
        {
            var culture = System.Globalization.CultureInfo.CurrentCulture;
            string mydate = DateTime.ParseExact(date, "dd-MMM-yyyy", new CultureInfo(culture.Name)).ToString("MM-dd-yyyy");
            return mydate;
        }

        //Harentsoa
        //Display all Document Details
        public ActionResult DisplayDetails()
        {

            string doc = Request["doc"];
            string cpn = Request["cpn"];
          
            string sql = "Select f3.DocumentAirlineID, f3.CheckDigit, f3.TicketingModeIndicator, f3.SettlementAuthorizationCode" + Environment.NewLine;
            sql = sql + ", f1.FlightNumber, f1.FlightDepartureDate, left(f1.FareBasisTicketDesignator,1), f1.OriginCity, f1.DestinationCity, f1.Carrier" + Environment.NewLine;
            sql = sql + "from pax.SalesDocumentCoupon f1" + Environment.NewLine;
            sql = sql + "left join pax.SalesRelatedDocumentInformation f2" + Environment.NewLine;
            sql = sql + "on f1.RelatedDocumentGuid = f2.RelatedDocumentGuid" + Environment.NewLine;
            sql = sql + "left join pax.SalesDocumentHeader f3" + Environment.NewLine;
            sql = sql + "on f3.HdrGuid = f2.HdrGuid" + Environment.NewLine;
            sql = sql + "WHERE f1.DocumentNumber = '" + doc + "' and f1.CouponNumber = '" + cpn + "'";

            SqlConnection con = new SqlConnection(pbConnectionString);
            DataSet ds = new DataSet();
            SqlDataAdapter ada = new SqlDataAdapter(sql, con);

            ada.Fill(ds);
            con.Close();

            int ligne = ds.Tables[0].Rows.Count;
            string[,] data2 = new string[11, ligne];

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
              
                ViewBag.IssuingAirline = dr[ds.Tables[0].Columns[0].ColumnName].ToString();
                ViewBag.CheckDigit = dr[ds.Tables[0].Columns[1].ColumnName].ToString();
                ViewBag.TicketingModeIndicator = dr[ds.Tables[0].Columns[2].ColumnName].ToString();
                ViewBag.SettlementAuthorizationCode = dr[ds.Tables[0].Columns[3].ColumnName].ToString();
                ViewBag.FlightNumber = dr[ds.Tables[0].Columns[4].ColumnName].ToString();
                ViewBag.FlightDepartureDate = dr[ds.Tables[0].Columns[5].ColumnName].ToString();
                ViewBag.FareBasisTicketDesignator = dr[ds.Tables[0].Columns[6].ColumnName].ToString();
                ViewBag.OriginCity = dr[ds.Tables[0].Columns[7].ColumnName].ToString();
                ViewBag.DestinationCity = dr[ds.Tables[0].Columns[8].ColumnName].ToString();
                ViewBag.Carrier = dr[ds.Tables[0].Columns[9].ColumnName].ToString(); 

            }

            ViewBag.cpn = cpn;
            ViewBag.doc = doc;

            return PartialView("_DisplayDetails");
        }

        // Harentsoa
        // Display Records Query usin Billing Memo Number and Invoice Number
        public ActionResult BMRecordsDisplayQry()
        {

            string billingPeriod = Request["billingPeriod"];
            string billingMemoNumber = Request["billingMemoNumber"];
            string invoiceNumber = Request["invoiceNumber"];

            string Sql = "SELECT top 100 '1' AS [No.],F2.[BMCMNumber] as [BMCM Number] ,F2.[InvoiceNumber] as [Invoice No],F2.[DocumentNumber] as [Document No.],F2.[CouponNumber] as [Coupon No.]";
            Sql = Sql + ",[BillingMonth]+[BillingPeriod] as [Billing Period]";
            Sql = Sql + ",F1.[TOAirline] As [Billed Airline]  ";
            Sql = Sql + " FROM [XmlFile].[BMHEADER] F1 left join [XmlFile].[BMBILLINGCOUPONDETAILS] F2 on  F2.[BMCMNumber]= F1.CreditMemoNumber and F1.InvoiceNumber = f2.InvoiceNumber";
            Sql = Sql + " where  1=1";
            if (billingPeriod.Length > 0) { Sql = Sql + " and   [BillingMonth]+[BillingPeriod]='" + billingPeriod + "'"; }
            if (billingMemoNumber.Length > 0) { Sql = Sql + " and [BMCMNumber] ='" + billingMemoNumber + "'"; }
            if (invoiceNumber.Length > 0) { Sql = Sql + " and F1.[InvoiceNumber] ='" + invoiceNumber + "'"; }

            SqlConnection con = new SqlConnection(pbConnectionString);
            DataSet ds = new DataSet();
            SqlDataAdapter ada = new SqlDataAdapter(Sql, con);

            ada.Fill(ds);
            int ligne = ds.Tables[0].Rows.Count;
            string[,] data = new string[7, ligne];
            int i = 0;

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                
                if (ds.Tables[0].Rows.Count > 0)
                {
                   PVCreditMemoNumber = dr[ds.Tables[0].Columns[1].ColumnName].ToString();
                   PVInvoiceNumber = dr[ds.Tables[0].Columns[2].ColumnName].ToString();
                   cboTo = dr[ds.Tables[0].Columns[6].ColumnName].ToString();
  
                    try
                    {
                        //PopulateBillingMemoList();
                        //return RedirectToAction("[PopulateBillingMemoList]");
                        //BilledAirline();
                        //BMXML();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        throw;
                    }
                }
              
                data[0, i] = dr[ds.Tables[0].Columns[0].ColumnName].ToString();
                data[1, i] = dr[ds.Tables[0].Columns[1].ColumnName].ToString();
                data[2, i] = dr[ds.Tables[0].Columns[2].ColumnName].ToString();
                data[3, i] = dr[ds.Tables[0].Columns[3].ColumnName].ToString();
                data[4, i] = dr[ds.Tables[0].Columns[4].ColumnName].ToString();
                data[5, i] = dr[ds.Tables[0].Columns[5].ColumnName].ToString();
                data[6, i] = dr[ds.Tables[0].Columns[6].ColumnName].ToString();

                i++;

            }

            con.Close();
            ViewBag.lonMmQuery = ligne;
            ViewBag.BmRecords = data;

            return PartialView("_BMRecordsDisplayQry");
        }

        public void dgvBMRecQry_CellClick()
        {
            //PVCreditMemoNumber = dgvBMRecQry[1, e.RowIndex].Value.ToString();
            //PVInvoiceNumber = dgvBMRecQry[2, e.RowIndex].Value.ToString();
            //cboTo.Text = dgvBMRecQry[6, e.RowIndex].Value.ToString();
            PopulateBillingMemoList();
            BilledAirline();
            BMXML();
        }

        public ActionResult PopulateBillingMemoList()
        {
            //dgvBillingMemoList

            string PVCreditMemoNumber = Request["PVCreditMemoNumber"];
            string PVInvoiceNumber = Request["PVInvoiceNumber"];
            string invoiceNumber = Request["invoiceNumber"];
            string txtCreditMemoNumber = Request["txtCreditMemoNumber"];
            string txtInvoiceNumber = Request["txtInvoiceNumber"];


            string Sql = "SELECT Sequence,[GrossBilled1] as [Gross Billed],[Tax1] as [Tax],[ISCPer1] as [ ISC Percentage],[ISCAmount] as [ISC Amount],[OtherCommPer] as [Other Commission %],[OtherCommission] as [Other Commission Amount] ,[UATPPer] as [UATP %],[UATP] as [UATP Amount]";
            Sql = Sql + ",[HandlingFeesPer] as [Handling Fees %],[HandlingFees] as [Handling Fees Amount] ,[VATAmountPer] as [VAT Amount %],[VATAmount] as [VAT Amount],[NetAmount] as [Net Amount]";
            Sql = Sql + " FROM [XmlFile].[BMBILLINGCOUPONDETAILS]";

            if (PVCreditMemoNumber.Length > 0)
            {
                Sql = Sql + " where BMCMNumber='" + PVCreditMemoNumber + "'";
                Sql = Sql + " AND [InvoiceNumber] ='" + PVInvoiceNumber + "'";
            }
            else
            {
                Sql = Sql + " where BMCMNumber='" + txtCreditMemoNumber + "'";
                Sql = Sql + " AND [InvoiceNumber] ='" + txtInvoiceNumber + "'";
            }


            SqlConnection con = new SqlConnection(pbConnectionString);
            DataSet ds = new DataSet();
            SqlDataAdapter ada = new SqlDataAdapter(Sql, con);

            ada.Fill(ds);
            int ligne = ds.Tables[0].Rows.Count;
            string[,] data = new string[14, ligne];
            int i = 0;

            foreach (DataRow dr in ds.Tables[0].Rows)
            {

                data[0, i] = dr[ds.Tables[0].Columns[0].ColumnName].ToString();
                data[1, i] = dr[ds.Tables[0].Columns[1].ColumnName].ToString();
                data[2, i] = dr[ds.Tables[0].Columns[2].ColumnName].ToString();
                data[3, i] = dr[ds.Tables[0].Columns[3].ColumnName].ToString();
                data[4, i] = dr[ds.Tables[0].Columns[4].ColumnName].ToString();
                data[5, i] = dr[ds.Tables[0].Columns[5].ColumnName].ToString();
                data[6, i] = dr[ds.Tables[0].Columns[6].ColumnName].ToString();
                data[7, i] = dr[ds.Tables[0].Columns[7].ColumnName].ToString();
                data[8, i] = dr[ds.Tables[0].Columns[8].ColumnName].ToString();
                data[9, i] = dr[ds.Tables[0].Columns[9].ColumnName].ToString();
                data[10, i] = dr[ds.Tables[0].Columns[10].ColumnName].ToString();
                data[11, i] = dr[ds.Tables[0].Columns[11].ColumnName].ToString();
                data[12, i] = dr[ds.Tables[0].Columns[12].ColumnName].ToString();
                data[13, i] = dr[ds.Tables[0].Columns[13].ColumnName].ToString();

                i++;

            }

            con.Close();
            ViewBag.MBListLon = ligne;
            ViewBag.BmList = data;


            return PartialView("_PopulateBillingMemoList");
        }

        public ActionResult BMXML()
        {

            string txtCreditMemoNumber = Request["txtCreditMemoNumber"];
            string txtInvoiceNumber = Request["txtInvoiceNumber"];
            string txtBillingMonth = Request["txtBillingMonth"];
            string txtBillingPeriod = Request["txtBillingPeriod"];
            string cboBMBillingPeriod = Request["cboBMBillingPeriod"];

            if (!(PVCreditMemoNumber.Length > 0))
            {
                PVCreditMemoNumber = txtCreditMemoNumber;
                PVInvoiceNumber = txtInvoiceNumber;
            }
            else
            {
                PVCreditMemoNumber = PVCreditMemoNumber.Trim();
                PVInvoiceNumber = PVInvoiceNumber.Trim();

            }

            string pr = txtBillingMonth + txtBillingPeriod;


            if (pr == "")
            {
                pr = cboBMBillingPeriod;
            }
            try
            {
                string SqlA = " SELECT F3.[AirlineID] As [Billed Airline]   , F3.[AirlineID] As [ Airline]   , f3.[AirlineName],";
                SqlA = SqlA + "[BillingMonth]+[BillingPeriod] as [Billing Period],";
                SqlA = SqlA + "count(*) as CouponCount,";
                SqlA = SqlA + "SUM(F2.[GrossBilled1]) as [Gross Billed],";
                SqlA = SqlA + "sum(f2.[ISCAmount]) as [ISC Amount],";
                SqlA = SqlA + "sum(F2.[GrossBilled1]) - Sum(f2.[ISCAmount]) As NetAmount,";
                SqlA = SqlA + "sum(F2.[TAX1]) as TAXES,";
                SqlA = SqlA + "sum(F2.[NetAmount]) as [Amount to Bill],";
                SqlA = SqlA + "F2.[InvoiceNumber] as [Invoice No],";
                SqlA = SqlA + "F1.[Status]";
                SqlA = SqlA + " FROM [XmlFile].[BMHEADER] F1 ";
                SqlA = SqlA + " left join [XmlFile].[BMBILLINGCOUPONDETAILS] F2 on  F2.[BMCMNumber]= F1.CreditMemoNumber ";
                SqlA = SqlA + " left join [Ref].[Airlines] F3 on  F3.AirlineCode = F1.[TOAirline]";
                SqlA = SqlA + " where   [BillingMonth]+[BillingPeriod]='" + pr + "'";
                SqlA = SqlA + " and [BMCMNumber] ='" + PVCreditMemoNumber + "'";
                SqlA = SqlA + " and f2.[InvoiceNumber] ='" + PVInvoiceNumber + "'";
                SqlA = SqlA + " and   f3.Airlinecode='" + cboTo + "'";
                SqlA = SqlA + "GROUP BY F3.[AirlineID],f3.[AirlineName],[BillingMonth]+[BillingPeriod] ,F2.[BMCMNumber],F2.[InvoiceNumber],F1.[Status];";

                SqlConnection con = new SqlConnection(pbConnectionString);
                DataSet ds = new DataSet();
                SqlDataAdapter ada = new SqlDataAdapter(SqlA, con);

                ada.Fill(ds);
                int ligne = ds.Tables[0].Rows.Count;
                string[,] data = new string[11, ligne];
                int i = 0;

                foreach (DataRow dr in ds.Tables[0].Rows)
                {

                    data[0, i] = dr[ds.Tables[0].Columns[0].ColumnName].ToString();
                    data[1, i] = dr[ds.Tables[0].Columns[1].ColumnName].ToString();
                    data[2, i] = dr[ds.Tables[0].Columns[2].ColumnName].ToString();
                    data[3, i] = dr[ds.Tables[0].Columns[3].ColumnName].ToString();
                    data[4, i] = dr[ds.Tables[0].Columns[4].ColumnName].ToString();
                    data[5, i] = dr[ds.Tables[0].Columns[5].ColumnName].ToString();
                    data[6, i] = dr[ds.Tables[0].Columns[6].ColumnName].ToString();
                    data[7, i] = dr[ds.Tables[0].Columns[7].ColumnName].ToString();
                    data[8, i] = dr[ds.Tables[0].Columns[8].ColumnName].ToString();
                    data[9, i] = dr[ds.Tables[0].Columns[9].ColumnName].ToString();
                    data[10, i] = dr[ds.Tables[0].Columns[10].ColumnName].ToString();
                    data[11, i] = dr[ds.Tables[0].Columns[11].ColumnName].ToString();     

                    i++;

                }

                con.Close();
                ViewBag.MBXMLListLon = ligne;
                ViewBag.MBXMLList = data;
            }
            catch { }

            return PartialView("_BMXML");
        }

        /*
       private void acceptTax()
        {

            string BMCMNumber = Request[""];;
            string DocumentNumber = Request[""];;
            string CouponNumber = Request[""];; 
            int Seq = 0;
            string BillingPeriod = Request[""];; +Request[""];;      
            string TaxCode = "";
            decimal TaxAmount = 0;
            decimal TotTaxAmt = 0;

            for (int z = 0; z < dgvTAXBreakDown.Rows.Count; z++)
            {
                TaxCode = dgvTAXBreakDown[0, z].Value.ToString();
                TaxAmount = Convert.ToDecimal(dgvTAXBreakDown[1, z].Value.ToString());
                TotTaxAmt = TotTaxAmt + TaxAmount;
                Seq = z + 1;

                string SqlA = "select * from [XmlFile].[BMTAXBREAKDOWN] ";
                SqlA = SqlA + " where BMCMNumber='" + BMCMNumber + "'";
                SqlA = SqlA + " AND [DocumentNumber] ='" + DocumentNumber + "'";
                SqlA = SqlA + " AND [CouponNumber] = " + CouponNumber;
                SqlA = SqlA + " AND [TAXCODE] = '" + TaxCode + "'";

                string Sql = "";
                Sql = "IF NOT EXISTS(" + SqlA + ")";

                Sql = Sql + " BEGIN" + Environment.NewLine;
                Sql = Sql + "DECLARE @MaxRecId bigint" + Environment.NewLine;
                Sql = Sql + "set @MaxRecId = (select iif(MAX(RecId) is null,1, MAX(RecId)+1) As MaxLineid from [XmlFile].[BMTAXBREAKDOWN]) ;" + Environment.NewLine;
                Sql = Sql + "INSERT INTO [XmlFile].[BMTAXBREAKDOWN] (RecId,[BMCMNumber] ,[Sequence],[DocumentNumber],[CouponNumber],[BillingPeriod],[TaxCode],[TaxAmount])" + Environment.NewLine; ;

                Sql = Sql + "VALUES (@MaxRecId,'" + BMCMNumber + "',";
                Sql = Sql + "'" + Seq + "',";
                Sql = Sql + "'" + DocumentNumber + "',";
                Sql = Sql + "'" + CouponNumber + "',";
                Sql = Sql + "'" + BillingPeriod + "',";
                Sql = Sql + "'" + TaxCode + "',";
                Sql = Sql + "" + TaxAmount + "";
                Sql = Sql + ")";
                Sql = Sql + "END";
                string MySql = Sql;
                bool flag = DbUpdate(MySql);
                DbClose();


            }
            txtTax1.Text = TotTaxAmt.ToString("###0.000");

        }

       */

        //Harentsoa
        // Save all billing memo records
        public void SaveInformations()
        {
           
            string Recid = "";
            int Sequence = 1;
            string BMCMNumber = Request["txtCreditMemoNumber"] ;
            string InvoiceNumber = Request["txtInvoiceNumber"] ;

            // Save BMHeader
            string FROMAirline = Request["selectFrom"];
            string TOAirline = Request["selectTo"];
            string CreditMemoNumber = Request["txtCreditMemoNumber"];
            string BillingMonth = Request["txtBillingMonth"] ;
            string BillingPeriod = Request["txtBillingPeriod"] ;
            string OurRefIntUseFrom = Request["txtOurRefIntUseFrom"] ;
            string YourInvoiceNumber = Request["txtYourInvoiceNumber"] ;
            string YourBillingMonth = Request["txtYourBillingMonth"] ;
            string YourBillingPeriod = Request["txtYourBillingPeriod"];
            string OurRefIntUseTo = Request["txtOurRefIntUseTo"] ;
            string SourceCode = Request["txtSourceCode"] ;
            string CorrespondanceNumber = Request["txtCorrespondanceNumber"];
            string AttachmentIndicatorOriginal = Request["txtAttachmentIndicatorOriginal"] ;
            string ExchangeRate = Request["txtExchangeRate"] ;
            string CurrencyOfBillingMemo = Request["txtCurrencyOfBillingMemo"] ;
            string FIMNumber = Request["txtFIMNumber"];
            string FIMCouponNumber = Request["txtFIMCouponNumber"];
            string BatchNumber = Request["txtBatchNumber"];
            string SequenceNumber = Request["txtSequenceNumber"];
            string ReasonCode = Request["cmbReasonCode"];
            string SettlementMethod = Request["txtSettlementMethod"];
            string XmlGenarated = "0";
            string Status = "0";

            
            string SqlA = "select CreditMemoNumber from [XmlFile].[BMHEADER] ";
            SqlA = SqlA + " where CreditMemoNumber='" + CreditMemoNumber + "'";
            SqlA = SqlA + " AND [InvoiceNumber] ='" + InvoiceNumber + "'";
            SqlA = SqlA + " AND BillingMonth='" + BillingMonth + "'";
            SqlA = SqlA + " AND BillingPeriod ='" + BillingPeriod + "'";
            string Sql = "";
            Sql = "IF NOT EXISTS(" + SqlA + ")";
            Sql = Sql + " BEGIN" + Environment.NewLine;
            Sql = Sql + "DECLARE @MaxRecId bigint" + Environment.NewLine;
            Sql = Sql + "set @MaxRecId = (select iif(MAX(RecId) is null,1, MAX(RecId)+1) As MaxLineid from [XmlFile].[BMHEADER]);" + Environment.NewLine;
            Sql = Sql + "INSERT INTO [XmlFile].[BMHEADER](";
            Sql = Sql + "[Recid] ,[FROMAirline],[TOAirline],[CreditMemoNumber],[InvoiceNumber],[BillingMonth],[BillingPeriod],[OurRefIntUseFrom]";
            Sql = Sql + ",[YourInvoiceNumber],[YourBillingMonth],[YourBillingPeriod],[OurRefIntUseTo]";
            Sql = Sql + ",[SourceCode] ,[CorrespondanceNumber],[AttachmentIndicatorOriginal],[ExchangeRate],[CurrencyOfBillingMemo]";
            Sql = Sql + ",[FIMNumber],[FIMCouponNumber] ,[BatchNumber],[SequenceNumber] ,[ReasonCode],[SettlementMethod],[XmlGenarated],[Status])";
            Sql = Sql + "VALUES(";
            Sql = Sql + "@MaxRecId" + ",'" + FROMAirline + "','" + TOAirline + "','" + CreditMemoNumber + "','" + InvoiceNumber + "','" + BillingMonth + "','" + BillingPeriod + "','" + OurRefIntUseFrom + "','";
            Sql = Sql + YourInvoiceNumber + "','" + YourBillingMonth + "','" + YourBillingPeriod + "','" + OurRefIntUseTo + "','";
            Sql = Sql + SourceCode + "','" + CorrespondanceNumber + "','" + AttachmentIndicatorOriginal + "'," + ExchangeRate + ",'" + CurrencyOfBillingMemo + "','";
            Sql = Sql + FIMNumber + "','" + FIMCouponNumber + "'," + BatchNumber + "," + SequenceNumber + ",'" + ReasonCode + "','" + SettlementMethod + "'," + XmlGenarated + "," + Status;
            Sql = Sql + ")";
            Sql = Sql + "END";
           

            DbUpdate(Sql);
            
            string TotalGrossBilled = Request["ItGrossBilled"];
            string TotalTaxAmount = Request["ItTaxAmount"];
            string TotalISCAmount = Request["ItISCAmount"];
            string TotalOtherCommission = Request["ItOtherCommission"];
            string TotalUATPAmount = Request["ItUATPAmount"];
            string TotalHandlingFees = Request["ItHandingFees"];
            string TotalVATAmount = Request["ItVATAmount"];
            string TotalNetAmount = Request["ItNetAmount"];
            string NetBillingAmount = Request["ItBillingAmount"];


            SqlA = "select BMCMNumber from [XmlFile].[BMINVOICETOTALS] ";
            SqlA = SqlA + " where BMCMNumber='" + CreditMemoNumber + "'";
            SqlA = SqlA + " AND [InvoiceNumber] ='" + InvoiceNumber + "'";

            Sql = "";
            Sql = "IF NOT EXISTS(" + SqlA + ")";
            Sql = Sql + " BEGIN" + Environment.NewLine;
            Sql = Sql + "DECLARE @MaxRecId bigint" + Environment.NewLine;
            Sql = Sql + "set @MaxRecId = (select iif(MAX(RecId) is null,1, MAX(RecId)+1) As MaxLineid from [XmlFile].[BMINVOICETOTALS])" + Environment.NewLine;
            Sql = Sql + "INSERT INTO [XmlFile].[BMINVOICETOTALS](RecId";
            Sql = Sql + ",[Sequence],[BMCMNumber],[InvoiceNumber],[TotalGrossBilled],[TotalTaxAmount] ,[TotalISCAmount],[TotalOtherCommission],[TotalUATPAmount]";
            Sql = Sql + ",[TotalHandlingFees],[TotalVATAmount],[TotalNetAmount],[NetBillingAmount],[NumberOfBillingRecords])";
            Sql = Sql + "VALUES(@MaxRecId,";
            Sql = Sql + Sequence + ",'" + BMCMNumber + "','" + InvoiceNumber + "'," + TotalGrossBilled + "," + TotalTaxAmount + "," + TotalISCAmount + "," + TotalOtherCommission + "," + TotalUATPAmount + ",";
            Sql = Sql + TotalHandlingFees + "," + TotalVATAmount + "," + TotalNetAmount + "," + NetBillingAmount + ", 1";
            Sql = Sql + ")";
            Sql = Sql + "END";

            DbUpdate(Sql);


            string TicketIssuingAirline = Request["IssuingAirline"];
            string CouponNumber = Request["cpnD"];
            string DocumentNumber = Request["doc"];
            string CheckDigit = Request["CheckDigit"];
            string ElectronicTicketIndicator = Request["TicketingModeIndicator"];
            string ESAC = Request["SettlementAuthorizationCode"];
            string AirlineFlightDesignator = Request["AirlineFlightDesignator"];
            string FlightNumber = Request["FlightNumber"];
            string FlightDate = Request["FlightDepartureDate"];
            string FromAirport = Request["OriginCity"];
            string ToAirport = Request["DestinationCity"];
            string CabinClass = Request["FareBasisTicketDesignator"];
            string AgreementIndicator = Request["AgreementIndicator"];
            string OriginalPMI = Request["OriginalPMI"];
            string ValidatedMPI = Request["ValidateMPI"];
            string ProrateMethodology = Request["ProrateMethodologie"];
            string AirlineOwnUse = Request["AirlineOwnUse"];
            string ReferenceField1 = Request["ReferenceField1"];
            string ReferenceField2 = Request["ReferenceField2"];
            string ReferenceField3 = Request["ReferenceField3"];
            string ReferenceField4 = Request["ReferenceField4"];
            string ReferenceField5 = Request["ReferenceField5"]; 
            
            string GrossBilled1 = Request["CaGrossBilled"];
            string Tax1 = Request["CaTax"];
            string ISCPer1 = Request["CaIscPer"];
            string ISCAmount = Request["CaIscAmount"];
            string OtherCommPer = Request["CaOhterCommissionPer"];
            string OtherCommission = Request["CaOhterCommission"];
            string UATPPer = Request["CaUatpPer"];
            string UATP = Request["CaUatp"];
            string HandlingFeesPer = Request["CaHandlingFeesPer"];
            string HandlingFees = Request["CaHandlingFees"];
            string VATAmountPer = Request["CaVatAmountPer"];
            string VATAmount = Request["CaVatAmount"];
            string NetAmount = Request["CaNetAmount"];

            string CurrencyAdjustmentIndicator = Request["CurrencyAdjustmentIndicator"];

            SqlA = "select BMCMNumber from [XmlFile].[BMBILLINGCOUPONDETAILS] ";
            SqlA = SqlA + " where BMCMNumber='" + CreditMemoNumber + "'";
            SqlA = SqlA + " AND [InvoiceNumber] ='" + InvoiceNumber + "'";
            // SqlA = SqlA + " AND BillingMonth='" + BillingMonth + "'";
            //SqlA = SqlA + " AND BillingPeriod ='" + BillingPeriod + "'";
            SqlA = SqlA + " AND CouponNumber='" + CouponNumber + "'";
            SqlA = SqlA + " AND DocumentNumber ='" + DocumentNumber + "'";
            SqlA = SqlA + " AND NetAmount = " + NetAmount + "";

            Sql = "";
            Sql = "IF NOT EXISTS(" + SqlA + ")";
            Sql = Sql + " BEGIN" + Environment.NewLine;
            Sql = Sql + "DECLARE @MaxRecId bigint" + Environment.NewLine;
            Sql = Sql + "set @MaxRecId = (select iif(MAX(RecId) is null,1, MAX(RecId)+1) As MaxLineid from [XmlFile].[BMBILLINGCOUPONDETAILS])" + Environment.NewLine;
            Sql = Sql + "INSERT INTO [XmlFile].[BMBILLINGCOUPONDETAILS] (";
            Sql = Sql + "[Recid],[Sequence],[BMCMNumber],[InvoiceNumber],[TicketIssuingAirline],[CouponNumber],[DocumentNumber],[CheckDigit],[ElectronicTicketIndicator],[ESAC]";
            Sql = Sql + ",[AirlineFlightDesignator],[FlightNumber],[FlightDate],[FromAirport],[ToAirport],[CabinClass]";
            Sql = Sql + ",[AgreementIndicator],[OriginalPMI],[ValidatedMPI],[ProrateMethodology],[AirlineOwnUse]";
            Sql = Sql + ",[ReferenceField1],[ReferenceField2],[ReferenceField3],[ReferenceField4],[ReferenceField5]";
            Sql = Sql + ",[GrossBilled1],[Tax1],[ISCPer1],[ISCAmount],[OtherCommPer],[OtherCommission],[UATPPer],[UATP]";
            Sql = Sql + ",[HandlingFeesPer],[HandlingFees],[VATAmountPer],[VATAmount],[NetAmount],[CurrencyAdjustmentIndicator])";
            Sql = Sql + " VALUES (";
            Sql = Sql + "@MaxRecId" + "," + Sequence + ",'" + BMCMNumber + "','" + InvoiceNumber + "','" + TicketIssuingAirline + "','" + CouponNumber + "','" + DocumentNumber + "','" + CheckDigit + "','" + ElectronicTicketIndicator + "','" + ESAC + "','";
            Sql = Sql + AirlineFlightDesignator + "','" + FlightNumber + "', Convert(date,'" + FlightDate + "',105),'" + FromAirport + "','" + ToAirport + "','" + CabinClass + "','";
            Sql = Sql + AgreementIndicator + "','" + OriginalPMI + "','" + ValidatedMPI + "','" + ProrateMethodology + "','" + AirlineOwnUse + "','";
            Sql = Sql + ReferenceField1 + "','" + ReferenceField2 + "','" + ReferenceField3 + "','" + ReferenceField4 + "','" + ReferenceField5 + "',";
            Sql = Sql + GrossBilled1 + "," + Tax1 + "," + ISCPer1 + "," + ISCAmount + "," + OtherCommPer + "," + OtherCommission + "," + UATPPer + "," + UATP + ",";
            Sql = Sql + HandlingFeesPer + "," + HandlingFees + "," + VATAmountPer + "," + VATAmount + "," + NetAmount + ",'" + CurrencyAdjustmentIndicator + "')";
            Sql = Sql + "END";

            DbUpdate(Sql);


            PopulateBillingMemoList();
            BMXML();
        }

        public void GetTax()
        {
            string Sector = Request["sourceCode"];
            try
            {
                GetTaxCode();
                GetTaxRate();
            }
            catch { }
        }

       
        // Harentsoa
        public ActionResult GetTaxCode()
        {
            // Get dropdown values
            string Sector = Request["sourceCode"];
            string taxLog = Request["taxLog"];

            //string Param0 = Request["Param0"];
            //string Param1 = Request["Param1"];


                //string[] Delta = taxLog.Split('|');
                //Sector = Delta.ToString() + "|" + Delta[1].ToString();

                try
                {
                string[] Param = Sector.Split('-');

                string Sql = "SELECT [FromAirport]  as [From Airport] ,[ToAirport] as [ To Airport] ,[MappedPrimeCode] as [Mapped Prime Code]";
                Sql = Sql + ",[PassengerType] as [Passenger Type],[DomInt] as [Dom Int],[TransitDuration] as [Transit Duration],[ValidFrom] as [Valid From] ,[ValidTo] as [Valid To]";
                Sql = Sql + ",[TaxCode] as [Tax Code],[TaxCurrency] as [Tax Currency],[TaxAmount] as [Tax Amount] ,[TaxPercentage] as [Tax Percentage],'USD' as [Currency],'0.000' as [USD Amount],[TAXREFID]";
                Sql = Sql + " FROM  [Ref].[Tax]";
                Sql = Sql + " Where [FromAirport] ='" + Param[0] + "'";
                Sql = Sql + " And [ToAirport] ='" + Param[1] + "'";
                Sql = Sql + " Order by [FromAirport] ,[ToAirport],TaxCode";


                SqlConnection con = new SqlConnection(pbConnectionString);
                DataSet ds = new DataSet();
                SqlDataAdapter ada = new SqlDataAdapter(Sql, con);

                ada.Fill(ds);
                int ligne = ds.Tables[0].Rows.Count;
                string[,] data = new string[15, ligne];
                int i = 0;

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    data[0, i] = dr[ds.Tables[0].Columns[0].ColumnName].ToString();
                    data[1, i] = dr[ds.Tables[0].Columns[1].ColumnName].ToString();
                    data[2, i] = dr[ds.Tables[0].Columns[2].ColumnName].ToString();
                    data[3, i] = dr[ds.Tables[0].Columns[3].ColumnName].ToString();
                    data[4, i] = dr[ds.Tables[0].Columns[4].ColumnName].ToString();
                    data[5, i] = dr[ds.Tables[0].Columns[5].ColumnName].ToString();
                    data[6, i] = dr[ds.Tables[0].Columns[6].ColumnName].ToString();
                    data[7, i] = dr[ds.Tables[0].Columns[7].ColumnName].ToString();
                    data[8, i] = dr[ds.Tables[0].Columns[8].ColumnName].ToString();
                    data[9, i] = dr[ds.Tables[0].Columns[9].ColumnName].ToString();
                    data[10, i] = dr[ds.Tables[0].Columns[10].ColumnName].ToString();
                    data[11, i] = dr[ds.Tables[0].Columns[11].ColumnName].ToString();

                    
                    data[12, i] = dr[ds.Tables[0].Columns[12].ColumnName].ToString();
                    data[13, i] = dr[ds.Tables[0].Columns[13].ColumnName].ToString();
                    data[14, i] = dr[ds.Tables[0].Columns[14].ColumnName].ToString();
                    
                    i++;
                }

                con.Close();
                ViewBag.lonTaxRate = ligne;
                ViewBag.TaxRateRecords = data;

                //ViewBag.Param1 = Param[0];
                //ViewBag.Param2 = Param[1];
                }
            catch { }

            return PartialView("_GetTaxCode");
        }


        public ActionResult GetTaxRef()
        {
            string TAXREFID = Request["taxId"];

            string Sql = "SELECT [FromAirport]  as [From Airport] ,[ToAirport] as [ To Airport] ,[MappedPrimeCode] as [Mapped Prime Code]";
            Sql = Sql + ",[PassengerType] as [Passenger Type],[DomInt] as [Dom Int],[TransitDuration] as [Transit Duration],[ValidFrom] as [Valid From] ,[ValidTo] as [Valid To]";
            Sql = Sql + ",[TaxCode] as [Tax Code],[TaxCurrency] as [Tax Currency],[TaxAmount] as [Tax Amount] ,[TaxPercentage] as [Tax Percentage]";
            Sql = Sql + " FROM  [Ref].[Tax]";
            Sql = Sql + " Where [TAXREFID] ='" + TAXREFID + "'";

            SqlConnection con = new SqlConnection(pbConnectionString);
            DataSet ds = new DataSet();
            SqlDataAdapter ada = new SqlDataAdapter(Sql, con);

            ada.Fill(ds);
            int ligne = ds.Tables[0].Rows.Count;
            string[,] data = new string[12, ligne];

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                ViewBag.FromAirport = dr[ds.Tables[0].Columns[0].ColumnName].ToString();
                ViewBag.ToAirport = dr[ds.Tables[0].Columns[1].ColumnName].ToString();
                ViewBag.MappedPrimeCode = dr[ds.Tables[0].Columns[2].ColumnName].ToString();
                ViewBag.PassengerType = dr[ds.Tables[0].Columns[3].ColumnName].ToString();
                ViewBag.DomInt = dr[ds.Tables[0].Columns[4].ColumnName].ToString();
                ViewBag.TransitDuration = dr[ds.Tables[0].Columns[5].ColumnName].ToString();
                ViewBag.ValidFrom = dr[ds.Tables[0].Columns[6].ColumnName].ToString();
                ViewBag.ValidTo = dr[ds.Tables[0].Columns[7].ColumnName].ToString();
                ViewBag.TaxCode = dr[ds.Tables[0].Columns[8].ColumnName].ToString();
                ViewBag.TaxCurrency = dr[ds.Tables[0].Columns[9].ColumnName].ToString();
                ViewBag.TaxAmount = dr[ds.Tables[0].Columns[10].ColumnName].ToString();
                ViewBag.TaxPercentage = dr[ds.Tables[0].Columns[11].ColumnName].ToString();
            }

            con.Close();
            ViewBag.TaxId = TAXREFID;

            return PartialView("_GetTaxRef", TAXREFID);
        }

        public void DestringeTaxRef()
        {
            string TAXREFID = Request["taxId"];

            string SqlDel = "DEstringE REF.TAX WHERE TAXREFID=" + TAXREFID;

            DbUpdate(SqlDel);
        }

        public ActionResult UpdateRefTax()
        {
            string TAXREFID = Request["TaxRefID"];
            string TaxRefFromAirport = Request["#TaxRefFromAirport"];
            string taxRefToAirport = Request["taxRefToAirport"];
            string taxRefMappedPrimeCode = Request["taxRefMappedPrimeCode"];
            string taxRefPassengerType = Request["taxRefPassengerType"];
            string taxRefDomInt = Request["taxRefDomInt"];
            string taxRefTransitDuration = Request["taxRefTransitDuration"];
            string taxRefValidFrom = Request["taxRefValidFrom"];
            string taxRefValidTo = Request["taxRefValidTo"];
            string taxRefTaxCode = Request["taxRefTaxCode"];
            string taxRefTaxCurrency = Request["taxRefTaxCurrency"];
            string taxRefTaxAmount = Request["taxRefTaxAmount"];
            string taxRefTaxPercentage = Request["taxRefTaxPercentage"];

            string Sql = "UPDATE  REF.TAX SET";
            Sql = Sql + " [FromAirport]='" + TaxRefFromAirport + "',";
            Sql = Sql + "[ToAirport]='" + taxRefToAirport + "',";
            Sql = Sql + "[MappedPrimeCode]='" + taxRefMappedPrimeCode + "',";
            Sql = Sql + "[PassengerType]='" + taxRefPassengerType + "',";
            Sql = Sql + "[DomInt]='" + taxRefDomInt + "',";
            Sql = Sql + "[TransitDuration]=" + taxRefTransitDuration + ",";
            Sql = Sql + "[ValidFrom]='" + taxRefValidFrom + "',";
            Sql = Sql + "[ValidTo]='" + taxRefValidTo + "',";
            Sql = Sql + "[TaxCode]='" + taxRefTaxCode + "',";
            Sql = Sql + "[TaxCurrency]='" + taxRefTaxCurrency + "',";
            Sql = Sql + "[TaxAmount]=" + taxRefTaxAmount + ",";
            Sql = Sql + "[TaxPercentage]=" + taxRefTaxPercentage + ",";
            Sql = Sql + " WHERE TAXREFID=" + TAXREFID;
            Sql = Sql.Replace("'Null'", "Null");

            DbUpdate(Sql);

            // SELECT THE LAST MODIFIED DATA TO LOAD ON TABLE VIEW
            string Sql2 = "SELECT [FromAirport]  as [From Airport] ,[ToAirport] as [ To Airport] ,[MappedPrimeCode] as [Mapped Prime Code]";
            Sql2 = Sql2 + ",[PassengerType] as [Passenger Type],[DomInt] as [Dom Int],[TransitDuration] as [Transit Duration],[ValidFrom] as [Valid From] ,[ValidTo] as [Valid To]";
            Sql2 = Sql2 + ",[TaxCode] as [Tax Code],[TaxCurrency] as [Tax Currency],[TaxAmount] as [Tax Amount] ,[TaxPercentage] as [Tax Percentage]";
            Sql2 = Sql2 + " FROM  [Ref].[Tax]";
            Sql2 = Sql2 + " Where [TAXREFID] ='" + TAXREFID + "'";

            SqlConnection con = new SqlConnection(pbConnectionString);
            DataSet ds = new DataSet();
            SqlDataAdapter ada = new SqlDataAdapter(Sql2, con);
            ada.Fill(ds);

            int ligne = ds.Tables[0].Rows.Count;
            string[,] data = new string[15, ligne];
            int i = 0;

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                data[0, i] = dr[ds.Tables[0].Columns[0].ColumnName].ToString();
                data[1, i] = dr[ds.Tables[0].Columns[1].ColumnName].ToString();
                data[2, i] = dr[ds.Tables[0].Columns[2].ColumnName].ToString();
                data[3, i] = dr[ds.Tables[0].Columns[3].ColumnName].ToString();
                data[4, i] = dr[ds.Tables[0].Columns[4].ColumnName].ToString();
                data[5, i] = dr[ds.Tables[0].Columns[5].ColumnName].ToString();
                data[6, i] = dr[ds.Tables[0].Columns[6].ColumnName].ToString();
                data[7, i] = dr[ds.Tables[0].Columns[7].ColumnName].ToString();
                data[8, i] = dr[ds.Tables[0].Columns[8].ColumnName].ToString();
                data[9, i] = dr[ds.Tables[0].Columns[9].ColumnName].ToString();
                data[10, i] = dr[ds.Tables[0].Columns[10].ColumnName].ToString();
                data[11, i] = dr[ds.Tables[0].Columns[11].ColumnName].ToString();

                i++;
            }
            ViewBag.lonTaxRateUpdated = ligne;
            ViewBag.TaxRateRecordUpdated = data;

            con.Close();

            GetTaxCode();
            return PartialView("_UpdateRefTax");
        }

        public ActionResult SaveRefTax()
        {
            string TaxRefFromAirportToAdd = Request["#TaxRefFromAirportToAdd"];
            string taxRefToAirportToAdd = Request["taxRefToAirportToAdd"];
            string taxRefMappedPrimeCodeToAdd = Request["taxRefMappedPrimeCodeToAdd"];
            string taxRefPassengerTypeToAdd = Request["taxRefPassengerTypeToAdd"];
            string taxRefDomIntToAdd = Request["taxRefDomIntToAdd"];
            string taxRefTransitDurationToAdd = Request["taxRefTransitDurationToAdd"];
            string taxRefValidFromToAdd = Request["taxRefValidFromToAdd"];
            //DateTime otaxRefValidFromToAdd = DateTime.Parse(taxRefValidFromToAdd);
            string taxRefValidToToAdd = Request["taxRefValidToToAdd"];
            //DateTime otaxRefValidToToAdd = DateTime.Parse(taxRefValidToToAdd);
            string taxRefTaxCodeToAdd = Request["taxRefTaxCodeToAdd"];
            string taxRefTaxCurrencyToAdd = Request["taxRefTaxCurrencyToAdd"];
            string taxRefTaxAmountToAdd = Request["taxRefTaxAmountToAdd"];
            string taxRefTaxPercentageToAdd = Request["taxRefTaxPercentageToAdd"];

            string Sql = "DECLARE @RecId bigint" + Environment.NewLine;
            Sql = Sql + "set @RecId = (select iif(MAX(TAXREFID) is null,1, MAX(TAXREFID)+1) As MaxLineid from REF.TAX2);" + Environment.NewLine; ;
            Sql = Sql + "INSERT INTO REF.TAX (";
            Sql = Sql + "[FromAirport],[ToAirport],[MappedPrimeCode],[PassengerType],[DomInt],[TransitDuration],[ValidFrom],[ValidTo],[TaxCode],[TaxCurrency],[TaxAmount],[TaxPercentage])";
            Sql = Sql + "VALUES('";
            Sql = Sql + TaxRefFromAirportToAdd + "','";
            Sql = Sql + taxRefToAirportToAdd + "','";
            Sql = Sql + taxRefMappedPrimeCodeToAdd + "','";
            Sql = Sql + taxRefPassengerTypeToAdd + "','";
            Sql = Sql + taxRefDomIntToAdd + "',";
            Sql = Sql + taxRefTransitDurationToAdd + ",'";
            Sql = Sql + taxRefValidFromToAdd + "','";
            Sql = Sql + taxRefValidToToAdd + "','";
            Sql = Sql + taxRefTaxCodeToAdd + "','";
            Sql = Sql + taxRefTaxCurrencyToAdd + "',";
            Sql = Sql + taxRefTaxAmountToAdd + ",";
            Sql = Sql + taxRefTaxPercentageToAdd + ")";
            Sql = Sql.Replace("'Null'", "Null");

            DbUpdate(Sql);


            // SELECT THE LAST RECORD INSRTED INTO DB
            // SELECT * FROM Table ORDER BY ID DESC LIMIT 1
            
            string Sql2 = "SELECT TOP 1 [FromAirport]  as [From Airport] ,[ToAirport] as [ To Airport] ,[MappedPrimeCode] as [Mapped Prime Code]";
            Sql2 = Sql2 + ",[PassengerType] as [Passenger Type],[DomInt] as [Dom Int],[TransitDuration] as [Transit Duration],[ValidFrom] as [Valid From] ,[ValidTo] as [Valid To]";
            Sql2 = Sql2 + ",[TaxCode] as [Tax Code],[TaxCurrency] as [Tax Currency],[TaxAmount] as [Tax Amount] ,[TaxPercentage] as [Tax Percentage]";
            Sql2 = Sql2 + " FROM  [Ref].[Tax] ORDER BY [TAXREFID] DESC";

            SqlConnection con = new SqlConnection(pbConnectionString);
            DataSet ds = new DataSet();
            SqlDataAdapter ada = new SqlDataAdapter(Sql2, con);
            ada.Fill(ds);

            int ligne = ds.Tables[0].Rows.Count;
            string[,] data = new string[15, ligne];
            int i = 0;

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                data[0, i] = dr[ds.Tables[0].Columns[0].ColumnName].ToString();
                data[1, i] = dr[ds.Tables[0].Columns[1].ColumnName].ToString();
                data[2, i] = dr[ds.Tables[0].Columns[2].ColumnName].ToString();
                data[3, i] = dr[ds.Tables[0].Columns[3].ColumnName].ToString();
                data[4, i] = dr[ds.Tables[0].Columns[4].ColumnName].ToString();
                data[5, i] = dr[ds.Tables[0].Columns[5].ColumnName].ToString();
                data[6, i] = dr[ds.Tables[0].Columns[6].ColumnName].ToString();
                data[7, i] = dr[ds.Tables[0].Columns[7].ColumnName].ToString();
                data[8, i] = dr[ds.Tables[0].Columns[8].ColumnName].ToString();
                data[9, i] = dr[ds.Tables[0].Columns[9].ColumnName].ToString();
                data[10, i] = dr[ds.Tables[0].Columns[10].ColumnName].ToString();
                data[11, i] = dr[ds.Tables[0].Columns[11].ColumnName].ToString();

                i++;
            }
            ViewBag.lonTaxRateAdded = ligne;
            ViewBag.TaxRateRecordAdded = data;
            
            con.Close();
            
            return PartialView("_SaveRefTax");
        }

        // Harentsoa
        private void GetTaxRate()
        {
            string Sector = Request["sourceCode"];
            string taxLog = Request["taxLog"];

            string[] Param = Sector.Split('-');

            string Sql = "SELECT [FromAirport]  as [From Airport] ,[ToAirport] as [ To Airport] ,[MappedPrimeCode] as [Mapped Prime Code],[PassengerType] as [Passenger Type],[DomInt] as [Dom Int],[TransitDuration] as [Transit Duration],[ValidFrom] as [Valid From] ,[ValidTo] as [Valid To]";
            Sql = Sql + ",[TaxCode] as [Tax Code],[TaxCurrency] as [Tax Currency],[TaxAmount] as [Tax Amount] ,[TaxPercentage] as [Tax Percentage],'USD' as [Currency],'0.000' as [USD Amount],[TAXREFID]";
            Sql = Sql + " FROM  [Ref].[Tax]";
            Sql = Sql + " Where [FromAirport] ='" + Param[0] + "'";
            Sql = Sql + " And [ToAirport] ='" + Param[1] + "'";
            Sql = Sql + " Order by [FromAirport] ,[ToAirport],TaxCode";


            SqlConnection con = new SqlConnection(pbConnectionString);
            DataSet ds = new DataSet();
            SqlDataAdapter ada = new SqlDataAdapter(Sql, con);

            ada.Fill(ds);
            int ligne = ds.Tables[0].Rows.Count;
            string[,] data = new string[11, ligne];
            int i = 0;

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                for (int k = 0; k < ligne; k++)
                {
                    string Curr = dr[ds.Tables[0].Columns[9].ColumnName].ToString();
                    FiveDaysRate(Curr, i);
                }
            }

            con.Close();
        }

        // Harentsoa
        private void FiveDaysRate(string Currency, int i)
        {

            string Period = Request["lbl5drP"];
            Period = Period.Substring(0, 6);
            int per = Convert.ToInt32(Period);// -1;
            Period = Convert.ToString(per);
            string Sql = "SELECT [Period],[Currency],[USDRate]";
            //201201 -1= 201400= 2013=12

            Sql = Sql + " FROM [Ref].[CurrencyRate]";
            Sql = Sql + " where CurrencyType='2'";
            Sql = Sql + " and period='" + Period + "'";
            Sql = Sql + " and Currency='" + Currency + "'";

            try
            {
                /*
                SqlDataReader myReader;
                int R = 0;
                myReader = GetlistofTables(Sql);

                if (myReader == null) {; return; }
                if (myReader.HasRows == true)
                {
                    while (myReader.Read())
                    {


                        lbl5drR.Text = myReader.GetValue(2).ToString(); ;

                        decimal A = Convert.ToDecimal(myReader.GetValue(2).ToString());
                        decimal cur1 = Convert.ToDecimal(dataGridView1[10, i].Value);

                        decimal tx = cur1 / A;
                        dataGridView1[13, i].Value = tx.ToString("####0.000");

                        decimal Pvstx = cur1 / A;

                    }


                }
                // always call Close when done reading.
                myReader.Close();
                myReader.Dispose();
                myReader = null;
                // Close the connection when done with it.

                DbClose();
                */

            }
            catch { }

            MeanMonthlyRate(Period, Currency);

        }
        
        //Harentsoa
        private void MeanMonthlyRate(string Period, string Currency)
        {
            /*
            lblMmrP.Text = "";
            lblMmrC.Text = "";
            lblMmrR.Text = "";
            */

            string Sql = "SELECT [Period],[Currency],[USDRate]";
            Sql = Sql + " FROM [Ref].[MMR]";
            Sql = Sql + " where [CurrencyType] ='3'";
            Sql = Sql + " and period='" + Period + "'";
            Sql = Sql + " and Currency='" + Currency + "'";

            try
            {
                /*
                SqlDataReader myReader;
                int R = 0;
                myReader = GetlistofTables(Sql);

                if (myReader == null) {; return; }
                if (myReader.HasRows == true)
                {
                    while (myReader.Read())
                    {


                        lblMmrP.Text = myReader.GetValue(0).ToString(); ;
                        lblMmrC.Text = myReader.GetValue(1).ToString(); ;
                        lblMmrR.Text = myReader.GetValue(2).ToString(); ;


                    }


                }

                myReader.Close();
                myReader.Dispose();
                myReader = null;
                // Close the connection when done with it.

                DbClose();

    */
            }
            catch
            {
                //handefa changes
            }

        }

        //Harentsoa
        //calcul Invoice Total
        public void InvoiceTotals()
        {
            decimal bg = 0;
            decimal ta = 0;
            decimal isc = 0;
            decimal co = 0;
            decimal ua = 0;
            decimal hf = 0;
            decimal va = 0;

            decimal ne = 0;
            decimal ba = 0;

            string ItGrossBilled = Request["ItGrossBilled"];
            string ItTaxAmount = Request["ItTaxAmount"];
            string ItISCAmount = Request["ItISCAmount"];
            string ItOtherCommission = Request["ItOtherCommission"];
            string ItUATPAmount = Request["ItUATPAmount"];
            string ItHandingFees = Request["ItHandingFees"];
            string ItVATAmount = Request["ItVATAmount"];

            string ItNetAmount = Request["ItNetAmount"];
            string ItBillingAmount = Request["ItBillingAmount"];

            try { ba = Convert.ToDecimal(ItGrossBilled); }
            catch { ba = 0; }
            try { ta = Convert.ToDecimal(ItTaxAmount); }
            catch { ta = 0; }
            try { isc = Convert.ToDecimal(ItISCAmount); }
            catch { isc = 0; }
            try { co = Convert.ToDecimal(ItOtherCommission); }
            catch { co = 0; }
            try { ua = Convert.ToDecimal(ItUATPAmount); }
            catch { ua = 0; }
            try { hf = Convert.ToDecimal(ItHandingFees); }
            catch { hf = 0; }
            try { va = Convert.ToDecimal(ItVATAmount); }
            catch { va = 0; }     

            ne = ba + ta + isc + co + ua + hf + va;
            ba = ne;

            ViewBag.NetAmount = ne;
            ViewBag.BillingAmount = ba;
        }


        public void ChargeAmount()
        {
            decimal ba1 = 0;
            decimal ta1 = 0;
            decimal isc1 = 0;
            decimal co1 = 0;
            decimal ua1 = 0;
            decimal hf1 = 0;
            decimal va1 = 0;

            decimal ne1 = 0;
            decimal ba = 0;

            decimal IscPer = 0;
            decimal OthPer = 0;
            decimal UatpPer = 0;
            decimal HfPer = 0;
            decimal VatPer = 0;


            decimal IscPer1 = 0;
            decimal OthPer1 = 0;
            decimal UatpPer1 = 0;
            decimal HfPer1 = 0;
            decimal VatPer1 = 0;

            string CaGrossBilled = Request["CaGrossBilled"];
            string CaIscPer = Request["CaIscPer"];
            string CaIscAmount = Request["CaIscAmount"];
            string CaOhterCommissionPer = Request["CaOhterCommissionPer"];
            string CaOtherCommission = Request["CaOtherCommission"];
            string CaUatpPer = Request["CaUatpPer"];
            string CaUatp = Request["CaUatp"];
            string CaHandlingFeesPer = Request["CaHandlingFeesPer"];
            string CaHandlingFees = Request["CaHandlingFees"];
            string CaVatAmountPer = Request["CaVatAmountPer"];
            string CaVatAmount = Request["CaVatAmount"];

            string CaTax = Request["CaTax"];
            string CaNetAmount = Request["CaNetAmount"];

            try { ba1 = Convert.ToDecimal(CaGrossBilled); }
            catch { ba1 = 0; }
            try { IscPer1 = -1 * Math.Abs(Convert.ToDecimal(CaIscPer)); }
            catch { IscPer1 = 0; }
            try { OthPer1 = 1 * Math.Abs(Convert.ToDecimal(CaOhterCommissionPer)); }
            catch { OthPer1 = 0; }
            try { UatpPer1 = 1 * Math.Abs(Convert.ToDecimal(CaUatpPer)); }
            catch { UatpPer1 = 0; }
            try { HfPer1 = 1 * Math.Abs(Convert.ToDecimal(CaHandlingFeesPer)); }
            catch { HfPer1 = 0; }
            try { VatPer1 = 1 * Math.Abs(Convert.ToDecimal(CaVatAmountPer)); }
            catch { VatPer1 = 0; }


            try { IscPer = (Convert.ToDecimal(CaIscPer) / 100) * ba1; }
            catch { IscPer = 0; }
            try { OthPer = (Convert.ToDecimal(CaOhterCommissionPer) / 100) * ba1; }
            catch { OthPer = 0; }
            try { UatpPer = (Convert.ToDecimal(CaUatpPer) / 100) * ba1; }
            catch { UatpPer = 0; }
            try { HfPer = (Convert.ToDecimal(CaHandlingFeesPer) / 100) * ba1; }
            catch { HfPer = 0; }
            try { VatPer = (Convert.ToDecimal(CaVatAmountPer) / 100) * ba1; }
            catch { VatPer = 0; }

            try { ta1 = Convert.ToDecimal(CaTax); }
            catch { ta1 = 0; }
            try { isc1 = Convert.ToDecimal(CaIscAmount); }
            catch { isc1 = 0; }
            try { co1 = Convert.ToDecimal(CaOtherCommission); }
            catch { co1 = 0; }
            try { ua1 = Convert.ToDecimal(CaUatp); }
            catch { ua1 = 0; }
            try { hf1 = Convert.ToDecimal(CaHandlingFees); }
            catch { hf1 = 0; }
            try { va1 = Convert.ToDecimal(CaVatAmount); }
            catch { va1 = 0; }

            ne1 = ba1 + ta1 + isc1 + co1 + ua1 + hf1 + va1;
            ViewBag.CaNetAmount = ne1;
        }

    } //!classe
} //! namespace