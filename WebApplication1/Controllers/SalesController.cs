using System;
using System.Web.Mvc;
using WebApplication1.Models;
using System.Data.SqlClient;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using System.Globalization;
using System.Text;
using System.IO;
using System.Configuration;
using System.Text.RegularExpressions;
using Microsoft.VisualBasic;

namespace WebApplication1.Controllers
{
   
    public class SalesController : Controller
    {
        //public string pbConnectionString = "Server=.\\RELATE;Database=OnsiteBiatss_KK;User Id=sa; Password=1234";
        //public string pbConnectionString = "Server=FANO-PC;Database=OnsiteBiatss_KK;User Id=so; Integrated Security=True";
        // public string pbConnectionString = "Server=DESKTOP-54APPF8\\SYMPHONY2;Database=OnsiteBiatss_KK;User Id=sa; Password=1234";
        //public string pbConnectionString = "Server=DESKTOP-O0K2BQJ\\SA;Database=OnsiteBiatss_KK;User Id=sa; Password=1234";
        //public string pbConnectionString = "Server=DESKTOP-7HJUR50;Database=OnsiteBiatss_KK;User Id=sa; Password=1234";
        //public string pbConnectionString = "Server=DESKTOP-CGR76E3;Database=OnsiteBiatss_KK;User Id=sa; Password=1234";
        //public string pbConnectionString = "Server=DESKTOP-K56R42H;Database=OnsiteBiatss_KK;User Id=sa; Password=1234";
        //  public string pbConnectionString = "Server=DESKTOP-CGR76E3;Database=OnsiteBiatss_KK;User Id=sa; Password=1234";
       // public string pbConnectionString = "Server=DESKTOP-54APPF8\\SYMPHONY2;Database=OnsiteBiatss_KK;User Id=sa; Password=1234";
        public string pbConnectionString = "Server=DESKTOP-Q821GFS;Database=OnsiteBiatss_KK;User Id=sa; Password=1234";
        ConnexionSQLServer.DbConnection dbconnect = new ConnexionSQLServer.DbConnection(); 

        // GET: Sales
        public ActionResult Index()
        {
            dbconnect.pbConnectionString = pbConnectionString;
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

        public ActionResult Test()
        {
            return View();
        }

        public ActionResult PAXTKTs(string type)
        {
            List<PaxModel> model = new List<PaxModel>();
            //string dateFrom = DateTime.Now.AddMonths(-1).ToString("dd-MMM-yyyy");
            string dateTo = DateTime.Now.ToString("dd-MMM-yyyy");
            string[] date = new string[1] {  dateTo };
            ViewBag.date = date;
            ViewBag.type = type;
            return PartialView(model);
        }

        public ActionResult GetSearchCriteria()
        {
            string strSQL = "";
            strSQL = "SELECT top 1000 * FROM (SELECT ROW_NUMBER() OVER( ORDER BY [DocumentNumber]) AS RowNum " + Environment.NewLine;
            strSQL += ", sdh.DocumentNumber,sdh.CheckDigit,sdh.Tax1Amount,sdh.Tax2Amount, sdh.Tax3Amount, sdh.AgentNumericCode " + Environment.NewLine;
            strSQL += ", isnull( agtown.TradingName, agt.TradingName ) as [TradingName]" + Environment.NewLine;
            strSQL += ", sdh.DateofIssue,sdh.SaleDate" + Environment.NewLine;
            strSQL += ", isnull( " + Environment.NewLine;
            strSQL += "( select top 1 sdc.DomesticInternational from Pax.SalesRelatedDocumentInformation srd " + Environment.NewLine;
            strSQL += "join Pax.SalesDocumentCoupon sdc on srd.HdrGuid = sdh.HdrGuid and srd.RelatedDocumentGuid = sdc.RelatedDocumentGuid and sdc.DomesticInternational = 'I'" + Environment.NewLine;
            strSQL += ") , 'D') as [DomesticInternational]" + Environment.NewLine;
            strSQL += ", sdh.PassengerName, sdh.PassengerSpecificData, sdh.FareCalculationArea, sdh.EndosRestriction,sdh.Fare,sdh.FareCurrency" + Environment.NewLine;
            strSQL += ",concat(sdh.AmountCollectedCurrency, ' ',(sdh.[AmountCollected]-(isnull(sdh.[TaxCollected],0)+isnull(sdh.[SurchargeCollected],0)))) as [ComputedFare],sdh.EquivalentFare,sdh.TotalAmount,isnull(sdh.AmountCollectedCurrency ,   sdh.TotalCurrency) as [TotalCurrency]" + Environment.NewLine;
            strSQL += ",( select top 1 sdo.OtherAmountRate from Pax.SalesRelatedDocumentInformation srd join Pax.SalesDocumentOtherAmount sdo " + Environment.NewLine;
            strSQL += "on srd.HdrGuid = sdh.HdrGuid and srd.RelatedDocumentGuid = sdo.RelatedDocumentGuid and sdo.DocumentAmountType like 'Com%' and sdo.OtherAmountCode = 'Effective')as [CommRate]" + Environment.NewLine;
            strSQL += ", concat( sdh.CommissionCollectedCurrency, ' ', sdh.CommissionCollected ) as [Comm]" + Environment.NewLine;
            strSQL += " ,sdh.BookingAgentIdentification,sdh.BookingAgencyLocationNumber,sdh.BookingEntityOutletType,sdh.FareCalculationPricingIndicator, sdh.TransactionCode, sdh.FareCalculationModeIndicator" + Environment.NewLine;
            strSQL += ",sdh.AmountCollectedCurrency, sdh.AmountCollected,sdh.TaxOnCommissionCollected" + Environment.NewLine;
            strSQL += "FROM Pax.SalesDocumentHeader sdh left join Ref.Agent_Own agtown	on left(sdh.AgentNumericCode,7) = agtown.AgencyNumericCode left join Ref.Agent	agt	on left(sdh.AgentNumericCode,7) = agt.AgencyNumericCode";

            string txtAgentNumericCode = Request["agentCode"];
            string txtDocumentNo = Request["docummentNo"];
            string txtPNRValue = Request["pnr"];
            string txtPaxName = Request["passengerName"];
            string radStartingWith = Request["radStartingWith"];
            string radContains = Request["radContains"];
            string dtpIssueDateFrom = Request["dtpIssueDateFrom"];
            string dtpIssueDateTo = Request["dtpIssueDateTo"];
            string datedtpIssueDateFrom = Request["datedtpIssueDateFrom"];
            string datedtpIssueDateTo = Request["datedtpIssueDateTo"];
            string ownOal = Request["ownOal"];
            string domeInt = Request["domeInt"];
            string type = Request["type"];
            bool hasAtleastOneOperator = false;
            string strOperator = " AND ";
            string condition = "";
            string notConditionCount = "";
            string nbResult = Request["nbResult"];
            string lastRow = Request["lastRow"];
            string between = "";
            string next = Request["next"];
            string preview = Request["preview"];
            string totalPage = Request["totalPage"];

            if (String.IsNullOrWhiteSpace(next))
            {
                next = "1";
                preview = "1";
            }

            if (!String.IsNullOrWhiteSpace(txtPaxName))
            {
                string strPassengerName = string.Empty;
                if (!String.IsNullOrWhiteSpace(radContains) && radContains == "contains")
                    strPassengerName = strOperator + " sdh.PassengerName LIKE '%" + txtPaxName + "%'";

                if (!String.IsNullOrWhiteSpace(radContains) && radContains == "starting")
                    strPassengerName = strOperator + " sdh.PassengerName LIKE '" + txtPaxName + "%'";

                condition = condition + strPassengerName;
                hasAtleastOneOperator = true;
            }

            if (!String.IsNullOrWhiteSpace(txtPNRValue))
            {
                condition = condition + strOperator + " sdh.BookingReference LIKE '%" + txtPNRValue + "%'";
                hasAtleastOneOperator = true;
            }

            if (!String.IsNullOrWhiteSpace(txtDocumentNo))
            {
                condition = condition + strOperator + " sdh.DocumentNumber LIKE '%" + txtDocumentNo + "%' AND sdh.TransactionCode <> 'CANX'";
                hasAtleastOneOperator = true;
            }

            if (!String.IsNullOrWhiteSpace(txtAgentNumericCode))
            {
                condition = condition + strOperator + " sdh.AgentNumericCode LIKE '%" + txtAgentNumericCode + "%'";
                hasAtleastOneOperator = true;
            }

            if (dtpIssueDateFrom == "on" && dtpIssueDateTo == "on")
            {
                string strIssueDateFrom = String.Format("{0:MM/dd/yyyy}", datedtpIssueDateFrom);
                string strIssueDateTo = String.Format("{0:MM/dd/yyyy}", datedtpIssueDateTo);
                condition = condition + strOperator + " sdh.DateofIssue BETWEEN '" + strIssueDateFrom + "' AND '" + strIssueDateTo + "' ";
                hasAtleastOneOperator = true;
            }

            if (hasAtleastOneOperator == true)
            {

                switch (type)
                {
                    case "PAX TKTs":
                        notConditionCount = notConditionCount + strOperator + " sdh.TransactionCode = 'TKTT' ";
                        break;
                    case "MCOs":
                        notConditionCount = notConditionCount + strOperator + " sdh.DocumentType = 'MCO' ";
                        break;
                    case "ISSUES":
                        notConditionCount = notConditionCount + strOperator + " sdh.TransactionGroup = 'ISSUES' and sdh.OwnTicket = 'Y' ";
                        break;
                    case "ADMA":
                        notConditionCount = notConditionCount + strOperator + " sdh.TransactionCode = 'ADMA' ";
                        break;
                    case "ACMA":
                        notConditionCount = notConditionCount + strOperator + " sdh.TransactionCode = 'ACMA' ";
                        break;
                    case "MPDs":
                        notConditionCount = notConditionCount + strOperator + " sdh.DocumentType = 'VMPD' ";
                        break;
                    case "EBTs":
                        notConditionCount = notConditionCount + strOperator + " sdh.DocumentType = 'EBT' ";
                        break;
                   
                    case "EMDs":
                        notConditionCount = notConditionCount + strOperator + " sdh.TransactionCode = 'EMDS' ";
                        break;
                    case "Flown TKTs":
                        notConditionCount = notConditionCount + strOperator + " sdh.TransactionCode = 'TKTT' ";
                        break;
                    case "Flown EBTs":
                        notConditionCount = notConditionCount + strOperator + " sdh.DocumentType = 'EBT' ";
                        break;
                    case "Flown EMDs":
                        notConditionCount = notConditionCount + strOperator + " sdh.DocumentType = 'EMD' ";
                        break;
                    case "Flown MCOs":
                        notConditionCount = notConditionCount + strOperator + " sdh.DocumentType = 'MCO' ";
                        break;
                    case "RFND":
                        notConditionCount = notConditionCount + strOperator + " sdh.TransactionCode = 'RFND' ";
                        break;
                    default:
                        notConditionCount = notConditionCount + strOperator + " sdh.TransactionCode = " + "'" + type + "'";
                        break;
                }
            }

            if (!String.IsNullOrEmpty(domeInt) && domeInt == "Int")
            {

                condition += "and isnull(( select top 1 sdc.DomesticInternational from Pax.SalesRelatedDocumentInformation srd" + Environment.NewLine;
                condition += "join Pax.SalesDocumentCoupon sdc on srd.HdrGuid = sdh.HdrGuid and srd.RelatedDocumentGuid = sdc.RelatedDocumentGuid and sdc.DomesticInternational = 'I'" + Environment.NewLine;
                condition += "), 'D') = 'I'" + Environment.NewLine;
            }
            else
                if (!String.IsNullOrEmpty(domeInt) && domeInt == "Dom")
            {
                condition += "and isnull(( select top 1 sdc.DomesticInternational from Pax.SalesRelatedDocumentInformation srd" + Environment.NewLine;
                condition += "join Pax.SalesDocumentCoupon sdc on srd.HdrGuid = sdh.HdrGuid and srd.RelatedDocumentGuid = sdc.RelatedDocumentGuid and sdc.DomesticInternational = 'I'" + Environment.NewLine;
                condition += "), 'D') = 'D'" + Environment.NewLine;
            }

            if (!String.IsNullOrEmpty(ownOal) && ownOal == "Own")
            {
                condition += "and ownticket = 'Y'" + Environment.NewLine;
            }
            else
                if (!String.IsNullOrEmpty(ownOal) && ownOal == "Oal")
            {
                condition += "and ownticket = 'N'" + Environment.NewLine;
            }

            if (!String.IsNullOrEmpty(lastRow) && !String.IsNullOrEmpty(nbResult))
            {
                if (Request["nextOrPreview"] == "next")
                {
                    int setNext = Int32.Parse(next) + 1;
                    int val = setNext * 1000;
                    int bigin = val - 999;
                    between = "WHERE rownum BETWEEN " + bigin + " AND " + val + "";
                    next = (Int32.Parse(next) + 1).ToString();
                    preview = (Int32.Parse(preview) + 1).ToString();
                }
                if (Request["nextOrPreview"] == "preview")
                {
                    int setNext = Int32.Parse(next) - 1;
                    int val = setNext * 1000;
                    int bigin = val - 999;
                    between = "WHERE rownum BETWEEN " + bigin + " AND " + val + "";
                    next = (Int32.Parse(next) - 1).ToString();
                    preview = (Int32.Parse(preview) - 1).ToString();

                }

                if (Request["nextOrPreview"] == "first")
                {
                    int val = 1000;
                    int bigin = 0;
                    between = "WHERE rownum BETWEEN " + bigin + " AND " + val + "";
                    next = (Int32.Parse(next) - 1).ToString();
                    preview = (Int32.Parse(preview) - 1).ToString();

                }
                if (Request["nextOrPreview"] == "last")
                {
                    int setNext = Int32.Parse(totalPage);
                    int val = setNext * 1000;
                    int bigin = val - 999;
                    between = "WHERE rownum BETWEEN " + bigin + " AND " + val + "";
                    next = (Int32.Parse(totalPage)).ToString();
                    preview = (Int32.Parse(totalPage)).ToString();

                }

            }
            strSQL = strSQL + " WHERE 1=1 " + condition + notConditionCount + ")AS rows " + between + " order by RowNum";
            if (type == "PAX TKTs")
            {
                type = "TKTT";
            }
            string countPax = RecCount(condition);
            ViewBag.countTransaction = GetDataRCount(countPax);
            ViewBag.valueTransaction = GetData(strSQL);
            ViewBag.next = next;
            ViewBag.preview = preview;

            ViewBag.type = type;
            return PartialView();

        }

        private string RecCount(string condition)
        {

            string strSQL = "SELECT count(*) as docCount, sdh.TransactionCode as docType" + Environment.NewLine;
            strSQL += "FROM Pax.SalesDocumentHeader sdh left join Ref.Agent_Own agtown	on left(sdh.AgentNumericCode,7) = agtown.AgencyNumericCode left join Ref.Agent	agt	on left(sdh.AgentNumericCode,7) = agt.AgencyNumericCode WHERE 1=1";
            strSQL += condition;
            strSQL += "Group by sdh.TransactionCode";
            return strSQL;
        }

        private List<PaxModel> GetData(string StrSQL)
        {
            dbconnect.pbConnectionString = pbConnectionString;
            SqlDataReader myReader = dbconnect.GetData(StrSQL);

            List<PaxModel> model = new List<PaxModel>();
            while (myReader.Read())
            {
                var details = new PaxModel();
                details.RowNum = myReader["RowNum"].ToString();
                details.DocumentNumber = myReader["DocumentNumber"].ToString();
                details.CheckDigit = myReader["CheckDigit"].ToString();
                details.Tax1Amount = myReader["Tax1Amount"].ToString();
                details.Tax2Amount = myReader["Tax2Amount"].ToString();
                details.Tax3Amount = myReader["Tax3Amount"].ToString();
                details.AgentNumericCode = myReader["AgentNumericCode"].ToString();
                details.TradingName = myReader["TradingName"].ToString();
                if (!String.IsNullOrEmpty(myReader["DateofIssue"].ToString()))
                    details.DateofIssue = myReader["DateofIssue"].ToString().Substring(0, 10);
                else
                    details.DateofIssue = myReader["DateofIssue"].ToString();
                if (!String.IsNullOrEmpty(myReader["SaleDate"].ToString()))
                    details.DateofIssue = myReader["SaleDate"].ToString().Substring(0, 10);
                else
                    details.DateofIssue = myReader["SaleDate"].ToString();
                details.DomesticInternational = myReader["DomesticInternational"].ToString();
                details.PassengerName = myReader["PassengerName"].ToString();
                details.PassengerSpecificData = myReader["PassengerSpecificData"].ToString();
                details.FareCalculationArea = myReader["FareCalculationArea"].ToString();
                details.EndosRestriction = myReader["EndosRestriction"].ToString();
                details.Fare = myReader["Fare"].ToString();
                details.FareCurrency = myReader["FareCurrency"].ToString();
                details.ComputedFare = myReader["ComputedFare"].ToString();
                details.EquivalentFare = myReader["EquivalentFare"].ToString();
                details.TotalAmount = myReader["TotalAmount"].ToString();
                details.TotalCurrency = myReader["TotalCurrency"].ToString();
                details.Comm = myReader["Comm"].ToString();
                details.BookingAgentIdentification = myReader["BookingAgentIdentification"].ToString();
                details.BookingEntityOutletType = myReader["BookingEntityOutletType"].ToString();
                details.FareCalculationPricingIndicator = myReader["FareCalculationPricingIndicator"].ToString();
                details.TransactionCode = myReader["TransactionCode"].ToString();
                details.FareCalculationModeIndicator = myReader["FareCalculationModeIndicator"].ToString();
                details.AmountCollectedCurrency = myReader["AmountCollectedCurrency"].ToString();
                details.AmountCollected = myReader["AmountCollected"].ToString();
                details.TaxOnCommissionCollected = myReader["TaxOnCommissionCollected"].ToString();
                model.Add(details);
            }
            return model;
        }

        private List<RCountPaxModel> GetDataRCount(string StrSQL)
        {
            dbconnect.pbConnectionString = pbConnectionString;
            SqlDataReader myReaderCount = dbconnect.GetData(StrSQL);

            List<RCountPaxModel> model = new List<RCountPaxModel>();
            while (myReaderCount.Read())
            {
                var details = new RCountPaxModel();
                details.docCount = myReaderCount["docCount"].ToString();
                details.docType = myReaderCount["docType"].ToString();
                model.Add(details);
            }
            return model;
        }

        public ActionResult Transaction()
        {
            string docNumber = Request["docNumber"];
            string fistDocNumber = docNumber.Substring(0, 3);
            string secondDocNumber = docNumber.Substring(3, 10);
            string transactionCode = Request["transactionCode"];
            string sql = "SELECT FORMAT(F1.DateofIssue,'dd-MMM-yyyy') as DateOfIssue ,F1.*, F2.* FROM[Pax].[SalesDocumentHeader] F1 left join Ref.VW_Agent F2 on substring(F1.AgentNumericCode, 1, 7) =";
            sql += "substring(F2.AgencyNumericCode, 1, 7) where F1.DocumentNumber = '" + docNumber + "' ";
            sql += "and F1.TransactionCode = '" + transactionCode + "' ";

            string sqlMdl = "SELECT item.CouponNumber, item.StopOverCode,  item.Carrier, item.FlightNumber,  item.OriginAirportCityCode + '-' + item.DestinationAirportCityCode as GFPA, ";
            sqlMdl += "FORMAT(item.FlightDepartureDate,'dd-MMM-yyyy') as FlightDepartureDate, item.FlightDepartureTime, item.CouponStatus, item.FareBasisTicketDesignator,FORMAT(item.NotValidBefore,'dd-MMM-yyyy') as NotValidBefore,";
            sqlMdl += "FORMAT(item.NotValidAfter,'dd-MMM-yyyy') as NotValidAfter, item.FreeBaggageAllowance, item.FlightBookingStatus, item.UsedClassofService, ";
            sqlMdl += "item.UsageOriginCode + '-' + item.UsageDestinationCode as UsageSector ,item.UsageAirline, item.UsageFlightNumber, item.RelatedDocumentNumber ,F2.IsConjunction as isConjonction,";
            sqlMdl += "FORMAT(item.UsageDate,'dd-MMM-yyyy') as UsageDate, item.FrequentFlyerReference from Pax.SalesDocumentHeader F1 ";
            sqlMdl += "join Pax.SalesRelatedDocumentInformation F2 on F1.HdrGuid = F2.HdrGuid ";
            sqlMdl += "join Pax.SalesDocumentCoupon item on F2.RelatedDocumentGuid = item.RelatedDocumentGuid   where F1.DocumentNumber = '" + docNumber + "' order by item.RelatedDocumentNumber , item.CouponNumber";

            string cnj = "SELECT DocumentNumber, RelatedDocumentNumber from Pax.SalesRelatedDocumentInformation where DocumentNumber = '" + docNumber + "' and DocumentNumber <> RelatedDocumentNumber and TransactionCode = '" + transactionCode + "'";



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
        /*fait par christian*/
        public ActionResult Transaction2()
        {
            string docNumber = Request["docNumber"];
            string fistDocNumber = docNumber.Substring(0, 3);
            string secondDocNumber = docNumber.Substring(3, 10);
            string transactionCode = "";
            /*transaction*/
            string sqllaz = "SELECT [TransactionCode] FROM [Pax].[SalesRelatedDocumentInformation]  where RelatedDocumentNumber = '" + docNumber + "' and TransactionCode not like 'CAN%'";
            DataSet dsaz = new DataSet();
            dsaz = dbconnect.RetObjDS(sqllaz);
            foreach (DataRow draz in dsaz.Tables[0].Rows)
            {
                transactionCode = draz[dsaz.Tables[0].Columns[0].ColumnName].ToString();
            }
            /*fin transaction*/

            //string transactionCode = Request["transactionCode"];
            string sql = "SELECT FORMAT(F1.DateofIssue,'dd-MMM-yyyy') as DateOfIssue ,F1.*, F2.* FROM[Pax].[SalesDocumentHeader] F1 left join Ref.VW_Agent F2 on substring(F1.AgentNumericCode, 1, 7) =";
            sql += "substring(F2.AgencyNumericCode, 1, 7) where F1.DocumentNumber = '" + docNumber + "' ";
            sql += "and F1.TransactionCode = '" + transactionCode + "' ";

            string sqlMdl = "SELECT item.CouponNumber, item.StopOverCode,  item.Carrier, item.FlightNumber,  item.OriginAirportCityCode + '-' + item.DestinationAirportCityCode as GFPA, ";
            sqlMdl += "FORMAT(item.FlightDepartureDate,'dd-MMM-yyyy') as FlightDepartureDate, item.FlightDepartureTime, item.CouponStatus, item.FareBasisTicketDesignator,FORMAT(item.NotValidBefore,'dd-MMM-yyyy') as NotValidBefore,";
            sqlMdl += "FORMAT(item.NotValidAfter,'dd-MMM-yyyy') as NotValidAfter, item.FreeBaggageAllowance, item.FlightBookingStatus, item.UsedClassofService, ";
            sqlMdl += "item.UsageOriginCode + '-' + item.UsageDestinationCode as UsageSector ,item.UsageAirline, item.UsageFlightNumber, item.RelatedDocumentNumber ,F2.IsConjunction as isConjonction,";
            sqlMdl += "FORMAT(item.UsageDate,'dd-MMM-yyyy') as UsageDate, item.FrequentFlyerReference from Pax.SalesDocumentHeader F1 ";
            sqlMdl += "join Pax.SalesRelatedDocumentInformation F2 on F1.HdrGuid = F2.HdrGuid ";
            sqlMdl += "join Pax.SalesDocumentCoupon item on F2.RelatedDocumentGuid = item.RelatedDocumentGuid   where F1.DocumentNumber = '" + docNumber + "' order by item.RelatedDocumentNumber , item.CouponNumber";

            string cnj = "SELECT DocumentNumber, RelatedDocumentNumber from Pax.SalesRelatedDocumentInformation where DocumentNumber = '" + docNumber + "' and DocumentNumber <> RelatedDocumentNumber and TransactionCode = '" + transactionCode + "'";



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
        /*fin fait par christian*/


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
            } else
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
                transaction.DateofIssue ="";
                transaction.AgentNumericCode = "";
                transaction.BookingAgentIdentification = "";
                transaction.ReportingSystemIdentifier = "";
                transaction.VendorIdentification = "";
                transaction.LegalName ="";
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
        public ActionResult PayementTFCs()
        {
            return PartialView();
        }
        public ActionResult ViewMoreDetails()
        {
            return PartialView();
        }
        public ActionResult SPLITFCA()
        {
            string docNumber = Request["docNumber"];
            string farefca = Request["fare"];
            // string farefca = "IAS RO BUH Q3.28 121.52RO IST Q6.56 183.38KK IZM50.00KK IST50.00RO BUH Q6.56 183.38RO IAS Q3.28 121.52NUC729.48END ROE0.913391PD XT6.20HE24.32RO13.62TR";
            //string docNumber = "6109231062089";
            ViewData["document"] = docNumber;
            ViewData["fare"] = farefca;

            ViewData["parser"] = GetParseFca2(ParserFca(farefca, docNumber));
            string fca = farefca.Trim();
            ViewBag.tableau = GetValueKey(GetParseFca2(ParserFca(farefca, docNumber)), docNumber);
            ViewData["destfinal"] = getfca(ParserFca(farefca, docNumber));
            ViewBag.fareComponent = getFare(GetValueKey(GetParseFca2(ParserFca(farefca, docNumber)), docNumber), docNumber, getfca(ParserFca(farefca, docNumber)));
            ViewBag.CountFare = GetCountFare(GetValueKey(GetParseFca2(ParserFca(farefca, docNumber)), docNumber), getfca(ParserFca(farefca, docNumber)));
            ViewBag.Sector = GetSector(getFare(GetValueKey(GetParseFca2(ParserFca(farefca, docNumber)), docNumber), docNumber, getfca(ParserFca(farefca, docNumber))), GetValueKey(GetParseFca2(ParserFca(farefca, docNumber)), docNumber), GetParseFca2(ParserFca(farefca, docNumber)));
            /*******************Ticket Information********************/
            string sql = "SELECT  * " +
            "FROM [Pax].[SalesDocumentHeader]  where " +
            " DocumentNumber ='" + docNumber + "'";

            string Countsql = "SELECT COUNT(*) as countsql " +
            "FROM [Pax].[SalesDocumentHeader]  where " +
            " DocumentNumber ='" + docNumber + "'";

            string Csql = "Select SRI.* , SDC.* from pax.SalesDocumentHeader SDH " +
            " left join pax.SalesRelatedDocumentInformation SRI on SDH.HdrGuid = SRI.HdrGuid " +
            " left join pax.SalesDocumentCoupon SDC on SRI.RelatedDocumentGuid=SDC.RelatedDocumentGuid " +
            " where SDH.DocumentNumber = '" + docNumber + "'";

            string CountCsql = "Select COUNT(*) as countcsql from pax.SalesDocumentHeader SDH " +
            " left join pax.SalesRelatedDocumentInformation SRI on SDH.HdrGuid = SRI.HdrGuid " +
            " left join pax.SalesDocumentCoupon SDC on SRI.RelatedDocumentGuid=SDC.RelatedDocumentGuid " +
            " where SDH.DocumentNumber = '" + docNumber + "'";

            string Psql = " Select SRI.* , SDP.* from pax.SalesDocumentHeader SDH  " +
                  "left join pax.SalesRelatedDocumentInformation SRI on SDH.HdrGuid = SRI.HdrGuid  " +
                  "left join pax.SalesDocumentPayment SDP on SRI.RelatedDocumentGuid=SDP.RelatedDocumentGuid " +
                  " where SDH.DocumentNumber = '" + docNumber + "'";

            string CountPsql = " Select COUNT(*) as countpsql from pax.SalesDocumentHeader SDH  " +
                  "left join pax.SalesRelatedDocumentInformation SRI on SDH.HdrGuid = SRI.HdrGuid  " +
                  "left join pax.SalesDocumentPayment SDP on SRI.RelatedDocumentGuid=SDP.RelatedDocumentGuid " +
                  " where SDH.DocumentNumber = '" + docNumber + "'";


            string OPsql = "Select SDOA.* from pax.SalesDocumentHeader SDH " +
                "left join pax.SalesRelatedDocumentInformation SRI on SDH.HdrGuid = SRI.HdrGuid  " +
                "left join pax.SalesDocumentOtherAmount SDOA on SRI.RelatedDocumentGuid=SDOA.RelatedDocumentGuid  " +
                " where SDH.DocumentNumber = '" + docNumber + "'";

            string CountOPsql = "Select COUNT(*) as countopsql from pax.SalesDocumentHeader SDH " +
                "left join pax.SalesRelatedDocumentInformation SRI on SDH.HdrGuid = SRI.HdrGuid  " +
                "left join pax.SalesDocumentOtherAmount SDOA on SRI.RelatedDocumentGuid=SDOA.RelatedDocumentGuid  " +
                " where SDH.DocumentNumber = '" + docNumber + "'";

            string PDsql = "Select PD.* from pax.SalesDocumentHeader SDH " +
                   " left join pax.SalesRelatedDocumentInformation SRI on SDH.HdrGuid = SRI.HdrGuid " +
                   " left join pax.ProrationDetail PD on SRI.RelatedDocumentGuid=PD.RelatedDocumentGuid " +
                   " where SDH.DocumentNumber = '" + docNumber + "'";

            string CountPDsql = "Select COUNT(*) as countpdsql from pax.SalesDocumentHeader SDH " +
                   " left join pax.SalesRelatedDocumentInformation SRI on SDH.HdrGuid = SRI.HdrGuid " +
                   " left join pax.ProrationDetail PD on SRI.RelatedDocumentGuid=PD.RelatedDocumentGuid " +
                   " where SDH.DocumentNumber = '" + docNumber + "'";

            string PEsql = "Select PE.* from pax.SalesDocumentHeader SDH " +
                       " left join pax.SalesRelatedDocumentInformation SRI on SDH.HdrGuid = SRI.HdrGuid" +
                       " left join pax.[ProrationException] PE on SRI.RelatedDocumentGuid=PE.RelatedDocumentGuid" +
                       " where SDH.DocumentNumber = '" + docNumber + "'";

            string CountPEsql = "Select COUNT(*) as countpesql from pax.SalesDocumentHeader SDH " +
                       " left join pax.SalesRelatedDocumentInformation SRI on SDH.HdrGuid = SRI.HdrGuid" +
                       " left join pax.[ProrationException] PE on SRI.RelatedDocumentGuid=PE.RelatedDocumentGuid" +
                       " where SDH.DocumentNumber = '" + docNumber + "'";

            string Exsql = " Select DocumentNumber,OriginalIssueDocumentNumber  from pax.SalesDocumentHeader SDH " +
                             " where DocumentNumber = '" + docNumber + "'";

            string CountExsql = " Select COUNT(*)  as countexsql from pax.SalesDocumentHeader SDH " +
                             " where DocumentNumber = '" + docNumber + "'";

            /*****************GetArrCPNFareBasis***************************/

            string ArrCPNFareBasis = "Select   distinct SDC.FareBasisTicketDesignator from pax.SalesDocumentHeader SDH " +
                " left join pax.SalesRelatedDocumentInformation SRI on SDH.HdrGuid = SRI.HdrGuid " +
                " left join pax.SalesDocumentCoupon SDC on SRI.RelatedDocumentGuid=SDC.RelatedDocumentGuid " +
                " where SDH.DocumentNumber ='" + docNumber + "'";

            ViewBag.ArrCPNFareBasis = GetFareBasis(ArrCPNFareBasis);
            ViewBag.Ticket = GetTicket(sql);
            ViewBag.Coupons = GetCoupons(Csql);
            ViewBag.Payement = GetPayement(Psql);
            ViewBag.OtherPayement = GetOtherPayement(OPsql);
            ViewBag.ProrationDtl = GetProratioDtl(PDsql);
            ViewBag.ProrationExp = GetProrationExp(PEsql);
            ViewBag.Exchange = GetExchange(Exsql);

            dbconnect.pbConnectionString = pbConnectionString;
            SqlDataReader myReader = dbconnect.GetData(Countsql);
            while (myReader.Read())
            {
                ViewBag.Countsql = Convert.ToInt16(myReader["countsql"].ToString());
            }

            SqlDataReader myReadercsql = dbconnect.GetData(CountCsql);
            while (myReadercsql.Read())
            {
                ViewBag.CountCsql = Convert.ToInt16(myReadercsql["countcsql"].ToString());
            }

            SqlDataReader myReaderPsql = dbconnect.GetData(CountPsql);
            while (myReaderPsql.Read())
            {
                ViewBag.CountPsql = Convert.ToInt16(myReaderPsql["countpsql"].ToString());
            }

            SqlDataReader myReaderOPsql = dbconnect.GetData(CountOPsql);
            while (myReaderOPsql.Read())
            {
                ViewBag.CountOPsql = Convert.ToInt16(myReaderOPsql["countopsql"].ToString());
            }

            SqlDataReader myReaderPDsql = dbconnect.GetData(CountPDsql);
            while (myReaderPDsql.Read())
            {
                ViewBag.CountPDsql = Convert.ToInt16(myReaderPDsql["countpdsql"].ToString());
            }

            SqlDataReader myReaderPEsql = dbconnect.GetData(CountPEsql);
            while (myReaderPEsql.Read())
            {
                ViewBag.CountPEsql = Convert.ToInt16(myReaderPEsql["countpesql"].ToString());
            }

            SqlDataReader myReaderExsql = dbconnect.GetData(CountExsql);
            while (myReaderExsql.Read())
            {
                ViewBag.CountExsql = Convert.ToInt16(myReaderExsql["countexsql"].ToString());
            }

            var obs = getObservation(docNumber);
            return PartialView(obs);
        }
        private string ParserFca(string fca, string docNumber)
        {
            string farebasis = "Select distinct SDC.FareBasisTicketDesignator from pax.SalesDocumentHeader SDH " +
                " left join pax.SalesRelatedDocumentInformation SRI on SDH.HdrGuid = SRI.HdrGuid " +
                " left join pax.SalesDocumentCoupon SDC on SRI.RelatedDocumentGuid=SDC.RelatedDocumentGuid " +
                " where SDH.DocumentNumber ='" + docNumber + "'";
            dbconnect.pbConnectionString = pbConnectionString;
            SqlDataReader myReaderMore = dbconnect.GetData(farebasis);
            List<ArrCPNFareBasis> ListFare = new List<ArrCPNFareBasis>();
            while (myReaderMore.Read())
            {
                var details = new ArrCPNFareBasis();
                details.FareBasisTicketDesignator = myReaderMore["FareBasisTicketDesignator"].ToString();
                ListFare.Add(details);
            }
            string fca2 = "";
            string Pat = "";
            string Word = "";
            int Entry = 0;
            for (int f = 0; f < GetFareBasis(farebasis).Count; f++)
            {

                int Locate = fca.IndexOf(ListFare[f].ToString());
                if (Locate > 0)
                {
                    fca = fca.Replace(ListFare[f].ToString(), "XXXXXXXX");

                }

            }
            for (int x = 0; x < fca.Length; x++)
            {

                string Letter = fca.Substring(x, 1);

                if ("0123456789.".Contains(Letter))
                {


                    if (Entry == 0) { Pat = Pat + "|"; fca2 = fca2 + "|"; }
                    Entry = 1; Pat = Pat + "9";
                    fca2 = fca2 + Letter;
                }
                else
                {

                    if (Entry == 1) { Pat = Pat + "|"; fca2 = fca2 + "|"; Entry = 0; }
                    if (Letter == " ") { Pat = Pat + " "; Letter = "|"; }
                    else { Pat = Pat + "X"; }
                    fca2 = fca2 + Letter;

                }
                try
                {

                    int c1 = -1;
                    c1 = fca2.IndexOf("*");
                    if (c1 != -1) { fca2 = fca2.Insert(c1 + 1, "|"); }
                    if (Pat.StartsWith("XX|99|XXX|99")) { fca2 = fca2.Substring(12, fca2.Length - 12); }
                    if (Pat.StartsWith("|99|XXX|99|")) { fca2 = fca2.Substring(11, fca2.Length - 11); }
                    if (Pat.StartsWith("X|99|XXX|99|")) { fca2 = fca2.Substring(13, fca2.Length - 13); }
                    if (Pat.StartsWith("|99|XXX XXX")) { fca2 = fca2.Substring(8, fca2.Length - 8); }
                    if (Pat.StartsWith("XX XXXXX XX |99|XXX")) { fca2 = fca2.Substring(19, fca2.Length - 19).Trim(); }
                    if (Pat.StartsWith("XX XXXXXXX XX|9999999999999|XX XXX |99|XXX|99|XXX XXX")) { fca2 = fca2.Substring(53, fca2.Length - 53).Trim(); }
                    int c4 = fca2.IndexOf("XT");
                    if (c4 > -1) { fca2 = fca2.Insert(c4, "|"); }
                    if (fca2.StartsWith("FP"))
                    {
                        c1 = fca2.IndexOf("FC");
                        fca2 = fca2.Substring(c1 + 2, fca2.Length - (c1 + 2));
                        c1 = fca2.IndexOf("JAN");
                        if (c1 == -1)
                        {
                            c1 = fca2.IndexOf("FEB");

                            if (c1 == -1)
                            {
                                c1 = fca2.IndexOf("MAR");
                            }

                            if (c1 == -1)
                            {
                                c1 = fca2.IndexOf("APR");
                            }

                            if (c1 == -1)
                            {
                                c1 = fca2.IndexOf("MAY");
                            }

                            if (c1 == -1)
                            {
                                c1 = fca2.IndexOf("JUN");
                            }

                            if (c1 == -1)
                            {
                                c1 = fca2.IndexOf("JUL");
                            }

                            if (c1 == -1)
                            {
                                c1 = fca2.IndexOf("AUG");
                            }

                            if (c1 == -1)
                            {
                                c1 = fca2.IndexOf("SEP");
                            }

                            if (c1 == -1)
                            {
                                c1 = fca2.IndexOf("OCT");
                            }

                            if (c1 == -1)
                            {
                                c1 = fca2.IndexOf("NOV");
                            }

                            if (c1 == -1)
                            {
                                c1 = fca2.IndexOf("DEC");
                            }
                        }

                        if (c1 > -1) { fca2 = fca2.Substring(c1 + 4, fca2.Length - (c1 + 4)); }
                    }
                }
                catch { }
            }

            int c2 = fca2.IndexOf("*");
            if (c2 > 0) { fca2 = fca2.Substring(0, c2); }
            c2 = fca2.IndexOf("MGA5");
            if (c2 > 0) { fca2 = fca2.Substring(0, c2); }
            fca2 = fca2.Replace("|MGA5", "|MGA?|");
            fca2 = fca2.Replace("Z|4", "Z4|");
            fca2 = fca2.Replace("G|9", "G9|");
            fca2 = fca2.Replace("|J|2", "|J2|");
            fca2 = fca2.Replace("|A|5", "|A?|");
            fca2 = fca2.Replace("|A|9", "|A?|");
            fca2 = fca2.Replace("||", "|");
            if (fca2.Substring(0, 1) == "*") { fca2 = fca2.Substring(2, fca2.Length - 2); }
            return fca2;
        }
        private string getfca(string fca2)
        {
            string facdestfinal = "";
            int pos = fca2.IndexOf("||");
            if (pos > -1)
            {
                var facdest = fca2.Substring(0, pos);
                facdestfinal = facdest.Replace('|', ' ');
            }
            else
            {
                facdestfinal = fca2.Replace('|', ' ');
            }

            return facdestfinal;
        }
        private String GetParseFca2(string fca2)
        {
            string fca1 = fca2.Replace("||", "|");
            string fca3 = "";
            fca2 = fca1;
            int pos = fca2.IndexOf("||");
            if (pos > -1)
            {
                string tempfca1 = fca2.Substring(0, pos);
                string tempfca = fca2.Substring(pos);
                string tempfca2 = tempfca.TrimStart('|');
                fca3 = tempfca1 + '|' + tempfca2;
            }
            else
            {
                fca3 = fca2;
            }
            return fca3;
        }
        private String[,] GetValueKey(string fca3, string docNumber)
        {
            string farebasis = "Select distinct SDC.FareBasisTicketDesignator from pax.SalesDocumentHeader SDH " +
                " left join pax.SalesRelatedDocumentInformation SRI on SDH.HdrGuid = SRI.HdrGuid " +
                " left join pax.SalesDocumentCoupon SDC on SRI.RelatedDocumentGuid=SDC.RelatedDocumentGuid " +
                " where SDH.DocumentNumber ='" + docNumber + "'";
            dbconnect.pbConnectionString = pbConnectionString;
            SqlDataReader myReaderMore = dbconnect.GetData(farebasis);
            List<ArrCPNFareBasis> ListFare = new List<ArrCPNFareBasis>();
            while (myReaderMore.Read())
            {
                var details = new ArrCPNFareBasis();
                details.FareBasisTicketDesignator = myReaderMore["FareBasisTicketDesignator"].ToString();
                ListFare.Add(details);
            }


            string[] datapart = fca3.Split('|');
            int ArrayLength = datapart.Length + 1;
            String[,] tableau = new String[2, ArrayLength];

            int V = 0;
            for (int x = 0; x < ArrayLength - 1; x++)
            {
                // tableau.Add();

                if (datapart[x].ToString().Trim() == "XXXXXXXX")
                {
                    datapart[x] = ListFare[V].ToString();
                    V++;
                    tableau[0, x] = "Fare Basis";
                }


                tableau[0, x] = "Unknown";
                tableau[1, x] = datapart[x].ToString();
            }
            /***********************Airport*******************************/

            string Airports = "SELECT  [AirportCode]  FROM [Ref].[City]";
            dbconnect.pbConnectionString = pbConnectionString;
            SqlDataReader myReaderAirports = dbconnect.GetData(Airports);
            //List<ArrAirports> Airport = new List<ArrAirports>();
            string[] array = new string[] { };

            while (myReaderAirports.Read())
            {
                //var details = new ArrAirports();
                //details.AirportCode = myReaderAirports["AirportCode"].ToString();
                //Airport.Add(details);
                array = array.Concat(new string[] { myReaderAirports["AirportCode"].ToString() }).ToArray();


            }
            int IsAirport = -1;

            for (int R = 0; R < ArrayLength - 1; R++)
            {

                try
                {
                    IsAirport = -1;
                    string Data = tableau[1, R].ToString().Trim();
                    string Data1 = tableau[1, R].ToString().Trim();
                    Data = Data.Replace("X/E/", "");
                    Data = Data.Replace("X/", "");
                    Data = Data.Replace("/-", "");
                    Data = Data.Replace("//", "");
                    Data = Data.Replace("/*", "");
                    Data = Data.Replace("*/", "");
                    Data = Data.Replace("(", "");
                    Data = Data.Replace(")", "");
                    Data = Data.Replace("I-", "");
                    Data = Data.Replace("/E", "");
                    Data = Data.Replace("//", "");
                    Data = Data.Replace("/*", "");
                    Data = Data.Replace("*/", "");
                    Data = Data.Replace("(", "");
                    Data = Data.Replace(")", "");
                    Data = Data.Replace("I-", "");
                    if (Data.ToString().Equals("END"))
                    {
                        tableau[0, R] = "END";
                        tableau[1, R] = " ";
                    }

                    if (Data.ToString().Equals("ROE"))
                    {
                        tableau[0, R] = "ROE";
                        tableau[1, R] = tableau[1, R + 1];
                        tableau[1, R + 1] = "Unknown";
                        tableau[1, R + 1] = null;
                    }
                    if (Data.Contains("X/") || Data.Contains("/-") || Data.Contains("//") || Data.Contains("/*") || Data.Contains("*/") || Data.Contains("(") || Data.Contains(")"))
                    {
                        // string[] array = Airport.ToArray();
                        string str = Data.Substring(2, 3);
                        IsAirport = Array.BinarySearch(array, str);

                        tableau[0, R] = "City";

                        if (Data1.Contains("X/")) { tableau[0, R] = "Transit City"; }
                        if (Data1.Contains("/-")) { tableau[0, R] = "Surface/City"; }
                    }
                    else
                    {
                        if (Data.Length == 3)
                        {
                            IsAirport = Array.BinarySearch(array, Data.Substring(0, 3));

                            if (IsAirport > -1)
                            {
                                tableau[0, R] = "City";

                                if (Data1.Contains("X/")) { tableau[0, R] = "Transit City"; }
                                if (Data1.Contains("/-")) { tableau[0, R] = "Surface/City"; }
                            }
                        }



                        var T1 = tableau[0, R];
                        if (T1 == null)
                        {
                            if (IsAirport > -1)
                            {
                                if (tableau[1, R].ToString() != "MGA")
                                {
                                    tableau[0, R] = "City";
                                    if (Data1.Contains("X/")) { tableau[0, R] = "Transit City"; }
                                    if (Data1.Contains("/-")) { tableau[0, R] = "Surface/City"; }
                                }
                                else { tableau[0, R] = "Unknown"; }
                            }
                            else { tableau[0, R] = "Unknown"; }
                        }
                    }
                }
                catch { }
            }
            /*******************************ChecCarrier**********************************/


            string CheckCarrier = "SELECT [AirlineCode] FROM [Ref].[Airlines]";
            dbconnect.pbConnectionString = pbConnectionString;
            SqlDataReader myReaderCarrier = dbconnect.GetData(CheckCarrier);
            //List<ArrAirports> Airport = new List<ArrAirports>();
            string[] carrier = new string[] { };

            while (myReaderCarrier.Read())
            {
                //var details = new ArrAirports();
                //details.AirportCode = myReaderAirports["AirportCode"].ToString();
                //Airport.Add(details);
                carrier = carrier.Concat(new string[] { myReaderCarrier["AirlineCode"].ToString() }).ToArray();
            }
            int IsCarrier = -1;

            for (int R = 0; R < ArrayLength - 1; R++)
            {
                if (tableau[1, R] == "END" || tableau[0, R] == "ROE") { break; }
                try
                {
                    var tmp = tableau[0, R];
                    if (tableau[0, R] == "Unknown" || tmp == null)
                    {
                        string Data = tableau[1, R].ToString().Trim();
                        Data = Data.Replace("X/E/", "");
                        Data = Data.Replace("X/", "");
                        Data = Data.Replace("/-", "");
                        Data = Data.Replace("//", "");
                        Data = Data.Replace("/*", "");
                        Data = Data.Replace("*/", "");
                        Data = Data.Replace("(", "");
                        Data = Data.Replace(")", "");
                        Data = Data.Replace("/E", "");
                        Data = Data.Replace("//", "");
                        Data = Data.Replace("/*", "");
                        Data = Data.Replace("*/", "");
                        Data = Data.Replace("(", "");
                        Data = Data.Replace(")", "");
                        if (Data.Length == 2)
                        {
                            IsCarrier = Array.BinarySearch(carrier, Data);
                            if (IsCarrier > -1) { tableau[0, R] = "Carrier"; }
                            else
                            {
                                // tableau[0, R]  = "Unknown"; 
                            }
                        }
                        else
                        {
                            if (Data.Length == 1)
                                if (tableau[0, R - 1].ToString().Trim().IndexOf("City") != -1)
                                {
                                    string Tmp = tableau[1, R].ToString().Trim() + tableau[1, R + 1].ToString().Trim();

                                    IsCarrier = Array.BinarySearch(carrier, Tmp);
                                    if (IsCarrier > -1)
                                    {
                                        tableau[0, R] = "Carrier";
                                        tableau[1, R] = Tmp;
                                        tableau[1, R + 1] = null;

                                        for (int R1 = 0; R1 < tableau.Length; R1++)
                                        {
                                            if (tableau[0, R1] == "Unknown" && (tableau[1, R1] == null || tableau[1, R1] == ""))
                                            {
                                                //tableau.Rows.RemoveAt(R1);
                                            }
                                        }
                                    }
                                    else { tableau[0, R] = "Unknown"; }
                                }
                        }
                    }
                }
                catch { }
            }
            /****************************************CheckFareComponent*****************************************/

            int X = 0;
            // int IsfareBasis = -1;

            for (int R = 0; R < ArrayLength; R++)
            {
                if (tableau[0, R] == "END" || tableau[0, R] == "ROE") { break; }
                try
                {
                    if (tableau[0, R].Equals("Unknown"))
                    {
                        if (tableau[1, R].ToString().Trim().IndexOf(".") != -1)
                        {
                            try
                            {
                                string Fc = tableau[1, R].ToString().Trim();
                                decimal tmp = Convert.ToDecimal(Fc, System.Globalization.CultureInfo.InvariantCulture);

                                if (tableau[0, R - 1].ToString().Contains("Fare Component"))
                                {
                                    tableau[0, R] = tableau[0, R - 1];
                                    tableau[0, R - 1] = "Unknown";
                                }
                                else
                                {
                                    X++;
                                    tableau[0, R] = "Fare Component " + X.ToString();
                                }

                            }
                            catch
                            {
                            }
                        }
                        else
                        {
                            /*try {

                               // decimal tmp = Convert.ToDecimal((tableau[1, R].ToString().Trim()));
                            //X++;
                            //tableau[0, R] = "Fare Component " + X.ToString(); }
                            catch { }*/
                        }
                    }
                }
                catch { }
                var c = tableau;
            }
            /****************************************chechFareBasis**************************************/

            string CheckFareBasis = "SELECT DISTINCT [FareBasisTicketDesignator] FROM pax.[SalesDocumentCoupon] where [FareBasisTicketDesignator] is not null  order by  [FareBasisTicketDesignator]";
            dbconnect.pbConnectionString = pbConnectionString;
            SqlDataReader myReaderBasis = dbconnect.GetData(CheckFareBasis);
            //List<ArrAirports> Airport = new List<ArrAirports>();
            string[] farebasis1 = new string[] { };

            while (myReaderBasis.Read())
            {
                //var details = new ArrAirports();
                //details.AirportCode = myReaderAirports["AirportCode"].ToString();
                //Airport.Add(details);
                farebasis1 = farebasis1.Concat(new string[] { myReaderBasis["FareBasisTicketDesignator"].ToString() }).ToArray();
            }
            int Y = 0;
            int IsfareBasis = -1;
            for (int R = 0; R < tableau.Length; R++)
            {
                if (tableau[0, R] == "END" || tableau[0, R] == "ROE") { break; }
                try
                {
                    if (tableau[0, R] == "Unknown")
                    {
                        string Data1 = tableau[0, R - 1].ToString().Trim();
                        string DataX = tableau[0, R].ToString().Trim();
                        string DataY = tableau[0, R + 1].ToString().Trim();
                        string DataA = "";
                        string DataB = "";
                        string DataC = "";
                        if (Data1 == "Unknown") { DataA = tableau[1, R - 1].ToString().Trim(); }
                        if (DataX == "Unknown") { DataB = tableau[1, R].ToString().Trim(); }
                        if (DataY == "Unknown") { DataC = tableau[1, R + 1].ToString().Trim(); }
                        string Data = DataA + DataB + DataC;
                        if (Data.Trim().Length > 0)
                        {
                            string LastSearch = "";
                            for (Y = 1; Y <= Data.Length; Y++)
                            {
                                string Sch = Data.Trim().Substring(0, X);
                                IsfareBasis = Array.BinarySearch(farebasis1, Sch);
                                if (IsfareBasis > -1)
                                {
                                    LastSearch = LastSearch + Sch.Trim();
                                }
                            }
                            string[] matchesA = farebasis1.Cast<string>().Where(i => i.StartsWith(LastSearch.Trim())).ToArray();
                            var b = matchesA;
                            if (matchesA.Count() == 1) { tableau[0, R] = "Fare Basis"; }
                            else { }
                        }
                    }
                }
                catch { }
                /*****************************************checkEnd*********************************************/
                string[] FCABKDN = new string[] { }; ;
                //bool XTPass = false;
                string pvCurrAmount = "";
                int IsCurrency = 0;
                string Checkcurrency = "SELECT [CurrISOCode] FROM [Ref].[Currency]  order by  [CurrISOCode]";
                dbconnect.pbConnectionString = pbConnectionString;
                SqlDataReader myReaderCurrency = dbconnect.GetData(Checkcurrency);
                //List<ArrAirports> Airport = new List<ArrAirports>();
                string[] currency = new string[] { };

                while (myReaderCurrency.Read())
                {
                    //var details = new ArrAirports();
                    //details.AirportCode = myReaderAirports["AirportCode"].ToString();
                    //Airport.Add(details);
                    currency = currency.Concat(new string[] { myReaderCurrency["CurrISOCode"].ToString() }).ToArray();
                }
                for (int Z = 0; Z < ArrayLength - 1; Z++)
                {
                    try
                    {
                        string Data5 = "";
                        string Data6 = "";
                        {
                            if (tableau[0, Z] == "Unknown")
                            {
                                if (tableau[1, Z] != "")
                                {
                                    Data5 = tableau[1, Z].ToString();
                                    Data6 = tableau[1, Z].ToString();
                                    {
                                        if (Data6.ToString().Equals("M"))
                                        {
                                            tableau[0, Z] = "Millage ";
                                            tableau[1, Z] = tableau[1, Z + 1];
                                            // tableau[1, Z + 1] = "";
                                            //int pos = ;
                                            int pos = Array.IndexOf(FCABKDN, "P");
                                            {
                                            }
                                        }
                                        if (Data6.ToString().Equals("P"))
                                        {
                                            tableau[0, Z] = "Plus Up ";
                                            int pos = Array.IndexOf(FCABKDN, "P");
                                            if (pos > -1)
                                            {
                                                string ele1 = FCABKDN[pos].ToString();
                                                string ele2 = FCABKDN[pos + 1].ToString();
                                                int IsAirport1 = Array.BinarySearch(array, ele2.Substring(0, 3));
                                                int IsAirport2 = Array.BinarySearch(array, ele2.Substring(3, 3));
                                                string ele3 = FCABKDN[pos + 2].ToString();
                                                if (IsAirport1 > 0 && IsAirport2 > 0)
                                                {
                                                    tableau[0, Z] += tableau[1, Z + 1].ToString();
                                                    tableau[1, Z] = ele3;
                                                    tableau[1, Z + 1] = "";
                                                    tableau[1, Z + 2] = "";
                                                    tableau[0, Z + 1] = "Unknown";
                                                    tableau[0, Z + 2] = "Unknown";
                                                }
                                            }
                                        }
                                        if (Data6.ToString().Equals("S"))
                                        {
                                            tableau[0, Z] = "Surplus ";
                                            tableau[1, Z] = tableau[1, Z + 1];
                                            tableau[1, Z + 1] = "";
                                        }
                                        if (Data6.ToString().Equals("D"))
                                        {
                                            tableau[0, Z] = "Differential ";
                                            //tableau[1, R]  = tableau[1, R + 1] ;
                                            //tableau[1, R + 1]  = "";
                                            try
                                            {
                                                decimal tmp = Convert.ToDecimal((tableau[1, Z + 1].ToString().Trim()));
                                                tableau[1, Z] = tableau[1, Z + 1];
                                                tableau[1, Z + 1] = "";
                                                // tableau[0, R + 1]  = "Unknown";
                                            }
                                            catch
                                            {
                                                tableau[0, Z] = tableau[0, Z] + "[" + tableau[1, Z + 1] + "]";
                                                tableau[1, Z] = tableau[1, R + 2];
                                                tableau[0, Z + 2] = "Unknown";
                                                tableau[0, Z + 1] = "Unknown";
                                                tableau[1, Z + 2] = "";
                                                tableau[1, Z + 1] = "";
                                            }
                                        }
                                        if (Data6.ToString().Equals("Q"))
                                        {
                                            tableau[0, Z] = "Q-Surcharge ";
                                            try
                                            {
                                                decimal tmp = Convert.ToDecimal((tableau[1, Z + 1].ToString().Trim()), System.Globalization.CultureInfo.InvariantCulture);
                                                tableau[1, Z] = tableau[1, Z + 1];
                                                tableau[1, Z + 1] = "";
                                                // tableau[0, R + 1]  = "Unknown";
                                            }
                                            catch
                                            {
                                                tableau[0, Z] = tableau[0, Z] + "[" + tableau[1, Z + 1] + "]";
                                                tableau[1, Z] = tableau[1, Z + 2];
                                                tableau[0, Z + 2] = "Unknown";
                                                tableau[0, Z + 1] = "Unknown";
                                                tableau[1, Z + 2] = "";
                                                tableau[1, Z + 1] = "";
                                            }
                                        }
                                        if (Data6.ToString().Equals("NUC"))
                                        {
                                            tableau[0, Z] = "NUC";
                                            tableau[1, Z] = tableau[1, Z + 1];
                                            tableau[1, Z + 1] = "";
                                            tableau[0, Z + 1] = "Unknown";
                                        }
                                        else
                                        {
                                            string Data = "";
                                            if (Data5.ToString().Length == 3)
                                            {
                                                IsCurrency = Array.BinarySearch(currency, Data5);
                                                if (IsCurrency > -1)
                                                {
                                                    tableau[0, Z] = "Currency";

                                                    for (int C = Z; C < Z + 3; C++)
                                                    {
                                                        try
                                                        {
                                                            decimal tmp = Convert.ToDecimal((tableau[1, C].ToString().Trim()));
                                                            tableau[1, Z] = Data5 + ":" + tmp.ToString();
                                                            tableau[0, C] = "Unknown";
                                                            tableau[1, C] = "";
                                                        }
                                                        catch { }
                                                    }
                                                    pvCurrAmount = tableau[1, Z + 2].ToString();
                                                    //  dataGridView2.Rows[R]
                                                    //  dataGridView2.Rows.Remove(R-1);

                                                    // if (Convert.ToBoolean(tableau.Rows[R].Cells[[yourCheckBoxColIndex] ) == true)
                                                    {
                                                        //tableau.RemoveAt(1,Z + 1);
                                                    }

                                                    break;
                                                }
                                                else
                                                {
                                                    tableau[0, Z] = "Unknown";
                                                }
                                            }
                                        }
                                        if (Data6.ToString().Contains("M/IT"))
                                        {
                                            tableau[0, Z] = tableau[1, Z];
                                            // tableau[1, R]  = tableau[1, R + 1] ;
                                        }
                                        if (Data6.ToString().Contains("/BT"))
                                        {
                                            tableau[0, Z] = tableau[1, Z];
                                            // tableau[1, R]  = tableau[1, R + 1] ;
                                        }
                                        if (Data6.ToString().Contains("PLUS"))
                                        {
                                            tableau[0, Z] = tableau[1, Z];
                                            // tableau[1, R]  = tableau[1, R + 1] ;
                                        }
                                        if (Data6.ToString().Contains("/IT"))
                                        {
                                            tableau[0, Z] = tableau[1, Z];
                                            // tableau[1, R]  = tableau[1, R + 1] ;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    catch
                    {
                        int x = 0;
                        x = -9099;
                    }
                }
            }
            var a = tableau;
            // FCABKDN = fca2.Split('|');
            /************************checktaxe*******************/

            bool XTPass = false;
            string Data2 = "";
            string Data3 = "";
            int Compt = 0;
            int Pos = 0;
            int pass = 0;
            for (Compt = 0; Compt < ArrayLength - 1; Compt++)
            {
                try
                {

                    Data2 = tableau[1, Compt].ToString();
                    Data3 = tableau[1, Compt].ToString();
                    if (Data3.Contains("*")) { Pos = Compt; break; }
                    if (Data3.ToString().Contains("XT") || Data3.ToString().Contains("XF") || Data3.ToString().Contains("PD"))
                    {
                        tableau[0, Compt] = "TAXES";
                        XTPass = true;
                        pass = 1;
                        tableau[1, Compt] = Data3;

                    }
                    else
                    {

                        if (XTPass == true)
                        {

                            string tmpa = tableau[1, Compt].ToString();
                            decimal tmp = 0;
                            try
                            {
                                tmp = decimal.Parse(tmpa, System.Globalization.CultureInfo.InvariantCulture);

                                var convertDecimal = Convert.ToDecimal(tmpa, System.Globalization.CultureInfo.InvariantCulture);
                            }
                            catch
                            {
                                int fff = 0;
                                fff = 9;
                            }
                            if (tableau[1, Compt].ToString().Trim().IndexOf(".") != -1 || tmp > 0)
                            {
                                try
                                {
                                    tableau[0, Compt] = tableau[1, Compt + 1];
                                    tableau[1, Compt + 1] = "";
                                    tableau[0, Compt + 1] = "Unknown";
                                }
                                catch { }
                            }
                            else
                            {
                                //tableau[0, Compt] = tableau[1, Compt +1];
                                //tableau[1, Compt + 1] = "";
                                //tableau[0, Compt + 1] = "Unknown";
                            }
                        }
                    }
                }
                catch { }
            }
            var tab = tableau;
            return tableau;
        }
        /**********************checksector********************************/
        private ArrayList GetSector(String[,] tableauFare, String[,] tableau, string fca3)
        {
            int Countfc = 0;
            int longueur = fca3.Length;
            for (int z = 0; z < tableau.Length - 1; z++)
            {
                if (tableau[0, z].ToString().Equals("ROE")) { break; }
                else
                {
                    if (tableau[0, z].ToString().Contains("Fare Component"))
                    {
                        Countfc = Countfc + 1;
                    }
                }
            }
            int n = 0;
            string[] tableausector = new string[5 * Countfc];
            String[][] tab = new string[Countfc][];
            string Start = "";
            string Carrier = "";
            string NextCity = "";
            string Q = "";
            string P = "";
            string Sector = "";
            Start = tableau[1, 0].ToString();
            int SecCount = 0;
            int fcCnt = 0;
            ArrayList mylistdef = new ArrayList();
            int Cell = 1;
            int R = 0;
            ArrayList FC1 = new ArrayList();
            string Data = "";
            int Pass = 0;
            int LastCol = 1;
            int CarrCnt = 0;
            string Line = "";
            string Previous = "";
            for (int x = 0; x < tableau.Length / 2; x++)
            {
                if (tableau[0, x].ToString().Equals("ROE"))
                {
                    break;
                }
                string Parts = tableau[0, x].ToString() + "xxxxxx";
                Parts = Parts.Substring(0, 4);
                if (Parts == "Tran") { Parts = "City"; }
                if (Parts == "Mill") { Parts = "Fare"; }
                switch (Parts)
                {
                    case "City":
                        if (Line.Trim().Length == 0)
                        {
                            NextCity = tableau[1, x].ToString();
                            Line += NextCity + "|";
                        }
                        else
                        {
                            NextCity = tableau[1, x].ToString();
                            Line += NextCity + "|";
                        }
                        Previous = Parts;
                        break;
                    case "Carr":
                        Carrier = tableau[1, x].ToString();
                        if (CarrCnt == 0) { Line += Carrier + "|"; }
                        else { Line += "^" + NextCity + "|" + Carrier + "|"; }
                        CarrCnt++;
                        break;
                    case "Q-Su":
                        Line += "q:" + tableau[1, x].ToString() + ":";
                        break;
                    case "Plus":
                        Line += "p:" + tableau[1, x].ToString() + ":";
                        break;
                    case "Fare":
                        string xxx = Data;
                        String A = tableau[0, x - 1].ToString();
                        String B = tableau[0, x].ToString();
                        String C = tableau[0, x + 1].ToString();
                        String A1 = tableau[1, x - 1].ToString();
                        String B2 = tableau[1, x].ToString();
                        String C1 = tableau[1, x + 1].ToString();
                        fcCnt++;
                        Line = fcCnt.ToString() + "+" + Line;
                        Line = Line.Replace("+^", "+");
                        if (Line.Length > 3)
                        {
                            FC1.Add(Line);
                        }

                        Line = "";
                        if (tableau[0, x + 1].ToString().Contains("Carrier"))
                        {
                            //Line += "%" + NextCity + "|"; ;


                        }
                        //  NextCity = dataGridView1[1, x].Value.ToString();
                        //    Data += NextCity + "|^" + NextCity + "|";
                        break;
                }
            }
            Data = Data.Replace("X/E/", "");
            Data = Data.Replace("X/", "");
            Data = Data.Replace("/-", "");
            Data = Data.Replace("//", "");
            Data = Data.Replace("/*", "");
            Data = Data.Replace("*/", "");
            Data = Data.Replace("(", "");
            Data = Data.Replace(")", "");
            Data = Data.Replace("I-", "");
            Data = Data.Replace("/E", "");
            Data = Data.Replace("//", "");
            Data = Data.Replace("/*", "");
            Data = Data.Replace("*/", "");
            Data = Data.Replace("(", "");
            Data = Data.Replace(")", "");
            int Row = 0;
            int GRow = 0;
            // int col=1;
            int i = -1;
            int sets = 0;
            foreach (string strData in FC1)
            {
                ArrayList list = new ArrayList();
                i++; GRow = 0;
                string str = strData.Substring(2, strData.Length - 2);
                int X1 = Convert.ToInt16(strData.Substring(0, 1));
                string[] Sec;
                if (str.IndexOf("^") > 0)
                {
                    Sec = str.Split('^');
                }
                else
                {
                    Sec = str.Split('^');
                }
                int X = Sec.Length; ;
                for (int z = 0; z < Sec.Length; z++)
                {
                    ArrayList mylist = new ArrayList();
                    if (tableausector.Length < z + 1)
                    {
                        //tableausector.Add();

                        GRow = tableausector.Length - 1;
                    }
                    else
                    { GRow = z; }
                    int col = 0;
                    if (i == 0) { col = 1; }
                    if (i == 1) { col = 6; }
                    if (i == 2) { col = 11; }
                    if (i == 3) { col = 16; }
                    if (i == 4) { col = 1; }
                    if (i == 5) { col = 1; }
                    if (i == 6) { col = 1; }
                    if (i == 7) { col = 1; }
                    if (i == 8) { col = 1; }
                    string[] SecA = Sec[z].ToString().Split('|');
                    for (int c = 0; c < SecA.Length; c++)
                    {
                        if (SecA[c].ToString().Trim().Length > 0)
                        {


                            Data = SecA[c].ToString();
                            Data = Data.Replace("X/E/", "");
                            Data = Data.Replace("X/", "");
                            Data = Data.Replace("/-", "");
                            Data = Data.Replace("//", "");
                            Data = Data.Replace("/*", "");
                            Data = Data.Replace("*/", "");
                            Data = Data.Replace("(", "");
                            Data = Data.Replace(")", "");

                            Data = Data.Replace("/E", "");
                            Data = Data.Replace("//", "");
                            Data = Data.Replace("/*", "");
                            Data = Data.Replace("*/", "");
                            Data = Data.Replace("(", "");
                            Data = Data.Replace(")", "");
                            Data = Data.Replace("q:", "");
                            mylist.Add(Data);
                            col++;
                            n = GRow;
                        }


                    }
                    list.Add(mylist);
                    /*for(int comp1 = 0; comp1 < mylist.Count; comp1++)
                    {
                        mylist.RemoveAt(comp1);
                    }*/
                }

                mylistdef.Add(list);

            }

            int c1 = 0;
            /*foreach (DataGridViewColumn column in dgvSectors.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
                column.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                column.DefaultCellStyle.ForeColor = Color.Navy;
            }*/
            // this.dgvSectors.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            return mylistdef;

        }
        private String[,] getFare(String[,] tableau, string docNumber, string fca3)
        {
            string Start = "";
            string End = "";

            string Start1 = "";
            string End1 = "";
            int longueur = fca3.Length;
            string NUC = "";
            string FC = "";
            string FB = "";
            //string[,] tableau = ViewBag.tableau;

            Start = tableau[1, 0].ToString();
            int fcCnt = 1;
            int Countfc = 1;
            for (int z = 0; z < tableau.Length - 1; z++)
            {
                if (tableau[0, z].ToString().Equals("ROE")) { break; }
                else
                {
                    if (tableau[0, z].ToString().Contains("Fare Component"))
                    {
                        Countfc = Countfc + 1;
                    }
                }
            }
            string[,] tableauFare = new string[Countfc, 4];
            tableauFare[0, 0] = "FC NUC";
            tableauFare[0, 1] = "FC HIP";
            tableauFare[0, 2] = "FC Component";
            tableauFare[0, 3] = "FC Basis";

            for (int x = 1; x < tableau.Length - 1; x++)
            {
                if (tableau[0, x].ToString().Equals("ROE")) { break; }

                if (tableau[0, x].ToString() == "City") { End = tableau[1, x].ToString(); FC = Start + End; }
                if (tableau[0, x].ToString().Contains("Fare Component") || tableau[0, x].ToString().Contains("Millage"))
                {
                    NUC = tableau[1, x].ToString();
                    //tableauFare.Colums.Add(fcCnt.ToString(), fcCnt.ToString());
                    //dgvFareComponent.Columns[fcCnt].Width = 250;

                    tableauFare[fcCnt, 0] = NUC;

                    tableauFare[fcCnt, 1] = "";

                    tableauFare[fcCnt, 2] = FC;
                    tableauFare[fcCnt, 3] = "Fare Basis";

                    // string FB GetFareBasis()
                    string Countsql = "Select COUNT(*) as countcsql from pax.SalesDocumentHeader SDH " +
                        " left join pax.SalesRelatedDocumentInformation SRI on SDH.HdrGuid = SRI.HdrGuid " +
                        " left join pax.SalesDocumentCoupon SDC on SRI.RelatedDocumentGuid=SDC.RelatedDocumentGuid " +
                        " where SDH.DocumentNumber = '" + docNumber + "'";
                    dbconnect.pbConnectionString = pbConnectionString;
                    SqlDataReader myReaderMore = dbconnect.GetData(Countsql);
                    int ligne = 0;
                    while (myReaderMore.Read())
                    {
                        ligne = Int32.Parse(myReaderMore["countcsql"].ToString());
                    }

                    string Csql = "Select SRI.RelatedDocumentNumber,  SDC.OriginCity, SDC.DestinationCity, " +
                        "SDC.FareBasisTicketDesignator from pax.SalesDocumentHeader " +
                        "SDH left join pax.SalesRelatedDocumentInformation SRI on SDH.HdrGuid = SRI.HdrGuid " +
                        "left join pax.SalesDocumentCoupon SDC on SRI.RelatedDocumentGuid=SDC.RelatedDocumentGuid " +
                        "where SDH.DocumentNumber = '" + docNumber + "'";
                    dbconnect.pbConnectionString = pbConnectionString;
                    SqlDataReader myReaderMore1 = dbconnect.GetData(Csql);
                    int insert = 0;
                    string[,] tableauCoupons = new string[4, ligne];
                    while (myReaderMore1.Read())
                    {
                        /*for(int j=0; j< 57; j++)
                        {*/
                        tableauCoupons[0, insert] = myReaderMore1["RelatedDocumentNumber"].ToString();
                        tableauCoupons[1, insert] = myReaderMore1["OriginCity"].ToString();
                        tableauCoupons[2, insert] = myReaderMore1["DestinationCity"].ToString();
                        tableauCoupons[3, insert] = myReaderMore1["FareBasisTicketDesignator"].ToString();
                        // tableauCoupons[4, insert] = myReaderMore1.GetValue(4).ToString();
                        //tableauCoupons[5, insert] = myReaderMore1.GetValue(5).ToString();
                        // }
                        insert++;

                    }

                    for (int r = 0; r < ligne; r++)
                    {//dgvCoupons[16, r].Value.ToString() + 
                        if (Start + tableauCoupons[2, r].ToString() == FC)
                        {
                            FB = tableauCoupons[3, r].ToString();
                            tableauFare[fcCnt, 3] = FB;
                            break;
                        }
                        /*else
                        {
                            if (Start + tableauCoupons[52, r].ToString() == FC)
                            {
                                FB = tableauCoupons[20, r].ToString();
                                tableauFare[fcCnt, 3] = FB;
                                break;
                            }

                        }*/
                    }
                    Start = End;
                    End = "";
                    NUC = "";
                    FC = "";
                    FB = "";
                    fcCnt++;

                }

            }
            return tableauFare;
        }
        private int GetCountFare(String[,] tableau, string fca3)
        {
            int Countfc = 0;
            int longueur = fca3.Length;
            for (int z = 0; z < tableau.Length - 1; z++)
            {
                if (tableau[0, z].ToString().Equals("ROE")) { break; }
                else
                {
                    if (tableau[0, z].ToString().Contains("Fare Component"))
                    {
                        Countfc = Countfc + 1;
                    }
                }
            }
            return Countfc;
        }
        private List<ArrCPNFareBasis> GetFareBasis(string FareBasis)
        {

            dbconnect.pbConnectionString = pbConnectionString;
            SqlDataReader myReaderMore = dbconnect.GetData(FareBasis);
            List<ArrCPNFareBasis> model = new List<ArrCPNFareBasis>();
            model.Clear();
            while (myReaderMore.Read())
            {
                var details = new ArrCPNFareBasis();
                details.FareBasisTicketDesignator = myReaderMore["FareBasisTicketDesignator"].ToString();
                model.Add(details);
            }
            return model;

        }
        private List<TicketInformation> GetTicket(string sql)
        {
            dbconnect.pbConnectionString = pbConnectionString;
            SqlDataReader myReader = dbconnect.GetData(sql);

            List<TicketInformation> model = new List<TicketInformation>();
            if (myReader.HasRows)
            {
                ViewBag.etat = "true";
                while (myReader.Read())
                {
                    var details = new TicketInformation();
                    if (myReader["DocumentNumber"].ToString() != "")
                    {
                        details.HdrGuid = myReader["HdrGuid"].ToString();
                        details.DocumentNumber = myReader["DocumentNumber"].ToString();
                        details.CheckDigit = myReader["CheckDigit"].ToString();
                        details.UploadedRef = myReader["UploadedRef"].ToString();
                        details.AgentNumericCode = myReader["AgentNumericCode"].ToString();
                        details.DateofIssue = myReader["DateofIssue"].ToString();
                        details.SaleDate = myReader["SaleDate"].ToString();
                        details.PassengerName = myReader["PassengerName"].ToString();
                        details.PassengerSpecificData = myReader["PassengerSpecificData"].ToString();
                        details.FareCalculationArea = myReader["FareCalculationArea"].ToString();
                        details.EndosRestriction = myReader["EndosRestriction"].ToString();
                        details.OriginalIssueInformation = myReader["OriginalIssueInformation"].ToString();
                        details.OriginalIssueDocumentNumber = myReader["OriginalIssueDocumentNumber"].ToString();
                        details.OriginalIssueCity = myReader["OriginalIssueCity"].ToString();
                        details.OriginalIssueDate = myReader["OriginalIssueDate"].ToString();
                        details.OriginalIssueAgentNumericCode = myReader["OriginalIssueAgentNumericCode"].ToString();
                        details.InConnectionWithDocumentNumber = myReader["InConnectionWithDocumentNumber"].ToString();
                        details.Fare = myReader["Fare"].ToString();
                        details.FareCurrency = myReader["FareCurrency"].ToString();
                        details.EquivalentFare = myReader["EquivalentFare"].ToString();
                        details.Tax1Currency = myReader["Tax1Currency"].ToString();
                        details.Tax2Currency = myReader["Tax2Currency"].ToString();
                        details.Tax3Currency = myReader["Tax3Currency"].ToString();
                        details.Tax1Code = myReader["Tax1Code"].ToString();
                        details.Tax2Code = myReader["Tax2Code"].ToString();
                        details.Tax3Code = myReader["Tax3Code"].ToString();
                        details.Tax1Amount = myReader["Tax1Amount"].ToString();
                        details.Tax2Amount = myReader["Tax2Amount"].ToString();
                        details.Tax3Amount = myReader["Tax3Amount"].ToString();
                        details.TotalCurrency = myReader["TotalCurrency"].ToString();
                        details.TotalAmount = myReader["TotalAmount"].ToString();
                        details.SequenceNumber = myReader["SequenceNumber"].ToString();
                        details.ITBT = myReader["ITBT"].ToString();
                        details.ExchangeADC = myReader["ExchangeADC"].ToString();
                        details.DocumentType = myReader["DocumentType"].ToString();
                        details.TicketingModeIndicator = myReader["TicketingModeIndicator"].ToString();
                        details.FareCalculationModeIndicator = myReader["FareCalculationModeIndicator"].ToString();
                        details.BookingAgentIdentification = myReader["BookingAgentIdentification"].ToString();
                        details.BookingAgencyLocationNumber = myReader["BookingAgencyLocationNumber"].ToString();
                        details.BookingEntityOutletType = myReader["BookingEntityOutletType"].ToString();
                        details.BookingReference = myReader["BookingReference"].ToString();
                        details.AuditStatusIndicator = myReader["AuditStatusIndicator"].ToString();
                        details.ClientIdentification = myReader["ClientIdentification"].ToString();
                        details.CommercialAgreementReference = myReader["CommercialAgreementReference"].ToString();
                        details.CustomerFileReference = myReader["CustomerFileReference"].ToString();
                        details.DataInputStatusIndicator = myReader["DataInputStatusIndicator"].ToString();
                        details.FareCalculationPricingIndicator = myReader["FareCalculationPricingIndicator"].ToString();
                        details.FormatIdentifier = myReader["FormatIdentifier"].ToString();
                        details.TourCode = myReader["TourCode"].ToString();
                        details.NetReportingIndicator = myReader["NetReportingIndicator"].ToString();
                        details.NeutralTicketingSystemIdentifier = myReader["NeutralTicketingSystemIdentifier"].ToString();
                        details.ReportingSystemIdentifier = myReader["ReportingSystemIdentifier"].ToString();
                        details.ServicingAirlineSystemProviderIdentifier = myReader["ServicingAirlineSystemProviderIdentifier"].ToString();
                        details.TrueOriginDestinationCityCodes = myReader["TrueOriginDestinationCityCodes"].ToString();
                        details.SettlementAuthorizationCode = myReader["SettlementAuthorizationCode"].ToString();
                        details.VendorIdentification = myReader["VendorIdentification"].ToString();
                        details.VendorISOCountryCode = myReader["VendorISOCountryCode"].ToString();
                        details.TransactionCode = myReader["TransactionCode"].ToString();
                        details.SalesDataAvailable = myReader["SalesDataAvailable"].ToString();
                        details.AccountingStatus = myReader["AccountingStatus"].ToString();
                        details.ProratedFlag = myReader["ProratedFlag"].ToString();
                        details.FBSFlag = myReader["FBSFlag"].ToString();
                        details.DocumentAirlineID = myReader["DocumentAirlineID"].ToString();
                        details.TransactionGroup = myReader["TransactionGroup"].ToString();
                        details.PaxSPC = myReader["PaxSPC"].ToString();
                        details.PaxType = myReader["PaxType"].ToString();
                        details.SalesPeriod = myReader["SalesPeriod"].ToString();
                        details.PrevMOS = myReader["PrevMOS"].ToString();
                        details.TotalTax = myReader["TotalTax"].ToString();
                        details.TaxCurrency = myReader["TaxCurrency"].ToString();
                        details.PayCurrency = myReader["PayCurrency"].ToString();
                        details.BookingAgentID = myReader["BookingAgentID"].ToString();
                        details.TicketingAgentID = myReader["TicketingAgentID"].ToString();
                        details.IsReissue = myReader["IsReissue"].ToString();
                        details.DataSource = myReader["DataSource"].ToString();
                        details.ProcessingFileType = myReader["ProcessingFileType"].ToString();
                        details.ProcessingDate = myReader["ProcessingDate"].ToString();
                        details.OwnTicket = myReader["OwnTicket"].ToString();
                        details.OwnCarrier = myReader["OwnCarrier"].ToString();
                        details.OwnISOCountry = myReader["OwnISOCountry"].ToString();
                        details.OwnAirline = myReader["OwnAirline"].ToString();
                        details.HostCurrency = myReader["HostCurrency"].ToString();
                        details.AmountCollected = myReader["AmountCollected"].ToString();
                        details.AmountCollectedCurrency = myReader["AmountCollectedCurrency"].ToString();
                        details.TaxCollected = myReader["TaxCollected"].ToString();
                        details.TaxCollectedCurrency = myReader["TaxCollectedCurrency"].ToString();
                        details.SurchargeCollected = myReader["SurchargeCollected"].ToString();
                        details.SurchargeCollectedCurrency = myReader["SurchargeCollectedCurrency"].ToString();
                        details.CommissionCollected = myReader["CommissionCollected"].ToString();
                        details.CommissionCollectedCurrency = myReader["CommissionCollectedCurrency"].ToString();
                        details.BilateralEndorsement = myReader["BilateralEndorsement"].ToString();
                        details.InvoluntaryReroute = myReader["InvoluntaryReroute"].ToString();
                        details.BspIdentifier = myReader["BspIdentifier"].ToString();
                        details.IsoCountryCode = myReader["IsoCountryCode"].ToString();
                        details.USDRatePayCur = myReader["USDRatePayCur"].ToString();
                        details.USDRateHostCur = myReader["USDRateHostCur"].ToString();
                        details.PMPPeriod = myReader["PMPPeriod"].ToString();
                        details.TaxOnCommissionCollected = myReader["TaxOnCommissionCollected"].ToString();
                        details.TaxOnCommissionCollectedCurrency = myReader["TaxOnCommissionCollectedCurrency"].ToString();
                        details.SignCode = myReader["SignCode"].ToString();
                        model.Add(details);
                    }

                }
            }
            else
            {
                ViewBag.etat = "false";
            }

            return model;
        }
        private List<Coupons> GetCoupons(string Csql)
        {
            dbconnect.pbConnectionString = pbConnectionString;
            SqlDataReader myReaderMore = dbconnect.GetData(Csql);
            List<Coupons> model = new List<Coupons>();
            while (myReaderMore.Read())
            {
                var details = new Coupons();
                if (myReaderMore["DocumentNumber"].ToString() != "")
                {
                    details.RelatedDocumentGuid = myReaderMore["RelatedDocumentGuid"].ToString();
                    details.HdrGuid = myReaderMore["HdrGuid"].ToString();
                    details.DocumentNumber = myReaderMore["DocumentNumber"].ToString();
                    details.CheckDigit = myReaderMore["CheckDigit"].ToString();
                    details.IsConjunction = myReaderMore["IsConjunction"].ToString();
                    details.IsReissue = myReaderMore["IsReissue"].ToString();
                    details.RelatedDocumentNumber = myReaderMore["RelatedDocumentNumber"].ToString();
                    details.RelatedTicketCheckDigit = myReaderMore["RelatedTicketCheckDigit"].ToString();
                    details.CouponIndicator = myReaderMore["CouponIndicator"].ToString();
                    details.TransactionCode = myReaderMore["TransactionCode"].ToString();
                    details.TransmissionControlNumber = myReaderMore["TransmissionControlNumber"].ToString();
                    details.SalesUploadGuid = myReaderMore["SalesUploadGuid"].ToString();
                    details.LiftUploadGuid = myReaderMore["LiftUploadGuid"].ToString();
                    details.SequenceNumber = myReaderMore["SequenceNumber"].ToString();
                    details.RelatedDocumentGuid = myReaderMore["RelatedDocumentGuid"].ToString();
                    details.CouponNumber = myReaderMore["CouponNumber"].ToString();
                    details.OriginAirportCityCode = myReaderMore["OriginAirportCityCode"].ToString();
                    details.DestinationAirportCityCode = myReaderMore["DestinationAirportCityCode"].ToString();
                    details.Carrier = myReaderMore["Carrier"].ToString();
                    details.StopOverCode = myReaderMore["StopOverCode"].ToString();
                    details.FareBasisTicketDesignator = myReaderMore["FareBasisTicketDesignator"].ToString();
                    details.FrequentFlyerReference = myReaderMore["FrequentFlyerReference"].ToString();
                    details.FlightNumber = myReaderMore["FlightNumber"].ToString();
                    details.FlightDepartureDate = myReaderMore["FlightDepartureDate"].ToString();
                    details.FlightDepartureTime = myReaderMore["FlightDepartureTime"].ToString();
                    details.NotValidBefore = myReaderMore["NotValidBefore"].ToString();
                    details.NotValidAfter = myReaderMore["NotValidAfter"].ToString();
                    details.ReservationBookingDesignator = myReaderMore["ReservationBookingDesignator"].ToString();
                    details.FreeBaggageAllowance = myReaderMore["FreeBaggageAllowance"].ToString();
                    details.FlightBookingStatus = myReaderMore["FlightBookingStatus"].ToString();
                    details.OriginAirportCityCode1 = myReaderMore["OriginAirportCityCode1"].ToString();
                    details.StopoverCode1 = myReaderMore["StopoverCode1"].ToString();
                    details.OriginAirportCityCode2 = myReaderMore["OriginAirportCityCode2"].ToString();
                    details.StopoverCode2 = myReaderMore["StopoverCode2"].ToString();
                    details.SegmentIdentifier = myReaderMore["SegmentIdentifier"].ToString();
                    details.UsageAirline = myReaderMore["UsageAirline"].ToString();
                    details.UsageDate = myReaderMore["UsageDate"].ToString();
                    details.UsageFlightNumber = myReaderMore["UsageFlightNumber"].ToString();
                    details.UsageOriginCode = myReaderMore["UsageOriginCode"].ToString();
                    details.UsageDestinationCode = myReaderMore["UsageDestinationCode"].ToString();
                    details.UsedClassofService = myReaderMore["UsedClassofService"].ToString();
                    details.SettlementAuthorizationCode = myReaderMore["SettlementAuthorizationCode"].ToString();
                    details.CouponStatus = myReaderMore["CouponStatus"].ToString();
                    details.AccountingStatus = myReaderMore["AccountingStatus"].ToString();
                    details.AccountingOnEstimate = myReaderMore["AccountingOnEstimate"].ToString();
                    details.BillingStatus = myReaderMore["BillingStatus"].ToString();
                    details.HdrGuidRef = myReaderMore["HdrGuidRef"].ToString();
                    details.DocumentNumber = myReaderMore["DocumentNumber"].ToString();
                    details.RelatedDocumentNumber = myReaderMore["RelatedDocumentNumber"].ToString();
                    details.BillingType = myReaderMore["BillingType"].ToString();
                    details.DomesticInternational = myReaderMore["DomesticInternational"].ToString();
                    details.OriginCity = myReaderMore["OriginCity"].ToString();
                    details.DestinationCity = myReaderMore["DestinationCity"].ToString();
                    details.OriginCountry = myReaderMore["OriginCountry"].ToString();
                    details.DestinationCountry = myReaderMore["DestinationCountry"].ToString();
                    details.IsOAL = myReaderMore["IsOAL"].ToString();
                    details.SequenceNumber = myReaderMore["SequenceNumber"].ToString();

                    model.Add(details);
                }

            }
            return model;
        }
        private List<Payement> GetPayement(string Psql)
        {
            dbconnect.pbConnectionString = pbConnectionString;
            SqlDataReader myReaderMore = dbconnect.GetData(Psql);

            List<Payement> model = new List<Payement>();
            if (myReaderMore.HasRows)
            {
                ViewBag.etat = "true";
                while (myReaderMore.Read())
                {
                    var details = new Payement();
                    if (myReaderMore["DocumentNumber"].ToString() != "")
                    {
                        details.RelatedDocumentGuid = myReaderMore["RelatedDocumentGuid"].ToString();
                        details.HdrGuid = myReaderMore["HdrGuid"].ToString();
                        details.DocumentNumber = myReaderMore["DocumentNumber"].ToString();
                        details.CheckDigit = myReaderMore["CheckDigit"].ToString();
                        details.IsConjunction = myReaderMore["IsConjunction"].ToString();
                        details.IsReissue = myReaderMore["IsReissue"].ToString();
                        details.RelatedDocumentNumber = myReaderMore["RelatedDocumentNumber"].ToString();
                        details.RelatedTicketCheckDigit = myReaderMore["RelatedTicketCheckDigit"].ToString();
                        details.CouponIndicator = myReaderMore["CouponIndicator"].ToString();
                        details.TransactionCode = myReaderMore["TransactionCode"].ToString();
                        details.TransmissionControlNumber = myReaderMore["TransmissionControlNumber"].ToString();
                        details.SalesUploadGuid = myReaderMore["SalesUploadGuid"].ToString();
                        details.LiftUploadGuid = myReaderMore["LiftUploadGuid"].ToString();
                        details.SequenceNumber = myReaderMore["SequenceNumber"].ToString();
                        details.RelatedDocumentGuid = myReaderMore["RelatedDocumentGuid"].ToString();
                        details.SequenceNumber = myReaderMore["SequenceNumber"].ToString();
                        details.DateofIssue = myReaderMore["DateofIssue"].ToString();
                        details.AccountNumber = myReaderMore["AccountNumber"].ToString();
                        details.Amount = myReaderMore["Amount"].ToString();
                        details.Currency = myReaderMore["Currency"].ToString();
                        details.ApprovalCode = myReaderMore["ApprovalCode"].ToString();
                        details.ExtendedPaymentCode = myReaderMore["ExtendedPaymentCode"].ToString();
                        details.FormofPaymentType = myReaderMore["FormofPaymentType"].ToString();
                        details.ExpiryDate = myReaderMore["ExpiryDate"].ToString();
                        details.InvoiceNumber = myReaderMore["InvoiceNumber"].ToString();
                        details.InvoiceDate = myReaderMore["InvoiceDate"].ToString();
                        details.CustomerFileReference = myReaderMore["CustomerFileReference"].ToString();
                        details.CreditCardCorporateContract = myReaderMore["CreditCardCorporateContract"].ToString();
                        details.AddressVerificationCode = myReaderMore["AddressVerificationCode"].ToString();
                        details.SourceofApprovalCode = myReaderMore["SourceofApprovalCode"].ToString();
                        details.TransactionInformation = myReaderMore["TransactionInformation"].ToString();
                        details.AuthorisedAmount = myReaderMore["AuthorisedAmount"].ToString();
                        details.AccountNumber1 = myReaderMore["AccountNumber1"].ToString();
                        details.AuthorisedAmount1 = myReaderMore["AuthorisedAmount1"].ToString();
                        details.CardVerificationValueResult = myReaderMore["CardVerificationValueResult"].ToString();
                        details.CardVerificationValueResult1 = myReaderMore["CardVerificationValueResult1"].ToString();
                        details.SignedforAmount = myReaderMore["SignedforAmount"].ToString();
                        details.RemittanceAmount = myReaderMore["RemittanceAmount"].ToString();
                        details.RemittanceCurrency = myReaderMore["RemittanceCurrency"].ToString();
                        details.TransactionCode = myReaderMore["TransactionCode"].ToString();
                        details.HdrGuidRef = myReaderMore["HdrGuidRef"].ToString();
                        details.DocumentNumber = myReaderMore["DocumentNumber"].ToString();
                        details.TransactionCode2 = myReaderMore["TransactionCode2"].ToString();
                        details.RelatedDocumentNumber = myReaderMore["RelatedDocumentNumber"].ToString();
                        model.Add(details);
                    }


                }
            }
            else
            {
                ViewBag.etat = "false";
            }
            return model;
        }
        private List<OtherPayement> GetOtherPayement(string OPsql)
        {
            dbconnect.pbConnectionString = pbConnectionString;
            SqlDataReader myReaderMore = dbconnect.GetData(OPsql);
            List<OtherPayement> model = new List<OtherPayement>();
            if (myReaderMore.HasRows)
            {
                ViewBag.etat = "true";

                while (myReaderMore.Read())
                {
                    var details = new OtherPayement();
                    if (myReaderMore["DocumentNumber"].ToString() != "")
                    {
                        details.RelatedDocumentGuid = myReaderMore["RelatedDocumentGuid"].ToString();
                        details.SequenceNumber = myReaderMore["SequenceNumber"].ToString();
                        details.DocumentAmountType = myReaderMore["DocumentAmountType"].ToString();
                        details.DateofIssue = myReaderMore["DateofIssue"].ToString();
                        details.CurrencyType = myReaderMore["CurrencyType"].ToString();
                        details.TransactionCode = myReaderMore["TransactionCode"].ToString();
                        details.OtherAmountCode = myReaderMore["OtherAmountCode"].ToString();
                        details.OtherAmount = myReaderMore["OtherAmount"].ToString();
                        details.OtherAmountRate = myReaderMore["OtherAmountRate"].ToString();
                        details.TicketDocumentAmount = myReaderMore["TicketDocumentAmount"].ToString();
                        details.CommissionableAmount = myReaderMore["CommissionableAmount"].ToString();
                        details.AmountEnteredbyAgent = myReaderMore["AmountEnteredbyAgent"].ToString();
                        details.AmountPaidbyCustomer = myReaderMore["AmountPaidbyCustomer"].ToString();
                        details.LateReportingPenalty = myReaderMore["LateReportingPenalty"].ToString();
                        details.NetFareAmount = myReaderMore["NetFareAmount"].ToString();
                        details.StatisticalCode = myReaderMore["StatisticalCode"].ToString();
                        details.HdrGuidRef = myReaderMore["HdrGuidRef"].ToString();
                        details.DocumentNumber = myReaderMore["DocumentNumber"].ToString();
                        details.RelatedDocumentNumber = myReaderMore["RelatedDocumentNumber"].ToString();
                        details.FileSequence = myReaderMore["FileSequence"].ToString();
                        model.Add(details);
                    }

                }
            }
            else
            {
                ViewBag.etat = "false";
            }
            return model;
        }
        private List<ProratioDtl> GetProratioDtl(string PDsql)
        {
            dbconnect.pbConnectionString = pbConnectionString;
            SqlDataReader myReaderMore = dbconnect.GetData(PDsql);

            List<ProratioDtl> model = new List<ProratioDtl>();
            if (myReaderMore.HasRows)
            {
                ViewBag.etat = "true";

                while (myReaderMore.Read())
                {
                    var details = new ProratioDtl();
                    if (myReaderMore["DocumentNumber"].ToString() != "")
                    {
                        details.ProrateGuid = myReaderMore["ProrateGuid"].ToString();
                        details.HeaderGuid = myReaderMore["HeaderGuid"].ToString();
                        details.ProrationFlag = myReaderMore["ProrationFlag"].ToString();
                        details.SectorOrigin = myReaderMore["SectorOrigin"].ToString();
                        details.SectorDestination = myReaderMore["SectorDestination"].ToString();
                        details.SectorCarrier = myReaderMore["SectorCarrier"].ToString();
                        details.SourceType = myReaderMore["SourceType"].ToString();
                        details.Currency = myReaderMore["Currency"].ToString();
                        details.ProrateFactor = myReaderMore["ProrateFactor"].ToString();
                        details.ProrateValue = myReaderMore["ProrateValue"].ToString();
                        details.StraightRateProrate = myReaderMore["StraightRateProrate"].ToString();
                        details.SpecialProrateAgreement = myReaderMore["SpecialProrateAgreement"].ToString();
                        details.Diffentials = myReaderMore["Diffentials"].ToString();
                        details.Surcharge = myReaderMore["Surcharge"].ToString();
                        details.YQ = myReaderMore["YQ"].ToString();
                        details.FinalShare = myReaderMore["FinalShare"].ToString();
                        details.TaxesFeesCharges = myReaderMore["TaxesFeesCharges"].ToString();
                        details.BaseAmount = myReaderMore["BaseAmount"].ToString();
                        details.IscPercent = myReaderMore["IscPercent"].ToString();
                        details.IscAmount = myReaderMore["IscAmount"].ToString();
                        details.HandlingFeeAmt = myReaderMore["HandlingFeeAmt"].ToString();
                        details.OtherCommissions = myReaderMore["OtherCommissions"].ToString();
                        details.TaxAmount = myReaderMore["TaxAmount"].ToString();
                        details.CouponNumber = myReaderMore["CouponNumber"].ToString();
                        details.UatpAmount = myReaderMore["UatpAmount"].ToString();
                        details.FareComponent = myReaderMore["FareComponent"].ToString();
                        details.DocumentNumber = myReaderMore["DocumentNumber"].ToString();
                        details.Roe = myReaderMore["Roe"].ToString();
                        details.RelatedDocumentGuid = myReaderMore["RelatedDocumentGuid"].ToString();
                        details.CodeShareValue = myReaderMore["CodeShareValue"].ToString();

                        model.Add(details);

                    }

                }
            }
            else
            {
                ViewBag.etat = "false";
            }
            return model;
        }
        private List<ProrationExp> GetProrationExp(string PEsql)
        {
            dbconnect.pbConnectionString = pbConnectionString;
            SqlDataReader myReaderMore = dbconnect.GetData(PEsql);

            List<ProrationExp> model = new List<ProrationExp>();
            if (myReaderMore.HasRows)
            {
                ViewBag.etat = "true";
                while (myReaderMore.Read())
                {

                    var details = new ProrationExp();
                    if (myReaderMore["DocumentNumber"].ToString() != "")
                    {
                        details.DateProcess = myReaderMore["DateProcess"].ToString();
                        details.RelatedDocumentGuid = myReaderMore["RelatedDocumentGuid"].ToString();
                        details.DocumentNumber = myReaderMore["DocumentNumber"].ToString();
                        details.FareCalculationArea = myReaderMore["FareCalculationArea"].ToString();
                        details.CouponNumber = myReaderMore["CouponNumber"].ToString();
                        details.CouponStatus = myReaderMore["CouponStatus"].ToString();
                        details.Fare = myReaderMore["Fare"].ToString();
                        details.EquivalentFare = myReaderMore["EquivalentFare"].ToString();
                        details.ErrorMsg = myReaderMore["ErrorMsg"].ToString();
                        model.Add(details);
                    }

                }
            }
            else
            {
                ViewBag.etat = "false";
            }
            return model;
        }
        private List<Exchange> GetExchange(string Exsql)
        {
            dbconnect.pbConnectionString = pbConnectionString;
            SqlDataReader myReaderMore = dbconnect.GetData(Exsql);

            List<Exchange> model = new List<Exchange>();
            if (myReaderMore.HasRows)
            {
                ViewBag.etat = "true";

                while (myReaderMore.Read())
                {
                    var details = new Exchange();
                    if (myReaderMore["OriginalIssueDocumentNumber"].ToString() != "")
                    {
                        details.DocumentNumber = myReaderMore["DocumentNumber"].ToString();
                        details.OriginalIssueDocumentNumber = myReaderMore["OriginalIssueDocumentNumber"].ToString();
                        model.Add(details);
                    }


                }
            }
            else
            {
                ViewBag.etat = "false";
            }
            return model;
        }
        public ActionResult CherchByCode()
        {
            string value = Request["value"];
            string CommandText = "";
            int n;
            bool isNumeric = int.TryParse(value, out n);
            if (isNumeric)
            {

                CommandText = "select distinct AgentNumericCode, agtown.TradingName as [AgentName] ,agtown.LocationCity" + Environment.NewLine;
                CommandText += "from pax.SalesDocumentHeader sdh" + Environment.NewLine;
                CommandText += "left join Ref.VW_Agent agtown	on left(sdh.AgentNumericCode,7) = agtown.AgencyNumericCode" + Environment.NewLine;
                CommandText += "WHERE 1=1";
                CommandText += " and AgentNumericCode like " + "'" + value + "%'";
            }
            else
            {
                CommandText = "select distinct AgentNumericCode, agtown.TradingName as [AgentName] ,agtown.LocationCity" + Environment.NewLine;
                CommandText += "from pax.SalesDocumentHeader sdh" + Environment.NewLine;
                CommandText += "left join Ref.VW_Agent agtown	on left(sdh.AgentNumericCode,7) = agtown.AgencyNumericCode" + Environment.NewLine;
                CommandText += "WHERE 1=1";
                CommandText += " and agtown.TradingName like " + "'" + value + "%'";
            }


            dbconnect.pbConnectionString = pbConnectionString;
            SqlDataReader myReader = dbconnect.GetData(CommandText);

            List<AgentCode> model = new List<AgentCode>();
            while (myReader.Read())
            {
                var details = new AgentCode();
                details.AgentNumericCode = myReader["AgentNumericCode"].ToString();
                details.AgentName = myReader["AgentName"].ToString();
                details.Count = myReader["LocationCity"].ToString();
                model.Add(details);
            }

            return PartialView(model);

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

        public DataSet SalesTicketHistory(string docNum, string transactionCode)
        {
            if (transactionCode == "RFND")
            {
                transactionCode = "TKTT";

            }

            string sql = "declare @TicketNo nvarchar(13); SET @TicketNo = '" + docNum + "'" + Environment.NewLine;
            sql += "select DocumentNumber, CheckDigit, OwnAirline, OwnISOCountry, FORMAT(DateofIssue, 'dd-MMM-yyyy') as DateofIssue, AgentNumericCode,BookingAgentIdentification," + Environment.NewLine;
            sql += "PassengerName, BookingReference, TourCode,ReportingSystemIdentifier, TransactionCode, FareCurrency, Fare, TotalCurrency, " + Environment.NewLine;
            sql += "EquivalentFare, " + Environment.NewLine;

            sql += "iif(" + Environment.NewLine;
            sql += "(select [Tax1] from [FileHot].[Bar64_DocumentAmount] where ticketdocumentnumber = @TicketNo) is null, " + Environment.NewLine;
            sql += "concat(iif(Tax1Currency is null, totalcurrency,Tax1Currency), Tax1Amount,Tax1Code)," + Environment.NewLine;
            sql += "(select [Tax1] from [FileHot].[Bar64_DocumentAmount] where ticketdocumentnumber = @TicketNo) " + Environment.NewLine;
            sql += ")," + Environment.NewLine;

            sql += "iif(" + Environment.NewLine;
            sql += "(select [Tax2] from [FileHot].[Bar64_DocumentAmount] where ticketdocumentnumber  = @TicketNo) is null, " + Environment.NewLine;
            sql += "concat(iif(Tax2Currency is null, totalcurrency,Tax2Currency), Tax2Amount,Tax2Code)," + Environment.NewLine;
            sql += "(select [Tax2] from [FileHot].[Bar64_DocumentAmount] where ticketdocumentnumber = @TicketNo) " + Environment.NewLine;
            sql += ")," + Environment.NewLine;

            sql += "iif(" + Environment.NewLine;
            sql += "(select [Tax3] from [FileHot].[Bar64_DocumentAmount] where ticketdocumentnumber = @TicketNo) is null, " + Environment.NewLine;
            sql += "concat(iif(Tax3Currency is null, totalcurrency,Tax3Currency), Tax3Amount,Tax3Code)," + Environment.NewLine;
            sql += "(select [Tax3] from [FileHot].[Bar64_DocumentAmount] where ticketdocumentnumber = @TicketNo) " + Environment.NewLine;
            sql += ")," + Environment.NewLine;

            sql += "FareCalculationArea, FareCalculationModeIndicator,TrueOriginDestinationCityCodes, DocumentType," + Environment.NewLine;
            sql += "iif(" + Environment.NewLine;
            sql += "(select Total from [FileHot].[Bar64_DocumentAmount] where ticketdocumentnumber = @TicketNo) is null, " + Environment.NewLine;
            sql += "concat(TotalCurrency, TotalAmount)," + Environment.NewLine;
            sql += "(select Total from [FileHot].[Bar64_DocumentAmount] where ticketdocumentnumber = @TicketNo)" + Environment.NewLine;
            sql += ")," + Environment.NewLine;

            sql += " EndosRestriction," + Environment.NewLine;
            sql += "concat([AmountCollectedCurrency],' ',[AmountCollected],' ',[ExchangeADC])" + Environment.NewLine;
            sql += "from pax.salesdocumentheader" + Environment.NewLine;
            sql += "where documentnumber  = @TicketNo" + Environment.NewLine;
            sql += "and TransactionCode = '" + transactionCode + "'" + Environment.NewLine;

            DataSet ds = new DataSet();
            ds = dbconnect.RetObjDS(sql);

            return ds;
        }

        public DataSet ConjonctionTicket(string docNum)
        {
            string sql = "SELECT right(Relateddocumentnumber,2)" + Environment.NewLine;
            sql += "from [Pax].[SalesRelatedDocumentInformation]" + Environment.NewLine;
            sql += " where DocumentNumber = '" + docNum + "'";
            DataSet ds = new DataSet();
            ds = dbconnect.RetObjDS(sql);

            return ds;
        }

        private DataSet BMP70(string DocNum)
        {
            string sql = "SELECT [DateofIssue] ,[ReasonforIssuanceDescription] ,[ReasonforIssuanceCode]" + Environment.NewLine;
            sql += "FROM [FileHot].[Bmp70_ReasonForIssuance]" + Environment.NewLine;
            sql += "where TicketDocumentNumber = '" + DocNum + "'";

            DataSet ds = new DataSet();
            ds = dbconnect.RetObjDS(sql);

            return ds;

        }

        private DataSet BMD7576(string DocNum)
        {
            string sql = " select f1.[EMDCouponNumber],f1.[EMDReasonforIssuanceSubCode],f2.EMDRemarks,f1.[EMDRelatedCouponNumber],f1.[EMDRelatedTicketNumber]" + Environment.NewLine;
            sql += ",f1.[EMDExcessBaggageOverAllowanceQualifier],f1.[EMDExcessBaggageRateperUnit],f1.[EMDExcessBaggageTotalNumberinExcess]" + Environment.NewLine;
            sql += ",f1.[EMDExcessBaggageCurrencyType]" + Environment.NewLine;
            sql += "from [Filehot].[Bmd75_ECouponRecords] f1" + Environment.NewLine;
            sql += "left join [FileHot].[Bmd76_ERemarkRecords] f2 on f1.EMDCouponNumber= f2.CouponNumber" + Environment.NewLine;
            sql += " and f1.TicketDocumentNumber = f2.TicketDocumentNumber" + Environment.NewLine;
            sql += " where f1.TicketDocumentNumber= '" + DocNum + "'";
            DataSet ds = new DataSet();
            ds = dbconnect.RetObjDS(sql);
            return ds;

        }

        private DataSet UsesEMD(string Docnum)
        {

            string sql = "  select " + Environment.NewLine;
            sql += "case when f1.FormofPaymentType = 'EX' then" + Environment.NewLine;
            sql += "'E' else ''" + Environment.NewLine;
            sql += "end, ltrim(rtrim(substring(f1.AccountNumber,15, len(f1.accountnumber)))) as Cpn" + Environment.NewLine;
            sql += ",f1.DocumentNumber,f2.AgentNumericCode, f1.DateofIssue, f2.FareCurrency,f2.Fare, f2.CommissionCollectedCurrency, f2.CommissionCollected, f2.TaxCollectedCurrency, " + Environment.NewLine;
            sql += "f2.TaxCollected" + Environment.NewLine;
            sql += "from [Pax].[SalesDocumentPayment] f1" + Environment.NewLine;
            sql += "left join pax.SalesDocumentHeader f2 on f1.HdrGuidRef = f2.HdrGuid" + Environment.NewLine;
            sql += "where left(f1.accountnumber,13) = '" + Docnum + "'" + Environment.NewLine;
            sql += "order by f1.DocumentNumber " + Environment.NewLine;

            DataSet ds = new DataSet();
            ds = dbconnect.RetObjDS(sql);
            return ds;
        }

        private string[] AgentDetails(string agentCode)
        {
            string[] response = new string[3];
            string sql = "select LegalName, LocationCity, LocationCountry from Ref.VW_Agent where AgencyNumericCode = left('" + agentCode + "', 7)";
            DataSet ds = new DataSet();
            ds = dbconnect.RetObjDS(sql);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                response[0] = dr[ds.Tables[0].Columns[0].ColumnName].ToString();
                response[1] = dr[ds.Tables[0].Columns[1].ColumnName].ToString();
                response[2] = dr[ds.Tables[0].Columns[2].ColumnName].ToString();
            }
            return response;
        }
        public ActionResult ticketHistoryPayment()
        {
            string b1 = "";
            string b2 = "";
            string b3 = "";
            string DocNum = Request["docNumber"];
            string TransactionCode = Request["transactionCode"];
            string sql = "select f2.DocumentNumber from pax.SalesDocumentHeader f1 ";
            sql += "left join pax.SalesRelatedDocumentInformation f2 on f1.OriginalIssueDocumentNumber = f2.RelatedDocumentNumber ";
            sql += "where f1.DocumentNumber = '" + DocNum + "' ";

            b1 = Val(sql);  //return docuNumber

            string sql1 = "Select left(f1.AccountNumber,13) as response1 from pax.SalesDocumentPayment f1 ";
            sql1 += "left join pax.SalesRelatedDocumentInformation f2 on left(f1.AccountNumber,13) = f2.RelatedDocumentNumber ";
            sql1 += "where f1.DocumentNumber = '" + DocNum + "' ";
            sql1 += "and f1.AccountNumber <>''  and f2.TransactionCode <> '' ";
            sql1 += "order by left(f1.AccountNumber,13)";
            int j = 0;
            SqlDataReader reader = dbconnect.GetData(sql1);
            while (reader.Read())
            {
                if (j == 0)
                    b2 = reader["response1"].ToString(); // accountNumber
                else
                    if (j == 1)
                    b3 = reader["response1"].ToString();  // accountNumber
                j++;
            }
            //List<TicketHistoryPaymentModel> model = new List<TicketHistoryPaymentModel>();
            string[,] dgpayment = new string[12, 12];

            if ((b1 == "") && (b2 == ""))
            {
                string sql2 = "";
                sql2 += "select top 1 left(f1.FormofPaymentType,2) as FormofPayment, f1.DateofIssue,(select OtherAmountRate from pax.SalesDocumentOtherAmount where DocumentNumber = '" + DocNum + "' and DocumentAmountType = 'Commission3') as OtherAmountRate , ";
                sql2 += "iif(f2.EquivalentFare>0,f2.TotalCurrency,f2.FareCurrency) as Currency,iif(f2.EquivalentFare>0,f2.EquivalentFare,f2.Fare) as Fare ";
                sql2 += ",f1.Amount from pax.salesdocumentpayment f1 left join pax.SalesDocumentHeader f2 on f1.HdrGuidRef = f2.HdrGuid where f1.DocumentNumber = '" + DocNum + "' and left(f1.FormofPaymentType,2) <> 'CA'";
                SqlDataReader reader1 = dbconnect.GetData(sql2);
                int i = 0;

                while (reader1.Read())
                {
                    dgpayment[0, i] = reader1["FormofPayment"].ToString();
                    try
                    {
                        dgpayment[1, i] = Convert.ToDateTime(reader1["DateofIssue"].ToString()).ToString("dd-MMM-yyyy");
                    }
                    catch
                    { }
                    dgpayment[2, i] = reader1["OtherAmountRate"].ToString();
                    dgpayment[3, i] = reader1["Currency"].ToString();
                    dgpayment[4, i] = reader1["Fare"].ToString();
                    dgpayment[6, i] = reader1["Amount"].ToString();
                    i++;
                }

                if (i == 0)
                {

                    string sqls = "";
                    sqls += "select top 1 left(f1.FormofPaymentType,2) as FormofPayment, f1.DateofIssue,(select top 1 OtherAmountRate from pax.SalesDocumentOtherAmount where DocumentNumber = '" + DocNum + "' and DocumentAmountType = 'Commission3' and TransactionCode <> 'RFND') as OtherAmountRate, ";
                    sqls += "iif(f2.EquivalentFare>0,f2.TotalCurrency,f2.FareCurrency) as Currency,iif(f2.EquivalentFare>0,f2.EquivalentFare,f2.Fare) as Fare ";
                    sqls += ",f1.Amount from pax.salesdocumentpayment f1 left join pax.SalesDocumentHeader f2 on f1.HdrGuidRef = f2.HdrGuid ";
                    sqls += "where f1.DocumentNumber = '" + DocNum + "' and left(f1.FormofPaymentType,2) = 'CA' and f1.transactionCode <> 'RFND'";

                    SqlDataReader readers = dbconnect.GetData(sqls);
                    while (readers.Read())
                    {
                        dgpayment[0, i] = readers["FormofPayment"].ToString();
                        try
                        {
                            dgpayment[1, i] = Convert.ToDateTime(readers["DateofIssue"].ToString()).ToString("dd-MMM-yyyy");
                        }
                        catch
                        { }
                        dgpayment[2, i] = readers["OtherAmountRate"].ToString();
                        dgpayment[3, i] = readers["Currency"].ToString();
                        dgpayment[4, i] = readers["Fare"].ToString();
                        dgpayment[6, i] = readers["Amount"].ToString();
                        i++;
                    }
                }
            }
            else if ((b1 == b2) && (b3 == ""))
            {
                string sql3 = "";
                sql3 += "select left(f1.FormofPaymentType,2) as FormofPayment, f1.DateofIssue, (select OtherAmountRate from pax.SalesDocumentOtherAmount where DocumentNumber = '" + DocNum + "' ";
                sql3 += "and DocumentAmountType = 'Commission3') as OtherAmountRate from pax.salesdocumentpayment f1 ";
                sql3 += "left join pax.SalesDocumentHeader f2 on f1.HdrGuidRef = f2.HdrGuid ";
                sql3 += "where f1.DocumentNumber = '" + DocNum + "' and left(f1.FormofPaymentType,2) = 'EX'";

                SqlDataReader reader2 = dbconnect.GetData(sql3);
                int i = 0;
                while (reader2.Read())
                {
                    var details = new TicketHistoryPaymentModel();
                    dgpayment[0, i] = reader2["FormofPayment"].ToString();

                    try
                    {
                        dgpayment[1, i] = Convert.ToDateTime(reader2["DateofIssue"].ToString()).ToString("dd-MMM-yyyy");
                    }
                    catch
                    { }
                    dgpayment[2, i] = reader2["OtherAmountRate"].ToString();
                    i++;
                }

                string sql4 = "select top 1 substring(f1.AccountNumber,1, 13) as AccountNumber,ltrim(rtrim(substring(f1.AccountNumber,15, len(f1.accountnumber)))) as Cpn ";
                sql4 += "from pax.SalesDocumentPayment f1 ";
                sql4 += "left join pax.SalesDocumentHeader f2 on left(f1.accountnumber,13) = f2.DocumentNumber where f1.DocumentNumber = '" + DocNum + "'";
                SqlDataReader reader3 = dbconnect.GetData(sql4);
                while (reader3.Read())
                {
                    dgpayment[7, 0] = reader3["AccountNumber"].ToString();
                    dgpayment[8, 0] = reader3["Cpn"].ToString();
                }

                string sql5 = "SELECT f2.DateofIssue, f2.AgentNumericCode ";
                sql5 += ",iif(f2.EquivalentFare>0,f2.TotalCurrency,f2.FareCurrency) as Currency,iif(f2.EquivalentFare>0,f2.EquivalentFare,f2.Fare) as Fare, f2.TransactionCode ";
                sql5 += "from pax.SalesRelatedDocumentInformation f1 left join pax.SalesDocumentHeader f2 on f1.HdrGuid = f2.HdrGuid ";
                sql5 += "where f1.relatedDocumentNumber = '" + b1 + "'";
                SqlDataReader reader4 = dbconnect.GetData(sql5);
                while (reader4.Read())
                {
                    var details = new TicketHistoryPaymentModel();
                    dgpayment[3, 0] = reader4["Currency"].ToString();
                    dgpayment[4, 0] = reader4["Fare"].ToString();
                    try
                    {
                        dgpayment[9, 0] = Convert.ToDateTime(reader4["DateofIssue"].ToString()).ToString("dd-MMM-yyyy");
                    }
                    catch
                    { }
                    dgpayment[10, 0] = reader4["AgentNumericCode"].ToString();
                    dgpayment[11, 0] = reader4["TransactionCode"].ToString();
                }
            }
            else if ((b1 != "") && (b2 != ""))
            {
                string sql6 = "";
                sql6 += "select left(f1.FormofPaymentType,2) as FormofPayment, f1.DateofIssue from pax.salesdocumentpayment f1 ";
                sql6 += "left join pax.SalesDocumentHeader f2 on f1.HdrGuidRef = f2.HdrGuid ";
                sql6 += "where f1.DocumentNumber = '" + DocNum + "' and left(f1.FormofPaymentType,2) = 'EX'";
                SqlDataReader reader5 = dbconnect.GetData(sql6);
                while (reader5.Read())
                {
                    dgpayment[0, 0] = reader5["FormofPayment"].ToString();
                    dgpayment[0, 1] = reader5["FormofPayment"].ToString();
                    try
                    {
                        dgpayment[1, 0] = Convert.ToDateTime(reader5["DateofIssue"].ToString()).ToString("dd-MMM-yyyy");
                        dgpayment[1, 1] = Convert.ToDateTime(reader5["DateofIssue"].ToString()).ToString("dd-MMM-yyyy");
                    }
                    catch
                    { }
                }

                string sql7 = "select top 1 substring(f1.AccountNumber,1, 13) as AccountNumber,ltrim(rtrim(substring(f1.AccountNumber,15, len(f1.accountnumber)))) as Cpn ";
                sql7 += "from pax.SalesDocumentPayment f1 left join pax.SalesDocumentHeader f2 on left(f1.accountnumber,13) = f2.DocumentNumber ";
                sql7 += "where f1.DocumentNumber = '" + DocNum + "'";

                SqlDataReader reader6 = dbconnect.GetData(sql7);

                while (reader6.Read())
                {
                    dgpayment[7, 0] = reader6["AccountNumber"].ToString();
                    dgpayment[8, 0] = reader6["Cpn"].ToString();
                    dgpayment[7, 1] = b1;
                    dgpayment[8, 1] = "1";
                }

                string sql8 = "SELECT f2.DateofIssue, f2.AgentNumericCode ";
                sql8 += ", iif(f3.TicketDocumentAmount >0,f3.currencyType, iif(f2.EquivalentFare>0,f2.TotalCurrency,f2.FareCurrency)) as Currency ";
                sql8 += ", iif(f3.TicketDocumentAmount >0,f3.TicketDocumentAmount,iif(f2.EquivalentFare>0,f2.EquivalentFare,f2.Fare)) as Fare, f2.TransactionCode ";
                sql8 += "from pax.SalesRelatedDocumentInformation f1 left join [Pax].[SalesDocumentOtherAmount] f3 on f1.RelatedDocumentGuid = f3.RelatedDocumentGuid ";
                sql8 += "left join pax.SalesDocumentHeader f2 on f1.HdrGuid = f2.HdrGuid where f1.relatedDocumentNumber = '" + b2 + "' and f3.DocumentAmountType = ''";

                SqlDataReader reader7 = dbconnect.GetData(sql8);
                while (reader7.Read())
                {
                    dgpayment[3, 0] = reader7["Currency"].ToString();
                    dgpayment[4, 0] = reader7["Fare"].ToString();
                    try
                    {
                        dgpayment[9, 0] = Convert.ToDateTime(reader7["DateofIssue"].ToString()).ToString("dd-MMM-yyyy");
                    }
                    catch
                    { }
                    dgpayment[10, 0] = reader7["AgentNumericCode"].ToString();
                    dgpayment[11, 0] = reader7["TransactionCode"].ToString();
                }

                string sql9 = "SELECT f2.DateofIssue, f2.AgentNumericCode ";
                sql9 += ", iif(f3.TicketDocumentAmount >0,f3.currencyType, iif(f2.EquivalentFare>0,f2.TotalCurrency,f2.FareCurrency)) as Currency ";
                sql9 += ", iif(f3.TicketDocumentAmount >0,f3.TicketDocumentAmount,iif(f2.EquivalentFare>0,f2.EquivalentFare,f2.Fare)) as Fare, f2.TransactionCode ";
                sql9 += "from pax.SalesRelatedDocumentInformation f1 left join [Pax].[SalesDocumentOtherAmount] f3 on f1.RelatedDocumentGuid = f3.RelatedDocumentGuid ";
                sql9 += "left join pax.SalesDocumentHeader f2 on f1.HdrGuid = f2.HdrGuid where f1.relatedDocumentNumber = '" + b1 + "' and f3.DocumentAmountType = ''";

                SqlDataReader reader8 = dbconnect.GetData(sql9);
                while (reader8.Read())
                {
                    dgpayment[3, 1] = reader8["Currency"].ToString();
                    dgpayment[4, 1] = reader8["Fare"].ToString();
                    try
                    {
                        dgpayment[9, 1] = Convert.ToDateTime(reader8["DateofIssue"].ToString()).ToString("dd-MMM-yyyy");
                    }
                    catch
                    {
                    }
                    dgpayment[10, 1] = reader8["AgentNumericCode"].ToString();
                    dgpayment[11, 1] = reader8["TransactionCode"].ToString();
                }
            }
            else if ((b2 != b1) && (b3 == ""))
            {
                string sql10 = "";
                sql10 += "select top 1 left(f1.FormofPaymentType,2) as FormofPayment, f1.DateofIssue,(select OtherAmountRate from pax.SalesDocumentOtherAmount where DocumentNumber = '" + DocNum + "' ";
                sql10 += "and DocumentAmountType = 'Commission3') as OtherAmountRate,";
                sql10 += "iif(f2.EquivalentFare>0,f2.TotalCurrency,f2.FareCurrency) as Currency,iif(f2.EquivalentFare>0,f2.EquivalentFare,f2.Fare) as Fare ,f1.Amount ";
                sql10 += "from pax.salesdocumentpayment f1 left join pax.SalesDocumentHeader f2 on f1.HdrGuidRef = f2.HdrGuid where f1.DocumentNumber = '" + DocNum + "' ";
                sql10 += "and left(f1.FormofPaymentType,2) <> 'CA'";
                SqlDataReader reader9 = dbconnect.GetData(sql10);
                int i = 0;
                while (reader9.Read())
                {
                    dgpayment[0, i] = reader9["FormofPayment"].ToString();

                    try
                    {
                        dgpayment[1, i] = Convert.ToDateTime(reader9["DateofIssue"].ToString()).ToString("dd-MMM-yyyy");
                    }
                    catch
                    {
                    }
                    dgpayment[2, i] = reader9["OtherAmountRate"].ToString();
                    dgpayment[3, i] = reader9["Curry"].ToString();
                    dgpayment[4, i] = reader9["Fare"].ToString();
                    dgpayment[6, i] = reader9["Amount"].ToString();
                    i++;
                }

                if (i == 0)  // compte list 
                {
                    string sql11 = "";
                    sql11 += "select top 1 left(f1.FormofPaymentType,2) as FormofPayment, f1.DateofIssue, (select OtherAmountRate from pax.SalesDocumentOtherAmount where DocumentNumber = '" + DocNum + "' and DocumentAmountType = 'Commission3') as OtherAmountRate, ";
                    sql11 += "iif(f2.EquivalentFare>0,f2.TotalCurrency,f2.FareCurrency) as Currency,iif(f2.EquivalentFare>0,f2.EquivalentFare,f2.Fare) as Fare, f1.Amount ";
                    sql11 += "from pax.salesdocumentpayment f1 left join pax.SalesDocumentHeader f2 on f1.HdrGuidRef = f2.HdrGuid where f1.DocumentNumber = '" + DocNum + "' ";
                    sql11 += "and left(f1.FormofPaymentType,2) = 'CA'";
                    SqlDataReader reader10 = dbconnect.GetData(sql11);
                    while (reader10.Read())
                    {
                        dgpayment[0, i] = reader10["FormofPayment"].ToString();

                        try
                        {
                            dgpayment[1, i] = Convert.ToDateTime(reader10["DateofIssue"].ToString()).ToString("dd-MMM-yyyy");
                        }
                        catch
                        { }
                        dgpayment[2, i] = reader10["OtherAmountRate"].ToString();
                        dgpayment[3, i] = reader10["Currency"].ToString();
                        dgpayment[4, i] = reader10["Fare"].ToString();
                        dgpayment[6, i] = reader10["Amount"].ToString();
                    }
                }
                else if ((b2 != "") && (b3 != ""))
                {
                    int ko = dgpayment.Length; // compt nb list
                    string sql12 = "";

                    sql12 += "select left(f1.FormofPaymentType,2) as FormofPayment, f1.DateofIssue from pax.salesdocumentpayment f1 left join pax.SalesDocumentHeader f2 on f1.HdrGuidRef = f2.HdrGuid ";
                    sql12 += "where f1.DocumentNumber = '" + DocNum + "' and left(f1.FormofPaymentType,2) = 'EX'";
                    SqlDataReader reader11 = dbconnect.GetData(sql12);
                    int p = 0;
                    while (reader11.Read())
                    {
                        dgpayment[0, p] = reader11["FormofPayment"].ToString();
                        try
                        {
                            dgpayment[1, p] = Convert.ToDateTime(reader11["DateofIssue"].ToString()).ToString("dd-MMM-yyyy");
                        }
                        catch
                        { }
                    }

                    string sql13 = "select substring(f1.AccountNumber,1, 13) as AccountNumber,ltrim(rtrim(substring(f1.AccountNumber,15, len(f1.accountnumber)))) as Cpn ";
                    sql13 += "from pax.SalesDocumentPayment f1 left join pax.SalesRelatedDocumentInformation f2 on left(f1.accountnumber,13) = f2.relatedDocumentNumber ";
                    sql13 += "where f1.DocumentNumber = '" + DocNum + "' and left(f1.FormofPaymentType,2) = 'EX'";
                    SqlDataReader reader12 = dbconnect.GetData(sql13);
                    int uu = 0;

                    while (reader12.Read())
                    {
                        dgpayment[7, uu] = reader12["AccountNumber"].ToString();
                        dgpayment[8, uu] = reader12["Cpn"].ToString();
                    }

                    string sql14 = "SELECT f2.DateofIssue, f2.AgentNumericCode ";
                    sql14 += ",iif(f2.EquivalentFare>0,f2.TotalCurrency,f2.FareCurrency) as Currency,iif(f2.EquivalentFare>0,f2.EquivalentFare,f2.Fare) as Fare, f2.TransactionCode ";
                    sql14 += "from pax.SalesRelatedDocumentInformation f1 left join pax.SalesDocumentHeader f2 on f1.HdrGuid = f2.HdrGuid where f1.relatedDocumentNumber = '" + b2 + "'";
                    SqlDataReader reader13 = dbconnect.GetData(sql14);
                    while (reader13.Read())
                    {
                        dgpayment[3, 0] = reader13["Currency"].ToString();
                        dgpayment[4, 0] = reader13["Fare"].ToString();
                        try
                        {
                            dgpayment[9, 0] = Convert.ToDateTime(reader13["DateofIssue"].ToString()).ToString("dd-MMM-yyyy");
                        }
                        catch
                        { }
                        dgpayment[10, 0] = reader13["AgentNumericCode"].ToString();
                        dgpayment[11, 0] = reader13["TransactionCode"].ToString();
                    }

                    string sql15 = "SELECT f2.DateofIssue, f2.AgentNumericCode ";
                    sql15 += ",iif(f2.EquivalentFare>0,f2.TotalCurrency,f2.FareCurrency) as Currency,iif(f2.EquivalentFare>0,f2.EquivalentFare,f2.Fare) as Fare, f2.transactionCode ";
                    sql15 += "from pax.SalesRelatedDocumentInformation f1 left join pax.SalesDocumentHeader f2 on f1.HdrGuid = f2.HdrGuid where f1.relatedDocumentNumber = '" + b3 + "'";

                    SqlDataReader reader14 = dbconnect.GetData(sql15);
                    while (reader14.Read())
                    {
                        dgpayment[3, 1] = reader14["Currency"].ToString();
                        dgpayment[4, 1] = reader14["Fare"].ToString();
                        try
                        {
                            dgpayment[9, 1] = Convert.ToDateTime(reader14["DateofIssue"].ToString()).ToString("dd-MMM-yyyy");
                        }
                        catch
                        {
                        }
                        dgpayment[10, 1] = reader14["AgentNumericCode"].ToString();
                        dgpayment[11, 1] = reader14["TransactionCode"].ToString();
                    }
                }

                double sum = 0;
                var dgSalesAmount = SaleAmount(DocNum);
                for (int p = 0; p < dgpayment.Length; p++)
                {
                    try
                    {
                        sum += valid(dgpayment[4, p].ToString());
                    }
                    catch
                    { }
                }

                try
                {
                    if (sum < valid(dgSalesAmount[1, 0].ToString()))
                    {
                        int z = dgpayment.Length - 1;

                        dgpayment[0, z] = "CA";
                        dgpayment[1, z] = dgpayment[1, 0];
                        dgpayment[3, z] = dgpayment[3, 0];
                        dgpayment[4, z] = (sum - valid(dgSalesAmount[1, 0].ToString())).ToString();
                        // dgpayment[5, z].Value = "CA";
                    }

                }
                catch { }
            }

            /* coupon */
            string sqlcoup = "select f1.CouponNumber, f1.OriginCity, f1.DestinationCity,f1.Carrier,f1.ReservationBookingDesignator ";
            sqlcoup += ",f1.FareBasisTicketDesignator, f1.FlightNumber,f1.FlightDepartureDate,f1.CouponStatus, f2.FinalShare ";
            sqlcoup += "from pax.SalesDocumentCoupon f1 left join pax.ProrationDetail f2 on f1.RelatedDocumentGuid = f2.RelatedDocumentGuid ";
            sqlcoup += "and f1.CouponNumber = f2.CouponNumber and f1.CouponStatus = f2.ProrationFlag WHERE f1.relatedDocumentNumber = '" + DocNum + "'";
            SqlDataReader readerCoup = dbconnect.GetData(sqlcoup);

            List<TicketHistoryPaymentModel> model = new List<TicketHistoryPaymentModel>();
            while (readerCoup.Read())
            {
                var details = new TicketHistoryPaymentModel();
                details.CouponNumber = readerCoup["CouponNumber"].ToString();
                details.OriginCity = readerCoup["OriginCity"].ToString();
                details.DestinationCity = readerCoup["DestinationCity"].ToString();
                details.Carrier = readerCoup["Carrier"].ToString();
                details.ReservationBookingDesignator = readerCoup["ReservationBookingDesignator"].ToString();
                details.FareBasisTicketDesignator = readerCoup["FareBasisTicketDesignator"].ToString();
                details.FlightNumber = readerCoup["FlightNumber"].ToString();
                details.FlightDepartureDate = Convert.ToDateTime(readerCoup["FlightDepartureDate"].ToString()).ToString("dd-MMM-yyyy");
                details.CouponStatus = readerCoup["CouponStatus"].ToString();
                details.FinalShare = readerCoup["FinalShare"].ToString();
                model.Add(details);

            }
            /* uses */
            string sqluse = "";

            sqluse += " select f1.CouponStatus, f1.CouponNumber, case f1.couponstatus ";
            sqluse += " when 'F' then CONCAT(usageairline, ' ', flightnumber, ' ',Usageorigincode,UsageDestinationCode) ";
            sqluse += " when 'E' then concat((select DocumentNumber from pax.salesdocumentpayment where AccountNumber like '" + DocNum + "%'),' ',(select Agentnumericcode from pax.SalesDocumentHeader where DocumentNumber = (select DocumentNumber from pax.salesdocumentpayment where AccountNumber like '" + DocNum + "%'))) ";
            sqluse += " when 'R' then CONCAT ((select AgentnumericCode from pax.salesdocumentheader where documentnumber = '" + DocNum + "' and TransactionCode = 'RFND'), ' ', (select top 1 formofpaymenttype from pax.SalesDocumentPayment where TransactionCode = 'RFND' and DocumentNumber = '" + DocNum + "')) ";
            sqluse += " end as Regkey, f1.UsageDate, f2.FinalShare from pax.SalesDocumentCoupon f1  left join pax.ProrationDetail f2 on f1.RelatedDocumentGuid = f2.RelatedDocumentGuid ";
            sqluse += " and f1.CouponNumber = f2.CouponNumber and f1.CouponStatus = f2.ProrationFlag where f1.DocumentNumber = '" + DocNum + "' and f1.CouponStatus not in ('S') Order by 2";
            SqlDataReader readerUse = dbconnect.GetData(sqluse);
            List<TicketUsesModel> modelUse = new List<TicketUsesModel>();
            while (readerUse.Read())
            {
                var detailUse = new TicketUsesModel();
                detailUse.CouponStatus = readerUse["CouponStatus"].ToString();
                detailUse.CouponNumber = readerUse["CouponNumber"].ToString();
                detailUse.Regkey = readerUse["Regkey"].ToString();
                try
                {
                    detailUse.UsageDate = Convert.ToDateTime(readerUse["UsageDate"].ToString()).ToString("dd-MMM-yyyy");
                }
                catch
                {
                }

                detailUse.FinalShare = readerUse["FinalShare"].ToString();
                modelUse.Add(detailUse);
            }
            /* historique doc numb */

            string[] t = new string[11];
            string[] dg1 = new string[11];
            dg1[0] = DocNum;
            t[0] = DocNum;

            string sqlHisto = " select Concat(DocumentNumber, ' ', transactioncode) as DocumentTransaction from pax.SalesDocumentHeader  where DocumentNumber = '" + DocNum + "' and TransactionCode <> 'RFND'";

            SqlDataReader readerHisto = dbconnect.GetData(sqlHisto);
            while (readerHisto.Read())
            {
                t[0] = readerHisto["DocumentTransaction"].ToString();
            }
            int h = 0;
            for (int i = 0; i < 10; i++)
            {
                string g = "";
                try
                {
                    g = t[i].Substring(0, 13).ToString();
                }
                catch
                {

                }

                string sqlHisto1 = "SELECT concat(left(f1.AccountNumber,13), ' ', f2.transactioncode) as AccountTransaction from pax.SalesDocumentPayment f1 left join pax.SalesRelatedDocumentInformation f2 on left(f1.AccountNumber,13) = f2.relateddocumentnumber  where f1.DocumentNumber = '" + g + "' AND AccountNumber <> '' and f2.TransactionCode <> ''";
                SqlDataReader readerHisto1 = dbconnect.GetData(sqlHisto1);
                h = i;

                while (readerHisto1.Read())
                {
                    t[h + 1] = readerHisto1["AccountTransaction"].ToString();
                    h++;
                }
            }

            int k = 0;
            for (int i = 10; i >= 0; i--)
            {
                if (t[i] != null)
                {
                    dg1[k] = t[i];
                    k++;
                }
            }
            var ds = SalesTicketHistory(DocNum, TransactionCode);
            string agentNumCode = "%";
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                agentNumCode = dr[ds.Tables[0].Columns[5].ColumnName].ToString();
            }
            ViewBag.agentDetail = AgentDetails(agentNumCode);
            ViewBag.salesAmount = SaleAmount(DocNum);
            ViewBag.historyDocNum = dg1;
            ViewBag.uses = modelUse;
            ViewBag.coupon = model;
            ViewBag.payment = dgpayment;
            if (Request["conjonction"] == "valide")
            {
                return PartialView("SalesTicketHistory", ds);
            }
            else
            {
                return PartialView(ds);
            }

        }
        public double valid(string a)
        {
            double x = string.IsNullOrEmpty(a) ? 0 : Convert.ToDouble(a);
            return x;
        }

        private string Val(string sql)
        {
            string a = "";
            dbconnect.pbConnectionString = pbConnectionString;
            SqlDataReader myReader = dbconnect.GetData(sql);
            while (myReader.Read())
            {
                a = myReader["DocumentNumber"].ToString();
            }
            return a;
        }

        private string[,] SaleAmount(string docnum)
        {
            string[,] dgSalesAmount = new string[7, 7];
            try
            {
                string sql = "select iif(f1.EquivalentFare>0,f1.TotalCurrency,f1.FareCurrency) as Currency,iif(f1.EquivalentFare>0,f1.EquivalentFare,f1.Fare) as Fare,f1.CommissionCollected, sum(f2.otherAmount) as Sumotheramount,";
                sql += "(select OtherAmount from pax.SalesDocumentOtherAmount where OtherAmountCode = 'YR' and DocumentNumber = '" + docnum + "' )  as otheramount ";
                sql += "from pax.salesdocumentheader f1 join pax.SalesRelatedDocumentInformation f3 on f1.HdrGuid = f3.HdrGuid ";
                sql += "join pax.SalesDocumentOtherAmount f2 on f3.RelatedDocumentGuid = f2.RelatedDocumentGuid ";
                sql += "where f2.OtherAmountCode <>'YR' and f2.TransactionCode <> 'RFND' and f1.DocumentNumber = '" + docnum + "'";
                sql += "group by f1.TotalCurrency,f1.EquivalentFare,f1.FareCurrency, f1.Fare, f1.CommissionCollected";

                SqlDataReader reader = dbconnect.GetData(sql);
                int i = 0;
                string lblFareCOC;
                while (reader.Read())
                {
                    lblFareCOC = reader.GetValue(0).ToString() + " " + reader.GetValue(1).ToString();

                    dgSalesAmount[0, i] = reader.GetValue(0).ToString();
                    dgSalesAmount[1, i] = reader.GetValue(1).ToString();
                    dgSalesAmount[2, i] = reader.GetValue(2).ToString();
                    dgSalesAmount[4, i] = reader.GetValue(3).ToString();
                    dgSalesAmount[5, i] = reader.GetValue(4).ToString();
                    i++;
                }
            }
            catch { }
            return dgSalesAmount;
        }

        /************************************************frmPaymentTfcComm****************************************************/
        public ActionResult FrmOfPayementTfcComm()
        {
            dbconnect.pbConnectionString = pbConnectionString;
            string strSQL = "";
            string docNumber = Request["docNumber"];
            strSQL += "SELECT f1.DocumentNumber,item.FormofPaymentType as FOP, item.Currency as CUR, item.Amount as AmountCollected, item.RemittanceCurrency as remitCur,item.TransactionCode as RemitanceAmount,item.RemittanceAmount as TransactionCode, item.DateofIssue as IssueDate ";
            strSQL = strSQL + " from pax.SalesDocumentHeader F1 join pax.SalesRelatedDocumentInformation F2 on f1.HdrGuid = f2.HdrGuid join pax.SalesDocumentPayment item on f2.RelatedDocumentGuid = item.RelatedDocumentGuid where f1.DocumentNumber='" + docNumber + "'";


            SqlDataReader myReaderPayement = dbconnect.GetData(strSQL);
            List<FormOfPayementModel> model = new List<FormOfPayementModel>();
            while (myReaderPayement.Read())
            {
                var details = new FormOfPayementModel();
                details.FOP = myReaderPayement["FOP"].ToString();
                details.CUR = myReaderPayement["CUR"].ToString();
                details.AmountCollected = myReaderPayement["AmountCollected"].ToString();
                details.RemitanceAmount = myReaderPayement["RemitanceAmount"].ToString();
                details.remitCur = myReaderPayement["remitCur"].ToString();
                details.TransactionCode = myReaderPayement["TransactionCode"].ToString();
                details.IssueDate = myReaderPayement["IssueDate"].ToString();

                model.Add(details);
            }

            ViewBag.FrmPayement = model;
            return PartialView();
        }

        private List<FormOfPayementModel> GetDataOfPayement(String StrSQL)
        {
            dbconnect.pbConnectionString = pbConnectionString;
            SqlDataReader myReaderPayement = dbconnect.GetData(StrSQL);
            List<FormOfPayementModel> model = new List<FormOfPayementModel>();
            while (myReaderPayement.Read())
            {
                var details = new FormOfPayementModel();
                details.FOP = myReaderPayement["FOP"].ToString();
                details.CUR = myReaderPayement["CUR"].ToString();
                details.AmountCollected = myReaderPayement["AmountCollected"].ToString();
                details.RemitanceAmount = myReaderPayement["RemitanceAmount"].ToString();
                details.TransactionCode = myReaderPayement["TransactionCode"].ToString();
                details.IssueDate = myReaderPayement["IssueDate"].ToString();

                model.Add(details);
            }
            return model;
        }

        /******************************************Get TFCs****************************************************************/
        public ActionResult GetTf()
        {
            dbconnect.pbConnectionString = pbConnectionString;
            string strSQL = "";
            string docNumber = Request["docNumber"];
            strSQL += "SELECT F3.DocumentAmountType as TFC, F3.OtherAmountCode as TFCCode, F3.CurrencyType as CUR, F3.OtherAmount as Amount,F3.OtherAmountRate as Rate";
            strSQL = strSQL + " from  pax.SalesDocumentHeader F1 join pax.SalesRelatedDocumentInformation F2 on f1.HdrGuid=f2.HdrGuid join pax.SalesDocumentOtherAmount f3 on f2.RelatedDocumentGuid=f3.RelatedDocumentGuid where f1.DocumentNumber='" + docNumber + "' ";

            SqlDataReader MyReaderTFCs = dbconnect.GetData(strSQL);
            List<TFCsModel> model = new List<TFCsModel>();

            while (MyReaderTFCs.Read())
            {
                var details = new TFCsModel();
                details.TFC = MyReaderTFCs["TFC"].ToString();
                details.TFCCode = MyReaderTFCs["TFCCode"].ToString();
                details.CUR = MyReaderTFCs["CUR"].ToString();
                details.Amount = MyReaderTFCs["Amount"].ToString();
                details.Rate = MyReaderTFCs["Rate"].ToString();

                model.Add(details);
            }

            ViewBag.TFCs = model;

            return PartialView();

        }

        private List<TFCsModel> GetDataOfTFCs(String StrSQL)
        {
            dbconnect.pbConnectionString = pbConnectionString;
            SqlDataReader MyReaderTFCs = dbconnect.GetData(StrSQL);
            List<TFCsModel> model = new List<TFCsModel>();

            while (MyReaderTFCs.Read())
            {

                var details = new TFCsModel();
                details.TFC = MyReaderTFCs["TFC"].ToString();
                details.TFCCode = MyReaderTFCs["TFCCode"].ToString();
                details.CUR = MyReaderTFCs["CUR"].ToString();
                details.Amount = MyReaderTFCs["Amount"].ToString();
                details.Rate = MyReaderTFCs["Rate"].ToString();

                model.Add(details);
            }
            return model;
        }

        /*********************************************ViewMore DEtails***********************************************************************/
        public ActionResult ViewsMoreDetails()
        {
            string DocNo = Request["docNumber"];
            dbconnect.pbConnectionString = pbConnectionString;

            string StrSQL = "SELECT [DocumentNumber],[CheckDigit],[AgentNumericCode],FORMAT(DateofIssue,'dd-MMM-yyyy') as DateofIssue,FORMAT(SaleDate,'dd-MMM-yyyy') as SaleDate";
            StrSQL += ",[PassengerName],[PassengerSpecificData],[FareCalculationArea],[EndosRestriction],[OriginalIssueInformation] ";
            StrSQL += " ,[OriginalIssueDocumentNumber],[OriginalIssueCity],[OriginalIssueDate] as OriginalIssueDate ,[OriginalIssueAgentNumericCode] ";
            StrSQL += " ,[InConnectionWithDocumentNumber] ,concat([FareCurrency]+' ',[Fare]) as Fare ";
            StrSQL += " ,concat(iif([EquivalentFare] > 0 ,[TotalCurrency]+' ',''),[EquivalentFare]) as [EquivalentFare], ";

            StrSQL += "(select Case when exists(select [Tax1] from [FileHot].[Bar64_DocumentAmount] where ticketdocumentnumber ='" + DocNo + "') then ";
            StrSQL += "concat(iif(";
            StrSQL += "(select [Tax1] from [FileHot].[Bar64_DocumentAmount] where ticketdocumentnumber = '" + DocNo + "') ='', ";
            StrSQL += "concat(iif(Tax1Currency is null, totalcurrency,Tax1Currency), Tax1Amount),";
            StrSQL += "(select substring([Tax1],1,len(Tax1) -2) from [FileHot].[Bar64_DocumentAmount] where ticketdocumentnumber = '" + DocNo + "') ";
            StrSQL += "),Tax1Code) ";
            StrSQL += "else(concat(iif(Tax1Currency is null, totalcurrency,Tax1Currency), Tax1Amount, Tax1Code))end) as Tax1, ";

            StrSQL += "(select Case when exists(select [Tax2] from [FileHot].[Bar64_DocumentAmount] where ticketdocumentnumber = '" + DocNo + "') then ";
            StrSQL += "concat(iif(";
            StrSQL += "(select [Tax2] from [FileHot].[Bar64_DocumentAmount] where ticketdocumentnumber  = '" + DocNo + "') ='', ";
            StrSQL += "concat(iif(Tax2Currency is null, totalcurrency,Tax2Currency), Tax2Amount), ";
            StrSQL += "(select substring([Tax2],1,len(Tax2) -2) from [FileHot].[Bar64_DocumentAmount] where ticketdocumentnumber = '" + DocNo + "') ";
            StrSQL += "),Tax2Code) ";
            StrSQL += "else(concat(iif(Tax2Currency is null, totalcurrency,Tax2Currency), Tax2Amount, Tax2Code))end) as Tax2, ";

            StrSQL += "(select Case when exists(select [Tax1] from [FileHot].[Bar64_DocumentAmount] where ticketdocumentnumber ='" + DocNo + "') then ";
            StrSQL += "concat(iif(";
            StrSQL += "(select [Tax3] from [FileHot].[Bar64_DocumentAmount] where ticketdocumentnumber ='" + DocNo + "') ='', ";
            StrSQL += "concat(iif(Tax3Currency is null, totalcurrency,Tax3Currency), Tax3Amount), ";
            StrSQL += "(select substring([Tax3],1,len(Tax3) -2) from [FileHot].[Bar64_DocumentAmount] where ticketdocumentnumber = '" + DocNo + "') ";
            StrSQL += "),Tax3Code) ";
            StrSQL += "else(concat(iif(Tax3Currency is null, totalcurrency,Tax3Currency), Tax3Amount, Tax3Code))end) as Tax3 ";

            StrSQL += ",concat(isnull([AmountCollectedCurrency]  ,   [TotalCurrency]) + ' ' , [TotalAmount]) as [TotalAmount] ";
            StrSQL += ",[SequenceNumber],[ITBT],[ExchangeADC],[DocumentType],[TicketingModeIndicator],[FareCalculationModeIndicator] ";
            StrSQL += ",[BookingAgentIdentification],[BookingAgencyLocationNumber],[BookingEntityOutletType],[BookingReference] ";
            StrSQL += ",[AuditStatusIndicator],[ClientIdentification],[CommercialAgreementReference],[CustomerFileReference] ";
            StrSQL += ",[DataInputStatusIndicator],[FareCalculationPricingIndicator],[FormatIdentifier],[TourCode],[NetReportingIndicator] ";
            StrSQL += ",[NeutralTicketingSystemIdentifier],[ReportingSystemIdentifier],[ServicingAirlineSystemProviderIdentifier] ";
            StrSQL += ",[TrueOriginDestinationCityCodes],[SettlementAuthorizationCode],[VendorIdentification],[VendorISOCountryCode] ";
            StrSQL += ",[TransactionCode],[SalesDataAvailable],[AccountingStatus],[ProratedFlag],[FBSFlag],[DocumentAirlineID] ";
            StrSQL += ",[TransactionGroup],[PaxSPC],[PaxType],[SalesPeriod],[PrevMOS],concat([TaxCurrency] +' ',[TotalTax]) as [TotalTax] ";
            StrSQL += ",[BookingAgentID],[TicketingAgentID],[IsReissue],[DataSource],[ProcessingFileType],[ProcessingDate],[OwnTicket] ";
            StrSQL += ",[OwnCarrier],[OwnISOCountry],[OwnAirline],[HostCurrency] ";
            StrSQL += ",concat([AmountCollectedCurrency] +' ',[AmountCollected] ) as [AmountCollected] ";
            StrSQL += ",concat([TaxCollectedCurrency] +' ',[TaxCollected] ) as [TaxCollected] ";
            StrSQL += ",concat([SurchargeCollectedCurrency] +' ',[SurchargeCollected] ) as [SurchargeCollected] ";
            StrSQL += ",concat([CommissionCollectedCurrency] +' ',[CommissionCollected] ) as [CommissionCollected] ";
            StrSQL += ",[BilateralEndorsement],[InvoluntaryReroute],[BspIdentifier],[IsoCountryCode],[USDRatePayCur],[USDRateHostCur],[PMPPeriod] ";
            StrSQL += ",concat([TaxOnCommissionCollectedCurrency]+' ',[TaxOnCommissionCollected] ) as [TaxOnCommissionCollected],[SignCode] ";

            StrSQL += " from [Pax].[SalesDocumentHeader]";
            StrSQL += " WHERE [DocumentNumber]='" + DocNo + "' ";


            SqlDataReader myReaderMore = dbconnect.GetData(StrSQL);
            List<MoreDetailsModel> model = new List<MoreDetailsModel>();
            while (myReaderMore.Read())
            {
                var details = new MoreDetailsModel();

                /*details.FieldName = myReaderMore["FieldName"].ToString();
                details.DataValue = myReaderMore["DataValue"].ToString();*/

                //=====================================================================

                details.DocumentNumber = myReaderMore["DocumentNumber"].ToString();
                details.CheckDigit = myReaderMore["CheckDigit"].ToString();

                details.AgentNumericCode = myReaderMore["AgentNumericCode"].ToString();
                details.DateofIssue = myReaderMore["DateofIssue"].ToString();
                details.SaleDate = myReaderMore["SaleDate"].ToString();
                details.PassengerName = myReaderMore["PassengerName"].ToString();
                details.PassengerSpecificData = myReaderMore["PassengerSpecificData"].ToString();
                details.FareCalculationArea = myReaderMore["FareCalculationArea"].ToString();
                details.EndosRestriction = myReaderMore["EndosRestriction"].ToString();
                details.OriginalIssueInformation = myReaderMore["OriginalIssueInformation"].ToString();
                details.OriginalIssueDocumentNumber = myReaderMore["OriginalIssueDocumentNumber"].ToString();
                details.OriginalIssueCity = myReaderMore["OriginalIssueCity"].ToString();
                details.OriginalIssueDate = myReaderMore["OriginalIssueDate"].ToString();
                details.OriginalIssueAgentNumericCode = myReaderMore["OriginalIssueAgentNumericCode"].ToString();
                details.InConnectionWithDocumentNumber = myReaderMore["InConnectionWithDocumentNumber"].ToString();
                details.Fare = myReaderMore["Fare"].ToString();
                details.EquivalentFare = myReaderMore["EquivalentFare"].ToString();
                details.Tax1 = myReaderMore["Tax1"].ToString();
                details.Tax2 = myReaderMore["Tax2"].ToString();
                details.Tax3 = myReaderMore["Tax3"].ToString();
                details.TotalAmount = myReaderMore["TotalAmount"].ToString();
                details.SequenceNumber = myReaderMore["SequenceNumber"].ToString();
                details.ITBT = myReaderMore["ITBT"].ToString();
                details.ExchangeADC = myReaderMore["ExchangeADC"].ToString();
                details.DocumentType = myReaderMore["DocumentType"].ToString();
                details.TicketingModeIndicator = myReaderMore["TicketingModeIndicator"].ToString();
                details.FareCalculationModeIndicator = myReaderMore["FareCalculationModeIndicator"].ToString();
                details.BookingAgentIdentification = myReaderMore["BookingAgentIdentification"].ToString();
                details.BookingAgencyLocationNumber = myReaderMore["BookingAgencyLocationNumber"].ToString();
                details.BookingEntityOutletType = myReaderMore["BookingEntityOutletType"].ToString();
                details.BookingReference = myReaderMore["BookingReference"].ToString();
                details.AuditStatusIndicator = myReaderMore["AuditStatusIndicator"].ToString();
                details.ClientIdentification = myReaderMore["ClientIdentification"].ToString();
                details.CommercialAgreementReference = myReaderMore["CommercialAgreementReference"].ToString();
                details.CustomerFileReference = myReaderMore["CustomerFileReference"].ToString();
                details.DataInputStatusIndicator = myReaderMore["DataInputStatusIndicator"].ToString();
                details.FareCalculationPricingIndicator = myReaderMore["FareCalculationPricingIndicator"].ToString();
                details.FormatIdentifier = myReaderMore["FormatIdentifier"].ToString();
                details.TourCode = myReaderMore["TourCode"].ToString();
                details.NetReportingIndicator = myReaderMore["NetReportingIndicator"].ToString();
                details.NeutralTicketingSystemIdentifier = myReaderMore["NeutralTicketingSystemIdentifier"].ToString();
                details.ReportingSystemIdentifier = myReaderMore["ReportingSystemIdentifier"].ToString();
                details.ServicingAirlineSystemProviderIdentifier = myReaderMore["ServicingAirlineSystemProviderIdentifier"].ToString();
                details.TrueOriginDestinationCityCodes = myReaderMore["TrueOriginDestinationCityCodes"].ToString();
                details.SettlementAuthorizationCode = myReaderMore["SettlementAuthorizationCode"].ToString();
                details.VendorIdentification = myReaderMore["VendorIdentification"].ToString();
                details.VendorISOCountryCode = myReaderMore["VendorISOCountryCode"].ToString();
                details.TransactionCode = myReaderMore["TransactionCode"].ToString();
                details.SalesDataAvailable = myReaderMore["SalesDataAvailable"].ToString();
                details.AccountingStatus = myReaderMore["AccountingStatus"].ToString();
                details.ProratedFlag = myReaderMore["ProratedFlag"].ToString();
                details.FBSFlag = myReaderMore["FBSFlag"].ToString();
                details.DocumentAirlineID = myReaderMore["DocumentAirlineID"].ToString();
                details.TransactionGroup = myReaderMore["TransactionGroup"].ToString();
                details.PaxSPC = myReaderMore["PaxSPC"].ToString();
                details.PaxType = myReaderMore["PaxType"].ToString();
                details.SalesPeriod = myReaderMore["SalesPeriod"].ToString();
                details.PrevMOS = myReaderMore["PrevMOS"].ToString();
                details.TotalTax = myReaderMore["TotalTax"].ToString();
                details.BookingAgentID = myReaderMore["BookingAgentID"].ToString();
                details.TicketingAgentID = myReaderMore["TicketingAgentID"].ToString();
                details.IsReissue = myReaderMore["IsReissue"].ToString();
                details.DataSource = myReaderMore["DataSource"].ToString();
                details.ProcessingFileType = myReaderMore["ProcessingFileType"].ToString();
                details.ProcessingDate = myReaderMore["ProcessingDate"].ToString();
                details.OwnTicket = myReaderMore["OwnTicket"].ToString();
                details.OwnCarrier = myReaderMore["OwnCarrier"].ToString();
                details.OwnISOCountry = myReaderMore["OwnISOCountry"].ToString();
                details.OwnAirline = myReaderMore["OwnAirline"].ToString();
                details.HostCurrency = myReaderMore["HostCurrency"].ToString();
                details.AmountCollected = myReaderMore["AmountCollected"].ToString();
                details.TaxCollected = myReaderMore["TaxCollected"].ToString();
                details.SurchargeCollected = myReaderMore["SurchargeCollected"].ToString();
                details.CommissionCollected = myReaderMore["CommissionCollected"].ToString();
                details.BilateralEndorsement = myReaderMore["BilateralEndorsement"].ToString();
                details.InvoluntaryReroute = myReaderMore["InvoluntaryReroute"].ToString();
                details.BspIdentifier = myReaderMore["BspIdentifier"].ToString();
                details.IsoCountryCode = myReaderMore["IsoCountryCode"].ToString();
                details.USDRatePayCur = myReaderMore["USDRatePayCur"].ToString();
                details.USDRateHostCur = myReaderMore["USDRateHostCur"].ToString();
                details.PMPPeriod = myReaderMore["PMPPeriod"].ToString();
                details.TaxOnCommissionCollected = myReaderMore["TaxOnCommissionCollected"].ToString();
                details.SignCode = myReaderMore["SignCode"].ToString();
                model.Add(details);
            }

            ViewBag.MoreDetails = model;
            return PartialView();
        }

        private List<MoreDetailsModel> GetDataOfMore(String StrSQL)
        {
            dbconnect.pbConnectionString = pbConnectionString;
            SqlDataReader myReaderMore = dbconnect.GetData(StrSQL);
            List<MoreDetailsModel> model = new List<MoreDetailsModel>();
            while (myReaderMore.Read())
            {
                var details = new MoreDetailsModel();


                details.DocumentNumber = myReaderMore["DocumentNumber"].ToString();
                details.CheckDigit = myReaderMore["CheckDigit"].ToString();

                details.AgentNumericCode = myReaderMore["AgentNumericCode"].ToString();
                details.DateofIssue = myReaderMore["DateofIssue"].ToString();
                details.SaleDate = myReaderMore["SaleDate"].ToString();
                details.PassengerName = myReaderMore["PassengerName"].ToString();
                details.PassengerSpecificData = myReaderMore["PassengerSpecificData"].ToString();
                details.FareCalculationArea = myReaderMore["FareCalculationArea"].ToString();
                details.EndosRestriction = myReaderMore["EndosRestriction"].ToString();
                details.OriginalIssueInformation = myReaderMore["OriginalIssueInformation"].ToString();
                details.OriginalIssueDocumentNumber = myReaderMore["OriginalIssueDocumentNumber"].ToString();
                details.OriginalIssueCity = myReaderMore["OriginalIssueCity"].ToString();
                details.OriginalIssueDate = myReaderMore["OriginalIssueDate"].ToString();
                details.OriginalIssueAgentNumericCode = myReaderMore["OriginalIssueAgentNumericCode"].ToString();
                details.InConnectionWithDocumentNumber = myReaderMore["InConnectionWithDocumentNumber"].ToString();
                details.Fare = myReaderMore["Fare"].ToString();
                details.EquivalentFare = myReaderMore["EquivalentFare"].ToString();
                details.Tax1 = myReaderMore["Tax1"].ToString();
                details.Tax2 = myReaderMore["Tax2"].ToString();
                details.Tax3 = myReaderMore["Tax3"].ToString();
                details.TotalAmount = myReaderMore["TotalAmount"].ToString();
                details.SequenceNumber = myReaderMore["SequenceNumber"].ToString();
                details.ITBT = myReaderMore["ITBT"].ToString();
                details.ExchangeADC = myReaderMore["ExchangeADC"].ToString();
                details.DocumentType = myReaderMore["DocumentType"].ToString();
                details.TicketingModeIndicator = myReaderMore["TicketingModeIndicator"].ToString();
                details.FareCalculationModeIndicator = myReaderMore["FareCalculationModeIndicator"].ToString();
                details.BookingAgentIdentification = myReaderMore["BookingAgentIdentification"].ToString();
                details.BookingAgencyLocationNumber = myReaderMore["BookingAgencyLocationNumber"].ToString();
                details.BookingEntityOutletType = myReaderMore["BookingEntityOutletType"].ToString();
                details.BookingReference = myReaderMore["BookingReference"].ToString();
                details.AuditStatusIndicator = myReaderMore["AuditStatusIndicator"].ToString();
                details.ClientIdentification = myReaderMore["ClientIdentification"].ToString();
                details.CommercialAgreementReference = myReaderMore["CommercialAgreementReference"].ToString();
                details.CustomerFileReference = myReaderMore["CustomerFileReference"].ToString();
                details.DataInputStatusIndicator = myReaderMore["DataInputStatusIndicator"].ToString();
                details.FareCalculationPricingIndicator = myReaderMore["FareCalculationPricingIndicator"].ToString();
                details.FormatIdentifier = myReaderMore["FormatIdentifier"].ToString();
                details.TourCode = myReaderMore["TourCode"].ToString();
                details.NetReportingIndicator = myReaderMore["NetReportingIndicator"].ToString();
                details.NeutralTicketingSystemIdentifier = myReaderMore["NeutralTicketingSystemIdentifier"].ToString();
                details.ReportingSystemIdentifier = myReaderMore["ReportingSystemIdentifier"].ToString();
                details.ServicingAirlineSystemProviderIdentifier = myReaderMore["ServicingAirlineSystemProviderIdentifier"].ToString();
                details.TrueOriginDestinationCityCodes = myReaderMore["TrueOriginDestinationCityCodes"].ToString();
                details.SettlementAuthorizationCode = myReaderMore["SettlementAuthorizationCode"].ToString();
                details.VendorIdentification = myReaderMore["VendorIdentification"].ToString();
                details.VendorISOCountryCode = myReaderMore["VendorISOCountryCode"].ToString();
                details.TransactionCode = myReaderMore["TransactionCode"].ToString();
                details.SalesDataAvailable = myReaderMore["SalesDataAvailable"].ToString();
                details.AccountingStatus = myReaderMore["AccountingStatus"].ToString();
                details.ProratedFlag = myReaderMore["ProratedFlag"].ToString();
                details.FBSFlag = myReaderMore["FBSFlag"].ToString();
                details.DocumentAirlineID = myReaderMore["DocumentAirlineID"].ToString();
                details.TransactionGroup = myReaderMore["TransactionGroup"].ToString();
                details.PaxSPC = myReaderMore["PaxSPC"].ToString();
                details.PaxType = myReaderMore["PaxType"].ToString();
                details.SalesPeriod = myReaderMore["SalesPeriod"].ToString();
                details.PrevMOS = myReaderMore["PrevMOS"].ToString();
                details.TotalTax = myReaderMore["TotalTax"].ToString();
                details.BookingAgentID = myReaderMore["BookingAgentID"].ToString();
                details.TicketingAgentID = myReaderMore["TicketingAgentID"].ToString();
                details.IsReissue = myReaderMore["IsReissue"].ToString();
                details.DataSource = myReaderMore["DataSource"].ToString();
                details.ProcessingFileType = myReaderMore["ProcessingFileType"].ToString();
                details.ProcessingDate = myReaderMore["ProcessingDate"].ToString();
                details.OwnTicket = myReaderMore["OwnTicket"].ToString();
                details.OwnCarrier = myReaderMore["OwnCarrier"].ToString();
                details.OwnISOCountry = myReaderMore["OwnISOCountry"].ToString();
                details.OwnAirline = myReaderMore["OwnAirline"].ToString();
                details.HostCurrency = myReaderMore["HostCurrency"].ToString();
                details.AmountCollected = myReaderMore["AmountCollected"].ToString();
                details.TaxCollected = myReaderMore["TaxCollected"].ToString();
                details.SurchargeCollected = myReaderMore["SurchargeCollected"].ToString();
                details.CommissionCollected = myReaderMore["CommissionCollected"].ToString();
                details.BilateralEndorsement = myReaderMore["BilateralEndorsement"].ToString();
                details.InvoluntaryReroute = myReaderMore["InvoluntaryReroute"].ToString();
                details.BspIdentifier = myReaderMore["BspIdentifier"].ToString();
                details.IsoCountryCode = myReaderMore["IsoCountryCode"].ToString();
                details.USDRatePayCur = myReaderMore["USDRatePayCur"].ToString();
                details.USDRateHostCur = myReaderMore["USDRateHostCur"].ToString();
                details.PMPPeriod = myReaderMore["PMPPeriod"].ToString();
                details.TaxOnCommissionCollected = myReaderMore["TaxOnCommissionCollected"].ToString();
                details.SignCode = myReaderMore["SignCode"].ToString();


                model.Add(details);
            }
            return model;
        }

        public ActionResult KeyControllingData()
        {
            string sql = "select TABLE_NAME, TABLE_SCHEMA,TABLE_TYPE, right(TABLE_NAME,4) as fff  from INFORMATION_SCHEMA.TABLES where TABLE_SCHEMA='ref' and TABLE_TYPE ='BASE TABLE' and (right(TABLE_NAME,4)<> 'test' and right(TABLE_NAME,4)<> 'est2' and  right(TABLE_NAME,3)<> 'old')  order by TABLE_NAME ";
            DataSet ds = new DataSet();
            ds = dbconnect.RetObjDS(sql);

            List<string> header = new List<string>();

            foreach (DataTable dt in ds.Tables)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    header.Add(dr[0].ToString());
                }
            }
            ViewBag.header = header;
            return PartialView();
        }

        public ActionResult getAllData()
        {
            string nameHeader = Request["nameHeader"];

            DataSet ds = new DataSet();
            string query = "SELECT * FROM Ref." + nameHeader;
            ds = dbconnect.RetObjDS(query);
            List<string> colName = new List<string>();
            foreach (DataColumn column in ds.Tables[0].Columns)
            {
                colName.Add(AddSpacesToSentence(column.ColumnName, true));
            }
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                for (int i = 0; i < ds.Tables[0].Columns.Count; i++)
                {
                    dr[ds.Tables[0].Columns[i].ColumnName].ToString();
                }
            }
            ViewBag.colName = colName;
            return PartialView(ds);
        }

        string AddSpacesToSentence(string text, bool preserveAcronyms)
        {
            if (string.IsNullOrWhiteSpace(text))
                return string.Empty;

            String newText = "";
            newText = newText + text[0];
            for (int i = 1; i < text.Length; i++)
            {
                if (char.IsUpper(text[i]))
                    if ((text[i - 1] != ' ' && !char.IsUpper(text[i - 1])) ||
                        (preserveAcronyms && char.IsUpper(text[i - 1]) &&
                         i < text.Length - 1 && !char.IsUpper(text[i + 1])))
                        newText = newText + ' ';
                newText = newText + text[i];
            }
            return newText.ToString();
        }

        public ActionResult Cancellations()
        {
            string dateFrom = DateTime.Now.AddMonths(-1).ToString("dd-MMM-yyyy");
            string dateTo = DateTime.Now.ToString("dd-MMM-yyyy");
            string[] date = new string[2] { dateFrom, dateTo };
            ViewBag.date = date;
            return PartialView();
        }

        public string ConvertDate(string date)
        {
            string mydate = DateTime.ParseExact(date, "dd-MMM-yyyy", CultureInfo.InvariantCulture).ToString("MM-dd-yyyy");
            return mydate;
        }
        public ActionResult LoadCancellations()
        {
            string sql = "";
            string dateFrom1 = Request["dateFrom"];
            string dateFrom = ConvertDate(dateFrom1);
            string dateTo1 = Request["dateTo"];
            string dateTo = ConvertDate(dateTo1);
            string rbcancellation = Request["rbcancellation"];
            string agentCodeSet = Request["agentCodeSet"];
            string j = "";
            int h = 0;
            int p = 0;
            int q = 0;
            int x = 0;


            sql += "SELECT FORMAT(SaleDate, 'dd-MMM-yyyy') as [Sales Date] ,FORMAT(DateofIssue, 'dd-MMM-yyyy') as [Date Of Issue] ,f1.[DocumentNumber]  as [Document No] ,f1.[TransactionCode] as [Transaction Code] ";
            sql += ",AgentNumericCode as [Agent Numeric Code] ,BookingReference as [PNR] ,[BookingAgentID] as [Booking AgentID] ,[TicketingAgentID] as [Ticketing AgentID]  ";
            sql += ",[PassengerName] as [Passenger Name] ,Left(TrueOriginDestinationCityCodes,3) as Origin ,right(TrueOriginDestinationCityCodes,3) as Destination  ";
            sql += ",[TotalCurrency] as [Total Currency] ,[TotalAmount]  as [Total Amount] FROM [Pax].[SalesDocumentHeader] f1 join Pax.SalesRelatedDocumentInformation f2 on f1.HdrGuid = f2.HdrGuid  ";
            sql += "where DateofIssue between '" + dateFrom + "' and '" + dateTo + "'   ";

            if (rbcancellation == "all")
            {
                sql += " and ( f1.TransactionCode like 'CAN%' or ";
                sql += " exists( select * from Pax.SalesDocumentCoupon sdc where sdc.RelatedDocumentGuid = f2.RelatedDocumentGuid and sdc.CouponStatus = 'V' ) ) ";
            }

            if (rbcancellation == "cancel")
            {
                sql += " and ( f1.TransactionCode like 'CAN%' ) ";
            }

            if (rbcancellation == "void")
            {
                sql += " and (  exists( select * from Pax.SalesDocumentCoupon sdc where sdc.RelatedDocumentGuid = f2.RelatedDocumentGuid and sdc.CouponStatus = 'V' ) )";
            }

            if (!string.IsNullOrWhiteSpace(agentCodeSet))
            {
                sql += "and AgentNumericCode = '" + agentCodeSet + "' ";
            }
            DataSet ds = new DataSet();
            ds = dbconnect.RetObjDS(sql);
            List<string> colName = new List<string>();
            foreach (DataColumn column in ds.Tables[0].Columns)
            {
                colName.Add(AddSpacesToSentence(column.ColumnName, true));
            }

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                j = dr[ds.Tables[0].Columns[3].ColumnName].ToString();

                switch (j)
                {
                    case "CANX":
                        h++;
                        break;
                    case "CANN":
                        p++;
                        break;
                    case "CANR":
                        q++;
                        break;
                    default:
                        x++;
                        break;
                }
            }
            int[] total = new int[5] { h, p, q, x, h + p + q + x };
            ViewBag.total = total;
            ViewBag.colName = colName;
            return PartialView(ds);
        }

        public ActionResult CherchByCodeCancellation()
        {
            string sql = "";
            string myvar = Request["value"];
            int n;

            sql = "select top 10 AgentNumericCode, isnull( agtown.TradingName, agt.TradingName ) as [AgentName], count(*) as [CancelCount] from pax.VW_SalesHeader sdh ";
            sql += "left join Ref.Agent_Own agtown	on left(sdh.AgentNumericCode,7) = agtown.AgencyNumericCode ";
            sql += "left join Ref.Agent	agt	on left(sdh.AgentNumericCode,7) = agt.AgencyNumericCode where sdh.TransactionCode in ('CANX', 'CANN', 'CANR') ";

            if (!string.IsNullOrEmpty(myvar))
            {
                bool isNumeric = int.TryParse(myvar, out n);
                if (isNumeric)
                {
                    sql += "and AgentNumericCode like " + "'" + myvar + "%'";
                }
                else
                {
                    sql += "and agt.TradingName like " + "'" + myvar + "%'";
                }
            }

            sql += "Group by AgentNumericCode, isnull( agtown.TradingName, agt.TradingName )";


            SqlDataReader myReader = dbconnect.GetData(sql);

            List<AgentCode> model = new List<AgentCode>();
            while (myReader.Read())
            {
                var details = new AgentCode();
                details.AgentNumericCode = myReader["AgentNumericCode"].ToString();
                details.AgentName = myReader["AgentName"].ToString();
                details.Count = myReader["CancelCount"].ToString();
                model.Add(details);
            }
            return PartialView("CherchByCode", model);

        }

        public ActionResult CherchByCodeCancellationFop()
        {
            string sql = "";
            string myvar = Request["value"];
            string id1 = Request["id1"];
            string id2 = Request["id2"];
            int n;

            sql = "select top 10 AgentNumericCode, isnull( agtown.TradingName, agt.TradingName ) as [AgentName], count(*) as [CancelCount] from pax.VW_SalesHeader sdh ";
            sql += "left join Ref.Agent_Own agtown	on left(sdh.AgentNumericCode,7) = agtown.AgencyNumericCode ";
            sql += "left join Ref.Agent	agt	on left(sdh.AgentNumericCode,7) = agt.AgencyNumericCode where sdh.TransactionCode in ('CANX', 'CANN', 'CANR') ";

            if (!string.IsNullOrEmpty(myvar))
            {
                bool isNumeric = int.TryParse(myvar, out n);
                if (isNumeric)
                {
                    sql += "and AgentNumericCode like " + "'" + myvar + "%'";
                }
                else
                {
                    sql += "and agt.TradingName like " + "'" + myvar + "%'";
                }
            }

            sql += "Group by AgentNumericCode, isnull( agtown.TradingName, agt.TradingName )";


            SqlDataReader myReader = dbconnect.GetData(sql);

            List<AgentCode> model = new List<AgentCode>();
            while (myReader.Read())
            {
                var details = new AgentCode();
                details.AgentNumericCode = myReader["AgentNumericCode"].ToString();
                details.AgentName = myReader["AgentName"].ToString();
                details.Count = myReader["CancelCount"].ToString();
                model.Add(details);
            }
            ViewBag.Agent = model;
            ViewBag.name = id1;
            ViewBag.code = id2;
            return PartialView();

        }
        public ActionResult TransactionType()
        {
            string sql = "";
            string dateFrom = DateTime.Now.AddMonths(-1).ToString("dd-MMM-yyyy");
            string dateTo = DateTime.Now.ToString("dd-MMM-yyyy");
            sql = "SELECT DISTINCT TransactionGroup FROM pax.SalesDocumentHeader";
            sql += " WHERE SaleDate BETWEEN cast('" + ConvertDate(dateFrom) + "' as date) AND cast('" + ConvertDate(dateTo) + "' as date)";

            
            string[] date = new string[2] { dateFrom, dateTo };
            ViewBag.date = date;

            DataSet ds = new DataSet();
            ds = dbconnect.RetObjDS(sql);
            return PartialView(ds);
        }

        public ActionResult LoadTransactionCode()
        {
            string sql = "";
            string dateFrom = Request["dateFrom"];
            //string dateFrom = ConvertDate(dateFrom1);
            string dateTo = Request["dateTo"];
            //string dateTo = ConvertDate(dateTo1);
            sql = "SELECT DISTINCT TransactionGroup FROM pax.SalesDocumentHeader";
            sql += " WHERE SaleDate BETWEEN cast('" + dateFrom + "' as date) AND cast('" + dateTo + "' as date)";

            DataSet ds = new DataSet();
            ds = dbconnect.RetObjDS(sql);
            return PartialView(ds);
        }

        public ActionResult LoadTransactionType()
        {
            string sql = "";
            string dateFrom1 = Request["dateFrom"];
            string dateFrom = ConvertDate(dateFrom1);
            string dateTo1 = Request["dateTo"];
            string dateTo = ConvertDate(dateTo1);
            string transactionCode = Request["transactionCode"];
            sql += "select SalesPeriod, FORMAT(saledate, 'dd-MMM-yyyy') as saledate, AgentNumericCode,isnull( agtown.TradingName, agt.TradingName ) as [Agent Name], ";
            sql += " TransactionGroup, TransactionCode, f1.Currency, AmountCollected,AmountRemitted,ComputedCommission,ItemsCount ";
            sql += "from pAX.SalesDocumentPaymentSummary( null,null, NULL, NULL,NULL, 'TransactionGroup Saledate AgentNumericCode TransactionCode Currency' ) f1";
            sql += " left join Ref.Agent_Own agtown	on left(f1.AgentNumericCode,7) = agtown.AgencyNumericCode ";
            sql += "left join Ref.Agent	agt	on left(f1.AgentNumericCode,7) = agt.AgencyNumericCode ";
            sql += "where saledate between cast('" + dateFrom + "' as date) and cast('" + dateTo + "' as date) ";

            sql += testTransactionCode(transactionCode);
            DataSet ds = new DataSet();
            ds = dbconnect.RetObjDS(sql);
            ViewBag.summary = LoadSummary(dateFrom, dateTo, transactionCode);
            return PartialView(ds);
        }

        public DataSet LoadSummary(string dateFrom, string dateTo, string transactionCode)
        {
            string sql = "select  TransactionGroup, Currency, sum(AmountCollected) as AmtCollected,Sum(AmountRemitted) as AmtRemitted,sum(ComputedCommission) as ComputedComm ,sum(ItemsCount) as Itemscount ";
            sql += "from pAX.SalesDocumentPaymentSummary( null,null, NULL, NULL,NULL, 'TransactionGroup Saledate Currency' ) ";
            sql += "where SaleDate between cast('" + dateFrom + "' as date) and cast('" + dateTo + "'as date) ";
            sql += testTransactionCode(transactionCode);
            sql += " group by TransactionGroup, Currency order by TransactionGroup,Currency";

            DataSet ds = new DataSet();
            ds = dbconnect.RetObjDS(sql);
            return ds;
        }

        private string testTransactionCode(string transactionCode)
        {
            string sql = "";
            if (transactionCode != "All")
            {
                sql += " and TransactionGroup like '" + transactionCode + "' ";
            }
            return sql;
        }

        private DataSet getObservation(string docNum)
        {
            string sql = "select p1.HdrGuid, p1.DocumentNumber,p2.Date,CONVERT(varchar(max), p2.Observation, 0) as Observation,p2.Subject,p2.Time,p2.RecID from Pax.SalesDocumentHeader p1 join Pax.TicketObservation p2 on p1.HdrGuid = p2.Hdrguid where p1.DocumentNumber = '" + docNum + "' ";
            sql += "order by p2.Date,p2.RecID DESC ";

            DataSet ds = new DataSet();
            ds = dbconnect.RetObjDS(sql);
            return ds;
        }

        public ActionResult saveObservation(ObservationModel obs)
        {

            string subject = Request["editSubject"];
            string docNumber = Request["docNumber"];
            string dateObs = Request["dateObs"];
            string obsValue = Request.Unvalidated.Form["obsValue"];
            string recId = Request["recId"];
            string hdrguid = Request["hdrguid"];
            string user = "user";///Request["user"];
            string Sql = "";
            string time = "";
            if (string.IsNullOrEmpty(recId))
            {
                Sql = "DECLARE @MaxRecId bigint ";
                Sql += "set @MaxRecId = (select iif(MAX(RecID) is null, 1, MAX(RecID) + 1) As MaxLineid from Pax.TicketObservation) ";
                Sql += "insert into Pax.TicketObservation([RecID],[Hdrguid],[Date],[Time],[UserID],[Subject],[Observation]) ";
                Sql += "VALUES(@MaxRecId,'" + hdrguid + "','" + dateObs + "','" + time + "','" + user + "','" + subject + "', CONVERT(VARBINARY(100),'" + obsValue + "'))";
            }
            else
            {
                Sql = "Update Pax.TicketObservation  SET [Date]='" + dateObs + "', [Observation] ='" + obsValue + "' ";
                Sql += "[Subject]='" + subject + "' WHERE recId =" + recId + " AND Hdrguid ='" + hdrguid + "' AND [Date]='" + dateObs + "'";

                Sql = "Update Pax.TicketObservation  SET [Date]='" + dateObs + "', [Observation] = CONVERT(VARBINARY(100),'" + obsValue + "'), ";
                Sql += " [Subject] = '" + subject + "' WHERE Hdrguid = '" + hdrguid + "' AND RecID = '" + recId + "' ";
            }
            DataSet ds = new DataSet();
            ds = dbconnect.RetObjDS(Sql);
            return PartialView(getObservation(docNumber));
        }

        public ActionResult TotalAmountPSR()
        {
            /*string dateFrom1 = Request["dateFrom"];
            string dateFrom = ConvertDate(dateFrom1);
            string dateTo1 = Request["dateTo"];
            string dateTo = ConvertDate(dateTo1);
            string agentNumCode = string.IsNullOrEmpty(Request["agentNumCode"]) ? "%" : Request["agentNumCode"];
            string agentName = Request["agentName"];
            string agentLocation = Request["agentLocation"];
            string bookingAgent = string.IsNullOrEmpty(Request["bookingAgent"]) ? "%" : Request["bookingAgent"];
            string reportingEntity = string.IsNullOrEmpty(Request["reportingEntity"]) ? "%" : Request["reportingEntity"];
            string page = "";
            string record = "";

            string sqlPassengerDetail = "select top 100 percent row_number() over ( partition by passengername order by DateofIssue ) as [Travel ID],";
            sqlPassengerDetail += "DateofIssue,passengername, DocumentNumber ,  f1.OriginalIssueDocumentNumber,f1.TransactionCode from pax.SalesDocumentHeader f1";
            sqlPassengerDetail += " Where PassengerName = '" + agentName + "'";
            sqlPassengerDetail +=" order by passengername, DateofIssue ";*/
            string dateFrom = DateTime.Now.AddMonths(-1).ToString("dd-MMM-yyyy");
            string dateTo = DateTime.Now.ToString("dd-MMM-yyyy");
            string[] date = new string[2] { dateFrom, dateTo };
            ViewBag.date = date;
            return PartialView();
        }

        public ActionResult getAgentNumCode()
        {
            string dateFrom1 = Request["dateFrom"];
            string dateFrom = ConvertDate(dateFrom1);
            string dateTo1 = Request["dateTo"];
            string dateTo = ConvertDate(dateTo1);
            string sql = "select distinct AgentNumericCode  from [Pax].[SalesDocumentHeader] where DateofIssue between '" + dateFrom + "'and '" + dateTo + "' and SalesDataAvailable = 1 ";
            DataSet dss = new DataSet();
            dss = dbconnect.RetObjDS(sql);
            return PartialView("LoadTransactionCode", dss);
        }
        public ActionResult LoadTotalAmountPSR()
        {
            string dateFrom1 = Request["dateFrom"];
            string dateFrom = ConvertDate(dateFrom1);
            string dateTo1 = Request["dateTo"];
            string dateTo = ConvertDate(dateTo1);
            string agentNumCode = Request["agentNumCode"] == "All" ? "%" : Request["agentNumCode"];
            string agentName = Request["agentName"];
            string agentLocation = Request["agentLocation"];
            string bookingAgent = Request["bookingAgent"] == "All" ? "%" : Request["bookingAgent"];
            string reportingEntity = Request["reportingEntity"] == "All" ? "%" : Request["reportingEntity"];
            string page = "";
            string record = "";
            DataSet ds = new DataSet();
            DataSet ds1 = new DataSet();
            DataSet ds2 = new DataSet();
            if (!string.IsNullOrWhiteSpace(dateFrom))
            {
                /* psr summary */
                SqlConnection con = new SqlConnection(pbConnectionString);
                con.Open();
                SqlCommand cmd = new SqlCommand("[Pax].[SP_SlashPSR]", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 1000;
                cmd.Parameters.AddWithValue("@IssueDate_from", dateFrom);
                cmd.Parameters.AddWithValue("@IssueDate_to", dateTo);
                cmd.Parameters.AddWithValue("@AgentNumericCode", agentNumCode);
                cmd.Parameters.AddWithValue("@BookingAgentIdentification", bookingAgent);
                cmd.Parameters.AddWithValue("@BSP", reportingEntity);
                var ada = new SqlDataAdapter(cmd);
                //DataSet ds = new DataSet();
                ada.Fill(ds);
                con.Close();
                ViewBag.summary = ds;
                /* office total */
                con.Open();
                SqlCommand cmd1 = new SqlCommand("[Pax].[SP_PSR_OfficeTotal]", con);
                cmd1.CommandType = CommandType.StoredProcedure;
                cmd1.CommandTimeout = 1000;
                cmd1.Parameters.AddWithValue("@IssueDate_from", dateFrom);
                cmd1.Parameters.AddWithValue("@IssueDate_to", dateTo);
                cmd1.Parameters.AddWithValue("@AgentNumericCode", agentNumCode);
                cmd1.Parameters.AddWithValue("@BookingAgentIdentification", bookingAgent);
                cmd1.Parameters.AddWithValue("@BSP", reportingEntity);

                var ada1 = new SqlDataAdapter(cmd1);
                ada1.Fill(ds1);
                con.Close();
                ViewBag.officeTotal = ds1;
                /* transactin code summaried */
                con.Open();
                SqlCommand cmd2 = new SqlCommand("[Pax].[SP_SlashPSR_TranxCode_Summary]", con);
                cmd2.CommandType = CommandType.StoredProcedure;
                cmd2.CommandTimeout = 1000;
                cmd2.Parameters.AddWithValue("@IssueDate_from", dateFrom);
                cmd2.Parameters.AddWithValue("@IssueDate_to", dateTo);
                cmd2.Parameters.AddWithValue("@AgentNumericCode", agentNumCode);
                cmd2.Parameters.AddWithValue("@BookingAgentIdentification", bookingAgent);
                cmd2.Parameters.AddWithValue("@BSP", reportingEntity);

                var ada2 = new SqlDataAdapter(cmd2);
                ada2.Fill(ds2);
                con.Close();
                ViewBag.transSummaried = ds2;
            }
            return PartialView();
        }

        public ActionResult KeyElement()
        {
            string dateFrom1 = Request["dateFrom"];
            string dateFrom = ConvertDate(dateFrom1);
            string dateTo1 = Request["dateTo"];
            string dateTo = ConvertDate(dateTo1);
            string agentNumCode = Request["agentNumCode"] == "All" ? "%" : Request["agentNumCode"];
            string bookingAgent = Request["bookingAgent"] == "All" ? "%" : Request["bookingAgent"];
            string reportingEntity = Request["reportingEntity"] == "All" ? "%" : Request["reportingEntity"];
            string sql = "";
            string page = "1";
            string record = "150";
            DataSet ds = new DataSet();
            if (!string.IsNullOrEmpty(dateFrom))
            {
                SqlConnection con = new SqlConnection(pbConnectionString);
                con.Open();
                SqlCommand cmd = new SqlCommand("[Pax].[SP_PSR_Key_Elements]", con);
                cmd.CommandTimeout = 1000;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IssueDate_from", dateFrom);
                cmd.Parameters.AddWithValue("@IssueDate_to", dateTo);
                cmd.Parameters.AddWithValue("@AgentNumericCode", agentNumCode);
                var ada = new SqlDataAdapter(cmd);
                ada.Fill(ds);
                con.Close();
                ViewBag.keyElement = ds;
                /* key summary */
                sql = "DECLARE @IssueDate_from date = '" + dateFrom + "'; DECLARE @IssueDate_to date = '" + dateTo + "'; ";
                sql += " DECLARE @AgentNumericCode nvarchar (20) = '" + agentNumCode + "'; DECLARE @BookingAgentIdentification nvarchar (20) = '" + bookingAgent + "'; ";
                sql += " DECLARE @BSP nvarchar (12) = '" + reportingEntity + "'; DECLARE @PageNo int = '" + page + "'; DECLARE @RecordsPerPage int = '" + record + "'; ";
                sql += " with A as ( select f1.AgentNumericCode as AgentNumericCode ,isnull( f3.RelatedDocumentNumber, f1.DocumentNumber ) as TicketNo  ";
                sql += " ,Pax.fn_FOP_Combined( f1.HdrGuid ) as FOP ,isnull( f1.FareCurrency, '' ) as FareCurrency , f1.Fare as Fare  ";
                sql += " , isnull( f1.TotalCurrency, '' ) as EquivalentFareCurrency ,f1.EquivalentFare as EquivalentFare , f1.TransactionCode ";
                sql += " from Pax.SalesDocumentHeader f1 join Pax.SalesRelatedDocumentInformation f2  ";
                sql += " on f1.HdrGuid = f2.HdrGuid and f1.AgentNumericCode like @AgentNumericCode and  f1.SalesDataAvailable = 1 ";
                sql += " and f1.DateofIssue between @IssueDate_from and @IssueDate_to  ";
                sql += " left join Pax.SalesDocumentCoupon f3 on f2.RelatedDocumentGuid = f3.RelatedDocumentGuid  ";
                sql += " left join ref.VW_Agent f10 on left( f1.AgentNumericCode, 7 ) = f10.AgencyNumericCode )  ";
                sql += " select AgentNumericCode,TransactionCode ,FOP,FareCurrency,sum(Fare) as Fare ,EquivalentFareCurrency,sum(EquivalentFare) as [Equivalent Number],count(distinct TicketNo) as [Count] from A   ";
                sql += " group by AgentNumericCode,TransactionCode ,FOP,FareCurrency,EquivalentFareCurrency  ";

                DataSet ds1 = new DataSet();
                ds1 = dbconnect.RetObjDS(sql);
                ViewBag.keySummary = ds1;
            }
            return PartialView();
        }

        public ActionResult FareBasisAnalytics()
        {
            string dateFrom1 = Request["dateFrom"];
            string dateFrom = ConvertDate(dateFrom1);
            string dateTo1 = Request["dateTo"];
            string dateTo = ConvertDate(dateTo1);
            string agentNumCode = Request["agentNumCode"] == "All" ? "%" : Request["agentNumCode"];
            string bookingAgent = Request["bookingAgent"] == "All" ? "%" : Request["bookingAgent"];
            string reportingEntity = Request["reportingEntity"] == "All" ? "%" : Request["reportingEntity"];
            string selection = Request["selection"];
            switch (selection)
            {
                case "Fare Component":
                    selection = "1";
                    break;
                case "Journey":
                    selection = "2";
                    break;
                case "Sector":
                    selection = "3";
                    break;
            }
            string sql = "";
            DataSet ds = new DataSet();
            DataSet ds1 = new DataSet();
            if (!string.IsNullOrEmpty(dateFrom))
            {
                SqlConnection con = new SqlConnection(pbConnectionString);
                con.Open();
                SqlCommand cmd = new SqlCommand("[Pax].[SP_PSR_Orin_Dest_FareBasis_Analytics]", con);

                cmd.CommandTimeout = 1000;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IssueDate_from", dateFrom);
                cmd.Parameters.AddWithValue("@IssueDate_to", dateTo);
                cmd.Parameters.AddWithValue("@AgentNumericCode", agentNumCode);
                cmd.Parameters.AddWithValue("@selection", selection);
                var ada = new SqlDataAdapter(cmd);
                ada.Fill(ds);
                con.Close();
                ViewBag.fareBasisAnalytics = ds;
                /* summary */
                sql = " declare @IssueDate_from date = '" + dateFrom + "'; declare @IssueDate_to date ='" + dateTo + "';   " + Environment.NewLine;
                sql += " declare @AgentNumericCode nvarchar (20) = '" + agentNumCode + "'; declare @selection int = '" + selection + "'; " + Environment.NewLine;
                sql += " IF OBJECT_ID('tempdb..#base') IS NOT NULL DROP TABLE #base  " + Environment.NewLine;
                sql += " select  f1.DateofIssue, isnull( f3.RelatedDocumentNumber, f1.DocumentNumber ) as TicketNo  " + Environment.NewLine;
                sql += " , f1.TrueOriginDestinationCityCodes, f1.AgentNumericCode  " + Environment.NewLine;
                sql += " , isnull( f10.LocationCity, '' )  as POS, f3.FareBasisTicketDesignator as FareBasis  " + Environment.NewLine;
                sql += " , iif( isnull(f4.FareComponent,0) = 0, 1, FareComponent ) as FareComponent  " + Environment.NewLine;
                sql += " , f3.CouponNumber + ( cast(f3.RelatedDocumentNumber as bigint) -  cast( f1.DocumentNumber as bigint ) ) * 4  " + Environment.NewLine;
                sql += " as CouponNumber, f3.OriginAirportCityCode, f3.DestinationAirportCityCode, f1.tourcode  " + Environment.NewLine;
                sql += " , isnull( f1.FareCurrency, '' ) as FareCurrency, f1.Fare, isnull( f1.TotalCurrency, '' ) as EquivalentFareCurrency  " + Environment.NewLine;
                sql += " , f1.EquivalentFare as EFP, f1.Tax1Code, f1.Tax1Amount, f1.Tax2Code, f1.Tax2Amount, f1.Tax3Code  " + Environment.NewLine;
                sql += " , f1.Tax3Amount, f1.SurchargeCollectedCurrency, f1.SurchargeCollected, f3.ReservationBookingDesignator  " + Environment.NewLine;
                sql += " , NotValidBefore as NVB, NotValidAfter as NVA, FreeBaggageAllowance as FBA, f1.TransactionCode,f1.FareCalculationArea  " + Environment.NewLine;
                sql += " into #base from Pax.SalesDocumentHeader f1  " + Environment.NewLine;
                sql += " join Pax.SalesRelatedDocumentInformation f2 on f1.HdrGuid = f2.HdrGuid and f1.DateofIssue between @IssueDate_from and @IssueDate_to and ( f1.AgentNumericCode like @AgentNumericCode )  " + Environment.NewLine;
                sql += " join Pax.SalesDocumentCoupon f3 on f2.RelatedDocumentGuid = f3.RelatedDocumentGuid  " + Environment.NewLine;
                sql += " left join Pax.ProrationDetail f4 on f3.RelatedDocumentGuid = f4.RelatedDocumentGuid and f3.CouponNumber = f4.CouponNumber and f3.CouponStatus = f4.ProrationFlag  " + Environment.NewLine;
                sql += " left join ref.VW_Agent f10 on left( f1.AgentNumericCode, 7 ) = f10.AgencyNumericCode;  " + Environment.NewLine;
                sql += " IF @selection = 1  " + Environment.NewLine;
                sql += " select AgentNumericCode,TransactionCode,count(distinct TicketNo) as [Count],TrueOriginDestinationCityCodes  " + Environment.NewLine;
                sql += " ,OriginAirportCityCode,DestinationAirportCityCode, FareCurrency,sum( Fare) as Fare,EquivalentFareCurrency,Sum(EFP) as EFP  " + Environment.NewLine;
                sql += " from #base where (fare<>'0.00' OR efp<>'0.00') group by  OriginAirportCityCode,DestinationAirportCityCode,FareCurrency,EquivalentFareCurrency,TransactionCode,AgentNumericCode,TrueOriginDestinationCityCodes  " + Environment.NewLine;
                sql += " IF @selection = 2  " + Environment.NewLine;
                sql += " select AgentNumericCode,TransactionCode,Count(distinct TicketNo) as [COUNT],TrueOriginDestinationCityCodes,f2.OriginAirportCityCode,f3.DestinationAirportCityCode  " + Environment.NewLine;
                sql += " ,FareCurrency,sum(Fare) as Fare,EquivalentFareCurrency,sum( EFP) as EFP  " + Environment.NewLine;
                sql += " from #base f1 cross apply (select top 1 OriginAirportCityCode from #base O where f1.TicketNo = O.TicketNo and f1.FareComponent = O.FareComponent order by CouponNumber ASC) f2   " + Environment.NewLine;
                sql += " cross apply (select top 1 DestinationAirportCityCode from #base O where f1.TicketNo = O.TicketNo and f1.FareComponent = O.FareComponent order by CouponNumber DESC) f3 where (fare<>'0.00' OR efp<>'0.00')  " + Environment.NewLine;
                sql += " group by AgentNumericCode, TransactionCode, f2.OriginAirportCityCode, f3.DestinationAirportCityCode, FareCurrency, EquivalentFareCurrency,TrueOriginDestinationCityCodes  " + Environment.NewLine;
                sql += " IF @selection = 3  " + Environment.NewLine;
                sql += " select AgentNumericCode, TransactionCode,count(distinct ticketno) as [Count],TrueOriginDestinationCityCodes,OriginAirportCityCode,DestinationAirportCityCode,FareCurrency,sum( Fare) as Fare  " + Environment.NewLine;
                sql += " ,EquivalentFareCurrency,sum( EFP) as EFP from #base where (fare<>'0.00' OR efp<>'0.00')  " + Environment.NewLine;
                sql += " group by AgentNumericCode, TrueOriginDestinationCityCodes,FareCurrency,EquivalentFareCurrency,TransactionCode,OriginAirportCityCode,DestinationAirportCityCode  " + Environment.NewLine;

                ds1 = dbconnect.RetObjDS(sql);
                ViewBag.summary = ds1;
            }
            return PartialView();
        }

        public ActionResult PsrPassenger()
        {
            string dateFrom1 = Request["dateFrom"];
            string dateFrom = ConvertDate(dateFrom1);
            string dateTo1 = Request["dateTo"];
            string dateTo = ConvertDate(dateTo1);
            string agentNumCode = Request["agentNumCode"] == "All" ? "%" : Request["agentNumCode"];
            DataSet ds = new DataSet();
            if (!string.IsNullOrEmpty(dateFrom))
            {
                SqlConnection con = new SqlConnection(pbConnectionString);
                con.Open();
                SqlCommand cmd = new SqlCommand("[Pax].[SP_PSR_Passenger_Management]", con);
                cmd.CommandTimeout = 1000;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IssueDate_from", dateFrom);
                cmd.Parameters.AddWithValue("@IssueDate_to", dateTo);
                cmd.Parameters.AddWithValue("@AgentNumericCode", agentNumCode);
                var ada = new SqlDataAdapter(cmd);
                ada.Fill(ds);
                con.Close();
                ViewBag.psrPassenger = ds;
            }
            return PartialView();
        }

        public ActionResult PassengerDetail()
        {
            string pax = Request["name"];
            string sql = "select top 100 percent row_number() over ( partition by passengername order by DateofIssue ) as [Travel ID],";
            sql = sql + "DateofIssue,passengername, DocumentNumber ,  f1.OriginalIssueDocumentNumber,f1.TransactionCode from pax.SalesDocumentHeader f1";
            sql = sql + " Where PassengerName = '" + pax + "'";
            sql = sql + " order by passengername, DateofIssue ";
            DataSet ds = new DataSet();
            ds = dbconnect.RetObjDS(sql);
            return PartialView(ds);
        }
        public ActionResult GetBookingAgent()
        {
            string dateFrom = Request["dateFrom"];
            string dateTo = Request["dateTo"];
            string agentNumCode = Request["agentNumCode"];
            string sql = "select distinct BookingAgentIdentification from[Pax].[SalesDocumentHeader] ";
            sql += "where AgentNumericCode = '" + agentNumCode + "' AND DateofIssue between '" + dateFrom + "'and '" + dateTo + "' ";
            DataSet ds = new DataSet();
            ds = dbconnect.RetObjDS(sql);
            return PartialView("LoadTransactionCode", ds);
        }

        public ActionResult GetReportingEntity()
        {
            string dateFrom1 = Request["dateFrom"];
            string dateFrom = ConvertDate(dateFrom1);
            string dateTo1 = Request["dateTo"];
            string dateTo = ConvertDate(dateTo1);
            string sql = "select distinct DataSource from [Pax].[SalesDocumentHeader] where DateofIssue between '" + dateFrom + "'and '" + dateTo + "' and SalesDataAvailable = 1 ";
            DataSet ds = new DataSet();
            ds = dbconnect.RetObjDS(sql);
            return PartialView("LoadTransactionCode", ds);
        }

        public ActionResult UnusedDocsCpns()
        {
            string dateFrom = DateTime.Now.AddMonths(-1).ToString("dd-MMM-yyyy");
            string dateTo = DateTime.Now.ToString("dd-MMM-yyyy");
            string[] date = new string[2] { dateFrom, dateTo };
            ViewBag.date = date;
            return PartialView();
        }

        public ActionResult LoadUnusedDocsCpns()
        {
            string dateFrom1 = Request["dateFrom"];
            string dateFrom = ConvertDate(dateFrom1);
            string dateTo1 = Request["dateTo"];
            string dateTo = ConvertDate(dateTo1);
            string sql = "";
            sql += "select f1.DocumentNumber , f1.CouponNumber , FORMAT(f1.SaleDate,'dd-MMM-yyyy') as SaleDate, FORMAT(f1.FlightDepartureDate,'dd-MMM-yyyy') as FlightDepartureDate, f1.FareBasisTicketDesignator, FORMAT(f1.NotValidAfter,'dd-MMM-yyyy') as NotValidAfter ";
            sql += ",f1.OriginAirportCityCode, f1.DestinationAirportCityCode , isnull(f1.FinalShare,'0.00') as [FinalShare(USD)] ";
            sql += ", case when f1.NotValidAfter < DATEADD(d,-1  ,SYSDATETIME()) then 'Subject to refund & Check rules.' else '' end as SubjectToRefund ";
            sql += "from Pax.fn_SalesCouponDetail(null) f1 join Pax.VW_SalesHeader f3 on f1.HdrGuid = f3.HdrGuid ";
            sql += "where f1.UsageDate is null and f1.FlightDepartureDate between '" + dateFrom + "' and '" + dateTo + "' and f3.TransactionGroup = 'ISSUES' ";
            DataSet ds = new DataSet();
            ds = dbconnect.RetObjDS(sql);
            return PartialView(ds);
        }

        public ActionResult FormOfPayementType(string type)
        {
            string dateFrom = DateTime.Now.AddMonths(-1).ToString("dd-MMM-yyyy");
            string dateTo = DateTime.Now.ToString("dd-MMM-yyyy");
            string[] date = new string[2] { dateFrom, dateTo };
            ViewBag.date = date;
            ViewBag.typeId = type;
            return PartialView();
        }
        public ActionResult LoadFormOfPayementType()
        {
            string dateFrom = Request["dateFrom"];
            //string dateFrom = ConvertDate(dateFrom1);
            string dateTo = Request["dateTo"];
            //string dateTo = ConvertDate(dateTo1);
            string agentNumCode = Request["agentNumCode"];
            string fop = Request["fop"];
            string sql = "";
            if (fop == "All")
                sql = "f3.FormofPaymentType like '%'" + Environment.NewLine;
            else if (fop.Length == 2)
                sql = "f3.FormofPaymentType = '" + fop + "'" + Environment.NewLine;
            else
                sql = "f3.FormofPaymentType like '" + fop + "%'" + Environment.NewLine;
            string sql1 = "select FORMAT(f1.SaleDate,'dd-MMM-yyyy') as SaleDate, f1.DocumentNumber, f1.AgentNumericCode, f1.FareCalculationModeIndicator, f1.TransactionCode" + Environment.NewLine;
            sql1 += ",f3.FormofPaymentType as FOP, f3.Currency, f3.Amount, f3.RemittanceAmount, f1.CommissionCollected" + Environment.NewLine;
            sql1 += "from pax.SalesDocumentHeader f1" + Environment.NewLine;
            sql1 += "join pax.SalesRelatedDocumentInformation f2 on f1.HdrGuid = f2.HdrGuid" + Environment.NewLine;
            sql1 += "join pax.SalesDocumentPayment f3 on f2.RelatedDocumentGuid = f3.RelatedDocumentGuid" + Environment.NewLine;
            sql1 += "where f3.Amount <> 0 " + Environment.NewLine;
            sql1 += "and f1.SaleDate between cast('" + dateFrom + "' as date) and cast('" + dateTo + "' as date)" + Environment.NewLine;
            sql1 += "and f1.AgentNumericCode like '" + agentNumCode + "%' and " + sql + " order by SaleDate,fop, Currency";
            DataSet ds = new DataSet();
            ds = dbconnect.RetObjDS(sql1);
            ViewBag.form1 = ds;

            /* form 2 ou summary */
            if (fop == "All")
                fop = "%";
            string sqlSumm = "select f1.AgentNumericCode,  f1.TransactionCode ,f3.FormofPaymentType as FOP, f3.Currency";
            sqlSumm += ",sum(f3.Amount) as AmountCollected, sum(f3.RemittanceAmount) as AmountRemitted";
            sqlSumm += ", sum(f1.CommissionCollected) as CommissionCollected, count(f1.documentnumber) as DocCount";
            sqlSumm += " from pax.SalesDocumentHeader f1 join pax.SalesRelatedDocumentInformation f2 on f1.HdrGuid = f2.HdrGuid";
            sqlSumm += " join pax.SalesDocumentPayment f3 on f2.RelatedDocumentGuid = f3.RelatedDocumentGuid where f3.Amount <> 0 ";
            sqlSumm += " and f1.SaleDate between cast('" + dateFrom + "' as date) and cast('" + dateTo + "' as date)";
            sqlSumm += " and f1.AgentNumericCode like '" + agentNumCode + "%' and f3.FormofPaymentType like '" + fop + "'";
            sqlSumm += " group by f1.AgentNumericCode,f1.TransactionCode, f3.FormofPaymentType,f3.Currency ";
            sqlSumm += " order by f1.AgentNumericCode,f1.TransactionCode,FOP, Currency";
            DataSet ds1 = new DataSet();
            ds1 = dbconnect.RetObjDS(sqlSumm);
            ViewBag.form2 = ds1;
            return PartialView();
        }
        public ActionResult FopTypeTransaction(string type)
        {
            string dateFrom = Request["dateFrom"];
            //string dateFrom = ConvertDate(dateFrom1);
            string dateTo = Request["dateTo"];
            //string dateTo = ConvertDate(dateTo1);
            string agentNumCode = Request["agentNumCode"];
            string ag;
            if (agentNumCode == "")
            {
                ag = "%";
            }
            else
            {
                ag = agentNumCode;
            }
            string sql = " select distinct iif(len(f3.FormofPaymentType)> 4, substring(f3.FormofPaymentType,1,4), f3.FormofPaymentType) from pax.SalesDocumentPayment f3" + Environment.NewLine;
            sql += "join pax.SalesRelatedDocumentInformation f2 on f3.RelatedDocumentGuid = f2.RelatedDocumentGuid" + Environment.NewLine;
            sql += "join pax.SalesDocumentHeader f1 on f1.HdrGuid = f2.HdrGuid" + Environment.NewLine;
            sql += "where Amount <>0 and f1.SaleDate between cast('" + dateFrom + "' as date) and" + Environment.NewLine;
            sql += "cast('" + dateTo + "' as date) and f1.AgentNumericCode like '" + ag + "'";
            DataSet ds = new DataSet();
            ds = dbconnect.RetObjDS(sql);
            ViewBag.typeId = type;
            return PartialView("LoadTransactionCode", ds);
        }
        public ActionResult FOPOthers(string type)
        {
            string dateFrom = DateTime.Now.AddMonths(-1).ToString("dd-MMM-yyyy");
            string dateTo = DateTime.Now.ToString("dd-MMM-yyyy");
            string[] date = new string[2] { dateFrom, dateTo };
            ViewBag.date = date;
            ViewBag.typeId = type;
            return PartialView();
        }
        public ActionResult FopTypeTransaction0()
        {
            string dateFrom = Request["dateFrom"];

            //string dateFrom = ConvertDate(dateFrom1);
            string dateTo = Request["dateTo"];
            //string dateTo = ConvertDate(dateTo1);
          
            string sql = " select distinct iif(len(f3.FormofPaymentType)> 4, substring(f3.FormofPaymentType,1,4), f3.FormofPaymentType) from pax.SalesDocumentPayment f3" + Environment.NewLine;
            sql += "join pax.SalesRelatedDocumentInformation f2 on f3.RelatedDocumentGuid = f2.RelatedDocumentGuid" + Environment.NewLine;
            sql += "join pax.SalesDocumentHeader f1 on f1.HdrGuid = f2.HdrGuid" + Environment.NewLine;
            sql += "where Amount <>0 and f1.SaleDate between cast('" + dateFrom + "' as date) and" + Environment.NewLine;
            sql += "cast('" + dateTo + "' as date)  and f1.AgentNumericCode like '%'";
            DataSet ds = new DataSet();
            ds = dbconnect.RetObjDS(sql);
            return PartialView("LoadTransactionCode", ds);
        }
        public ActionResult FopTypeTransactionOther(string type)
        {
            ViewBag.typeId = type;
            string dateFrom = Request["dateFrom"];
            //string dateFrom = ConvertDate(dateFrom1);
            string dateTo = Request["dateTo"];
            //string dateTo = ConvertDate(dateTo1);
            string agentNumCode = Request["agentNumCode"];
            string ag;
            if (agentNumCode == "")
            {
                ag = "%";
            }
            else
            {
                ag = agentNumCode;
            }
            string sql = " select distinct iif(len(f3.FormofPaymentType)> 4, substring(f3.FormofPaymentType,1,4), f3.FormofPaymentType) from pax.SalesDocumentPayment f3" + Environment.NewLine;
            sql += "join pax.SalesRelatedDocumentInformation f2 on f3.RelatedDocumentGuid = f2.RelatedDocumentGuid" + Environment.NewLine;
            sql += "join pax.SalesDocumentHeader f1 on f1.HdrGuid = f2.HdrGuid" + Environment.NewLine;
            sql += "where Amount <>0 and f1.SaleDate between cast('" + dateFrom + "' as date) and" + Environment.NewLine;
            sql += "cast('" + dateTo + "' as date) and f1.AgentNumericCode like '" + ag + "'";
            SqlConnection con = new SqlConnection(pbConnectionString);
            DataSet ds = new DataSet();
            SqlDataAdapter ada = new SqlDataAdapter(sql, con);
            ada.Fill(ds);
            //ds = dbconnect.RetObjDS(sql);
            int lon = ds.Tables[0].Rows.Count;
            string[] flnum = new string[lon];
            int j = 0;
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                flnum[j] = dr[ds.Tables[0].Columns[0].ColumnName].ToString();
                j = j + 1;
            }
            ViewBag.Flnum = flnum;
            ViewBag.lon = lon;
            return PartialView();

        }

        public ActionResult Exchanges(string type)
        {
            string dateFrom = DateTime.Now.AddMonths(-1).ToString("dd-MMM-yyyy");
            string dateTo = DateTime.Now.ToString("dd-MMM-yyyy");
            string[] date = new string[2] { dateFrom, dateTo };
            ViewBag.date = date;
            ViewBag.disabled = "";
            ViewBag.all = "checked";
            ViewBag.own = "";
            ViewBag.oal = "";
            ViewBag.typeId = type;
            return PartialView();
        }
        public ActionResult ExchangesOAL(string type)
        {
            string dateFrom = DateTime.Now.AddMonths(-1).ToString("dd-MMM-yyyy");
            string dateTo = DateTime.Now.ToString("dd-MMM-yyyy");
            string[] date = new string[2] { dateFrom, dateTo };
            ViewBag.date = date;
            ViewBag.disabled = "disabled";
            ViewBag.all = "";
            ViewBag.own = "";
            ViewBag.oal = "checked";
            ViewBag.typeId = type;
            return PartialView("Exchanges");
        }
        public ActionResult ExchangesOWN(string type)
        {
            string dateFrom = DateTime.Now.AddMonths(-1).ToString("dd-MMM-yyyy");
            string dateTo = DateTime.Now.ToString("dd-MMM-yyyy");
            string[] date = new string[2] { dateFrom, dateTo };
            ViewBag.date = date;
            ViewBag.disabled = "disabled";
            ViewBag.all = "";
            ViewBag.own = "checked";
            ViewBag.oal = "";
            ViewBag.typeId = type;
            return PartialView("Exchanges");
        }

        public ActionResult LoadExchanges()
        {
            string name = Request["name"];
            string starting = Request["starting"];
            string docNum = Request["docNum"];
            string agentCode = Request["agentCode"];
            string dateFrom = ConvertDate(Request["dateFrom"]);
            string dateTo = ConvertDate(Request["dateTo"]);
            string document = Request["document"];

            string voluntary = Request["voluntary"];
            string involuntary = Request["involuntary"];
            string mco = Request["mco"];
            string emd = Request["emd"];
            string ebt = Request["ebt"];
            string mpd = Request["mpd"];
            string et = Request["et"];
            string refine = Request["refine"];
            string sql = "";
            string Own = "";
            string vol = "";
            string tictype = "";

            if (document == "All")
            {
                Own = " [OwnTicket] LIKE '%'";
            }
            else if (document == "Own")
            {
                Own = " [OwnTicket] = 'Y'";
            }
            else if (document == "Oal")
            {
                Own = " [OwnTicket] = 'N'";
            }

            sql = "SELECT Format([NewDateofIssue],'dd-MMM-yyyy') as NewDateofIssue,[NewAgentNumericCode],[NewDocumentNumber],[PassengerName],[NewTransactionCode],[NewDocumentType],[OriginalIssueDocumentNumber],[PrecedingDocument] ,Format([PrecedingDateofIssue],'dd-MMM-yyyy') as PrecedingDateofIssue,[PrecedingDocumentType], BILLINGPERIOD, INVOICENO FROM [Pax].[VW_ExchangeDocument] where " + Own + " ";
            if (!string.IsNullOrEmpty(Request["checkDateTo"]) && !string.IsNullOrEmpty(Request["checkDateFrom"]) && !string.IsNullOrEmpty(dateFrom) && !string.IsNullOrEmpty(dateTo))
            {
                sql = sql + " AND NewDateofIssue between cast('" + dateFrom + "'as date)  and cast('" + dateTo + "' as date)";
            }

            if (!string.IsNullOrEmpty(name))
            {
                string strPassengerName = string.Empty;
                if (starting == "starting")
                {
                    strPassengerName = " AND PassengerName LIKE '%" + name + "%'";
                }
                else if (starting == "contains")
                {
                    strPassengerName = " AND PassengerName LIKE '" + name + "%'";
                }
                sql = sql + strPassengerName;
            }

            if (!string.IsNullOrEmpty(docNum) && (docNum.Length == 13))
            {
                sql = sql + " AND NewDocumentNumber = '" + docNum + "'";
            }
            if (!string.IsNullOrEmpty(agentCode))
            {
                sql = sql + " AND NewAgentNumericCode = '" + agentCode + "'";
            }

            if (!string.IsNullOrEmpty(refine))
            {
                if (voluntary == "voluntary" && involuntary == "involuntary") { vol = " AND [InvoluntaryReroute] like '%'"; }
                else if (voluntary == "voluntary") { vol = " AND [InvoluntaryReroute] = 'Y'"; }
                else if (involuntary == "involuntary") { vol = " AND [InvoluntaryReroute] = 'N'"; }
                if (mco == "mco") { tictype = tictype + " AND ([NewDocumentType] = 'MCO'"; }
                else if (emd == "emd") { tictype = tictype + " AND ([NewDocumentType] = 'EMD'"; }
                else if (ebt == "ebt") { tictype = tictype + " AND ([NewDocumentType] = 'EBT'"; }
                else if (mpd == "mpd") { tictype = tictype + " AND ([NewDocumentType] = 'VMPD'"; }
                else if (et == "et") { tictype = tictype + " AND ([NewDocumentType] = 'ET'"; }
                if (mco == "mco") { tictype = tictype + " or [NewDocumentType] = 'MCO'"; }
                if (emd == "emd") { tictype = tictype + " or [NewDocumentType] = 'EMD'"; }
                if (ebt == "ebt") { tictype = tictype + " or [NewDocumentType] = 'EBT'"; }
                if (mpd == "mpd") { tictype = tictype + " or [NewDocumentType] = 'VMPD'"; }
                if (et == "et") { tictype = tictype + " or [NewDocumentType] = 'ET'"; }
                sql = sql + vol + tictype + ")";
            }
            DataSet ds = new DataSet();
            ds = dbconnect.RetObjDS(sql);
            return PartialView(ds);
        }

        public ActionResult ExchangeTicketDetails()
        {
            string DocNum = Request["docNum"]; // new
            string preDocNum = Request["preDocNum"]; // old
            string sql = "";
            string differentFare = "";
            string differentFD = "";
            string originDoc = "";
            string newDoc = "";
            string originUsd = "";
            string newUsd = "";
            string labelText = "";
            string txtNetBilling = "";
            string newdoi = "";
            string Orgdoi = "";
            sql += "SELECT[NewDocumentNumber],[NewTransactionCode],[NewDocumentType] ,Format([NewDateofIssue],'dd-MMM-yyyy') ,[NewAgentNumericCode] ,";
            sql += "[NewFareCalculationArea],[EMD_MCO_DocumentNumber],[EMD_MCO_DocumentType],[EMD_MCO_TransactionCode] ,[EMD_MCO_FareCurrency],[EMD_MCO_Fare],[OriginalIssueDocumentNumber] ,";
            sql += "[PrecedingDocument],Format([PrecedingDateofIssue],'dd-MMM-yyyy') ,[PrecedingDocumentType] ,[PrecedingFareCalculationArea] FROM [Pax].[VW_ExchangeDocument] ";
            sql += " where NewDocumentNumber = '" + DocNum + "'  AND PrecedingDocument = '" + preDocNum + "'";

            DataSet ds = new DataSet();
            ds = dbconnect.RetObjDS(sql);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                originDoc = dr[ds.Tables[0].Columns[11].ColumnName].ToString();
                newDoc = dr[ds.Tables[0].Columns[0].ColumnName].ToString();
                if (string.IsNullOrEmpty(originDoc))
                {
                    originDoc = dr[ds.Tables[0].Columns[12].ColumnName].ToString();
                }
                newdoi = dr[ds.Tables[0].Columns[3].ColumnName].ToString();
            }

            //ViewBag.originDetail 
            var origine = OriginalDetails(originDoc);
            string oldFareCurr = "";
            string oldFare = "";
            foreach (DataRow dr in origine.Tables[0].Rows)
            {
                oldFareCurr = dr[origine.Tables[0].Columns[0].ColumnName].ToString();
                oldFare = dr[origine.Tables[0].Columns[1].ColumnName].ToString();
                Orgdoi = dr[origine.Tables[0].Columns[6].ColumnName].ToString();
            }
            if (!string.IsNullOrEmpty(oldFareCurr))
            {
                originUsd = val(oldFareCurr, oldFare, Orgdoi);
            }

            //ViewBag.newDetail = 
            var news = OriginalDetails(newDoc);
            string newFareCurr = "";
            string newFare = "";
            string dateFare = "";
            foreach (DataRow dr in news.Tables[0].Rows)
            {
                newFareCurr = dr[news.Tables[0].Columns[0].ColumnName].ToString();
                newFare = dr[news.Tables[0].Columns[1].ColumnName].ToString();
                dateFare = dr[news.Tables[0].Columns[6].ColumnName].ToString();
            }
            if (!string.IsNullOrEmpty(newFareCurr))
            {
                newUsd = val(newFareCurr, newFare, dateFare);
            }


            if (newFareCurr == oldFareCurr)
            {
                differentFare = fareDiff(newFare, oldFare);
            }
            else
            {
                differentFare = Convert.ToDouble(fareDiff(newUsd, originUsd)).ToString("#.##");
            }

            double b = Convert.ToDouble(fareDiff(newUsd, originUsd));
            if (b > 0)
            {
                labelText = "ADC";
            }
            else
                if (b < 0)
            {
                labelText = "REFUND";
            }
            else
                    if (b == 0)
            {
                labelText = "NO ADC";
            }

            if (string.IsNullOrEmpty(oldFare))
            {
                ViewBag.message = "Original Document Number " + originDoc + " details not found";

            }
            string descTicket = String.Join<char>(".", cpn(newDoc));
            string detailDescTicket = "";
            if (descTicket.Contains("1"))
            {
                detailDescTicket = "Totally Unused Ticket";
            }
            else
            {
                detailDescTicket = "Partly Used Ticket";
            }

            if (labelText == "REFUND")
            {
                double val = 0;
                double val1 = 0;
                string fr = "";
                val = Convert.ToDouble(differentFare);
                val1 = val * (0.09);
                string txtISC = val1.ToString("########.00");
                DateTime dn = Convert.ToDateTime(newdoi);
                string Par = dn.ToString("yyyyMM");
                string Tot = "";
                if (!string.IsNullOrEmpty(Orgdoi))
                {
                    DateTime dor = Convert.ToDateTime(Orgdoi);
                    Tot = dor.ToString("yyyyMM");
                }

                if (detailDescTicket == "Totally Unsused Ticket")
                {
                    fr = con(Par, oldFareCurr, newFare);
                }
                else
                    if (detailDescTicket == "Partly Used Ticket")
                {
                    fr = con(Tot, oldFareCurr, newFare);
                }
                txtNetBilling = (Convert.ToDouble(fr) - Convert.ToDouble(txtISC)).ToString("########.00");
            }
            ViewBag.txtNetBilling = txtNetBilling;
            ViewBag.cpnExchange = descTicket;
            ViewBag.descTicket = detailDescTicket;
            ViewBag.labelText = labelText;
            ViewBag.originDoc = originDoc;
            ViewBag.newDoc = newDoc;
            ViewBag.originDetail = origine;
            ViewBag.newDetail = news;
            ViewBag.displayOriginDetail = DisplayOriginDetails(originDoc);
            ViewBag.DisplayNewDetails = DisplayNewDetails(newDoc);
            ViewBag.differentFare = differentFare;
            ViewBag.bmd70 = bmd70(newDoc);
            ViewBag.bmd71 = bmd71(newDoc);
            ViewBag.bmd72 = bmd72(newDoc);
            ViewBag.bmd73 = bmd73(newDoc);
            ViewBag.bmd74 = bmd74(newDoc);
            ViewBag.bmd75 = bmd75(newDoc);
            ViewBag.bmd76 = bmd76(newDoc);
            ViewBag.penality = penality(newDoc);
            return PartialView();
        }

        public string con(string per, string Curency, string amount)
        {
            string crc = "";
            string conversion = "";

            try
            {
                string sql = "SELECT [USDRate] FROM[Ref].[CurrencyRate] where Period = '" + per + "' AND Currency = '" + Curency + "'";

                DataSet ds = new DataSet();
                ds = dbconnect.RetObjDS(sql);
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    crc = dr[ds.Tables[0].Columns[0].ColumnName].ToString();

                }
                double a = Convert.ToDouble(amount) / Convert.ToDouble(crc);

                conversion = a.ToString("######.00");
            }
            catch
            {
            }

            return conversion;

        }
        public string cpn(string Doc)
        {
            string Acc = "";
            string sql = "SELECT AccountNumber FROM pax.SalesDocumentPayment WHERE DocumentNumber = '" + Doc + "' AND FormofPaymentType = 'EX'";
            DataSet ds = new DataSet();
            ds = dbconnect.RetObjDS(sql);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                Acc = dr[ds.Tables[0].Columns[0].ColumnName].ToString();
            }

            string j = "";
            if (Acc != "")
            {
                string su = Acc.Substring(14, Acc.Length - 14).ToString();
                j = su.TrimStart(new Char[] { '0' });
            }
            return j;
        }

        private DataSet bmd70(string docNum)
        {
            string sql = " ";
            sql += " declare @doc varchar (13)='" + docNum + "' ";
            sql += " declare @uploadguid uniqueidentifier =(select SalesUploadGuid from pax.SalesRelatedDocumentInformation where DocumentNumber = @doc and IsConjunction = 0 and TransactionCode <> 'RFND' ) ";
            sql += " declare @SequenceNumber varchar(8) = (select SequenceNumber from pax.SalesRelatedDocumentInformation where DocumentNumber = @doc and IsConjunction = 0 and TransactionCode <> 'RFND' ) ";
            sql += " declare @TransactionNumber varchar(6) = (select TransactionNumber from FileHot2.vw_Bks24_DocumentIdentification where UploadGuid = @uploadguid and TicketDocumentNumber = @doc and SequenceNumber = @SequenceNumber ) ";
            sql += " select TicketDocumentNumber,ReasonforIssuanceCode ,ReasonforIssuanceCode from FileHot.Bmp70_ReasonForIssuance  ";
            sql += " where UploadGuid = @uploadguid and TransactionNumber = @TransactionNumber ";
            DataSet ds = new DataSet();
            ds = dbconnect.RetObjDS(sql);
            return ds;
        }

        private DataSet bmd71(string docNum)
        {
            string sql = "";
            sql += " declare @doc varchar (13)='" + docNum + "' " + Environment.NewLine;
            sql += " declare @uploadguid uniqueidentifier =(select SalesUploadGuid from pax.SalesRelatedDocumentInformation where DocumentNumber = @doc and IsConjunction = 0 and TransactionCode <> 'RFND' ) " + Environment.NewLine;
            sql += " declare @SequenceNumber varchar(8) = (select SequenceNumber from pax.SalesRelatedDocumentInformation where DocumentNumber = @doc and IsConjunction = 0 and TransactionCode <> 'RFND' ) " + Environment.NewLine;
            sql += " declare @TransactionNumber varchar(6) = (select TransactionNumber from FileHot2.vw_Bks24_DocumentIdentification where UploadGuid = @uploadguid and TicketDocumentNumber = @doc and SequenceNumber = @SequenceNumber ) " + Environment.NewLine;
            sql += " select TicketDocumentNumber,InConnectionWithDocumentNumber,BankExchangeRate " + Environment.NewLine;
            sql += " ,MultiplePurposeDocumentEquivalentChargePaid as MPD_EquiChargePaid,MultiplePurposeDocumentOtherCharges as MPD_OtherCharges,MultiplePurposeDocumentServiceCharge as MPD_ServiceCharge " + Environment.NewLine;
            sql += " ,MultiplePurposeDocumentTotal as MPD_Total,MultiplePurposeDocumentTotalExchangeValue as MPD_ExchangeValue " + Environment.NewLine;
            sql += " from FileHot.Bmp71_AdditionalInformation  " + Environment.NewLine;
            sql += " where UploadGuid = @uploadguid and TransactionNumber = @TransactionNumber " + Environment.NewLine;
            DataSet ds = new DataSet();
            ds = dbconnect.RetObjDS(sql);
            return ds;
        }

        private DataSet bmd72(string docNum)
        {
            string sql = " ";
            sql += " declare @doc varchar (13)='" + docNum + "' " + Environment.NewLine;
            sql += " declare @uploadguid uniqueidentifier =(select SalesUploadGuid from pax.SalesRelatedDocumentInformation where DocumentNumber = @doc and IsConjunction = 0 and TransactionCode <> 'RFND' ) " + Environment.NewLine;
            sql += " declare @SequenceNumber varchar(8) = (select SequenceNumber from pax.SalesRelatedDocumentInformation where DocumentNumber = @doc and IsConjunction = 0 and TransactionCode <> 'RFND' ) " + Environment.NewLine;
            sql += " declare @TransactionNumber varchar(6) = (select TransactionNumber from FileHot2.vw_Bks24_DocumentIdentification where UploadGuid = @uploadguid and TicketDocumentNumber = @doc and SequenceNumber = @SequenceNumber ) " + Environment.NewLine;
            sql += " select TicketDocumentNumber,AmountinLetters from FileHot.Bmp72_AmountInLetters  " + Environment.NewLine;
            sql += " where UploadGuid = @uploadguid and TransactionNumber = @TransactionNumber " + Environment.NewLine;
            DataSet ds = new DataSet();
            ds = dbconnect.RetObjDS(sql);
            return ds;
        }

        private DataSet bmd73(string docNum)
        {
            string sql = " ";
            sql += " declare @doc varchar (13)='" + docNum + "' " + Environment.NewLine;
            sql += " declare @uploadguid uniqueidentifier =(select SalesUploadGuid from pax.SalesRelatedDocumentInformation where DocumentNumber = @doc and IsConjunction = 0 and TransactionCode <> 'RFND' ) " + Environment.NewLine;
            sql += " declare @SequenceNumber varchar(8) = (select SequenceNumber from pax.SalesRelatedDocumentInformation where DocumentNumber = @doc and IsConjunction = 0 and TransactionCode <> 'RFND' ) " + Environment.NewLine;
            sql += " declare @TransactionNumber varchar(6) = (select TransactionNumber from FileHot2.vw_Bks24_DocumentIdentification where UploadGuid = @uploadguid and TicketDocumentNumber = @doc and SequenceNumber = @SequenceNumber ) " + Environment.NewLine;
            sql += " select  TicketDocumentNumber,OptionalAgencyAirlineInformation " + Environment.NewLine;
            sql += " from FileHot.Bmp73_AirlineInformation where UploadGuid = @uploadguid and TransactionNumber = @TransactionNumber" + Environment.NewLine;

            DataSet ds = new DataSet();
            ds = dbconnect.RetObjDS(sql);
            return ds;
        }

        private DataSet penality(string docNum)
        {
            string sql = " ";
            sql += " select cast (DateofIssue as Date) as DateofIssue,CurrencyType, " + Environment.NewLine;
            sql += "case when OtherAmountCode ='CP' then 'Penalty Fees' end as OtherAmountCode " + Environment.NewLine;
            sql += ",OtherAmount from Pax.SalesDocumentOtherAmount where OtherAmountCode = 'CP' and DocumentNumber='" + docNum + "' " + Environment.NewLine;

            DataSet ds = new DataSet();
            ds = dbconnect.RetObjDS(sql);
            return ds;
        }
        private DataSet bmd74(string docNum)
        {
            string sql = " ";
            sql += " declare @doc varchar (13)='" + docNum + "' " + Environment.NewLine;
            sql += " declare @uploadguid uniqueidentifier =(select SalesUploadGuid from pax.SalesRelatedDocumentInformation where DocumentNumber = @doc and IsConjunction = 0 and TransactionCode <> 'RFND' ) " + Environment.NewLine;
            sql += " declare @SequenceNumber varchar(8) = (select SequenceNumber from pax.SalesRelatedDocumentInformation where DocumentNumber = @doc and IsConjunction = 0 and TransactionCode <> 'RFND' ) " + Environment.NewLine;
            sql += " declare @TransactionNumber varchar(6) = (select TransactionNumber from FileHot2.vw_Bks24_DocumentIdentification where UploadGuid = @uploadguid and TicketDocumentNumber = @doc and SequenceNumber = @SequenceNumber ) " + Environment.NewLine;
            sql += " select  TicketDocumentNumber,PrintLineIdentifier,PrintLineText  " + Environment.NewLine;
            sql += " from FileHot.Bmp74_DocumentPrintLines where UploadGuid = @uploadguid and TransactionNumber = @TransactionNumber " + Environment.NewLine;
            DataSet ds = new DataSet();
            ds = dbconnect.RetObjDS(sql);
            return ds;
        }

        private DataSet bmd75(string docNum)
        {
            string sql = " ";
            sql += " declare @doc varchar (13)='" + docNum + "' " + Environment.NewLine;
            sql += " declare @uploadguid uniqueidentifier =(select SalesUploadGuid from pax.SalesRelatedDocumentInformation where DocumentNumber = @doc and IsConjunction = 0 and TransactionCode <> 'RFND' ) " + Environment.NewLine;
            sql += " declare @SequenceNumber varchar(8) = (select SequenceNumber from pax.SalesRelatedDocumentInformation where DocumentNumber = @doc and IsConjunction = 0 and TransactionCode <> 'RFND' ) " + Environment.NewLine;
            sql += " declare @TransactionNumber varchar(6) = (select TransactionNumber from FileHot2.vw_Bks24_DocumentIdentification where UploadGuid = @uploadguid and TicketDocumentNumber = @doc and SequenceNumber = @SequenceNumber ) " + Environment.NewLine;
            sql += " select  TicketDocumentNumber,EMDCouponNumber , iif(EMDCouponValue = '',0,[Pax].[DishAmountConvert] (EMDCouponValue,EMDCouponCurrency)) as EMDCouponValue " + Environment.NewLine;
            sql += " , left (EMDCouponCurrency,3) as EMDCouponCurrency ,EMDRelatedTicketNumber,EMDRelatedCouponNumber ,EMDReasonforIssuanceSubCode,EMDFeeOwnerAirlineDesignator " + Environment.NewLine;
            sql += " ,EMDExcessBaggageOverAllowanceQualifier,EMDExcessBaggageRateperUnit ,EMDExcessBaggageTotalNumberinExcess,EMDConsumedatIssuanceIndicator " + Environment.NewLine;
            sql += " ,EMDExcessBaggageCurrencyType from FileHot.Bmd75_ECouponRecords where UploadGuid = @uploadguid and TransactionNumber = @TransactionNumber " + Environment.NewLine;
            DataSet ds = new DataSet();
            ds = dbconnect.RetObjDS(sql);
            return ds;
        }

        private DataSet bmd76(string docNum)
        {
            string sql = " ";
            sql += " declare @doc varchar (13)='" + docNum + "' " + Environment.NewLine;
            sql += " declare @uploadguid uniqueidentifier =(select SalesUploadGuid from pax.SalesRelatedDocumentInformation where DocumentNumber = @doc and IsConjunction = 0 and TransactionCode <> 'RFND' ) " + Environment.NewLine;
            sql += " declare @SequenceNumber varchar(8) = (select SequenceNumber from pax.SalesRelatedDocumentInformation where DocumentNumber = @doc and IsConjunction = 0 and TransactionCode <> 'RFND' ) " + Environment.NewLine;
            sql += " declare @TransactionNumber varchar(6) = (select TransactionNumber from FileHot2.vw_Bks24_DocumentIdentification where UploadGuid = @uploadguid and TicketDocumentNumber = @doc and SequenceNumber = @SequenceNumber ) " + Environment.NewLine;
            sql += "select  TicketDocumentNumber,CouponNumber,EMDRemarks from FileHot.Bmd76_ERemarkRecords where UploadGuid = @uploadguid and TransactionNumber = @TransactionNumber " + Environment.NewLine;

            DataSet ds = new DataSet();
            ds = dbconnect.RetObjDS(sql);
            return ds;
        }

        private DataSet OriginalDetails(string DocNum)
        {
            string sql = "";
            sql = "SELECT FareCurrency, Fare, TotalCurrency, EquivalentFare, AgentNumericCode, TransactionCode, Format(DateofIssue,'dd-MMM-yyyy'), DocumentType, FareCalculationArea,";
            sql += "concat(AmountCollectedCurrency, ' ', AmountCollected ) as AmountCollected from pax.SalesDocumentHeader where DocumentNumber = '" + DocNum + "'";
            DataSet ds = new DataSet();
            ds = dbconnect.RetObjDS(sql);
            return ds;
        }

        private DataSet DisplayOriginDetails(string DocNum)
        {
            string sql = "";
            sql += "SELECT f2.CouponNumber,f2.ProrationFlag, f2.SectorOrigin, f2.SectorCarrier, f2.SectorDestination,Format(f1.UsageDate,'dd-MMM-yyyy'), f2.FinalShare FROM pax.SalesDocumentCoupon f1 ";
            sql += "left join pax.ProrationDetail f2 on f1.RelatedDocumentGuid = f2.RelatedDocumentGuid and f1.CouponNumber = f2.CouponNumber and f1.CouponStatus = f2.ProrationFlag ";
            sql += "WHERE f2.DocumentNumber = '" + DocNum + "' ORDER BY CouponNumber , ProrationFlag ";

            DataSet ds = new DataSet();
            ds = dbconnect.RetObjDS(sql);
            return ds;
        }

        private DataSet DisplayNewDetails(string DocNum)
        {
            string sql = "";
            sql += " select f2.CouponNumber,f2.CouponStatus,f2.OriginAirportCityCode, f2.Carrier,f2.DestinationAirportCityCode,Format(f2.UsageDate,'dd-MMM-yyyy'),f1.FinalShare  ";
            sql += "FROM  pax.SalesDocumentCoupon f2 left join pax.ProrationDetail f1  on f1.RelatedDocumentGuid = f2.RelatedDocumentGuid  ";
            sql += "and f1.CouponNumber = f2.CouponNumber and f1.ProrationFlag = f2.CouponStatus WHERE f2.DocumentNumber =  '" + DocNum + "' ORDER BY f1.CouponNumber , f1.ProrationFlag ";

            DataSet ds = new DataSet();
            ds = dbconnect.RetObjDS(sql);
            return ds;
        }
        public ActionResult MoreDetailsExchange()
        {
            string docnum = Request["dataValue"];
            string sql = "";
            sql = sql + "SELECT  [DocumentNumber] ,[AgentNumericCode],FORMAT(DateofIssue,'dd-MMM-yyyy') as  DateofIssue,FORMAT(SaleDate,'dd-MMM-yyyy') as SaleDate,[PassengerName] ,[FareCalculationArea],[EndosRestriction],[OriginalIssueDocumentNumber] ,[OriginalIssueAgentNumericCode], ";
            sql = sql + "[InConnectionWithDocumentNumber] ,[FareCurrency],[Fare] ,[TotalCurrency] as EquiFareCurrency ,[EquivalentFare] ,[ExchangeADC] ,[DocumentType] ,[TransactionCode] ,[IsReissue] ,[AmountCollectedCurrency],[AmountCollected] ";
            sql = sql + " FROM [Pax].[SalesDocumentHeader]";
            sql = sql + " WHERE OriginalIssueDocumentNumber = '" + docnum + "' ORDER BY DateofIssue";

            DataSet ds = new DataSet();
            ds = dbconnect.RetObjDS(sql);
            return PartialView(ds);
        }



        private string val(string CurrOri, string amout, string dateIss)
        {
            string a = "";
            string USDRATE = "";

            DateTime dt = Convert.ToDateTime(dateIss);
            string period = dt.ToString("yyyyMM");

            string sql = "SELECT [USDRate]" + Environment.NewLine;
            sql += "From [Ref].[CurrencyRate]" + Environment.NewLine;
            sql += "WHERE [Period] = '" + period + "' and [Currency] = '" + CurrOri + "'";

            DataSet ds = new DataSet();
            ds = dbconnect.RetObjDS(sql);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                USDRATE = dr[ds.Tables[0].Columns[0].ColumnName].ToString();
            }

            string IniVal = "";
            try
            {
                IniVal = (valid(amout) / valid(USDRATE)).ToString();
            }
            catch
            {
                IniVal = "0";
            }
            a = IniVal;
            return a;
        }

        private string fareDiff(string x, string y)
        {
            string diff = "";

            if (x != "" && y != "")
                diff = (Convert.ToDouble(x) - Convert.ToDouble(y)).ToString();
            else
                diff = "";

            return diff;
        }


        /***************************************** begin Surcharges*************************************************/
        public ActionResult LoadSurcharges()
        {

            string ag = "";
            string tc = "";
            string docn = "";
            string dateFrom1 = Request["dateFrom"];
            string dateFrom = ConvertDate(dateFrom1);
            string dateTo1 = Request["dateTo"];
            string dateTo = ConvertDate(dateTo1);
            string surcharges = Request["surcharges"];
            string dropDownSelctType = Request["dropDownSelctType"];
            string dropDownSelctDoc = Request["dropDownSelctDoc"];
            if (dropDownSelctType == "Agent Numeric Code")
            {
                if (dropDownSelctDoc == "-All-")
                {
                    ag = "%";
                    docn = "%";

                }
                else
                {
                    ag = dropDownSelctDoc;
                    docn = "%";

                }

            }
            else
            if (dropDownSelctType == "Document Number")
            {
                if (dropDownSelctDoc == "-All-")
                {
                    ag = "%";
                    docn = "%";

                }
                else
                {
                    ag = "%";
                    docn = dropDownSelctDoc;

                }

            }

            else
            {
                ag = "%";
                docn = "%";

            }
            if (surcharges == "-All-")
            {
                tc = "%";
            }
            else
            {
                tc = surcharges;
            }
            string sql = "with base as ( select  FORMAT(f1.SaleDate, 'dd-MMM-yyyy') as SalesDate, f1.AgentNumericCode, f1.DocumentNumber,'' as CouponNumber" + Environment.NewLine;
            sql = sql + ", Pax.fn_FOP_Combined( f1.HdrGuid ) as FormOfpayment, '' as StopOverCode" + Environment.NewLine;
            sql = sql + ",f5.DocumentAmountType as DocumentType, f5.CurrencyType as Currency ,f5.OtherAmountCode  as TaxCode ,f5.OtherAmount as Taxamount , f1.EndosRestriction as  [Endorsements/Restrictions]" + Environment.NewLine;
            sql = sql + ",f1.TransactionCode from Pax.SalesDocumentHeader f1  join Pax.SalesRelatedDocumentInformation f2 on f1.HdrGuid = f2.HdrGuid and f1.SaleDate" + Environment.NewLine;
            sql = sql + "between '" + dateFrom + "' and '" + dateTo + "' left join pax.salesdocumentotheramount f5 on f2.RelatedDocumentGuid = f5.RelatedDocumentGuid )" + Environment.NewLine;
            sql = sql + "select * from base where DocumentType = 'Surcharges' and AgentNumericCode like '" + ag + "' and DocumentNumber like '" + docn + "' and TaxCode like '" + tc + "'";
            DataSet ds = new DataSet();
            SqlConnection con = new SqlConnection(pbConnectionString);

            SqlDataAdapter ada = new SqlDataAdapter(sql, con);
            ada.Fill(ds);
            con.Close();
            ViewBag.SurchargesSum = SurchargesSum(dateFrom, dateTo, docn, ag, tc);
            return PartialView(ds);
        }

        public DataSet SurchargesSum(string dateFrom, string dateTo, string docn, string ag, string tc)
        {

            string sql = "with base as ( select  FORMAT(f1.SaleDate, 'dd-MMM-yyyy') as SalesDate, f1.TransactionCode , f1.AgentNumericCode, f1.DocumentNumber,f5.DocumentAmountType as DocumentType" + Environment.NewLine;
            sql = sql + ", Pax.fn_FOP_Combined( f1.HdrGuid ) as FormOfpayment  ,f5.CurrencyType as Currency ,f5.OtherAmountCode  as TaxCode" + Environment.NewLine;
            sql = sql + ",f5.OtherAmount  as Taxamount from Pax.SalesDocumentHeader f1  join Pax.SalesRelatedDocumentInformation f2" + Environment.NewLine;
            sql = sql + "on f1.HdrGuid = f2.HdrGuid and f1.SaleDate   between  '" + dateFrom + "' and '" + dateTo + "'" + Environment.NewLine;
            sql = sql + "left join pax.salesdocumentotheramount f5 on f2.RelatedDocumentGuid = f5.RelatedDocumentGuid )" + Environment.NewLine;
            sql = sql + "select FormOfpayment,Transactioncode,Currency,TaxCode,sum(Taxamount),count(DocumentNumber) as ItemCount from base " + Environment.NewLine;
            sql = sql + "where DocumentType = 'Surcharges' and AgentNumericCode like '" + ag + "' and DocumentNumber like '" + docn + "'" + Environment.NewLine;
            sql = sql + "and TaxCode like '" + tc + "'  group by FormOfpayment,TransactionCode, Currency, TaxCode  ";

            DataSet ds = new DataSet();
            SqlConnection con = new SqlConnection(pbConnectionString);
            SqlDataAdapter ada = new SqlDataAdapter(sql, con);
            ada.Fill(ds);
            con.Close();
            return ds;
        }

        public ActionResult LoadSurchargesType()
        {

            string dateFrom1 = Request["dateFrom"];
            string dateFrom = ConvertDate(dateFrom1);
            string dateTo1 = Request["dateTo"];
            string dateTo = ConvertDate(dateTo1);
            string surcharges = Request["surcharges"];
            string dropDownSelctType = Request["dropDownSelctType"];
            string dropDownSelctDoc = Request["dropDownSelctDoc"];
            string sql = " select distinct OtherAmountCode from pax.SalesDocumentOtherAmount f1 where OtherAmountCode like 'Y%' and f1.DocumentAmountType = 'surcharges'";
            DataSet ds = new DataSet();
            SqlConnection con = new SqlConnection(pbConnectionString);
            SqlDataAdapter ada = new SqlDataAdapter(sql, con);
            ada.Fill(ds);
            con.Close();
            return PartialView(ds);
        }

        public ActionResult Surcharges()
        {
            ViewBag.dateFrom = DateTime.Now.AddMonths(-1).ToString("dd-MMM-yyyy");
            ViewBag.dateTo = DateTime.Now.ToString("dd-MMM-yyyy");
            string sql = "select distinct OtherAmountCode from pax.SalesDocumentOtherAmount f1 where OtherAmountCode like 'Y%' and f1.DocumentAmountType = 'surcharges'";
            /*DataSet ds = new DataSet();
            SqlConnection con = new SqlConnection(pbConnectionString);
            SqlDataAdapter ada = new SqlDataAdapter(sql, con);
            ada.Fill(ds);
            con.Close();*/
            dbconnect.pbConnectionString = pbConnectionString;
            SqlDataReader myReader = dbconnect.GetData(sql);
            List<AmountCode> model = new List<AmountCode>();
            while (myReader.Read())
            {
                var details = new AmountCode();
                details.OtherAmountCode = myReader["OtherAmountCode"].ToString();
                model.Add(details);
            }
            ViewBag.Surcharges = model;
            return PartialView();
        }

        public ActionResult AgentCherch()
        {
            string dateFrom1 = Request["dateFrom"];
            string dateFrom = ConvertDate(dateFrom1);
            string dateTo1 = Request["dateTo"];
            string dateTo = ConvertDate(dateTo1);
            string sql = "select distinct AgentNumericCode, agtown.TradingName as [AgentName] from pax.SalesDocumentHeader sdh left join Ref.VW_Agent agtown	on left(sdh.AgentNumericCode,7) = agtown.AgencyNumericCode WHERE [SaleDate] between '" + dateFrom + "' and '" + dateTo + "' and SurchargeCollected is not null order by 1";
            dbconnect.pbConnectionString = pbConnectionString;
            SqlDataReader myReader = dbconnect.GetData(sql);

            List<AgentCode> model = new List<AgentCode>();
            while (myReader.Read())
            {
                var details = new AgentCode();
                details.AgentNumericCode = myReader["AgentNumericCode"].ToString();
                details.AgentName = myReader["AgentName"].ToString();
                model.Add(details);
            }
            ViewBag.Agent = model;

            return PartialView();
        }

        /*******************************************************End Surcharges***************************************************************/

        /*******************************************************Begin Tfcs*******************************************************************/
        public ActionResult Tfcs()
        {
            ViewBag.dateFrom = DateTime.Now.AddMonths(-1).ToString("dd-MMM-yyyy");
            ViewBag.dateTo = DateTime.Now.ToString("dd-MMM-yyyy");
            return PartialView();
        }

        public ActionResult AgentCherchTfcs()
        {
            string dateFrom1 = Request["dateFrom"];
            string dateFrom = ConvertDate(dateFrom1);
            string dateTo1 = Request["dateTo"];
            string dateTo = ConvertDate(dateTo1);
            string sql = "select distinct AgentNumericCode, agtown.TradingName as [AgentName] from pax.SalesDocumentHeader sdh left join Ref.VW_Agent agtown	on left(sdh.AgentNumericCode,7) = agtown.AgencyNumericCode WHERE [SaleDate] between '" + dateFrom + "' and '" + dateTo + "'";
            dbconnect.pbConnectionString = pbConnectionString;
            SqlDataReader myReader = dbconnect.GetData(sql);

            List<AgentCode> model = new List<AgentCode>();
            while (myReader.Read())
            {
                var details = new AgentCode();
                details.AgentNumericCode = myReader["AgentNumericCode"].ToString();
                details.AgentName = myReader["AgentName"].ToString();
                model.Add(details);
            }
            ViewBag.Agent = model;
            return PartialView();
        }

        public ActionResult LoadTfcs()
        {
            string dateFrom1 = Request["dateFrom"];
            string dateFrom = ConvertDate(dateFrom1);
            string dateTo1 = Request["dateTo"];
            string dateTo = ConvertDate(dateTo1);
            string docType = Request["docType"];
            string selctDoc = Request["selectDoc"];
            string ag = "";
            string docn = "";
            string DocType = "";
            if (docType == "Agent Numeric Code")
            {
                if (selctDoc == "-All-")
                {
                    ag = "%";
                    docn = "%";

                }
                else
                {
                    ag = selctDoc;
                    docn = "%";

                }

            }
            else
                           if (docType == "Document Number")
            {
                if (selctDoc == "-All-")
                {
                    ag = "%";
                    docn = "%";

                }
                else
                {
                    ag = "%";
                    docn = selctDoc;

                }

            }

            else
            {
                ag = "%";
                docn = "%";

            }
            SqlConnection con = new SqlConnection(pbConnectionString);
            SqlCommand cmd = new SqlCommand("[Pax].[SP_Tax_By_Coupon_Level_And_Surcharge]", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 300;
            cmd.Parameters.AddWithValue("@SalesDate_From", dateFrom);
            cmd.Parameters.AddWithValue("@SalesDate_To", dateTo);
            cmd.Parameters.AddWithValue("@DocumentType", "Tax");
            cmd.Parameters.AddWithValue("@AgentNumCode", ag);
            cmd.Parameters.AddWithValue("@DocNum", docn);
            DataSet ds = new DataSet();
            SqlDataAdapter ada = new SqlDataAdapter(cmd);
            ada.Fill(ds);
            int lon = ds.Tables[0].Rows.Count;
            con.Close();
            ViewBag.compt = lon;
            String[,] tableau = new String[12, lon];
            int i = 0;
            foreach (DataRow dr in ds.Tables[0].Rows)
            {


                string test = dr[ds.Tables[0].Columns[0].ColumnName].ToString();
                string date = Convert.ToDateTime(test).ToString("yyyyMM");
                string date1 = Convert.ToDateTime(test).ToString("dd/MM/yyyy");
                tableau[0, i] = date1;
                tableau[1, i] = date;
                tableau[2, i] = dr[ds.Tables[0].Columns[1].ColumnName].ToString();
                tableau[3, i] = dr[ds.Tables[0].Columns[2].ColumnName].ToString();
                tableau[4, i] = dr[ds.Tables[0].Columns[3].ColumnName].ToString();
                tableau[5, i] = dr[ds.Tables[0].Columns[5].ColumnName].ToString();
                tableau[6, i] = dr[ds.Tables[0].Columns[6].ColumnName].ToString();
                tableau[7, i] = dr[ds.Tables[0].Columns[11].ColumnName].ToString();
                tableau[8, i] = dr[ds.Tables[0].Columns[7].ColumnName].ToString();
                tableau[9, i] = dr[ds.Tables[0].Columns[8].ColumnName].ToString();
                tableau[10, i] = dr[ds.Tables[0].Columns[9].ColumnName].ToString();
                tableau[11, i] = dr[ds.Tables[0].Columns[10].ColumnName].ToString();
                i = i + 1;
            }
            ViewBag.tableau = tableau;
            string[] cll = new string[lon];

            for (int g = 0; g < lon; g++)
            {
                cll[g] = tableau[8, g].ToString();
            }

            string[] b = cll.Distinct().ToArray();
            string[,] dgsum = new string[3, b.Length];

            for (int y = 0; y < b.Length; y++)
            {
                //dgsum.Rows.Add("");
                dgsum[0, y] = b[y].ToString();
            }


            string[] Rfn = new string[b.Length];
            string[] tick = new string[b.Length];


            for (int o = 0; o < b.Length; o++)
            {
                for (int k = 0; k < lon; k++)
                {
                    if (dgsum[0, o].ToString() == "")
                    {

                    }
                    else if ((dgsum[0, o].ToString() == tableau[8, k].ToString()) && (tableau[7, k].ToString() != "RFND"))
                    {
                        if (tick[o] == null)
                        {
                            tick[o] = "0";
                        }
                        var culture = System.Globalization.CultureInfo.CurrentCulture;
                        decimal interm = Convert.ToDecimal(tick[o].ToString(), new CultureInfo(culture.Name));
                        decimal interm1 = Convert.ToDecimal(tableau[10, k].ToString(), new CultureInfo(culture.Name));
                        decimal val = interm + interm1;
                        tick[o] = val.ToString();
                        //tick[o] = (Convert.ToDecimal(tick[o].ToString(), System.Globalization.CultureInfo.InvariantCulture) + Convert.ToDecimal(tableau[10, k], System.Globalization.CultureInfo.InvariantCulture)).ToString();
                    }
                    else
                        if ((dgsum[0, o].ToString() == tableau[8, k].ToString()) && (tableau[7, k].ToString() == "RFND"))
                    {
                        if (Rfn[o] == null)
                        {
                            Rfn[o] = "0";
                        }
                        var culture = System.Globalization.CultureInfo.CurrentCulture;
                        decimal interm2 = Convert.ToDecimal(Rfn[o].ToString(), new CultureInfo(culture.Name));
                        decimal interm3 = Convert.ToDecimal(tableau[10, k], new CultureInfo(culture.Name));
                        decimal rfn = interm2 + interm3;
                        Rfn[o] = rfn.ToString();
                        //Rfn[o] = (Convert.ToDecimal(Rfn[o].ToString(), System.Globalization.CultureInfo.InvariantCulture) + Convert.ToDecimal(tableau[10,k], System.Globalization.CultureInfo.InvariantCulture)).ToString();
                    }

                }
            }

            for (int j = 0; j < b.Length; j++)
            {

                try
                {
                    dgsum[1, j] = tick[j].ToString();
                    dgsum[2, j] = Rfn[j].ToString();
                }
                catch
                {
                    dgsum[2, j] = "0";
                }
            }
            ViewBag.dgsum = dgsum;
            ViewBag.comp1 = b.Length;
            /*Array.Clear(cll, 0, cll.Length);
            Array.Clear(b, 0, b.Length);
            Array.Clear(Rfn, 0, Rfn.Length);
            Array.Clear(tick, 0, tick.Length);*/

            return PartialView(ds);
        }

        /********************************************************************end Tfcs************************************************************/
        /*******************************************************************Begin Vat************************************************************/
        public ActionResult Vat()
        {
            string sql = "SELECT distinct SalesPeriod FROM pax.fn_SalesCouponDetail(null) WHERE DomesticInternational = 'D' ORDER by 1 desc";
            SqlDataReader myReader = dbconnect.GetData(sql);

            List<Period> model = new List<Period>();
            while (myReader.Read())
            {
                var details = new Period();
                details.SalesPeriod = myReader["SalesPeriod"].ToString();
                model.Add(details);
            }
            /* DataSet ds = new DataSet();
             SqlConnection con = new SqlConnection(pbConnectionString);
             SqlDataAdapter ada = new SqlDataAdapter(sql, con);
             ada.Fill(ds);
             con.Close();*/
            ViewBag.Agent = AgentVat();
            ViewBag.Period = model;
            return PartialView();
        }

        public List<AgentCode> AgentVat()
        {
            string sql = "SELECT distinct AgentNumericCode FROM pax.fn_SalesCouponDetail(null) WHERE DomesticInternational = 'D' ORDER by 1 desc";
            dbconnect.pbConnectionString = pbConnectionString;
            SqlDataReader myReader = dbconnect.GetData(sql);

            List<AgentCode> model = new List<AgentCode>();
            while (myReader.Read())
            {
                var details = new AgentCode();
                details.AgentNumericCode = myReader["AgentNumericCode"].ToString();
                model.Add(details);
            }

            return model;
        }

        public ActionResult LoadVat()
        {
            string dateFrom = Request["dateFrom"];
            string dateTo = Request["dateTo"];
            string AgentCode = Request["AgentCode"];
            string agPAra = null;




            if (AgentCode == "-All-")
            {
                agPAra = "%";
            }
            else
            {
                agPAra = AgentCode;
            }

            decimal Share = 0;
            decimal VatA = 0;
            decimal VatI = 0;
            int compt = 0;
            string sql = "select f1.RelatedDocumentNumber,f1.CouponNumber, f1.SalesPeriod, f1.AgentNumericCode ";
            sql = sql + " ,OriginAirportCityCode ,DestinationAirportCityCode ,FinalShare ,VATPercentage ";
            sql = sql + " ,((FinalShare * f2.VATPercentage )/100) as VatAmount ";
            sql = sql + " ,((FinalShare * f2.VATPercentage )/100) + FinalShare as TotalWithVat ";
            sql = sql + " from  pax.fn_SalesCouponDetail(null) f1 join Pax.VW_SalesHeader f2 on f1.HdrGuidRef = f2.HdrGuid ";
            sql = sql + " where DomesticInternational ='D'  and f1.salesPeriod between '" + dateFrom + "' and '" + dateTo + "'   ";
            sql = sql + " and f1.AgentNumericCode like '" + agPAra + "'  group by f1.RelatedDocumentNumber,f1.CouponNumber,f1.SalesPeriod ";
            sql = sql + " ,f1.AgentNumericCode,OriginAirportCityCode,DestinationAirportCityCode,FinalShare,VATPercentage ";
            DataSet ds = new DataSet();
            SqlConnection con = new SqlConnection(pbConnectionString);
            SqlDataAdapter ada = new SqlDataAdapter(sql, con);
            ada.Fill(ds);
            con.Close();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                if (dr[ds.Tables[0].Columns[0].ColumnName].ToString() != null)
                    compt += 1;
                if (dr[ds.Tables[0].Columns[6].ColumnName].ToString() != null && !string.IsNullOrWhiteSpace(dr[ds.Tables[0].Columns[6].ColumnName].ToString()))
                    Share += Convert.ToDecimal(dr[ds.Tables[0].Columns[6].ColumnName], System.Globalization.CultureInfo.InvariantCulture);
                if (dr[ds.Tables[0].Columns[8].ColumnName].ToString() != null && !string.IsNullOrWhiteSpace(dr[ds.Tables[0].Columns[8].ColumnName].ToString()))
                    VatA += Convert.ToDecimal(dr[ds.Tables[0].Columns[8].ColumnName], System.Globalization.CultureInfo.InvariantCulture);
                if (dr[ds.Tables[0].Columns[9].ColumnName].ToString() != null && !string.IsNullOrWhiteSpace(dr[ds.Tables[0].Columns[9].ColumnName].ToString()))
                    VatI += Convert.ToDecimal(dr[ds.Tables[0].Columns[9].ColumnName], System.Globalization.CultureInfo.InvariantCulture);
            }
            ViewBag.Share = Share;
            ViewBag.VatA = VatA;
            ViewBag.VatI = VatI;
            ViewBag.compt = compt;
            return PartialView(ds);

        }

        /********************************************************end Vat************************************************************/
        /********************************************************begin Refund*******************************************************/

        public ActionResult ListofRefunds()
        {
            ViewBag.dateFrom = DateTime.Now.AddMonths(-1).ToString("dd-MMM-yyyy");
            ViewBag.dateTo = DateTime.Now.ToString("dd-MMM-yyyy");
            return PartialView();
        }

        public ActionResult LoadListRefund()
        {
            string dateFrom1 = Request["dateFrom"];
            string dateFrom = ConvertDate(dateFrom1);
            string dateTo1 = Request["dateTo"];
            string dateTo = ConvertDate(dateTo1);
            string selectType = Request["dropdownType"];
            string selectDoc = Request["dropdownDoc"];
            string sql = "";
            var culture = System.Globalization.CultureInfo.CurrentCulture;

             sql += "with base as ( " + Environment.NewLine;
             sql += " select * " + Environment.NewLine;
             sql += " from pax.SalesDocumentOtherAmount " + Environment.NewLine;
             sql += "where OtherAmountCode = 'CP') " + Environment.NewLine;
             sql += "SELECT f1.[DocumentNumber],f1.[DateofIssue],f2.OriginalTicket,f2.DateOfIssue, " + Environment.NewLine;
             sql += " f1.[AgentNumericCode],f1.[PassengerName],f1.[DocumentType],f1.[AmountCollectedCurrency],f1.[AmountCollected]" + Environment.NewLine;
             sql += " ,f3.CurrencyType, f3.OtherAmount, f3.OtherAmountCode" + Environment.NewLine;
             sql += " FROM [Pax].[SalesDocumentHeader]  f1 " + Environment.NewLine;
             sql += " left JOIN pax.VW_RefundedTicket f2 on f1.DocumentNumber = f2.RefundedTicket" + Environment.NewLine;
             sql += " left join base f3 on f1.DocumentNumber = f3.DocumentNumber" + Environment.NewLine;
             sql = sql + "WHERE f1.TransactionCode = 'RFND' AND (f1.[DateofIssue] between cast('" + dateFrom + "' as date) AND cast('" + dateTo + "' as date))" + Environment.NewLine;
             sql = sql + Selection(selectType, selectDoc) + Environment.NewLine;
             sql = sql + "ORDER BY 2";

            DataSet ds = new DataSet();
            SqlConnection con = new SqlConnection(pbConnectionString);
            SqlDataAdapter ada = new SqlDataAdapter(sql, con);
            ada.Fill(ds);
            con.Close();
            ViewBag.esd = ds;
            int lon = ds.Tables[0].Rows.Count;
            string[,] tableau = new string[11, lon];
            int i = 0;
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                string d1 = dr[ds.Tables[0].Columns[1].ColumnName].ToString();
                string d2 = dr[ds.Tables[0].Columns[3].ColumnName].ToString();

                tableau[0, i] = dr[ds.Tables[0].Columns[0].ColumnName].ToString();

                if (d1 != "")
                {
                    tableau[1, i] = Convert.ToDateTime(d1).ToShortDateString();
                }
                tableau[2, i] = dr[ds.Tables[0].Columns[2].ColumnName].ToString();

                if (d2 != "")
                {
                    tableau[3, i] = Convert.ToDateTime(d2).ToShortDateString();
                }


                tableau[4, i] = dr[ds.Tables[0].Columns[4].ColumnName].ToString();
                tableau[5, i] = dr[ds.Tables[0].Columns[5].ColumnName].ToString();
                tableau[6, i] = dr[ds.Tables[0].Columns[6].ColumnName].ToString();
                tableau[7, i] = dr[ds.Tables[0].Columns[7].ColumnName].ToString();
                tableau[8, i] = dr[ds.Tables[0].Columns[8].ColumnName].ToString();
                tableau[9, i] = dr[ds.Tables[0].Columns[9].ColumnName].ToString();
                tableau[10, i] = dr[ds.Tables[0].Columns[10].ColumnName].ToString();

                i++;
            }
            ViewBag.Tableau = tableau;
            ViewBag.compt = lon;
            string[] cll = new string[lon];

            for (int g = 0; g < lon; g++)
            {
                cll[g] = tableau[7, g].ToString();
            }

            string[] b = cll.Distinct().ToArray();
            string[,] dgsummary = new string[3, b.Length];
            for (int y = 0; y < b.Length; y++)
            {
                //dgsummary.Rows.Add("");
                dgsummary[1, y] = b[y].ToString();
            }

            string[] sum = new string[b.Length];
            string[] cnt = new string[b.Length];

            for (int o = 0; o < b.Length; o++)
            {
                for (int k = 0; k < lon; k++)
                {
                    if (dgsummary[1, o].ToString() == tableau[7, k].ToString())
                    {
                        if (sum[o] == null)
                        {
                            sum[o] = "0";
                        }
                        decimal interm = Convert.ToDecimal(sum[o].ToString(), new CultureInfo(culture.Name));
                        decimal interm1 = Convert.ToDecimal(tableau[8, k].ToString(), new CultureInfo(culture.Name));
                        decimal val = interm + interm1;
                        //sum[o] = (Convert.ToDecimal(sum[o].ToString()) + Convert.ToDecimal(tableau[8,k])).ToString();
                        sum[o] = val.ToString();

                        if (cnt[o] == null)
                        {
                            cnt[o] = "0";
                        }

                        cnt[o] = (Convert.ToDecimal(cnt[o].ToString(), new CultureInfo(culture.Name)) + 1).ToString();
                    }
                }
            }

            for (int yy = 0; yy < b.Length; yy++)
            {
                dgsummary[2, yy] = sum[yy].ToString();
                dgsummary[0, yy] = cnt[yy].ToString();
            }
            ViewBag.comp1 = b.Length;

            /*Array.Clear(cll, 0, cll.Length);
            Array.Clear(b, 0, b.Length);
            Array.Clear(sum, 0, sum.Length);
            Array.Clear(cnt, 0, cnt.Length);*/
            ViewBag.dgsummary = dgsummary;
            return PartialView(ds);
        }

        private string Selection(string selectType, string selectDoc)
        {
            string sql = "";

            if ((selectType == "Document Number") && (selectDoc != ""))
            {
                sql = sql + "and f1.[DocumentNumber] = '" + selectDoc + "'";
            }
            else
                if ((selectType == "Passenger Name") && (selectDoc != ""))
            {
                sql = sql + "and f1.[PassengerName] = '" + selectDoc + "'";
            }
            else
                    if ((selectType == "Agent Numeric Code") && (selectDoc != ""))
            {
                sql = sql + "and f1.[AgentNumericCode] = '" + selectDoc + "'";
            }

            return sql;


        }

        public ActionResult AgentRefund()
        {
            string value = Request["value"];
            string CommandText = "";
            int n;
            bool isNumeric = int.TryParse(value, out n);
            if (value == "-All-")
                value = "";
            if (isNumeric)
            {
                CommandText = "select distinct AgentNumericCode, agtown.TradingName as [AgentName]" + Environment.NewLine;
                CommandText += "from pax.SalesDocumentHeader sdh" + Environment.NewLine;
                CommandText += "left join Ref.VW_Agent agtown	on left(sdh.AgentNumericCode,7) = agtown.AgencyNumericCode" + Environment.NewLine;
                CommandText += "WHERE 1=1";
                CommandText += "and sdh.TransactionCode = 'RFND'";
                CommandText += " and AgentNumericCode like " + "'" + value + "%'";
            }
            else
            {
                CommandText = "select distinct AgentNumericCode, agtown.TradingName as [AgentName]" + Environment.NewLine;
                CommandText += "from pax.SalesDocumentHeader sdh" + Environment.NewLine;
                CommandText += "left join Ref.VW_Agent agtown	on left(sdh.AgentNumericCode,7) = agtown.AgencyNumericCode" + Environment.NewLine;
                CommandText += "WHERE 1=1";
                CommandText += "and sdh.TransactionCode <> 'RFND'";
                CommandText += " and agtown.TradingName like " + "'" + value + "%'";
            }


            dbconnect.pbConnectionString = pbConnectionString;
            SqlDataReader myReader = dbconnect.GetData(CommandText);

            List<AgentCode> model = new List<AgentCode>();
            while (myReader.Read())
            {
                var details = new AgentCode();
                details.AgentNumericCode = myReader["AgentNumericCode"].ToString();
                details.AgentName = myReader["AgentName"].ToString();
                model.Add(details);
            }
            ViewBag.Agent = model;
            return PartialView();
        }

        public ActionResult DetailsRfn()
        {
            string RfnDoc = Request["RfnDoc"];
            string OrDtIss = Request["OrDtIss"];
            string OrDcType = Request["OrDcType"];
            string DocNum = Request["DocNum"];
            string RfnDt = Request["RfnDt"];
            string name = Request["name"];
            string AgtCode = Request["AgtCode"];
           
            ViewBag.RfnDoc = RfnDoc;
            ViewBag.OrDtIss = OrDtIss;
            ViewBag.OrDcType = OrDcType;
            ViewBag.DocNum = DocNum;
            ViewBag.RfnDt = RfnDt;
            ViewBag.name = name;
            ViewBag.AgtCode = AgtCode;
            ViewBag.Penalty = Request["Penalty"];
            //ViewBag.dtrfn = dtrfn;

            string sql = " SELECT  " + Environment.NewLine;
            sql = sql + " FORMAT(sdh.[DateofIssue], 'd/M/yyyy') as DateofIssue,sdh.[TransactionGroup] ,sdh.[AmountCollectedCurrency],sdh.[AmountCollected]  " + Environment.NewLine;
            sql = sql + " ,sdh.[TaxCollectedCurrency]," + Environment.NewLine;
            sql = sql + " (case when (sdh.[TaxCollectedCurrency] is not null) then sum(sda.otheramount) else 0 END) as TaxCollected" + Environment.NewLine;
            sql = sql + ",sdh.[SurchargeCollectedCurrency]  " + Environment.NewLine;
            sql = sql + " ,sdh.[SurchargeCollected],sdh.[CommissionCollectedCurrency],sdh.[CommissionCollected],sdh.[AgentNumericCode], sdh.[EndosRestriction]  " + Environment.NewLine;
            sql = sql + " FROM [Pax].[SalesDocumentHeader] sdh   " + Environment.NewLine;
            sql = sql + " left Join Pax.SalesRelatedDocumentInformation srd on sdh.HdrGuid = srd.HdrGuid  " + Environment.NewLine;
            sql = sql + " left join pax.SalesDocumentOtherAmount sda on srd.RelatedDocumentGuid = sda.RelatedDocumentGuid  " + Environment.NewLine;
            sql = sql + " WHERE sda.OtherAmountCode<> 'CP' and sdh.[DocumentNumber]  = '" + DocNum + "'  " + Environment.NewLine;
            sql = sql + " AND sdh.[Transactioncode] <> 'RFND'" + Environment.NewLine;
            sql = sql + " group by sdh.[DateofIssue] ,sdh.[TransactionGroup] ,sdh.[AmountCollectedCurrency],sdh.[AmountCollected]  " + Environment.NewLine;
            sql = sql + " ,sdh.[TaxCollectedCurrency],sdh.[SurchargeCollectedCurrency]  " + Environment.NewLine;
            sql = sql + " ,sdh.[SurchargeCollected],sdh.[CommissionCollectedCurrency],sdh.[CommissionCollected],sdh.[AgentNumericCode],sdh.[EndosRestriction]  " + Environment.NewLine;
            DataSet ds = new DataSet();
            SqlConnection con = new SqlConnection(pbConnectionString);
            SqlDataAdapter ada = new SqlDataAdapter(sql, con);
            ada.Fill(ds);
            con.Close();
            ViewBag.RfnDtls = RfndDetails(DocNum);
            ViewBag.tabfare = LoadFareCalc(DocNum);
            DataSet Diff = ViewBag.RfnDtls;
            string a = null;
            string b = null;

            string c = null;
            string d = null;

            string f = null;
            string g = null;

            string h = null;
            string i = null;
            string currOr1 = null;
            string currOr2 = null;
            string currOr3 = null;
            string currOr4 = null;
            string currRf1 = null;
            string currRf2 = null;
            string currRf3 = null;
            string currRf4 = null;

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                a = dr[ds.Tables[0].Columns[3].ColumnName].ToString();
                c = dr[ds.Tables[0].Columns[5].ColumnName].ToString();
                f = dr[ds.Tables[0].Columns[7].ColumnName].ToString();
                h = dr[ds.Tables[0].Columns[9].ColumnName].ToString();
                currOr1 = dr[ds.Tables[0].Columns[2].ColumnName].ToString();
                currOr2 = dr[ds.Tables[0].Columns[4].ColumnName].ToString();
                currOr3 = dr[ds.Tables[0].Columns[6].ColumnName].ToString();
                currOr4 = dr[ds.Tables[0].Columns[8].ColumnName].ToString();
            }
            foreach (DataRow dr in Diff.Tables[0].Rows)
            {
                b = dr[Diff.Tables[0].Columns[3].ColumnName].ToString();
                d = dr[Diff.Tables[0].Columns[5].ColumnName].ToString();
                g = dr[Diff.Tables[0].Columns[7].ColumnName].ToString();
                i = dr[Diff.Tables[0].Columns[9].ColumnName].ToString();
                currRf1 = dr[Diff.Tables[0].Columns[2].ColumnName].ToString();
                currRf2 = dr[Diff.Tables[0].Columns[4].ColumnName].ToString();
                currRf3 = dr[Diff.Tables[0].Columns[6].ColumnName].ToString();
                currRf4 = dr[Diff.Tables[0].Columns[3].ColumnName].ToString();
            }
            string[] tabDiff = new string[8];
            if (currOr1 == currRf1)
            {
                if ((a == "null") || (a == ""))
                {
                    a = "0";
                }

                if ((b == "null") || (b == ""))
                {
                    b = "0";
                }
                var culture = System.Globalization.CultureInfo.CurrentCulture;
                double interm1 = Convert.ToDouble(a, new CultureInfo(culture.Name));
                double interm2 = Convert.ToDouble(b, new CultureInfo(culture.Name));
                double val = interm1 + interm2;
                tabDiff[0] = currOr1;
                tabDiff[1] = val.ToString();
            }

            if (currOr2 == currRf2)
            {
                if ((c == "null") || (c == ""))
                {
                    c = "0";
                }

                if ((d == "null") || (d == ""))
                {
                    d = "0";
                }
                double interm1 = Convert.ToDouble(c, new CultureInfo("en-US"));
                double interm2 = Convert.ToDouble(d, new CultureInfo("en-US"));
                double val = interm1 + interm2;
                tabDiff[2] = currOr1;
                tabDiff[3] = val.ToString();
            }

            if (currOr3 == currRf3)
            {
                if ((f == "null") || (f == ""))
                {
                    f = "0";
                }

                if ((g == "null") || (g == ""))
                {
                    g = "0";
                }
                double interm1 = Convert.ToDouble(f, new CultureInfo("en-US"));
                double interm2 = Convert.ToDouble(g, new CultureInfo("en-US"));
                double val = interm1 + interm2;
                tabDiff[4] = currOr1;
                tabDiff[5] = val.ToString();
            }

            if (currOr4 == currRf4)
            {
                if ((h == "null") || (h == ""))
                {
                    h = "0";
                }

                if ((i == "null") || (i == ""))
                {
                    i = "0";
                }
                double interm1 = Convert.ToDouble(h, new CultureInfo("en-US"));
                double interm2 = Convert.ToDouble(i, new CultureInfo("en-US"));
                double val = interm1 + interm2;
                tabDiff[6] = currOr1;
                tabDiff[7] = val.ToString();
            }
            ViewBag.Diff = tabDiff;
            return PartialView(ds);
        }

        public DataSet RfndDetails(string docNum)
        {
            string DocNum = docNum;
            string sql = " SELECT  " + Environment.NewLine;
            sql = sql + " FORMAT(sdh.[DateofIssue], 'd/M/yyyy') as DateofIssue ,sdh.[TransactionGroup] ,sdh.[AmountCollectedCurrency],sdh.[AmountCollected]  " + Environment.NewLine;
            sql = sql + " ,sdh.[TaxCollectedCurrency]," + Environment.NewLine;
            sql = sql + " (case when (sdh.[TaxCollectedCurrency] is not null) then sum(sda.otheramount) else 0 END) as TaxCollected" + Environment.NewLine;
            sql = sql + ",sdh.[SurchargeCollectedCurrency]  " + Environment.NewLine;
            sql = sql + " ,sdh.[SurchargeCollected],sdh.[CommissionCollectedCurrency],sdh.[CommissionCollected],sdh.[AgentNumericCode], sdh.[EndosRestriction]  " + Environment.NewLine;
            sql = sql + " FROM [Pax].[SalesDocumentHeader] sdh   " + Environment.NewLine;
            sql = sql + " left Join Pax.SalesRelatedDocumentInformation srd on sdh.HdrGuid = srd.HdrGuid  " + Environment.NewLine;
            sql = sql + " left join pax.SalesDocumentOtherAmount sda on srd.RelatedDocumentGuid = sda.RelatedDocumentGuid  " + Environment.NewLine;
            sql = sql + " WHERE sda.OtherAmountCode<> 'CP' and sdh.[DocumentNumber]  = '" + DocNum + "'  " + Environment.NewLine;
            sql = sql + " AND sdh.[Transactioncode] = 'RFND'" + Environment.NewLine;
            sql = sql + " group by sdh.[DateofIssue] ,sdh.[TransactionGroup] ,sdh.[AmountCollectedCurrency],sdh.[AmountCollected]  " + Environment.NewLine;
            sql = sql + " ,sdh.[TaxCollectedCurrency],sdh.[SurchargeCollectedCurrency]  " + Environment.NewLine;
            sql = sql + " ,sdh.[SurchargeCollected],sdh.[CommissionCollectedCurrency],sdh.[CommissionCollected],sdh.[AgentNumericCode],sdh.[EndosRestriction]  " + Environment.NewLine;
            DataSet ds = new DataSet();
            SqlConnection con = new SqlConnection(pbConnectionString);
            SqlDataAdapter ada = new SqlDataAdapter(sql, con);
            ada.Fill(ds);
            con.Close();
            return ds;
        }

        public string[] LoadFareCalc(string docNum)
        {
            string doc = docNum;
            string sql = "SELECT [DocumentNumber],[DateofIssue],[PassengerName],[DocumentType]";
            sql = sql + ",[FareCalculationArea],[TransactionCode]";
            sql = sql + " FROM [Pax].[SalesDocumentHeader]";
            sql = sql + "WHERE [DocumentNumber] = '" + doc + "' AND FareCalculationArea <>''";
            DataSet ds = new DataSet();
            SqlConnection con = new SqlConnection(pbConnectionString);
            SqlDataAdapter ada = new SqlDataAdapter(sql, con);
            ada.Fill(ds);
            con.Close();
            string[] tabfare = new string[2];

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                tabfare[0] = dr[ds.Tables[0].Columns[4].ColumnName].ToString();
                tabfare[1] = dr[ds.Tables[0].Columns[5].ColumnName].ToString();
            }
            return tabfare;

        }

        public ActionResult RfndCoupons()
        {
            string DocNum = Request["DocNum"];
            string sql = "SELECT [CouponNumber],[OriginAirportCityCode],[DestinationAirportCityCode] ,[Carrier]" + Environment.NewLine;
            sql += ",[FareBasisTicketDesignator],[FlightNumber],FORMAT([FlightDepartureDate],'d/M/yyyy') as FlightDepartureDate ,[FlightDepartureTime]" + Environment.NewLine;
            sql += ",FORMAT([NotValidBefore],'d/M/yyyy') as NotValidBefore ,FORMAT([NotValidAfter],'d/M/y') as NotValidAfter,[ReservationBookingDesignator] ,[FreeBaggageAllowance]" + Environment.NewLine;
            sql += ",[FlightBookingStatus],[UsageAirline],FORMAT([UsageDate], 'd/M/yyyy')as UsageDate,[UsageFlightNumber],[UsageOriginCode]" + Environment.NewLine;
            sql += ",[UsageDestinationCode],[UsedClassofService],[CouponStatus],[DomesticInternational]" + Environment.NewLine;
            sql += ",[OriginCity],[DestinationCity]" + Environment.NewLine;
            sql += "FROM [Pax].[SalesDocumentCoupon]" + Environment.NewLine;
            sql += "where CouponStatus = 'R' and DocumentNumber = '" + DocNum + "'";
            DataSet ds = new DataSet();
            SqlConnection con = new SqlConnection(pbConnectionString);
            SqlDataAdapter ada = new SqlDataAdapter(sql, con);
            ada.Fill(ds);
            con.Close();
            List<string> colName = new List<string>();
            foreach (DataColumn column in ds.Tables[0].Columns)
            {
                colName.Add(AddSpacesToSentence(column.ColumnName, true));
            }
            ViewBag.colName = colName;
            return PartialView(ds);
        }

        public ActionResult OALRefundedCouponsBilling()
        {
            ViewBag.dateFrom = DateTime.Now.AddMonths(-1).ToString("dd-MMM-yyyy");
            ViewBag.dateTo = DateTime.Now.ToString("dd-MMM-yyyy");
            return PartialView();
        }

        public ActionResult LoadOalRefund()
        {
            string dateFrom1 = Request["dateFrom"];
            string dateFrom = ConvertDate(dateFrom1);
            string dateTo1 = Request["dateTo"];
            string dateTo = ConvertDate(dateTo1);
            string _OWN = "N";
            SqlConnection con = new SqlConnection(pbConnectionString);
            SqlCommand cmd = new SqlCommand("[Pax].[SP_RefundOAL]", con);

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 900;
            cmd.Parameters.AddWithValue("@DateFrom", dateFrom);
            cmd.Parameters.AddWithValue("@DateTo", dateTo);
            cmd.Parameters.AddWithValue("@Own", _OWN);
            DataSet ds = new DataSet();
            SqlDataAdapter ada = new SqlDataAdapter(cmd);
            ada.Fill(ds);
            int lon = ds.Tables[0].Rows.Count;
            con.Close();
            ViewBag.compt = lon;
            String[,] tableau = new String[10, lon];
            int i = 0;
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                string test = dr[ds.Tables[0].Columns[0].ColumnName].ToString();
                //string date = Convert.ToDateTime(test).ToString("yyyyMM");
                string date1 = Convert.ToDateTime(test).ToString("dd-MMM-yyyy");
                tableau[0, i] = date1;
                tableau[1, i] = dr[ds.Tables[0].Columns[1].ColumnName].ToString();
                tableau[2, i] = dr[ds.Tables[0].Columns[2].ColumnName].ToString();
                tableau[3, i] = dr[ds.Tables[0].Columns[3].ColumnName].ToString();
                string test2 = dr[ds.Tables[0].Columns[3].ColumnName].ToString();
                tableau[4, i] = Convert.ToDateTime(test2).ToString("dd-MMM-yyyy");
                tableau[5, i] = dr[ds.Tables[0].Columns[5].ColumnName].ToString();
                tableau[6, i] = dr[ds.Tables[0].Columns[6].ColumnName].ToString();
                tableau[7, i] = dr[ds.Tables[0].Columns[7].ColumnName].ToString();
                tableau[8, i] = dr[ds.Tables[0].Columns[5].ColumnName].ToString();
                tableau[9, i] = dr[ds.Tables[0].Columns[6].ColumnName].ToString();
                i = i + 1;

            }
            ViewBag.tableau = tableau;
            string[] cll = new string[lon];

            for (int g = 0; g < lon; g++)
            {
                cll[g] = tableau[8, g].ToString();
            }

            string[] b = cll.Distinct().ToArray();
            ViewBag.comp1 = b.Length;
            string[,] tableausum = new string[3, b.Length];
            for (int y = 0; y < b.Length; y++)
            {
                //dgsum.Rows.Add("");
                tableausum[0, y] = b[y].ToString();
            }

            string[] sum = new string[b.Length];
            string[] cnt = new string[b.Length];
            var culture = System.Globalization.CultureInfo.CurrentCulture;
            for (int o = 0; o < b.Length; o++)
            {
                for (int k = 0; k < lon; k++)
                {
                    if (tableausum[0, o].ToString() == tableau[8, k].ToString())
                    {
                        if (sum[o] == null)
                        {
                            sum[o] = "0";
                        }
                        sum[o] = (Convert.ToDecimal(sum[o].ToString(), new CultureInfo(culture.Name)) + Convert.ToDecimal(tableau[7, k].ToString(), new CultureInfo(culture.Name))).ToString();

                        if (cnt[o] == null)
                        {
                            cnt[o] = "0";
                        }

                        cnt[o] = (Convert.ToDecimal(cnt[o].ToString(), new CultureInfo(culture.Name)) + Convert.ToDecimal(tableau[9, k].ToString(), new CultureInfo(culture.Name))).ToString();
                    }
                }
            }

            for (int yy = 0; yy < b.Length; yy++)
            {
                tableausum[1, yy] = sum[yy].ToString();
                tableausum[2, yy] = cnt[yy].ToString();
            }



            ViewBag.tableausum = tableausum;

            return PartialView(ds);
        }

        /********************************Refund-Own************************************/

        public ActionResult RefundOwn()
        {
            ViewBag.dateFrom = DateTime.Now.AddMonths(-1).ToString("dd-MMM-yyyy");
            ViewBag.dateTo = DateTime.Now.ToString("dd-MMM-yyyy");
            return PartialView();
        }

        public ActionResult LoadOwnRefund()
        {
            string dateFrom1 = Request["dateFrom"];
            string dateFrom = ConvertDate(dateFrom1);
            string dateTo1 = Request["dateTo"];
            string dateTo = ConvertDate(dateTo1);
            SqlConnection con = new SqlConnection(pbConnectionString);
            SqlCommand cmd = new SqlCommand("[Pax].[SP_RefundOAL]", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 1000;
            cmd.Parameters.AddWithValue("@DateFrom", dateFrom);
            cmd.Parameters.AddWithValue("@DateTo", dateTo);
            cmd.Parameters.AddWithValue("@Own", "Y");
            DataSet ds = new DataSet();
            SqlDataAdapter ada = new SqlDataAdapter(cmd);
            ada.Fill(ds);
            con.Close();
            List<string> colName = new List<string>();
            foreach (DataColumn column in ds.Tables[0].Columns)
            {
                colName.Add(AddSpacesToSentence(column.ColumnName, true));
            }
            ViewBag.colName = colName;
            return PartialView(ds);
        }
        /************************************end**************************************/
        /*********************************Refund Engine******************************/

        public ActionResult FrmRefundEngine()
        {
            return PartialView();
        }

        public ActionResult LoadRefundEngine()
        {
            string passengerName = Request["pasengerName"];
            string strPassengerName = "";
            string cmd = "";
            string docNum = Request["docNum"];
            string valrad = Request["radContains"];
            if (passengerName != "")
            {
                docNum = "";
                if (valrad == "starting")
                {
                    strPassengerName = " PassengerName LIKE '" + passengerName + "%'";
                }
                else
                    if (valrad == "contains")
                {
                    strPassengerName = " PassengerName LIKE '%" + passengerName + "%'";
                }
                cmd = "SELECT [DocumentNumber],[PassengerName],[DocumentType],[DateofIssue],[SaleDate],[AgentNumericCode],[FareCalculationArea] FROM [Pax].[SalesDocumentHeader] where " + strPassengerName + " AND FareCalculationArea <> ''";

            }
            else
                if ((docNum != "") && (docNum.Length == 13))
            {
                cmd = "SELECT [DocumentNumber],[PassengerName],[DocumentType],[DateofIssue],[SaleDate],[AgentNumericCode],[FareCalculationArea] FROM [Pax].[SalesDocumentHeader] where DocumentNumber = '" + docNum + "' AND FareCalculationArea <> ''";

            }
            SqlConnection con = new SqlConnection(pbConnectionString);
            DataSet ds = new DataSet();
            SqlDataAdapter ada = new SqlDataAdapter(cmd, con);
            ada.Fill(ds);

            int lonRefun = ds.Tables[0].Rows.Count;

            ViewBag.nbRefun = lonRefun;

            con.Close();
            return PartialView(ds);
        }

        /* List Of Manual Refund Update Joseph*/
        public ActionResult manualList()
        {
            ViewBag.dateFrom = DateTime.Now.AddMonths(-1).ToString("dd-MMM-yyyy");
            ViewBag.dateTo = DateTime.Now.ToString("dd-MMM-yyyy");
            return PartialView();
        }
        public ActionResult LoadManualRefund()
        {
            string dateFrom1 = Request["dateFrom"];
            string dateFrom = ConvertDate(dateFrom1);
            string dateTo1 = Request["dateTo"];
            string dateTo = ConvertDate(dateTo1);
            string sl = "WHERE [EntryDate] between cast('" + dateFrom + "' as date) and cast('" + dateTo + "' as date)";
            string sql = "SELECT [EntryDate],[UserID],[UserIP],[RefundType],[Currency]" + Environment.NewLine;
            sql += ",[FareAmount],[FareRefunded],[FareRemain],[TaxAmount],[TaxRefunded],[TaxRemain],[Taxes] as TaxDue " + Environment.NewLine;
            sql += ",[RefundedAmount],[ReasonForRefund],[FormOfPayment]" + Environment.NewLine;
            sql += "FROM [Pax].[ManualRefund]" + Environment.NewLine;
            sql += sl;
            SqlConnection con = new SqlConnection(pbConnectionString);
            DataSet ds = new DataSet();
            SqlDataAdapter ada = new SqlDataAdapter(sql, con);
            ada.Fill(ds);

            int nbManualRef = ds.Tables[0].Rows.Count;

            con.Close();
            List<string> colName = new List<string>();
            foreach (DataColumn column in ds.Tables[0].Columns)
            {
                colName.Add(AddSpacesToSentence(column.ColumnName, true));
            }
            ViewBag.colName = colName;

            ViewBag.nbManualRef = nbManualRef;

            return PartialView(ds);
        }

        /* End  List Of Manual Refund*/


        public ActionResult RfnEngine()
        {
            var culture = System.Globalization.CultureInfo.CurrentCulture;
            string docNum = Request["docNum"];
            string sql = "select CouponNumber, StopOverCode,Concat(OriginAirportCityCode, '-', DestinationAirportCityCode), Carrier" + Environment.NewLine;
            sql += ", FlightNumber, ReservationBookingDesignator, FlightDepartureDate, FlightDepartureTime, CouponStatus" + Environment.NewLine;
            sql += ", FareBasisTicketDesignator, NotValidBefore, NotValidAfter, FreeBaggageAllowance,FlightBookingStatus" + Environment.NewLine;
            sql += ", UsedClassofService, Concat(UsageOriginCode,'-', UsageDestinationCode), UsageAirline, UsageFlightNumber" + Environment.NewLine;
            sql += ", UsageDate,FrequentFlyerReference,[RelatedDocumentGuid]" + Environment.NewLine;
            sql += " from pax.SalesDocumentCoupon" + Environment.NewLine;
            sql += " where DocumentNumber = '" + docNum + "'" + Environment.NewLine;
            sql += " order by DocumentNumber, CouponNumber" + Environment.NewLine;
            SqlConnection con = new SqlConnection(pbConnectionString);
            DataSet ds = new DataSet();
            SqlDataAdapter ada = new SqlDataAdapter(sql, con);
            ada.Fill(ds);
            con.Close();
            int i = 0;

            int lon = ds.Tables[0].Rows.Count;
            string[,] dbgCPNlist = new string[21, lon];
            if (lon != 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    dbgCPNlist[0, i] = dr[ds.Tables[0].Columns[0].ColumnName].ToString();
                    dbgCPNlist[1, i] = dr[ds.Tables[0].Columns[1].ColumnName].ToString();
                    string a = dr[ds.Tables[0].Columns[2].ColumnName].ToString().Trim();
                    if (a != "-")
                    {
                        dbgCPNlist[2, i] = dr[ds.Tables[0].Columns[2].ColumnName].ToString();
                    }
                    dbgCPNlist[3, i] = dr[ds.Tables[0].Columns[3].ColumnName].ToString();
                    dbgCPNlist[4, i] = dr[ds.Tables[0].Columns[4].ColumnName].ToString();
                    dbgCPNlist[5, i] = dr[ds.Tables[0].Columns[5].ColumnName].ToString();
                    try
                    {
                        dbgCPNlist[6, i] = Convert.ToDateTime(dr[ds.Tables[0].Columns[6].ColumnName].ToString()).ToString("dd-MMM-yyyy");
                    }
                    catch
                    {

                    }
                    dbgCPNlist[7, i] = dr[ds.Tables[0].Columns[7].ColumnName].ToString();
                    dbgCPNlist[8, i] = dr[ds.Tables[0].Columns[8].ColumnName].ToString();
                    dbgCPNlist[9, i] = dr[ds.Tables[0].Columns[9].ColumnName].ToString();
                    try
                    {
                        dbgCPNlist[10, i] = Convert.ToDateTime(dr[ds.Tables[0].Columns[10].ColumnName].ToString()).ToString("dd-MMM-yyyy");
                    }
                    catch
                    {

                    }
                    try
                    {
                        dbgCPNlist[11, i] = Convert.ToDateTime(dr[ds.Tables[0].Columns[11].ColumnName].ToString()).ToString("dd-MMM-yyyy");
                    }
                    catch
                    {

                    }
                    dbgCPNlist[12, i] = dr[ds.Tables[0].Columns[12].ColumnName].ToString();
                    dbgCPNlist[13, i] = dr[ds.Tables[0].Columns[13].ColumnName].ToString();
                    dbgCPNlist[14, i] = dr[ds.Tables[0].Columns[14].ColumnName].ToString();
                    dbgCPNlist[15, i] = dr[ds.Tables[0].Columns[15].ColumnName].ToString();
                    dbgCPNlist[16, i] = dr[ds.Tables[0].Columns[16].ColumnName].ToString();
                    dbgCPNlist[17, i] = dr[ds.Tables[0].Columns[17].ColumnName].ToString();
                    try
                    {
                        dbgCPNlist[18, i] = Convert.ToDateTime(dr[ds.Tables[0].Columns[18].ColumnName].ToString()).ToString("dd-MMM-yyyy");
                    }
                    catch
                    {

                    }
                    dbgCPNlist[19, i] = dr[ds.Tables[0].Columns[19].ColumnName].ToString();
                    dbgCPNlist[20, i] = dr[ds.Tables[0].Columns[20].ColumnName].ToString();
                    i = i + 1;
                }
            }
            double amt = 0.00;
            double totalTax = 0;
            string sql1 = "SELECT DocumentAmountType, CurrencyType, OtherAmountCode, OtherAmount,SequenceNumber,Filesequence" + Environment.NewLine;
            sql1 += "FROM [Pax].[SalesDocumentOtherAmount]" + Environment.NewLine;
            sql1 += "WHERE DocumentNumber = '" + docNum + "' AND TransactionCode <> 'RFND'" + Environment.NewLine;
            DataSet ds1 = new DataSet();
            SqlDataAdapter ada1 = new SqlDataAdapter(sql1, con);
            ada1.Fill(ds1);
            con.Close();
            int lonOther = ds1.Tables[0].Rows.Count;
            string[,] other = new string[7, lonOther];
            int j = 0;
            foreach (DataRow dr in ds1.Tables[0].Rows)
            {
                other[0, j] = dr[ds1.Tables[0].Columns[0].ColumnName].ToString();
                other[1, j] = dr[ds1.Tables[0].Columns[1].ColumnName].ToString();
                other[2, j] = dr[ds1.Tables[0].Columns[2].ColumnName].ToString();
                other[3, j] = dr[ds1.Tables[0].Columns[3].ColumnName].ToString();
                other[5, j] = dr[ds1.Tables[0].Columns[4].ColumnName].ToString();
                other[6, j] = dr[ds1.Tables[0].Columns[5].ColumnName].ToString();
                totalTax += Convert.ToDouble(dr[ds1.Tables[0].Columns[3].ColumnName].ToString(), new CultureInfo(culture.Name));
                j = j + 1;
            }
            string sql2 = "SELECT DocumentAmountType, CurrencyType, OtherAmountCode, OtherAmount,SequenceNumber,Filesequence" + Environment.NewLine;
            sql2 += "FROM [Pax].[SalesDocumentOtherAmount]" + Environment.NewLine;
            sql2 += "WHERE DocumentNumber = '" + docNum + "' AND TransactionCode = 'RFND'" + Environment.NewLine;
            //SqlConnection con = new SqlConnection(pbConnectionString);
            DataSet ds2 = new DataSet();
            SqlDataAdapter ada2 = new SqlDataAdapter(sql2, con);
            ada2.Fill(ds2);
            con.Close();

            double an = 0;
            string TaxRefund;

            if (ds2.Tables[0].Rows.Count == 0)
            {
                int k = 0;
                foreach (DataRow dr in ds1.Tables[0].Rows)
                {
                    other[4, k] = "false";
                    k = k + 1;
                }
            }
            else
            {

                foreach (DataRow dr in ds2.Tables[0].Rows)
                {

                    string a1 = dr[ds2.Tables[0].Columns[0].ColumnName].ToString();
                    string b1 = dr[ds2.Tables[0].Columns[2].ColumnName].ToString();
                    string c1 = (Convert.ToDouble(dr[ds2.Tables[0].Columns[3].ColumnName].ToString()) * (-1)).ToString("0.00");
                    for (int n = 0; n < lon; n++)
                    {
                        string a = other[0, n].ToString();
                        string b = other[2, n].ToString();
                        string c = other[3, n].ToString();
                        if (a1 == a && b1 == b && c1 == c)
                        {
                            amt = Convert.ToDouble(other[3, n], new CultureInfo(culture.Name));
                            other[4, n] = "true";
                            an += amt;
                        }
                        else
                        {
                            other[4, n] = "false";
                        }
                    }


                }
                //TaxRefund = amt.ToString("0.00");
            }
            double totalB = 0;
            double TaxRemain = totalTax - amt;
            ViewBag.TotalTax = totalTax.ToString("0.00");
            ViewBag.TaxRefund = amt.ToString("0.00");
            ViewBag.TaxRemain = TaxRemain.ToString("0.00");
            ViewBag.count = lonOther;
            ViewBag.Tableau = dbgCPNlist;
            ViewBag.compt = lon;
            // ViewBag.Info = OriginalInformation(docNum);

            //ViewBag.other = otherTaxe(docNum);
            ViewBag.other = other;
            ViewBag.refund = refundHeader(docNum);
            string hdrGuid = dbgCPNlist[20, 0];
            string sql3 = "SELECT [RefundType],[RefundFareEqui],[Currency]" + Environment.NewLine;
            sql3 += ",[FareRefunded],[FareRemain] ,[TaxAmount] ,[TaxRefunded] ,[TaxRemain]" + Environment.NewLine;
            sql3 += ",[RefundedAmount] ,[ReasonForRefund] ,[FormOfPayment], [Taxes]" + Environment.NewLine;
            sql3 += "FROM [Pax].[ManualRefund]" + Environment.NewLine;
            sql3 += "WHERE [HdrGuid] = '" + hdrGuid + "'";
            DataSet ds3 = new DataSet();
            SqlDataAdapter ada3 = new SqlDataAdapter(sql3, con);
            ada3.Fill(ds3);
            con.Close();
            string[] mainRef = new string[12];
            string taxE = "";
            string taxD = "";
            double totBE = 0;
            double rbt = 0;
            if (ds3.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds3.Tables[0].Rows)
                {
                    for (int x = 0; x < 12; x++)
                    {
                        mainRef[x] = dr[ds3.Tables[0].Columns[x].ColumnName].ToString();


                    }
                    totalB = TaxRemain + Convert.ToDouble(mainRef[4], new CultureInfo(culture.Name));

                }
            }
            else
            {
                for (int x = 0; x < 12; x++)
                {
                    mainRef[x] = "";

                }
                totalB = TaxRemain;
            }

            if (mainRef[0] == "VOLUNTARY")
            {
                taxE = mainRef[11];

            }
            else if (mainRef[0] == "INVOLUNTARY")
            {
                taxD = mainRef[11];

            }
            if (taxE == "")
            {
                taxE = "0";
                totBE = totalB + Convert.ToDouble(taxE, new CultureInfo(culture.Name));
            }
            else
            {
                totBE = totalB + Convert.ToDouble(taxE, new CultureInfo(culture.Name));
            }
            ViewBag.TotalB = totalB.ToString("0.00");
            ViewBag.mainRef = mainRef;
            ViewBag.TaxE = taxE;

            ViewBag.TotBE = totBE;
            string sql4 = "SELECT [Observation]" + Environment.NewLine;
            sql4 += "FROM [Pax].[ManualRefund]" + Environment.NewLine;
            sql4 += "where hdrguid = '" + hdrGuid + "'" + Environment.NewLine;
            DataSet ds4 = new DataSet();
            SqlDataAdapter ada4 = new SqlDataAdapter(sql4, con);
            ada4.Fill(ds4);
            con.Close();
            string observation = "";
            foreach (DataRow dr in ds3.Tables[0].Rows)
            {
                observation = dr[ds4.Tables[0].Columns[0].ColumnName].ToString();
            }
            byte[] bt = new byte[100];
            // bt = ds4.ExecuteScalar() as byte[]
            ViewBag.observation = observation;
            string sql5 = "SELECT DocumentNumber, TrueOriginDestinationCityCodes, OriginalIssueDocumentNumber, TransactionCode" + Environment.NewLine;
            sql5 += ",FORMAT(DateofIssue, 'dd-MMM-yyyy') ,AgentNumericCode, DocumentType,PassengerName, EndosRestriction, FareCalculationArea" + Environment.NewLine;
            sql5 += ",FareCurrency ,Fare, TotalCurrency, EquivalentFare, AmountCollectedCurrency, AmountCollected,[HdrGuid],[CheckDigit]" + Environment.NewLine;
            sql5 += "FROM [Pax].[SalesDocumentHeader]" + Environment.NewLine;
            sql5 += "WHERE DocumentNumber = '" + docNum + "' AND TransactionCode <> 'RFND'" + Environment.NewLine;
            //SqlConnection con = new SqlConnection(pbConnectionString);
            DataSet ds5 = new DataSet();
            SqlDataAdapter ada5 = new SqlDataAdapter(sql5, con);
            ada5.Fill(ds5);
            con.Close();
            string[] tabInfo = new string[18];
            //int i = 0;
            foreach (DataRow dr in ds5.Tables[0].Rows)
            {
                for (int n = 0; n < 18; n++)
                {
                    tabInfo[n] = dr[ds5.Tables[0].Columns[n].ColumnName].ToString();

                }

            }

            double difAB = Convert.ToDouble(tabInfo[15], new CultureInfo(culture.Name)) - totalB;
            double compar1 = 0;
            double compar2 = 0;
            double destVal = 0;
            rbt = Convert.ToDouble(tabInfo[15], new CultureInfo(culture.Name)) - totBE;
            if (taxD == "")
            {
                taxD = "0";
                compar1 = totalB + Convert.ToDouble(taxD, new CultureInfo(culture.Name));
                compar2 = Convert.ToDouble(taxD, new CultureInfo(culture.Name)) + difAB;
                if (compar1 > compar2)
                {
                    destVal = compar1;
                }
                else
                {
                    destVal = compar2;
                }
            }
            else
            {
                compar1 = totalB + Convert.ToDouble(taxD, new CultureInfo(culture.Name));
                compar2 = Convert.ToDouble(taxD, new CultureInfo(culture.Name)) + difAB;
                if (compar1 > compar2)
                {
                    destVal = compar1;
                }
                else
                {
                    destVal = compar2;
                }
            }
            ViewBag.DifAB = difAB.ToString("0.00");
            ViewBag.Rbt = rbt;
            ViewBag.DestVal = destVal;
            ViewBag.Info = tabInfo;
            ViewBag.TaxD = taxD;
            return PartialView();

        }

        public string[] OriginalInformation(string docNum)
        {
            string sql = "SELECT DocumentNumber, TrueOriginDestinationCityCodes, OriginalIssueDocumentNumber, TransactionCode" + Environment.NewLine;
            sql += ",FORMAT(DateofIssue, 'dd-MMM-yyyy') ,AgentNumericCode, DocumentType,PassengerName, EndosRestriction, FareCalculationArea" + Environment.NewLine;
            sql += ",FareCurrency ,Fare, TotalCurrency, EquivalentFare, AmountCollectedCurrency, AmountCollected,[HdrGuid],[CheckDigit]" + Environment.NewLine;
            sql += "FROM [Pax].[SalesDocumentHeader]" + Environment.NewLine;
            sql += "WHERE DocumentNumber = '" + docNum + "' AND TransactionCode <> 'RFND'" + Environment.NewLine;
            SqlConnection con = new SqlConnection(pbConnectionString);
            DataSet ds = new DataSet();
            SqlDataAdapter ada = new SqlDataAdapter(sql, con);
            ada.Fill(ds);
            con.Close();
            string[] tabInfo = new string[17];
            //int i = 0;
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                for (int i = 0; i < 17; i++)
                {
                    tabInfo[i] = dr[ds.Tables[0].Columns[i].ColumnName].ToString();

                }

            }
            return tabInfo;
        }
        public String saveRfn(string[] tabother)
        {
            string docNum = Request["docNum"];
            string hdrGuid = Request["hdrGuid"];
            string checkDigit = Request["checkDigit"];
            string rfnType = Request["rfnType"];
            string fareAmount = Request["fareAmount"];
            string fareCurr = Request["fareCurr"];
            string fareType = Request["fareType"];
            string curRfn = Request["curRfn"];
            string totRfnAmt = Request["totRfnAmt"];
            string fareTypeCur = Request["fareTypeCur"];
            string eqFareTypeCur = Request["esFareTypeCur"];
            string fareTypeAmt = Request["fareTypeAmt"];
            string eqFareTypeAmt = Request["eqFareTypeAmt"];
            string fareRemain = Request["fareRemain"];
            string totalTaxe = Request["totalTaxe"];
            string totalRfn = Request["totalRfn"];
            string taxeRemain = Request["taxeRemain"];
            string observation = Request["observation"];
            string volReason = Request["volReason"];
            string InvReason = Request["InvReason"];
            string refundBy = Request["refundBy"];
            string totalD = Request["totalD"];
            string taxe = Request["taxe"];
            string dateOfRfn = Request["dateOfRfn"];
            string msg = "";

            var tabotherRes = tabother;
            int compt = tabotherRes.Count();
            string[,] dgother = new string[6, compt];
            for (int ix = 0; ix < tabotherRes.Count(); ix++)
            {
                string[] tabInterm = new string[6];
                tabInterm = tabotherRes[ix].Split(',');
                for (int j = 0; j < 6; j++)
                {
                    dgother[j, ix] = tabInterm[j];
                }
            }
            string sql = "select CouponNumber, StopOverCode,Concat(OriginAirportCityCode, '-', DestinationAirportCityCode), Carrier" + Environment.NewLine;
            sql += ", FlightNumber, ReservationBookingDesignator, FlightDepartureDate, FlightDepartureTime, CouponStatus" + Environment.NewLine;
            sql += ", FareBasisTicketDesignator, NotValidBefore, NotValidAfter, FreeBaggageAllowance,FlightBookingStatus" + Environment.NewLine;
            sql += ", UsedClassofService, Concat(UsageOriginCode,'-', UsageDestinationCode), UsageAirline, UsageFlightNumber" + Environment.NewLine;
            sql += ", UsageDate,FrequentFlyerReference,[RelatedDocumentGuid]" + Environment.NewLine;
            sql += " from pax.SalesDocumentCoupon" + Environment.NewLine;
            sql += " where DocumentNumber = '" + docNum + "'" + Environment.NewLine;
            sql += " order by DocumentNumber, CouponNumber" + Environment.NewLine;
            SqlConnection con = new SqlConnection(pbConnectionString);
            DataSet ds = new DataSet();
            SqlDataAdapter ada = new SqlDataAdapter(sql, con);
            ada.Fill(ds);
            con.Close();
            int iy = 0;

            int lon = ds.Tables[0].Rows.Count;
            string[,] dbgCPNlist = new string[21, lon];
            if (lon != 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    dbgCPNlist[0, iy] = dr[ds.Tables[0].Columns[0].ColumnName].ToString();
                    dbgCPNlist[1, iy] = dr[ds.Tables[0].Columns[1].ColumnName].ToString();
                    string a = dr[ds.Tables[0].Columns[2].ColumnName].ToString().Trim();
                    if (a != "-")
                    {
                        dbgCPNlist[2, iy] = dr[ds.Tables[0].Columns[2].ColumnName].ToString();
                    }
                    dbgCPNlist[3, iy] = dr[ds.Tables[0].Columns[3].ColumnName].ToString();
                    dbgCPNlist[4, iy] = dr[ds.Tables[0].Columns[4].ColumnName].ToString();
                    dbgCPNlist[5, iy] = dr[ds.Tables[0].Columns[5].ColumnName].ToString();
                    try
                    {
                        dbgCPNlist[6, iy] = Convert.ToDateTime(dr[ds.Tables[0].Columns[6].ColumnName].ToString()).ToString("dd-MMM-yyyy");
                    }
                    catch
                    {

                    }
                    dbgCPNlist[7, iy] = dr[ds.Tables[0].Columns[7].ColumnName].ToString();
                    dbgCPNlist[8, iy] = dr[ds.Tables[0].Columns[8].ColumnName].ToString();
                    dbgCPNlist[9, iy] = dr[ds.Tables[0].Columns[9].ColumnName].ToString();
                    try
                    {
                        dbgCPNlist[10, iy] = Convert.ToDateTime(dr[ds.Tables[0].Columns[10].ColumnName].ToString()).ToString("dd-MMM-yyyy");
                    }
                    catch
                    {

                    }
                    try
                    {
                        dbgCPNlist[11, iy] = Convert.ToDateTime(dr[ds.Tables[0].Columns[11].ColumnName].ToString()).ToString("dd-MMM-yyyy");
                    }
                    catch
                    {

                    }
                    dbgCPNlist[12, iy] = dr[ds.Tables[0].Columns[12].ColumnName].ToString();
                    dbgCPNlist[13, iy] = dr[ds.Tables[0].Columns[13].ColumnName].ToString();
                    dbgCPNlist[14, iy] = dr[ds.Tables[0].Columns[14].ColumnName].ToString();
                    dbgCPNlist[15, iy] = dr[ds.Tables[0].Columns[15].ColumnName].ToString();
                    dbgCPNlist[16, iy] = dr[ds.Tables[0].Columns[16].ColumnName].ToString();
                    dbgCPNlist[17, iy] = dr[ds.Tables[0].Columns[17].ColumnName].ToString();
                    try
                    {
                        dbgCPNlist[18, iy] = Convert.ToDateTime(dr[ds.Tables[0].Columns[18].ColumnName].ToString()).ToString("dd-MMM-yyyy");
                    }
                    catch
                    {

                    }
                    dbgCPNlist[19, iy] = dr[ds.Tables[0].Columns[19].ColumnName].ToString();
                    dbgCPNlist[20, iy] = dr[ds.Tables[0].Columns[20].ColumnName].ToString();
                    iy = iy + 1;
                }
            }
            string sqla = " DELETE FROM [Pax].SalesDocumentPayment where DocumentNumber = '" + docNum + "' and TransactionCode = 'RFND' " + Environment.NewLine;
            sqla += "DELETE FROM [Pax].SalesDocumentOtherAmount where DocumentNumber = '" + docNum + "' and TransactionCode = 'RFND' " + Environment.NewLine;
            sqla += "DELETE FROM [Pax].SalesRelatedDocumentInformation where DocumentNumber = '" + docNum + "' and TransactionCode = 'RFND' " + Environment.NewLine;
            sqla += "DELETE FROM [Pax].[SalesDocumentHeader] where DocumentNumber = '" + docNum + "' and TransactionCode = 'RFND' " + Environment.NewLine;
            sqla += "DELETE FROM [Pax].[ManualRefund] WHERE HdrGuid = '" + hdrGuid + "'" + Environment.NewLine;
            SqlConnection cs = new SqlConnection(pbConnectionString);
            cs.Open();
            SqlCommand cmdsa = new SqlCommand(sqla, cs);
            cmdsa.CommandType = CommandType.Text;
            cmdsa.CommandText = sqla;
            cmdsa.Connection = cs;
            cmdsa.ExecuteNonQuery();

            cs.Close();
            try
            {
                Guid HdrGuid = Guid.NewGuid();
                SaveMain(hdrGuid, HdrGuid, rfnType, fareType, fareCurr, fareAmount, totRfnAmt, curRfn, fareTypeCur, eqFareTypeCur, fareTypeAmt, eqFareTypeAmt, fareRemain, totalTaxe, totalRfn, taxeRemain, observation, volReason, InvReason, refundBy, totalD, taxe);
                ManualHeader(HdrGuid, checkDigit, docNum);

                for (int ii = 0; ii < lon; ii++)
                {
                    string a = CheckGrid(0, ii, docNum);
                    string b = CheckGrid(1, ii, docNum);
                    string c = CheckGrid(2, ii, docNum);
                    string d = CheckGrid(3, ii, docNum);
                    string ee = CheckGrid(4, ii, docNum);
                    string f = CheckGrid(5, ii, docNum);
                    string g = CheckGrid(6, ii, docNum);
                    string h = CheckGrid(7, ii, docNum);
                    string i = CheckGrid(8, ii, docNum);
                    string j = CheckGrid(9, ii, docNum);
                    string k = CheckGrid(10, ii, docNum);
                    string l = CheckGrid(11, ii, docNum);
                    string m = CheckGrid(12, ii, docNum);
                    string n = CheckGrid(13, ii, docNum);
                    string o = CheckGrid(14, ii, docNum);
                    string p = CheckGrid(15, ii, docNum);
                    string q = CheckGrid(16, ii, docNum);
                    string r = CheckGrid(17, ii, docNum);
                    string s = CheckGrid(18, ii, docNum);
                    string t = CheckGrid(19, ii, docNum);
                    string w = CheckGrid(20, ii, docNum);
                    string u = "";

                    u = docNum;
                    Guid v = HdrGuid;
                    Guid y = new Guid(w);
                    cpnSave(a, b, c, d, ee, f, g, h, i, j, k, l, m, n, o, p, q, r, s, t, u, v, y);
                }

                ManualRelatedDocInf(HdrGuid, checkDigit, docNum);
                Saveotheramt(HdrGuid, dgother, docNum, dateOfRfn, compt);
                SavePayment(HdrGuid, dateOfRfn, totRfnAmt, curRfn, refundBy, docNum);

                string sql1 = " EXEC Pax.SP_ManualRefundUpdate  @hdrguid = '" + HdrGuid + "'";

                cs.Open();
                SqlCommand cmds = new SqlCommand(sql1, cs);
                cmds.CommandType = CommandType.Text;
                cmds.CommandText = sql;
                cmds.Connection = cs;
                cmds.ExecuteNonQuery();

                cs.Close();

                SaveCoupon(docNum, dbgCPNlist, lon);
                msg = "Records Save Successfully";
            }
            catch
            {
                msg = "Error";
            }

            return msg;

        }
        private void SaveMain(string HdrGuid, Guid RGuid, string rfnType, string fareType, string fareCurr, string fareAmount, string totRfnAmt, string curRfn, string fareTypeCur, string eqFareTypeCur, string fareTypeAmt, string eqFareTypeAmt, string fareRemain, string totalTaxe, string totalRfn, string taxeRemain, string observation, string volReason, string InvReason, string refundBy, string totalD, string taxe)
        {
            var culture = System.Globalization.CultureInfo.CurrentCulture;
            SqlConnection cs = new SqlConnection(pbConnectionString);
            SqlDataAdapter da = new SqlDataAdapter();


            string sql = "INSERT INTO [Pax].[ManualRefund] VALUES (@HdrGuid,@EntryDate,@UserID ,@UserIP,@RefundType,@RefundFareEqui,@Currency ,@FareAmount,@FareRefunded,@FareRemain ";
            sql += ",@TaxAmount ,@TaxRefunded,@TaxRemain ,@RefundedAmount,@Observation ,@ReasonForRefund ,@FormOfPayment, @RefundedGuid,@Taxes)";


            da.InsertCommand = new SqlCommand(sql, cs);
            da.InsertCommand.Parameters.Add("@HdrGuid", SqlDbType.UniqueIdentifier).Value = new Guid(HdrGuid);
            da.InsertCommand.Parameters.Add("@EntryDate", SqlDbType.Date).Value = DateTime.Now;
            da.InsertCommand.Parameters.Add("@UserID", SqlDbType.NVarChar).Value = "Username";
            da.InsertCommand.Parameters.Add("@UserIP", SqlDbType.NVarChar).Value = "IpAddress";
            da.InsertCommand.Parameters.Add("@RefundType", SqlDbType.NVarChar).Value = rfnType;

            da.InsertCommand.Parameters.Add("@RefundFareEqui", SqlDbType.NVarChar).Value = fareType;

            if (fareType == "Fare")
            {
                da.InsertCommand.Parameters.Add("@Currency", SqlDbType.NVarChar).Value = fareTypeCur;
                da.InsertCommand.Parameters.Add("@FareAmount", SqlDbType.Float).Value = valid(fareTypeAmt);
            }

            if (fareType == "Equivalent Fare")
            {
                da.InsertCommand.Parameters.Add("@Currency", SqlDbType.NVarChar).Value = eqFareTypeCur;
                da.InsertCommand.Parameters.Add("@FareAmount", SqlDbType.Float).Value = valid(eqFareTypeAmt);
            }

            da.InsertCommand.Parameters.Add("@FareRefunded", SqlDbType.Float).Value = valid(fareAmount);
            da.InsertCommand.Parameters.Add("@FareRemain", SqlDbType.Float).Value = valid(fareRemain);
            da.InsertCommand.Parameters.Add("@TaxAmount", SqlDbType.Float).Value = valid(totalTaxe);
            da.InsertCommand.Parameters.Add("@TaxRefunded", SqlDbType.Float).Value = valid(totalRfn);
            da.InsertCommand.Parameters.Add("@TaxRemain", SqlDbType.Float).Value = valid(taxeRemain);

            if (rfnType == "VOLUNTARY")
            {
                da.InsertCommand.Parameters.Add("@RefundedAmount", SqlDbType.Float).Value = valid(totRfnAmt);
            }

            if (rfnType == "INVOLUNTARY")
            {
                da.InsertCommand.Parameters.Add("@RefundedAmount", SqlDbType.Float).Value = valid(totRfnAmt);
            }

            da.InsertCommand.Parameters.Add("@Observation", SqlDbType.VarBinary).Value = observation;
            if (rfnType == "VOLUNTARY")
            {
                da.InsertCommand.Parameters.Add("@ReasonForRefund", SqlDbType.NVarChar).Value = volReason;
            }

            if (rfnType == "INVOLUNTARY")
            {
                da.InsertCommand.Parameters.Add("@ReasonForRefund", SqlDbType.NVarChar).Value = InvReason;
            }
            da.InsertCommand.Parameters.Add("@FormOfPayment", SqlDbType.NVarChar).Value = refundBy;
            da.InsertCommand.Parameters.Add("@RefundedGuid", SqlDbType.UniqueIdentifier).Value = RGuid;

            if (rfnType == "INVOLUNTARY")
            {
                da.InsertCommand.Parameters.Add("@Taxes", SqlDbType.Float).Value = valid(totalD);
            }
            else
                if (rfnType == "VOLUNTARY")
            {
                da.InsertCommand.Parameters.Add("@Taxes", SqlDbType.Float).Value = valid(taxe);
            }

            if (cs.State == ConnectionState.Open)
            {
                cs.Close();
            }

            cs.Open();
            try
            {
                da.InsertCommand.ExecuteNonQuery();
            }
            catch
            {
            }
            cs.Close();
        }

        private void ManualHeader(Guid HdrGuid, string CheckDigit, string docNum)
        {
            var culture = System.Globalization.CultureInfo.CurrentCulture;
            string[] tabInfo = OriginalInformation(docNum);
            SqlConnection cs = new SqlConnection(pbConnectionString);

            SqlDataAdapter dr = new SqlDataAdapter();


            string sql = "";
            sql = sql + " INSERT INTO TMP.SalesDocumentHeader ( HdrGuid,SalesDataAvailable,AccountingStatus, ProratedFlag,FBSFlag,DocumentNumber,CheckDigit ,TrueOriginDestinationCityCodes ,FareCalculationArea ,FareCurrency ";
            sql = sql + ",Fare,EndosRestriction ,BookingReference ,PassengerName,DateofIssue ,AgentNumericCode ,BookingAgentIdentification,TourCode,FareCalculationModeIndicator   ";
            sql = sql + " ,TotalCurrency,EquivalentFare ,OriginalIssueDocumentNumber,TotalAmount,VendorIdentification   ,transactioncode,ReportingSystemIdentifier ,DataSource   )   ";
            sql = sql + " VALUES ( '" + HdrGuid + "','1','N','1','0' ";

            sql = sql + ",'" + tabInfo[0] + "'";//DocumentNumber
            sql = sql + ",'" + CheckDigit + "'"; //CheckDigit
            sql = sql + ",'" + tabInfo[1] + "' ";//TrueOriginDestinationCityCodes          
            sql = sql + ",'" + tabInfo[9] + "' ";//FareCalculationArea          
            sql = sql + ",'" + tabInfo[10] + "' ";//FareCurrency         
            sql = sql + ",'" + (Convert.ToDouble(tabInfo[11], new CultureInfo(culture.Name)) * (-1)).ToString("0.00") + "' ";//Fare          
            sql = sql + ",'" + tabInfo[8] + "' ";//EndosRestriction
            sql = sql + ",NULL ";//Bookingreference
            sql = sql + ",'" + tabInfo[7] + "' ";//PassengerName
            sql = sql + ",'" + tabInfo[4] + "' ";//DateofIssue          
            sql = sql + ",'" + tabInfo[5] + "' ";//AgentNumericCode          
            sql = sql + ",NULL ";//BookingAgentIdentification
            sql = sql + ",'' ";//TourCode        
            sql = sql + ",NULL ";//FareCalculationModeIndicator
            sql = sql + ",'" + tabInfo[14] + "' ";//TotalCurrency
            sql = sql + ",'0.00' ";//EquivalentFare          
            sql = sql + ",NULL ";//OriginalIssueDocumentNumber
            sql = sql + ",'" + tabInfo[15] + "' ";//TotalAmount
            sql = sql + ",NULL ";//VendorIdentification
            sql = sql + ",'RFND' ";//transactioncode        
            sql = sql + ",NULL, 'MANUAL') ";//ReportingSystemIdentifier


            cs.Open();


            dr.InsertCommand = new SqlCommand(sql, cs);
            dr.InsertCommand.ExecuteNonQuery();
            cs.Close();


        }

        private void cpnSave(string a, string b, string c, string d, string e, string f, string g, string h, string i, string j, string k, string l, string m, string n, string o, string p, string q, string r, string s, string t, string u, Guid v, Guid y)
        {
            SqlConnection cs = new SqlConnection(pbConnectionString);
            SqlDataAdapter da = new SqlDataAdapter();
            if (cs.State == ConnectionState.Open)
            {
                cs.Close();
            }
            cs.Close();

            da.InsertCommand = new SqlCommand("INSERT INTO [TMP].[SalesDocumentCoupon] (CouponNumber ,StopOverCode ,OriginAirportCityCode,DestinationAirportCityCode,Carrier,FlightNumber,ReservationBookingDesignator ,FlightDepartureDate ,[FlightDepartureTime],CouponStatus,FareBasisTicketDesignator,NotValidBefore,NotValidAfter,FreeBaggageAllowance,FlightBookingStatus,UsedClassofService,UsageOriginCode,UsageDestinationCode,UsageAirline,UsageFlightNumber,UsageDate,FrequentFlyerReference,DocumentNumber,HdrGuidRef, RelatedDocumentGuid)VALUES (@CouponNumber  ,@StopOverCode ,@OriginAirportCityCode,@DestinationAirportCityCode,@Carrier,@FlightNumber,@ReservationBookingDesignator ,@FlightDepartureDate ,@FlightDepartureTime,@CouponStatus,@FareBasisTicketDesignator,@NotValidBefore,@NotValidAfter,@FreeBaggageAllowance,@FlightBookingStatus,@UsedClassofService,@UsageOriginCode,@UsageDestinationCode,@UsageAirline,@UsageFlightNumber,@UsageDate,@FrequentFlyerReference,@DocumentNumber,@HdrGuidRef, @RelatedDocumentGuid)", cs);

            if (a == "") { da.InsertCommand.Parameters.Add("@CouponNumber", SqlDbType.VarChar).Value = DBNull.Value; }
            else { da.InsertCommand.Parameters.Add("@CouponNumber", SqlDbType.VarChar).Value = a; }

            if (b == "") { da.InsertCommand.Parameters.Add("@StopOverCode", SqlDbType.VarChar).Value = DBNull.Value; }
            else { da.InsertCommand.Parameters.Add("@StopOverCode", SqlDbType.VarChar).Value = b; }




            if (c.Trim() == "-")
            {
                da.InsertCommand.Parameters.Add("@OriginAirportCityCode", SqlDbType.VarChar).Value = DBNull.Value;
                da.InsertCommand.Parameters.Add("@DestinationAirportCityCode", SqlDbType.VarChar).Value = DBNull.Value;

            }
            else
            {
                da.InsertCommand.Parameters.Add("@OriginAirportCityCode", SqlDbType.VarChar).Value = c.Substring(0, 3).ToString();
                da.InsertCommand.Parameters.Add("@DestinationAirportCityCode", SqlDbType.VarChar).Value = c.Substring(4, 3).ToString();

            }

            //if (c == "") { da.InsertCommand.Parameters.Add("@OriginAirportCityCode", SqlDbType.VarChar).Value = DBNull.Value; }
            //else { da.InsertCommand.Parameters.Add("@OriginAirportCityCode", SqlDbType.VarChar).Value = c.Substring(0, 3).ToString(); }

            //if (c == "") { da.InsertCommand.Parameters.Add("@DestinationAirportCityCode", SqlDbType.VarChar).Value = DBNull.Value; }
            //else { da.InsertCommand.Parameters.Add("@DestinationAirportCityCode", SqlDbType.VarChar).Value = c.Substring(4, 3).ToString(); }

            if (d == "") { da.InsertCommand.Parameters.Add("@Carrier", SqlDbType.VarChar).Value = DBNull.Value; }
            else { da.InsertCommand.Parameters.Add("@Carrier", SqlDbType.VarChar).Value = d; }

            if (e == "") { da.InsertCommand.Parameters.Add("@FlightNumber", SqlDbType.VarChar).Value = DBNull.Value; }
            else { da.InsertCommand.Parameters.Add("@FlightNumber", SqlDbType.VarChar).Value = e; }

            if (f == "") { da.InsertCommand.Parameters.Add("@ReservationBookingDesignator", SqlDbType.VarChar).Value = DBNull.Value; }
            else { da.InsertCommand.Parameters.Add("@ReservationBookingDesignator", SqlDbType.VarChar).Value = f; }

            if (g == "") { da.InsertCommand.Parameters.Add("@FlightDepartureDate", SqlDbType.VarChar).Value = DBNull.Value; }
            else { da.InsertCommand.Parameters.Add("@FlightDepartureDate", SqlDbType.VarChar).Value = g; }

            if (h == "") { da.InsertCommand.Parameters.Add("@FlightDepartureTime", SqlDbType.VarChar).Value = DBNull.Value; }
            else { da.InsertCommand.Parameters.Add("@FlightDepartureTime", SqlDbType.VarChar).Value = h; }

            if (i == "") { da.InsertCommand.Parameters.Add("@CouponStatus", SqlDbType.VarChar).Value = DBNull.Value; }
            else { da.InsertCommand.Parameters.Add("@CouponStatus", SqlDbType.VarChar).Value = i; }

            if (j == "") { da.InsertCommand.Parameters.Add("@FareBasisTicketDesignator", SqlDbType.VarChar).Value = DBNull.Value; }
            else { da.InsertCommand.Parameters.Add("@FareBasisTicketDesignator", SqlDbType.VarChar).Value = j; }

            if (k == "") { da.InsertCommand.Parameters.Add("@NotValidBefore", SqlDbType.VarChar).Value = DBNull.Value; }
            else { da.InsertCommand.Parameters.Add("@NotValidBefore", SqlDbType.VarChar).Value = k; }

            if (l == "") { da.InsertCommand.Parameters.Add("@NotValidAfter", SqlDbType.VarChar).Value = DBNull.Value; }
            else { da.InsertCommand.Parameters.Add("@NotValidAfter", SqlDbType.VarChar).Value = l; }

            if (m == "") { da.InsertCommand.Parameters.Add("@FreeBaggageAllowance", SqlDbType.VarChar).Value = DBNull.Value; }
            else { da.InsertCommand.Parameters.Add("@FreeBaggageAllowance", SqlDbType.VarChar).Value = m; }

            if (n == "") { da.InsertCommand.Parameters.Add("@FlightBookingStatus", SqlDbType.VarChar).Value = DBNull.Value; }
            else { da.InsertCommand.Parameters.Add("@FlightBookingStatus", SqlDbType.VarChar).Value = n; }

            if (o == "") { da.InsertCommand.Parameters.Add("@UsedClassofService", SqlDbType.VarChar).Value = DBNull.Value; }
            else { da.InsertCommand.Parameters.Add("@UsedClassofService", SqlDbType.VarChar).Value = o; }




            if (p.Trim() == "-")
            {
                da.InsertCommand.Parameters.Add("@UsageOriginCode", SqlDbType.VarChar).Value = DBNull.Value;
                da.InsertCommand.Parameters.Add("@UsageDestinationCode", SqlDbType.VarChar).Value = DBNull.Value;

            }
            else
            {
                da.InsertCommand.Parameters.Add("@UsageOriginCode", SqlDbType.VarChar).Value = p.Substring(0, 3).ToString();
                da.InsertCommand.Parameters.Add("@UsageDestinationCode", SqlDbType.VarChar).Value = p.Substring(4, 3).ToString();

            }


            //if (p == "") { da.InsertCommand.Parameters.Add("@UsageOriginCode", SqlDbType.VarChar).Value = DBNull.Value; }
            //else 
            //{ 
            //    da.InsertCommand.Parameters.Add("@UsageOriginCode", SqlDbType.VarChar).Value = p.Substring(0, 3).ToString(); 
            //}

            //if (p == "") { da.InsertCommand.Parameters.Add("@UsageDestinationCode", SqlDbType.VarChar).Value = DBNull.Value; }
            //else 
            //{ 
            //    da.InsertCommand.Parameters.Add("@UsageDestinationCode", SqlDbType.VarChar).Value = p.Substring(4, 3).ToString(); 
            //}

            if (q == "") { da.InsertCommand.Parameters.Add("@UsageAirline", SqlDbType.VarChar).Value = DBNull.Value; }
            else { da.InsertCommand.Parameters.Add("@UsageAirline", SqlDbType.VarChar).Value = q; }

            if (r == "") { da.InsertCommand.Parameters.Add("@UsageFlightNumber", SqlDbType.VarChar).Value = DBNull.Value; }
            else { da.InsertCommand.Parameters.Add("@UsageFlightNumber", SqlDbType.VarChar).Value = r; }

            if (s == "") { da.InsertCommand.Parameters.Add("@UsageDate", SqlDbType.VarChar).Value = DBNull.Value; }
            else { da.InsertCommand.Parameters.Add("@UsageDate", SqlDbType.VarChar).Value = s; }

            if (t == "") { da.InsertCommand.Parameters.Add("@FrequentFlyerReference", SqlDbType.VarChar).Value = DBNull.Value; }
            else { da.InsertCommand.Parameters.Add("@FrequentFlyerReference", SqlDbType.VarChar).Value = t; }

            if (u == "") { da.InsertCommand.Parameters.Add("@DocumentNumber", SqlDbType.VarChar).Value = DBNull.Value; }
            else { da.InsertCommand.Parameters.Add("@DocumentNumber", SqlDbType.VarChar).Value = u; }

            da.InsertCommand.Parameters.Add("@HdrGuidRef", SqlDbType.UniqueIdentifier).Value = v;

            da.InsertCommand.Parameters.Add("@RelatedDocumentGuid", SqlDbType.UniqueIdentifier).Value = y;


            cs.Open();
            //da.InsertCommand = new SqlCommand(sql, cs);
            da.InsertCommand.ExecuteNonQuery();
            cs.Close();

        }

        public string CheckGrid(int col, int row, string docNum)
        {
            var culture = System.Globalization.CultureInfo.CurrentCulture;
            //string docNum = Request["docNum"];
            string sql = "select CouponNumber, StopOverCode,Concat(OriginAirportCityCode, '-', DestinationAirportCityCode), Carrier" + Environment.NewLine;
            sql += ", FlightNumber, ReservationBookingDesignator, FlightDepartureDate, FlightDepartureTime, CouponStatus" + Environment.NewLine;
            sql += ", FareBasisTicketDesignator, NotValidBefore, NotValidAfter, FreeBaggageAllowance,FlightBookingStatus" + Environment.NewLine;
            sql += ", UsedClassofService, Concat(UsageOriginCode,'-', UsageDestinationCode), UsageAirline, UsageFlightNumber" + Environment.NewLine;
            sql += ", UsageDate,FrequentFlyerReference,[RelatedDocumentGuid]" + Environment.NewLine;
            sql += " from pax.SalesDocumentCoupon" + Environment.NewLine;
            sql += " where DocumentNumber = '" + docNum + "'" + Environment.NewLine;
            sql += " order by DocumentNumber, CouponNumber" + Environment.NewLine;
            SqlConnection con = new SqlConnection(pbConnectionString);
            DataSet ds = new DataSet();
            SqlDataAdapter ada = new SqlDataAdapter(sql, con);
            ada.Fill(ds);
            con.Close();
            int i = 0;

            int lon = ds.Tables[0].Rows.Count;
            string[,] dbgCPNlist = new string[21, lon];
            if (lon != 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    dbgCPNlist[0, i] = dr[ds.Tables[0].Columns[0].ColumnName].ToString();
                    dbgCPNlist[1, i] = dr[ds.Tables[0].Columns[1].ColumnName].ToString();
                    string a = dr[ds.Tables[0].Columns[2].ColumnName].ToString().Trim();
                    if (a != "-")
                    {
                        dbgCPNlist[2, i] = dr[ds.Tables[0].Columns[2].ColumnName].ToString();
                    }
                    dbgCPNlist[3, i] = dr[ds.Tables[0].Columns[3].ColumnName].ToString();
                    dbgCPNlist[4, i] = dr[ds.Tables[0].Columns[4].ColumnName].ToString();
                    dbgCPNlist[5, i] = dr[ds.Tables[0].Columns[5].ColumnName].ToString();
                    try
                    {
                        dbgCPNlist[6, i] = Convert.ToDateTime(dr[ds.Tables[0].Columns[6].ColumnName].ToString()).ToString("dd-MMM-yyyy");
                    }
                    catch
                    {

                    }
                    dbgCPNlist[7, i] = dr[ds.Tables[0].Columns[7].ColumnName].ToString();
                    dbgCPNlist[8, i] = dr[ds.Tables[0].Columns[8].ColumnName].ToString();
                    dbgCPNlist[9, i] = dr[ds.Tables[0].Columns[9].ColumnName].ToString();
                    try
                    {
                        dbgCPNlist[10, i] = Convert.ToDateTime(dr[ds.Tables[0].Columns[10].ColumnName].ToString()).ToString("dd-MMM-yyyy");
                    }
                    catch
                    {

                    }
                    try
                    {
                        dbgCPNlist[11, i] = Convert.ToDateTime(dr[ds.Tables[0].Columns[11].ColumnName].ToString()).ToString("dd-MMM-yyyy");
                    }
                    catch
                    {

                    }
                    dbgCPNlist[12, i] = dr[ds.Tables[0].Columns[12].ColumnName].ToString();
                    dbgCPNlist[13, i] = dr[ds.Tables[0].Columns[13].ColumnName].ToString();
                    dbgCPNlist[14, i] = dr[ds.Tables[0].Columns[14].ColumnName].ToString();
                    dbgCPNlist[15, i] = dr[ds.Tables[0].Columns[15].ColumnName].ToString();
                    dbgCPNlist[16, i] = dr[ds.Tables[0].Columns[16].ColumnName].ToString();
                    dbgCPNlist[17, i] = dr[ds.Tables[0].Columns[17].ColumnName].ToString();
                    try
                    {
                        dbgCPNlist[18, i] = Convert.ToDateTime(dr[ds.Tables[0].Columns[18].ColumnName].ToString()).ToString("dd-MMM-yyyy");
                    }
                    catch
                    {

                    }
                    dbgCPNlist[19, i] = dr[ds.Tables[0].Columns[19].ColumnName].ToString();
                    dbgCPNlist[20, i] = dr[ds.Tables[0].Columns[20].ColumnName].ToString();
                    i = i + 1;
                }
            }
            string b = "";
            if (dbgCPNlist[row, col] != null && !string.IsNullOrWhiteSpace(dbgCPNlist[col, row].ToString()) && dbgCPNlist[col, row] != "") { b = dbgCPNlist[col, row].ToString(); }
            else { b = ""; }
            return b;
        }

        private void ManualRelatedDocInf(Guid HdrGuid, string CheckDigit, string docNum)
        {
            SqlConnection cs = new SqlConnection(pbConnectionString);
            if (cs.State == ConnectionState.Open)
            {
                cs.Close();
            }
            cs.Close();
            SqlDataAdapter dr = new SqlDataAdapter();
            string sql = "";
            sql = sql + " INSERT INTO [TMP].[SalesRelatedDocumentInformation] (RelatedDocumentGuid,HdrGuid,[DocumentNumber],[CheckDigit],[CouponIndicator],[TransactionCode]) ";
            sql = sql + " VALUES( '" + HdrGuid + "','" + HdrGuid + "' ";
            sql = sql + ",'" + docNum + "'";//DocumentNumber          
            sql = sql + ",'" + CheckDigit + "'"; //CheckDigit       
            sql = sql + ",'' "; //CPUI
            sql = sql + ",'RFND') ";//transactioncode

            cs.Open();


            dr.InsertCommand = new SqlCommand(sql, cs);
            dr.InsertCommand.ExecuteNonQuery();
        }

        private void Saveotheramt(Guid HdrGuid, string[,] dgother, string docNum, string dateOfRfn, int compt)
        {
            SqlConnection cs = new SqlConnection(pbConnectionString);
            if (cs.State == ConnectionState.Open)
            {
                cs.Close();
            }
            cs.Close();
            SqlDataAdapter da = new SqlDataAdapter();

            string sql = "";
            for (int vi = 0; vi < compt; vi++)
            {


                sql = sql + " INSERT INTO TMP.SalesDocumentOtherAmount ";
                sql = sql + " (DocumentAmountType,DateofIssue,CurrencyType,TransactionCode,OtherAmountCode,OtherAmount,OtherAmountRate,HdrGuidRef,DocumentNumber,SequenceNumber, FileSequence) VALUES ";


                sql = sql + " ('" + dgother[0, vi] + "','" + dateOfRfn + "','" + dgother[1, vi] + "','RFND','" + dgother[2, vi].ToString() + "' ";
                sql = sql + " ,'" + (valid(dgother[3, vi].ToString()) * (-1)) + "' ";
                sql = sql + " , NULL ";
                sql = sql + " ,'" + HdrGuid + "','" + docNum + "','" + dgother[4, vi] + "'";
                sql = sql + " ,'" + dgother[5, vi].ToString() + "')";



            }

            cs.Open();
            da.InsertCommand = new SqlCommand(sql, cs);
            da.InsertCommand.ExecuteNonQuery();
            cs.Close();
        }

        private void SavePayment(Guid HdrGuid, string dateOfRfn, string totRfnAmt, string curRfn, string refundBy, string docNum)
        {
            SqlConnection cs = new SqlConnection(pbConnectionString);
            if (cs.State == ConnectionState.Open)
            {
                cs.Close();
            }
            cs.Close();
            SqlDataAdapter da = new SqlDataAdapter();
            string sql = "";

            sql = sql + " INSERT INTO TMP.SalesDocumentPayment (DateofIssue,Amount,Currency,FormofPaymentType,RemittanceAmount,RemittanceCurrency,TransactionCode,HdrGuidRef,DocumentNumber)  ";
            sql = sql + " VALUES('" + dateOfRfn + "' ";
            sql = sql + "  , '" + totRfnAmt + "'";
            sql = sql + "  , '" + curRfn + "'";
            sql = sql + "  , '" + refundBy + "'";
            sql = sql + "  , 0.00 ";
            sql = sql + "  , '" + curRfn + "'";
            sql = sql + ",'RFND','" + HdrGuid + "','" + docNum + "')  ";

            cs.Open();
            da.InsertCommand = new SqlCommand(sql, cs);
            da.InsertCommand.ExecuteNonQuery();
            cs.Close();

        }

        private void SaveCoupon(string doc, string[,] dbgCPNlist, int lon)
        {


            string sql = "if not exists(select * " + Environment.NewLine;
            sql += "from pax.SalesDocumentcouponOriginal " + Environment.NewLine;
            sql += "where documentnumber = '" + doc + "') " + Environment.NewLine;
            sql += "INSERT INTO pax.SalesDocumentcouponOriginal " + Environment.NewLine;
            sql += " SELECT * FROM pax.SalesDocumentcoupon where documentnumber = '" + doc + "'  " + Environment.NewLine;
            SqlConnection cs = new SqlConnection(pbConnectionString);
            if (cs.State == ConnectionState.Open) { cs.Close(); }
            cs.Open();
            SqlDataAdapter da = new SqlDataAdapter();
            da.InsertCommand = new SqlCommand(sql, cs);
            da.InsertCommand.ExecuteNonQuery();
            cs.Close();

            for (int i = 0; i < lon; i++)
            {

                string CouponStatus = "";
                string UsageRBD = "";
                string UsageOri = "";
                string UsageDest = "";
                string UsageAirline = "";
                string UsageFltNo = "";
                string UsageDate = "";

                if (dbgCPNlist[8, i] != null && !string.IsNullOrWhiteSpace(dbgCPNlist[8, i].ToString()) && dbgCPNlist[8, i] != "")
                {
                    CouponStatus = dbgCPNlist[8, i].ToString().ToUpper();

                }

                if (dbgCPNlist[14, i] != null && !string.IsNullOrWhiteSpace(dbgCPNlist[14, i].ToString()) && dbgCPNlist[14, i] != "")
                {
                    UsageRBD = dbgCPNlist[14, i].ToString().ToUpper();
                }

                if (dbgCPNlist[15, i] != null && !string.IsNullOrWhiteSpace(dbgCPNlist[15, i].ToString()) && dbgCPNlist[15, i] != "")
                {
                    if (dbgCPNlist[15, i].ToString().Trim() != "-")
                    {
                        UsageOri = dbgCPNlist[15, i].ToString().Substring(0, 3).ToUpper();
                        UsageDest = dbgCPNlist[15, i].ToString().Substring(4, 3).ToUpper();
                    }
                }

                if (dbgCPNlist[16, i] != null && !string.IsNullOrWhiteSpace(dbgCPNlist[16, i].ToString()) && dbgCPNlist[16, i] != "")
                {
                    UsageAirline = dbgCPNlist[16, i].ToString();
                }

                if (dbgCPNlist[17, i] != null && !string.IsNullOrWhiteSpace(dbgCPNlist[17, i].ToString()) && dbgCPNlist[17, i] != "")
                {
                    UsageFltNo = dbgCPNlist[17, i].ToString();
                }

                if (dbgCPNlist[18, i] != null && !string.IsNullOrWhiteSpace(dbgCPNlist[18, i].ToString()) && dbgCPNlist[18, i] != "")
                {
                    UsageDate = dbgCPNlist[18, i].ToString();
                }

                #region UPDATE USAGE IN PAX.SALESDOCUMENTCOUPON
                string sl = "update pax.salesdocumentcoupon set " + Environment.NewLine;

                if (CouponStatus != "")
                {
                    sl += "CouponStatus = '" + CouponStatus + "'," + Environment.NewLine;

                }
                else
                {
                    sl += "CouponStatus = 'S' ," + Environment.NewLine;

                }

                if (UsageRBD != "")
                {
                    sl += "UsedClassofService = '" + UsageRBD + "'," + Environment.NewLine;

                }
                else
                {
                    sl += "UsedClassofService = null ," + Environment.NewLine;

                }

                if (UsageOri != "")
                {
                    sl += "UsageOriginCode = '" + UsageOri + "'" + Environment.NewLine;

                    sl += ", UsageDestinationCode = '" + UsageDest + "'," + Environment.NewLine;
                }
                else
                {
                    sl += "UsageOriginCode = null" + Environment.NewLine;

                    sl += ", UsageDestinationCode = null ," + Environment.NewLine;

                }
                if (UsageAirline != "")
                {
                    sl += "UsageAirline = '" + UsageAirline + "'," + Environment.NewLine;
                }
                else
                {
                    sl += "UsageAirline = null ," + Environment.NewLine;
                }


                if (UsageFltNo != "")
                {
                    sl += "UsageFlightNumber = '" + UsageFltNo + "'," + Environment.NewLine;
                }
                else
                {
                    sl += "UsageFlightNumber = null ," + Environment.NewLine;
                }

                if (UsageDate != "")
                {
                    sl += "UsageDate = cast('" + UsageDate + "' as date) " + Environment.NewLine + Environment.NewLine;
                }
                else
                {
                    sl += "UsageDate = null" + Environment.NewLine + Environment.NewLine;
                }


                sl += "where CouponNumber = '" + dbgCPNlist[0, i] + "' and RelatedDocumentNumber = '" + doc + "'" + Environment.NewLine;

                if (cs.State == ConnectionState.Open) { cs.Close(); }
                cs.Open();


                SqlDataAdapter daf = new SqlDataAdapter();
                daf.UpdateCommand = new SqlCommand(sl, cs);
                #endregion


                try
                {
                    daf.UpdateCommand.ExecuteNonQuery();

                }
                catch
                {

                }

                cs.Close();
            }

        }
        /***************Commission Reclaim****************************/

        public ActionResult CommissionReclaim()
        {
            ViewBag.dateFrom = DateTime.Now.AddMonths(-1).ToString("dd-MMM-yyyy");
            ViewBag.dateTo = DateTime.Now.ToString("dd-MMM-yyyy");
            return PartialView();
        }
        public ActionResult LoadCommissionReclaim()
        {
            string dateFrom1 = Request["dateFrom"];
            string dateFrom = ConvertDate(dateFrom1);
            string dateTo1 = Request["dateTo"];
            string dateTo = ConvertDate(dateTo1);
            SqlConnection con = new SqlConnection(pbConnectionString);
            SqlCommand cmd = new SqlCommand("[Pax].[SP_Commission_Reclaim]", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 1000;
            cmd.Parameters.AddWithValue("@fromDate", dateFrom);
            cmd.Parameters.AddWithValue("@toDate", dateTo);

            DataSet ds = new DataSet();
            SqlDataAdapter ada = new SqlDataAdapter(cmd);
            ada.Fill(ds);
            con.Close();
            return PartialView(ds);

        }

        /***********************************end*************************************/


        /***************ExpectedEIR_OFR****************************/

        public ActionResult ExpectedEIROFR()
        {
            ViewBag.dateFrom = DateTime.Now.AddMonths(-1).ToString("dd-MMM-yyyy");
            ViewBag.dateTo = DateTime.Now.ToString("dd-MMM-yyyy");
            return PartialView();
        }
        public ActionResult LoadExpectedEIROFR()
        {

            var culture = System.Globalization.CultureInfo.CurrentCulture;
            string dateFrom1 = Request["dateFrom"];
            string dateFrom = ConvertDate(dateFrom1);
            string dateTo1 = Request["dateTo"];
            string dateTo = ConvertDate(dateTo1);
            string selectType = Request["dropdownType"];
            string ex = "";

            if (selectType == "EXPECTED INTERLINE PAYABLES")
            {
                ex = "f3.couponstatus = 'S'  and f3.BillingType ='I'";
            }
            else
            {
                ex = "f3.couponstatus = 'S' and f3.BillingType ='O'";
            }


            string sql = " select  f3.RelatedDocumentNumber,f3.CouponNumber as CPN,f3.OriginAirportCityCode as Orig  " + Environment.NewLine;
            sql = sql + " ,f3.DestinationAirportCityCode as Dest ,f3.Carrier    " + Environment.NewLine;
            sql = sql + " ,f3.FlightNumber as [Flight Number] ,f3.FlightDepartureDate as [Flight Date]   " + Environment.NewLine;
            sql = sql + " , case when f4.SpecialProrateAgreement = '0.00' then 'MPA Applies' else 'SPA Applies' end as [PM]   " + Environment.NewLine;
            sql = sql + " ,f4.FinalShare as [Final Share]   " + Environment.NewLine;
            sql = sql + " , case    " + Environment.NewLine;
            sql = sql + " when f3.couponstatus = 'S'  and f3.BillingType ='I' then 'EXPECTED INTERLINE PAYABLE'    " + Environment.NewLine;
            sql = sql + " when f3.couponstatus = 'S' and f3.BillingType ='O' then 'EXPECTED FLOWN REVENUE'    " + Environment.NewLine;
            sql = sql + "   else null  end  as Status       " + Environment.NewLine;
            sql = sql + " from Pax.salesdocumentheader f1  join pax.SalesRelatedDocumentInformation f2 on f1.HdrGuid = f2.HdrGuid   " + Environment.NewLine;
            sql = sql + " join pax.salesdocumentcoupon f3   on f2.RelatedDocumentguid = f3.RelatedDocumentguid   " + Environment.NewLine;
            sql = sql + " left join pax.ProrationDetail f4 on f4.RelatedDocumentGuid = f3.RelatedDocumentGuid and f3.CouponNumber = f4.CouponNumber    " + Environment.NewLine;
            sql = sql + " and f3.CouponStatus = f4.ProrationFlag   " + Environment.NewLine;
            sql = sql + " where FlightDepartureDate between cast('" + dateFrom + "' as date) and cast('" + dateTo + "' as date)  " + Environment.NewLine;
            sql = sql + " and  " + ex + "  and f4.FinalShare is not null " + Environment.NewLine;

            DataSet ds = new DataSet();
            SqlConnection con = new SqlConnection(pbConnectionString);
            SqlDataAdapter ada = new SqlDataAdapter(sql, con);
            ada.Fill(ds);
            con.Close();
            ViewBag.esd = ds;
            int lon = ds.Tables[0].Rows.Count;
            int col = ds.Tables[0].Columns.Count;
            string[,] tableau = new string[10, lon];
            int i = 0;
            foreach (DataRow dr in ds.Tables[0].Rows)
            {

                for (int j = 0; j < col; j++)
                {
                    if (j == 6)
                    {
                        string h = null;

                        h = dr[ds.Tables[0].Columns[j].ColumnName].ToString();

                        if (h != "")
                        {

                            tableau[j, i] = Convert.ToDateTime(h).ToShortDateString();
                        }

                    }
                    else
                    {

                        tableau[j, i] = dr[ds.Tables[0].Columns[j].ColumnName].ToString();
                    }
                }


                i++;
            }
            ViewBag.Tableau = tableau;
            ViewBag.compt = lon;

            double x = 0;
            for (int ii = 0; ii < lon; ii++)
            {
                x += Convert.ToDouble(tableau[8, ii].ToString());
            }

            ViewBag.TAmount = string.Format("{0:0.00}", x);
            string[] cll = new string[lon];
            for (int g = 0; g < lon; g++)
            {
                cll[g] = tableau[4, g].ToString();
            }

            string[] b = cll.Distinct().ToArray();
            string[,] dglia = new string[3, b.Length];
            //int dlg = dglia[0].Rows.Count;
            for (int y = 0; y < b.Length; y++)
            {

                dglia[1, y] = b[y].ToString();
            }

            string[] sum = new string[b.Length];
            string[] cnt = new string[b.Length];



            for (int o = 0; o < b.Length; o++)
            {
                for (int k = 0; k < lon; k++)
                {
                    if (dglia[1, o].ToString() == tableau[4, k].ToString())
                    {
                        if (sum[o] == null)
                        {
                            sum[o] = "0";
                        }
                        decimal interm = Convert.ToDecimal(sum[o].ToString(), new CultureInfo(culture.Name));
                        decimal interm1 = Convert.ToDecimal(tableau[8, k].ToString(), new CultureInfo(culture.Name));
                        decimal val = interm + interm1;

                        sum[o] = val.ToString();

                        if (cnt[o] == null)
                        {
                            cnt[o] = "0";
                        }

                        cnt[o] = (Convert.ToDecimal(cnt[o].ToString(), new CultureInfo(culture.Name)) + 1).ToString();
                    }
                }
            }

            for (int yy = 0; yy < b.Length; yy++)
            {
                dglia[2, yy] = sum[yy].ToString();
                dglia[0, yy] = cnt[yy].ToString();
            }
            ViewBag.comp1 = b.Length;

            ViewBag.dgsummary = dglia;
            if (lon == 0)
            {
                ViewBag.mes = "No data available for the selected criteria.";
            }

            return PartialView(ds);


        }

        /***********************************end*************************************/
        /* other taxe*/
        public string[,] otherTaxe(string Doc)
        {
            string sql = "SELECT DocumentAmountType, CurrencyType, OtherAmountCode, OtherAmount,SequenceNumber,Filesequence" + Environment.NewLine;
            sql += "FROM [Pax].[SalesDocumentOtherAmount]" + Environment.NewLine;
            sql += "WHERE DocumentNumber = '" + Doc + "' AND TransactionCode <> 'RFND'" + Environment.NewLine;
            SqlConnection con = new SqlConnection(pbConnectionString);
            DataSet ds = new DataSet();
            SqlDataAdapter ada = new SqlDataAdapter(sql, con);

            ada.Fill(ds);
            con.Close();
            int lon = ds.Tables[0].Rows.Count;
            string[,] other = new string[7, lon];
            int i = 0;
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                other[0, i] = dr[ds.Tables[0].Columns[0].ColumnName].ToString();
                other[1, i] = dr[ds.Tables[0].Columns[1].ColumnName].ToString();
                other[2, i] = dr[ds.Tables[0].Columns[2].ColumnName].ToString();
                other[3, i] = dr[ds.Tables[0].Columns[3].ColumnName].ToString();
                other[5, i] = dr[ds.Tables[0].Columns[4].ColumnName].ToString();
                other[6, i] = dr[ds.Tables[0].Columns[5].ColumnName].ToString();
                i = i + 1;
            }
            string sql1 = "SELECT DocumentAmountType, CurrencyType, OtherAmountCode, OtherAmount,SequenceNumber,Filesequence" + Environment.NewLine;
            sql1 += "FROM [Pax].[SalesDocumentOtherAmount]" + Environment.NewLine;
            sql1 += "WHERE DocumentNumber = '" + Doc + "' AND TransactionCode = 'RFND'" + Environment.NewLine;
            //SqlConnection con = new SqlConnection(pbConnectionString);
            DataSet ds1 = new DataSet();
            SqlDataAdapter ada1 = new SqlDataAdapter(sql1, con);
            ada1.Fill(ds1);
            con.Close();
            if (ds1.Tables[0].Rows.Count == 0)
            {
                int j = 0;
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    other[4, j] = "false";
                    j = j + 1;
                }
            }
            else
            {
                foreach (DataRow dr in ds1.Tables[0].Rows)
                {

                    string a1 = dr[ds1.Tables[0].Columns[0].ColumnName].ToString();
                    string b1 = dr[ds1.Tables[0].Columns[2].ColumnName].ToString();
                    string c1 = (Convert.ToDouble(dr[ds1.Tables[0].Columns[3].ColumnName].ToString()) * (-1)).ToString("0.00");
                    for (int j = 0; j < lon; j++)
                    {
                        string a = other[0, j].ToString();
                        string b = other[2, j].ToString();
                        string c = other[3, j].ToString();
                        if (a1 == a && b1 == b && c1 == c)
                        {
                            other[4, j] = "true";
                        }
                        else
                        {
                            other[4, j] = "false";
                        }
                    }


                }
            }

            return other;
        }

        public string[] refundHeader(string Doc)
        {
            string sql = "select FORMAT(DateofIssue, 'dd-MMM-yyyy'), AgentNumericCode, EndosRestriction, FareCalculationArea" + Environment.NewLine;
            sql += "from pax.SalesDocumentHeader" + Environment.NewLine;
            sql += "where DocumentNumber = '" + Doc + "'" + Environment.NewLine;
            sql += "and TransactionCode = 'RFND'" + Environment.NewLine;
            SqlConnection con = new SqlConnection(pbConnectionString);
            DataSet ds = new DataSet();
            SqlDataAdapter ada = new SqlDataAdapter(sql, con);
            ada.Fill(ds);
            con.Close();
            string[] rfnHeader = new string[4];
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                if ((dr[ds.Tables[0].Columns[0].ColumnName].ToString() != "") || dr[ds.Tables[0].Columns[0].ColumnName].ToString() != "null")
                {
                    rfnHeader[0] = dr[ds.Tables[0].Columns[0].ColumnName].ToString();
                }
                else
                {
                    rfnHeader[0] = DateTime.Now.ToString("dd-MMM-yyyy");
                }
                rfnHeader[1] = dr[ds.Tables[0].Columns[1].ColumnName].ToString();
                rfnHeader[2] = dr[ds.Tables[0].Columns[2].ColumnName].ToString();
                rfnHeader[3] = dr[ds.Tables[0].Columns[3].ColumnName].ToString();
            }
            return rfnHeader;
        }


        public ActionResult FIMManager()
        {

            string Sql = "SELECT [FIMNumber] as [FIM NO] " + Environment.NewLine;
            Sql += ",case when [XmlGenetared] = '1' then 'YES' ELSE 'NO' END as [XML GENERATED] " + Environment.NewLine;
            Sql += ",[ProcessDate] as [PROCESS DATE] " + Environment.NewLine;
            Sql += ",case when [XmlGenetared] = '1' then 'Process' ELSE 'Unprocess' END as [STATUS] " + Environment.NewLine;
            Sql += " FROM [Pax].[FIMHeaderData]  order by [RecId] desc " + Environment.NewLine;
            DataSet ds = new DataSet();
            ds = dbconnect.RetObjDS(Sql);

            /*if (rdprocess.Checked == true)
            {
                Sql += "SELECT [FIMNumber] as [FIM NO] " + Environment.NewLine;
                Sql += ",case when [XmlGenetared] = 1 then 'YES' ELSE 'NO' END as [XML GENERATED] " + Environment.NewLine;
                Sql += ",[ProcessDate] as [PROCESS DATE] " + Environment.NewLine;
                Sql += ",case when [XmlGenetared] = 1 then 'Process' ELSE 'Unprocess' END as [Status] " + Environment.NewLine;
                Sql += " FROM [Pax].[FIMHeaderData] where [XmlGenetared] = '1'   order by [RecId] desc " + Environment.NewLine;
            }
            if (rdunprocess.Checked == true)
            {
                Sql += " SELECT [FIMNumber] as [FIM NO] " + Environment.NewLine;
                Sql += " ,case when [XmlGenetared] = 1 then 'YES' ELSE 'NO' END as [XML GENERATED] " + Environment.NewLine;
                Sql += ",[ProcessDate] as [PROCESS DATE] " + Environment.NewLine;
                Sql += ",case when [XmlGenetared] = 1 then 'Process' ELSE 'Unprocess' END as [STATUS]  " + Environment.NewLine;
                Sql += " FROM [Pax].[FIMHeaderData]  where isnull([XmlGenetared],0)<> '1'    order by [RecId] desc " + Environment.NewLine;
            }*/
            return PartialView(ds);
        }

        public ActionResult LoadFimDetailData(string dataValue)
        {
            string sql = "";
            string sql1 = "";
            sql = "SELECT[Recid],[OWN_AOL],[FIMNumber],[OriginalFlightNumber],[OriginalFlightDate], ";
            sql += "[SectorFrom],[AirlineId],[RoutedFromFlightNo],FORMAT(RoutedFromDate,'dd-MMM-yyyy') as [RoutedFromDate],[PlaceOfInterruption],[RoutedTo1],[RoutedTo_Airline1],RoutedTo_FlightNumber1,FORMAT(RoutedTo_Date1,'dd-MMM-yyyy') as [RoutedTo_Date1],[RoutedTo_From1],[RoutedTo_To1] ";
            sql += ",[RoutedTo2],[RoutedTo_Airline2],[RoutedTo_FlightNumber2],FORMAT(RoutedTo_Date2,'dd-MMM-yyyy') as [RoutedTo_Date2],[RoutedTo_From2],[RoutedTo_To2] ";
            sql += ",[ReasonForIssurance],[DiversionByCarrierCode],[TotalNoOfPassencer],[ValidatorName] ";
            sql += ",[FIMDocReceived],[TempFIMNumber],[Status],[OriginalSector] FROM[Pax].[FIMHeaderData] WHERE FIMNumber = '" + dataValue + "' ";
            DataSet ds = new DataSet();
            ds = dbconnect.RetObjDS(sql);

            sql1 = "select[RecordNumber],[PassengerName],[CouponNumber],[AirlineCode],[RelatedDocumentNumber],[CheckDigit],[DocumentType],[FareBasis_PaxTypeCode],[NewFlightNo1],[NewFlightNo2],[Billed1],[Accepted1],[Difference1] ";
            sql1 += " FROM[Pax].[FIMDetailsEntry] WHERE FIMNumber = '" + dataValue + "' Order by RecordNumber";
            DataSet ds1 = new DataSet();
            ds1 = dbconnect.RetObjDS(sql1);
            ViewBag.fimBody = ds1;

            return PartialView(ds);
        }

        public ActionResult AiroportCode(string dataValue)
        {
            string sql = "";
            sql = "SELECT[AirportCode],[CityCode],[AirportName],[CityName],[Country],[CityIsoCode],[Status] ";
            sql += "FROM[Ref].[City] where[AirportCode] like '" + dataValue + "%' ";
            DataSet ds = new DataSet();
            ds = dbconnect.RetObjDS(sql);
            return PartialView(ds);
        }

        public ActionResult FlightLookup(string dataValue)
        {

            string Sql = "IF OBJECT_ID('tempdb..#Tmp') IS NOT NULL DROP TABLE #Tmp ";
            Sql += "select distinct (select AirlineID from ref.Airlines where AirlineCode = f1.Carrier) as Airline,";
            Sql += " FORMAT(FlightDepartureDate,'dd-MMM-yyyy') as [Flight Date], FlightNumber as [Flight Number], OriginCity as Origin, ";
            Sql += " DestinationCity as Destination into #Tmp from Pax.SalesDocumentCoupon f1 where FlightNumber like '%" + dataValue + "%' and FlightNumber <>'OPEN'";
            Sql += " and cast (FlightDepartureDate as Date) = '25-Mar-2018' ";
            //Sql += " and cast (FlightDepartureDate as Date) = '" + dateTimePicker1.ToString("dd-MMM-yyyy") + "' ";
            Sql += " select * from #Tmp  order by [Flight Date] desc";
            DataSet ds = new DataSet();
            ds = dbconnect.RetObjDS(Sql);
            return PartialView(ds);
        }

        public ActionResult PassengerLookup(string[] dataValue)
        {
            string Sql = " IF OBJECT_ID('tempdb..#Tmp') IS NOT NULL DROP TABLE #Tmp  SELECT distinct F2.[RelatedDocumentNumber] as [Document Number]  ,F3.[CouponNumber] as [Coupon Number]  ";
            Sql += " ,F1.[PassengerName] as [PassengerName] ,F1.[PaxType] as [Pax Type] ,(select AirlineID from ref.Airlines where AirlineCode = f3.Carrier) as Airline ";
            Sql += " ,FORMAT(FlightDepartureDate,'dd-MMM-yyyy') as [Flight Date] ,FlightNumber as [Flight Number] ,OriginCity as Origin ,DestinationCity as Destination  ";
            Sql += " ,F3.[DomesticInternational] as [Intl Dom] ,F3.[IsOAL] as [Is OAL] ,F1.[InvoluntaryReroute] as [Involuntary Reroute] ";
            Sql += " ,F2.[RelatedTicketCheckDigit] as [Check Digit] ,F1.[DocumentAirlineID] as [Document AirlineID] ,F1.[OwnTicket],F1.[OwnCarrier] as [Own Carrier] ";
            Sql += " ,F3.[FareBasisTicketDesignator] as [Fare Basis] , substring(F1.[DocumentType],1,1) as [Document Type] into #Tmp ";
            Sql += " FROM [Pax].[SalesDocumentHeader] F1 left join [Pax].[SalesRelatedDocumentInformation] F2  on F1.DocumentNumber= f2.DocumentNumber ";
            Sql += " left join [Pax].[SalesDocumentCoupon] F3  on F2.[RelatedDocumentNumber]= f3.RelatedDocumentNumber where FlightNumber='" + dataValue[0] + "'";
            Sql += " and FlightDepartureDate ='" + dataValue[1] + "' and OriginCity='" + dataValue[2] + "' and DestinationCity='" + dataValue[3] + "'";
            Sql += " and FlightNumber <>'OPEN' select * from #Tmp ";
            DataSet ds = new DataSet();
            ds = dbconnect.RetObjDS(Sql);
            return PartialView(ds);
        }

        public string SaveFIMHeaderData(string[] dataValue, string[] dataValue1)
        {
            /*tab = [ownOal, fimNumber, RoutedFrom, AirlineCode, RoutedFromFlight, DateRoutedFrom, RoutedFromPlaceOfInterruption, ReRoutedTo1, ReroutedAirline1, ReRoute1Flight,
                   DateRoutedTo1, ReRoute1From, ReRoute1To, ReRoutedTo2, ReroutedAirline2,ReRoute2Flight, DateRoutedto2, ReRoute2From, ReRoute2To, reasonIssuance, DiversionCarrCode,
                   TotalNoOfPax, ValidatorName];*/
            //Header Data

            string FIMNo = dataValue[1];
            string TempFIMNumber = "";
            if (FIMNo.Trim().Length == 0)
            {
                // Get a temporary FIM No
                string CurrentId = DateTime.Now.ToString("yyyyMMddHHmmss");
                TempFIMNumber = "T" + CurrentId;
                FIMNo = TempFIMNumber;
            }


            int FIMDocReceived = 0; // 0 = not received /1 = received
            TempFIMNumber = FIMNo;
            string Status = "0";

            string SqlFIMNumber = "Select FIMNumber From  [Pax].[FIMHeaderData]  WHERE  FIMNumber='" + FIMNo + "'";
            string Sql = "IF NOT EXISTS(" + SqlFIMNumber + ")";
            Sql = Sql + "BEGIN" + Environment.NewLine;
            Sql = Sql + "DECLARE @MaxRecId bigint;" + Environment.NewLine;
            Sql = Sql + "set @MaxRecId = (select iif(MAX(RecId) is null,1, MAX(RecId)+1) As MaxLineid from [Pax].[FIMHeaderData])" + Environment.NewLine;
            Sql = Sql + "INSERT INTO [Pax].[FIMHeaderData] ";
            Sql = Sql + "([Recid],[FIMNumber],[OWN_AOL],[OriginalFlightNumber],[OriginalFlightDate],[OriginalSector],[SectorFrom],[AirlineId],RoutedFromFlightNo,[RoutedFromDate],[PlaceOfInterruption]" + Environment.NewLine; ;
            Sql = Sql + ",[RoutedTo1],[RoutedTo_Airline1],[RoutedTo_FlightNumber1],[RoutedTo_Date1],[RoutedTo_From1],[RoutedTo_To1]" + Environment.NewLine; ;
            Sql = Sql + ",[RoutedTo2],[RoutedTo_Airline2],[RoutedTo_FlightNumber2],[RoutedTo_Date2],[RoutedTo_From2],[RoutedTo_To2]" + Environment.NewLine; ;
            Sql = Sql + ",[ReasonForIssurance],[DiversionByCarrierCode],[TotalNoOfPassencer],[ValidatorName],[FIMDocReceived],[TempFIMNumber],[Status])" + Environment.NewLine; ;
            Sql = Sql + "VALUES(";
            Sql = Sql + " @MaxRecId,";
            Sql = Sql + "'" + FIMNo + "',";
            Sql = Sql + "'" + dataValue[0] + "',";
            Sql = Sql + "'" + dataValue[4] + "',";
            Sql = Sql + "'" + dataValue[5] + "',";
            Sql = Sql + "'" + dataValue[23] + "',";
            Sql = Sql + "'" + dataValue[2] + "',";

            Sql = Sql + "'" + dataValue[3] + "',";
            Sql = Sql + "'" + dataValue[4] + "',";
            Sql = Sql + "'" + dataValue[5] + "',";
            Sql = Sql + "'" + dataValue[6] + "'," + Environment.NewLine; ;

            Sql = Sql + "'" + dataValue[7] + "',";
            Sql = Sql + "'" + dataValue[8] + "',";
            Sql = Sql + "'" + dataValue[9] + "',";
            Sql = Sql + "'" + dataValue[10] + "',";
            Sql = Sql + "'" + dataValue[11] + "',";
            Sql = Sql + "'" + dataValue[12] + "'," + Environment.NewLine; ;

            Sql = Sql + "'" + dataValue[13] + "',";
            Sql = Sql + "'" + dataValue[14] + "',";
            Sql = Sql + "'" + dataValue[15] + "',";
            Sql = Sql + "'" + dataValue[16] + "',";
            Sql = Sql + "'" + dataValue[17] + "',";
            Sql = Sql + "'" + dataValue[18] + "'," + Environment.NewLine; ;

            Sql = Sql + "'" + dataValue[19] + "',";
            Sql = Sql + "'" + dataValue[20] + "',";
            Sql = Sql + "'" + dataValue[21] + "',";
            Sql = Sql + "'" + dataValue[22] + "',";
            Sql = Sql + "" + FIMDocReceived + ",";
            Sql = Sql + "'" + TempFIMNumber + "',";
            Sql = Sql + "'" + Status + "'" + Environment.NewLine; ;
            Sql = Sql + ")";
            Sql = Sql + "END" + Environment.NewLine;
            Sql = Sql + "ELSE" + Environment.NewLine;
            Sql = Sql + "BEGIN" + Environment.NewLine;
            Sql = Sql + "UPDATE [Pax].[FIMHeaderData]";
            Sql = Sql + " SET ";
            Sql = Sql + "[OWN_AOL]= '" + dataValue[0] + "'";
            Sql = Sql + ",[OriginalFlightNumber]= '" + dataValue[4] + "'";
            Sql = Sql + ",[OriginalFlightDate]= '" + dataValue[5] + "'";
            Sql = Sql + ",[OriginalSector]= '" + dataValue[23] + "'";
            Sql = Sql + ",[SectorFrom]= '" + dataValue[2] + "'";


            Sql = Sql + ",[AirlineId]= '" + dataValue[3] + "'";
            Sql = Sql + ",[RoutedFromFlightNo]= '" + dataValue[4] + "'";
            Sql = Sql + ",[RoutedFromDate]= '" + dataValue[5] + "'";
            Sql = Sql + ",[PlaceOfInterruption]= '" + dataValue[6] + "'";

            Sql = Sql + ",[RoutedTo1]= '" + dataValue[7] + "'";
            Sql = Sql + ",[RoutedTo_Airline1]= '" + dataValue[8] + "'";
            Sql = Sql + ",[RoutedTo_FlightNumber1]= '" + dataValue[9] + "'";
            Sql = Sql + ",[RoutedTo_Date1]= '" + dataValue[10] + "'";
            Sql = Sql + ",[RoutedTo_From1]= '" + dataValue[11] + "'";
            Sql = Sql + ",[RoutedTo_To1]= '" + dataValue[12] + "'";

            Sql = Sql + ",[RoutedTo2]= '" + dataValue[13] + "'";
            Sql = Sql + ",[RoutedTo_Airline2]= '" + dataValue[14] + "'";
            Sql = Sql + ",[RoutedTo_FlightNumber2]= '" + dataValue[15] + "'";
            Sql = Sql + ",[RoutedTo_Date2]= '" + dataValue[16] + "'";
            Sql = Sql + ",[RoutedTo_From2]= '" + dataValue[17] + "'";
            Sql = Sql + ",[RoutedTo_To2]= '" + dataValue[18] + "'";

            Sql = Sql + ",[ReasonForIssurance]= '" + dataValue[19] + "'";
            Sql = Sql + ",[DiversionByCarrierCode]= '" + dataValue[20] + "'";
            Sql = Sql + ",[TotalNoOfPassencer]= '" + dataValue[21] + "'";
            Sql = Sql + ",[ValidatorName]= '" + dataValue[22] + "'";
            Sql = Sql + "WHERE  FIMNumber='" + FIMNo + "'; ";
            Sql = Sql + "END" + Environment.NewLine;

            DataSet ds = new DataSet();
            ds = dbconnect.RetObjDS(Sql);

            if (ds.HasErrors == false && SaveFIMDetailData(FIMNo, dataValue1))
            {
                return "Record Save Sucessfully";
            }
            return "Error, Fail To Save Record";

        }

        public bool SaveFIMDetailData(string FimNumber, string[] dataValue)
        {
            string[] tmpArray = new string[13];
            string SqlLk = "Select [Recid] as [FIMHeaderRecId],FIMNumber From  [Pax].[FIMHeaderData]  WHERE  FIMNumber='" + FimNumber + "'; ";
            string A, B, C, D, E, F = "";
            bool tmp = false;
            for (int i = 0; i < dataValue.Length; i++)
            {
                tmpArray = dataValue[i].ToString().Split(',');
                A = tmpArray[0].ToString();
                B = tmpArray[1].ToString();
                C = tmpArray[2].ToString();
                D = tmpArray[3].ToString();
                E = tmpArray[4].ToString();
                F = tmpArray[5].ToString();

                string SqlFIMNumber = "Select FIMNumber From  [Pax].[FIMDetailsEntry]  WHERE  FIMNumber='" + FimNumber + "' And[CouponNumber] ='" + C + "' and RelatedDocumentNumber ='" + E + "'";
                string Sql = "IF NOT EXISTS(" + SqlFIMNumber + ")";
                Sql = Sql + "BEGIN DECLARE @MaxRecIdFIMHeader bigint; DECLARE @MaxRecId bigint;" + Environment.NewLine;
                Sql = Sql + "set @MaxRecIdFIMHeader = (select iif(MAX(RecId) is null,1, MAX(RecId)+1) As MaxLineid from [Pax].[FIMDetailsEntry]) ;" + Environment.NewLine;
                Sql = Sql + "set @MaxRecId = (select iif(MAX(RecId) is null,1, MAX(RecId)+1) As MaxLineid from [Pax].[FIMDetailsEntry]) ;" + Environment.NewLine;
                Sql = Sql + "INSERT INTO [Pax].[FIMDetailsEntry] ([Recid],[FIMHeaderRecId],[FIMNumber],";
                Sql = Sql + "[RecordNumber],[PassengerName] ,[CouponNumber],[AirlineCode],[RelatedDocumentNumber],[CheckDigit],[DocumentType],[FareBasis_PaxTypeCode],[NewFlightNo1],[NewFlightNo2],[Billed1],[Accepted1],[Difference1],[Indicator1])";

                string Sql2 = " VALUES(@MaxRecId,@MaxRecIdFIMHeader,'" + FimNumber + "',";
                string Sql3 = "";

                try { Sql3 = Sql3 + A + ","; }
                catch { Sql3 = Sql3 + "Null,"; }
                try { Sql3 = Sql3 + "'" + B + "',"; }
                catch { Sql3 = Sql3 + "Null,"; }
                try { Sql3 = Sql3 + "'" + C + "',"; }
                catch { Sql3 = Sql3 + "Null,"; }
                try { Sql3 = Sql3 + "'" + D + "',"; }
                catch { Sql3 = Sql3 + "Null,"; }
                try { Sql3 = Sql3 + "'" + E + "',"; }
                catch { Sql3 = Sql3 + "Null,"; }
                try { Sql3 = Sql3 + "'" + F + "',"; }
                catch { Sql3 = Sql3 + "Null,"; }
                try { Sql3 = Sql3 + "'" + tmpArray[6].ToString() + "',"; }
                catch { Sql3 = Sql3 + "Null,"; }
                try { Sql3 = Sql3 + "'" + tmpArray[7].ToString() + "',"; }
                catch { Sql3 = Sql3 + "Null,"; }
                try { Sql3 = Sql3 + "'" + tmpArray[8].ToString() + "',"; }
                catch { Sql3 = Sql3 + "Null,"; }

                try
                {
                    if (tmpArray[9].ToString().Trim().Length > 0)
                    { Sql3 = Sql3 + tmpArray[9].ToString() + ","; }
                    else
                    { Sql3 = Sql3 + "Null,"; }

                }
                catch { Sql3 = Sql3 + "Null,"; }

                // try { Sql3 = Sql3 +  gdPaxDetail[10, i].Value.ToString() + ","; } catch { Sql3 = Sql3 + "Null,"; }
                try
                {
                    if (tmpArray[10].ToString().Trim().Length > 0)
                    { Sql3 = Sql3 + tmpArray[10].ToString() + ","; }
                    else
                    { Sql3 = Sql3 + "Null,"; }

                }
                catch { Sql3 = Sql3 + "Null,"; }

                //  try { Sql3 = Sql3 +  gdPaxDetail[11, i].Value.ToString() + ","; } catch { Sql3 = Sql3 + "Null,"; }
                try
                {
                    if (tmpArray[11].ToString().Trim().Length > 0)
                    { Sql3 = Sql3 + tmpArray[11].ToString() + ","; }
                    else
                    { Sql3 = Sql3 + "Null,"; }

                }
                catch { Sql3 = Sql3 + "Null,"; }

                // try { Sql3 = Sql3 + gdPaxDetail[12, i].Value.ToString() + ","; } catch { Sql3 = Sql3 + "Null,"; }
                try
                {
                    if (tmpArray[12].ToString().Trim().Length > 0)
                    { Sql3 = Sql3 + tmpArray[12].ToString() + ","; }
                    else
                    { Sql3 = Sql3 + "Null,"; }

                }
                catch { Sql3 = Sql3 + "Null,"; }

                Sql3 = Sql3 + "0)" + Environment.NewLine;
                Sql3 = Sql3 + "END" + Environment.NewLine;
                Sql3 = Sql3 + "ELSE" + Environment.NewLine;
                Sql3 = Sql3 + "BEGIN" + Environment.NewLine;
                Sql3 = Sql3 + "UPDATE [Pax].[FIMDetailsEntry]";
                Sql3 = Sql3 + " SET ";
                try { Sql3 = Sql3 + "[RecordNumber]= " + A + ""; }
                catch { }
                try { Sql3 = Sql3 + ",[PassengerName]= '" + B + "'"; }
                catch { }
                try { Sql3 = Sql3 + ",[CouponNumber]= '" + C + "'"; }
                catch { }
                try { Sql3 = Sql3 + ",[AirLineCode]= '" + D + "'"; }
                catch { }
                try { Sql3 = Sql3 + ",[RelatedDocumentNumber]= '" + E + "'"; }
                catch { }
                try { Sql3 = Sql3 + ",[CheckDigit]= '" + F + "'"; }
                catch { }
                try { Sql3 = Sql3 + ",[DocumentType]= '" + tmpArray[6].ToString() + "'"; }
                catch { }
                try { Sql3 = Sql3 + ",[FareBasis_PaxTypeCode]= '" + tmpArray[7].ToString() + "'"; }
                catch { }
                try { Sql3 = Sql3 + ",[NewFlightNo1]= '" + tmpArray[8].ToString() + "'"; }
                catch { }
                try { Sql3 = Sql3 + ",[NewFlightNo2]= '" + tmpArray[9].ToString() + "'"; }
                catch { }


                if (tmpArray[10].ToString() != null)
                {
                    if (tmpArray[10].ToString().Trim().Length > 0)
                    {
                        Sql3 = Sql3 + ",[Billed1]= " + tmpArray[10].ToString();
                    }
                };
                if (tmpArray[11].ToString() != null)
                {
                    if (tmpArray[11].ToString().Trim().Length > 0)
                    {
                        Sql3 = Sql3 + ",[Accepted1]= " + tmpArray[11].ToString();
                    }
                };
                if (tmpArray[12].ToString() != null)
                {
                    if (tmpArray[12].ToString().Trim().Length > 0)
                    {
                        Sql3 = Sql3 + ",[Difference1]= " + tmpArray[12].ToString();
                    }
                };

                Sql3 = Sql3 + " WHERE  FIMNumber='" + FimNumber + "' And[CouponNumber] ='" + C + "' and RelatedDocumentNumber ='" + E + "'"; ;
                Sql3 = Sql3 + " END" + Environment.NewLine;
                string rquete = Sql + Sql2 + Sql3;
                DataSet ds = new DataSet();
                ds = dbconnect.RetObjDS(rquete);
                if (ds.HasErrors == false)
                {
                    tmp = true; ;
                }
                else
                {
                    tmp = false;
                }
            }
            return tmp;

        }
       /* public ActionResult FOPOthers()
        {
            string dateFrom = DateTime.Now.AddMonths(-1).ToString("dd-MMM-yyyy");
            string dateTo = DateTime.Now.ToString("dd-MMM-yyyy");
            string[] date = new string[2] { dateFrom, dateTo };
            ViewBag.date = date;
            return PartialView();
        }*/
        /**********************************end************************************/
        public ActionResult RetroactiveAdjustement()
        {
            return View("RetroactiveAdjustement");
        }

        /*************************************************************************/
        public string setListeRetroactiveAdjustement(string[] dataValue)
        {
            string response = "";
            string sql = "Select DocumentNumber From[Pax].[SalesDocumentHeader] WHERE DocumentNumber = '" + dataValue[1] + "'";
            DataSet ds1 = new DataSet();
            ds1 = dbconnect.RetObjDS(sql);
            if (ds1.HasErrors == false)
            {
                string sql1 = "Select FORMAT(DateOfIssue,'dd-MM-yyyy') as DateOfIssue,FareBasisTicketDesignator,FareCalculationArea,PassengerName,FORMAT(NotValidAfter,'dd-MM-yyyy') as NotValidAfter From[Pax].[VW_SalesCouponDetail] WHERE DocumentNumber = '" + dataValue[1] + "'";
                DataSet ds = new DataSet();
                ds = dbconnect.RetObjDS(sql1);
                foreach (DataRow dr in ds.Tables[0].Rows)
                {

                    response = dr[ds.Tables[0].Columns[0].ColumnName].ToString() + "$" + dr[ds.Tables[0].Columns[1].ColumnName].ToString() + "$" + dr[ds.Tables[0].Columns[2].ColumnName].ToString() + "$" + dr[ds.Tables[0].Columns[3].ColumnName].ToString() + "$" + dr[ds.Tables[0].Columns[4].ColumnName].ToString();

                }
                return response;
            }
            else
            {

                return response;
            }
        }


        public string setListeRetroactiveAdjustementName(string[] dataValue)
        {
            string response = "";
            string a = dataValue[1];
            string sql1 = "Select FORMAT(DateOfIssue,'dd-MM-yyyy') as DateOfIssue,FareBasisTicketDesignator,FareCalculationArea,DocumentNumber,FORMAT(NotValidAfter,'dd-MM-yyyy') as NotValidAfter From[Pax].[VW_SalesCouponDetail] WHERE PassengerName = '" + dataValue[0] + "'";
            DataSet ds = new DataSet();
            ds = dbconnect.RetObjDS(sql1);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {

                response = dr[ds.Tables[0].Columns[0].ColumnName].ToString() + "$" + dr[ds.Tables[0].Columns[1].ColumnName].ToString() + "$" + dr[ds.Tables[0].Columns[2].ColumnName].ToString() + "$" + dr[ds.Tables[0].Columns[3].ColumnName].ToString() + "$" + dr[ds.Tables[0].Columns[4].ColumnName].ToString();

            }
            return response;
        }

        /*************************Begin IndustryRemuneration*********************/
        public ActionResult IndustryRemuneration()
        {
            string dateFrom = DateTime.Now.AddMonths(-1).ToString("dd-MMM-yyyy");
            string dateTo = DateTime.Now.ToString("dd-MMM-yyyy");
            string[] date = new string[2] { dateFrom, dateTo };
            ViewBag.date = date;
            string sql = "select distinct AgentNumericCode  from [Agent].[PLBHeader]";
            SqlConnection con = new SqlConnection(pbConnectionString);
            DataSet ds = new DataSet();
            SqlDataAdapter ada = new SqlDataAdapter(sql, con);
            ada.Fill(ds);
            con.Close();
            int lonAg = ds.Tables[0].Rows.Count;
            string[] AgCode = new string[lonAg];
            int i = 0;
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                AgCode[i] = dr[ds.Tables[0].Columns[0].ColumnName].ToString();
                i = i + 1;
            }

            ViewBag.AgCode = AgCode;
            ViewBag.lonAg = lonAg;
            return PartialView();

        }
        public ActionResult clearcom()
        {
            string dateFrom = DateTime.Now.AddMonths(-1).ToString("dd-MMM-yyyy");
            string dateTo = DateTime.Now.ToString("dd-MMM-yyyy");
            string[] date = new string[2] { dateFrom, dateTo };
            ViewBag.date = date;
            return PartialView();
        }

        public ActionResult clearbonus()
        {
            string dateFrom = DateTime.Now.AddMonths(-1).ToString("dd-MMM-yyyy");
            string dateTo = DateTime.Now.ToString("dd-MMM-yyyy");
            string[] date = new string[2] { dateFrom, dateTo };
            ViewBag.date = date;
            string sql = "select distinct AgentNumericCode  from [Agent].[PLBHeader]";
            SqlConnection con = new SqlConnection(pbConnectionString);
            DataSet ds = new DataSet();
            SqlDataAdapter ada = new SqlDataAdapter(sql, con);
            ada.Fill(ds);
            con.Close();
            int lonAg = ds.Tables[0].Rows.Count;
            string[] AgCode = new string[lonAg];
            int i = 0;
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                AgCode[i] = dr[ds.Tables[0].Columns[0].ColumnName].ToString();
                i = i + 1;
            }

            ViewBag.AgCode = AgCode;
            ViewBag.lonAg = lonAg;
            return PartialView();
        }
        public ActionResult LoadCom()
        {
            string dateFrom1 = Request["dateFrom"];
            string dtpFrom = ConvertDate(dateFrom1);
            string dateTo1 = Request["dateTo"];
            string dtpTo = ConvertDate(dateTo1);
            string cboAgent = Request["agCode"];
            string paramAgen = null;
            var culture = System.Globalization.CultureInfo.CurrentCulture;
            if (cboAgent == "-All-")
            {
                paramAgen = "%";
            }
            else
            {
                paramAgen = cboAgent;
            }


            SqlConnection con = new SqlConnection(pbConnectionString);
            SqlCommand cmd = new SqlCommand("[Pax].[SP_comission]", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 300;
            cmd.Parameters.AddWithValue("@fromDate", dtpFrom);
            cmd.Parameters.AddWithValue("@toDate", dtpTo);
            cmd.Parameters.AddWithValue("@agent", paramAgen);
            DataSet ds = new DataSet();
            SqlDataAdapter ada = new SqlDataAdapter(cmd);
            ada.Fill(ds);

            int lon = ds.Tables[0].Rows.Count;
            string[,] dbgComm = new string[9, lon];
            int i = 0;
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                dbgComm[0, i] = dr[ds.Tables[0].Columns[0].ColumnName].ToString();
                try
                {
                    dbgComm[1, i] = Convert.ToDateTime(dr[ds.Tables[0].Columns[1].ColumnName].ToString()).ToShortDateString();
                }
                catch
                {
                }

                dbgComm[2, i] = dr[ds.Tables[0].Columns[2].ColumnName].ToString();
                dbgComm[3, i] = dr[ds.Tables[0].Columns[3].ColumnName].ToString();

                dbgComm[4, i] = dr[ds.Tables[0].Columns[4].ColumnName].ToString();
                dbgComm[5, i] = dr[ds.Tables[0].Columns[7].ColumnName].ToString();
                dbgComm[6, i] = dr[ds.Tables[0].Columns[8].ColumnName].ToString();
                dbgComm[7, i] = dr[ds.Tables[0].Columns[9].ColumnName].ToString();
                dbgComm[8, i] = dr[ds.Tables[0].Columns[10].ColumnName].ToString();

                i++;
            }

            string[] cll = new string[lon];

            for (int g = 0; g < lon; g++)
            {
                cll[g] = dbgComm[5, g].ToString();
            }

            string[] b = cll.Distinct().ToArray();
            string[,] dgsum = new string[3, b.Length];
            for (int y = 0; y < b.Length; y++)
            {


                try
                {
                    dgsum[0, y] = b[y].ToString();
                }
                catch
                {
                }
            }

            string[] sum = new string[b.Length];
            string[] cnt = new string[b.Length];

            for (int o = 0; o < b.Length; o++)
            {
                for (int k = 0; k < lon; k++)
                {
                    if (dgsum[0, o].ToString() == dbgComm[5, k].ToString())
                    {
                        if (sum[o] == null)
                        {
                            sum[o] = "0";
                        }
                        sum[o] = (Convert.ToDecimal(sum[o].ToString(), System.Globalization.CultureInfo.InvariantCulture) + Convert.ToDecimal(dbgComm[6, k], System.Globalization.CultureInfo.InvariantCulture)).ToString();

                        if (cnt[o] == null)
                        {
                            cnt[o] = "0";
                        }

                        cnt[o] = (Convert.ToDecimal(cnt[o].ToString(), System.Globalization.CultureInfo.InvariantCulture) + 1).ToString();
                    }
                }
            }

            for (int yy = 0; yy < b.Length; yy++)
            {
                dgsum[1, yy] = sum[yy].ToString();
                dgsum[2, yy] = cnt[yy].ToString();
            }

            string sumUSD = "0";

            for (int h = 0; h < lon; h++)
            {
                try
                {
                    sumUSD = (Convert.ToDouble(sumUSD, new CultureInfo(culture.Name)) + Convert.ToDouble(dbgComm[8, h], new CultureInfo(culture.Name))).ToString("#.00");
                }
                catch
                {
                }
            }
            ViewBag.compt = lon;
            ViewBag.dbgComm = dbgComm;
            ViewBag.sumUSD = sumUSD;
            ViewBag.Count = cnt;
            ViewBag.Count = b.Length;
            ViewBag.Sum = dgsum;

            return PartialView();
        }

        public ActionResult getAgCode()
        {
            string dateFrom1 = Request["dateFrom"];
            string dateFrom = ConvertDate(dateFrom1);
            string dateTo1 = Request["dateTo"];
            string dateTo = ConvertDate(dateTo1);
            string sql = "SELECT distinct AgentNumericCode FROM pax.VW_SalesHeader where SaleDate between  '" + dateFrom + "'  and '" + dateTo + "'  ORDER by 1 desc";
            DataSet dss = new DataSet();
            dss = dbconnect.RetObjDS(sql);
            return PartialView("LoadTransactionCode", dss);
        }

        public ActionResult getFinP()
        {
            string ag = Request["agCode"];
            string sql1 = "select distinct [FinancialYear]  from [Agent].[PLBHeader] where AgentNumericCode = '" + ag + "' ";
            SqlConnection con = new SqlConnection(pbConnectionString);
            DataSet ds1 = new DataSet();
            SqlDataAdapter ada1 = new SqlDataAdapter(sql1, con);
            ada1.Fill(ds1);
            con.Close();
            int lon = ds1.Tables[0].Rows.Count;
            string[] finPer = new string[lon];
            int j = 0;
            foreach (DataRow dr in ds1.Tables[0].Rows)
            {
                finPer[j] = dr[ds1.Tables[0].Columns[j].ColumnName].ToString();
                j = j + 1;
            }
            return PartialView("LoadTransactionCode", ds1);
        }

        public ActionResult loadBonus(string ag, string yr)
        {
            //string ag = Request["agCode"];
            //string yr = Request["finYear"];
            string dateFrom = DateTime.Now.AddMonths(-1).ToString("dd-MMM-yyyy");
            string dateTo = DateTime.Now.ToString("dd-MMM-yyyy");
            string[] date = new string[2] { dateFrom, dateTo };
            ViewBag.date = date;
            string agc = "";
            string yrc = "";
            if (ag == "-All-")
            {
                agc = "%";
            }
            else
            {
                agc = ag;
            }
            if (yr == "-All-")
            {
                yrc = "%";
            }
            else
            {
                yrc = yr;
            }
            string sql = "SELECT *  FROM [Agent].[PLBHeader] where AgentNumericCode like '" + agc + "' and FinancialYear like '" + yrc + "'";
            SqlConnection con = new SqlConnection(pbConnectionString);
            DataSet ds = new DataSet();
            SqlDataAdapter ada = new SqlDataAdapter(sql, con);
            ada.Fill(ds);
            con.Close();
            int row = 0;
            int lon = ds.Tables[0].Rows.Count;
            if (lon == 0)
            {
                row = lon + 1;
            }
            else
            {
                row = lon;
            }
            string[,] dgPLBHeader = new string[12, row];


            int t = 0;
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                for (int y = 0; y < 12; y++)
                {

                    if ((y == 2) || (y == 3))
                    {
                        string h = null;
                        string hh = null;

                        h = dr[ds.Tables[0].Columns[y].ColumnName].ToString();
                        if (h != "")
                        {
                            hh = h.Substring(0, 11).ToString();
                            dgPLBHeader[y, t] = hh;
                        }
                        else
                        {
                            dgPLBHeader[y, t] = date[1];
                        }

                    }

                    else
                    {

                        dgPLBHeader[y, t] = dr[ds.Tables[0].Columns[y].ColumnName].ToString();
                    }
                }
                t++;
            }



            ViewBag.dgPLBHeader = dgPLBHeader;

            ViewBag.compt1 = lon;

            return PartialView();
        }

        public ActionResult addBonus()
        {
            string agCode = Request["agCode"];
            // string ag = Request["BonusAgent"];
            //string yr = Request["FinancialPeriod"];
            string finYear = Request["finYear"];
            string dateFrom = Request["dateFrom"];
            string dateTo = Request["dateTo"];
            string bonusBase = Request["bonusBase"];
            string bonusCur = Request["bonusCur"];
            string bonusPayable = Request["bonusPayable"];


            string[,] dgPLBHeader = new string[12, 1];
            //string[,] tmpdgPLBHeader = new string[12, rowadd];


            dgPLBHeader[0, 0] = agCode;
            dgPLBHeader[1, 0] = finYear;
            dgPLBHeader[2, 0] = dateFrom;
            dgPLBHeader[3, 0] = dateTo;
            dgPLBHeader[4, 0] = bonusPayable;
            dgPLBHeader[5, 0] = "";
            dgPLBHeader[6, 0] = bonusCur;
            dgPLBHeader[7, 0] = bonusBase;
            dgPLBHeader[5, 0] = "";
            dgPLBHeader[5, 0] = "";
            dgPLBHeader[5, 0] = "";
            ViewBag.dgPLBHeader = dgPLBHeader;
            return PartialView();
        }

        public ActionResult saveOperation()
        {
            string agCode = Request["agCode"];
            string finYear = Request["finYear"];
            string dateFrom1 = Request["dateFrom"];
            string dateFrom = dateFrom1;
            string dateTo1 = Request["dateTo"];
            string dateTo = dateTo1;
            string bonusBase = Request["bonusBase"];
            string bonusCur = Request["bonusCur"];
            string bonusPayable = Request["bonusPayable"];
            string action = Request["action"];
            string ag = Request["ag"];
            string yr = Request["yr"];
            if (action == "Save")
            {
                SqlDataAdapter dap = new SqlDataAdapter();

                string sql = "";

                sql = sql + "INSERT INTO [Agent].[PLBHeader] Values ( '" + agCode + "'"; // AgentNumericCode
                sql = sql + ",'" + finYear + "',"; //Financial Year
                sql = sql + "'" + dateFrom + "',"; //start financial
                sql = sql + "'" + dateTo + "',";//end financial
                sql = sql + "'" + bonusPayable + "',"; //payable
                sql = sql + "NULL,";// flownUSD
                sql = sql + "'" + bonusCur + "',";//curr
                sql = sql + "'" + bonusBase + "',"; //baseamt
                sql = sql + "NULL,";// flownrevenue
                sql = sql + "NULL,";// AmtCal
                sql = sql + "NULL,";// Amtpaid
                sql = sql + "NULL )"; //Datepaid
                SqlConnection con = new SqlConnection(pbConnectionString);
                DataSet ds = new DataSet();
                SqlDataAdapter ada = new SqlDataAdapter(sql, con);
                ada.Fill(ds);
                con.Close();

            }
            else if (action == "Update")
            {
                string sql = "";

                sql = sql + "UPDATE [Agent].[PLBHeader] SET " + Environment.NewLine;
                sql = sql + "  [AgentNumericCode] = '" + agCode + "'" + Environment.NewLine;
                sql = sql + " , [FinancialYear]   = '" + finYear + "'" + Environment.NewLine;
                sql = sql + " , [FinYearFromDate] = '" + dateFrom + "'" + Environment.NewLine;
                sql = sql + " , [FinYearToDate]   = '" + dateTo + "'" + Environment.NewLine;
                sql = sql + " , [Payable]         = '" + bonusPayable + "'" + Environment.NewLine;
                sql = sql + " , [FlownRevenueUSD] = NULL " + Environment.NewLine;
                sql = sql + " , [Currency]        = '" + bonusCur + "'" + Environment.NewLine;
                sql = sql + " , [BaseAmount]      = '" + bonusBase + "'" + Environment.NewLine;
                sql = sql + " , [FlownRevenue]    = NULL" + Environment.NewLine;
                sql = sql + " , [AmountCalculated]= NULL " + Environment.NewLine;
                sql = sql + " , [AmountPaid]      = NULL " + Environment.NewLine;
                sql = sql + " , [DatePaid]        = NULL " + Environment.NewLine;

                sql = sql + " where [AgentNumericCode]  = '" + agCode + "' and  " + Environment.NewLine;
                sql = sql + "  [FinancialYear] = '" + finYear + "'  " + Environment.NewLine;
                sql = sql.Replace("'Null'", "Null");
                SqlConnection con = new SqlConnection(pbConnectionString);
                DataSet ds = new DataSet();
                SqlDataAdapter ada = new SqlDataAdapter(sql, con);
                ada.Fill(ds);
                con.Close();
            }

            string sql1 = "SELECT *  FROM [Agent].[PLBHeader] where AgentNumericCode like '" + agCode + "' and FinancialYear like '" + finYear + "'";
            SqlConnection con1 = new SqlConnection(pbConnectionString);
            DataSet ds1 = new DataSet();
            SqlDataAdapter ada1 = new SqlDataAdapter(sql1, con1);
            ada1.Fill(ds1);
            con1.Close();
            int row = 0;
            int lon = ds1.Tables[0].Rows.Count;
            if (lon == 0)
            {
                row = lon + 1;
            }
            else
            {
                row = lon;
            }
            string[,] dgPLBHeader = new string[12, row];


            int t = 0;
            foreach (DataRow dr in ds1.Tables[0].Rows)
            {
                for (int y = 0; y < 12; y++)
                {

                    if ((y == 2) || (y == 3))
                    {
                        string h = null;
                        string hh = null;

                        h = dr[ds1.Tables[0].Columns[y].ColumnName].ToString();
                        if (h != "")
                        {
                            hh = h.Substring(0, 11).ToString();
                            dgPLBHeader[y, t] = hh;
                        }

                    }

                    else
                    {

                        dgPLBHeader[y, t] = dr[ds1.Tables[0].Columns[y].ColumnName].ToString();
                    }
                }
            }

            t++;
            /*string sql2 = "SELECT *  FROM [Agent].[PLBDetail] where AgentNumericCode = '" + ag + "' ";
            DataSet ds2 = new DataSet();
            SqlDataAdapter ada2 = new SqlDataAdapter(sql1, con1);
            ada2.Fill(ds2);
            con1.Close();
            int lon1 = ds2.Tables[0].Rows.Count;
            // 
            int row1 = 0;
            if (lon1 == 0)
            {
                row1 = lon1 + 1;
            }
            else
            {
                row1 = lon1;
            }
            string[,] dgvDetails = new string[4, row1];
            int i = 0;
            for (int y = 0; y < 4; y++)
            {

                dgvDetails[y, i] = ds2.Tables[0].Columns[y].ColumnName.ToString();
            }
            i++;*/
            ViewBag.dgPLBHeader = dgPLBHeader;
            // ViewBag.dgvDetails = dgvDetails;
            ViewBag.compt1 = lon;
            //ViewBag.compt2 = lon1;
            return PartialView();
        }

        public ActionResult saveOperation2()
        {
            string agCode = Request["agCode"];
            string finYear = Request["finYear"];
            string plbrange = Request["plbrange"];
            string plbper = Request["plbper"];
            string seqNume = seqNum(agCode, finYear);
            string action = Request["action"];
            if (action == "Update")
            {
                string sql = "";

                sql = sql + "UPDATE [Agent].[PLBDetail] SET " + Environment.NewLine;
                sql = sql + "  [PLBRange] = '" + plbrange + "'" + Environment.NewLine;
                sql = sql + " , [PLBPercentage]   = '" + plbper + "'" + Environment.NewLine;
                sql = sql + " where [AgentNumericCode]  = '" + agCode + "' and  " + Environment.NewLine;
                sql = sql + "  [FinancialYear] = '" + finYear + "' and [Sequence] = '" + seqNume + "'  " + Environment.NewLine;
                sql = sql.Replace("'Null'", "Null");
                SqlConnection con = new SqlConnection(pbConnectionString);
                DataSet ds = new DataSet();
                SqlDataAdapter ada = new SqlDataAdapter(sql, con);
                ada.Fill(ds);
                con.Close();
            }
            else
            {
                string sql = "";

                sql = sql + "INSERT INTO [Agent].[PLBDetail] Values ( '" + agCode.Trim() + "'"; // AgentNumericCode
                sql = sql + ",'" + finYear.Trim() + "',"; //Financial Year
                sql = sql + "'" + seqNume + "',"; //SEQ
                sql = sql + "'" + plbrange + "',";//Range
                sql = sql + "'" + plbper + "',"; //Percentage
                sql = sql + "NULL,";// Amtpaid
                sql = sql + "NULL )"; //Datepaid
                SqlConnection con = new SqlConnection(pbConnectionString);
                DataSet ds = new DataSet();
                SqlDataAdapter ada = new SqlDataAdapter(sql, con);
                ada.Fill(ds);
                con.Close();
            }

            string sql2 = "SELECT *  FROM [Agent].[PLBDetail] where AgentNumericCode = '" + agCode.Trim() + "' ";
            DataSet ds2 = new DataSet();
            SqlConnection con1 = new SqlConnection(pbConnectionString);
            SqlDataAdapter ada2 = new SqlDataAdapter(sql2, con1);
            ada2.Fill(ds2);
            con1.Close();
            int lon1 = ds2.Tables[0].Rows.Count;
            // 
            int row1 = 0;
            if (lon1 == 0)
            {
                row1 = lon1 + 1;
            }
            else
            {
                row1 = lon1;
            }
            string[,] dgvDetails = new string[7, row1];
            int i = 0;
            foreach (DataRow dr in ds2.Tables[0].Rows)
            {
                for (int y = 0; y < 7; y++)
                {

                    dgvDetails[y, i] = dr[ds2.Tables[0].Columns[y].ColumnName].ToString();
                }
                i++;
            }
            ViewBag.compt2 = lon1;
            ViewBag.dgvDetails = dgvDetails;

            return PartialView();

        }

        public string seqNum(string agCode, string finYear)
        {

            string sql = "";
            sql = sql + "SELECT iif(MAX([Sequence]) is null,1,MAX([Sequence])+1) as XRefId FROM [Agent].[PLBDetail] WHERE [AgentNumericCode] = '" + agCode.Trim() + "'";
            sql = sql + " AND [FinancialYear] = '" + finYear.Trim() + "'";
            SqlConnection con = new SqlConnection(pbConnectionString);
            DataSet ds = new DataSet();
            SqlDataAdapter ada = new SqlDataAdapter(sql, con);
            ada.Fill(ds);
            con.Close();
            string seq = "";
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                seq = dr[ds.Tables[0].Columns[0].ColumnName].ToString();
            }

            return seq;
        }

        public ActionResult showDetail()
        {
            string agc = Request["agcode"];
            string sql1 = "SELECT *  FROM [Agent].[PLBDetail] where AgentNumericCode = '" + agc + "' ";
            SqlConnection con = new SqlConnection(pbConnectionString);
            DataSet ds1 = new DataSet();
            SqlDataAdapter ada1 = new SqlDataAdapter(sql1, con);
            ada1.Fill(ds1);
            con.Close();
            int lon1 = ds1.Tables[0].Rows.Count;
            // 
            int row1 = 0;
            if (lon1 == 0)
            {
                row1 = lon1 + 1;
            }
            else
            {
                row1 = lon1;
            }
            string[,] dgvDetails = new string[7, row1];
            int i = 0;
            foreach (DataRow dr in ds1.Tables[0].Rows)
            {
                for (int y = 0; y < 7; y++)
                {

                    dgvDetails[y, i] = dr[ds1.Tables[0].Columns[y].ColumnName].ToString();
                }
                i++;
            }

            ViewBag.dgvDetails = dgvDetails;
            ViewBag.compt2 = lon1;

            return PartialView();
        }

        public ActionResult deleteDetail()
        {
            var culture = System.Globalization.CultureInfo.CurrentCulture;
            // decimal interm2 = Convert.ToDecimal(Rfn[o].ToString(), );
            string agcode = Request["agcode"];
            string finYr = Request["finYr"];
            string range = Request["range"];
            double percent = Convert.ToDouble(Request["percent"], new CultureInfo(culture.Name));

            string sql = "";
            sql = sql + "Delete FROM [Agent].[PLBDetail] where AgentNumericCode = '" + agcode + "' and [FinancialYear] = '" + finYr + "' and [PLBRange] = '" + range + "' and  [PLBPercentage] = '" + percent + "' ";
            SqlConnection con = new SqlConnection(pbConnectionString);
            DataSet ds = new DataSet();
            SqlDataAdapter ada = new SqlDataAdapter(sql, con);
            ada.Fill(ds);
            con.Close();
            string sql1 = "SELECT *  FROM [Agent].[PLBDetail] where AgentNumericCode = '" + agcode + "' ";
            //SqlConnection con = new SqlConnection(pbConnectionString);
            DataSet ds1 = new DataSet();
            SqlDataAdapter ada1 = new SqlDataAdapter(sql1, con);
            ada1.Fill(ds1);
            con.Close();
            int lon1 = ds1.Tables[0].Rows.Count;
            // 
            int row1 = 0;
            if (lon1 == 0)
            {
                row1 = lon1 + 1;
            }
            else
            {
                row1 = lon1;
            }
            string[,] dgvDetails = new string[7, row1];
            int i = 0;
            foreach (DataRow dr in ds1.Tables[0].Rows)
            {
                for (int y = 0; y < 7; y++)
                {

                    dgvDetails[y, i] = dr[ds1.Tables[0].Columns[y].ColumnName].ToString();
                }
                i++;
            }

            ViewBag.dgvDetails = dgvDetails;
            ViewBag.compt2 = lon1;
            return PartialView();
        }

        public ActionResult deleteHead()
        {
            string ag = Request["ag"];
            string yr = Request["yr"];
            string agcode = Request["agcode"];
            string finYr = Request["finYr"];
            string sql1 = "";
            sql1 = sql1 + "Delete FROM [Agent].[PLBHeader] where AgentNumericCode = '" + agcode + "' and [FinancialYear] = '" + finYr + "'  ";
            SqlConnection con = new SqlConnection(pbConnectionString);
            DataSet ds1 = new DataSet();
            SqlDataAdapter ada1 = new SqlDataAdapter(sql1, con);
            ada1.Fill(ds1);
            con.Close();
            string agc = "";
            string yrc = "";
            if (ag == "-All-")
            {
                agc = "%";
            }
            else
            {
                agc = ag;
            }
            if (yr == "-All-")
            {
                yrc = "%";
            }
            else
            {
                yrc = yr;
            }
            string sql = "SELECT *  FROM [Agent].[PLBHeader] where AgentNumericCode like '" + agc + "' and FinancialYear like '" + yrc + "'";
            //SqlConnection con = new SqlConnection(pbConnectionString);
            DataSet ds = new DataSet();
            SqlDataAdapter ada = new SqlDataAdapter(sql, con);
            ada.Fill(ds);
            con.Close();
            int row = 0;
            int lon = ds.Tables[0].Rows.Count;
            if (lon == 0)
            {
                row = lon + 1;
            }
            else
            {
                row = lon;
            }
            string[,] dgPLBHeader = new string[12, row];


            int t = 0;
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                for (int y = 0; y < 12; y++)
                {

                    if ((y == 2) || (y == 3))
                    {
                        string h = null;
                        string hh = null;

                        h = dr[ds.Tables[0].Columns[y].ColumnName].ToString();
                        if (h != "")
                        {
                            hh = h.Substring(0, 11).ToString();
                            dgPLBHeader[y, t] = hh;
                        }

                    }

                    else
                    {

                        dgPLBHeader[y, t] = dr[ds.Tables[0].Columns[y].ColumnName].ToString();
                    }
                }
                t++;
            }



            ViewBag.dgPLBHeader = dgPLBHeader;

            ViewBag.compt1 = lon;

            return PartialView();

        }

        public ActionResult launchRefresh()
        {
            string dateFrom = Request["datefrom"];
            string dateTo = Request["dateto"];
            string[] date = new string[2] { dateFrom, dateTo };
            ViewBag.date = date;
            string ag = Request["ag"];
            string yr = Request["yr"];
            string agcode = Request["agcode"];
            string finYr = Request["finYr"];
            string amt = Request["amt"];
            string payable = Request["payable"];
            string cur = Request["cur"];
            string[] headDetail = new string[7];
            headDetail[0] = agcode;
            headDetail[1] = finYr;
            headDetail[2] = dateFrom;
            headDetail[3] = dateTo;
            headDetail[4] = amt;
            headDetail[5] = payable;
            headDetail[6] = cur;
            ViewBag.headDetail = headDetail;
            string agc = "";
            string yrc = "";
            if (ag == "-All-")
            {
                agc = "%";
            }
            else
            {
                agc = ag;
            }
            if (yr == "-All-")
            {
                yrc = "%";
            }
            else
            {
                yrc = yr;
            }

            string sql = "SELECT *  FROM [Agent].[PLBHeader] where AgentNumericCode like '" + agc + "' and FinancialYear like '" + yrc + "'";
            SqlConnection con = new SqlConnection(pbConnectionString);
            DataSet ds = new DataSet();
            SqlDataAdapter ada = new SqlDataAdapter(sql, con);
            ada.Fill(ds);
            con.Close();
            int row = 0;
            int lon = ds.Tables[0].Rows.Count;
            if (lon == 0)
            {
                row = lon + 1;
            }
            else
            {
                row = lon;
            }
            string[,] dgPLBHeader = new string[12, row];


            int t = 0;
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                for (int y = 0; y < 12; y++)
                {

                    if ((y == 2) || (y == 3))
                    {
                        string h = null;
                        string hh = null;

                        h = dr[ds.Tables[0].Columns[y].ColumnName].ToString();
                        if (h != "")
                        {
                            hh = h.Substring(0, 11).ToString();
                            dgPLBHeader[y, t] = hh;
                        }

                    }

                    else
                    {

                        dgPLBHeader[y, t] = dr[ds.Tables[0].Columns[y].ColumnName].ToString();
                    }
                }
                t++;
            }
            ViewBag.dgPLBHeader = dgPLBHeader;
            ViewBag.compt1 = lon;
            string sql1 = "SELECT *  FROM [Agent].[PLBDetail] where AgentNumericCode = '" + agcode + "' ";
            //SqlConnection con = new SqlConnection(pbConnectionString);
            DataSet ds1 = new DataSet();
            SqlDataAdapter ada1 = new SqlDataAdapter(sql1, con);
            ada1.Fill(ds1);
            con.Close();
            int lon1 = ds1.Tables[0].Rows.Count;
            // 
            int row1 = 0;
            if (lon1 == 0)
            {
                row1 = lon1 + 1;
            }
            else
            {
                row1 = lon1;
            }
            string[,] dgvDetails = new string[7, row1];
            int i = 0;
            foreach (DataRow dr in ds1.Tables[0].Rows)
            {
                for (int y = 0; y < 7; y++)
                {

                    dgvDetails[y, i] = dr[ds1.Tables[0].Columns[y].ColumnName].ToString();
                }
                i++;
            }

            ViewBag.dgvDetails = dgvDetails;
            ViewBag.compt2 = lon1;

            return View();
        }
        /*********************************end************************************/
        /***************************Beging ISCReclaimOAL*************************/
        public ActionResult ISCReclaimOAL()
        {
            string dateFrom = DateTime.Now.AddMonths(-1).ToString("dd-MMM-yyyy");
            string dateTo = DateTime.Now.ToString("dd-MMM-yyyy");
            string[] date = new string[2] { dateFrom, dateTo };
            ViewBag.date = date;
            return PartialView();
        }

        public ActionResult loadIscReclaim()
        {
            string dateFrom1 = Request["dateFrom"];
            string dateFrom = ConvertDate(dateFrom1);
            string dateTo1 = Request["dateTo"];
            string dateTo = ConvertDate(dateTo1);
            string sql = "Select * from [Ib].[Isc] WHERE DateProcessed BETWEEN '" + dateFrom + "' AND '" + dateTo + "' ";
            SqlConnection con = new SqlConnection(pbConnectionString);
            DataSet ds = new DataSet();
            SqlDataAdapter ada = new SqlDataAdapter(sql, con);
            ada.Fill(ds);
            con.Close();
            int row = 0;
            int lon = ds.Tables[0].Rows.Count;
            if (lon == 0)
            {
                row = lon + 1;
            }
            else
            {
                row = lon;
            }
            string[,] tabISC = new string[8, row];
            int i = 0;
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                for (int y = 0; y < 8; y++)
                {
                    tabISC[y, i] = dr[ds.Tables[0].Columns[y].ColumnName].ToString();
                }
                i++;
            }

            ViewBag.tabISC = tabISC;
            ViewBag.compteur = lon;
            return PartialView();

        }

        /*********************************end************************************/

        /***************INADOalCostSharing****************************/

        public ActionResult INADOalCostSharing()
        {
            ViewBag.dateFrom = DateTime.Now.AddMonths(-1).ToString("dd-MMM-yyyy");
            ViewBag.dateTo = DateTime.Now.ToString("dd-MMM-yyyy");
            return PartialView();
        }
        public ActionResult LoadINADOalCostSharing()
        {
            var culture = System.Globalization.CultureInfo.CurrentCulture;
            string dateFrom1 = Request["dateFrom"];
            string dateFrom = ConvertDate(dateFrom1);
            string dateTo1 = Request["dateTo"];
            string dateTo = ConvertDate(dateTo1);
            string sql = "select f2.FlightDepartureDate, f1.HdrGuid,f1.DocumentNumber, f1.PassengerName, f3.InConnectionWithDocumentNumber AS OriginalDocumentNumber, f2.OriginCityCode, f2.Carrier, f2.DestinationCityCode, f2.FinalShare" + Environment.NewLine;
            sql = sql + " , ( select top 1  Carrier from Pax.fn_SalesCouponDetail(null) as Last_Stop_Over" + Environment.NewLine;
            sql = sql + " where HdrGuid in (select HdrGuid from Pax.fn_SalesCouponDetail(null) where StopOverCode = 'X'  )" + Environment.NewLine;
            sql = sql + "and Last_Stop_Over.DocumentNumber = f3.InConnectionWithDocumentNumber and StopOverCode <> 'X'" + Environment.NewLine;
            sql = sql + " order by Last_Stop_Over.DocumentNumber, Last_Stop_Over.RelatedDocumentNumber, Last_Stop_Over.CouponNumber desc )" + Environment.NewLine;
            sql = sql + " as Last_Stop_Over_Carrier from Pax.VW_SalesHeader f1 join test.Bmp71_AdditionalInformation f3 on f1.HdrGuid = f3.HdrGuid and f1.DocumentNumber = f3.TicketDocumentNumber" + Environment.NewLine;
            sql = sql + "left join Pax.VW_SalesCouponDetail f2 on f1.DocumentNumber = f2.DocumentNumber where CHARINDEX('INAD', f1.PassengerName) > 0 " + Environment.NewLine;
            sql = sql + " and FlightDepartureDate between cast('" + dateFrom + "' as date) and cast('" + dateTo + "' as date)  " + Environment.NewLine;

            DataSet ds = new DataSet();
            SqlConnection con = new SqlConnection(pbConnectionString);
            SqlDataAdapter ada = new SqlDataAdapter(sql, con);
            ada.Fill(ds);
            con.Close();
            int lon = ds.Tables[0].Rows.Count;

            string compt = "";
            ViewBag.message = "";
            ViewBag.datefrom = dateFrom1;
            string[] tabInfo = new string[10];

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                for (int i = 0; i < 10; i++)
                {

                    tabInfo[i] = dr[ds.Tables[0].Columns[i].ColumnName].ToString();
                    compt += tabInfo[i];

                }

                //compt = dr[ds.Tables[0].Columns[0].ColumnName].ToString();

            }

            if (string.IsNullOrEmpty(compt))
            {
                ViewBag.message = "No data available for the selected criteria.";
            }



            return PartialView(ds);

        }

        /***********************************end*************************************/

        /***************INAD****************************/

        public ActionResult INAD()
        {

            return PartialView();
        }
        public ActionResult LoadINAD()
        {
            string passengerName = Request["pasengerName"];
            string strPassengerName = "";
            string cmd = "";
            string docNum = Request["docNum"];
            string valrad = Request["radContains"];
            if (passengerName != "")
            {
                docNum = "";
                if (valrad == "starting")
                {
                    strPassengerName = " PassengerName LIKE '" + passengerName + "%'";
                }
                else
                    if (valrad == "contains")
                {
                    strPassengerName = " PassengerName LIKE '%" + passengerName + "%'";
                }

                cmd = "SELECT [DocumentNumber],[PassengerName],[DocumentType],format([DateofIssue],'dd-MMM-yyyy') as DateofIssue ,format([SaleDate], 'dd-MMM-yyyy') as SaleDate ,[AgentNumericCode],[FareCalculationArea] FROM [Pax].[SalesDocumentHeader] where " + strPassengerName + " AND FareCalculationArea <> ''";

            }
            else
                if ((docNum != "") && (docNum.Length == 13))
            {

                cmd = "SELECT [DocumentNumber],[PassengerName],[DocumentType],format([DateofIssue],'dd-MMM-yyyy') as DateofIssue ,format([SaleDate], 'dd-MMM-yyyy') as SaleDate,[AgentNumericCode],[FareCalculationArea] FROM [Pax].[SalesDocumentHeader] where DocumentNumber = '" + docNum + "' AND FareCalculationArea <> ''";

            }
            SqlConnection con = new SqlConnection(pbConnectionString);
            DataSet ds = new DataSet();
            SqlDataAdapter ada = new SqlDataAdapter(cmd, con);
            ada.Fill(ds);
            con.Close();
            return PartialView(ds);

        }

        public ActionResult RetINAD()
        {
            string FCA = Request["FCA"];
            string Secq = @"(((?!([A-Z]{6}))([A-Z]{3})))";
            string s = "";

            string xq = null;

            int i = 0;
            string[] y = new string[100];

            try
            {
                foreach (Match matSecq in Regex.Matches(FCA, Secq))
                {

                    xq = matSecq.Value.ToString();

                    y[i] = xq;
                    i++;

                }

            }
            catch
            {
            }

            string[] b = y.Distinct().ToArray();
            string sql = "";

            for (int yl = 0; yl < b.Length; yl++)
            {
                string a = b[yl];

                if (a != null)
                {
                    sql += "[CityCode] like '%" + a + "%' AND[AirportCode] like '%" + a + "%'";
                    sql += "OR";

                }

            }

            s = "SELECT [AirportCode],[CityCode],[AirportName],[CityName] FROM [Ref].[City] WHERE  1=1 and " + sql.TrimEnd("OR".ToCharArray());

            DataSet ds = new DataSet();
            SqlConnection con = new SqlConnection(pbConnectionString);
            SqlDataAdapter ada = new SqlDataAdapter(s, con);
            ada.Fill(ds);
            con.Close();

            return PartialView(ds);

        }

        public ActionResult PopINAD()
        {
            string FCA = Request["FCA"];
            string CityCode = Request["Citycode"];
            string Secq = @"(((?!([A-Z]{6}))([A-Z]{2,3}))|(1A )|(1B )|(1G )|(1S )|(2I )|(2J )|(2M )|(2V )|(3A )|(3E )|(3L )|(3M )|(3S )|(3U )|(3V )|(3X )|(4C )|(4H )|(4M )|(4Q )|(4S )|(4X )|(5C )|(5D )|(5J )|(5K )|(5L )|(5N )|(5T )|(5X )|(6A )|(6H )|(6R )|(7D )|(7F )|(7H )|(7I )|(7U )|(7W )|(8D )|(8M )|(8U )|(8V )|(9E )|(9H )|(9K )|(9U )|(9W )|(A3 )|(A6 )|(A9 )|(B6 )|(B7 )|(B8 )|(C5 )|(C9 )|(D5 )|(D6 )|(D9 )|(E0 )|(E6 )|(E7 )|(F2 )|(F7 )|(F9 )|(G3 )|(G4 )|(G7 )|(H2 )|(I5 )|(J2 )|(J8 )|(K5 )|(K6 )|(L7 )|(M3 )|(M5 )|(M6 )|(M7 )|(N3 )|(N8 )|(O9 )|(P5 )|(Q2 )|(R2 )|(R4 )|(S2 )|(S3 )|(S4 )|(S5 )|(S7 )|(T0 )|(U6 )|(U7 )|(U8 )|(U9 )|(V0 )|(V3 )|(V4 )|(W5 )|(W8 )|(W9 )|(X3 )|(Y4 )|(Y8 )|(Z2 )|(Z4 )|(Z5 )|(Z6 )|((//))|((/-)))";
            string s = "";

            string xq = null;

            int i = 0;
            string[] y = new string[100];

            try
            {
                foreach (Match matSecq in Regex.Matches(FCA, Secq))
                {
                    xq = matSecq.Value.ToString();

                    y[i] = xq;
                    i++;

                }

            }
            catch
            {
            }
            string[] n = new string[10];
            int p = 0;

            for (int j = 0; j < 50; j++)
            {
                if (y[j] != null)
                {
                    if (j != 0)
                    {
                        if (y[j].ToString() == CityCode)
                        {
                            n[p] = y[j - 1].ToString();
                            p++;
                        }
                    }
                }
                else
                {
                    break;
                }
            }
            string sql = "";

            for (int yl = 0; yl < n.Length; yl++)
            {
                string a = n[yl];

                if (a != null)
                {
                    sql += " SELECT [AirlineCode] ,[AirlineID] ,[AirlineName] FROM [Ref].[Airlines] WHERE [AirlineCode] ='" + a.Trim() + "'";
                }

            }

            s = s + sql;

            DataSet ds = new DataSet();
            SqlConnection con = new SqlConnection(pbConnectionString);
            SqlDataAdapter ada = new SqlDataAdapter(s, con);
            ada.Fill(ds);
            con.Close();

            return PartialView(ds);

        }

        public ActionResult ExecuteINAD()
        {
            string FCA = Request["FCA"];
            string inadpoint = Request["inadpoint"];
            string finalbound = Request["finalbound"];
            string issuedate = Request["issuedate"];
            string inbound = Request["inbound"];
            string inbound1 = Request["inbound1"];
            string inbound2 = Request["inbound2"];
            string txtUncollectedFareAmt = Request["txtUncollectedFareAmt"];
            string txtUncollectedNonTransFareAmt = Request["txtUncollectedNonTransFareAmt"];
            ViewBag.inbound = inbound;
            ViewBag.inbound1 = inbound1;
            ViewBag.inbound2 = inbound2;
            ViewBag.finalbound = finalbound;
            string[] SecArray = new string[10];
            SqlConnection cs = new SqlConnection(pbConnectionString);

            InadClass INDg = new InadClass();

            for (int i = 0; i < 10; i++)
            {
                SecArray[i] = INDg.BreakINboundCarriage(FCA, inadpoint, finalbound, 1)[i];
            }

            string[,] dg = new string[10, 10];
            string[,] dga = INDg.PopulateGrid(SecArray, dg);
            string[,] dga1 = INDg.PopulateProrateFactor(dga, "3/11/2017", cs);

            INDg = null;

            dga1[0, 7] = "Total";
            ViewBag.total = dga1[0, 7];
            Proration IND = new Proration();

            dga1[2, 7] = IND.Total(dga1, 2);
            ViewBag.total1 = dga1[2, 7];

            string TotalProrateValue = ViewBag.total1;
            string UncolFarewithouISC = txtUncollectedFareAmt;

            string share = (IND.valid(UncolFarewithouISC) / IND.valid(TotalProrateValue)).ToString();

            string[,] share1 = Shareindg(dga1, share, 3);

            dga1[3, 7] = IND.Total(share1, 3);
            ViewBag.total2 = dga1[3, 7];

            string ISC = "9";

            string ISCPerAmt = ((IND.valid(ISC) / 100) * IND.valid(UncolFarewithouISC)).ToString();

            string share0 = (IND.valid(ISCPerAmt) / IND.valid(TotalProrateValue)).ToString();

            string[,] share2 = Shareindg(dga1, share0, 4);

            dga1[4, 7] = IND.Total(share2, 4);
            ViewBag.total3 = dga1[4, 7];


            string[,] shard = ShareNet(dga1);

            dga1[5, 7] = IND.Total(shard, 5);
            ViewBag.total4 = dga1[5, 7];

            string UncolNTC = txtUncollectedNonTransFareAmt;

            string Share = (IND.valid(UncolNTC) / IND.valid(TotalProrateValue)).ToString();

            string[,] dg3 = Shareindg(dga1, Share, 6);

            dga1[6, 7] = IND.Total(dg3, 6);
            ViewBag.total4 = dga1[6, 7];

            string[,] dg4 = TotalShare(dga1);

            dga1[7, 7] = IND.Total(dg4, 7);
            ViewBag.total5 = dga1[7, 7];
            string[,] dgProrat = new string[8, 8];
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    dgProrat[i, j] = dga1[j, i];
                }
            }

            ViewBag.dgProrates = dgProrat;
            //TempData["dgProrates"] = dgProrat;
            return PartialView();
        }

        public string[,] Shareindg(string[,] dg, string Prval, int cl)
        {

            for (int i = 1; i < 6; i++)
            {
                if (dg[2, i] != null && !string.IsNullOrWhiteSpace(dg[2, i].ToString()) && dg[2, i] != "")
                {

                    dg[cl, i] = (Convert.ToDouble(dg[2, i]) * Convert.ToDouble(Prval)).ToString("0.00");
                }

            }
            return dg;
        }

        public string[,] ShareNet(string[,] dg)
        {
            for (int i = 1; i < 8; i++)
            {
                if (dg[2, i] != null && !string.IsNullOrWhiteSpace(dg[2, i].ToString()) && dg[2, i] != "")
                {

                    dg[5, i] = (Convert.ToDouble(dg[3, i]) - (Convert.ToDouble(dg[4, i]))).ToString("0.00");
                }


            }
            return dg;

        }

        public string[,] TotalShare(string[,] dg)
        {
            for (int i = 1; i < 8; i++)
            {
                if (dg[2, i] != null && !string.IsNullOrWhiteSpace(dg[2, i].ToString()) && dg[2, i] != "")
                {

                    dg[7, i] = (Convert.ToDouble(dg[6, i]) + (Convert.ToDouble(dg[5, i]))).ToString("0.00");
                }


            }
            return dg;

        }

        public string deleteInad(string[] dgProrateShare)
        {
            string txtNewDoc = Request["txtNewDoc"];
            string lblNewIssueDate = Request["lblNewIssueDate"];
            //string lblNewIssueDate = "3/11/2017";
            string lblOrgIssueDate = Request["lblNewIssueDate"];
            //string lblOrgIssueDate = "3/11/2017";
            string lblPaxName = Request["lblPaxName"];
            //string lblPaxName = "Altuner";
            string txtNewFCA = Request["txtNewFCA"];
            string Au = Request["Au"];
            string txtOrgDoc = Request["txtOrgDoc"];
            string txtOrgFCA = Request["txtOrgFCA"];
            string txtINADpoint = Request["txtINADpoint"];
            string txtFinalInboundCarri = Request["txtFinalInboundCarri"];
            string txtInboundCarriage1 = Request["txtInboundCarriage1"];
            string txtInboundCarriage2 = Request["txtInboundCarriage2"];
            string txtInboundCarriage3 = Request["txtInboundCarriage3"];
            string txtInboundCarriage4 = Request["txtInboundCarriage4"];
            string txtInboundCarriage5 = Request["txtInboundCarriage5"];
            string txtBillPer = Request["txtBillPer"];
            string txtUncollFareCurrency = Request["txtUncollFareCurrency"];
            string txtUncollFareAmt = Request["txtUncollFareAmt"];
            double txtUncollFareAmt1 = Convert.ToDouble(txtUncollFareAmt.ToString());
            string txtUncollectedFareAmt = Request["txtUncollectedFareAmt"];
            double txtUncollectedFareAmt1 = Convert.ToDouble(txtUncollectedFareAmt.ToString());
            string txtUncolNTCurrency = Request["txtUncolNTCurrency"];
            string txtUncollNTAmt = Request["txtUncollNTAmt"];
            string txtUncollectedNonTransFareAmt = Request["txtUncollectedNonTransFareAmt"];
            string txtIscPer = Request["txtIscPer"];

            string txtOutboundCarriage = Request["txtOutboundCarriage"];
            string txtOutboundCarriage1 = Request["txtOutboundCarriage1"];
            string txtOutboundCarriage2 = Request["txtOutboundCarriage2"];
            string txtOutboundCarriage3 = Request["txtOutboundCarriage3"];
            string txtOutboundCarriage4 = Request["txtOutboundCarriage4"];
            string txtOutboundCarriage5 = Request["txtOutboundCarriage5"];
            string txtHandlingAirline = Request["txtHandlingAirline"];

            string sq = "DELETE FROM [Pax].[INADProrateDetails] WHERE [DocumentNumber] = '" + txtNewDoc + "'";
            DataSet ds10 = new DataSet();
            ds10 = dbconnect.RetObjDS(sq);
            string sqd = "DELETE FROM [Pax].[INADHeader] WHERE [NewDocumentNumber] = '" + txtNewDoc + "'";
            DataSet ds0 = new DataSet();
            ds0 = dbconnect.RetObjDS(sqd);

            string sq1 = "";

            sq1 = sq1 + "INSERT INTO [Pax].[INADHeader]" + Environment.NewLine;
            sq1 = sq1 + "VALUES (" + Environment.NewLine;
            sq1 = sq1 + "'" + txtNewDoc + "'," + Environment.NewLine;
            sq1 = sq1 + "Convert(date,'" + lblNewIssueDate + "',101)," + Environment.NewLine;
            sq1 = sq1 + "'" + txtNewFCA + "'," + Environment.NewLine;
            sq1 = sq1 + "'" + lblPaxName + "'," + Environment.NewLine;
            sq1 = sq1 + "'" + txtOrgDoc + "'," + Environment.NewLine;
            sq1 = sq1 + "Convert(date,'" + lblOrgIssueDate + "',101)," + Environment.NewLine;
            sq1 = sq1 + "'" + txtOrgFCA + "'," + Environment.NewLine;
            sq1 = sq1 + "'" + txtINADpoint + "'," + Environment.NewLine;
            sq1 = sq1 + "'" + txtFinalInboundCarri + "'," + Environment.NewLine;
            sq1 = sq1 + "'" + txtInboundCarriage1 + "'," + Environment.NewLine;
            sq1 = sq1 + "'" + txtInboundCarriage2 + "'," + Environment.NewLine;
            sq1 = sq1 + "'" + txtInboundCarriage3 + "'," + Environment.NewLine;
            sq1 = sq1 + "'" + txtInboundCarriage4 + "'," + Environment.NewLine;
            sq1 = sq1 + "'" + txtInboundCarriage5 + "'," + Environment.NewLine;
            sq1 = sq1 + "'" + txtBillPer + "'," + Environment.NewLine;
            sq1 = sq1 + "'" + txtUncollFareCurrency + "'," + Environment.NewLine;

            if (txtUncollFareAmt == "")
            {
                sq1 = sq1 + 0 + "," + Environment.NewLine;
            }
            else
            {
                sq1 = sq1 + txtUncollFareAmt + "," + Environment.NewLine;
            }

            if (txtUncollectedFareAmt == "")
            {
                sq1 = sq1 + 0 + "," + Environment.NewLine;
            }
            else
            {
                sq1 = sq1 + txtUncollectedFareAmt + "," + Environment.NewLine;
            }


            sq1 = sq1 + "'" + txtUncolNTCurrency + "'," + Environment.NewLine;

            if (txtUncollNTAmt == "")
            {
                sq1 = sq1 + 0 + "," + Environment.NewLine;
            }
            else
            {
                sq1 = sq1 + "'" + txtUncollNTAmt + "'," + Environment.NewLine;
            }

            if (txtUncollectedNonTransFareAmt == "")
            {
                sq1 = sq1 + 0 + "," + Environment.NewLine;
            }
            else
            {
                sq1 = sq1 + txtUncollectedNonTransFareAmt + "," + Environment.NewLine;
            }

            sq1 = sq1 + txtIscPer + "," + Environment.NewLine;
            sq1 = sq1 + "'" + txtOutboundCarriage + "'," + Environment.NewLine;
            sq1 = sq1 + "'" + txtOutboundCarriage1 + "'," + Environment.NewLine;
            sq1 = sq1 + "'" + txtOutboundCarriage2 + "'," + Environment.NewLine;
            sq1 = sq1 + "'" + txtOutboundCarriage3 + "'," + Environment.NewLine;
            sq1 = sq1 + "'" + txtOutboundCarriage4 + "'," + Environment.NewLine;
            sq1 = sq1 + "'" + txtOutboundCarriage5 + "'," + Environment.NewLine;
            sq1 = sq1 + "'" + txtHandlingAirline + "'," + Environment.NewLine;
            sq1 = sq1 + 0 + Environment.NewLine;
            sq1 = sq1 + ")" + Environment.NewLine;

            DataSet ds1 = new DataSet();
            ds1 = dbconnect.RetObjDS(sq1);

            for (int i = 1; i < dgProrateShare.Length - 3; i++)
            {
                string[] tab = dgProrateShare[i].Split(',');
                if (tab[0] != null && !string.IsNullOrWhiteSpace(tab[0].ToString()))
                {
                    string sqdet = "";

                    sqdet = sqdet + "INSERT INTO [Pax].[INADProrateDetails]" + Environment.NewLine;
                    sqdet = sqdet + "VALUES (" + Environment.NewLine;
                    sqdet = sqdet + "'" + txtNewDoc + "'," + Environment.NewLine;
                    sqdet = sqdet + i + "," + Environment.NewLine;
                    sqdet = sqdet + "'" + tab[0] + "'," + Environment.NewLine;
                    sqdet = sqdet + "'" + tab[0] + "'," + Environment.NewLine;
                    sqdet = sqdet + "'" + tab[1] + "'," + Environment.NewLine;
                    sqdet = sqdet + "'" + tab[2] + "'," + Environment.NewLine;
                    sqdet = sqdet + tab[3] + "," + Environment.NewLine;
                    sqdet = sqdet + tab[4] + "," + Environment.NewLine;
                    sqdet = sqdet + tab[5] + "," + Environment.NewLine;
                    sqdet = sqdet + tab[6] + Environment.NewLine;
                    sqdet = sqdet + ")" + Environment.NewLine;
                    DataSet ds3 = new DataSet();
                    ds3 = dbconnect.RetObjDS(sqdet);
                }
            }

            return "Record Successfully Updated";

        }
        public string SaveInad(string[] dgProrateShare)
        {
            string txtNewDoc = Request["txtNewDoc"];
            string lblNewIssueDate = Request["lblNewIssueDate"];
            //string lblNewIssueDate = "3/11/2017";
            string lblOrgIssueDate = Request["lblNewIssueDate"];
            //string lblOrgIssueDate = "3/11/2017";
            string lblPaxName = Request["lblPaxName"];
            //string lblPaxName = "Altuner";
            string txtNewFCA = Request["txtNewFCA"];
            string Au = Request["Au"];
            string txtOrgDoc = Request["txtOrgDoc"];
            string txtOrgFCA = Request["txtOrgFCA"];
            string txtINADpoint = Request["txtINADpoint"];
            string txtFinalInboundCarri = Request["txtFinalInboundCarri"];
            string txtInboundCarriage1 = Request["txtInboundCarriage1"];
            string txtInboundCarriage2 = Request["txtInboundCarriage2"];
            string txtInboundCarriage3 = Request["txtInboundCarriage3"];
            string txtInboundCarriage4 = Request["txtInboundCarriage4"];
            string txtInboundCarriage5 = Request["txtInboundCarriage5"];
            string txtBillPer = Request["txtBillPer"];
            string txtUncollFareCurrency = Request["txtUncollFareCurrency"];
            string txtUncollFareAmt = Request["txtUncollFareAmt"];
            double txtUncollFareAmt1 = Convert.ToDouble(txtUncollFareAmt.ToString());
            string txtUncollectedFareAmt = Request["txtUncollectedFareAmt"];
            double txtUncollectedFareAmt1 = Convert.ToDouble(txtUncollectedFareAmt.ToString());
            string txtUncolNTCurrency = Request["txtUncolNTCurrency"];
            string txtUncollNTAmt = Request["txtUncollNTAmt"];
            string txtUncollectedNonTransFareAmt = Request["txtUncollectedNonTransFareAmt"];
            string txtIscPer = Request["txtIscPer"];

            string txtOutboundCarriage = Request["txtOutboundCarriage"];
            string txtOutboundCarriage1 = Request["txtOutboundCarriage1"];
            string txtOutboundCarriage2 = Request["txtOutboundCarriage2"];
            string txtOutboundCarriage3 = Request["txtOutboundCarriage3"];
            string txtOutboundCarriage4 = Request["txtOutboundCarriage4"];
            string txtOutboundCarriage5 = Request["txtOutboundCarriage5"];
            string txtHandlingAirline = Request["txtHandlingAirline"];

            bool hs = CheckIfExist(txtNewDoc);

            if (hs == true)
            {
                return "update";
            }
            else
            {
                string sq3 = "";

                sq3 = sq3 + "INSERT INTO [Pax].[INADHeader]" + Environment.NewLine;
                sq3 = sq3 + "VALUES (" + Environment.NewLine;
                sq3 = sq3 + "'" + txtNewDoc + "'," + Environment.NewLine;
                sq3 = sq3 + "Convert(date,'" + lblNewIssueDate + "',101)," + Environment.NewLine;
                sq3 = sq3 + "'" + txtNewFCA + "'," + Environment.NewLine;
                sq3 = sq3 + "'" + lblPaxName + "'," + Environment.NewLine;
                sq3 = sq3 + "'" + txtOrgDoc + "'," + Environment.NewLine;
                sq3 = sq3 + "Convert(date,'" + lblOrgIssueDate + "',101)," + Environment.NewLine;
                sq3 = sq3 + "'" + txtOrgFCA + "'," + Environment.NewLine;
                sq3 = sq3 + "'" + txtINADpoint + "'," + Environment.NewLine;
                sq3 = sq3 + "'" + txtFinalInboundCarri + "'," + Environment.NewLine;
                sq3 = sq3 + "'" + txtInboundCarriage1 + "'," + Environment.NewLine;
                sq3 = sq3 + "'" + txtInboundCarriage2 + "'," + Environment.NewLine;
                sq3 = sq3 + "'" + txtInboundCarriage3 + "'," + Environment.NewLine;
                sq3 = sq3 + "'" + txtInboundCarriage4 + "'," + Environment.NewLine;
                sq3 = sq3 + "'" + txtInboundCarriage5 + "'," + Environment.NewLine;
                sq3 = sq3 + "'" + txtBillPer + "'," + Environment.NewLine;
                sq3 = sq3 + "'" + txtUncollFareCurrency + "'," + Environment.NewLine;

                if (txtUncollFareAmt == "")
                {
                    sq3 = sq3 + 0 + "," + Environment.NewLine;
                }
                else
                {
                    sq3 = sq3 + txtUncollFareAmt + "," + Environment.NewLine;
                }

                if (txtUncollectedFareAmt == "")
                {
                    sq3 = sq3 + 0 + "," + Environment.NewLine;
                }
                else
                {
                    sq3 = sq3 + txtUncollectedFareAmt + "," + Environment.NewLine;
                }


                sq3 = sq3 + "'" + txtUncolNTCurrency + "'," + Environment.NewLine;

                if (txtUncollNTAmt == "")
                {
                    sq3 = sq3 + 0 + "," + Environment.NewLine;
                }
                else
                {
                    sq3 = sq3 + "'" + txtUncollNTAmt + "'," + Environment.NewLine;
                }

                if (txtUncollectedNonTransFareAmt == "")
                {
                    sq3 = sq3 + 0 + "," + Environment.NewLine;
                }
                else
                {
                    sq3 = sq3 + txtUncollectedNonTransFareAmt + "," + Environment.NewLine;
                }

                sq3 = sq3 + txtIscPer + "," + Environment.NewLine;
                sq3 = sq3 + "'" + txtOutboundCarriage + "'," + Environment.NewLine;
                sq3 = sq3 + "'" + txtOutboundCarriage1 + "'," + Environment.NewLine;
                sq3 = sq3 + "'" + txtOutboundCarriage2 + "'," + Environment.NewLine;
                sq3 = sq3 + "'" + txtOutboundCarriage3 + "'," + Environment.NewLine;
                sq3 = sq3 + "'" + txtOutboundCarriage4 + "'," + Environment.NewLine;
                sq3 = sq3 + "'" + txtOutboundCarriage5 + "'," + Environment.NewLine;
                sq3 = sq3 + "'" + txtHandlingAirline + "'," + Environment.NewLine;
                sq3 = sq3 + 0 + Environment.NewLine;
                sq3 = sq3 + ")" + Environment.NewLine;

                DataSet ds = new DataSet();
                ds = dbconnect.RetObjDS(sq3);
                for (int i = 1; i < dgProrateShare.Length - 3; i++)
                {
                    string[] tab = dgProrateShare[i].Split(',');
                    if (tab[0] != null && !string.IsNullOrWhiteSpace(tab[0].ToString()))
                    {
                        string sqdet = "";

                        sqdet = sqdet + "INSERT INTO [Pax].[INADProrateDetails]" + Environment.NewLine;
                        sqdet = sqdet + "VALUES (" + Environment.NewLine;
                        sqdet = sqdet + "'" + txtNewDoc + "'," + Environment.NewLine;
                        sqdet = sqdet + i + "," + Environment.NewLine;
                        sqdet = sqdet + "'" + tab[0] + "'," + Environment.NewLine;
                        sqdet = sqdet + "'" + tab[0] + "'," + Environment.NewLine;
                        sqdet = sqdet + "'" + tab[1] + "'," + Environment.NewLine;
                        sqdet = sqdet + "'" + tab[2] + "'," + Environment.NewLine;
                        sqdet = sqdet + tab[3] + "," + Environment.NewLine;
                        sqdet = sqdet + tab[4] + "," + Environment.NewLine;
                        sqdet = sqdet + tab[5] + "," + Environment.NewLine;
                        sqdet = sqdet + tab[6] + Environment.NewLine;
                        sqdet = sqdet + ")" + Environment.NewLine;
                        
                    }
                }
                return "Record Successfully Saved";
            }

        }

        public bool CheckIfExist(string doc)
        {
            DataSet ds = new DataSet();
            SqlConnection cs = new SqlConnection(pbConnectionString);
            string sq = "SELECT * FROM [Pax].[INADHeader]" + Environment.NewLine;
            sq = sq + "WHERE [NewDocumentNumber] = '" + doc + "'";

            if (cs.State == ConnectionState.Open)
            {
                cs.Close();
            }

            cs.Open();
            SqlDataReader rd;
            SqlCommand cmdd = new SqlCommand(sq, cs);
            cmdd.CommandType = CommandType.Text;
            cmdd.CommandText = sq;
            cmdd.Connection = cs;
            rd = cmdd.ExecuteReader();
            bool hasFoundd = false;
            if (rd.HasRows)
            {
                if (rd != null)
                {
                    while (rd.Read())
                    {
                        hasFoundd = true;

                    }

                }

            }
            cs.Close();

            return hasFoundd;
        }
        /***********************************end*************************************/

        /***************FOP-UATP Billings to OALS****************************/

        public ActionResult FOPUATPBillingsToOALs()
        {
            ViewBag.dateFrom = DateTime.Now.AddMonths(-1).ToString("dd-MMM-yyyy");
            ViewBag.dateTo = DateTime.Now.ToString("dd-MMM-yyyy");
            return PartialView();
        }
        public ActionResult LoadFOPUATPBillingsToOALs()
        {
            var culture = System.Globalization.CultureInfo.CurrentCulture;
            string dateFrom1 = Request["dateFrom"];
            string dateFrom = ConvertDate(dateFrom1);
            string dateTo1 = Request["dateTo"];
            string dateTo = ConvertDate(dateTo1);

            string sql = "select * from pax.fn_fop()" + Environment.NewLine;
            sql = sql + "  where AirlineID_UATPCard is not null and atcan = 'N'" + Environment.NewLine;
            sql = sql + " and SaleDate between cast('" + dateFrom + "' as date) and cast('" + dateTo + "' as date)  " + Environment.NewLine;
            sql = sql + " order by SaleDate,fop, Currency " + Environment.NewLine;

            DataSet ds = new DataSet();
            SqlConnection con = new SqlConnection(pbConnectionString);
            SqlDataAdapter ada = new SqlDataAdapter(sql, con);
            ada.Fill(ds);
            con.Close();
            int lon = ds.Tables[0].Rows.Count;

            int compt = 0;
            ViewBag.message = "";
            ViewBag.datefrom = dateFrom1;
            string[] tabInfo = new string[7];
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                for (int i = 0; i < 7; i++)
                {

                    tabInfo[i] = dr[ds.Tables[0].Columns[i].ColumnName].ToString();
                    //compt += tabInfo[i];
                    compt++;

                }
                //compt = dr[ds.Tables[0].Columns[0].ColumnName].ToString();

            }
            if (compt == 0)
            //if (string.IsNullOrEmpty(compt))
            {
                ViewBag.message = "No data available for the selected criteria.";
                ViewBag.Total = ".00";
            }
            else
            {
                ViewBag.Total = compt.ToString("0.00");
            }


            return PartialView(ds);
        }


        /***********************************end*************************************/

        /***************UATP Discount Reclaim****************************/

        public ActionResult UATPDiscountReclaim()
        {
            ViewBag.dateFrom = DateTime.Now.AddMonths(-1).ToString("dd-MMM-yyyy");
            ViewBag.dateTo = DateTime.Now.ToString("dd-MMM-yyyy");
            return PartialView();
        }
        public ActionResult LoadUATPDiscountReclaim()
        {
            var culture = System.Globalization.CultureInfo.CurrentCulture;
            string dateFrom1 = Request["dateFrom"];
            string dateFrom = ConvertDate(dateFrom1);
            string dateTo1 = Request["dateTo"];
            string dateTo = ConvertDate(dateTo1);

            string sql = "select * from pax.fn_FOP_UATP_Reclaim()" + Environment.NewLine;
            sql = sql + " where SaleDate between cast('" + dateFrom + "' as date) and cast('" + dateTo + "' as date)  " + Environment.NewLine;
            sql = sql + " ORDER BY SaleDate ,DocumentNumber" + Environment.NewLine;

            DataSet ds = new DataSet();
            SqlConnection con = new SqlConnection(pbConnectionString);
            SqlDataAdapter ada = new SqlDataAdapter(sql, con);
            ada.Fill(ds);
            con.Close();
            int lon = ds.Tables[0].Rows.Count;

            string compt = "";
            ViewBag.message = "";
            ViewBag.datefrom = dateFrom1;
            string[] tabInfo = new string[11];
            foreach (DataRow dr in ds.Tables[0].Rows)
            {

                for (int i = 0; i < 11; i++)
                {

                    tabInfo[i] = dr[ds.Tables[0].Columns[i].ColumnName].ToString();
                    compt += tabInfo[i];

                }
            }

            if (string.IsNullOrEmpty(compt))
            {
                ViewBag.message = "No data available for the selected criteria.";
                ViewBag.Total = ".00";

            }

            return PartialView(ds);

        }

        /***********************************end*************************************/
        /* passenger agency details */

        public ActionResult PassengerAgencyDetails()
        {
          //  string dateFrom = DateTime.Now.AddMonths(-1).ToString("dd-MMM-yyyy");
            string dateTo = DateTime.Now.ToString("dd-MMM-yyyy");
            string[] date = new string[1] {dateTo };
            ViewBag.date = date;
            string sql = "select distinct f1.AgencyNumericCode from[Ref].[PassengerAgencyDetails]f1";
            DataSet ds = new DataSet();
            ds = dbconnect.RetObjDS(sql);
            return PartialView(ds);
        }

        public ActionResult PassengerMan(string[] dataValue)
        {

            DataSet ds = new DataSet();
            string ag = "";
            if (dataValue[2] == "All")
            {
                ag = "%";
            }
            else
            {
                ag = dataValue[2];
            }

            if (dataValue[0] != "")
            {
                string strPassengerName = string.Empty;
                if (!string.IsNullOrEmpty(dataValue[1]))
                {
                    strPassengerName = " f2.LegalName LIKE '%" + dataValue[0] + "%'";
                }
                else
                {
                    strPassengerName = " f2.LegalName LIKE '" + dataValue[0] + "%'";
                }
                string sql = "";
                sql += "SELECT  f2.[AgencyNumericCode],f2.LegalName as Name from [Ref].[VW_Agent] f2  ";
                sql += "left join  [Ref].[PassengerAgencyDetails] f1 on f1.AgencyNumericCode = f2.AgencyNumericCode  ";
                sql += "where  " + strPassengerName + "  ";
                ds = dbconnect.RetObjDS(sql);
            }
            else
            {
                if (dataValue[2] != "")
                {

                    string sql = "";
                    sql += "SELECT  f2.[AgencyNumericCode],f2.LegalName as Name from [Ref].[VW_Agent] f2  ";
                    sql += "left join  [Ref].[PassengerAgencyDetails] f1 on f1.AgencyNumericCode = f2.AgencyNumericCode  ";
                    sql += "where f2.AgencyNumericCode like '" + ag + "' ";

                    ds = dbconnect.RetObjDS(sql);
                }
            }

            return PartialView(ds);
        }

        public ActionResult LoadPassengerMan(string dataValue)
        {
            string sql = "SELECT iif(f2.AgencyNumericCode is null, f1.[AgencyNumericCode],f2.AgencyNumericCode) as AgencyNumericCode,f1.LegalName,f1.LocationAddress,[Status],[Category],[Remarks]   ";
            sql += ",iif ([DateOfAppointment] is null,(select top 1  SaleDate from pax.SalesDocumentHeader f3 where left ( f3.AgentNumericCode,7) = f1.[AgencyNumericCode])  ,FORMAT(DateOfAppointment,'dd-MMM-yyyy')) as [DateOfAppointment]  ";
            sql += " , f1.LocationCity,f1.LocationCountry as BSP from ref.VW_Agent f1 left join [Ref].[PassengerAgencyDetails] f2 on f1.AgencyNumericCode = f2.AgencyNumericCode  ";
            sql += " where f1.AgencyNumericCode = '" + dataValue + "' ";
            DataSet ds = new DataSet();
            ds = dbconnect.RetObjDS(sql);
            return PartialView(ds);
        }

        public ActionResult ListNewAgents(string[] dataValue)
        {

            string sql = "select adm.GetOwnAirline()";
            string ownairline = "";
            DataSet ds = new DataSet();
            ds = dbconnect.RetObjDS(sql);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                for (int i = 0; i < ds.Tables[0].Columns.Count; i++)
                {
                    ownairline = dr[ds.Tables[0].Columns[0].ColumnName].ToString();
                }
            }

            string dateFrom1 = dataValue[0];
            string dateFrom = ConvertDate(dateFrom1);
            string dateTo1 = dataValue[1];
            string dateTo = ConvertDate(dateTo1);

            string sql1 = "SELECT distinct f1.AgentNumericCode,count(f1.documentnumber) as DocCount" + Environment.NewLine;
            sql1 += "FROM pax.SalesDocumentHeader f1" + Environment.NewLine;
            sql1 += "left join ref.VW_Agent f2 on ((left(f1.AgentNumericCode,7) =  f2.AgencyNumericCode) or (left(f1.AgentNumericCode,8) =  f2.AgencyNumericCode)  ) " + Environment.NewLine;
            sql1 += "where f2.LegalName is null and f1.DateofIssue between '" + dateFrom + "' and '" + dateTo + "' and left(f1.DocumentNumber,3) = '" + ownairline + "'" + Environment.NewLine;
            sql1 += "group by f1.AgentNumericCode" + Environment.NewLine;

            DataSet ds1 = new DataSet();
            ds1 = dbconnect.RetObjDS(sql1);
            return PartialView(ds1);
        }

        public ActionResult addEdditDetails(string dataValue)
        {
            string sql1 = "SELECT AgencyNumericCode, TypesofNumeration, Rate,[Level], Applicability, FORMAT(PeriodFrom,'dd-MMM-yyyy') as PeriodFrom ,FORMAT(PeriodTo, 'dd-MMM-yyyy') as PeriodTo from[Ref].[PaxAgencyDetails]  where AgencyNumericCode = '" + dataValue + "' ";
            DataSet ds1 = new DataSet();
            ds1 = dbconnect.RetObjDS(sql1);
            return PartialView(ds1);
        }

        public string SaveNewPassenger(string[] dataValue)
        {
            string response = "";
            try
            {
                if (CheckifExist(dataValue[6]) == false)
                {
                    if (saveInfo(dataValue))
                    {
                        response = "Record successfully Saved";
                    }

                }
                else
                {
                    Deleteinfo(dataValue[6]);
                    if (saveInfo(dataValue))
                    {
                        response = "Record Updated successfully Saved";
                    }

                }
            }
            catch { }
            return response;
        }

        private bool saveInfo(string[] dataValue)
        {  //tab = [$("#remark").val(0), $("#dateOpp").val(1), $("#location").val(2), $("#bsps").val(3), $("#address").val(4), $("#name").val(5), $("#agentCode").val(6), $("#status").val(7), $("#category").val(8)];
            string sql = "";
            string sql1 = "";
            sql += "INSERT INTO [Ref].[PassengerAgencyDetails] VALUES (" + Environment.NewLine;
            sql += " '" + dataValue[6] + "'";
            sql += " ,  '" + dataValue[7] + "'";
            sql += " ,  '" + dataValue[8] + "'";
            sql += " ,  '" + dataValue[0] + "'";
            sql += " , '" + dataValue[1] + "' )";
            DataSet ds = new DataSet();
            ds = dbconnect.RetObjDS(sql);

            sql1 += "INSERT INTO [Ref].[Agent_Own] ([AgencyNumericCode],[LegalName],[LocationAddress],LocationCity,LocationCountry) values ";
            sql1 += "('" + dataValue[6] + "','" + dataValue[5] + "','" + dataValue[4] + "','" + dataValue[2] + "','" + dataValue[3] + "')" + Environment.NewLine;
            DataSet ds1 = new DataSet();
            ds1 = dbconnect.RetObjDS(sql1);
            if (1 == 1)
            {

            }
            return true;
        }
        private bool CheckifExist(string agenNumCode)
        {
            bool flg = false;

            string sql = "";
            sql += "SELECT  f1.[AgencyNumericCode],isnull(f3.legalname,f2.LegalName) as Name from [Ref].[PassengerAgencyDetails] f1  ";
            sql += "left join ref.Agent_Own f2 on f1.AgencyNumericCode = f2.AgencyNumericCode  ";
            sql += "left join ref.Agent f3 on f1.AgencyNumericCode = f3.AgencyNumericCode   ";
            sql += "where  f2.[AgencyNumericCode] = '" + agenNumCode + "' ";

            DataSet ds = new DataSet();
            ds = dbconnect.RetObjDS(sql);

            if (ds.DefaultViewManager.DataViewSettings.Count >= 1)
            {
                flg = true;
            }
            return flg;

        }

        private bool Deleteinfo(string agenNumCode)
        {
            string sql = "";
            string sql1 = "";
            sql = sql + "DELETE FROM [Ref].[PassengerAgencyDetails]" + Environment.NewLine;
            sql = sql + "WHERE [AgencyNumericCode] = '" + agenNumCode + "'";
            DataSet ds = new DataSet();
            ds = dbconnect.RetObjDS(sql);

            sql1 = sql1 + "DELETE FROM [Ref].[Agent_Own] WHERE [AgencyNumericCode] = '" + agenNumCode + "'" + Environment.NewLine;
            DataSet ds1 = new DataSet();
            ds1 = dbconnect.RetObjDS(sql1);

            return true;
        }

        public string DeletesingleClick(string dataValue)
        {
            try
            {
                if (dataValue != "")
                {
                    {

                        string sql = "DELETE from [Ref].[Agent_Own] where  [AgencyNumericCode] = '" + dataValue + "'";
                        DataSet ds = new DataSet();
                        ds = dbconnect.RetObjDS(sql);

                        string sql1 = "DELETE from [Ref].[PassengerAgencyDetails] where  [AgencyNumericCode] = '" + dataValue + "'";
                        DataSet ds1 = new DataSet();
                        ds1 = dbconnect.RetObjDS(sql1);

                        string sql2 = "DELETE from [Ref].[PaxAgencyDetails] WHERE [AgencyNumericCode] = '" + dataValue + "'" + Environment.NewLine;
                        DataSet ds2 = new DataSet();
                        ds2 = dbconnect.RetObjDS(sql2);
                    }
                }
            }
            catch { }
            return "Record deleted Successfully";

        }

        public string SaveAllRenumerate(string[] dataValue, string dataValue1)
        {
            bool tmp = false;
            for (int ii = 0; ii < dataValue.Length; ii++)
            {
                double r1 = 0;
                double l1 = 0;
                String[] table = dataValue[ii].Split('$');

                if (table[1] == "Percentage")
                {
                    r1 = Convert.ToDouble(table[2]);
                }
                else
                    if (table[1] == "Amount")
                {
                    l1 = Convert.ToDouble(table[2]);
                }
                string sql = "";
                sql = sql + "INSERT INTO [Ref].[PaxAgencyDetails] VALUES (" + Environment.NewLine;
                sql = sql + " '" + dataValue1 + "'";
                sql = sql + " , '" + table[0] + "'";
                sql = sql + " ,   '" + r1 + "'";
                sql = sql + " ,  '" + l1 + "'";
                sql = sql + " ,  '" + table[3] + "'";
                sql = sql + " ,  '" + table[4] + "'";
                sql = sql + " , '" + table[5] + "' ";
                sql = sql + " , '" + DateTime.Now + "' )";
                DataSet ds = new DataSet();
                ds = dbconnect.RetObjDS(sql);
                if (ds.HasErrors == false)
                {
                    tmp = true; ;
                }
                else
                {
                    tmp = false;
                }
            }
            if (tmp)
            {
                return "Record successfully Saved";
            }
            else
            {
                return "Error Saved Record";
            }

        }

        public string UpdateRenumerate(string[] dataValue, string dataValue1)
        {
            string sql = "DELETE from [Ref].[PaxAgencyDetails] where  [AgencyNumericCode] = '" + dataValue1 + "'";
            DataSet ds = new DataSet();
            ds = dbconnect.RetObjDS(sql);
            return SaveAllRenumerate(dataValue, dataValue1);
        }

        public string Deletesingle(string[] dataValue)
        {
            string sql = "DELETE from [Ref].[PaxAgencyDetails] WHERE [AgencyNumericCode] = '" + dataValue[0] + "'" + Environment.NewLine;
            sql += "AND [TypesofNumeration] = '" + dataValue[1] + "'  AND [PeriodFrom] = cast('" + dataValue[2] + "' as date)" + Environment.NewLine;
            sql += "AND [PeriodTo] = cast('" + dataValue[3] + "' as date)" + Environment.NewLine;
            DataSet ds = new DataSet();
            ds = dbconnect.RetObjDS(sql);
            return "Record deleted Successfully";
        }

        public ActionResult ClickListNewAgent(string[] dataValue)
        {
            string sql = "select DocumentNumber, FORMAT(DateofIssue,'dd-MMM-yyyy') as DateofIssue,PassengerName, FareCalculationArea," + Environment.NewLine;
            sql += "FareCurrency, Fare, TotalCurrency, EquivalentFare, TransactionGroup from pax.SalesDocumentHeader" + Environment.NewLine;
            sql += "WHERE  AgentNumericCode = '" + dataValue[0] + "' and DateofIssue between '" + dataValue[1] + "' and '" + dataValue[2] + "'";
            DataSet ds = new DataSet();
            ds = dbconnect.RetObjDS(sql);
            return PartialView(ds);
        }

    }
}