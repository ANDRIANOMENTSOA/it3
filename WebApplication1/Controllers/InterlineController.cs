using System;
using System.Data;
using WebApplication1.Models;
using System.Data.SqlClient;
using System.Web.Mvc;
using System.Globalization;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;
using System.IO;
using System.Xml;
using System.Net;
using System.Text;
using System.Web.WebPages;
using System.Collections;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;

namespace WebApplication1.Controllers
{
    public class InterlineController : Controller
    {
        //public string pbConnectionString = "Server=.\\RELATE;Database=OnsiteBiatss_KK;User Id=sa; Password=1234";
        //public string pbConnectionString = "Server=FANO-PC;Database=OnsiteBiatss_KK;User Id=so; Integrated Security=True";
        public string pbConnectionString = "Server=DESKTOP-Q821GFS;Database=OnsiteBiatss_KK;User Id=sa; Password=1234";
        //public string pbConnectionString = "Server=DESKTOP-O0K2BQJ\\SA;Database=OnsiteBiatss_KK;User Id=sa; Password=1234";
       // public string pbConnectionString = "Server=DESKTOP-O0K2BQJ\\SA;Database=OnsiteBiatss_KK;User Id=sa; Password=1234";
        //public string pbConnectionString = "Server=DESKTOP-7HJUR50;Database=OnsiteBiatss_KK;User Id=sa; Password=1234";
        //public string pbConnectionString = "Server=DESKTOP-CGR76E3;Database=OnsiteBiatss_KK;User Id=sa; Password=1234";
        //public string pbConnectionString = "Server=DESKTOP-54APPF8\\SYMPHONY2;Database=OnsiteBiatss_KK;User Id=sa; Password=1234";
        // public string pbConnectionString = "Server=DESKTOP-O0K2­BQJ\\SA;Database=Ons­iteBiatss_KK;User Id=sa; Password=1234";
        //  public string pbConnectionString = "Server=DESKTOP-DORH05Q;Database=OnsiteBiatss_KK;User Id=sa; Password=1234";

        ConnexionSQLServer.DbConnection dbconnect = new ConnexionSQLServer.DbConnection();
        String es = "";
        String txtCustomerCode = "";
        SqlConnection conn;
        // GET: Interline
        public ActionResult Index()
        {
            string userAgent;
            userAgent = Request.UserAgent;
            if ((userAgent.IndexOf("Edge") != -1) || (userAgent.IndexOf("Trident") != -1))
            {
                ViewBag.message = "ie";
            }
            else
            {
                ViewBag.message = "Autre";
            }
            return PartialView();
        }
        public ActionResult test()
        {
            return PartialView();
        }
        public ActionResult Correspondence()
        {
            string dateTo = DateTime.Now.ToString("dd-MMM-yyyy");
            string[] date = new string[1] { dateTo };

            /*correspondence*/
            string Sql = "SELECT  distinct [InvoiceNumber] + '-' + CASE [Status] WHEN 1 THEN 'Disputed' WHEN 2 THEN 'Resolved'END AS Correspondences  FROM [XmlFile].[Correspondences]";
            SqlDataReader myReader;
            int R = 0;
            myReader = GetlistofTables(Sql);
            SqlConnection cs = new SqlConnection(pbConnectionString);
            DataSet ds = new DataSet();
            SqlDataAdapter ada = new SqlDataAdapter(Sql, cs);
            ada.Fill(ds);
            int ligne = ds.Tables[0].Rows.Count;
            string[,] comboBox3tb = new string[ligne, 1];
            if (myReader != null)
            {
                if (myReader.HasRows == true)
                {
                    while (myReader.Read())
                    {
                        comboBox3tb[R, 0] = myReader.GetValue(0).ToString();
                        R++;
                    }
                }
            }
            myReader.Close();
            myReader.Dispose();
            myReader = null;
            /*fin correspondence*/
            ViewBag.ligne = ligne;
            ViewBag.comboBox3tb = comboBox3tb;
            ViewBag.date = date;
            return PartialView();
        }


        public ActionResult KeyControllingData()
        {
            return PartialView();
        }
        public ActionResult INAD()
        {
            return PartialView();
        }


        /*Code Shares   Joseph */
        public ActionResult Codeshares()
        {
            return PartialView();
        }

        public ActionResult ListeCodeshares()
        {
            string sql = "SELECT *  FROM [SPA].[CodeShareFlights]";

            SqlConnection con = new SqlConnection(pbConnectionString);
            DataSet ds = new DataSet();
            SqlDataAdapter ada = new SqlDataAdapter(sql, con);
            ada.Fill(ds);
            con.Close();

            int logCodeshare = ds.Tables[0].Rows.Count;

            string[,] ListeCodeShare = new string[10, logCodeshare];

            int i = 0;

            foreach (DataRow dr in ds.Tables[0].Rows)
            {

                for (int j = 0; j < 8; j++)
                {
                    ListeCodeShare[j, i] = dr[ds.Tables[0].Columns[j].ColumnName].ToString();
                }

                i++;
            }

            ViewBag.logCodeshare = logCodeshare;
            ViewBag.ListeCodeShare = ListeCodeShare;


            return PartialView("Codeshares");
        }

        /*End Code Shares   Joseph */


        public ActionResult CreditMemos()
        {
            return PartialView();
        }
        public ActionResult SPAEngine()
        {
            return PartialView();
        }
        public ActionResult RejectionToleranceValue()
        {
            return PartialView();
        }
        public ActionResult CheckTolerance()
        {
            return PartialView();
        }
        public ActionResult RefundAdjustements() {
            string dateTo = DateTime.Now.ToString("dd-MMM-yyyy");
            string[] date = new string[1] { dateTo };
            ViewBag.date = date;
            return PartialView();
        }


        public ActionResult InterlineBilling()
        {
            string dateFrom = DateTime.Now.AddMonths(-1).ToString("dd-MMM-yyyy");
            string dateTo = DateTime.Now.ToString("dd-MMM-yyyy");
            string[] date = new string[2] { dateFrom, dateTo };
            ViewBag.date = date;

            /* All Function Interline Invoice  Joseph*/
            OnLoadInvoices();
            AllItem();
            /* End  Function Interline Invoice  Joseph*/

            return PartialView();
        }


        /*Interline Interline Invoice   Joseph*/
       
            // Declaration
        bool showflag = false;
        bool BillingPeriodFlag = false;
        private string pvfile = "";
        private string PVCreditMemoNumber = "";
        private string PVInvoiceNumber = "";
        private string pvDuplicate = "";
        private string PvCheckDuplicateInvoive = "";
        private string BatchSeq = "";
        private string CONNECTIONSTATUS = "NOT CONNECTED";
        private string ConnString = "";
        private string GeneralPeriod = "";
        private string WHERECLAUSE = "-1";
        private int pvLineId = 0;
        private string pvChargeCode = "";
        private string pvInvoiceDate = "";
        private string PVPROCESSDATE = "";
        private int pvDetailNumber = 0;
        private int pvBillingStatus = 0;
        private string pvTransmissionID = "";
        int RejectCounter = 0;
        int AcceptCounter = 0;
        decimal zTot = 0;
        int dataGridView13CurrentRowIndex = -1;
        string FullDocNo = "";
        string PreviousBillingPeriod = "";
        decimal RejTNAmt = 0;
        decimal AcpTNAmt = 0;
        List<string> dtColumns = new List<string>();
        string pvseller = "";
        private string pvAlc = "";
        private string pvTicketNo = "";
        private string pvCoupontNo = "";

        private string pvInvoiceNumber = "";
        private string pvInvoiceNumberSeq = "";
        private int Xinvnum = 0;
        private int DatabaseError = 0;
        private string Alc = "";
        private string Tk = "";
        private string Cpn = "";
        private string Bp = "";

        private string pvFlightDate = "";
        private string pvSecFrom = "";
        private string pvSecTo = "";

        private string pvAirLineRef = "";
        private string pvCountryRef = "";
        private string pvCurrencyRef = "";
        private string pvAirlineNumericCodeRef = "";
        private string pvSalesPeriodRef = "";
        private string pvIssuePeriodRef = "";
        private string ChargeCode = "";
        string pvTaxCodes = "";
        string pvsellerId = "";
        private string PvTransmissionId = "";


        public ActionResult InterlineInvoice()
        {
            string cboIVInvoiceNumber = Request["valInvoiceNumber"];
            string cboBillingPeriod = Request["valbillingPeriod"];
            string cboChargeCode = Request["valchargeCode"];
            string cboProStatus = Request["valprocesStatus"];

            LoadedInvoices(cboIVInvoiceNumber, cboBillingPeriod, cboChargeCode, cboProStatus);

            AllItem();

            return PartialView();
        }


        /*All Item  Interline Invoice   Joseph*/

        public void AllItem()
        {
            GetInvoiceNumber();
            GetBillingPeriode();
            GetChargeCode();
        }

        public void GetInvoiceNumber()
        {
            string sql = "SELECT  distinct SellerOrganization_OrganizationDesignator+'-'+InvoiceNumber FROM [XmlFile].[DataInvoicesHRD] where isnull(Archived,0) <> 1";

            SqlConnection con = new SqlConnection(pbConnectionString);
            DataSet ds = new DataSet();
            SqlDataAdapter ada = new SqlDataAdapter(sql, con);
            ada.Fill(ds);
            con.Close();

            int logInvoice = ds.Tables[0].Rows.Count;

            string[,] ItemInvoice = new string[1, logInvoice];

            int i = 0;

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                ItemInvoice[0, i] = dr[ds.Tables[0].Columns[0].ColumnName].ToString();

                i++;
            }

            ViewBag.logInvoice = logInvoice;
            ViewBag.ItemInvoice = ItemInvoice;
        }

        public void GetBillingPeriode()
        {
            string Sql = " SELECT  distinct ";
            Sql = Sql + " CAST([SettlementMonthPeriod] AS date) AS [Settlement Month Period]  ";
            Sql = Sql + " FROM  [XmlFile].[DataInvoicesHRD] F1";
            Sql = Sql + " left join [XmlFile].InvoiceAnalytics  F2 on F1.[TransmissionId]  =F2.TransmisionId";
            Sql = Sql + " AND F1.InvoiceNumber= F2.InvoiceNo";
            Sql = Sql + " AND F1.ChargeCode= F2.ChargeCode";
            Sql = Sql + " where [InvoiceNumber] <> '' ";

            // if (cboIVInvoiceNumber.SelectedIndex > 0) { Sql = Sql + " and f1.[SellerOrganization_OrganizationDesignator]+'-'+ f1.[InvoiceNumber]='" + cboIVInvoiceNumber.Text.ToString().Trim() + "'"; };


            SqlConnection con = new SqlConnection(pbConnectionString);
            DataSet ds = new DataSet();
            SqlDataAdapter ada = new SqlDataAdapter(Sql, con);
            ada.Fill(ds);
            con.Close();

            int logBillingP = ds.Tables[0].Rows.Count;

            string[,] ItemBillingPeriode = new string[1, logBillingP];

            int i = 0;

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                ItemBillingPeriode[0, i] = dr[ds.Tables[0].Columns[0].ColumnName].ToString();

                i++;
            }

            ViewBag.logBillingP = logBillingP;
            ViewBag.ItemBillingPeriode = ItemBillingPeriode;
        }

        public void GetChargeCode()
        {
            string Sql = " SELECT  distinct ";
            Sql = Sql + "  F1.[ChargeCode]  +' - '+[Description]   AS [Description]  ";
            Sql = Sql + " FROM  [XmlFile].[DataInvoicesHRD] F1";
            Sql = Sql + " left join [XmlFile].InvoiceAnalytics  F2 on F1.[TransmissionId]  =F2.TransmisionId";
            Sql = Sql + " AND F1.InvoiceNumber= F2.InvoiceNo";
            Sql = Sql + " AND F1.ChargeCode= F2.ChargeCode";
            Sql = Sql + " where [InvoiceNumber] <> '' ";
           // if (cboIVInvoiceNumber.SelectedIndex > 0) { Sql = Sql + " and f1.[SellerOrganization_OrganizationDesignator]+'-'+ f1.[InvoiceNumber]='" + cboIVInvoiceNumber.Text.ToString().Trim() + "'"; };
           // if (cboBillingPeriod.SelectedIndex > 0) { Sql = Sql + " AND  [SettlementMonthPeriod]='" + cboBillingPeriod.Text.ToString().Trim() + "'"; };

            SqlConnection con = new SqlConnection(pbConnectionString);
            DataSet ds = new DataSet();
            SqlDataAdapter ada = new SqlDataAdapter(Sql, con);
            ada.Fill(ds);
            con.Close();

            int logChargeC = ds.Tables[0].Rows.Count;

            string[,] ItemChargeCode = new string[1, logChargeC];

            int i = 0;

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                ItemChargeCode[0, i] = dr[ds.Tables[0].Columns[0].ColumnName].ToString();

                i++;
            }

            ViewBag.logChargeC = logChargeC;
            ViewBag.ItemChargeCode = ItemChargeCode;
        }

        /* All Item  Interline Invoice   Joseph*/


        public void LoadedInvoices(string cboIVInvoiceNumber, string cboBillingPeriod, string cboChargeCode, string cboProStatus)
        {

            string Sql = " SELECT  distinct [TransmissionId]  AS [ Transmission Id],f1.[InvoiceNumber]   AS [ Invoice Number] ,  CAST(f1.[InvoiceDate] AS date) AS [Invoice Date]  ,[InvoiceType]   AS [Invoice Type] ,[ChargeCategory]   AS [Charge Category]";
            Sql = Sql + ",[SellerOrganization_OrganizationID]   AS [Seller Organization Id],f1.[SellerOrganization_OrganizationDesignator]   AS [Seller Organization Designator]";
            Sql = Sql + ",[BuyerOrganization_OrganizationID]   AS [Buyer Organization ID],[BuyerOrganization_OrganizationDesignator]   AS [Buyer Organization Designator ]";
            Sql = Sql + ",CAST([SettlementMonthPeriod] AS date) AS [Settlement Month Period]  ,[SettlementMethod]   AS [Settlement Method], F1.[LineItemNumber]   AS [Line Item Number]";
            Sql = Sql + ",F1.[ChargeCode]  AS [Charge Code],[Description]   AS [Description],[ChargeAmount]   AS [Charge Amount]";
            Sql = Sql + ",[TaxType]  AS [Tax Type],[TaxAmount]   AS [Tax Amount],[TotalNetAmount]   AS [Total Net Amount]";
            Sql = Sql + ",[DetailCount]   AS [Detail Count],[AddOnChargeName]   AS [AddOn Charge Name],[AddOnChargeAmount]   AS [AddOn Charge Amount]";
            Sql = Sql + "   ,F2.Processdate AS [Processed Date]";
            Sql = Sql + " FROM  [XmlFile].[DataInvoicesHRD] F1";
            Sql = Sql + " left join [XmlFile].InvoiceAnalytics  F2 on F1.[TransmissionId]  =F2.TransmisionId";
            Sql = Sql + " AND F1.InvoiceNumber= F2.InvoiceNo";
            Sql = Sql + " AND F1.ChargeCode= F2.ChargeCode";
            Sql = Sql + " where [InvoiceNumber] <> '' ";

            if (cboIVInvoiceNumber != "-ALL-" ) {
                Sql = Sql + " and f1.[SellerOrganization_OrganizationDesignator]+'-'+ f1.[InvoiceNumber]='" + cboIVInvoiceNumber.ToString().Trim() + "'";
            };
            if (cboBillingPeriod != "-ALL-") {
                Sql = Sql + " AND  [SettlementMonthPeriod]='" + cboBillingPeriod.ToString().Trim() + "'";
            };
            if (cboChargeCode != "-ALL-") {
                Sql = Sql + " AND f1.[ChargeCode]='" + cboChargeCode.ToString().Trim().Substring(0, 1) + "'";
            };

                if (cboProStatus == "1")
                {
                    Sql = Sql + " AND F2.Processdate is not null";

                }
                if (cboProStatus == "2")
                {
                    Sql = Sql + " AND F2.Processdate is null";
                }

            SqlConnection con = new SqlConnection(pbConnectionString);
            DataSet ds = new DataSet();
            SqlCommand cmd = new SqlCommand("" + Sql + "", con);

            SqlDataAdapter ada = new SqlDataAdapter(cmd);

            ada.Fill(ds);

            con.Close();

            int logInvoices = ds.Tables[0].Rows.Count;

            string[,] ListeInvoices = new string[22, logInvoices];
            int i = 0;
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                for (int j = 0; j < 22; j++)
                {

                    ListeInvoices[j, i] = dr[ds.Tables[0].Columns[j].ColumnName].ToString();
                }

                i++;
            }
            ViewBag.logInvoices = logInvoices;
            ViewBag.ListeInvoices = ListeInvoices;
        }

        public void OnLoadInvoices()
        {
            string Sql = " SELECT  distinct [TransmissionId]  AS [ Transmission Id],f1.[InvoiceNumber]   AS [ Invoice Number] ,  CAST(f1.[InvoiceDate] AS date) AS [Invoice Date]  ,[InvoiceType]   AS [Invoice Type] ,[ChargeCategory]   AS [Charge Category]";
            Sql = Sql + ",[SellerOrganization_OrganizationID]   AS [Seller Organization Id],f1.[SellerOrganization_OrganizationDesignator]   AS [Seller Organization Designator]";
            Sql = Sql + ",[BuyerOrganization_OrganizationID]   AS [Buyer Organization ID],[BuyerOrganization_OrganizationDesignator]   AS [Buyer Organization Designator ]";
            Sql = Sql + ",CAST([SettlementMonthPeriod] AS date) AS [Settlement Month Period]  ,[SettlementMethod]   AS [Settlement Method], F1.[LineItemNumber]   AS [Line Item Number]";
            Sql = Sql + ",F1.[ChargeCode]  AS [Charge Code],[Description]   AS [Description],[ChargeAmount]   AS [Charge Amount]";
            Sql = Sql + ",[TaxType]  AS [Tax Type],[TaxAmount]   AS [Tax Amount],[TotalNetAmount]   AS [Total Net Amount]";
            Sql = Sql + ",[DetailCount]   AS [Detail Count],[AddOnChargeName]   AS [AddOn Charge Name],[AddOnChargeAmount]   AS [AddOn Charge Amount]";
            Sql = Sql + "   ,F2.Processdate AS [Processed Date]";
            Sql = Sql + " FROM  [XmlFile].[DataInvoicesHRD] F1";
            Sql = Sql + " left join [XmlFile].InvoiceAnalytics  F2 on F1.[TransmissionId]  =F2.TransmisionId";
            Sql = Sql + " AND F1.InvoiceNumber= F2.InvoiceNo";
            Sql = Sql + " AND F1.ChargeCode= F2.ChargeCode";
            Sql = Sql + " where [InvoiceNumber] <> '' ";

            SqlConnection con = new SqlConnection(pbConnectionString);
            DataSet ds = new DataSet();
            SqlCommand cmd = new SqlCommand("" + Sql + "", con);

            SqlDataAdapter ada = new SqlDataAdapter(cmd);

            ada.Fill(ds);

            con.Close();

            int logInvoices = ds.Tables[0].Rows.Count;

            string[,] ListeInvoices = new string[22, logInvoices];
            int i = 0;
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                for (int j = 0; j < 22; j++)
                {

                    ListeInvoices[j, i] = dr[ds.Tables[0].Columns[j].ColumnName].ToString();
                }

                i++;
            }
            ViewBag.logInvoices = logInvoices;
            ViewBag.ListeInvoices = ListeInvoices;
        }


        private void Termofpayment(string invoiceno, string invoicedate)
        {
            string Sql = "";
            Sql += "SELECT [CurrencyCode],[ClearanceCurrencyCode],[ExchangeRate]   " + Environment.NewLine;
            Sql += ",SUBSTRING(cast(cast (SettlementMonthPeriod as Date) as nvarchar),3,2)+SUBSTRING(cast(cast (SettlementMonthPeriod as Date) as nvarchar),6,2)+SUBSTRING(cast(cast (SettlementMonthPeriod as Date) as nvarchar),9,2) as [SettlementMonthPeriod]  " + Environment.NewLine;
            Sql += " FROM [XmlFile].[DataInvoicesHRD] where InvoiceNumber = '" + invoiceno + "' and InvoiceDate = '" + invoicedate + "'  " + Environment.NewLine;

            SqlDataReader myReader;
            int R = 0;
            myReader = GetlistofTables(Sql);

            if (myReader == null) {; return; }

            if (myReader.HasRows)
            {
                while (myReader.Read())
                {
                    ViewBag.lblCurrencyCode = myReader.GetValue(0).ToString();
                    ViewBag.lblClearanceCode = myReader.GetValue(1).ToString();
                    ViewBag.lblExchange = myReader.GetValue(2).ToString();
                    ViewBag.lblSettlementPeriod = myReader.GetValue(3).ToString();

                }
            }

            myReader.Close();
        }

        private SqlDataReader GetlistofTables(string agsql)
        {

            SqlConnection con = new SqlConnection(pbConnectionString);

            SqlDataReader myReader;
            myReader = null;

            //Tsql working
            try
            {
                SqlCommand command;
                con = new SqlConnection(pbConnectionString);
                con.Open();

                command = new SqlCommand();
                string SqlSelect = agsql;
                command.Connection = con;
                command.CommandText = SqlSelect;
                command = new SqlCommand(SqlSelect, con);
                // command.ExecuteNonQuery();
                myReader = command.ExecuteReader();
                while (con.State == ConnectionState.Executing)
                {
                    // stay here until query is not completed
                }
                //Do not colse from here
                // conn.Close();

            }
            catch (Exception e)
            {
                //  SendMessage(e.Message);

            }
            return myReader;
        }

        public void LoadedRejLineItemDetails(string agInvoiceNo, int agLineItemNumber, string seller)
        {

            int DetailNumber = 0;
            AcceptCounter = 0;
            RejectCounter = 0;
            RejTNAmt = 0;
            AcpTNAmt = 0;

            string Sql = "SELECT  Distinct f2.[InvoiceNo] AS [Invoice No],f1.[DetailNumber] AS [Detail Number],f1.[LineItemNumber] AS [LineItem Number],[BatchSequenceNumber] AS [Batch Sequence Number],[RecordSequenceWithinBatch]  AS [Record Sequence Within Batch],f2.[ChargeAmountName _ GrossBilled]  AS [Charge AmountName GrossBilled],f2.[ChargeAmountName _ GrossAccepted] AS [Charge AmountName GrossAccepted],f2.[ChargeAmountName _ GrossDifference]  AS [Charge AmountName GrossDifference],f2.[TotalNetAmount]  AS [Total Net Amount],";
            Sql = Sql + "TicketIssuingAirline  AS [Ticket Or FIM Issuing Airline],";
            Sql = Sql + "[CouponNo]  AS [Ticket Or FIM Coupon Number],[Ticketno]  AS [ Ticket Doc Or FIM Number],f1.[TaxType]  AS [Tax Type],f2.[TaxAmountName _ Billed]  AS [TaxAmount Name],f2.[TaxAmountName _ Accepted]  AS [Tax Amount Name Accepted],f2.[TaxAmountName _ Difference]  AS [Tax AmountName  Difference],";
            Sql = Sql + "'               ' as  [AddOnChargeName Allow], ";
            Sql = Sql + "'               ' as [AddOnCharge Percentage], ";
            Sql = Sql + " F2.[FromAirportCode],f2.[ToAirportCode], ";
            Sql = Sql + "'               ' as [AddOnCharge Amount Allow],";
            Sql = Sql + "'               ' as  [AddOnChargeName - Accepted], ";
            Sql = Sql + "'               ' as [AddOnCharge PercentageAcceped], ";
            Sql = Sql + "'               ' as [AddOnCharge Amount Accepted],";
            Sql = Sql + "'               ' as  [AddOnChargeName - Difference] ,";
            Sql = Sql + "'               ' as [AddOnCharge PercentageDifference], ";
            Sql = Sql + "'               ' as [AddOnCharge Amount Difference] ";

            //   '              ' as  [AddOnChargeName Allow], '               ' as [AddOnCharge Percentage], '               ' as [AddOnCharge Amount],'              ' as  [AddOnChargeName - Accepted], '               ' as [AddOnCharge Amount],'              ' as  [AddOnChargeName - Difference] , '               ' as [AddOnCharge Amount] ";
            Sql = Sql + "FROM [XmlFile].[DataLineItemDetails] F1 left join [XmlFile].[DataRejMemoCouponBrkDwn]  F2 on F1.[TransmissionId]  =F2.TransmissionId ";
            Sql = Sql + " AND F1.InvoiceNo= F2.InvoiceNo";
            Sql = Sql + " AND F1.DetailNumber = F2.DetailNumber  and f1.LineItemNumber = f2.LineItemNumber";
            Sql = Sql + " left join [XmlFile].[DataInvoicesHRD] F3 on F3.InvoiceNumber =  F1.[InvoiceNo] and f3.LineItemNumber=f1.LineItemNumber";
            Sql = Sql + " where f1.InvoiceNo <> '' ";
            Sql = Sql + " AND  F1.[InvoiceNo] = '" + agInvoiceNo + "' and not  f2.TicketDocNumber is null ";
            Sql = Sql + " and f3.ChargeCode=" + pvChargeCode;
            Sql = Sql + " and F1.SellerOrganization_OrganizationDesignator='" + seller + "'";
            Sql = Sql + " and f1.[LineItemNumber]=" + agLineItemNumber;
            //Sql = Sql + " Order by ";
            //Sql = Sql + "f2.[InvoiceNo] ";

            //Sql = Sql + ",f1.[LineItemNumber] ";
            //Sql = Sql + ",f2.[DetailNumber] ";
            //Sql = Sql + ",f1.[BatchSequenceNumber] ";
            //Sql = Sql + ",f1.[RecordSequenceWithinBatch] ";


            // Affichage Liste

            SqlConnection con = new SqlConnection(pbConnectionString);
            DataSet ds = new DataSet();
            SqlCommand cmd = new SqlCommand("" + Sql + "", con);

            SqlDataAdapter ada = new SqlDataAdapter(cmd);

            ada.Fill(ds);

            con.Close();

            int logInvoicesDetail = ds.Tables[0].Rows.Count;

            string[,] ListeInvoicesDetail = new string[27, logInvoicesDetail];
            int i = 0;
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                for (int j = 0; j < 27; j++)
                {
                    ListeInvoicesDetail[j, i] = dr[ds.Tables[0].Columns[j].ColumnName].ToString();
                    if (ListeInvoicesDetail[j, i] != "")
                    {
                        ViewBag.ListeInvoicesDetail = ListeInvoicesDetail;
                    }
                    else
                    {
                        ViewBag.ListeInvoicesDetail[j, i] = "0.00";
                    }

                }

                i++;
            }
            ViewBag.logInvoicesDetail = logInvoicesDetail;
            ViewBag.ListeInvoicesDetail = ListeInvoicesDetail;

            try
            {
                SqlDataReader myReader;
                int R = 0;
                myReader = GetlistofTables(Sql);

                if (myReader.HasRows == true)
                {
                    while (myReader.Read())
                    {
                        // Billing
                        int BillStat = -1;

                        BillStat = BillingSatatus(agInvoiceNo, agLineItemNumber, DetailNumber);

                        ViewBag.BillingStat = BillStat;

                        int d = Convert.ToInt16(WHERECLAUSE);

                        /*if (d != -1)
                        {
                            if (d == BillStat)
                            { dataGridView13.Rows[R].Visible = true; }
                            else { dataGridView13.Rows[R].Visible = false; }
                        }*/
                        if (BillStat != -1)
                        {
                            if (BillStat == 0)
                            {
                                // button15.Visible = true;

                                AcceptCounter++;

                                ViewBag.ListeInvoicesDetail[0, R] = "0";

                                try { AcpTNAmt = AcpTNAmt + Convert.ToDecimal(myReader.GetValue(5).ToString()); }
                                catch { }


                                // color tableaux  -1  Color.PaleGreen 

                            }
                            if (BillStat == 1)
                            {
                                // button15.Visible = true;

                                ViewBag.ListeInvoicesDetail[0, R] = "1";
                                RejectCounter++;
                                try { RejTNAmt = RejTNAmt + Convert.ToDecimal(myReader.GetValue(5).ToString()); }
                                catch { }

                                // color tableaux   Color.HotPink

                            }

                            if (BillStat == 2)
                            {
                                // button15.Visible = true;

                                ViewBag.ListeInvoicesDetail[0, R] = "1";
                                RejectCounter++;
                                try { RejTNAmt = RejTNAmt + Convert.ToDecimal(myReader.GetValue(5).ToString()); }
                                catch { }

                                // color tableaux  - 1 Color.Red
                            }

                        }
                        R++;

                    }// End While

                } // End My Reader HasRows


            }catch{
                     int Err = 0;
            }


            for (int ii = 0; ii < ViewBag.logInvoicesDetail; ii++)
            {

                //Sql = Sql + ",'              ' as  [AddOnChargeName Allow]";
                //Sql = Sql + " , '               ' as [AddOnChargePercentage]";
                //Sql = Sql + " , '               ' as [AddOnChargeAmount]";
                //Sql = Sql + ",'              ' as  [AddOnChargeName- Accepted]";
                //Sql = Sql + " , '               ' as [AddOnChargePercentage]";
                //Sql = Sql + " , '               ' as [AddOnChargeAmount]";
                //Sql = Sql + ",'              ' as  [AddOnChargeName- Difference]";
                //Sql = Sql + " , '               ' as [AddOnChargePercentage]";
                //Sql = Sql + " , '               ' as [AddOnChargeAmount]";

                string Inv = ListeInvoicesDetail[0, ii].ToString();
                string Tkt = ListeInvoicesDetail[11, ii].ToString();
                string Cpn = ListeInvoicesDetail[10, ii].ToString();
                string Dn = ListeInvoicesDetail[1, ii].ToString();

                if (pvChargeCode == "4") { }
                string Sql1 = "SELECT distinct [AddOnChargeName],[AddOnChargePercentage] ,[AddOnChargeAmount]";
                Sql1 = Sql1 + " FROM [XmlFile].[DataRejAddOnCharges]";
                Sql1 = Sql1 + " where [InvoiceNo]='" + Inv + "'";
                Sql1 = Sql1 + " and [TicketNo]=" + Tkt;
                Sql1 = Sql1 + " and  [CouponNo]=" + Cpn;
                Sql1 = Sql1 + " and DetailNumber =" + Dn;

                try
                {
                    SqlDataReader myReader;
                    int R = 0;
                    myReader = GetlistofTables(Sql1);

                    if (myReader == null) {; return; }
                    if (myReader.HasRows == true)
                    {

                        ViewBag.ListeInvoicesDetail[16, ii].Value = "";
                        ViewBag.ListeInvoicesDetail[17, ii].Value = "";
                        ViewBag.ListeInvoicesDetail[18, ii].Value = "";

                        ViewBag.ListeInvoicesDetail[19, ii].Value = "";
                        ViewBag.ListeInvoicesDetail[20, ii].Value = "";
                        ViewBag.ListeInvoicesDetail[21, ii].Value = "";

                        ViewBag.ListeInvoicesDetail[22, ii].Value = "ISCDifference";
                        ViewBag.ListeInvoicesDetail[23, ii].Value = "";
                        ViewBag.ListeInvoicesDetail[24, ii].Value = "0.000";

                        while (myReader.Read())
                        {

                            string Source = myReader.GetValue(0).ToString();

                            switch (Source)
                            {

                                case "ISCAllowed":
                                    ViewBag.ListeInvoicesDetail[16, ii] = myReader.GetValue(0).ToString();
                                    ViewBag.ListeInvoicesDetail[17, ii] = myReader.GetValue(1).ToString();
                                    ViewBag.ListeInvoicesDetail[18, ii] = myReader.GetValue(2).ToString();
                                    break;

                                case "ISCAccepted":
                                    ViewBag.ListeInvoicesDetail[19, ii] = myReader.GetValue(0).ToString();
                                    ViewBag.ListeInvoicesDetail[20, ii] = myReader.GetValue(1).ToString();
                                    ViewBag.ListeInvoicesDetail[21, ii] = myReader.GetValue(2).ToString();
                                    break;

                                case "ISCDifference":
                                    ViewBag.ListeInvoicesDetail[22, ii]= myReader.GetValue(0).ToString();
                                    ViewBag.ListeInvoicesDetail[23, ii] = myReader.GetValue(1).ToString();
                                    ViewBag.ListeInvoicesDetail[24, ii]= myReader.GetValue(2).ToString();
                                    break;
                            }
                            R++;
                        }
                    }

                    ViewBag.logInvoicesDetail = logInvoicesDetail;
                    ViewBag.ListeInvoicesDetail = ListeInvoicesDetail;

                    myReader.Close();
                    myReader.Dispose();
                    myReader = null;

                }
                catch
                {
                    int Err = 0;

                }
            }// End For 


            ViewBag.lblAcp = AcceptCounter.ToString() + " of " + ViewBag.label39;
            ViewBag.lblRej = RejectCounter.ToString() + " of " + ViewBag.label39;
            decimal Z = Convert.ToInt32(ViewBag.label39);
            decimal A = (AcceptCounter / Z) * 100;
            decimal B = (RejectCounter / Z) * 100;
            ViewBag.lblAcpPer = A.ToString("###.00") + "%";
            ViewBag.lblRejPer = B.ToString("###.00") + "%";

            ViewBag.lblRejTNAmt = RejTNAmt.ToString("#####.000");//  Convert.ToString(RejTNAmt);
            ViewBag.lblAcpTNAmt = AcpTNAmt.ToString("#####.000"); //Convert.ToString();

           // if (dataGridView13.Rows.Count > 0) { dataGridView13.Rows[0].Selected = true; dataGridView13_CellClick(null, null); }
        }


        public int BillingSatatus(string agInvoiceNo, int agLineItemNumbe, int DetailNumber)
        {
            Int32 BillingStat = -1;
            Int32 NOtinsystem = 0;
            string Sql = "SELECT  F1.[BillingStatus],F1.Processdate,F1.Notinsystem";
            Sql = Sql + " FROM [XmlFile].[InvoiceAnalytics] F1";
            Sql = Sql + " left join [XmlFile].[DataLineItemDetails] F2 on";
            Sql = Sql + " F1.[TransmisionId]=F2.[TransmissionId] And";
            Sql = Sql + " F1.[InvoiceNo] =F2.[InvoiceNo] And";
            Sql = Sql + " F1.[LineItemNumber]=F2.[LineItemNumber] And";
            Sql = Sql + " F1.[DetailNumber]=F2.[DetailNumber]";

            Sql = Sql + " Where ";
            Sql = Sql + " F1.[InvoiceNo] ='" + agInvoiceNo + "' And";
            Sql = Sql + " F1.[LineItemNumber]=" + agLineItemNumbe + " And";
            Sql = Sql + " F1.[DetailNumber]=" + DetailNumber;

            try
            {
                SqlDataReader myReader;
                int R = 0;
                myReader = GetlistofTables(Sql);

                if (myReader == null) {; }
                if (myReader.HasRows == true)
                {
                    while (myReader.Read())
                    {
                        BillingStat = Convert.ToInt32(myReader.GetValue(0).ToString());
                        NOtinsystem = Convert.ToInt32(myReader.GetValue(2).ToString());

                           // button15.Visible = true;
                            DateTime dt = myReader.GetDateTime(1);
                            string Alprocess = "ALREADY PROCESSED :" + String.Format("{0:yyyy-MMM-dd HH:mm:ss}", dt); ;
                            ViewBag.button15 = Alprocess;
                           
                          //  button15 = "1";
                       
                        if (NOtinsystem == 1) { BillingStat = 2; }
                    }
                }

                myReader.Close();
                myReader.Dispose();
                myReader = null;
                // Close the connection when done with it.

            }
            catch
            {
                int Err = 0;

            }


            return BillingStat;

        }


        public void LoadedLineItemDetails(string agInvoiceNo, int agLineItemNumber, string seller)
        {

            int DetailNumber = 0;
            AcceptCounter = 0;
            RejectCounter = 0;
            RejTNAmt = 0;
            AcpTNAmt = 0;

            string Sql = " SELECT  Distinct F1.[InvoiceNo] AS [Invoice No] , f1.[DetailNumber] AS [Detail Number],f1.[LineItemNumber] AS [LineItem Number],[BatchSequenceNumber] AS [Batch Sequence Number] ,";
            Sql = Sql + " [RecordSequenceWithinBatch]  AS [Record Sequence Within Batch] ,[ChargeAmountName _ GrossBilled]  AS [Charge AmountName GrossBilled] ,[TaxType]  AS [Tax Type],[TaxAmountName _ Billed]  AS [TaxAmount Name],";
            Sql = Sql + " [TotalNetAmount]  AS [Total Net Amount] ,[TicketOrFIMIssuingAirline]  AS [Ticket Or FIM Issuing Airline],[TicketOrFIMCouponNumber]  AS [Ticket Or FIM Coupon Number],";
            Sql = Sql + " [TicketDocOrFIMNumber]  AS [ Ticket Doc Or FIM Number],[CheckDigit]  AS [Check Digit],[CurrAdjustmentIndicator]  AS [Curr Adjustment Indicator],[ElectronicTicketIndicator]  AS [Electronic Ticket Indicator],";
            Sql = Sql + " [AirlineFlightDesignator]  AS [Airline Flight Designator] ,[FlightNo]  AS [Flight No] ,[FlightDate]  AS [Flight Date],[FromAirportCode]  AS [From Airport Code],[ToAirportCode]  AS [To Airport Code],";
            Sql = Sql + " [SettlementAuthorizationCode]  AS [Settlement Authorization Code] ,[AttachmentIndicatorOriginal]  AS [Attachment Indicator Original] ,[AttachmentIndicatorValidated]  AS [Attachment Indicator Validated] ,";
            Sql = Sql + " [NumberOfAttachments]  AS [Number Of Attachments] ,[TaxAmountName _ Accepted]  AS [Tax Amount Name Accepted] ,[TaxAmountName _ Difference]  AS [Tax AmountName  Difference],";

            Sql = Sql + " F2.AddOnChargeName	,F2.AddOnChargePercentage	,F2.AddOnChargeAmount";
            Sql = Sql + " FROM [XmlFile].[DataLineItemDetails] F1";
            Sql = Sql + " left join [XmlFile].[DataAddOnCharges] F2 on  f1.InvoiceNo=f2.InvoiceNo and F1.TicketDocOrFIMNumber=f2.TicketNo and f1.TicketOrFIMCouponNumber=f2.CouponNo";

            Sql = Sql + " where  f1.SellerOrganization_OrganizationDesignator = '" + seller + "'  and  f1.InvoiceNo= '" + agInvoiceNo + "'";
            if (pvChargeCode != "44") { Sql = Sql + " AND F1.[LineItemNumber] =" + agLineItemNumber; }


            // Affichage Liste
            SqlConnection con = new SqlConnection(pbConnectionString);
            DataSet ds = new DataSet();
            SqlCommand cmd = new SqlCommand("" + Sql + "", con);

            SqlDataAdapter ada = new SqlDataAdapter(cmd);

            ada.Fill(ds);

            con.Close();

            int logInvoicesDetail = ds.Tables[0].Rows.Count;

            string[,] ListeInvoicesDetail = new string[27, logInvoicesDetail];
            int i = 0;
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                for (int j = 0; j < 27; j++)
                {
                    ListeInvoicesDetail[j, i] = dr[ds.Tables[0].Columns[j].ColumnName].ToString();
                    if (ListeInvoicesDetail[j, i] != "")
                    {
                        ViewBag.ListeInvoicesDetail = ListeInvoicesDetail;
                    }
                    else
                    {
                        ViewBag.ListeInvoicesDetail[j, i] = "0.00";
                    }

                }

                i++;
            }

            ViewBag.logInvoicesDetail = logInvoicesDetail;
            ViewBag.ListeInvoicesDetail = ListeInvoicesDetail;


            try
            {
                SqlDataReader myReader;
                int R = 0;
                myReader = GetlistofTables(Sql);

                if (myReader.HasRows == true)
                {
                    while (myReader.Read())
                    {
                        int BillStat = -1;

                        BillStat = BillingSatatus(agInvoiceNo, agLineItemNumber, DetailNumber);

                        int d = Convert.ToInt16(WHERECLAUSE);

                       /* if (d != -1)
                        {
                            if (d == BillStat)
                            { dataGridView13.Rows[R].Visible = true; }
                            else { dataGridView13.Rows[R].Visible = false; }
                        }*/

                        if (BillStat != -1)
                        {
                            if (BillStat == 2)
                            {

                                ListeInvoicesDetail[0, R] = "1";
                                RejectCounter++;
                                try { RejTNAmt = RejTNAmt + Convert.ToDecimal(myReader.GetValue(5).ToString()); }
                                catch { }

                            }

                            if (BillStat == 0)
                            {
                              
                                AcceptCounter++;
                                ListeInvoicesDetail[0, R] = "0";
                                AcpTNAmt = AcpTNAmt + Convert.ToDecimal(myReader.GetValue(5).ToString());
                               
                            }


                            if (BillStat == 1)
                            {
                                ListeInvoicesDetail[0, R] = "1";
                                RejectCounter++;
                                RejTNAmt = RejTNAmt + Convert.ToDecimal(myReader.GetValue(5).ToString());
                               

                            }

                        }
                        R++;

                    }// End While

                } // End My Reader HasRows


            }
            catch
            {
                int Err = 0;
            }

            ViewBag.lblAcp = AcceptCounter.ToString() + " of " + ViewBag.label39;
            ViewBag.lblRej = RejectCounter.ToString() + " of " + ViewBag.label39;
            decimal Z = Convert.ToInt32(ViewBag.label39);
            decimal A = (AcceptCounter / Z) * 100;
            decimal B = (RejectCounter / Z) * 100;
            ViewBag.lblAcpPer = A.ToString("###.00") + "%";
            ViewBag.lblRejPer = B.ToString("###.00") + "%";

            ViewBag.lblRejTNAmt = RejTNAmt.ToString("#####.000");//  Convert.ToString(RejTNAmt);
            ViewBag.lblAcpTNAmt = AcpTNAmt.ToString("#####.000"); //Convert.ToString();

        }

        public void LoadedBMCMLineItemDetails(string agInvoiceNo, int agLineItemNumber, string seller)
        {

            int DetailNumber = 0;
            AcceptCounter = 0;
            RejectCounter = 0;
            RejTNAmt = 0;
            AcpTNAmt = 0;

            string Sql = " SELECT  Distinct f1.[InvoiceNo] AS [Invoice No],f1.[DetailNumber] AS [Detail Number],[LineItemNumber] AS [LineItem Number],[BatchSequenceNumber] AS [Batch Sequence Number],";
            Sql = Sql + "[RecordSequenceWithinBatch]  AS [Record Sequence Within Batch],f2.[ChargeAmountName _ GrossBilled]  AS [Charge AmountName GrossBilled],";
            Sql = Sql + "f2.[ChargeAmountName _ GrossAccepted] AS [Charge AmountName GrossAccepted],f2.[ChargeAmountName _ GrossDifference]  AS [Charge AmountName GrossDifference], ";
            Sql = Sql + "f2.[TotalNetAmount]  AS [Total Net Amount], f2.TicketIssuingAirline  AS [Ticket Or FIM Issuing Airline], f2.[CouponNumber]  AS [Ticket Or FIM Coupon Number], ";
            Sql = Sql + "f2.[TicketDocNumber]  AS [ Ticket Doc Or FIM Number],f3.BillingMemoNumber AS [Billing Memo Number], f2.[TaxType]  AS [Tax Type],f2.[TaxAmountName _ Billed]  AS [TaxAmount Name], ";
            Sql = Sql + "f2.[TaxAmountName _ Accepted]  AS [Tax Amount Name Accepted],f1.[TaxAmountName _ Difference]  AS [Tax AmountName  Difference],'              ' as  [AddOnChargeName Allow], ";
            Sql = Sql + " '               ' as [AddOnCharge Percentage], '               ' as [AddOnCharge Amount],'              ' as  [AddOnChargeName - Accepted],  ";
            Sql = Sql + "'               ' as [AddOnCharge Amount],'              ' as  [AddOnChargeName - Difference] , '               ' as [AddOnCharge Amount]  ";
            Sql = Sql + " FROM [XmlFile].[DataLineItemDetails] F1 left join [XmlFile].[BMCMCouponBreakdown]  F2 on F1.InvoiceNo= F2.InvoiceNo AND F1.[TransmissionId]  =F2.TransmissionId AND F1.DetailNumber = F2.DetailNumber  ";
            Sql = Sql + "left join [XmlFile].[BillingMemoDetails]  F3  on F2.InvoiceNo= F2.InvoiceNo AND F2.[TransmissionId]  =F2.TransmissionId  AND F2.DetailNumber = F2.DetailNumber ";
            Sql = Sql + "where f1.InvoiceNo <> ''  ";
            Sql = Sql + " AND  F1.[InvoiceNo] = '" + agInvoiceNo + "' and  f1.SellerOrganization_OrganizationDesignator = '" + seller + "' ";


            // Affichage Liste
            SqlConnection con = new SqlConnection(pbConnectionString);
            DataSet ds = new DataSet();
            SqlCommand cmd = new SqlCommand("" + Sql + "", con);

            SqlDataAdapter ada = new SqlDataAdapter(cmd);

            ada.Fill(ds);

            con.Close();

            int logInvoicesDetail = ds.Tables[0].Rows.Count;

            string[,] ListeInvoicesDetail = new string[27, logInvoicesDetail];
            int i = 0;
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                for (int j = 0; j < 27; j++)
                {
                    ListeInvoicesDetail[j, i] = dr[ds.Tables[0].Columns[j].ColumnName].ToString();
                    if (ListeInvoicesDetail[j, i] != "")
                    {
                        ViewBag.ListeInvoicesDetail = ListeInvoicesDetail;
                    }
                    else
                    {
                        ViewBag.ListeInvoicesDetail[j, i] = "0.00";
                    }

                }

                i++;
            }

            ViewBag.logInvoicesDetail = logInvoicesDetail;
            ViewBag.ListeInvoicesDetail = ListeInvoicesDetail;


            try
            {
                SqlDataReader myReader;
                int R = 0;
                myReader = GetlistofTables(Sql);

                if (myReader.HasRows == true)
                {
                    while (myReader.Read())
                    {
                        int BillStat = -1;

                        BillStat = BillingSatatus(agInvoiceNo, agLineItemNumber, DetailNumber);

                        int d = Convert.ToInt16(WHERECLAUSE);

                        /* if (d != -1)
                         {
                             if (d == BillStat)
                             { dataGridView13.Rows[R].Visible = true; }
                             else { dataGridView13.Rows[R].Visible = false; }
                         }*/

                        if (BillStat != -1)
                        {
                            if (BillStat == 0)
                            {

                                AcceptCounter++;
                                ListeInvoicesDetail[0, R] = "0";
                                AcpTNAmt = AcpTNAmt + Convert.ToDecimal(myReader.GetValue(5).ToString());

                            }
                            else
                            {
                                ListeInvoicesDetail[0, R] = "1";
                                RejectCounter++;
                                RejTNAmt = RejTNAmt + Convert.ToDecimal(myReader.GetValue(5).ToString());
                            }
                        }

                        R++;

                    }// End While

                } // End My Reader HasRows


            }
            catch
            {
                int Err = 0;
            }


            // AddOnCharges//

            for (int k = 0; k < logInvoicesDetail; k++)
            {

                string Inv = ListeInvoicesDetail[0, k].ToString();
                string Tkt = ListeInvoicesDetail[11, k].ToString();
                string Cpn = ListeInvoicesDetail[10, k].ToString();
                string Dn = ListeInvoicesDetail[1, k].ToString();

                string Sql1 = "SELECT distinct [AddOnChargeName],[AddOnChargePercentage] ,[AddOnChargeAmount]";
                Sql1 = Sql1 + " FROM [XmlFile].[DataRejAddOnCharges]";
                Sql1 = Sql1 + " where [InvoiceNo]='" + Inv + "'";
                Sql1 = Sql1 + " and [TicketNo]=" + Tkt;
                Sql1 = Sql1 + " and  [CouponNo]=" + Cpn;
                Sql1 = Sql1 + " and DetailNumber =" + Dn;

                try
                {
                    SqlDataReader myReader;
                    int R = 0;
                    myReader = GetlistofTables(Sql1);

                    if (myReader == null) {; return; }
                    if (myReader.HasRows == true)
                    {
                        while (myReader.Read())
                        {

                            string Source = myReader.GetValue(0).ToString();


                            switch (Source)
                            {

                                case "ISCAllowed":
                                    ListeInvoicesDetail[16, k] = myReader.GetValue(0).ToString();
                                    ListeInvoicesDetail[17, k] = myReader.GetValue(1).ToString();
                                    ListeInvoicesDetail[18, k] = myReader.GetValue(2).ToString();
                                    break;
                                case "ISCAccepted":
                                    ListeInvoicesDetail[19, k] = myReader.GetValue(0).ToString();
                                    ListeInvoicesDetail[20, k] = myReader.GetValue(2).ToString();

                                    break;

                                case "ISCDifference":
                                    ListeInvoicesDetail[21, k] = myReader.GetValue(0).ToString();
                                    ListeInvoicesDetail[22, k] = myReader.GetValue(2).ToString();

                                    break;
                            }

                            R++;
                        }
                    }

                }
                catch
                {
                    int Err = 0;

                }

            }

            ViewBag.lblAcp = AcceptCounter.ToString() + " of " + ViewBag.label39;
            ViewBag.lblRej = RejectCounter.ToString() + " of " + ViewBag.label39;
            decimal Z = Convert.ToInt32(ViewBag.label39);
            decimal A = (AcceptCounter / Z) * 100;
            decimal B = (RejectCounter / Z) * 100;
            ViewBag.lblAcpPer = A.ToString("###.00") + "%";
            ViewBag.lblRejPer = B.ToString("###.00") + "%";

            ViewBag.lblRejTNAmt = RejTNAmt.ToString("#####.000");//  Convert.ToString(RejTNAmt);
            ViewBag.lblAcpTNAmt = AcpTNAmt.ToString("#####.000"); //Convert.ToString();

        }


        /*click   Interline Invoice   */
        public ActionResult ChangeInterlineInvoice()
        {
            // get Combo All
            AllItem();

            // Interline Invoice Liste General et Query
            string valProcess = Request["param21"];
            string cboIVInvoiceNumber = Request["valInvoiceNumber"];
            string cboBillingPeriod = Request["valbillingPeriod"];
            string cboChargeCode = Request["valchargeCode"];
            string cboProStatus = Request["valprocesStatus"];

            LoadedInvoices(cboIVInvoiceNumber, cboBillingPeriod, cboChargeCode, cboProStatus);

            // End Interline  Liste Invoice General et Query


            string invoiceno = Request["param1"];
            int LineItemNumber = Request["param11"].AsInt();
            ViewBag.lblBillingPeriod = Request["param9"];
            pvChargeCode = Request["param12"];
            pvInvoiceDate = Request["param2"];
            pvTransmissionID = Request["param0"];
            string seller = Request["param6"];
            pvseller = seller;

            ViewBag.label33 = Request["param13"];
            ViewBag.label34 = Request["param14"];
            ViewBag.label36 = Request["param15"];
            ViewBag.label37 = Request["param16"];
            ViewBag.label38 = Request["param17"];
            ViewBag.label39 = Request["param18"];

            ViewBag.lblIsc = Request["param20"];
            ViewBag.label47 = invoiceno;
            ViewBag.label45 = LineItemNumber.ToString();
            GeneralPeriod = Request["param9"];

            /* Currency  */

            Termofpayment(invoiceno, pvInvoiceDate);


            // REJECTION
            if (pvChargeCode == "4") { LoadedRejLineItemDetails(invoiceno, LineItemNumber, seller); }
            if (pvChargeCode == "5") { LoadedRejLineItemDetails(invoiceno, LineItemNumber, seller); }
            if (pvChargeCode == "6") { LoadedRejLineItemDetails(invoiceno, LineItemNumber, seller); }

            if (pvChargeCode == "25") { LoadedLineItemDetails(invoiceno, LineItemNumber, seller); }

            // FIM
            if (pvChargeCode == "14") { LoadedLineItemDetails(invoiceno, LineItemNumber, seller); }

            if (pvChargeCode == "44") { LoadedRejLineItemDetails(invoiceno, LineItemNumber, seller); }
            if (pvChargeCode == "45") { LoadedRejLineItemDetails(invoiceno, LineItemNumber, seller); }
            if (pvChargeCode == "46") { LoadedRejLineItemDetails(invoiceno, LineItemNumber, seller); }

            //PRIME
            if (pvChargeCode == "1") { LoadedLineItemDetails(invoiceno, LineItemNumber, seller); }
            if (pvChargeCode == "2") { LoadedLineItemDetails(invoiceno, LineItemNumber, seller); }


            //Billing Memo
            if (pvChargeCode == "9") { LoadedBMCMLineItemDetails(invoiceno, LineItemNumber, seller); }


            string Alprocess = "ALREADY PROCESSED :" + String.Format("{0:yyyy-MMM-dd}", valProcess); ;
            ViewBag.button15 = Alprocess;


            return PartialView();
        }
        /* End click Interline Invoice */


        /*  Cell Click LoadIterline Ivoice  Joseph*/

        // Declaration 
        public ActionResult ChargementAllInterlin()
        {

            Boolean ticketnoexist = false;

            ViewBag.lblDieFlown = "0.000";
            ViewBag.lblDieInterline = "0.000";
            ViewBag.lblDieDiff = "0.000";

            int RX = Request["selection"].AsInt();

            /* Get All index Valeur  dataGridView13 */
            string dataGridView13_1_RX = Request["index1"];
            string dataGridView13_0_RX = Request["index0"];
            string dataGridView13_10_RX = Request["index10"];
            string dataGridView13_11_RX = Request["index11"];
            string dataGridView13_9_RX = Request["index9"];
            /* End Get All index Valeur */


            /* GET All index valeur dataGridView12 */
            string dataGridView12_5_RX1 = Request["grid5"];
            /* End GET All index valeur dataGridView12 */


            try
            {
                string Dn = dataGridView13_1_RX;
                string Inv = dataGridView13_0_RX; ;
                pvInvoiceNumber = Inv;

                int agcouponNo = Convert.ToInt16(dataGridView13_10_RX);
                pvDetailNumber = Convert.ToInt16(dataGridView13_1_RX);
                string agTicket = dataGridView13_11_RX;

                Alc = dataGridView13_9_RX.Trim();

                // Alc ="771"; // dataGridView13[7, RX].Value.ToString().Trim();//PC
                if (Alc == "")
                {
                   // int RX1 = dataGridView12.CurrentRow.Index;

                    Alc = dataGridView12_5_RX1;

                }
                Bp = "";// dgvTaxes[0, e.RowIndex].Value.ToString();
                //  GetRelatedTax(Alc, Tk, Cpn, Bp);
                Tk = agTicket;

                ViewBag.lblDocNo = Alc + agTicket;
                ViewBag.lblCpn = agcouponNo.ToString();

                Cpn = agcouponNo.ToString();

                if (RX == 103)
                {
                    int w = 0;
                }

                GetPassenger(Alc + agTicket);
                GetSelectedISC(agcouponNo, agTicket, Dn, pvInvoiceNumber);

                pvAlc = Alc;
                pvTicketNo = agTicket;
                pvCoupontNo = Cpn;

                pvFlightDate = "";
                pvSecFrom = "";
                pvSecTo = "";

                /*  Mbola tsy tafiditra/ GetCoupondetailsLineItemDetail(Alc + agTicket, agcouponNo.ToString());
                */




            }
            catch
            {

            }

            return PartialView("ChangeInterlineInvoice");
        }


        /* All function Interline Load */
        private void GetPassenger(string DocNo)
        {

            ViewBag.lblPaxName = "";
            ViewBag.lblPaxT = "";


            string Sql = "SELECT [PassengerName],[PaxType] FROM [Pax].[SalesDocumentHeader]";
            Sql = Sql + " WHERE  [DocumentNumber]='" + DocNo + "'";
            try
            {
                SqlDataReader myReader;
                int R = 0;
                myReader = GetlistofTables(Sql);

               // comboBox1.Items.Clear();

                if (myReader == null) {; return; }
                if (myReader.HasRows == true)
                {
                    while (myReader.Read())
                    {
                        ViewBag.lblPaxName = myReader.GetValue(0).ToString();
                        ViewBag.lblPaxT = myReader.GetValue(1).ToString();
                    }
                }
                // always call Close when done reading.
                myReader.Close();
                myReader.Dispose();
                myReader = null;
               
            }
            catch { }
        }

        private void GetSelectedISC(int agcouponNo, string agTicket, string Dn, string invoiceNum)
        {

                string Sql = " SELECT  distinct ";
                Sql = Sql + "[InvoiceNo] as [Invoice No]";
                Sql = Sql + ",[TicketNo] as [Ticket No]";
                Sql = Sql + ",[CouponNo] as [Coupon No]";
                Sql = Sql + ",[AddOnChargeName] as [ISC]";
                Sql = Sql + ",[AddOnChargePercentage] as [ISC %] ";
                Sql = Sql + ",[AddOnChargeAmount] as [ISC Amount]";
                Sql = Sql + "FROM [XmlFile].[DataAddOnCharges] ";
                Sql = Sql + " where InvoiceNo='" + invoiceNum + "'";
                Sql = Sql + " and TicketNo='" + agTicket + "'";
                Sql = Sql + " and CouponNo=" + agcouponNo;
                Sql = Sql + " and DetailNumber=" + Dn;


            SqlConnection con = new SqlConnection(pbConnectionString);
            DataSet ds = new DataSet();
            SqlCommand cmd = new SqlCommand("" + Sql + "", con);

            SqlDataAdapter ada = new SqlDataAdapter(cmd);

            ada.Fill(ds);

            con.Close();

            int logSelectedISC = ds.Tables[0].Rows.Count;

            string[,] ListeSelectedISC = new string[22, logSelectedISC];
            int i = 0;
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                for (int j = 0; j < 6; j++)
                {

                    ListeSelectedISC[j, i] = dr[ds.Tables[0].Columns[j].ColumnName].ToString();
                }

                i++;
            }

            ViewBag.label47 = invoiceNum;

           ViewBag.logSelectedISC = logSelectedISC;
            ViewBag.ListeSelectedISC = ListeSelectedISC;


        }

        /* End All function Interline Load*/

        /*************************************************

          private void dataGridView13_CellClick(object sender, DataGridViewCellEventArgs e)
        {

            Boolean ticketnoexist = false;
            lblDieFlown.Text = "0.000";
            lblDieInterline.Text = "0.000";
            lblDieDiff.Text = "0.000";

       
            if (dataGridView13.Rows.Count == 0) { return; }
            int RX = 0;
            if (dataGridView13.Rows.Count == 1) { RX = 0; }

            else
            {

                try { RX = e.RowIndex; }
                catch { RX = dataGridView13CurrentRowIndex; }
                // 
            }

            try
            {

                string Dn = dataGridView13[1, RX].Value.ToString();
                string Inv = dataGridView13[0, RX].Value.ToString(); ;
                pvInvoiceNumber = Inv;
                int agcouponNo = Convert.ToInt16(dataGridView13[10, RX].Value);
                pvDetailNumber = Convert.ToInt16(dataGridView13[1, RX].Value);
                string agTicket = dataGridView13[11, RX].Value.ToString();
                Alc = dataGridView13[9, RX].Value.ToString().Trim();
                // Alc ="771"; // dataGridView13[7, RX].Value.ToString().Trim();//PC
                if (Alc == "")
                {
                    int RX1 = dataGridView12.CurrentRow.Index;
                    Alc = dataGridView12[5, RX1].Value.ToString();

                }
                Bp = "";// dgvTaxes[0, e.RowIndex].Value.ToString();
                //  GetRelatedTax(Alc, Tk, Cpn, Bp);
                Tk = agTicket;
                lblDocNo.Text = Alc + agTicket;
                lblCpn.Text = agcouponNo.ToString();
                Cpn = agcouponNo.ToString();

                if (RX == 103)
                {
                    int w = 0;
                }

        

                GetPassenger(Alc + agTicket);

                GetSelectedISC(agcouponNo, agTicket, Dn);


                pvAlc = Alc;
                pvTicketNo = agTicket;
                pvCoupontNo = Cpn;

                pvFlightDate = "";
                pvSecFrom = "";
                pvSecTo = "";

            /* Mbola tsy tafiditra

                GetCoupondetailsLineItemDetail(Alc + agTicket, agcouponNo.ToString());
                if (label51.Text.Trim().Length < 4)
                {
                    ticketnoexist = true;
                }
                dataGridView23.Columns.Clear();
                dataGridView24.Columns.Clear();
                
            end mbola tsy tafiditra
            ////////////

                // ambony ok


                if (pvChargeCode == "4")
                {
                    OB2ndRej(agTicket, agcouponNo.ToString());
                    OBData(agTicket, agcouponNo.ToString(), Inv, Dn);

                }

                //DataGridViewRow selectedRow = dataGridView13.Rows[e.RowIndex];
                //DataGridView1.Rows[e.RowIndex].Selected = true;

                dataGridView14.Rows.Clear();
                dataGridView14.Columns.Clear();

                string Sql = "SELECT  distinct " + Environment.NewLine;
                Sql += " [InvoiceNo] AS [Invoice No]" + Environment.NewLine;
                Sql += " ,[TicketNo] as [Ticket No]" + Environment.NewLine;
                Sql += " ,[CouponNo] as [Coupon No]" + Environment.NewLine;
                Sql += " ,[TaxCode] as [Tax Code]" + Environment.NewLine;
                Sql += " ,[TaxAmountName _ Billed] as [TaxAmt Billed] " + Environment.NewLine;
                if (pvChargeCode == "4" || pvChargeCode == "5" || pvChargeCode == "6")
                {
                    Sql += " ,[TaxAmountName _ Accepted] as [TaxAmt Accepted] " + Environment.NewLine;
                    Sql += " ,[TaxAmountName _ Difference]  as  [TaxAmt Difference] " + Environment.NewLine;                    
                    Sql += " FROM [XmlFile].[DataRejMemoTXBrkDwn]" + Environment.NewLine;
                }
                else
                {
                    Sql += " FROM [XmlFile].[DataLineItemDetetailsTXBrkDnw]" + Environment.NewLine;
                }


                Sql += "  where InvoiceNo='" + label47.Text.Trim() + "'" + Environment.NewLine;
                Sql += "  and TicketNo='" + agTicket + "'" + Environment.NewLine;
                Sql += "  and CouponNo=" + agcouponNo + Environment.NewLine;

                //if (pvChargeCode == "4" || pvChargeCode == "5" || pvChargeCode == "6")
                //{

                Sql += "  and DetailNumber =" + Dn + Environment.NewLine;

                //}


                try
                {
                    SqlDataReader myReader;
                    int R = 0;
                    myReader = GetlistofTables(Sql);

                    if (myReader == null) { ; return; }


                    for (int i = 0; i < myReader.FieldCount; i++)
                    {
                        dataGridView14.Columns.Add(i.ToString(), myReader.GetName(i));
                    }
                  
                    dataGridView14.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                    dataGridView14.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                    dataGridView14.RowHeadersVisible = false;
                    dataGridView14.MultiSelect = false;
                    dataGridView14.EnableHeadersVisualStyles = false;
                    dataGridView14.ColumnHeadersDefaultCellStyle.BackColor = Color.Maroon;
                    dataGridView14.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;


                    for (int i = 0; i < dataGridView14.ColumnCount; i++)
                    {
                        try
                        {
                            this.dataGridView14.Columns[i].ReadOnly = true;
                            this.dataGridView14.Columns[i].DefaultCellStyle.BackColor = Color.Khaki;
                            this.dataGridView14.Columns[1].Width = 100;
                            this.dataGridView14.Columns[i].Resizable = 0;
                            this.dataGridView14.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                            this.dataGridView14.Columns[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                        }
                        catch { }

                    }


                    dataGridView13.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                    dataGridView13.MultiSelect = false;
                    // dataGridView13.RowPrePaint += new DataGridViewRowPrePaintEventHandler(dataGridView14_RowPrePaint);


                    if (myReader.HasRows == true)
                    {

                        while (myReader.Read())
                        {
                            dataGridView14.Rows.Add();
                            for (int i = 0; i < myReader.FieldCount; i++)
                            {
                                dataGridView14[i, R].Value = myReader.GetValue(i).ToString();
                            }


                            R++;

                        }

                        for (int i = 0; i < 2; i++)
                        {
                            dataGridView14.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                        }


                    }

                    this.dataGridView14.Columns[2].Width = 45;
                    this.dataGridView14.Columns[3].Width = 35;
                    this.dataGridView14.Columns[4].Width = 50;



                    myReader.Close();
                    myReader.Dispose();
                    myReader = null;
                    // Close the connection when done with it.

                    DbClose(); //OB_TotalLine();

                }
                catch
                {
                    int Err = 0;
                }



            }
            catch { }

            try
            {
                for (int i = 0; i < 2; i++)
                {
                    dataGridView14.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                }


                TaxVariance();
                if (pvChargeCode == "1" || pvChargeCode == "2" || pvChargeCode == "4" || pvChargeCode == "5" || pvChargeCode == "6")
                {
                    PRORATIONENGINE.ProrationBatchProratecs bp = new PRORATIONENGINE.ProrationBatchProratecs();
                    string Tkt = Tk; ;
                    string Airline = Alc;
                    string Tkt2 = pvTicketNo;
                    if (Tkt2.Length == 10)
                    {
                        Tkt2 = Alc + Tk;
                    }

                    string CouponNumber = Cpn;
                    //string Period = label197.Text;
                    string Period = "20"+lblSettlementPeriod.Text.Substring(0,4);
                    string airline = Airline;
                    string strDocumentNumber = Tkt2;
                    int iHasCoupon = Convert.ToInt16(CouponNumber);

                    //  Call Proration Method
                    bp.RunIB(Period, airline, iHasCoupon, strDocumentNumber);
                    // bp.RunIB(string Period, string airline, int iHasCoupon, string strDocumentNumber);
                    bp = null;

                    GetFinalShare1(Alc + Tk, Cpn);
                    try
                    {
                        if (dataGridView17.Rows.Count > 0)
                        { dataGridView17_CellClick(null, null); }
                    }
                    catch { }
                    GetSpaApplies(Alc + Tk, Cpn);
                    Die(Alc + Tk, Cpn);

                }

                if (RX != -1)
                {




                   if (pvChargeCode == "1" || pvChargeCode == "4" || pvChargeCode == "5" || pvChargeCode == "6")
                    {
                        GetRejAnalytics(RX);
                    }
                    else
                    {
                        GetAnalytics(RX);

                    }
                    decimal A = 0;
                    decimal B = 0;
                    decimal C = 0;
                    decimal S = 0;
                    txtFS_SPA.Text = "";
                    try { A = Convert.ToDecimal(txtAnaFShare.Text); }
                    catch { A = 0; };
                    try { B = Convert.ToDecimal(txtSPAAMOUNT.Text); ;}
                    catch { B = 0; };
                    if (lblSPAApplies.Text == "Y") { try { S = Convert.ToDecimal(txtSurcharge.Text); ;} catch { S = 0; }; }

                    C = A - (B + S);

                    txtFS_SPA.Text = C.ToString("####.000");
                    decimal S1 = B + S;
                    if (B > 0) { txtAppliedNetAmount.Text = S1.ToString("####.000"); }
                    else { txtAppliedNetAmount.Text = A.ToString("####.000"); }

                }

                textBox13.Text = "";

                lblAcceptMsg.Visible = false;
                lblAcceptMsg.Text = "";
                if (lblAnalMessage.Text.Trim().Length == 0)
                {
                    AcceptCounter++;
                    dataGridView13[0, RX].Tag = "0";
                    for (int col1 = 0; col1 < dataGridView13.Columns.Count - 1; col1++)
                    {
                        dataGridView13[col1, RX].Style.BackColor = Color.PaleGreen;
                    }
                    //  lblAcceptMsg.Text = "ADM needs to be issued or adjustment made to proration";
                    // lblAcceptMsg.Visible = true;




                    if (txtDiff1.Text.Trim().Length > 0)
                    {
                        if (Math.Abs(Convert.ToDecimal(txtDiff1.Text)) != 0)
                        {
                            if (Math.Abs(Convert.ToDecimal(txtDiff1.Text)) >= -5 && Math.Abs(Convert.ToDecimal(txtDiff1.Text)) <= 5)
                            {
                                lblAcceptMsg.Text = "ADM needs to be issued or adjustment made to proration";
                                lblAcceptMsg.Visible = true;
                            }

                        }



                    }



                    if (txtDiff2.Text.Trim().Length > 0)
                    {
                        if (Math.Abs(Convert.ToDecimal(txtDiff2.Text)) != 0)
                        {
                            if (Math.Abs(Convert.ToDecimal(txtDiff2.Text)) <= 2)
                            {
                                lblAcceptMsg.Text = "ADM needs to be issued or adjustment made to proration";
                                lblAcceptMsg.Visible = true;
                            }

                        }

                    }




                }
                else
                {
                    dataGridView13[0, RX].Tag = "1";
                    RejectCounter++;
                    for (int col1 = 0; col1 < dataGridView13.Columns.Count - 1; col1++)
                    {
                        dataGridView13[col1, RX].Style.BackColor = Color.HotPink;

                    }
                    lblAcceptMsg.Text = "Billing Rejected";
                    lblAcceptMsg.Visible = true;

                    if (ticketnoexist == true)
                    {
                        for (int col1 = 0; col1 < dataGridView13.Columns.Count - 1; col1++)
                        {
                            dataGridView13[col1, RX].Style.BackColor = Color.Red;
                            dataGridView13[col1, RX].Style.ForeColor = Color.White;

                        }
                        lblAcceptMsg.Text = "Billing Rejected - Ticket Not Recorded In System";
                    }

                }

                bool Checked = CheckDuplicate(Alc + Tk, Cpn, pvChargeCode, pvInvoiceNumber);
                if (Checked == true)
                {
                    dataGridView13[0, RX].Tag = "1";
                    dataGridView13[3, RX].Tag = "D";
                    dataGridView13[4, RX].Tag = CheckDuplicateInvoive(Alc + Tk, Cpn, pvChargeCode, pvInvoiceNumber);
                    for (int col1 = 0; col1 < dataGridView13.Columns.Count - 1; col1++)
                    {
                        dataGridView13[col1, RX].Style.BackColor = Color.Blue;

                    }
                    lblAcceptMsg.Text = "Duplicate Billing";
                    lblAcceptMsg.Visible = true;

                }





                foreach (Control Ctrl in panel88.Controls)
                {

                    string CCC = Ctrl.GetType().Name.ToString();

                    if (Ctrl.GetType().Name.ToString() == "TextBox")
                    {
                        if (Ctrl.Text.ToString().Trim().Length == 0) { Ctrl.Text = "0.000"; }

                    }

                }


            }
            catch { }

        }

        *************************/

        /*  End Cell Click LoadIterline Ivoice  Joseph*/


        /*End Interline Interline Invoice   Joseph*/





        /*fait par christian*/
        public ActionResult RejectionList()
        {
            /*RejectionList*/
            int Param = Request.Form["Param"].AsInt();
            int cboRAChargeCodeSelectedIndex = Request.Form["cboRAChargeCodeSelectedIndex"].AsInt();
            int cboRAInvoiceNumberSelectedIndex = Request.Form["cboRAInvoiceNumberSelectedIndex"].AsInt();
            string cboRAChargeCode = Request["cboRAChargeCode"].Trim();
            string cboRAInvoiceNumber = Request["cboRAInvoiceNumber"].Trim();

            string[] ChgCode = cboRAChargeCode.Trim().Split('-');
            string ChargeCode = ChgCode[0].ToString().Trim();

            if (ChargeCode == "6")
            {
                ViewBag.lblNextStage = "Proceed With Correspondence";
                ViewBag.pnlAcceptVisible = "hidden";
                ViewBag.pnlRejectionVisible = "hidden";
            }
            else
            {
                ViewBag.pnlAcceptVisible = "Visible";
                ViewBag.pnlRejectionVisible = "Visible";
            }

            string Sql = "";
            Sql = " SELECT  DISTINCT  ";
            Sql = Sql + "F1.[InvoiceNo] AS [Invoice No],";
            Sql = Sql + "F1.[InvoiceDate] AS [Invoice Date],";
            Sql = Sql + "F1.[BillingPeriod] AS [Billing Period],";
            Sql = Sql + "F1.[LineItemNumber] AS [Line Item Number],";
            Sql = Sql + "F1.[DetailNumber] AS [Detail Number],";
            Sql = Sql + "F1.[ChargeCode] AS [Charge Code],";
            Sql = Sql + "F1.[DocumentNumber] AS [Document Number],";
            Sql = Sql + "F1.[CouponNumber] AS [Coupon Number],";
            if (ChargeCode == "4" || ChargeCode == "5" || ChargeCode == "6")
            {
                Sql = Sql + "F1.REJECTIONMEMONUMBER AS [REJECTION MEMO NUMBER],";
                Sql = Sql + "f2.[FlightNo] AS [Flight No] , ";
                Sql = Sql + "f2.[FlightDate]  AS [Flight Date], ";
                Sql = Sql + "f2.[FromAirportCode] AS [From Airport Code], ";
                Sql = Sql + "f2.[ToAirportCode] AS [To Airport Code], ";
                Sql = Sql + "cast(f1.BreakdownSerialNumber as integer) AS [Breakdown Serial Number],";

            }
            else
            {
                Sql = Sql + "f2.[AirlineFlightDesignator] AS [Airline Flight Designator], ";
                Sql = Sql + "f2.[FlightNo] AS [Flight No] , ";
                Sql = Sql + "f2.[FlightDate]  AS [Flight Date], ";
                Sql = Sql + "f2.[FromAirportCode] AS [From Airport Code], ";
                Sql = Sql + "f2.[ToAirportCode] AS [To Airport Code], ";
                Sql = Sql + "f2.[SettlementAuthorizationCode] AS [Settlement Authorization Code],";

            }
            Sql = Sql + "F1.[FinalShare] AS [Final Share],";
            Sql = Sql + "F1.[SpaAmount] AS [SPA Amount],";
            Sql = Sql + "F1.[Variance] AS [Variance],";

            Sql = Sql + " case when [SpaAmount]>0 then 'Y' else 'N' end AS [SPA Applies?],";
            Sql = Sql + "F1.[YourNetAmourt] AS [Your Net Amount], ";
            Sql = Sql + "F1.[OurNetAmount] AS [Our Net Amount],";
            Sql = Sql + "F1.[DiffNetAmount] AS [Diff Net Amount],";
            Sql = Sql + "F1.[Processdate] AS [Process Date],";
            Sql = Sql + "F1.[REMARKS] AS [REMARKS],";
            Sql = Sql + "F1.[DieFlown] AS [MOS SALES],";
            Sql = Sql + "F1.[DieInterLine] AS [MOC INTERLINE],";
            Sql = Sql + "F1.[DieDiff] AS [DIE (Difference In Exchange)],";
            Sql = Sql + "F1.USERACTION AS [User Action],";
            Sql = Sql + "F1.[NotinSystem] AS [Not In System],";
            Sql = Sql + "F1.[Accounting] AS [ACCOUNTING],";
            Sql = Sql + "F1.[PostAccounting] AS [Validated]";

            Sql = Sql + " FROM [XmlFile].[InvoiceAnalytics] F1";
            Sql = Sql + " left join [XmlFile].[DataLineItemDetails] f2";
            Sql = Sql + " ON    F1.[TransmisionId] =  F2.[TransmissionId]";
            Sql = Sql + " AND  F1.[InvoiceNo] = f2.[InvoiceNo]";
            Sql = Sql + " AND F1.[LineItemNumber] =f2.[LineItemNumber]";
            Sql = Sql + " AND  F1.[DetailNumber] =f2.[DetailNumber]";
            Sql = Sql + " and f1.SellerOrganization_OrganizationDesignator = f2.SellerOrganization_OrganizationDesignator";

            if (Param == 1)
            {
                Sql = Sql + " where BillingStatus =1";
                Sql = Sql + " and F1.[DiffNetAmount]>0";
            }
            if (Param == -1)
            {
                Sql = Sql + " where BillingStatus =1";
                Sql = Sql + " and F1.[DiffNetAmount]<=0";

            }

            if (Param == 2)
            {
                Sql = Sql + " where BillingStatus =0";
                Sql = Sql + " and (F1.USERACTION ='ACCEPTED'";
                Sql = Sql + " or F1.[REMARKS]='ACCEPTED')";

            }
            if (Param == 3)
            {
                Sql = Sql + " where BillingStatus =1";
                Sql = Sql + " and F1.USERACTION ='REJECTED'";

            }

            if (Param == 4)
            {
                Sql = Sql + " where BillingStatus =1";
                Sql = Sql + " and F1.Duplicate =1";

            }

            if (Param == 5)
            {
                Sql = Sql + " where BillingStatus =1";
                Sql = Sql + " and f1.notinsystem = '1'";

            }

            if (Param == 0)
            {
                if (cboRAInvoiceNumberSelectedIndex > 0)
                {
                    Sql = Sql + " WHERE f1.[SellerOrganization_OrganizationDesignator]+'-'+F1.[InvoiceNo]='" + cboRAInvoiceNumber.Trim() + "'";


                    if (cboRAChargeCodeSelectedIndex > 0) { Sql = Sql + " AND F1.[ChargeCode]='" + ChargeCode + "'"; };

                };

                if (cboRAInvoiceNumberSelectedIndex == 0)
                {
                    if (cboRAChargeCodeSelectedIndex > 0) { Sql = Sql + " WHERE F1.[ChargeCode]='" + ChargeCode + "'"; };
                }
            }
            else
            {
                if (cboRAInvoiceNumberSelectedIndex > 0)
                {
                    Sql = Sql + " WHERE f1.[SellerOrganization_OrganizationDesignator]+'-'+F1.[InvoiceNo]='" + cboRAInvoiceNumber.Trim() + "' ";
                };
                if (cboRAChargeCodeSelectedIndex > 0) { Sql = Sql + " AND F1.[ChargeCode]='" + ChargeCode + "'"; };
            }
            if (ChargeCode == "4" || ChargeCode == "5" || ChargeCode == "6")
            {

                Sql = Sql + " ORDER BY F1.[InvoiceNo],F1.REJECTIONMEMONUMBER, F1.[DetailNumber],cast(f1.BreakdownSerialNumber as integer)";
            }

            else
            {
                Sql = Sql + " ORDER BY F1.[InvoiceNo],F1.[DetailNumber]";
            }
            SqlDataReader myReader;
            int R = 0;
            myReader = GetlistofTables(Sql);
            int l = myReader.FieldCount;
            string[] dgvRejectionTH = new string[l];

            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet();
            ds = dbconnect.RetObjDS(Sql);

            int ligne = ds.Tables[0].Rows.Count;
            int colone = ds.Tables[0].Columns.Count;
            string[,] dgvRejection = new string[ligne, colone];
            string[,] dgvRejectioncolore = new string[ligne, colone];
            string[] dgvRejectionColumnsDefaultCellStyleBackColor = new string[colone];
            string[] dgvRejectionColumnsVisible = new string[colone];

            if (myReader != null)
            {
                for (int i = 0; i < myReader.FieldCount; i++)
                {
                    dgvRejectionTH[i] = myReader.GetName(i);
                }

                ViewBag.dgvRejectionColumnHeadersBackColor = "background-color:Maroon";
                ViewBag.dgvRejectionColumnHeadersForeColor = "color:White";

                for (int j = 0; j < ligne; j++)
                {
                    for (int i = 0; i < colone; i++)
                    {
                        dgvRejectioncolore[j, i] = "background-color:Khaki";
                    }
                }

                if (myReader.HasRows == true)
                {

                    while (myReader.Read())
                    {
                        var USERACTION = myReader.GetValue(26).ToString();
                        var Notinsystem = myReader.GetValue(27).ToString();
                        var Remark = myReader.GetValue(22).ToString();
                        for (int i = 0; i < myReader.FieldCount; i++)
                        {
                            string dataTypeName = myReader.GetDataTypeName(i);
                            if (myReader.GetValue(i) != System.DBNull.Value)
                            {
                                if (dataTypeName == "date")
                                {
                                    if (i == 9)
                                    {
                                        DateTime dt = myReader.GetDateTime(i);
                                        dgvRejection[R, i] = String.Format("{0:yyMMdd}", dt);
                                        ViewBag.dgvRejectionColumnsWidth = "68px";
                                    }
                                    else
                                    {
                                        DateTime dt = myReader.GetDateTime(i);
                                        dgvRejection[R, i] = String.Format("{0:yyyy-MM-dd}", dt);
                                        ViewBag.dgvRejectionColumnsWidth = "68px";
                                    }
                                }
                                else
                                {
                                    if (myReader.GetValue(i) != System.DBNull.Value)
                                    {
                                        dgvRejection[R, i] = myReader.GetValue(i).ToString();
                                    }
                                }
                            }
                        }


                        if (USERACTION == "ACCEPTED" || Remark == "ACCEPTED")
                        {
                            for (int col1 = 0; col1 < colone; col1++)
                            {
                                dgvRejectioncolore[R, col1] = "background-color:PaleGreen";
                            }
                        }
                        else
                        {
                            if (USERACTION == "REJECTED")
                            {
                                for (int col1 = 0; col1 < colone; col1++)
                                {
                                    if (Notinsystem == "1")
                                    { dgvRejectioncolore[R, col1] = "background-color:Red"; }
                                    else
                                    { dgvRejectioncolore[R, col1] = "background-color:HotPink"; }
                                }
                            }
                            else
                            {
                                if (USERACTION.Trim().Length == 0)
                                {
                                    for (int col1 = 0; col1 < colone; col1++)
                                    {
                                        dgvRejectioncolore[R, col1] = "background-color:Turquoise";
                                    }
                                }
                            }
                            if (Notinsystem == "1")
                            {
                                for (int col1 = 0; col1 < colone; col1++)
                                {
                                    dgvRejectioncolore[R, col1] = "background-color:Red";
                                }
                            }
                        }
                        R++;
                    }
                }
                myReader.Close();
                myReader.Dispose();
                myReader = null;

                for (int i = 2; i < colone; i++)
                {
                    ViewBag.dgvRejectionColumnsWidth = "55px";
                }

                if (ChargeCode == "4" || ChargeCode == "5" || ChargeCode == "6")
                {
                    dgvRejectionColumnsVisible[9] = "display:none";
                    dgvRejectionColumnsVisible[10] = "display:none";
                    dgvRejectionColumnsVisible[11] = "display:none";
                    dgvRejectionColumnsVisible[12] = "display:none";

                }
                else
                {
                    dgvRejectionColumnsVisible[9] = "";
                    dgvRejectionColumnsVisible[10] = "";
                    dgvRejectionColumnsVisible[11] = "";
                    dgvRejectionColumnsVisible[12] = "";
                }


            }
            ViewBag.ligne = ligne;
            ViewBag.colone = colone;
            ViewBag.dgvRejection = dgvRejection;
            ViewBag.dgvRejectionColumnsVisible = dgvRejectionColumnsVisible;
            ViewBag.l = l;
            ViewBag.dgvRejectioncolore = dgvRejectioncolore;
            ViewBag.dgvRejectionColumnsDefaultCellStyleBackColor = dgvRejectionColumnsDefaultCellStyleBackColor;
            ViewBag.dgvRejectionTH = dgvRejectionTH;
            /*fin RejectionList*/

            return PartialView();
        }
        public ActionResult LoadInvoiceNumber()
        {
            string Sql = "SELECT  distinct SellerOrganization_OrganizationDesignator,  [InvoiceNo] ,chargecode FROM [XmlFile].[InvoiceAnalytics] where isnull(Archived,0) <> 1";
            SqlDataReader myReader;
            myReader = GetlistofTables(Sql);

            DataSet ds = new DataSet();
            ds = dbconnect.RetObjDS(Sql);
            int ligne = ds.Tables[0].Rows.Count;

            string[] cboRAInvoiceNumber = new string[ligne];
            int i = 0;
            if (myReader != null)
            {
                if (myReader.HasRows == true)
                {
                    while (myReader.Read())
                    {
                        cboRAInvoiceNumber[i] = myReader.GetValue(0).ToString() + "-" + myReader.GetValue(1).ToString();
                    }
                }
                myReader.Close();
                myReader.Dispose();
                myReader = null;
            }
            ViewBag.ligne = ligne;
            ViewBag.cboRAInvoiceNumber = cboRAInvoiceNumber;
            return PartialView();
        }
        public ActionResult GetInvoiceCountrejectionanalytic()
        {
            int x = Request.Form["x"].AsInt();
            #region ArrChargeCode
            ArrayList ArrChargeCode;
            ArrChargeCode = new ArrayList();
            string Sql = "SELECT  distinct SellerOrganization_OrganizationDesignator,  [InvoiceNo] ,chargecode FROM [XmlFile].[InvoiceAnalytics] where isnull(Archived,0) <> 1";
            SqlDataReader myReader;
            myReader = GetlistofTables(Sql);
            if (myReader != null)
            {
                if (myReader.HasRows == true)
                {
                    while (myReader.Read())
                    {
                        ArrChargeCode.Add(myReader.GetValue(2).ToString());
                    }
                }
                myReader.Close();
                myReader.Dispose();
                myReader = null;
            }

            #endregion
            string chargecode = ArrChargeCode[x - 1].ToString();
            string lblArchiveInvNo = Request["lblArchiveInvNo"].Trim();

            string sql = "";
            sql += "SELECT [SellerOrganization_OrganizationDesignator] +'-'+ [InvoiceNo] as [Invoice No] ,'TOTAL COUNT' as UserAction,ChargeCode ,count(*) as Counts " + Environment.NewLine;
            sql += "FROM [XmlFile].[InvoiceAnalytics] where [SellerOrganization_OrganizationDesignator] +'-'+ [InvoiceNo] = '" + lblArchiveInvNo + "' and chargecode ='" + chargecode + "'" + Environment.NewLine;
            sql += "group by [InvoiceNo],[SellerOrganization_OrganizationDesignator],ChargeCode  " + Environment.NewLine;
            sql += "union all  " + Environment.NewLine;
            sql += "SELECT [SellerOrganization_OrganizationDesignator] +'-'+ [InvoiceNo] as [Invoice No],case when [USERACTION] is null then 'PENDING' else [USERACTION] END,ChargeCode ,count(*) as Counts  " + Environment.NewLine;
            sql += "FROM [XmlFile].[InvoiceAnalytics] where  [SellerOrganization_OrganizationDesignator] +'-'+ [InvoiceNo] = '" + lblArchiveInvNo + "'  and chargecode ='" + chargecode + "' " + Environment.NewLine;
            sql += "group by [InvoiceNo],[SellerOrganization_OrganizationDesignator],[USERACTION],ChargeCode  " + Environment.NewLine;

            SqlCommand command;
            conn = new SqlConnection(pbConnectionString);
            command = new SqlCommand();
            command.Connection = conn;
            if (conn.State == ConnectionState.Open)
            {
                conn.Close();
            }
            conn.Open();
            command = new SqlCommand(sql, conn);
            command.CommandType = CommandType.Text;
            SqlDataReader read = command.ExecuteReader();
            DataSet ds = new DataSet();
            DataTable dt = new DataTable("Table1");
            ds.Clear();
            ds.Reset();
            dt.Clear();
            dt.Reset();
            ds.Tables.Add(dt);
            ds.Load(read, LoadOption.PreserveChanges, ds.Tables[0]);

            int ligne = ds.Tables[0].Rows.Count;
            int colone = ds.Tables[0].Columns.Count;
            string[,] dvgarchive = new string[ligne, colone];
            int i = 0;
            if (ligne > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    for (int j = 0; j < colone; j++)
                    {
                        dvgarchive[i, j] = dr[ds.Tables[0].Columns[j].ColumnName].ToString();
                    }
                    i++;
                }
            }
            conn.Close();
            int Pend = 0;
            for (int ii = 0; ii < colone; ii++)
            {
                if (dvgarchive[1, ii] == "PENDING")
                {
                    Pend = 1;
                    break;
                }
            }
            if (Pend == 1)
            {
                ViewBag.lblarcmsg = "THIS INVOICE CAN NOT BE ARCHIVED";
                ViewBag.bntArchivedEnabled = "false";
            }
            else
            {
                ViewBag.lblarcmsg = "THIS INVOICE READY TO BE ARCHIVED";
                ViewBag.bntArchivedEnabled = "true";
            }

            ViewBag.ligne = ligne;
            ViewBag.colone = colone;
            ViewBag.dvgarchive = dvgarchive;

            return PartialView();
        }
        public ActionResult RALoadChargeCoderejectionanalytic()
        {
            int cboRAInvoiceNumberSelectedIndex = Request.Form["cboRAInvoiceNumberSelectedIndexq"].AsInt();
            string cboRAInvoiceNumber = Request["cboRAInvoiceNumberq"].Trim();


            string Sql = "SELECT  Distinct  ChargeCode + ' - ' + [Description]  AS [Description]";
            Sql = Sql + " FROM  [XmlFile].[DataInvoicesHRD]";
            if (cboRAInvoiceNumberSelectedIndex > 0) { Sql = Sql + " WHERE [SellerOrganization_OrganizationDesignator]+'-'+[InvoiceNumber]='" + cboRAInvoiceNumber + "'"; };
            SqlDataReader myReader;
            myReader = GetlistofTables(Sql);

            DataSet ds = new DataSet();
            ds = dbconnect.RetObjDS(Sql);
            int ligne = ds.Tables[0].Rows.Count;

            string[] cboRAChargeCode = new string[ligne];

            int i = 0;
            if (myReader != null)
            {
                if (myReader.HasRows == true)
                {
                    while (myReader.Read())
                    {
                        cboRAChargeCode[i] = myReader.GetValue(0).ToString();
                        i++;
                    }
                }
                myReader.Close();
                myReader.Dispose();
                myReader = null;

                if (ligne > 0) { ViewBag.cboRAChargeCodeSelectedIndex = "1"; }
            }

            ViewBag.ligne = ligne;
            ViewBag.cboRAChargeCode = cboRAChargeCode;
            return PartialView();
        }
        public ActionResult LoadInvoiceSummaryrejectionanaltityc()
        {
            string cboRAInvoiceNumber = Request["cboRAInvoiceNumber"].Trim();
            string Sql = "SELECT  [InvoiceNo] as [invoice Number]  ,[BillingPeriod] as [Billing Period],[ChargeCode] As [Charge Code],[BillingStatus] as [Billing Status] ,Substring([REMARKS],1,3) as Remarks ,[USERACTION] As [User Action] ,COUNT(*) as RecCount";
            Sql = Sql + " ,sum([YourGrossBillAmount]) as [Your GrossBil lAmount]";
            Sql = Sql + " ,sum([YourTaxAmount]) as [Your Tax Amount]";
            Sql = Sql + " ,sum([YourISCAmount]) as[Your ISC Amount]";
            Sql = Sql + " ,sum([YourOtherCommision]) as [Your Other Commision]";
            Sql = Sql + "  ,sum([YourUATP]) as [YourUATP]";
            Sql = Sql + "  ,sum([YourHandlingFees]) as [Your Handling Fees]";
            Sql = Sql + " ,sum([YourVatAmount]) as [Your Vat Amount]";
            Sql = Sql + " ,sum([YourNetAmourt]) as [Your Net Amourt]";

            Sql = Sql + " ,sum([OurGrossBill]) as [Our GrossBill]";
            Sql = Sql + " ,sum([OurTaxAmount]) as [Our Tax Amount]";
            Sql = Sql + " ,sum([OurISCAmount]) as [Our ISC Amount]";
            Sql = Sql + " ,sum([OurOtherCommission]) as [Our Other Commission]";
            Sql = Sql + " ,sum([OurUATP]) as [Our UATP]";
            Sql = Sql + " ,sum([OurHandlingFees]) as [Our Handling Fees]";
            Sql = Sql + " ,sum([OurVatAmount]) as [Our Vat Amount]";
            Sql = Sql + " ,sum([OurNetAmount]) as [OurNetAmount]";

            Sql = Sql + " ,sum([DiffGrossAmount]) as [Diff Gross Amount]";
            Sql = Sql + " ,sum([DiffTaxAmount]) as [DiffTax Amount]";
            Sql = Sql + " ,sum([DiffISCAmount]) as [Diff ISC Amount]";
            Sql = Sql + " ,sum([DiffOtherCommission]) as [Diff Other Commission]";
            Sql = Sql + " ,sum([DiffUATP]) as [Diff UATP]";
            Sql = Sql + "  ,sum([DiffHandlingFees]) as [Diff Handling Fees]";
            Sql = Sql + " ,sum([DiffVatAmount]) as [Diff Vat Amount]";
            Sql = Sql + "  ,sum([DiffNetAmount]) as [Diff Net Amount]";
            Sql = Sql + "  FROM [XmlFile].[InvoiceAnalytics]";
            Sql = Sql + " where InvoiceNo='" + cboRAInvoiceNumber + "'";

            Sql = Sql + " group by ";
            Sql = Sql + " [InvoiceNo]  ";
            Sql = Sql + " ,[BillingPeriod]";
            Sql = Sql + " ,[ChargeCode]";
            Sql = Sql + " ,[BillingStatus]";

            Sql = Sql + "  , Substring([REMARKS],1,3)";
            Sql = Sql + " ,[USERACTION]";

            SqlDataReader myReader;
            int R = 0;
            myReader = GetlistofTables(Sql);

            DataSet ds = new DataSet();
            ds = dbconnect.RetObjDS(Sql);
            int ligne = ds.Tables[0].Rows.Count;
            int colone = ds.Tables[0].Columns.Count;

            string[,] dataGridView20 = new string[ligne, colone];
            int l1 = myReader.FieldCount;
            string[] dataGridView20TH = new string[l1];
            string[] dataGridView20ColumnsBackColor = new string[colone];
            string[] dataGridView20ColumnsWidth = new string[colone];

            if (myReader != null)
            {
                if (myReader.HasRows == true)
                {
                    for (int i = 0; i < myReader.FieldCount; i++)
                    {
                        dataGridView20TH[i] = myReader.GetName(i);
                    }
                    ViewBag.dataGridView20ColumnHeadersBackColor = "background-color:Maroon";
                    ViewBag.dataGridView20ColumnHeadersForeColor = "color:White";

                    for (int i = 0; i < colone; i++)
                    {
                        try
                        {
                            dataGridView20ColumnsBackColor[i] = "background-color:Khaki";
                            dataGridView20ColumnsWidth[1] = "width:120px";
                        }
                        catch { }
                    }
                    while (myReader.Read())
                    {
                        for (int col = 0; col < myReader.FieldCount; col++)
                        {
                            dataGridView20[R, col] = myReader.GetValue(col).ToString();
                        }
                        R++;
                    }
                }
                myReader.Close();
                myReader.Dispose();
                myReader = null;
            }


            ViewBag.l1 = l1;
            ViewBag.ligne = ligne;
            ViewBag.colone = colone;
            ViewBag.dataGridView20ColumnsWidth = dataGridView20ColumnsWidth;
            ViewBag.dataGridView20ColumnsBackColor = dataGridView20ColumnsBackColor;
            ViewBag.dataGridView20 = dataGridView20;
            ViewBag.dataGridView20TH = dataGridView20TH;
            return PartialView();
        }
        public ActionResult dgvRejection_CellClickrejectionanalytic()
        {
            string pvChargeCode = Request["pvChargeCode"].Trim();
            int dgvRejectionCurrentRowIndex = Request["dgvRejectionCurrentRowIndex"].AsInt();
            string value0 = Request["value0"].Trim();
            string value1 = Request["value1"].Trim();
            string value2 = Request["value2"].Trim();
            string value3 = Request["value3"].Trim(); 
            int value3n = Request["value3"].AsInt();
            string value4 = Request["value4"].Trim();
            int value4n = Request["value4"].AsInt();
            string value5 = Request["value5"].Trim(); 
            string value6 = Request["value6"].Trim(); 
            string value7 = Request["value7"].Trim(); 
            string value22 = Request["value22"].Trim(); 
            string value26 = Request["value26"].Trim();
            string txtReasonCode = Request["txtReasonCode"].Trim();
            int NotinSystem = 0;

            if (pvChargeCode == "4" || pvChargeCode == "5" | pvChargeCode == "6")
            {
                //rejectionRecordToolStripMenuItem.Enabled = true;
            }
            else {
                //rejectionRecordToolStripMenuItem.Enabled = false;
            }

            if (pvChargeCode == "6")
            {
               // correspondenceEntryToolStripMenuItem.Enabled = true;
            }
            else {
              //  correspondenceEntryToolStripMenuItem.Enabled = false;
            }
            string txtReasonCodeX = "";
            string txtReasonDescriptionX = "";
            decimal v1 = 0;
            decimal v2 = 0;
            NumberStyles style;
            CultureInfo provider;
            //pnlUploadStat.Visible = false;
            int RX = -1;
            try
            {
                RX = dgvRejectionCurrentRowIndex;
                string invoiceno = value0;
                string InvoiceDate = value1;
                string BillingPeriod = value2;
                PreviousBillingPeriod = BillingPeriod;
                int DetailNumber;
                int LineItemNumber;
                if (value3 != "")
                {
                    LineItemNumber = Convert.ToInt16(value3);
                }
                else
                {
                    LineItemNumber = value3n;
                }
                if (value4 != "")
                {
                    DetailNumber = Convert.ToInt16(value4);
                }
                else
                {
                    DetailNumber = value4n;
                }
                string ChargeCode = value5.ToString();
                pvChargeCode = ChargeCode;

                string DocumentNumber = value6.ToString();
                string CouponNumber = value7.ToString();
                string Tkt = "";
                if (DocumentNumber.Length >=4)
                {
                    Tkt = DocumentNumber.Substring(3, DocumentNumber.Length - 3);
                }
                else
                {
                    Tkt = DocumentNumber;
                }
                FullDocNo = DocumentNumber;

                if (ChargeCode == "4" || ChargeCode == "5" || ChargeCode == "6")
                {
                    /*OB2RejndRej(Tkt, CouponNumber);*/
                    #region OB2RejndRej
                    string Cpn = CouponNumber;
                    string Sqlq = "SELECT [BALC] as [Billed Airline],[DOC] As [Ticket Number],[CPN] as [Coupon Number],[PMC] as [Source Code],[SECTORFROM] as [Sector From],[SECTORTO] as [sector To]";
                    Sqlq = Sqlq + " ,[AMOUNT] as [Amount] ,[ISC] as [ISC%],[COMM] AS [ISC Amount],[FLIGHT],[FLTDATE] as [Flight Date] ,[CUR],[ETKTSAC],[INVOICENO],[BILLINGPERIOD],[ManualFlag]";
                    Sqlq = Sqlq + " FROM [Pax].[OutwardBilling]";
                    Sqlq = Sqlq + " where [DOC] ='" + Tkt + "'";
                    Sqlq = Sqlq + " and [CPN]='" + Cpn + "'";

                    SqlConnection csq = new SqlConnection(pbConnectionString);
                    DataSet dsq = new DataSet();
                    SqlDataAdapter adaq = new SqlDataAdapter(Sqlq, csq);
                    adaq.Fill(dsq);
                    int ligne = dsq.Tables[0].Rows.Count;
                    int colone = dsq.Tables[0].Columns.Count;
                    string[,] dataGridView25 = new string[ligne, colone];
                    int l = dsq.Tables[0].Columns.Count;
                    string[] dataGridView25TH = new string[l];
                    string[] dataGridView25ColumnsBackColor = new string[colone]; 
                    string[] dataGridView25ColumnsWidth = new string[colone];
                    string dataGridView25RowHeadersVisible = "";
                    string dataGridView25ColumnHeadersBackColor = "";
                    string dataGridView25ColumnHeadersForeColor = "";
                    try
                    {
                        SqlDataReader myReaderq;
                        int Rq = 0;
                        myReaderq = GetlistofTables(Sqlq);

                        if (myReaderq != null) {
                            if (myReaderq.HasRows == true)
                            {
                                for (int i = 0; i < myReaderq.FieldCount; i++)
                                {
                                    dataGridView25TH[i] = myReaderq.GetName(i);
                                }

                                dataGridView25RowHeadersVisible = "visibility:hidden";
                                dataGridView25ColumnHeadersBackColor = "background-color:Maroon";
                                dataGridView25ColumnHeadersForeColor = "color:White";

                                for (int i = 0; i < colone; i++)
                                {
                                    try
                                    {
                                        dataGridView25ColumnsBackColor[i] = "background-color:Khaki"; 
                                        dataGridView25ColumnsWidth[1] = "width:120px";
                                    }
                                    catch { }
                                }
                                while (myReaderq.Read())
                                {
                                    for (int col = 0; col < myReaderq.FieldCount - 1; col++)
                                    {
                                        dataGridView25[Rq, col] = myReaderq.GetValue(col).ToString();
                                    }
                                    Rq++;
                                }
                            }
                        }
                        if (ligne > 0)
                        {
                            dataGridView25RowHeadersVisible = "visibility:visible";
                        }
                        myReaderq.Close();
                        myReaderq.Dispose();
                        myReaderq = null;

                        /*SumOfOB2ndRejTax(Tkt, Cpn);*/
                        #region SumOfOB2ndRejTax
                        string Sql1 = "SELECT  Sum ([TAXAMT]) as [Tax Amount]";
                        Sql1 = Sql1 + " FROM [Pax].[OutwardBillingTax]";
                        Sql1 = Sql1 + " where [DOC] ='" + Tkt + "'";
                        Sql1 = Sql1 + " and [CPN]='" + Cpn + "'";

                        try
                        {
                            SqlDataReader myReader1;
                            int R1 = 0;
                            myReader1 = GetlistofTables(Sql1);

                            if (myReader1 != null)
                            {
                                if (myReader1.HasRows == true)
                                {

                                    while (myReader1.Read())
                                    {
                                        dataGridView25ColumnsBackColor[7] = "background-color:Khaki";
                                        dataGridView25[R1,7] = myReader1.GetValue(0).ToString();
                                        R1++;
                                    }
                                }
                            }
                            myReader1.Close();
                            myReader1.Dispose();
                            myReader1 = null;
                        }
                        catch
                        {
                            int err = 0;
                        }
                        dataGridView25ColumnsWidth[0] = "width:50px";
                        dataGridView25ColumnsWidth[1] = "width:100px";
                        dataGridView25ColumnsWidth[2] = "width:50px";
                        dataGridView25ColumnsWidth[3] = "width:50px";
                        dataGridView25ColumnsWidth[4] = "width:50px";
                        dataGridView25ColumnsWidth[5] = "width:50px";
                        dataGridView25ColumnsWidth[6] = "width:75px";
                        dataGridView25ColumnsWidth[7] = "width:75px";
                        dataGridView25ColumnsWidth[8] = "width:50px";
                        dataGridView25ColumnsWidth[9] = "width:75px";
                        dataGridView25ColumnsWidth[10] = "width:50px";
                        dataGridView25ColumnsWidth[11] = "width:100px";
                        dataGridView25ColumnsWidth[12] = "width:50px";
                        dataGridView25ColumnsWidth[13] = "width:100px";
                        dataGridView25ColumnsWidth[14] = "width:100px";
                        dataGridView25ColumnsWidth[15] = "width:50px";
                        #endregion
                        /*fin SumOfOB2ndRejTax(Tkt, Cpn);*/
                    }
                    catch
                    {
                        int err = 0;
                    }
                    ViewBag.dataGridView25RowHeadersVisible = dataGridView25RowHeadersVisible;
                    ViewBag.dataGridView25ColumnHeadersBackColor = dataGridView25ColumnHeadersBackColor;
                    ViewBag.dataGridView25ColumnHeadersForeColor = dataGridView25ColumnHeadersForeColor;
                    ViewBag.dataGridView25ColumnsBackColor = dataGridView25ColumnsBackColor;
                    ViewBag.dataGridView25ColumnsWidth = dataGridView25ColumnsWidth;
                    ViewBag.ligne = ligne;
                    ViewBag.colone = colone;
                    ViewBag.dataGridView25 = dataGridView25;
                    ViewBag.l = l;
                    ViewBag.dataGridView25TH = dataGridView25TH;
                    #endregion
                    /*fin OB2RejndRej(Tkt, CouponNumber);*/
                    /*OBData(Tkt, CouponNumber.ToString(), invoiceno, DetailNumber.ToString());*/
                    #region OBData
                    Cpn = CouponNumber.ToString();
                    string Inv = invoiceno;
                    string Dn = DetailNumber.ToString();

                    string Sql2 = "SELECT distinct  ";
                    Sql2 = Sql2 + " F2.[RejectionMemoNumber]  ,F2.[RejectionStage] ,F2.[ReasonCode] ,F2.[ReasonDescription]";
                    Sql2 = Sql2 + " ,F2.[YourInvoiceNumber]  ,F2.[YourInvoiceBillingDate]";
                    Sql2 = Sql2 + " FROM [XmlFile].[DataLineItemDetails] F1";
                    Sql2 = Sql2 + " left join [XmlFile].[DataRejMemoDetails] F2 on ";
                    Sql2 = Sql2 + " f1.[TransmissionId] = f2.[TransmissionId]";
                    Sql2 = Sql2 + " and    f1.DetailNumber= f2.DetailNumber";
                    Sql2 = Sql2 + " and    f1.[InvoiceNo]= f2.[InvoiceNo]";
                    Sql2 = Sql2 + " and     F1.[TicketDocOrFIMNumber]= f2.[TicketNo]";
                    Sql2 = Sql2 + " and     F1.[TicketOrFIMCouponNumber]= f2.[CouponNo]";
                    Sql2 = Sql2 + " where f2.[InvoiceNo]='" + Inv + "'";
                    Sql2 = Sql2 + " and f2.TicketDocNumber =" + Tkt;
                    Sql2 = Sql2 + " and f2.[CouponNumber]=" + Cpn;
                    Sql2 = Sql2 + " and f2.[detailnumber]=" + Dn;

                    Sql2 = "SELECT distinct  ";
                    Sql2 = Sql2 + " F2.[RejectionMemoNumber]  ,F2.[RejectionStage] ,F2.[ReasonCode] ,F2.[ReasonDescription]";
                    Sql2 = Sql2 + " ,F2.[YourInvoiceNumber]  ,F2.[YourInvoiceBillingDate]";
                    Sql2 = Sql2 + " FROM [XmlFile].[DataRejMemoDetails] F2 ";
                    Sql2 = Sql2 + " where f2.[InvoiceNo]='" + Inv + "'";
                    Sql2 = Sql2 + " and f2.TicketNo =" + Tkt;
                    Sql2 = Sql2 + " and f2.[CouponNo]=" + Cpn;
                    Sql2 = Sql2 + " and f2.[detailnumber]=" + Dn;

                    try
                    {
                        SqlDataReader myReader2;
                        int R2 = 0;
                        myReader2 = GetlistofTables(Sql2);

                        if (myReader2 != null) {
                            if (myReader2.HasRows == true)
                            {
                                while (myReader2.Read())
                                {
                                   // ViewBag.lblRMN = myReader2.GetValue(0).ToString();
                                    ViewBag.lblRMNStage = myReader2.GetValue(1).ToString();
                                    ViewBag.lblRMNCode = myReader2.GetValue(2).ToString();
                                    ViewBag.lblRMNRDesc = myReader2.GetValue(3).ToString();
                                    ViewBag.lblRMNInvoiceNumber = myReader2.GetValue(4).ToString();
                                    ViewBag.lblRMNBillingDate = myReader2.GetValue(5).ToString();

                                    ViewBag.lblRMN1 = myReader2.GetValue(0).ToString();
                                    ViewBag.lblRMNStage1 = myReader2.GetValue(1).ToString();
                                    ViewBag.lblRMNCode1 = myReader2.GetValue(2).ToString();
                                    ViewBag.txtRMNRDesc1 = myReader2.GetValue(3).ToString();
                                    ViewBag.lblRMNInvoiceNumber1 = myReader2.GetValue(4).ToString();
                                    ViewBag.lblRMNBillingDate1 = myReader2.GetValue(5).ToString();
                                }
                            }
                        }
                        myReader2.Close();
                        myReader2.Dispose();
                        myReader2 = null;
                    }
                    catch
                    {
                        int err = 0;
                    }
                    #endregion
                    /* fin OBData(Tkt, CouponNumber.ToString(), invoiceno, DetailNumber.ToString());*/
                    ViewBag.pnlAcceptVisible = "visible";
                }
                /*LoadAttachment();*/
                #region LoadAttachment
                string SqlA = "select RecID as [Attachment ID], Attachment As [File Attachment] from [XmlFile].[Attachments] ";
                SqlA = SqlA + " WHERE [ChargeCode] ='" + ChargeCode + "'";
                SqlA = SqlA + " AND [DocumentNumber] =" + DocumentNumber;
                SqlA = SqlA + " AND [CouponNumber] = " + CouponNumber;

                SqlConnection csA = new SqlConnection(pbConnectionString);
                DataSet dsA = new DataSet();
                SqlDataAdapter adaA = new SqlDataAdapter(SqlA, csA);
                adaA.Fill(dsA);
                int ligneA = dsA.Tables[0].Rows.Count;
                int coloneA = dsA.Tables[0].Columns.Count;
                string[,] dgvAttachment = new string[ligneA, coloneA];
                int lA = dsA.Tables[0].Columns.Count;
                string[] dgvAttachmentTH = new string[lA];
                string dgvAttachmentRowHeadersVisible = "";
                string dgvAttachmentColumnHeadersBackColor = "";
                string dgvAttachmentColumnHeadersForeColor = "";
                string[] dgvAttachmentColumnsBackColor = new string[coloneA]; 
                string[] dgvAttachmentColumnsWidth = new string[coloneA]; 
                try
                {
                    SqlDataReader myReaderA;
                    int RA = 0;
                    myReaderA = GetlistofTables(SqlA);

                    if (myReaderA != null) {
                        if (myReaderA.HasRows == true)
                        {
                            for (int i = 0; i < myReaderA.FieldCount; i++)
                            {
                                dgvAttachmentTH[i] = myReaderA.GetName(i).ToString();
                            }
                            dgvAttachmentRowHeadersVisible = "visibility:hidden";
                            dgvAttachmentColumnHeadersBackColor = "background-color:Maroon";
                            dgvAttachmentColumnHeadersForeColor = "color:White";

                            for (int i = 0; i < coloneA; i++)
                            {
                                try
                                {
                                    dgvAttachmentColumnsBackColor[i] = "background-color:Khaki";
                                    dgvAttachmentColumnsWidth[i] = "100px";
                                }
                                catch { }

                            }
                            dgvAttachmentColumnsWidth[1] = "500px";
                            for (int i = 0; i < coloneA; i++)
                            {
                                //  dgvAttachment.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;
                            }

                            while (myReaderA.Read())
                            {
                                for (int col = 0; col < myReaderA.FieldCount; col++)
                                {
                                    dgvAttachment[RA, col] = myReaderA.GetValue(col).ToString();
                                }
                                RA++;
                            }
                        }
                    }
                    myReaderA.Close();
                    myReaderA.Dispose();
                    myReaderA = null;
                }
                catch
                {
                    int err = 0;
                }
                if (ligneA > 0)
                {
                    dgvAttachmentRowHeadersVisible = "visibility:visible";
                }

                ViewBag.ligneA = ligneA; 
                ViewBag.coloneA = coloneA;
                ViewBag.dgvAttachment = dgvAttachment; 

                ViewBag.lA = lA;
                ViewBag.dgvAttachmentTH = dgvAttachmentTH;
                ViewBag.dgvAttachmentRowHeadersVisible = dgvAttachmentRowHeadersVisible;
                ViewBag.dgvAttachmentColumnHeadersBackColor = dgvAttachmentColumnHeadersBackColor;
                ViewBag.dgvAttachmentColumnHeadersForeColor = dgvAttachmentColumnHeadersForeColor;

                ViewBag.dgvAttachmentColumnsBackColor = dgvAttachmentColumnsBackColor;
                ViewBag.dgvAttachmentColumnsWidth = dgvAttachmentColumnsWidth;

                ViewBag.lblAttCount = Convert.ToString(ligneA);
                #endregion
                /*fin LoadAttachment();*/
                var UserAction = "";
                ViewBag.lblRejMsg = value22.ToString();
                try
                {
                    UserAction = value26.ToString();
                    if (UserAction == "ACCEPTED")
                    {
                        ViewBag.lblRejMsg = "ALREADY ACCEPTED BY USER";
                        ViewBag.lblNextStage = "";
                    }
                }
                catch { }


                string Sql = "SELECT  DISTINCT  ";
                Sql = Sql + "  F1.[YourGrossBillAmount],F1.[YourTaxAmount],F1.[YourISCPercentage] ,F1.[YourISCAmount]";
                Sql = Sql + "  ,[YourOtherCommisionPercentage],F1.[YourOtherCommision],F1.[YourUATPPercentage],F1.[YourUATP],F1.[YourHandlingFeesPercentage] ,F1.[YourHandlingFees] ,F1.[YourVatAmountPercentage],F1.[YourVatAmount] ,F1.[YourNetAmourt]";

                Sql = Sql + " ,F1.[OurGrossBill],F1.[OurTaxAmount],F1.[OurISCPercentage],F1.[OurISCAmount],F1.[OurOtherCommissionPercentage] ,F1.[OurOtherCommission],";
                Sql = Sql + "  F1.[OurUATPPercentage],F1.[OurUATP],F1.[OurHandlingFeesPercentage],F1.[OurHandlingFees],F1.[OurVatAmountPercentage],F1.[OurVatAmount],F1.[OurNetAmount]";

                Sql = Sql + " ,F1.[DiffGrossAmount],F1.[DiffTaxAmount],F1.[DiffISCPercentage],F1.[DiffISCAmount],F1.[DiffOtherCommissionPercentage],F1.[DiffOtherCommission]";
                Sql = Sql + " ,F1.[DiffUATPPercentage],F1.[DiffUATP],F1.[DiffHandlingFeesPercentage],F1.[DiffHandlingFees],F1.[DiffVatAmountPercentage] ,F1.[DiffVatAmount],F1.[DiffNetAmount]";

                Sql = Sql + " , F1.[ReasonCode]";
                Sql = Sql + " ,F1.[ReasonDescription] ,F1.[NotInSystem] ";
                Sql = Sql + " FROM [XmlFile].[InvoiceAnalytics] F1";
                Sql = Sql + " left join [XmlFile].[DataLineItemDetails] f2";
                Sql = Sql + " ON    F1.[TransmisionId] =  F2.[TransmissionId]";
                Sql = Sql + " AND  F1.[InvoiceNo] = f2.[InvoiceNo]";
                Sql = Sql + " AND F1.[LineItemNumber] =f2.[LineItemNumber]";
                Sql = Sql + " AND  F1.[DetailNumber] =f2.[DetailNumber]";
                Sql = Sql + " where ";
                Sql = Sql + "  F1.[InvoiceNo] = '" + invoiceno + "'";
                Sql = Sql + " AND F1.[InvoiceDate]= '" + InvoiceDate + "'";
                Sql = Sql + " AND  F1.[BillingPeriod]= '" + BillingPeriod + "'";
                Sql = Sql + " AND  F1.[LineItemNumber]= '" + LineItemNumber + "'";
                Sql = Sql + " AND  F1.[DetailNumber]= '" + DetailNumber + "'";
                Sql = Sql + " AND  F1.[ChargeCode]= '" + ChargeCode + "'";
                Sql = Sql + " AND  F1.[DocumentNumber]= '" + DocumentNumber + "'";
                Sql = Sql + " AND  F1.[CouponNumber]= '" + CouponNumber + "'";

                SqlDataReader myReader;
                int R = 0;
                myReader = GetlistofTables(Sql);

                if (myReader != null)
                {
                    if (myReader.HasRows == true)
                    {
                        while (myReader.Read())
                        {
                            try { ViewBag.R1C1 = myReader.GetValue(0).ToString(); }
                            catch { }
                            try { ViewBag.R1C2 = myReader.GetValue(1).ToString(); }
                            catch { }
                            try { ViewBag.R1C3 = myReader.GetValue(2).ToString(); }
                            catch { }
                            try { ViewBag.R1C4 = myReader.GetValue(3).ToString(); }
                            catch { }
                            try { ViewBag.R1C5 = myReader.GetValue(4).ToString(); }
                            catch { }
                            try { ViewBag.R1C6 = myReader.GetValue(5).ToString(); }
                            catch { }
                            try { ViewBag.R1C7 = myReader.GetValue(6).ToString(); }
                            catch { }
                            try { ViewBag.R1C8 = myReader.GetValue(7).ToString(); }
                            catch { }
                            try { ViewBag.R1C9 = myReader.GetValue(8).ToString(); }
                            catch { }
                            try { ViewBag.R1C10 = myReader.GetValue(9).ToString(); }
                            catch { }
                            try { ViewBag.R1C11 = myReader.GetValue(10).ToString(); }
                            catch { }
                            try { ViewBag.R1C12 = myReader.GetValue(11).ToString(); }
                            catch { }
                            try {
                                ViewBag.R1C13 = myReader.GetValue(12).ToString();
                                string R1C13 = myReader.GetValue(12).ToString();
                                style = NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands;
                                provider = new CultureInfo("fr-FR");
                                v1 = Decimal.Parse(R1C13, style, provider);
                            }
                            catch { v1 = 0; }

                            try { ViewBag.R2C1 = myReader.GetValue(13).ToString(); }
                            catch { }
                            try { ViewBag.R2C2 = myReader.GetValue(14).ToString(); }
                            catch { }
                            try { ViewBag.R2C3 = myReader.GetValue(15).ToString(); }
                            catch { }
                            try { ViewBag.R2C4 = myReader.GetValue(16).ToString(); }
                            catch { }
                            try { ViewBag.R2C5 = myReader.GetValue(17).ToString(); }
                            catch { }
                            try { ViewBag.R2C6 = myReader.GetValue(18).ToString(); }
                            catch { }
                            try { ViewBag.R2C7 = myReader.GetValue(19).ToString(); }
                            catch { }
                            try { ViewBag.R2C8 = myReader.GetValue(20).ToString(); }
                            catch { }
                            try { ViewBag.R2C9 = myReader.GetValue(21).ToString(); }
                            catch { }
                            try { ViewBag.R2C10 = myReader.GetValue(22).ToString(); }
                            catch { }
                            try { ViewBag.R2C11 = myReader.GetValue(23).ToString(); }
                            catch { }
                            try { ViewBag.R2C12 = myReader.GetValue(24).ToString(); }
                            catch { }
                            try {
                                ViewBag.R2C13 = myReader.GetValue(25).ToString();
                                string R2C13 = myReader.GetValue(25).ToString();
                                style = NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands;
                                provider = new CultureInfo("fr-FR");
                                v2 = Decimal.Parse(R2C13, style, provider);
                            }
                            catch { v2 = 0; }

                            if (ChargeCode == "4")
                            {
                                try { ViewBag.R3C1 = myReader.GetValue(26).ToString(); }
                                catch { }
                                try { ViewBag.R3C2 = myReader.GetValue(27).ToString(); }
                                catch { }
                                try { ViewBag.R3C3 = myReader.GetValue(28).ToString(); }
                                catch { }
                                try { ViewBag.R3C4 = myReader.GetValue(29).ToString(); }
                                catch { }
                                try { ViewBag.R3C5 = myReader.GetValue(30).ToString(); }
                                catch { }
                                try { ViewBag.R3C6 = myReader.GetValue(31).ToString(); }
                                catch { }
                                try { ViewBag.R3C7 = myReader.GetValue(32).ToString(); }
                                catch { }
                                try { ViewBag.R3C8 = myReader.GetValue(33).ToString(); }
                                catch { }
                                try { ViewBag.R3C9 = myReader.GetValue(34).ToString(); }
                                catch { }
                                try { ViewBag.R3C10 = myReader.GetValue(35).ToString(); }
                                catch { }
                                try { ViewBag.R3C11 = myReader.GetValue(36).ToString(); }
                                catch { }
                                try { ViewBag.R3C12 = myReader.GetValue(37).ToString(); }
                                catch { }
                                try { ViewBag.R3C13 = myReader.GetValue(38).ToString(); }
                                catch { }

                            }
                            else
                            {
                                try { ViewBag.R3C1 = myReader.GetValue(26).ToString(); }
                                catch { }
                                try { ViewBag.R3C2 = myReader.GetValue(27).ToString(); }
                                catch { }
                                try { ViewBag.R3C3 = myReader.GetValue(28).ToString(); }
                                catch { }
                                try { ViewBag.R3C4 = myReader.GetValue(29).ToString(); }
                                catch { }
                                try { ViewBag.R3C5 = myReader.GetValue(30).ToString(); }
                                catch { }
                                try { ViewBag.R3C6 = myReader.GetValue(31).ToString(); }
                                catch { }
                                try { ViewBag.R3C7 = myReader.GetValue(32).ToString(); }
                                catch { }
                                try { ViewBag.R3C8 = myReader.GetValue(33).ToString(); }
                                catch { }
                                try { ViewBag.R3C9 = myReader.GetValue(34).ToString(); }
                                catch { }
                                try { ViewBag.R3C10 = myReader.GetValue(35).ToString(); }
                                catch { }
                                try { ViewBag.R3C11 = myReader.GetValue(36).ToString(); }
                                catch { }
                                try { ViewBag.R3C12 = myReader.GetValue(37).ToString(); }
                                catch { }
                                try { ViewBag.R3C13 = myReader.GetValue(38).ToString(); }
                                catch { }

                            }
                            #region R4C1
                            /*R4C1*/
                            /*try { R4C1 = myReader.GetValue(39).ToString(); }
                            catch { }
                            try { R4C2 = myReader.GetValue(40).ToString(); }
                            catch { }
                            try { R4C3 = myReader.GetValue(41).ToString(); }
                            catch { }
                            try { R4C4 = myReader.GetValue(42).ToString(); }
                            catch { }
                            try { R4C5 = myReader.GetValue(43).ToString(); }
                            catch { }
                            try { R4C6 = myReader.GetValue(44).ToString(); }
                            catch { }
                            try { R4C7 = myReader.GetValue(45).ToString(); }
                            catch { }
                            try { R4C8 = myReader.GetValue(46).ToString(); }
                            catch { }
                            try { R4C9 = myReader.GetValue(47).ToString(); }
                            catch { }
                            try { R4C10 = myReader.GetValue(48).ToString(); }
                            catch { }
                            try { R4C11 = myReader.GetValue(49).ToString(); }
                            catch { }
                            try { R4C12 = myReader.GetValue(51).ToString(); }
                            catch { }
                            try { R4C13 = myReader.GetValue(52).ToString(); }
                            catch { }
                            /* fin R4C1*/
                            #endregion


                            txtReasonCodeX = myReader.GetValue(39).ToString();
                            ViewBag.txtReasonCodeX = txtReasonCodeX;
                            txtReasonDescriptionX = myReader.GetValue(40).ToString();
                            ViewBag.txtReasonDescriptionX = txtReasonDescriptionX;
                            try {
                                NotinSystem = Convert.ToInt32(myReader.GetValue(41).ToString());
                                ViewBag.NotinSystem = NotinSystem;
                            }
                            catch { ViewBag.NotinSystem = NotinSystem; }
                        }
                    }
                    myReader.Close();
                    myReader.Dispose();
                    myReader = null;
                }
                if (ChargeCode == "4" || ChargeCode == "5" || ChargeCode == "6")
                {
                    string XTicketNo = DocumentNumber.Substring(3, DocumentNumber.Length - 3).ToString().Trim();
                    Sql = " SELECT Distinct TicketIssuingAirline  AS [Ticket Or FIM Issuing Airline],";
                    Sql = Sql + "[Ticketno]  AS [ Ticket Doc Or FIM Number],[CouponNo]  AS [Ticket Or FIM Coupon Number],";

                    Sql = Sql + "f1.[ChargeAmountName _ GrossBilled]  AS [Charge AmountName GrossBilled],";
                    Sql = Sql + "f1.[TaxAmountName _ Billed]  AS [TaxAmount Name] ";

                    Sql = Sql + "FROM [XmlFile].[DataLineItemDetails] F1 left join [XmlFile].[DataRejMemoCouponBrkDwn] ";
                    Sql = Sql + " F2 on F1.[TransmissionId]  =F2.TransmissionId ";
                    Sql = Sql + " AND F1.InvoiceNo= F2.InvoiceNo AND F1.DetailNumber = F2.DetailNumber";
                    Sql = Sql + " where f1.InvoiceNo <> ''  AND  F1.[InvoiceNo] = '" + invoiceno + "'";
                    Sql = Sql + "and [Ticketno]='" + XTicketNo + "'";
                    Sql = Sql + " AND  [CouponNo]= '" + CouponNumber + "'";

                    myReader = GetlistofTables(Sql);
                    if (myReader != null)
                    {
                        if (myReader.HasRows == true)
                        {
                            while (myReader.Read())
                            {
                            }
                        }
                    }
                    myReader.Close();
                    myReader.Dispose();
                    myReader = null;

                    Sql = "SELECT distinct [AddOnChargeName],[AddOnChargePercentage] ,[AddOnChargeAmount] FROM [XmlFile].[DataRejAddOnCharges] ";
                    Sql = Sql + " where InvoiceNo <> ''  AND  [InvoiceNo] = '" + invoiceno + "'";
                    Sql = Sql + "and [Ticketno]='" + XTicketNo + "'";
                    Sql = Sql + " AND  [CouponNo]= '" + CouponNumber + "' and [AddOnChargeName]='ISCAllowed' and DetailNumber = " + DetailNumber;

                    myReader = GetlistofTables(Sql);
                    if (myReader != null)
                    {
                        if (myReader.HasRows == true)
                        {
                            while (myReader.Read())
                            {
                            }
                        }
                    }
                    myReader.Close();
                    myReader.Dispose();
                    myReader = null;
                }
                if (v1 > v2)
                {
                    switch (ChargeCode)
                    {
                        case "1":
                            ViewBag.lblNextStage = "Next Stage: Charge Code 4 - 1st Rejection - OVER BILLING";
                            break;

                        case "4":
                            ViewBag.lblNextStage = "Next Stage Charge Code 5 - 2nd Rejection - OVER BILLING";
                          //  SubtotalsRej();
                            break;
                        case "5":
                            ViewBag.lblNextStage = "Next Stage Charge Code 6 - 3rd Rejection - OVER BILLING ";
                          //  SubtotalsRej();
                            break;
                        case "6":
                            ViewBag.lblNextStage = "Next Stage Correnpondance - OVER BILLING";
                          //  SubtotalsRej();
                            break;
                    }
                    switch (ChargeCode)
                    {
                        case "1":
                            break;
                        case "4":
                        case "5":
                        case "6":
                            #region SubtotalsRej
                            /*SubtotalsRej();*/
                            /*decimal A1 = 0;// txtRlogBAGB.Text = "0.000";
                            decimal A2 = 0;//txtRlogBATax.Text = "0.000";
                            decimal A3 = 0;//txtRlogBAISCPer.Text = "0.000";
                            decimal A4 = 0;//txtRlogBAISCAmt.Text = "0.000";
                            decimal A5 = 0;//txtRlogBAOthComm.Text = "0.000";
                            decimal A6 = 0;//txtRlogBAUatp.Text = "0.000";
                            decimal A7 = 0;//txtRlogBAHndFee.Text = "0.000";
                            decimal A8 = 0;//txtRlogBAVat.Text = "0.000";
                            decimal A9 = 0;//txtRlogBANetAmt.Text = "0.000";

                            decimal B1 = 0;//txtRlogAAGB.Text = "0.000";
                            decimal B2 = 0;//txtRlogAATax.Text = "0.000";
                            decimal B3 = 0;//txtRlogAAISCPer.Text = "0.000";
                            decimal B4 = 0;//txtRlogAAISCAmt.Text = "0.000";
                            decimal B5 = 0;// txtRlogAAOthComm.Text = "0.000";
                            decimal B6 = 0;// txtRlogAAUatp.Text = "0.000";
                            decimal B7 = 0;// txtRlogAAHndFee.Text = "0.000";
                            decimal B8 = 0;//txtRlogAAVat.Text = "0.000";
                            decimal B9 = 0;//txtRlogAANetAmt.Text = "0.000";

                            decimal C1 = 0;//txtRlogDiffGB.Text = "0.000";
                            decimal C2 = 0;//txtRlogDiffTax.Text = "0.000";
                            decimal C3 = 0;//txtRlogDiffISCPer.Text = "0.000";
                            decimal C4 = 0;//txtRlogDiffISCAmt.Text = "0.000";
                            decimal C5 = 0;//txtRlogDiffOthComm.Text = "0.000";
                            decimal C6 = 0;//txtRlogDiffUatp.Text = "0.000";
                            decimal C7 = 0;//txtRlogDiffHndFee.Text = "0.000";
                            decimal C8 = 0;//txtRlogDiffVat.Text = "0.000";
                            decimal C9 = 0;//txtRlogDiffNetAmt.Text = "0.000";


                            try {
                                A1 = Convert.ToDecimal(txtRlogBAGB.Text);
                            }
                            catch { A1 = 0; }
                            try { A2 = Convert.ToDecimal(txtRlogBATax.Text); }
                            catch { A2 = 0; }
                            try { A3 = Convert.ToDecimal(txtRlogBAISCPer.Text); }
                            catch { A3 = 0; }
                            try { A4 = Convert.ToDecimal(txtRlogBAISCAmt.Text); }
                            catch { A4 = 0; }
                            try { A5 = Convert.ToDecimal(txtRlogBAOthComm.Text); }
                            catch { A5 = 0; }
                            try { A6 = Convert.ToDecimal(txtRlogBAUatp.Text); }
                            catch { A6 = 0; }
                            try { A7 = Convert.ToDecimal(txtRlogBAHndFee.Text); }
                            catch { A7 = 0; }
                            try { A8 = Convert.ToDecimal(txtRlogBAVat.Text); }
                            catch { A8 = 0; }

                            A9 = A1 + A2 + A4 + A5 + A6 + A7 + A8;
                            txtRlogBANetAmt.Text = A9.ToString("####.000");



                            try { B1 = Convert.ToDecimal(txtRlogAAGB.Text); }
                            catch { B1 = 0; }
                            try { B2 = Convert.ToDecimal(txtRlogAATax.Text); }
                            catch { B2 = 0; }
                            try { B3 = Convert.ToDecimal(txtRlogAAISCPer.Text); }
                            catch { B3 = 0; }
                            try { B4 = Convert.ToDecimal(txtRlogAAISCAmt.Text); }
                            catch { B4 = 0; }
                            try { B5 = Convert.ToDecimal(txtRlogAAOthComm.Text); }
                            catch { B5 = 0; }
                            try { B6 = Convert.ToDecimal(txtRlogAAUatp.Text); }
                            catch { B6 = 0; }
                            try { B7 = Convert.ToDecimal(txtRlogAAHndFee.Text); }
                            catch { B7 = 0; }
                            try { B8 = Convert.ToDecimal(txtRlogAAVat.Text); }
                            catch { B8 = 0; }

                            B9 = B1 + B2 + B4 + B5 + B6 + B7 + B8;
                            txtRlogAANetAmt.Text = B9.ToString("####.000");


                            try { C1 = Convert.ToDecimal(txtRlogDiffGB.Text); }
                            catch { C1 = 0; }
                            try { C2 = Convert.ToDecimal(txtRlogDiffTax.Text); }
                            catch { C2 = 0; }
                            try { C3 = Convert.ToDecimal(txtRlogDiffISCPer.Text); }
                            catch { C3 = 0; }
                            try { C4 = Convert.ToDecimal(txtRlogDiffISCAmt.Text); }
                            catch { C4 = 0; }
                            try { C5 = Convert.ToDecimal(txtRlogDiffOthComm.Text); }
                            catch { C5 = 0; }
                            try { C6 = Convert.ToDecimal(txtRlogDiffUatp.Text); }
                            catch { C6 = 0; }
                            try { C7 = Convert.ToDecimal(txtRlogDiffHndFee.Text); }
                            catch { C7 = 0; }
                            try { C8 = Convert.ToDecimal(txtRlogDiffVat.Text); }
                            catch { C8 = 0; }

                            C9 = C1 + C2 + C4 + C5 + C6 + C7 + C8;
                            txtRlogDiffNetAmt.Text = C9.ToString("####.000");*/
                            #endregion
                            /*fin*/
                            break;
                    }
                }
                if (v1 <= v2)
                {
                    ViewBag.lblNextStage = "UNDER-BILLING - YOU MAY ACCEPT THIS TRANSACTION";
                    ViewBag.lblRejMsg = "UNDER-BILLING";
                    ViewBag.pnlAcceptVisible = "visibility:visible";
                }

                if (v1 == v2)
                {
                    ViewBag.lblNextStage = "MATCHED TRANSACTION  - YOU MAY ACCEPT THIS TRANSACTION";
                    ViewBag.lblRejMsg = "MATCHED TRANSACTION";
                    ViewBag.pnlAcceptVisible = "visibility:visible";
                }

                if (txtReasonCode.ToString().Trim() == "2B")
                {
                    ViewBag.lblRejMsg = "REJECTED DUE TO DUPLICATE BILLING.";
                    ViewBag.lblNextStage = "CHECK OURWARD BILLING RECORD.";
                }
                if (UserAction == "ACCEPTED")
                {
                    ViewBag.lblRejMsg = "ALREADY ACCEPTED BY USER";
                    ViewBag.lblNextStage = "";
                }
                String TicketNo = DocumentNumber.Substring(3, DocumentNumber.Length - 3).ToString().Trim();

                if (UserAction == "REJECTED")
                {
                    #region UpdatedTaxBreakdownRej(invoiceno, TicketNo, CouponNumber);
                    string Sqlu = "select TicketNumber as [Document Number],CouponNumber as[ Cpn], TaxCode,TaxAmount as [Amount],Accepted as [Accepted] ,Diff as [Diff] from [XmlFile].[InvoiceAnalyticsTaxBkDwn] ";
                    Sqlu = Sqlu + " where InvoiceNumber='" + invoiceno + "'";
                    Sqlu = Sqlu + " AND [TicketNumber] ='" + DocumentNumber + "'";
                    Sqlu = Sqlu + " AND [CouponNumber] = " + CouponNumber;
                    try
                    {
                        SqlDataReader myReaderu;
                        int Ru = 0;
                        myReaderu = GetlistofTables(Sqlu);

                        DataSet dsu = new DataSet();
                        dsu = dbconnect.RetObjDS(Sqlu);

                        int ligneu = dsu.Tables[0].Rows.Count;
                        int coloneu = dsu.Tables[0].Columns.Count;
                        string[] dataGridView21TH = new string[coloneu];
                        string[,] dataGridView21 = new string[ligneu,coloneu];
                        string dataGridView21RowHeadersVisible = "";
                        string dataGridView21ColumnHeadersBackColor = "";
                        string dataGridView21ColumnHeadersForeColor = "";
                        string[] dataGridView21ColumnsBackColor = new string[coloneu]; 
                        string[] dataGridView21ColumnsWidth = new string[coloneu];
                        string lblRejTotalTax = "";
                        decimal RejTotalTax = 0;


                        if (myReaderu != null) {
                            for (int i = 0; i < myReaderu.FieldCount; i++)
                            {
                                dataGridView21TH[i] = myReaderu.GetName(i).ToString();
                            }
                            dataGridView21TH[3] = "Tax Amount";

                            dataGridView21RowHeadersVisible = "visibility:hidden";
                            dataGridView21ColumnHeadersBackColor = "background-color:Maroon";
                            dataGridView21ColumnHeadersForeColor = "color:White";

                            for (int i = 0; i < coloneu; i++)
                            {
                                try
                                {
                                    dataGridView21ColumnsBackColor[i] = "background-color:Khaki";
                                    dataGridView21ColumnsWidth[1] = "100px";
                                }
                                catch { }
                            }
                            if (myReaderu.HasRows == true)
                            {
                                while (myReaderu.Read())
                                {
                                    RejTotalTax = RejTotalTax + Convert.ToDecimal(myReaderu.GetValue(3));
                                    for (int i = 0; i < myReaderu.FieldCount; i++)
                                    {
                                        dataGridView21[Ru,i] = myReaderu.GetValue(i).ToString();
                                    }
                                    Ru++;
                                }
                                lblRejTotalTax = RejTotalTax.ToString("####.000");
                            }
                            dataGridView21ColumnsWidth[1] = "45px";
                            dataGridView21ColumnsWidth[2] = "35px";
                            dataGridView21ColumnsWidth[3] = "50px";
                            dataGridView21ColumnsWidth[4] = "75px";
                            dataGridView21ColumnsWidth[5] = "50px";

                            myReaderu.Close();
                            myReaderu.Dispose();
                            myReaderu = null;
                        }

                        if (ligneu == 0)
                        {
                            string Sqld = "SELECT  distinct ";
                            Sqld = Sqld + " [TicketNumber] as [Ticket No],[CouponNumber] as CPN,[TaxCode],[Billed] as   [Tax Amount Name Billed],[Accepted] ,[Diff]";
                            Sqld = Sqld + " FROM [XmlFile].[InvoiceAnalyticsTaxBkDwn]";
                            Sqld = Sqld + " where [InvoiceNumber]='" + invoiceno + "'";
                            Sqld = Sqld + " and [TicketNumber]='" + DocumentNumber + "'";
                            Sqld = Sqld + " and [CouponNumber]=" + CouponNumber;
                            try
                            {
                                SqlDataReader myReaderd;
                                int Rd = 0;
                                myReaderd = GetlistofTables(Sqld);
                                DataSet dsd = new DataSet();
                                dsd = dbconnect.RetObjDS(Sqld);

                                int ligned = dsd.Tables[0].Rows.Count;
                                int coloned = dsd.Tables[0].Columns.Count;

                                if (myReaderd != null)
                                {

                                    for (int i = 0; i < myReaderd.FieldCount; i++)
                                    {
                                        dataGridView21TH[i] = myReaderd.GetName(i).ToString();
                                    }
                                    dataGridView21TH[3] = "Tax Amount";

                                    lblRejTotalTax = "0.000";
                                    if (myReaderd.HasRows == true)
                                    {
                                        while (myReaderd.Read())
                                        {

                                            RejTotalTax = RejTotalTax + Convert.ToDecimal(myReaderd.GetValue(3));
                                            for (int i = 0; i < myReaderd.FieldCount; i++)
                                            {
                                                dataGridView21[Rd, i] = myReaderd.GetValue(i).ToString();
                                            }
                                            Rd++;
                                        }
                                    }
                                    myReaderd.Close();
                                    myReaderd.Dispose();
                                    myReaderd = null;
                                   ligneu = ligned;
                                   coloneu = coloned;
                                }
                            }
                            catch { }
                        }

                        ViewBag.ligneu = ligneu;
                        ViewBag.coloneu = coloneu;

                        ViewBag.dataGridView21TH = dataGridView21TH;

                        ViewBag.dataGridView21 = dataGridView21;

                        ViewBag.dataGridView21RowHeadersVisible = dataGridView21RowHeadersVisible;
                        ViewBag.dataGridView21ColumnHeadersBackColor = dataGridView21ColumnHeadersBackColor;
                        ViewBag.dataGridView21ColumnHeadersForeColor = dataGridView21ColumnHeadersForeColor;

                        ViewBag.dataGridView21ColumnsBackColor = dataGridView21ColumnsBackColor;
                        ViewBag.dataGridView21ColumnsWidth = dataGridView21ColumnsWidth;

                        ViewBag.lblRejTotalTax = lblRejTotalTax;
                        ViewBag.invoiceno = invoiceno;
                        ViewBag.TicketNo = TicketNo;
                        ViewBag.CouponNumber = CouponNumber;
                    }
                    catch
                    {
                        int Err = 0;

                    }
                    #endregion
                }
                else
                {
                    #region  TaxBreakdownRej(invoiceno, TicketNo, CouponNumber);

                    string Sqlt = "SELECT  distinct ";
                    // Sql = Sql + "[InvoiceNo] AS [Invoice No]";
                    Sqlt = Sqlt + "[TicketNo] as [Ticket No]";
                    Sqlt = Sqlt + ",[CouponNo] as [CPN]";
                    Sqlt = Sqlt + ",[TaxCode] as [Tax Code]";
                    Sqlt = Sqlt + ",[TaxAmountName _ Billed] as [Tax Amount Name Billed] ";
                    Sqlt = Sqlt + ",Accepted,Diff";

                    Sqlt = Sqlt + " FROM [XmlFile].[DataLineItemDetetailsTXBrkDnw]";
                    Sqlt = Sqlt + " where InvoiceNo='" + invoiceno + "'";
                    Sqlt = Sqlt + " and TicketNo='" + DocumentNumber + "'";
                    Sqlt = Sqlt + " and CouponNo=" + CouponNumber;
                    decimal RejTotalTax = 0;

                    try
                    {
                        SqlDataReader myReadert;
                        int Rt = 0;
                        myReadert = GetlistofTables(Sqlt);

                        DataSet dst = new DataSet();
                        dst = dbconnect.RetObjDS(Sqlt);

                        int lignet = dst.Tables[0].Rows.Count;
                        int colonet = dst.Tables[0].Columns.Count;
                        string[] dataGridView21TH = new string[colonet];
                        string[,] dataGridView21 = new string[lignet, colonet];
                        string dataGridView21RowHeadersVisible = "";
                        string dataGridView21ColumnHeadersBackColor = "";
                        string dataGridView21ColumnHeadersForeColor = "";
                        string[] dataGridView21ColumnsBackColor = new string[colonet];
                        string[] dataGridView21ColumnsWidth = new string[colonet];
                        string lblRejTotalTax = ""; 
                        string[,] dataGridView21StyleBackColor = new string[lignet, colonet];

                        if (myReadert != null) {
                            for (int i = 0; i < myReadert.FieldCount; i++)
                            {
                                dataGridView21TH[i] = myReadert.GetName(i).ToString();
                            }
                            dataGridView21TH[3] = "Tax Amount";

                            dataGridView21RowHeadersVisible = "visibility:hidden";
                            dataGridView21ColumnHeadersBackColor = "background-color:Maroon";
                            dataGridView21ColumnHeadersForeColor = "color:White";

                            for (int i = 0; i < colonet; i++)
                            {
                                try
                                {
                                    dataGridView21ColumnsBackColor[i] = "background-color:Khaki";
                                    dataGridView21ColumnsWidth[1] = "75px";
                                }
                                catch { }
                            }
                            lblRejTotalTax = "0.000";
                            if (myReadert.HasRows == true)
                            {
                                while (myReadert.Read())
                                {
                                    RejTotalTax = RejTotalTax + Convert.ToDecimal(myReadert.GetValue(3));
                                    for (int i = 0; i < myReadert.FieldCount; i++)
                                    {
                                        dataGridView21[Rt,i] = myReadert.GetValue(i).ToString();
                                    }
                                    Rt++;
                                }
                                lblRejTotalTax = RejTotalTax.ToString("####.000");
                            }
                            dataGridView21ColumnsWidth[1] = "45px";
                            dataGridView21ColumnsWidth[2] = "35px";
                            dataGridView21ColumnsWidth[3] = "50px";
                            dataGridView21ColumnsWidth[4] = "75px";
                            dataGridView21ColumnsWidth[5] = "50px";
                            myReadert.Close();
                            myReadert.Dispose();
                            myReadert = null;
                            for (int j = 0; j < lignet; j++)
                            {
                                var XX = dataGridView21[j, 5];

                                if (Convert.ToDecimal(XX) > 0)
                                {
                                    for (int i = 0; i < colonet; i++)
                                    {
                                        dataGridView21StyleBackColor[j,i] = "background-color:Red";
                                    }
                                }
                                else
                                {
                                    for (int i = 0; i < colonet; i++)
                                    {
                                        dataGridView21StyleBackColor[j,i] = "background-color:PaleGreen";
                                    }
                                }
                            }
                        }

                        if(lignet == 0)
                        {
                            string Sqld = "SELECT  distinct ";
                            Sqld = Sqld + " [TicketNumber] as [Ticket No],[CouponNumber] as CPN,[TaxCode],[Billed] as   [Tax Amount Name Billed],[Accepted] ,[Diff]";
                            Sqld = Sqld + " FROM [XmlFile].[InvoiceAnalyticsTaxBkDwn]";
                            Sqld = Sqld + " where [InvoiceNumber]='" + invoiceno + "'";
                            Sqld = Sqld + " and [TicketNumber]='" + DocumentNumber + "'";
                            Sqld = Sqld + " and [CouponNumber]=" + CouponNumber;
                            try
                            {
                                SqlDataReader myReaderd;
                                int Rd = 0;
                                myReaderd = GetlistofTables(Sqld);
                                DataSet dsd = new DataSet();
                                dsd = dbconnect.RetObjDS(Sqld);

                                int ligned = dsd.Tables[0].Rows.Count;
                                int coloned = dsd.Tables[0].Columns.Count;

                                if (myReaderd != null) {

                                    for (int i = 0; i < myReaderd.FieldCount; i++)
                                    {
                                        dataGridView21TH[i] = myReaderd.GetName(i).ToString();
                                    }
                                    dataGridView21TH[3] = "Tax Amount";

                                    lblRejTotalTax = "0.000";
                                    if (myReaderd.HasRows == true)
                                    {
                                        while (myReaderd.Read())
                                        {

                                            RejTotalTax = RejTotalTax + Convert.ToDecimal(myReaderd.GetValue(3));
                                            for (int i = 0; i < myReaderd.FieldCount; i++)
                                            {
                                                dataGridView21[Rd,i] = myReaderd.GetValue(i).ToString();
                                            }
                                            Rd++;
                                        }
                                    }
                                    myReaderd.Close();
                                    myReaderd.Dispose();
                                    myReaderd = null;
                                    lignet = ligned;
                                    colonet = coloned;
                                }
                            }
                            catch { }
                        }

                        ViewBag.ligneu = lignet;
                        ViewBag.coloneu = colonet;

                        ViewBag.dataGridView21TH = dataGridView21TH;

                        ViewBag.dataGridView21 = dataGridView21;

                        ViewBag.dataGridView21RowHeadersVisible = dataGridView21RowHeadersVisible;
                        ViewBag.dataGridView21ColumnHeadersBackColor = dataGridView21ColumnHeadersBackColor;
                        ViewBag.dataGridView21ColumnHeadersForeColor = dataGridView21ColumnHeadersForeColor;

                        ViewBag.dataGridView21ColumnsBackColor = dataGridView21ColumnsBackColor;
                        ViewBag.dataGridView21StyleBackColor = dataGridView21StyleBackColor;
                        ViewBag.dataGridView21ColumnsWidth = dataGridView21ColumnsWidth;

                        ViewBag.lblRejTotalTax = lblRejTotalTax;

                        ViewBag.invoiceno = invoiceno;
                        ViewBag.TicketNo = TicketNo;
                        ViewBag.CouponNumber = CouponNumber;
                    }
                    catch
                    {
                        int Err = 0;
                    }
                    #endregion
                }


            }
            catch { }
            ViewBag.pvChargeCode = pvChargeCode;
            ViewBag.FullDocNo = FullDocNo;
            ViewBag.PreviousBillingPeriod = PreviousBillingPeriod;
            
            return PartialView();
        }
        public ActionResult testR3C2()
        {
            string R3C2 = Request["R3C2"].Trim();
            decimal R3C2d;
            NumberStyles style;
            CultureInfo provider;

            style = NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands;
            provider = new CultureInfo("fr-FR");

            R3C2d = Decimal.Parse(R3C2, style, provider);

            if (R3C2d == 0) { ViewBag.button19Enabled = "true"; } else { ViewBag.button19Enabled = "true"; }
            return PartialView();
        }
        public ActionResult LoadRefReasonCode()
        {
            string pvChargeCode = Request["pvChargeCode"].Trim();
            int cc;
            if(pvChargeCode != "")
            {
                 cc = Convert.ToInt16(pvChargeCode);
            }
            else
            {
                 cc = 0;
            }
            string Param = "";

            switch (cc)
            {
                case 1:
                case 2:
                case 3:
                case 4:
                case 5:
                case 6:
                    Param = "'P'";
                    break;
                case 9:
                    Param = "'B','C'";
                    break;
                default:
                    Param = "'P','B','C'";
                    break;
            }
            string Sql = "SELECT  [ReasonCode]+ ' '+ [ReasonDescription] As [Reason Code] FROM [Ref].[ReasonCodes]";
            Sql = Sql + "where [ReasonType] in(" + Param + ")";

            SqlConnection cs = new SqlConnection(pbConnectionString);
            DataSet ds = new DataSet();
            SqlDataAdapter ada = new SqlDataAdapter(Sql, cs);
            ada.Fill(ds);
            int ligne = ds.Tables[0].Rows.Count;
            string[] cboReasonCode = new string[ligne];

            try
            {
                SqlDataReader myReader;
                int R = 0;
                myReader = GetlistofTables(Sql);

                if (myReader != null) {
                    if (myReader.HasRows == true)
                    {
                        while (myReader.Read())
                        {
                            cboReasonCode[R] = myReader.GetValue(0).ToString();
                            R++;
                        }
                    }
                    myReader.Close();
                    myReader.Dispose();
                    myReader = null;
                }
            }
            catch { }

            ViewBag.ligne = ligne;
            ViewBag.cboReasonCode = cboReasonCode;
            return PartialView();
        }
        /*fin fait par christian*/

                }
                    }