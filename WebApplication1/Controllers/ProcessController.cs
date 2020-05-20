using System;
using System.Data;
using WebApplication1.Models;
using System.Data.SqlClient;
using System.Web.Mvc;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.IO;
using System.Xml;
using System.Net;
using System.Text;


/* Export */
using System.Web.WebPages;
using System.Threading;
using System.Xml;
using System.Xml.Linq;

namespace WebApplication1.Controllers
{
    public class ProcessController : Controller
    {

        //public string pbConnectionString = "Server=.\\RELATE;Database=OnsiteBiatss_KK;User Id=sa; Password=1234";
        //public string pbConnectionString = "Server=FANO-PC;Database=OnsiteBiatss_KK;User Id=so; Integrated Security=True";
        public string pbConnectionString = "Server=DESKTOP-Q821GFS;Database=OnsiteBiatss_KK;User Id=sa; Password=1234";
        //public string pbConnectionString = "Server=DESKTOP-O0K2BQJ\\SA;Database=OnsiteBiatss_KK;User Id=sa; Password=1234";
        //public string pbConnectionString = "Server=DESKTOP-O0K2BQJ\\SA;Database=OnsiteBiatss_KK;User Id=sa; Password=1234";
        //public string pbConnectionString = "Server=DESKTOP-7HJUR50;Database=OnsiteBiatss_KK;User Id=sa; Password=1234";
        //public string pbConnectionString = "Server=DESKTOP-CGR76E3;Database=OnsiteBiatss_KK;User Id=sa; Password=1234";
        //public string pbConnectionString = "Server=DESKTOP-54APPF8\\SYMPHONY2;Database=OnsiteBiatss_KK;User Id=sa; Password=1234";
       // public string pbConnectionString = "Server=DESKTOP-O0K2­BQJ\\SA;Database=Ons­iteBiatss_KK;User Id=sa; Password=1234";
        //  public string pbConnectionString = "Server=DESKTOP-DORH05Q;Database=OnsiteBiatss_KK;User Id=sa; Password=1234";

        ConnexionSQLServer.DbConnection dbconnect = new ConnexionSQLServer.DbConnection();
        String es = "";
        String txtCustomerCode = "";


       SymphonyEntities ef;
        public string DocumentNumber = "";
        public string CouponNo = "";
        public string ChargeCode = "";
        public string CorrespondenceNo = "";
        public string Carrier = "";
        public string DocumentReference = "";
        public string InvoiceNumber = "";
        public string DocumentAmount = "";
        public string AmountAccepted = "";
        public string CoresNo = "";

        // GET: Process
        public ActionResult Index()
        {
            string dtpFromValue = DateTime.Now.AddMonths(-1).ToString("dd-MMM-yyyy");
            string dtpToValue = DateTime.Now.ToString("dd-MMM-yyyy");

            ViewBag.dtpFromValue = dtpFromValue;
            ViewBag.dtpToValue = dtpToValue;
            return PartialView();
        }
        public string ConvertDate(string date)
          {
              string mydate = DateTime.ParseExact(date, "dd-MMM-yyyy", CultureInfo.InvariantCulture).ToString("dd-MMM-yyyy");
              return mydate;
          }
        

        /* Proration     Joseph */
        


        public ActionResult Proration()
        {
            string numberTicket = Request["number"];

            string sql = "";
            sql = "select f1.DateofIssue, f1.FareCurrency, f1.Fare, f1.TotalCurrency, f1.EquivalentFare, f1.FareCalculationArea, srd.RelatedDocumentGuid, f1.OriginalIssueDate, f1.SaleDate " + Environment.NewLine;
            sql += "from Pax.SalesRelatedDocumentInformation srd" + Environment.NewLine;
            sql += "join Pax.SalesDocumentHeader f1 on RelatedDocumentNumber = '" + numberTicket + "' and  IsConjunction=0" + Environment.NewLine;
            sql += "and f1.HdrGuid = srd.HdrGuid" + Environment.NewLine;
            sql += "and srd.TransactionCode <> 'RFND'";


            SqlConnection con2 = new SqlConnection(pbConnectionString);
            DataSet ds = new DataSet();
            SqlDataAdapter ada = new SqlDataAdapter(sql, con2);

            ada.Fill(ds);
            // con.Close();

            int lon = ds.Tables[0].Rows.Count;
            if (lon == 0)
            {
                ViewBag.mes = "No data available for the selected criteria.";
            }

            ViewBag.log = lon;

            string doi = "";
            string txtFare = "";
            string txtFareNumber = "";
            string txtEfp = "";
            string txtEfpNumber = "";
            string txtFca = "";
            string rel = "";
            string txtOrigIssueDate = "";
            string txtDateOfSale = "";


            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                doi = dr[ds.Tables[0].Columns[0].ColumnName].ToString();
                txtFare = dr[ds.Tables[0].Columns[1].ColumnName].ToString();
                txtFareNumber = dr[ds.Tables[0].Columns[2].ColumnName].ToString();
                txtEfp = dr[ds.Tables[0].Columns[3].ColumnName].ToString();
                txtEfpNumber = dr[ds.Tables[0].Columns[4].ColumnName].ToString();
                txtFca = dr[ds.Tables[0].Columns[5].ColumnName].ToString();
                rel = dr[ds.Tables[0].Columns[6].ColumnName].ToString();
                txtOrigIssueDate = dr[ds.Tables[0].Columns[7].ColumnName].ToString();
                txtDateOfSale = dr[ds.Tables[0].Columns[8].ColumnName].ToString();
            }

            ViewBag.fare = txtFare + " " + txtFareNumber;
            ViewBag.Efp = txtEfp + " " + txtEfpNumber;
            ViewBag.dtpDateOfSale = doi;
            ViewBag.Fca = txtFca;
            ViewBag.rel = rel;

            ViewBag.txtOrigIssueDate = txtOrigIssueDate;
            ViewBag.txtDateOfSale = txtDateOfSale;

            ViewBag.numberTicket = numberTicket;

            /* Detail Coupon View */
            Grid(Display(numberTicket));
            /* End Detail Coupon View*/

            return PartialView();
        }

        /* End Proration  jOSEPH */

        /*********fait par christian*****/
        public ActionResult Facsimile()
        {

            string docNumber = Request["docNumber"];
            SqlConnection con = new SqlConnection(pbConnectionString);

            /********************************************************************************************/

            string fistDocNumber = docNumber.Substring(0, 3);
            string secondDocNumber = docNumber.Substring(3, 10);
            string sql = "SELECT FORMAT(F1.DateofIssue,'dd-MMM-yyyy') as DateOfIssue ,F1.*, F2.* FROM[Pax].[SalesDocumentHeader] F1 left join Ref.VW_Agent F2 on substring(F1.AgentNumericCode, 1, 7) =";
            sql += "substring(F2.AgencyNumericCode, 1, 7) where F1.DocumentNumber = '" + docNumber + "' ";

            string sqlMdl = "SELECT item.CouponNumber, item.StopOverCode,  item.Carrier, item.FlightNumber,  item.OriginAirportCityCode + '-' + item.DestinationAirportCityCode as GFPA, ";
            sqlMdl += "FORMAT(item.FlightDepartureDate,'dd-MMM-yyyy') as FlightDepartureDate, item.FlightDepartureTime, item.CouponStatus, item.FareBasisTicketDesignator,FORMAT(item.NotValidBefore,'dd-MMM-yyyy') as NotValidBefore,";
            sqlMdl += "FORMAT(item.NotValidAfter,'dd-MMM-yyyy') as NotValidAfter, item.FreeBaggageAllowance, item.FlightBookingStatus, item.UsedClassofService, ";
            sqlMdl += "item.UsageOriginCode + '-' + item.UsageDestinationCode as UsageSector ,item.UsageAirline, item.UsageFlightNumber, item.RelatedDocumentNumber ,F2.IsConjunction as isConjonction,";
            sqlMdl += "FORMAT(item.UsageDate,'dd-MMM-yyyy') as UsageDate, item.FrequentFlyerReference from Pax.SalesDocumentHeader F1 ";
            sqlMdl += "join Pax.SalesRelatedDocumentInformation F2 on F1.HdrGuid = F2.HdrGuid ";
            sqlMdl += "join Pax.SalesDocumentCoupon item on F2.RelatedDocumentGuid = item.RelatedDocumentGuid   where F1.DocumentNumber = '" + docNumber + "' order by item.RelatedDocumentNumber , item.CouponNumber";

            string cnj = "SELECT DocumentNumber, RelatedDocumentNumber from Pax.SalesRelatedDocumentInformation where DocumentNumber = '" + docNumber + "' and DocumentNumber <> RelatedDocumentNumber ";



            string sqlHisto = "select top 1 concat(substring(f1.AccountNumber,1, 3),substring(f1.AccountNumber,4, 10))+'  /'+ltrim(rtrim(substring(f1.AccountNumber,15, len(f1.accountnumber)))) as DocCpn ";
            sqlHisto += " from pax.SalesDocumentPayment f1 where DocumentNumber = '" + docNumber + "' and FormofPaymentType = 'EX'";
            dbconnect.pbConnectionString = pbConnectionString;
            SqlDataReader readerHisto = dbconnect.GetData(sqlHisto);
            string exchange = "";
            while (readerHisto.Read())
            {
                exchange = readerHisto["DocCpn"].ToString();
            }

            /* get cpui */

            string sqlCpui = "select CouponUseIndicator from [FileHot].[Bks24_DocumentIdentification] where TicketDocumentNumber = '" + docNumber + "' ";
            DataSet ds = new DataSet();
            ds = dbconnect.RetObjDS(sqlCpui);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                ViewBag.cpui = dr[ds.Tables[0].Columns[0].ColumnName].ToString();
            }



            string sql1 = "SELECT right(relatedDocumentNumber,10), RelatedTicketCheckDigit from pax.SalesRelatedDocumentInformation where relatedDocumentNumber = '" + docNumber + "' ";
            DataSet ds1 = new DataSet();
            ds1 = dbconnect.RetObjDS(sql1);
            foreach (DataRow dr1 in ds1.Tables[0].Rows)
            {
                ViewBag.ChkCode = dr1[ds1.Tables[0].Columns[1].ColumnName].ToString();
            }



            ViewData["exchange"] = exchange;
            ViewBag.transaction = GetTransaction(sql, fistDocNumber, secondDocNumber);
            ViewBag.transactionMiddle = GetTransactionMiddle(sqlMdl);
            ViewBag.conjoctionTicket = GetConjoctionTicket(cnj);
            ViewBag.ticketHistory = ticketHistory(docNumber);
            //ViewBag.payment = ticketHistoryPayment(docNumber, transactionCode); 
            /* string tabcity = City();
             ViewBag.tabCity = tabcity;*/


            return PartialView();
        }
        private List<TransactionHeaderModel> GetTransaction(string str, string fistDocNumber, string secondDocNumber)
        {
            dbconnect.pbConnectionString = pbConnectionString;
            SqlDataReader myReader = dbconnect.GetData(str);
            List<TransactionHeaderModel> model = new List<TransactionHeaderModel>();
            var iii = myReader.FieldCount;
            if (myReader.HasRows)
            {

                while (myReader.Read())
                {
                    var transaction = new TransactionHeaderModel();
                    transaction.firstDocNumber = fistDocNumber.ToString();
                    transaction.secondDocNumber = secondDocNumber.ToString();
                    transaction.CheckDigit = myReader["CheckDigit"].ToString();
                    transaction.TrueOriginDestinationCityCodes = myReader["TrueOriginDestinationCityCodes"].ToString();
                    transaction.FareCalculationArea = myReader["FareCalculationArea"].ToString();
                    transaction.FareCurrency = myReader["FareCurrency"].ToString();
                    transaction.Fare = myReader["Fare"].ToString();
                    transaction.EquivalentFare = myReader["EquivalentFare"].ToString();
                    transaction.BookingReference = myReader["BookingReference"].ToString();
                    transaction.PassengerName = myReader["PassengerName"].ToString();
                    transaction.DateofIssue = myReader["DateOfIssue"].ToString();
                    transaction.AgentNumericCode = myReader["AgentNumericCode"].ToString();
                    transaction.BookingAgentIdentification = myReader["BookingAgentIdentification"].ToString();
                    transaction.ReportingSystemIdentifier = myReader["ReportingSystemIdentifier"].ToString();
                    transaction.VendorIdentification = myReader["VendorIdentification"].ToString();
                    transaction.LegalName = myReader["LegalName"].ToString();
                    transaction.LocationAddress = myReader["LocationAddress"].ToString();
                    transaction.LocationCity = myReader["LocationCity"].ToString();
                    transaction.LocationCountry = myReader["LocationCountry"].ToString();
                    transaction.Tax1Amount = myReader["Tax1Amount"].ToString();
                    transaction.Tax2Amount = myReader["Tax2Amount"].ToString();
                    transaction.Tax3Amount = myReader["Tax3Amount"].ToString();
                    transaction.Tax1Code = myReader["Tax1Code"].ToString();
                    transaction.Tax2Code = myReader["Tax2Code"].ToString();
                    transaction.Tax3Code = myReader["Tax3Code"].ToString();
                    transaction.Tax1Currency = myReader["Tax1Currency"].ToString();
                    transaction.Tax2Currency = myReader["Tax2Currency"].ToString();
                    transaction.Tax3Currency = myReader["Tax3Currency"].ToString();
                    transaction.TotalAmount = myReader["TotalAmount"].ToString();
                    transaction.TotalCurrency = myReader["TotalCurrency"].ToString();
                    transaction.FareCalculationModeIndicator = myReader["FareCalculationModeIndicator"].ToString();
                    transaction.EndosRestriction = myReader["EndosRestriction"].ToString();
                    transaction.Discount = myReader["Discount"].ToString();
                    transaction.VendorISOCountryCode = myReader["VendorISOCountryCode"].ToString();
                    transaction.CommercialAgreementReference = myReader["CommercialAgreementReference"].ToString();
                    transaction.TransactionCode = myReader["TransactionCode"].ToString();
                    transaction.AmountCollected = myReader["AmountCollected"].ToString();
                    transaction.OwnIsoCountry = myReader["OwnIsoCountry"].ToString();
                    transaction.OwnAirline = myReader["OwnAirline"].ToString();
                    transaction.CommissionCollected = myReader["CommissionCollected"].ToString();
                    transaction.CommissionCollectedCurrency = myReader["CommissionCollectedCurrency"].ToString();
                    model.Add(transaction);
                };
            }
            else
            {
                var transaction = new TransactionHeaderModel();
                transaction.firstDocNumber = fistDocNumber.ToString();
                transaction.secondDocNumber = secondDocNumber.ToString();
                transaction.CheckDigit = "";
                transaction.TrueOriginDestinationCityCodes = "";
                transaction.FareCalculationArea = "";
                transaction.FareCurrency = "";
                transaction.Fare = "";
                transaction.EquivalentFare = "";
                transaction.BookingReference = "";
                transaction.PassengerName = "";
                transaction.DateofIssue = "";
                transaction.AgentNumericCode = "";
                transaction.BookingAgentIdentification = "";
                transaction.ReportingSystemIdentifier = "";
                transaction.VendorIdentification = "";
                transaction.LegalName = "";
                transaction.LocationAddress = "";
                transaction.LocationCity = "";
                transaction.LocationCountry = "";
                transaction.Tax1Amount = "";
                transaction.Tax2Amount = "";
                transaction.Tax3Amount = "";
                transaction.Tax1Code = "";
                transaction.Tax2Code = "";
                transaction.Tax3Code = "";
                transaction.Tax1Currency = "";
                transaction.Tax2Currency = "";
                transaction.Tax3Currency = "";
                transaction.TotalAmount = "";
                transaction.TotalCurrency = "";
                transaction.FareCalculationModeIndicator = "";
                transaction.EndosRestriction = "";
                transaction.Discount = "";
                transaction.VendorISOCountryCode = "";
                transaction.CommercialAgreementReference = "";
                transaction.TransactionCode = "";
                transaction.AmountCollected = "";
                transaction.OwnIsoCountry = "";
                transaction.OwnAirline = "";
                transaction.CommissionCollected = "";
                transaction.CommissionCollectedCurrency = "";
                model.Add(transaction);
            }
            return model;
        }
        private List<TransactionMiddleModel> GetTransactionMiddle(string StrSQL)
        {
            dbconnect.pbConnectionString = pbConnectionString;
            SqlDataReader myReaderCount = dbconnect.GetData(StrSQL);

            List<TransactionMiddleModel> model = new List<TransactionMiddleModel>();
            while (myReaderCount.Read())
            {
                var details = new TransactionMiddleModel();
                details.CouponNumber = myReaderCount["CouponNumber"].ToString();
                details.StopOverCode = myReaderCount["StopOverCode"].ToString();
                details.Carrier = myReaderCount["Carrier"].ToString();
                details.FlightNumber = myReaderCount["FlightNumber"].ToString();
                details.GFPA = myReaderCount["GFPA"].ToString();

                details.FlightDepartureDate = myReaderCount["FlightDepartureDate"].ToString();
                details.FlightDepartureTime = myReaderCount["FlightDepartureTime"].ToString();
                details.CouponStatus = myReaderCount["CouponStatus"].ToString();
                details.FareBasisTicketDesignator = myReaderCount["FareBasisTicketDesignator"].ToString();
                details.NotValidBefore = myReaderCount["NotValidBefore"].ToString();
                details.NotValidAfter = myReaderCount["NotValidAfter"].ToString();
                details.FreeBaggageAllowance = myReaderCount["FreeBaggageAllowance"].ToString();
                details.FlightBookingStatus = myReaderCount["FlightBookingStatus"].ToString();
                details.UsedClassofService = myReaderCount["UsedClassofService"].ToString();
                details.UsageSector = myReaderCount["UsageSector"].ToString();
                details.UsageAirline = myReaderCount["UsageAirline"].ToString();
                details.UsageFlightNumber = myReaderCount["UsageFlightNumber"].ToString();
                details.RelatedDocumentNumber = myReaderCount["RelatedDocumentNumber"].ToString();
                details.isConjonction = Convert.ToInt32(myReaderCount["isConjonction"]);
                details.UsageDate = myReaderCount["UsageDate"].ToString();
                details.FrequentFlyerReference = myReaderCount["FrequentFlyerReference"].ToString();
                model.Add(details);
            }
            return model;
        }
        private List<ConjonctionTicket> GetConjoctionTicket(string StrSQL)
        {
            dbconnect.pbConnectionString = pbConnectionString;
            SqlDataReader myReaderCount = dbconnect.GetData(StrSQL);

            List<ConjonctionTicket> model = new List<ConjonctionTicket>();
            while (myReaderCount.Read())
            {
                var details = new ConjonctionTicket();
                details.conjoction = myReaderCount["DocumentNumber"].ToString();
                details.relatedConjoction = myReaderCount["RelatedDocumentNumber"].ToString();
                model.Add(details);
            }
            return model;
        }
        private string[] ticketHistory(string docnum)
        {
            string sql = "";
            string[] response = new string[5];
            sql = "select iif(f1.EquivalentFare > 0, f1.TotalCurrency, f1.FareCurrency) as Currency,iif(f1.EquivalentFare > 0, f1.EquivalentFare, f1.Fare) as Fare,f1.CommissionCollected, sum(f2.otherAmount) as Sumotheramount,";
            sql += "(select OtherAmount from pax.SalesDocumentOtherAmount where OtherAmountCode = 'YR' and DocumentNumber = '" + docnum + "' )  as otheramount ";
            sql += "from pax.salesdocumentheader f1 ";
            sql += "join pax.SalesRelatedDocumentInformation f3 on f1.HdrGuid = f3.HdrGuid ";
            sql += "join pax.SalesDocumentOtherAmount f2 on f3.RelatedDocumentGuid = f2.RelatedDocumentGuid ";
            sql += "where f2.OtherAmountCode <>'YR' ";
            sql += "and f2.TransactionCode <> 'RFND' ";
            sql += "and f1.DocumentNumber = '" + docnum + "' ";
            sql += "group by f1.TotalCurrency,f1.EquivalentFare,f1.FareCurrency, f1.Fare, f1.CommissionCollected ";
            dbconnect.pbConnectionString = pbConnectionString;
            SqlDataReader myReader = dbconnect.GetData(sql);
            while (myReader.Read())
            {
                response[0] = myReader["Currency"].ToString();
                response[1] = myReader["Fare"].ToString();
                response[2] = myReader["CommissionCollected"].ToString();
                response[3] = myReader["Sumotheramount"].ToString();
                response[4] = myReader["otheramount"].ToString();
            }
            return response;
        }
        public ActionResult City()
        {
            string tabgfpa = Request["tgfpa"];
            var tableGfpa = tabgfpa.TrimStart('[').TrimEnd(']').Split(',');
            string sqlrec = "SELECT CityName,AirportCode from ref.City";
            DataSet ds = new DataSet();
            SqlConnection con = new SqlConnection(pbConnectionString);
            SqlDataAdapter ada = new SqlDataAdapter(sqlrec, con);
            ada.Fill(ds);
            con.Close();
            int lenghtcity = ds.Tables[0].Rows.Count;
            string[] tabCity = new string[lenghtcity];
            string[] tabCityCode = new string[lenghtcity];
            string tcity = "";
            int i = 0;
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                tabCityCode[i] = dr[ds.Tables[0].Columns[1]].ToString();
                tabCity[i] = dr[ds.Tables[0].Columns[0]].ToString();
                i++;
            }

            foreach (var item in tableGfpa)
            {

                if (tabCityCode.Contains(item.Replace(" ", "")))

                {
                    int index = Array.IndexOf(tabCityCode, item.Replace(" ", ""));

                    tcity += tabCity[index].ToString() + " " + item + "/";
                    //break;
                }

            }

            return Json(new { teste = tcity });
        }
        public ActionResult TFC()
        {
            string dateFrom = DateTime.Now.AddMonths(-1).ToString("dd-MMM-yyyy");
            string dateTo = DateTime.Now.ToString("dd-MMM-yyyy");
            string[] date = new string[2] { dateFrom, dateTo };
            ViewBag.date = date;

            /*********/
            string sql = "select distinct concat([From Sector],' - ',[To Sector]) as sector from [TFC].[TFCBySector]  f1 order by 1 ";
            SqlConnection con = new SqlConnection(pbConnectionString);
            DataSet ds = new DataSet();
            SqlDataAdapter ada = new SqlDataAdapter(sql, con);
            ada.Fill(ds);
            con.Close();

            int lonItemsec = ds.Tables[0].Rows.Count;

            string[,] ListeItemsec = new string[1, lonItemsec];

            int i = 0;

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                ListeItemsec[0, i] = dr[ds.Tables[0].Columns[0].ColumnName].ToString();

                i++;
            }

            ViewBag.lonItemsec = lonItemsec;
            ViewBag.ListeItemsec = ListeItemsec;

            /*********/

            ViewBag.read = "readonly";
            return PartialView();
        }
        public ActionResult TFC2()
        {
            /*********/
            string FlightTrans = Request["FlightTrans"].ToUpper();

            string sql = "select isnull( Departure,Arrival ) from [TFC].[TFCBySector]  f1 where [From Sector] =left('" + FlightTrans.ToUpper() + "',3) and [To Sector] =right('" + FlightTrans.ToUpper() + "',3)  ";
            SqlConnection con = new SqlConnection(pbConnectionString);
            DataSet ds = new DataSet();
            SqlDataAdapter ada = new SqlDataAdapter(sql, con);
            ada.Fill(ds);
            con.Close();

            int lonItemtax = ds.Tables[0].Rows.Count;

            string[,] ListeItemtax = new string[1, lonItemtax];

            int i = 0;

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                ListeItemtax[0, i] = dr[ds.Tables[0].Columns[0].ColumnName].ToString();

                i++;
            }

            ViewBag.lonItemtax = lonItemtax;
            ViewBag.ListeItemtax = ListeItemtax;
            /*********/

            return PartialView();
        }
        public ActionResult recherche()
        {
            string FlightTrans = Request["FlightTrans"].ToUpper();

            string FlightTrans1 = Request["FlightTrans1"].ToUpper();

            string sql1 = " select [From Sector] ,[To Sector],isnull( Departure,Arrival )as TaxCode,[TaxName],[INTERNATIONAL /DOM] " + Environment.NewLine;
            sql1 = sql1 + ",[RATD_TTBS],[Interlineability ] " + Environment.NewLine;
            sql1 = sql1 + ",[Pax (INT)],[Pax (DOM)],[Airline (INT)] " + Environment.NewLine;
            sql1 = sql1 + ",[Airline (DOM)],[Collection at Point Of Sales],[Basic Remitance] " + Environment.NewLine;
            sql1 = sql1 + ",city.Country ,ctry.Name ,[PTA MCO EMD]  ,[Tax Definition],case when Departure is null then 'ARRIVAL' ELSE 'DEPARTURE' END  as Departure_Arrival  from" + Environment.NewLine;
            sql1 = sql1 + "[TFC].[TFCBySector] sec " + Environment.NewLine;
            sql1 = sql1 + " left join ref.city city on sec.[From Sector] = city.AirportCode left join ref.Country ctry on city.Country = ctry.Code where [From Sector] = left('" + FlightTrans.ToUpper() + "',3)  and [To Sector] =right('" + FlightTrans.ToUpper() + "',3) " + Environment.NewLine;
            sql1 = sql1 + " and isnull( Departure,Arrival ) = '" + FlightTrans1.ToUpper() + "'" + Environment.NewLine;

            SqlConnection cs1 = new SqlConnection(pbConnectionString);
            DataSet ds1 = new DataSet();
            SqlDataAdapter ada1 = new SqlDataAdapter(sql1, cs1);
            ada1.Fill(ds1);

            foreach (DataRow dr1 in ds1.Tables[0].Rows)
            {
                #region
                ViewBag.sectorfrom = dr1[ds1.Tables[0].Columns[0].ColumnName].ToString();
                ViewBag.sectorto = dr1[ds1.Tables[0].Columns[1].ColumnName].ToString();
                ViewBag.TaxName = dr1[ds1.Tables[0].Columns[3].ColumnName].ToString();
                ViewBag.TaxCode = FlightTrans1;
                ViewBag.Country = dr1[ds1.Tables[0].Columns[13].ColumnName].ToString();
                ViewBag.Definition = dr1[ds1.Tables[0].Columns[16].ColumnName].ToString();
                ViewBag.arridep = dr1[ds1.Tables[0].Columns[17].ColumnName].ToString();
                ViewBag.FromSector = dr1[ds1.Tables[0].Columns[0].ColumnName].ToString();
                ViewBag.toSector = dr1[ds1.Tables[0].Columns[1].ColumnName].ToString();
                ViewBag.aptaxcode = FlightTrans1;
                ViewBag.apname = dr1[ds1.Tables[0].Columns[3].ColumnName].ToString();

                string paxdom = dr1[ds1.Tables[0].Columns[8].ColumnName].ToString();
                if (paxdom == "Y")
                { ViewBag.chkPaxDom = "checked"; }
                else
                { ViewBag.chkPaxDom = ""; }

                string paxint = dr1[ds1.Tables[0].Columns[7].ColumnName].ToString();
                if (paxint == "Y")
                { ViewBag.chkPaxInt = "checked"; }
                else
                { ViewBag.chkPaxInt = ""; }

                string airdom = dr1[ds1.Tables[0].Columns[10].ColumnName].ToString();
                if (airdom == "Y")
                { ViewBag.chkAirDom = "checked"; }
                else
                { ViewBag.chkAirDom = ""; }

                string airint = dr1[ds1.Tables[0].Columns[9].ColumnName].ToString();
                if (airint == "Y")
                { ViewBag.chkAirInt = "checked"; }
                else
                { ViewBag.chkAirInt = ""; }

                string Selling = dr1[ds1.Tables[0].Columns[12].ColumnName].ToString();
                if (Selling == "S")
                { ViewBag.rbselling = "checked"; }
                else
                { ViewBag.rbselling = ""; }

                string Transport = dr1[ds1.Tables[0].Columns[12].ColumnName].ToString();
                if (Transport == "T")
                { ViewBag.rbtransport = "checked"; }
                else
                { ViewBag.rbtransport = ""; }

                string Other = dr1[ds1.Tables[0].Columns[12].ColumnName].ToString();
                if (Other == "O")
                { ViewBag.rbOther = "checked"; }
                else
                { ViewBag.rbOther = ""; }

                string POS = dr1[ds1.Tables[0].Columns[11].ColumnName].ToString();
                if (POS == "Y")
                { ViewBag.chkpos = "checked"; }
                else
                { ViewBag.chkpos = ""; }

                string Interliability = dr1[ds1.Tables[0].Columns[6].ColumnName].ToString();
                if (Interliability == "Y")
                { ViewBag.rbIntYes = "checked"; }
                else
                { ViewBag.rbIntNo = "checked"; }
                string pta = dr1[ds1.Tables[0].Columns[15].ColumnName].ToString();
                if (pta == "Y")
                { ViewBag.chkPTA = "checked"; }
                else
                { ViewBag.chkPTA = "checked"; }
                #endregion
            }

            cs1.Close();

            string sql11 = "select distinct Effective,Expiry,Percentage,TaxAmount,RATDCurrency,RATDDetails,Sale,Travel,DomesticInternational from TFC.ApplicableTFC where  [FromSector] =  left('" + FlightTrans.ToUpper() + "',3)  and [ToSector]=right('" + FlightTrans.ToUpper() + "',3)  and TaxCode ='" + FlightTrans1.ToUpper() + "'  ";
            SqlConnection cs11 = new SqlConnection(pbConnectionString);
            DataSet ds11 = new DataSet();
            SqlDataAdapter ada11 = new SqlDataAdapter(sql11, cs11);
            ada11.Fill(ds11);
            int ligne0 = ds11.Tables[0].Rows.Count;
            int colone0 = ds11.Tables[0].Columns.Count;
            string[,] Liste0 = new string[ligne0, colone0];
            int i0 = 0;
            foreach (DataRow dr11 in ds11.Tables[0].Rows)
            {
                for (int j = 0; j < ds11.Tables[0].Columns.Count; j++)
                {
                    if ((j == 0) || (j == 1) || (j == 6) || (j == 7))
                    {
                        string h = null;
                        string hh = null;
                        h = dr11[ds11.Tables[0].Columns[j].ColumnName].ToString();
                        if (h != "")
                        {
                            hh = h.Substring(0, 11).ToString();
                            Liste0[i0, j] = hh;
                        }
                    }
                    else
                      {
                        Liste0[i0, j] = dr11[ds11.Tables[0].Columns[j].ColumnName].ToString();
                      }
                }
                i0 = i0 + 1;
            }
            ViewBag.ligne0 = ligne0;
            ViewBag.colone0 = colone0;

            ViewBag.Liste0 = Liste0;

            cs11.Close();

            string sql12 = "Select distinct ExemptionsCode,[ExemptionEffective],ExemptionsDetail,DomesticInternational from TFC.ApplicableTFC where  [FromSector] =  left('" + FlightTrans.ToUpper() + "',3)  and [ToSector]=right('" + FlightTrans.ToUpper() + "',3)  and TaxCode ='" + FlightTrans1.ToUpper() + "'  ";
            SqlConnection c12 = new SqlConnection(pbConnectionString);
            DataSet ds12 = new DataSet();
            SqlDataAdapter ada12 = new SqlDataAdapter(sql12, c12);
            ada12.Fill(ds12);
            int ligne = ds12.Tables[0].Rows.Count;
            int colone = ds12.Tables[0].Columns.Count;
            string[,] Liste = new string[ligne, colone];
            int i = 0;

            foreach (DataRow dr12 in ds12.Tables[0].Rows)
            {
                for (int j = 0; j < ds12.Tables[0].Columns.Count; j++)
                {
                    if (j == 1)
                    {
                        string h = null;
                        string hh = null;
                        h = dr12[ds12.Tables[0].Columns[j].ColumnName].ToString();
                        if (h != "")
                        {
                            hh = h.Substring(0, 11).ToString();
                            Liste[i, j] = hh;
                        }
                    }
                    else
                    {
                        Liste[i, j] = dr12[ds12.Tables[0].Columns[j].ColumnName].ToString();
                    }
                }
                i = i + 1;
            }

            ViewBag.ligne = ligne;
            ViewBag.colone = colone;

            ViewBag.Liste = Liste;

            c12.Close();





            string sql13 = "SELECT *  FROM [Ref].[Tax] where  [FromAirport] = left('" + FlightTrans.ToUpper() + "',3)  and [ToAirport]=right('" + FlightTrans.ToUpper() + "',3)  and TaxCode ='" + FlightTrans1.ToUpper() + "'  ";
            SqlConnection c13 = new SqlConnection(pbConnectionString);
            DataSet ds13 = new DataSet();
            SqlDataAdapter ada13 = new SqlDataAdapter(sql13, c13);
            ada13.Fill(ds13);


            int ligne13 = ds13.Tables[0].Rows.Count;
            int colone13 = ds13.Tables[0].Columns.Count;

            string[,] Liste13 = new string[ligne13, colone13];

            int i13 = 0;

            foreach (DataRow dr13 in ds13.Tables[0].Rows)
            {
                for (int j13 = 0; j13 < ds13.Tables[0].Columns.Count; j13++)
                {
                    if ((j13 == 6) || (j13 == 7))
                    {
                        string h = null;
                        string hh = null;
                        h = dr13[ds13.Tables[0].Columns[j13].ColumnName].ToString();

                        if (h != "")
                        {
                            hh = h.Substring(0, 11).ToString();
                            Liste13[i13, j13] = hh;
                        }
                    }
                    else
                    {
                        Liste13[i13, j13] = dr13[ds13.Tables[0].Columns[j13].ColumnName].ToString();
                    }
                }
                i13 = i13 + 1;
            }

            ViewBag.ligne13 = ligne13;
            ViewBag.colone13 = colone13;

            ViewBag.Liste13 = Liste13;

            c13.Close();

            ViewBag.read = "readonly";
            
            return PartialView(ds11);
        }
        public ActionResult viewmissingsector()
        {
            string sql = " select distinct     " + Environment.NewLine;
            sql = sql + "(f3.OriginAirportCityCode+f3.DestinationAirportCityCode) as CpnSector " + Environment.NewLine;
            sql = sql + ",[From Sector], [To Sector], F4.Arrival, F4.Departure  " + Environment.NewLine;
            sql = sql + "from Pax.salesdocumentheader f1 " + Environment.NewLine;
            sql = sql + "join pax.SalesRelatedDocumentInformation f2 on f1.HdrGuid = f2.HdrGuid" + Environment.NewLine; ;
            sql = sql + "join pax.salesdocumentcoupon f3 on f2.RelatedDocumentguid = f3.RelatedDocumentguid" + Environment.NewLine; ;
            sql = sql + "left join TFC.TFCBySector f4 on f3.OriginAirportCityCode = f4.[From Sector] and f3.DestinationAirportCityCode = f4.[To Sector] " + Environment.NewLine; ;
            sql = sql + "where [From Sector] is  null" + Environment.NewLine; ;

            SqlConnection c = new SqlConnection(pbConnectionString);
            DataSet ds = new DataSet();
            SqlDataAdapter ada = new SqlDataAdapter(sql, c);
            ada.Fill(ds);


            int ligne = ds.Tables[0].Rows.Count;
            int colone = ds.Tables[0].Columns.Count;

            string[,] Liste = new string[ligne, colone];

            int i = 0;

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                for (int j = 0; j < ds.Tables[0].Columns.Count; j++)
                {
                    Liste[i, j] = dr[ds.Tables[0].Columns[j].ColumnName].ToString();

                }
                i = i + 1;
            }

            ViewBag.ligne = ligne;
            ViewBag.colone = colone;

            ViewBag.Liste = Liste;

            c.Close();

            return PartialView();
        }
        public ActionResult sectornotaxes()
        {
            string sql = " select * from Ref.SectorsNoApplicableTax   " + Environment.NewLine;

            SqlConnection c = new SqlConnection(pbConnectionString);
            DataSet ds = new DataSet();
            SqlDataAdapter ada = new SqlDataAdapter(sql, c);
            ada.Fill(ds);


            int ligne = ds.Tables[0].Rows.Count;
            int colone = ds.Tables[0].Columns.Count;

            string[,] Liste = new string[ligne, colone];

            int i = 0;

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                for (int j = 0; j < ds.Tables[0].Columns.Count; j++)
                {
                    Liste[i, j] = dr[ds.Tables[0].Columns[j].ColumnName].ToString();

                }
                i = i + 1;
            }

            ViewBag.ligne = ligne;
            ViewBag.colone = colone;

            ViewBag.Liste = Liste;

            c.Close();

            return PartialView();
        }
        public ActionResult recherchetfc()
        {
            string ag = "%";
            string dateFrom1 = Request["dateFrom"];
            string dateTo1 = Request["dateTo"];
            DateTime dateFrom = DateTime.Parse(dateFrom1);
            DateTime dateTo = DateTime.Parse(dateTo1);
            string DocNo = Request["DocNo"];
            string AgentNumericCode = Request["AgentNumericCode"];

            SqlConnection con2 = new SqlConnection(pbConnectionString);
            DataSet ds = new DataSet();

            SqlCommand cmd = new SqlCommand("[Pax].[SP_TaxCompare]", con2);
            cmd.CommandTimeout = 1000;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@dateFrom", dateFrom);
            cmd.Parameters.AddWithValue("@dateTo", dateTo);
            cmd.Parameters.AddWithValue("@Remarks", ag);
            if (DocNo == "" || DocNo == null) { cmd.Parameters.AddWithValue("@DocumentNumber", "%"); }
            else { cmd.Parameters.AddWithValue("@DocumentNumber", DocNo); }
            if (AgentNumericCode == "" || AgentNumericCode == null) { cmd.Parameters.AddWithValue("@AgentNumericCode", "%"); }
            else { cmd.Parameters.AddWithValue("@AgentNumericCode", AgentNumericCode); }


            int i = 0;
            SqlDataAdapter ada = new SqlDataAdapter(cmd);
            ada.Fill(ds);

            int lon = ds.Tables[0].Rows.Count;

            string[,] resultat = new string[14, lon];

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                resultat[0, i] = dr[ds.Tables[0].Columns[0].ColumnName].ToString();
                resultat[1, i] = dr[ds.Tables[0].Columns[1].ColumnName].ToString();
                resultat[2, i] = dr[ds.Tables[0].Columns[2].ColumnName].ToString();
                resultat[3, i] = dr[ds.Tables[0].Columns[3].ColumnName].ToString();
                resultat[4, i] = dr[ds.Tables[0].Columns[4].ColumnName].ToString();
                resultat[5, i] = dr[ds.Tables[0].Columns[5].ColumnName].ToString();
                resultat[6, i] = dr[ds.Tables[0].Columns[6].ColumnName].ToString();
                resultat[7, i] = dr[ds.Tables[0].Columns[7].ColumnName].ToString();
                resultat[8, i] = dr[ds.Tables[0].Columns[8].ColumnName].ToString();
                resultat[9, i] = dr[ds.Tables[0].Columns[9].ColumnName].ToString();
                resultat[10, i] = dr[ds.Tables[0].Columns[10].ColumnName].ToString();
                resultat[11, i] = dr[ds.Tables[0].Columns[11].ColumnName].ToString();
                resultat[12, i] = dr[ds.Tables[0].Columns[12].ColumnName].ToString();
                resultat[13, i] = dr[ds.Tables[0].Columns[13].ColumnName].ToString();

                i++;
            }

            ViewBag.resultat = resultat;

            ViewBag.nombre = lon;


            return PartialView();
        }
        public ActionResult addtfc()
        {
            ViewBag.etat = "disabled";
            return PartialView();
        }
        public void delete()
        {
            string sectorfrom = Request["sectorfrom"].ToUpper();
            string sectorto = Request["sectorto"].ToUpper();
            string taxcode = Request["taxcode"].ToUpper();

            SqlDataAdapter da = new SqlDataAdapter();
            string sql = "";
            sql = sql + "Delete From TFC.TFCBySector where [From Sector] = '" + sectorfrom + "' and [To Sector] ='" + sectorto + "' " + Environment.NewLine;
            sql = sql + " and isnull( Departure,Arrival ) = '" + taxcode + "'" + Environment.NewLine;

            DataSet ds = new DataSet();
            ds = dbconnect.RetObjDS(sql);
        }
        public ActionResult saveoperation()
        {          
            string sectorfrom = Request["sectorfrom"].ToUpper();
            string sectorto = Request["sectorto"].ToUpper();
            string taxcode = Request["taxcode"].ToUpper();
            string cbotaxcode = taxcode;
            string cboSector = sectorfrom + " - " + sectorto;
            string taxname = Request["taxname"];
            string passengersdomestic = Request["passengersdomestic"];
            string passengersinternational = Request["passengersinternational"];
            string airlinesdomestic = Request["airlinesdomestic"];
            string airlinesinternational = Request["airlinesinternational"];
            string APPLICABLETO = Request["APPLICABLETO"];
            string INTERNATIONAL = Request["INTERNATIONAL"];
            string DOMESTIC = Request["DOMESTIC"];
            string salecollection = Request["salecollection"];
            string defaultGroupExample1 = Request["defaultGroupExample1"];
            string ptamco = Request["ptamco"];

            SqlDataAdapter da = new SqlDataAdapter();

            string paxdom = "";
            if (passengersdomestic == "true")
            {
                paxdom = "Y";

            }
            else
            {
                paxdom = "N";

            }

            string paxint = "";
            if (passengersinternational == "true")
            {
                paxint = "Y";

            }
            else
            {
                paxint = "N"; ;

            }

            string airdom = "";
            if (airlinesdomestic == "true")
            {
                airdom = "Y";

            }
            else
            {
                airdom = "N";

            }
            string airint = "";
            if (airlinesinternational == "true")
            {
                airint = "Y";

            }
            else
            {
                airint = "N";

            }

            string remitance = "";
            if (APPLICABLETO == "true" && INTERNATIONAL == "false" && DOMESTIC == "false")
            {
                remitance = "S";
            }
            else

                if (APPLICABLETO == "false" && INTERNATIONAL == "true" && DOMESTIC == "false")
            {
                remitance = "T";

            }
            else

                    if (APPLICABLETO == "false" && INTERNATIONAL == "false" && DOMESTIC == "true")
            {
                remitance = "O";

            }

            string POS = "";
            if (salecollection == "true")
            {
                POS = "Y";

            }
            else
            {
                POS = "N";

            }

            string Interliability = "";
            if (defaultGroupExample1 == "true")
            {
                Interliability = "Y";

            }
            else
            {
                Interliability = "N";

            }
            string pta = "";
            if (ptamco == "true")
            {
                pta = "Y";
            }
            else
            {
                ptamco = "true";
                pta = "N";

            }

            string sql = "";
            sql = sql + "INSERT INTO [TFC].[TFCBySector] ([From Sector] ,[To Sector] ,[Departure] ,[Arrival] ";
            sql = sql + ",[TaxName],[Interlineability ]  ";
            sql = sql + ",[Pax (INT)],[Pax (DOM)],[Airline (INT)],[Airline (DOM)],[Collection at Point Of Sales]  ";
            sql = sql + ",[Basic Remitance],[PTA MCO EMD],[Tax Definition]) values ('" + sectorfrom.ToUpper() + "','" + sectorto.ToUpper() + "','" + taxcode.ToUpper() + "','' ,";
            sql = sql + "'" + taxname + "', '" + Interliability + "',";
            sql = sql + " '" + paxint + "','" + paxdom + "','" + airint + "','" + airdom + "','" + POS + "', ";
            sql = sql + "'" + remitance + "', '" + pta + "', '" + Interliability + "' ) ";

            DataSet ds = new DataSet();
            ds = dbconnect.RetObjDS(sql);


            /**********FlightTrans*************/


            string dateFrom = DateTime.Now.AddMonths(-1).ToString("dd-MMM-yyyy");
            string dateTo = DateTime.Now.ToString("dd-MMM-yyyy");
            string[] date = new string[2] { dateFrom, dateTo };
            ViewBag.date = date;

            /*********/
            string sqla = "select distinct concat([From Sector],' - ',[To Sector]) as sector from [TFC].[TFCBySector]  f1 order by 1 ";
            SqlConnection cona = new SqlConnection(pbConnectionString);
            DataSet dsa = new DataSet();
            SqlDataAdapter adaa = new SqlDataAdapter(sqla, cona);
            adaa.Fill(dsa);
            cona.Close();

            int lonItemsec = dsa.Tables[0].Rows.Count;

            string[,] ListeItemsec = new string[1, lonItemsec];

            int i = 0;

            foreach (DataRow dra in dsa.Tables[0].Rows)
            {
                ListeItemsec[0, i] = dra[dsa.Tables[0].Columns[0].ColumnName].ToString();

                i++;
            }

            ViewBag.lonItemsec = lonItemsec;
            ViewBag.ListeItemsec = ListeItemsec;

            /**************************/

            ViewBag.cbotaxcode = cbotaxcode;
            ViewBag.cboSector = cboSector;

            
            
            return PartialView();
        }
        public ActionResult edit()
        {
            string FlightTrans = Request["FlightTrans"].ToUpper();
            string FlightTrans1 = Request["FlightTrans1"].ToUpper();

            string sql1 = " select [From Sector] ,[To Sector],isnull( Departure,Arrival )as TaxCode,[TaxName],[INTERNATIONAL /DOM] " + Environment.NewLine;
            sql1 = sql1 + ",[RATD_TTBS],[Interlineability ] " + Environment.NewLine;
            sql1 = sql1 + ",[Pax (INT)],[Pax (DOM)],[Airline (INT)] " + Environment.NewLine;
            sql1 = sql1 + ",[Airline (DOM)],[Collection at Point Of Sales],[Basic Remitance] " + Environment.NewLine;
            sql1 = sql1 + ",city.Country ,ctry.Name ,[PTA MCO EMD]  ,[Tax Definition],case when Departure is null then 'ARRIVAL' ELSE 'DEPARTURE' END  as Departure_Arrival  from" + Environment.NewLine;
            sql1 = sql1 + "[TFC].[TFCBySector] sec " + Environment.NewLine;
            sql1 = sql1 + " left join ref.city city on sec.[From Sector] = city.AirportCode left join ref.Country ctry on city.Country = ctry.Code where [From Sector] = left('" + FlightTrans.ToUpper() + "',3)  and [To Sector] =right('" + FlightTrans.ToUpper() + "',3) " + Environment.NewLine;
            sql1 = sql1 + " and isnull( Departure,Arrival ) = '" + FlightTrans1.ToUpper() + "'" + Environment.NewLine;

            SqlConnection cs1 = new SqlConnection(pbConnectionString);
            DataSet ds1 = new DataSet();
            SqlDataAdapter ada1 = new SqlDataAdapter(sql1, cs1);
            ada1.Fill(ds1);

            foreach (DataRow dr1 in ds1.Tables[0].Rows)
            {
                #region
                ViewBag.sectorfrom = dr1[ds1.Tables[0].Columns[0].ColumnName].ToString();
                ViewBag.sectorto = dr1[ds1.Tables[0].Columns[1].ColumnName].ToString();
                ViewBag.TaxName = dr1[ds1.Tables[0].Columns[3].ColumnName].ToString();
                ViewBag.TaxCode = FlightTrans1;
                ViewBag.Country = dr1[ds1.Tables[0].Columns[13].ColumnName].ToString();
                ViewBag.Definition = dr1[ds1.Tables[0].Columns[16].ColumnName].ToString();
                ViewBag.arridep = dr1[ds1.Tables[0].Columns[17].ColumnName].ToString();
                ViewBag.FromSector = dr1[ds1.Tables[0].Columns[0].ColumnName].ToString();
                ViewBag.toSector = dr1[ds1.Tables[0].Columns[1].ColumnName].ToString();
                ViewBag.aptaxcode = FlightTrans1;
                ViewBag.apname = dr1[ds1.Tables[0].Columns[3].ColumnName].ToString();

                string paxdom = dr1[ds1.Tables[0].Columns[8].ColumnName].ToString();
                if (paxdom == "Y")
                { ViewBag.chkPaxDom = "checked"; }
                else
                { ViewBag.chkPaxDom = ""; }

                string paxint = dr1[ds1.Tables[0].Columns[7].ColumnName].ToString();
                if (paxint == "Y")
                { ViewBag.chkPaxInt = "checked"; }
                else
                { ViewBag.chkPaxInt = ""; }

                string airdom = dr1[ds1.Tables[0].Columns[10].ColumnName].ToString();
                if (airdom == "Y")
                { ViewBag.chkAirDom = "checked"; }
                else
                { ViewBag.chkAirDom = ""; }

                string airint = dr1[ds1.Tables[0].Columns[9].ColumnName].ToString();
                if (airint == "Y")
                { ViewBag.chkAirInt = "checked"; }
                else
                { ViewBag.chkAirInt = ""; }

                string Selling = dr1[ds1.Tables[0].Columns[12].ColumnName].ToString();
                if (Selling == "S")
                { ViewBag.rbselling = "checked"; }
                else
                { ViewBag.rbselling = ""; }

                string Transport = dr1[ds1.Tables[0].Columns[12].ColumnName].ToString();
                if (Transport == "T")
                { ViewBag.rbtransport = "checked"; }
                else
                { ViewBag.rbtransport = ""; }

                string Other = dr1[ds1.Tables[0].Columns[12].ColumnName].ToString();
                if (Other == "O")
                { ViewBag.rbOther = "checked"; }
                else
                { ViewBag.rbOther = ""; }

                string POS = dr1[ds1.Tables[0].Columns[11].ColumnName].ToString();
                if (POS == "Y")
                { ViewBag.chkpos = "checked"; }
                else
                { ViewBag.chkpos = ""; }

                string Interliability = dr1[ds1.Tables[0].Columns[6].ColumnName].ToString();
                if (Interliability == "Y")
                { ViewBag.rbIntYes = "checked"; }
                else
                { ViewBag.rbIntNo = "checked"; }
                string pta = dr1[ds1.Tables[0].Columns[15].ColumnName].ToString();
                if (pta == "Y")
                { ViewBag.chkPTA = "checked"; }
                else
                { ViewBag.chkPTA = "checked"; }
                #endregion
            }

            cs1.Close();

            string sql11 = "select distinct Effective,Expiry,Percentage,TaxAmount,RATDCurrency,RATDDetails,Sale,Travel,DomesticInternational from TFC.ApplicableTFC where  [FromSector] =  left('" + FlightTrans.ToUpper() + "',3)  and [ToSector]=right('" + FlightTrans.ToUpper() + "',3)  and TaxCode ='" + FlightTrans1.ToUpper() + "'  ";

            SqlConnection cs11 = new SqlConnection(pbConnectionString);

            DataSet ds11 = new DataSet();
            SqlDataAdapter ada11 = new SqlDataAdapter(sql11, cs11);
            ada11.Fill(ds11);

            foreach (DataRow dr11 in ds11.Tables[0].Rows)
            {
                for (int j = 0; j < ds11.Tables[0].Columns.Count; j++)
                {
                    if ((j == 0) || (j == 1) || (j == 6) || (j == 7))
                    {
                        string h = null;
                        string hh = null;

                        h = dr11[ds11.Tables[0].Columns[j].ColumnName].ToString();
                        if (h != "")
                        {
                            hh = h.Substring(0, 11).ToString();
                            ds11.Tables[0].Columns[j].DefaultValue = hh;
                        }
                    }
                }
            }
            cs11.Close();

            string sql12 = "Select distinct ExemptionsCode,[ExemptionEffective],ExemptionsDetail,DomesticInternational from TFC.ApplicableTFC where  [FromSector] =  left('" + FlightTrans.ToUpper() + "',3)  and [ToSector]=right('" + FlightTrans.ToUpper() + "',3)  and TaxCode ='" + FlightTrans1.ToUpper() + "'  ";
            SqlConnection c12 = new SqlConnection(pbConnectionString);
            DataSet ds12 = new DataSet();
            SqlDataAdapter ada12 = new SqlDataAdapter(sql12, c12);
            ada12.Fill(ds12);

            int ligne = ds12.Tables[0].Rows.Count;
            int colone = ds12.Tables[0].Columns.Count;

            string[,] Liste = new string[ligne, colone];

            int i = 0;

            foreach (DataRow dr12 in ds12.Tables[0].Rows)
            {
                for (int j = 0; j < ds12.Tables[0].Columns.Count; j++)
                {
                    if (j == 1)
                    {
                        string h = null;
                        string hh = null;

                        h = dr12[ds12.Tables[0].Columns[j].ColumnName].ToString();

                        if (h != "")
                        {
                            hh = h.Substring(0, 11).ToString();
                            Liste[i, j] = hh;
                        }
                    }
                    else
                    {
                        Liste[i, j] = dr12[ds12.Tables[0].Columns[j].ColumnName].ToString();
                    }
                }
                i = i + 1;
            }

            ViewBag.ligne = ligne;
            ViewBag.colone = colone;

            ViewBag.Liste = Liste;

            c12.Close();
            
            string sql13 = "SELECT *  FROM [Ref].[Tax] where  [FromAirport] = left('" + FlightTrans.ToUpper() + "',3)  and [ToAirport]=right('" + FlightTrans.ToUpper() + "',3)  and TaxCode ='" + FlightTrans1.ToUpper() + "'  ";
            SqlConnection c13 = new SqlConnection(pbConnectionString);
            DataSet ds13 = new DataSet();
            SqlDataAdapter ada13 = new SqlDataAdapter(sql13, c13);
            ada13.Fill(ds13);


            int ligne13 = ds13.Tables[0].Rows.Count;
            int colone13 = ds13.Tables[0].Columns.Count;

            string[,] Liste13 = new string[ligne13, colone13];

            int i13 = 0;

            foreach (DataRow dr13 in ds13.Tables[0].Rows)
            {
                for (int j13 = 0; j13 < ds13.Tables[0].Columns.Count; j13++)
                {
                    if ((j13 == 6) || (j13 == 7))
                    {
                        string h = null;
                        string hh = null;
                        h = dr13[ds13.Tables[0].Columns[j13].ColumnName].ToString();

                        if (h != "")
                        {
                            hh = h.Substring(0, 11).ToString();
                            Liste13[i13, j13] = hh;
                        }
                    }
                    else
                    {
                        Liste13[i13, j13] = dr13[ds13.Tables[0].Columns[j13].ColumnName].ToString();
                    }
                }
                i13 = i13 + 1;
            }

            ViewBag.ligne13 = ligne13;
            ViewBag.colone13 = colone13;

            ViewBag.Liste13 = Liste13;

            c13.Close();
            
            ViewBag.FlightTrans = FlightTrans;
            ViewBag.FlightTrans1 = FlightTrans1;
            ViewBag.etat = "disabled";
            ViewBag.read = "";

            return PartialView(ds11);
        }
        public ActionResult clear1()
        {
            string dateFrom = DateTime.Now.AddMonths(-1).ToString("dd-MMM-yyyy");
            string dateTo = DateTime.Now.ToString("dd-MMM-yyyy");
            string[] date = new string[2] { dateFrom, dateTo };
            ViewBag.date = date;

            /*********/
            string sql = "select distinct concat([From Sector],' - ',[To Sector]) as sector from [TFC].[TFCBySector]  f1 order by 1 ";
            SqlConnection con = new SqlConnection(pbConnectionString);
            DataSet ds = new DataSet();
            SqlDataAdapter ada = new SqlDataAdapter(sql, con);
            ada.Fill(ds);
            con.Close();

            int lonItemsec = ds.Tables[0].Rows.Count;

            string[,] ListeItemsec = new string[1, lonItemsec];

            int i = 0;

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                ListeItemsec[0, i] = dr[ds.Tables[0].Columns[0].ColumnName].ToString();

                i++;
            }

            ViewBag.lonItemsec = lonItemsec;
            ViewBag.ListeItemsec = ListeItemsec;

            ViewBag.read = "readonly";

            return PartialView();
        }
        public void deletedonne()
        {
            string sectorfrom = Request["sectorfrom"].ToUpper();
            string sectorto = Request["sectorto"].ToUpper();
            string taxcode = Request["taxcode"].ToUpper();

            SqlDataAdapter da = new SqlDataAdapter();
            string sql = "";
            sql = sql + "Delete From TFC.TFCBySector where [From Sector] = '" + sectorfrom.ToUpper() + "' and [To Sector] ='" + sectorto.ToUpper() + "' " + Environment.NewLine;
            sql = sql + " and isnull( Departure,Arrival ) = '" + taxcode.ToUpper() + "'" + Environment.NewLine;

            DataSet ds = new DataSet();
            ds = dbconnect.RetObjDS(sql);
        }
        public ActionResult searchrefundaudit()
        {
            string dateFrom1 = Request["dateFrom"];
            string dateTo1 = Request["dateTo"];
            DateTime dateFrom = DateTime.Parse(dateFrom1);
            DateTime dateTo = DateTime.Parse(dateTo1);
    
            SqlConnection con = new SqlConnection(pbConnectionString);

            string sql = "";
            sql = sql + "SELECT [DocumentNumber],[AgentNumericCode],[DateofIssue],[PassengerName],[DocumentType],[TicketingAgentID],[AmountCollected],[AmountCollectedCurrency],hdrguid" + Environment.NewLine;
            sql = sql + "FROM [Pax].[SalesDocumentHeader]" + Environment.NewLine;
            sql = sql + "WHERE TransactionCode = 'RFND' AND ([DateofIssue] between CONVERT(date,'" + dateFrom + "',105) AND CONVERT(date,'" + dateTo + "',105))" + Environment.NewLine;
            sql = sql + "ORDER BY 3";
            
            DataSet ds = new DataSet();
            SqlDataAdapter ada = new SqlDataAdapter(sql, con);

            ada.Fill(ds);

            int ligne = ds.Tables[0].Rows.Count;
            int colone = 8;

            string[,] Liste = new string[ligne, 8];

            int i = 0;

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                    string d1 = dr[ds.Tables[0].Columns[2].ColumnName].ToString();

                    Liste[i, 0] = dr[ds.Tables[0].Columns[0].ColumnName].ToString();
                    Liste[i, 1] = dr[ds.Tables[0].Columns[1].ColumnName].ToString();

                    if (d1 != "")
                    {
                        Liste[i, 2] = Convert.ToDateTime(d1).ToShortDateString();
                    }


                    Liste[i, 3] = dr[ds.Tables[0].Columns[3].ColumnName].ToString();
                    Liste[i, 4] = dr[ds.Tables[0].Columns[4].ColumnName].ToString();
                    Liste[i, 5] = dr[ds.Tables[0].Columns[5].ColumnName].ToString();
                    Liste[i, 6] = dr[ds.Tables[0].Columns[7].ColumnName].ToString();
                    Liste[i, 7] = dr[ds.Tables[0].Columns[6].ColumnName].ToString();
                    //esrfn = rd.GetValue(8).ToString();
                    i++;
            }

            ViewBag.totaldoc = Liste.Length;

            ViewBag.ligne = ligne;
            ViewBag.colone = colone;
            ViewBag.Liste = Liste;

            con.Close();

            return PartialView();
        }
        public void delReftax()
        {
            string reftaxid = Request["reftaxid"];

            SqlDataAdapter da = new SqlDataAdapter();

            string sql = "";
            sql = sql + "DELETE REF.TAX WHERE TAXREFID='" + reftaxid + "'" + Environment.NewLine;

            DataSet ds = new DataSet();
            ds = dbconnect.RetObjDS(sql);
            
        }
        public void SaveSecond()
        {
            string sectorfrom = Request["sectorfrom"]; 
            string sectorto = Request["sectorto"]; 
            string taxcode = Request["taxcode"]; 
            string effectiveDate = Request["effectivedate"]; 
            string expirydate = Request["expirydate"]; 
            string Percentage = Request["percentage"];
            int Percentagen = Request.Form["percentage"].AsInt();
            string RADTAmount = Request["amount"];
            int RADTAmountn = Request.Form["amount"].AsInt();
            string RADTCurrency = Request["currency"]; 
            string RATDDetails = Request["detail"]; 
            string FromSector = Request["newFromSector"]; 
            string toSector = Request["newtoSector"]; 
            string aptaxcode = Request["newaptaxcode"]; 
            string apname = Request["newapname"]; 
            string FlightTrans = Request["FlightTrans"]; 
            string FlightTrans1 = Request["FlightTrans1"]; 
            #region SaveMain
            #region Apptaxdelete
            SqlDataAdapter da = new SqlDataAdapter();
            string sql = "";
            sql = sql + "Delete From TFC.ApplicableTFC " + Environment.NewLine;
            sql = sql + "where [FromSector]  = '" + sectorfrom + "'  " + Environment.NewLine;
            sql = sql + "and  [ToSector]  = '" + sectorto + "'  " + Environment.NewLine;
            sql = sql + "and [TaxCode]  = '" + taxcode + "'  " + Environment.NewLine;
            if (effectiveDate != "")
            {
                DateTime effectiveDate1 = DateTime.Parse(effectiveDate);
                sql = sql + "and [Effective] = '" + effectiveDate1 + "' ";
            }
            else
            {
                sql = sql + "and [Effective] = '" + effectiveDate + "'" + Environment.NewLine;
            }
            if (expirydate != "")
            {
                DateTime expirydate1 = DateTime.Parse(expirydate);
                sql = sql + "and [Expiry] = '" + expirydate1 + "' ";
            }
            else
            {
                sql = sql + "and [Expiry] = '" + expirydate + "'" + Environment.NewLine;
            }
            if (Percentage != "")
            {
                sql = sql + "and [Percentage] = '" + Percentagen + "'" ;
            }
            else
            {
                sql = sql + "and [Percentage] = '" + Percentagen + "'" + Environment.NewLine;
            }
            if (RADTAmount != "")
            {
                sql = sql + "and [TaxAmount] = '" + RADTAmountn + "' ";
            }
            else
            {
                sql = sql + "and [TaxAmount] = '" + RADTAmountn + "'" + Environment.NewLine;
            }
            if (RADTCurrency != "")
            {
                sql = sql + "and [RATDCurrency] = '" + RADTCurrency + "' ";
            }
            else
            {
                sql = sql + "and [RATDCurrency] = '" + RADTCurrency + "'" + Environment.NewLine;
            }
            if (RATDDetails != "")
            {
                sql = sql + "and [RATDDetails] = '" + RATDDetails + "' ";
            }
            else
            {
                sql = sql + "and [RATDDetails] = '" + RATDDetails + "'" + Environment.NewLine;
            }

            sql = sql.Replace("'Null'", "Null");

            DataSet ds = new DataSet();
            ds = dbconnect.RetObjDS(sql);
            #endregion
            string Sql = "DECLARE @REFID bigint; " + Environment.NewLine;
            Sql = Sql + "SET @REFID = (Select iif(MAX([ID]) is null,1,MAX([ID])+1) as XRefId  FROM [TFC].[ApplicableTFC]);" + Environment.NewLine;
            Sql = Sql + "INSERT INTO [TFC].[ApplicableTFC] VALUES (" + Environment.NewLine;
            Sql = Sql + "@REFID ," + Environment.NewLine;
            Sql = Sql + "'" + FromSector + "'," + Environment.NewLine;
            Sql = Sql + "'" + toSector + "'," + Environment.NewLine;
            Sql = Sql + "'" + aptaxcode + "'," + Environment.NewLine;
            Sql = Sql + "'" + apname + "'," + Environment.NewLine;
            #endregion
            string sql1 = Sql;
            string EffDate = "";
            string Expdate = "";
            string Sale = "";
            string Travel = "";
            string ExempEffec = "";
            #region dgvtaxspec
            string sql11 = "select distinct Effective,Expiry,Percentage,TaxAmount,RATDCurrency,RATDDetails,Sale,Travel,DomesticInternational from TFC.ApplicableTFC where  [FromSector] =  left('" + FlightTrans.ToUpper() + "',3)  and [ToSector]=right('" + FlightTrans.ToUpper() + "',3)  and TaxCode ='" + FlightTrans1.ToUpper() + "'  ";
            SqlConnection cs11 = new SqlConnection(pbConnectionString);
            cs11.Open();
            DataSet ds11 = new DataSet();
            SqlDataAdapter ada11 = new SqlDataAdapter(sql11, cs11);
            ada11.Fill(ds11);
            cs11.Close();


            int ligne = ds11.Tables[0].Rows.Count;
            int colone = ds11.Tables[0].Columns.Count;
            string[,] Liste = new string[ligne, colone];
            int ii = 0;

            foreach (DataRow dr11 in ds11.Tables[0].Rows)
            {
                for (int j = 0; j < ds11.Tables[0].Columns.Count; j++)
                {
                    if ((j == 0) || (j == 1) || (j == 6) || (j == 7))
                    {
                        string h = null;
                        string hh = null;

                        h = dr11[ds11.Tables[0].Columns[j].ColumnName].ToString();
                        if (h != "")
                        {
                            hh = h.Substring(0, 11).ToString();
                            Liste[ii, j] = hh;
                        }
                    }
                    else
                    {
                        Liste[ii, j] = dr11[ds11.Tables[0].Columns[j].ColumnName].ToString();
                    }
                }
                ii = ii + 1;
            }
            cs11.Close();
            #endregion
            for (int i = 0; i < ligne - 1; i++)
            {
                string sql7 = "";
                sql7 = sql1;
                EffDate = Liste[i, 0];
                Expdate = Liste[i, 1];
                Sale = Liste[i, 6];
                Travel = Liste[i, 7];
                if (EffDate != "Null")
                {
                    sql7 = sql7 + "Convert(date, '" + EffDate + "', 105)," + Environment.NewLine;
                }
                else
                {
                    sql7 = sql7 + EffDate + "," + Environment.NewLine;
                }

                if (Expdate != "Null")
                {
                    sql7 = sql7 + "Convert(date, '" + Expdate + "', 105)," + Environment.NewLine;
                }
                else
                {
                    sql7 = sql7 + Expdate + "," + Environment.NewLine;
                }
                sql7 = sql7 + "'" + Liste[i, 2] + "'," + Environment.NewLine;
                sql7 = sql7 + "'" + Liste[i, 3] + "'," + Environment.NewLine;
                sql7 = sql7 + "'" + Liste[i, 4] + "'," + Environment.NewLine;

                if (Sale != "Null")
                {
                    sql7 = sql7 + "Convert(date, '" + Sale + "', 105)," + Environment.NewLine;
                }
                else
                {
                    sql7 = sql7 + Sale + "," + Environment.NewLine;
                }

                sql7 = sql7 + "'" + Liste[i, 5] + "'," + Environment.NewLine;

                if (Travel != "Null")
                {
                    sql7 = sql7 + "Convert(date, '" + Travel + "', 105)," + Environment.NewLine;
                }
                else
                {
                    sql7 = sql7 + Travel + "," + Environment.NewLine;
                }
                #region dgExemp
                string sql12 = "Select distinct ExemptionsCode,[ExemptionEffective],ExemptionsDetail,DomesticInternational from TFC.ApplicableTFC where  [FromSector] =  left('" + FlightTrans.ToUpper() + "',3)  and [ToSector]=right('" + FlightTrans.ToUpper() + "',3)  and TaxCode ='" + FlightTrans1.ToUpper() + "'  ";
                SqlConnection c12 = new SqlConnection(pbConnectionString);
                c12.Open();
                DataSet ds12 = new DataSet();
                SqlDataAdapter ada12 = new SqlDataAdapter(sql12, c12);
                ada12.Fill(ds12);
                c12.Close();

                int ligne12 = ds12.Tables[0].Rows.Count;
                int colone12 = ds12.Tables[0].Columns.Count;
                string[,] Liste12 = new string[ligne12, colone12];

                int i12 = 0;

                foreach (DataRow dr12 in ds12.Tables[0].Rows)
                {
                    for (int j = 0; j < ds12.Tables[0].Columns.Count; j++)
                    {
                        if (j == 1)
                        {
                            string h = null;
                            string hh = null;

                            h = dr12[ds12.Tables[0].Columns[j].ColumnName].ToString();

                            if (h != "")
                            {
                                hh = h.Substring(0, 11).ToString();
                                Liste12[i12, j] = hh;
                            }
                        }
                        else
                        {
                            Liste12[i12, j] = dr12[ds12.Tables[0].Columns[j].ColumnName].ToString();
                        }
                    }
                    i12 = i12 + 1;
                }
                c12.Close();
                #endregion
                int t = 0;
                if (ligne12 == 1)
                {
                    t = 1;
                }
                else
                {
                    t = ligne12 - 1;
                }
                for (int k = 0; k < t; k++)
                {
                    string sq = sql7;

                    ExempEffec = Liste12[k, 1];
                    sq = sq + "'" + Liste12[k, 0] + "'," + Environment.NewLine;

                    if (ExempEffec != "Null")
                    {
                        sq = sq + "Convert(date, '" + ExempEffec + "', 105)," + Environment.NewLine;
                    }
                    else
                    {
                        sq = sq + ExempEffec + "," + Environment.NewLine;
                    }
                    //sq = sq + "'" + CheckifNull(dgExemp, k, 1) + "'," + Environment.NewLine;
                    sq = sq + "'" + Liste12[k, 2] + "'," + Environment.NewLine;
                    sq = sq + "'" + Liste12[k, 3] + "')" + Environment.NewLine;
                    sq = sq.Replace("'Null'", "Null");
                    
                    SqlDataAdapter dap = new SqlDataAdapter();
                    DataSet dsp = new DataSet();
                    dsp = dbconnect.RetObjDS(sq);
                }
            }
        }
        public void updateAppTFC()
        {
            string sectorfrom = Request["sectorfrom"]; 
            string sectorto = Request["sectorto"]; 
            string taxcode = Request["taxcode"]; 
            string taxname = Request["taxname"]; 
            string effectivedate = Request["effectivedate"].Trim(); 
            string expirydate = Request["expirydate"].Trim(); 
            string percentage = Request["percentage0"]; 
            string currency = Request["currency"]; 
            string saledate = Request["saledate"].Trim(); 
            string detail = Request["detail"]; 
            string traveldate = Request["traveldate"].Trim();
            int percentage0 = Request.Form["percentage0"].AsInt();
            int amount = Request.Form["amount"].AsInt();


            SqlDataAdapter da = new SqlDataAdapter();

            string sql = "";
            sql = sql + "UPDATE [TFC].[ApplicableTFC]  ";

            sql = sql + "set  [FromSector]  = '" + sectorfrom.Trim() + "'  " + Environment.NewLine;
            sql = sql + ",  [ToSector]  = '" + sectorto.Trim() + "'  " + Environment.NewLine;
            sql = sql + ",[TaxCode]  = '" + taxcode.Trim() + "'  " + Environment.NewLine;
            sql = sql + ",[TaxName]  = '" + taxname.Trim() + "'  " + Environment.NewLine;
            if (effectivedate != "")
            {
                DateTime effectivedate1 = DateTime.Parse(effectivedate);
                sql = sql + ",[Effective] = '" + effectivedate1 + "' ";
            }
            else
            {
                sql = sql + ",[Effective] = '" + effectivedate + "'" + Environment.NewLine;
            }

            if (expirydate != "")
            {
                DateTime expirydate1 = DateTime.Parse(expirydate);
                sql = sql + ",[Expiry] = '" + expirydate1 + "' ";
            }
            else
            {
                sql = sql + ",[Expiry] = '" + expirydate + "'" + Environment.NewLine;
            }

            sql = sql + ",[Percentage] = '" + percentage0 + "'  " + Environment.NewLine;
            sql = sql + ",[TaxAmount] = '" + amount + "'  " + Environment.NewLine;
            sql = sql + ",[RATDCurrency]  = '" + currency.Trim() + "'  " + Environment.NewLine;
            if (saledate != "")
            {
                DateTime saledate1 = DateTime.Parse(saledate);
                sql = sql + ",[Sale] = '" + saledate1 + "' ";
            }
            else
            {
                sql = sql + ",[Sale] = '" + saledate + "'" + Environment.NewLine;
            }

            sql = sql + ",[RATDDetails]  = '" + detail.Trim() + "'  " + Environment.NewLine;

            if (traveldate != "")
            {
                DateTime traveldate1 = DateTime.Parse(traveldate);
                sql = sql + ",[Travel] = '" + traveldate1 + "' ";
            }
            else
            {
                sql = sql + ",[Travel] = '" + traveldate + "'" + Environment.NewLine;
            }


            sql = sql + "where [FromSector]  = '" + sectorfrom.Trim() + "'  " + Environment.NewLine;
            sql = sql + "and  [ToSector]  = '" + sectorto.Trim() + "'  " + Environment.NewLine;
            sql = sql + "and [TaxCode]  = '" + taxcode.Trim() + "'  " + Environment.NewLine;
            sql = sql.Replace("'Null'", "Null");

            DataSet ds = new DataSet();
            ds = dbconnect.RetObjDS(sql);
        }
        public void SaveRefTax()
        {
            string from = Request["from"]; 
            string to = Request["to"]; 
            string prime = Request["prime"]; 
            string passenger = Request["passenger"]; 
            string domestic = Request["domestic"]; 
            string duration = Request["duration"]; 
            string validfrom = Request["validfrom"]; 
            string validto = Request["validto"]; 
            string taxcode1 = Request["taxcode1"]; 
            string taxcurrency = Request["taxcurrency"]; 
            int percentage12 = Request.Form["percentage12"].AsInt();
            int taxamount = Request.Form["taxamount"].AsInt();


            SqlDataAdapter da = new SqlDataAdapter();

            string Sql = "DECLARE @RecId bigint" + Environment.NewLine;
            Sql = Sql + "set @RecId = (select iif(MAX(TAXREFID) is null,1, MAX(TAXREFID)+1) As MaxLineid from REF.TAX);" + Environment.NewLine; ;
            Sql = Sql + "INSERT INTO REF.TAX (";
            Sql = Sql + "[FromAirport],[ToAirport],[MappedPrimeCode],[PassengerType],[DomInt],[TransitDuration],[ValidFrom],[ValidTo],[TaxCode],[TaxCurrency],[TaxAmount],[TaxPercentage],TAXREFID)";
            Sql = Sql + "VALUES('";
            Sql = Sql + from + "','";
            Sql = Sql + to + "','";
            Sql = Sql + prime + "','";
            Sql = Sql + passenger + "','";
            Sql = Sql + domestic + "','";
            Sql = Sql + duration + "','";
            Sql = Sql + validfrom + "','";
            Sql = Sql + validto + "','";

            Sql = Sql + taxcode1 + "','";
            Sql = Sql + taxcurrency + "','";
            Sql = Sql + taxamount + "','";
            Sql = Sql + percentage12 + "', @RecId )";
            Sql = Sql.Replace("'Null'", "Null");

            DataSet ds = new DataSet();
            ds = dbconnect.RetObjDS(Sql);
        }
        public void Apptaxdelete()
        {
            string txtsectorfrom = Request["txtsectorfrom"];
            string txtsectorto = Request["txtsectorto"];
            string TxtTaxCode = Request["TxtTaxCode"];
            string effectiveDate = Request["effectiveDate"];
            string expirydate = Request["expirydate"];
            string Percentage = Request["Percentage"];
            string RADTAmount = Request["RADTAmount"];
            string RADTCurrency = Request["RADTCurrency"];
            string RATDDetails = Request["RATDDetails"];

            int Percentagen = Request.Form["Percentage"].AsInt();
            int RADTAmountn = Request.Form["RADTAmount"].AsInt();


            SqlDataAdapter da = new SqlDataAdapter();

            string sql = "";
            sql = sql + "Delete From TFC.ApplicableTFC " + Environment.NewLine;
            sql = sql + "where [FromSector]  = '" + txtsectorfrom.Trim() + "'  " + Environment.NewLine;
            sql = sql + "and  [ToSector]  = '" + txtsectorto.Trim() + "'  " + Environment.NewLine;
            sql = sql + "and [TaxCode]  = '" + TxtTaxCode.Trim() + "'  " + Environment.NewLine;
            if (effectiveDate != "")
            {
                DateTime effectivedate1 = DateTime.Parse(effectiveDate);
                sql = sql + "and [Effective] =  '" + effectivedate1 + "' ";
            }
            else
            {
                sql = sql + "and [Effective] = '" + effectiveDate + "'" + Environment.NewLine;
            }

            if (expirydate != "")
            {
                DateTime expirydate1 = DateTime.Parse(expirydate);
                sql = sql + "and [Expiry] =  '" + expirydate1 + "' ";
            }
            else
            {
                sql = sql + "and [Expiry] = '" + expirydate + "'" + Environment.NewLine;
            }
            if (Percentage != "")
            {
                sql = sql + "and [Percentage] =  '" + Percentagen + "'  ";
            }
            else
            {
                sql = sql + "and [Percentage] = '" + Percentagen + "'" + Environment.NewLine;
            }
            if (RADTAmount != "")
            {
                sql = sql + "and [TaxAmount] =  '" + RADTAmountn + "' ";
            }
            else
            {
                sql = sql + "and [TaxAmount] = '" + RADTAmountn + "'" + Environment.NewLine;
            }
            if (RADTCurrency != "")
            {
                sql = sql + "and [RATDCurrency] =  '" + RADTCurrency + "' ";
            }
            else
            {
                sql = sql + "and [RATDCurrency] = '" + RADTCurrency + "'" + Environment.NewLine;
            }
            if (RATDDetails != "")
            {
                sql = sql + "and [RATDDetails] = '" + RATDDetails + "' ";
            }
            else
            {
                sql = sql + "and [RATDDetails] = '" + RATDDetails + "'" + Environment.NewLine;
            }
            //sql = sql + "and [TaxAmount]  = '" + RetNullOrValue(RADTAmount) + "'  " + Environment.NewLine;
            //sql = sql + "and [RATDCurrency]  = '" + RetNullOrValue(RADTCurrency) + "'  " + Environment.NewLine;
            //sql = sql + "and [RATDDetails]    = '" + RetNullOrValue(RATDDetails) + "'  " + Environment.NewLine;
            sql = sql.Replace("'Null'", "Null");
            DataSet ds = new DataSet();
            ds = dbconnect.RetObjDS(sql);
        }
        public ActionResult agentRBD()
        {
            string dateFromTFC1 = Request["dateFromTFC"];
            string dateToTFC1 = Request["dateToTFC"];
            DateTime dateFromTFC = DateTime.Parse(dateFromTFC1);
            DateTime dateToTFC = DateTime.Parse(dateToTFC1);

            SqlConnection con = new SqlConnection(pbConnectionString);

            string sql = " select distinct sdh.AgentNumericCode  from [Pax].[SalesDocumentHeader] sdh   " + Environment.NewLine;
            sql = sql + " join Pax.SalesRelatedDocumentInformation srd on sdh.HdrGuid = srd.HdrGuid   " + Environment.NewLine;
            sql = sql + " left join Pax.SalesDocumentCoupon sdc on srd.RelatedDocumentGuid = sdc.RelatedDocumentGuid   " + Environment.NewLine;
            sql = sql + " where  sdc.FlightDepartureDate between '" + dateFromTFC + "' and '" + dateToTFC + "' and sdc.CouponStatus = 'F'   " + Environment.NewLine;

            DataSet ds = new DataSet();
            SqlDataAdapter ada = new SqlDataAdapter(sql, con);
            ada.Fill(ds);
            con.Close();


            int lonagentRBD = ds.Tables[0].Rows.Count;

            string[,] ListeagentRBD = new string[1, lonagentRBD];

            int i = 0;

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                ListeagentRBD[0, i] = dr[ds.Tables[0].Columns[0].ColumnName].ToString();
                i++;
            }
            ViewBag.lonagentRBD = lonagentRBD;
            ViewBag.ListeagentRBD = ListeagentRBD;

            return PartialView();
        }
        public ActionResult btnSearch_Click(object sender, EventArgs e)
        {
            /*Discount*/
            string page = "1";
            string record = "150";
            string dateFromTFC1 = Request["dateFromTFC"];
            string dateToTFC1 = Request["dateToTFC"];
            string ag = Request["ag"];
            DateTime dateFromTFC = DateTime.Parse(dateFromTFC1);
            DateTime dateToTFC = DateTime.Parse(dateToTFC1);
            string sql = " DECLARE @PageNo int = '" + page + " ';    " + Environment.NewLine;
            sql = sql + " DECLARE @RecordsPerPage int = '" + record + " ';   " + Environment.NewLine;
            sql = sql + "   " + Environment.NewLine;
            sql = sql + " select sdh.DocumentNumber, sdc.CouponNumber,sdh.AgentNumericCode, sdc.FlightNumber,sdc.FlightDepartureDate  " + Environment.NewLine;
            sql = sql + " ,sdc.FareBasisTicketDesignator, f15.FareAndPassengerTypeCodeDiscount , f15.Discount,sdh.PassengerName,sdh.AmountCollectedCurrency,sdh.AmountCollected,sdh.FareCalculationArea,sdc.ReservationBookingDesignator   " + Environment.NewLine;
            sql = sql + " from Pax.SalesDocumentHeader sdh    " + Environment.NewLine;
            sql = sql + " join Pax.SalesRelatedDocumentInformation srd on sdh.HdrGuid = srd.HdrGuid   " + Environment.NewLine;
            sql = sql + " left join Pax.SalesDocumentCoupon sdc on srd.RelatedDocumentGuid = sdc.RelatedDocumentGuid   " + Environment.NewLine;
            sql = sql + " join Pax.FareBasis f15 on sdc.FareBasisTicketDesignator = f15.FareBasisTicketDesignator  " + Environment.NewLine;
            sql = sql + " where    " + Environment.NewLine;
            sql = sql + " (CHARINDEX( ' DIS', sdc.FareBasisTicketDesignator ) > 0   " + Environment.NewLine;
            sql = sql + " or CHARINDEX( 'DISC', sdc.FareBasisTicketDesignator ) > 0   " + Environment.NewLine;
            sql = sql + " or CHARINDEX( '/DIS', sdc.FareBasisTicketDesignator ) > 0   " + Environment.NewLine;
            sql = sql + " or f15.FareAndPassengerTypeCodeDiscount is not null ) " + Environment.NewLine;
            sql = sql + " and FlightDepartureDate between '" + dateFromTFC + "' and '" + dateToTFC + "'  " + Environment.NewLine;
            sql = sql + " and sdh.AgentNumericCode like '" + ag + "'  " + Environment.NewLine;
            sql = sql + " order by sdc.FlightDepartureDate,sdh.DocumentNumber,sdc.CouponNumber OFFSET (@PageNo-1)*@RecordsPerPage ROWS    " + Environment.NewLine;
            sql = sql + " FETCH NEXT @RecordsPerPage ROWS ONLY   " + Environment.NewLine;
            SqlConnection cs = new SqlConnection(pbConnectionString);
            DataSet ds = new DataSet();
            SqlDataAdapter ada = new SqlDataAdapter(sql, cs);
            ada.Fill(ds);
            int lignebtnSearch = ds.Tables[0].Rows.Count;
            int colonebtnSearch = ds.Tables[0].Columns.Count;
            string[,] ListebtnSearch = new string[lignebtnSearch, colonebtnSearch];
            int i = 0;
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                for (int j = 0; j < ds.Tables[0].Columns.Count; j++)
                {
                    if (j == 4)
                    {
                        string h = null;
                        string hh = null;
                        h = dr[ds.Tables[0].Columns[j].ColumnName].ToString();
                        if (h != "")
                        {
                            hh = h.Substring(0, 11).ToString();
                            ListebtnSearch[i, j] = hh;
                        }
                    }
                    else
                    {
                        ListebtnSearch[i, j] = dr[ds.Tables[0].Columns[j].ColumnName].ToString();
                    }
                }
                i++;
            }
            ViewBag.lignebtnSearch = lignebtnSearch;
            ViewBag.colonebtnSearch = colonebtnSearch;
            ViewBag.ListebtnSearch = ListebtnSearch;
            cs.Close();
            /*fin Discount*/

            /*aginfo*/
            if (ag != "%")
            {
                SqlConnection cs1 = new SqlConnection(pbConnectionString);
                string sql1 = " with a as (  " + Environment.NewLine;
                sql1 = sql1 + " select f1.AgencyNumericCode,f1.LegalName,f1.Locationaddress from Ref.agent f1 where f1.AgencyNumericCode like '" + ag + "'  " + Environment.NewLine;
                sql1 = sql1 + " union   " + Environment.NewLine;
                sql1 = sql1 + " select f2.AgencyNumericCode,f2.LegalName,f2.Locationaddress from Ref.Agent_Own f2 where f2.AgencyNumericCode like '" + ag + "'  " + Environment.NewLine;
                sql1 = sql1 + "   " + Environment.NewLine;
                sql1 = sql1 + " )  " + Environment.NewLine;
                sql1 = sql1 + "   " + Environment.NewLine;
                sql1 = sql1 + " select   " + Environment.NewLine;
                sql1 = sql1 + " isnull (f3.AgencyNumericCode,a.AgencyNumericCode) as AgencyNumericCode   " + Environment.NewLine;
                //sql1 = sql1 + " ,isnull (f3.Name,a.LegalName) as LegalName   " + Environment.NewLine;
               // sql1 = sql1 + " ,isnull (f3.Address,a.LocationAddress) as LegalAddress    " + Environment.NewLine;
                sql1 = sql1 + " from a  " + Environment.NewLine;
                sql1 = sql1 + " left join ref.PassengerAgencyDetails f3   " + Environment.NewLine;
                sql1 = sql1 + " on a.AgencyNumericCode = f3.AgencyNumericCode   " + Environment.NewLine;
                sql1 = sql1 + " where a.AgencyNumericCode like '" + ag + "'  " + Environment.NewLine;
                DataSet ds1 = new DataSet();
                SqlDataAdapter ada1 = new SqlDataAdapter(sql1, cs1);
                ada1.Fill(ds1);
                foreach (DataRow dr1 in ds1.Tables[0].Rows)
                {
                    ViewBag.txtagtname = dr1[ds1.Tables[0].Columns[1].ColumnName].ToString();
                    ViewBag.txtagtadd = dr1[ds1.Tables[0].Columns[2].ColumnName].ToString();
                }
                cs1.Close();
            }
            else
            {
                ViewBag.txtagtname = "";
                ViewBag.txtagtadd = "";
            }
            /*fin aginfo*/
            return PartialView();
        }
        public ActionResult btnReset_Click()
        {
            string dateFrom = DateTime.Now.AddMonths(-1).ToString("dd-MMM-yyyy");
            string dateTo = DateTime.Now.ToString("dd-MMM-yyyy");
            string[] date = new string[2] { dateFrom, dateTo };
            ViewBag.date = date;
            return PartialView();
        }
        public ActionResult Query()
        {
            string page = "1"; 
            string record = "150";
            string finalSareFrom1 = Request["finalSareFrom"];
            string finalSareTo1 = Request["finalSareTo"];
            string residual = Request["residual"];
            //int residual = Request.Form["residual"].AsInt();

            DateTime finalSareFrom = DateTime.Parse(finalSareFrom1);
            DateTime finalSareTo = DateTime.Parse(finalSareTo1);
            string aa = OwnAirline();
            SqlConnection cs = new SqlConnection(pbConnectionString);
            cs.Open();
            DataSet ds = new DataSet();
            SqlCommand cmd = new SqlCommand("[Pax].[FinalSharesCheck]", cs);
            cmd.CommandTimeout = 1000;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ownairline", aa);
            cmd.Parameters.AddWithValue("@res", residual);
            cmd.Parameters.AddWithValue("@DateofIssueFrom", finalSareFrom);
            cmd.Parameters.AddWithValue("@DateofIssueTo", finalSareTo);
            cmd.Parameters.AddWithValue("@PageNo", page);
            cmd.Parameters.AddWithValue("@RecordsPerPage", record);
            int i = 0;
            SqlDataAdapter ada = new SqlDataAdapter(cmd);
            ada.Fill(ds);
            int ligne = ds.Tables[0].Rows.Count;
            int colone = ds.Tables[0].Columns.Count;
            string[,] Liste = new string[ligne, colone];
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                for (int j = 0; j < ds.Tables[0].Columns.Count; j++)
                {
                    if (j == 1)
                    {
                        string h = null;
                        h = dr[ds.Tables[0].Columns[j].ColumnName].ToString();
                        if (h != "")
                        {
                            Liste[i,j] = DateTime.Parse(h).ToString("dd-MMM-yyyy");
                        }
                    }
                    else
                    {
                        Liste[i, j] = dr[ds.Tables[0].Columns[j].ColumnName].ToString();
                    }
                }
                i++;
            }
            cs.Close();
            ViewBag.ligne = ligne;
            ViewBag.colone = colone;
            ViewBag.Liste = Liste;
            return PartialView();
        }
        public string OwnAirline()
        {
            string own = "";
            string sql = "select String4 from Adm.GSP where Parameter = 'SYS0001'";
            DataSet ds = new DataSet();
            ds = dbconnect.RetObjDS(sql);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                own = dr[ds.Tables[0].Columns[0].ColumnName].ToString();
            }
            return own;
        }
        public ActionResult clearfinalsharevalidation()
        {
            //fait par christian
            string dateFrom = DateTime.Now.AddMonths(-1).ToString("dd-MMM-yyyy");
            string dateTo = DateTime.Now.ToString("dd-MMM-yyyy");
            string[] date = new string[2] { dateFrom, dateTo };
            ViewBag.date = date;
            //and fait par christian
            return PartialView();
        }
        public ActionResult ErrorMSG()
        {
            string dtpFromValue1 = Request["dtpFromValue"];
            string dtpToValue1 = Request["dtpToValue"];

            DateTime dtpFromValue = DateTime.Parse(dtpFromValue1);
            DateTime dtpToValue = DateTime.Parse(dtpToValue1);

            SqlConnection cs = new SqlConnection(pbConnectionString);

            string sql = "select distinct [ErrorMsg]  from [Pax].[ProrationException] where cast(DateProcess as date) between cast('" + dtpFromValue + "' as date) and cast('" + dtpToValue + "' as date)  ";

            DataSet ds = new DataSet();
            SqlDataAdapter ada = new SqlDataAdapter(sql, cs);
            ada.Fill(ds);

            int ligne = ds.Tables[0].Rows.Count;
            string[,] Liste = new string[1, ligne];
            int i = 0;
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                Liste[0,i] = dr[ds.Tables[0].Columns[0].ColumnName].ToString();
            }


            ViewBag.ligne = ligne;
            ViewBag.Liste = Liste;
            return PartialView();
        }
        public ActionResult Querypev()
        {
            string dtpFromValue1 = Request["dtpFromValue"];
            string dtpToValue1 = Request["dtpToValue"];
            DateTime dtpFromValue = DateTime.Parse(dtpFromValue1);
            DateTime dtpToValue = DateTime.Parse(dtpToValue1);
            string dtpIssueDateFromValue1 = Request["dtpIssueDateFromValue"];
            string dtpIssueDateToValue1 = Request["dtpIssueDateToValue"];
            DateTime dtpIssueDateFromValue = DateTime.Parse(dtpIssueDateFromValue1);
            DateTime dtpIssueDateToValue = DateTime.Parse(dtpIssueDateToValue1);
            string dtpFromChecked = Request["dtpFromChecked"];
            string dtpIssueDateFromChecked = Request["dtpIssueDateFromChecked"];
            string ag = Request["ag"];
            string doc = Request["doc"];
            string sql = "select  distinct f1.DocumentNumber,f3.dateofissue,f3.[FareCalculationArea] " + Environment.NewLine;
            sql += ",concat(f3.Farecurrency,' ',f3.Fare) as Fare,concat(f3.totalcurrency,' ',f3.EquivalentFare) as EquivalentFare ,f1.[ErrorMsg],f1.DateProcess" + Environment.NewLine;
            sql += ",case when f3.ProcessingFileType like '%lift%' then 'LIFT' else f3.DataSource END as DataSource" + Environment.NewLine;
            sql += ",f3.AgentNumericCode, f3.EndosRestriction, f3.TransactionCode, f3.OriginalIssueDocumentNumber" + Environment.NewLine;
            sql += ",case when f4.FormofPaymentType = 'EX' then 'EXCHANGE' ELSE NULL END AdditionalRemarks" + Environment.NewLine;
            sql += "from pax.ProrationException f1 " + Environment.NewLine;
            sql += "left join Pax.SalesRelatedDocumentInformation f2 on f1.RelatedDocumentGuid = f2.RelatedDocumentGuid " + Environment.NewLine;
            sql += "left join Pax.SalesDocumentpayment f4 on f2.HdrGuid = f4.HdrGuidRef and FormofPaymentType = 'EX'" + Environment.NewLine;
            sql += "left join Pax.SalesDocumentHeader f3 on f3.HdrGuid = f2.HdrGuid " + Environment.NewLine;
            if (dtpFromChecked == "true")
            {
                sql += "and cast(f1.DateProcess as date) between cast('" + dtpFromValue + "' as date) and cast('" + dtpToValue + "' as date)" + Environment.NewLine;
            }
            if (dtpIssueDateFromChecked == "true")
            {
                sql += "and cast(f3.dateofissue as date) between cast('" + dtpIssueDateFromValue + "' as date) and cast('" + dtpIssueDateToValue + "' as date)" + Environment.NewLine;
            }
            sql += "WHERE f2.RelatedDocumentNumber like  '" + doc + "' and [ErrorMsg] like  '" + ag + "' " + Environment.NewLine;
            SqlConnection cs = new SqlConnection(pbConnectionString);
            DataSet ds = new DataSet();
            SqlDataAdapter ada = new SqlDataAdapter(sql, cs);
            ada.Fill(ds);
            int ligne = ds.Tables[0].Rows.Count;
            int colone = ds.Tables[0].Columns.Count;
            string[,] Liste = new string[ligne, colone];
            string[] count = new string[1000000];
            int t = 0;
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                count[t] = dr[ds.Tables[0].Columns[0].ColumnName].ToString();

                for (int y = 0; y < ds.Tables[0].Columns.Count; y++)
                {
                    if ((y == 1) || (y == 6))
                    {
                        string h = null;
                        h = dr[ds.Tables[0].Columns[y].ColumnName].ToString();
                        if (h != "")
                        {
                            Liste[t,y] = DateTime.Parse(h).ToString("dd-MMM-yyyy");
                        }
                    }
                    else
                    {
                        Liste[t, y] = dr[ds.Tables[0].Columns[y].ColumnName].ToString();
                    }
                }
                t++;
            }
            string[] b = count.Distinct().ToArray();

            ViewBag.txtDocCount = (b.Length - 1).ToString();
            ViewBag.ligne = ligne;
            ViewBag.colone = colone;
            ViewBag.Liste = Liste;

            return PartialView();
        }
        public ActionResult fcadoc()
        {
            string fca = Request["fca"];
            string dtpFromValue1 = Request["dtpFromValue"];
            string dtpToValue1 = Request["dtpToValue"];
            DateTime dtpFromValue = DateTime.Parse(dtpFromValue1);
            DateTime dtpToValue = DateTime.Parse(dtpToValue1);
            string dtpIssueDateFromValue1 = Request["dtpIssueDateFromValue"];
            string dtpIssueDateToValue1 = Request["dtpIssueDateToValue"];
            DateTime dtpIssueDateFromValue = DateTime.Parse(dtpIssueDateFromValue1);
            DateTime dtpIssueDateToValue = DateTime.Parse(dtpIssueDateToValue1);
            string dtpFromChecked = Request["dtpFromChecked"];
            string dtpIssueDateFromChecked = Request["dtpIssueDateFromChecked"];
            string ag = Request["ag"];
            string doc = Request["doc"];
            string sql = "select  distinct f1.DocumentNumber,f3.dateofissue,f3.[FareCalculationArea] " + Environment.NewLine;
            sql += ",concat(f3.Farecurrency,' ',f3.Fare) as Fare,concat(f3.totalcurrency,' ',f3.EquivalentFare) as EquivalentFare ,f1.[ErrorMsg],f1.DateProcess" + Environment.NewLine;
            sql += ",case when f3.ProcessingFileType like '%lift%' then 'LIFT' else f3.DataSource END as DataSource" + Environment.NewLine;
            sql += ",f3.AgentNumericCode, f3.EndosRestriction, f3.TransactionCode, f3.OriginalIssueDocumentNumber" + Environment.NewLine;
            sql += ",case when f4.FormofPaymentType = 'EX' then 'EXCHANGE' ELSE NULL END AdditionalRemarks" + Environment.NewLine;
            sql += "from pax.ProrationException f1 " + Environment.NewLine;
            sql += "left join Pax.SalesRelatedDocumentInformation f2 on f1.RelatedDocumentGuid = f2.RelatedDocumentGuid " + Environment.NewLine;
            sql += "left join Pax.SalesDocumentpayment f4 on f2.HdrGuid = f4.HdrGuidRef and FormofPaymentType = 'EX'" + Environment.NewLine;
            sql += "left join Pax.SalesDocumentHeader f3 on f3.HdrGuid = f2.HdrGuid " + Environment.NewLine;
            if (dtpFromChecked == "true")
            {
                sql += "and cast(f1.DateProcess as date) between cast('" + dtpFromValue + "' as date) and cast('" + dtpToValue + "' as date)" + Environment.NewLine;
            }
            if (dtpIssueDateFromChecked == "true")
            {
                sql += "and cast(f3.dateofissue as date) between cast('" + dtpIssueDateFromValue + "' as date) and cast('" + dtpIssueDateToValue + "' as date)" + Environment.NewLine;
            }
            sql += "WHERE f2.RelatedDocumentNumber like  '" + doc + "' and [ErrorMsg] like  '" + ag + "' " + Environment.NewLine;
            SqlConnection cs = new SqlConnection(pbConnectionString);
            DataSet ds = new DataSet();
            SqlDataAdapter ada = new SqlDataAdapter(sql, cs);
            ada.Fill(ds);


            string[] arr = new string[1000000];
            int k = 0;



            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                if (dr[ds.Tables[0].Columns[2].ColumnName].ToString() == fca)
                {
                    arr[k] = dr[ds.Tables[0].Columns[0].ColumnName].ToString();
                    k++;
                }
            }

            string[] b = arr.Distinct().ToArray();
            int h = (b.Length - 1);
            ViewBag.h = h;

            return PartialView();
        }
        public ActionResult clearpev()
        {
            //fait par christian
            string dtpFromValue = DateTime.Now.AddMonths(-1).ToString("dd-MMM-yyyy");
            string dtpToValue = DateTime.Now.ToString("dd-MMM-yyyy");

            string dtpIssueDateFromValue = DateTime.Now.AddMonths(-1).ToString("dd-MMM-yyyy");
            string dtpIssueDateToValue = DateTime.Now.ToString("dd-MMM-yyyy");

            string dtpFromChecked = "";
            string dtpToChecked = "";

            string dtpIssueDateFromChecked = "";
            string dtpIssueDateToChecked = "";

            ViewBag.dtpFromValue = dtpFromValue;
            ViewBag.dtpToValue = dtpToValue;

            ViewBag.dtpFromChecked = dtpFromChecked;
            ViewBag.dtpToChecked = dtpToChecked;


            ViewBag.dtpIssueDateFromValue = dtpIssueDateFromValue;
            ViewBag.dtpIssueDateToValue = dtpIssueDateToValue;

            ViewBag.dtpIssueDateFromChecked = dtpIssueDateFromChecked;
            ViewBag.dtpIssueDateToChecked = dtpIssueDateToChecked;
            //and fait par christian

            return PartialView();
        }
        public ActionResult AgentNum()
        {

            string pricingFrom1 = Request["pricingFrom"];
            string pricingTo1 = Request["pricingTo"];
            DateTime pricingFrom = DateTime.Parse(pricingFrom1);
            DateTime pricingTo = DateTime.Parse(pricingTo1);

            SqlConnection con = new SqlConnection(pbConnectionString);

            string sql = " ";
            sql += " select distinct sdh.AgentNumericCode from Pax.SalesDocumentHeader sdh   " + Environment.NewLine;
            sql += " join pax.SalesRelatedDocumentInformation srd on sdh.HdrGuid = srd.HdrGuid   " + Environment.NewLine;
            sql += " join pax.SalesDocumentCoupon sdc on srd.RelatedDocumentGuid =sdc.RelatedDocumentGuid   " + Environment.NewLine;
            sql += " where DateofIssue between '" + pricingFrom + "' and  '" + pricingTo + "'  " + Environment.NewLine;


            DataSet ds = new DataSet();
            SqlDataAdapter ada = new SqlDataAdapter(sql, con);
            ada.Fill(ds);
            con.Close();


            int loncboAgt = ds.Tables[0].Rows.Count;

            string[,] ListecboAgt = new string[1, loncboAgt];

            int i = 0;

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                ListecboAgt[0, i] = dr[ds.Tables[0].Columns[0].ColumnName].ToString();
                i++;
            }
            ViewBag.loncboAgt = loncboAgt;
            ViewBag.ListecboAgt = ListecboAgt;

            return PartialView();
        }
        public ActionResult frmcode()
        {
            string code = Request["code"];
            int coden = Request.Form["code"].AsInt();


            string pricingFrom1 = Request["pricingFrom"];
            string pricingTo1 = Request["pricingTo"];
            DateTime pricingFrom = DateTime.Parse(pricingFrom1);
            DateTime pricingTo = DateTime.Parse(pricingTo1);

            SqlConnection con = new SqlConnection(pbConnectionString);

            string sql = " ";
            sql += " select distinct sdh.AgentNumericCode from Pax.SalesDocumentHeader sdh   " + Environment.NewLine;
            sql += " join pax.SalesRelatedDocumentInformation srd on sdh.HdrGuid = srd.HdrGuid   " + Environment.NewLine;
            sql += " join pax.SalesDocumentCoupon sdc on srd.RelatedDocumentGuid =sdc.RelatedDocumentGuid   " + Environment.NewLine;
            sql += " where DateofIssue between '" + pricingFrom + "' and  '" + pricingTo + "'  " + Environment.NewLine;


            DataSet ds = new DataSet();
            SqlDataAdapter ada = new SqlDataAdapter(sql, con);
            ada.Fill(ds);
            con.Close();


            int loncboAgt = ds.Tables[0].Rows.Count;

            string[,] ListecboAgt = new string[1, loncboAgt];

            int i = 0;

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                ListecboAgt[0, i] = dr[ds.Tables[0].Columns[0].ColumnName].ToString();
                i++;
            }
            ViewBag.loncboAgt = loncboAgt;
            ViewBag.ListecboAgt = ListecboAgt;
            ViewBag.code = code;
            /*Test*/
            SqlConnection con1 = new SqlConnection(pbConnectionString);
            string sql1 = "select LegalName,LocationAddress from Ref.VW_Agent where AgencyNumericCode =left('" + coden + "',7) ";

            DataSet ds1 = new DataSet();
            SqlDataAdapter ada1 = new SqlDataAdapter(sql1, con1);
            ada1.Fill(ds1);
            con1.Close();


            foreach (DataRow dr1 in ds1.Tables[0].Rows)
            {
                ViewBag.txtagentname = dr1[ds1.Tables[0].Columns[0].ColumnName].ToString();
                ViewBag.txtAgentLocation = ViewBag.txtagentname = dr1[ds1.Tables[0].Columns[1].ColumnName].ToString();
            }
            
            /*fin Test*/
            return PartialView();
        }
        public ActionResult Testagcode()
        {
            int cboAgt = Request.Form["cboAgt"].AsInt();
            /*Test*/

            SqlConnection cs = new SqlConnection(pbConnectionString);
            DataSet ds = new DataSet();
            string sql = "select LegalName,LocationAddress from Ref.VW_Agent where AgencyNumericCode =left('" + cboAgt + "',7) ";

            SqlDataAdapter ada = new SqlDataAdapter(sql, cs);
            ada.Fill(ds);

            ViewBag.l = ds.Tables[0].Rows.Count;
            foreach (DataRow dr1 in ds.Tables[0].Rows)
            {
                ViewBag.txtagentname = dr1[ds.Tables[0].Columns[0].ColumnName].ToString();
                ViewBag.txtAgentLocation = dr1[ds.Tables[0].Columns[1].ColumnName].ToString();
            }
            /*fin Test*/
            return PartialView();
        }
        public ActionResult cbSelectionSector()
        {
            return PartialView();
        }
        public ActionResult cbSelectionFareComponent()
        {
            return PartialView();
        }
        public ActionResult cbSelectionJourney()
        {
            return PartialView();
        }
        public ActionResult QUERYpricing()
        {
            string cbSelection = Request["cbSelection"];
            string cboAgt = Request["cboAgt"];
            string cmbdow = Request["cmbdow"];

            string pricingFrom1 = Request["pricingFrom"];
            string pricingTo1 = Request["pricingTo"];
            DateTime pricingFrom = DateTime.Parse(pricingFrom1);
            DateTime pricingTo = DateTime.Parse(pricingTo1);

            string ag = "";
            string we = "";
            string se = "";
            string page = "1";              // For Pagination 
            string record = "150";


            if (cboAgt == "-All-")
            {
                ag = "%";
            }
            else
            {
                ag = cboAgt;
            }

            if (cmbdow == "-All-")
            {
                we = "%";
            }
            else
            {
                we = cmbdow;
            }
            /*Selection*/
            if (cbSelection == "Sector")
            {
                se = "1";
            }
            if (cbSelection == "Fare Component")
            {
                se = "2";
            }
            if (cbSelection == "Journey")
            {
                se = "3";
            }
            /*fin Selection*/

            SqlConnection cs = new SqlConnection(pbConnectionString);
            DataSet ds = new DataSet();

            SqlCommand cmd = new SqlCommand("[Pax].[SP_PricingManagement]", cs);
            cmd.CommandTimeout = 1000;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@IssueDate_from", pricingFrom);
            cmd.Parameters.AddWithValue("@IssueDate_to", pricingTo);
            cmd.Parameters.AddWithValue("@AgentNumericCode", ag);
            cmd.Parameters.AddWithValue("@selection", se);
            cmd.Parameters.AddWithValue("@WEKDAY", we);
            cmd.Parameters.AddWithValue("@PageNo", page);
            cmd.Parameters.AddWithValue("@RecordsPerPage", record);

            int i = 0;
            SqlDataAdapter ada = new SqlDataAdapter(cmd);
            ada.Fill(ds);

            int ligne = ds.Tables[0].Rows.Count;
            int colone = ds.Tables[0].Columns.Count;
            string[,] Liste = new string[ligne, colone];

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                for (int j = 0; j<ds.Tables[0].Columns.Count; j++)
                {
                    if ((j == 1) || (j == 24) || (j == 29) || (j == 30))
                    {
                        string h = null;

                        h = dr[ds.Tables[0].Columns[j].ColumnName].ToString();

                        if (h != "")
                        {
                            Liste[i,j] = DateTime.Parse(h).ToString("dd-MMM-yyyy");
                        }
                    }
                    else
                    {
                        Liste[i, j] = dr[ds.Tables[0].Columns[j].ColumnName].ToString();
                    }
                }
                i++;
            }

            ViewBag.ligne = ligne;
            ViewBag.colone = colone;
            ViewBag.Liste = Liste;
            ViewBag.cbSelection = cbSelection;

            return PartialView();
        }
        public ActionResult AgentNum1pricing()
        {
            string basisFrom1 = Request["basisFrom"];
            string basisTo1 = Request["basisTo"];
            DateTime basisFrom = DateTime.Parse(basisFrom1);
            DateTime basisTo = DateTime.Parse(basisTo1);
            SqlConnection con = new SqlConnection(pbConnectionString);
            string sql = " ";
            sql += " select distinct sdh.AgentNumericCode from Pax.SalesDocumentHeader sdh   " + Environment.NewLine;
            sql += " join pax.SalesRelatedDocumentInformation srd on sdh.HdrGuid = srd.HdrGuid   " + Environment.NewLine;
            sql += " join pax.SalesDocumentCoupon sdc on srd.RelatedDocumentGuid =sdc.RelatedDocumentGuid   " + Environment.NewLine;
            sql += " where DateofIssue between '" + basisFrom + "' and  '" + basisTo + "'  " + Environment.NewLine;
            DataSet ds = new DataSet();
            SqlDataAdapter ada = new SqlDataAdapter(sql, con);
            ada.Fill(ds);
            con.Close();

            int loncboAgt1 = ds.Tables[0].Rows.Count;

            string[,] ListecboAgt1 = new string[1, loncboAgt1];

            int i = 0;

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                ListecboAgt1[0, i] = dr[ds.Tables[0].Columns[0].ColumnName].ToString();
                i++;
            }
            ViewBag.loncboAgt1 = loncboAgt1;
            ViewBag.ListecboAgt1 = ListecboAgt1;
            
            return PartialView();
        }
        public ActionResult Test1basicpricing()
        {
            int cboagt1 = Request.Form["cboagt1"].AsInt();
            /*Test1*/
            SqlConnection cs = new SqlConnection(pbConnectionString);
            DataSet ds = new DataSet();
            string sql = "select LegalName,LocationAddress from Ref.VW_Agent where AgencyNumericCode =left('" + cboagt1 + "',7) ";

            SqlDataAdapter ada = new SqlDataAdapter(sql, cs);
            ada.Fill(ds);

            ViewBag.l = ds.Tables[0].Rows.Count;
            foreach (DataRow dr1 in ds.Tables[0].Rows)
            {
                ViewBag.txtagtname1 = dr1[ds.Tables[0].Columns[0].ColumnName].ToString();
                ViewBag.txtagtPOS = dr1[ds.Tables[0].Columns[1].ColumnName].ToString();
            }
            /*fin Test1*/

            return PartialView();
        }
        public ActionResult frmcode1()
        {
            string code = Request["code"];
            int coden = Request.Form["code"].AsInt();

            string basisFrom1 = Request["basisFrom"];
            string basisTo1 = Request["basisTo"];
            DateTime basisFrom = DateTime.Parse(basisFrom1);
            DateTime basisTo = DateTime.Parse(basisTo1);
            SqlConnection con = new SqlConnection(pbConnectionString);
            string sql = " ";
            sql += " select distinct sdh.AgentNumericCode from Pax.SalesDocumentHeader sdh   " + Environment.NewLine;
            sql += " join pax.SalesRelatedDocumentInformation srd on sdh.HdrGuid = srd.HdrGuid   " + Environment.NewLine;
            sql += " join pax.SalesDocumentCoupon sdc on srd.RelatedDocumentGuid =sdc.RelatedDocumentGuid   " + Environment.NewLine;
            sql += " where DateofIssue between '" + basisFrom + "' and  '" + basisTo + "'  " + Environment.NewLine;
            DataSet ds = new DataSet();
            SqlDataAdapter ada = new SqlDataAdapter(sql, con);
            ada.Fill(ds);
            con.Close();

            int loncboAgt1 = ds.Tables[0].Rows.Count;

            string[,] ListecboAgt1 = new string[1, loncboAgt1];

            int i = 0;

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                ListecboAgt1[0, i] = dr[ds.Tables[0].Columns[0].ColumnName].ToString();
                i++;
            }
            ViewBag.loncboAgt1 = loncboAgt1;
            ViewBag.ListecboAgt1 = ListecboAgt1;
            ViewBag.code = code;
            /*Test1*/
            SqlConnection cs1 = new SqlConnection(pbConnectionString);
            DataSet ds1 = new DataSet();
            string sql1 = "select LegalName,LocationAddress from Ref.VW_Agent where AgencyNumericCode =left('" + coden + "',7) ";

            SqlDataAdapter ada1 = new SqlDataAdapter(sql1, cs1);
            ada1.Fill(ds1);

            ViewBag.l = ds1.Tables[0].Rows.Count;
            foreach (DataRow dr1 in ds1.Tables[0].Rows)
            {
                ViewBag.txtagtname1 = dr1[ds1.Tables[0].Columns[0].ColumnName].ToString();
                ViewBag.txtagtPOS = dr1[ds1.Tables[0].Columns[1].ColumnName].ToString();
            }
            /*fin Test1*/
            return PartialView();
        }
        public ActionResult FareBasispricing()
        {
            string basisFrom1 = Request["basisFrom"];
            string basisTo1 = Request["basisTo"];
            DateTime basisFrom = DateTime.Parse(basisFrom1);
            DateTime basisTo = DateTime.Parse(basisTo1);
            SqlConnection con = new SqlConnection(pbConnectionString);

            string sql = " ";
            sql += " select distinct sdc.FareBasisTicketDesignator from Pax.SalesDocumentHeader sdh   " + Environment.NewLine;
            sql += " join pax.SalesRelatedDocumentInformation srd on sdh.HdrGuid = srd.HdrGuid   " + Environment.NewLine;
            sql += " join pax.SalesDocumentCoupon sdc on srd.RelatedDocumentGuid =sdc.RelatedDocumentGuid   " + Environment.NewLine;
            sql += " where DateofIssue between '" + basisFrom + "' and  '" + basisTo + "'  " + Environment.NewLine;
            DataSet ds = new DataSet();
            SqlDataAdapter ada = new SqlDataAdapter(sql, con);
            ada.Fill(ds);
            con.Close();

            int loncbofarbasis = ds.Tables[0].Rows.Count;

            string[,] Listecbofarbasis = new string[1, loncbofarbasis];

            int i = 0;

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                Listecbofarbasis[0, i] = dr[ds.Tables[0].Columns[0].ColumnName].ToString();
                i++;
            }
            ViewBag.loncbofarbasis = loncbofarbasis;
            ViewBag.Listecbofarbasis = Listecbofarbasis;

            return PartialView();
        }
        public ActionResult btnSearch1_Clickbasicpricing() {
            string cboagt1 = Request["cboagt1"];
            string cboFBdow = Request["cboFBdow"];
            string cbofarbasis = Request["cbofarbasis"];

            string basisFrom1 = Request["basisFrom"];
            string basisTo1 = Request["basisTo"];
            DateTime basisFrom = DateTime.Parse(basisFrom1);
            DateTime basisTo = DateTime.Parse(basisTo1);

            string FBagt = "";
            string FBwe = "";
            string farebasis = "";
            /*QueryFarebasis*/
            if (cboagt1 == "-All-")
            {
                FBagt = "%";
            }
            else
            {
                FBagt = cboagt1;
            }

            if (cboFBdow == "-All-")
            {
                FBwe = "%";
            }
            else
            {
                FBwe = cboFBdow;
            }
            if (cbofarbasis == "-All-")
            {
                farebasis = "%";
            }
            else
            {
                farebasis = cbofarbasis;
            }

            string sql = "";

            sql += " WITH A as ( " + Environment.NewLine;
            sql += " select sdh.DateofIssue ,DATENAME(dw,sdh.DateofIssue) as DayOfTheWeek,sdh.AgentNumericCode,isNull(agt.LegalName,'Missing Agent Info') as agentname,isNull(agt.LocationCountry,'Missing Agent Info') as  POS " + Environment.NewLine;
            sql += " ,sdh.PassengerName,sdh.FareCalculationModeIndicator " + Environment.NewLine;
            sql += " ,sdc.OriginAirportCityCode,sdc.DestinationAirportCityCode, sdh.TrueOriginDestinationCityCodes " + Environment.NewLine;
            sql += " ,sdc.FareBasisTicketDesignator,sdc.ReservationBookingDesignator " + Environment.NewLine;
            sql += " from Pax.SalesDocumentHeader sdh " + Environment.NewLine;
            sql += " join pax.SalesRelatedDocumentInformation srd on sdh.HdrGuid = srd.HdrGuid  " + Environment.NewLine;
            sql += " and sdh.DateofIssue between '" + basisFrom + "' and '" + basisTo + "'  " + Environment.NewLine;
            sql += " and ( sdh.AgentNumericCode like '" + FBagt + "' ) " + Environment.NewLine;
            sql += " and DATENAME(dw,sdh.DateofIssue) like '" + FBwe + "' " + Environment.NewLine;
            sql += " join pax.SalesDocumentCoupon sdc on sdc.RelatedDocumentGuid = srd.RelatedDocumentGuid " + Environment.NewLine;
            sql += " left join ref.VW_Agent agt on agt.AgencyNumericCode = left(sdh.AgentNumericCode,7) " + Environment.NewLine;
            sql += " ) " + Environment.NewLine;

            sql += " select f1.DayOfTheWeek,f1.AgentNumericCode,f1.agentname,f1.POS,OriginAirportCityCode,DestinationAirportCityCode,FareBasisTicketDesignator from A f1 " + Environment.NewLine;
            sql += " where FareBasisTicketDesignator like  '" + farebasis + "' " + Environment.NewLine;
            sql += " group by f1.DayOfTheWeek,f1.AgentNumericCode,f1.agentname,f1.POS,OriginAirportCityCode,DestinationAirportCityCode,FareBasisTicketDesignator " + Environment.NewLine;

            SqlConnection con = new SqlConnection(pbConnectionString);

            DataSet ds = new DataSet();
            SqlDataAdapter ada = new SqlDataAdapter(sql, con);
            ada.Fill(ds);

            int i = 0;
            int ligne = ds.Tables[0].Rows.Count;
            int colone = ds.Tables[0].Columns.Count;
            string[,] Liste = new string[ligne, colone];


            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                for (int j = 0; j < ds.Tables[0].Columns.Count; j++)
                {

                    Liste[i,j] = dr[ds.Tables[0].Columns[j].ColumnName].ToString();
                }
                i++;
            }
            ViewBag.ligne = ligne;
            ViewBag.colone = colone;
            ViewBag.Liste = Liste;
            /*fin QueryFarebasis*/

            return PartialView();
        }
        public ActionResult onchangecboagt3_SelectedIndexChanged()
        {
            int cboagt3 = Request.Form["cboagt3"].AsInt();
            /*Test2*/
            SqlConnection cs = new SqlConnection(pbConnectionString);
            DataSet ds = new DataSet();
            string sql = "select LegalName,LocationAddress from Ref.VW_Agent where AgencyNumericCode =left('" + cboagt3 + "',7) ";


            SqlDataAdapter ada = new SqlDataAdapter(sql, cs);
            ada.Fill(ds);

            ViewBag.l = ds.Tables[0].Rows.Count;
            foreach (DataRow dr1 in ds.Tables[0].Rows)
            {
                ViewBag.agtname3 = dr1[ds.Tables[0].Columns[0].ColumnName].ToString();
                ViewBag.agtpos3 = dr1[ds.Tables[0].Columns[1].ColumnName].ToString();
            }
            /*fin Test2*/

            return PartialView();
        }
        public ActionResult onchangeAgentNum2()
        {
            string rbdFrom1 = Request["rbdFrom"];
            string rbdTo1 = Request["rbdTo"];
            DateTime rbdFrom = DateTime.Parse(rbdFrom1);
            DateTime rbdTo = DateTime.Parse(rbdTo1);
            string rbd = "%";

            SqlConnection con = new SqlConnection(pbConnectionString);

            string sql = " ";
            sql += " select distinct sdh.AgentNumericCode from Pax.SalesDocumentHeader sdh   " + Environment.NewLine;
            sql += " join pax.SalesRelatedDocumentInformation srd on sdh.HdrGuid = srd.HdrGuid   " + Environment.NewLine;
            sql += " join pax.SalesDocumentCoupon sdc on srd.RelatedDocumentGuid =sdc.RelatedDocumentGuid   " + Environment.NewLine;
            sql += " where DateofIssue between '" + rbdFrom + "' and  '" + rbdTo + "' and sdc.ReservationBookingDesignator like  '" + rbd + "'  " + Environment.NewLine;

            DataSet ds = new DataSet();
            SqlDataAdapter ada = new SqlDataAdapter(sql, con);
            ada.Fill(ds);
            con.Close();

            int loncboagt3 = ds.Tables[0].Rows.Count;

            string[,] Listecboagt3 = new string[1, loncboagt3];

            int i = 0;

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                Listecboagt3[0, i] = dr[ds.Tables[0].Columns[0].ColumnName].ToString();
                i++;
            }
            ViewBag.loncboagt3 = loncboagt3;
            ViewBag.Listecboagt3 = Listecboagt3;

            return PartialView();
        }
        public ActionResult frmcode2()
        {
            string code = Request["code"];
            int coden = Request.Form["code"].AsInt();

            string rbdFrom1 = Request["rbdFrom"];
            string rbdTo1 = Request["rbdTo"];
            DateTime rbdFrom = DateTime.Parse(rbdFrom1);
            DateTime rbdTo = DateTime.Parse(rbdTo1);
            string rbd = "%";

            SqlConnection con = new SqlConnection(pbConnectionString);

            string sql = " ";
            sql += " select distinct sdh.AgentNumericCode from Pax.SalesDocumentHeader sdh   " + Environment.NewLine;
            sql += " join pax.SalesRelatedDocumentInformation srd on sdh.HdrGuid = srd.HdrGuid   " + Environment.NewLine;
            sql += " join pax.SalesDocumentCoupon sdc on srd.RelatedDocumentGuid =sdc.RelatedDocumentGuid   " + Environment.NewLine;
            sql += " where DateofIssue between '" + rbdFrom + "' and  '" + rbdTo + "' and sdc.ReservationBookingDesignator like  '" + rbd + "'  " + Environment.NewLine;

            DataSet ds = new DataSet();
            SqlDataAdapter ada = new SqlDataAdapter(sql, con);
            ada.Fill(ds);
            con.Close();

            int loncboagt3 = ds.Tables[0].Rows.Count;

            string[,] Listecboagt3 = new string[1, loncboagt3];

            int i = 0;

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                Listecboagt3[0, i] = dr[ds.Tables[0].Columns[0].ColumnName].ToString();
                i++;
            }
            ViewBag.loncboagt3 = loncboagt3;
            ViewBag.Listecboagt3 = Listecboagt3;
            ViewBag.code = code;
            /*Test2*/
            SqlConnection cs1 = new SqlConnection(pbConnectionString);
            DataSet ds1 = new DataSet();
            string sql1 = "select LegalName,LocationAddress from Ref.VW_Agent where AgencyNumericCode =left('" + coden + "',7) ";


            SqlDataAdapter ada1 = new SqlDataAdapter(sql1, cs1);
            ada1.Fill(ds1);

            ViewBag.l = ds1.Tables[0].Rows.Count;
            foreach (DataRow dr1 in ds1.Tables[0].Rows)
            {
                ViewBag.agtname3 = dr1[ds1.Tables[0].Columns[0].ColumnName].ToString();
                ViewBag.agtpos3 = dr1[ds1.Tables[0].Columns[1].ColumnName].ToString();
            }
            /*fin Test2*/
            return PartialView();
        }
        public ActionResult onchangeResrBD()
        {
            string rbdFrom1 = Request["rbdFrom"];
            string rbdTo1 = Request["rbdTo"];
            DateTime rbdFrom = DateTime.Parse(rbdFrom1);
            DateTime rbdTo = DateTime.Parse(rbdTo1);

            SqlConnection con = new SqlConnection(pbConnectionString);
            string sql = " ";
            sql += " select distinct sdc.ReservationBookingDesignator from Pax.SalesDocumentHeader sdh   " + Environment.NewLine;
            sql += " join pax.SalesRelatedDocumentInformation srd on sdh.HdrGuid = srd.HdrGuid   " + Environment.NewLine;
            sql += " join pax.SalesDocumentCoupon sdc on srd.RelatedDocumentGuid =sdc.RelatedDocumentGuid   " + Environment.NewLine;
            sql += " where DateofIssue between '" + rbdFrom + "' and  '" + rbdTo + "'  " + Environment.NewLine;

            DataSet ds = new DataSet();
            SqlDataAdapter ada = new SqlDataAdapter(sql, con);
            ada.Fill(ds);

            int loncboRBD = ds.Tables[0].Rows.Count;

            string[,] ListecboRBD = new string[1, loncboRBD];

            int i = 0;

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                ListecboRBD[0, i] = dr[ds.Tables[0].Columns[0].ColumnName].ToString();
                i++;
            }
            ViewBag.loncboRBD = loncboRBD;
            ViewBag.ListecboRBD = ListecboRBD;

            return PartialView();
        }
        public ActionResult btnSearch3_Clickrbdpricing()
        {
            string cboagt3 = Request["cboagt3"];
            string cboRBD = Request["cboRBD"];
            string cboRBDwe = Request["cboRBDwe"];

            string rbdFrom1 = Request["rbdFrom"];
            string rbdTo1 = Request["rbdTo"];
            DateTime rbdFrom = DateTime.Parse(rbdFrom1);
            DateTime rbdTo = DateTime.Parse(rbdTo1);

            string rbdagt = "%";
            string rbdwe = "%";
            string rbd = "%";


            if (cboagt3 == "-All-")
            {
                rbdagt = "%";
            }
            else
            {
                rbdagt = cboagt3;
            }

            if (cboRBDwe == "-All-")
            {
                rbdwe = "%";
            }
            else
            {
                rbdwe = cboRBDwe;
            }
            if (cboRBD == "-All-")
            {
                rbd = "%";
            }
            else
            {
                rbd = cboRBD;
            }

            string sql = "";

            sql += " WITH A as ( " + Environment.NewLine;
            sql += " select sdh.DateofIssue ,DATENAME(dw,sdh.DateofIssue) as DayOfTheWeek,sdh.AgentNumericCode,isNull(agt.LegalName,'Missing Agent Info') as agentname,isNull(agt.LocationCountry,'Missing Agent Info') as  POS " + Environment.NewLine;
            sql += " ,sdh.PassengerName,sdh.FareCalculationModeIndicator " + Environment.NewLine;
            sql += " ,sdc.OriginAirportCityCode,sdc.DestinationAirportCityCode, sdh.TrueOriginDestinationCityCodes " + Environment.NewLine;
            sql += " ,sdc.FareBasisTicketDesignator,sdc.ReservationBookingDesignator " + Environment.NewLine;
            sql += " from Pax.SalesDocumentHeader sdh " + Environment.NewLine;
            sql += " join pax.SalesRelatedDocumentInformation srd on sdh.HdrGuid = srd.HdrGuid  " + Environment.NewLine;
            sql += " and sdh.DateofIssue between '" + rbdFrom + "' and '" + rbdTo + "'  " + Environment.NewLine;
            sql += " and ( sdh.AgentNumericCode like '" + rbdagt + "' ) " + Environment.NewLine;
            sql += " and DATENAME(dw,sdh.DateofIssue) like '" + rbdwe + "' " + Environment.NewLine;
            sql += " join pax.SalesDocumentCoupon sdc on sdc.RelatedDocumentGuid = srd.RelatedDocumentGuid " + Environment.NewLine;
            sql += " left join ref.VW_Agent agt on agt.AgencyNumericCode = left(sdh.AgentNumericCode,7) " + Environment.NewLine;
            sql += " ) " + Environment.NewLine;

            sql += " select f1.DayOfTheWeek,f1.AgentNumericCode,f1.agentname,f1.POS,OriginAirportCityCode,DestinationAirportCityCode,ReservationBookingDesignator from A f1 " + Environment.NewLine;
            sql += " where ReservationBookingDesignator like  '" + rbd + "' " + Environment.NewLine;
            sql += " group by f1.DayOfTheWeek,f1.AgentNumericCode,f1.agentname,f1.POS,OriginAirportCityCode,DestinationAirportCityCode,ReservationBookingDesignator " + Environment.NewLine;

            SqlConnection con = new SqlConnection(pbConnectionString);

            DataSet ds = new DataSet();
            SqlDataAdapter ada = new SqlDataAdapter(sql, con);
            ada.Fill(ds);

            int i = 0;
            int ligne = ds.Tables[0].Rows.Count;
            int colone = ds.Tables[0].Columns.Count;
            string[,] Liste = new string[ligne, colone];


            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                for (int j = 0; j < ds.Tables[0].Columns.Count; j++)
                {

                    Liste[i, j] = dr[ds.Tables[0].Columns[j].ColumnName].ToString();
                }
                i++;
            }
            ViewBag.ligne = ligne;
            ViewBag.colone = colone;

            ViewBag.dg3 = Liste;

            return PartialView();
        }
        public ActionResult onchangedatemarquepricing()
        {
            string marketFrom1 = Request["marketFrom"];
            string marketTo1 = Request["marketTo"];
            DateTime marketFrom = DateTime.Parse(marketFrom1);
            DateTime marketTo = DateTime.Parse(marketTo1);
            /*POSQuery*/
            SqlConnection con = new SqlConnection(pbConnectionString);
            string sql = " ";
            sql += " select distinct isnull(agt.LocationCountry,'Missing Agent Info') as POS from Pax.SalesDocumentHeader sdh   " + Environment.NewLine;
            sql += " join pax.SalesRelatedDocumentInformation srd on sdh.HdrGuid = srd.HdrGuid   " + Environment.NewLine;
            sql += " join pax.SalesDocumentCoupon sdc on srd.RelatedDocumentGuid =sdc.RelatedDocumentGuid   " + Environment.NewLine;
            sql += " left join ref.VW_Agent agt on agt.AgencyNumericCode = left(sdh.AgentNumericCode,7)  " + Environment.NewLine;
            sql += " where DateofIssue between '" + marketFrom + "' and  '" + marketTo + "' and agt.LocationCountry <> ''  " + Environment.NewLine;
            DataSet ds = new DataSet();
            SqlDataAdapter ada = new SqlDataAdapter(sql, con);
            ada.Fill(ds);
            con.Close();

            int loncboPoS = ds.Tables[0].Rows.Count;

            string[,] ListecboPoS = new string[1, loncboPoS];

            int i = 0;

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                ListecboPoS[0, i] = dr[ds.Tables[0].Columns[0].ColumnName].ToString();
                i++;
            }
            ViewBag.loncboPoS = loncboPoS;
            ViewBag.ListecboPoS = ListecboPoS;
            /*fin POSQuery*/
            /*FCMIQuery*/
            SqlConnection conFCMI = new SqlConnection(pbConnectionString);
            string sqlFCMI = " ";
            sqlFCMI += " select distinct sdh.FareCalculationModeIndicator from Pax.SalesDocumentHeader sdh   " + Environment.NewLine;
            sqlFCMI += " where sdh.DateofIssue between '" + marketFrom + "' and  '" + marketTo + "' and FareCalculationModeIndicator <> '' " + Environment.NewLine;
            DataSet dsFCMI = new DataSet();
            SqlDataAdapter adaFCMI = new SqlDataAdapter(sqlFCMI, conFCMI);
            adaFCMI.Fill(dsFCMI);
            conFCMI.Close();

            int lonFCMI = dsFCMI.Tables[0].Rows.Count;

            string[,] ListeFCMI = new string[1, lonFCMI];

            int ip = 0;

            foreach (DataRow drFCMI in dsFCMI.Tables[0].Rows)
            {
                ListeFCMI[0, ip] = drFCMI[dsFCMI.Tables[0].Columns[0].ColumnName].ToString();
                ip++;
            }
            ViewBag.lonFCMI = lonFCMI;
            ViewBag.ListeFCMI = ListeFCMI;
            /*fin FCMIQuery*/
            /*fopFarebasis*/
            SqlConnection concboFOPFarebasis = new SqlConnection(pbConnectionString);
            string sqlcboFOPFarebasis = " ";
            sqlcboFOPFarebasis += " select distinct sdc.FareBasisTicketDesignator from Pax.SalesDocumentHeader sdh   " + Environment.NewLine;
            sqlcboFOPFarebasis += " join pax.SalesRelatedDocumentInformation srd on sdh.HdrGuid = srd.HdrGuid   " + Environment.NewLine;
            sqlcboFOPFarebasis += " join pax.SalesDocumentCoupon sdc on srd.RelatedDocumentGuid =sdc.RelatedDocumentGuid   " + Environment.NewLine;
            sqlcboFOPFarebasis += " where DateofIssue between '" + marketFrom + "' and  '" + marketTo + "'  " + Environment.NewLine;
            DataSet dscboFOPFarebasis = new DataSet();
            SqlDataAdapter adacboFOPFarebasis = new SqlDataAdapter(sqlcboFOPFarebasis, concboFOPFarebasis);
            adacboFOPFarebasis.Fill(dscboFOPFarebasis);
            concboFOPFarebasis.Close();

            int loncboFOPFarebasis = dscboFOPFarebasis.Tables[0].Rows.Count;

            string[,] ListecboFOPFarebasis = new string[1, loncboFOPFarebasis];

            int icboFOPFarebasis = 0;

            foreach (DataRow drcboFOPFarebasis in dscboFOPFarebasis.Tables[0].Rows)
            {
                ListecboFOPFarebasis[0, icboFOPFarebasis] = drcboFOPFarebasis[dscboFOPFarebasis.Tables[0].Columns[0].ColumnName].ToString();
                icboFOPFarebasis++;
            }
            ViewBag.loncboFOPFarebasis = loncboFOPFarebasis;
            ViewBag.ListecboFOPFarebasis = ListecboFOPFarebasis;
            /*fin fopFarebasis*/
            /*FOP*/
            SqlConnection conFOP = new SqlConnection(pbConnectionString);
            string sqlFOP = " ";
            sqlFOP += " select distinct isnull(agt.LocationCountry,'Missing Agent Info') as POS from Pax.SalesDocumentHeader sdh   " + Environment.NewLine;
            sqlFOP += " join pax.SalesRelatedDocumentInformation srd on sdh.HdrGuid = srd.HdrGuid   " + Environment.NewLine;
            sqlFOP += " join pax.SalesDocumentCoupon sdc on srd.RelatedDocumentGuid =sdc.RelatedDocumentGuid   " + Environment.NewLine;
            sqlFOP += " left join ref.VW_Agent agt on agt.AgencyNumericCode = left(sdh.AgentNumericCode,7)  " + Environment.NewLine;
            sqlFOP += " where DateofIssue between '" + marketFrom + "' and  '" + marketTo + "' and agt.LocationCountry <> ''  " + Environment.NewLine;
            DataSet dsFOP = new DataSet();
            SqlDataAdapter adaFOP = new SqlDataAdapter(sqlFOP, conFOP);
            adaFOP.Fill(dsFOP);
            conFOP.Close();

            int lonFOP = dsFOP.Tables[0].Rows.Count;

            string[,] ListeFOP = new string[1, lonFOP];

            int iFOP = 0;

            foreach (DataRow drFOP in dsFOP.Tables[0].Rows)
            {
                ListeFOP[0, iFOP] = drFOP[dsFOP.Tables[0].Columns[0].ColumnName].ToString();
                iFOP++;
            }
            ViewBag.lonFOP = lonFOP;
            ViewBag.ListeFOP = ListeFOP;
            /*fin FOP*/
            return PartialView();
        }
        public ActionResult btnSearch4_Clickmarketpricing()
        {
            string marketFrom1 = Request["marketFrom"];
            string marketTo1 = Request["marketTo"];
            DateTime marketFrom = DateTime.Parse(marketFrom1);
            DateTime marketTo = DateTime.Parse(marketTo1);

            string cboPoS = Request["cboPoS"];
            string cboFCMI = Request["cboFCMI"];
            string cboFOPFarebasis = Request["cboFOPFarebasis"];
            string cboFOP = Request["FOP"];


            string _FOP = "%";
            string _POS = "%";
            string _FCMI = "%";
            string FOPFB = "%";

            if (cboFOP == "-All-")
            {
                _FOP = "%";
            }
            else
            {
                _FOP = cboFOP;
            }

            if (cboPoS == "-All-")
            {
                _POS = "%";
            }
            else
            {
                _POS = cboPoS;
            }
            if (cboFCMI == "-All-")
            {
                _FCMI = "%";
            }
            else
            {
                _FCMI = cboFCMI;
            }
            if (cboFOPFarebasis == "-All-")
            {
                FOPFB = "%";
            }
            else
            {
                FOPFB = cboFOPFarebasis;
            }

            string sql = "";

            sql += "  select sdh.DateofIssue ,DATENAME(dw,sdh.DateofIssue) as PurchaseDay,sdh.AgentNumericCode  " + Environment.NewLine;
            sql += " ,isnull(agt.LegalName,'Missing Agent Info') as agentname,isnull(agt.LocationCountry,'Missing Agent Info') as POS  " + Environment.NewLine;
            sql += " , sdc.RelatedDocumentNumber  " + Environment.NewLine;
            sql += " , sdc.CouponNumber  " + Environment.NewLine;
            sql += " ,sdh.FareCalculationModeIndicator  " + Environment.NewLine;
            sql += " ,sdc.OriginAirportCityCode,sdc.DestinationAirportCityCode  " + Environment.NewLine;
            sql += " ,sdc.FareBasisTicketDesignator,sdc.ReservationBookingDesignator,sdh.EndosRestriction  " + Environment.NewLine;
            sql += " ,sdc.FlightDepartureDate as TravelDate,DATENAME(dw,sdc.FlightDepartureDate) as TravelDay   " + Environment.NewLine;
            sql += " ,DATEDIFF(DAY, sdh.DateofIssue,(select FlightDepartureDate from pax.SalesDocumentCoupon cpn where srd.isconjunction=0 and cpn.RelatedDocumentGuid = sdc.RelatedDocumentGuid and  cpn.CouponNumber ='1') ) as [Ticketing & COT Lapse]  " + Environment.NewLine;
            sql += " ,sdp.FormofPaymentType ,sdp.RemittanceCurrency ,sdp.RemittanceAmount  " + Environment.NewLine;
            sql += " from Pax.SalesDocumentHeader sdh  " + Environment.NewLine;
            sql += " join pax.SalesRelatedDocumentInformation srd on sdh.HdrGuid = srd.HdrGuid and sdh.OwnTicket ='Y'  " + Environment.NewLine;
            sql += " join pax.SalesDocumentCoupon sdc on sdc.RelatedDocumentGuid = srd.RelatedDocumentGuid  " + Environment.NewLine;
            sql += " left join ref.VW_Agent agt on agt.AgencyNumericCode = left(sdh.AgentNumericCode,7)  " + Environment.NewLine;
            sql += " left join Pax.SalesDocumentPayment sdp on srd.RelatedDocumentGuid = sdp.RelatedDocumentGuid and sdh.DateofIssue = sdp.DateofIssue and sdh.TransactionCode = sdp.TransactionCode   " + Environment.NewLine;
            sql += " where sdh.DateofIssue between '" + marketFrom + "' and  '" + marketTo + "' and sdp.FormofPaymentType like  '" + _FOP + "' " + Environment.NewLine;
            sql += " and isnull(agt.LocationCountry,'Missing Agent Info') like  '" + _POS + "' and sdh.FareCalculationModeIndicator like '" + _FCMI + "' and sdc.FareBasisTicketDesignator like '" + FOPFB + "'  " + Environment.NewLine;
            SqlConnection cs = new SqlConnection(pbConnectionString);
            DataSet ds = new DataSet();
            SqlDataAdapter ada = new SqlDataAdapter(sql, cs);
            ada.Fill(ds);
            int ligne = ds.Tables[0].Rows.Count;
            int colone = ds.Tables[0].Columns.Count;
            string[,] Liste = new string[ligne, colone];
            int i = 0;
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                for (int j = 0; j < ds.Tables[0].Columns.Count; j++)
                {
                    if ((j == 0) || (j == 13))
                    {
                        string h = null;

                        h = dr[ds.Tables[0].Columns[j].ColumnName].ToString();

                        if (h != "")
                        {
                            Liste[i,j] = DateTime.Parse(h).ToString("dd-MMM-yyyy");
                        }
                    }
                    else
                    {
                        Liste[i, j] = dr[ds.Tables[0].Columns[j].ColumnName].ToString();
                    }
                }
                i++;
            }
            ViewBag.ligne = ligne;
            ViewBag.colone = colone;
            ViewBag.Liste = Liste;
            return PartialView();
        }
        public ActionResult AgentNum5pricing()
        {
            string fareCompFrom1 = Request["fareCompFrom"];
            string fareCompTo1 = Request["fareCompTo"];
            DateTime fareCompFrom = DateTime.Parse(fareCompFrom1);
            DateTime fareCompTo = DateTime.Parse(fareCompTo1);
            string rbd = "%";


            /*AgentNum5*/
            SqlConnection con = new SqlConnection(pbConnectionString);
            string sql = " ";
            sql += " select distinct sdh.AgentNumericCode from Pax.SalesDocumentHeader sdh   " + Environment.NewLine;
            sql += " join pax.SalesRelatedDocumentInformation srd on sdh.HdrGuid = srd.HdrGuid   " + Environment.NewLine;
            sql += " join pax.SalesDocumentCoupon sdc on srd.RelatedDocumentGuid =sdc.RelatedDocumentGuid   " + Environment.NewLine;
            sql += " where DateofIssue between '" + fareCompFrom + "' and  '" + fareCompTo + "' and sdc.ReservationBookingDesignator like  '" + rbd + "'  " + Environment.NewLine;
            DataSet ds = new DataSet();
            SqlDataAdapter ada = new SqlDataAdapter(sql, con);
            ada.Fill(ds);
            con.Close();

            int loncboagt5 = ds.Tables[0].Rows.Count;

            string[,] Listecboagt5 = new string[1, loncboagt5];

            int i = 0;

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                Listecboagt5[0, i] = dr[ds.Tables[0].Columns[0].ColumnName].ToString();
                i++;
            }
            ViewBag.loncboagt5 = loncboagt5;
            ViewBag.Listecboagt5 = Listecboagt5;
            /*fin AgentNum5*/

            return PartialView();
        }
        public ActionResult frmcode4()
        {
            string code = Request["code"];
            int coden = Request.Form["code"].AsInt();

            string fareCompFrom1 = Request["fareCompFrom"];
            string fareCompTo1 = Request["fareCompTo"];
            DateTime fareCompFrom = DateTime.Parse(fareCompFrom1);
            DateTime fareCompTo = DateTime.Parse(fareCompTo1);
            string rbd = "%";

            SqlConnection con = new SqlConnection(pbConnectionString);

            string sql = " ";
            sql += " select distinct sdh.AgentNumericCode from Pax.SalesDocumentHeader sdh   " + Environment.NewLine;
            sql += " join pax.SalesRelatedDocumentInformation srd on sdh.HdrGuid = srd.HdrGuid   " + Environment.NewLine;
            sql += " join pax.SalesDocumentCoupon sdc on srd.RelatedDocumentGuid =sdc.RelatedDocumentGuid   " + Environment.NewLine;
            sql += " where DateofIssue between '" + fareCompFrom + "' and  '" + fareCompTo + "' and sdc.ReservationBookingDesignator like  '" + rbd + "'  " + Environment.NewLine;

            DataSet ds = new DataSet();
            SqlDataAdapter ada = new SqlDataAdapter(sql, con);
            ada.Fill(ds);
            con.Close();

            int loncboagt5 = ds.Tables[0].Rows.Count;

            string[,] Listecboagt5 = new string[1, loncboagt5];

            int i = 0;

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                Listecboagt5[0, i] = dr[ds.Tables[0].Columns[0].ColumnName].ToString();
                i++;
            }
            ViewBag.loncboagt5 = loncboagt5;
            ViewBag.Listecboagt5 = Listecboagt5;
            ViewBag.code = code;
            /*Test5*/
            SqlConnection cs1 = new SqlConnection(pbConnectionString);
            DataSet ds1 = new DataSet();
            string sql1 = "select LegalName,LocationAddress from Ref.VW_Agent where AgencyNumericCode =left('" + coden + "',7) ";


            SqlDataAdapter ada1 = new SqlDataAdapter(sql1, cs1);
            ada1.Fill(ds1);

            ViewBag.l = ds1.Tables[0].Rows.Count;
            foreach (DataRow dr1 in ds1.Tables[0].Rows)
            {
                ViewBag.agtname4 = dr1[ds1.Tables[0].Columns[0].ColumnName].ToString();
                ViewBag.agtpos4 = dr1[ds1.Tables[0].Columns[1].ColumnName].ToString();
            }
            /*fin Test5*/
            return PartialView();
        }
        public ActionResult Test5farecomp()
        {
            string cboagt5 = Request["cboagt5"];

            SqlConnection cs1 = new SqlConnection(pbConnectionString);
            DataSet ds1 = new DataSet();
            string sql1 = "select LegalName,LocationAddress from Ref.VW_Agent where AgencyNumericCode =left('" + cboagt5 + "',7) ";


            SqlDataAdapter ada1 = new SqlDataAdapter(sql1, cs1);
            ada1.Fill(ds1);


            ViewBag.l = ds1.Tables[0].Rows.Count;
            foreach (DataRow dr1 in ds1.Tables[0].Rows)
            {
                ViewBag.agtname4 = dr1[ds1.Tables[0].Columns[0].ColumnName].ToString();
                ViewBag.agtpos4 = dr1[ds1.Tables[0].Columns[1].ColumnName].ToString();
            }
            
            return PartialView();
        }
        public ActionResult btnseach_Clickmfarecomppricing()
        {
            string cboagt5 = Request["cboagt5"];

            string fareCompFrom1 = Request["fareCompFrom"];
            string fareCompTo1 = Request["fareCompTo"];
            DateTime fareCompFrom = DateTime.Parse(fareCompFrom1);
            DateTime fareCompTo = DateTime.Parse(fareCompTo1);

            string agt5 = "%";

            if (cboagt5 == "-All-")
            {
                agt5 = "%";
            }
            else
            {
                agt5 = cboagt5;
            }
            string sql = "";
            sql += "  IF OBJECT_ID('tempdb..#base') IS NOT NULL DROP TABLE #base   " + Environment.NewLine;
            sql += "  select top 100 PERCENT ROW_NUMBER() OVER(ORDER BY sdc.DocumentNumber, f4.FareComponent) AS ROWNUM,   " + Environment.NewLine;
            sql += "  sdh.DateofIssue,sdc.DocumentNumber,sdc.RelatedDocumentNumber, ROW_NUMBER() OVER (PARTITION BY SDC.DocumentNumber ORDER BY SDC.RelatedDocumentNumber, SDC.CouponNumber) AS SEGMENTS,   " + Environment.NewLine;
            sql += "  sdc.OriginAirportCityCode,DestinationAirportCityCode,   " + Environment.NewLine;
            sql += "  sdh.AgentNumericCode,agt.LegalName as agentname,agt.LocationCountry as POS,sdh.EndosRestriction,sdh.FareCalculationArea   " + Environment.NewLine;
            sql += "  ,f4.FareComponent,f5.AtbpFare ,f5.FareInNuc   " + Environment.NewLine;
            sql += "  into #base   " + Environment.NewLine;
            sql += "  from Pax.salesdocumentheader sdh   " + Environment.NewLine;
            sql += "  join pax.SalesRelatedDocumentInformation srd on sdh.HdrGuid = srd.HdrGuid    " + Environment.NewLine;
            sql += "  and sdh.DateofIssue between '" + fareCompFrom + "' and '" + fareCompTo + "'    " + Environment.NewLine;
            sql += "  and ( sdh.AgentNumericCode like '" + agt5 + "' )   " + Environment.NewLine;
            sql += "  join pax.SalesDocumentCoupon sdc on srd.RelatedDocumentGuid = sdc.RelatedDocumentGuid    " + Environment.NewLine;
            sql += "  left join Pax.ProrationDetail f4 on sdc.RelatedDocumentGuid = f4.RelatedDocumentGuid and sdc.CouponNumber = f4.CouponNumber and sdc.CouponStatus = f4.ProrationFlag    " + Environment.NewLine;
            sql += "  left join Proration.ProrateSteps f5 on f5.HdrGuid =srd.HdrGuid and f4.FareComponent = f5.Fctbp    " + Environment.NewLine;
            sql += "  left join ref.VW_Agent agt on agt.AgencyNumericCode = left(sdh.AgentNumericCode,7)   " + Environment.NewLine;
            sql += "  --where f5.AtbpFare is not null   " + Environment.NewLine;
            sql += "  order by RelatedDocumentNumber,FareComponent     " + Environment.NewLine;
            sql += "  ;   " + Environment.NewLine;
            sql += "  select f1.DateofIssue,f1.DocumentNumber,f1.RelatedDocumentNumber,f2.OriginAirportCityCode,f3.DestinationAirportCityCode   " + Environment.NewLine;
            sql += "  ,f1.AgentNumericCode,isNull(f1.agentname,'Missing Agent Info')as AgentName,isnull(f1.POS,'Missing Agent Info') as POS,EndosRestriction,FareComponent   " + Environment.NewLine;
            sql += "  ,isnull(AtbpFare,'Missing Info')  as [ATBP Fare]   " + Environment.NewLine;
            sql += "  --,f1.FareCalculationArea   " + Environment.NewLine;
            sql += "  from #base f1   " + Environment.NewLine;
            sql += "  cross apply   " + Environment.NewLine;
            sql += "      (   " + Environment.NewLine;
            sql += "          select top 1 OriginAirportCityCode from #base O where f1.DocumentNumber = O.DocumentNumber and f1.FareComponent = O.FareComponent   " + Environment.NewLine;
            sql += "          order by SEGMENTS ASC   " + Environment.NewLine;
            sql += "      ) f2    " + Environment.NewLine;
            sql += "      cross apply    " + Environment.NewLine;
            sql += "      (   " + Environment.NewLine;
            sql += "          select top 1 DestinationAirportCityCode from #base O where f1.DocumentNumber = O.DocumentNumber and f1.FareComponent = O.FareComponent   " + Environment.NewLine;
            sql += "          order by SEGMENTS desc   " + Environment.NewLine;
            sql += "      ) f3   " + Environment.NewLine;
            sql += "   group by   " + Environment.NewLine;
            sql += "  f1.DateofIssue,f1.DocumentNumber,f1.RelatedDocumentNumber,f2.OriginAirportCityCode,f3.DestinationAirportCityCode   " + Environment.NewLine;
            sql += "  ,f1.AgentNumericCode,f1.agentname,f1.POS,EndosRestriction,FareComponent,AtbpFare --,f1.FareCalculationArea   " + Environment.NewLine;
            sql += "  order by DocumentNumber,f1.RelatedDocumentNumber,FareComponent   " + Environment.NewLine;

            SqlConnection cs = new SqlConnection(pbConnectionString);
            DataSet ds = new DataSet();
            SqlDataAdapter ada = new SqlDataAdapter(sql, cs);
            ada.Fill(ds);
            int i = 0;

            int ligne = ds.Tables[0].Rows.Count;
            int colone = ds.Tables[0].Columns.Count;
            string[,] Liste = new string[ligne, colone];

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                for (int j = 0; j < ds.Tables[0].Columns.Count; j++)
                {
                    if ((j == 0))
                    {
                        string h = null;
                        h = dr[ds.Tables[0].Columns[j].ColumnName].ToString();
                        if (h != "")
                        {
                            Liste[i, j] = DateTime.Parse(h).ToString("dd-MMM-yyyy");
                        }
                    }
                    else
                    {
                        Liste[i, j] = dr[ds.Tables[0].Columns[j].ColumnName].ToString();
                    }
                }
                i++;
            }

            ViewBag.ligne = ligne;
            ViewBag.colone = colone;

            ViewBag.Liste = Liste;

            return PartialView();
        }
        public ActionResult btnSearch_Clickcoupon()
        {
            string couponUsageFrom1 = Request["couponUsageFrom"];
            string couponUsageTo1 = Request["couponUsageTo"];
            DateTime couponUsageFrom = DateTime.Parse(couponUsageFrom1);
            DateTime couponUsageTo = DateTime.Parse(couponUsageTo1);

            string dtpFromChecked = Request["dtpFromChecked"];
            string dtpToChecked = Request["dtpToChecked"];

            SqlConnection cs = new SqlConnection(pbConnectionString);

            string s = "";
            s = sql();

            if (dtpFromChecked == "true")
            {
                s = s + "AND sdh.DateofIssue BETWEEN Convert(date, '" + couponUsageFrom + "', 105) AND Convert(date, '" + couponUsageTo + "', 105)" + Environment.NewLine;
            }
            s = s + "order by FinalShare asc" + Environment.NewLine;

            DataSet ds = new DataSet();
            SqlDataAdapter ada = new SqlDataAdapter(s, cs);
            ada.Fill(ds);

            int i = 0;

            int ligne = ds.Tables[0].Rows.Count;
            int colone = ds.Tables[0].Columns.Count;
            string[,] Liste = new string[ligne, colone];

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                for (int j = 0; j < ds.Tables[0].Columns.Count; j++)
                {
                    if (j == 0)
                    {
                        try
                        {
                            Liste[i, j] = DateTime.Parse(dr[ds.Tables[0].Columns[j].ColumnName].ToString()).ToShortDateString();
                        }
                        catch
                        {
                        }
                    }
                    else
                    {
                        Liste[i, j] = dr[ds.Tables[0].Columns[j].ColumnName].ToString();
                    }
                }
                i++;
            }
            ViewBag.ligne = ligne;
            ViewBag.colone = colone;
            ViewBag.Liste = Liste;
            return PartialView();
        }
        private string sql()
        {
            string sql = "";
            sql = " select sdh.DateofIssue,sdh.AgentNumericCode,sdh.DocumentNumber as TicketNo,sdc.RelatedDocumentNumber as RelatedNo" + Environment.NewLine;
            sql = sql + ",sdc.CouponNumber as CPN,sdc.OriginAirportCityCode,sdc.DestinationAirportCityCode,sdc.ReservationBookingDesignator as RBD" + Environment.NewLine;
            sql = sql + ",f2.PrimeCode,pd.FinalShare,f1.MinSectorVal,f1.MaxSectorVal" + Environment.NewLine;
            sql = sql + "from Pax.SalesDocumentHeader sdh " + Environment.NewLine;
            sql = sql + "join pax.SalesRelatedDocumentInformation srd on sdh.HdrGuid = srd.HdrGuid" + Environment.NewLine;
            sql = sql + "join Pax.SalesDocumentCoupon sdc on srd.RelatedDocumentGuid = sdc.RelatedDocumentGuid" + Environment.NewLine;
            sql = sql + "join pax.ProrationDetail pd on sdc.RelatedDocumentGuid = pd.RelatedDocumentGuid and sdc.CouponNumber = pd.CouponNumber and sdc.CouponStatus = pd.ProrationFlag" + Environment.NewLine;
            sql = sql + "join pax.Farebasis  f2 on f2.FareBasisTicketDesignator = sdc.FareBasisTicketDesignator" + Environment.NewLine;
            sql = sql + "join Ref.FareControl f1  on sdc.OriginCity = f1.OrigCity  and sdc.DestinationCity = f1.DestCity and f1.PrimeCode = f2.PrimeCode" + Environment.NewLine;
            sql = sql + "and sdc.ReservationBookingDesignator = f1.RBD where 1 = 1 " + Environment.NewLine;
            return sql;
        }
        public ActionResult btnCCMSearch_Click()
        {
            string creditCardFrom1 = Request["creditCardFrom"];
            string creditCardTo1 = Request["creditCardTo"];
            DateTime creditCardFrom = DateTime.Parse(creditCardFrom1);
            DateTime creditCardTo = DateTime.Parse(creditCardTo1);
            string sql = " select f1.DateofIssue,f1.DocumentNumber, f1.PassengerName , f1.TransactionCode, f2.FormofPaymentType, f2.AccountNumber ";
            sql = sql + " , f2.CardVerificationValueResult , f2.CreditCardCorporateContract, f2.ExpiryDate, F2.Currency, F2.Amount, f1.AmountCollectedCurrency, f1.AmountCollected ";
            sql = sql + " ,f1.FareCurrency, isnull(f1.AmountCollected,0) - isnull(f1.TaxCollected,0) - isnull(f1.SurchargeCollected,0) as FareCollected, f1.TaxCollected ";
            sql = sql + " , f1.SurchargeCollected from Pax.SalesDocumentHeader f1 join Pax.SalesDocumentPayment f2 on f1.HdrGuid = f2.HdrGuidRef and f2.FormofPaymentType not in ('EX','CA') and Amount <> 0 ";
            sql = sql + " where f1.DateofIssue between '" + creditCardFrom + "'and '" + creditCardTo + "' ";
            SqlConnection cs = new SqlConnection(pbConnectionString);

            DataSet ds = new DataSet();
            SqlDataAdapter ada = new SqlDataAdapter(sql, cs);
            ada.Fill(ds);
            int ligne = ds.Tables[0].Rows.Count;
            int colone = ds.Tables[0].Columns.Count;
            string[,] Liste = new string[ligne, colone];
            int i = 0;

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                for (int y = 0; y < ds.Tables[0].Columns.Count; y++)
                {
                    if (y == 0)
                    {
                        string h = null;
                        string hh = null;

                        h = dr[ds.Tables[0].Columns[y].ColumnName].ToString();
                        if (h != "")
                        {
                            hh = h.Substring(0, 11).ToString();
                            Liste[i,y] = hh;
                        }
                    }
                    else
                    {
                        Liste[i,y] = dr[ds.Tables[0].Columns[y].ColumnName].ToString();
                    }
                }
                i++;
            }

            ViewBag.ligne = ligne;
            ViewBag.colone = colone;
            ViewBag.Liste = Liste;

            return PartialView();
    }
        public ActionResult btnSearch_ClickManualTicketEntry()
        {
           // int _xtcdoc = Request.Form["_xtcdoc"].AsInt();
            string _xtcdoc = Request["_xtcdoc"].Trim(); 
            string txtIssuedInExchangeFor = Request["txtIssuedInExchangeFor"].Trim(); 

            /*QueryHeader 0*/
            #region QueryHeader
            string sql = "";
            sql += "SELECT left(DocumentNumber,3) As Arl,right(DocumentNumber,10) as Doc,CheckDigit ,TrueOriginDestinationCityCodes ,FareCalculationArea ,FareCurrency " + Environment.NewLine;
            sql += ",Fare,EndosRestriction ,BookingReference ,PassengerName,DateofIssue ,AgentNumericCode ,BookingAgentIdentification,TourCode,FareCalculationModeIndicator   " + Environment.NewLine;
            sql += ",TotalCurrency,EquivalentFare ,OriginalIssueDocumentNumber,TotalAmount,VendorIdentification   ,transactioncode,ReportingSystemIdentifier ,TicketingAgentID " + Environment.NewLine;
            sql += "FROM PAX.[SalesDocumentHeader] where DocumentNumber = '" + _xtcdoc + "' and TransactionCode = 'TKTM' " + Environment.NewLine;

            SqlConnection con = new SqlConnection(pbConnectionString);

            DataSet ds = new DataSet();
            SqlDataAdapter ada = new SqlDataAdapter(sql, con);
            ada.Fill(ds);

            int ligne = ds.Tables[0].Rows.Count;


            if (ligne > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    ViewBag.txtDocumentCarrier = dr[ds.Tables[0].Columns[0].ColumnName].ToString();
                    ViewBag.txtDocumentNumber = dr[ds.Tables[0].Columns[1].ColumnName].ToString(); 
                    ViewBag.txtChkDgt = dr[ds.Tables[0].Columns[2].ColumnName].ToString();
                    ViewBag.txtOrgDest = dr[ds.Tables[0].Columns[3].ColumnName].ToString();
                    ViewBag.txtFareComponent = dr[ds.Tables[0].Columns[4].ColumnName].ToString();
                    ViewBag.txtFareCurrency = dr[ds.Tables[0].Columns[5].ColumnName].ToString();
                    ViewBag.txtFareAmount = dr[ds.Tables[0].Columns[6].ColumnName].ToString();
                    ViewBag.txtEndorsementArea = dr[ds.Tables[0].Columns[7].ColumnName].ToString();
                    ViewBag.txtPNR2 = dr[ds.Tables[0].Columns[8].ColumnName].ToString();
                    ViewBag.txtPassengerNam = dr[ds.Tables[0].Columns[9].ColumnName].ToString();
                    ViewBag.txtCustomerCode = dr[ds.Tables[0].Columns[11].ColumnName].ToString();
                    ViewBag.txtBookingAgentID = dr[ds.Tables[0].Columns[12].ColumnName].ToString();
                    ViewBag.FromManuelTicketScreen = dr[ds.Tables[0].Columns[10].ColumnName].ToString();
                    ViewBag.txtIssuedInExchangeFor = dr[ds.Tables[0].Columns[17].ColumnName].ToString();
                    // txtCPUI.Text = "FVVV";
                    ViewBag.txtTourCode = dr[ds.Tables[0].Columns[13].ColumnName].ToString();
                    ViewBag.txtFcmi = dr[ds.Tables[0].Columns[14].ColumnName].ToString();
                    ViewBag.txtFarePaidCur = dr[ds.Tables[0].Columns[15].ColumnName].ToString();
                    ViewBag.txtFarePaidAmt = dr[ds.Tables[0].Columns[16].ColumnName].ToString();
                    ViewBag.txtVendorIdentifier = dr[ds.Tables[0].Columns[19].ColumnName].ToString();
                    ViewBag.txtTotalamtcur = dr[ds.Tables[0].Columns[15].ColumnName].ToString();
                    ViewBag.txtTotalamt = dr[ds.Tables[0].Columns[18].ColumnName].ToString();
                    ViewBag.txtReportingSystemIdentifier = dr[ds.Tables[0].Columns[21].ColumnName].ToString();
                    ViewBag.txttransactioncode = dr[ds.Tables[0].Columns[20].ColumnName].ToString();
                    ViewBag.txtTicketingAgentId = dr[ds.Tables[0].Columns[22].ColumnName].ToString();
                }
            }
            con.Close();
            #endregion
            /*fin QueryHeader 0*/

            /*QueryCoupon 1*/
            #region QueryCoupon
            string sql1 = "SELECT CouponNumber ,StopOverCode ,OriginAirportCityCode+'-'+DestinationAirportCityCode as Sector,Carrier,FlightNumber  " + Environment.NewLine;
            sql1 += ",ReservationBookingDesignator ,FlightDepartureDate ,[FlightDepartureTime]  " + Environment.NewLine;
            sql1 += ",CouponStatus,FareBasisTicketDesignator,NotValidBefore,NotValidAfter,FreeBaggageAllowance,FlightBookingStatus,UsedClassofService  " + Environment.NewLine;
            sql1 += ",UsageOriginCode+'-'+UsageDestinationCode,UsageAirline,UsageFlightNumber,UsageDate,FrequentFlyerReference  " + Environment.NewLine;
            sql1 += "FROM PAX.SalesDocumentCoupon where DocumentNumber =  '" + _xtcdoc + "'  " + Environment.NewLine;
            SqlConnection con1 = new SqlConnection(pbConnectionString);
            DataSet ds1 = new DataSet();
            SqlDataAdapter ada1 = new SqlDataAdapter(sql1, con1);
            ada1.Fill(ds1);
            int ligne1 = ds1.Tables[0].Rows.Count;
            int colone1 = ds1.Tables[0].Columns.Count;
            string[,] Liste = new string[colone1,ligne1];
            int t = 0;
            foreach (DataRow dr1 in ds1.Tables[0].Rows)
            {
                for (int y = 0; y < ds1.Tables[0].Columns.Count; y++)
                {
                    if ((y == 6) || (y == 10) || (y == 11) || (y == 18))
                    {
                        string h = dr1[ds1.Tables[0].Columns[y].ColumnName].ToString();
                        if (h != "")
                        {
                            Liste[y, t] = DateTime.Parse(dr1[ds1.Tables[0].Columns[y].ColumnName].ToString()).ToShortDateString();
                        }
                    }
                    else
                    {
                        Liste[y, t] = dr1[ds1.Tables[0].Columns[y].ColumnName].ToString();
                    }
                }
                t++;
            }
            ViewBag.ligne1 = ligne1;
            ViewBag.colone1 = colone1;
            ViewBag.Liste = Liste;
            #endregion
            /*fin QueryCoupon 1*/

            /*QueryPayment 2*/
            #region QueryPayment
            string sql2 = "SELECT Amount,FormofPaymentType,RemittanceAmount FROM PAX.SalesDocumentPayment where DocumentNumber =  '" + _xtcdoc + "'  and TransactionCode = 'TKTM' AND FormofPaymentType <> 'EX' " + Environment.NewLine;
            SqlConnection con2 = new SqlConnection(pbConnectionString);
            DataSet ds2 = new DataSet();
            SqlDataAdapter ada2 = new SqlDataAdapter(sql2, con2);
            ada2.Fill(ds2);
            int t2 = 0;

            int ligne2 = ds2.Tables[0].Rows.Count;
            int colone2 = ds2.Tables[0].Columns.Count;
            string[,] Liste2 = new string[colone2, ligne2];

            foreach (DataRow dr2 in ds2.Tables[0].Rows)
            {
                for (int y = 0; y < ds2.Tables[0].Columns.Count; y++)
                {
                    Liste2[y, t2] = dr2[ds2.Tables[0].Columns[y].ColumnName].ToString();
                }
                t2++;
            }
            ViewBag.ligne2 = ligne2;
            ViewBag.colone2 = colone2;
            ViewBag.Liste2 = Liste2;
            #endregion
            /*fin QueryPayment 2*/

            /*QueryOtherAmount 3*/
            #region QueryOtherAmount
            string sql3 = "SELECT case when f2.DocumentAmountType = 'Tax' Then 'Tax,Surcharges'when f2.DocumentAmountType = 'Surcharges' Then 'Tax,Surcharges' " + Environment.NewLine;
            sql3 += "when f2.DocumentAmountType = 'Commission3' Then 'Commission3'when f2.DocumentAmountType = 'TaxCommission1' Then 'TaxCommission1' END as DocumentAmountType " + Environment.NewLine;
            //sql += ",OtherAmountCode,OtherAmount,OtherAmountRate,FileSequence FROM PAX.SalesDocumentOtherAmount where DocumentNumber =  '" + _xtcdoc.Text + "' and TransactionCode = 'TKTM' " + Environment.NewLine;
            sql3 += ",f2.OtherAmountCode,f2.OtherAmount,f2.OtherAmountRate,f2.FileSequence from Pax.SalesRelatedDocumentInformation f1 join pax.SalesDocumentOtherAmount f2 on f1.RelatedDocumentGuid = f2.RelatedDocumentGuid and f1.DocumentNumber = '" + _xtcdoc + "' and f1.TransactionCode = 'TKTM' " + Environment.NewLine;

            SqlConnection con3 = new SqlConnection(pbConnectionString);

            DataSet ds3 = new DataSet();
            SqlDataAdapter ada3 = new SqlDataAdapter(sql3, con3);
            ada3.Fill(ds3);

            int ligne3 = ds3.Tables[0].Rows.Count;
            int colone3 = ds3.Tables[0].Columns.Count;
            string[,] Liste3 = new string[colone3, ligne3];
            int t3 = 0;

            foreach (DataRow dr3 in ds3.Tables[0].Rows)
            {
                for (int y = 0; y < ds3.Tables[0].Columns.Count; y++)
                {
                    Liste3[y, t3] = dr3[ds3.Tables[0].Columns[y].ColumnName].ToString();
                }
                t3++;
            }
            ViewBag.ligne3 = ligne3;
            ViewBag.colone3 = colone3;
            ViewBag.Liste3 = Liste3;
            #endregion
            /*fin QueryOtherAmount 3*/

            /*QueryAccountNo 4*/
            #region QueryAccountNo
            string sql4 = "select left(f2.[AccountNumber],13) as Doc  from Pax.SalesRelatedDocumentInformation f1 join pax.SalesDocumentPayment f2 on f1.RelatedDocumentGuid = f2.RelatedDocumentGuid and f1.DocumentNumber =  '" + _xtcdoc + "' and f1.TransactionCode = 'TKTM' and [AccountNumber] is not null " + Environment.NewLine;

            SqlConnection con4 = new SqlConnection(pbConnectionString);

            DataSet ds4 = new DataSet();
            SqlDataAdapter ada4 = new SqlDataAdapter(sql4, con4);
            ada4.Fill(ds4);

            int ligne4 = ds4.Tables[0].Rows.Count;
            int colone4 = ds4.Tables[0].Columns.Count;
            string[,] Liste4 = new string[colone4, ligne4];

            int t4 = 0;
            ViewBag.ligne = ligne4;
            foreach (DataRow dr4 in ds4.Tables[0].Rows)
            {
                for (int y = 0; y < ds4.Tables[0].Columns.Count; y++)
                {
                    try
                    {
                        Liste4[y, t4] = dr4[ds4.Tables[0].Columns[y].ColumnName].ToString();
                    }
                    catch
                    { }
                }
                t4++;
                if (Liste4[0, 0] == null || string.IsNullOrWhiteSpace(Liste4[0, 0]) || Liste4[0, 0] == "")
                {
                    Liste4[0, 0] = txtIssuedInExchangeFor;
                }
            }
            ViewBag.ligne4 = ligne4;
            ViewBag.colone4 = colone4;
            ViewBag.Liste4 = Liste4;
            #endregion
            /*fin QueryAccountNo 4*/
            return PartialView();
        }
        public ActionResult clearall()
        {
            return PartialView();
        }
        public void TRUNCATE()
        {
            string sql = "TRUNCATE TABLE [Tmp].[SalesDocumentHeader] " + Environment.NewLine;
            sql += "TRUNCATE TABLE [Tmp].[SalesDocumentCoupon]  " + Environment.NewLine;
            sql += "TRUNCATE TABLE [Tmp].[SalesRelatedDocumentInformation]  " + Environment.NewLine;
            sql += "TRUNCATE TABLE [Tmp].[SalesDocumentOtherAmount]  " + Environment.NewLine;
            sql += "TRUNCATE TABLE [Tmp].[SalesDocumentPayment]  " + Environment.NewLine;

            SqlConnection cs = new SqlConnection(pbConnectionString);


            //SqlCommand cmd = new SqlCommand(sql, cs);
            cs.Open();
            SqlCommand cmds = new SqlCommand(sql, cs);
            cmds.CommandType = CommandType.Text;
            cmds.CommandText = sql;
            cmds.Connection = cs;
            cmds.ExecuteNonQuery();

            cs.Close();
        }
        public void DELETEditFrmTicketDiscrepancy()
        {
            string _xtcdoc = Request["_xtcdoc"];
            string sql = " DELETE FROM [Pax].SalesDocumentPayment where DocumentNumber = '" + _xtcdoc + "' and TransactionCode = 'TKTM' " + Environment.NewLine;
            sql += "DELETE FROM [Pax].SalesDocumentOtherAmount where DocumentNumber = '" + _xtcdoc + "' and TransactionCode = 'TKTM' " + Environment.NewLine;
            sql += "DELETE FROM [Pax].SalesDocumentCoupon where DocumentNumber = '" + _xtcdoc + "'  " + Environment.NewLine;
            sql += "DELETE FROM [Pax].SalesRelatedDocumentInformation where DocumentNumber = '" + _xtcdoc + "' and TransactionCode = 'TKTM' " + Environment.NewLine;
            sql += "DELETE FROM [Pax].[SalesDocumentHeader] where DocumentNumber = '" + _xtcdoc + "' and TransactionCode = 'TKTM' " + Environment.NewLine;

            SqlConnection cs = new SqlConnection(pbConnectionString);

            cs.Open();
            SqlCommand cmds = new SqlCommand(sql, cs);
            cmds.CommandType = CommandType.Text;
            cmds.CommandText = sql;
            cmds.Connection = cs;
            cmds.ExecuteNonQuery();

            cs.Close();
        }
        public void ManualHeader()
        {
            string DocNum = Request["DocNum"];
            string txtDocumentCarrier = Request["txtDocumentCarrier"].Trim(); 
            string txtDocumentNumber = Request["txtDocumentNumber"].Trim(); 
            string _Doc = Request["_Doc"].Trim(); 
            string txtChkDgt = Request["txtChkDgt"].Trim(); 
            string txtOrgDest = Request["txtOrgDest"].Trim(); 
            string txtFareComponent = Request["txtFareComponent"].Trim();

            string txtFareCurrency = Request["txtFareCurrency"].Trim(); 
            string txtFareAmountn = Request["txtFareAmount"].Trim(); 
            int txtFareAmount = Request.Form["txtFareAmount"].AsInt();
            string txtEndorsementArea = Request["txtEndorsementArea"].Trim(); 
            string txtPNR2 = Request["txtPNR2"].Trim(); 
            string txtPassengerNam = Request["txtPassengerNam"].Trim(); 

            string dtpIssDoc1 = Request["dtpIssDoc"].Trim();

            string txtBookingAgentID = Request["txtBookingAgentID"].Trim(); 
            string txtTourCode = Request["txtTourCode"].Trim(); 
            string txtFcmin = Request["txtFcmi"].Trim(); 
            int txtFcmi = Request.Form["txtFcmi"].AsInt();
            string txtTotalamtcur = Request["txtTotalamtcur"].Trim(); 
            string txtFarePaidAmtn = Request["txtFarePaidAmt"].Trim(); 
            int txtFarePaidAmt = Request.Form["txtFarePaidAmt"].AsInt();
            string txtIssuedInExchangeFor = Request["txtIssuedInExchangeFor"].Trim(); 
            string txtTotalamtn = Request["txtTotalamt"].Trim(); 
            int txtTotalamt = Request.Form["txtTotalamt"].AsInt();
            string txtVendorIdentifier = Request["txtVendorIdentifier"].Trim(); 
            string txttransactioncode = Request["txttransactioncode"].Trim(); 
            string txtReportingSystemIdentifier = Request["txtReportingSystemIdentifier"].Trim(); 
            string txtTicketingAgentId = Request["txtTicketingAgentId"].Trim();
            string[] orgi = new string[5];
            if (txtIssuedInExchangeFor != "")
            {
                orgi = OrgDetails(txtIssuedInExchangeFor);
            }

             Guid HdrGuid = new Guid();
            HdrGuid = Guid.NewGuid();
            string sql = "";
            sql = sql + " INSERT INTO TMP.SalesDocumentHeader ( HdrGuid,SalesDataAvailable,AccountingStatus, ProratedFlag,FBSFlag,DocumentNumber,CheckDigit ,TrueOriginDestinationCityCodes ,FareCalculationArea ,FareCurrency ";
            sql = sql + ",Fare,EndosRestriction ,BookingReference ,PassengerName,DateofIssue ,AgentNumericCode ,BookingAgentIdentification,TourCode,FareCalculationModeIndicator   ";
            sql = sql + " ,TotalCurrency,EquivalentFare ,OriginalIssueDocumentNumber,OriginalIssueInformation,OriginalIssueCity,OriginalIssueDate,OriginalIssueAgentNumericCode,IsReissue,TotalAmount,VendorIdentification   ,transactioncode,ReportingSystemIdentifier ,TicketingAgentID,DataSource   )   ";
            sql = sql + " VALUES ( '" + HdrGuid + "','1','N','1','0' ";
            if (txtDocumentCarrier == "" && txtDocumentNumber == "")
            {
                return;
            }
            else { 
                 sql = sql + ",concat('" + txtDocumentCarrier + "','" + txtDocumentNumber + "')  ";//DocumentNumber
             }
             if (txtChkDgt == "")
             {
                if (_Doc != "")
                {
                    string ck = (Convert.ToInt64(_Doc) % 7).ToString();
                    sql = sql + ",'" + ck + "'"; //CheckDigit
                }
            }
             else
             {
                 sql = sql + ",'" + txtChkDgt + "'"; //CheckDigit
             }
            if (txtOrgDest == "")
            {
                return;
            }
            else
             {
                 sql = sql + ",'" + txtOrgDest + "' ";//TrueOriginDestinationCityCodes
             }
            if (txtFareComponent == "")
            {
                return;
            }
            else
             {
                 sql = sql + ",'" + txtFareComponent + "' ";//FareCalculationArea
             }
            if (txtFareCurrency == "")
            {
                return;
            }
            else
             {
                 sql = sql + ",'" + txtFareCurrency + "' ";//FareCurrency
             }
            if (txtFareAmountn == "")
            {
                return;
            }
            else
             {
                 sql = sql + ",'" + txtFareAmount + "' ";//Fare
             }
             if (txtEndorsementArea == "")
             {
                 sql = sql + ",NULL ";//EndosRestriction
             }
             else
             {
                 sql = sql + ",'" + txtEndorsementArea + "' ";//EndosRestriction
             }

             if (txtPNR2 == "")
             {
                 sql = sql + ",NULL ";//BookingReference
             }
             else
             {
                 sql = sql + ",'" + txtPNR2 + "' ";//BookingReference
             }
            if (txtPassengerNam == "")
            {
                return;
            }
            else
             {
                 sql = sql + ",'" + txtPassengerNam + "' ";//PassengerName
             }

             if (dtpIssDoc1 == "")
             {
                 sql = sql + ",NULL ";//DateofIssue
             }
             else
             {
                if (dtpIssDoc1 != "")
                {
                    DateTime dtpIssDoc = DateTime.Parse(dtpIssDoc1);
                    sql = sql + ",'" + dtpIssDoc + "' ";//DateofIssue
                }
                else
                {
                    sql = sql + ",NULL";//DateofIssue
                }
            }

             if (txtCustomerCode == "")
             {
                 sql = sql + ",NULL ";//AgentNumericCode
             }
             else
             {
                 sql = sql + ",'" + txtCustomerCode + "' ";//AgentNumericCode
             }
             if (txtBookingAgentID == "")
             {
                 sql = sql + ",NULL ";//BookingAgentIdentification
             }
             else
             {
                 sql = sql + ",'" + txtBookingAgentID + "' ";//BookingAgentIdentification
             }
             if (txtTourCode == "")
             {
                 sql = sql + ",NULL ";//TourCode
             }
             else
             {
                 sql = sql + ",'" + txtTourCode + "' ";//TourCode
             }
             if (txtFcmin == "")
             {
                 sql = sql + ",NULL ";//FareCalculationModeIndicator
             }
             else
             {
                 sql = sql + ",'" + txtFcmi + "' ";//FareCalculationModeIndicator
             }
             //if (txtCommRef.Text == "")
             //{
             //    sql = sql + ",NULL ";//CommercialAgreementReference
             //}
             //else
             //{
             //    sql = sql + ",'" + txtCommRef.Text + "' ";//CommercialAgreementReference
             //}
             if (txtTotalamtcur == "")
             {
                 sql = sql + ",NULL ";//TotalCurrency
             }
             else
             {
                 sql = sql + ",'" + txtTotalamtcur + "' ";//TotalCurrency
             }
             if (txtFarePaidAmtn == "")
             {
                 sql = sql + ",'0.00' ";//EquivalentFare
             }
             else
             {
                 sql = sql + ",'" + txtFarePaidAmt + "' ";//EquivalentFare 
             }
             if (txtIssuedInExchangeFor == "")
             {
                 sql = sql + ",NULL ";//OriginalIssueDocumentNumber
                 sql = sql + ",NULL ";//OriginalIssueInformation
                 sql = sql + ",NULL ";//OriginalIssueCity
                 sql = sql + ",NULL ";//OriginalIssueDate
                 sql = sql + ",NULL ";//OriginalIssueAgentNumericCode
                 sql = sql + ",'0'";//IsReissue

                 //,OriginalIssueDocumentNumber,OriginalIssueInformation,OriginalIssueCity,OriginalIssueDate,OriginalIssueAgentNumericCode,IsReissue

             }
             else
             {
                if(txtIssuedInExchangeFor.ToString().Length >= 13)
                {
                    sql = sql + ",'" + txtIssuedInExchangeFor.Substring(0, 13).ToString() + "' ";//OriginalIssueDocumentNumber
                }
                else
                {
                    sql = sql + ",'" + txtIssuedInExchangeFor.ToString() + "' ";//OriginalIssueDocumentNumber
                }
                sql = sql + ",'" + orgi[1] + "' ";//OriginalIssueInformation
                 sql = sql + ",'" + orgi[2] + "' ";//OriginalIssueCity
                 sql = sql + ",'" + orgi[3] + "' ";//OriginalIssueDate
                 sql = sql + ",'" + orgi[4] + "' ";//OriginalIssueAgentNumericCode
                 sql = sql + ",'1'";//IsReissue


             }
            if (txtTotalamtn == "")
            {
                sql = sql + ",NULL ";//TotalAmount
            }
            else
            {
                sql = sql + ",'" + txtTotalamt + "' ";//TotalAmount
            }
            if (txtVendorIdentifier == "")
            {
                sql = sql + ",NULL ";//VendorIdentification
            }
            else
            {
                sql = sql + ",'" + txtVendorIdentifier + "' ";//VendorIdentification
            }
            if (txttransactioncode == "")
            {
                return;
            }
            else
            {
                sql = sql + ",'" + txttransactioncode + "' ";//transactioncode
            }

            if (txtReportingSystemIdentifier == "")
            {
                sql = sql + ",NULL";//ReportingSystemIdentifier
            }
            else
            {
                sql = sql + ",'" + txtReportingSystemIdentifier + "' ";//ReportingSystemIdentifier
            }
           if (txtTicketingAgentId == "")
           {
               sql = sql + ",NULL, 'MANUAL') ";//TicketingAgentID + Datasource
           }
           else
           {
               sql = sql + ",'" + txtTicketingAgentId + "'  , 'MANUAL') ";//TicketingAgentID + Datasource
           }
            SqlDataAdapter dap = new SqlDataAdapter();
             DataSet dsp = new DataSet();
             dsp = dbconnect.RetObjDS(sql);
        }
        public void cpnSave()
        {
            string a = Request["a"];
            string b = Request["b"];
            string c = Request["c"];
            string d = Request["d"];
            string ee = Request["ee"];
            string f = Request["f"];
            string g1 = Request["g"];
            string h = Request["h"];
            string i = Request["i"];
            string j = Request["j"];
            string k1 = Request["k"];
            string l1 = Request["l"];
            string m = Request["m"];
            string n = Request["n"];
            string o = Request["o"];
            string p = Request["p"];
            string q = Request["q"];
            string r = Request["r"];
            string s1 = Request["s"];
            string t = Request["t"];
            string u = Request["u"];

            Guid HdrGuid = new Guid();
            HdrGuid = Guid.NewGuid();
            Guid v = HdrGuid;

            SqlDataAdapter da = new SqlDataAdapter();
            SqlConnection cs = new SqlConnection(pbConnectionString);

            da.InsertCommand = new SqlCommand("INSERT INTO [Tmp].[SalesDocumentCoupon] (CouponNumber ,StopOverCode ,OriginAirportCityCode,DestinationAirportCityCode,Carrier,FlightNumber,ReservationBookingDesignator ,FlightDepartureDate ,[FlightDepartureTime],CouponStatus,FareBasisTicketDesignator,NotValidBefore,NotValidAfter,FreeBaggageAllowance,FlightBookingStatus,UsedClassofService,UsageOriginCode,UsageDestinationCode,UsageAirline,UsageFlightNumber,UsageDate,FrequentFlyerReference,DocumentNumber,HdrGuidRef)VALUES (@CouponNumber  ,@StopOverCode ,@OriginAirportCityCode,@DestinationAirportCityCode,@Carrier,@FlightNumber,@ReservationBookingDesignator ,@FlightDepartureDate ,@FlightDepartureTime,@CouponStatus,@FareBasisTicketDesignator,@NotValidBefore,@NotValidAfter,@FreeBaggageAllowance,@FlightBookingStatus,@UsedClassofService,@UsageOriginCode,@UsageDestinationCode,@UsageAirline,@UsageFlightNumber,@UsageDate,@FrequentFlyerReference,@DocumentNumber,@HdrGuidRef)", cs);

            if (a == "") { da.InsertCommand.Parameters.Add("@CouponNumber", SqlDbType.VarChar).Value = DBNull.Value; }
            else { da.InsertCommand.Parameters.Add("@CouponNumber", SqlDbType.VarChar).Value = a; }

            if (b == "") { da.InsertCommand.Parameters.Add("@StopOverCode", SqlDbType.VarChar).Value = DBNull.Value; }
            else { da.InsertCommand.Parameters.Add("@StopOverCode", SqlDbType.VarChar).Value = b; }

            if (c == "") { da.InsertCommand.Parameters.Add("@OriginAirportCityCode", SqlDbType.VarChar).Value = DBNull.Value; }
            else { da.InsertCommand.Parameters.Add("@OriginAirportCityCode", SqlDbType.VarChar).Value = c.Substring(0, 3).ToString(); }

            if (c == "") { da.InsertCommand.Parameters.Add("@DestinationAirportCityCode", SqlDbType.VarChar).Value = DBNull.Value; }
            else { da.InsertCommand.Parameters.Add("@DestinationAirportCityCode", SqlDbType.VarChar).Value = c.Substring(4, 3).ToString(); }

            if (d == "") { da.InsertCommand.Parameters.Add("@Carrier", SqlDbType.VarChar).Value = DBNull.Value; }
            else { da.InsertCommand.Parameters.Add("@Carrier", SqlDbType.VarChar).Value = d; }

            if (ee == "") { da.InsertCommand.Parameters.Add("@FlightNumber", SqlDbType.VarChar).Value = DBNull.Value; }
            else { da.InsertCommand.Parameters.Add("@FlightNumber", SqlDbType.VarChar).Value = ee; }

            if (f == "") { da.InsertCommand.Parameters.Add("@ReservationBookingDesignator", SqlDbType.VarChar).Value = DBNull.Value; }
            else { da.InsertCommand.Parameters.Add("@ReservationBookingDesignator", SqlDbType.VarChar).Value = f; }

            if (g1 == "") { da.InsertCommand.Parameters.Add("@FlightDepartureDate", SqlDbType.VarChar).Value = DBNull.Value; }
            else {
                DateTime g = DateTime.Parse(g1);
                da.InsertCommand.Parameters.Add("@FlightDepartureDate", SqlDbType.VarChar).Value = g;
            }

            if (h == "") { da.InsertCommand.Parameters.Add("@FlightDepartureTime", SqlDbType.VarChar).Value = DBNull.Value; }
            else { da.InsertCommand.Parameters.Add("@FlightDepartureTime", SqlDbType.VarChar).Value = h; }

            if (i == "") { da.InsertCommand.Parameters.Add("@CouponStatus", SqlDbType.VarChar).Value = DBNull.Value; }
            else { da.InsertCommand.Parameters.Add("@CouponStatus", SqlDbType.VarChar).Value = i; }

            if (j == "") { da.InsertCommand.Parameters.Add("@FareBasisTicketDesignator", SqlDbType.VarChar).Value = DBNull.Value; }
            else { da.InsertCommand.Parameters.Add("@FareBasisTicketDesignator", SqlDbType.VarChar).Value = j; }

            if (k1 == "") { da.InsertCommand.Parameters.Add("@NotValidBefore", SqlDbType.VarChar).Value = DBNull.Value; }
            else {
                DateTime k = DateTime.Parse(k1);
                da.InsertCommand.Parameters.Add("@NotValidBefore", SqlDbType.VarChar).Value = k;
            }

            if (l1 == "") { da.InsertCommand.Parameters.Add("@NotValidAfter", SqlDbType.VarChar).Value = DBNull.Value; }
            else {
                DateTime l = DateTime.Parse(l1);
                da.InsertCommand.Parameters.Add("@NotValidAfter", SqlDbType.VarChar).Value = l;
            }

            if (m == "") { da.InsertCommand.Parameters.Add("@FreeBaggageAllowance", SqlDbType.VarChar).Value = DBNull.Value; }
            else { da.InsertCommand.Parameters.Add("@FreeBaggageAllowance", SqlDbType.VarChar).Value = m; }

            if (n == "") { da.InsertCommand.Parameters.Add("@FlightBookingStatus", SqlDbType.VarChar).Value = DBNull.Value; }
            else { da.InsertCommand.Parameters.Add("@FlightBookingStatus", SqlDbType.VarChar).Value = n; }

            if (o == "") { da.InsertCommand.Parameters.Add("@UsedClassofService", SqlDbType.VarChar).Value = DBNull.Value; }
            else { da.InsertCommand.Parameters.Add("@UsedClassofService", SqlDbType.VarChar).Value = o; }

            if (p == "") { da.InsertCommand.Parameters.Add("@UsageOriginCode", SqlDbType.VarChar).Value = DBNull.Value; }
            else {
                if (p.ToString().Length >= 7)
                {
                    da.InsertCommand.Parameters.Add("@UsageOriginCode", SqlDbType.VarChar).Value = p.Substring(0, 3).ToString();
                }
                else
                {
                    string[] words = p.ToString().Split('-');
                    da.InsertCommand.Parameters.Add("@UsageOriginCode", SqlDbType.VarChar).Value = words[0]; 
                }
            }

            if (p == "") { da.InsertCommand.Parameters.Add("@UsageDestinationCode", SqlDbType.VarChar).Value = DBNull.Value; }
            else {
                if (p.ToString().Length >= 7) { 
                    da.InsertCommand.Parameters.Add("@UsageDestinationCode", SqlDbType.VarChar).Value = p.Substring(4, 3).ToString();
                }
                else
                {
                    string[] words = p.ToString().Split('-');
                    da.InsertCommand.Parameters.Add("@UsageDestinationCode", SqlDbType.VarChar).Value = words[1];
                }
            }

            if (q == "") { da.InsertCommand.Parameters.Add("@UsageAirline", SqlDbType.VarChar).Value = DBNull.Value; }
            else { da.InsertCommand.Parameters.Add("@UsageAirline", SqlDbType.VarChar).Value = q; }

            if (r == "") { da.InsertCommand.Parameters.Add("@UsageFlightNumber", SqlDbType.VarChar).Value = DBNull.Value; }
            else { da.InsertCommand.Parameters.Add("@UsageFlightNumber", SqlDbType.VarChar).Value = r; }

            if (s1 == "") { da.InsertCommand.Parameters.Add("@UsageDate", SqlDbType.VarChar).Value = DBNull.Value; }
            else {
                if (s1 != "")
                {
                    DateTime s = DateTime.Parse(s1);
                    da.InsertCommand.Parameters.Add("@UsageDate", SqlDbType.VarChar).Value = s;
                }
            }

            if (t == "") { da.InsertCommand.Parameters.Add("@FrequentFlyerReference", SqlDbType.VarChar).Value = DBNull.Value; }
            else { da.InsertCommand.Parameters.Add("@FrequentFlyerReference", SqlDbType.VarChar).Value = t; }

            if (u == "") { da.InsertCommand.Parameters.Add("@DocumentNumber", SqlDbType.VarChar).Value = DBNull.Value; }
            else { da.InsertCommand.Parameters.Add("@DocumentNumber", SqlDbType.VarChar).Value = u; }

            da.InsertCommand.Parameters.Add("@HdrGuidRef", SqlDbType.UniqueIdentifier).Value = v;
            cs.Open();
            da.InsertCommand.ExecuteNonQuery();
            cs.Close();

        }
        public void ResqeuencingcheckifexistOA()
        {

            Guid HdrGuid = new Guid();
            HdrGuid = Guid.NewGuid();
            Guid aa = HdrGuid;
            int dgotherRowCount = Request.Form["dgotherRowCount"].AsInt();
            int dgpaymentRowCount = Request.Form["dgpaymentRowCount"].AsInt();

            if (dgotherRowCount > 1)
            {
                if (checkifexistOA(aa) == true)
                {
                    deleteOA();
                }
            }
            if (dgpaymentRowCount > 1)
            {
                if (checkifexistP(aa) == true)
                {
                    deleteP();
                }
            }
        }
        private void deleteOA()
        {
            Guid HdrGuid = new Guid();
            HdrGuid = Guid.NewGuid();
            string sql = "DELETE FROM TMP.SalesDocumentOtherAmount WHERE [HdrGuidRef] = '" + HdrGuid + "'";

            SqlConnection cs = new SqlConnection(pbConnectionString);

            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet();
            ds = dbconnect.RetObjDS(sql);
            cs.Close();

        }
        public void Saveotheramt()
        {
            string txtDocumentCarrier = Request["txtDocumentCarrier"].Trim();
            string txtDocumentNumber = Request["txtDocumentNumber"].Trim();
            string t0 = Request["t0"].Trim();
            string t1 = Request["t1"].Trim();
            string t2 = Request["t2"].Trim();
            int t2n = Request.Form["t2"].AsInt();

            string t3 = Request["t3"].Trim();
            int t3n = Request.Form["t3"].AsInt();
            //string t4 = Request["t4"].Trim();
            string _Doc = "";
            var list = new List<string>();
            list.Add(txtDocumentCarrier);
            list.Add(txtDocumentNumber);
            string _DOI = "";
            string _Cur = "";
            string _TranxCode = "";
            Guid HdrGuid = new Guid();
            HdrGuid = Guid.NewGuid();

            _Doc = string.Concat(list);

            SqlDataAdapter dap = new SqlDataAdapter();

            string sql = "";

            sql = sql + " INSERT INTO TMP.SalesDocumentOtherAmount ";
          //  sql = sql + " (DocumentAmountType,DateofIssue,CurrencyType,TransactionCode,OtherAmountCode,OtherAmount,OtherAmountRate,HdrGuidRef,DocumentNumber,SequenceNumber) VALUES ";
            sql = sql + " (DocumentAmountType,DateofIssue,CurrencyType,TransactionCode,OtherAmountCode,OtherAmount,OtherAmountRate,HdrGuidRef,DocumentNumber) VALUES ";


            sql = sql + " ('" + t0 + "','" + _DOI + "','" + _Cur + "','" + _TranxCode + "','" + t1 + "' ";
            if (t2 != "") {
                sql = sql + "  ,'" + Convert.ToDouble(t2n) + "' ";
            }
            else
            {
                sql = sql + "  ,'" + t2n + "' ";
            }

            if (t3 != null && !string.IsNullOrWhiteSpace(t3) && t3 != "")
            {
                sql = sql + "  , '" + t3n + "'                 ";
            }
            else
            {
                sql = sql + "  , NULL ";
            }
           // sql = sql + "   ,'" + HdrGuid + "','" + _Doc + "','" + t4 + "') ";
            sql = sql + "   ,'" + HdrGuid + "','" + _Doc + "') ";

            DataSet dsp = new DataSet();
            dsp = dbconnect.RetObjDS(sql);
        }
        public void SavePayment()
        {
            string txtDocumentCarrier = Request["txtDocumentCarrier"].Trim();
            string txtDocumentNumber = Request["txtDocumentNumber"].Trim();
            string dgOriDoc0 = Request["dgOriDoc0"].Trim();
            string ac = "";
            string _Doc = "";
            int dgOriDocRowCount = Request.Form["dgOriDocRowCount"].AsInt();
            string seqaccseq = Request["seqaccseq"].Trim();


            var list = new List<string>();
            list.Add(txtDocumentCarrier);
            list.Add(txtDocumentNumber);

            _Doc = string.Concat(list);
            string _DOI = "";
            int accseq = 1000;
            string _Cur = "";
            string _TranxCode = "";
            Guid HdrGuid = new Guid();
            HdrGuid = Guid.NewGuid();

            SqlDataAdapter da = new SqlDataAdapter();
            string sql = "";

            if (dgOriDoc0 != null && !string.IsNullOrWhiteSpace(dgOriDoc0) && dgOriDoc0 != "")
            {
                sql += " INSERT INTO Tmp.SalesDocumentPayment (DateofIssue,SequenceNumber,AccountNumber,Amount,Currency,FormofPaymentType,RemittanceAmount,RemittanceCurrency,TransactionCode,HdrGuidRef,DocumentNumber)  ";
                sql += " VALUES('" + _DOI + "'  " + Environment.NewLine;
                sql += ", '" + accseq + seqaccseq + "'  " + Environment.NewLine;
                if (Accno(dgOriDoc0.Substring(0, 13)) == null)
                {
                    sql += ", '" + dgOriDoc0.Substring(0, 13) + "'  " + Environment.NewLine;
                }
                else
                {
                    sql += ", '" + Accno(dgOriDoc0.Substring(0, 13)) + "'  " + Environment.NewLine;
                }
                sql += " , '0.00'  " + Environment.NewLine;
                sql += " , '" + _Cur + "' " + Environment.NewLine;
                sql += " , 'EX'  " + Environment.NewLine;
                sql += " , '0.00'  " + Environment.NewLine;
                sql += " , '" + _Cur + "' " + Environment.NewLine;
                sql += ",'" + _TranxCode + "','" + HdrGuid + "','" + _Doc + "')  " + Environment.NewLine;
            }
            SqlConnection cs = new SqlConnection(pbConnectionString);

            DataSet ds = new DataSet();
            ds = dbconnect.RetObjDS(sql);
            cs.Close();

        }
        private string Accno(string Doc)
        {
            string Acc = "";
            string sql = "Select concat(documentnumber, CheckDigit)" + Environment.NewLine;
            sql += "from pax.SalesDocumentHeader" + Environment.NewLine;
            sql += "where DocumentNumber = '" + Doc + "'" + Environment.NewLine;

            SqlConnection cs = new SqlConnection(pbConnectionString);
            DataSet ds = new DataSet();
            SqlDataAdapter ada = new SqlDataAdapter(sql, cs);
            ada.Fill(ds);
            int ligne = ds.Tables[0].Rows.Count;
            if (ligne > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    Acc = dr[ds.Tables[0].Columns[0].ColumnName].ToString();
                }
            }
            cs.Close();

            string sql1 = "select CouponNumber " + Environment.NewLine;
            sql1 += "from pax.SalesDocumentCoupon" + Environment.NewLine;
            sql1 += "where relatedDocumentNumber = '" + Doc + "'" + Environment.NewLine;
            sql1 += "and CouponStatus = 'E'" + Environment.NewLine;
            sql1 += "order by CouponNumber " + Environment.NewLine;

            SqlConnection cs1 = new SqlConnection(pbConnectionString);
            DataSet ds1 = new DataSet();
            SqlDataAdapter ada1 = new SqlDataAdapter(sql1, cs1);
            ada1.Fill(ds1);
            int ligne1 = ds1.Tables[0].Rows.Count;
            if (ligne1 > 0)
            {
                foreach (DataRow dr1 in ds1.Tables[0].Rows)
                {
                    Acc += dr1[ds1.Tables[0].Columns[0].ColumnName].ToString();
                }
            }
            cs1.Close();
            return Acc;
        }
        public void SP_ManualEntryUpdate()
        {
            string u2 = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
            string v2 = GetUserIP();
            Guid HdrGuid = new Guid();
            HdrGuid = Guid.NewGuid();

            string sql = " EXEC Pax.SP_ManualEntryUpdate @hdrguid = '" + HdrGuid + "' ,@IP ='" + v2 + "',@User ='" + u2 + "'";

            SqlConnection cs = new SqlConnection(pbConnectionString);

            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet();
            ds = dbconnect.RetObjDS(sql);
            cs.Close();
        }
        private string GetUserIP()
        {
            string ipList = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if (!string.IsNullOrEmpty(ipList))
            {
                return ipList.Split(',')[0];
            }

            return Request.ServerVariables["REMOTE_ADDR"];
        }
        public void ManualEntryLog()
        {
            string Sql = "DELETE FROM [Pax].[ManualEntryLog] WHERE HdrGuid NOT IN (SELECT HdrGuid FROM PAX.SalesDocumentHeader) ";
            SqlConnection cs = new SqlConnection(pbConnectionString);

            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet();
            ds = dbconnect.RetObjDS(Sql);
            cs.Close();
        }
        public void SavePaymentdgpayment()
        {
            string txtDocumentCarrier = Request["txtDocumentCarrier"].Trim();
            string txtDocumentNumber = Request["txtDocumentNumber"].Trim();
            string dgpayment0 = Request["dgpayment0"].Trim();
            int dgpayment0n = Request.Form["dgpayment0"].AsInt();

            string dgpayment1 = Request["dgpayment1"].Trim();
            string dgpayment2 = Request["dgpayment2"].Trim();
            string _TranxCode = "";
            Guid HdrGuid = new Guid();
            HdrGuid = Guid.NewGuid();
            string _Doc = "";
            var list = new List<string>();
            list.Add(txtDocumentCarrier);
            list.Add(txtDocumentNumber);

            _Doc = string.Concat(list);

            string _DOI = "";
            string _Cur = "";
            string sql = "";
            sql = sql + " INSERT INTO Tmp.SalesDocumentPayment (DateofIssue,Amount,Currency,FormofPaymentType,RemittanceAmount,RemittanceCurrency,TransactionCode,HdrGuidRef,DocumentNumber)  ";
            sql = sql + " VALUES('" + _DOI + "' ";
            if (dgpayment0 != null && !string.IsNullOrWhiteSpace(dgpayment0) && dgpayment0 != "")
            {
                sql = sql + "  , '" + dgpayment0n + "'";
            }
            else
            {
                sql = sql + "  , '0.00' ";
            }

            sql = sql + "  , '" + _Cur + "'";


            if (dgpayment1 != null && !string.IsNullOrWhiteSpace(dgpayment1) && dgpayment1 != "")
            {
                sql = sql + "  , '" + dgpayment1 + "'";
            }
            else
            {
                sql = sql + "  , '' ";
            }
            if (dgpayment2 != null && !string.IsNullOrWhiteSpace(dgpayment2) && dgpayment2 != "")
            {
                sql = sql + "  , '" + dgpayment2 + "'";
            }
            else
            {
                sql = sql + "  , NULL ";
            }

            sql = sql + "  , '" + _Cur + "'";


            sql = sql + ",'" + _TranxCode + "','" + HdrGuid + "','" + _Doc + "')" + Environment.NewLine;

            SqlConnection cs = new SqlConnection(pbConnectionString);

            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet();
            ds = dbconnect.RetObjDS(sql);
            cs.Close();

        }
        private bool checkifexistOA(Guid a)
        {
            bool t = false;

            string sql = "SELECT * FROM TMP.SalesDocumentOtherAmount WHERE [HdrGuidRef] = '" + a + "'";

            SqlConnection cs = new SqlConnection(pbConnectionString);
            DataSet ds = new DataSet();
            SqlDataAdapter ada = new SqlDataAdapter(sql, cs);
            ada.Fill(ds);

            int ligne = ds.Tables[0].Rows.Count;
            if (ligne > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    t = true;
                }
            }
            cs.Close();

            return t;
        }
        private void deleteP()
        {
            Guid HdrGuid = new Guid();
            HdrGuid = Guid.NewGuid();
            string sql = "DELETE FROM Tmp.SalesDocumentPayment WHERE [HdrGuidRef] = '" + HdrGuid + "'";

            SqlConnection cs = new SqlConnection(pbConnectionString);

            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet();
            ds = dbconnect.RetObjDS(sql);
            cs.Close();

        }
        private bool checkifexistP(Guid a)
        {
            bool t = false;

            string sql = "SELECT * FROM Tmp.SalesDocumentPayment WHERE [HdrGuidRef] = '" + a + "'";

            SqlConnection cs = new SqlConnection(pbConnectionString);
            DataSet ds = new DataSet();
            SqlDataAdapter ada = new SqlDataAdapter(sql, cs);
            ada.Fill(ds);
            int ligne = ds.Tables[0].Rows.Count;
            if (ligne > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    t = true;
                }
            }
            cs.Close();

            return t;
        }
        public string[] OrgDetails(string DocNum)
        {
            string[] a = new string[5];
            string sql = "select DocumentNumber" + Environment.NewLine;
            sql += ", concat(DocumentNumber,substring(TrueOriginDestinationCityCodes,1,3)," + Environment.NewLine;
            sql += "SUBSTRING(CONVERT(NVARCHAR,DateofIssue, 106),1,2)," + Environment.NewLine;
            sql += "UPPER(SUBSTRING(CONVERT(NVARCHAR,DateofIssue, 106),4,3))," + Environment.NewLine;
            sql += "SUBSTRING(CONVERT(NVARCHAR,DateofIssue, 106),10,2), AgentNumericCode)" + Environment.NewLine;
            sql += ", substring(Ltrim(TrueOriginDestinationCityCodes),1,3), DateofIssue, AgentNumericCode" + Environment.NewLine;
            sql += "from pax.SalesDocumentHeader" + Environment.NewLine;
            sql += "where DocumentNumber = '" + DocNum + "'" + Environment.NewLine;
            SqlConnection con = new SqlConnection(pbConnectionString);
            DataSet ds = new DataSet();
            SqlDataAdapter ada = new SqlDataAdapter(sql, con);
            ada.Fill(ds);
            int ligne = ds.Tables[0].Rows.Count;
            if (ligne > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    a[0] = dr[ds.Tables[0].Columns[0].ColumnName].ToString();
                    a[1] = dr[ds.Tables[0].Columns[1].ColumnName].ToString();
                    a[2] = dr[ds.Tables[0].Columns[2].ColumnName].ToString();
                    a[3] = dr[ds.Tables[0].Columns[3].ColumnName].ToString();
                    a[4] = dr[ds.Tables[0].Columns[4].ColumnName].ToString();
                }
            }
            con.Close();
            return (a);
        }
        public void ManualRelatedDocInf()
        {
            string txtDocumentCarrier = Request["txtDocumentCarrier"].Trim(); 
            string txtDocumentNumber = Request["txtDocumentNumber"].Trim(); 
            string txtChkDgt = Request["txtChkDgt"].Trim(); 
            string txtCPUI = Request["txtCPUI"].Trim(); 
            string txttransactioncode = Request["txttransactioncode"].Trim(); 
            string _Doc = "";
            Guid HdrGuid = new Guid();
            var list = new List<string>();
            list.Add(txtDocumentCarrier);
            list.Add(txtDocumentNumber);

            _Doc = string.Concat(list);


            SqlDataAdapter dap = new SqlDataAdapter();

            string sql = "";
            sql = sql + " INSERT INTO [TMP].[SalesRelatedDocumentInformation] (RelatedDocumentGuid,HdrGuid,[DocumentNumber],[CheckDigit],[CouponIndicator],[TransactionCode]) ";
            sql = sql + " VALUES( '" + HdrGuid + "','" + HdrGuid + "' ";
            if (txtDocumentCarrier == "" && txtDocumentNumber == "")
            {
                return;
            }
            else
            {
                sql = sql + ",concat('" + txtDocumentCarrier + "','" + txtDocumentNumber + "')  ";//DocumentNumber
            }
            if (txtChkDgt == "")
            {
                if (_Doc != "")
                {
                    string ck = (Convert.ToInt64(_Doc) % 7).ToString();
                    sql = sql + ",'" + ck + "'"; //CheckDigit
                }
                else
                {
                    sql = sql + ",NULL"; //CheckDigit
                }
            }
            else
            {
                sql = sql + ",'" + txtChkDgt + "'"; //CheckDigit

            }
            if (txtCPUI == "")
            {
                sql = sql + ",NULL ";
            }
            else
            {
                sql = sql + ",'" + txtCPUI + "' ";
            }
            if (txttransactioncode == "")
            {
                return;
            }
            else
            {
                sql = sql + ",'" + txttransactioncode + "') ";//transactioncode
            }
            DataSet dsp = new DataSet();
            dsp = dbconnect.RetObjDS(sql);
        }
        public string CheckGrid()
        {
            int col = Request.Form["jj"].AsInt();
            int row = Request.Form["ii"].AsInt();
            string countdbgCPNlist = Request["countdbgCPNlist"];

            string a = "";
            if (countdbgCPNlist != null && !string.IsNullOrWhiteSpace(countdbgCPNlist) && countdbgCPNlist != "") { a = countdbgCPNlist; }
            return a;
        }
        public ActionResult btnClear_Clickfiftedcoupons()
        {
            ViewBag.dateTo = DateTime.Now.ToString("dd-MMM-yyyy");
            return PartialView();
        }
        public ActionResult btnSearch_Clickfiftedcoupons()
        {
            string txtFlightNo = Request["txtFlightNo"].Trim();
            string dateFromlifted = Request["dateFromlifted"].Trim();
            string dateTolifted = Request["dateTolifted"].Trim();
            DateTime dtpFrom = DateTime.Parse(dateFromlifted);
            DateTime dtpTo = DateTime.Parse(dateTolifted);
            string b = OWN();
            SqlConnection con = new SqlConnection(pbConnectionString);
            DataSet ds = new DataSet();
            SqlCommand cmd = new SqlCommand("[FileLift].[SP_Lifted_Coupon_Validation]", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 300;
            cmd.Parameters.AddWithValue("@UsageFlightNumber", txtFlightNo.ToString().Trim());
            cmd.Parameters.AddWithValue("@UsageDate_from", dtpFrom);
            cmd.Parameters.AddWithValue("@UsageDate_to", dtpTo);
            SqlDataAdapter ada = new SqlDataAdapter(cmd);
            ada.Fill(ds);
            int ligne = ds.Tables[0].Rows.Count;
            int colone = ds.Tables[0].Columns.Count;
            string[,] dgOWN = new string[ligne, 5];
            string[,] dgOAL = new string[ligne, 8];
            int i = 0;
            int j = 0;
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                string ao = dr[ds.Tables[0].Columns[1].ColumnName].ToString();
                if (ao == b)
                {
                    dgOWN[j,0] = dr[ds.Tables[0].Columns[0].ColumnName].ToString();
                    dgOWN[j,1] = dr[ds.Tables[0].Columns[1].ColumnName].ToString();
                    dgOWN[j,2] = dr[ds.Tables[0].Columns[2].ColumnName].ToString();
                    dgOWN[j,3] = dr[ds.Tables[0].Columns[3].ColumnName].ToString();
                    dgOWN[j,4] = dr[ds.Tables[0].Columns[4].ColumnName].ToString();
                    j++;
                }
                else
                {
                    dgOAL[i,0] = dr[ds.Tables[0].Columns[0].ColumnName].ToString();
                    dgOAL[i,1] = dr[ds.Tables[0].Columns[1].ColumnName].ToString();
                    dgOAL[i,2] = dr[ds.Tables[0].Columns[2].ColumnName].ToString();
                    dgOAL[i,3] = dr[ds.Tables[0].Columns[3].ColumnName].ToString();
                    dgOAL[i,4] = dr[ds.Tables[0].Columns[4].ColumnName].ToString();
                    dgOAL[i,5] = dr[ds.Tables[0].Columns[5].ColumnName].ToString();
                    dgOAL[i,6] = dr[ds.Tables[0].Columns[6].ColumnName].ToString();
                    i++;
                }
            }
            con.Close();
            SqlConnection cona = new SqlConnection(pbConnectionString);
            DataSet dsa = new DataSet();
            SqlCommand cmda = new SqlCommand("[FileLift].[SP_Lifted_Coupon_Validation_DNA_Sum]", cona);
            cmda.CommandType = CommandType.StoredProcedure;
            cmda.CommandTimeout = 300;
            cmda.Parameters.AddWithValue("@UsageFlightNumber", txtFlightNo.ToString().Trim());
            cmda.Parameters.AddWithValue("@UsageDate_from", dtpFrom);
            cmda.Parameters.AddWithValue("@UsageDate_to", dtpTo);
            SqlDataAdapter adaa = new SqlDataAdapter(cmda);
            adaa.Fill(dsa);
            int lignea = dsa.Tables[0].Rows.Count;
            string[,] dgtype = new string[lignea, 2];
            int p = 0;
            foreach (DataRow dra in dsa.Tables[0].Rows)
            {
                dgtype[p,0] = dra[dsa.Tables[0].Columns[0].ColumnName].ToString();
                dgtype[p,1] = dra[dsa.Tables[0].Columns[1].ColumnName].ToString();
                p++;
            }
            cona.Close();
            double SumliftedOwn = 0;
            double SumliftedOal = 0;
            double SumPNLOwn = 0;
            double SUmPNLOal = 0;
            string coloredgOAL567 = "";
            for (int h = 0; h < i; h++)
            {
                SumliftedOal = SumliftedOal + Convert.ToDouble(dgOAL[h,2]);
                SUmPNLOal = SUmPNLOal + Convert.ToDouble(dgOAL[h,4]);
            }
            for (int h = 0; h < j; h++)
            {
                SumliftedOwn = SumliftedOwn + Convert.ToDouble(dgOWN[h,2]);
                SumPNLOwn = SumPNLOwn + Convert.ToDouble(dgOWN[h,4]);
            }
            string[,] dgtotal = new string[3, 4];
            dgtotal[0,0] = "OAL Total :";
            dgtotal[0,1] = SumliftedOal.ToString();
            dgtotal[0,3] = SUmPNLOal.ToString();
            dgtotal[1,0] = "OWN Total :";
            dgtotal[1,1] = SumliftedOwn.ToString();
            dgtotal[1,3] = SumPNLOwn.ToString();
            dgtotal[2,0] = "Total :";
            double temps = SumliftedOwn + SumliftedOal;
            dgtotal[2,1] = temps.ToString();
            double tempsa = SUmPNLOal + SumPNLOwn;
            dgtotal[2, 3] = tempsa.ToString();
            double count = 0;
            for (int h = 0; h < p; h++)
            {
                count = count + Convert.ToDouble(dgtype[h,1]);
            }
            for (int g = 0; g < i; g++)
            {
                if (dgOAL[g,6] == null || string.IsNullOrWhiteSpace(dgOAL[g,6].ToString()))
                {
                    dgOAL[g,6] = "To be updated when billed";
                }
                if (dgOAL[g,5].ToString() == "N")
                {
                    dgOAL[g,7] = dgOAL[g,2];
                    coloredgOAL567 = "1";
                }
            }
            ViewBag.coloredgOAL567 = coloredgOAL567;
            ViewBag.dgtotal = dgtotal;
            ViewBag.count = count;
            ViewBag.dgtype = dgtype;
            ViewBag.dgOWN = dgOWN;
            ViewBag.dgOAL = dgOAL;
            ViewBag.ligne = ligne;
            ViewBag.lignea = lignea;
            return PartialView();
        }
        private string OWN()
        {
            string ow = "";
            string sql = "";
            sql = sql + "SELECT String4 FROM [Adm].[GSP] WHERE Parameter = 'SYS0001'";
            SqlConnection con = new SqlConnection(pbConnectionString);
            DataSet ds = new DataSet();
            SqlDataAdapter ada = new SqlDataAdapter(sql, con);
            ada.Fill(ds);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                ow = dr[ds.Tables[0].Columns[0].ColumnName].ToString();
            }
            con.Close();
            return ow;
        }
        public ActionResult dgOWN_CellContentDoubleClick()
        {
            string Airline = "";
            string UsageType = "";
            string Own = "Y";

            Airline = Request["Airline"].Trim();
            UsageType = Request["UsageType"].Trim();
            string dateFromlifted = Request["dateFromlifted"].Trim();
            string dateTolifted = Request["dateTolifted"].Trim();
            DateTime dtpFrom = DateTime.Parse(dateFromlifted);
            DateTime dtpTo = DateTime.Parse(dateTolifted);
            /*Details*/
            string sql = "";
            sql = sql + "SELECT AirlineCode, UsageType, UsageFlightNumber, UsageDate, DocumentType, RelatedDocumentNumber, BILLINGPERIOD, INVOICENO FROM [FileLift].[VW_Lifted_Coupon_Validation] ";
            sql = sql + " WHERE AirlineCode = '" + Airline + "' AND UsageType = '" + UsageType + "' AND OwnTicket = '" + Own + "' and UsageDate between Convert(date,'" + dtpFrom + "', 105) AND Convert(date,'" + dtpTo + "', 105)";
            SqlConnection con = new SqlConnection(pbConnectionString);
            DataSet ds = new DataSet();
            SqlDataAdapter ada = new SqlDataAdapter(sql, con);
            ada.Fill(ds);
            int ligne = ds.Tables[0].Rows.Count;
            string[,] dgdetails = new string[ligne, 8];
            int h = 0;
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                dgdetails[h, 0] = dr[ds.Tables[0].Columns[0].ColumnName].ToString();
                dgdetails[h, 1] = dr[ds.Tables[0].Columns[1].ColumnName].ToString();
                dgdetails[h, 2] = dr[ds.Tables[0].Columns[0].ColumnName].ToString();
                dgdetails[h, 3] = dr[ds.Tables[0].Columns[3].ColumnName].ToString();
                dgdetails[h, 4] = dr[ds.Tables[0].Columns[4].ColumnName].ToString();
                dgdetails[h, 5] = dr[ds.Tables[0].Columns[5].ColumnName].ToString();
                dgdetails[h, 6] = dr[ds.Tables[0].Columns[6].ColumnName].ToString();
                dgdetails[h, 7] = dr[ds.Tables[0].Columns[7].ColumnName].ToString();
                h++;
            }
            /*fin Details*/
            ViewBag.ligne = ligne;
            ViewBag.dgdetails = dgdetails;

            return PartialView();
        }
        public ActionResult dgOAL_CellContentDoubleClick()
        {
            string Airline = "";
            string UsageType = "";
            string Own = "N";
            Airline = Request["Airline"].Trim();
            UsageType = Request["UsageType"].Trim();
            string dateFromlifted = Request["dateFromlifted"].Trim();
            string dateTolifted = Request["dateTolifted"].Trim();
            DateTime dtpFrom = DateTime.Parse(dateFromlifted);
            DateTime dtpTo = DateTime.Parse(dateTolifted);
            /*Details*/
            string sql = "";
            sql = sql + "SELECT AirlineCode, UsageType, UsageFlightNumber, UsageDate, DocumentType, RelatedDocumentNumber, BILLINGPERIOD, INVOICENO FROM [FileLift].[VW_Lifted_Coupon_Validation] ";
            sql = sql + " WHERE AirlineCode = '" + Airline + "' AND UsageType = '" + UsageType + "' AND OwnTicket = '" + Own + "' and UsageDate between Convert(date,'" + dtpFrom + "', 105) AND Convert(date,'" + dtpTo + "', 105)";
            SqlConnection con = new SqlConnection(pbConnectionString);
            DataSet ds = new DataSet();
            SqlDataAdapter ada = new SqlDataAdapter(sql, con);
            ada.Fill(ds);
            int ligne = ds.Tables[0].Rows.Count;
            string[,] dgdetails = new string[ligne, 8];
            int h = 0;
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                dgdetails[h, 0] = dr[ds.Tables[0].Columns[0].ColumnName].ToString();
                dgdetails[h, 1] = dr[ds.Tables[0].Columns[1].ColumnName].ToString();
                dgdetails[h, 2] = dr[ds.Tables[0].Columns[0].ColumnName].ToString();
                dgdetails[h, 3] = dr[ds.Tables[0].Columns[3].ColumnName].ToString();
                dgdetails[h, 4] = dr[ds.Tables[0].Columns[4].ColumnName].ToString();
                dgdetails[h, 5] = dr[ds.Tables[0].Columns[5].ColumnName].ToString();
                dgdetails[h, 6] = dr[ds.Tables[0].Columns[6].ColumnName].ToString();
                dgdetails[h, 7] = dr[ds.Tables[0].Columns[7].ColumnName].ToString();
                h++;
            }
            /*fin Details*/
            ViewBag.ligne = ligne;
            ViewBag.dgdetails = dgdetails;
            return PartialView();
        }
        public ActionResult button2_Clickchehconnection(object sender, EventArgs e)
        {
            string test = "";
            try
            {
                test = "1";
                //SqlCommand command;
                //SqlConnection conn = new SqlConnection(pbConnectionString);
                //command = new SqlCommand();
                //conn.Open();
                //command.Connection = conn;
               // conn.Close();
                SqlConnection cs = new SqlConnection(pbConnectionString);
                string CONNECTIONSTATUS = "CONNECTED TO " + cs.DataSource.ToString();

                /*showdata()*/
                string DataLine = "DOVIZ_KODU|DOVIZ_ADI|ALIS|SATIS|ALIS_MB|SATIS_MB|TARIH|SAAT|IND|DATED";
                string Sql = " select  [CurrCode] ,[CurrCountry],[Buying],[Selling],[CentralBank_Buying],[CentralBank_Selling],[DateUpdated] ,[TimeUpdated],[DATED]from [Ref].[BANKERRATE] where Ind='1'";
                SqlConnection con = new SqlConnection(pbConnectionString);
                DataSet ds = new DataSet();
                SqlDataAdapter ada = new SqlDataAdapter(Sql, con);
                ada.Fill(ds);
                string LASTUPDATED = "";
                int colone = ds.Tables[0].Columns.Count - 1;
                int ligne = ds.Tables[0].Rows.Count;  
                string[,] dataGridView1 = new string[ligne, colone];
                if (ligne > 1)
                {
                    int cnt = 0;
                    int R = 0;
                    LASTUPDATED = "";
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        LASTUPDATED = dr[ds.Tables[0].Columns[colone].ColumnName].ToString();

                        for (int col = 0; col < colone; col++)
                        {
                            try
                            {
                                dataGridView1[R, col] = dr[ds.Tables[0].Columns[col].ColumnName].ToString(); ;
                            }
                            catch { }
                            cnt++;
                        }
                        R++;
                    }
                }
                ViewBag.lblLastUpdated = "Last Downloaded " + LASTUPDATED;
                ViewBag.dataGridView1 = dataGridView1;
                ViewBag.colone = colone;
                ViewBag.ligne = ligne;
                ViewBag.lblCONNECTIONSTATUS = CONNECTIONSTATUS;
                /*fin showdata()*/
            }
            catch (Exception ex)
            {
                test = "0";
                int err = 0;
                string CONNECTIONSTATUS = "NOT CONNECTED";
                ViewBag.lblCONNECTIONSTATUS = CONNECTIONSTATUS;
            }
            ViewBag.test = test;
            return PartialView();
        }
        public ActionResult button1_Clickchehconnection(object sender, EventArgs e)
        {
            /* // using System.Net;
             ServicePointManager.Expect100Continue = true;
             ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
             // Use SecurityProtocolType.Ssl3 if needed for compatibility reasons

             // Create a request for the URL. 		
             WebRequest request = WebRequest.Create("http://www.paragaranti.com/gonline/fon_currency_xml?sid=1");
             // If required by the server, set the credentials.
             request.Credentials = CredentialCache.DefaultCredentials;
             // Get the response.
             HttpWebResponse response = (HttpWebResponse)request.GetResponse();
             // Display the status.
             Console.WriteLine(response.StatusDescription);
             // Get the stream containing content returned by the server.
             Stream dataStream = response.GetResponseStream();
             // Open the stream using a StreamReader for easy access.
             StreamReader reader = new StreamReader(dataStream);
             // Read the content.Lisez le contenu.
             string responseFromServer = reader.ReadToEnd();
             // Display the content.Affichez le contenu.
             Console.WriteLine(responseFromServer);
             // Cleanup the streams and the response.
             XmlDocument xmlDoc = new XmlDocument();
             xmlDoc.LoadXml(responseFromServer);*/


            /*// Encode the XML string in a UTF-8 byte array
            byte[] encodedString = Encoding.UTF8.GetBytes(xmlStr);
            // Put the byte array into a stream and rewind it to the beginning
            MemoryStream ms = new MemoryStream(encodedString);
            ms.Flush();
            ms.Position = 0;
            // Build the XmlDocument from the MemorySteam of UTF-8 encoded bytes
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.PreserveWhitespace = true;
            //xmlDoc.Load(ms);*/



            // using System.Net;
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            // Use SecurityProtocolType.Ssl3 if needed for compatibility reasons

            var m_strFilePath = "http://www.paragaranti.com/gonline/fon_currency_xml?sid=1";
            string xmlStr;
            using (var wc = new System.Net.WebClient())
            {
                xmlStr = wc.DownloadString(m_strFilePath);
            }
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.PreserveWhitespace = true;
            try
            {
                xmlDoc.LoadXml(xmlStr);
            }
            catch (System.IO.FileNotFoundException)
            {
                xmlDoc.LoadXml("<?xml version=\"1.0\"?> \n" +
                "<books xmlns=\"http://www.paragaranti.com/gonline/fon_currency_xml?sid=1\"> \n" +
                "  <book genre=\"novel\" ISBN=\"1-861001-57-8\" publicationdate=\"1823-01-28\"> \n" +
                "    <title>Pride And Prejudice</title> \n" +
                "    <price>24.95</price> \n" +
                "  </book> \n" +
                "  <book genre=\"novel\" ISBN=\"1-861002-30-1\" publicationdate=\"1985-01-01\"> \n" +
                "    <title>The Handmaid's Tale</title> \n" +
                "    <price>29.95</price> \n" +
                "  </book> \n" +
                "</books>");
            }


            DateTime dte = new DateTime();
                dte = DateTime.Now;
                string INSERTSQL = "INSERT INTO [Ref].[BANKERRATE] VALUES";
                string INSERTVALUES = "";
                string Ind = "1";
                string DataLine = "DOVIZ_KODU|DOVIZ_ADI|ALIS|SATIS|ALIS_MB|SATIS_MB|TARIH|SAAT|IND|DATED";
                DataLine = DataLine + Environment.NewLine;
                foreach (XmlNode node in xmlDoc.DocumentElement.ChildNodes)
                {
                    int c = node.ChildNodes.Count;
                    INSERTVALUES = "(";
                    for (int v = 0; v < c; v++)
                    {
                        string text = node.ChildNodes[v].InnerXml.ToString().Trim().Replace(',', '.') + "|"; //or loop through its children as well
                        INSERTVALUES = INSERTVALUES + "'" + text.Trim().Replace('|', ' ').Trim() + "',";
                    }
                    INSERTVALUES = INSERTVALUES + "'" + Ind + "','" + dte.ToString() + "')," + Environment.NewLine; ;
                    INSERTSQL = INSERTSQL + INSERTVALUES;
                }
                INSERTSQL = INSERTSQL.Substring(0, INSERTSQL.Trim().Length - 1);
                ViewBag.lblCONNECTIONSTATUS = UpDateDb("Update [Ref].[BANKERRATE] SET Ind='0'");
                ViewBag.lblCONNECTIONSTATUS = UpDateDb(INSERTSQL);
                /*showdata()*/
            //string DataLine = "DOVIZ_KODU|DOVIZ_ADI|ALIS|SATIS|ALIS_MB|SATIS_MB|TARIH|SAAT|IND|DATED";
            string Sql = " select  [CurrCode] ,[CurrCountry],[Buying],[Selling],[CentralBank_Buying],[CentralBank_Selling],[DateUpdated] ,[TimeUpdated],[DATED]from [Ref].[BANKERRATE] where Ind='1'";
                SqlConnection con = new SqlConnection(pbConnectionString);
                DataSet ds = new DataSet();
                SqlDataAdapter ada = new SqlDataAdapter(Sql, con);
                ada.Fill(ds);
                string LASTUPDATED = "";
                int colone = ds.Tables[0].Columns.Count - 1;
                int ligne = ds.Tables[0].Rows.Count;
                string[,] dataGridView1 = new string[ligne, colone];
                if (ligne > 1)
                {
                    int cnt = 0;
                    int R = 0;
                    LASTUPDATED = "";
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        LASTUPDATED = dr[ds.Tables[0].Columns[colone].ColumnName].ToString();

                        for (int col = 0; col < colone; col++)
                        {
                            try
                            {
                                dataGridView1[R, col] = dr[ds.Tables[0].Columns[col].ColumnName].ToString(); 
                            }
                            catch { }
                            cnt++;
                        }
                        R++;
                    }
                }
                ViewBag.lblLastUpdated = "Last Downloaded " + LASTUPDATED;
                ViewBag.dataGridView1 = dataGridView1;
                ViewBag.colone = colone;
                ViewBag.ligne = ligne;
            /*fin showdata()*/

            /*reader.Close();
            dataStream.Close();
            response.Close();*/
            return PartialView();
        }
        private string UpDateDb(string Sql)
        {

            try
            {
                SqlConnection con = new SqlConnection(pbConnectionString);
                SqlDataAdapter ada = new SqlDataAdapter(Sql, con);

                string CONNECTIONSTATUS = "Database updated";
                return CONNECTIONSTATUS;
            }
            catch (Exception ex)
            {
                //SendMessage(e.Message);
                int err = 0;
                string CONNECTIONSTATUS = "Update fail";
                return CONNECTIONSTATUS;
            }
        }
        public ActionResult button3_Clickinterlincorrespondence(object sender, System.EventArgs e)
        {
            int comboBox3index = Request.Form["comboBox3index"].AsInt();
            string comboBox3 = Request["comboBox3"].Trim();

            /*Getdata()*/
            string Sql = "SELECT [RecId],[DocumentNumber] as [Document No],[CouponNo] as [Coupon No],[ChargeCode] as [Charge Code],[CorrespondenceNo] as [Corr No] ,[InvoiceNumber] as [Invoice No]";
            Sql = Sql + " FROM [XmlFile].[Correspondences]";

            if (comboBox3index == 0)
                {
                    if (DocumentNumber.Trim().Length > 0)
                    {
                        Sql = Sql + "  WHERE   DocumentNumber='" + DocumentNumber + "'";
                    }
                }
                else
                {
                    string[] Part = comboBox3.ToString().Split('-');
                    Sql = Sql + "  WHERE   InvoiceNumber ='" + Part[0].ToString().Trim() + "'";

                    int Status = 0;
                    if (Part[1].ToString().Substring(0, 1) == "D") { Status = 1; } else { Status = 2; }
                    Sql = Sql + "  AND   Status =" + Status;
            }

            Sql = Sql + " ORDER BY [DocumentNumber],[CouponNo],[ChargeCode],[CorrespondenceNo],[Dated]";

            SqlConnection cs = new SqlConnection(pbConnectionString);
            DataSet ds = new DataSet();
            SqlDataAdapter ada = new SqlDataAdapter(Sql, cs);
            ada.Fill(ds);
            int ligne = ds.Tables[0].Rows.Count;
            int colone = ds.Tables[0].Columns.Count;
            string[,] dataGridView1 = new string[ligne, colone];

            int R = 0;
            if (ligne > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    for (int col = 0; col < colone; col++)
                    {
                            dataGridView1[R, col] = dr[ds.Tables[0].Columns[col].ColumnName].ToString();
                    }
                    R++;
                }
            }
            ViewBag.ligne = ligne;
            ViewBag.colone = colone;
            ViewBag.dataGridView1 = dataGridView1;
            return PartialView();
        }
        public ActionResult dataGridView1_CellClickcorrespondenceinterline()
        {
            int RowIndex = Request.Form["RowIndex"].AsInt();
            string value0 = Request["value0"].Trim();
            string value1 = Request["value1"].Trim();
            string value2 = Request["value2"].Trim();
            string value3 = Request["value3"].Trim();
            string value4 = Request["value4"].Trim();
            string value5 = Request["value5"].Trim();
            int RX = 0;
            if (RowIndex > -1)
            {
                RX = RowIndex;
            }
            else
            {
                RX = 0;
            }
            DocumentNumber = value1;
            CouponNo = value2;
            InvoiceNumber = value5;
            CoresNo = value4;
            CorrespondenceNo = CoresNo;
            ChargeCode = value3;

            ViewBag.DocumentNumber = DocumentNumber;
            ViewBag.CouponNo = CouponNo;
            ViewBag.InvoiceNumber = InvoiceNumber;
            ViewBag.CoresNo = CoresNo;
            ViewBag.CorrespondenceNo = CorrespondenceNo;
            ViewBag.ChargeCode = ChargeCode;

            string Sql = "SELECT * from [XmlFile].[Correspondences]";
            Sql = Sql + " where DocumentNumber='" + DocumentNumber + "'";
            Sql = Sql + " and CouponNo='" + CouponNo + "'";
            if (InvoiceNumber.Trim().Length > 0)
            {
                Sql = Sql + "  AND  InvoiceNumber='" + InvoiceNumber + "'";
            }
            if (CoresNo.Trim().Length > 0)
            {
                Sql = Sql + "  AND  [CorrespondenceNo]=" + CoresNo;
            }

            if (DocumentNumber.Trim().Length > 0)
            {
                Sql = Sql + "  and   DocumentNumber='" + DocumentNumber + "'";
            }



            SqlDataReader myReader;
            int R = 0;
            myReader = GetlistofTables(Sql);
            if (myReader != null)
            {
                if (myReader.HasRows == true)
                {
                    while (myReader.Read())
                    {
                        ViewBag.comboBox1SelectedIndex = myReader.GetInt32(4);//-1
                        ViewBag.txtAuthor = myReader.GetValue(5).ToString();
                        try {
                            if (!myReader.IsDBNull(6))
                            {
                                ViewBag.dateTimePicker1 = myReader.GetDateTime(6).ToString("yyyy-MM-dd"); ;
                            }
                            else
                            {
                                ViewBag.dateTimePicker1 = DateTime.Now.ToString("dd-MMM-yyyy");
                            }
                        }
                        catch
                        {

                        };
                        try { ViewBag.txtDateLimit = myReader.GetDateTime(7).ToString("yyyy-MM-dd"); }catch { }
                        ViewBag.txtDocRef = myReader.GetValue(8).ToString();
                        ViewBag.txtInvoiceNO = myReader.GetValue(9).ToString();
                        ViewBag.txtDocAmt = myReader.GetValue(10).ToString();
                        ViewBag.txtAccepted = myReader.GetValue(11).ToString();
                        ViewBag.txtDisputed = myReader.GetValue(12).ToString();
                        ViewBag.comboBox2SelectedIndex = myReader.GetInt32(14);//-1
                        try {
                            if (!myReader.IsDBNull(15))
                            {
                                ViewBag.dateTimePicker2 = myReader.GetDateTime(15).ToString("yyyy-MM-dd"); 
                            }
                            else
                            {
                                ViewBag.dateTimePicker2 = DateTime.Now.ToString("dd-MMM-yyyy");
                            }
                        } catch { };
                    }
                }
            }
            myReader.Close();
            myReader.Dispose();
            myReader = null;
            /*GetMessage()*/
            SqlConnection conn = new SqlConnection(pbConnectionString);
            conn.Open();
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.Text;
            cmdGet.Connection = conn;
            string Sql1 = "SELECT TOP 1 [XmlFile].[Correspondences].[MESSAGE] FROM [XmlFile].[Correspondences]";
            Sql1 = Sql1 + "  WHERE [DocumentNumber] ='" + DocumentNumber + "'";
            Sql1 = Sql1 + " AND [CouponNo]='" + CouponNo + "'";
            Sql1 = Sql1 + " AND[ChargeCode]='" + ChargeCode + "'";
            Sql1 = Sql1 + " AND [CorrespondenceNo]=" + CorrespondenceNo;
            cmdGet.CommandText = Sql1;
            int nb = cmdGet.ExecuteScalar().ToString().Length - 140;
            if (cmdGet.ExecuteScalar().ToString().Trim() != "") { 
                ViewBag.richTextBox1 = cmdGet.ExecuteScalar().ToString().Substring(132, nb);
            }
            else
            {
                ViewBag.richTextBox1 = cmdGet.ExecuteScalar().ToString();
            }
            conn.Close();
            /*fin GetMessage()*/
            return PartialView();
        }
        public string Getcarrier()
        {
            string value1 = Request["value1"].Trim();
            DocumentNumber = value1;

            string RetVal = "";

            string Sql = "SELECT [AirlineCode],[AirlineID],[AirlineName]  FROM [Ref].[Airlines]";
            Sql = Sql + " where [AirlineID]='" + DocumentNumber.Substring(0, 3) + "'";

            SqlConnection cs = new SqlConnection(pbConnectionString);
            DataSet ds = new DataSet();
            SqlDataAdapter ada = new SqlDataAdapter(Sql, cs);
            ada.Fill(ds);
            int ligne = ds.Tables[0].Rows.Count;

            SqlDataReader myReader;
            myReader = GetlistofTables(Sql);

            if (myReader != null)
            {
                if (myReader.HasRows == true)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        RetVal = dr[ds.Tables[0].Columns[0].ColumnName].ToString() + " - " + dr[ds.Tables[0].Columns[1].ColumnName].ToString() + " - " + dr[ds.Tables[0].Columns[2].ColumnName].ToString();
                    }
                }
            }
            myReader.Close();
            myReader.Dispose();
            myReader = null;

            return RetVal;
        }
        public void button1_Clickinterlinecorrespondence()
        {
            DocumentNumber = Request["DocumentNumber"].Trim();
            CouponNo = Request["CouponNo"].Trim();
            ChargeCode = Request["ChargeCode"].Trim();
            CorrespondenceNo = Request["CorrespondenceNo"].Trim();
            string lcCorrespondenceNo = Request["comboBox1"].Trim();
            string Author = Request["txtAuthor"].Trim();
            string Dated1 = Request["dateTimePicker1"].Trim();
            string DateLimit1 = Request["txtDateLimit"].Trim();
            string DocumentReference = Request["txtDocRef"].Trim();
            string InvoiceNumber = Request["txtInvoiceNO"].Trim();
            string DocumentAmount = Request["txtDocAmt"].Trim();
            DocumentAmount = DocumentAmount.Replace(",", ".");
            string AmountAccepted = Request["txtAccepted"].Trim();
            AmountAccepted = AmountAccepted.Replace(",", ".");
            string DisputeAmount = Request["txtDisputed"].Trim();
            DisputeAmount = DisputeAmount.Replace(",", ".");
            string Message = Request["richTextBox1"].Trim(); 
            int cx = Request.Form["comboBox2SelectedIndex"].AsInt();
            string Status = cx.ToString();
            string ResolvedDate1 = Request["dateTimePicker2"].Trim();
            
            CorrespondenceNo = lcCorrespondenceNo;

            SqlDataAdapter da = new SqlDataAdapter();
            string Sql1 = "SELECT *  FROM [XmlFile].[Correspondences] where ";
            Sql1 = Sql1 + " [DocumentNumber] ='" + DocumentNumber + "'";
            Sql1 = Sql1 + " AND [CouponNo]='" + CouponNo + "'";
            Sql1 = Sql1 + " AND[ChargeCode]='" + ChargeCode + "'";
            Sql1 = Sql1 + " AND [CorrespondenceNo]=" + CorrespondenceNo;
            string Sql = "IF NOT EXISTS(" + Sql1 + ")";
            Sql = Sql + " BEGIN" + Environment.NewLine;
            Sql = Sql + "DECLARE @Lineid bigint;DECLARE @Lineid2 bigint;" + Environment.NewLine;
            Sql = Sql + "set @Lineid = (select iif(MAX(recId) is null,1, MAX(RecId)+1) As MaxLineid from [XmlFile].[Correspondences]);" + Environment.NewLine; ;
            Sql = Sql + " INSERT INTO [XmlFile].[Correspondences] ([Recid] ,[DocumentNumber],[CouponNo],[ChargeCode],[CorrespondenceNo],[Author],[Dated],[DateLimit],[DocumentReference],[InvoiceNumber],[DocumentAmount],[AmountAccepted],[DisputeAmount],[Status],[ResolvedDate]) ";
            Sql = Sql + " VALUES(@Lineid,";
            Sql = Sql + " '" + DocumentNumber + "','" + CouponNo + "','" + ChargeCode + "','" + CorrespondenceNo + "','" + Author + "',";
            if (ResolvedDate1 != "")
            {
                DateTime Dated = DateTime.Parse(Dated1);
                Sql = Sql + "'" + Dated.ToString("yyyy-MM-dd") + "',";
            }
            else
            {
                Sql = Sql + "NULL,";
            }

            if (DateLimit1 != "")
            {
                DateTime DateLimit = DateTime.Parse(DateLimit1);
                Sql = Sql + "'" + DateLimit + "',";
            }
            else
            {
                Sql = Sql + "NULL,";
            }
            Sql = Sql + "'" + DocumentReference + "','" + InvoiceNumber + "','" + DocumentAmount + "','" + AmountAccepted + "','" + DisputeAmount + "','" + Status + "',";
            if (ResolvedDate1 != "")
            {
                DateTime ResolvedDate = DateTime.Parse(ResolvedDate1);
                Sql = Sql + "'" + ResolvedDate.ToString("yyyy-MM-dd") + "'";
            }
            else
            {
                Sql = Sql + "NULL";
            }
            Sql = Sql + " )" + Environment.NewLine;
            Sql = Sql + " END " + Environment.NewLine;
            Sql = Sql + " ELSE " + Environment.NewLine;
            Sql = Sql + " BEGIN" + Environment.NewLine;
            Sql = Sql + "UPDATE [XmlFile].[Correspondences] SET ";
            Sql = Sql + "[Author] ='" + Author + "',";
            if (Dated1 != "")
            {
                DateTime Dated = DateTime.Parse(Dated1);
                Sql = Sql + "[Dated] ='" + Dated.ToString("yyyy-MM-dd") + "',";
            }
            else
            {
                Sql = Sql + "[Dated] = NULL,";
            }
            if (DateLimit1 != "")
            {
                DateTime DateLimit = DateTime.Parse(DateLimit1);
                Sql = Sql + "[DateLimit] ='" + DateLimit + "',";
            }
            else
            {
                Sql = Sql + "[DateLimit] = NULL,";
            }
            Sql = Sql + "[DocumentReference] ='" + DocumentReference + "',";
            Sql = Sql + "[InvoiceNumber] ='" + InvoiceNumber + "',";
            Sql = Sql + "[DocumentAmount] ='" + DocumentAmount + "',";
            Sql = Sql + "[AmountAccepted] ='" + AmountAccepted + "',";
            Sql = Sql + "[DisputeAmount] ='" + DisputeAmount + "',";
            Sql = Sql + "[Status] =" + Status + ",";
            if (ResolvedDate1 != "")
            {
                DateTime ResolvedDate = DateTime.Parse(ResolvedDate1);
                Sql = Sql + "[ResolvedDate] ='" + ResolvedDate.ToString("yyyy-MM-dd") + "'";
            }
            else
            {
                Sql = Sql + "[ResolvedDate] = NULL ";
            }
            Sql = Sql + " WHERE [DocumentNumber] ='" + DocumentNumber + "'";
            Sql = Sql + " AND [CouponNo]='" + CouponNo + "'";
            Sql = Sql + " AND[ChargeCode]='" + ChargeCode + "'";
            Sql = Sql + " AND [CorrespondenceNo]=" + CorrespondenceNo;
            Sql = Sql + " END " + Environment.NewLine;
            DataSet ds = new DataSet();
            ds = dbconnect.RetObjDS(Sql);
            bool RetVal = DbUpdate(Sql);
            if (Message.Length > 0)
            {
                Sql = "UPDATE [XmlFile].[Correspondences] SET  [XmlFile].[Correspondences].[MESSAGE] = @prichTextBox1";
                Sql = Sql + "  WHERE [DocumentNumber] ='" + DocumentNumber + "'";
                Sql = Sql + " AND [CouponNo]='" + CouponNo + "'";
                Sql = Sql + " AND[ChargeCode]='" + ChargeCode + "'";
                Sql = Sql + " AND [CorrespondenceNo]=" + CorrespondenceNo;
                try
                {
                    SqlCommand command;
                    SqlConnection conn = new SqlConnection(pbConnectionString);
                    conn.Open();
                    command = new SqlCommand();
                    string SqlSelect = Sql;
                    command.Connection = conn;
                    command.CommandType = CommandType.Text;
                    command.CommandText = SqlSelect;
                    command.CommandTimeout = 0;
                    string test = " ";
                    SqlParameter parrichTextBox1 = new
                    SqlParameter("@prichTextBox1", SqlDbType.VarChar);
                    parrichTextBox1.Value = PlainTextToRtf(Message);
                    //"{\\rtf1\\ansi\\ansicpg1252\\deff0\\deflang1036{\\fonttbl{\\f0\\fnil\\fcharset0" + ' ' + "Calibri;}}\\viewkind4\\uc1\\pard\\f0\\fs23" + ' '+"" +Message+ "\\par"+' '+"}";
                    command.Parameters.Add(parrichTextBox1);
                    command.ExecuteNonQuery();
                    while (conn.State == ConnectionState.Executing)
                    {
                    }
                    conn.Close();
                }
                catch (Exception ex)
                {
                    int Rtt = 0;
                }
            }
        }
        /*conver rtf*/
        public string PlainTextToRtf(string plainText)
        {
            if (string.IsNullOrEmpty(plainText))
                return "";

            string escapedPlainText = plainText.Replace(@"\", @"\\").Replace("{", @"\{").Replace("}", @"\}");

            byte[] myBytes = Encoding.UTF8.GetBytes(escapedPlainText);
            escapedPlainText = Encoding.UTF8.GetString(myBytes);

             string rtf = @"{\rtf1\ansi\ansicpg1250\deff0{\fonttbl{\f0\fswiss\fcharset0 Helvetica;}{\f1\fswiss Helvetica;}}\viewkind4\uc1\pard\lang1036\f0\fs24 ";
             rtf = rtf + escapedPlainText.Replace(Environment.NewLine, "\\par\r\n ") + @"\f1\par}";

            return rtf;
        }
        /*fin conver rtf*/
        public ActionResult divselectcorrespondance()
        {
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
            return PartialView();
        }
        public string onchangecorrespondance()
        {
            CorrespondenceNo = Request["value"].Trim();
            string val = "";
            string Sql = "SELECT *  FROM [XmlFile].[Correspondences] where ";
            Sql = Sql + "[CorrespondenceNo]=" + CorrespondenceNo;
            DataSet ds = new DataSet();
            ds = dbconnect.RetObjDS(Sql);
            int ligne = ds.Tables[0].Rows.Count;
            if (ligne > 0)
            {
                val = "true";
            }
            else
            {
                val = "false";
            }
            return val;
        }
        /********fin fait par christian*****/


















        public ActionResult FinalSharevsAmountCollected()
        {
            return PartialView();
        }
        public ActionResult FileUploadStatus() {
            string dateFrom = DateTime.Now.AddMonths(-1).ToString("dd-MMM-yyyy");
            string dateTo = DateTime.Now.ToString("dd-MMM-yyyy");
            string[] date = new string[2] { dateFrom, dateTo };
            ViewBag.date = date;
            return PartialView();
        }
        public ActionResult InvolontaryReroute()
        {
            string dateFrom = DateTime.Now.AddMonths(-1).ToString("dd-MMM-yyyy");
            string dateTo = DateTime.Now.ToString("dd-MMM-yyyy");
            string[] date = new string[2] { dateFrom, dateTo };
            ViewBag.date = date;
            return PartialView();
        }







        public ActionResult DuplicateUtilisation() {
            string dateFrom = DateTime.Now.AddMonths(-1).ToString("dd-MMM-yyyy");
            string dateTo = DateTime.Now.ToString("dd-MMM-yyyy");
            string[] date = new string[2] { dateFrom, dateTo };
            ViewBag.date = date;
            return PartialView();
        }
        public ActionResult EMDTransactionScreen()
        {
            string dateFrom = DateTime.Now.AddMonths(-1).ToString("dd-MMM-yyyy");
            string dateTo = DateTime.Now.ToString("dd-MMM-yyyy");
            string[] date = new string[2] { dateFrom, dateTo };
            ViewBag.date = date;
            return PartialView();
        }
        public ActionResult ExcessBaggageProration() {
            string dateFrom = DateTime.Now.AddMonths(-1).ToString("dd-MMM-yyyy");
            string dateTo = DateTime.Now.ToString("dd-MMM-yyyy");
            string[] date = new string[2] { dateFrom, dateTo };
            ViewBag.date = date;
            return PartialView();
        }
        public ActionResult DiscountAudit()
        {
            string dateFrom = DateTime.Now.AddMonths(-1).ToString("dd-MMM-yyyy");
            string dateTo = DateTime.Now.ToString("dd-MMM-yyyy");
            string[] date = new string[2] { dateFrom, dateTo };
            ViewBag.date = date;
            return PartialView();
        }
        public ActionResult ATDA() {
            string dateFrom = DateTime.Now.AddMonths(-1).ToString("dd-MMM-yyyy");
            string dateTo = DateTime.Now.ToString("dd-MMM-yyyy");
            string[] date = new string[2] { dateFrom, dateTo };
            ViewBag.date = date;
            return PartialView();
        }
        public ActionResult ManualTicketList() {
            string dateFrom = DateTime.Now.AddMonths(-1).ToString("dd-MMM-yyyy");
            string dateTo = DateTime.Now.ToString("dd-MMM-yyyy");
            string[] date = new string[2] { dateFrom, dateTo };
            ViewBag.date = date;
            return PartialView();
        }
        public ActionResult DateUpdated()
        {
            string dateFrom = DateTime.Now.AddMonths(-1).ToString("dd-MMM-yyyy");
            string dateTo = DateTime.Now.ToString("dd-MMM-yyyy");
            string[] date = new string[2] { dateFrom, dateTo };
            ViewBag.date = date;
            return PartialView();
        }
        public ActionResult DocumentNumberM()
        {
            string dateFrom = DateTime.Now.AddMonths(-1).ToString("dd-MMM-yyyy");
            string dateTo = DateTime.Now.ToString("dd-MMM-yyyy");
            string[] date = new string[2] { dateFrom, dateTo };
            ViewBag.date = date;
            return PartialView();
        }

        /*****************Mihaja beging*******************/
        public ActionResult PassengerNameList()
        {
            return PartialView();
        }

        public ActionResult SalesDataUploadSummary()
        {
            return PartialView();
        }

        public ActionResult FinalShareValidation()
        {
            //fait par christian
            string dateFrom = DateTime.Now.AddMonths(-1).ToString("dd-MMM-yyyy");
            string dateTo = DateTime.Now.ToString("dd-MMM-yyyy");
            string[] date = new string[2] { dateFrom, dateTo };
            ViewBag.date = date;
            //and fait par christian
            return PartialView();
        }

        public ActionResult PricingManagement()
        {
            //fait par christian
            string dateFrom = DateTime.Now.AddMonths(-1).ToString("dd-MMM-yyyy");
            string dateTo = DateTime.Now.ToString("dd-MMM-yyyy");
            string[] date = new string[2] { dateFrom, dateTo };
            ViewBag.date = date;
            //and fait par christian
            return PartialView();
        }
        public ActionResult CouponUsage()
        {
            //fait par christian
            string dateFrom = DateTime.Now.AddMonths(-1).ToString("dd-MMM-yyyy");
            string dateTo = DateTime.Now.ToString("dd-MMM-yyyy");
            string[] date = new string[2] { dateFrom, dateTo };
            ViewBag.date = date;
            ViewBag.dtpToChecked = "";
            ViewBag.dtpFromChecked = "";
            //and fait par christian
            return PartialView();
        }
        public ActionResult LiftInfo()
        {
            return PartialView();
        }

        public ActionResult Provisio()
        {
            return PartialView();
        }
        public ActionResult RefundAudit()
        {
            //fait par christian
            string dateFrom = DateTime.Now.AddMonths(-1).ToString("dd-MMM-yyyy");
            string dateTo = DateTime.Now.ToString("dd-MMM-yyyy");
            string[] date = new string[2] { dateFrom, dateTo };
            ViewBag.date = date;
            //and fait par christian

            return PartialView();
        }
      
        /*****************mihaja end**********************/
        /*****************Tolotra**********************/
        public ActionResult OutwardBillingManual()
        {

            /*FAIT PAR NOMENTSOA Christian*/

            /************ Get Airline code ****/
            string sql = "Select distinct BALC   FROM [Pax].[OutwardBilling] order by BALC asc";
            SqlConnection con = new SqlConnection(pbConnectionString);
            DataSet ds = new DataSet();
            SqlDataAdapter ada = new SqlDataAdapter(sql, con);
            ada.Fill(ds);
            con.Close();

            int ligne = ds.Tables[0].Rows.Count;

            string[,] Listeairlinecode = new string[1, ligne];

            int i = 0;

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                Listeairlinecode[0, i] = dr[ds.Tables[0].Columns[0].ColumnName].ToString();

                i++;
            }

            ViewBag.ligne = ligne;
            ViewBag.Listeairlinecode = Listeairlinecode;
            /************End Get Airline code ****/


            /************ Get Billing period ****/
            string sql1 = "Select distinct BILLINGPERIOD   FROM [Pax].[OutwardBilling] ";
            sql1 = sql1 + " order by BILLINGPERIOD Desc";

            SqlConnection con1 = new SqlConnection(pbConnectionString);
            DataSet ds1 = new DataSet();
            SqlDataAdapter ada1 = new SqlDataAdapter(sql1, con1);
            ada1.Fill(ds1);
            con1.Close();

            int ligneBillingperiod = ds1.Tables[0].Rows.Count;

            string[,] Billingperiod = new string[1, ligneBillingperiod];

            int ii = 0;

            foreach (DataRow dr1 in ds1.Tables[0].Rows)
            {
                Billingperiod[0, ii] = dr1[ds1.Tables[0].Columns[0].ColumnName].ToString();

                ii++;
            }

            ViewBag.ligneBillingperiod = ligneBillingperiod;
            ViewBag.Billingperiod = Billingperiod;
            /************End Get Billing period ****/

            /*End NOMENTSOA Christian*/


            /*  OutwardBilling Manual     Joseph*/

            string airlineCode = Listeairlinecode[0, 0];
            string billingPeriod = Billingperiod[0, 0];

            GetSystemParameter();
            GetDataOutwardGeneral(airlineCode, billingPeriod,"");


            /*  XML Billing Manual    */
            LoadCboXmlBillingPeriod();

            /* End  OutwardBilling Manual     Joseph*/


            return PartialView();
        }
        public void GetAirlineCodej() { 
}





        public ActionResult BankersRate()
        {
            return PartialView();
        }
        /************************FreeAndReduced*******************/

        public ActionResult FreeAndReduced()
        {
            ViewBag.dateFrom = DateTime.Now.AddMonths(-1).ToString("dd-MMM-yyyy");
            ViewBag.dateTo = DateTime.Now.ToString("dd-MMM-yyyy");
            return PartialView();
        }
        public ActionResult LoadFreeAndReduced()
        {
            string dateFrom1 = Request["dateFrom"];
            string dateFrom = ConvertDate(dateFrom1);
            string dateTo1 = Request["dateTo"];
            string dateTo = ConvertDate(dateTo1);
            string Exc = Request["excChk"];

            string page = "1";              // For Pagination 
            string record = "150";
            //checkexc();
            //RebateNDiscount();

            string sql = "DECLARE @ExcludeExchange int= " + Exc + ";" + Environment.NewLine;
            sql = sql + "DECLARE  @PageNo int= " + page + ";" + Environment.NewLine;
            sql = sql + "DECLARE  @RecordsPerPage int = " + record + ";" + Environment.NewLine;
            sql = sql + "with a as ( select ROW_NUMBER()OVER(ORDER BY f1.DateofIssue,f1.DocumentNumber) AS ROWNUM,f1.DateofIssue ,f1.DocumentNumber, f1.OriginalIssueDocumentNumber, f1.PassengerName, f1.ITBT, f1.ExchangeADC  ";
            sql = sql + ", iif( exists(select * from Pax.salesdocumentpayment sdp  where sdp.hdrguidref = f1.hdrguid and sdp.FormofPaymentType = 'EX' ), 'Y', '' ) as Exchange  ";
            sql = sql + ", Pax.fn_FareBasis_Combined( F1.hdrguid ) as FareBasis, f1.FareCalculationModeIndicator as FCMI, FareCurrency, f1.Fare, f1.TotalCurrency, f1.EquivalentFare  ";
            sql = sql + ", f1.AmountCollectedCurrency, f1.AmountCollected, isnull(f1.AmountCollected,0) - isnull(f1.TaxCollected,0) - isnull(f1.SurchargeCollected,0) as FareCollected  ";
            sql = sql + ", f1.TaxCollected, f1.SurchargeCollected from Pax.SalesDocumentHeader f1 where f1.DateofIssue between '" + dateFrom + "'and '" + dateTo + "'   ";
            sql = sql + "and f1.TransactionCode = 'TKTT' and f1.SalesDataAvailable = 1 	and isnull( f1.ExchangeADC, '' )   ";
            sql = sql + "<> 'NO ADC'and ( isnull(f1.AmountCollected,0) - isnull(f1.TaxCollected,0) - isnull(f1.SurchargeCollected,0) = 0  and   ";
            sql = sql + "not exists ( select * from Pax.VW_SalesCouponDetail sdc where sdc.HdrGuid = f1.HdrGuid and sdc.Discount is not null)))  ";
            sql = sql + "select * from a where CHARINDEX( 'CHARTER', FareBasis )  = 0 and CHARINDEX( '/DIS', FareBasis )  = 0  ";
            sql = sql + "and ( @ExcludeExchange = 0 or @ExcludeExchange = 1  and Exchange <> 'Y' )  ORDER BY DateofIssue,DocumentNumber OFFSET (@PageNo-1)*@RecordsPerPage ROWS FETCH NEXT @RecordsPerPage ROWS ONLY";
            SqlConnection con = new SqlConnection(pbConnectionString);
            DataSet ds = new DataSet();
            SqlDataAdapter ada = new SqlDataAdapter(sql, con);
            ada.Fill(ds);
            con.Close();
            int lon = ds.Tables[0].Rows.Count;
            if (lon == 0)
            {
                ViewBag.mes = "No data available for the selected criteria.";
            }
            return PartialView(ds);
        }
        /************************End FreeAndReduced*******************/
        public ActionResult LiftedCoupons()
        {
            /*fait par christian LiftedCoupons*/
            ViewBag.dateFrom = DateTime.Now.AddMonths(-1).ToString("dd-MMM-yyyy");
            ViewBag.dateTo = DateTime.Now.ToString("dd-MMM-yyyy");
            /*fin fait par christian LiftedCoupons*/
            return PartialView();
        }
        /************************Special Purpose*******************/

        public ActionResult SpecialPurpose()
        {
            ViewBag.dateFrom = DateTime.Now.AddMonths(-1).ToString("dd-MMM-yyyy");
            ViewBag.dateTo = DateTime.Now.ToString("dd-MMM-yyyy");
            return PartialView();

        }
        public ActionResult LoadSpecialPurpose()
        {
            string dateFrom1 = Request["dateFrom"];
            string dateFrom = ConvertDate(dateFrom1);
            string dateTo1 = Request["dateTo"];
            string dateTo = ConvertDate(dateTo1);
            string cbospc = Request["agspc"];

            string s = "";
            if (cbospc != "-All-")
            {
                s = "AND f1.PaxSPC = '" + cbospc + "'";
            }
            string sql = "";
            sql = sql + "select f1.DateofIssue,f1.DocumentNumber,f1.PassengerName, f1.TransactionCode,   f1.PaxSPC, f3.[Description], f1.TrueOriginDestinationCityCodes as Journey ,f1.FareCalculationModeIndicator as FCMI, ";
            sql = sql + " f2.FareBasisTicketDesignator as FareBasis , f1.FareCurrency, f1.Fare, f1.TotalCurrency as EquivalentFareCurrency,f1.EquivalentFare, f1.TotalCurrency, f1.TotalAmount,  F1.AmountCollectedCurrency, f1.AmountCollected, ";
            sql = sql + " f1.TaxCollected, f1.SurchargeCollected , f1.EndosRestriction, f1.FareCalculationArea as FCA, pax.fn_FOP_Combined(f1.hdrguid) as FOP ";
            sql = sql + " from Pax.SalesDocumentHeader  f1 join pax.SalesRelatedDocumentInformation sdi on sdi.HdrGuid = f1.HdrGuid and f1.PaxSPC is not null join Pax.SalesDocumentCoupon f2 on sdi.RelatedDocumentGuid = f2.RelatedDocumentGuid  left join ref.SPC f3 on f1.PaxSPC = f3.SPC where f1.DateofIssue between '" + dateFrom + "'and '" + dateTo + "'";
            sql = sql + s;
            SqlConnection con = new SqlConnection(pbConnectionString);
            DataSet ds = new DataSet();
            SqlDataAdapter ada = new SqlDataAdapter(sql, con);
            ada.Fill(ds);
            con.Close();
            int lon = ds.Tables[0].Rows.Count;
            if (lon == 0)
            {
                ViewBag.mes = "No data available for the selected criteria.";
            }
            ViewBag.txtCount = lon;

            return PartialView(ds);
        }
        public ActionResult LoadAgtSPC()
        {
            string dateFrom = Request["dateFrom"];
            //string dateFrom = ConvertDate(dateFrom1);
            string dateTo = Request["dateTo"];
            //string dateTo = ConvertDate(dateTo1);
            string sql = "";
            sql = sql + "select distinct PaxSPC from pax.SalesDocumentHeader where PaxSPC is not null and DateofIssue between '" + dateFrom + "' and '" + dateTo + "'";

            SqlConnection con1 = new SqlConnection(pbConnectionString);
            DataSet ds1 = new DataSet();
            SqlDataAdapter ada1 = new SqlDataAdapter(sql, con1);
            ada1.Fill(ds1);
            con1.Close();
            ViewBag.form2 = ds1;
            return PartialView();
        }
        /************************End Special Purpose*******************/

        /************************POS SUmmary*******************/
        public ActionResult POSSummary()
        {
            ViewBag.dateFrom = DateTime.Now.AddMonths(-1).ToString("dd-MMM-yyyy");
            ViewBag.dateTo = DateTime.Now.ToString("dd-MMM-yyyy");
            return PartialView();

        }
        public ActionResult LoadPOS()
        {
            string dateFrom = Request["dateFrom"];
            //string dateFrom = ConvertDate(dateFrom1);
            string dateTo = Request["dateTo"];
            //string dateTo = ConvertDate(dateTo1);
            string sql = " ";
            sql += " select distinct isnull(agt.LocationCountry,'Missing Agent Info') as POS from Pax.SalesDocumentHeader sdh   " + Environment.NewLine;
            sql += " left join ref.VW_Agent agt on agt.AgencyNumericCode = left(sdh.AgentNumericCode,7)  " + Environment.NewLine;
            sql += " where DateofIssue between '" + dateFrom + "' and  '" + dateTo + "' and agt.LocationCountry <> ''  " + Environment.NewLine;

            SqlConnection con1 = new SqlConnection(pbConnectionString);
            DataSet ds1 = new DataSet();
            SqlDataAdapter ada1 = new SqlDataAdapter(sql, con1);
            ada1.Fill(ds1);
            con1.Close();
            ViewBag.form2 = ds1;
            return PartialView();
        }
        public ActionResult LoadFCMI()
        {
            string dateFrom = Request["dateFrom"];
            //string dateFrom = ConvertDate(dateFrom1);
            string dateTo = Request["dateTo"];
            //string dateTo = ConvertDate(dateTo1);
            string sql = " ";
            sql += " select distinct sdh.FareCalculationModeIndicator from Pax.SalesDocumentHeader sdh   " + Environment.NewLine;
            sql += " where sdh.DateofIssue between '" + dateFrom + "' and  '" + dateTo + "' and FareCalculationModeIndicator <> '' " + Environment.NewLine;

            SqlConnection con1 = new SqlConnection(pbConnectionString);
            DataSet ds1 = new DataSet();
            SqlDataAdapter ada1 = new SqlDataAdapter(sql, con1);
            ada1.Fill(ds1);
            con1.Close();
            ViewBag.form2 = ds1;
            return PartialView();
        }
        public ActionResult LoadFareBasis()
        {
            string dateFrom = Request["dateFrom"];
            //string dateFrom = ConvertDate(dateFrom1);
            string dateTo = Request["dateTo"];
            //string dateTo = ConvertDate(dateTo1);
            string sql = " ";
            sql += " select distinct sdc.FareBasisTicketDesignator from Pax.SalesDocumentHeader sdh   " + Environment.NewLine;
            sql += " join pax.SalesRelatedDocumentInformation srd on sdh.HdrGuid = srd.HdrGuid   " + Environment.NewLine;
            sql += " join pax.SalesDocumentCoupon sdc on srd.RelatedDocumentGuid =sdc.RelatedDocumentGuid   " + Environment.NewLine;
            sql += " where DateofIssue between '" + dateFrom + "' and  '" + dateTo + "'  " + Environment.NewLine;
            SqlConnection con1 = new SqlConnection(pbConnectionString);
            DataSet ds1 = new DataSet();
            SqlDataAdapter ada1 = new SqlDataAdapter(sql, con1);
            ada1.Fill(ds1);
            con1.Close();
            ViewBag.form2 = ds1;
            return PartialView();
        }
        public ActionResult LoadFopPOS()
        {
            string dateFrom = Request["dateFrom"];
            //string dateFrom = ConvertDate(dateFrom1);
            string dateTo = Request["dateTo"];
            //string dateTo = ConvertDate(dateTo1);
            string sql = " ";
            sql += " select distinct sdp.FormofPaymentType from Pax.SalesDocumentHeader sdh  " + Environment.NewLine;
            sql += " join pax.SalesRelatedDocumentInformation srd on sdh.HdrGuid = srd.HdrGuid and sdh.OwnTicket ='Y'  " + Environment.NewLine;
            sql += " join pax.SalesDocumentCoupon sdc on sdc.RelatedDocumentGuid = srd.RelatedDocumentGuid  " + Environment.NewLine;
            sql += " left join Pax.SalesDocumentPayment sdp on srd.RelatedDocumentGuid = sdp.RelatedDocumentGuid and sdh.DateofIssue = sdp.DateofIssue and sdh.TransactionCode = sdp.TransactionCode  " + Environment.NewLine;
            sql += " where sdh.DateofIssue between '" + dateFrom + "' and  '" + dateTo + "' and FormofPaymentType <> '' " + Environment.NewLine;

            SqlConnection con1 = new SqlConnection(pbConnectionString);
            DataSet ds1 = new DataSet();
            SqlDataAdapter ada1 = new SqlDataAdapter(sql, con1);
            ada1.Fill(ds1);
            con1.Close();
            ViewBag.form2 = ds1;
            return PartialView();
        }
        public ActionResult LoadAgtPOS()
        {
            string dateFrom = Request["dateFrom"];
            //string dateFrom = ConvertDate(dateFrom1);
            string dateTo = Request["dateTo"];
            //string dateTo = ConvertDate(dateTo1);
            string sql = "select distinct AgentNumericCode  from [Pax].[SalesDocumentHeader]  where DateofIssue between '" + dateFrom + "' and '" + dateTo + "' and SalesDataAvailable = 1 ";

            SqlConnection con1 = new SqlConnection(pbConnectionString);
            DataSet ds1 = new DataSet();
            SqlDataAdapter ada1 = new SqlDataAdapter(sql, con1);
            ada1.Fill(ds1);
            con1.Close();
            ViewBag.form2 = ds1;
            return PartialView();
        }

        public ActionResult LoadPOSSummary()
        {
            string dateFrom1 = Request["dateFrom"];
            string dateFrom = ConvertDate(dateFrom1);
            string dateTo1 = Request["dateTo"];
            string dateTo = ConvertDate(dateTo1);
            string cboAgt = Request["agpos"];
            string cboPoS = Request["namepos"];
            string cboFCMI = Request["fcmi"];
            string cboFOPFarebasis = Request["farebas"];
            string cboFOP = Request["fopP"];

            string _FOP = "%";
            string _POS = "%";
            string _FCMI = "%";
            string FOPFB = "%";
            string _Agt = "%";

            string a = "";
            string b = "";
            string c = "";
            string d = "";
            string ee = "";
            string f = "";
            string g = "";
            string h = "";
            string i = "";

            if (cboFOP == "-All-")
            {
                _FOP = "%";
            }
            else
            {
                _FOP = cboFOP;
            }

            if (cboPoS == "-All-")
            {
                _POS = "%";
            }
            else
            {
                _POS = cboPoS;
            }
            if (cboFCMI == "-All-")
            {
                _FCMI = "%";
            }
            else
            {
                _FCMI = cboFCMI;
            }
            if (cboFOPFarebasis == "-All-")
            {
                FOPFB = "%";
            }
            else
            {
                FOPFB = cboFOPFarebasis;
            }
            if (cboAgt == "-All-")
            {
                _Agt = "%";
            }
            else
            {
                _Agt = cboAgt;
            }


            string sql = "";
            sql += " DECLARE @selection INT =1  " + Environment.NewLine;
            sql += "    " + Environment.NewLine;
            sql += " IF OBJECT_ID('tempdb..#base') IS NOT NULL DROP TABLE #base  " + Environment.NewLine;
            sql += "     " + Environment.NewLine;
            sql += "  SELECT sdh.DateofIssue as [Date of Issue] ,DATENAME(dw,sdh.DateofIssue) AS [Purchase Day],sdh.AgentNumericCode as [Agent Numeric Code]    " + Environment.NewLine;
            sql += "  ,isnull(agt.LegalName,'Missing Agent Info') AS [Agent Name]    " + Environment.NewLine;
            sql += " , CASE   " + Environment.NewLine;
            sql += " WHEN  agt.LegalName is  null and  agt.LocationCountry is null  THEN 'Missing Agent Info'  " + Environment.NewLine;
            sql += " WHEN agt.LegalName is not null and  agt.LocationCountry is not null  THEN iif( agt.LocationCountry = '', 'POS Info Missing', agt.LocationCountry )  " + Environment.NewLine;
            sql += " END   " + Environment.NewLine;
            sql += " AS POS  " + Environment.NewLine;
            sql += " ,sdh.DocumentNumber as [Document Number]      " + Environment.NewLine;
            sql += " ,sdc.CouponNumber  as [Coupon Number]  " + Environment.NewLine;
            sql += " ,sdh.FareCalculationModeIndicator as [FCMI]      " + Environment.NewLine;
            sql += " ,sdc.OriginAirportCityCode as [ORI],sdc.DestinationAirportCityCode    as [DES]    " + Environment.NewLine;
            sql += "  ,sdc.FareBasisTicketDesignator as [Fare-Basis],sdc.ReservationBookingDesignator as [RBD],sdh.EndosRestriction as [Endos Restriction]     " + Environment.NewLine;
            sql += "  ,sdc.FlightDepartureDate as [Travel Date],DATENAME(dw,sdc.FlightDepartureDate) AS [Travel Day]      " + Environment.NewLine;
            sql += " ,DATEDIFF(DAY, sdh.DateofIssue,(select FlightDepartureDate from pax.SalesDocumentCoupon cpn where srd.isconjunction=0 and cpn.RelatedDocumentGuid = sdc.RelatedDocumentGuid and  cpn.CouponNumber ='1') ) as [Ticketing & COT Lapse]    " + Environment.NewLine;
            sql += "  ,sdp.FormofPaymentType   as [FOP Type]  " + Environment.NewLine;
            sql += "  ,isnull(nullif(sdp.RemittanceCurrency,''),sdp.currency ) AS [Remittance Currency]  " + Environment.NewLine;
            sql += "  ,sdp.RemittanceAmount as [Remittance Amount]    " + Environment.NewLine;
            sql += " INTO #base  " + Environment.NewLine;
            sql += " FROM Pax.SalesDocumentHeader sdh    " + Environment.NewLine;
            sql += " join pax.SalesRelatedDocumentInformation srd ON sdh.HdrGuid = srd.HdrGuid and sdh.OwnTicket ='Y'    " + Environment.NewLine;
            sql += " join pax.SalesDocumentCoupon sdc ON sdc.RelatedDocumentGuid = srd.RelatedDocumentGuid    " + Environment.NewLine;
            sql += " left join ref.VW_Agent agt ON agt.AgencyNumericCode = left(sdh.AgentNumericCode,7)    " + Environment.NewLine;
            sql += " left join Pax.SalesDocumentPayment sdp ON srd.RelatedDocumentGuid = sdp.RelatedDocumentGuid and sdh.DateofIssue = sdp.DateofIssue and sdh.TransactionCode = sdp.TransactionCode     " + Environment.NewLine;
            sql += " WHERE   " + Environment.NewLine;
            sql += " sdh.DateofIssue between '" + dateFrom + "' and   '" + dateTo + "'  " + Environment.NewLine;
            sql += " and sdp.FormofPaymentType like  '" + _FOP + "'  " + Environment.NewLine;
            sql += " and CASE   " + Environment.NewLine;
            sql += " WHEN  agt.LegalName is  null and  agt.LocationCountry is null  THEN 'Missing Agent Info'  " + Environment.NewLine;
            sql += " WHEN agt.LegalName is not null and  agt.LocationCountry is not null  THEN iif( agt.LocationCountry = '', 'POS Info Missing', agt.LocationCountry )  " + Environment.NewLine;
            sql += " END like   '" + _POS + "'  " + Environment.NewLine;
            sql += " and sdh.FareCalculationModeIndicator like '" + _FCMI + "'   " + Environment.NewLine;
            sql += " and sdc.FareBasisTicketDesignator like '" + FOPFB + "'    " + Environment.NewLine;
            sql += "   " + Environment.NewLine;
            sql += "   " + Environment.NewLine;
            sql += "   " + Environment.NewLine;
            sql += " IF @selection = 1  " + Environment.NewLine;
            sql += "   " + Environment.NewLine;
            sql += "  SELECT [Date of Issue] ,[Agent Numeric Code] , [Agent Name],POS,[Purchase Day],[Fare-Basis],[FCMI],[FOP Type],[Travel Day],[Ticketing & COT Lapse],[Remittance Currency]  " + Environment.NewLine;
            sql += "  ,sum([Remittance Amount]) as [Remittance Amount] , count (*) AS [Counts]   " + Environment.NewLine;
            sql += " FROM #base where  [Date of Issue] between '" + dateFrom + "' and '" + dateTo + "' and  [FOP Type] like '" + _FOP + "' and POS like  '" + _POS + "'  and [FCMI] like '" + _FCMI + "'  and [Fare-Basis] like  '" + FOPFB + "' and [Agent Numeric Code] like  '" + _Agt + "'  " + Environment.NewLine;
            sql += "  GROUP BY  [Date of Issue] ,[Agent Numeric Code], [Agent Name],POS,[Purchase Day],[Fare-Basis],[FCMI],[FOP Type],[Travel Day],[Ticketing & COT Lapse],[Remittance Currency]  " + Environment.NewLine;
            sql += "   " + Environment.NewLine;
            sql += " IF @selection =2  " + Environment.NewLine;
            sql += " SELECT *  FROM #base   " + Environment.NewLine;
            sql += " WHERE [Agent Numeric Code] = '" + a + "'  and POS = '" + b + "'  and [Purchase Day] = '" + c + "'  and [Fare-Basis] = '" + d + "'  and [FCMI]= '" + ee + "'  and [FOP Type] = '" + f + "'  and [Travel Day] = '" + g + "'   " + Environment.NewLine;
            sql += " and [Remittance Currency] = '" + h + "'  and [Ticketing & COT Lapse] = '" + i + "'   " + Environment.NewLine;


            SqlConnection con = new SqlConnection(pbConnectionString);
            DataSet ds = new DataSet();
            SqlDataAdapter ada = new SqlDataAdapter(sql, con);
            ada.Fill(ds);
            con.Close();
            int lon = ds.Tables[0].Rows.Count;
            if (lon == 0)
            {
                ViewBag.mes = "No data available for the selected criteria.";
            }
            ViewBag.txtCount = lon;
            return PartialView(ds);

        }
        public ActionResult LoadPOSSummaryDetails()
        {
            string dateFrom1 = Request["dateFrom"];
            string dateFrom = ConvertDate(dateFrom1);
            string dateTo1 = Request["dateTo"];
            string dateTo = ConvertDate(dateTo1);
            string cboAgt = Request["agpos"];
            string cboPoS = Request["namepos"];
            string cboFCMI = Request["fcmi"];
            string cboFOPFarebasis = Request["farebas"];
            string cboFOP = Request["fopP"];

            string _FOP = "%";
            string _POS = "%";
            string _FCMI = "%";
            string FOPFB = "%";
            string _Agt = "%";

            string a = cboAgt;
            string b = cboPoS; //pos
            string c = Request["purchase"];
            string d = cboFOPFarebasis;
            string ee = cboFCMI;
            string f = cboFOP;
            string g = Request["travelday"];
            string h = Request["remcur"];
            string i = Request["lapse"];


            if (cboFOP == "-All-")
            {
                _FOP = "%";
            }
            else
            {
                _FOP = cboFOP;
            }

            if (cboPoS == "-All-")
            {
                _POS = "%";
            }
            else
            {
                _POS = cboPoS;
            }
            if (cboFCMI == "-All-")
            {
                _FCMI = "%";
            }
            else
            {
                _FCMI = cboFCMI;
            }
            if (cboFOPFarebasis == "-All-")
            {
                FOPFB = "%";
            }
            else
            {
                FOPFB = cboFOPFarebasis;
            }

            if (cboAgt == "-All-")
            {
                _Agt = "%";
            }
            else
            {
                _Agt = cboAgt;
            }


            string sql = "";
            sql += " DECLARE @selection INT ='2'  " + Environment.NewLine;
            sql += "    " + Environment.NewLine;
            sql += " IF OBJECT_ID('tempdb..#base') IS NOT NULL DROP TABLE #base  " + Environment.NewLine;
            sql += "     " + Environment.NewLine;
            sql += "  SELECT sdh.DateofIssue as [Date of Issue] ,DATENAME(dw,sdh.DateofIssue) AS [Purchase Day],sdh.AgentNumericCode as [Agent Numeric Code]    " + Environment.NewLine;
            sql += "  ,isnull(agt.LegalName,'Missing Agent Info') AS [Agent Name]    " + Environment.NewLine;
            sql += " , CASE   " + Environment.NewLine;
            sql += " WHEN  agt.LegalName is  null and  agt.LocationCountry is null  THEN 'Missing Agent Info'  " + Environment.NewLine;
            sql += " WHEN agt.LegalName is not null and  agt.LocationCountry is not null  THEN iif( agt.LocationCountry = '', 'POS Info Missing', agt.LocationCountry )  " + Environment.NewLine;
            sql += " END   " + Environment.NewLine;
            sql += " AS POS  " + Environment.NewLine;
            sql += " ,sdh.DocumentNumber as [Document Number]      " + Environment.NewLine;
            sql += " ,sdc.CouponNumber  as [Coupon Number]  " + Environment.NewLine;
            sql += " ,sdh.FareCalculationModeIndicator as [FCMI]      " + Environment.NewLine;
            sql += " ,sdc.OriginAirportCityCode as [ORI],sdc.DestinationAirportCityCode    as [DES]    " + Environment.NewLine;
            sql += "  ,sdc.FareBasisTicketDesignator as [Fare-Basis],sdc.ReservationBookingDesignator as [RBD],sdh.EndosRestriction as [Endos Restriction]     " + Environment.NewLine;
            sql += "  ,sdc.FlightDepartureDate as [Travel Date],DATENAME(dw,sdc.FlightDepartureDate) AS [Travel Day]      " + Environment.NewLine;
            sql += " ,DATEDIFF(DAY, sdh.DateofIssue,(select FlightDepartureDate from pax.SalesDocumentCoupon cpn where srd.isconjunction=0 and cpn.RelatedDocumentGuid = sdc.RelatedDocumentGuid and  cpn.CouponNumber ='1') ) as [Ticketing & COT Lapse]    " + Environment.NewLine;
            sql += "  ,sdp.FormofPaymentType   as [FOP Type]  " + Environment.NewLine;
            sql += "  ,isnull(nullif(sdp.RemittanceCurrency,''),sdp.currency ) AS [Remittance Currency]  " + Environment.NewLine;
            sql += "  ,sdp.RemittanceAmount as [Remittance Amount]    " + Environment.NewLine;
            sql += " INTO #base  " + Environment.NewLine;
            sql += " FROM Pax.SalesDocumentHeader sdh    " + Environment.NewLine;
            sql += " join pax.SalesRelatedDocumentInformation srd ON sdh.HdrGuid = srd.HdrGuid and sdh.OwnTicket ='Y'    " + Environment.NewLine;
            sql += " join pax.SalesDocumentCoupon sdc ON sdc.RelatedDocumentGuid = srd.RelatedDocumentGuid    " + Environment.NewLine;
            sql += " left join ref.VW_Agent agt ON agt.AgencyNumericCode = left(sdh.AgentNumericCode,7)    " + Environment.NewLine;
            sql += " left join Pax.SalesDocumentPayment sdp ON srd.RelatedDocumentGuid = sdp.RelatedDocumentGuid and sdh.DateofIssue = sdp.DateofIssue and sdh.TransactionCode = sdp.TransactionCode     " + Environment.NewLine;
            sql += " WHERE   " + Environment.NewLine;
            sql += " sdh.DateofIssue between '" + dateFrom + "' and   '" + dateTo + "'  " + Environment.NewLine;
            sql += " and sdp.FormofPaymentType like  '" + _FOP + "'  " + Environment.NewLine;
            sql += " and CASE   " + Environment.NewLine;
            sql += " WHEN  agt.LegalName is  null and  agt.LocationCountry is null  THEN 'Missing Agent Info'  " + Environment.NewLine;
            sql += " WHEN agt.LegalName is not null and  agt.LocationCountry is not null  THEN iif( agt.LocationCountry = '', 'POS Info Missing', agt.LocationCountry )  " + Environment.NewLine;
            sql += " END like   '" + _POS + "'  " + Environment.NewLine;
            sql += " and sdh.FareCalculationModeIndicator like '" + _FCMI + "'   " + Environment.NewLine;
            sql += " and sdc.FareBasisTicketDesignator like '" + FOPFB + "'    " + Environment.NewLine;
            sql += "   " + Environment.NewLine;
            sql += "   " + Environment.NewLine;
            sql += "   " + Environment.NewLine;
            sql += " IF @selection = 1  " + Environment.NewLine;
            sql += "   " + Environment.NewLine;
            sql += "  SELECT [Date of Issue] ,[Agent Numeric Code] , [Agent Name],POS,[Purchase Day],[Fare-Basis],[FCMI],[FOP Type],[Travel Day],[Ticketing & COT Lapse],[Remittance Currency]  " + Environment.NewLine;
            sql += "  ,sum([Remittance Amount]) as [Remittance Amount] , count (*) AS [Counts]   " + Environment.NewLine;
            sql += " FROM #base where  [Date of Issue] between '" + dateFrom + "' and '" + dateTo + "' and  [FOP Type] like '" + _FOP + "' and POS like  '" + _POS + "'  and [FCMI] like '" + _FCMI + "'  and [Fare-Basis] like  '" + FOPFB + "' and [Agent Numeric Code] like  '" + _Agt + "'  " + Environment.NewLine;
            sql += "  GROUP BY  [Date of Issue] ,[Agent Numeric Code], [Agent Name],POS,[Purchase Day],[Fare-Basis],[FCMI],[FOP Type],[Travel Day],[Ticketing & COT Lapse],[Remittance Currency]  " + Environment.NewLine;
            sql += "   " + Environment.NewLine;
            sql += " IF @selection =2  " + Environment.NewLine;
            sql += " SELECT *  FROM #base   " + Environment.NewLine;
            sql += " WHERE [Agent Numeric Code] = '" + a + "'  and POS = '" + b + "'  and [Purchase Day] = '" + c + "'  and [Fare-Basis] = '" + d + "'  and [FCMI]= '" + ee + "'  and [FOP Type] = '" + f + "'  and [Travel Day] = '" + g + "'   " + Environment.NewLine;
            sql += " and [Remittance Currency] = '" + h + "'  and [Ticketing & COT Lapse] = '" + i + "'   " + Environment.NewLine;

            SqlConnection con = new SqlConnection(pbConnectionString);
            DataSet ds = new DataSet();
            SqlDataAdapter ada = new SqlDataAdapter(sql, con);
            ada.Fill(ds);
            con.Close();
            int lon = ds.Tables[0].Rows.Count;
            if (lon == 0)
            {
                ViewBag.mes = "No data available for the selected criteria.";
            }
            ViewBag.txtCount = lon;
            return PartialView(ds);

        }
        /************************End POS SUmmary*******************/


        public ActionResult RoutingChange()
        {
            return PartialView();
        }
        /*
        public ActionResult ExcessBilling()
        {
            return PartialView();
        }
        public ActionResult Commission()
        {
            return PartialView();
        }
        */
        //sml discrepencyAnalitics ///

        public ActionResult Discrepency()
        {
            ViewBag.dateFromanal = DateTime.Now.AddMonths(-1).ToString("dd-MMM-yyyy");
            ViewBag.dateToanal = DateTime.Now.ToString("dd-MMM-yyyy");
            ViewBag.dateFromRBD = DateTime.Now.AddMonths(-1).ToString("dd-MMM-yyyy");
            ViewBag.dateToRBD = DateTime.Now.ToString("dd-MMM-yyyy");
            ViewBag.dateFromDate = DateTime.Now.AddMonths(-1).ToString("dd-MMM-yyyy");
            ViewBag.dateToDate = DateTime.Now.ToString("dd-MMM-yyyy");
            AgentNumanal();
            agentDate();
            agentRBDDiscrepency();
            return PartialView();
        }
        public ActionResult GetAgentNumanal()
        {
            AgentNumanal();

            return PartialView();
        }
        public ActionResult GetAgentRBD()
        {
            agentRBDDiscrepency();

            return PartialView();
        }
        public ActionResult GetAgentDate()
        {
            agentDate();

            return PartialView();
        }
        public ActionResult SearchDiscrepencyAnalitics()
        {
            string dateFrom1 = Request["dateFromanal"];
            string dateFromanal = ConvertDate(dateFrom1);
            string dateTo1 = Request["dateToanal"];
            string dateToanal = ConvertDate(dateTo1);
            string ag = "%";
            string page = "1";
            string record = "150";
            SqlConnection cs = new SqlConnection(pbConnectionString);

            if (cs.State == ConnectionState.Open)
            {
                cs.Close();
            }
            cs.Open();

            SqlCommand cmd = new SqlCommand("[Pax].[SP_Ticket_No_Fare_Collected]", cs);
            cmd.CommandTimeout = 1000;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@DateofIssue_From", dateFromanal);
            cmd.Parameters.AddWithValue("@DateofIssue_To", dateToanal);
            cmd.Parameters.AddWithValue("@AgentNumericCode", ag);
            cmd.Parameters.AddWithValue("@PageNo", page);
            cmd.Parameters.AddWithValue("@RecordsPerPage", record);
            DataSet dsanal = new DataSet();
            SqlDataAdapter ada = new SqlDataAdapter(cmd);
            ada.Fill(dsanal);
            cs.Close();
            int lon = dsanal.Tables[0].Rows.Count;
            if (lon == 0)
            {
                ViewBag.mes = "No data available for the selected criteria.";
            }
            ViewBag.txtCount = lon;

            return PartialView(dsanal);
        }
        public ActionResult SearchRBDDiscrepency()
        {

            string dateFrom1 = Request["dateFromRBD"];
            string dateFromRBD = ConvertDate(dateFrom1);
            string dateTo1 = Request["dateToRBD"];
            string dateToRBD = ConvertDate(dateTo1);
            string ag = "%";
            string page = "1";
            string record = "150";
            string sql = " DECLARE @PageNo int = '" + page + " ';   " + Environment.NewLine;
            sql = sql + "  DECLARE @RecordsPerPage int = '" + record + " ';   " + Environment.NewLine;
            sql = sql + " select sdh.DocumentNumber,sdc.CouponNumber,sdh.AgentNumericCode,FlightNumber,FlightDepartureDate  " + Environment.NewLine;
            sql = sql + " ,sdc.FareBasisTicketDesignator  " + Environment.NewLine;
            sql = sql + " ,sdc.ReservationBookingDesignator as ReservedRBD  " + Environment.NewLine;
            sql = sql + " ,sdc.UsedClassofService as USAGERBD  " + Environment.NewLine;
            sql = sql + " ,iif(sdc.ReservationBookingDesignator<>sdc.UsedClassofService,'RBD has been Modified','') as Remarks  " + Environment.NewLine;
            sql = sql + " from   " + Environment.NewLine;
            sql = sql + " Pax.SalesDocumentHeader sdh  " + Environment.NewLine;
            sql = sql + " join Pax.SalesRelatedDocumentInformation srd on sdh.HdrGuid = srd.HdrGuid  " + Environment.NewLine;
            sql = sql + " left join Pax.SalesDocumentCoupon sdc on srd.RelatedDocumentGuid = sdc.RelatedDocumentGuid  " + Environment.NewLine;
            sql = sql + " where FlightDepartureDate between cast('" + dateFromRBD + "' as date) and cast('" + dateToRBD + "' as date)  " + Environment.NewLine;
            sql = sql + " and CouponStatus = 'F' and sdh.AgentNumericCode like '" + ag + "'  and iif(sdc.ReservationBookingDesignator<>sdc.UsedClassofService,'RBD has been Modified','') <> '' " + Environment.NewLine;
            sql = sql + " order by sdc.FlightDepartureDate,sdh.DocumentNumber,sdc.CouponNumber OFFSET (@PageNo-1)*@RecordsPerPage ROWS   " + Environment.NewLine;
            sql = sql + "  FETCH NEXT @RecordsPerPage ROWS ONLY  " + Environment.NewLine;
            SqlConnection cs = new SqlConnection(pbConnectionString);
            DataSet dsrbd = new DataSet();
            SqlDataAdapter ada = new SqlDataAdapter(sql, cs);
            ada.Fill(dsrbd);
            cs.Close();
            int lonrbd = dsrbd.Tables[0].Rows.Count;
            string[,] datarbd = new string[9, lonrbd];
            int i = 0;
            foreach (DataRow dr in dsrbd.Tables[0].Rows)
            {
                datarbd[0, i] = dr[dsrbd.Tables[0].Columns[0].ColumnName].ToString();
                datarbd[1, i] = dr[dsrbd.Tables[0].Columns[1].ColumnName].ToString();
                datarbd[2, i] = dr[dsrbd.Tables[0].Columns[2].ColumnName].ToString();
                datarbd[3, i] = dr[dsrbd.Tables[0].Columns[3].ColumnName].ToString();
                datarbd[4, i] = dr[dsrbd.Tables[0].Columns[4].ColumnName].ToString();
                datarbd[5, i] = dr[dsrbd.Tables[0].Columns[5].ColumnName].ToString();
                datarbd[6, i] = dr[dsrbd.Tables[0].Columns[6].ColumnName].ToString();
                datarbd[7, i] = dr[dsrbd.Tables[0].Columns[7].ColumnName].ToString();
                datarbd[8, i] = dr[dsrbd.Tables[0].Columns[8].ColumnName].ToString();
                i++;
            }

            ViewBag.lignerbd = lonrbd;
            ViewBag.datarbd = datarbd;
            agentRBDDiscrepency();
            return PartialView();
        }
        public ActionResult SearchDateDiscrepency()
        {
            string dateFrom1 = Request["dateFromDate"];
            string dateFromDate = ConvertDate(dateFrom1);
            string dateTo1 = Request["dateToDate"];
            string dateToDate = ConvertDate(dateTo1);
            string ag = "%";
            string page = "1";
            string record = "150";

            string sql = " DECLARE @PageNo int = '" + page + " ';   " + Environment.NewLine;
            sql = sql + "  DECLARE @RecordsPerPage int = '" + record + " ';   " + Environment.NewLine;
            sql = sql + "  select sdh.DocumentNumber,sdc.CouponNumber,sdh.AgentNumericCode,FlightNumber  " + Environment.NewLine;
            sql = sql + " ,sdc.FlightDepartureDate as [Planned Flight Date]  " + Environment.NewLine;
            sql = sql + " ,sdc.UsageDate as [Usage Flight Date]  " + Environment.NewLine;
            sql = sql + " ,iif(sdc.FlightDepartureDate<>sdc.UsageDate,'Date has been Modified','') as Remarks  " + Environment.NewLine;
            sql = sql + " from   " + Environment.NewLine;
            sql = sql + " Pax.SalesDocumentHeader sdh  " + Environment.NewLine;
            sql = sql + " join Pax.SalesRelatedDocumentInformation srd on sdh.HdrGuid = srd.HdrGuid  " + Environment.NewLine;
            sql = sql + " left join Pax.SalesDocumentCoupon sdc on srd.RelatedDocumentGuid = sdc.RelatedDocumentGuid  " + Environment.NewLine;
            sql = sql + " where FlightDepartureDate between cast('" + dateFromDate + "' as date) and cast('" + dateToDate + "' as date)  " + Environment.NewLine;
            sql = sql + " and CouponStatus = 'F' and sdh.AgentNumericCode like '" + ag + "' and iif(sdc.FlightDepartureDate<>sdc.UsageDate,'Date has been Modified','')  <> ''  " + Environment.NewLine;
            sql = sql + " order by sdc.FlightDepartureDate,sdh.DocumentNumber,sdc.CouponNumber OFFSET (@PageNo-1)*@RecordsPerPage ROWS   " + Environment.NewLine;
            sql = sql + "  FETCH NEXT @RecordsPerPage ROWS ONLY  " + Environment.NewLine;
            SqlConnection cs = new SqlConnection(pbConnectionString);
            DataSet dsdate = new DataSet();
            SqlDataAdapter ada = new SqlDataAdapter(sql, cs);
            ada.Fill(dsdate);
            cs.Close();
            int lonrbd = dsdate.Tables[0].Rows.Count;
            string[,] dataDate = new string[7, lonrbd];
            int i = 0;
            foreach (DataRow dr in dsdate.Tables[0].Rows)
            {
                dataDate[0, i] = dr[dsdate.Tables[0].Columns[0].ColumnName].ToString();
                dataDate[1, i] = dr[dsdate.Tables[0].Columns[1].ColumnName].ToString();
                dataDate[2, i] = dr[dsdate.Tables[0].Columns[2].ColumnName].ToString();
                dataDate[3, i] = dr[dsdate.Tables[0].Columns[3].ColumnName].ToString();
                dataDate[4, i] = dr[dsdate.Tables[0].Columns[4].ColumnName].ToString();
                dataDate[5, i] = dr[dsdate.Tables[0].Columns[5].ColumnName].ToString();
                dataDate[6, i] = dr[dsdate.Tables[0].Columns[6].ColumnName].ToString();
                i++;

            }

            ViewBag.lignedate = lonrbd;
            ViewBag.datadate = dataDate;
            agentDate();
            return PartialView();
        }
        public void AgentNumanal()
        {
            string dateFromanal = Request["dateFromanal"];
            string dateToanal = Request["dateToanal"];
            string sql = "select distinct AgentNumericCode  from [Pax].[SalesDocumentHeader] where DateofIssue between '" + dateFromanal + "'and '" + dateToanal + "' ";
            SqlConnection cs = new SqlConnection(pbConnectionString);
            DataSet dsanal = new DataSet();
            SqlDataAdapter ada = new SqlDataAdapter("" + sql + "", cs);
            ada.Fill(dsanal);
            cs.Close();
            int lonanal = dsanal.Tables[0].Rows.Count;
            string[,] dataan = new string[1, lonanal];
            int i = 0;
            foreach (DataRow dr in dsanal.Tables[0].Rows)
            {
                dataan[0, i] = dr[dsanal.Tables[0].Columns[0].ColumnName].ToString();
                i++;
            }
            ViewBag.ligneagnum = lonanal;
            ViewBag.dataagnum = dataan;
        }
        private void agentDate()
        {
            string dateFromDate = Request["dateFromDate"];
            string dateToDate = Request["dateToDate"];
            string sql = " select distinct sdh.AgentNumericCode  from [Pax].[SalesDocumentHeader] sdh   " + Environment.NewLine;
            sql = sql + " join Pax.SalesRelatedDocumentInformation srd on sdh.HdrGuid = srd.HdrGuid   " + Environment.NewLine;
            sql = sql + " left join Pax.SalesDocumentCoupon sdc on srd.RelatedDocumentGuid = sdc.RelatedDocumentGuid   " + Environment.NewLine;
            sql = sql + " where  sdc.FlightDepartureDate between cast('" + dateFromDate + "' as date) and cast('" + dateToDate + "' as date) and sdc.CouponStatus = 'F'   " + Environment.NewLine;

            SqlConnection cs = new SqlConnection(pbConnectionString);
            DataSet dsdateag = new DataSet();
            SqlDataAdapter ada = new SqlDataAdapter(sql, cs);
            ada.Fill(dsdateag);
            cs.Close();
            int lonrbdag = dsdateag.Tables[0].Rows.Count;
            string[,] dataDateag = new string[1, lonrbdag];
            int i = 0;
            foreach (DataRow dr in dsdateag.Tables[0].Rows)
            {
                dataDateag[0, i] = dr[dsdateag.Tables[0].Columns[0].ColumnName].ToString();
                i++;
            }
            ViewBag.lignedateag = lonrbdag;
            ViewBag.datadateag = dataDateag;
        }
        private void agentRBDDiscrepency()
        {
            string dateFromRBD = Request["dateFromRBD"];
            string dateToRBD = Request["dateToRBD"];
            string sql = " select distinct sdh.AgentNumericCode  from [Pax].[SalesDocumentHeader] sdh   " + Environment.NewLine;
            sql = sql + " join Pax.SalesRelatedDocumentInformation srd on sdh.HdrGuid = srd.HdrGuid   " + Environment.NewLine;
            sql = sql + " left join Pax.SalesDocumentCoupon sdc on srd.RelatedDocumentGuid = sdc.RelatedDocumentGuid   " + Environment.NewLine;
            sql = sql + " where  sdc.FlightDepartureDate between cast('" + dateFromRBD + "' as date) and cast('" + dateToRBD + "' as date) and sdc.CouponStatus = 'F'   " + Environment.NewLine;
            SqlConnection cs = new SqlConnection(pbConnectionString);
            DataSet dsrbdag = new DataSet();
            SqlDataAdapter ada = new SqlDataAdapter(sql, cs);
            ada.Fill(dsrbdag);
            cs.Close();
            int lonrbdag = dsrbdag.Tables[0].Rows.Count;
            string[,] datarbdeag = new string[1, lonrbdag];
            int i = 0;
            foreach (DataRow dr in dsrbdag.Tables[0].Rows)
            {
                datarbdeag[0, i] = dr[dsrbdag.Tables[0].Columns[0].ColumnName].ToString();
                i++;
            }
            ViewBag.lignerbdag = lonrbdag;
            ViewBag.datarbdag = datarbdeag;
        }
        public ActionResult clearFare()
        {
            return PartialView();
        }
        public ActionResult clearRBD()
        {
            return PartialView();
        }
        public ActionResult clearDate()
        {
            return PartialView();
        }
        //sml end  discrepencyAnalitics ///

        /*****************Tolotra end**********************/
        /**
            code by Fano
        */
        public ActionResult OutwardBilling()
        {
            return PartialView();
        }

        /*   Tab Main and prorate factor in transaction in a Nutshell   Joseph  */

        public ActionResult TransactionInANutshell()
        {
            /*Get Item  Joseph*/
            getItemFlown();
            getItemInterline();

            getItemPeriod();
            getItemCountry();

            getItemCurrency();
            getItemAddCountry();

            // function City / Airport Exist
            getexist();
            getExistAirCode();

            // function Proration factorie
            getItemCity("");
            getItemDCity("");


            return PartialView();
        }

        // function recuperation getcps Joseph
        public ActionResult getcps()
        {
            string cp = "";

            string txtTicketNo = "";

            string sql = "SELECT CouponStatus from Pax.SalesDocumentCoupon Where DocumentNumber = '" + txtTicketNo + "'"; 

            SqlConnection con = new SqlConnection(pbConnectionString);


            DataSet ds = new DataSet();
            SqlDataAdapter ada = new SqlDataAdapter(sql, con);

            ada.Fill(ds);

            con.Close();


            int lon = ds.Tables[0].Rows.Count;

            if (lon == 0)
            {
                ViewBag.mes = "No data available for the selected criteria.";
            }; 


            ViewBag.txtCount = lon;

            return PartialView(ds);
        }


        public ActionResult ResultatTransactionInANutshell()
        {

            string numberTicket = Request["number"];

            string sql = "";
            sql = "select f1.DateofIssue, f1.FareCurrency, f1.Fare, f1.TotalCurrency, f1.EquivalentFare, f1.FareCalculationArea, srd.RelatedDocumentGuid, f1.OriginalIssueDate, f1.SaleDate " + Environment.NewLine;
            sql += "from Pax.SalesRelatedDocumentInformation srd" + Environment.NewLine;
            sql += "join Pax.SalesDocumentHeader f1 on RelatedDocumentNumber = '" + numberTicket + "' and  IsConjunction=0" + Environment.NewLine;
            sql += "and f1.HdrGuid = srd.HdrGuid" + Environment.NewLine;
            sql += "and srd.TransactionCode <> 'RFND'";


            SqlConnection con2 = new SqlConnection(pbConnectionString);
            DataSet ds = new DataSet();
            SqlDataAdapter ada = new SqlDataAdapter(sql, con2);

            ada.Fill(ds);
            // con.Close();

            int lon = ds.Tables[0].Rows.Count;
            if (lon == 0)
            {
                ViewBag.mes = "No data available for the selected criteria.";
            }

            ViewBag.log = lon;

            string doi = "";
            string txtFare = "";
            string txtFareNumber = "";
            string txtEfp = "";
            string txtEfpNumber = "";
            string txtFca = "";
            string rel = "";
            string txtOrigIssueDate = "";
            string txtDateOfSale = "";


            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                doi = dr[ds.Tables[0].Columns[0].ColumnName].ToString();
                txtFare = dr[ds.Tables[0].Columns[1].ColumnName].ToString();
                txtFareNumber = dr[ds.Tables[0].Columns[2].ColumnName].ToString();
                txtEfp = dr[ds.Tables[0].Columns[3].ColumnName].ToString();
                txtEfpNumber = dr[ds.Tables[0].Columns[4].ColumnName].ToString();
                txtFca = dr[ds.Tables[0].Columns[5].ColumnName].ToString();
                rel = dr[ds.Tables[0].Columns[6].ColumnName].ToString();
                txtOrigIssueDate = dr[ds.Tables[0].Columns[7].ColumnName].ToString();
                txtDateOfSale = dr[ds.Tables[0].Columns[8].ColumnName].ToString();
            }

            ViewBag.fare = txtFare + " " + txtFareNumber;
            ViewBag.Efp = txtEfp + " " + txtEfpNumber;
            ViewBag.dtpDateOfSale = doi;
            ViewBag.Fca = txtFca;
            ViewBag.rel = rel;

            ViewBag.txtOrigIssueDate = txtOrigIssueDate;
            ViewBag.txtDateOfSale = txtDateOfSale;

            ViewBag.numberTicket = numberTicket;

            /* Detail Coupon View */
           Grid(Display(numberTicket));
            /* End Detail Coupon View*/ 

            return PartialView();
        }


        // function getMPR   Joseph
        public string getMPR()
        {
            string MPR = "";
            string sql = "SELECT [MPR] FROM [Ref].[MPR] WHERE Convert(date, '" + DateTime.Now + "', 105) BETWEEN Convert(date,[From],105) AND Convert(date,[To],105)";
            // MPR = sl.InstanceSingleRecord(sql, cs);
            return MPR;
        }


        // function Detail Coupon  View   Joseph
        public string Display(string DocNum)
        {
            string DocNumber = "";

            string sql = "select DocumentNumber from pax.SalesRelatedDocumentInformation where RelatedDocumentNumber = '" + DocNum + "'";


            SqlConnection con = new SqlConnection(pbConnectionString);
            DataSet ds = new DataSet();
            SqlDataAdapter ada = new SqlDataAdapter(sql, con);

            ada.Fill(ds);

            int lon = ds.Tables[0].Rows.Count;

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                DocNumber = dr[ds.Tables[0].Columns[0].ColumnName].ToString();

            }

            ViewBag.NombreTable = lon;

            ViewBag.DocumentNO = DocNumber;


            return DocNumber;
        }


        public void Grid(string Docu)
        {

            string sql = "select RelatedDocumentNumber,CouponNumber, StopOverCode,Concat(OriginCity, '-',DestinationCity), Carrier," + Environment.NewLine;
            sql += "FlightNumber, FlightDepartureDate, FlightDepartureTime, CouponStatus, FareBasisTicketDesignator, NotValidBefore, NotValidAfter" + Environment.NewLine;
            sql += ",ReservationBookingDesignator,Concat(UsageOriginCode , '-',UsageDestinationCode), UsageAirline,UsageFlightNumber,UsageDate" + Environment.NewLine;
            sql += "from pax.salesdocumentcoupon " + Environment.NewLine;
            sql += "where documentnumber = '" + Docu + "'" + Environment.NewLine;
            sql += "order by 1,2" + Environment.NewLine;

            SqlConnection con = new SqlConnection(pbConnectionString);
            DataSet ds = new DataSet();
            SqlDataAdapter ada = new SqlDataAdapter(sql, con);

            ada.Fill(ds);


            int lon = ds.Tables[0].Rows.Count;

            string[,] CouponView = new string[28, lon];

            int i = 0;

            foreach (DataRow dr in ds.Tables[0].Rows)
            {

                CouponView[0, i] = dr[ds.Tables[0].Columns[0].ColumnName].ToString();
                CouponView[1, i] = dr[ds.Tables[0].Columns[1].ColumnName].ToString();
                CouponView[2, i] = dr[ds.Tables[0].Columns[2].ColumnName].ToString();
                CouponView[3, i] = dr[ds.Tables[0].Columns[3].ColumnName].ToString();
                CouponView[4, i] = dr[ds.Tables[0].Columns[4].ColumnName].ToString();
                CouponView[5, i] = dr[ds.Tables[0].Columns[5].ColumnName].ToString();
                CouponView[6, i] = dr[ds.Tables[0].Columns[6].ColumnName].ToString();
                CouponView[7, i] = dr[ds.Tables[0].Columns[7].ColumnName].ToString();
                CouponView[8, i] = dr[ds.Tables[0].Columns[8].ColumnName].ToString();
                CouponView[9, i] = dr[ds.Tables[0].Columns[9].ColumnName].ToString();
                CouponView[10, i] = dr[ds.Tables[0].Columns[10].ColumnName].ToString();
                CouponView[11, i] = dr[ds.Tables[0].Columns[11].ColumnName].ToString();
                CouponView[12, i] = dr[ds.Tables[0].Columns[12].ColumnName].ToString();
                CouponView[13, i] = dr[ds.Tables[0].Columns[13].ColumnName].ToString();
                CouponView[14, i] = dr[ds.Tables[0].Columns[14].ColumnName].ToString();
                CouponView[15, i] = dr[ds.Tables[0].Columns[15].ColumnName].ToString();
                CouponView[16, i] = dr[ds.Tables[0].Columns[16].ColumnName].ToString();

                i++;
            }

            ViewBag.CouponView = CouponView;
            ViewBag.lon = lon;

        }

        // End function Detail Coupon View


        /* END Tab Main and prorate factor in transaction in a Nutshell  */


        public ActionResult ProrationExceptionViewer()
        {
            //fait par christian
            string dtpFromValue = DateTime.Now.AddMonths(-1).ToString("dd-MMM-yyyy");
            string dtpToValue = DateTime.Now.ToString("dd-MMM-yyyy");

            string dtpIssueDateFromValue = DateTime.Now.AddMonths(-1).ToString("dd-MMM-yyyy");
            string dtpIssueDateToValue = DateTime.Now.ToString("dd-MMM-yyyy");

            string dtpFromChecked = "";
            string dtpToChecked = "";

            string dtpIssueDateFromChecked = "";
            string dtpIssueDateToChecked = "";

            ViewBag.dtpFromValue = dtpFromValue;
            ViewBag.dtpToValue = dtpToValue;
            ViewBag.dtpIssueDateFromValue = dtpIssueDateFromValue;
            ViewBag.dtpIssueDateToValue = dtpIssueDateToValue;
            ViewBag.dtpFromChecked = dtpFromChecked;
            ViewBag.dtpToChecked = dtpToChecked;
            ViewBag.dtpIssueDateFromChecked = dtpIssueDateFromChecked;
            ViewBag.dtpIssueDateToChecked = dtpIssueDateToChecked;
            //and fait par christian
            return PartialView();
        }

        public ActionResult FareTFCSurchargesTracking()
        {
            return PartialView();
        }


        /*****************SearchFinalSharevsAmount************************/

        public ActionResult ShowtableFinalSharevsAmount(String[] dataValue)
        {
            String document = dataValue[0];
            String sqlT = "select f1.DocumentNumber,iif(f1.DocumentNumber = f3.RelatedDocumentNumber, NULL, f3.RelatedDocumentNumber) as RelatedDocumentNumber,f3.CouponNumber, f3.OriginAirportCityCode,f3.DestinationAirportCityCode,f3.ReservationBookingDesignator,f3.FareBasisTicketDesignator , pd.Diffentials ,pd.Surcharge,pd.FinalShare,iif(pd.SpecialProrateAgreement > 0.00, 'SPA', 'MPA') as PM from Pax.SalesDocumentHeader f1 join Pax.SalesRelatedDocumentInformation f2 on f1.HdrGuid = f2.HdrGuid join Pax.SalesDocumentCoupon f3 on f2.RelatedDocumentGuid = f3.RelatedDocumentGuid left join pax.ProrationDetail PD on f3.DocumentNumber = PD.DocumentNumber and f3.CouponNumber = pd.CouponNumber and PD.ProrationFlag = f3.CouponStatus  where f1.DocumentNumber = '" + document + "'";


            SqlConnection cs = new SqlConnection(pbConnectionString);
            if (cs.State == ConnectionState.Open)
            {
                cs.Close();
            }
            DataSet dsT = new DataSet();
            SqlDataAdapter adaT = new SqlDataAdapter(sqlT, cs);
            adaT.Fill(dsT);
            cs.Close();
            return PartialView(dsT);
        }
        public ActionResult SearchFinalSharevsAmount(String[] dataValue)
        {
            String document = dataValue[0];
            SqlConnection cs = new SqlConnection(pbConnectionString);
            if (cs.State == ConnectionState.Open)
            {
                cs.Close();
            }

            cs.Open();

            SqlCommand cmd = new SqlCommand("[Pax].[FinalShares_V_AmountCollected]", cs);
            cmd.CommandTimeout = 1000;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@DocumentNumber", document);
            
            DataSet ds = new DataSet();
            SqlDataAdapter ada = new SqlDataAdapter(cmd);
            ada.Fill(ds);
            int ligne = ds.Tables[0].Rows.Count;
            string[,] flownData = new string[4, ligne];
            int i = 0;
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                flownData[1, i] = dr[ds.Tables[0].Columns[1].ColumnName].ToString();

                string dateFIssue = null;

                if (flownData[1, i] != null)
                {
                    dateFIssue = flownData[1, i].Substring(0, 11);

                }

                ViewBag.DateofIssue = dateFIssue;

                ViewBag.DR = dr[ds.Tables[0].Columns[7].ColumnName].ToString();

                ViewBag.AmtColl = dr[ds.Tables[0].Columns[4].ColumnName].ToString();

                ViewBag.totTax = dr[ds.Tables[0].Columns[5].ColumnName].ToString();

                ViewBag.FareEFP = dr[ds.Tables[0].Columns[6].ColumnName].ToString();

                ViewBag.Currency = dr[ds.Tables[0].Columns[3].ColumnName].ToString();

                ViewBag.TotalFinalShare = dr[ds.Tables[0].Columns[2].ColumnName].ToString();

                ViewBag.TotalFareUSD = dr[ds.Tables[0].Columns[8].ColumnName].ToString();

                ViewBag.Residue = dr[ds.Tables[0].Columns[9].ColumnName].ToString();

                ViewBag.FCA = dr[ds.Tables[0].Columns[10].ColumnName].ToString();
            }

            cs.Close();

            return PartialView(ds);

        }

        /*******************File Upload Status******************************/

        public string ConvertDateProcess(string date)
        {
            var culture = System.Globalization.CultureInfo.CurrentCulture;
            string mydate = DateTime.ParseExact(date, "dd-MMM-yyyy", new CultureInfo(culture.Name)).ToString("MM-dd-yyyy");
            return mydate;
        }
        public ActionResult FileUploadStatusSelectFileType(String[] dataValue)
        {
            string dtpFrom = dataValue[0];
            string dtpTo = dataValue[1];
            String dtpFromConvert = ConvertDateProcess(dtpFrom);
            String dtpTConvert = ConvertDateProcess(dtpTo);
            string sql = " select distinct ProcessingFileType FROM [FileMgr].[FileUploadStatus]   " + Environment.NewLine;
            sql += " where  cast ([ProcessingDate] as Date) between '" + dtpFromConvert + "' and '" + dtpTConvert + "'      " + Environment.NewLine;

            SqlConnection cs = new SqlConnection(pbConnectionString);
            if (cs.State == ConnectionState.Open)
            {
                cs.Close();
            }
            DataSet ds = new DataSet();
            SqlDataAdapter ada = new SqlDataAdapter(sql, cs);
            ada.Fill(ds);
            cs.Close();
            int lonAg = ds.Tables[0].Rows.Count;
            string[] AgCode = new string[lonAg];
            int i = 0;
            if (lonAg == 0)
            {
                ViewBag.mes = "No data available for the selected criteria";
            }
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                AgCode[i] = dr[ds.Tables[0].Columns[0].ColumnName].ToString();
                i = i + 1;
            }

            ViewBag.AgCode = AgCode;
            ViewBag.lonAg = lonAg;
            return PartialView();
        }

        public ActionResult FileUploadStatusSelectProcessingStatus(String[] dataValue)
        {
            string dtpFrom = dataValue[0];
            string dtpTo = dataValue[1];
            String dtpFromConvert = ConvertDateProcess(dtpFrom);
            String dtpTConvert = ConvertDateProcess(dtpTo);
            String SelecteFileType1 = dataValue[2];
            String SelecteFileType = "";

            if (SelecteFileType1 == "-All-")
            {

                SelecteFileType = "%";
            }
            else
            {
                SelecteFileType = SelecteFileType1;
            }

            string sql = " select distinct [ProcessingStatus] FROM [FileMgr].[FileUploadStatus]   " + Environment.NewLine;
            sql += " where  cast ([ProcessingDate] as Date) between '" + dtpFromConvert + "' and '" + dtpTConvert + "'  and ProcessingFileType like '" + SelecteFileType + "'  " + Environment.NewLine;

            SqlConnection cs = new SqlConnection(pbConnectionString);
            if (cs.State == ConnectionState.Open)
            {
                cs.Close();
            }
            DataSet ds = new DataSet();
            SqlDataAdapter ada = new SqlDataAdapter(sql, cs);
            ada.Fill(ds);
            cs.Close();
            int lonAg = ds.Tables[0].Rows.Count;
            string[] AgCode = new string[lonAg];
            int i = 0;
            if (lonAg == 0)
            {
                ViewBag.mes = "No data available for the selected criteria";
            }
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                AgCode[i] = dr[ds.Tables[0].Columns[0].ColumnName].ToString();
                i = i + 1;
            }

            ViewBag.AgCode = AgCode;
            ViewBag.lonAg = lonAg;
            return PartialView();
        }

        public ActionResult FileUploadStatusSelectFileName(String[] dataValue)
        {
            string dtpFrom = dataValue[0];
            string dtpTo = dataValue[1];
            String dtpFromConvert = ConvertDateProcess(dtpFrom);
            String dtpTConvert = ConvertDateProcess(dtpTo);
            String SelecteFileType1 = dataValue[2];
            String SelectProcessingStatus1 = dataValue[3];
            String SelecteFileType = "";
            String SelectProcessingStatus = "";

            if (SelecteFileType1 == "-All-")
            {

                SelecteFileType = "%";
            }
            else
            {
                SelecteFileType = SelecteFileType1;
            }

            if (SelectProcessingStatus1 == "-All-")
            {

                SelectProcessingStatus = "%";
            }
            else
            {
                SelectProcessingStatus = SelectProcessingStatus1;
            }

            string sql = " select distinct [ProcessingFileName] FROM [FileMgr].[FileUploadStatus]  " + Environment.NewLine;
            sql += " where cast ([ProcessingDate] as Date) between '" + dtpFromConvert + "' and '" + dtpTConvert + "' and ProcessingFileType like '" + SelecteFileType + "' and ProcessingStatus like '" + SelectProcessingStatus + "' " + Environment.NewLine;

            SqlConnection cs = new SqlConnection(pbConnectionString);
            if (cs.State == ConnectionState.Open)
            {
                cs.Close();
            }
            DataSet ds = new DataSet();
            SqlDataAdapter ada = new SqlDataAdapter(sql, cs);
            ada.Fill(ds);
            cs.Close();
            int lonAg = ds.Tables[0].Rows.Count;
            string[] AgCode = new string[lonAg];
            int i = 0;
            if (lonAg == 0)
            {
                ViewBag.mes = "No data available for the selected criteria";
            }
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                AgCode[i] = dr[ds.Tables[0].Columns[0].ColumnName].ToString();
                i = i + 1;
            }

            ViewBag.AgCode = AgCode;
            ViewBag.lonAg = lonAg;
            return PartialView();
        }

        public ActionResult FileUploadStatussearch(String[] dataValue)
        {
            String dtpFrom = dataValue[0];
            String dtpTo = dataValue[1];
            String dtpFromConvert = ConvertDateProcess(dtpFrom);
            String dtpTConvert = ConvertDateProcess(dtpTo);
            String SelecteFileType1 = dataValue[2];
            String SelectProcessingStatus1 = dataValue[3];
            String SelectFileName1 = dataValue[4];
            String ReportType1 = dataValue[5];


            String SelecteFileType = "";
            String SelectProcessingStatus = "";
            String SelectFileName = "";
            String ReportType = "";

            if (SelecteFileType1 == "-All-")
            {

                SelecteFileType = "%";
            }
            else
            {
                SelecteFileType = SelecteFileType1;
            }

            if (SelectProcessingStatus1 == "-All-")
            {

                SelectProcessingStatus = "%";
            }
            else
            {
                SelectProcessingStatus = SelectProcessingStatus1;
            }

            if (SelectFileName1 == "-All-")
            {

                SelectFileName = "%";
            }
            else
            {
                SelectFileName = SelectFileName1;
            }

            if (ReportType1 == "oui")
            {
                ReportType = "D";
            }
            else
            {
                ReportType = "S";
            }


            string sql = "EXEC FileMgr.SP_FileUploadStatus @ProcessingFileType='" + SelecteFileType + "', @ProcessingStatus='" + SelectProcessingStatus + "', @ProcessingFileName='" + SelectFileName + "', @BatchID_From='" + dtpFromConvert + "', @BatchID_To='" + dtpTConvert + "', @ReportType = '" + ReportType + "'";

            SqlConnection cs = new SqlConnection(pbConnectionString);
            if (cs.State == ConnectionState.Open)
            {
                cs.Close();
            }
            DataSet dsT = new DataSet();
            SqlDataAdapter adaT = new SqlDataAdapter(sql, cs);
            adaT.Fill(dsT);
            cs.Close();
            int lonAg = dsT.Tables[0].Rows.Count;
            string[] AgCode = new string[lonAg];
            if (lonAg == 0)
            {
                ViewBag.message = "No data available for the selected criteria";
            }
            return PartialView(dsT);
        }

        /********************Involontary Reroute*****************************************/
        public ActionResult InvolontaryRerouteSearch(String[] dataValue)
        {
            string dtpFrom = dataValue[0];
            string dtpTo = dataValue[1];
            String dtpFromConvert = ConvertDateProcess(dtpFrom);
            String dtpTConvert = ConvertDateProcess(dtpTo);
            string sql = "select Format(DateofIssue,'dd-MMM-yyyy') as DateofIssue,f1.DocumentNumber,f2.CouponNumber,f2.OriginAirportCityCode as Origin, f2.DestinationAirportCityCode as Destination";
            sql = sql + ", f2.FareBasisTicketDesignator as [Fare Basis],f1.FareCalculationModeIndicator as FCMI ,TransactionCode,  FareCurrency,Fare, TotalCurrency as EquivalentFareCurrency, EquivalentFare";
            sql = sql + ", f1.TotalCurrency, TotalAmount,  F1.AmountCollectedCurrency, AmountCollected, TaxCollected, SurchargeCollected, pax.fn_FOP_Combined(f1.hdrguid) as FOP, EndosRestriction, FareCalculationArea as FCA";
            sql = sql + " from Pax.SalesDocumentHeader f1 left join Pax.SalesDocumentCoupon f2 on f1.HdrGuid = f2.HdrGuidRef";
            sql = sql + " where InvoluntaryReroute = 'Y' and f1.DateofIssue between '" + dtpFromConvert + "'and '" + dtpTConvert + "' order by f1.DateofIssue,f1.DocumentNumber";

            SqlConnection cs = new SqlConnection(pbConnectionString);
            if (cs.State == ConnectionState.Open)
            {
                cs.Close();
            }
            DataSet ds = new DataSet();
            SqlDataAdapter ada = new SqlDataAdapter(sql, cs);
            ada.Fill(ds);
            cs.Close();
            int lonAg = ds.Tables[0].Rows.Count;
            string[] AgCode = new string[lonAg];
            if (lonAg == 0)
            {
                ViewBag.message = "No data available for the selected criteria";
            }
            return PartialView(ds);
        }
        /**********************EMD*****************************************/
        public ActionResult SearchEMD(String[] dataValue)
        {
            String FromEMD = dataValue[0];
            String ToEMD = dataValue[1];
            String dtpFromConvert = ConvertDateProcess(FromEMD);
            String dtpTConvert = ConvertDateProcess(ToEMD);

            String checkFrom = dataValue[2];
            String checkto = dataValue[3];
            String DocumentNum = dataValue[4];

            string strSQL = "select Format(DateofIssue,'dd-MMM-yyyy') as DateofIssue, DocumentNumber,AgentNumericCode,PassengerName,TransactionCode,ProcessingFileType from Pax.SalesDocumentHeader where TransactionCode in ('EMDS','EMDA') and   ";
            bool hasOperator = false;
            string strOperator = " AND ";

            if (DocumentNum != "")
            {
                if (hasOperator == true)
                    strSQL = strSQL + strOperator;
                strSQL = strSQL + " DocumentNumber LIKE '%" + DocumentNum + "%' ";
                hasOperator = true;
            }

            if (checkFrom == "oui" && checkto == "oui")
            {
                if (hasOperator == true)
                    strSQL = strSQL + strOperator;
                string strIssueDateFrom = String.Format("{0:MM/dd/yyyy}", dtpFromConvert);
                string strIssueDateTo = String.Format("{0:MM/dd/yyyy}", dtpTConvert);
                strSQL = strSQL + " DateofIssue BETWEEN '" + strIssueDateFrom + "' AND '" + strIssueDateTo + "'";
                hasOperator = true;
            }
            else
            {
                ViewBag.messageInvalid = "Invalid Date Range";
            }
            SqlConnection cs = new SqlConnection(pbConnectionString);
            if (cs.State == ConnectionState.Open)
            {
                cs.Close();
            }
            cs.Open();
            SqlCommand cmd = new SqlCommand(strSQL, cs);
            DataSet ds = new DataSet();
            SqlDataAdapter ada = new SqlDataAdapter(cmd);
            ada.Fill(ds);
            int lonAg = ds.Tables[0].Rows.Count;
            string[] AgCode = new string[lonAg];
            if (lonAg == 0)
            {
                ViewBag.messageNodata = "No data available for the selected criteria.";
            }
            return PartialView(ds);
        }

        public ActionResult ShowSearchEMD(String[] dataValue) {

            String DocNum = dataValue[0];

            SqlConnection cs = new SqlConnection(pbConnectionString);
            if (cs.State == ConnectionState.Open)
            {
                cs.Close();
            }

            cs.Open();

            string sql = "   " + Environment.NewLine;
            sql += " with Base as   " + Environment.NewLine;
            sql += " (   " + Environment.NewLine;
            sql += " select f1.HdrGuid, f1.DocumentNumber, f1.TransactionCode, f7.UploadGuid, f7.TransactionNumber,f1.DataSource   " + Environment.NewLine;
            sql += " from Pax.SalesDocumentHeader f1    " + Environment.NewLine;
            sql += " join Pax.SalesRelatedDocumentInformation f2 on f1.HdrGuid = f2.HdrGuid and f2.IsConjunction = 0   " + Environment.NewLine;
            sql += " join Pax.VW_HOT_Header f7 on f1.DocumentNumber = f7.TicketDocumentNumber and f1.TransactionCode = f7.TransactionCode and f2.SalesUploadGuid = f7.UploadGuid   " + Environment.NewLine;
            sql += " )   " + Environment.NewLine;
            sql += " ,FINAL as (   " + Environment.NewLine;
            sql += " select  f1.DocumentNumber, f1.TransactionCode--,f1.DataSource   " + Environment.NewLine;
            sql += " ,Bmd75.EMDCouponNumber   " + Environment.NewLine;
            sql += " ,pax.DishAmountConvert(Bmd75.EMDCouponValue,Bmd75.CurrencyType) as EMDCouponValue   " + Environment.NewLine;
            sql += " ,Bmd75.CurrencyType   " + Environment.NewLine;
            sql += " ,Bmd75.EMDRelatedTicketDocumentNumber   " + Environment.NewLine;
            sql += " ,Bmd75.EMDRelatedCouponNumber   " + Environment.NewLine;
            sql += " ,Bmd75.EMDReasonforIssuanceSubCode   " + Environment.NewLine;
            sql += " ,Bmd75.EMDFeeOwnerAirlineDesignator   " + Environment.NewLine;
            sql += " ,Bmd75.EMDExcessBaggageOverAllowanceQualifier   " + Environment.NewLine;
            sql += " ,Bmd75.EMDExcessBaggageRateperUnit   " + Environment.NewLine;
            sql += " ,Bmd75.EMDExcessBaggageTotalNumberinExcess   " + Environment.NewLine;
            sql += " ,Bmd75.EMDConsumedatIssuanceIndicator   " + Environment.NewLine;
            sql += " ,Bmd75.EMDExcessBaggageCurrencyCode   " + Environment.NewLine;
            sql += "    " + Environment.NewLine;
            sql += ",isnull(bks24.ReasonforIssuanceCode,Bmd70.ReasonforIssuanceCode) as ReasonforIssuanceCode,isnull(Bmd70.ReasonforIssuanceDescription ,Bmd76.EMDRemarks) as ReasonforIssuanceDescription " + Environment.NewLine;
            sql += " ,Bmd76.EMDRemarks     " + Environment.NewLine;
            sql += " " + Environment.NewLine;
            sql += "    from Base f1 " + Environment.NewLine;
            sql += " join [FileHot2].VW_Bmd75_ECouponRecords Bmd75 on Bmd75.UploadGuid = f1.UploadGuid and Bmd75.TransactionNumber = f1.TransactionNumber    " + Environment.NewLine;
            sql += " left join FileHot2.VW_Bmd76_ERemarkRecords Bmd76 on Bmd76.UploadGuid = f1.UploadGuid and Bmd76.TransactionNumber = f1.TransactionNumber   " + Environment.NewLine;
            sql += " left join filehot2.Bks24_DocumentIdentification bks24 on f1.UploadGuid = bks24.UploadGuid and f1.TransactionNumber = bks24.TransactionNumber  " + Environment.NewLine;
            sql += "left join FileHot.[Bmp70_ReasonForIssuance] Bmd70 on Bmd70.UploadGuid = f1.UploadGuid and Bmd70.TransactionNumber = f1.TransactionNumber  " + Environment.NewLine;
            sql += " union all    " + Environment.NewLine;
            sql += "    " + Environment.NewLine;
            sql += " select    " + Environment.NewLine;
            sql += "    " + Environment.NewLine;
            sql += "  f1.DocumentNumber, f1.TransactionCode--,f1.DataSource   " + Environment.NewLine;
            sql += "  ,EMDCouponNumber   " + Environment.NewLine;
            sql += "  , cast( cast( EMDCouponValue as numeric(18,2) ) /  power( 10, cast( right( EMDCouponCurrency, 1 ) as int ) ) as numeric(18,2) ) as EMDCouponValue   " + Environment.NewLine;
            sql += "  ,EMDCouponCurrency   " + Environment.NewLine;
            sql += "  , EMDRelatedTicketNumber   " + Environment.NewLine;
            sql += "  , f4.EMDRelatedCouponNumber   " + Environment.NewLine;
            sql += "  ,  EMDReasonforIssuanceSubCode   " + Environment.NewLine;
            sql += "  , EMDFeeOwnerAirlineDesignator   " + Environment.NewLine;
            sql += "  , EMDExcessBaggageOverAllowanceQualifier   " + Environment.NewLine;
            sql += "  , EMDExcessBaggageRateperUnit   " + Environment.NewLine;
            sql += "  , EMDExcessBaggageTotalNumberinExcess   " + Environment.NewLine;
            sql += "  , EMDConsumedatIssuanceIndicator   " + Environment.NewLine;
            sql += "  , EMDExcessBaggageCurrencyType   " + Environment.NewLine;
            sql += "  , f3.ReasonforIssuanceCode   " + Environment.NewLine;
            sql += " , f3.ReasonforIssuanceDescription , EMDRemarks   " + Environment.NewLine;
            sql += "    " + Environment.NewLine;
            sql += " from Pax.SalesDocumentHeader f1    " + Environment.NewLine;
            sql += " join FileRet.It02_Transaction f2 on f1.UploadedRef = f2.UploadGuid and f1.DocumentNumber = f2.TicketDocumentNumber and f1.TransactionCode = f2.TransactionCode   " + Environment.NewLine;
            sql += " join FileRet.It0A_MiscellaneousDocument f3 on f2.UploadGuid = f3.UploadGuid and f2.TransactionNumber = f3.TransactionNumber   " + Environment.NewLine;
            sql += " join FileRet.It0G_EMBCouponDetails f4 on f2.UploadGuid = f4.UploadGuid and f2.TransactionNumber = f4.TransactionNumber   " + Environment.NewLine;
            sql += "    " + Environment.NewLine;
            sql += " )   " + Environment.NewLine;
            sql += "    " + Environment.NewLine;
            sql += "    " + Environment.NewLine;
            sql += " select * from FINAL where DocumentNumber like '" + DocNum + "'    " + Environment.NewLine;

            SqlCommand cmd = new SqlCommand(sql, cs);
            DataSet ds = new DataSet();
            SqlDataAdapter ada = new SqlDataAdapter(cmd);
            ada.Fill(ds);
            int ligne = ds.Tables[0].Rows.Count;
            string[,] flownData = new string[4, ligne];

            foreach (DataRow dr in ds.Tables[0].Rows)
            {

                ViewBag.DocumentNo = dr[ds.Tables[0].Columns[0].ColumnName].ToString();

                ViewBag.TRANSACTIONCODE = dr[ds.Tables[0].Columns[1].ColumnName].ToString();

                ViewBag.EMDCOUPONNUMBER = dr[ds.Tables[0].Columns[2].ColumnName].ToString();

                ViewBag.EMDCOUPONVALUE = dr[ds.Tables[0].Columns[3].ColumnName].ToString();

                ViewBag.EMDCOUPONCURRENCY = dr[ds.Tables[0].Columns[4].ColumnName].ToString();

                ViewBag.EMDRELATEDTICKETNUMBER = dr[ds.Tables[0].Columns[5].ColumnName].ToString();

                ViewBag.EMDRELATEDCOUPONNUMBER = dr[ds.Tables[0].Columns[6].ColumnName].ToString();

                ViewBag.EMDREASONFORISSUANCESUBCODE = dr[ds.Tables[0].Columns[7].ColumnName].ToString();

                ViewBag.EMDFEEOWNERAIRLINEDESIGNATOR = dr[ds.Tables[0].Columns[8].ColumnName].ToString();

                ViewBag.EMDEXCESSBAGGAGEOVERALLOWANCEQUALIFIER = dr[ds.Tables[0].Columns[9].ColumnName].ToString();

                ViewBag.EMDEXCESSBAGGAGERATEPERUNIT = dr[ds.Tables[0].Columns[10].ColumnName].ToString();

                ViewBag.EMDEXCESSBAGGAGETOTALNUMBERINEXCESS = dr[ds.Tables[0].Columns[11].ColumnName].ToString();

                ViewBag.EMDCONSUMEDATISSUANCEINDICATOR = dr[ds.Tables[0].Columns[12].ColumnName].ToString();

                ViewBag.EMDEXCESSBAGGAGECURRENCYTYPE = dr[ds.Tables[0].Columns[13].ColumnName].ToString();

                ViewBag.REASONFORISSUANCECODE = dr[ds.Tables[0].Columns[14].ColumnName].ToString();

                ViewBag.REASONFORISSUANCEDESCRIPTION = dr[ds.Tables[0].Columns[15].ColumnName].ToString();

                ViewBag.EMDREMARKS = dr[ds.Tables[0].Columns[16].ColumnName].ToString();
            }

            cs.Close();

            return PartialView(ds);
        }

        public ActionResult EMDShowView(String[] dataValue) {

            string ex = dataValue[0];

            SqlConnection cs = new SqlConnection(pbConnectionString);
            if (cs.State == ConnectionState.Open)
            {
                cs.Close();
            }

            cs.Open();

            string sql = " select TOP 1  " + Environment.NewLine;
            sql = sql + " ---PANEL 1---  " + Environment.NewLine;
            sql = sql + " left(sdh.DocumentNumber,3) as ALC,right(sdh.documentnumber,10) as DOC , sdh.CheckDigit  " + Environment.NewLine;
            sql = sql + " ,sdh.VendorIdentification as Vendoridentifier,sdh.ReportingSystemIdentifier,srd.CouponIndicator,sdh.TourCode,sdh.ITBT,sdh.TrueOriginDestinationCityCodes  " + Environment.NewLine;
            sql = sql + " ,sdh.BookingReference as PNR, OriginalIssueDocumentNumber  " + Environment.NewLine;
            sql = sql + "   " + Environment.NewLine;
            sql = sql + " ---DATE AND PLACE OF ISSUE---  " + Environment.NewLine;
            sql = sql + " ,Format(sdh.DateofIssue,'dd-MMM-yyyy'),sdh.AgentNumericCode  " + Environment.NewLine;
            sql = sql + " ,isnull(agtown.LegalName,agt.LegalName) as AgentName  " + Environment.NewLine;
            sql = sql + " ,isnull(agtown.LocationCity,agt.LocationCity) as POS  " + Environment.NewLine;
            sql = sql + " ,sdh.BookingAgentID  " + Environment.NewLine;
            sql = sql + "   " + Environment.NewLine;
            sql = sql + " ---PAXNAME---  " + Environment.NewLine;
            sql = sql + " ,sdh.PassengerName,sdh.FareCalculationArea,sdh.EndosRestriction,sdh.FareCalculationModeIndicator  " + Environment.NewLine;
            sql = sql + "   " + Environment.NewLine;
            sql = sql + " ,sdh.[FareCurrency],sdh.[Fare]  " + Environment.NewLine;
            sql = sql + " ,sdh.[TotalCurrency] as [Eqi Curr], sdh.[EquivalentFare]  " + Environment.NewLine;
            sql = sql + " ,sdh.SurchargeCollectedCurrency,sdh.SurchargeCollected  " + Environment.NewLine;
            sql = sql + " ,sdh.TaxCollectedCurrency,sdh.TaxCollected  " + Environment.NewLine;
            sql = sql + " ,sdh.[Tax3Currency],sdh.[Tax3Amount]  " + Environment.NewLine;
            sql = sql + " ,sdh.AmountCollectedCurrency,sdh.AmountCollected  " + Environment.NewLine;
            sql = sql + "   " + Environment.NewLine;
            sql = sql + " ---others---  " + Environment.NewLine;
            sql = sql + " ,sdh.InConnectionWithDocumentNumber  " + Environment.NewLine;
            sql = sql + " ,sdh.[ExchangeADC]  " + Environment.NewLine;
            sql = sql + " ,sdh.[BookingAgentIdentification]  " + Environment.NewLine;
            sql = sql + " ,sdh.[BookingAgencyLocationNumber]  " + Environment.NewLine;
            sql = sql + " ,sdh.[SettlementAuthorizationCode]  " + Environment.NewLine;
            sql = sql + " ,sdh.[TransactionCode]  " + Environment.NewLine;
            sql = sql + " ,sdh.[TransactionGroup]  " + Environment.NewLine;
            sql = sql + " ,sdh.[PaxType]  " + Environment.NewLine;
            sql = sql + " ,sdh.[DataSource]  " + Environment.NewLine;
            sql = sql + " ,sdh.[InvoluntaryReroute],iif(sdh.DocumentNumber<>srd.RelatedDocumentNumber,srd.RelatedDocumentNumber,''),sdh.hdrguid,sdh.paxspc  " + Environment.NewLine;
            sql = sql + "   " + Environment.NewLine;
            sql = sql + " from Pax.SalesDocumentHeader sdh   " + Environment.NewLine;
            sql = sql + " join Pax.SalesRelatedDocumentInformation srd on sdh.HdrGuid = srd.HdrGuid     " + Environment.NewLine;
            sql = sql + " left join Pax.SalesDocumentCoupon f1 on srd.RelatedDocumentGuid = f1.RelatedDocumentGuid    " + Environment.NewLine;
            sql = sql + " left join Ref.Agent_Own agtown	on left(sdh.AgentNumericCode,7) = agtown.AgencyNumericCode  " + Environment.NewLine;
            sql = sql + " left join Ref.Agent	agt	on left(sdh.AgentNumericCode,7) = agt.AgencyNumericCode  " + Environment.NewLine;
            sql = sql + " where sdh.DocumentNumber like '" + ex + "'   order by TransactionGroup " + Environment.NewLine;

            SqlCommand cmd = new SqlCommand(sql, cs);
            DataSet ds = new DataSet();
            SqlDataAdapter ada = new SqlDataAdapter(cmd);
            ada.Fill(ds);
            //int ligne = ds.Tables[0].Rows.Count;
            //string[,] flownData = new string[4, ligne];

            foreach (DataRow dr in ds.Tables[0].Rows)
            {

                ViewBag.txtDocumentCarrier = dr[ds.Tables[0].Columns[0].ColumnName].ToString();

                ViewBag.txtDocumentNumber = dr[ds.Tables[0].Columns[1].ColumnName].ToString();

                ViewBag.txtChkDgt = dr[ds.Tables[0].Columns[2].ColumnName].ToString();

                ViewBag.txtVendorIdentifier = dr[ds.Tables[0].Columns[3].ColumnName].ToString();

                ViewBag.txtReportingSystemIdentifier = dr[ds.Tables[0].Columns[4].ColumnName].ToString();

                ViewBag.txtCPUI = dr[ds.Tables[0].Columns[5].ColumnName].ToString();

                ViewBag.txtTourCode = dr[ds.Tables[0].Columns[6].ColumnName].ToString();

                ViewBag.txtOrgDest = dr[ds.Tables[0].Columns[8].ColumnName].ToString();

                ViewBag.txtPnr = dr[ds.Tables[0].Columns[9].ColumnName].ToString();

                ViewBag.issuedInExcLinkLabel = dr[ds.Tables[0].Columns[10].ColumnName].ToString();

                ViewBag.dtpIssDoc = dr[ds.Tables[0].Columns[11].ColumnName].ToString();

                ViewBag.txtCustomerCode = dr[ds.Tables[0].Columns[12].ColumnName].ToString();

                ViewBag.txtAgentName = dr[ds.Tables[0].Columns[13].ColumnName].ToString();

                ViewBag.txtPOS = dr[ds.Tables[0].Columns[14].ColumnName].ToString();

                ViewBag.txtBookingAgentID = dr[ds.Tables[0].Columns[15].ColumnName].ToString();

                ViewBag.txtPassengerName = dr[ds.Tables[0].Columns[16].ColumnName].ToString();

                ViewBag.txtFareComponent = dr[ds.Tables[0].Columns[17].ColumnName].ToString();

                ViewBag.txtEndorsementArea = dr[ds.Tables[0].Columns[18].ColumnName].ToString();

                ViewBag.txtFcmi = dr[ds.Tables[0].Columns[19].ColumnName].ToString();

                ViewBag.txtInConnectionWith = dr[ds.Tables[0].Columns[32].ColumnName].ToString();

                ViewBag.txtExchangeADC = dr[ds.Tables[0].Columns[33].ColumnName].ToString();

                ViewBag.txtBAlocationNo = dr[ds.Tables[0].Columns[35].ColumnName].ToString();

                ViewBag.txtAuthorisationCode = dr[ds.Tables[0].Columns[36].ColumnName].ToString();

                ViewBag.txtTransCode = dr[ds.Tables[0].Columns[37].ColumnName].ToString();

                ViewBag.txtTransactionGroup = dr[ds.Tables[0].Columns[38].ColumnName].ToString();

                ViewBag.txtPaxType = dr[ds.Tables[0].Columns[39].ColumnName].ToString();

                ViewBag.txtdatasource = dr[ds.Tables[0].Columns[40].ColumnName].ToString();

                ViewBag.txtInvRer = dr[ds.Tables[0].Columns[41].ColumnName].ToString();

                ViewBag.txtRelateDocNo = dr[ds.Tables[0].Columns[42].ColumnName].ToString();

                ViewBag.es = dr[ds.Tables[0].Columns[43].ColumnName].ToString();

               string sdf = dr[ds.Tables[0].Columns[7].ColumnName].ToString();

                if (sdf == "IT")
                    {
                       ViewBag.It= "Oui";

                    }
                    else
                    {
                       ViewBag.It = "";
                    }

                ViewBag.txtSPC = dr[ds.Tables[0].Columns[44].ColumnName].ToString();

                ViewBag.txtfarecur = dr[ds.Tables[0].Columns[20].ColumnName].ToString();

                ViewBag.txtfare = dr[ds.Tables[0].Columns[21].ColumnName].ToString();

                ViewBag.txtEfpcur = dr[ds.Tables[0].Columns[22].ColumnName].ToString();

                ViewBag.txtEfp = dr[ds.Tables[0].Columns[23].ColumnName].ToString();

                ViewBag.txtSurchargeCur = dr[ds.Tables[0].Columns[24].ColumnName].ToString();

                ViewBag.txtSurcharge = dr[ds.Tables[0].Columns[25].ColumnName].ToString();

                ViewBag.txttotaltaxCur = dr[ds.Tables[0].Columns[26].ColumnName].ToString();

                ViewBag.txttotaltax = dr[ds.Tables[0].Columns[27].ColumnName].ToString();

                ViewBag.txtAmtColCur = dr[ds.Tables[0].Columns[30].ColumnName].ToString();

                ViewBag.txtAmtCol = dr[ds.Tables[0].Columns[31].ColumnName].ToString();

            }
            return PartialView(ds);

        }
        public ActionResult RELATEDDocumentShowView(String[] dataValue)
        {

            string ex = dataValue[0];

            SqlConnection cs = new SqlConnection(pbConnectionString);
            if (cs.State == ConnectionState.Open)
            {
                cs.Close();
            }

            cs.Open();

            string sql = " select TOP 1  " + Environment.NewLine;
            sql = sql + " ---PANEL 1---  " + Environment.NewLine;
            sql = sql + " left(sdh.DocumentNumber,3) as ALC,right(sdh.documentnumber,10) as DOC , sdh.CheckDigit  " + Environment.NewLine;
            sql = sql + " ,sdh.VendorIdentification as Vendoridentifier,sdh.ReportingSystemIdentifier,srd.CouponIndicator,sdh.TourCode,sdh.ITBT,sdh.TrueOriginDestinationCityCodes  " + Environment.NewLine;
            sql = sql + " ,sdh.BookingReference as PNR, OriginalIssueDocumentNumber  " + Environment.NewLine;
            sql = sql + "   " + Environment.NewLine;
            sql = sql + " ---DATE AND PLACE OF ISSUE---  " + Environment.NewLine;
            sql = sql + " ,Format(sdh.DateofIssue,'dd-MMM-yyyy'),sdh.AgentNumericCode  " + Environment.NewLine;
            sql = sql + " ,isnull(agtown.LegalName,agt.LegalName) as AgentName  " + Environment.NewLine;
            sql = sql + " ,isnull(agtown.LocationCity,agt.LocationCity) as POS  " + Environment.NewLine;
            sql = sql + " ,sdh.BookingAgentID  " + Environment.NewLine;
            sql = sql + "   " + Environment.NewLine;
            sql = sql + " ---PAXNAME---  " + Environment.NewLine;
            sql = sql + " ,sdh.PassengerName,sdh.FareCalculationArea,sdh.EndosRestriction,sdh.FareCalculationModeIndicator  " + Environment.NewLine;
            sql = sql + "   " + Environment.NewLine;
            sql = sql + " ,sdh.[FareCurrency],sdh.[Fare]  " + Environment.NewLine;
            sql = sql + " ,sdh.[TotalCurrency] as [Eqi Curr], sdh.[EquivalentFare]  " + Environment.NewLine;
            sql = sql + " ,sdh.SurchargeCollectedCurrency,sdh.SurchargeCollected  " + Environment.NewLine;
            sql = sql + " ,sdh.TaxCollectedCurrency,sdh.TaxCollected  " + Environment.NewLine;
            sql = sql + " ,sdh.[Tax3Currency],sdh.[Tax3Amount]  " + Environment.NewLine;
            sql = sql + " ,sdh.AmountCollectedCurrency,sdh.AmountCollected  " + Environment.NewLine;
            sql = sql + "   " + Environment.NewLine;
            sql = sql + " ---others---  " + Environment.NewLine;
            sql = sql + " ,sdh.InConnectionWithDocumentNumber  " + Environment.NewLine;
            sql = sql + " ,sdh.[ExchangeADC]  " + Environment.NewLine;
            sql = sql + " ,sdh.[BookingAgentIdentification]  " + Environment.NewLine;
            sql = sql + " ,sdh.[BookingAgencyLocationNumber]  " + Environment.NewLine;
            sql = sql + " ,sdh.[SettlementAuthorizationCode]  " + Environment.NewLine;
            sql = sql + " ,sdh.[TransactionCode]  " + Environment.NewLine;
            sql = sql + " ,sdh.[TransactionGroup]  " + Environment.NewLine;
            sql = sql + " ,sdh.[PaxType]  " + Environment.NewLine;
            sql = sql + " ,sdh.[DataSource]  " + Environment.NewLine;
            sql = sql + " ,sdh.[InvoluntaryReroute],iif(sdh.DocumentNumber<>srd.RelatedDocumentNumber,srd.RelatedDocumentNumber,''),sdh.hdrguid,sdh.paxspc  " + Environment.NewLine;
            sql = sql + "   " + Environment.NewLine;
            sql = sql + " from Pax.SalesDocumentHeader sdh   " + Environment.NewLine;
            sql = sql + " join Pax.SalesRelatedDocumentInformation srd on sdh.HdrGuid = srd.HdrGuid     " + Environment.NewLine;
            sql = sql + " left join Pax.SalesDocumentCoupon f1 on srd.RelatedDocumentGuid = f1.RelatedDocumentGuid    " + Environment.NewLine;
            sql = sql + " left join Ref.Agent_Own agtown	on left(sdh.AgentNumericCode,7) = agtown.AgencyNumericCode  " + Environment.NewLine;
            sql = sql + " left join Ref.Agent	agt	on left(sdh.AgentNumericCode,7) = agt.AgencyNumericCode  " + Environment.NewLine;
            sql = sql + " where sdh.DocumentNumber like '" + ex + "'   order by TransactionGroup " + Environment.NewLine;

            SqlCommand cmd = new SqlCommand(sql, cs);
            DataSet ds = new DataSet();
            SqlDataAdapter ada = new SqlDataAdapter(cmd);
            ada.Fill(ds);
            //int ligne = ds.Tables[0].Rows.Count;
            //string[,] flownData = new string[4, ligne];

            foreach (DataRow dr in ds.Tables[0].Rows)
            {

                ViewBag.txtDocumentCarrier = dr[ds.Tables[0].Columns[0].ColumnName].ToString();

                ViewBag.txtDocumentNumber = dr[ds.Tables[0].Columns[1].ColumnName].ToString();

                ViewBag.txtChkDgt = dr[ds.Tables[0].Columns[2].ColumnName].ToString();

                ViewBag.txtVendorIdentifier = dr[ds.Tables[0].Columns[3].ColumnName].ToString();

                ViewBag.txtReportingSystemIdentifier = dr[ds.Tables[0].Columns[4].ColumnName].ToString();

                ViewBag.txtCPUI = dr[ds.Tables[0].Columns[5].ColumnName].ToString();

                ViewBag.txtTourCode = dr[ds.Tables[0].Columns[6].ColumnName].ToString();

                ViewBag.txtOrgDest = dr[ds.Tables[0].Columns[8].ColumnName].ToString();

                ViewBag.txtPnr = dr[ds.Tables[0].Columns[9].ColumnName].ToString();

                ViewBag.issuedInExcLinkLabel = dr[ds.Tables[0].Columns[10].ColumnName].ToString();

                ViewBag.dtpIssDoc = dr[ds.Tables[0].Columns[11].ColumnName].ToString();

                ViewBag.txtCustomerCode = dr[ds.Tables[0].Columns[12].ColumnName].ToString();

                ViewBag.txtAgentName = dr[ds.Tables[0].Columns[13].ColumnName].ToString();

                ViewBag.txtPOS = dr[ds.Tables[0].Columns[14].ColumnName].ToString();

                ViewBag.txtBookingAgentID = dr[ds.Tables[0].Columns[15].ColumnName].ToString();

                ViewBag.txtPassengerName = dr[ds.Tables[0].Columns[16].ColumnName].ToString();

                ViewBag.txtFareComponent = dr[ds.Tables[0].Columns[17].ColumnName].ToString();

                ViewBag.txtEndorsementArea = dr[ds.Tables[0].Columns[18].ColumnName].ToString();

                ViewBag.txtFcmi = dr[ds.Tables[0].Columns[19].ColumnName].ToString();

                ViewBag.txtInConnectionWith = dr[ds.Tables[0].Columns[32].ColumnName].ToString();

                ViewBag.txtExchangeADC = dr[ds.Tables[0].Columns[33].ColumnName].ToString();

                ViewBag.txtBAlocationNo = dr[ds.Tables[0].Columns[35].ColumnName].ToString();

                ViewBag.txtAuthorisationCode = dr[ds.Tables[0].Columns[36].ColumnName].ToString();

                ViewBag.txtTransCode = dr[ds.Tables[0].Columns[37].ColumnName].ToString();

                ViewBag.txtTransactionGroup = dr[ds.Tables[0].Columns[38].ColumnName].ToString();

                ViewBag.txtPaxType = dr[ds.Tables[0].Columns[39].ColumnName].ToString();

                ViewBag.txtdatasource = dr[ds.Tables[0].Columns[40].ColumnName].ToString();

                ViewBag.txtInvRer = dr[ds.Tables[0].Columns[41].ColumnName].ToString();

                ViewBag.txtRelateDocNo = dr[ds.Tables[0].Columns[42].ColumnName].ToString();

                ViewBag.es = dr[ds.Tables[0].Columns[43].ColumnName].ToString();

                string sdf = dr[ds.Tables[0].Columns[7].ColumnName].ToString();

                if (sdf == "IT")
                {
                    ViewBag.It = "Oui";

                }
                else
                {
                    ViewBag.It = "";
                }

                ViewBag.txtSPC = dr[ds.Tables[0].Columns[44].ColumnName].ToString();

                ViewBag.txtfarecur = dr[ds.Tables[0].Columns[20].ColumnName].ToString();

                ViewBag.txtfare = dr[ds.Tables[0].Columns[21].ColumnName].ToString();

                ViewBag.txtEfpcur = dr[ds.Tables[0].Columns[22].ColumnName].ToString();

                ViewBag.txtEfp = dr[ds.Tables[0].Columns[23].ColumnName].ToString();

                ViewBag.txtSurchargeCur = dr[ds.Tables[0].Columns[24].ColumnName].ToString();

                ViewBag.txtSurcharge = dr[ds.Tables[0].Columns[25].ColumnName].ToString();

                ViewBag.txttotaltaxCur = dr[ds.Tables[0].Columns[26].ColumnName].ToString();

                ViewBag.txttotaltax = dr[ds.Tables[0].Columns[27].ColumnName].ToString();

                ViewBag.txtAmtColCur = dr[ds.Tables[0].Columns[30].ColumnName].ToString();

                ViewBag.txtAmtCol = dr[ds.Tables[0].Columns[31].ColumnName].ToString();

            }
            return PartialView(ds);

        }




        /******************************ATDA******************************/
        //sml  6104658475519 ///
        public ActionResult searchATDA(String[] dataValue)
        {
            String DocNum = dataValue[0];
            String ag = dataValue[0];
            SqlConnection cs = new SqlConnection(pbConnectionString);
            if (cs.State == ConnectionState.Open)
            {
                cs.Close();
            }

            cs.Open();

            string sql = " select   " + Environment.NewLine;
            sql = sql + " ---PANEL 1---  " + Environment.NewLine;
            sql = sql + " left(sdh.DocumentNumber,3) as ALC,right(sdh.documentnumber,10) as DOC , sdh.CheckDigit  " + Environment.NewLine;
            sql = sql + " ,sdh.VendorIdentification as Vendoridentifier,sdh.ReportingSystemIdentifier,srd.CouponIndicator,sdh.TourCode,sdh.ITBT,sdh.TrueOriginDestinationCityCodes  " + Environment.NewLine;
            sql = sql + " ,sdh.BookingReference as PNR, OriginalIssueDocumentNumber  " + Environment.NewLine;
            sql = sql + "   " + Environment.NewLine;
            sql = sql + " ---DATE AND PLACE OF ISSUE---  " + Environment.NewLine;
            sql = sql + " ,sdh.DateofIssue,sdh.AgentNumericCode  " + Environment.NewLine;
            sql = sql + " ,f2.LegalName as AgentName  " + Environment.NewLine;
            sql = sql + " ,f2.LocationCity as POS  " + Environment.NewLine;
            sql = sql + " ,sdh.BookingAgentID  " + Environment.NewLine;
            sql = sql + "   " + Environment.NewLine;
            sql = sql + " ---PAXNAME---  " + Environment.NewLine;
            sql = sql + " ,sdh.PassengerName,sdh.FareCalculationArea,sdh.EndosRestriction,sdh.FareCalculationModeIndicator  " + Environment.NewLine;
            sql = sql + "   " + Environment.NewLine;
            sql = sql + " ,sdh.[FareCurrency],sdh.[Fare]  " + Environment.NewLine;
            sql = sql + " ,sdh.[TotalCurrency] as [Eqi Curr], sdh.[EquivalentFare]  " + Environment.NewLine;
            sql = sql + " ,sdh.[Tax1Currency],sdh.[Tax1Amount]  " + Environment.NewLine;
            sql = sql + " ,sdh.[Tax2Currency],sdh.[Tax2Amount]  " + Environment.NewLine;
            sql = sql + " ,sdh.[Tax3Currency],sdh.[Tax3Amount]  " + Environment.NewLine;
            sql = sql + " ,sdh.[TotalCurrency],sdh.[TotalAmount]  " + Environment.NewLine;
            sql = sql + "   " + Environment.NewLine;
            sql = sql + " ---others---  " + Environment.NewLine;
            sql = sql + " ,sdh.InConnectionWithDocumentNumber  " + Environment.NewLine;
            sql = sql + " ,sdh.[ExchangeADC]  " + Environment.NewLine;
            sql = sql + " ,sdh.[BookingAgentIdentification]  " + Environment.NewLine;
            sql = sql + " ,sdh.[BookingAgencyLocationNumber]  " + Environment.NewLine;
            sql = sql + " ,sdh.[SettlementAuthorizationCode]  " + Environment.NewLine;
            sql = sql + " ,sdh.[TransactionCode]  " + Environment.NewLine;
            sql = sql + " ,sdh.[TransactionGroup]  " + Environment.NewLine;
            sql = sql + " ,sdh.[PaxType]  " + Environment.NewLine;
            sql = sql + " ,sdh.[DataSource]  " + Environment.NewLine;
            sql = sql + " ,sdh.[InvoluntaryReroute],iif(sdh.DocumentNumber<>srd.RelatedDocumentNumber,srd.RelatedDocumentNumber,''),sdh.hdrguid,sdh.paxspc  " + Environment.NewLine;
            sql = sql + "   " + Environment.NewLine;
            sql = sql + " from Pax.SalesDocumentHeader sdh   " + Environment.NewLine;
            sql = sql + " join Pax.SalesRelatedDocumentInformation srd on sdh.HdrGuid = srd.HdrGuid     " + Environment.NewLine;
            sql = sql + " left join Pax.SalesDocumentCoupon f1 on srd.RelatedDocumentGuid = f1.RelatedDocumentGuid    " + Environment.NewLine;
            sql = sql + " left join ref.Agent f2 on sdh.AgentNumericCode = f2.AgencyNumericCode  " + Environment.NewLine;
            sql = sql + " where sdh.DocumentNumber like '" + DocNum + "' " + Environment.NewLine;


            SqlCommand cmd = new SqlCommand(sql, cs);
            DataSet ds = new DataSet();
            SqlDataAdapter ada = new SqlDataAdapter(cmd);
            ada.Fill(ds);

            foreach (DataRow dr in ds.Tables[0].Rows)
            {

                ViewBag.txtDocumentCarrier = dr[ds.Tables[0].Columns[0].ColumnName].ToString();

                ViewBag.txtDocumentNumber = dr[ds.Tables[0].Columns[1].ColumnName].ToString();

                ViewBag.txtChkDgt = dr[ds.Tables[0].Columns[2].ColumnName].ToString();

                ViewBag.txtVendorIdentifier = dr[ds.Tables[0].Columns[3].ColumnName].ToString();

                ViewBag.txtReportingSystemIdentifier = dr[ds.Tables[0].Columns[4].ColumnName].ToString();

                ViewBag.txtCPUI = dr[ds.Tables[0].Columns[5].ColumnName].ToString();

                ViewBag.txtTourCode = dr[ds.Tables[0].Columns[6].ColumnName].ToString();

                ViewBag.txtOrgDest = dr[ds.Tables[0].Columns[8].ColumnName].ToString();

                ViewBag.txtPnr = dr[ds.Tables[0].Columns[9].ColumnName].ToString();

                ViewBag.issuedInExcLinkLabel = dr[ds.Tables[0].Columns[10].ColumnName].ToString();

                ViewBag.dtpIssDoc = dr[ds.Tables[0].Columns[11].ColumnName].ToString();

                txtCustomerCode = dr[ds.Tables[0].Columns[12].ColumnName].ToString();

                ViewBag.txtCustomerCode = txtCustomerCode;

                ViewBag.txtAgentName = dr[ds.Tables[0].Columns[13].ColumnName].ToString();

                ViewBag.txtPOS = dr[ds.Tables[0].Columns[14].ColumnName].ToString();

                ViewBag.txtBookingAgentID = dr[ds.Tables[0].Columns[15].ColumnName].ToString();

                ViewBag.txtPassengerName = dr[ds.Tables[0].Columns[16].ColumnName].ToString();

                ViewBag.txtFareComponent = dr[ds.Tables[0].Columns[17].ColumnName].ToString();

                ViewBag.txtEndorsementArea = dr[ds.Tables[0].Columns[18].ColumnName].ToString();

                ViewBag.txtFcmi = dr[ds.Tables[0].Columns[19].ColumnName].ToString();

                ViewBag.txtInConnectionWith = dr[ds.Tables[0].Columns[32].ColumnName].ToString();

                ViewBag.txtExchangeADC = dr[ds.Tables[0].Columns[33].ColumnName].ToString();

                ViewBag.txtBAlocationNo = dr[ds.Tables[0].Columns[35].ColumnName].ToString();

                ViewBag.txtAuthorisationCode = dr[ds.Tables[0].Columns[36].ColumnName].ToString();

                //ViewBag.txtTransCode = dr[ds.Tables[0].Columns[37].ColumnName].ToString();

                ViewBag.txtTransactionGroup = dr[ds.Tables[0].Columns[38].ColumnName].ToString();

                ViewBag.txtPaxType = dr[ds.Tables[0].Columns[39].ColumnName].ToString();

                ViewBag.txtdatasource = dr[ds.Tables[0].Columns[40].ColumnName].ToString();

                ViewBag.txtInvRer = dr[ds.Tables[0].Columns[41].ColumnName].ToString();

                ViewBag.txtRelateDocNo = dr[ds.Tables[0].Columns[42].ColumnName].ToString();

                es = dr[ds.Tables[0].Columns[43].ColumnName].ToString();

                string sdf = dr[ds.Tables[0].Columns[7].ColumnName].ToString();

                if (sdf == "IT")
                {
                    ViewBag.It = "Oui";

                }
                else
                {
                    ViewBag.It = "";
                }
            }

            // FORM OF PAYEMENT
            string sqlfop = "select FormofPaymentType as FOP ,Currency as [FOP Currency],Amount as [FOP Amount], RemittanceCurrency,RemittanceAmount from Pax.SalesDocumentPayment where HdrGuidRef like '" + es + "'  ";
            SqlCommand cmdfop = new SqlCommand(sqlfop, cs);
            DataSet dsfop = new DataSet();
            SqlDataAdapter adafop = new SqlDataAdapter(cmdfop);
            adafop.Fill(dsfop);
            int lonfop = dsfop.Tables[0].Rows.Count;
            string[,] AgCodefop = new string[5, lonfop];
            int i = 0;
            foreach (DataRow drfop in dsfop.Tables[0].Rows)
            {
                for (int j = 0; j < 5; j++)
                {
                    AgCodefop[j, i] = drfop[dsfop.Tables[0].Columns[j].ColumnName].ToString();
                }
                i++;
            }
            ViewBag.AgCodefop = AgCodefop;
            ViewBag.lonfop = lonfop;

            ////FARE//////
            string sqlfare = " select f1.FareCurrency,f1.Fare,f1.TotalCurrency as EFPCurrency,f1.EquivalentFare as EFP  ";
            sqlfare = sqlfare + " , f1.AmountCollectedCurrency,f1.AmountCollected from Pax.salesdocumentheader f1  ";
            sqlfare = sqlfare + " left join pax.SalesRelatedDocumentInformation f2 on f1.HdrGuid = f2.HdrGuid where f1.DocumentNumber like '" + DocNum + "'  ";

            SqlCommand cmdfare = new SqlCommand(sqlfare, cs);
            DataSet dsfare = new DataSet();
            SqlDataAdapter adafare = new SqlDataAdapter(cmdfare);
            adafare.Fill(dsfare);

            foreach (DataRow drfare in dsfare.Tables[0].Rows)
            {
                ViewBag.txtfarecur = drfare[dsfare.Tables[0].Columns[0].ColumnName].ToString();

                ViewBag.txtfare = drfare[dsfare.Tables[0].Columns[1].ColumnName].ToString();

                ViewBag.txtEfp = drfare[dsfare.Tables[0].Columns[2].ColumnName].ToString();

                ViewBag.txtEfpcur = drfare[dsfare.Tables[0].Columns[2].ColumnName].ToString();

                ViewBag.txtAmtColCur = drfare[dsfare.Tables[0].Columns[2].ColumnName].ToString();

                ViewBag.txtAmtCol = drfare[dsfare.Tables[0].Columns[2].ColumnName].ToString();
            }

            ////////////// TAX /////////////

            string sqltax = "select f1.DocumentAmountType, f1.OtherAmountCode,f1.CurrencyType as [Tax Currency] ,f1.OtherAmount from Pax.SalesDocumentOtherAmount f1  ";
            sqltax = sqltax + "where (f1.DocumentAmountType in ( 'Tax', 'Surcharges' )  or (DocumentAmountType like 'Com%' and OtherAmountCode = 'effective') or DocumentAmountType like 'TaxCom%' ) and HdrGuidRef like '" + es + "'";
            SqlCommand cmdtax = new SqlCommand(sqltax, cs);
            DataSet dstax = new DataSet();
            SqlDataAdapter adatax = new SqlDataAdapter(cmdtax);
            adatax.Fill(dstax);
            int lontax = dstax.Tables[0].Rows.Count;
            string[,] AgCodetax = new string[4, lontax];
            int t = 0;
            foreach (DataRow drtax in dstax.Tables[0].Rows)
            {
                for (int x = 0; x < 4; x++)
                {
                    AgCodetax[x, t] = drtax[dstax.Tables[0].Columns[x].ColumnName].ToString();
                }
                t++;
            }

            ViewBag.AgCodetax = AgCodetax;
            ViewBag.lontax = lontax;

            //////////////Ancillaries///////

            string sqlAncillaries = "SELECT [ReasonforIssuanceDescription] as Ancillaries ,f3.FareCurrency ,f3.Fare ";
            sqlAncillaries = sqlAncillaries + "FROM [FileRet].[It0A_MiscellaneousDocument] f1  ";
            sqlAncillaries = sqlAncillaries + "join FileRet.It02_Transaction f2 on f1.UploadGuid = f2.UploadGuid and f1.TransactionNumber = f2.TransactionNumber ";
            sqlAncillaries = sqlAncillaries + "join pax.SalesDocumentHeader f3 on f2.TicketDocumentNumber = f3.DocumentNumber and f2.TransactionCode = f3.TransactionCode ";
            sqlAncillaries = sqlAncillaries + "where f1.InConnectionwithDocumentNumber =  '" + DocNum + "' ";
            SqlCommand cmdAncillaries = new SqlCommand(sqlAncillaries, cs);
            DataSet dsAncillaries = new DataSet();
            SqlDataAdapter adaAncillaries = new SqlDataAdapter(cmdAncillaries);
            adaAncillaries.Fill(dsAncillaries);
            int lonAncillaries = dsAncillaries.Tables[0].Rows.Count;
            string[,] AgCodeAncillaries = new string[3, lonAncillaries];
            int a = 0;
            foreach (DataRow drAncillaries in dsAncillaries.Tables[0].Rows)
            {
                for (int n = 0; n < 3; n++)
                {
                    AgCodeAncillaries[n, a] = drAncillaries[dsAncillaries.Tables[0].Columns[n].ColumnName].ToString();
                }
                a++;
            }

            ViewBag.AgCodeAncillaries = AgCodeAncillaries;
            ViewBag.lonAncillaries = lonAncillaries;

            ///////////////////// DISPLAY IF DOCUMENT HAS BEEN MODIFIED/////////////////////////

            string sqlModifiedtkkt = " select  top 1 sdh.DocumentNumber   " + Environment.NewLine;
            sqlModifiedtkkt = sqlModifiedtkkt + " ,isnull(sdho.Fare,'0') as [Original Fare]  " + Environment.NewLine;
            sqlModifiedtkkt = sqlModifiedtkkt + " ,isnull(sdho.FareCurrency,'') as [Original Fare Currency]   " + Environment.NewLine;
            sqlModifiedtkkt = sqlModifiedtkkt + "   " + Environment.NewLine;
            sqlModifiedtkkt = sqlModifiedtkkt + " ,sdh.fare as [Modify Fare]  " + Environment.NewLine;
            sqlModifiedtkkt = sqlModifiedtkkt + " ,sdh.FareCurrency as [Modify Fare Currency]   " + Environment.NewLine;
            sqlModifiedtkkt = sqlModifiedtkkt + "   " + Environment.NewLine;
            sqlModifiedtkkt = sqlModifiedtkkt + " ,sdho.EquivalentFare as  [Original Equivalent Fare]  " + Environment.NewLine;
            sqlModifiedtkkt = sqlModifiedtkkt + " ,cast (sdh.fare * sdh.USDRatePayCur as numeric (18,2)) as [Reassessed Equivalent Fare]  " + Environment.NewLine;
            sqlModifiedtkkt = sqlModifiedtkkt + " ,cast (sdho.Fare / sdh.USDRatePayCur as numeric (18,2)) as [Original Payment In USD]  " + Environment.NewLine;
            sqlModifiedtkkt = sqlModifiedtkkt + "   " + Environment.NewLine;
            sqlModifiedtkkt = sqlModifiedtkkt + " ,cast (sdho.Fare / sdh.USDRatePayCur - sdh.fare as numeric (18,2)) as [Difference Fare in COP]  " + Environment.NewLine;
            sqlModifiedtkkt = sqlModifiedtkkt + " ,cast (sdh.fare * sdh.USDRatePayCur - isnull(sdho.Fare,sdh.fare)as numeric (18,2)) as [Difference Fare IN COC ]  " + Environment.NewLine;
            sqlModifiedtkkt = sqlModifiedtkkt + " from Pax.SalesDocumentHeader sdh left join Pax.SalesDocumentHeaderOriginal sdho on sdh.HdrGuid =sdho.HdrGuid    " + Environment.NewLine;
            sqlModifiedtkkt = sqlModifiedtkkt + " where sdh.DocumentNumber = '" + DocNum + "' and sdho.Fare is not null  " + Environment.NewLine;


            SqlCommand cmdModifiedtkkt = new SqlCommand(sqlModifiedtkkt, cs);
            DataSet dsModifiedtkkt = new DataSet();
            SqlDataAdapter adaModifiedtkkt = new SqlDataAdapter(cmdModifiedtkkt);
            adaModifiedtkkt.Fill(dsModifiedtkkt);

            foreach (DataRow drModifiedtkkt in dsModifiedtkkt.Tables[0].Rows)
            {
                ViewBag.txtOriFareCur = drModifiedtkkt[dsModifiedtkkt.Tables[0].Columns[2].ColumnName].ToString();

                ViewBag.txtOriFareAmt = drModifiedtkkt[dsModifiedtkkt.Tables[0].Columns[1].ColumnName].ToString();

                ViewBag.txtModFareAmt = drModifiedtkkt[dsModifiedtkkt.Tables[0].Columns[3].ColumnName].ToString();

                ViewBag.txtModFareCur = drModifiedtkkt[dsModifiedtkkt.Tables[0].Columns[4].ColumnName].ToString();

                ViewBag.txtOrgEquiFare = drModifiedtkkt[dsModifiedtkkt.Tables[0].Columns[5].ColumnName].ToString();

                ViewBag.txtResEqui = drModifiedtkkt[dsModifiedtkkt.Tables[0].Columns[6].ColumnName].ToString();

                ViewBag.txtOriPayUSD = drModifiedtkkt[dsModifiedtkkt.Tables[0].Columns[7].ColumnName].ToString();

                ViewBag.txtDiffFareCOP = drModifiedtkkt[dsModifiedtkkt.Tables[0].Columns[8].ColumnName].ToString();

                ViewBag.txtDiffCOC = drModifiedtkkt[dsModifiedtkkt.Tables[0].Columns[9].ColumnName].ToString();
            }

            /////////////////////////DISPLAY TEXT BOX SURCHARGE INFO////////////////////////

            string sqltotalsurcharge = "select sum(f1.OtherAmount) as [Total Surcharge],f1.CurrencyType as [Surcharge Currency] from Pax.SalesDocumentOtherAmount f1 ";
            sqltotalsurcharge = sqltotalsurcharge + " where f1.DocumentAmountType ='Surcharges' and HdrGuidRef like '" + es + "'  group by  f1.DocumentAmountType,f1.CurrencyType ";

            SqlCommand cmdtotalsurcharge = new SqlCommand(sqltotalsurcharge, cs);
            DataSet dstotalsurcharge = new DataSet();
            SqlDataAdapter adatotalsurcharge = new SqlDataAdapter(cmdtotalsurcharge);
            adatotalsurcharge.Fill(dstotalsurcharge);

            foreach (DataRow drtotalsurcharget in dstotalsurcharge.Tables[0].Rows)
            {
                ViewBag.txtSurcharge = drtotalsurcharget[dstotalsurcharge.Tables[0].Columns[0].ColumnName].ToString();

                ViewBag.txtSurchargeCur = drtotalsurcharget[dstotalsurcharge.Tables[0].Columns[1].ColumnName].ToString();
            }

            // DISPLAY COUPON DETAILS/////////////////////


            SqlCommand cmdCpnDetails = new SqlCommand("[Pax].[SP_Coupon_Tax_BreakDown_Pivot]", cs);
            cmdCpnDetails.CommandType = CommandType.StoredProcedure;
            cmdCpnDetails.CommandTimeout = 1000;
            cmdCpnDetails.Parameters.AddWithValue("@HdrHuid", es);

            DataSet dsCpnDetails = new DataSet();
            SqlDataAdapter adaCpnDetails = new SqlDataAdapter(cmdCpnDetails);
            adaCpnDetails.Fill(dsCpnDetails);

            int ligneCpnDetails = dsCpnDetails.Tables[0].Rows.Count;
            string[,] AgCodeCpnDetails = new string[13, ligneCpnDetails];
            int c = 0;
            foreach (DataRow drCpnDetails in dsCpnDetails.Tables[0].Rows)
            {
                for (int p = 0; p < 13; p++)
                {
                    AgCodeCpnDetails[p, c] = drCpnDetails[dsCpnDetails.Tables[0].Columns[p].ColumnName].ToString();
                }
                c++;
            }

            ViewBag.AgCodeCpnDetails = AgCodeCpnDetails;
            ViewBag.ligneCpnDetails = ligneCpnDetails;


            ///////////////DISPLAY COUPON BREAKDOWN/////////////////

            string sqlCpnDie = " With a as  " + Environment.NewLine;
            sqlCpnDie = sqlCpnDie + "(select distinct f1.CouponNumber as CPN,f1.Carrier,CouponStatus ";
            sqlCpnDie = sqlCpnDie + ",case       ";
            sqlCpnDie = sqlCpnDie + "        when f2.SpecialProrateAgreement <> '0.00' then 'SPA' ";
            sqlCpnDie = sqlCpnDie + "        when f2.SpecialProrateAgreement = '0.00' then 'MPA' ";
            sqlCpnDie = sqlCpnDie + "        ELSE 'MPA' end as [PM] ";

            sqlCpnDie = sqlCpnDie + ",f2.finalshare as FWDSales ";
            sqlCpnDie = sqlCpnDie + ",f3.FinalShare as [Utilisation Gross AMT] ";
            sqlCpnDie = sqlCpnDie + ",cast( case when f1.carrier ='" + Ownairline()[0] + "' then OtherAmountRate else '0' end as decimal(18,2)) as [Comm%] ";
            sqlCpnDie = sqlCpnDie + ",cast(((f3.FinalShare*case when f1.carrier ='" + Ownairline()[0] + "' then OtherAmountRate else '0' end)/100)as decimal(18,2)) as [Comm AMT] ";

            sqlCpnDie = sqlCpnDie + ",iif(f3.SpecialProrateAgreement = '0.00' and f1.carrier <>'" + Ownairline()[0] + "','9.00','0.00') as [ISC%] ";
            sqlCpnDie = sqlCpnDie + ",cast((f3.FinalShare*iif(f3.SpecialProrateAgreement = '0.00' and f1.carrier <>'" + Ownairline()[0] + "','9.00','0.00')/100 )as decimal(18,2))as [ISC AMT] ";
            sqlCpnDie = sqlCpnDie + ", isnull((f3.FinalShare - f2.finalshare ),0) as DIE   ";
            sqlCpnDie = sqlCpnDie + ",f1.RelatedDocumentNumber ";
            sqlCpnDie = sqlCpnDie + "from Pax.SalesDocumentHeader sdh ";
            sqlCpnDie = sqlCpnDie + "join Pax.SalesRelatedDocumentInformation srd on sdh.HdrGuid = srd.HdrGuid ";
            sqlCpnDie = sqlCpnDie + "left join Pax.SalesDocumentOtherAmount sdo on srd.RelatedDocumentGuid =  sdo.RelatedDocumentGuid and DocumentAmountType like 'Com%' and OtherAmountCode = 'Effective' ";
            sqlCpnDie = sqlCpnDie + "left join Pax.SalesDocumentCoupon f1 on srd.RelatedDocumentGuid = f1.RelatedDocumentGuid ";
            sqlCpnDie = sqlCpnDie + "left join pax.ProrationDetail f2 on f1.RelatedDocumentGuid = f2.RelatedDocumentGuid and f1.CouponNumber = f2.CouponNumber ";
            sqlCpnDie = sqlCpnDie + "left join pax.ProrationDetail f3 on f1.RelatedDocumentGuid = f3.RelatedDocumentGuid and f1.CouponNumber = f3.CouponNumber and f1.CouponStatus = f3.ProrationFlag and f3.ProrationFlag in ('F','I') ";
            sqlCpnDie = sqlCpnDie + "where f1.RelatedDocumentNumber like '" + DocNum + "' ";
            sqlCpnDie = sqlCpnDie + ")  ";

            sqlCpnDie = sqlCpnDie + "select distinct a.CPN,a.PM,a.FWDSales,a.[Utilisation Gross AMT],a.[Comm%] as [Sales Comm %] ,a.[Comm AMT]as [Sales Comm AMT],a.[ISC%],a.[ISC AMT] ";
            sqlCpnDie = sqlCpnDie + ",(a.[Utilisation Gross AMT] - ([Comm AMT]+[ISC AMT])) as [Net Utilisation AMT],a.DIE, '' as Adjustment ";
            sqlCpnDie = sqlCpnDie + "from A ";

            SqlCommand cmdCpnDie = new SqlCommand(sqlCpnDie, cs);
            DataSet dsCpnDie = new DataSet();
            SqlDataAdapter adaCpnDie = new SqlDataAdapter(cmdCpnDie);
            adaCpnDie.Fill(dsCpnDie);
            int lonCpnDie = dsCpnDie.Tables[0].Rows.Count;
            string[,] AgCodeCpnDie = new string[11, lonCpnDie];
            int ia = 0;
            foreach (DataRow drCpnDie in dsCpnDie.Tables[0].Rows)
            {
                for (int ja = 0; ja < 11; ja++)
                {
                    AgCodeCpnDie[ja, ia] = drCpnDie[dsCpnDie.Tables[0].Columns[ja].ColumnName].ToString();
                }
                ia++;
            }

            ViewBag.AgCodeCpnDie = AgCodeCpnDie;
            ViewBag.lonCpnDie = lonCpnDie;

            ////////////////DISPLAY AGENT COMMISSION PERCENTAGE////////////////////w

            string sqlagentcomm = "select Rate from [Ref].[PaxAgencyDetails] where [AgencyNumericCode] ='" + txtCustomerCode + "' and [TypesofNumeration] = 'Sales Commission'";

            SqlCommand cmdagentcomm = new SqlCommand("" + sqlagentcomm + "", cs);

            DataSet dsagentcomm = new DataSet();
            SqlDataAdapter adaagentcomm = new SqlDataAdapter(cmdagentcomm);
            adaagentcomm.Fill(dsagentcomm);

            foreach (DataRow dragentcomm in dsagentcomm.Tables[0].Rows)
            {
                ViewBag.txtagtcom = dragentcomm[dsagentcomm.Tables[0].Columns[0].ColumnName].ToString();
            }

            /////////////////DISPLAY CAF AND ADM/ACM INFO////////////////////////////////////////  
            string sqlCAFADinf = " SELECT [MemoNumber] ,case when [ACMADM] = 'C' then 'Agency Credit Memo to be Issued' when [ACMADM] = 'D' then 'Agency Debit Memo  to be Issued' END as [ADM/ACM],[Currency],[TotalNuc]    " + Environment.NewLine;
            sqlCAFADinf += ",[FareDiff],[EFPDiff],[TFCDiff],[FareCurr] ,[EFPCurr] FROM [Pax].[ADMACM]  where documentNo = '" + DocNum + "'  " + Environment.NewLine;

            SqlCommand cmdCAFADinf = new SqlCommand(sqlCAFADinf, cs);
            DataSet dsCAFADinf = new DataSet();
            SqlDataAdapter adaCAFADinf = new SqlDataAdapter(cmdCAFADinf);
            adaCAFADinf.Fill(dsCAFADinf);

            foreach (DataRow drCAFADinf in dsCAFADinf.Tables[0].Rows)
            {
                ViewBag.txtadmNO = drCAFADinf[dsCAFADinf.Tables[0].Columns[0].ColumnName].ToString();
                //ViewBag.lblCDissue = drCAFADinf[dsCAFADinf.Tables[0].Columns[1].ColumnName].ToString();
                ViewBag.txtcafcur = drCAFADinf[dsCAFADinf.Tables[0].Columns[7].ColumnName].ToString();
                ViewBag.txtcafamt = drCAFADinf[dsCAFADinf.Tables[0].Columns[3].ColumnName].ToString();
                ViewBag.txtadmFare = drCAFADinf[dsCAFADinf.Tables[0].Columns[4].ColumnName].ToString();
                ViewBag.txtadmefp = drCAFADinf[dsCAFADinf.Tables[0].Columns[5].ColumnName].ToString();
                ViewBag.txtadmTFC = drCAFADinf[dsCAFADinf.Tables[0].Columns[6].ColumnName].ToString();
            }
            //// sml CHECK COUPONS DETAILS////////

            string ATDNo = Request["ATDNo"];
            string sqlCHECKCOUPONSDETAILS = "";
            sqlCHECKCOUPONSDETAILS = sqlCHECKCOUPONSDETAILS + " SELECT  [CouponNumber],[CouponStatus],[OriginAirportCityCode],[DestinationAirportCityCode],[Carrier],[FareBasisTicketDesignator],[FlightNumber],";
            sqlCHECKCOUPONSDETAILS = sqlCHECKCOUPONSDETAILS + "[FlightDepartureDate] ,[NotValidBefore] ,[NotValidAfter],[ReservationBookingDesignator],[UsageAirline],[UsageDate],[UsageFlightNumber],[IsOAL]" + Environment.NewLine;
            sqlCHECKCOUPONSDETAILS = sqlCHECKCOUPONSDETAILS + "FROM [Pax].[SalesDocumentCoupon]" + Environment.NewLine;
            sqlCHECKCOUPONSDETAILS = sqlCHECKCOUPONSDETAILS + "WHERE [DocumentNumber] = '" + DocNum + "'";
            SqlCommand cmdCHECKCOUPONSDETAILS = new SqlCommand(sqlCHECKCOUPONSDETAILS, cs);
            DataSet dsCHECKCOUPONSDETAILS = new DataSet();
            SqlDataAdapter adaCHECKCOUPONSDETAILS = new SqlDataAdapter(cmdCHECKCOUPONSDETAILS);
            adaCHECKCOUPONSDETAILS.Fill(dsCHECKCOUPONSDETAILS);
            int ligne = dsCHECKCOUPONSDETAILS.Tables[0].Rows.Count;
            string[,] data = new string[15, ligne];
            int k = 0;
            foreach (DataRow drc in dsCHECKCOUPONSDETAILS.Tables[0].Rows)
            {
                data[0, k] = drc[dsCHECKCOUPONSDETAILS.Tables[0].Columns[0].ColumnName].ToString();
                data[1, k] = drc[dsCHECKCOUPONSDETAILS.Tables[0].Columns[1].ColumnName].ToString();
                data[2, k] = drc[dsCHECKCOUPONSDETAILS.Tables[0].Columns[2].ColumnName].ToString();
                data[3, k] = drc[dsCHECKCOUPONSDETAILS.Tables[0].Columns[3].ColumnName].ToString();
                data[4, k] = drc[dsCHECKCOUPONSDETAILS.Tables[0].Columns[4].ColumnName].ToString();
                data[5, k] = drc[dsCHECKCOUPONSDETAILS.Tables[0].Columns[5].ColumnName].ToString();
                data[6, k] = drc[dsCHECKCOUPONSDETAILS.Tables[0].Columns[6].ColumnName].ToString();
                data[7, k] = drc[dsCHECKCOUPONSDETAILS.Tables[0].Columns[7].ColumnName].ToString();
                data[8, k] = drc[dsCHECKCOUPONSDETAILS.Tables[0].Columns[8].ColumnName].ToString();
                data[9, k] = drc[dsCHECKCOUPONSDETAILS.Tables[0].Columns[9].ColumnName].ToString();
                data[10, k] = drc[dsCHECKCOUPONSDETAILS.Tables[0].Columns[10].ColumnName].ToString();
                data[11, k] = drc[dsCHECKCOUPONSDETAILS.Tables[0].Columns[11].ColumnName].ToString();
                data[12, k] = drc[dsCHECKCOUPONSDETAILS.Tables[0].Columns[12].ColumnName].ToString();
                data[13, k] = drc[dsCHECKCOUPONSDETAILS.Tables[0].Columns[13].ColumnName].ToString();
                data[14, k] = drc[dsCHECKCOUPONSDETAILS.Tables[0].Columns[14].ColumnName].ToString();
                k++;
            }
            ViewBag.ATDNocpn = ATDNo;
            ViewBag.loncouponsdetails = ligne;
            ViewBag.datacouponsdetails = data;
            ////sml FrmprorationDetails //////
            string sqlprodetails = "select f4.[CouponNumber],[ProrationFlag],[SectorOrigin],[SectorDestination],[SectorCarrier],[ProrateFactor],[ProrateValue],[Currency],[SpecialProrateAgreement],[Surcharge],[Diffentials],[FinalShare]  " + Environment.NewLine;
            sqlprodetails = sqlprodetails + " from Pax.SalesDocumentHeader sdh  " + Environment.NewLine;
            sqlprodetails = sqlprodetails + " join Pax.SalesRelatedDocumentInformation srd on sdh.HdrGuid = srd.HdrGuid    " + Environment.NewLine;
            sqlprodetails = sqlprodetails + " left join Pax.SalesDocumentCoupon sdc on srd.RelatedDocumentGuid = sdc.RelatedDocumentGuid  " + Environment.NewLine;
            sqlprodetails = sqlprodetails + " left join pax.ProrationDetail f4 on f4.RelatedDocumentGuid = sdc.RelatedDocumentGuid and sdc.CouponNumber = f4.CouponNumber and sdc.CouponStatus = f4.ProrationFlag where sdh.DocumentNumber ='" + DocNum + "' " + Environment.NewLine;
            SqlCommand cmdprodetails = new SqlCommand(sqlprodetails, cs);
            DataSet dsprodetails = new DataSet();
            SqlDataAdapter adaprodetails = new SqlDataAdapter(cmdprodetails);
            adaprodetails.Fill(dsprodetails);
            int ligne1 = dsprodetails.Tables[0].Rows.Count;
            string[,] data1 = new string[12, ligne1];
            int k1 = 0;
            foreach (DataRow drp in dsprodetails.Tables[0].Rows)
            {
                data1[0, k1] = drp[dsprodetails.Tables[0].Columns[0].ColumnName].ToString();
                data1[1, k1] = drp[dsprodetails.Tables[0].Columns[1].ColumnName].ToString();
                data1[2, k1] = drp[dsprodetails.Tables[0].Columns[2].ColumnName].ToString();
                data1[3, k1] = drp[dsprodetails.Tables[0].Columns[3].ColumnName].ToString();
                data1[4, k1] = drp[dsprodetails.Tables[0].Columns[4].ColumnName].ToString();
                data1[5, k1] = drp[dsprodetails.Tables[0].Columns[5].ColumnName].ToString();
                data1[6, k1] = drp[dsprodetails.Tables[0].Columns[6].ColumnName].ToString();
                data1[7, k1] = drp[dsprodetails.Tables[0].Columns[7].ColumnName].ToString();
                data1[8, k1] = drp[dsprodetails.Tables[0].Columns[8].ColumnName].ToString();
                data1[9, k1] = drp[dsprodetails.Tables[0].Columns[9].ColumnName].ToString();
                data1[10, k1] = drp[dsprodetails.Tables[0].Columns[10].ColumnName].ToString();
                data1[11, k1] = drp[dsprodetails.Tables[0].Columns[11].ColumnName].ToString();
                k1++;
            }
            ViewBag.lonprodetails = ligne1;
            ViewBag.dataprodetails = data1;

            /// sml views Agency Details///// 

            string sqlviewsAgencyDetails = "SELECT *  from [Ref].[PassengerAgencyDetails]  where AgencyNumericCode = '" + ag + "' ";
            SqlCommand cmdviewsAgencyDetails = new SqlCommand("" + sqlviewsAgencyDetails + "", cs);
            DataSet dsviewsAgencyDetails = new DataSet();
            SqlDataAdapter adaviewsAgencyDetails = new SqlDataAdapter(cmdviewsAgencyDetails);
            adaviewsAgencyDetails.Fill(dsviewsAgencyDetails);
            foreach (DataRow drviewsAgencyDetails in dsviewsAgencyDetails.Tables[0].Rows)
            {
                ViewBag.txtNamevad = drviewsAgencyDetails[dsviewsAgencyDetails.Tables[0].Columns[0].ColumnName].ToString();
                ViewBag.txtadressvad = drviewsAgencyDetails[dsviewsAgencyDetails.Tables[0].Columns[1].ColumnName].ToString();
                ViewBag.txtstatusvad = drviewsAgencyDetails[dsviewsAgencyDetails.Tables[0].Columns[2].ColumnName].ToString();
                ViewBag.txtcategoryvad = drviewsAgencyDetails[dsviewsAgencyDetails.Tables[0].Columns[3].ColumnName].ToString();
                ViewBag.txtremarksvad = drviewsAgencyDetails[dsviewsAgencyDetails.Tables[0].Columns[4].ColumnName].ToString();
                ViewBag.txtdatevad = drviewsAgencyDetails[dsviewsAgencyDetails.Tables[0].Columns[5].ColumnName].ToString();
            }

            string Sqlagent = "SELECT *  from [Ref].[PaxAgencyDetails]  where AgencyNumericCode = '" + ag + "' ";
            SqlCommand cmdagent = new SqlCommand("" + Sqlagent + "", cs);
            DataSet dsagent = new DataSet();
            SqlDataAdapter adaagent = new SqlDataAdapter(cmdagent);
            adaagent.Fill(dsagent);
            int ligneag = dsagent.Tables[0].Rows.Count;
            string[,] dataag = new string[6, ligneag];
            int kag = 0;
            foreach (DataRow dragent in dsagent.Tables[0].Rows)
            {
                dataag[0, kag] = dragent[dsagent.Tables[0].Columns[0].ColumnName].ToString();
                if (dragent[dsagent.Tables[0].Columns[1].ColumnName].ToString() == "0")
                {
                    dataag[1, kag] = "Amount";
                }
                else
                {
                    dataag[1, kag] = "Percentage";
                }
                if (dragent[dsagent.Tables[0].Columns[1].ColumnName].ToString() == "0")
                {
                    dataag[1, kag] = dragent[dsagent.Tables[0].Columns[2].ColumnName].ToString();
                }
                else
                {
                    dataag[2, kag] = dragent[dsagent.Tables[0].Columns[1].ColumnName].ToString();
                }
                dataag[3, kag] = dragent[dsagent.Tables[0].Columns[3].ColumnName].ToString();
                dataag[4, kag] = dragent[dsagent.Tables[0].Columns[4].ColumnName].ToString();
                dataag[5, kag] = dragent[dsagent.Tables[0].Columns[5].ColumnName].ToString();
                kag++;
            }
            ViewBag.lonligneag = ligneag;
            ViewBag.datadataag = dataag;

            /// sml Frm ticket discrepency coupon validity checks////
            string sqlcouponvalid = " with a as  ( " + Environment.NewLine;
            sqlcouponvalid = sqlcouponvalid + "select  sdh.DocumentNumber,f1.CouponNumber,f1.ReservationBookingDesignator,f1.FareBasisTicketDesignator,f1.NotValidBefore,f1.NotValidAfter,f3.MaximumValidity as ValidityPeriod,f1.FlightDepartureDate  ";
            sqlcouponvalid = sqlcouponvalid + ",iif( f2.LiftHeaderGuid is null and left(f1.RelatedDocumentNumber,3) ='" + Ownairline()[1] + "' and CouponStatus <> 'F' and f1.FlightDepartureDate < cast(getdate()-7 as date)   ";
            sqlcouponvalid = sqlcouponvalid + " and ( f1.NotValidAfter is not null and f1.NotValidAfter < cast(getdate()-7 as date)  ) or   ";
            sqlcouponvalid = sqlcouponvalid + "( f1.NotValidAfter is null and f3.MaximumValidity is not null and f1.FlightDepartureDate >   ";
            sqlcouponvalid = sqlcouponvalid + "case  when right(f3.MaximumValidity,1) = 'Y' then dateadd(YEAR, cast(SUBSTRING(f3.MaximumValidity,1,len(f3.MaximumValidity)-1) as int),sdh.DateofIssue)   ";
            sqlcouponvalid = sqlcouponvalid + "      when right(f3.MaximumValidity,1) = 'M' then dateadd(MONTH, cast(SUBSTRING(f3.MaximumValidity,1,len(f3.MaximumValidity)-1) as int),sdh.DateofIssue)   ";
            sqlcouponvalid = sqlcouponvalid + "      when right(f3.MaximumValidity,1) = 'D' then dateadd(DAY, cast(SUBSTRING(f3.MaximumValidity,1,len(f3.MaximumValidity)-1) as int),sdh.DateofIssue)  end)  ";
            sqlcouponvalid = sqlcouponvalid + "or ( f1.NotValidAfter is null and f3.MaximumValidity is null and f1.FlightDepartureDate > DATEADD(YEAR,1,sdh.DateofIssue) )  , 'Y', '' ) as [Is Expired?]  ";
            sqlcouponvalid = sqlcouponvalid + "from pax.SalesDocumentHeader sdh   ";
            sqlcouponvalid = sqlcouponvalid + "join pax.SalesRelatedDocumentInformation srd on sdh.HdrGuid =srd.HdrGuid  ";
            sqlcouponvalid = sqlcouponvalid + "join pax.SalesDocumentCoupon  f1 on srd.RelatedDocumentGuid = f1.RelatedDocumentGuid  ";
            sqlcouponvalid = sqlcouponvalid + "left join FileLift.LiftHeader f2 on f1.RelatedDocumentNumber = f2.AirlineCode+f2.TicketNumber and f1.CouponNumber = f2.CouponNumber   ";
            sqlcouponvalid = sqlcouponvalid + "left join pax.Farebasis f3 on f3.FareBasisTicketDesignator = f1.FareBasisTicketDesignator)  ";
            sqlcouponvalid = sqlcouponvalid + "select * from A where DocumentNumber like '" + DocNum + "'  ";
            sqlcouponvalid = sqlcouponvalid + "order by CouponNumber,FlightDepartureDate desc ";
            SqlCommand cmdcouponvalid = new SqlCommand(sqlcouponvalid, cs);
            DataSet dscouponvalid = new DataSet();
            SqlDataAdapter adacouponvalid = new SqlDataAdapter(cmdcouponvalid);
            adacouponvalid.Fill(dscouponvalid);
            int lignecp = dscouponvalid.Tables[0].Rows.Count;
            string[,] datacp = new string[9, lignecp];
            int kcp = 0;
            foreach (DataRow dcp in dscouponvalid.Tables[0].Rows)
            {

                datacp[1, kcp] = dcp[dscouponvalid.Tables[0].Columns[1].ColumnName].ToString();
                datacp[2, kcp] = dcp[dscouponvalid.Tables[0].Columns[2].ColumnName].ToString();
                datacp[3, kcp] = dcp[dscouponvalid.Tables[0].Columns[3].ColumnName].ToString();
                datacp[4, kcp] = dcp[dscouponvalid.Tables[0].Columns[4].ColumnName].ToString();
                datacp[5, kcp] = dcp[dscouponvalid.Tables[0].Columns[5].ColumnName].ToString();
                datacp[6, kcp] = dcp[dscouponvalid.Tables[0].Columns[6].ColumnName].ToString();
                datacp[7, kcp] = dcp[dscouponvalid.Tables[0].Columns[7].ColumnName].ToString();
                datacp[8, kcp] = dcp[dscouponvalid.Tables[0].Columns[8].ColumnName].ToString();
                /* if (dcp[dscouponvalid.Tables[0].Columns[8].ColumnName].ToString() == "N")
                 {
                     datacp[5, kcp] = "Y";
                 }
                 else
                 {
                     datacp[5, kcp] = "N";
                 }*/
                kcp++;
                ViewBag.londocnumber = dcp[dscouponvalid.Tables[0].Columns[0].ColumnName].ToString();
            }

            ViewBag.loncouponvalid = lignecp;
            ViewBag.datacouponvalid = datacp;
            /// sml Frm ticket discrepency check RBD ////
            string sqlcheckRBD = " select sdc.CouponNumber,FlightNumber,FlightDepartureDate  " + Environment.NewLine;
            sqlcheckRBD = sqlcheckRBD + " ,sdc.FareBasisTicketDesignator  " + Environment.NewLine;
            sqlcheckRBD = sqlcheckRBD + " ,sdc.ReservationBookingDesignator as ReservedRBD  " + Environment.NewLine;
            sqlcheckRBD = sqlcheckRBD + " ,sdc.UsedClassofService as USAGERBD  " + Environment.NewLine;
            sqlcheckRBD = sqlcheckRBD + " ,iif(sdc.ReservationBookingDesignator<>sdc.UsedClassofService,'RBD Modified','') as Remarks  " + Environment.NewLine;
            sqlcheckRBD = sqlcheckRBD + " from   " + Environment.NewLine;
            sqlcheckRBD = sqlcheckRBD + " Pax.SalesDocumentHeader sdh  " + Environment.NewLine;
            sqlcheckRBD = sqlcheckRBD + " join Pax.SalesRelatedDocumentInformation srd on sdh.HdrGuid = srd.HdrGuid  " + Environment.NewLine;
            sqlcheckRBD = sqlcheckRBD + " left join Pax.SalesDocumentCoupon sdc on srd.RelatedDocumentGuid = sdc.RelatedDocumentGuid  " + Environment.NewLine;
            sqlcheckRBD = sqlcheckRBD + " where CouponStatus = 'F' and sdh.AgentNumericCode like '" + ag + "' and sdh.DocumentNumber = '" + DocNum + "' " + Environment.NewLine;
            SqlCommand cmdcheckRBD = new SqlCommand("" + sqlcheckRBD + "", cs);
            DataSet dscheckRBD = new DataSet();
            SqlDataAdapter adacheckRBD = new SqlDataAdapter(cmdcheckRBD);
            adacheckRBD.Fill(dscheckRBD);
            int ligneRBD = dscheckRBD.Tables[0].Rows.Count;
            string[,] dataRBD = new string[7, ligneRBD];
            int krbd = 0;
            foreach (DataRow drRBD in dscheckRBD.Tables[0].Rows)
            {
                dataRBD[0, krbd] = drRBD[dscheckRBD.Tables[0].Columns[0].ColumnName].ToString();
                dataRBD[1, krbd] = drRBD[dscheckRBD.Tables[0].Columns[1].ColumnName].ToString();
                dataRBD[2, krbd] = drRBD[dscheckRBD.Tables[0].Columns[2].ColumnName].ToString();
                dataRBD[3, krbd] = drRBD[dscheckRBD.Tables[0].Columns[3].ColumnName].ToString();
                dataRBD[4, krbd] = drRBD[dscheckRBD.Tables[0].Columns[4].ColumnName].ToString();
                dataRBD[5, krbd] = drRBD[dscheckRBD.Tables[0].Columns[5].ColumnName].ToString();
                dataRBD[6, krbd] = drRBD[dscheckRBD.Tables[0].Columns[6].ColumnName].ToString();
                krbd++;
            }
            ViewBag.loncheckRBD = ligneRBD;
            ViewBag.datacheckRBD = dataRBD;

            /// sml Frm ticket discrepency check Date ////
            string sqlcheckDate = "  select sdc.CouponNumber,FlightNumber,sdc.FareBasisTicketDesignator ,sdc.ReservationBookingDesignator " + Environment.NewLine;
            sqlcheckDate = sqlcheckDate + " ,sdc.FlightDepartureDate as [Planned Flight Date]  " + Environment.NewLine;
            sqlcheckDate = sqlcheckDate + " ,sdc.UsageDate as [Usage Flight Date]  " + Environment.NewLine;
            sqlcheckDate = sqlcheckDate + " ,iif(sdc.FlightDepartureDate<>sdc.UsageDate,'Flight Date Modified','') as Remarks  " + Environment.NewLine;
            sqlcheckDate = sqlcheckDate + " from   " + Environment.NewLine;
            sqlcheckDate = sqlcheckDate + " Pax.SalesDocumentHeader sdh  " + Environment.NewLine;
            sqlcheckDate = sqlcheckDate + " join Pax.SalesRelatedDocumentInformation srd on sdh.HdrGuid = srd.HdrGuid  " + Environment.NewLine;
            sqlcheckDate = sqlcheckDate + " left join Pax.SalesDocumentCoupon sdc on srd.RelatedDocumentGuid = sdc.RelatedDocumentGuid  " + Environment.NewLine;
            sqlcheckDate = sqlcheckDate + " where CouponStatus = 'F' and sdh.AgentNumericCode like '" + ag + "' and sdh.DocumentNumber = '" + DocNum + "' " + Environment.NewLine;
            SqlCommand cmdcheckDate = new SqlCommand("" + sqlcheckDate + "", cs);
            DataSet dscheckDate = new DataSet();
            SqlDataAdapter adacheckDate = new SqlDataAdapter(cmdcheckDate);
            adacheckDate.Fill(dscheckDate);
            int lignedscheckDate = dscheckDate.Tables[0].Rows.Count;
            string[,] datacheckDate = new string[7, lignedscheckDate];
            int kcd = 0;
            foreach (DataRow drcd in dscheckDate.Tables[0].Rows)
            {
                datacheckDate[0, kcd] = drcd[dscheckDate.Tables[0].Columns[0].ColumnName].ToString();
                datacheckDate[1, kcd] = drcd[dscheckDate.Tables[0].Columns[1].ColumnName].ToString();
                datacheckDate[2, kcd] = drcd[dscheckDate.Tables[0].Columns[2].ColumnName].ToString();
                datacheckDate[3, kcd] = drcd[dscheckDate.Tables[0].Columns[3].ColumnName].ToString();
                datacheckDate[4, kcd] = drcd[dscheckDate.Tables[0].Columns[4].ColumnName].ToString();
                datacheckDate[5, kcd] = drcd[dscheckDate.Tables[0].Columns[5].ColumnName].ToString();
                datacheckDate[6, kcd] = drcd[dscheckDate.Tables[0].Columns[6].ColumnName].ToString();
                krbd++;
            }
            ViewBag.loncheckDate = lignedscheckDate;
            ViewBag.datacheckDate = datacheckDate;

            /// sml Frm ticket discrepency check Commission ////
            string sqlcheckCommission = " select sdh.DateofIssue,sdh.AgentNumericCode,sdh.DocumentNumber,DocumentAmountType,OtherAmountCode,OtherAmountRate,OtherAmount,agt.Rate, ( agt.Rate-OtherAmountRate) as Diff  " + Environment.NewLine;
            sqlcheckCommission += " ,case when ( agt.Rate-OtherAmountRate) is null then 'Agent Info Missing'  " + Environment.NewLine;
            sqlcheckCommission += "when ( agt.Rate-OtherAmountRate) < 0 then 'Agent Exceeding Commission % Allocated'end as remarks ,sdh.TransactionGroup  " + Environment.NewLine;
            sqlcheckCommission += "from Pax.SalesDocumentHeader sdh " + Environment.NewLine;
            sqlcheckCommission += "join Pax.SalesRelatedDocumentInformation srd on sdh.HdrGuid = srd.HdrGuid    " + Environment.NewLine;
            sqlcheckCommission += "left join Pax.SalesDocumentOtherAmount sdo on srd.RelatedDocumentGuid =  sdo.RelatedDocumentGuid and DocumentAmountType like 'Com%' and OtherAmountCode = 'Effective'    " + Environment.NewLine;
            sqlcheckCommission += "left join [Ref].[PaxAgencyDetails] agt on agt.AgencyNumericCode = left(sdh.AgentNumericCode,7)   " + Environment.NewLine;
            sqlcheckCommission += "where OtherAmountCode  = 'Effective' and DocumentAmountType like 'Com%'   " + Environment.NewLine;
            sqlcheckCommission += "and agt.AgencyNumericCode   = left('" + ag + "',7)  and sdh.DocumentNumber = '" + DocNum + "'   " + Environment.NewLine;
            sqlcheckCommission += "order by sdh.DateofIssue,sdh.DocumentNumber   " + Environment.NewLine;
            SqlCommand cmdcheckCommission = new SqlCommand(sqlcheckCommission, cs);
            DataSet dscheckCommission = new DataSet();
            SqlDataAdapter adacheckCommission = new SqlDataAdapter(cmdcheckCommission);
            adacheckCommission.Fill(dscheckCommission);
            int lignecc = dscheckCommission.Tables[0].Rows.Count;
            string[,] datacc = new string[11, lignecc];
            int kcc = 0;
            foreach (DataRow drc in dscheckCommission.Tables[0].Rows)
            {
                datacc[0, kcc] = drc[dscheckCommission.Tables[0].Columns[0].ColumnName].ToString();
                datacc[1, kcc] = drc[dscheckCommission.Tables[0].Columns[1].ColumnName].ToString();
                datacc[2, kcc] = drc[dscheckCommission.Tables[0].Columns[2].ColumnName].ToString();
                datacc[3, kcc] = drc[dscheckCommission.Tables[0].Columns[3].ColumnName].ToString();
                datacc[4, kcc] = drc[dscheckCommission.Tables[0].Columns[4].ColumnName].ToString();
                datacc[5, kcc] = drc[dscheckCommission.Tables[0].Columns[5].ColumnName].ToString();
                datacc[6, kcc] = drc[dscheckCommission.Tables[0].Columns[6].ColumnName].ToString();
                datacc[7, kcc] = drc[dscheckCommission.Tables[0].Columns[7].ColumnName].ToString();
                datacc[8, kcc] = drc[dscheckCommission.Tables[0].Columns[8].ColumnName].ToString();
                datacc[9, kcc] = drc[dscheckCommission.Tables[0].Columns[9].ColumnName].ToString();
                datacc[10, kcc] = drc[dscheckCommission.Tables[0].Columns[10].ColumnName].ToString();
                k1++;
            }
            ViewBag.loncheckCommission = lignecc;
            ViewBag.datacheckCommission = datacc;
            return PartialView();

        }
        private string[] Ownairline()
        {

            SqlConnection cs = new SqlConnection(pbConnectionString);

            if (cs.State == ConnectionState.Open)
            {
                cs.Close();
            }


            string sqlOwnairline = "select String1,string4 from ADM.GSP where Parameter = 'SYS0001'";

            SqlCommand cmdOwnairline = new SqlCommand("" + sqlOwnairline + "", cs);

            DataSet dsOwnairline = new DataSet();
            SqlDataAdapter adaOwnairline = new SqlDataAdapter(cmdOwnairline);
            adaOwnairline.Fill(dsOwnairline);
            string[] own = new string[2];
            foreach (DataRow drOwnairline in dsOwnairline.Tables[0].Rows)
            {
                own[0] = drOwnairline[dsOwnairline.Tables[0].Columns[0].ColumnName].ToString();

                own[1] = drOwnairline[dsOwnairline.Tables[0].Columns[1].ColumnName].ToString();
            }
            return own;

        }



        /****** ATDs With Endorsements/Restrictions    Joseph *********/
        public ActionResult EndorsementsRestrictions()
        {

            string dateFromER = Request["dateFrom"];
            string dateToER = Request["dateTo"];

            SqlConnection con2 = new SqlConnection(pbConnectionString);
            DataSet ds = new DataSet();

            SqlCommand cmd = new SqlCommand("[Pax].[ATDsWithEndorsementsRestrictions]", con2);
            cmd.CommandTimeout = 1000;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@FromDate", dateFromER);
            cmd.Parameters.AddWithValue("@ToDate", dateToER);


            int i = 0;
            SqlDataAdapter ada = new SqlDataAdapter(cmd);
            ada.Fill(ds);

            int lon = ds.Tables[0].Rows.Count;

            string[,] resultatEr = new string[10, lon];

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                resultatEr[0, i] = dr[ds.Tables[0].Columns[0].ColumnName].ToString();
                resultatEr[1, i] = dr[ds.Tables[0].Columns[1].ColumnName].ToString();
                resultatEr[2, i] = dr[ds.Tables[0].Columns[2].ColumnName].ToString();
                resultatEr[3, i] = dr[ds.Tables[0].Columns[3].ColumnName].ToString();
                resultatEr[4, i] = dr[ds.Tables[0].Columns[4].ColumnName].ToString();
                resultatEr[5, i] = dr[ds.Tables[0].Columns[5].ColumnName].ToString();
                resultatEr[6, i] = dr[ds.Tables[0].Columns[6].ColumnName].ToString();
                resultatEr[7, i] = dr[ds.Tables[0].Columns[7].ColumnName].ToString();
                resultatEr[8, i] = dr[ds.Tables[0].Columns[8].ColumnName].ToString();
                resultatEr[9, i] = dr[ds.Tables[0].Columns[9].ColumnName].ToString();

                i++;
            }

            ViewBag.resultatEr = resultatEr;
            ViewBag.nombreEr = lon;


            ViewBag.dateFrom = dateFromER;
            ViewBag.dateTo = dateToER;

            return PartialView();
        }

        /******** End ATDs With Endorsements/Restrictions  ***********/




        /********  ATDs With Exchanged Docs   Joseph **********/
        public ActionResult ATDExchangedDocs()
        {
            string dateFromED = Request["dateFrom"];
            string dateToED = Request["dateTo"];

            SqlConnection con2 = new SqlConnection(pbConnectionString);
            DataSet ds = new DataSet();

            SqlCommand cmd = new SqlCommand("[Pax].[ATDsWithExchangedDocs]", con2);
            cmd.CommandTimeout = 1000;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@FromDate", dateFromED);
            cmd.Parameters.AddWithValue("@ToDate", dateToED);


            int i = 0;
            SqlDataAdapter ada = new SqlDataAdapter(cmd);
            ada.Fill(ds);

            int lon = ds.Tables[0].Rows.Count;

            string[,] resultat = new string[7, lon];

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                resultat[0, i] = dr[ds.Tables[0].Columns[0].ColumnName].ToString();
                resultat[1, i] = dr[ds.Tables[0].Columns[1].ColumnName].ToString();
                resultat[2, i] = dr[ds.Tables[0].Columns[2].ColumnName].ToString();
                resultat[3, i] = dr[ds.Tables[0].Columns[3].ColumnName].ToString();
                resultat[4, i] = dr[ds.Tables[0].Columns[4].ColumnName].ToString();
                resultat[5, i] = dr[ds.Tables[0].Columns[5].ColumnName].ToString();
                resultat[6, i] = dr[ds.Tables[0].Columns[6].ColumnName].ToString();

                i++;
            }

            ViewBag.resultat = resultat;

            ViewBag.nombre = lon;

            ViewBag.dateFromED = dateFromED;
            ViewBag.dateToED = dateToED;

            return PartialView();
        }

        /*** End ATDs With Exchanged Docs  ***/



        /**********  ATDs With Conjuction Tickets    Joseph **************/

        public ActionResult ATDConjuctionTickets()
        {
            string dateFromCT = Request["dateFrom"];
            string dateToCT = Request["dateTo"];

            SqlConnection con3 = new SqlConnection(pbConnectionString);
            DataSet ds = new DataSet();

            
            SqlCommand cmd = new SqlCommand("[Pax].[ATDsWithConjuctionTickets]", con3);

            cmd.CommandTimeout = 1000;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@FromDate", dateFromCT);
            cmd.Parameters.AddWithValue("@ToDate", dateToCT);


            int i = 0;
            SqlDataAdapter ada = new SqlDataAdapter(cmd);
            ada.Fill(ds);

            int lon = ds.Tables[0].Rows.Count;

            string[,] resultatCT = new string[9, lon];

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                resultatCT[0, i] = dr[ds.Tables[0].Columns[0].ColumnName].ToString();
                resultatCT[1, i] = dr[ds.Tables[0].Columns[1].ColumnName].ToString();
                resultatCT[2, i] = dr[ds.Tables[0].Columns[2].ColumnName].ToString();
                resultatCT[3, i] = dr[ds.Tables[0].Columns[3].ColumnName].ToString();
                resultatCT[4, i] = dr[ds.Tables[0].Columns[4].ColumnName].ToString();
                resultatCT[5, i] = dr[ds.Tables[0].Columns[5].ColumnName].ToString();
                resultatCT[6, i] = dr[ds.Tables[0].Columns[6].ColumnName].ToString();
                resultatCT[7, i] = dr[ds.Tables[0].Columns[7].ColumnName].ToString();
                resultatCT[8, i] = dr[ds.Tables[0].Columns[8].ColumnName].ToString();

                i++;
            }
            ViewBag.resultatCT = resultatCT;

            ViewBag.nombreCT = lon;
         
           ViewBag.dateFromCT = dateFromCT;
           ViewBag.dateToCT = dateToCT;



           return PartialView();
       }


        /*
        public ActionResult DiscountUncollected()
        {
            return PartialView();
        }
        public ActionResult RoutingChange()
        {
            return PartialView();
        }
        */

        // Harentsoa
        public ActionResult ExcessBilling()
        {
            GetFromAirLineCode();
            GetAllAirLineCode();
            cmbReasonCode_DropDown();
            GetBillingPeriodQuery__Dropdown();   

            return PartialView();
        }

        // Harentsoa
        // Get AirLines Code FROM
        public void GetFromAirLineCode()
        {
            string sql = "SELECT [AirlineCode], [AirlineID] FROM [Ref].[Airlines] WHERE [AirlineID] = '610'";

            SqlConnection con = new SqlConnection(pbConnectionString);
            DataSet ds = new DataSet();
            SqlDataAdapter ada = new SqlDataAdapter(sql, con);

            ada.Fill(ds);
            con.Close();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                ViewBag.codeFromCode = dr[ds.Tables[0].Columns[0].ColumnName].ToString();
                ViewBag.codeFromId = dr[ds.Tables[0].Columns[1].ColumnName].ToString();
            }
        }


        // Harentsoa
        // Get AirLines Code To
        public void GetAllAirLineCode()
        {
            string sql = "SELECT [AirLineCode], [AirlineID] FROM [Ref].[Airlines]";

            SqlConnection con = new SqlConnection(pbConnectionString);
            DataSet ds = new DataSet();
            SqlDataAdapter ada = new SqlDataAdapter(sql, con);

            ada.Fill(ds);
            con.Close();


            int lon = ds.Tables[0].Rows.Count;
            string[,] tab = new string[2, lon];
            string[,] tab2 = new string[2, lon];

            int i = 0;
            foreach (DataRow dr in ds.Tables[0].Rows)
            {

                tab[0, i] = dr[ds.Tables[0].Columns[0].ColumnName].ToString();
                tab2[0, i] = dr[ds.Tables[0].Columns[1].ColumnName].ToString();

                i++;
            }

            ViewBag.lon = lon;
            ViewBag.tab = tab;
            ViewBag.tabId = tab2;

        }

        // Harentsoa
        // Get Reason Code Dropdown
        public void cmbReasonCode_DropDown()
        {
            string sql = "SELECT [ReasonCode] FROM [Ref].[ReasonCodes]";

            SqlConnection con = new SqlConnection(pbConnectionString);
            DataSet ds = new DataSet();
            SqlDataAdapter ada = new SqlDataAdapter(sql, con);

            ada.Fill(ds);
            con.Close();


            int lon = ds.Tables[0].Rows.Count;
            string[,] tab = new string[1, lon];

            int i = 0;

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                tab[0, i] = dr[ds.Tables[0].Columns[0].ColumnName].ToString();
                i++;
            }

            ViewBag.lon = lon;
            ViewBag.reasonCode = tab;
        }

        // Harentsoa
        // Get Billing Period Query on the dropdown
        public void GetBillingPeriodQuery__Dropdown()
        {
            string sql = "SELECT Distinct [BillingPeriod] FROM [XmlFile].[BMHEADER]";

            SqlConnection con = new SqlConnection(pbConnectionString);
            DataSet ds = new DataSet();
            SqlDataAdapter ada = new SqlDataAdapter(sql, con);

            ada.Fill(ds);
            con.Close();

            int lon = ds.Tables[0].Rows.Count;
            string[,] tab = new string[1, lon];

            int i = 0;

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                tab[0, i] = dr[ds.Tables[0].Columns[0].ColumnName].ToString();
                i++;
            }

            ViewBag.lonBpQ = lon;
            ViewBag.billingPeriodQuery = tab;

            GetFromAirLineCode();
            GetAllAirLineCode();
            cmbReasonCode_DropDown();

            GetSectorCode();
            Taxlog();
        }

        // Harentsoa
        // Get Billing Period Records Dropdown on Change to assign valut to Billing Memo Number
        public ActionResult getBillingPeriodRecordsQuery()
        {

            GetBillingPeriodQuery__Dropdown();

            string cboBMBillingPeriod = Request["billingPeriodQuery"];

            string Sql = "SELECT Distinct  [CreditMemoNumber] as [CreditMemoNumber] FROM [XmlFile].[BMHEADER] where [BillingMonth]+[BillingPeriod]='" + cboBMBillingPeriod + "'";
            //,[CreditMemoNumber]

            SqlConnection con = new SqlConnection(pbConnectionString);
            DataSet ds = new DataSet();
            SqlDataAdapter ada = new SqlDataAdapter(Sql, con);

            ada.Fill(ds);
            con.Close();

            int lon = ds.Tables[0].Rows.Count;
            string[,] tabtest = new string[1, lon];

            int i = 0;

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                tabtest[0, i] = dr[ds.Tables[0].Columns[0].ColumnName].ToString();
                i++;
            }
            ViewBag.lonBpQBM = lon;
            ViewBag.tabtest = tabtest;

            GetFromAirLineCode();
            GetAllAirLineCode();
            cmbReasonCode_DropDown();

            return PartialView("ExcessBilling");
        }


        //Harentsoa
        // Get Billing Memo Query Dropdown on Change to assign valut to Invoice Number
        public ActionResult getInvoiceNumberQuery()
        {
            getBillingPeriodRecordsQuery();

            string txtx = Request["billingPeriodQuery"];
            string txtx2 = Request["billingMemoNumberQuery"];

            string Sql = "SELECT Distinct  [InvoiceNumber] as [InvoiceNumber] FROM [XmlFile].[BMHEADER] where [BillingMonth]+[BillingPeriod]='" + txtx + "'";

            Sql = Sql + "and CreditMemoNumber='" + txtx2 + "'";

            SqlConnection con = new SqlConnection(pbConnectionString);
            DataSet ds = new DataSet();
            SqlDataAdapter ada = new SqlDataAdapter(Sql, con);

            ada.Fill(ds);
            con.Close();

            int lon = ds.Tables[0].Rows.Count;
            string[,] tab = new string[1, lon];

            int i = 0;

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                tab[0, i] = dr[ds.Tables[0].Columns[0].ColumnName].ToString();
            }

            ViewBag.lonCMNQ = lon;
            ViewBag.InvoiceMemoNumber = tab;

            GetFromAirLineCode();
            GetAllAirLineCode();
            cmbReasonCode_DropDown();

            return PartialView("ExcessBilling");
        }

        //Harentsoa
        // Get Source Code from Tax Breakdown
        private void GetSectorCode()
        {
            
            string Sql = "SELECT   [FromAirport]+ '-' + [ToAirport]  AS Sector  FROM [Ref].[Tax]  Group by FromAirport,ToAirport Order  by FromAirport,ToAirport;";

            SqlConnection con = new SqlConnection(pbConnectionString);
            DataSet ds = new DataSet();
            SqlDataAdapter ada = new SqlDataAdapter(Sql, con);

            ada.Fill(ds);
            con.Close();


            int lon = ds.Tables[0].Rows.Count;
            string[,] tab = new string[1, lon];

            int i = 0;

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                tab[0, i] = dr[ds.Tables[0].Columns[0].ColumnName].ToString();
                i++;
            }

            ViewBag.lonSectorCode = lon;
            ViewBag.sectorCode = tab;
        }

        // Harentsoa
        // Get tax log from Tax Breakdown
        private void Taxlog()
        {
            string Sql = "SELECT DISTINCT TAXLOG From Ref.taxlog";

            SqlConnection con = new SqlConnection(pbConnectionString);
            DataSet ds = new DataSet();
            SqlDataAdapter ada = new SqlDataAdapter(Sql, con);

            ada.Fill(ds);
            con.Close();


            int lon = ds.Tables[0].Rows.Count;
            string[,] tab = new string[1, lon];

            int i = 0;

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                tab[0, i] = dr[ds.Tables[0].Columns[0].ColumnName].ToString();
                i++;
            }

            ViewBag.lonTaxLog = lon;
            ViewBag.taxLog = tab;

        }

        //Harentsoa
        [HttpGet]
        public ActionResult DocDetailsSearch()
        {
            //6104837313082 Ex

            string documentNumber = Request["DocNumbValue"];
            string sql = " SELECT CouponNumber, CouponStatus, UsageOriginCode, UsageDestinationCode,Carrier, FareBasisTicketDesignator, ReservationBookingDesignator" + Environment.NewLine;
            sql = sql + " ,UsageFlightNumber,UsageAirline, UsageDate, IsOAL" + Environment.NewLine;
            sql = sql + "FROM [Pax].[SalesDocumentCoupon]" + Environment.NewLine;
            sql = sql + " WHERE DocumentNumber = '" + documentNumber + "' " + Environment.NewLine;
            sql = sql + "ORDER BY CouponNumber";
           
            SqlConnection con = new SqlConnection(pbConnectionString);
            DataSet ds = new DataSet();
            SqlDataAdapter ada = new SqlDataAdapter(sql, con);

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

                i++;
              }

            con.Close();
            ViewBag.DocDetails = data;
           
            ViewBag.lon = ligne;
            return PartialView("DocDetailsSearch", documentNumber);
        }

        //Harentsoa
        [HttpGet]
        public ActionResult DocDetailsSearchEmpty()
        {
            //6104837313082 Ex

            string documentNumber = "";
            string sql = " SELECT CouponNumber, CouponStatus, UsageOriginCode, UsageDestinationCode,Carrier, FareBasisTicketDesignator, ReservationBookingDesignator" + Environment.NewLine;
            sql = sql + " ,UsageFlightNumber,UsageAirline, UsageDate, IsOAL" + Environment.NewLine;
            sql = sql + "FROM [Pax].[SalesDocumentCoupon]" + Environment.NewLine;
            sql = sql + " WHERE DocumentNumber = '" + documentNumber + "' " + Environment.NewLine;
            sql = sql + "ORDER BY CouponNumber";

            SqlConnection con = new SqlConnection(pbConnectionString);
            DataSet ds = new DataSet();
            SqlDataAdapter ada = new SqlDataAdapter(sql, con);

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

           
                i++;
            }

            con.Close();
            ViewBag.DocDetails = data;

            ViewBag.lon = ligne;
            return PartialView("DocDetailsSearch", documentNumber);
        }

        //Harentsoa
        public void ClearTaxLog()
        {
            string taxLogToDelete = Request["taxLogToDelete"];

            string SqlDel = "DELETE REF.TAXLOG WHERE TAXLOG='" + taxLogToDelete + "'";
            //DbUpdate(SqlDel);
            //DbClose();
            SqlConnection con = new SqlConnection(pbConnectionString);
            DataSet ds = new DataSet();
            SqlDataAdapter ada = new SqlDataAdapter(SqlDel, con);
            ada.Fill(ds);

            Taxlog();
        }



        /******** End  ATDs With Conjuction Tickets **************/


        /***** ATDs With UATP FOP    Joseph *****/

        public ActionResult ATDUatpFop()
        {
            string dateFromUatpFop = Request["dateFrom"];
            string dateToUatpFop = Request["dateTo"];

            SqlConnection con4 = new SqlConnection(pbConnectionString);
            DataSet ds = new DataSet();

            SqlCommand cmd = new SqlCommand("[Pax].[ATDsWithUATPFOP]", con4);

            cmd.CommandTimeout = 1000;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@FromDate", dateFromUatpFop);
            cmd.Parameters.AddWithValue("@ToDate", dateToUatpFop);


            int i = 0;
            SqlDataAdapter ada = new SqlDataAdapter(cmd);
            ada.Fill(ds);

            int lon = ds.Tables[0].Rows.Count;

            string[,] resultatUatpFop = new string[6, lon];

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                resultatUatpFop[0, i] = dr[ds.Tables[0].Columns[0].ColumnName].ToString();
                resultatUatpFop[1, i] = dr[ds.Tables[0].Columns[1].ColumnName].ToString();
                resultatUatpFop[2, i] = dr[ds.Tables[0].Columns[2].ColumnName].ToString();
                resultatUatpFop[3, i] = dr[ds.Tables[0].Columns[3].ColumnName].ToString();
                resultatUatpFop[4, i] = dr[ds.Tables[0].Columns[4].ColumnName].ToString();
                resultatUatpFop[5, i] = dr[ds.Tables[0].Columns[5].ColumnName].ToString();
               
                i++;
            }
            ViewBag.resultatUatpFop = resultatUatpFop;

            ViewBag.nombreUatpFop = lon;

            ViewBag.dateFromUatpFop = dateFromUatpFop;
            ViewBag.dateToUatpFop = dateToUatpFop;

            return PartialView();
        }


        /***** End ATDs With UATP FOP  *****/



        /***** Expired ATDs Partially Unused  Joseph *****/

        public ActionResult ATDPartiallyUnused()
        {
            string par = "Partially";
            string dateFromPU = Request["dateFrom"];
            string dateToPU = Request["dateTo"];

            SqlConnection con5 = new SqlConnection(pbConnectionString);
            DataSet ds = new DataSet();

            SqlCommand cmd = new SqlCommand("[Pax].[SP_TotallyandPartiallyUnusedTickets]", con5);
            cmd.CommandTimeout = 1000;
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@FromDate", dateFromPU);
            cmd.Parameters.AddWithValue("@ToDate", dateToPU);
            cmd.Parameters.AddWithValue("@UnusedCategory", par);


            int i = 0;
            SqlDataAdapter ada = new SqlDataAdapter(cmd);
            ada.Fill(ds);

            int lon = ds.Tables[0].Rows.Count;

            string[,] resultatPU = new string[6, lon];

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                resultatPU[0, i] = dr[ds.Tables[0].Columns[0].ColumnName].ToString();
                resultatPU[1, i] = dr[ds.Tables[0].Columns[1].ColumnName].ToString();
                resultatPU[2, i] = dr[ds.Tables[0].Columns[2].ColumnName].ToString();
                resultatPU[3, i] = dr[ds.Tables[0].Columns[3].ColumnName].ToString();
                resultatPU[4, i] = dr[ds.Tables[0].Columns[4].ColumnName].ToString();
                resultatPU[5, i] = dr[ds.Tables[0].Columns[5].ColumnName].ToString();

                i++;
            }
            ViewBag.resultatPU = resultatPU;

            ViewBag.nombrePU = lon;

            ViewBag.dateFromPU = dateFromPU;
            ViewBag.dateToPU = dateToPU;

            return PartialView();
        }

        /***** End  Expired ATDs Partially Unused *****/


        /**** Expired ATDs Totally Unused  Joseph ****/
        public ActionResult ATDTotallyUnused()
        {
            string tot = "Totally";
            string dateFromTU = Request["dateFrom"];
            string dateToTU = Request["dateTo"];

            SqlConnection con6 = new SqlConnection(pbConnectionString);
            DataSet ds = new DataSet();

            SqlCommand cmd = new SqlCommand("[Pax].[SP_TotallyandPartiallyUnusedTickets]", con6);
            cmd.CommandTimeout = 1000;
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@FromDate", dateFromTU);
            cmd.Parameters.AddWithValue("@ToDate", dateToTU);
            cmd.Parameters.AddWithValue("@UnusedCategory", tot);


            int i = 0;
            SqlDataAdapter ada = new SqlDataAdapter(cmd);
            ada.Fill(ds);

            int lon = ds.Tables[0].Rows.Count;

            string[,] resultatTU = new string[5, lon];

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                resultatTU[0, i] = dr[ds.Tables[0].Columns[0].ColumnName].ToString();
                resultatTU[1, i] = dr[ds.Tables[0].Columns[1].ColumnName].ToString();
                resultatTU[2, i] = dr[ds.Tables[0].Columns[2].ColumnName].ToString();
                resultatTU[3, i] = dr[ds.Tables[0].Columns[3].ColumnName].ToString();
                resultatTU[4, i] = dr[ds.Tables[0].Columns[4].ColumnName].ToString();

                i++;
            }
            ViewBag.resultatTU = resultatTU;

            ViewBag.nombreTU = lon;

            ViewBag.dateFromTU = dateFromTU;
            ViewBag.dateToTU = dateToTU;

            return PartialView();
        }

        /**** End  Expired ATDs Totally Unused   ****/


        /**** Tickets With Transit Points  Joseph ****/
        public ActionResult TicketsTransitPoints()
        {
            string dateFromTP = Request["dateFrom"];
            string dateToTP = Request["dateTo"];

            SqlConnection con7 = new SqlConnection(pbConnectionString);
            DataSet ds = new DataSet();

            SqlCommand cmd = new SqlCommand("[Pax].[ATDsWithTransitPoints]", con7);
            cmd.CommandTimeout = 1000;
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@FromDate", dateFromTP);
            cmd.Parameters.AddWithValue("@ToDate", dateToTP);


            int i = 0;
            SqlDataAdapter ada = new SqlDataAdapter(cmd);
            ada.Fill(ds);

            int lon = ds.Tables[0].Rows.Count;

            string[,] resultatTP = new string[10, lon];

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                resultatTP[0, i] = dr[ds.Tables[0].Columns[0].ColumnName].ToString();
                resultatTP[1, i] = dr[ds.Tables[0].Columns[1].ColumnName].ToString();
                resultatTP[2, i] = dr[ds.Tables[0].Columns[2].ColumnName].ToString();
                resultatTP[3, i] = dr[ds.Tables[0].Columns[3].ColumnName].ToString();
                resultatTP[4, i] = dr[ds.Tables[0].Columns[4].ColumnName].ToString();
                resultatTP[5, i] = dr[ds.Tables[0].Columns[5].ColumnName].ToString();
                resultatTP[6, i] = dr[ds.Tables[0].Columns[6].ColumnName].ToString();
                resultatTP[7, i] = dr[ds.Tables[0].Columns[7].ColumnName].ToString();
                resultatTP[8, i] = dr[ds.Tables[0].Columns[8].ColumnName].ToString();
                resultatTP[9, i] = dr[ds.Tables[0].Columns[9].ColumnName].ToString();

                i++;
            }
            ViewBag.resultatTP = resultatTP;

            ViewBag.nombreTP = lon;

            ViewBag.dateFromTP = dateFromTP;
            ViewBag.dateToTP = dateToTP;

            return PartialView();
        }

        /**** End  Tickets With Transit Points  ****/



        /***  Sales Transaction By Type  Joseph ***/

        public ActionResult SalesTransactionBT(string param)
        {
            string dateFromSalesTbt = Request["dateFrom"];
            string dateToSalesTbt = Request["dateTo"];

            SqlConnection con7 = new SqlConnection(pbConnectionString);
            DataSet ds = new DataSet();

            SqlCommand cmd = new SqlCommand("[Pax].[SP_SalesTransactionByType]", con7);
            cmd.CommandTimeout = 1000;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@FromDate", dateFromSalesTbt);
            cmd.Parameters.AddWithValue("@ToDate", dateToSalesTbt);
            cmd.Parameters.AddWithValue("@TransactionType", param);

            int i = 0;
            SqlDataAdapter ada = new SqlDataAdapter(cmd);
            ada.Fill(ds);

            int lon = ds.Tables[0].Rows.Count;

            string[,] resultatSalesTbt = new string[8, lon];

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                resultatSalesTbt[0, i] = dr[ds.Tables[0].Columns[0].ColumnName].ToString();
                resultatSalesTbt[1, i] = dr[ds.Tables[0].Columns[1].ColumnName].ToString();
                resultatSalesTbt[2, i] = dr[ds.Tables[0].Columns[2].ColumnName].ToString();
                resultatSalesTbt[3, i] = dr[ds.Tables[0].Columns[3].ColumnName].ToString();
                resultatSalesTbt[4, i] = dr[ds.Tables[0].Columns[4].ColumnName].ToString();
                resultatSalesTbt[5, i] = dr[ds.Tables[0].Columns[5].ColumnName].ToString();
                resultatSalesTbt[6, i] = dr[ds.Tables[0].Columns[6].ColumnName].ToString();
                resultatSalesTbt[7, i] = dr[ds.Tables[0].Columns[7].ColumnName].ToString();

                i++;
            }


            ViewBag.resultatSalesTbt = resultatSalesTbt;

            ViewBag.nombreSalesTbt = lon;

            ViewBag.dateFromSalesTbt = dateFromSalesTbt;
            ViewBag.dateToSalesTbt = dateToSalesTbt;

            getItemCombo();

            return PartialView();
        }


        // function recuper list combo Josph
        public ActionResult GetTransationCode()
        {

            getItemCombo();

            return PartialView();
        }


        public void getItemCombo()
        {
            string dtpSaleTranFrom = Request["dateFrom"];
            string dtpSaleTranTo = Request["dateTo"];

            string sql = "select distinct TransactionCode from [Pax].[VW_SalesHeader] where DateofIssue between '" + dtpSaleTranFrom + "'and '" + dtpSaleTranTo + "'  ";

            SqlConnection con = new SqlConnection(pbConnectionString);
            DataSet ds = new DataSet();
            SqlCommand cmd = new SqlCommand("" + sql + "", con);


            SqlDataAdapter ada = new SqlDataAdapter(cmd);

            ada.Fill(ds);

            int lonItem = ds.Tables[0].Rows.Count;

            string[,] ListeItem = new string[1, lonItem];


            int i = 0;

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                ListeItem[0, i] = dr[ds.Tables[0].Columns[0].ColumnName].ToString();

                i++;
            }


            ViewBag.lonItem = lonItem;
            ViewBag.ListeItem = ListeItem;
            ViewBag.dateFromSalesTbt = dtpSaleTranFrom;
            ViewBag.dateToSalesTbt = dtpSaleTranTo;
        }

        /***  End  Transaction By Type  Joseph ***/


        /***  Flown Transaction By Type   Joseph ***/
        public ActionResult FlownTransactionBT(string flwntr)
        {
            string dtpFlownTranFrom = Request["dateFrom"];
            string dtpFlownTranTo = Request["dateTo"];
            

            SqlConnection con7 = new SqlConnection(pbConnectionString);
            DataSet ds = new DataSet();

            SqlCommand cmd = new SqlCommand("[Pax].[SP_FlownTransactionByType]", con7);
            cmd.CommandTimeout = 1000;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@FromDate", dtpFlownTranFrom);
            cmd.Parameters.AddWithValue("@ToDate", dtpFlownTranTo);
            cmd.Parameters.AddWithValue("@TransactionType", flwntr);

            int i = 0;
            SqlDataAdapter ada = new SqlDataAdapter(cmd);
            ada.Fill(ds);

            int lon = ds.Tables[0].Rows.Count;

            string[,] resultatFlown = new string[8, lon];

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                resultatFlown[0, i] = dr[ds.Tables[0].Columns[0].ColumnName].ToString();
                resultatFlown[1, i] = dr[ds.Tables[0].Columns[1].ColumnName].ToString();
                resultatFlown[2, i] = dr[ds.Tables[0].Columns[2].ColumnName].ToString();
                resultatFlown[3, i] = dr[ds.Tables[0].Columns[3].ColumnName].ToString();
                resultatFlown[4, i] = dr[ds.Tables[0].Columns[4].ColumnName].ToString();
                resultatFlown[5, i] = dr[ds.Tables[0].Columns[5].ColumnName].ToString();
                resultatFlown[6, i] = dr[ds.Tables[0].Columns[6].ColumnName].ToString();
                resultatFlown[7, i] = dr[ds.Tables[0].Columns[7].ColumnName].ToString();

                i++;
            }


            ViewBag.resultatFlown = resultatFlown;

            ViewBag.nombreFlown = lon;

            ViewBag.dateFromFlown = dtpFlownTranFrom;
            ViewBag.dateToFlown = dtpFlownTranTo;

            getItemFlown();

            return PartialView();
        }


        public void getItemFlown()
        {
            string sql = "SELECT distinct f1.TransactionCode FROM pax.VW_SalesHeader f1 left join pax.VW_SalesCouponDetail f2 on f2.HdrGuid = f1.HdrGuid ";

            SqlConnection con = new SqlConnection(pbConnectionString);
            DataSet ds = new DataSet();
            SqlCommand cmd = new SqlCommand("" + sql + "", con);


            SqlDataAdapter ada = new SqlDataAdapter(cmd);

            ada.Fill(ds);

            int lonItem = ds.Tables[0].Rows.Count;

            string[,] ListeItemFlown = new string[1, lonItem];

            int i = 0;

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                ListeItemFlown[0, i] = dr[ds.Tables[0].Columns[0].ColumnName].ToString();

                i++;
            }

            ViewBag.lonItem = lonItem;
            ViewBag.ListeItemFlown = ListeItemFlown;

        }

        /***  End Flown Transaction By Type   Joseph ***/


        /*** Interline Transaction By Type  Joseph ***/
        public ActionResult InterlineTransactionBT(string Intertr)
        {
            string dtpInterTranFrom = Request["dateFrom"];
            string dtpInterTranTo = Request["dateTo"];


            SqlConnection con7 = new SqlConnection(pbConnectionString);
            DataSet ds = new DataSet();

            SqlCommand cmd = new SqlCommand("[Pax].[SP_InterlineTransactionByType]", con7);
            cmd.CommandTimeout = 1000;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@FromDate", dtpInterTranFrom);
            cmd.Parameters.AddWithValue("@ToDate", dtpInterTranTo);
            cmd.Parameters.AddWithValue("@TransactionType", Intertr);

            int i = 0;
            SqlDataAdapter ada = new SqlDataAdapter(cmd);
            ada.Fill(ds);

            int lon = ds.Tables[0].Rows.Count;

            string[,] resultatInter = new string[8, lon];

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                resultatInter[0, i] = dr[ds.Tables[0].Columns[0].ColumnName].ToString();
                resultatInter[1, i] = dr[ds.Tables[0].Columns[1].ColumnName].ToString();
                resultatInter[2, i] = dr[ds.Tables[0].Columns[2].ColumnName].ToString();
                resultatInter[3, i] = dr[ds.Tables[0].Columns[3].ColumnName].ToString();
                resultatInter[4, i] = dr[ds.Tables[0].Columns[4].ColumnName].ToString();
                resultatInter[5, i] = dr[ds.Tables[0].Columns[5].ColumnName].ToString();
                resultatInter[6, i] = dr[ds.Tables[0].Columns[6].ColumnName].ToString();
                resultatInter[7, i] = dr[ds.Tables[0].Columns[7].ColumnName].ToString();

                i++;
            }

            ViewBag.resultatInter = resultatInter;
            ViewBag.nombreInter = lon;
            ViewBag.dateFromInterline = dtpInterTranFrom;
            ViewBag.dateToInterline = dtpInterTranTo;

            getItemInterline();

            return PartialView();
        }

       /* function get Item Interline       Joseph */  
        public void getItemInterline()
        {
            string sql = "SELECT distinct f1.TransactionCode FROM pax.VW_SalesHeader f1 left join pax.VW_SalesCouponDetail f2 on f2.HdrGuid = f1.HdrGuid ";

            SqlConnection con = new SqlConnection(pbConnectionString);
            DataSet ds = new DataSet();
            SqlCommand cmd = new SqlCommand("" + sql + "", con);


            SqlDataAdapter ada = new SqlDataAdapter(cmd);

            ada.Fill(ds);

            int lonItem = ds.Tables[0].Rows.Count;

            string[,] ListeItemInter = new string[1, lonItem];

            int i = 0;

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                ListeItemInter[0, i] = dr[ds.Tables[0].Columns[0].ColumnName].ToString();

                i++;
            }

            ViewBag.lonItem = lonItem;
            ViewBag.ListeItemInter = ListeItemInter;
        }


        /*** Interline Transaction By Type ***/


        /*** Five Days Rate By Currency / Country  Joseph ***/

        public ActionResult DaysRateByCurrency()
        {
            // Get All Item
            getItemPeriod();
            getItemCountry();

            string valPeriod = Request["paramPeriode"];
            string valCountry = Request["paramCountry"];
            string valCurrency = Request["paramCurrency"];

            SqlConnection con = new SqlConnection(pbConnectionString);
            DataSet ds = new DataSet();

            SqlCommand cmd = new SqlCommand("[Pax].[SPFiveDayRate]", con);
            cmd.CommandTimeout = 1000;
            cmd.CommandType = CommandType.StoredProcedure;


            cmd.Parameters.AddWithValue("@period", valPeriod);
            cmd.Parameters.AddWithValue("@country", valCountry);
            cmd.Parameters.AddWithValue("@currency", valCurrency);

            SqlDataAdapter ada = new SqlDataAdapter(cmd);
            ada.Fill(ds);

            int lonDr = ds.Tables[0].Rows.Count;

            string[,] resultatDr= new string[7, lonDr];

            int i = 0;

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                resultatDr[0, i] = dr[ds.Tables[0].Columns[0].ColumnName].ToString();
                resultatDr[1, i] = dr[ds.Tables[0].Columns[1].ColumnName].ToString();
                resultatDr[2, i] = dr[ds.Tables[0].Columns[2].ColumnName].ToString();
                resultatDr[3, i] = dr[ds.Tables[0].Columns[3].ColumnName].ToString();
                resultatDr[4, i] = dr[ds.Tables[0].Columns[4].ColumnName].ToString();
                resultatDr[5, i] = dr[ds.Tables[0].Columns[5].ColumnName].ToString();
                resultatDr[6, i] = dr[ds.Tables[0].Columns[6].ColumnName].ToString();

                i++;
            }

            // Get Currency with Country
                string valueCountry;

                if(valCountry == "%")
                {
                    valueCountry = "";
                }
                else
                {
                    valueCountry = valCountry;
                }
            
            // Change Current Change
            changeCurrent(valueCountry);

            ViewBag.valPeriod = valPeriod;
            ViewBag.valCountry = valCountry;
            ViewBag.valCurrency = valCurrency;

            ViewBag.nombreDR = lonDr;
            ViewBag.resultatDr = resultatDr;

            return PartialView();
        }

        /*Get Item Period sans parametre   Joseph */
        public void getItemPeriod()
        {

            string sql = "SELECT DISTINCT Period  FROM Ref.CurrencyRate";

            SqlConnection conp = new SqlConnection(pbConnectionString);

            DataSet ds = new DataSet();
            SqlCommand cmd = new SqlCommand("" + sql + "", conp);


            SqlDataAdapter ada = new SqlDataAdapter(cmd);

            ada.Fill(ds);

            int lonItemP = ds.Tables[0].Rows.Count;

            string[,] ListeItemPeriod = new string[1, lonItemP];

            int i = 0;

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                ListeItemPeriod[0, i] = dr[ds.Tables[0].Columns[0].ColumnName].ToString();

                i++;
            }

            ViewBag.lonItemP = lonItemP;
            ViewBag.ListeItemPeriod = ListeItemPeriod;
        }

        /*Get Item Country sans parametre   Joseph */
        public void getItemCountry()
        {

            string sql = "SELECT DISTINCT Name,Code, Currency   FROM Ref.Country ORDER BY Name ASC ";

            SqlConnection conp = new SqlConnection(pbConnectionString);

            DataSet ds = new DataSet();
            SqlCommand cmd = new SqlCommand("" + sql + "", conp);


            SqlDataAdapter ada = new SqlDataAdapter(cmd);

            ada.Fill(ds);

            int lonItemCou = ds.Tables[0].Rows.Count;

            string[,] ListeItemCou = new string[3, lonItemCou];

            int i = 0;

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                ListeItemCou[0, i] = dr[ds.Tables[0].Columns[0].ColumnName].ToString();
                ListeItemCou[1, i] = dr[ds.Tables[0].Columns[1].ColumnName].ToString();
                ListeItemCou[2, i] = dr[ds.Tables[0].Columns[2].ColumnName].ToString();

                i++;
            }

            ViewBag.lonItemCou = lonItemCou;
            ViewBag.ListeItemCou = ListeItemCou;
        }

        /*Get Item Currency sans parametre   Joseph */
        public void getItemCurrency()
        {

            string sql= "SELECT  Currency FROM Ref.Country ";

            SqlConnection conp = new SqlConnection(pbConnectionString);

            DataSet ds = new DataSet();
            SqlCommand cmd = new SqlCommand("" + sql + "", conp);

            SqlDataAdapter ada = new SqlDataAdapter(cmd);

            ada.Fill(ds);

            int lonItemCur = ds.Tables[0].Rows.Count;

            string[,] ListeItemCur = new string[2, lonItemCur];

            int i = 0;

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                ListeItemCur[0, i] = dr[ds.Tables[0].Columns[0].ColumnName].ToString();
              
                i++;
            }

            ViewBag.lonItemCur = lonItemCur;
            ViewBag.ListeItemCur = ListeItemCur;

        }

        /* Get  ItemCurrency with Country      Joseph*/

        public ActionResult recupItemCurrency()
        {
            // Get All Item
            getItemPeriod();
            getItemCountry();

            string period = Request["paramPeriode"];
            string country = Request["paramCountry"];

            changeCurrent(country);

            ViewBag.valPeriod = period;
            ViewBag.valCountry = country;

            return PartialView();
        }

        /* Get ChangeCurrent    Joseph */
        public void changeCurrent(string country)
        {
            string sql;

            if (country == "")
            {
                sql = "SELECT  Currency FROM Ref.Country ";
            }
            else
            {
                sql = "SELECT  Currency  FROM Ref.Country WHERE Name= '" + country + "'";
            }


            SqlConnection conp = new SqlConnection(pbConnectionString);
            DataSet ds = new DataSet();
            SqlCommand cmd = new SqlCommand("" + sql + "", conp);

            SqlDataAdapter ada = new SqlDataAdapter(cmd);

            ada.Fill(ds);

            int lonItemCur = ds.Tables[0].Rows.Count;
            string[,] ListeItemCur = new string[2, lonItemCur];

            int i = 0;

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                ListeItemCur[0, i] = dr[ds.Tables[0].Columns[0].ColumnName].ToString();
                i++;
            }

            ViewBag.lonItemCur = lonItemCur;
            ViewBag.ListeItemCur = ListeItemCur;
        }

        /*** End Five Days Rate By Currency / Country  ***/


        /* City / Airport     Joseph*/

        //function CityAirport Search 
        public ActionResult CityAirport()
        {
            string CityAirport = Request["paramCA"];
            string ByCityAirport = Request["paramByCA"];

            // function City / Airport Exist
            getexist();
            getExistAirCode();
            getItemAddCountry();

            listeCityAirport(CityAirport, ByCityAirport);

            return PartialView();
        }

        /*funtion getListe CityAirport      Joseph */
        public void listeCityAirport(string CityAirport,string ByCityAirport)
        {
            string sql = "";

            if (CityAirport != "-ALL-")
            {
                switch (CityAirport)
                {

                    case "Airport Code":
                        sql = "SELECT AirportCode, CityCode, AirportName, CityName, Country, CityIsoCode, Status FROM Ref.City WHERE AirportCode= '" + ByCityAirport + "'";

                        break;
                    case "City Code":
                        sql = "SELECT AirportCode, CityCode, AirportName, CityName, Country, CityIsoCode, Status FROM Ref.City WHERE CityCode= '" + ByCityAirport + "'";
                        break;
                    case "Airport Name":
                        sql = "SELECT AirportCode, CityCode, AirportName, CityName, Country, CityIsoCode, Status FROM Ref.City WHERE AirportName= '" + ByCityAirport + "'";
                        break;
                    case "City Name":
                        sql = "SELECT AirportCode, CityCode, AirportName, CityName, Country, CityIsoCode, Status FROM Ref.City WHERE CityName= '" + ByCityAirport + "'";
                        break;
                    case "Country":
                        sql = "SELECT AirportCode, CityCode, AirportName, CityName, Country, CityIsoCode, Status FROM Ref.City WHERE Country= '" + ByCityAirport + "'";
                        break;
                    case "CityIsoCode":
                        sql = "SELECT AirportCode, CityCode, AirportName, CityName, Country, CityIsoCode, Status FROM Ref.City WHERE CityIsoCode= '" + ByCityAirport + "'";
                        break;
                }
            }
            else
            {
                sql = "SELECT * FROM Ref.City";
            }

            SqlConnection con = new SqlConnection(pbConnectionString);
            DataSet ds = new DataSet();
            SqlCommand cmd = new SqlCommand("" + sql + "", con);

            SqlDataAdapter ada = new SqlDataAdapter(cmd);

            ada.Fill(ds);

            int lonCity = ds.Tables[0].Rows.Count;

            string[,] ListeCity = new string[7, lonCity];

            int i = 0;

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                ListeCity[0, i] = dr[ds.Tables[0].Columns[0].ColumnName].ToString();
                ListeCity[1, i] = dr[ds.Tables[0].Columns[1].ColumnName].ToString();
                ListeCity[2, i] = dr[ds.Tables[0].Columns[2].ColumnName].ToString();
                ListeCity[3, i] = dr[ds.Tables[0].Columns[3].ColumnName].ToString();
                ListeCity[4, i] = dr[ds.Tables[0].Columns[4].ColumnName].ToString();
                ListeCity[5, i] = dr[ds.Tables[0].Columns[5].ColumnName].ToString();
                ListeCity[6, i] = dr[ds.Tables[0].Columns[6].ColumnName].ToString();

                i++;
            }

            ViewBag.lonCity = lonCity;
            ViewBag.ListeCity = ListeCity;

            ViewBag.valCA = CityAirport;
            ViewBag.valParamS = ByCityAirport;

            //getItem
            getItemByCity(CityAirport, ByCityAirport);
        }

        /* End funtion getListe CityAirport      Joseph    */


        /* function to changeCombo  Search By  CityAirport     Joseph */
        public ActionResult SearchByCityAirport()
        {
            string CityAirport = Request["paramCA"];
            string ByCityAirport = Request["paramCA"];

            getItemByCity(CityAirport , "");

            getItemAddCountry();
            // function City / Airport Exist
            getexist();
            getExistAirCode();

            return PartialView();
        }
        /* End function to changeCombo  Search By  CityAirport     Joseph */


        /* get changeCombo     Joseph */
        public void getItemByCity(string CityAirport, string ByCityAirport)
        {

            if (CityAirport != "-ALL-")
            {
                string sql = "";

                switch (CityAirport)
                {
                    case "Airport Code":
                        sql = "SELECT DISTINCT AirportCode FROM Ref.City ORDER BY AirportCode ASC";
                        break;
                    case "City Code":
                        sql = "SELECT DISTINCT CityCode FROM Ref.City ORDER BY CityCode ASC";
                        break;
                    case "Airport Name":
                        sql = "SELECT DISTINCT AirportName FROM Ref.City ORDER BY AirportName ASC";
                        break;
                    case "City Name":
                        sql = "SELECT DISTINCT CityName FROM Ref.City ORDER BY CityName ASC";
                        break;
                    case "Country":
                        sql = "SELECT DISTINCT Country FROM Ref.City ORDER BY Country ASC";
                        break;
                    case "CityIsoCode":
                        sql = "SELECT DISTINCT CityIsoCode FROM Ref.City ORDER BY CityIsoCode ASC";
                        break;
                }

                SqlConnection con = new SqlConnection(pbConnectionString);
                DataSet ds = new DataSet();
                SqlCommand cmd = new SqlCommand("" + sql + "", con);

                SqlDataAdapter ada = new SqlDataAdapter(cmd);

                ada.Fill(ds);

                int lonItemCity = ds.Tables[0].Rows.Count;

                string[,] ListeItemCity = new string[1, lonItemCity];

                int i = 0;

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    ListeItemCity[0, i] = dr[ds.Tables[0].Columns[0].ColumnName].ToString();

                    i++;
                }

                ViewBag.lonItemCity = lonItemCity;
                ViewBag.ListeItemCity = ListeItemCity;

                ViewBag.valCA = CityAirport;
                ViewBag.valParamS = ByCityAirport;
            }
            else
            {
                ViewBag.lonItemCity = 0;
                ViewBag.ListeItemCity = "";

                ViewBag.valCA = "-ALL-";
            }
        }
        /* End get changeCombo     Joseph */


        /* function Add New Entry */
        public ActionResult AddNewEntry()
        {
            string airportCode = Request["airportCode"];
            string cityCode = Request["cityCode"];
            string airportName = Request["ariportName"];
            string cityName = Request["cityName"];
            string country = Request["country"];
            string cityIsoCode = Request["cityIsoCode"];
            string status = Request["status"];

            string sql = "INSERT INTO [Ref].[City]([AirportCode],[CityCode],[AirportName],[CityName],[Country],[CityIsoCode],[Status]) VALUES ('"+airportCode+"','"+cityCode+"','"+airportName+"','"+cityName+"','"+country+"','"+cityIsoCode+"','"+status+"')";

            DataSet ds = new DataSet();
            ds = dbconnect.RetObjDS(sql);

            // Affiche liste all
            string CityAirport = "-ALL-";
            string ByCityAirport = "";

            // function City / Airport Exist
            getexist();
            getExistAirCode();
            getItemAddCountry();

            listeCityAirport(CityAirport, ByCityAirport);


            return PartialView();
        }

        //function getExistCityCode      Joseph
        public void getexist()
        {
            string sql = "SELECT CityCode FROM [Ref].[City]";

            SqlConnection con = new SqlConnection(pbConnectionString);
            DataSet ds = new DataSet();
            SqlCommand cmd = new SqlCommand("" + sql + "", con);

            SqlDataAdapter ada = new SqlDataAdapter(cmd);

            ada.Fill(ds);

            int lonCityCode = ds.Tables[0].Rows.Count;

            string[,] cityCode = new string[1, lonCityCode];

            int i = 0;

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                cityCode[0, i] = dr[ds.Tables[0].Columns[0].ColumnName].ToString();

                i++;
            }

            ViewBag.lonCityCode = lonCityCode;
            ViewBag.cityCode = cityCode;

        }


        public void getExistAirCode()
        {
            string sql = "SELECT AirportCode FROM [Ref].[City]";

            SqlConnection con = new SqlConnection(pbConnectionString);
            DataSet ds = new DataSet();
            SqlCommand cmd = new SqlCommand("" + sql + "", con);

            SqlDataAdapter ada = new SqlDataAdapter(cmd);

            ada.Fill(ds);

            int lonAirCode = ds.Tables[0].Rows.Count;

            string[,] airCode = new string[1, lonAirCode];

            int i = 0;

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                airCode[0, i] = dr[ds.Tables[0].Columns[0].ColumnName].ToString();

                i++;
            }

            ViewBag.lonAirCode = lonAirCode;
            ViewBag.airCode = airCode;
        }


        /*function get Item Country      Joseph */
        public void getItemAddCountry()
        {
            string sql = "SELECT DISTINCT Country FROM Ref.City ORDER BY Country ASC";

            SqlConnection con = new SqlConnection(pbConnectionString);
            DataSet ds = new DataSet();
            SqlCommand cmd = new SqlCommand("" + sql + "", con);

            SqlDataAdapter ada = new SqlDataAdapter(cmd);

            ada.Fill(ds);

            int lonCountry = ds.Tables[0].Rows.Count;

            string[,] ItemCountry = new string[1, lonCountry];

            int i = 0;

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                ItemCountry[0, i] = dr[ds.Tables[0].Columns[0].ColumnName].ToString();

                i++;
            }

            ViewBag.lonCountry = lonCountry;
            ViewBag.ItemCountry = ItemCountry;

        }


        /*function delete City Airport     Joseph*/

        public ActionResult DeleteCityAirport()
        {
            string airCode = Request["arCode"];
            string cityCode = Request["cityCode"];

            string sql = "DELETE FROM [Ref].[City]  WHERE [AirportCode] = '" + airCode + "'  AND  [CityCode] = '" + cityCode + "' ";


            DataSet ds = new DataSet();
            ds = dbconnect.RetObjDS(sql);

            // Affiche liste all
            string CityAirport = "-ALL-";
            string ByCityAirport = "";

            // function City / Airport Exist
            getexist();
            getExistAirCode();
            getItemAddCountry();

            listeCityAirport(CityAirport, ByCityAirport);

            return PartialView();
        }


        /* End City / Airport     Joseph */


        /*  Prorate Factors     Joseph*/
        public ActionResult ProrateFactors()
        {
            return PartialView();
        }

        // function Changement Combo  Joseph
        public ActionResult ChangeProrateFactors()
        {
            getItemCountry();
            getItemDCity("");

            string CountryO = Request["OCountryN"];
            string valOriginCount = Request["valOC"];

           ViewBag.valCN = CountryO;
           getItemCity(valOriginCount);

            return PartialView();
        }

        public ActionResult ChangeDestination()
        {
            getItemCountry();
            getItemCity("");

            string CountryD = Request["DCountryN"];
            string valDestCount = Request["valDC"];

            ViewBag.valDCN = CountryD;
            getItemDCity(valDestCount);

            //recuperation Origin Country 
            string CountryO = Request["OCountryN"];
            string valOriginCount = Request["valOC"];
            ViewBag.valCN = CountryO;

            // recuperation Origin CityName
            string CityNameO = Request["OCityName"];
            string valCityNameO = Request["valOCN"];
            ViewBag.valCityName = CityNameO;


            return PartialView("ChangeProrateFactors");
        }


        // function Search Proration Factor  Joseph
        public ActionResult SearchProrationFactor()
        {
            getItemCountry();

            string OCountryN = Request["valOCountryN"];
            string OCityName = Request["valOCityName"];
            string DCountryN = Request["valDCountryN"];
            string DCityName = Request["valDCityName"];
            string dtpValid = Request["datefrom"];

            string paramOrinCity = Request["valOC"];
            string paramDestCity = Request["valDC"];

            getItemCity(paramOrinCity);
            getItemDCity(paramDestCity);

            string paramOrgCountryName;
            string paramOriCityName;
            string paramDestCountryName;
            string paramDestCityName;

            // param Origin Country Name
            if (OCountryN == "-ALL-")
            {
                paramOrgCountryName = "%";
            }
            else
            {
                paramOrgCountryName = OCountryN + "%";
            }
            
            // param Origin City Name
            if (OCityName == "-ALL-")
            {
                paramOriCityName = "%";
            }
            else
            {
                paramOriCityName = OCityName + "%";
            }

            // param Dest Country Name
            if (DCountryN == "-ALL-")
            {
                paramDestCountryName = "%";
            }
            else
            {
                paramDestCountryName = DCountryN + "%";
            }

            // param Dest City Name
            if (DCityName == "-ALL-")
            {
                paramDestCityName = "%";
            }
            else
            {
                paramDestCityName = DCityName + "%";
            }

            SqlConnection con = new SqlConnection(pbConnectionString);
            DataSet ds = new DataSet();

            SqlCommand cmd = new SqlCommand("[Pax].[SPProrateFactor]", con);
            cmd.CommandTimeout = 1000;
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@OrgCityName", paramOriCityName);
            cmd.Parameters.AddWithValue("@OrgCountryName", paramOrgCountryName);
            cmd.Parameters.AddWithValue("@DestCityName", paramDestCityName);
            cmd.Parameters.AddWithValue("@DestCountryName", paramDestCountryName);
            cmd.Parameters.AddWithValue("@Validity", dtpValid);

            int i = 0;
            SqlDataAdapter ada = new SqlDataAdapter(cmd);
            ada.Fill(ds);

            int lon = ds.Tables[0].Rows.Count;

            string[,] prorateFactor = new string[13, lon];

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                prorateFactor[0, i] = dr[ds.Tables[0].Columns[0].ColumnName].ToString();
                prorateFactor[1, i] = dr[ds.Tables[0].Columns[1].ColumnName].ToString();
                prorateFactor[2, i] = dr[ds.Tables[0].Columns[2].ColumnName].ToString();
                prorateFactor[3, i] = dr[ds.Tables[0].Columns[3].ColumnName].ToString();
                prorateFactor[4, i] = dr[ds.Tables[0].Columns[4].ColumnName].ToString();
                prorateFactor[5, i] = dr[ds.Tables[0].Columns[5].ColumnName].ToString();
                prorateFactor[6, i] = dr[ds.Tables[0].Columns[6].ColumnName].ToString();
                prorateFactor[7, i] = dr[ds.Tables[0].Columns[7].ColumnName].ToString();
                prorateFactor[8, i] = dr[ds.Tables[0].Columns[8].ColumnName].ToString();
                prorateFactor[9, i] = dr[ds.Tables[0].Columns[9].ColumnName].ToString();
                prorateFactor[10, i] = dr[ds.Tables[0].Columns[10].ColumnName].ToString();
                prorateFactor[11, i] = dr[ds.Tables[0].Columns[11].ColumnName].ToString();
                prorateFactor[12, i] = dr[ds.Tables[0].Columns[12].ColumnName].ToString();

                i++;
            }

            ViewBag.prorateFactor = prorateFactor;
            ViewBag.nbProrate = lon;

            ViewBag.valCN = OCountryN;
            ViewBag.valCityName = OCityName;
            ViewBag.valDCN = DCountryN;
            ViewBag.valDCity = DCityName;
            ViewBag.dtpIssueDateFrom = dtpValid;

            return PartialView("ChangeProrateFactors");
        }


        /* Get Item City   Joseph */

        public void getItemCity(string countryCode)
        {
            string sql;

            if(countryCode == "-ALL-" || countryCode == "")
            {
                sql = "SELECT DISTINCT CityName, CityCode  FROM Ref.City WHERE CityName != '' ORDER BY CityName ASC";
            }
            else
            {
                sql = "SELECT DISTINCT CityName, CityCode FROM Ref.City WHERE CityName != '' AND Country = '" + countryCode + "' ORDER BY CityName ASC";
            }

            SqlConnection con = new SqlConnection(pbConnectionString);
            DataSet ds = new DataSet();
            SqlCommand cmd = new SqlCommand("" + sql + "", con);

            SqlDataAdapter ada = new SqlDataAdapter(cmd);

            ada.Fill(ds);

            int lonItemCit = ds.Tables[0].Rows.Count;

            string[,] ItemCity = new string[2, lonItemCit];

            int i = 0;

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                ItemCity[0, i] = dr[ds.Tables[0].Columns[0].ColumnName].ToString();
                ItemCity[1, i] = dr[ds.Tables[0].Columns[1].ColumnName].ToString();

                i++;
            }

            ViewBag.lonItemCit = lonItemCit;
            ViewBag.ItemCity = ItemCity;
        }

        public void getItemDCity(string countryCode)
        {
            string sql;

            if (countryCode == "-ALL-" || countryCode == "")
            {
                sql = "SELECT DISTINCT CityName, CityCode  FROM Ref.City WHERE CityName != '' ORDER BY CityName ASC";
            }
            else
            {
                sql = "SELECT DISTINCT CityName, CityCode FROM Ref.City WHERE CityName != '' AND Country = '" + countryCode + "' ORDER BY CityName ASC";
            }

            SqlConnection con = new SqlConnection(pbConnectionString);
            DataSet ds = new DataSet();
            SqlCommand cmd = new SqlCommand("" + sql + "", con);

            SqlDataAdapter ada = new SqlDataAdapter(cmd);

            ada.Fill(ds);

            int lonItemDCit = ds.Tables[0].Rows.Count;

            string[,] ItemDCity = new string[2, lonItemDCit];

            int i = 0;

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                ItemDCity[0, i] = dr[ds.Tables[0].Columns[0].ColumnName].ToString();
                ItemDCity[1, i] = dr[ds.Tables[0].Columns[1].ColumnName].ToString();

                i++;
            }

            ViewBag.lonItemDCit = lonItemDCit;
            ViewBag.ItemDCity = ItemDCity;
        }

        /* End Get Item City   Joseph */

        /* End  Prorate Factors     Joseph*/


         //Samuel
        /************************SML SVC *******************/
        public ActionResult SectorValueControl()
        {
            ViewBag.dateFrom = DateTime.Now.AddMonths(-1).ToString("dd-MMM-yyyy");
            ViewBag.dateTo = DateTime.Now.ToString("dd-MMM-yyyy");
            return PartialView();

        }
        public ActionResult LoadSectorValueControl()
        {
            string sql = "select * from Ref.FareControl";
            SqlConnection conn = new SqlConnection(pbConnectionString);
            DataSet ds = new DataSet();
            SqlDataAdapter ada1 = new SqlDataAdapter(sql, conn);

            ada1.Fill(ds);
            conn.Close();
            int ligne = ds.Tables[0].Rows.Count;

            string[,] data = new string[7, ligne];

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
                i++;
            }
            ViewBag.lon = ligne;
            ViewBag.data = data;
            return PartialView();

        }

        public ActionResult viewSelectedSectorValueControl()
        {
            string sql = "select * from Ref.FareControl";
            SqlConnection conn = new SqlConnection(pbConnectionString);
            DataSet ds = new DataSet();
            SqlDataAdapter ada1 = new SqlDataAdapter(sql, conn);

            ada1.Fill(ds);
            conn.Close();
            int ligne = ds.Tables[0].Rows.Count;
            string[,] data = new string[7, ligne];

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

                i++;
            }
            ViewBag.lon = ligne;
            ViewBag.data = data;
            return PartialView();
        }
        public ActionResult LoadSectorValueDiscrepency()
        {
            string dateFrom1 = Request["dateFrom"];
            string dateFrom = ConvertDate(dateFrom1);
            string dateTo1 = Request["dateTo"];
            string dateTo = ConvertDate(dateTo1);
            string sql = "";
            sql = " select sdh.DateofIssue,sdh.AgentNumericCode,sdh.DocumentNumber as TicketNo,sdc.RelatedDocumentNumber as RelatedNo" + Environment.NewLine;
            sql = sql + ",sdc.CouponNumber as CPN,sdc.OriginAirportCityCode,sdc.DestinationAirportCityCode,sdc.ReservationBookingDesignator as RBD" + Environment.NewLine;
            sql = sql + ",f2.PrimeCode,pd.FinalShare,f1.MinSectorVal,f1.MaxSectorVal" + Environment.NewLine;
            sql = sql + "from Pax.SalesDocumentHeader sdh " + Environment.NewLine;
            sql = sql + "join pax.SalesRelatedDocumentInformation srd on sdh.HdrGuid = srd.HdrGuid" + Environment.NewLine;
            sql = sql + "join Pax.SalesDocumentCoupon sdc on srd.RelatedDocumentGuid = sdc.RelatedDocumentGuid" + Environment.NewLine;
            sql = sql + "join pax.ProrationDetail pd on sdc.RelatedDocumentGuid = pd.RelatedDocumentGuid and sdc.CouponNumber = pd.CouponNumber and sdc.CouponStatus = pd.ProrationFlag" + Environment.NewLine;
            sql = sql + "join pax.Farebasis  f2 on f2.FareBasisTicketDesignator = sdc.FareBasisTicketDesignator" + Environment.NewLine;
            sql = sql + "join Ref.FareControl f1  on sdc.OriginCity = f1.OrigCity  and sdc.DestinationCity = f1.DestCity and f1.PrimeCode = f2.PrimeCode" + Environment.NewLine;
            sql = sql + "and sdc.ReservationBookingDesignator = f1.RBD where 1 = 1 " + Environment.NewLine;
            sql = sql + "AND sdh.DateofIssue BETWEEN Convert(date, '" + dateFrom + "', 105) AND Convert(date, '" + dateTo + "', 105)" + Environment.NewLine;
            sql = sql + "order by FinalShare asc" + Environment.NewLine;

            SqlConnection conn = new SqlConnection(pbConnectionString);
            DataSet ds = new DataSet();
            SqlDataAdapter ada1 = new SqlDataAdapter(sql, conn);
            ada1.Fill(ds);
            conn.Close();
            int ligne2 = ds.Tables[0].Rows.Count;
            string[,] data2 = new string[12, ligne2];
            int i = 0;
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                data2[0, i] = dr[ds.Tables[0].Columns[0].ColumnName].ToString();
                data2[1, i] = dr[ds.Tables[0].Columns[1].ColumnName].ToString();
                data2[2, i] = dr[ds.Tables[0].Columns[2].ColumnName].ToString();
                data2[3, i] = dr[ds.Tables[0].Columns[3].ColumnName].ToString();
                data2[4, i] = dr[ds.Tables[0].Columns[4].ColumnName].ToString();
                data2[5, i] = dr[ds.Tables[0].Columns[5].ColumnName].ToString();
                data2[6, i] = dr[ds.Tables[0].Columns[6].ColumnName].ToString();
                data2[7, i] = dr[ds.Tables[0].Columns[7].ColumnName].ToString();
                data2[8, i] = dr[ds.Tables[0].Columns[8].ColumnName].ToString();
                data2[9, i] = dr[ds.Tables[0].Columns[9].ColumnName].ToString();
                data2[10, i] = dr[ds.Tables[0].Columns[10].ColumnName].ToString();
                data2[11, i] = dr[ds.Tables[0].Columns[11].ColumnName].ToString();
                i++;
            }
            ViewBag.lon2 = ligne2;
            ViewBag.data2 = data2;
           
            return PartialView();
}

    /*    public string ConvertDate(string date)
        {

            string mydate = DateTime.ParseExact(date, "dd-MMM-yyyy", CultureInfo.InvariantCulture).ToString("MM-dd-yyyy");
            return mydate;
        }
        */
        public ActionResult SaveNewSectorValueControl()
        {
            string origincity = Request["origincity"].ToUpper();
            string destinationcity = Request["destinationcity"].ToUpper();
            string primecode = Request["primecode"].ToUpper();
            string rbd = Request["rbd"].ToUpper();
            string minsharevalue = Request["minsharevalue"];
            string maxsharevalue = Request["maxsharevalue"];
            string validation = Request["validation"];
            string sql = "";
            sql = sql + "INSERT INTO [ref].[FareControl] Values ( '" + origincity + "'";
            sql = sql + ",'" + destinationcity + "',";
            sql = sql + " '" + primecode + "',";
            sql = sql + " '" + rbd + "',";
            sql = sql + " '" + minsharevalue + "',";
            sql = sql + " '" + maxsharevalue + "',";
            sql = sql + " '" + validation + "')";
            DataSet ds = new DataSet();
            ds = dbconnect.RetObjDS(sql);
            ViewBag.orgcity = origincity;
            ViewBag.destcity = destinationcity;
            ViewBag.prm = primecode;
            ViewBag.rbd = rbd;
            ViewBag.minshare = minsharevalue;
            ViewBag.maxshare = maxsharevalue;
            ViewBag.val = validation;


            return PartialView("SaveNewSectorValueControl");
        }

        /************************ endSML SVC *******************/

        public ActionResult DataSplitManagment()
        {
            string dateFrom = DateTime.Now.AddMonths(-1).ToString("dd-MMM-yyyy");
            string dateTo = DateTime.Now.ToString("dd-MMM-yyyy");
            string[] date = new string[2] { dateFrom, dateTo };
            ViewBag.date = date;


            return PartialView();
        }

        /* Function  Fare Query Screen  Joseph */

        public ActionResult FareAuditEngine()
        {
            getSectorFrom();
            getSectorTo();

            functionInput();

            return PartialView();
        }

        // get Item SectorFrom
        public void getSectorFrom()
        {
         
            string sql = "SELECT Distinct SECTORFROM as fare from [Ref].[CltFareTable]";

            SqlConnection con = new SqlConnection(pbConnectionString);
            DataSet ds = new DataSet();
            SqlCommand cmd = new SqlCommand("" + sql + "", con);

            SqlDataAdapter ada = new SqlDataAdapter(cmd);

            ada.Fill(ds);

            int lon = ds.Tables[0].Rows.Count;

            string[,] ItemFrom = new string[2, lon];

            int i = 0;

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                ItemFrom[0, i] = dr[ds.Tables[0].Columns[0].ColumnName].ToString();
                
                i++;
            }

            ViewBag.ItemFrom = ItemFrom;
            ViewBag.lon = lon;
        }


        // get Item SectorTo
        public void getSectorTo()
        {

            string sql = "SELECT Distinct SECTORTO as fare  from [Ref].[CltFareTable] ";

            SqlConnection con = new SqlConnection(pbConnectionString);
            DataSet ds = new DataSet();
            SqlCommand cmd = new SqlCommand("" + sql + "", con);

            SqlDataAdapter ada = new SqlDataAdapter(cmd);

            ada.Fill(ds);

            int lonTo = ds.Tables[0].Rows.Count;

            string[,] ItemTo = new string[2, lonTo];

            int i = 0;

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                ItemTo[0, i] = dr[ds.Tables[0].Columns[0].ColumnName].ToString();
                
                i++;
            }

            ViewBag.ItemTo = ItemTo;
            ViewBag.lonTo = lonTo;
        }


        public ActionResult ViewFareBySector()
        {
            getSectorFrom();
            getSectorTo();

            string sectorFrom = Request["from"];
            string sectorTo = Request["to"];

            farebySector(sectorFrom, sectorTo);

            return PartialView();
        }


        public void farebySector(string sectorFrom, string sectorTo)
        {
            string sql = "";
            //sql = sql + "SELECT * from [ref].[fare] where left([fare_component],3) = '" + cmbsecf.Text + "' ";
            //sql = sql + "SELECT left([fare_component],6) as fare_component,NUC,[Rule],MPM,TPM,farebasis,EMA,fare into #temp_table_name6 from ref.fare where fare_component <> 'UNSPECIFIED' and fare_component <> 'AMS' and fare_component <> 'FRA'  order by fare_component; ";
            sql = sql + "SELECT ";
            sql = sql + "[SECTORFROM] + '-' + [SECTORTO] as [Fare Component],[NUC],[RULE],[MPM], f2.TMP  ,[FAREBASIS] As [Fare Basis] ,[ONEWAYROUNDTRIP] as [OW/RT],[CURRENCYCODE] AS[Currency Code] ,[LOCALFARECURRENCY] As [Local Fare Currency],[Carrier],[GlobalIndicator] As [Global Indicator]";
            sql = sql + " ,[ReservationBookingDesignator] AS RBD ,[SalesValidityFrom]  as [Sales Validity From] ,[SalesValidityTo] as [Sales Validity To] ,[FlownValidityFrom] as [Flown Validity From] ,[FlownValidityTo] as [Flown Validity To] ,[PrimeCode] as [Prime Code] ,[SeasonalCode] as [Seasonal Code]";
            sql = sql + ",[PartOfWeekCode]  as [Part Of Week Code] ,[PartOfDayCode] as [Part Of Day Code] ,[Fare_PassengerTypeCode] as [Fare & Passenger Type Code] ,[FareLevelIdentifier] as [Fare Level Identifier] from [Ref].[CltFareTable] f1 ";
            sql = sql + " left join ref.tpm f2 on (f1.SECTORFROM= f2.Orig and f1.SECTORTO=f2.Dest) or  (f1.SECTORFROM= f2.Dest and f1.SECTORTO=f2.Orig) ";
            sql = sql + "where SECTORFROM = '" + sectorFrom + "' and SECTORTO = '" + sectorTo + "'";
            sql = sql + " AND BatchSeq in(Select Max(BatchSeq) from [Ref].[CltFareTable] where SECTORFROM = '" + sectorFrom + "' and SECTORTO = '" + sectorTo + "' );";

            SqlConnection con = new SqlConnection(pbConnectionString);
            DataSet ds = new DataSet();
            SqlCommand cmd = new SqlCommand("" + sql + "", con);

            SqlDataAdapter ada = new SqlDataAdapter(cmd);

            ada.Fill(ds);

            con.Close();

            int ligne2 = ds.Tables[0].Rows.Count;

            string[,] data2 = new string[26, ligne2];
            int i = 0;
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                data2[0, i] = dr[ds.Tables[0].Columns[0].ColumnName].ToString();
                data2[1, i] = dr[ds.Tables[0].Columns[1].ColumnName].ToString();
                data2[2, i] = dr[ds.Tables[0].Columns[2].ColumnName].ToString();
                data2[3, i] = dr[ds.Tables[0].Columns[3].ColumnName].ToString();
                data2[4, i] = dr[ds.Tables[0].Columns[4].ColumnName].ToString();
                data2[5, i] = dr[ds.Tables[0].Columns[5].ColumnName].ToString();
                data2[6, i] = dr[ds.Tables[0].Columns[6].ColumnName].ToString();
                data2[7, i] = dr[ds.Tables[0].Columns[7].ColumnName].ToString();
                data2[8, i] = dr[ds.Tables[0].Columns[8].ColumnName].ToString();
                data2[9, i] = dr[ds.Tables[0].Columns[9].ColumnName].ToString();
                data2[10, i] = dr[ds.Tables[0].Columns[10].ColumnName].ToString();
                data2[11, i] = dr[ds.Tables[0].Columns[11].ColumnName].ToString();
                data2[12, i] = dr[ds.Tables[0].Columns[12].ColumnName].ToString();
                data2[13, i] = dr[ds.Tables[0].Columns[13].ColumnName].ToString();
                data2[14, i] = dr[ds.Tables[0].Columns[14].ColumnName].ToString();
                data2[15, i] = dr[ds.Tables[0].Columns[15].ColumnName].ToString();
                data2[16, i] = dr[ds.Tables[0].Columns[16].ColumnName].ToString();
                data2[17, i] = dr[ds.Tables[0].Columns[17].ColumnName].ToString();
                data2[18, i] = dr[ds.Tables[0].Columns[18].ColumnName].ToString();
                data2[19, i] = dr[ds.Tables[0].Columns[19].ColumnName].ToString();
                data2[20, i] = dr[ds.Tables[0].Columns[20].ColumnName].ToString();
                data2[21, i] = dr[ds.Tables[0].Columns[21].ColumnName].ToString();

                i++;
            }
            ViewBag.lonData = ligne2;
            ViewBag.data2 = data2;

            ViewBag.sectorFrom = sectorFrom;
            ViewBag.sectorTo = sectorTo;
        }

        public ActionResult FareByComponent()
        {
            getSectorFrom();
            getSectorTo();

            string getView = Request["idStatusView"];

            if(getView == "1")
            {
                ViewFareEntries();
            }

            getfarebyComponent();

            return PartialView("ViewFareBySector");
        }


        public void getfarebyComponent()
        {
            string sql = "SELECT * from [Ref].[CltFareTable] ";

            SqlConnection con = new SqlConnection(pbConnectionString);
            DataSet ds = new DataSet();
            SqlCommand cmd = new SqlCommand("" + sql + "", con);

            SqlDataAdapter ada = new SqlDataAdapter(cmd);

            ada.Fill(ds);

            con.Close();

            int ligne2 = ds.Tables[0].Rows.Count;

            string[,] data2 = new string[26, ligne2];
            int i = 0;
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                data2[0, i] = dr[ds.Tables[0].Columns[0].ColumnName].ToString();
                data2[1, i] = dr[ds.Tables[0].Columns[1].ColumnName].ToString();
                data2[2, i] = dr[ds.Tables[0].Columns[2].ColumnName].ToString();
                data2[3, i] = dr[ds.Tables[0].Columns[3].ColumnName].ToString();
                data2[4, i] = dr[ds.Tables[0].Columns[4].ColumnName].ToString();
                data2[5, i] = dr[ds.Tables[0].Columns[5].ColumnName].ToString();
                data2[6, i] = dr[ds.Tables[0].Columns[6].ColumnName].ToString();
                data2[7, i] = dr[ds.Tables[0].Columns[7].ColumnName].ToString();
                data2[8, i] = dr[ds.Tables[0].Columns[8].ColumnName].ToString();
                data2[9, i] = dr[ds.Tables[0].Columns[9].ColumnName].ToString();
                data2[10, i] = dr[ds.Tables[0].Columns[10].ColumnName].ToString();
                data2[11, i] = dr[ds.Tables[0].Columns[11].ColumnName].ToString();
                data2[12, i] = dr[ds.Tables[0].Columns[12].ColumnName].ToString();
                data2[13, i] = dr[ds.Tables[0].Columns[13].ColumnName].ToString();
                data2[14, i] = dr[ds.Tables[0].Columns[14].ColumnName].ToString();
                data2[15, i] = dr[ds.Tables[0].Columns[15].ColumnName].ToString();
                data2[16, i] = dr[ds.Tables[0].Columns[16].ColumnName].ToString();
                data2[17, i] = dr[ds.Tables[0].Columns[17].ColumnName].ToString();
                data2[18, i] = dr[ds.Tables[0].Columns[18].ColumnName].ToString();
                data2[19, i] = dr[ds.Tables[0].Columns[19].ColumnName].ToString();
                data2[20, i] = dr[ds.Tables[0].Columns[20].ColumnName].ToString();
                data2[21, i] = dr[ds.Tables[0].Columns[21].ColumnName].ToString();
                data2[22, i] = dr[ds.Tables[0].Columns[22].ColumnName].ToString();
                data2[23, i] = dr[ds.Tables[0].Columns[23].ColumnName].ToString();
                data2[24, i] = dr[ds.Tables[0].Columns[24].ColumnName].ToString();
                data2[25, i] = dr[ds.Tables[0].Columns[25].ColumnName].ToString();


                i++;
            }
            ViewBag.lonData = ligne2;
            ViewBag.data2 = data2;

            ViewBag.IdFareComp = "1";
        }


        public ActionResult ViewFareEntries()
        {

            getSectorFrom();
            getSectorTo();

            string sectorFrom = Request["from"];
            string sectorTo = Request["to"];
            string status = Request["idStatus"];

            farebySector(sectorFrom, sectorTo);

            if(status == "1")
            {
                getfarebyComponent();
            }

            requeteComponent();

            return PartialView("ViewFareBySector");
        }
        public void requeteComponent()
        {
            string sql = "SELECT ";
            sql = "SELECT ";
            sql = sql + "[SECTORFROM] + '-' + [SECTORTO] as [Fare Component],[NUC],[RULE],[MPM], f2.TMP  ,[FAREBASIS] As [Fare Basis] ,[ONEWAYROUNDTRIP] as [OW/RT],[CURRENCYCODE] AS[Currency Code] ,[LOCALFARECURRENCY] As [Local Fare Currency],[Carrier],[GlobalIndicator] As [Global Indicator]";
            sql = sql + " ,[ReservationBookingDesignator] AS RBD ,[SalesValidityFrom]  as [Sales Validity From] ,[SalesValidityTo] as [Sales Validity To] ,[FlownValidityFrom] as [Flown Validity From] ,[FlownValidityTo] as [Flown Validity To] ,[PrimeCode] as [Prime Code] ,[SeasonalCode] as [Seasonal Code]";
            sql = sql + ",[PartOfWeekCode]  as [Part Of Week Code] ,[PartOfDayCode] as [Part Of Day Code] ,[Fare_PassengerTypeCode] as [Fare & Passenger Type Code] ,[FareLevelIdentifier] as [Fare Level Identifier] from [Ref].[CltFareTable] f1 ";
            sql = sql + " left join ref.tpm f2 on (f1.SECTORFROM= f2.Orig and f1.SECTORTO=f2.Dest) or  (f1.SECTORFROM= f2.Dest and f1.SECTORTO=f2.Orig) ";
            //  sql = sql + "where SECTORFROM = '" + cmbsecf.Text + "' and SECTORTO = '" + cmbsect.Text + "'";
            // sql = sql + " AND BatchSeq in(Select Max(BatchSeq) from [Ref].[CltFareTable] where SECTORFROM = '" + cmbsecf.Text + "' and SECTORTO = '" + cmbsect.Text + "' );";

            sql = sql + " Order By [SECTORFROM] , [SECTORTO] ASC ";

            SqlConnection con = new SqlConnection(pbConnectionString);
            DataSet ds = new DataSet();
            SqlCommand cmd = new SqlCommand("" + sql + "", con);

            SqlDataAdapter ada = new SqlDataAdapter(cmd);

            ada.Fill(ds);

            con.Close();

            int ligne2 = ds.Tables[0].Rows.Count;

            string[,] dataView = new string[26, ligne2];
            int i = 0;
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                dataView[0, i] = dr[ds.Tables[0].Columns[0].ColumnName].ToString();
                dataView[1, i] = dr[ds.Tables[0].Columns[1].ColumnName].ToString();
                dataView[2, i] = dr[ds.Tables[0].Columns[2].ColumnName].ToString();
                dataView[3, i] = dr[ds.Tables[0].Columns[3].ColumnName].ToString();
                dataView[4, i] = dr[ds.Tables[0].Columns[4].ColumnName].ToString();
                dataView[5, i] = dr[ds.Tables[0].Columns[5].ColumnName].ToString();
                dataView[6, i] = dr[ds.Tables[0].Columns[6].ColumnName].ToString();
                dataView[7, i] = dr[ds.Tables[0].Columns[7].ColumnName].ToString();
                dataView[8, i] = dr[ds.Tables[0].Columns[8].ColumnName].ToString();
                dataView[9, i] = dr[ds.Tables[0].Columns[9].ColumnName].ToString();
                dataView[10, i] = dr[ds.Tables[0].Columns[10].ColumnName].ToString();
                dataView[11, i] = dr[ds.Tables[0].Columns[11].ColumnName].ToString();
                dataView[12, i] = dr[ds.Tables[0].Columns[12].ColumnName].ToString();
                dataView[13, i] = dr[ds.Tables[0].Columns[13].ColumnName].ToString();
                dataView[14, i] = dr[ds.Tables[0].Columns[14].ColumnName].ToString();
                dataView[15, i] = dr[ds.Tables[0].Columns[15].ColumnName].ToString();
                dataView[16, i] = dr[ds.Tables[0].Columns[16].ColumnName].ToString();
                dataView[17, i] = dr[ds.Tables[0].Columns[17].ColumnName].ToString();
                dataView[18, i] = dr[ds.Tables[0].Columns[18].ColumnName].ToString();
                dataView[19, i] = dr[ds.Tables[0].Columns[19].ColumnName].ToString();
                dataView[20, i] = dr[ds.Tables[0].Columns[20].ColumnName].ToString();
                dataView[21, i] = dr[ds.Tables[0].Columns[21].ColumnName].ToString();

                i++;
            }
            ViewBag.lonDataView = ligne2;
            ViewBag.dataView = dataView;

            ViewBag.IdViewFare = "1";
        }


        public ActionResult ClearViewFare()
        {
            getSectorFrom();
            getSectorTo();

            ViewBag.lonDataView = 0;
            ViewBag.dataView = "";

            string sectorFrom = Request["from"];
            string sectorTo = Request["to"];
            string status = Request["idStatus"];

           
            ViewBag.sectorFrom = sectorFrom;
            ViewBag.sectorTo = sectorTo;

            farebySector(sectorFrom, sectorTo);

            if (status == "1")
            {
                getfarebyComponent();
            }

            ViewBag.IdViewFare = "";

            return PartialView("ViewFareBySector");
        }

        public ActionResult ClearFareComponent()
        {
            string sectorFrom = Request["from"];
            string sectorTo = Request["to"];
            string status = Request["idStatus"];

            getSectorFrom();
            getSectorTo();

            ViewBag.lonData = 0;
            ViewBag.data = "";

            ViewBag.IdFareComp = "";

            if (status == "1")
            {
                requeteComponent();
            }

            return PartialView("ViewFareBySector");
        }

        /* End  Function  Fare Query Screen  Joseph */

        /* Fare Inpute Screen    Joseph*/

        public ActionResult saveInputeScreen()
        {

            string fareIsn = Request["valfareIsn"];
            string rule = Request["valrule"];
            string primeCode = Request["valprimeCode"];
            string carrierCode = Request["valcarrierCode"];
            string mpm = Request["valmpm"];
            string seasonal = Request["valseasonal"];
            string sectorFrom = Request["valsectorFrom"];
            string gi = Request["valgi"];
            string partofWeek = Request["valpartofWeek"];
            string sectorTo = Request["valsectorTo"];
            string rbd = Request["valrbd"];
            string partOfDayCode = Request["valpartOfDayCode"];
            string fareBasis = Request["valfareBasis"];
            string salesValidityFrom = Request["valsalesValidityFrom"];
            string fptc = Request["valfptc"];
            string journeyType = Request["valjourneyType"];
            string salesValidityTo = Request["valsalesValidityTo"];
            string fareLevelId = Request["valfareLevelId"];
            string currencyCode = Request["valcurrencyCode"];
            string localCurrencyFare = Request["vallocalCurrencyFare"];
            string nuc = Request["valnuc"];
            string flownValidityFrom = Request["valflownValidityFrom"];
            string flownValidityTo = Request["valflownValidityTo"];


            string date = DateTime.Now.ToShortDateString();
            string da = Convert.ToDateTime(date).ToString("dd-MMM-yyyy");

            //string date = DateTime.Now.ToShortDateString();
            string UploadDate = Convert.ToDateTime(date).ToString("dd-MMM-yyyy");

            string BatchSeq = "";
            string FName = "Manual " + UploadDate;

            string SqlBatchSeq = "SELECT iif(MAX(BatchSeq) is null,1, MAX(BatchSeq)) As MaxBatchSeq  FROM [Ref].[CltFareTable]";
            BatchSeq = GetSqlValues(SqlBatchSeq).Replace("|", "");


            string Id = "SELECT iif(MAX(ID) is null,1, MAX(ID)+1) As MaxBatchSeq  FROM [Ref].[CltFareTable] where BatchSeq=" + BatchSeq;
            Id = GetSqlValues(Id).Replace("|", "");
            fareIsn = Id;


            string Sql = "";
            Sql = "Insert into [Ref].[CltFareTable]  (ID,Carrier,SectorFrom,SectorTo,FareBasis,OneWayRoundTrip,CurrencyCode,localFareCurrency,";
            Sql = Sql + "NUC,[RULE],MPM,GlobalIndicator,ReservationBookingDesignator,SalesValidityFrom,SalesValidityTo,FlownValidityFrom,FlownValidityTo,PrimeCode,";
            Sql = Sql + "SeasonalCode,PartOfWeekCode,PartOfDayCode,Fare_PassengerTypeCode,FareLevelIdentifier,[BatchSeq],[DateUploaded],[DataFileRef])";
            Sql = Sql + " VALUES (";

            Sql = Sql + fareIsn + ",'";
            Sql = Sql + carrierCode + "','";
            Sql = Sql + sectorFrom + "','";
            Sql = Sql + sectorTo + "','";
            Sql = Sql + fareBasis + "','";
            Sql = Sql + journeyType + "','";
            Sql = Sql + currencyCode + "',";
            Sql = Sql + localCurrencyFare + ",";
            Sql = Sql + nuc + ",'";
            Sql = Sql + rule + "',";
            Sql = Sql + mpm + ",'";
            Sql = Sql + gi + "','";
            Sql = Sql + rbd + "','";
            Sql = Sql + salesValidityFrom + "','";
            Sql = Sql + salesValidityTo + "','";
            Sql = Sql + flownValidityFrom + "','";
            Sql = Sql + flownValidityTo + "','";
            Sql = Sql + primeCode + "','";
            Sql = Sql + seasonal + "','";
            Sql = Sql + partofWeek + "','";
            Sql = Sql + partOfDayCode + "','";
            Sql = Sql + fptc + "','";
            Sql = Sql + fareLevelId + "',";
            Sql = Sql + BatchSeq + ",";
            Sql = Sql + "'" + UploadDate + "',";
            Sql = Sql + "'" + FName + "')";

            DbUpdate(Sql);

            SqlBatchSeq = "SELECT iif(MAX(BatchSeq) is null,1, MAX(BatchSeq)) As MaxBatchSeq  FROM [Ref].[CltFareTable]";
            BatchSeq = GetSqlValues(SqlBatchSeq).Replace("|", "");

            Id = "SELECT iif(MAX(ID) is null,1, MAX(ID)) As MaxBatchSeq  FROM [Ref].[CltFareTable]  where BatchSeq =" + BatchSeq;
            Id = GetSqlValues(Id).Replace("|", "");
            ViewBag.fareIsn = Id;


            Id = "SELECT iif(MAX(ID) is null,1, MAX(ID)+1) As MaxBatchSeq  FROM [Ref].[CltFareTable] where left(DataFileRef,6)= 'Manual'";
            Id = GetSqlValues(Id).Replace("|", "");
            ViewBag.fareIsn = Id;

           // MessageBox.Show("Record Successfully Saved", "Symphony", MessageBoxButtons.OK, MessageBoxIcon.Information);

           // conn.Close();

            return PartialView();
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

        string GetSqlValues(string Sql)
        {
            string retVal = "";
            SqlDataReader myReader;
            int R = 0;
            try
            {
                myReader = GetlistofTables(Sql);

                if (myReader != null)
                {
                    if (myReader.HasRows == true)
                    {
                        while (myReader.Read())
                        {
                            for (int col = 0; col < myReader.FieldCount; col++)
                            {
                                retVal = retVal + myReader.GetValue(col).ToString() + "|";
                            }
                            R++;
                        }
                    }
                }
                // always call Close when done reading.
                myReader.Close();
                myReader.Dispose();
                myReader = null;
                // Close the connection when done with it.
            }
            catch
            {
                int err = 0;
            }

            return retVal;
        }

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
                    // stay here until query is not completed
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

        public void functionInput()
        {

            string BatchSeq = "";
            string SqlBatchSeq = "SELECT iif(MAX(BatchSeq) is null,1, MAX(BatchSeq)) As MaxBatchSeq  FROM [Ref].[CltFareTable]";
            BatchSeq = GetSqlValues(SqlBatchSeq).Replace("|", "");

            string Id = "SELECT iif(MAX(ID) is null,1, MAX(ID)) As MaxBatchSeq  FROM [Ref].[CltFareTable] where BatchSeq=" + BatchSeq;
            Id = GetSqlValues(Id).Replace("|", "");

            ViewBag.fareIsn = Id;
        }

        /* End Fare Inpute Screen   Joseph*/



        /*    Data splite Management onglet Lift   Josesph */

        public ActionResult DataSpliteLift()
        {
            return PartialView();
        }

        public ActionResult LiftData()
        {

            DateTime dtpFromSales = DateTime.Parse(Request["valfrom"]);
            DateTime dtpToSales = DateTime.Parse(Request["valto"]);
            string se = Request["paramSe"];

            string oal = "%";
            string Lift = "1";            
            string page = "1";              
            string record = "150";
           
            SqlConnection con = new SqlConnection(pbConnectionString);
            DataSet ds = new DataSet();

            SqlCommand cmd = new SqlCommand("[Pax].[SP_DataSplitEngine_SALES]", con);
            cmd.CommandTimeout = 1000;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@IssueDate_from", dtpFromSales);
            cmd.Parameters.AddWithValue("@IssueDate_to", dtpToSales);
            cmd.Parameters.AddWithValue("@Carrier", oal);
            cmd.Parameters.AddWithValue("@Interlining", se);
            cmd.Parameters.AddWithValue("@Lift", Lift);
            cmd.Parameters.AddWithValue("@PageNo", page);
            cmd.Parameters.AddWithValue("@RecordsPerPage", record);

            int i = 0;

            SqlDataAdapter ada = new SqlDataAdapter(cmd);
            ada.Fill(ds);

            int lon = ds.Tables[0].Rows.Count;

            string[,] dgLift = new string[36, lon];

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                dgLift[0, i] = dr[ds.Tables[0].Columns[0].ColumnName].ToString();
                dgLift[1, i] = dr[ds.Tables[0].Columns[1].ColumnName].ToString();
                dgLift[2, i] = dr[ds.Tables[0].Columns[2].ColumnName].ToString();
                dgLift[3, i] = dr[ds.Tables[0].Columns[3].ColumnName].ToString();
                dgLift[4, i] = dr[ds.Tables[0].Columns[4].ColumnName].ToString();
                dgLift[5, i] = dr[ds.Tables[0].Columns[5].ColumnName].ToString();
                dgLift[6, i] = dr[ds.Tables[0].Columns[6].ColumnName].ToString();
                dgLift[7, i] = dr[ds.Tables[0].Columns[7].ColumnName].ToString();
                dgLift[8, i] = dr[ds.Tables[0].Columns[8].ColumnName].ToString();

                dgLift[9, i] = dr[ds.Tables[0].Columns[9].ColumnName].ToString();

                if (dgLift[9, i] != null && !string.IsNullOrWhiteSpace(dgLift[9, i].ToString()))
                {
                    if (dgLift[9, i].ToString() == "O")
                    {
                        dgLift[9, i] = "OB";
                    }
                    else
                        if (dgLift[9, i].ToString() == "I")
                    {
                        dgLift[9, i] = "IB";
                    }
                }


                dgLift[10, i] = dr[ds.Tables[0].Columns[10].ColumnName].ToString();
                dgLift[11, i] = dr[ds.Tables[0].Columns[11].ColumnName].ToString();
                dgLift[12, i] = dr[ds.Tables[0].Columns[12].ColumnName].ToString();
                dgLift[13, i] = dr[ds.Tables[0].Columns[13].ColumnName].ToString();
                dgLift[14, i] = dr[ds.Tables[0].Columns[14].ColumnName].ToString();
                dgLift[15, i] = dr[ds.Tables[0].Columns[15].ColumnName].ToString();
                dgLift[16, i] = dr[ds.Tables[0].Columns[16].ColumnName].ToString();
                dgLift[17, i] = dr[ds.Tables[0].Columns[17].ColumnName].ToString();
                dgLift[18, i] = dr[ds.Tables[0].Columns[18].ColumnName].ToString();
                dgLift[19, i] = dr[ds.Tables[0].Columns[19].ColumnName].ToString();
                dgLift[20, i] = dr[ds.Tables[0].Columns[20].ColumnName].ToString();
                dgLift[21, i] = dr[ds.Tables[0].Columns[21].ColumnName].ToString();
                dgLift[22, i] = dr[ds.Tables[0].Columns[22].ColumnName].ToString();
                dgLift[23, i] = dr[ds.Tables[0].Columns[23].ColumnName].ToString();
                dgLift[24, i] = dr[ds.Tables[0].Columns[24].ColumnName].ToString();
                dgLift[25, i] = dr[ds.Tables[0].Columns[25].ColumnName].ToString();
                dgLift[26, i] = dr[ds.Tables[0].Columns[26].ColumnName].ToString();
                dgLift[27, i] = dr[ds.Tables[0].Columns[27].ColumnName].ToString();
                dgLift[28, i] = dr[ds.Tables[0].Columns[28].ColumnName].ToString();
                dgLift[29, i] = dr[ds.Tables[0].Columns[29].ColumnName].ToString();
                dgLift[30, i] = dr[ds.Tables[0].Columns[30].ColumnName].ToString();
                dgLift[31, i] = dr[ds.Tables[0].Columns[31].ColumnName].ToString();
                dgLift[32, i] = dr[ds.Tables[0].Columns[32].ColumnName].ToString();
                dgLift[33, i] = dr[ds.Tables[0].Columns[33].ColumnName].ToString();
                dgLift[34, i] = dr[ds.Tables[0].Columns[34].ColumnName].ToString();
                dgLift[35, i] = dr[ds.Tables[0].Columns[35].ColumnName].ToString();
                

                i++;
            }

            con.Close();

            ViewBag.dgLift = dgLift;
            ViewBag.nombre = lon;


            ViewBag.dateFrom = dtpFromSales;
            ViewBag.dateTo = dtpToSales;

            ViewBag.type = "TKTT";

            return PartialView();

        }



        //christian

        public ActionResult SalesData()
        {

            DateTime dtpFromSales = DateTime.Parse(Request["valfrom"]);
            DateTime dtpToSales = DateTime.Parse(Request["valto"]);
            string se = Request["paramSe"];
            string selectinterline = Request["selectinterline"];

            string oal = "";
            string sales = "0";
            string page = "1";
            string record = "150";

            if (selectinterline == "-All-")
            {
                oal = "%";
            }
            else
            {
                oal = selectinterline;
            }

            SqlConnection con = new SqlConnection(pbConnectionString);
            DataSet ds = new DataSet();

            SqlCommand cmd = new SqlCommand("[Pax].[SP_DataSplitEngine_SALES]", con);
            cmd.CommandTimeout = 1000;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@IssueDate_from", dtpFromSales);
            cmd.Parameters.AddWithValue("@IssueDate_to", dtpToSales);
            cmd.Parameters.AddWithValue("@Carrier", oal);
            cmd.Parameters.AddWithValue("@Interlining", se);
            cmd.Parameters.AddWithValue("@Lift", sales);
            cmd.Parameters.AddWithValue("@PageNo", page);
            cmd.Parameters.AddWithValue("@RecordsPerPage", record);
            

            int i = 0;

            SqlDataAdapter ada = new SqlDataAdapter(cmd);
            ada.Fill(ds);

            int lon = ds.Tables[0].Rows.Count;

            string[,] dgSales = new string[37, lon];

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                dgSales[0, i] = dr[ds.Tables[0].Columns[0].ColumnName].ToString();
                dgSales[1, i] = dr[ds.Tables[0].Columns[1].ColumnName].ToString();
                dgSales[2, i] = dr[ds.Tables[0].Columns[2].ColumnName].ToString();
                dgSales[3, i] = dr[ds.Tables[0].Columns[3].ColumnName].ToString();
                dgSales[4, i] = dr[ds.Tables[0].Columns[4].ColumnName].ToString();
                dgSales[5, i] = dr[ds.Tables[0].Columns[5].ColumnName].ToString();
                dgSales[6, i] = dr[ds.Tables[0].Columns[6].ColumnName].ToString();
                dgSales[7, i] = dr[ds.Tables[0].Columns[7].ColumnName].ToString();
                dgSales[8, i] = dr[ds.Tables[0].Columns[8].ColumnName].ToString();

                dgSales[9, i] = dr[ds.Tables[0].Columns[9].ColumnName].ToString();

                if (dgSales[9, i] != null && !string.IsNullOrWhiteSpace(dgSales[9, i].ToString()))
                {
                    if (dgSales[9, i].ToString() == "O")
                    {
                        dgSales[9, i] = "OB";
                    }
                    else
                        if (dgSales[9, i].ToString() == "I")
                    {
                        dgSales[9, i] = "IB";
                    }
                }


                dgSales[10, i] = dr[ds.Tables[0].Columns[10].ColumnName].ToString();
                dgSales[11, i] = dr[ds.Tables[0].Columns[11].ColumnName].ToString();
                dgSales[12, i] = dr[ds.Tables[0].Columns[12].ColumnName].ToString();
                dgSales[13, i] = dr[ds.Tables[0].Columns[13].ColumnName].ToString();
                dgSales[14, i] = dr[ds.Tables[0].Columns[14].ColumnName].ToString();
                dgSales[15, i] = dr[ds.Tables[0].Columns[15].ColumnName].ToString();
                dgSales[16, i] = dr[ds.Tables[0].Columns[16].ColumnName].ToString();
                dgSales[17, i] = dr[ds.Tables[0].Columns[17].ColumnName].ToString();
                dgSales[18, i] = dr[ds.Tables[0].Columns[18].ColumnName].ToString();
                dgSales[19, i] = dr[ds.Tables[0].Columns[19].ColumnName].ToString();
                dgSales[20, i] = dr[ds.Tables[0].Columns[20].ColumnName].ToString();
                dgSales[21, i] = dr[ds.Tables[0].Columns[21].ColumnName].ToString();
                dgSales[22, i] = dr[ds.Tables[0].Columns[22].ColumnName].ToString();
                dgSales[23, i] = dr[ds.Tables[0].Columns[23].ColumnName].ToString();
                dgSales[24, i] = dr[ds.Tables[0].Columns[24].ColumnName].ToString();
                dgSales[25, i] = dr[ds.Tables[0].Columns[25].ColumnName].ToString();
                dgSales[26, i] = dr[ds.Tables[0].Columns[26].ColumnName].ToString();
                dgSales[27, i] = dr[ds.Tables[0].Columns[27].ColumnName].ToString();
                dgSales[28, i] = dr[ds.Tables[0].Columns[28].ColumnName].ToString();
                dgSales[29, i] = dr[ds.Tables[0].Columns[29].ColumnName].ToString();
                dgSales[30, i] = dr[ds.Tables[0].Columns[30].ColumnName].ToString();
                dgSales[31, i] = dr[ds.Tables[0].Columns[31].ColumnName].ToString();
                dgSales[32, i] = dr[ds.Tables[0].Columns[32].ColumnName].ToString();
                dgSales[33, i] = dr[ds.Tables[0].Columns[33].ColumnName].ToString();
                dgSales[34, i] = dr[ds.Tables[0].Columns[34].ColumnName].ToString();
                dgSales[35, i] = dr[ds.Tables[0].Columns[35].ColumnName].ToString();
                dgSales[36, i] = dr[ds.Tables[0].Columns[36].ColumnName].ToString();

                i++;
            }

            con.Close();

            ViewBag.dgSales = dgSales;
            ViewBag.nombre = lon;


            ViewBag.dateFrom = dtpFromSales;
            ViewBag.dateTo = dtpToSales;

            return PartialView();

        }

        //christian

        /*   End  Data splite Management onglet Lift   Josesph */

        /* Execce Bagage Proration   Joseph */
        public ActionResult ActionBaggageProration()
        {
           
            string fare = Request["valFare"];
            string month = Request["valMonth"];
            string fca = Request["valfca"];
            string dateExcess = Request["valdateExcess"];

            DateTime dtpDateExcess = DateTime.Parse(dateExcess);

            Prorate(fca, dtpDateExcess, month, fare);


            ViewBag.Message = "Bonjour";
            ViewBag.Test = dtpDateExcess;

            return PartialView();
        }


        public void Prorate(string FCA, DateTime DOS, string MOC, string Fare)
        {
            BreakFCA(FCA, DOS);
            FareComponent(FCA);

            /*
            AddTotal();
             Total();
             FareToProrate(Fare, MOC);

           
            FinalShare();
            TotalSRP();
            TotalFinalShare(); */

        }

        /* Declaration string */
        public string _FareComponent = null;
        public string _Totalpf = null;
        public double _ProratedAmount = 0;
        public double _Quotient = 0;
        public string _FDR = null;



        public void BreakFCA(string FCA, DateTime DOS)
        {
            string Sec = @"(((?!([A-Z]{6}))([A-Z]{2,3}))|(B2)|(9U)|(J2)|(A3)|((//))|((/-)))";

            string x = null;
            string Sector = null;
            string cr = null;

            /* rgvTrip.Rows.Clear();
             int rowindex;


             DataGridViewRow row;*/

            int i = 0;
            string[] y = new string[100];
            string[] z = new string[100];


            foreach (Match matSec in Regex.Matches(FCA, Sec))
            {

                x = matSec.Value.ToString();

                if (x.Length == 3)
                {
                    Sector = x;
                }
                else
                {
                    cr = x;
                }

                y[i] = Sector;

                ViewBag.Valeur1 = i;
            
                if (i > 0)
                {
                   string a = y[i];

                    ViewBag.ValeurA = a;

                   string b = y[(i - 1)];

                    ViewBag.ValeurB = b;

                    /* 
                     var query = from Q in ef.PMPFactors
                                 where Q.OrgCity == b && Q.DestCity == a && ((DOS >= Q.ValidFrom && DOS <= Q.ValidTo) || (DOS == Q.ValidTo))
                                 select Q.Factor; */

                    string sql = "SELECT Factor from ref.PMPFactor where OrgCity = '" + b + "' AND  DestCity = '" + a + "'  AND ((  ValidFrom  <= '" + DOS + "'  AND  ValidTo >= '" + DOS + "' ) OR ( ValidTo = '" + DOS + "' )) ";

                    SqlConnection conp = new SqlConnection(pbConnectionString);
                    DataSet ds = new DataSet();
                    SqlCommand cmd = new SqlCommand("" + sql + "", conp);

                    SqlDataAdapter ada = new SqlDataAdapter(cmd);

                    ada.Fill(ds);

                    int lonFactori = ds.Tables[0].Rows.Count;

                    string[,] ListeFactor = new string[1, lonFactori];

                    int iTab = 0;

                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        ListeFactor[0, iTab] = dr[ds.Tables[0].Columns[0].ColumnName].ToString();

                        ViewBag.sector = Sector;

                        iTab++;
                    }

                    ViewBag.lonFactori = lonFactori;
                    ViewBag.ListeFactor = ListeFactor;


                    /*
                    foreach (var el in query)
                    {
                        rowindex = rgvTrip.Rows.Add();
                        row = rgvTrip.Rows[rowindex];
                       row.Cells[0].Value = Sector;
                        row.Cells[2].Value = el;
                        row.Cells[1].Value = cr; 

                        ViewBag.sector = Sector;
                        ViewBag.tab2 = el;
                        ViewBag.tab1 = cr;
                   } */


                }

                else
                {
                    // rowindex = rgvTrip.Rows.Add();
                    //row = rgvTrip.Rows[rowindex];

                    /*  row.Cells[0].Value = Sector;
                      row.Cells[2].Value = "";
                      row.Cells[1].Value = ""; */

                    ViewBag.sector = Sector;
                    ViewBag.tab2 = "valeur 2 a";
                    ViewBag.tab1 = "valeur 3"; 
                }

                i++;

            }

        }

        public void FareComponent(string FCA)
        {
            string Sec = @"(((?!([A-Z]{6}))([A-Z]{2,3}))|(B2)|(9U)|(J2)|(A3)|((//))|((/-)))";

            string x = null;

            int i = 0;
            string[] y = new string[100];

            foreach (Match matSec in Regex.Matches(FCA, Sec))
            {

                x = matSec.Value.ToString();

                if (x.Length == 3)
                {

                    y[i] = x;
                    i++;

                }

            }

            _FareComponent = y.First() + y[i - 1];

            ViewBag.FareComponent = _FareComponent;
        }


        public void AddTotal()
        {
           /* int i;

            for (i = 0; i < 11; i++)
            {
                if (rgvTrip.Rows[i].Cells[0].Value == null || string.IsNullOrWhiteSpace(rgvTrip.Rows[i].Cells[0].Value.ToString()))
                {
                    rgvTrip.Rows.Add("");
                }

            }*/
        }

        public void Total()
        {
            /*
            int i;
            double sum = 0;
            try
            {

              //  rgvTrip.Rows[10].Cells[0].Value = "Total";

                ViewBag.Tab10 = "Total";

                for (i = 1; i < 10; i++)
                {
                    if (rgvTrip.Rows[i].Cells[0].Value != null && !string.IsNullOrWhiteSpace(rgvTrip.Rows[i].Cells[0].Value.ToString()) && rgvTrip.Rows[i].Cells[0].Value != "")
                    {
                        sum += Convert.ToDouble(rgvTrip.Rows[i].Cells[2].Value);
                    }

                }

                rgvTrip.Rows[10].Cells[2].Value = sum;
                _Totalpf = Convert.ToString(sum);
            }
            catch
            {
            }*/
        }

        public void FareToProrate(string Fare, string MOC)
        {
            /*
            string curr = null;
            string amount = null;
            string f = @"([A-Z]{3})";

            Match matchf = Regex.Match(Fare, f);

            if (matchf.Success)
            {
                curr = matchf.Value;
            }

            string amt = @"((([0-9])+(\.)([0-9]{2}))|(([0-9]){1,10}))";

            Match matchamt = Regex.Match(Fare, amt);

            if (matchamt.Success)
            {
                amount = matchamt.Value;
            }

            ConverttoUSD(curr, amount, MOC);

    */
    }


            /*Free Baggage Allowan     Joseph*/
          public ActionResult FreeBaggageAllowanceAudit()
        {

            getMappedCode();

            return PartialView();
        }


        // function get Mapped Prime Code  Joseph

        public void getMappedCode()
        {

            string sql = "";

            sql = sql + "select distinct [MappedPrimeCode] from [Ref].[FreeBaggageAllowance]";

            SqlConnection con = new SqlConnection(pbConnectionString);
            DataSet ds = new DataSet();
            SqlCommand cmd = new SqlCommand("" + sql + "", con);


            SqlDataAdapter ada = new SqlDataAdapter(cmd);

            ada.Fill(ds);

            int logMapped = ds.Tables[0].Rows.Count;

            string[,] ItemMapped = new string[1, logMapped];

            int i = 0;

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                ItemMapped[0, i] = dr[ds.Tables[0].Columns[0].ColumnName].ToString();

                i++;
            }

            ViewBag.logMapped = logMapped;
            ViewBag.ItemMapped = ItemMapped;

    }


        // function Display Grid Free Baggage  JOSEPH

        private void DisplayGrid(string cboMPC)
        {

            string h = "%";

            if (cboMPC != "-All-")
            {
                h = cboMPC;
            }

            string sql = "";

            sql = sql + "with Coupon as" + Environment.NewLine;
            sql = sql + "(" + Environment.NewLine;
            sql = sql + "select f1.RelatedDocumentGuid, f1.RelatedDocumentNumber,f1.CouponNumber" + Environment.NewLine;
            sql = sql + " , f1.carrier, f1.FareBasisTicketDesignator, f1.ReservationBookingDesignator" + Environment.NewLine;
            sql = sql + ", Pax.GetMappedPrimeCode( f1.FareBasisTicketDesignator ) as MappedPrimeCode, FreeBaggageAllowance" + Environment.NewLine;
            sql = sql + ", try_cast( Adm.ExtractNumberFromString(FreeBaggageAllowance) as int" + Environment.NewLine;
            sql = sql + ") as FreeBaggageAllowanceQty" + Environment.NewLine;
            sql = sql + "from Pax.SalesDocumentCoupon f1" + Environment.NewLine;
            sql = sql + ")" + Environment.NewLine;
            sql = sql + "select f1.RelatedDocumentNumber,f1.CouponNumber" + Environment.NewLine;
            sql = sql + ", f1.carrier, f1.FareBasisTicketDesignator, f1.ReservationBookingDesignator" + Environment.NewLine;
            sql = sql + " , Pax.GetMappedPrimeCode( f1.FareBasisTicketDesignator ) as MappedPrimeCode, FreeBaggageAllowance" + Environment.NewLine;
            sql = sql + ", try_cast( Adm.ExtractNumberFromString(FreeBaggageAllowance) as int" + Environment.NewLine;
            sql = sql + ") as FreeBaggageAllowanceQty, f2.Unit, Allowance" + Environment.NewLine;
            sql = sql + "from Coupon f1" + Environment.NewLine;
            sql = sql + "left join Ref.FreeBaggageAllowance f2 on f1.MappedPrimeCode = f2.MappedPrimeCode and ( f2.Unit =  right(FreeBaggageAllowance, 1 ) or f2.Unit =  right( FreeBaggageAllowance, 2) )" + Environment.NewLine;
            sql = sql + "where FreeBaggageAllowanceQty > Allowance and f1.MappedPrimeCode like '" + h + "'" + Environment.NewLine;


            SqlConnection con = new SqlConnection(pbConnectionString);
            DataSet ds = new DataSet();
            SqlCommand cmd = new SqlCommand("" + sql + "", con);


            SqlDataAdapter ada = new SqlDataAdapter(cmd);

            ada.Fill(ds);

            int logDisp = ds.Tables[0].Rows.Count;

            string[,] dgcoupons = new string[10, logDisp];

            int i = 0;



            string[] count = new string[1000000];

            foreach (DataRow dr in ds.Tables[0].Rows)
            {

                count[i] = dr[ds.Tables[0].Columns[0].ColumnName].ToString();

                dgcoupons[0, i] = dr[ds.Tables[0].Columns[0].ColumnName].ToString();
                dgcoupons[1, i] = dr[ds.Tables[0].Columns[1].ColumnName].ToString();
                dgcoupons[2, i] = dr[ds.Tables[0].Columns[2].ColumnName].ToString();
                dgcoupons[3, i] = dr[ds.Tables[0].Columns[3].ColumnName].ToString();
                dgcoupons[4, i] = dr[ds.Tables[0].Columns[4].ColumnName].ToString();
                dgcoupons[5, i] = dr[ds.Tables[0].Columns[5].ColumnName].ToString();
                dgcoupons[6, i] = dr[ds.Tables[0].Columns[7].ColumnName].ToString();
                dgcoupons[7, i] = dr[ds.Tables[0].Columns[8].ColumnName].ToString();
                dgcoupons[8, i] = dr[ds.Tables[0].Columns[9].ColumnName].ToString();

                dgcoupons[9, i] = (Convert.ToDouble(dgcoupons[6, i]) - Convert.ToDouble(dgcoupons[8, i]) ).ToString();

                i++;
            }

            string[] b = count.Distinct().ToArray();

            ViewBag.DocCout = (b.Length - 1).ToString();

            ViewBag.CpnCount = logDisp;

            ViewBag.dgcoupons = dgcoupons;

        }

        /*function Action FreeBaggage Allowance Joseph*/

        public ActionResult ActionFreeBaggage()
        {
            getMappedCode();

            string param = Request["code"];

            if (param == "-All-")
            {
                ViewBag.txtPC = "";
                ViewBag.LB = "";
                ViewBag.K = "";

                DisplayGrid(param);
            }
            else
            {
                DisplayValeur(param);
                DisplayGrid(param);
            }

            ViewBag.valPresetCode = param;

            return PartialView("FreeBaggageAllowanceAudit");
        } 

        private void DisplayValeur(string cboMPC)
        {

            ViewBag.txtPC = "";
            ViewBag.LB = "";
            ViewBag.K = "";

            string sql = "";

            sql = sql + "SELECT  [MappedPrimeCode] ,[Unit] ,[Allowance] FROM [Ref].[FreeBaggageAllowance] where MappedPrimeCode = '" + cboMPC + "'";

            SqlConnection con = new SqlConnection(pbConnectionString);
            DataSet ds = new DataSet();
            SqlCommand cmd = new SqlCommand("" + sql + "", con);


            SqlDataAdapter ada = new SqlDataAdapter(cmd);

            ada.Fill(ds);

            int logDisp = ds.Tables[0].Rows.Count;

            string[,] ItemDis = new string[3, logDisp];

            int i = 0;

            foreach (DataRow dr in ds.Tables[0].Rows)
            {

                string a = dr[ds.Tables[0].Columns[1].ColumnName].ToString();

                if (a == "K")
                {
                    ViewBag.K = dr[ds.Tables[0].Columns[2].ColumnName].ToString();
                }
                else
                  if (a == "LB")
                {
                    ViewBag.LB = dr[ds.Tables[0].Columns[2].ColumnName].ToString();
                }
                else
                      if (a == "PC")
                {
                    ViewBag.txtPC = dr[ds.Tables[0].Columns[2].ColumnName].ToString();
                }

            }

            ViewBag.logDisp = logDisp;

            ViewBag.ItemDis = ItemDis;
        }

        //function SAVE Free Baggage  Joseph
        private void Save(string cboMPC, string k, string lb, string pc)
        {

            SqlConnection cs = new SqlConnection(pbConnectionString);

            string a = "0";
            string b = "0";
            string c = "0";

            if (k != "")
            {
                a = k;
            }

            if (lb != "")
            {
                b = lb;
            }

            if (pc != "")
            {
                c = pc;
            }

            for (int i = 0; i < 3; i++)
            {
                string sql = "";

                if (i == 0)
                {

                    sql = sql + "INSERT INTO [Ref].[FreeBaggageAllowance] VALUES ( " + Environment.NewLine;
                    sql = sql + "'" + cboMPC + "'," + Environment.NewLine;
                    sql = sql + "'K','" + a + "')" + Environment.NewLine;

                }
                else
                    if (i == 1)
                {

                    sql = sql + "INSERT INTO [Ref].[FreeBaggageAllowance] VALUES ( " + Environment.NewLine;
                    sql = sql + "'" + cboMPC + "'," + Environment.NewLine;
                    sql = sql + "'LB','" + b + "')" + Environment.NewLine;

                }
                else
                        if (i == 2)
                {

                    sql = sql + "INSERT INTO [Ref].[FreeBaggageAllowance] VALUES ( " + Environment.NewLine;
                    sql = sql + "'" + cboMPC + "'," + Environment.NewLine;
                    sql = sql + "'PC','" + c + "')" + Environment.NewLine;
                }

                if (cs.State == ConnectionState.Open)
                {
                    cs.Close();
                }

                cs.Open();

                SqlDataAdapter da = new SqlDataAdapter();
                da.InsertCommand = new SqlCommand(sql, cs);

                da.InsertCommand.ExecuteNonQuery();

                cs.Close();
            }
        }

        // function DELET Joseph
        private void DELETE(string cboMPC)
        {

            SqlConnection con = new SqlConnection(pbConnectionString);

            string sql = "";
            sql = sql + "DELETE FROM [Ref].[FreeBaggageAllowance] WHERE [MappedPrimeCode] = '" + cboMPC + "'";

            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }

            con.Open();

            SqlDataAdapter da = new SqlDataAdapter();
            da.DeleteCommand = new SqlCommand(sql, con);

            da.DeleteCommand.ExecuteNonQuery();

            con.Close();

        }

        // Action Save Free Baggage    Joseph
        public ActionResult ActionSave()
        {
            getMappedCode();

            string param = Request["code"];

            string paramK = Request["valK"];
            string paramLB = Request["valLB"];
            string paramPC = Request["valPC"];

            DELETE(param);

            Save (param, paramK, paramLB, paramPC);

            // re affichage

            if (param == "-All-")
            {
                ViewBag.txtPC = "";
                ViewBag.LB = "";
                ViewBag.K = "";

                DisplayGrid(param);
            }
            else
            {
                DisplayValeur(param);
                DisplayGrid(param);
            }

            ViewBag.valPresetCode = param;

            return PartialView("FreeBaggageAllowanceAudit");
        }


        // Action Cancel    Joseph
        public ActionResult ActionCancel()
        {

            getMappedCode();

            string param = Request["code"];

            DisplayValeur(param);

            // re affichage

            if (param == "-All-")
            {
                ViewBag.txtPC = "";
                ViewBag.LB = "";
                ViewBag.K = "";

                DisplayGrid(param);
            }
            else
            {
                DisplayValeur(param);
                DisplayGrid(param);
            }

            ViewBag.valPresetCode = param;


            return PartialView("FreeBaggageAllowanceAudit");
        }


        /* End Free Baggage Allowans   Joseph*/




        /*  Audit   Commission Audit       Joseph*/
        public ActionResult Commission()
        {
            string dtpFrom = DateTime.Now.AddMonths(-1).ToString("dd-MMM-yyyy");
            string dtpTo = DateTime.Now.ToString("dd-MMM-yyyy");

            ViewBag.dateFrom = dtpFrom;
            ViewBag.dateTo = dtpTo;

            return PartialView();
        }

        // Declaration Commission

        string ag = "%";

        // Get Angent Numeric Code by Date From & To
        public ActionResult GetAgentNum()
        {

            string dtpFrom = Request["from"];
            string dtpTo = Request["to"];

            AgentNumCode(dtpFrom, dtpTo);

            return PartialView("Commission");

        }

        public void AgentNumCode(string dtpFrom, string dtpTo)
        {
            string sql = "select distinct AgentNumericCode from [Pax].[SalesDocumentHeader] where DateofIssue between '" + dtpFrom + "' and '" + dtpTo + "' ";

            SqlConnection con = new SqlConnection(pbConnectionString);
            DataSet ds = new DataSet();
            SqlCommand cmd = new SqlCommand("" + sql + "", con);


            SqlDataAdapter ada = new SqlDataAdapter(cmd);

            ada.Fill(ds);

            int logAgent = ds.Tables[0].Rows.Count;

            string[,] ItemAgentNum = new string[1, logAgent];

            int i = 0;

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                ItemAgentNum[0, i] = dr[ds.Tables[0].Columns[0].ColumnName].ToString();

                i++;
            }

            ViewBag.logAgent = logAgent;
            ViewBag.ItemAgentNum = ItemAgentNum;

            ViewBag.dateFrom = dtpFrom;
            ViewBag.dateTo = dtpTo;
        }

        public ActionResult SearchCommission()
        {
            string dtpFrom = Request["from"];
            string dtpTo = Request["to"];
            string AgentCode = Request["code"];

            AgentNumCode(dtpFrom, dtpTo);

            ViewBag.valAgentCode = AgentCode;

            Comm(AgentCode, dtpFrom, dtpTo);

            Agentinfo(AgentCode);

            return PartialView("Commission");
        }

        public void Comm(string AgentCode, string dtpFrom, string dtpTo)
        {
            if (AgentCode == "-All-")
            {
                ag = "%";
            }
            else
            {
                ag = AgentCode;
            }

            string sql = "";
            sql += "select sdh.DateofIssue as 'Date of Issue',sdh.AgentNumericCode as 'Agent Code',sdh.DocumentNumber as 'Document Number',DocumentAmountType as 'Doc Amt Type',OtherAmountCode as 'Other Amt Code' ,OtherAmountRate as 'Comm %' ,OtherAmount as 'Comm Amt',agt.Rate as 'Agent Comm%', ( agt.Rate-OtherAmountRate) as 'Comm% Diff'  " + Environment.NewLine;
            sql += ",case when ( agt.Rate-OtherAmountRate) is null then 'Agent Info Missing'  " + Environment.NewLine;
            sql += "when ( agt.Rate-OtherAmountRate) < 0 then 'Agent Exceeding Commission % Allocated'end as Remarks ,sdh.TransactionGroup  as 'Transaction Group' " + Environment.NewLine;
            sql += "from Pax.SalesDocumentHeader sdh  " + Environment.NewLine;
            sql += "join Pax.SalesRelatedDocumentInformation srd on sdh.HdrGuid = srd.HdrGuid    " + Environment.NewLine;
            sql += "left join Pax.SalesDocumentOtherAmount sdo on srd.RelatedDocumentGuid =  sdo.RelatedDocumentGuid and DocumentAmountType like 'Com%' and OtherAmountCode = 'Effective'    " + Environment.NewLine;
            sql += "left join [Ref].[PaxAgencyDetails] agt on left(agt.AgencyNumericCode,7) = left(sdh.AgentNumericCode,7)    " + Environment.NewLine;
            sql += "where OtherAmountCode  = 'Effective' and DocumentAmountType like 'Com%' and AgentNumericCode like '" + ag + "'  " + Environment.NewLine;
            sql += "  and  sdh.DateofIssue between '" + dtpFrom + "' and '" + dtpTo + "'     " + Environment.NewLine;
            sql += "order by sdh.DateofIssue,sdh.DocumentNumber    " + Environment.NewLine;


            SqlConnection con = new SqlConnection(pbConnectionString);
            DataSet ds = new DataSet();
            SqlCommand cmd = new SqlCommand("" + sql + "", con);


            SqlDataAdapter ada = new SqlDataAdapter(cmd);

            ada.Fill(ds);

            int logComm = ds.Tables[0].Rows.Count;

            string[,] ListeComm = new string[12, logComm];

            int i = 0;

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                ListeComm[0, i] = dr[ds.Tables[0].Columns[0].ColumnName].ToString();
                ListeComm[1, i] = dr[ds.Tables[0].Columns[1].ColumnName].ToString();
                ListeComm[2, i] = dr[ds.Tables[0].Columns[2].ColumnName].ToString();
                ListeComm[3, i] = dr[ds.Tables[0].Columns[3].ColumnName].ToString();
                ListeComm[4, i] = dr[ds.Tables[0].Columns[4].ColumnName].ToString();
                ListeComm[5, i] = dr[ds.Tables[0].Columns[5].ColumnName].ToString();
                ListeComm[6, i] = dr[ds.Tables[0].Columns[6].ColumnName].ToString();
                ListeComm[7, i] = dr[ds.Tables[0].Columns[7].ColumnName].ToString();
                ListeComm[8, i] = dr[ds.Tables[0].Columns[8].ColumnName].ToString();
                ListeComm[9, i] = dr[ds.Tables[0].Columns[9].ColumnName].ToString();
                ListeComm[10, i] = dr[ds.Tables[0].Columns[10].ColumnName].ToString();

                i++;
            }

            ViewBag.logComm = logComm;
            ViewBag.ListeComm = ListeComm;
        }

        // function Clear

        public ActionResult ClearCommi()
        {
            string dtpFrom = Request["from"];
            string dtpTo = Request["to"];
            string AgentCode = Request["code"];

            Comm(AgentCode, dtpFrom, dtpTo);

            string initdtpFrom = DateTime.Now.AddMonths(-1).ToString("dd-MMM-yyyy");
            string initdtpTo = DateTime.Now.ToString("dd-MMM-yyyy");

            ViewBag.dateFrom = initdtpFrom;
            ViewBag.dateTo = initdtpTo;

            ViewBag.ItemAgentNum = "";

            return PartialView("Commission");
        }

        // function Get Agent Info     Joseph
        public void Agentinfo(string ag)
        {

            ViewBag.nameAgent = "";
            ViewBag.addressAgent = "";
            ViewBag.statusAgent = "";
            ViewBag.categoryAgent = "";
            ViewBag.remarksAgent = "";
            ViewBag.lblDOA = "";


            string sql = "SELECT f2.[AgencyNumericCode],f1.LegalName,f1.LocationAddress,[Status]     ";
            sql += ",[Category],[Remarks],[DateOfAppointment] from ref.VW_Agent f1 ";
            sql += " left join [Ref].[PassengerAgencyDetails] f2 on f1.AgencyNumericCode = f2.AgencyNumericCode ";
            sql += " where f2.AgencyNumericCode = left('" + ag + "',7) ";

            SqlConnection cs = new SqlConnection(pbConnectionString);

            SqlCommand cmd = new SqlCommand("" + sql + "", cs);
            cs.Open();
            SqlDataReader rd = cmd.ExecuteReader();

            if (rd.HasRows)
            {
                while (rd.Read())
                {
                    ViewBag.nameAgent = rd.GetValue(1).ToString();
                    ViewBag.addressAgent = rd.GetValue(2).ToString();
                    ViewBag.statusAgent = rd.GetValue(3).ToString();
                    ViewBag.categoryAgent = rd.GetValue(4).ToString();
                    ViewBag.remarksAgent = rd.GetValue(5).ToString();
                    ViewBag.lblDOA = rd.GetDateTime(6).ToString("dd-MMM-yyyy");

                }
            }
            cs.Close();

            string SqlAgency = "SELECT *  from [Ref].[PaxAgencyDetails]  where AgencyNumericCode = left('" + ag + "',7) ";

            SqlConnection con = new SqlConnection(pbConnectionString);
            DataSet ds2 = new DataSet();
            SqlCommand cmd2 = new SqlCommand("" + SqlAgency + "", con);


            SqlDataAdapter ada2 = new SqlDataAdapter(cmd2);

            ada2.Fill(ds2);

            int logAgency = ds2.Tables[0].Rows.Count;

            string[,] ListeTabAgency = new string[6, logAgency];

            int i = 0;

            foreach (DataRow dr in ds2.Tables[0].Rows)
            {
                ListeTabAgency[0, i] = dr[ds2.Tables[0].Columns[0].ColumnName].ToString();
                ListeTabAgency[1, i] = dr[ds2.Tables[0].Columns[1].ColumnName].ToString();
                ListeTabAgency[2, i] = dr[ds2.Tables[0].Columns[2].ColumnName].ToString();
                ListeTabAgency[3, i] = dr[ds2.Tables[0].Columns[3].ColumnName].ToString();
                ListeTabAgency[4, i] = dr[ds2.Tables[0].Columns[4].ColumnName].ToString();
                ListeTabAgency[5, i] = dr[ds2.Tables[0].Columns[5].ColumnName].ToString();
                
                i++;
            }

            ViewBag.logAgency = logAgency;
            ViewBag.ListeTabAgency = ListeTabAgency;
        }


        /* End Audit   Commission Audit       Joseph */



        /* SPA Management   Joseph*/
        public ActionResult SPAManagement()
        {

            GetSPANumber("Application");

            return PartialView();
        }


        // function Get SPA Number   Joseph
        public void GetSPANumber(string tablename)
        {

            string sql = "SELECT distinct  [SPANumber]  as SPANumber FROM [SPA]." + tablename + " order by [SPANumber] " + Environment.NewLine;


            SqlConnection con = new SqlConnection(pbConnectionString);
            DataSet ds = new DataSet();
            SqlCommand cmd = new SqlCommand("" + sql + "", con);


            SqlDataAdapter ada = new SqlDataAdapter(cmd);

            ada.Fill(ds);

            int logSPA = ds.Tables[0].Rows.Count;

            string[,] ItemSPANumber = new string[1, logSPA];

            int i = 0;

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                ItemSPANumber[0, i] = dr[ds.Tables[0].Columns[0].ColumnName].ToString();

                i++;
            }

            ViewBag.logSPA = logSPA;
            ViewBag.ItemSPANumber = ItemSPANumber;
        }


        // Declaration All Variable SPA 
        string[] SPAMAINLen = new string[100];
        string[] IATASeasonLen = new string[100];
        string[] ASVLen = new string[100];
        string[] DiscountLen = new string[100];
        string[] FlightRestrictionLen = new string[100];
        string[] SPAExceptionLen = new string[100];
        string[] AVSExceptionLen = new string[100];
        string[] SignatoriesLen = new string[100];
        string val = "";
        string Charact = @"([A-Z]{1,100})";
        string Row1 = "";
        string Row2 = "";
        string[] ValidatedRow = new string[1000];
        int Valrow = 0;


        /* SPA MAIN     Joseph   */
        public void SPAMAINQuery(string valparam)
        {
            val = "";
            string sql = "";

            if (valparam == "-ALL-")
            {
                val = "%";

            }
            else
            {
                val = valparam;

            }

            sql = "SELECT [SPANumber]   " + Environment.NewLine;
            sql += " ,[SPAReference],[PrimeCode],[FareBasis],[IssuedInExchangeForOwn],[IssuedInExchangeForOAL],[IssuedInExchangeForOthers]  " + Environment.NewLine;
            sql += " ,[PlaceOfIssuanceWorldwide],[PlaceOfIssuanceSpecifiedLoc],[PlaceOfSaleWorldwide],[PlaceOfSaleSpecifiedLoc],[RoutingBoth]  " + Environment.NewLine;
            sql += " ,[RoutingOwn],[RoutingOAL],[RoutingJourneyATBP],[ThirdPartyInvolvement],[Revalidation],[OpenDatedTicket],[TicketIssuedBy]  " + Environment.NewLine;
            sql += " ,[IndemnifyingClause],[FIM],[InvoluntaryRouting],[FlightRestriction],[ValidityofSPA],[ValidityofSPAFrom],[ValidityofSPATo]  " + Environment.NewLine;
            sql += " ,[ASVAmountExpressed],[SPAExclusiveOfTFCs],[ISCPercentage],[IATAClearingHouse],[TourCodeBox],[EndorsementBox],[SectorShare]  " + Environment.NewLine;
            sql += " FROM [SPA].[Application]   " + Environment.NewLine;

            sql += " where [SPANumber] like '" + val + "'   " + Environment.NewLine;



            SqlConnection con = new SqlConnection(pbConnectionString);
            DataSet ds = new DataSet();
            SqlCommand cmd = new SqlCommand("" + sql + "", con);

            SqlDataAdapter ada = new SqlDataAdapter(cmd);

            ada.Fill(ds);

            int logSpaMain = ds.Tables[0].Rows.Count;

            string[,] ListeSPAMain = new string[33, logSpaMain];

            int i = 0;
           
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                for (int j = 0; j < 33; j++)
                {
                    ListeSPAMain[j, i] = dr[ds.Tables[0].Columns[j].ColumnName].ToString();

                    if ( (j == 24) || (j == 25) )
                    {
                        string h = null;

                        h = dr[ds.Tables[0].Columns[j].ColumnName].ToString();

                        if (h != "") { ListeSPAMain[j, i] = Convert.ToDateTime(h).ToString("dd-MMM-yyyy"); }
                    }
                    else { ListeSPAMain[j, i] = dr[ds.Tables[0].Columns[j].ColumnName].ToString(); }
                }

                i++;
            }

            ViewBag.logSpaMain = logSpaMain;
            ViewBag.ListeSPAMain = ListeSPAMain;
        }

        private void SPAMAINLn()
        {
            SqlConnection cs = new SqlConnection(pbConnectionString);

            Array.Clear(SPAMAINLen, 0, SPAMAINLen.Length);

            string sql = "select Concat(isnull(character_maximum_length,0),'|', data_type)  from information_schema.columns   where table_name = 'Application'" + Environment.NewLine;

            try
            {
                if (cs.State == ConnectionState.Open) { cs.Close(); }
                cs.Open();
                SqlCommand cmd = new SqlCommand(sql, cs);

                SqlDataReader rd = cmd.ExecuteReader();
                int i = 0;

                if (rd.HasRows)
                {
                    if (rd != null)
                    {
                        while (rd.Read())
                        {
                            SPAMAINLen[i] = rd.GetValue(0).ToString();
                            i++;
                        }
                    }
                }

                rd.Close();
                cs.Close();

            }
            catch { }
        }

        public ActionResult SPA_SPAMain()
        {
            GetSPANumber("Application");

            string ValSpaNumber = Request["spaNumber"];

            SPAMAINQuery(ValSpaNumber);

            SPAMAINLn();

            ViewBag.valSPANumber = ValSpaNumber;

            return PartialView();
        }

        /* END   SPA MAIN     Joseph   */


        /* IATA SEASON */

        public ActionResult SPA_IATASeason()
        {
            string valSpaIata = Request["iataSe"];

            SPAIATASeasonQuery(valSpaIata);

            IataSeasonLn();

            ViewBag.valIataSeason = valSpaIata;

            return PartialView();
        }

        public void SPAIATASeasonQuery(string param)
        {
            val = "";

            if (param == "-All-")
            {
                val = "%";
            }
            else
            {
                val = param;
            }

            string sql = "SELECT [IATASeason],[From],[To]   FROM [SPA].[IATASeasons] where IATASeason like '" + val + "' order by 2 desc" + Environment.NewLine;

            SqlConnection con = new SqlConnection(pbConnectionString);
            DataSet ds = new DataSet();
            SqlCommand cmd = new SqlCommand("" + sql + "", con);

            SqlDataAdapter ada = new SqlDataAdapter(cmd);

            ada.Fill(ds);

            con.Close();

            int logSPAIata = ds.Tables[0].Rows.Count;

            string[,] ListeSPAIata = new string[3, logSPAIata];
            int i = 0;
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                ListeSPAIata[0, i] = dr[ds.Tables[0].Columns[0].ColumnName].ToString();
                ListeSPAIata[1, i] = Convert.ToDateTime(dr[ds.Tables[0].Columns[1].ColumnName].ToString()).ToString("dd-MMM-yyyy"); 
                ListeSPAIata[2, i] = Convert.ToDateTime(dr[ds.Tables[0].Columns[2].ColumnName].ToString()).ToString("dd-MMM-yyyy");

                i++;
            }
            ViewBag.logSPAIata = logSPAIata;
            ViewBag.ListeSPAIata = ListeSPAIata;
        }

        private void IataSeasonLn()
        {
            SqlConnection cs = new SqlConnection(pbConnectionString);

            Array.Clear(IATASeasonLen, 0, IATASeasonLen.Length);

            string sql = "select Concat(isnull(character_maximum_length,0),'|', data_type)  from information_schema.columns   where table_name = 'IATASeasons'" + Environment.NewLine;

            try
            {
                if (cs.State == ConnectionState.Open) { cs.Close(); }
                cs.Open();
                SqlCommand cmd = new SqlCommand(sql, cs);

                SqlDataReader rd = cmd.ExecuteReader();
                int i = 0;

                if (rd.HasRows)
                {
                    if (rd != null)
                    {
                        while (rd.Read())
                        {
                            IATASeasonLen[i] = rd.GetValue(0).ToString();
                            i++;
                        }
                    }
                }

                rd.Close();
                cs.Close();

            }
            catch { }
        }
        /* END IATA SEASON */


        /* ASV   Joseph  */
        public ActionResult SPA_ASV()
        {
            GetSPANumber("Application");

            string spanumber = Request["spa"];
            string asvnumber = Request["asv"];


            SPAASVQuery(spanumber, asvnumber);
            ASVLn();

            ViewBag.valSpaNumber = spanumber;
            ViewBag.valAsvNumber = asvnumber;

            GetASVNumber(spanumber);

            return PartialView();
        }


        public void SPAASVQuery(string param1, string param2)
        {
            string asv = "";
            string a = "";

            //spa
            if (param1 == "-All-")
            {
                a = "%";
            }
            else
            {
                a = param1;
            }

            //asv
            if (param2 == "-All-")
            {
                asv = "%";
            }
            else
            {
                asv = param2;
            }

            string sql = "SELECT [SPANumber] " + Environment.NewLine;
            sql += " ,[ASVNumber],[BillingEntity],[Trip],[DomesticInternational],[FIMIVL],[ViceVersa],[Currency]  " + Environment.NewLine;
            sql += " ,[SectorOrigin],[SectorDestination],[Gross],[Net],[RBD],[PublishedUnpublished],[FareBasisPrimeCode],[FareCategory]  " + Environment.NewLine;
            sql += " ,[FareBasisPrimeCodeSpecific],[FareAndPaxTypeCode],[ASVExclusiveOfTFCs],[MPAPercentage],[BaseAmountRequired],[ApplicabilityDateFlag]  " + Environment.NewLine;
            sql += " ,[IATASeason],[SeasonalityFrom],[SeasonalityTo],[IssuedOnAfter],[TravelOnBefore],[ASVExceptionNumber],[IssueDateValidFrom]  " + Environment.NewLine;
            sql += " ,[IssueDateValidTo],[FlightDateValidFrom],[FlightDateValidTo]  " + Environment.NewLine;
            sql += " FROM [SPA].[ASV]  where [SPANumber]  like '" + a + "'  and [ASVNumber] like  '" + asv + "'   " + Environment.NewLine;

            SqlConnection con = new SqlConnection(pbConnectionString);
            DataSet ds = new DataSet();
            SqlCommand cmd = new SqlCommand("" + sql + "", con);

            SqlDataAdapter ada = new SqlDataAdapter(cmd);

            ada.Fill(ds);

            con.Close();

            int logSPAASV = ds.Tables[0].Rows.Count;

            string[,] ListeASV = new string[32, logSPAASV];
            int i = 0;
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                for (int j = 0; j < 32; j++)
                {
                    ListeASV[j, i] = dr[ds.Tables[0].Columns[j].ColumnName].ToString();


                    if ((j == 23) || (j == 24) || (j == 25) || (j == 26) || (j == 28) || (j == 29) || (j == 30) || (j == 31))
                    {
                        string h = null;

                        h = dr[ds.Tables[0].Columns[j].ColumnName].ToString();

                        if (h != "") { ListeASV[j, i] = Convert.ToDateTime(h).ToString("dd-MMM-yyyy"); }
                    }
                    else { ListeASV[j, i] = dr[ds.Tables[0].Columns[j].ColumnName].ToString(); }
                }

                i++;
            }
            ViewBag.logSPAASV = logSPAASV;
            ViewBag.ListeASV = ListeASV;
        }


        public void ASVLn()
        {
            SqlConnection cs = new SqlConnection(pbConnectionString);

            Array.Clear(ASVLen, 0, ASVLen.Length);

            string sql = "select Concat(isnull(character_maximum_length,0),'|', data_type)  from information_schema.columns   where table_name = 'ASV'" + Environment.NewLine;

            try
            {
                if (cs.State == ConnectionState.Open) { cs.Close(); }
                cs.Open();
                SqlCommand cmd = new SqlCommand(sql, cs);

                SqlDataReader rd = cmd.ExecuteReader();
                int i = 0;

                if (rd.HasRows)
                {
                    if (rd != null)
                    {
                        while (rd.Read())
                        {
                            ASVLen[i] = rd.GetValue(0).ToString();
                            i++;
                        }
                    }
                }

                rd.Close();
                cs.Close();

            }
            catch { }
        }

        public ActionResult ASVNumber()
        {
            GetSPANumber("Application");

            string spanumber = Request["spa"];
            string asvnumber = Request["asv"];

            ViewBag.valSpaNumber = spanumber;
            ViewBag.valAsvNumber = asvnumber;

            GetASVNumber(spanumber);

            return PartialView("SPA_ASV");
        }


        public void GetASVNumber(string param)
        {

            val = "";
            if (param == "-All-")
            {
                val = "";
            }
            else
            {
                val = param;
            }

            string sql = "SELECT distinct  ASVNumber  as SPANumber FROM [SPA].[ASV] where SPANumber  ='" + val + "'  order by [SPANumber]  " + Environment.NewLine;


            SqlConnection con = new SqlConnection(pbConnectionString);
            DataSet ds = new DataSet();
            SqlCommand cmd = new SqlCommand("" + sql + "", con);


            SqlDataAdapter ada = new SqlDataAdapter(cmd);

            ada.Fill(ds);

            int logASV = ds.Tables[0].Rows.Count;

            string[,] ItemASVNumber = new string[1, logASV];

            int i = 0;

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                ItemASVNumber[0, i] = dr[ds.Tables[0].Columns[0].ColumnName].ToString();

                i++;
            }

            ViewBag.logASV = logASV;
            ViewBag.ItemASVNumber = ItemASVNumber;
        }
        /*  END ASV  */



        /* DISCOUNT  */
        public ActionResult SPA_Discount()
        {
            GetSPANumber("Application");

            string spanumber = Request["spa"];

            SPADiscountQuery(spanumber);

            DiscountLn();

            ViewBag.valSpaNumberD = spanumber;

            return PartialView();
        }

        public void SPADiscountQuery(string param1)
        {
            string a = "";

            //spa
            if (param1 == "-All-")
            {
                a = "%";
            }
            else
            {
                a = param1;
            }

            string sql = "SELECT [SPANumber]  " + Environment.NewLine;
            sql += " ,[Signatory],[FareAndPaxTypeCode],[Percentage],[DiscountType],[Method],[InfantOccupyingSeat]  " + Environment.NewLine;
            sql += " ,[RBD],[FareBasisPrimeCode],[IssueDateValidityFrom],[IssueDateValidityTo]  " + Environment.NewLine;
            sql += " FROM [SPA].[Discount] where [SPANumber] like '" + a + "'   " + Environment.NewLine;

            SqlConnection con = new SqlConnection(pbConnectionString);
            DataSet ds = new DataSet();
            SqlCommand cmd = new SqlCommand("" + sql + "", con);

            SqlDataAdapter ada = new SqlDataAdapter(cmd);

            ada.Fill(ds);

            con.Close();

            int logDiscount = ds.Tables[0].Rows.Count;

            string[,] ListeDiscount = new string[11, logDiscount];
            int i = 0;
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                for (int j = 0; j < 11; j++)
                {
                    ListeDiscount[j, i] = dr[ds.Tables[0].Columns[j].ColumnName].ToString();


                    if ((j == 9) || (j == 10))
                    {
                        string h = null;

                        h = dr[ds.Tables[0].Columns[j].ColumnName].ToString();

                        if (h != "") { ListeDiscount[j, i] = Convert.ToDateTime(h).ToString("dd-MMM-yyyy"); }
                    }
                    else { ListeDiscount[j, i] = dr[ds.Tables[0].Columns[j].ColumnName].ToString(); }
                }

                i++;
            }
            ViewBag.logDiscount = logDiscount;
            ViewBag.ListeDiscount = ListeDiscount;
        }

        public void DiscountLn()
        {
            SqlConnection cs = new SqlConnection(pbConnectionString);

            Array.Clear(DiscountLen, 0, DiscountLen.Length);

            string sql = "select Concat(isnull(character_maximum_length,0),'|', data_type)  from information_schema.columns   where table_name = 'Discount'" + Environment.NewLine;

            try
            {
                if (cs.State == ConnectionState.Open) { cs.Close(); }
                cs.Open();
                SqlCommand cmd = new SqlCommand(sql, cs);

                SqlDataReader rd = cmd.ExecuteReader();
                int i = 0;

                if (rd.HasRows)
                {
                    if (rd != null)
                    {
                        while (rd.Read())
                        {
                            DiscountLen[i] = rd.GetValue(0).ToString();
                            i++;
                        }
                    }
                }

                rd.Close();
                cs.Close();

            }
            catch { }
        }

        /* END DISCOUNT */



        /* FLIGHT RESTRICTION   Joseph*/
        public ActionResult SPA_FlightRestriction()
        {
            GetSPANumber("Application");

            string spanumber = Request["spa"];

            SPAFlightRestrictionQuery(spanumber);

            FlightRestrictionLn();

            ViewBag.valspaFlight = spanumber;

            return PartialView();
        }

        public void SPAFlightRestrictionQuery(string param1)
        {
            string a = "";

            //spa
            if (param1 == "-All-")
            {
                a = "%";
            }
            else
            {
                a = param1;
            }

            string sql = "SELECT  [SPANumber]  " + Environment.NewLine;
            sql += " ,[Carrier],[FlightFrom],[FlightTo],[ValidityFrom],[ValidityTo]  " + Environment.NewLine;
            sql += " FROM [SPA].[FlightRestriction]  where [SPANumber]  like '" + a + "'   " + Environment.NewLine;

            SqlConnection con = new SqlConnection(pbConnectionString);
            DataSet ds = new DataSet();
            SqlCommand cmd = new SqlCommand("" + sql + "", con);

            SqlDataAdapter ada = new SqlDataAdapter(cmd);

            ada.Fill(ds);

            con.Close();

            int logFlight = ds.Tables[0].Rows.Count;

            string[,] ListeFlight = new string[6, logFlight];
            int i = 0;
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                for (int j = 0; j < 6; j++)
                {
                    ListeFlight[j, i] = dr[ds.Tables[0].Columns[j].ColumnName].ToString();


                    if ((j == 4) || (j == 5))
                    {
                        string h = null;

                        h = dr[ds.Tables[0].Columns[j].ColumnName].ToString();

                        if (h != "") { ListeFlight[j, i] = Convert.ToDateTime(h).ToString("dd-MMM-yyyy"); }
                    }
                    else { ListeFlight[j, i] = dr[ds.Tables[0].Columns[j].ColumnName].ToString(); }
                }

                i++;
            }
            ViewBag.logFlight = logFlight;
            ViewBag.ListeFlight = ListeFlight;
        }


        public void FlightRestrictionLn()
        {
            SqlConnection cs = new SqlConnection(pbConnectionString);

            Array.Clear(FlightRestrictionLen, 0, FlightRestrictionLen.Length);

            string sql = "select Concat(isnull(character_maximum_length,0),'|', data_type)  from information_schema.columns   where table_name = 'FlightRestriction'" + Environment.NewLine;

            try
            {
                if (cs.State == ConnectionState.Open) { cs.Close(); }
                cs.Open();
                SqlCommand cmd = new SqlCommand(sql, cs);

                SqlDataReader rd = cmd.ExecuteReader();
                int i = 0;

                if (rd.HasRows)
                {
                    if (rd != null)
                    {
                        while (rd.Read())
                        {
                            FlightRestrictionLen[i] = rd.GetValue(0).ToString();
                            i++;
                        }
                    }
                }

                rd.Close();
                cs.Close();

            }
            catch { }
        }

        /* END   FLIGHT RESTRICTION   Joseph*/



        /* SPA EXCEPTION  Joseph*/
        public ActionResult SPA_Exception()
        {
            GetSPANumber("Application");

            string spanumber = Request["spa"];

            SPASPAExceptionQuery(spanumber);

            SPAExceptionLn();

            ViewBag.valspaException = spanumber;

            return PartialView();
        }


        public void SPASPAExceptionQuery(string param1)
        {
            string a = "";

            //spa
            if (param1 == "-All-")
            {
                a = "%";
            }
            else
            {
                a = param1;
            }

            string sql = "SELECT [SPANumber] " + Environment.NewLine;
            sql += " ,[ApplicableExclude],[Seq],[RowRelation],[Element],[JAPS],[WO],[RegionType],[Region]  " + Environment.NewLine;
            sql += " ,[ProvisoApplicability],[RegionTypeBF],[RegionBF],[RegionTypeAT],[RegionAT],[FareCategory]  " + Environment.NewLine;
            sql += " ,[PrimeCode],[FareAndPaxTypeCode],[FareBasis]  " + Environment.NewLine;
            sql += " FROM [SPA].[SPAException] where [SPANumber] like '" + a + "' order by 1,3 " + Environment.NewLine;


            SqlConnection con = new SqlConnection(pbConnectionString);
            DataSet ds = new DataSet();
            SqlCommand cmd = new SqlCommand("" + sql + "", con);

            SqlDataAdapter ada = new SqlDataAdapter(cmd);

            ada.Fill(ds);

            con.Close();

            int logSpaExcep = ds.Tables[0].Rows.Count;

            string[,] ListeSpaExcep= new string[18, logSpaExcep];
            int i = 0;
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                for (int j = 0; j < 18; j++)
                {
                    ListeSpaExcep[j, i] = dr[ds.Tables[0].Columns[j].ColumnName].ToString();
                }

                i++;
            }
            ViewBag.logSpaExcep = logSpaExcep;
            ViewBag.ListeSpaExcep = ListeSpaExcep;
        }

        public void SPAExceptionLn()
        {
            SqlConnection cs = new SqlConnection(pbConnectionString);

            Array.Clear(SPAExceptionLen, 0, SPAExceptionLen.Length);

            string sql = "select Concat(isnull(character_maximum_length,0),'|', data_type)  from information_schema.columns   where table_name = 'SPAException'" + Environment.NewLine;

            try
            {
                if (cs.State == ConnectionState.Open) { cs.Close(); }
                cs.Open();
                SqlCommand cmd = new SqlCommand(sql, cs);

                SqlDataReader rd = cmd.ExecuteReader();
                int i = 0;

                if (rd.HasRows)
                {
                    if (rd != null)
                    {
                        while (rd.Read())
                        {
                            SPAExceptionLen[i] = rd.GetValue(0).ToString();
                            i++;
                        }
                    }
                }

                rd.Close();
                cs.Close();

            }
            catch { }
        }

        /* END  SPA EXCEPTION  Joseph*/



        /*  ASV EXCEPTION   Joseph */
        public ActionResult SPA_AsvException()
        {
            GetSPANumber("Application");

            string spanumber = Request["spa"];

            SPAASVExceptionQuery(spanumber);

            AVSExceptionLn();

            ViewBag.valAsvException = spanumber;

            return PartialView();
        }


        public void SPAASVExceptionQuery(string param1)
        {
            string a = "";

            //spa
            if (param1 == "-All-")
            {
                a = "%";
            }
            else
            {
                a = param1;
            }

            string sql = "SELECT [SPANumber]" + Environment.NewLine;
            sql += " ,[ASVNumber],[ASVExceptionNumber],[ApplicableExclude],[Seq],[RowRelation],[Element],[OperatingCarrier]  " + Environment.NewLine;
            sql += " ,[JAPS],[WO],[RegionType],[Region],[ProvisoApplicability],[RegionTypeBF],[RegionBF],[RegionTypeAT]  " + Environment.NewLine;
            sql += " ,[RegionAT],[FareCategory],[PrimeCode],[FareAndPaxTypeCode],[RBD]  " + Environment.NewLine;
            sql += " FROM [SPA].[ASVException]  where [SPANumber]  like '" + a + "'   " + Environment.NewLine;

            SqlConnection con = new SqlConnection(pbConnectionString);
            DataSet ds = new DataSet();
            SqlCommand cmd = new SqlCommand("" + sql + "", con);

            SqlDataAdapter ada = new SqlDataAdapter(cmd);

            ada.Fill(ds);

            con.Close();

            int logASVExcep = ds.Tables[0].Rows.Count;

            string[,] ListeAsvExcep = new string[21, logASVExcep];
            int i = 0;
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                for (int j = 0; j < 21; j++)
                {
                    ListeAsvExcep[j, i] = dr[ds.Tables[0].Columns[j].ColumnName].ToString();
                }

                i++;
            }
            ViewBag.logASVExcep = logASVExcep;
            ViewBag.ListeAsvExcep = ListeAsvExcep;
        }

        public void AVSExceptionLn()
        {
            SqlConnection cs = new SqlConnection(pbConnectionString);

            Array.Clear(AVSExceptionLen, 0, AVSExceptionLen.Length);

            string sql = "select Concat(isnull(character_maximum_length,0),'|', data_type)  from information_schema.columns   where table_name = 'ASVException'" + Environment.NewLine;

        //    ViewBag.fareIsn = Id; 
        //}
            try
            {
                if (cs.State == ConnectionState.Open) { cs.Close(); }
                cs.Open();
                SqlCommand cmd = new SqlCommand(sql, cs);

                SqlDataReader rd = cmd.ExecuteReader();
                int i = 0;

                if (rd.HasRows)
                {
                    if (rd != null)
                    {
                        while (rd.Read())
                        {
                            AVSExceptionLen[i] = rd.GetValue(0).ToString();
                            i++;
                        }
                    }
                }

                rd.Close();
                cs.Close();

            }
            catch { }
        }

        /*  END  ASV EXCEPTION   Joseph */


        /* SIGNATORIES    Joseph*/
        public ActionResult SPA_Signatories()
        {
            GetSPANumber("Application");

            string spanumber = Request["spa"];

            SPASignatoryQuery(spanumber);

            SignatoriesLn();

            ViewBag.valSignatories = spanumber;

            return PartialView();
        }


        public void SPASignatoryQuery(string param1)
        {
            string a = "";

            //spa
            if (param1 == "-All-")
            {
                a = "%";
            }
            else
            {
                a = param1;
            }

            string sql = "SELECT [SPANumber],[Carrier],[AirlineID] FROM [SPA].[Signatory]  where [SPANumber] like '" + a + "'  order by [SPANumber] " + Environment.NewLine;

            SqlConnection con = new SqlConnection(pbConnectionString);
            DataSet ds = new DataSet();
            SqlCommand cmd = new SqlCommand("" + sql + "", con);

            SqlDataAdapter ada = new SqlDataAdapter(cmd);

            ada.Fill(ds);

            con.Close();

            int logSignatories = ds.Tables[0].Rows.Count;

            string[,] ListeSignatories = new string[3, logSignatories];
            int i = 0;
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                for (int j = 0; j < 3; j++)
                {
                    ListeSignatories[j, i] = dr[ds.Tables[0].Columns[j].ColumnName].ToString();
                }

                i++;
            }
            ViewBag.logSignatories = logSignatories;
            ViewBag.ListeSignatories = ListeSignatories;
        }


        public void SignatoriesLn()
        {
            SqlConnection cs = new SqlConnection(pbConnectionString);

            Array.Clear(SignatoriesLen, 0, SignatoriesLen.Length);

            string sql = "select Concat(isnull(character_maximum_length,0),'|', data_type)  from information_schema.columns   where table_name = 'Signatory'" + Environment.NewLine;

            try
            {
                if (cs.State == ConnectionState.Open) { cs.Close(); }
                cs.Open();
                SqlCommand cmd = new SqlCommand(sql, cs);

                SqlDataReader rd = cmd.ExecuteReader();
                int i = 0;

                if (rd.HasRows)
                {
                    if (rd != null)
                    {
                        while (rd.Read())
                        {
                            SignatoriesLen[i] = rd.GetValue(0).ToString();
                            i++;
                        }
                    }
                }

                rd.Close();
                cs.Close();

            }
            catch { }
        }

        /* END  SIGNATORIES    Joseph*/

        /* END  SPA Management   Joseph*/

        public ActionResult CreditCardManagement()
        {
            string dateFrom = DateTime.Now.AddMonths(-1).ToString("dd-MMM-yyyy");
            string dateTo = DateTime.Now.ToString("dd-MMM-yyyy");
            string[] date = new string[2] { dateFrom, dateTo };
            ViewBag.date = date;
            return PartialView();
        }


        /* All Function Delete  SPA  */
        public void ConnectBase (string Sql)
        {
            DataSet ds = new DataSet();
            SqlConnection con = new SqlConnection(pbConnectionString);
            con.Open();
            SqlDataAdapter ada = new SqlDataAdapter(Sql, con);
            ada.Fill(ds);
            con.Close();
            
        }

        // Delete SPA MAIN
        public ActionResult DeleteSPAMain()
        {
            string ItemSpa = Request["itemselected"];

            string getSpa = Request["spaNumber"];

            string sql = "DELETE FROM [SPA].[Application] WHERE [SPANumber] = '" + ItemSpa + "'";

            ConnectBase(sql);


            GetSPANumber("Application");
            SPAMAINQuery(getSpa);

         
            return PartialView("SPA_SPAMain");
        }


        // Delete IATA SENSION
        public ActionResult DeleteIATASeason()
        {
            string a = Request["itemselected"];
            string b = Request["paramFrom"];
            string c = Request["paramTo"];

            string sql = "DELETE FROM [SPA].[IATASeasons] WHERE [IATASeason] = '" + a + "' and From  = '" + b + "' and To = '" + c + "' ";

            ConnectBase(sql);

            SPAIATASeasonQuery(a);

            return PartialView("SPA_IATASeason");
        }

        // Delete ASV  Joseph
        public ActionResult DeleteASV()
        {
            string a = Request["spa"];
            string b = Request["asv"];

            string param1 = Request["spaNumber"];
            string param2 = Request["asvNumber"];

            string sql = "DELETE FROM [SPA].[ASV] WHERE [SPANumber] = '" + a + "' and ASVNumber = '" + b + "' ";

            ConnectBase(sql);

            SPAASVQuery(param1, param2);

            GetSPANumber("Application");

            GetASVNumber(param1);

            return PartialView("SPA_ASV");
        }

        // Delete SPA DISCOUNT Joseph

        public ActionResult DeleteDiscount()
        {
            GetSPANumber("Application");

            string a = Request["spa"];

            string param1 = Request["spaNumber"];

            string sql = "DELETE FROM [SPA].[Discount] WHERE [SPANumber] = '" + a + "'";

            ConnectBase(sql);

            SPADiscountQuery(param1);

            return PartialView("SPA_Discount");
        }


        // Delete SPA_FlightRestriction Joseph
        public ActionResult DeleteFlightRestriction()
        {
            GetSPANumber("Application");

            string a = Request["spa"];

            string param1 = Request["spaNumber"];

            string sql = "DELETE FROM [SPA].[FlightRestriction] WHERE [SPANumber] = '" + a + "'";

            ConnectBase(sql);

            SPAFlightRestrictionQuery(param1);

            return PartialView("SPA_FlightRestriction");
        }


        // Delete SPA_Exception Joseph
        public ActionResult DeleteSPA_Exception()
        {
            GetSPANumber("Application");

            string a = Request["spa"];

            string param1 = Request["spaNumber"];

            string sql = "DELETE FROM [SPA].[SPAException] WHERE [SPANumber] = '" + a + "'";

            ConnectBase(sql);

            SPASPAExceptionQuery(param1);

            return PartialView("SPA_Exception");
        }

        // Delete SPA_AsvException Joseph
        public ActionResult DeleteSPA_AsvException()
        {
            GetSPANumber("Application");
            string a = Request["spa"];

            string param1 = Request["spaNumber"];

            string sql = "DELETE FROM [SPA].[ASVException] WHERE [SPANumber] = '" + a + "'";

            ConnectBase(sql);

            SPAASVExceptionQuery(param1);

            return PartialView("SPA_AsvException");
        }

        // Delete SPA_AsvException Joseph
        public ActionResult DeleteSPA_Signatories()
        {
            GetSPANumber("Application");
            string a = Request["spa"];

            string param1 = Request["spaNumber"];

            string sql = "DELETE FROM [SPA].[Signatory] WHERE [SPANumber] = '" + a + "'";

            ConnectBase(sql);

            SPASignatoryQuery(param1);

            return PartialView("SPA_Signatories");
        }


        /*  End  Credit Card Management     Joseph*/



        /*  Usage Date vs Booking   Joseph*/
        public ActionResult UsageDateBookingDate()
        {
             string dateFrom = DateTime.Now.ToString("dd-MMM-yyyy");
             string dateTo = DateTime.Now.ToString("dd-MMM-yyyy");

             ViewBag.dateFrom = dateFrom;
             ViewBag.dateTo = dateTo;


            return PartialView();
        }


        public ActionResult SearchUsageDateBooking()
        {
            string dtpFrom = Request["dateFrom"];
            string dtpTo = Request["dateTo"];

            string sql = "select relateddocumentnumber as [Document Number], f1.UsageDate as [Usage Date] ,f1.FlightDepartureDate as [Booking Date],f1.NotValidBefore as [NVB], f1.NotValidAfter as [NVA]," + Environment.NewLine;
            sql += " case when f1.UsageDate < f1.FlightDepartureDate then 'Flown before Booking Date, Please check if any Penalty applies' " + Environment.NewLine;
            sql += " when f1.UsageDate > f1.FlightDepartureDate then 'Flown after Booking Date, Please check if any Penalty applies' end as Remarks " + Environment.NewLine;
            sql += " from Pax.SalesDocumentCoupon f1  " + Environment.NewLine;
            sql += " where UsageDate is not null and f1.UsageDate <> f1.FlightDepartureDate and f1.UsageDate between '" + dtpFrom + "' and '" + dtpTo + "' " + Environment.NewLine;
            sql += "order by 1";

            SqlConnection con = new SqlConnection(pbConnectionString);
            DataSet ds = new DataSet();
            SqlCommand cmd = new SqlCommand("" + sql + "", con);

            SqlDataAdapter ada = new SqlDataAdapter(cmd);

            ada.Fill(ds);

            con.Close();

            int logUsage = ds.Tables[0].Rows.Count;

            string[,] ListeUsageBooking = new string[6, logUsage];
            int i = 0;
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                for (int j = 0; j < 6; j++)
                {
                    ListeUsageBooking[j, i] = dr[ds.Tables[0].Columns[j].ColumnName].ToString();
                }

                i++;
            }
            ViewBag.logUsage = logUsage;
            ViewBag.ListeUsageBooking = ListeUsageBooking;

            ViewBag.dateFrom = dtpFrom;
            ViewBag.dateTo = dtpTo;

            return PartialView("UsageDateBookingDate");
        }
        /* End Usage Date vs Booking   Joseph*/



        /*   Outward Billing Manual(Engines)     Joseph */

        private string pvAirLineRef = "";
        private string pvCountryRef = "";
        private string pvCurrencyRef = "";
        private string pvAirlineNumericCodeRef = "";

        decimal sum1 = 0;
        decimal sum2 = 0;
        decimal sumTax = 0;

        private void GetSystemParameter()
        {
            string Sql = "SELECT    [String1] as Airline ,[String2]as country ,[String3]  as Currency ,[String4]as AirlineNumericCode";
            Sql = Sql + " FROM [Biatss_MR].[Adm].[GSP]";
            Sql = Sql + " where Parameter='SYS0001'";

            try
            {
                SqlDataReader myReader;

                myReader = GetlistofTables(Sql);

                if (myReader != null)
                {
                    if (myReader.HasRows == true)
                    {
                        while (myReader.Read())
                        {
                            pvAirLineRef = myReader.GetValue(0).ToString();
                            pvCountryRef = myReader.GetValue(1).ToString();
                            pvCurrencyRef = myReader.GetValue(2).ToString();
                            pvAirlineNumericCodeRef = myReader.GetValue(3).ToString();
                        }
                    }
                    // always call Close when done reading.
                    myReader.Close();
                    myReader.Dispose();
                    myReader = null;

                }
            }
            catch { }
        }

        // Affichage Prinicpale  Joseph
        public void GetDataOutwardGeneral(string BillingAirline, string BillingPeriod, string Records)
              {
                  try
                  {
                      string Sql = "Select *  FROM [Pax].[OutwardBilling] where BillingPeriod ='" + BillingPeriod + "'";

                      string billingAir = BillingAirline;

                      if (billingAir != "") { Sql = Sql + " and BALC ='" + BillingAirline + "'"; }

                      string rec = Records;

                      string WhereClause = "";
                      if (rec == "Manual") { WhereClause = "and ManualFlag ='M'"; }
                      if (rec == "System Generated") { WhereClause = "and (ManualFlag) is null"; }

                      Sql = Sql + WhereClause;


                // Liste
                SqlConnection con = new SqlConnection(pbConnectionString);
                DataSet ds = new DataSet();
                SqlCommand cmd = new SqlCommand("" + Sql + "", con);

                SqlDataAdapter ada = new SqlDataAdapter(cmd);

                ada.Fill(ds);

                con.Close();

                int logOutward = ds.Tables[0].Rows.Count;

                string[,] LOutward = new string[18, logOutward];


                int i = 0;
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    for (int j = 0; j < 18; j++)
                    {
                        LOutward[j, i] = dr[ds.Tables[0].Columns[j].ColumnName].ToString();

                        if ((j == 12))
                        {
                            string h = null;

                            h = dr[ds.Tables[0].Columns[j].ColumnName].ToString();

                            if (h != "") { LOutward[j, i] = Convert.ToDateTime(h).ToString("yyyy-MM-dd"); }
                        }
                        else { LOutward[j, i] = dr[ds.Tables[0].Columns[j].ColumnName].ToString(); }


                        if ((j == 8) || (j == 10))
                        {
                            sum1 = sum1 + Convert.ToDecimal(dr[ds.Tables[0].Columns[8].ColumnName].ToString());
                            sum2 = sum2 + Convert.ToDecimal(dr[ds.Tables[0].Columns[10].ColumnName].ToString());
                        }
                    }

                    i++;

                }
                ViewBag.lblTotTax = sumTax.ToString("######.000");
                ViewBag.lblTotAmt = sum1.ToString("######.000");
                ViewBag.lblTotICS = sum2.ToString("######.000");
                ViewBag.lblTotNetAmt = Convert.ToDecimal(sum1 + sumTax - (sum2)).ToString("######.000");

                ViewBag.logOutward = logOutward;
                ViewBag.LOutward = LOutward;


                LoadTaxes(BillingAirline, BillingPeriod,"");

                  }
                  catch { }


              }


        public void LoadTaxes(string BillingAirline, string BillingPeriod, string Records)
        {
            try
            {
                string Sql = "Select *  FROM [Pax].[OutwardBillingTax] where BillingPeriod ='" + BillingPeriod + "'";

                string billingAir = BillingAirline;

                if (billingAir != "") { Sql = Sql + " and ALC ='" + BillingAirline + "'"; }

                string rec = Records;

                string WhereClause = "";
                if (rec == "Manual") { WhereClause = "and ManualFlag ='M'"; }
                if (rec == "System Generated") { WhereClause = "and (ManualFlag) is null"; }

                Sql = Sql + WhereClause;


                // Liste
                SqlConnection con = new SqlConnection(pbConnectionString);
                DataSet ds = new DataSet();
                SqlCommand cmd = new SqlCommand("" + Sql + "", con);

                SqlDataAdapter ada = new SqlDataAdapter(cmd);

                ada.Fill(ds);

                con.Close();

                int logRelatedOutward = ds.Tables[0].Rows.Count;

                string[,] ListeRelatedOutward = new string[8, logRelatedOutward];


                int i = 0;
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        ListeRelatedOutward[j, i] = dr[ds.Tables[0].Columns[j].ColumnName].ToString();

                        if ((j == 5))
                        {
                            sumTax = sumTax + Convert.ToDecimal(dr[ds.Tables[0].Columns[10].ColumnName].ToString());
                        }
                    }

                    i++;

                }
                ViewBag.sumTax = sumTax;

                ViewBag.logRelatedOutward = logRelatedOutward;
                ViewBag.ListeRelatedOutward = ListeRelatedOutward;

            }
            catch { }

        }


        public string ParamBillingPeriod = "";

       public void GetBillingPeriod(string agAirlineCode)
        {

            string Sql = "Select distinct BILLINGPERIOD   FROM [Pax].[OutwardBilling] ";

            if (agAirlineCode != "-ALL-") { Sql = Sql + " where BALC ='" + agAirlineCode + "'"; }
            Sql = Sql + " order by BILLINGPERIOD Desc";

            SqlConnection con1 = new SqlConnection(pbConnectionString);
            DataSet ds1 = new DataSet();

            SqlDataAdapter ada1 = new SqlDataAdapter(Sql, con1);
            ada1.Fill(ds1);
            con1.Close();

            int ligneBillingperiod = ds1.Tables[0].Rows.Count;

            string[,] Billingperiod = new string[1, ligneBillingperiod];

            int ii = 0;

            foreach (DataRow dr1 in ds1.Tables[0].Rows)
            {
                Billingperiod[0, ii] = dr1[ds1.Tables[0].Columns[0].ColumnName].ToString();

                ii++;
            }
            ParamBillingPeriod = Billingperiod[0, 0];

            ViewBag.ligneBillingperiod = ligneBillingperiod;
            ViewBag.Billingperiod = Billingperiod;
        }

        /* Changement AirlineCode   Joseph*/
        public ActionResult GetChangeAirlineCode()
        {
            string paramAirlineCode = Request["valAirlineCode"];
            string paramRecord = Request["valRecords"];

            GetBillingPeriod(paramAirlineCode);
            dupAirlineCode();
            GetDataOutwardGeneral(paramAirlineCode, ParamBillingPeriod, "");

            ViewBag.GetAirline = GetAirline(paramAirlineCode, "").Replace("|", " - ");

            AllFunctionBilling();

            return PartialView("OutwardBillingManual");
        }



        /* Dymanic AirlineCode    Joseph*/
        private string GetAirline(string agRefAirlineId, string agRefAirlineCode)
        {
            string AirLineRef = "";
            string Sql = " SELECT  [AirlineID],[AirlineCode] ,[AirlineName],[Zone],[Status] ,[ICHMember]";
            Sql = Sql + " FROM [Ref].[Airlines]";
            Sql = Sql + "where [AirlineID]='" + agRefAirlineId + "'";
            Sql = Sql + "  or [AirlineCode]='" + agRefAirlineCode + "'";
            try
            {
                SqlDataReader myReader;

                myReader = GetlistofTables(Sql);
                
                if (myReader != null)
                {
                    if (myReader.HasRows == true)
                    {
                        while (myReader.Read())
                        {
                            AirLineRef = myReader.GetValue(0).ToString() + " | " + myReader.GetValue(1).ToString() + " | " + myReader.GetValue(2).ToString();

                           ViewBag.GetAirline = AirLineRef;
                        }
                    }
                    // always call Close when done reading.
                    myReader.Close();
                    myReader.Dispose();
                    myReader = null;
                    // Close the connection when done with it.
                }
            }
            catch { }


            return AirLineRef;
        }


        public void dupAirlineCode()
        {
            /************ Get Airline code ****/
            string sql = "Select distinct BALC   FROM [Pax].[OutwardBilling] order by BALC asc";
            SqlConnection con = new SqlConnection(pbConnectionString);
            DataSet ds = new DataSet();
            SqlDataAdapter ada = new SqlDataAdapter(sql, con);
            ada.Fill(ds);
            con.Close();

            int ligne = ds.Tables[0].Rows.Count;

            string[,] Listeairlinecode = new string[1, ligne];

            int i = 0;

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                Listeairlinecode[0, i] = dr[ds.Tables[0].Columns[0].ColumnName].ToString();

                i++;
            }

            ViewBag.ligne = ligne;
            ViewBag.Listeairlinecode = Listeairlinecode;
            /************End Get Airline code ****/
        }

        public void dupBillingPeriod()
        {
            /************ Get Billing period ****/
            string sql1 = "Select distinct BILLINGPERIOD   FROM [Pax].[OutwardBilling] ";
            sql1 = sql1 + " order by BILLINGPERIOD Desc";

            SqlConnection con1 = new SqlConnection(pbConnectionString);
            DataSet ds1 = new DataSet();
            SqlDataAdapter ada1 = new SqlDataAdapter(sql1, con1);
            ada1.Fill(ds1);
            con1.Close();

            int ligneBillingperiod = ds1.Tables[0].Rows.Count;

            string[,] Billingperiod = new string[1, ligneBillingperiod];

            int ii = 0;

            foreach (DataRow dr1 in ds1.Tables[0].Rows)
            {
                Billingperiod[0, ii] = dr1[ds1.Tables[0].Columns[0].ColumnName].ToString();

                ii++;
            }

            ViewBag.ligneBillingperiod = ligneBillingperiod;
            ViewBag.Billingperiod = Billingperiod;
            /************End Get Billing period ****/
        }


        public ActionResult GetChangeBillingPeriod()
        {
            string paramAirlineCode = Request["valAirlineCode"];
            string paramBilling = Request["valBillingPeriod"];
            string paramRecord = Request["valRecords"];

            AllFunctionBilling();

            GetBillingPeriod(paramAirlineCode);

            GetDataOutwardGeneral(paramAirlineCode, paramBilling, paramRecord);

            ViewBag.GetAirline = GetAirline(paramAirlineCode, "").Replace("|", " - ");

            return PartialView("OutwardBillingManual");
        }


        /* Click Billing Manual   Joseph*/
            /*Get Related Tax    Joseph*/
        public void GetRelatedTax(string agBillingAirline, string agTicketNumber, string agCouponNumber, string agBillingPeriod)
        {

            try
            {
                string Sql = "Select *  FROM [Pax].[OutwardBillingTax] where ALC ='" + agBillingAirline + "'and BillingPeriod ='" + agBillingPeriod + "'" + " and DOC ='" + agTicketNumber + " 'and cpn ='" + agCouponNumber + "'";

                SqlDataReader myReader;

                myReader = GetlistofTables(Sql);

                if (myReader == null) {; return; }
                if (myReader.HasRows == true)
                {
                    while (myReader.Read())
                    {
                        ViewBag.TaxCode = myReader.GetValue(4).ToString();
                        ViewBag.TaxAmount = myReader.GetValue(5).ToString();
                    }
                }
                // always call Close when done reading.
                myReader.Close();
                myReader.Dispose();
                myReader = null;
                // Close the connection when done with it.

                GetCoupondetailsFlown(agBillingAirline + agTicketNumber, agCouponNumber);
                GetCoupondetailsUsage(agBillingAirline + agTicketNumber, agCouponNumber);
            }
            catch { }

        }

        /*Get Coupon Details  Joseph*/
        public void GetCoupondetailsFlown(string agTicketNumber, string agCouponNumber)
        {
            int ShowUsage = 0;

           // if (checkBox1.Checked == true) { ShowUsage = 1; } else { ShowUsage = 0; }
           
                string Sql = " select f1.FareCalculationArea ,";

                Sql = Sql + "  f2.[CouponStatus] as [Coupon Status] ,f2.[IsOAL] as [Is Other Airline?] ,f2.[CouponNumber] as [Coupon Number],f2.[OriginAirportCityCode]  as [Origin Airport City Code],f2.[DestinationAirportCityCode] as [Destination Airport City Code] ,f2.[Carrier],f1.CheckDigit";

                if (ShowUsage == 0)
                {
                    Sql = Sql + "  ,f2.[StopOverCode] as [Stop Over Code] ,f2.[FareBasisTicketDesignator] as [FareBasis Ticket Designator],f2.[FlightNumber] as [Flight Number] ,f2.[FlightDepartureDate] as [Flight Departure Date],f2.[FlightDepartureTime] as [Flight Departure Time] ";
                    Sql = Sql + " ,f2.[NotValidBefore] as [Not Valid Before],f2.[NotValidAfter] as [Not Valid After],f2.[ReservationBookingDesignator]  as [Reservation Booking Designator] ";
                    Sql = Sql + " ,f2.[SegmentIdentifier] as [Segment Identifier],f2.[UsageAirline] as [Usage Airline],f2.[UsageDate] as [Usage Date],f2.[UsageFlightNumber] as [Usage Flight Number],f2.[UsageOriginCode] as [Usage Origin Code],f2.[UsageDestinationCode] as [Usage Destination Code]";
                    Sql = Sql + " ,f2.[UsedClassofService] as [Used Class of Service] ,f2.[SettlementAuthorizationCode] as [Settlement Authorization Code] ,f2.[DomesticInternational] as [Domestic International]";
                    Sql = Sql + "  ,f2.[OriginCity] as [Origin City],f2.[DestinationCity] as [Destination City],f2.[OriginCountry] as[Origin Country],f2.[DestinationCountry] as [Destination Country] ";
                }
                else
                {
                    Sql = Sql + " ,f2.[SegmentIdentifier] as [Segment Identifier],f2.[UsageAirline] as [Usage Airline],f2.[UsageDate] as [Usage Date],f2.[UsageFlightNumber] as [Usage Flight Number],f2.[UsageOriginCode] as [Usage Origin Code],f2.[UsageDestinationCode] as [Usage Destination Code]";
                    Sql = Sql + " ,f2.[UsedClassofService] as [Used Class of Service] ,f2.[SettlementAuthorizationCode] as [Settlement Authorization Code] ,f2.[DomesticInternational] as [Domestic International]";
                    Sql = Sql + "  ,f2.[OriginCity] as [Origin City],f2.[DestinationCity] as [Destination City],f2.[OriginCountry] as[Origin Country],f2.[DestinationCountry] as [Destination Country] ";
                }
                Sql = Sql + ", f1.[DateofIssue] ,f1.[SaleDate]";
                Sql = Sql + " from Pax.SalesDocumentHeader f1 left join pax.SalesDocumentCoupon f2 on f1.HdrGuid=f2.RelatedDocumentGuid";
                Sql = Sql + " where f1.DocumentNumber='" + agTicketNumber.Trim() + "'";


                // Liste
            SqlConnection con = new SqlConnection(pbConnectionString);
            DataSet ds = new DataSet();
            SqlCommand cmd = new SqlCommand("" + Sql + "", con);

            SqlDataAdapter ada = new SqlDataAdapter(cmd);

            ada.Fill(ds);

            con.Close();

            int logCouponDetail = ds.Tables[0].Rows.Count;

            string[,] ListeCouponDetail = new string[31, logCouponDetail];


            int i = 0;
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                for (int j = 0; j < 31; j++)
                {
                    ListeCouponDetail[j, i] = dr[ds.Tables[0].Columns[j].ColumnName].ToString();
                }

                i++;
            }
            ViewBag.log = 1;

            ViewBag.logCouponDetail = logCouponDetail;
            ViewBag.ListeCouponDetail = ListeCouponDetail;
        }

        public void GetCoupondetailsUsage(string agTicketNumber, string agCouponNumber)
        {

                string Sql = " select f1.FareCalculationArea ,";
                Sql = Sql + "  f2.[CouponStatus] as [Coupon Status] ,f2.[IsOAL] as [Is Other Airline?] ,f2.[CouponNumber] as [Coupon Number],f2.[OriginAirportCityCode]  as [Origin Airport City Code],f2.[DestinationAirportCityCode] as [Destination Airport City Code] ,f2.[Carrier],f1.CheckDigit";
                Sql = Sql + " ,f2.[SegmentIdentifier] as [Segment Identifier],f2.[UsageAirline] as [Usage Airline],f2.[UsageDate] as [Usage Date],f2.[UsageFlightNumber] as [Usage Flight Number],f2.[UsageOriginCode] as [Usage Origin Code],f2.[UsageDestinationCode] as [Usage Destination Code]";
                Sql = Sql + " ,f2.[UsedClassofService] as [Used Class of Service] ,f2.[SettlementAuthorizationCode] as [Settlement Authorization Code] ,f2.[DomesticInternational] as [Domestic International]";
                Sql = Sql + "  ,f2.[OriginCity] as [Origin City],f2.[DestinationCity] as [Destination City],f2.[OriginCountry] as[Origin Country],f2.[DestinationCountry] as [Destination Country] ";

            Sql = Sql + ", f1.[DateofIssue] ,f1.[SaleDate]";
            Sql = Sql + " from Pax.SalesDocumentHeader f1 left join pax.SalesDocumentCoupon f2 on f1.HdrGuid=f2.RelatedDocumentGuid";
            Sql = Sql + " where f1.DocumentNumber='" + agTicketNumber.Trim() + "'";


            // Liste
            SqlConnection con = new SqlConnection(pbConnectionString);
            DataSet ds = new DataSet();
            SqlCommand cmd = new SqlCommand("" + Sql + "", con);

            SqlDataAdapter ada = new SqlDataAdapter(cmd);

            ada.Fill(ds);

            con.Close();

            int logCouponUsage = ds.Tables[0].Rows.Count;

            string[,] ListeCouponUsage = new string[23, logCouponUsage];


            int i = 0;
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                for (int j = 0; j < 23; j++)
                {
                    ListeCouponUsage[j, i] = dr[ds.Tables[0].Columns[j].ColumnName].ToString();
                }

                i++;
            }
            ViewBag.log = 1;

            ViewBag.logCouponUsage = logCouponUsage;
            ViewBag.ListeCouponUsage = ListeCouponUsage;
        }

        /* End  Get Coupon Details  Joseph*/


        /* getDataEntry  Joseph*/

        public ActionResult GetDataEntry()
        {
            AllFunctionBilling();

            string BillingAirline = Request["param1"];
            string BillingPeriod = Request["valBillingPeriod"];
            string Records = Request["valRecords"];

            // re-affichage Billing
            GetDataOutwardGeneral(BillingAirline, BillingPeriod, Records);
            ViewBag.GetAirline = GetAirline(BillingAirline, "").Replace("|", " - ");

            // Get Tax Reference
            string SecFrom = Request["param7"];
            string SecTo = Request["param8"];
            string FlightDate = Request["param13"];

            string BillingPeriodFormater = Request["param17"];

            string yyyy = "20" + BillingPeriodFormater.ToString().Substring(0, 2);
            string MM = BillingPeriodFormater.ToString().Substring(2, 2);
            string period = BillingPeriodFormater.ToString().Substring(4, 2);

            ViewBag.cboBPyyyy = yyyy;
            ViewBag.cboBPMM = MM;
            ViewBag.cboBPPeriod = period;

            GetApplicableTax(SecFrom, SecTo, FlightDate, yyyy, MM);

            // GetFinal Share
           string txtAirlineCode = Request["param2"];
            string txtTicketNumber = Request["param3"];
            string txtCouponNumber = Request["param4"];

            GetFinalShare(txtAirlineCode.Trim() + txtTicketNumber.Trim(), txtCouponNumber);


            GetRelatedTax(txtAirlineCode, txtTicketNumber, txtCouponNumber, BillingPeriodFormater);


            return PartialView("OutwardBillingManual");
        }

        /*Get Tax Reference */

        private void GetApplicableTax(string txtSecFrom, string txtSecTo, string txtFlightDate, string cboBPyyyy, string cboBPMM)
        {
            string Sql = "SELECT  distinct [TaxCode],[TaxName],[DomesticInternational],TaxAmount,RATDCurrency";
            Sql = Sql + " FROM [TFC].[ApplicableTFC] ";
            Sql = Sql + " where [FromSector]='" + txtSecFrom.ToString().Trim() + "' and [ToSector]='" + txtSecTo.ToString().Trim() + "'";
            Sql = Sql + " and [DomesticInternational]= 'I'"; //+ txtSecFrom.Tag.ToString().Trim() + "'";
            Sql = Sql + " and '" + txtFlightDate.ToString() + "' >= Effective AND Expiry is null AND '" + txtFlightDate.ToString() + "'>=Travel";

            SqlConnection con = new SqlConnection(pbConnectionString);
            DataSet ds = new DataSet();
            SqlCommand cmd = new SqlCommand("" + Sql + "", con);

            SqlDataAdapter ada = new SqlDataAdapter(cmd);

            ada.Fill(ds);

            con.Close();

            int logGetATax = ds.Tables[0].Rows.Count;

            string[,] ListeGetApplicableTax = new string[5, logGetATax];
            int i = 0;
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                for (int j = 0; j < 5; j++)
                {
                  //  string TaxCodes = myReader.GetValue(0).ToString() + " - " + myReader.GetValue(1).ToString() + " - " + myReader.GetValue(2).ToString() + " - " + myReader.GetValue(3).ToString() + " - " + myReader.GetValue(4).ToString();
                    ListeGetApplicableTax[j, i] = dr[ds.Tables[0].Columns[j].ColumnName].ToString();
                }

                i++;
            }
            ViewBag.logGetATax = logGetATax;
            ViewBag.ListeGetApplicableTax = ListeGetApplicableTax;


            if(ListeGetApplicableTax[4, 0] != "")
            {
                ViewBag.CurrencyRef = ListeGetApplicableTax[4, 0];
            }
            else
            {
                ViewBag.CurrencyRef = "";
            }

            RateOfExchange5DR(cboBPyyyy, cboBPMM, ViewBag.CurrencyRef);
        }


        /* Get Rate Of Exchange  Joseph*/
        private void RateOfExchange5DR(string cboBPyyyy, string cboBPMM, string CurrencyRef)
        {
            string BillinsalesPeriod = Convert.ToString(Convert.ToInt32(cboBPyyyy.ToString() + cboBPMM.ToString()) - 1);

           // string CurrencyRef = dataGridView6[4, 0].Value.ToString();
            string Sql = "SELECT [Period] ,[Currency],[USDRate]";
            Sql = Sql + " FROM [Ref].[CurrencyRate]";
            Sql = Sql + "  WHERE PERIOD='" + BillinsalesPeriod + "'";
            Sql = Sql + " AND CurrencyType ='2'";
            Sql = Sql + " AND Currency='" + CurrencyRef + "'";


            SqlConnection con = new SqlConnection(pbConnectionString);
            DataSet ds = new DataSet();
            SqlCommand cmd = new SqlCommand("" + Sql + "", con);

            SqlDataAdapter ada = new SqlDataAdapter(cmd);

            ada.Fill(ds);

            con.Close();

            int logRateOf = ds.Tables[0].Rows.Count;

            string[,] ListeRateOf = new string[3, logRateOf];
            int i = 0;
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                for (int j = 0; j < 3; j++)
                {

                    ListeRateOf[j, i] = dr[ds.Tables[0].Columns[j].ColumnName].ToString();
                }

                i++;
            }
            ViewBag.logRateOf = logRateOf;
            ViewBag.ListeRateOf = ListeRateOf;
        }

        /*Get Final Share    Joseph*/
        private void GetFinalShare(string agDocNumber, string agCouponNumber)
        {
            string Sql = "SELECT HeaderGuid ,[SpecialProrateAgreement],[FinalShare],[CouponNumber],f2.DocumentNumber";
            Sql = Sql + " FROM [Pax].[ProrationDetail] f1 ";
            Sql = Sql + " right join pax.SalesDocumentHeader f2 on f1.HeaderGuid= f2.HdrGuid";
            Sql = Sql + " where HeaderGuid= f2.HdrGuid";
            Sql = Sql + " AND f2.DocumentNumber ='" + agDocNumber + "' and f1.CouponNumber='" + agCouponNumber + "'";

            try
            {
                SqlDataReader myReader;

                myReader = GetlistofTables(Sql);

                if (myReader != null)
                {
                    if (myReader.HasRows == true)
                    {
                        while (myReader.Read())
                        {
                            string AirLineRef = myReader.GetValue(0).ToString() + " | " + myReader.GetValue(1).ToString() + " | " + myReader.GetValue(2).ToString();

                            ViewBag.txtAmount = myReader.GetValue(2).ToString();

                            if (myReader.GetDecimal(1) == 0)
                            {
                                ViewBag.lblSPA = "";
                                ViewBag.lblSPA = null;

                                ViewBag.txtICSPercent = "9";

                                decimal IcsAmount = Convert.ToDecimal(ViewBag.txtAmount) * Convert.ToDecimal(ViewBag.txtICSPercent) / 100;
                                ViewBag.txtICSAmount = IcsAmount.ToString("#####.000");

                            }
                            else
                            {
                                ViewBag.lblSPA = "Y";
                                ViewBag.txtICSPercent = "0";
                                decimal IcsAmount = Convert.ToDecimal(ViewBag.txtAmount) * Convert.ToDecimal(ViewBag.txtICSPercent) / 100;
                                ViewBag.txtICSAmount = IcsAmount.ToString("#####.000");
                            }

                        }


                    }
                    // always call Close when done reading.
                    myReader.Close();
                    myReader.Dispose();
                    myReader = null;
                    // Close the connection when done with it.
                }
            }
            catch { }
        }


        /* Delet OutwardBilling Manual  Joseph*/
        public ActionResult DeleteOutWardBilling()
        {

            string BillingAirline = Request["valAirlineCode"];
            string BillingPeriod = Request["valBillingPeriod"];
            string Records = Request["valRecords"];

            string sql = "DELETE FROM [Pax].[OutwardBillingTax] WHERE [ALC] = '" + BillingAirline + "' and [BillingPeriod]  = '" + BillingPeriod + "';" + Environment.NewLine;
            sql = sql + "DELETE FROM [Pax].[OutwardBilling] WHERE [ALC] = '" + BillingAirline + "' and [BillingPeriod]  = '" + BillingPeriod + "';" + Environment.NewLine;

            ConnectBase(sql);

            AllFunctionBilling();

            // re-affichage Billing
            GetDataOutwardGeneral(BillingAirline, BillingPeriod, Records);
            ViewBag.GetAirline = GetAirline(BillingAirline, "").Replace("|", " - ");

            return PartialView("OutwardBillingManual");

        }
        /* End Delet OutwardBilling Manual */


        /*  SAVE OutwardBilling Manual  Jospeh  */
        public ActionResult SaveOutwardBilling()
        {
            string txtBillingAirline =  Request["param1"];
            string txtAirlineCode = Request["param2"];
            string txtTicketNumber = Request["param3"];
            string txtCouponNumber = Request["param4"];
            string txtCheckDigit = Request["param5"];
            int txtPMC = Request["param6"].AsInt();        
            string txtSecFrom = Request["param7"];
            string txtSecTo = Request["param8"];
            Decimal txtAmount = Request["param9"].AsDecimal();
            int txtICSPercent = Request["param10"].AsInt();
            int txtICSAmount = Request["param11"].AsInt();
            string txtFlightNumber = Request["param12"];
            string FlightDate = Request["param13"];
            DateTime txtFlightDate = DateTime.Parse(FlightDate);
            string txtCurr = Request["param14"];
            string txtETKTSAC = Request["param15"];
            string txtInvoiceNumber = Request["param16"];
            string txtBillingPeriod = Request["param17"];
            string txtDataSource = Request["param18"];

            string Sql = "";
            Sql = Sql += "IF NOT EXISTS(SELECT * FROM [Pax].[OutwardBilling] WHERE  [DOC] = '" + txtTicketNumber + "' AND [CPN]  = '" + txtCouponNumber + "')";
            Sql = Sql + " BEGIN" + Environment.NewLine;
            //Sql = Sql + "ELSE"; ;
            Sql = Sql + " INSERT INTO [Pax].[OutwardBilling]([BALC],[ALC],[DOC],[CPN],[CHK],[PMC],[SECTORFROM],[SECTORTO],[AMOUNT],[ISC],[COMM],[FLIGHT],[FLTDATE],[CUR],[ETKTSAC],[INVOICENO],[BILLINGPERIOD],[ManualFlag]) ";
            Sql = Sql + " VALUES(";
            Sql = Sql + "'" + txtBillingAirline + "',";
            Sql = Sql + "'" + txtAirlineCode + "',";
            Sql = Sql + "'" + txtTicketNumber + "',";
            Sql = Sql + "'" + txtCouponNumber+ "',";
            Sql = Sql + "'" + txtCheckDigit + "',";
            Sql = Sql + "'" + txtPMC + "',";
            Sql = Sql + "'" + txtSecFrom + "',";
            Sql = Sql + "'" + txtSecTo + "',";
            Sql = Sql + "'" + txtAmount + "',";
            Sql = Sql + "'" + txtICSPercent + "',";
            Sql = Sql + "'" + txtICSAmount + "',";
            Sql = Sql + "'" + txtFlightNumber + "',";
            Sql = Sql + "'" + txtFlightDate + "',";
            Sql = Sql + "'" + txtCurr + "',";
            Sql = Sql + "'" + txtETKTSAC + "',";
            Sql = Sql + "'" + txtInvoiceNumber + "',";
            Sql = Sql + "'" + txtBillingPeriod + "',";
            Sql = Sql + "'" + txtDataSource + "'";
            Sql = Sql + " )" + Environment.NewLine;

            Sql = Sql + " END " + Environment.NewLine;
            Sql = Sql + "ELSE";
            Sql = Sql + " BEGIN" + Environment.NewLine;

            Sql = Sql + " UPDATE [Pax].[OutwardBilling]";
            Sql = Sql + " SET [BALC] ='" + txtBillingAirline.Trim() + "',";
            Sql = Sql + "      [ALC] ='" + txtAirlineCode.Trim() + "',";
            Sql = Sql + "      [DOC] ='" + txtTicketNumber.Trim() + "',";
            Sql = Sql + "      [CPN] ='" + txtCouponNumber.Trim() + "',";
            Sql = Sql + "      [CHK] ='" + txtCheckDigit.Trim() + "',";
            Sql = Sql + "      [PMC] ='" + txtPMC + "',"; 
            Sql = Sql + "      [SECTORFROM] ='" + txtSecFrom.Trim() + "',";
            Sql = Sql + "      [SECTORTO] ='" + txtSecTo.Trim() + "',";
            Sql = Sql + "      [AMOUNT] ='" + txtAmount + "',"; 
            Sql = Sql + "      [ISC] ='" + txtICSPercent + "',";
            Sql = Sql + "      [COMM] ='" + txtICSAmount + "',";
            Sql = Sql + "      [FLIGHT] ='" + txtFlightNumber.Trim() + "',"; 
            Sql = Sql + "      [FLTDATE] ='" + txtFlightDate + "',";
            Sql = Sql + "      [CUR] ='" + txtCurr.Trim() + "',";
            Sql = Sql + "      [ETKTSAC] ='" + txtETKTSAC.Trim() + "',";
            Sql = Sql + "      [INVOICENO] ='" + txtInvoiceNumber.Trim() + "',";//ok

         // Sql = Sql + "      [BILLINGPERIOD] ='" + txtBillingPeriod.Trim() + "'";
            Sql = Sql + "      [ManualFlag] ='" + txtDataSource.Trim() + "'";
            Sql = Sql + " WHERE [ALC] = '" + txtAirlineCode.Trim() + "'  AND  [DOC] = '" + txtTicketNumber + "' AND [CPN]  = '" + txtCouponNumber + "'" + Environment.NewLine;
            Sql = Sql + " END";

            DataSet ds = new DataSet();
            ds = dbconnect.RetObjDS(Sql);

            AllFunctionBilling();

            // re-affichage Billing
            string BillingAirline = Request["valAirlineCode"];
            string BillingPeriod = Request["valBillingPeriod"];
            string Records = Request["valRecords"];

            GetDataOutwardGeneral(BillingAirline, BillingPeriod, Records);
            ViewBag.GetAirline = GetAirline(BillingAirline, "").Replace("|", " - ");


            return PartialView("OutwardBillingManual");
        }

        /* End  SAVE OutwardBilling Manual  Jospeh  */

        public void AllFunctionBilling()
        {
            // recuperation  Item
            dupAirlineCode();
            dupBillingPeriod();

            // combo Billing
            LoadCboXmlBillingPeriod();
        }


        /*Outward Billing Xml File  Joseph*/

        public void LoadCboXmlBillingPeriod()
        {
            string sql = "Select distinct [BILLINGPERIOD] from [Pax].[OutwardBilling] order by [BILLINGPERIOD]  Desc";

            SqlConnection con = new SqlConnection(pbConnectionString);
            DataSet ds = new DataSet();
            SqlDataAdapter ada = new SqlDataAdapter(sql, con);
            ada.Fill(ds);
            con.Close();

            int logXMLBilling = ds.Tables[0].Rows.Count;

            string[,] ListeXMLBilling = new string[1, logXMLBilling];

            int i = 0;

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                ListeXMLBilling[0, i] = dr[ds.Tables[0].Columns[0].ColumnName].ToString();

                i++;
            }

            ViewBag.logXMLBilling = logXMLBilling;
            ViewBag.ListeXMLBilling = ListeXMLBilling;
        }

        public ActionResult XMLBillingChange()
        {
            string CboXmlBillingPeriod = Request["valXmlBilling"];

            string SqlA = " SELECT F1.BALC ,F2.[AirlineCode],F2.[AirlineName], BILLINGPERIOD, count(*) as CouponCount";
            SqlA = SqlA + " ,sum([AMOUNT]) as GrossAmount ";
            SqlA = SqlA + ",Sum([COMM]) As ICSAmount ";
            SqlA = SqlA + " , sum([AMOUNT]) - Sum([COMM]) As NetAmount,0 as [Total Tax] , 0 as [Amount To Bill], INVOICENO As [Invoice Number]";
            SqlA = SqlA + " FROM [Pax].[OutwardBilling] F1";
            SqlA = SqlA + " left join  [Ref].[Airlines] F2 on F2.[AirlineID] =F1.BALC";
            SqlA = SqlA + " WHERE BILLINGPERIOD='" + CboXmlBillingPeriod.Trim() + "'";
            SqlA = SqlA + " Group by F1.BALC, BILLINGPERIOD ,F2.[AirlineCode],F2.[AirlineName] ,INVOICENO   ";
            SqlA = SqlA + " ORDER BY  [AirlineName]";

            SqlConnection con = new SqlConnection(pbConnectionString);
            DataSet ds = new DataSet();
            SqlCommand cmd = new SqlCommand("" + SqlA + "", con);

            SqlDataAdapter ada = new SqlDataAdapter(cmd);

            ada.Fill(ds);

            con.Close();

            int logListXMLBil = ds.Tables[0].Rows.Count;

            string[,] XMLBillingChange = new string[11, logListXMLBil];

            int i = 0;
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                for (int j = 0; j < 11; j++)
                {
                    XMLBillingChange[j, i] = dr[ds.Tables[0].Columns[j].ColumnName].ToString();
                }

                i++;
            }

            ViewBag.logListXMLBil = logListXMLBil;
            ViewBag.XMLBillingChange = XMLBillingChange;

            /* for (int k = 0; k < ViewBag.logListXMLBil; k++)
             {
                 Taxe(XMLBillingChange[0, k], CboXmlBillingPeriod);
             }*/

            AllFunctionBilling();

            GetDataOutwardGeneral(XMLBillingChange[0, 0], CboXmlBillingPeriod,"");

            ViewBag.GetAirline = GetAirline(XMLBillingChange[0, 0], "").Replace("|", " - ");

            ViewBag.getAirlineXML = XMLBillingChange[0, 0];

            GetBillingPeriod (ViewBag.getAirlineXML);

            return PartialView();
        }

        /* End Outward Billing Xml File  Joseph*/

        /* Calcul Tax  Amoung to bill */
        public void Taxe( string billingAirlin, string CboXmlBillingPeriod)
        {

            string SqlT = "  SELECT sum([TAXAMT]) as TotalTax  FROM [Pax].[OutwardBillingTax]   ";
            SqlT = SqlT + "where [ALC] ='" + billingAirlin + "' and [BillingPeriod]='" + CboXmlBillingPeriod + "'";

            // Liste
            SqlConnection con2 = new SqlConnection(pbConnectionString);
            DataSet ds2 = new DataSet();
            SqlCommand cmd2 = new SqlCommand("" + SqlT + "", con2);

            SqlDataAdapter ada2 = new SqlDataAdapter(cmd2);

            ada2.Fill(ds2);

            con2.Close();

            int logListXMLBilSuite = ds2.Tables[0].Rows.Count;

           // string[,] XMLBillingChangeSuite = new string[1, logListXMLBilSuite];

            int k = 0;

            foreach (DataRow dr2 in ds2.Tables[0].Rows)
            {
                for (int jk = 0; jk < 1; jk++)
                {
                    ViewBag.XMLBillingChangeSuite = dr2[ds2.Tables[0].Columns[jk].ColumnName].ToString();

                }

                k++;
            }
            ViewBag.logListXMLBilSuite = logListXMLBilSuite;
          //  ViewBag.XMLBillingChangeSuite = XMLBillingChangeSuite;


        }
        /* End Calcul Tax  Amoung to bill */


        /*  Generate SIS IS XML FILES   Joseph*/
        private string GetSystemParameterA()
        {
            string pvAirLineRef = "";
            string pvCountryRef = "";
            string pvCurrencyRef = "";
            string pvAirlineNumericCodeRef = "";

            string Sql = "SELECT    [String1] as Airline ,[String2]as country ,[String3]  as Currency ,[String4]as AirlineNumericCode";
            Sql = Sql + " FROM [Adm].[GSP]";
            Sql = Sql + " where Parameter='SYS0001'";

            try
            {
                SqlDataReader myReader;

                myReader = GetlistofTables(Sql);

                if (myReader != null)
                {
                    if (myReader.HasRows == true)
                    {
                        while (myReader.Read())
                        {
                            pvAirLineRef = myReader.GetValue(0).ToString();
                            pvCountryRef = myReader.GetValue(1).ToString();
                            pvCurrencyRef = myReader.GetValue(2).ToString();
                            pvAirlineNumericCodeRef = myReader.GetValue(3).ToString();
                        }
                    }
                    myReader.Close();
                    myReader.Dispose();
                    myReader = null;
                 
                }
            }
            catch { }


            return (pvAirlineNumericCodeRef);

        }

        public ActionResult GenerateSIS()
        {
            string BillingAirlineCode = GetSystemParameterA();
            string dt = Convert.ToString(DateTime.Now);
            string dt1 = dt.Replace("/", String.Empty);
            string dt2 = dt1.Replace(":", String.Empty);
            string dt3 = dt2.Replace(" ", String.Empty);


           // int BilledAirlinesCount = dataGridView8.Rows.Count;

            for (int i = 0; i < ViewBag.logXMLBilling ; i++)
            {
                //  OutwardBillingXMLGenerator OBGen = new OutwardBillingXMLGenerator();
                XmlWriter02 OBGen = new XmlWriter02();
                string BilledAirline = ViewBag.ListeXMLBilling[0, i].Value.ToString();
                string BillingPeriod = ViewBag.ListeXMLBilling[3, i].Value.ToString();
                string InvoiceNumber = ViewBag.ListeXMLBilling[1, i].Value.ToString() + ViewBag.ListeXMLBilling[3, i].Value.ToString() + (i + 1).ToString("00");
                ViewBag.ListeXMLBilling[10, i].Value = InvoiceNumber;

                // SqlUpdate'
                string Sql = "UPDATE [Pax].[OutwardBilling] SET INVOICENO ='" + InvoiceNumber + "'";
                Sql = Sql + " WHERE ALC ='" + BilledAirline + "' And BillingPeriod ='" + BillingPeriod + "'";

                //  ,XMLFILENAME='" + OBGen.XMLFileName.ToString().Trim() + "'";

                DbUpdate(Sql);

                Thread.Sleep(1000);

                string url = "C:\\SIS_XML_INVOCES\\OUTWARD BILLING\\";

                string OuputFolder = url.ToString() + BillingPeriod + "\\" + dt3 + "\\";
                OBGen.xmlGenetator(BilledAirline, BillingPeriod, BillingAirlineCode, InvoiceNumber, OuputFolder);
                ViewBag.ListeXMLBilling[11, i].Value = OBGen.XMLFileName.ToString();

                if (OBGen.XmlError == "1")
                {
                   // dataGridView8.Rows[i].DefaultCellStyle.BackColor = System.Drawing.Color.Red; dataGridView1.Rows[i].DefaultCellStyle.ForeColor = System.Drawing.Color.White;
                    ViewBag.ListeXMLBilling[12, i].Value = "Error!";
                }
                else
                {
                    ViewBag.ListeXMLBilling[12, i].Value = "OK";
                }

               // dataGridView8.Refresh();
                OBGen = null;
                Thread.Sleep(1000);

            }

            return PartialView("XMLBillingChange");

        }

        /*  End  Generate SIS IS XML FILES   Joseph*/

        /* End   Outward Billing Manual(Engines)     Joseph */


        /* Revenue Analysis   Joseph*/

        public ActionResult RevenueAnalysis()
        {
            string dateFrom = DateTime.Now.AddMonths(-1).ToString("dd-MMM-yyyy");
            string dateTo = DateTime.Now.ToString("dd-MMM-yyyy");

            ViewBag.dateFrom = dateFrom;
            ViewBag.dateTo = dateTo;

            getFlightNo(dateFrom, dateTo);

            return PartialView();
        }

        public ActionResult changeDateRevenue()
        {
            string dateFrom = Request["valdateFrom"];
            string dateTo = Request["valdateTo"];

            getFlightNo(dateFrom, dateTo);

            ViewBag.dateFrom = dateFrom;
            ViewBag.dateTo = dateTo;

            return PartialView("RevenueAnalysis");
        }

        public ActionResult ListeRevenuAnalysis()
        {
            string dateFrom = Request["valdateFrom"];
            string dateTo = Request["valdateTo"];
            string flight = Request["valFligth"];

            string flg = "";

            getFlightNo(dateFrom, dateTo);

            if (flight == "-All-")
            {
                flg = "%";
            }
            else
            {
                flg = flight;
            }

            LoadDataRevenue(dateFrom, dateTo, flg);

            ViewBag.dateFrom = dateFrom;
            ViewBag.dateTo = dateTo;
            return PartialView("RevenueAnalysis");
        }

        public void LoadDataRevenue(string dtpFrom, string dtpTo, string flg)
        {
            string sql = "select 'TRANSPORTATION' as REVENUETYPE, RelatedDocumentNumber,sdc.CouponNumber as CPN ,cast(cast(UsageFlightNumber as int) as nvarchar  (5)) as FightNumber ,UsageDate as FLTDATE,UsageOriginCode as SECTORFROM,UsageDestinationCode as SECTORTO  ";
            sql = sql + ",iif(SpecialProrateAgreement = '0.00',finalshare,SpecialProrateAgreement) as FinalShare from pax.SalesDocumentCoupon sdc   ";
            sql = sql + "left join pax.ProrationDetail PR on sdc.RelatedDocumentGuid = pr.RelatedDocumentGuid and PR.ProrationFlag = 'F' and pr.CouponNumber = sdc.CouponNumber  ";
            sql = sql + "where cast(cast(UsageFlightNumber as int) as nvarchar  (5)) like  '" + flg + "' and UsageDate between  '" + dtpFrom + "'and '" + dtpTo + "'  and CouponStatus ='F'  ";
            sql = sql + "UNION  ";
            sql = sql + "select 'FIM' as REVENUETYPE, CONCAT(ALC,DOC) as RelatedDocumentNumber,CPN,cast(cast(FLIGHT as int) as nvarchar  (5))  as FightNumber,FLTDATE,SECTORFROM,SECTORTO ,AMOUNT as FinalShare  ";
            sql = sql + "from Pax.OutwardBilling where PMC = '14' and cast(cast(FLIGHT as int) as nvarchar  (5)) like  '" + flg + "'  and FLTDATE between '" + dtpFrom + "'and '" + dtpTo + "'   ";
            sql = sql + "UNION  ";
            sql = sql + "select 'EXCESS BAGGAGE' as REVENUETYPE, CONCAT(ALC,DOC) as RelatedDocumentNumber,CPN,cast(cast(FLIGHT as int) as nvarchar  (5)) as FightNumber,FLTDATE,SECTORFROM,SECTORTO ,AMOUNT as FinalShare  ";
            sql = sql + "from Pax.OutwardBilling where PMC = '25'  and cast(cast(FLIGHT as int) as nvarchar  (5)) like  '" + flg + "'  and FLTDATE between '" + dtpFrom + "'and '" + dtpTo + "'   ";


            SqlConnection con = new SqlConnection(pbConnectionString);
            DataSet ds = new DataSet();
            SqlDataAdapter ada = new SqlDataAdapter(sql, con);
            ada.Fill(ds);
            con.Close();

            int logListRevenue = ds.Tables[0].Rows.Count;

            string[,] ListeRevenue= new string[8, logListRevenue];

            int i = 0;

            foreach (DataRow dr in ds.Tables[0].Rows)
            {

                for (int j = 0; j < 8; j++)
                {
                    ListeRevenue[j, i] = dr[ds.Tables[0].Columns[j].ColumnName].ToString();
                }
               
                i++;
            }

            ViewBag.logListRevenue = logListRevenue;
            ViewBag.ListeRevenue = ListeRevenue;


            string[] cll = new string[logListRevenue];

            for (int g = 0; g < logListRevenue; g++)
            {
                cll[g] = ListeRevenue[0, g];   
            }

            string[] b = cll.Distinct().ToArray();


            string[,] dgsummary = new string[3, logListRevenue];

            int compteur = 0;
            for (int y = 0; y < b.Length; y++)
            {
                dgsummary[1,y] = b[y].ToString();
                compteur++;
            }

            string[] sum = new string[b.Length];
            string[] cnt = new string[b.Length];


            for (int o = 0; o < compteur; o++)
            {
                for (int k = 0; k < logListRevenue; k++)
                {
                    if (dgsummary[1,o] == ListeRevenue[0, k])
                    {
                        if (sum[o] == null)
                        {
                            sum[o] = "0";
                        }
                       if (ListeRevenue[7, k]  != null && !string.IsNullOrWhiteSpace(ListeRevenue[7, k]))
                         {
                            sum[o] = (Convert.ToDecimal(sum[o].ToString()) + Convert.ToDecimal(ListeRevenue[7, k])).ToString();
                        }
                        if (cnt[o] == null)
                        {
                            cnt[o] = "0";
                        }

                        cnt[o] = (Convert.ToDecimal(cnt[o].ToString()) + 1).ToString();
                    }
                }
            }

            for (int yy = 0; yy < b.Length; yy++)
            {
                dgsummary[0, yy]= b[yy].ToString();
                dgsummary[1, yy] = cnt[yy].ToString();
                dgsummary[2, yy] = sum[yy].ToString();
            }

            ViewBag.logSummary = b.Length;
            ViewBag.dgsummary = dgsummary;

            Array.Clear(cll, 0, cll.Length);
            Array.Clear(b, 0, b.Length);
            Array.Clear(sum, 0, sum.Length);
            Array.Clear(cnt, 0, cnt.Length);
        }

        public void getFlightNo(string dtpFrom, string dtpTo)
        {
            string sql = "select distinct cast(cast(UsageFlightNumber as int) as nvarchar  (5)) as FightNumber   ";
            sql = sql + "from pax.SalesDocumentCoupon where  UsageFlightNumber <>'OPEN'  and UsageDate between  '" + dtpFrom + "'and '" + dtpTo + "'      ";
            sql = sql + "order by cast(cast(UsageFlightNumber as int) as nvarchar  (5)) desc   ";


            SqlConnection con = new SqlConnection(pbConnectionString);
            DataSet ds = new DataSet();
            SqlDataAdapter ada = new SqlDataAdapter(sql, con);
            ada.Fill(ds);
            con.Close();

            int logFlighNo = ds.Tables[0].Rows.Count;

            string[,] ItemFlighNo = new string[1, logFlighNo];

            int i = 0;

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                ItemFlighNo[0, i] = dr[ds.Tables[0].Columns[0].ColumnName].ToString();

                i++;
            }

            ViewBag.logFlighNo = logFlighNo;
            ViewBag.ItemFlighNo = ItemFlighNo;
        }
        /* End Revenue Analysis   Joseph*/

        /* Discount Uncollected   Joseph */
        public ActionResult DiscountUncollected()
        {

            string dateFrom = DateTime.Now.AddMonths(-1).ToString("dd-MMM-yyyy");
            string dateTo = DateTime.Now.ToString("dd-MMM-yyyy");

            ViewBag.dateFrom = dateFrom;
            ViewBag.dateTo = dateTo;

            return PartialView();
        }

        public ActionResult ChangeDateUncollected()
        {
            string dateFrom = Request["valdateFrom"];
            string dateTo = Request["valdateTo"];

            AgentNumCodeUncollected(dateFrom, dateTo);

            ViewBag.dateFrom = dateFrom;
            ViewBag.dateTo = dateTo;

            return PartialView("DiscountUncollected");
        }

        public void AgentNumCodeUncollected(string dtpFrom, string dtpTo  )
        {

            string sql = " select distinct sdh.AgentNumericCode  from [Pax].[SalesDocumentHeader] sdh   " + Environment.NewLine;
            sql = sql + " join Pax.SalesRelatedDocumentInformation srd on sdh.HdrGuid = srd.HdrGuid   " + Environment.NewLine;
            sql = sql + " left join Pax.SalesDocumentCoupon sdc on srd.RelatedDocumentGuid = sdc.RelatedDocumentGuid   " + Environment.NewLine;
            sql = sql + " where  sdc.FlightDepartureDate between '" + dtpFrom + "' and '" + dtpTo + "' and sdc.CouponStatus = 'F'   " + Environment.NewLine;

            SqlConnection con = new SqlConnection(pbConnectionString);
            DataSet ds = new DataSet();
            SqlDataAdapter ada = new SqlDataAdapter(sql, con);
            ada.Fill(ds);
            con.Close();

            int logAgentCode = ds.Tables[0].Rows.Count;

            string[,] ItemAgentCodeU = new string[1, logAgentCode];

            int i = 0;

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                ItemAgentCodeU[0, i] = dr[ds.Tables[0].Columns[0].ColumnName].ToString();

                i++;
            }

            ViewBag.logAgentCode = logAgentCode;
            ViewBag.ItemAgentCodeU = ItemAgentCodeU;
        }

        public ActionResult RecherchAgentNumCode()
        {
            string dateFrom = Request["valdateFrom"];
            string dateTo = Request["valdateTo"];
            string agentNumericCode = Request["valdateTo"];


            if (agentNumericCode == "-All-")
            {
                agDiscount = "%";
            }
            else
            {
                agDiscount = agentNumericCode;
            }

            AgentNumCodeUncollected(dateFrom, dateTo);

            Discount(dateFrom, dateTo, agDiscount);

            /* Erreur Requet Desktop
                 aginfo(agDiscount);
            */
            ViewBag.dateFrom = dateFrom;
            ViewBag.dateTo = dateTo;

            return PartialView("DiscountUncollected");
        }

        string agDiscount = "%";
        string page = "1";
        string record = "150";
       
        public void Discount(string dtpFrom, string dtpTo, string agDiscount)
        {

            string sql = " DECLARE @PageNo int = '" + page + " ';    " + Environment.NewLine;
            sql = sql + " DECLARE @RecordsPerPage int = '" + record + " ';   " + Environment.NewLine;
            sql = sql + " with A as   " + Environment.NewLine;
            sql = sql + " ( SELECT [DateofIssue],[CheckDigit],[DocumentNumber],[AgentNumericCode] ,[PassengerName],[FareCalculationArea],pax.FOP_Combined ([HdrGuid]) as FOP  " + Environment.NewLine;
            sql = sql + " ,pax.fn_FareBasis_Combined ([HdrGuid]) as FAREBASIS,FareCalculationModeIndicator as FCMI  " + Environment.NewLine;
            sql = sql + " ,iif( exists (select * from Pax.SalesDocumentPayment sdp where sdp.HdrGuidRef = sdh.HdrGuid and sdp.FormofPaymentType = 'EX'),'Y','N') as Exchange  " + Environment.NewLine;
            sql = sql + " ,iif( ([AmountCollected]-([TaxCollected]+[SurchargeCollected])) = '0.00','Y','N') as l ,iif( (AmountCollected is null or AmountCollected ='0.00'),'Y','N') as m  " + Environment.NewLine;
            sql = sql + " ,[FareCurrency],[Fare],[TotalCurrency] As EquivalCurr,[EquivalentFare]  " + Environment.NewLine;
            sql = sql + " ,[TaxCollectedCurrency],[TaxCollected],[SurchargeCollectedCurrency],[SurchargeCollected],[AmountCollected],[AmountCollectedCurrency]  " + Environment.NewLine;
            sql = sql + " FROM [Pax].[SalesDocumentHeader] sdh where FareCalculationArea <>'' and SalesDataAvailable = '1' and OwnTicket = 'y' )  " + Environment.NewLine;
            sql = sql + " select [DateofIssue],[CheckDigit],[DocumentNumber],[AgentNumericCode] ,[PassengerName],[FareCalculationArea],FOP,FAREBASIS,FCMI  " + Environment.NewLine;
            sql = sql + " ,[FareCurrency],[Fare],EquivalCurr,[EquivalentFare]  " + Environment.NewLine;
            sql = sql + " ,[TaxCollectedCurrency],[TaxCollected],[SurchargeCollectedCurrency],[SurchargeCollected],[AmountCollectedCurrency], [AmountCollected]  " + Environment.NewLine;
            sql = sql + " from A where Exchange = 'N' and( l= 'Y' or m ='y') and DateofIssue between '" + dtpFrom + "' and '" + dtpTo + "'  " + Environment.NewLine;
            sql = sql + " and AgentNumericCode like '" + agDiscount + "'  " + Environment.NewLine;
            sql = sql + " order by DateofIssue,AgentNumericCode,DocumentNumber OFFSET (@PageNo-1)*@RecordsPerPage ROWS    " + Environment.NewLine;
            sql = sql + " FETCH NEXT @RecordsPerPage ROWS ONLY   " + Environment.NewLine;

            SqlConnection con = new SqlConnection(pbConnectionString);
            DataSet ds = new DataSet();
            SqlDataAdapter ada = new SqlDataAdapter(sql, con);
            ada.Fill(ds);
            con.Close();

            int logListDicount = ds.Tables[0].Rows.Count;

            string[,] ListeDicount = new string[19, logListDicount];

            int i = 0;

            foreach (DataRow dr in ds.Tables[0].Rows)
            {

                for( int j=0; j <15; j++)
                {
                    if (j == 0)
                    {
                        string h = null;
                        string hh = null;

                        h = dr[ds.Tables[0].Columns[j].ColumnName].ToString();

                        if (h != "")
                        {
                            hh = h.Substring(0, 11).ToString();
                            ListeDicount[j, i] = hh;
                        }
                    }
                    else
                    {
                        ListeDicount[j, i] = dr[ds.Tables[0].Columns[j].ColumnName].ToString();
                    }
                }

                i++;
            }

            ViewBag.logListDicount = logListDicount;
            ViewBag.ListeDicount = ListeDicount;

        }

        public void aginfo(string ag)
        {
            if (ag != "%")
            {
                SqlConnection con = new SqlConnection(pbConnectionString);

                string sql = " with a as (  " + Environment.NewLine;
                sql = sql + " select f1.AgencyNumericCode,f1.LegalName,f1.Locationaddress from Ref.agent f1 where f1.AgencyNumericCode like '" + ag + "'  " + Environment.NewLine;
                sql = sql + " union   " + Environment.NewLine;
                sql = sql + " select f2.AgencyNumericCode,f2.LegalName,f2.Locationaddress from Ref.Agent_Own f2 where f2.AgencyNumericCode like '" + ag + "'  " + Environment.NewLine;
                sql = sql + "   " + Environment.NewLine;
                sql = sql + " )  " + Environment.NewLine;
                sql = sql + "   " + Environment.NewLine;
                sql = sql + " select   " + Environment.NewLine;
                sql = sql + " isnull (f3.AgencyNumericCode,a.AgencyNumericCode) as AgencyNumericCode   " + Environment.NewLine;
                sql = sql + " ,isnull (f3.Name,a.LegalName) as LegalName   " + Environment.NewLine;
                sql = sql + " ,isnull (f3.Address,a.LocationAddress) as LegalAddress    " + Environment.NewLine;
                sql = sql + " from a  " + Environment.NewLine;
                sql = sql + " left join ref.PassengerAgencyDetails f3   " + Environment.NewLine;
                sql = sql + " on a.AgencyNumericCode = f3.AgencyNumericCode   " + Environment.NewLine;
                sql = sql + " where a.AgencyNumericCode like '" + ag + "'  " + Environment.NewLine;

                SqlCommand cmd = new SqlCommand(sql, con);
                con.Open();
                SqlDataReader rd = cmd.ExecuteReader();

                while (rd.Read())
                {
                    ViewBag.txtagtname = rd.GetValue(1).ToString();
                    ViewBag.txtagtadd = rd.GetValue(2).ToString();
                }
                rd.Close();
                con.Close();
            }
            else
            {
                ViewBag.txtagtname = "";
                ViewBag.txtagtadd = "";
            }
        }

        /* End Discount Uncollected   Joseph */
    }
}
 