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
    public class LiftController : Controller
    {
        //public string pbConnectionString = "Server=.\\RELATE;Database=OnsiteBiatss_KK;User Id=sa; Password=1234";
        //public string pbConnectionString = "Server=FANO-PC;Database=OnsiteBiatss_KK;User Id=so; Integrated Security=True";
        // public string pbConnectionString = "Server=DESKTOP-O0K2BQJ\\SA;Database=OnsiteBiatss_KK;User Id=sa; Password=1234";
        //public string pbConnectionString = "Server=DESKTOP-7HJUR50;Database=OnsiteBiatss_KK;User Id=sa; Password=1234";
        //public string pbConnectionString = "Server=DESKTOP-CGR76E3;Database=OnsiteBiatss_KK;User Id=sa; Password=1234";
        //public string pbConnectionString = "Server=DESKTOP-K56R42H;Database=OnsiteBiatss_KK;User Id=sa; Password=1234";
        // public string pbConnectionString = "Server=DESKTOP-O0K2BQJ\\SA;Database=OnsiteBiatss_KK;User Id=sa; Password=1234";
        public string pbConnectionString = "Server=DESKTOP-Q821GFS;Database=OnsiteBiatss_KK;User Id=sa; Password=1234";
        //public string pbConnectionString = "Server=DESKTOP-54APPF8\\SYMPHONY2;Database=OnsiteBiatss_KK;User Id=sa; Password=1234";
        ConnexionSQLServer.DbConnection dbconnect = new ConnexionSQLServer.DbConnection();

        // GET: lift
        public ActionResult Index()
        {
            return PartialView();
        }

        public string ConvertDate1(string date)
        {
            var culture = System.Globalization.CultureInfo.CurrentCulture;
            string mydate = DateTime.ParseExact(date, "dd-MMM-yyyy", new CultureInfo(culture.Name)).ToString("MM-dd-yyyy");
            return mydate;
        }

        public string ConvertDate(string date)
        {
            string mydate = DateTime.ParseExact(date, "dd-MMM-yyyy", CultureInfo.InvariantCulture).ToString("MM-dd-yyyy");
            return mydate;
        }

        /***************************Compare********************************/
        public ActionResult Compare()
        {
            ViewBag.dateFrom = DateTime.Now.AddMonths(-1).ToString("dd-MMM-yyyy");
            ViewBag.dateTo = DateTime.Now.ToString("dd-MMM-yyyy");
            return PartialView();
        }

        public ActionResult getCompFln()
        {
            string dateFrom1 = Request["dateFrom"];
            string dateFrom = ConvertDate(dateFrom1);
            string dateTo1 = Request["dateTo"];
            string dateTo = ConvertDate(dateTo1);
            /*SqlConnection con = new SqlConnection(pbConnectionString);
            SqlCommand cmd = new SqlCommand("[Pax].[SP_SLS_Flights]", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 300;
            DataSet ds = new DataSet();
            SqlDataAdapter ada = new SqlDataAdapter(cmd);
            ada.Fill(ds);
            con.Close();*/
            string sql= "select distinct  UsageFlightNumber from pax.SalesDocumentCoupon f1 where CouponStatus = 'F' and UsageDate between'" + dateFrom + "' and '" + dateTo + "' order by 1 " + Environment.NewLine;
            SqlConnection con = new SqlConnection(pbConnectionString);
            DataSet ds = new DataSet();
            SqlDataAdapter ada = new SqlDataAdapter(sql, con);
            ada.Fill(ds);
            con.Close();
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

        public ActionResult loadCompare()
        {
            string dateFrom1 = Request["dateFrom"];
            string dateFrom = ConvertDate(dateFrom1);
            string dateTo1 = Request["dateTo"];
            string dateTo = ConvertDate(dateTo1);
            string flc = Request["flnNum"];
            string fl = "";
            if (flc == "-All" || flc == "")
            {
                fl = "%";
            }
            else
            {
                fl = flc;
            }
            SqlConnection con = new SqlConnection(pbConnectionString);
           //SqlCommand cmd = new SqlCommand("[Pax].[SP_Compare]", con);
            SqlCommand cmd = new SqlCommand("[Pax].[SP_SLSvLIFT]", con);
            cmd.CommandTimeout = 1000;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@FromUsageDate", dateFrom);
            cmd.Parameters.AddWithValue("@ToUsageDate", dateTo);
            cmd.Parameters.AddWithValue("@FlightNumber", fl);
            cmd.Parameters.AddWithValue("@selection", 2);

           /* cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 300;
            cmd.Parameters.AddWithValue("@dateFrom", dateFrom);
            cmd.Parameters.AddWithValue("@dateTo", dateTo);
            // cmd.Parameters.AddWithValue("@AgentNumCode", ag);
            cmd.Parameters.AddWithValue("@Flight", fl);*/
            DataSet ds = new DataSet();
            SqlDataAdapter ada = new SqlDataAdapter(cmd);
            ada.Fill(ds);
            con.Close();
            Int64? paxSLS = 0;
            Int64? paxLIFT = 0;
            int lonAg = ds.Tables[0].Rows.Count;
            ViewBag.Compt = lonAg;
            string[,] compare = new string[8,lonAg];
            int i = 0;
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                compare[0, i] = Convert.ToDateTime(dr[ds.Tables[0].Columns[0].ColumnName].ToString()).ToString("dd-MMM-yyyy");
                compare[1, i] = dr[ds.Tables[0].Columns[1].ColumnName].ToString();
                compare[2, i] = dr[ds.Tables[0].Columns[2].ColumnName].ToString(); ;
                compare[3, i] = dr[ds.Tables[0].Columns[3].ColumnName].ToString(); ;
                compare[4, i] = dr[ds.Tables[0].Columns[4].ColumnName].ToString(); ;
                int slsPax = 0;
                int LiftPax = 0;
                if (dr[ds.Tables[0].Columns[5].ColumnName].ToString() != null && dr[ds.Tables[0].Columns[5].ColumnName].ToString()!="")
                {
                    slsPax = Convert.ToInt16(dr[ds.Tables[0].Columns[5].ColumnName].ToString());
                }
                compare[5, i] = slsPax.ToString();
                if(dr[ds.Tables[0].Columns[6].ColumnName].ToString() != null && dr[ds.Tables[0].Columns[6].ColumnName].ToString()!="")
                {
                    LiftPax = Convert.ToInt16(dr[ds.Tables[0].Columns[6].ColumnName].ToString());
                }
                compare[6, i] = LiftPax.ToString();
                compare[7, i] = (slsPax - LiftPax).ToString();
                paxSLS += slsPax;
                paxLIFT += LiftPax;
                i++;
            }
            ViewBag.compare = compare;
            ViewBag.sls = paxSLS;
            ViewBag.Lift = paxLIFT;
            ViewBag.Diff = paxSLS - paxLIFT;
            return PartialView();
        }

        /*******************************endCompare***************************************/
        /*******************************BegingFlownRevenue*****************************************/

        public ActionResult FlownRevenue()
        {
            string sql = "   select distinct year(sdc.UsageDate)*100+month(sdc.UsageDate) as Period FROM pax.SalesDocumentCoupon sdc " + Environment.NewLine;
            sql += "join Pax.ProrationDetail pd ON sdc.RelatedDocumentGuid=pd.RelatedDocumentGuid  AND sdc.CouponNumber=pd.CouponNumber  " + Environment.NewLine;
            sql += "where sdc.CouponStatus in ( 'F', 'E' ) order by 1 desc " + Environment.NewLine;
            SqlConnection con = new SqlConnection(pbConnectionString);
            DataSet ds = new DataSet();
            SqlDataAdapter ada = new SqlDataAdapter(sql, con);
            ada.Fill(ds);
            con.Close();
            int lonAg = ds.Tables[0].Rows.Count;
            string[] Period = new string[lonAg];
            int i = 0;
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                Period[i] = dr[ds.Tables[0].Columns[0].ColumnName].ToString();
                i = i + 1;
            }

            ViewBag.Period = Period;
            ViewBag.lonAg = lonAg;
            return PartialView();
        }

        public ActionResult LoadFlownRevenue()
        {
            string period = Request["Period"];
            var culture = System.Globalization.CultureInfo.CurrentCulture;
            string sql = "" + Environment.NewLine;

            /*sql += "select year(sdc.UsageDate)*100+month(sdc.UsageDate) as Period, concat( sdc.UsageOriginCode, sdc.UsageDestinationCode ) as Sector " + Environment.NewLine;
            sql += ", sdc.ReservationBookingDesignator AS RBD , sum(pd.FinalShare) AS FlownRevenue " + Environment.NewLine;
            sql += "FROM pax.SalesDocumentCoupon sdc " + Environment.NewLine;
            sql += "join Pax.ProrationDetail pd ON sdc.RelatedDocumentGuid=pd.RelatedDocumentGuid  AND sdc.CouponNumber=pd.CouponNumber " + Environment.NewLine;
            sql += "where sdc.CouponStatus in ( 'F', 'E' ) " + Environment.NewLine;
            sql += "and year(sdc.UsageDate)*100+month(sdc.UsageDate) BETWEEN '" + period.ToString() + "' and '" + period.ToString() + "' " + Environment.NewLine;
            sql += "group by year(sdc.UsageDate)*100+month(sdc.UsageDate), sdc.UsageOriginCode, sdc.UsageDestinationCode, sdc.ReservationBookingDesignator " + Environment.NewLine;
            sql += "order by 1,2 " + Environment.NewLine;*/

            sql += "with base as  " + Environment.NewLine;
            sql += "( " + Environment.NewLine;
            sql += "select cast(year(sdc.UsageDate)*100+month(sdc.UsageDate) as varchar(15)) as Period " + Environment.NewLine;
            sql += ", concat( sdc.UsageOriginCode, sdc.UsageDestinationCode ) as Sector  " + Environment.NewLine;
            sql += ", sdc.ReservationBookingDesignator AS RBD, 0 as RBDorder , sum(pd.FinalShare) AS FlownRevenue,0 as PeriodOrder,0 as SectorOrder " + Environment.NewLine;
            sql += "FROM pax.SalesDocumentCoupon sdc  " + Environment.NewLine;
            sql += "join Pax.ProrationDetail pd ON sdc.RelatedDocumentGuid=pd.RelatedDocumentGuid  AND sdc.CouponNumber=pd.CouponNumber  " + Environment.NewLine;
            sql += "where sdc.CouponStatus in ( 'F', 'E' )  " + Environment.NewLine;
            if (period != "-ALL-")
            {
                sql += "and year(sdc.UsageDate)*100+month(sdc.UsageDate) BETWEEN '" + period.ToString() + "' and '" + period.ToString() + "'   " + Environment.NewLine;
            }
            sql += "group by year(sdc.UsageDate)*100+month(sdc.UsageDate), sdc.UsageOriginCode, sdc.UsageDestinationCode, sdc.ReservationBookingDesignator " + Environment.NewLine;
            sql += ") " + Environment.NewLine;

            sql += "select Period,Sector,RBD,FlownRevenue " + Environment.NewLine;
            sql += "from ( " + Environment.NewLine;

            sql += "select period, 'Period Total' as Sector,'' as RBD,1 as RBDorder,Sum(FlownRevenue) as FlownRevenue,0 as PeriodOrder,1 as SectorOrder " + Environment.NewLine;
            sql += "from base " + Environment.NewLine;
            sql += "group by Period " + Environment.NewLine;

            sql += "UNION " + Environment.NewLine;

            sql += "select 'Grand Total'  as period,'' as Sector,'' as RBD,0 as RBDorder,Sum(FlownRevenue) as FlownRevenue,2 as PeriodOrder, 0 as SectorOrder " + Environment.NewLine;
            sql += "from base " + Environment.NewLine;

            sql += "UNION " + Environment.NewLine;

            sql += "select * from Base " + Environment.NewLine;

            sql += "UNION  " + Environment.NewLine;

            sql += "select period,Sector,'Sector Total' as RBD,1 as RBDorder,Sum(FlownRevenue) as FlownRevenue,0 as PeriodOrder,0 as SectorOrder " + Environment.NewLine;
            sql += "from base group by Period,Sector " + Environment.NewLine;
            sql += ") a  " + Environment.NewLine;
            sql += "order by PeriodOrder, period,SectorOrder,Sector,RBDorder,RBD " + Environment.NewLine;

            SqlConnection con = new SqlConnection(pbConnectionString);
            DataSet ds = new DataSet();
            SqlDataAdapter ada = new SqlDataAdapter(sql, con);
            ada.Fill(ds);
            con.Close();
            int ligne= ds.Tables[0].Rows.Count;
            string[,] flownData = new string[4,ligne];
            int i = 0;
            foreach(DataRow dr in ds.Tables[0].Rows)
            {
               
                flownData[0,i] = dr[ds.Tables[0].Columns[0].ColumnName].ToString();
               
                flownData[1, i] = dr[ds.Tables[0].Columns[1].ColumnName].ToString();
                
                flownData[2, i] = dr[ds.Tables[0].Columns[2].ColumnName].ToString();
                
                flownData[3, i] = dr[ds.Tables[0].Columns[3].ColumnName].ToString();
                i++;
            }
            decimal tot = 0;
            /*for(int j=0; j< ligne; j++)
            {
                if(flownData[3, j] !=null && !string.IsNullOrWhiteSpace(flownData[3, j].ToString()))
                {
                    tot += Convert.ToDecimal(flownData[3, j], new CultureInfo(culture.Name));
                }
            }*/
            tot = Convert.ToDecimal(flownData[3, ligne-1], new CultureInfo(culture.Name));
            ViewBag.flownData = flownData;
            ViewBag.Tot = tot.ToString("##,###,###,###.#0");
            ViewBag.ligne = ligne;
            return PartialView();
        }
        /**************************************endFlownRevenue**********************************/

        /*********************************BegingTfcFlown*****************************************/
        public ActionResult TFCFLOWN( string type)
        {
            ViewBag.dateFrom = DateTime.Now.AddMonths(-1).ToString("dd-MMM-yyyy");
            ViewBag.dateTo = DateTime.Now.ToString("dd-MMM-yyyy");
            ViewBag.type = type;
            return PartialView();
        }

        public ActionResult getflightNum()
        {
            string dateFrom1 = Request["dateFrom"];
            string dateFrom = ConvertDate(dateFrom1);
            string dateTo1 = Request["dateTo"];
            string dateTo = ConvertDate(dateTo1);
            string agc = Request["AgCode"];
            string sql = "";
            string ag = "";
            if (agc == "-All-" || agc=="")
            {
                ag = "%";
            }
            else
            {
                ag = agc;
            }
            sql = sql + "Select Distinct UsageFlightNumber FROM PAX.VW_LiftForTransportation where  UsageDate between convert(datetime,'" + dateFrom + "',105)  and convert(datetime,'" + dateTo + "',105) and AgentNumericCode like '" + ag + "'";
            SqlConnection con = new SqlConnection(pbConnectionString);
            DataSet ds1 = new DataSet();
            SqlDataAdapter ada1 = new SqlDataAdapter(sql, con);
            ada1.Fill(ds1);
            con.Close();
            int lon = ds1.Tables[0].Rows.Count;
            string[] flnum = new string[lon];
            int j = 0;
            foreach (DataRow dr in ds1.Tables[0].Rows)
            {
                flnum[j] = dr[ds1.Tables[0].Columns[0].ColumnName].ToString();
                j = j + 1;
            }
            ViewBag.Flnum = flnum;
            ViewBag.lon = lon;
            return PartialView();
        }

        public ActionResult getAgcode()
        {
            string dateFrom = Request["dateFrom"];
            //string dateFrom = ConvertDate(dateFrom1);
            string dateTo = Request["dateTo"];
            //string dateTo = ConvertDate(dateTo1);
            string flc = Request["flightNum"];
            string sql = "";
            string fl = "";
            if(flc=="-All-" || flc == "")
            {
                fl = "%";
            }
            else
            {
                fl = flc;
            }

            sql = sql + "Select Distinct AgentNumericCode FROM PAX.VW_LiftForTransportation where  UsageDate between convert(datetime,'" + dateFrom + "',105)  and convert(datetime,'" + dateTo + "', 105) and UsageFlightNumber like '" + fl + "' ";
            SqlConnection con = new SqlConnection(pbConnectionString);
            DataSet ds1 = new DataSet();
            SqlDataAdapter ada1 = new SqlDataAdapter(sql, con);
            ada1.Fill(ds1);
            con.Close();
            int lon = ds1.Tables[0].Rows.Count;
            string[] flnum = new string[lon];
            int j = 0;
            foreach (DataRow dr in ds1.Tables[0].Rows)
            {
                flnum[j] = dr[ds1.Tables[0].Columns[0].ColumnName].ToString();
                j = j + 1;
            }
            ViewBag.Flnum = flnum;
            ViewBag.lon = lon;
            return PartialView();
        }

        public ActionResult LoadTfcsFlown()
        {
            DateTime? testDate= Convert.ToDateTime(Request["dateFrom"]);
            decimal subtotGrossVal = 0;
            decimal subtotComm = 0;
            decimal subUATP = 0;
            double subTotTax = 0;
            double subNetAmt = 0;

            decimal totGrossVal = 0;
            decimal totComm = 0;
            decimal totUATP = 0;
            double totTax = 0;
            double totNetAmt = 0;

            decimal lsubtotGrossVal = 0;
            decimal lsubtotComm = 0;
            decimal lsubUATP = 0;
            double lsubTotTax = 0;
            double lsubNetAmt = 0;
            string dateFrom = Request["dateFrom"];
            //string dateFrom = ConvertDate(dateFrom1);
            string dateTo = Request["dateTo"];
            //string dateTo = ConvertDate(dateTo1);
            string agc = Request["AgCode"];
            string flc = Request["flightNum"];
            var culture = System.Globalization.CultureInfo.CurrentCulture;

            string ag = "";
            string fl = "";
            if (flc == "-All-" || flc == "")
            {
                fl = "%";
            }
            else
            {
                fl = flc;
            }
            if (agc == "-All-" || agc == "")
            {
                ag = "%";
            }
            else
            {
                ag = agc;
            }
            SqlConnection con = new SqlConnection(pbConnectionString);
            SqlCommand cmd = new SqlCommand("[Pax].[SP_TFCflown]", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 300;
            cmd.Parameters.AddWithValue("@dateFrom", dateFrom);
            cmd.Parameters.AddWithValue("@dateTo", dateTo);
            cmd.Parameters.AddWithValue("@AgentNumCode", ag);
            cmd.Parameters.AddWithValue("@FlightNum", fl);
            DataSet ds = new DataSet();
            SqlDataAdapter ada = new SqlDataAdapter(cmd);
            ada.Fill(ds);
            int lon = ds.Tables[0].Rows.Count;
            con.Close();
            ViewBag.compt = lon;
            String[,] tableau = new String[13, lon];
            int i = 0;
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                tableau[0, i] = dr[ds.Tables[0].Columns[0].ColumnName].ToString();
                tableau[1, i] = dr[ds.Tables[0].Columns[1].ColumnName].ToString();
                tableau[2, i] = dr[ds.Tables[0].Columns[2].ColumnName].ToString();
                tableau[3, i] = dr[ds.Tables[0].Columns[3].ColumnName].ToString();
                tableau[4, i] = dr[ds.Tables[0].Columns[4].ColumnName].ToString();
                tableau[5, i] = dr[ds.Tables[0].Columns[5].ColumnName].ToString();
                tableau[6, i] = dr[ds.Tables[0].Columns[6].ColumnName].ToString();
                string doi = "";
                if (dr[ds.Tables[0].Columns[7].ColumnName].ToString() != "null")
                {
                    doi = dr[ds.Tables[0].Columns[6].ColumnName].ToString();
                }
                tableau[7, i] = doi;
                tableau[8, i] = dr[ds.Tables[0].Columns[8].ColumnName].ToString();
                tableau[9, i] = dr[ds.Tables[0].Columns[9].ColumnName].ToString();
                tableau[10, i] = dr[ds.Tables[0].Columns[10].ColumnName].ToString();
                string amt = "";
                decimal amt1 = 0;
                amt = dr[ds.Tables[0].Columns[11].ColumnName].ToString();
                if ((amt != null) && (amt != ""))
                {
                    amt1 = Convert.ToDecimal(amt, new CultureInfo(culture.Name));
                }
                tableau[11, i] = amt1.ToString("####.##");
                string net = "";
                if(dr[ds.Tables[0].Columns[12].ColumnName].ToString() == "null")
                {
                    net = "0.0";
                }
                else
                {
                    net = dr[ds.Tables[0].Columns[12].ColumnName].ToString();
                }
                tableau[12, i] = net;
                //testDate = Convert.ToDateTime(doi);
                subtotGrossVal += Convert.ToDecimal(dr[ds.Tables[0].Columns[8].ColumnName].ToString(), new CultureInfo(culture.Name));
                subtotComm += Convert.ToDecimal(dr[ds.Tables[0].Columns[9].ColumnName].ToString(), new CultureInfo(culture.Name));
                subUATP += Convert.ToDecimal(dr[ds.Tables[0].Columns[10].ColumnName].ToString(), new CultureInfo(culture.Name));

                string h = dr[ds.Tables[0].Columns[11].ColumnName].ToString();
                if ((h != "null")&&(h!=""))
                {
                    subTotTax += Convert.ToDouble(dr[ds.Tables[0].Columns[11].ColumnName].ToString(), new CultureInfo(culture.Name));
                }
                subNetAmt += Convert.ToDouble(dr[ds.Tables[0].Columns[12].ColumnName].ToString(), new CultureInfo(culture.Name));

                totGrossVal += Convert.ToDecimal(dr[ds.Tables[0].Columns[8].ColumnName].ToString(), new CultureInfo(culture.Name));
                totComm += Convert.ToDecimal(dr[ds.Tables[0].Columns[9].ColumnName].ToString(), new CultureInfo(culture.Name));
                totUATP += Convert.ToDecimal(dr[ds.Tables[0].Columns[10].ColumnName].ToString(), new CultureInfo(culture.Name));

                string hh = dr[ds.Tables[0].Columns[11].ColumnName].ToString();

                if ((hh != null) && (hh != ""))
                {
                    totTax += Convert.ToDouble(dr[ds.Tables[0].Columns[11].ColumnName].ToString(), new CultureInfo(culture.Name));
                }

                totNetAmt += Convert.ToDouble(dr[ds.Tables[0].Columns[12].ColumnName].ToString(), new CultureInfo(culture.Name));
                lsubtotGrossVal = subtotGrossVal;
                lsubtotComm = subtotComm;
                lsubUATP = subUATP;
                lsubTotTax = subTotTax;
                lsubNetAmt = Convert.ToDouble(subNetAmt);

                lsubtotGrossVal = subtotGrossVal;
                lsubtotComm = subtotComm;
                lsubUATP = subUATP;
                lsubTotTax = subTotTax;
                lsubNetAmt = Convert.ToDouble(subNetAmt, new CultureInfo(culture.Name));
                

                i++;

            }

            string[,] tabtot = new string[13, 1];
            tabtot[0, 0] = "";
            tabtot[1, 0] = "";
            tabtot[2, 0] = "";
            tabtot[3, 0] = "";
            tabtot[4, 0] = "";
            tabtot[5, 0] = "";
            tabtot[6, 0] = "";
            tabtot[7, 0] = "Total";
            tabtot[8, 0] = lsubtotGrossVal.ToString();
            tabtot[9, 0] = lsubtotComm.ToString();
            tabtot[10, 0] = lsubUATP.ToString();
            tabtot[11, 0] = lsubTotTax.ToString();
            tabtot[12, 0] = lsubNetAmt.ToString();
            ViewBag.tableau = tableau;
            ViewBag.tabtot = tabtot;
            ViewBag.Grossval = subtotGrossVal;
            ViewBag.Com = subtotComm;
            ViewBag.Uatp = subUATP;
            ViewBag.Tax = subTotTax;
            ViewBag.Net = subNetAmt;
            return PartialView();
        }
        /***************************************************endTfcFlown*********************************************/
        /********************************************BegingPassengerLift********************************************/
        public ActionResult PassengerLift(string type)
        {
            ViewBag.dateFrom = DateTime.Now.AddMonths(-1).ToString("dd-MMM-yyyy");
            ViewBag.dateTo = DateTime.Now.ToString("dd-MMM-yyyy");
            ViewBag.typeId = type;
            return PartialView();
        }

        public ActionResult loadAgAdresse()
        {
            string AgCode = Request["agcode"];
            string sql = "select LegalName,LocationAddress from Ref.VW_Agent where AgencyNumericCode =left('" + AgCode + "',7) ";
            ViewBag.name = Request["id2"];
            ViewBag.adresse = Request["adresse"];
            SqlConnection con = new SqlConnection(pbConnectionString);
            DataSet ds = new DataSet();
            SqlDataAdapter ada = new SqlDataAdapter(sql, con);
            ada.Fill(ds);
            int lon = ds.Tables[0].Rows.Count;
            string[,] location = new string[2,1];
            
            if (lon > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    location[0,0] = dr[ds.Tables[0].Columns[0].ColumnName].ToString();
                    location[1, 0] = dr[ds.Tables[0].Columns[1].ColumnName].ToString();
                    
                }
            }
            else
            {
                location[0, 0] = "";
                location[1, 0] = "";
            }
            
            ViewBag.Location = location;
            return PartialView();

        }
        public ActionResult loadAgAdresse1()
        {
            string AgCode = Request["agcode"];
            string sql = "select LegalName,LocationAddress from Ref.VW_Agent where AgencyNumericCode =left('" + AgCode + "',7) ";
            SqlConnection con = new SqlConnection(pbConnectionString);
            DataSet ds = new DataSet();
            SqlDataAdapter ada = new SqlDataAdapter(sql, con);
            ada.Fill(ds);
            int lon = ds.Tables[0].Rows.Count;
            string[,] location = new string[2, 1];

            if (lon > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    location[0, 0] = dr[ds.Tables[0].Columns[0].ColumnName].ToString();
                    location[1, 0] = dr[ds.Tables[0].Columns[1].ColumnName].ToString();

                }
            }
            else
            {
                location[0, 0] = "";
                location[1, 0] = "";
            }

            ViewBag.Location = location;
            return PartialView();
        }
        public ActionResult getAgcodePass()
        {
            string dateFrom = Request["dateFrom"];
            string dateTo = Request["dateTo"];
            string statut = Request["statut"];
            string sql = " select distinct sdh.AgentNumericCode from Pax.SalesDocumentHeader SDH ";
            sql += "join Pax.SalesRelatedDocumentInformation SRD on SDH.HdrGuid = SRD.HdrGuid ";
            sql += " join Pax.SalesDocumentCoupon SDC on SRD.RelatedDocumentGuid = SDC.RelatedDocumentGuid where CouponStatus ='"+statut+"' and DateofIssue between '" + dateFrom + "'and '" + dateTo + "'";
            //SqlCommand cmd = new SqlCommand("" + sql + "", cs);
            SqlConnection con = new SqlConnection(pbConnectionString);
            DataSet ds1 = new DataSet();
            SqlDataAdapter ada1 = new SqlDataAdapter(sql, con);
            ada1.Fill(ds1);
            con.Close();
            int lon = ds1.Tables[0].Rows.Count;
            string[] flnum = new string[lon];
            int j = 0;
            foreach (DataRow dr in ds1.Tables[0].Rows)
            {
                flnum[j] = dr[ds1.Tables[0].Columns[0].ColumnName].ToString();
                j = j + 1;
            }
            ViewBag.Flnum = flnum;
            ViewBag.lon = lon;
            return PartialView();
        }

        public ActionResult loadTransportation()
        {
            string dateFrom = Request["dateFrom"];
            //string dateFrom = ConvertDate(dateFrom1);
            string dateTo = Request["dateTo"];
            //string dateTo = ConvertDate(dateTo1);
            string agc = Request["agc"];
            string check = Request["check"];
            string ag = "";
            string fln = "";
            string flnc = Request["fln"];
            if (agc == "-All-" || agc=="")
            {
                ag = "%";
            }
            else
            {
                ag = agc;
            }
            if (flnc == "-All-" || flnc == "")
            {
                fln = "%";
            }
            else
            {
                fln = flnc;
            }
            string sql = "";
            sql = sql + " with a as  " + Environment.NewLine;
            sql = sql + " (  " + Environment.NewLine;
            sql = sql + " select  " + Environment.NewLine;
            //sql = sql + " year(sdc.UsageDate)*100+month(sdc.UsageDate) as BillingPeriod " + Environment.NewLine;
            sql = sql + "  sdc.RelatedDocumentNumber as [Document No]   " + Environment.NewLine;
            sql = sql + " , sdc.CouponNumber as [Coupon No]    " + Environment.NewLine;
            sql = sql + " , sdc.UsageOriginCode as [Origin]    " + Environment.NewLine;
            sql = sql + " , sdc.UsageDestinationCode as [Dest]    " + Environment.NewLine;
            sql = sql + " , FORMAT(try_cast(sdc.UsageFlightNumber as int),'00000') as FlightNo    " + Environment.NewLine;
            sql = sql + " , sdc.UsageDate as FlightDate    " + Environment.NewLine;
            sql = sql + " , sdc.UsedClassofService    " + Environment.NewLine;
            sql = sql + " , 'USD' AS Currency  " + Environment.NewLine;
            sql = sql + "   " + Environment.NewLine;
            sql = sql + " , iif( exists( select * from Pax.SalesDocumentPayment sdp where srd.RelatedDocumentGuid = sdp.RelatedDocumentGuid and  sdp.FormOfPaymentType = 'EX' ) , 'Y', '' )   " + Environment.NewLine;
            sql = sql + " as [Exchanged Ticket]  " + Environment.NewLine;
            sql = sql + " , sdc.BillingType  " + Environment.NewLine;
            sql = sql + " , sdh.BspIdentifier as BSP  " + Environment.NewLine;
            sql = sql + " , left(sdh.AgentNumericCode,7) as Agent  " + Environment.NewLine;
            sql = sql + " , iif( sdh.OwnTicket = 'Y', com.Rate, null) as [Agent Remuneration Rate]  " + Environment.NewLine;
            sql = sql + " , sdo.OtherAmountRate as [Applied Sales Remuneration Rate]  " + Environment.NewLine;
            sql = sql + " , cast( isnull( (p.FinalShare  * sdo.OtherAmountRate) / 100, 0 ) as numeric(18,2) ) as [Agent Remuneration Amount]  " + Environment.NewLine;
            sql = sql + " , p.FinalShare AS grossValue  " + Environment.NewLine;
            sql = sql + " , iif(       sdc.BillingType is null, 0, iif( p.SpecialProrateAgreement > 0, p.IscPercent, 9 ) ) as ISCPerc  " + Environment.NewLine;
            sql = sql + " , cast( iif( sdc.BillingType is null, 0, iif( p.SpecialProrateAgreement > 0, p.IscPercent, 9 ) ) * 0.01 * p.FinalShare AS decimal(18, 2) ) *  iif( sdc.BillingType = 'O', -1, 1 )  AS [ISC Amount]  " + Environment.NewLine;
            sql = sql + " , 0.00 AS uatpAmount    " + Environment.NewLine;
            sql = sql + " , 0 AS handlingFee  " + Environment.NewLine;
            sql = sql + "   " + Environment.NewLine;
            sql = sql + " ,(SELECT TOP (1) TaxAmount /    " + Environment.NewLine;
            sql = sql + " (SELECT USDRate FROM Ref.CurrencyRate WHERE (Currency = Pax.VW_ApplicableTaxes.TaxCurrency)     " + Environment.NewLine;
            sql = sql + " AND (Period = SUBSTRING(CAST(sdh.DateofIssue AS nvarchar), 1, 4) + SUBSTRING(CAST(sdh.DateofIssue AS nvarchar), 6, 2))) AS Expr1    " + Environment.NewLine;
            sql = sql + " FROM Pax.VW_ApplicableTaxes    " + Environment.NewLine;
            sql = sql + " WHERE (RelatedDocumentNumber = sdc.RelatedDocumentNumber) AND (CouponNumber = sdc.CouponNumber)) AS totalTax  " + Environment.NewLine;
            sql = sql + "   " + Environment.NewLine;
            sql = sql + " ,case  " + Environment.NewLine;
            sql = sql + " when sdc.CouponStatus = 'F' then '1'   " + Environment.NewLine;
            sql = sql + " when sdc.CouponStatus = 'E' then '2'    " + Environment.NewLine;
            sql = sql + " when sdc.CouponStatus = 'R' then '3'   " + Environment.NewLine;
            sql = sql + " else null   " + Environment.NewLine;
            sql = sql + " end AS SourceCode    " + Environment.NewLine;
            sql = sql + "   " + Environment.NewLine;
            sql = sql + " ,sdc.CouponStatus    " + Environment.NewLine;
            sql = sql + " ,sdc.SettlementAuthorizationCode    " + Environment.NewLine;
            sql = sql + " ,OwnTicket    " + Environment.NewLine;
            sql = sql + "   " + Environment.NewLine;
            sql = sql + " from Pax.SalesDocumentHeader SDH  " + Environment.NewLine;
            sql = sql + " join Pax.SalesRelatedDocumentInformation SRD on SDH.HdrGuid = SRD.HdrGuid   " + Environment.NewLine;
            sql = sql + " join Pax.SalesDocumentCoupon SDC on SRD.RelatedDocumentGuid = SDC.RelatedDocumentGuid  " + Environment.NewLine;
            sql = sql + " left join Pax.ProrationDetail AS p ON sdc.RelatedDocumentGuid = p.RelatedDocumentGuid and sdc.CouponNumber = p.CouponNumber and  sdc.CouponStatus = p.ProrationFlag  " + Environment.NewLine;
            sql = sql + "   " + Environment.NewLine;
            sql = sql + " left join Ref.PaxAgencyDetails com  " + Environment.NewLine;
            sql = sql + " on  left(sdh.AgentNumericCode,7) = com.AgencyNumericCode and sdh.DateofIssue between com.PeriodFrom and com.PeriodTo  " + Environment.NewLine;
            sql = sql + " left join Pax.SalesDocumentOtherAmount sdo  " + Environment.NewLine;
            sql = sql + " on sdc.BillingType is null and srd.RelatedDocumentGuid = sdo.RelatedDocumentGuid and sdo.DocumentAmountType like 'Com%' and sdo.OtherAmountCode = 'Effective'  " + Environment.NewLine;
            sql = sql + "   " + Environment.NewLine;
            sql = sql + " WHERE sdc.UsageDate IS NOT NULL  " + Environment.NewLine;
            sql = sql + " and UsageDate between  cast('" + dateFrom + "' as Date) and  cast('" + dateTo + "' as Date) " + Environment.NewLine;
            sql = sql + " and CouponStatus='F' and sdh.AgentNumericCode like  '" + ag + "'  " + Environment.NewLine;
            sql = sql + "and carrier+format(try_cast(UsageFlightNumber as int) ,'00000') +' - '+UsageOriginCode+UsageDestinationCode like  '" + fln + "'  " + Environment.NewLine;
            sql = sql + " )  " + Environment.NewLine;
            sql = sql + "   " + Environment.NewLine;
            sql = sql + " select a.* , grossValue - isnull([Agent Remuneration Amount],0) + isnull([ISC Amount],0) - UatpAmount AS NetAmount from a  " + Environment.NewLine;
            // sql = sql + "where OwnTicket = '" + check + "'" + Environment.NewLine;
            if (check == "N")
            {
                sql = sql + " where OwnTicket =  'N' ";

            }
            else if (check == "Y")
            {
                sql = sql + " where OwnTicket =  'Y' ";
            }
            else {}
            SqlConnection con = new SqlConnection(pbConnectionString);
            DataSet ds = new DataSet();
            SqlDataAdapter ada = new SqlDataAdapter(sql, con);
            ada.Fill(ds);
            var culture = System.Globalization.CultureInfo.CurrentCulture;
            int lon = ds.Tables[0].Rows.Count;
            string[,] Transportation = new string[28, lon];
            decimal netval = 0;
            int i = 0;
            foreach(DataRow dr in ds.Tables[0].Rows)
            {
                Transportation[0,i]= dr[ds.Tables[0].Columns[0].ColumnName].ToString();
                Transportation[1, i] = dr[ds.Tables[0].Columns[1].ColumnName].ToString();
                Transportation[2, i] = dr[ds.Tables[0].Columns[2].ColumnName].ToString();
                Transportation[3, i] = dr[ds.Tables[0].Columns[3].ColumnName].ToString();
                Transportation[4, i] = dr[ds.Tables[0].Columns[4].ColumnName].ToString();
                string h = dr[ds.Tables[0].Columns[5].ColumnName].ToString();
                if (h != "")
                {
                    Transportation[5, i] = Convert.ToDateTime(h).ToString("dd-MMM- yyyy");
                }
                else
                {
                    Transportation[5, i] = h;
                }
                Transportation[6, i] = dr[ds.Tables[0].Columns[6].ColumnName].ToString();
                Transportation[7, i] = dr[ds.Tables[0].Columns[7].ColumnName].ToString();
                Transportation[8, i] = dr[ds.Tables[0].Columns[8].ColumnName].ToString();
                Transportation[9, i] = dr[ds.Tables[0].Columns[9].ColumnName].ToString();
                Transportation[10, i] = dr[ds.Tables[0].Columns[10].ColumnName].ToString();
                Transportation[11, i] = dr[ds.Tables[0].Columns[11].ColumnName].ToString();
                Transportation[12, i] = dr[ds.Tables[0].Columns[12].ColumnName].ToString();
                Transportation[13, i] = dr[ds.Tables[0].Columns[13].ColumnName].ToString();
                Transportation[14, i] = dr[ds.Tables[0].Columns[14].ColumnName].ToString();
                Transportation[15, i] = dr[ds.Tables[0].Columns[15].ColumnName].ToString();
                Transportation[16, i] = dr[ds.Tables[0].Columns[16].ColumnName].ToString();
                Transportation[17, i] = dr[ds.Tables[0].Columns[17].ColumnName].ToString();
                Transportation[18, i] = dr[ds.Tables[0].Columns[18].ColumnName].ToString();
                Transportation[19, i] = dr[ds.Tables[0].Columns[19].ColumnName].ToString();
                Transportation[20, i] = dr[ds.Tables[0].Columns[20].ColumnName].ToString();
                Transportation[21, i] = dr[ds.Tables[0].Columns[21].ColumnName].ToString();
                Transportation[22, i] = dr[ds.Tables[0].Columns[22].ColumnName].ToString();
                Transportation[23, i] = dr[ds.Tables[0].Columns[23].ColumnName].ToString();
                Transportation[24, i] = dr[ds.Tables[0].Columns[24].ColumnName].ToString();
                Transportation[25, i] = "";
                Transportation[26, i] = "";
                Transportation[27, i] = dr[ds.Tables[0].Columns[25].ColumnName].ToString();
                if (Transportation[27, i].ToString() != null && !string.IsNullOrWhiteSpace(Transportation[27, i].ToString()))
                {
                    netval += Convert.ToDecimal(Transportation[27, i].ToString(), new CultureInfo(culture.Name));
                }
                //Transportation[20, i] = dr[ds.Tables[0].Columns[21].ColumnName].ToString();

                i++;
            }

            
            ViewBag.Trasportation = Transportation;
            ViewBag.lon = lon; 
            ViewBag.netval = netval.ToString("#,###,###,###.#0"); ;
            return PartialView(ds);
        }

        public ActionResult LoadSectorExc()
        {
            string dateFrom1 = Request["dateFrom"];
            string dateFrom = ConvertDate(dateFrom1);
            string dateTo1 = Request["dateTo"];
            string dateTo = ConvertDate(dateTo1);
            ViewBag.myId = Request["myId"];
            ViewBag.sector = Request["sector"];
            ViewBag.sectTest = Request["sectTest"];
            ViewBag.dateFrom = Request["Bdate"];
            ViewBag.dateTo = Request["Edate"];
            ViewBag.test = Request["test"];
            ViewBag.id = Request["id"];
            ViewBag.test1 = Request["test1"];
            ViewBag.test2 = Request["test2"];
            ViewBag.dateHiddenF = dateFrom1;
            ViewBag.dateHiddenT = dateTo1;
            string sql = "select carrier+format(try_cast(UsageFlightNumber as int) ,'00000') +' - '+UsageOriginCode+UsageDestinationCode as sector " + Environment.NewLine;
            sql += "from pax.SalesDocumentCoupon where  UsageFlightNumber <>'OPEN'  and UsageDate between  '" + dateFrom + "' and '" + dateTo + "'  and   couponstatus = 'E'   " + Environment.NewLine;
            sql += "group by    carrier+format(try_cast(UsageFlightNumber as int) ,'00000') +' - '+UsageOriginCode+UsageDestinationCode, couponstatus      " + Environment.NewLine;
            SqlConnection con = new SqlConnection(pbConnectionString);
            DataSet ds1 = new DataSet();
            SqlDataAdapter ada1 = new SqlDataAdapter(sql, con);
            ada1.Fill(ds1);
            con.Close();
            int lon = ds1.Tables[0].Rows.Count;
            string[] flnum = new string[lon];
            int j = 0;
            foreach (DataRow dr in ds1.Tables[0].Rows)
            {
                flnum[j] = dr[ds1.Tables[0].Columns[0].ColumnName].ToString();
                j = j + 1;
            }
            ViewBag.Flnum = flnum;
            ViewBag.lon = lon;
            return PartialView();
        }
        public ActionResult loadExchange()
        {
            string dateFrom1 = Request["dateFrom"];
            string dateFrom = ConvertDate(dateFrom1);
            string dateTo1 = Request["dateTo"];
            string dateTo = ConvertDate(dateTo1);
            string agc = Request["agc"];
            string check = Request["check"];
            string ag = "";
            string fln = "";
            string flnc = Request["fln"];
            if (agc == "-All-" || agc == "")
            {
                ag = "%";
            }
            else
            {
                ag = agc;
            }
            if (flnc == "-All-" || flnc == "")
            {
                fln = "%";
            }
            else
            {
                if (flnc.Length <= 9)
                {
                    fln = flnc + '%';
                }
                else
                {
                    fln = flnc;
                }
                
            }
            string sql = "";
            sql = sql + " with a as  " + Environment.NewLine;
            sql = sql + " (  " + Environment.NewLine;
            sql = sql + " select  " + Environment.NewLine;
            sql = sql + " sdc.RelatedDocumentNumber as [Document No]   " + Environment.NewLine;
            sql = sql + " , sdc.CouponNumber as [Coupon No]    " + Environment.NewLine;
            sql = sql + " , sdc.UsageOriginCode as [Origin]    " + Environment.NewLine;
            sql = sql + " , sdc.UsageDestinationCode as [Dest]    " + Environment.NewLine;
            sql = sql + " , FORMAT(try_cast(sdc.UsageFlightNumber as int),'00000') as FlightNo    " + Environment.NewLine;
            sql = sql + " , sdc.UsageDate as FlightDate    " + Environment.NewLine;
            sql = sql + " , sdc.UsedClassofService    " + Environment.NewLine;
            sql = sql + " , 'USD' AS Currency  " + Environment.NewLine;
            sql = sql + "   " + Environment.NewLine;
            sql = sql + " , iif( exists( select * from Pax.SalesDocumentPayment sdp where srd.RelatedDocumentGuid = sdp.RelatedDocumentGuid and  sdp.FormOfPaymentType = 'EX' ) , 'Y', '' )   " + Environment.NewLine;
            sql = sql + " as [Exchanged Ticket]  " + Environment.NewLine;
            sql = sql + " , sdc.BillingType  " + Environment.NewLine;
            sql = sql + " , sdh.BspIdentifier as BSP  " + Environment.NewLine;
            sql = sql + " , left(sdh.AgentNumericCode,7) as Agent  " + Environment.NewLine;
            sql = sql + " , iif( sdh.OwnTicket = 'Y', com.Rate, null) as [Agent Remuneration Rate]  " + Environment.NewLine;
            sql = sql + " , sdo.OtherAmountRate as [Applied Sales Remuneration Rate]  " + Environment.NewLine;
            sql = sql + " , cast( isnull( (p.FinalShare  * sdo.OtherAmountRate) / 100, 0 ) as numeric(18,2) ) as [Agent Remuneration Amount]  " + Environment.NewLine;
            sql = sql + " , p.FinalShare AS grossValue  " + Environment.NewLine;
            sql = sql + " , iif(       sdc.BillingType is null, 0, iif( p.SpecialProrateAgreement > 0, p.IscPercent, 9 ) ) as ISCPerc  " + Environment.NewLine;
            sql = sql + " , cast( iif( sdc.BillingType is null, 0, iif( p.SpecialProrateAgreement > 0, p.IscPercent, 9 ) ) * 0.01 * p.FinalShare AS decimal(18, 2) ) *  iif( sdc.BillingType = 'O', -1, 1 )  AS [ISC Amount]  " + Environment.NewLine;
            sql = sql + " , 0.00 AS uatpAmount    " + Environment.NewLine;
            sql = sql + " , 0 AS handlingFee  " + Environment.NewLine;
            sql = sql + "   " + Environment.NewLine;
            sql = sql + " ,(SELECT TOP (1) TaxAmount /    " + Environment.NewLine;
            sql = sql + " (SELECT USDRate FROM Ref.CurrencyRate WHERE (Currency = Pax.VW_ApplicableTaxes.TaxCurrency)     " + Environment.NewLine;
            sql = sql + " AND (Period = SUBSTRING(CAST(sdh.DateofIssue AS nvarchar), 1, 4) + SUBSTRING(CAST(sdh.DateofIssue AS nvarchar), 6, 2))) AS Expr1    " + Environment.NewLine;
            sql = sql + " FROM Pax.VW_ApplicableTaxes    " + Environment.NewLine;
            sql = sql + " WHERE (RelatedDocumentNumber = sdc.RelatedDocumentNumber) AND (CouponNumber = sdc.CouponNumber)) AS totalTax  " + Environment.NewLine;
            sql = sql + "   " + Environment.NewLine;
            sql = sql + " ,case  " + Environment.NewLine;
            sql = sql + " when sdc.CouponStatus = 'F' then '1'   " + Environment.NewLine;
            sql = sql + " when sdc.CouponStatus = 'E' then '2'    " + Environment.NewLine;
            sql = sql + " when sdc.CouponStatus = 'R' then '3'   " + Environment.NewLine;
            sql = sql + " else null   " + Environment.NewLine;
            sql = sql + " end AS SourceCode    " + Environment.NewLine;
            sql = sql + "   " + Environment.NewLine;
            sql = sql + " ,sdc.CouponStatus    " + Environment.NewLine;
            sql = sql + " ,sdc.SettlementAuthorizationCode    " + Environment.NewLine;
            sql = sql + " ,OwnTicket    " + Environment.NewLine;
            sql = sql + "   " + Environment.NewLine;
            sql = sql + " from Pax.SalesDocumentHeader SDH  " + Environment.NewLine;
            sql = sql + " join Pax.SalesRelatedDocumentInformation SRD on SDH.HdrGuid = SRD.HdrGuid   " + Environment.NewLine;
            sql = sql + " join Pax.SalesDocumentCoupon SDC on SRD.RelatedDocumentGuid = SDC.RelatedDocumentGuid  " + Environment.NewLine;
            sql = sql + " left join Pax.ProrationDetail AS p ON sdc.RelatedDocumentGuid = p.RelatedDocumentGuid and sdc.CouponNumber = p.CouponNumber and  sdc.CouponStatus = p.ProrationFlag  " + Environment.NewLine;
            sql = sql + "   " + Environment.NewLine;
            sql = sql + " left join Ref.PaxAgencyDetails com  " + Environment.NewLine;
            sql = sql + " on  left(sdh.AgentNumericCode,7) = com.AgencyNumericCode and sdh.DateofIssue between com.PeriodFrom and com.PeriodTo  " + Environment.NewLine;
            sql = sql + " left join Pax.SalesDocumentOtherAmount sdo  " + Environment.NewLine;
            sql = sql + " on sdc.BillingType is null and srd.RelatedDocumentGuid = sdo.RelatedDocumentGuid and sdo.DocumentAmountType like 'Com%' and sdo.OtherAmountCode = 'Effective'  " + Environment.NewLine;
            sql = sql + "   " + Environment.NewLine;
            sql = sql + " WHERE sdc.UsageDate IS NOT NULL  " + Environment.NewLine;
            sql = sql + " and FlightDepartureDate between '" + dateFrom + "'and '" + dateTo + "' " + Environment.NewLine;
            sql = sql + " and CouponStatus='E' and sdh.AgentNumericCode like  '" + ag + "'  " + Environment.NewLine;
            sql = sql + "and carrier+format(try_cast(UsageFlightNumber as int) ,'00000') +' - '+UsageOriginCode+UsageDestinationCode like  '" + fln + "'  " + Environment.NewLine;
            sql = sql + " )  " + Environment.NewLine;
            sql = sql + "   " + Environment.NewLine;
            sql = sql + " select a.* , grossValue - isnull([Agent Remuneration Amount],0) + isnull([ISC Amount],0) - UatpAmount AS NetAmount from a  " + Environment.NewLine;
            if (check == "N")
            {
                sql = sql + " where OwnTicket =  'N' ";
            }
            else if (check == "Y")
            {
                sql = sql + " where OwnTicket =  'Y' ";
            }
            else { }
            SqlConnection con = new SqlConnection(pbConnectionString);
            DataSet ds = new DataSet();
            SqlDataAdapter ada = new SqlDataAdapter(sql, con);
            ada.Fill(ds);
            var culture = System.Globalization.CultureInfo.CurrentCulture;
            int lon = ds.Tables[0].Rows.Count;
            string[,] Transportation = new string[26, lon];
            decimal netval = 0;
            int i = 0;
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                Transportation[0, i] = dr[ds.Tables[0].Columns[0].ColumnName].ToString();
                Transportation[1, i] = dr[ds.Tables[0].Columns[1].ColumnName].ToString();
                Transportation[2, i] = dr[ds.Tables[0].Columns[2].ColumnName].ToString();
                Transportation[3, i] = dr[ds.Tables[0].Columns[3].ColumnName].ToString();
                Transportation[4, i] = dr[ds.Tables[0].Columns[4].ColumnName].ToString();
                string h = dr[ds.Tables[0].Columns[5].ColumnName].ToString();
                if (h != "")
                {
                    Transportation[5, i] = Convert.ToDateTime(h).ToString("dd-MMM- yyyy");
                }
                else
                {
                    Transportation[5, i] = h;
                }
                Transportation[6, i] = dr[ds.Tables[0].Columns[6].ColumnName].ToString();
                Transportation[7, i] = dr[ds.Tables[0].Columns[7].ColumnName].ToString();
                Transportation[8, i] = dr[ds.Tables[0].Columns[8].ColumnName].ToString();
                Transportation[9, i] = dr[ds.Tables[0].Columns[9].ColumnName].ToString();
                Transportation[10, i] = dr[ds.Tables[0].Columns[10].ColumnName].ToString();
                Transportation[11, i] = dr[ds.Tables[0].Columns[11].ColumnName].ToString();
                Transportation[12, i] = dr[ds.Tables[0].Columns[12].ColumnName].ToString();
                Transportation[13, i] = dr[ds.Tables[0].Columns[13].ColumnName].ToString();
                Transportation[14, i] = dr[ds.Tables[0].Columns[14].ColumnName].ToString();
                Transportation[15, i] = dr[ds.Tables[0].Columns[15].ColumnName].ToString();
                Transportation[16, i] = dr[ds.Tables[0].Columns[16].ColumnName].ToString();
                Transportation[17, i] = dr[ds.Tables[0].Columns[17].ColumnName].ToString();
                Transportation[18, i] = dr[ds.Tables[0].Columns[18].ColumnName].ToString();
                Transportation[19, i] = dr[ds.Tables[0].Columns[19].ColumnName].ToString();
                Transportation[20, i] = dr[ds.Tables[0].Columns[20].ColumnName].ToString();
                Transportation[21, i] = dr[ds.Tables[0].Columns[21].ColumnName].ToString();
                Transportation[22, i] = dr[ds.Tables[0].Columns[22].ColumnName].ToString();
                Transportation[23, i] = dr[ds.Tables[0].Columns[23].ColumnName].ToString();
                Transportation[24, i] = dr[ds.Tables[0].Columns[24].ColumnName].ToString();
                //Transportation[25, i] = dr[ds.Tables[0].Columns[0].ColumnName].ToString();
                //Transportation[26, i] = "";
                Transportation[25, i] = dr[ds.Tables[0].Columns[25].ColumnName].ToString();
                if (Transportation[25, i].ToString() != null && !string.IsNullOrWhiteSpace(Transportation[25, i].ToString()))
                {
                    netval += Convert.ToDecimal(Transportation[25, i].ToString(), new CultureInfo(culture.Name));
                }
                //Transportation[20, i] = dr[ds.Tables[0].Columns[21].ColumnName].ToString();

                i++;
            }


            ViewBag.Trasportation = Transportation;
            ViewBag.lon = lon;
            ViewBag.netval = netval;
            return PartialView(ds);


        }

        public ActionResult LoadSectorRfn()
        {
            string dateFrom1 = Request["dateFrom"];
            string dateFrom = ConvertDate(dateFrom1);
            string dateTo1 = Request["dateTo"];
            string dateTo = ConvertDate(dateTo1);
            ViewBag.myId = Request["myId"];
            ViewBag.sector = Request["sector"];
            ViewBag.sectTest = Request["sectTest"];
            ViewBag.dateFrom = Request["Bdate"];
            ViewBag.dateTo = Request["Edate"];
            ViewBag.test = Request["test"];
            ViewBag.id = Request["id"];
            ViewBag.test1 = Request["test1"];
            ViewBag.test2 = Request["test2"];
            ViewBag.dateHiddenF = dateFrom1;
            ViewBag.dateHiddenT = dateTo1;
            string sql = "select carrier+format(try_cast(UsageFlightNumber as int) ,'00000') +' - '+UsageOriginCode+UsageDestinationCode as sector " + Environment.NewLine;
            sql += "from pax.SalesDocumentCoupon where  UsageFlightNumber <>'OPEN'  and UsageDate between  '" + dateFrom + "' and '" + dateTo + "'  and   couponstatus = 'R'   " + Environment.NewLine;
            sql += "group by    carrier+format(try_cast(UsageFlightNumber as int) ,'00000') +' - '+UsageOriginCode+UsageDestinationCode, couponstatus      " + Environment.NewLine;
            SqlConnection con = new SqlConnection(pbConnectionString);
            DataSet ds1 = new DataSet();
            SqlDataAdapter ada1 = new SqlDataAdapter(sql, con);
            ada1.Fill(ds1);
            con.Close();
            int lon = ds1.Tables[0].Rows.Count;
            string[] flnum = new string[lon];
            int j = 0;
            foreach (DataRow dr in ds1.Tables[0].Rows)
            {
                flnum[j] = dr[ds1.Tables[0].Columns[0].ColumnName].ToString();
                j = j + 1;
            }
            ViewBag.Flnum = flnum;
            ViewBag.lon = lon;
            return PartialView();
        }
        public ActionResult loadRefundLift()
        {
            string dateFrom1 = Request["dateFrom"];
            string dateFrom = ConvertDate(dateFrom1);
            string dateTo1 = Request["dateTo"];
            string dateTo = ConvertDate(dateTo1);
            string agc = Request["agc"];
            string check = Request["check"];
            string ag = "";
            string fln = "";
            string flnc = Request["fln"];
            if (agc == "-All-" || agc == "")
            {
                ag = "%";
            }
            else
            {
                ag = agc;
            }
            if (flnc == "-All-" || flnc == "")
            {
                fln = "%";
            }
            else
            {
                if (flnc.Length <= 9)
                {
                    fln = flnc + '%';
                }
                else
                {
                    fln = flnc;
                }

            }

            string sql = "";
            sql = sql + " with a as  " + Environment.NewLine;
            sql = sql + " (  " + Environment.NewLine;
            sql = sql + " select  " + Environment.NewLine;
            sql = sql + " sdc.RelatedDocumentNumber as [Document No]   " + Environment.NewLine;
            sql = sql + " , sdc.CouponNumber as [Coupon No]    " + Environment.NewLine;
            sql = sql + " , sdc.UsageOriginCode as [Origin]    " + Environment.NewLine;
            sql = sql + " , sdc.UsageDestinationCode as [Dest]    " + Environment.NewLine;
            sql = sql + " , sdc.UsageFlightNumber as FlightNo    " + Environment.NewLine;
            sql = sql + " , sdc.UsageDate as FlightDate    " + Environment.NewLine;
            sql = sql + " , sdc.UsedClassofService    " + Environment.NewLine;
            sql = sql + " , 'USD' AS Currency  " + Environment.NewLine;
            sql = sql + "   " + Environment.NewLine;
            sql = sql + " , iif( exists( select * from Pax.SalesDocumentPayment sdp where srd.RelatedDocumentGuid = sdp.RelatedDocumentGuid and  sdp.FormOfPaymentType = 'EX' ) , 'Y', '' )   " + Environment.NewLine;
            sql = sql + " as [Exchanged Ticket]  " + Environment.NewLine;
            sql = sql + " , sdc.BillingType  " + Environment.NewLine;
            sql = sql + " , sdh.BspIdentifier as BSP  " + Environment.NewLine;
            sql = sql + " , left(sdh.AgentNumericCode,7) as Agent  " + Environment.NewLine;
            sql = sql + " , iif( sdh.OwnTicket = 'Y', com.Rate, null) as [Agent Remuneration Rate]  " + Environment.NewLine;
            sql = sql + " , sdo.OtherAmountRate as [Applied Sales Remuneration Rate]  " + Environment.NewLine;
            sql = sql + " , cast( isnull( (p.FinalShare  * sdo.OtherAmountRate) / 100, 0 ) as numeric(18,2) ) as [Agent Remuneration Amount]  " + Environment.NewLine;
            sql = sql + " , p.FinalShare AS grossValue  " + Environment.NewLine;
            sql = sql + " , iif( sdc.BillingType is null, 0, iif( p.SpecialProrateAgreement > 0, p.IscPercent, 9 ) ) as ISCPerc  " + Environment.NewLine;
            sql = sql + " , cast( iif( sdc.BillingType is null, 0, iif( p.SpecialProrateAgreement > 0, p.IscPercent, 9 ) ) * 0.01 * p.FinalShare AS decimal(18, 2) ) *  iif( sdc.BillingType = 'O', -1, 1 )  AS [ISC Amount]  " + Environment.NewLine;
            sql = sql + " , 0.00 AS uatpAmount    " + Environment.NewLine;
            sql = sql + " , 0 AS handlingFee  " + Environment.NewLine;
            sql = sql + "   " + Environment.NewLine;
            sql = sql + " ,(SELECT TOP (1) TaxAmount /    " + Environment.NewLine;
            sql = sql + " (SELECT USDRate FROM Ref.CurrencyRate WHERE (Currency = Pax.VW_ApplicableTaxes.TaxCurrency)     " + Environment.NewLine;
            sql = sql + " AND (Period = SUBSTRING(CAST(sdh.DateofIssue AS nvarchar), 1, 4) + SUBSTRING(CAST(sdh.DateofIssue AS nvarchar), 6, 2))) AS Expr1    " + Environment.NewLine;
            sql = sql + " FROM Pax.VW_ApplicableTaxes    " + Environment.NewLine;
            sql = sql + " WHERE (RelatedDocumentNumber = sdc.RelatedDocumentNumber) AND (CouponNumber = sdc.CouponNumber)) AS totalTax  " + Environment.NewLine;
            sql = sql + "   " + Environment.NewLine;
            sql = sql + " ,case  " + Environment.NewLine;
            sql = sql + " when sdc.CouponStatus = 'F' then '1'   " + Environment.NewLine;
            sql = sql + " when sdc.CouponStatus = 'E' then '2'    " + Environment.NewLine;
            sql = sql + " when sdc.CouponStatus = 'R' then '3'   " + Environment.NewLine;
            sql = sql + " else null   " + Environment.NewLine;
            sql = sql + " end AS SourceCode    " + Environment.NewLine;
            sql = sql + "   " + Environment.NewLine;
            sql = sql + " ,sdc.CouponStatus    " + Environment.NewLine;
            sql = sql + " ,sdc.SettlementAuthorizationCode    " + Environment.NewLine;
            sql = sql + " ,OwnTicket    " + Environment.NewLine;
            sql = sql + "   " + Environment.NewLine;
            sql = sql + " from Pax.SalesDocumentHeader SDH  " + Environment.NewLine;
            sql = sql + " join Pax.SalesRelatedDocumentInformation SRD on SDH.HdrGuid = SRD.HdrGuid   " + Environment.NewLine;
            sql = sql + " join Pax.SalesDocumentCoupon SDC on SRD.RelatedDocumentGuid = SDC.RelatedDocumentGuid  " + Environment.NewLine;
            sql = sql + " left join Pax.ProrationDetail AS p ON sdc.RelatedDocumentGuid = p.RelatedDocumentGuid and sdc.CouponNumber = p.CouponNumber and  sdc.CouponStatus = p.ProrationFlag  " + Environment.NewLine;
            sql = sql + "   " + Environment.NewLine;
            sql = sql + " left join Ref.PaxAgencyDetails com  " + Environment.NewLine;
            sql = sql + " on  left(sdh.AgentNumericCode,7) = com.AgencyNumericCode and sdh.DateofIssue between com.PeriodFrom and com.PeriodTo  " + Environment.NewLine;
            sql = sql + " left join Pax.SalesDocumentOtherAmount sdo  " + Environment.NewLine;
            sql = sql + " on sdc.BillingType is null and srd.RelatedDocumentGuid = sdo.RelatedDocumentGuid and sdo.DocumentAmountType like 'Com%' and sdo.OtherAmountCode = 'Effective'  " + Environment.NewLine;
            sql = sql + "   " + Environment.NewLine;
            sql = sql + " WHERE sdc.UsageDate IS NOT NULL  " + Environment.NewLine;
            sql = sql + " and FlightDepartureDate between '" + dateFrom + "'and '" + dateTo + "' " + Environment.NewLine;
            sql = sql + " and CouponStatus='R' and sdh.AgentNumericCode like  '" + ag + "'  " + Environment.NewLine;
            sql = sql + "and carrier+format(try_cast(UsageFlightNumber as int) ,'00000') +' - '+UsageOriginCode+UsageDestinationCode like  '" + fln + "'  " + Environment.NewLine;
            sql = sql + " )  " + Environment.NewLine;
            sql = sql + "   " + Environment.NewLine;
            sql = sql + " select a.* , grossValue - isnull([Agent Remuneration Amount],0) + isnull([ISC Amount],0) - UatpAmount AS NetAmount from a  " + Environment.NewLine;
            if (check == "N")
            {
                sql = sql + " where OwnTicket =  'N' ";
            }
            else if (check == "Y")
            {
                sql = sql + " where OwnTicket =  'Y' ";
            }
            else { }
            SqlConnection con = new SqlConnection(pbConnectionString);
            DataSet ds = new DataSet();
            SqlDataAdapter ada = new SqlDataAdapter(sql, con);
            ada.Fill(ds);
            var culture = System.Globalization.CultureInfo.CurrentCulture;
            int lon = ds.Tables[0].Rows.Count;
            string[,] Transportation = new string[26, lon];
            decimal netval = 0;
            int i = 0;
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                Transportation[0, i] = dr[ds.Tables[0].Columns[0].ColumnName].ToString();
                Transportation[1, i] = dr[ds.Tables[0].Columns[1].ColumnName].ToString();
                Transportation[2, i] = dr[ds.Tables[0].Columns[2].ColumnName].ToString();
                Transportation[3, i] = dr[ds.Tables[0].Columns[3].ColumnName].ToString();
                Transportation[4, i] = dr[ds.Tables[0].Columns[4].ColumnName].ToString();
                string h = dr[ds.Tables[0].Columns[5].ColumnName].ToString();
                if (h != "")
                {
                    Transportation[5, i] = Convert.ToDateTime(h).ToString("dd-MMM- yyyy");
                }
                else
                {
                    Transportation[5, i] = h;
                }
                Transportation[6, i] = dr[ds.Tables[0].Columns[6].ColumnName].ToString();
                Transportation[7, i] = dr[ds.Tables[0].Columns[7].ColumnName].ToString();
                Transportation[8, i] = dr[ds.Tables[0].Columns[8].ColumnName].ToString();
                Transportation[9, i] = dr[ds.Tables[0].Columns[9].ColumnName].ToString();
                Transportation[10, i] = dr[ds.Tables[0].Columns[10].ColumnName].ToString();
                Transportation[11, i] = dr[ds.Tables[0].Columns[11].ColumnName].ToString();
                Transportation[12, i] = dr[ds.Tables[0].Columns[12].ColumnName].ToString();
                Transportation[13, i] = dr[ds.Tables[0].Columns[13].ColumnName].ToString();
                Transportation[14, i] = dr[ds.Tables[0].Columns[14].ColumnName].ToString();
                Transportation[15, i] = dr[ds.Tables[0].Columns[15].ColumnName].ToString();
                Transportation[16, i] = dr[ds.Tables[0].Columns[16].ColumnName].ToString();
                Transportation[17, i] = dr[ds.Tables[0].Columns[17].ColumnName].ToString();
                Transportation[18, i] = dr[ds.Tables[0].Columns[18].ColumnName].ToString();
                Transportation[19, i] = dr[ds.Tables[0].Columns[19].ColumnName].ToString();
                Transportation[20, i] = dr[ds.Tables[0].Columns[20].ColumnName].ToString();
                Transportation[21, i] = dr[ds.Tables[0].Columns[21].ColumnName].ToString();
                Transportation[22, i] = dr[ds.Tables[0].Columns[22].ColumnName].ToString();
                Transportation[23, i] = dr[ds.Tables[0].Columns[23].ColumnName].ToString();
                Transportation[24, i] = dr[ds.Tables[0].Columns[24].ColumnName].ToString();
                //Transportation[25, i] = dr[ds.Tables[0].Columns[0].ColumnName].ToString();
                //Transportation[26, i] = "";
                Transportation[25, i] = dr[ds.Tables[0].Columns[25].ColumnName].ToString();
                if (Transportation[25, i].ToString() != null && !string.IsNullOrWhiteSpace(Transportation[25, i].ToString()))
                {
                    netval += Convert.ToDecimal(Transportation[25, i].ToString(), new CultureInfo(culture.Name));
                }
                //Transportation[20, i] = dr[ds.Tables[0].Columns[21].ColumnName].ToString();

                i++;
            }


            ViewBag.Trasportation = Transportation;
            ViewBag.lon = lon;
            ViewBag.netval = netval;
            return PartialView(ds);
        }

        public ActionResult getFlightNo()
        {
            string dateFrom1 = Request["dateFrom"];
            string dateFrom = ConvertDate(dateFrom1);
            string dateTo1 = Request["dateTo"];
            string dateTo = ConvertDate(dateTo1);
            string sql = "select carrier+format(try_cast(UsageFlightNumber as int) ,'00000') +' - '+UsageOriginCode+UsageDestinationCode as sector " + Environment.NewLine;
            sql += "from pax.SalesDocumentCoupon where  UsageFlightNumber <>'OPEN'  and UsageDate between  '" + dateFrom + "' and '" + dateTo + "'  and   couponstatus = 'F'   " + Environment.NewLine;
            sql += "group by    carrier+format(try_cast(UsageFlightNumber as int) ,'00000') +' - '+UsageOriginCode+UsageDestinationCode, couponstatus      " + Environment.NewLine;
            SqlConnection con = new SqlConnection(pbConnectionString);
            DataSet ds1 = new DataSet();
            SqlDataAdapter ada1 = new SqlDataAdapter(sql, con);
            ada1.Fill(ds1);
            con.Close();
            int lon = ds1.Tables[0].Rows.Count;
            string[] flnum = new string[lon];
            int j = 0;
            foreach (DataRow dr in ds1.Tables[0].Rows)
            {
                flnum[j] = dr[ds1.Tables[0].Columns[0].ColumnName].ToString();
                j = j + 1;
            }
            ViewBag.Flnum = flnum;
            ViewBag.lon = lon;
            return PartialView();
        }

        public ActionResult LoadSector()
        {
            string dateFrom1 = Request["dateFrom"];
            string dateFrom = ConvertDate(dateFrom1);
            string dateTo1 = Request["dateTo"];
            string dateTo = ConvertDate(dateTo1);
            ViewBag.myId = Request["myId"];
            ViewBag.sector = Request["sector"];
            ViewBag.sectTest = Request["sectTest"];
            ViewBag.dateFrom = Request["Bdate"];
            ViewBag.dateTo = Request["Edate"];
            ViewBag.test = Request["test"];
            ViewBag.id = Request["id"];
            ViewBag.test1 = Request["test1"];
            ViewBag.test2 = Request["test2"];
            ViewBag.dateHiddenF = dateFrom1;
            ViewBag.dateHiddenT = dateTo1;
            string sql = "select carrier+format(try_cast(UsageFlightNumber as int) ,'00000') +' - '+UsageOriginCode+UsageDestinationCode as sector " + Environment.NewLine;
            sql += "from pax.SalesDocumentCoupon where  UsageFlightNumber <>'OPEN'  and UsageDate between  '" + dateFrom + "' and '" + dateTo + "'  and   couponstatus = 'F'   " + Environment.NewLine;
            sql += "group by    carrier+format(try_cast(UsageFlightNumber as int) ,'00000') +' - '+UsageOriginCode+UsageDestinationCode, couponstatus      " + Environment.NewLine;
            SqlConnection con = new SqlConnection(pbConnectionString);
            DataSet ds1 = new DataSet();
            SqlDataAdapter ada1 = new SqlDataAdapter(sql, con);
            ada1.Fill(ds1);
            con.Close();
            int lon = ds1.Tables[0].Rows.Count;
            string[] flnum = new string[lon];
            int j = 0;
            foreach (DataRow dr in ds1.Tables[0].Rows)
            {
                flnum[j] = dr[ds1.Tables[0].Columns[0].ColumnName].ToString();
                j = j + 1;
            }
            ViewBag.Flnum = flnum;
            ViewBag.lon = lon;
            return PartialView();
        }

        public ActionResult LoadSectorOwn()
        {
            string dateFrom1 = Request["dateFrom"];
            string dateFrom = ConvertDate(dateFrom1);
            string dateTo1 = Request["dateTo"];
            string dateTo = ConvertDate(dateTo1);
            ViewBag.myId = Request["myId"];
            ViewBag.sector = Request["sector"];
            ViewBag.sectTest = Request["sectTest"];
            ViewBag.dateFrom = Request["Bdate"];
            ViewBag.dateTo = Request["Edate"];
            ViewBag.test = Request["test"];
            ViewBag.id = Request["id"];
            ViewBag.test1 = Request["test1"];
            ViewBag.test2 = Request["test2"];
            ViewBag.dateHiddenF = dateFrom1;
            ViewBag.dateHiddenT = dateTo1;
            string sql = "select carrier+format(try_cast(UsageFlightNumber as int) ,'00000') +' - '+UsageOriginCode+UsageDestinationCode as sector " + Environment.NewLine;
            sql += "from pax.SalesDocumentCoupon where  UsageFlightNumber <>'OPEN'  and UsageDate between  '" + dateFrom + "' and '" + dateTo + "'  and   couponstatus = 'F'   " + Environment.NewLine;
            sql += "group by    carrier+format(try_cast(UsageFlightNumber as int) ,'00000') +' - '+UsageOriginCode+UsageDestinationCode, couponstatus      " + Environment.NewLine;
            SqlConnection con = new SqlConnection(pbConnectionString);
            DataSet ds1 = new DataSet();
            SqlDataAdapter ada1 = new SqlDataAdapter(sql, con);
            ada1.Fill(ds1);
            con.Close();
            int lon = ds1.Tables[0].Rows.Count;
            string[] flnum = new string[lon];
            int j = 0;
            foreach (DataRow dr in ds1.Tables[0].Rows)
            {
                flnum[j] = dr[ds1.Tables[0].Columns[0].ColumnName].ToString();
                j = j + 1;
            }
            ViewBag.Flnum = flnum;
            ViewBag.lon = lon;
            return PartialView();
        }
        public ActionResult loadAnalysis()
        {
            string dateFrom1 = Request["dateFrom"];
            string dateFrom = ConvertDate(dateFrom1);
            string dateTo1 = Request["dateTo"];
            string dateTo = ConvertDate(dateTo1);
            string flnc = Request["fln"];
            string check = Request["check"];
            string fln = "";
            if (flnc == "-All-" || flnc == "")
            {
                fln = "%";
            }
            else
            {
                fln = flnc;
            }

            string sql = "with Base as (select 'TRANSPORTATION' as REVENUETYPE, sdc.RelatedDocumentNumber,sdc.CouponNumber as CPN,FareBasisTicketDesignator  " + Environment.NewLine;
            sql += ",FORMAT(try_cast(UsageFlightNumber as int),'00000') as FightNumber  " + Environment.NewLine;
            sql += ",format(try_cast(UsageDate as date),'dd-MMM-yyyy') as FLTDATE " + Environment.NewLine;
            sql += ",UsageOriginCode as SECTORFROM,UsageDestinationCode as SECTORTO   " + Environment.NewLine;
            sql += ",iif(SpecialProrateAgreement = '0.00',finalshare,SpecialProrateAgreement) as FinalShare ,BILLINGPERIOD,INVOICENO,OwnTicket " + Environment.NewLine;
            sql += "from " + Environment.NewLine;
            sql += "Pax.SalesDocumentHeader SDH   " + Environment.NewLine;
            sql += "join Pax.SalesRelatedDocumentInformation SRD on SDH.HdrGuid = SRD.HdrGuid    " + Environment.NewLine;
            sql += "join Pax.SalesDocumentCoupon SDC on SRD.RelatedDocumentGuid = SDC.RelatedDocumentGuid  " + Environment.NewLine;


            sql += "left join pax.ProrationDetail PR on sdc.RelatedDocumentGuid = pr.RelatedDocumentGuid and PR.ProrationFlag = 'F'  " + Environment.NewLine;
            sql += "and pr.CouponNumber = sdc.CouponNumber   " + Environment.NewLine;

            sql += "left join pax.OutwardBilling ob on left (sdc.RelatedDocumentNumber,3) = ob.ALC  and right (sdc.RelatedDocumentNumber,10) = ob.DOC and sdc.CouponNumber = ob.CPN " + Environment.NewLine;
            sql += "where carrier+format(try_cast(UsageFlightNumber as int) ,'00000') +' - '+UsageOriginCode+UsageDestinationCode like  '" + fln + "'  " + Environment.NewLine;
            sql += "and UsageDate  between '" + dateFrom + "'and '" + dateTo + "'   " + Environment.NewLine;
            sql += "and CouponStatus ='F'    " + Environment.NewLine;
            sql += "UNION    " + Environment.NewLine;
            sql += "select 'FIM' as REVENUETYPE, CONCAT(ALC,DOC) as RelatedDocumentNumber,CPN,null as Farebasis " + Environment.NewLine;
            sql += ",FORMAT(try_cast(FLIGHT as int),'00000')  as FightNumber " + Environment.NewLine;
            sql += ",FLTDATE,SECTORFROM,SECTORTO ,AMOUNT as FinalShare   ,BILLINGPERIOD,INVOICENO,null as OwnTicket " + Environment.NewLine;
            sql += "from Pax.OutwardBilling where PMC = '14' and cast(cast(FLIGHT as int) as nvarchar  (5))+' - '+SECTORFROM+SECTORTO like  '" + fln + "'   " + Environment.NewLine;
            sql += "and FLTDATE  between '" + dateFrom + "'and '" + dateTo + "'  " + Environment.NewLine;
            sql += "UNION    " + Environment.NewLine;
            sql += "select 'EXCESS BAGGAGE' as REVENUETYPE, CONCAT(ALC,DOC) as RelatedDocumentNumber,CPN,null as Farebasis " + Environment.NewLine;
            sql += ",FORMAT(try_cast(FLIGHT as int),'00000') as FightNumber " + Environment.NewLine;
            sql += ",FLTDATE,SECTORFROM,SECTORTO ,AMOUNT as FinalShare   ,BILLINGPERIOD,INVOICENO,null as OwnTicket " + Environment.NewLine;
            sql += "from Pax.OutwardBilling where PMC = '25'  and cast(cast(FLIGHT as int) as nvarchar  (5))+' - '+SECTORFROM+SECTORTO like  '" + fln + "'   " + Environment.NewLine;
            sql += "and FLTDATE  between '" + dateFrom + "'and '" + dateTo + "' ) " + Environment.NewLine;
            if (check == "N")
            {
                sql = sql + "select REVENUETYPE as [Revenue Type], RelatedDocumentNumber as DocumentNo, CPN as Cpn,FareBasisTicketDesignator as [Fare-Basis]";
                sql = sql + ",FightNumber as [Flight No],FLTDATE as [Flight Date], SECTORFROM+SECTORTO as Sector,FinalShare as [Final Share],BILLINGPERIOD as [Billing Period], INVOICENO as [Invoice No], OwnTicket  from Base  where OwnTicket =  'N' ";
            }
            else if (check == "Y")
            {
                
                sql = sql + "select REVENUETYPE as [Revenue Type], RelatedDocumentNumber as DocumentNo, CPN as Cpn,FareBasisTicketDesignator as [Fare-Basis]";
                sql = sql + ",FightNumber as [Flight No],FLTDATE as [Flight Date], SECTORFROM+SECTORTO as Sector,FinalShare as [Final Share],BILLINGPERIOD as [Billing Period], INVOICENO as [Invoice No], OwnTicket  from Base  where OwnTicket = 'Y' ";
            }
            else {
                sql = sql + "select REVENUETYPE as [Revenue Type],RelatedDocumentNumber as DocumentNo, CPN as Cpn,FareBasisTicketDesignator as [Fare-Basis]";
                sql = sql + ",FightNumber as [Flight No],FLTDATE as [Flight Date], SECTORFROM+SECTORTO as Sector,FinalShare as [Final Share],BILLINGPERIOD as [Billing Period], INVOICENO as [Invoice No], OwnTicket  from Base ";
            }
            SqlConnection con = new SqlConnection(pbConnectionString);
            DataSet ds = new DataSet();
            SqlDataAdapter ada = new SqlDataAdapter(sql, con);
            ada.Fill(ds);
            var culture = System.Globalization.CultureInfo.CurrentCulture;
            int lon = ds.Tables[0].Rows.Count;
            string[,] Analysis = new string[11, lon];
            decimal netval = 0;
            int i = 0;
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                Analysis[0, i] = dr[ds.Tables[0].Columns[0].ColumnName].ToString();
                Analysis[1, i] = dr[ds.Tables[0].Columns[1].ColumnName].ToString();
                Analysis[2, i] = dr[ds.Tables[0].Columns[2].ColumnName].ToString();
                Analysis[3, i] = dr[ds.Tables[0].Columns[3].ColumnName].ToString();
                Analysis[4, i] = dr[ds.Tables[0].Columns[4].ColumnName].ToString();
                Analysis[5, i] = dr[ds.Tables[0].Columns[5].ColumnName].ToString();
                Analysis[6, i] = dr[ds.Tables[0].Columns[6].ColumnName].ToString();
                Analysis[7, i] = dr[ds.Tables[0].Columns[7].ColumnName].ToString();
                Analysis[8, i] = dr[ds.Tables[0].Columns[8].ColumnName].ToString();
                if (Analysis[7, i].ToString() != null && !string.IsNullOrWhiteSpace(Analysis[7, i].ToString()))
                {
                    netval += Convert.ToDecimal(Analysis[7, i].ToString(), new CultureInfo(culture.Name));
                }
                Analysis[9, i] = dr[ds.Tables[0].Columns[9].ColumnName].ToString();
                Analysis[10, i] = dr[ds.Tables[0].Columns[10].ColumnName].ToString();
                i++;
            }
            string sql1 = "with Base as (select 'TRANSPORTATION' as REVENUETYPE, sdc.RelatedDocumentNumber,sdc.CouponNumber as CPN,FareBasisTicketDesignator  " + Environment.NewLine;
            sql1 += ",FORMAT(try_cast(UsageFlightNumber as int),'00000') as FightNumber  " + Environment.NewLine;
            sql1 += ",format(try_cast(UsageDate as date),'dd-MMM-yyyy') as FLTDATE " + Environment.NewLine;
            sql1 += ",UsageOriginCode as SECTORFROM,UsageDestinationCode as SECTORTO   " + Environment.NewLine;
            sql1 += ",iif(SpecialProrateAgreement = '0.00',finalshare,SpecialProrateAgreement) as FinalShare ,BILLINGPERIOD,INVOICENO,OwnTicket " + Environment.NewLine;
            sql1 += "from " + Environment.NewLine;
            sql1 += "Pax.SalesDocumentHeader SDH   " + Environment.NewLine;
            sql1 += "join Pax.SalesRelatedDocumentInformation SRD on SDH.HdrGuid = SRD.HdrGuid    " + Environment.NewLine;
            sql1 += "join Pax.SalesDocumentCoupon SDC on SRD.RelatedDocumentGuid = SDC.RelatedDocumentGuid  " + Environment.NewLine;


            sql1 += "left join pax.ProrationDetail PR on sdc.RelatedDocumentGuid = pr.RelatedDocumentGuid and PR.ProrationFlag = 'F'  " + Environment.NewLine;
            sql1 += "and pr.CouponNumber = sdc.CouponNumber   " + Environment.NewLine;

            sql1 += "left join pax.OutwardBilling ob on left (sdc.RelatedDocumentNumber,3) = ob.ALC  and right (sdc.RelatedDocumentNumber,10) = ob.DOC and sdc.CouponNumber = ob.CPN " + Environment.NewLine;
            sql1 += "where carrier+format(try_cast(UsageFlightNumber as int) ,'00000') +' - '+UsageOriginCode+UsageDestinationCode like  '" + fln + "'  " + Environment.NewLine;
            sql1 += "and UsageDate  between '" + dateFrom + "'and '" + dateTo + "'   " + Environment.NewLine;
            sql1 += "and CouponStatus ='F'    " + Environment.NewLine;
            sql1 += "UNION    " + Environment.NewLine;
            sql1 += "select 'FIM' as REVENUETYPE, CONCAT(ALC,DOC) as RelatedDocumentNumber,CPN,null as Farebasis " + Environment.NewLine;
            sql1 += ",FORMAT(try_cast(FLIGHT as int),'00000')  as FightNumber " + Environment.NewLine;
            sql1 += ",FLTDATE,SECTORFROM,SECTORTO ,AMOUNT as FinalShare   ,BILLINGPERIOD,INVOICENO,null as OwnTicket " + Environment.NewLine;
            sql1 += "from Pax.OutwardBilling where PMC = '14' and cast(cast(FLIGHT as int) as nvarchar  (5))+' - '+SECTORFROM+SECTORTO like  '" + fln + "'   " + Environment.NewLine;
            sql1 += "and FLTDATE  between '" + dateFrom + "'and '" + dateTo + "'  " + Environment.NewLine;
            sql1 += "UNION    " + Environment.NewLine;
            sql1 += "select 'EXCESS BAGGAGE' as REVENUETYPE, CONCAT(ALC,DOC) as RelatedDocumentNumber,CPN,null as Farebasis " + Environment.NewLine;
            sql1 += ",FORMAT(try_cast(FLIGHT as int),'00000') as FightNumber " + Environment.NewLine;
            sql1 += ",FLTDATE,SECTORFROM,SECTORTO ,AMOUNT as FinalShare   ,BILLINGPERIOD,INVOICENO,null as OwnTicket " + Environment.NewLine;
            sql1 += "from Pax.OutwardBilling where PMC = '25'  and cast(cast(FLIGHT as int) as nvarchar  (5))+' - '+SECTORFROM+SECTORTO like  '" + fln + "'   " + Environment.NewLine;
            sql1 += "and FLTDATE  between '" + dateFrom + "'and '" + dateTo + "' ) " + Environment.NewLine;
            sql1 += "select left(RelatedDocumentNumber,3) as Alc,count(*) as [Counts],SECTORFROM+SECTORTO as Sector,sum(FinalShare) as [Final Share],BILLINGPERIOD as [Billing Period],INVOICENO as [Invoice No] from Base ";
            if (check == "N")
            {
                sql1 = sql1 + " where OwnTicket =  'N' ";
            }
            else if (check == "Y")
            {
                sql1 = sql1 + " where OwnTicket =  'Y' ";
            }
            else { }
            sql1 += "group by left(RelatedDocumentNumber,3) ,SECTORFROM+SECTORTO ,BILLINGPERIOD,INVOICENO ";
            DataSet ds1 = new DataSet();
            SqlDataAdapter ada1 = new SqlDataAdapter(sql1, con);
            ada1.Fill(ds1);
            int lon1 = ds.Tables[0].Rows.Count;
            string[,] sumAnalysis = new string[6, lon1];
            int j = 0;
            foreach (DataRow dr1 in ds1.Tables[0].Rows)
            {
                sumAnalysis[0,j] = dr1[ds1.Tables[0].Columns[0].ColumnName].ToString();
                sumAnalysis[1, j] = dr1[ds1.Tables[0].Columns[1].ColumnName].ToString();
                sumAnalysis[2, j] = dr1[ds1.Tables[0].Columns[2].ColumnName].ToString();
                sumAnalysis[3, j] = dr1[ds1.Tables[0].Columns[3].ColumnName].ToString();
                sumAnalysis[4, j] = dr1[ds1.Tables[0].Columns[4].ColumnName].ToString();
                sumAnalysis[5, j] = dr1[ds1.Tables[0].Columns[5].ColumnName].ToString();
                j++;
            }
            ViewBag.Sum = sumAnalysis;
            ViewBag.lon1 = lon1;
            ViewBag.Analysis = Analysis;
            ViewBag.lon = lon;
            ViewBag.netval = netval;
            return PartialView();
        }
        /***************************************************end PassengerLift*****************************************/

        public ActionResult LiftForTransportOwn(String type)
        {
            ViewBag.dateFrom = DateTime.Now.AddMonths(-1).ToString("dd-MMM-yyyy");
            ViewBag.dateTo = DateTime.Now.ToString("dd-MMM-yyyy");
            ViewBag.typeId = type;
            ViewBag.param = "own";
            return PartialView();
        }

        public ActionResult LiftForTransportOAL(String type)
        {
            ViewBag.dateFrom = DateTime.Now.AddMonths(-1).ToString("dd-MMM-yyyy");
            ViewBag.dateTo = DateTime.Now.ToString("dd-MMM-yyyy");
            ViewBag.typeId = type;
            ViewBag.param = "oal";
            return PartialView("LiftForTransportOwn");
        }
        /*******************Sales Matching********************/
        public ActionResult SalesMatching()
        {
            ViewBag.dateFrom = DateTime.Now.AddMonths(-1).ToString("dd-MMM-yyyy");
            ViewBag.dateTo = DateTime.Now.ToString("dd-MMM-yyyy");
            return PartialView();
        }
        public ActionResult LoadSalesMatching()
        {
            
            string dateFrom = Request["dateFrom"];
            //string dateFrom = ConvertDate(dateFrom1);
            string dateTo = Request["dateTo"];
            //string dateTo = ConvertDate(dateTo1);
            SqlConnection con = new SqlConnection(pbConnectionString);
            SqlCommand cmd = new SqlCommand("[Pax].[SP_SalesMatch]", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 1000;
            cmd.Parameters.AddWithValue("@dateFrom", dateFrom);
            cmd.Parameters.AddWithValue("@dateTo", dateTo);

            DataSet ds = new DataSet();
            SqlDataAdapter ada = new SqlDataAdapter(cmd);
            ada.Fill(ds);
            con.Close();

            int lon = ds.Tables[0].Rows.Count;
            string[,] dgTransport = new string[11, lon];
            decimal netVal = 0;

            int j = 0;
            foreach (DataRow dr1 in ds.Tables[0].Rows)
            {
                for (int i = 0; i < 11; i++)
                {
                    dgTransport[i, j] = dr1[ds.Tables[0].Columns[i].ColumnName].ToString();

                }
                j++;
            }

            for (int id = 0; id < lon; id++)
            {
                if (dgTransport[10, id] != null && !string.IsNullOrWhiteSpace(dgTransport[10, id].ToString()))
                    netVal += Convert.ToDecimal(dgTransport[10, id]);
            }
            ViewBag.txtAmt = netVal.ToString("#,###,###,###.#0"); 

            return PartialView(ds);
        }

        /*******************end********************/
        /*******************FlownWithoutSales********************/
        public ActionResult FlownWithoutSales()
        {
            ViewBag.dateFrom = DateTime.Now.AddMonths(-1).ToString("dd-MMM-yyyy");
            ViewBag.dateTo = DateTime.Now.ToString("dd-MMM-yyyy");
            return PartialView();
        }
        /*******************end********************/
        /*******************LoadFlownWithoutSales********************/
        public ActionResult LoadFlownWithoutSales()
        {

            string dateFrom1 = Request["dateFrom"];
            string dateFrom = ConvertDate(dateFrom1);
            string dateTo1 = Request["dateTo"];
            string dateTo = ConvertDate(dateTo1);
            string cboAgt = Request["agNo"];
            string rdYes = Request["chkOAL"];
            string rdNo = Request["chkOWN"];
            string sql0 = "";

            if ((cboAgt != "-All-") && (cboAgt != ""))
            {
                sql0 = "and AgentNumericCode = '" + cboAgt + "'";
            }

            string sql1 = "";

            if (rdYes == "oui")
            {
                sql1 = "and SalesDataAvailable = '1'";
            }
            else
                if (rdNo == "non")
            {
                sql1 = "and SalesDataAvailable = '0'";
            }
            SqlConnection con = new SqlConnection(pbConnectionString);
            string sql = "select  format([SaleDate], 'dd-MMM-yyyy') as SaleDate , DocumentNumber, CouponNumber ,AgentNumericCode,isnull( agtown.TradingName, agt.TradingName ) as [Agent Name]" + Environment.NewLine;
            sql = sql + " ,FlightNumber ,format([FlightDepartureDate ], 'dd-MMM-yyyy') as FlightDepartureDate ,Carrier ,OriginAirportCityCode ,DestinationAirportCityCode , ReservationBookingDesignator" + Environment.NewLine;
            sql = sql + " ,format([UsageDate], 'dd-MMM-yyyy') as UsageDate  ,UsageCarrier ,UsageOriginCode ,UsageDestinationCode ,UsedClassofService" + Environment.NewLine;
            sql = sql + ",'USD' as Currency	 ,FinalShare ,PassengerName" + Environment.NewLine;

            sql = sql + "from pax.fn_SalesFlownMatching() sdh" + Environment.NewLine;
            sql = sql + "left join Ref.Agent_Own agtown	on left(sdh.AgentNumericCode,7) = agtown.AgencyNumericCode" + Environment.NewLine;
            sql = sql + "left join Ref.Agent	agt	on left(sdh.AgentNumericCode,7) = agt.AgencyNumericCode" + Environment.NewLine;

            sql = sql + "where SaleDate between '" + dateFrom + "' and '" + dateTo + "'" + Environment.NewLine;

            sql = sql + sql0 + Environment.NewLine;
            sql = sql + sql1 + Environment.NewLine;
            sql = sql + "order by SaleDate,DocumentNumber, AgentNumericCode,PassengerName";

            DataSet ds = new DataSet();
            SqlDataAdapter ada = new SqlDataAdapter(sql, con);
            ada.Fill(ds);
            con.Close();

            int lon = ds.Tables[0].Rows.Count;
            string[,] dbgSalesFlown = new string[18, lon];

            int j = 0;
            foreach (DataRow dr1 in ds.Tables[0].Rows)
            {
                for (int i = 0; i < 18; i++)
                {
                    dbgSalesFlown[i, j] = dr1[ds.Tables[0].Columns[i].ColumnName].ToString();

                }
                j++;
            }

            string[] cll = new string[lon];

            for (int g = 0; g < lon; g++)
            {
                cll[g] = dbgSalesFlown[1, g].ToString();
            }

            string[] b = cll.Distinct().ToArray();

            ViewBag.txtCount = lon;

            double sum = 0;


            for (int a = 0; a < lon; a++)
            {
                if (dbgSalesFlown[17, a] != null && !string.IsNullOrWhiteSpace(dbgSalesFlown[17, a].ToString()))
                    sum += Convert.ToDouble(dbgSalesFlown[17, a]);


            }
            if (lon == 0)
            {
                ViewBag.mes = "No data available for the selected criteria.";
            }
            ViewBag.txtAmt = sum.ToString("0.00");

            return PartialView(ds);
        }
        public ActionResult Cherche3ByCode()
        {
            string value = Request["value"];
            string sql = "";
            int n;

            bool isNumeric = int.TryParse(value, out n);
            if (isNumeric)
            {

                sql = "select distinct AgentNumericCode, isnull( agtown.TradingName, agt.TradingName ) as [Agent Name]" + Environment.NewLine;
                sql += "from pax.fn_SalesFlownMatching() sdh" + Environment.NewLine;
                sql += "left join Ref.Agent_Own agtown	on left(sdh.AgentNumericCode,7) = agtown.AgencyNumericCode" + Environment.NewLine;
                sql += "left join Ref.Agent	agt	on left(sdh.AgentNumericCode,7) = agt.AgencyNumericCode" + Environment.NewLine;
                // sql += " where FBS_Status = 'Y'" + Environment.NewLine;

                sql += " and sdh.AgentNumericCode like " + "'" + value + "'";
            }
            else
            {
                sql = "select distinct AgentNumericCode, isnull( agtown.TradingName, agt.TradingName ) as [Agent Name]" + Environment.NewLine;
                sql += "from pax.fn_SalesFlownMatching() sdh" + Environment.NewLine;
                sql += "left join Ref.Agent_Own agtown	on left(sdh.AgentNumericCode,7) = agtown.AgencyNumericCode" + Environment.NewLine;
                sql += "left join Ref.Agent	agt	on left(sdh.AgentNumericCode,7) = agt.AgencyNumericCode" + Environment.NewLine;
                //sql += " where FBS_Status = 'Y'" + Environment.NewLine;

                sql += " and isnull( agtown.TradingName, agt.TradingName )" + "'" + value + "'";
            }

            SqlConnection con = new SqlConnection(pbConnectionString);
            DataSet ds = new DataSet();
            SqlDataAdapter ada = new SqlDataAdapter(sql, con);
            ada.Fill(ds);

            con.Close();

            return PartialView(ds);


        }

        /*******************end********************/
        /*******************Surcharges********************/
        public ActionResult Surcharges()
        {
            ViewBag.dateFrom = DateTime.Now.AddMonths(-1).ToString("dd-MMM-yyyy");
            ViewBag.dateTo = DateTime.Now.ToString("dd-MMM-yyyy");
            return PartialView();
        }
        public ActionResult LoadSurcharges()
        {
            var culture = System.Globalization.CultureInfo.CurrentCulture;
            string dateFrom1 = Request["dateFrom"];
            string dateFrom = ConvertDate(dateFrom1);
            string dateTo1 = Request["dateTo"];
            string dateTo = ConvertDate(dateTo1);
            string cboAgent = Request["cbAgt"];
            string cboSur = Request["cbS"];
            string cboFlightNo = Request["cbF"];
            string rabALL = Request["chkYes"];
            string rabOwn = Request["chkOWN"];
            string rabOAL = Request["chkOAL"];
            decimal? totFare = 0;
            decimal? totVat = 0;
            string ag = "";
            string fl = "";
            string tc = "";
            if (cboAgent == "-All-")
            {
                ag = "%";
            }
            else
            {
                ag = cboAgent;
            }

            if (cboSur == "-All-")
            {
                tc = "%";
            }
            else
            {
                tc = cboSur;
            }

            if (cboFlightNo == "-All-")
            {
                fl = "%";
            }
            else
            {
                fl = cboFlightNo;
            }


            string ar = "";

            if (rabALL == "yes")
            {
                ar = "%";
            }
            else
                if (rabOwn == "non")
            {
                ar = "Y";
            }
            else
                    if (rabOAL == "oui")
            {
                ar = "N";
            }
            SqlConnection con = new SqlConnection(pbConnectionString);
            SqlCommand cmd = new SqlCommand("[Pax].[Flown_Surcharges]", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 300;
            cmd.Parameters.AddWithValue("@dateFrom", dateFrom);
            cmd.Parameters.AddWithValue("@dateTo", dateTo);
            cmd.Parameters.AddWithValue("@airlines", ar);
            cmd.Parameters.AddWithValue("@flight", fl);
            cmd.Parameters.AddWithValue("@AgentNumCode", ag);
            cmd.Parameters.AddWithValue("@OtherAmtCode", tc);

            DataSet ds = new DataSet();
            SqlDataAdapter ada = new SqlDataAdapter(cmd);
            ada.Fill(ds);
            con.Close();

            int lon = ds.Tables[0].Rows.Count;
            if (lon == 0)
            {
                ViewBag.mes = "No data available for the selected criteria.";
                ViewBag.txtTotalVAT = totVat.Value.ToString(".00");
                ViewBag.txtTotalFare = totFare.Value.ToString(".00");
            }

            ViewBag.txtTotalDocNum = lon;

            return PartialView(ds);
        }

        /*******************end********************/
        /*******************Surcharges********************/
        public ActionResult VAT()
        {
            ViewBag.dateFrom = DateTime.Now.AddMonths(-1).ToString("dd-MMM-yyyy");
            ViewBag.dateTo = DateTime.Now.ToString("dd-MMM-yyyy");
            return PartialView();
        }
        public ActionResult LoadVAT()
        {
            var culture = System.Globalization.CultureInfo.CurrentCulture;
            string dateFrom1 = Request["dateFrom"];
            string dateFrom = ConvertDate(dateFrom1);
            string dateTo1 = Request["dateTo"];
            string dateTo = ConvertDate(dateTo1);
            string cboAgent = Request["cbAgt"];

            string cboFlightNo = Request["cbF"];
            string rabALL = Request["chkYes"];
            string rabOwn = Request["chkOWN"];
            string rabOAL = Request["chkOAL"];

            string ef = "%";
            string ag = "";
            string fl = "";

            DateTime? testDate = Convert.ToDateTime("1800-01-01");

            decimal? totFare = 0;
            decimal? totVat = 0;


            if (cboAgent == "-All-")
            {
                ag = "%";
            }
            else
            {
                ag = cboAgent;
            }


            if (cboFlightNo == "-All-")
            {
                fl = "%";
            }
            else
            {
                fl = cboFlightNo;
            }


            string ar = "";

            if (rabALL == "yes")
            {
                ar = "%";
            }
            else
                if (rabOwn == "non")
            {
                ar = "Y";
            }
            else
                    if (rabOAL == "oui")
            {
                ar = "N";
            }
            SqlConnection con = new SqlConnection(pbConnectionString);
            SqlCommand cmd = new SqlCommand("[Pax].[Flown_Surcharges]", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 300;
            cmd.Parameters.AddWithValue("@dateFrom", dateFrom);
            cmd.Parameters.AddWithValue("@dateTo", dateTo);
            cmd.Parameters.AddWithValue("@airlines", ar);
            cmd.Parameters.AddWithValue("@flight", fl);
            cmd.Parameters.AddWithValue("@AgentNumCode", ag);
            cmd.Parameters.AddWithValue("@OtherAmtCode", ef);

            DataSet ds = new DataSet();
            SqlDataAdapter ada = new SqlDataAdapter(cmd);
            ada.Fill(ds);
            con.Close();

            int lon = ds.Tables[0].Rows.Count;

            if (lon == 0)
            {
                ViewBag.mes = "No data available for the selected criteria.";
                ViewBag.txtTotalVAT = totVat.Value.ToString(".00");
                ViewBag.txtTotalFare = totFare.Value.ToString(".00");
            }

            ViewBag.txtTotalDocNum = lon;

            return PartialView(ds);
        }
        public ActionResult LoadAgtVAT()
        {

            string dateFrom = Request["dateFrom"];
            //string dateFrom = ConvertDate(dateFrom1);
            string dateTo = Request["dateTo"];
            //string dateTo = ConvertDate(dateTo1);
            string sql = "";
            sql = sql + "Select Distinct AgentNumericCode FROM PAX.VW_LiftForTransportation where  UsageDate between convert(datetime,'" + dateFrom + "',105)  and convert(datetime,'" + dateTo + "',105) and UsageFlightNumber like '%' and DocType like '%'";

            SqlConnection con1 = new SqlConnection(pbConnectionString);
            DataSet ds1 = new DataSet();
            SqlDataAdapter ada1 = new SqlDataAdapter(sql, con1);
            ada1.Fill(ds1);
            con1.Close();
            ViewBag.form2 = ds1;
            return PartialView();
        }
        public ActionResult LoadcbFVAT()
        {

            string dateFrom1 = Request["dateFrom"];
            string dateFrom = ConvertDate(dateFrom1);
            string dateTo1 = Request["dateTo"];
            string dateTo = ConvertDate(dateTo1);
            string sql = "";
            sql = sql + "Select Distinct UsageFlightNumber FROM PAX.VW_LiftForTransportation where  UsageDate between convert(datetime,'" + dateFrom + "',105)  and convert(datetime,'" + dateTo + "',105) and AgentNumericCode like '%' and DocType like '%'";

            SqlConnection con1 = new SqlConnection(pbConnectionString);
            DataSet ds1 = new DataSet();
            SqlDataAdapter ada1 = new SqlDataAdapter(sql, con1);
            ada1.Fill(ds1);
            con1.Close();
            ViewBag.form2 = ds1;
            return PartialView();
        }
        /*******************end********************/
        /*******************SLS******************/
        public ActionResult NoShow()
        {
            string dateFrom = DateTime.Now.AddMonths(-1).ToString("dd-MMM-yyyy");
            string dateTo = DateTime.Now.ToString("dd-MMM-yyyy");
            string[] date = new string[2] { dateFrom, dateTo };
            ViewBag.date = date;
            return PartialView();
        }

        public ActionResult SearchNoShow(string[] dataValue)
        {
            SqlConnection con = new SqlConnection(pbConnectionString);
            //string response = "";
            string rem = "";
            string FlightNo = "";
            string DocNum = "";
            string PaxName = "";

            if (dataValue[6] == "-All-")
            {
                rem = "%";
            }
            else
            {
                rem = dataValue[6];
            }

            if (dataValue[2] == "-All-")
            {

                FlightNo = "%";
            }
            else
            {
                FlightNo = dataValue[2];
            }


            if (dataValue[3] == "")
            {
                DocNum = "%";
            }
            else
            {
                DocNum = dataValue[3];
            }

            if (dataValue[9] == "")
            {
                PaxName = "%";
            }
            else
            {
                PaxName = dataValue[9];
            }
            //string datFromFlt1 = dataValue[0];
            //string datFromFlt = ConvertDate(datFromFlt1);
            //string datToFlt1 = dataValue[1];
            //string datToFlt = ConvertDate(datToFlt1);
            String dtfltfro = "";
            string datToFlt = "";
            String dtfltto = "";
            string datFromFlt1 = "";
            string datToFlt1 = "";
            string datFromFlt = "";
            string datFromUs = "";
            string datToUs = "";
            string datFromUs1 = "";
            string datToUs1 = "";

            if (dataValue[7] == "")
            {
                datFromFlt = new DateTime(2010, 01, 01).ToString("dd-MMM-yyyy");
                datToFlt = new DateTime(2100, 01, 01).ToString("dd-MMM-yyyy");
                //datFromFlt = ConvertDate1(datFromFlt1);
                //datToFlt = ConvertDate1(datToFlt1);
            }
            else
            {
                // datFromFlt1 = dataValue[0];
                //datToFlt1 = dataValue[1];
                datFromFlt = dataValue[0];
                datToFlt = dataValue[1];
                //  datFromFlt = ConvertDate(datFromFlt1);
                //datToFlt = ConvertDate(datToFlt1);
            }

            //String dtfltfro = ConvertDate(dataValue[4]);
            //String dtfltto = ConvertDate(dataValue[5]);

            if (dataValue[8] == "")
            {
                datFromUs1 = new DateTime(2010, 01, 01).ToString("dd-MMM-yyyy");
                datToUs1 = new DateTime(2100, 01, 01).ToString("dd-MMM-yyyy");
                datFromUs = ConvertDate1(datFromUs1);
                datToUs = ConvertDate1(datToUs1);
            }
            else
            {
                /* datFromUs1 = dataValue[4];
                 datToUs1 = dataValue[5];
                 datFromUs = ConvertDate(datFromUs1);
                 datToUs = ConvertDate(datToUs1);*/
                datFromUs = dataValue[4];
                datToUs = dataValue[5];
            }




            SqlCommand cmd = new SqlCommand("[Pax].[SP_Noshow]", con);
            cmd.CommandTimeout = 1000;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@FlightdateFrom", datFromFlt);
            cmd.Parameters.AddWithValue("@FlightdateTo", datToFlt);
            cmd.Parameters.AddWithValue("@UsagedateFrom", datFromUs);
            cmd.Parameters.AddWithValue("@UsagedateTo", datToUs);
            cmd.Parameters.AddWithValue("@FlightNumber", FlightNo);
            cmd.Parameters.AddWithValue("@DocumentNo", DocNum);
            cmd.Parameters.AddWithValue("@PaxName", PaxName);
            cmd.Parameters.AddWithValue("@Remarks", rem);

            DataSet ds = new DataSet();
            SqlDataAdapter ada = new SqlDataAdapter(cmd);
            ada.Fill(ds);

            decimal netVal = 0;
            int lon = ds.Tables[0].Rows.Count;
            string[,] dbgComm = new string[9, lon];
            if (lon == 0)
            {
                ViewBag.message = " No data available for the selected criteria";
            }
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                if (!string.IsNullOrEmpty(dr[ds.Tables[0].Columns[22].ColumnName].ToString()))
                {
                    netVal += Convert.ToDecimal(dr[ds.Tables[0].Columns[22].ColumnName].ToString());
                }
            }
            ViewBag.netVal = netVal.ToString("#,###,###,###.#0");
            ViewBag.nbDocument = ds.Tables[0].Rows.Count;
            ViewBag.lon = lon;
            con.Close();
            return PartialView(ds);

        }

        public ActionResult Remarksselect(string[] dataValue)
        {

            /*string datFromFlt = dataValue[0];
            string datToFlt = dataValue[1];

            string datFromUs = dataValue[2];
            string datToUs = dataValue[3];*/
            string datFromFlt = "";
            string datToFlt = "";

            string datFromUs = "";
            string datToUs = "";

            string datFromFlt1 = "";
            string datToFlt1 = "";

            string dtfltfro = "";
            string dtfltto = "";

            string datFromUs1 = "";
            string datToUs1 = "";

            if (dataValue[4] == "")
            {
                datFromFlt1 = new DateTime(2010, 01, 01).ToString("dd-MMM-yyyy");
                datToFlt1 = new DateTime(2100, 01, 01).ToString("dd-MMM-yyyy");
                datFromFlt = ConvertDate1(datFromFlt1);
                datToFlt = ConvertDate1(datToFlt1);
            }
            else
            {
                /*datFromFlt1 = dataValue[0];
                datToFlt1 = dataValue[1];
                datFromFlt = ConvertDate(datFromFlt1);
                datToFlt = ConvertDate(datToFlt1);*/
                datFromFlt = dataValue[0];
                datToFlt = dataValue[1];
            }

            //String dtfltfro = ConvertDate(dataValue[4]);
            //String dtfltto = ConvertDate(dataValue[5]);

            if (dataValue[5] == "")
            {
                dtfltfro = new DateTime(2010, 01, 01).ToString("dd-MMM-yyyy");
                dtfltto = new DateTime(2100, 01, 01).ToString("dd-MMM-yyyy");
                datFromUs = ConvertDate1(dtfltfro);
                datToUs = ConvertDate1(dtfltto);
            }
            else
            {
                /*datFromUs1 = dataValue[2];
                datToUs1 = dataValue[3];
                datFromUs = ConvertDate(datFromUs1);
                datToUs = ConvertDate(datToUs1);*/
                datFromUs = dataValue[2];
                datToUs = dataValue[3];
            }

            string sql = "select  distinct cpn.FlightNumber" + Environment.NewLine;
            sql += "from Pax.SalesDocumentHeader sdh" + Environment.NewLine;
            sql += "join Pax.SalesRelatedDocumentInformation srd on sdh.HdrGuid = srd.HdrGuid " + Environment.NewLine;
            sql += "and sdh.TransactionCode like 'TK%' and isnull(sdh.OriginalIssueDocumentNumber,'') = ''" + Environment.NewLine;
            sql += "join Pax.fn_SalesCouponDetail(null) cpn on srd.RelatedDocumentGuid = cpn.RelatedDocumentGuid" + Environment.NewLine;
            sql += "where CouponStatus not in ('R','V')" + Environment.NewLine;
            sql += "and not exists( select * from Pax.SalesDocumentPayment sdp where srd.RelatedDocumentGuid  = sdp.RelatedDocumentGuid and sdp.FormOfPaymentType = 'EX' )" + Environment.NewLine;
            sql += "and" + Environment.NewLine;
            sql += "(" + Environment.NewLine;
            sql += " ( UsageDate <> FlightDepartureDate and UsageDate is not null ) Or" + Environment.NewLine;
            sql += " ( FlightDepartureDate < cast('" + datToFlt + "' as date) and UsageDate  is null )" + Environment.NewLine;
            sql += ")" + Environment.NewLine;
            sql += "and FlightDepartureDate  between cast('" + datFromFlt + "' as date) and cast('" + datToFlt + "' as date)" + Environment.NewLine;
            sql += "and UsageDate between cast('" + datFromUs + "' as date) and cast('" + datToUs + "' as date)" + Environment.NewLine;

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
      
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                AgCode[i] = dr[ds.Tables[0].Columns[0].ColumnName].ToString();
                i = i + 1;
            }

            ViewBag.AgCode = AgCode;
            ViewBag.lonAg = lonAg;
            return PartialView();
        }

        public ActionResult SLS()
        {
            string dateFrom = DateTime.Now.AddMonths(-1).ToString("dd-MMM-yyyy");
            string dateTo = DateTime.Now.ToString("dd-MMM-yyyy");
            string[] date = new string[2] { dateFrom, dateTo };
            ViewBag.date = date;
            return PartialView();
        }

        public ActionResult Fightselect(string[] dataValue)
        {
            string dtpFrom = dataValue[0];
            string dtpTo = dataValue[1];
            string Fight = dataValue[2];
            string sql = "SELECT distinct f1.FlightNumber FROM [MSGLDM].[FlightRecord] f1 join [MSGSLS].[FlightRecord] f3 on f1.FlightKey = f3.FlightKey where f3.LocalDate between'" + dtpFrom + "' and '" + dtpTo + "' order by 1 " + Environment.NewLine;
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
                ViewBag.message = " No data available for the selected criteria";
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

        public ActionResult SLSShowTable(string[] dataValue)
        {
            string dtpFrom = dataValue[0];
            string dtpTo = dataValue[1];
            string Fight = dataValue[2];
            string fltno = "";

            if (Fight == "-All-")
            {
                fltno = "%";
            }
            else
            {
                fltno = Fight;
            }

            /* if (dbgSLS.Rows.Count < 1)
             {
                 MessageBox.Show("No data available for the selected criteria.", "Symphony",
                 MessageBoxButtons.OK, MessageBoxIcon.Information);
             }
             */

            string sql = " SELECT  " + Environment.NewLine;
            sql += " format(cast(f3.LocalDate as date),'dd-MMM-yyyy') as FlightDate " + Environment.NewLine;
            sql += "  ,f1.FlightNumber as FlightNo " + Environment.NewLine;
            sql += " ,f4.[DestinationCode] " + Environment.NewLine;
            sql += " ,f3.[FlightNumberSuffix] " + Environment.NewLine;
            sql += " ,f3.[AirlineCode] " + Environment.NewLine;
            sql += ",f3.[AircraftRegistrationNumber] " + Environment.NewLine;
            sql += " ,[StationOfLoading] " + Environment.NewLine;
            sql += ",f3.[AircraftVersion] " + Environment.NewLine;
            sql += " ,[SupplementaryInfo] " + Environment.NewLine;
            sql += " ,[STC] " + Environment.NewLine;
            sql += " ,[ProductCode] " + Environment.NewLine;
            sql += " ,[IgnoreFlag]" + Environment.NewLine;
            sql += " ,[TotalRevPax] " + Environment.NewLine;
            sql += " ,[TotalNonRevPax] " + Environment.NewLine;
            sql += " ,[TotalNonRevInfants] " + Environment.NewLine;
            sql += " ,[TotalElectronicTktPax] " + Environment.NewLine;
            sql += " ,[TotalElectronicTkt_Infants] " + Environment.NewLine;
            sql += " ,[BaggageWeight] " + Environment.NewLine;
            sql += " ,[CargoWeight] " + Environment.NewLine;
            sql += " ,[NonRevCargo] " + Environment.NewLine;
            sql += " ,[Mail] " + Environment.NewLine;

            sql += " FROM [MSGLDM].[FlightRecord] f1 " + Environment.NewLine;
            sql += " join [MSGLDM].[LDM] f2 on f1.FlightKey = f2.FlightKey " + Environment.NewLine;
            sql += " join [MSGSLS].[FlightRecord] f3 on f1.FlightKey = f3.FlightKey " + Environment.NewLine;
            sql += " join [MSGSLS].[SLS] f4 on f1.FlightKey = f4.FlightKey " + Environment.NewLine;
            sql += "  where f3.LocalDate between'" + dtpFrom + "' and '" + dtpTo + "' and f1.FlightNumber like '" + fltno + "' " + Environment.NewLine;

            SqlConnection cs = new SqlConnection(pbConnectionString);
            if (cs.State == ConnectionState.Open)
            {
                cs.Close();
            }
            cs.Open();

            DataSet ds = new DataSet();
            SqlDataAdapter ada = new SqlDataAdapter(sql, cs);
            ada.Fill(ds);
            cs.Close();
            int lonAg = ds.Tables[0].Rows.Count;
            if (lonAg == 0)
            {
                ViewBag.message = " No data available for the selected criteria";
            }
            return PartialView(ds);
        }

        //////////////////////LDM//////////

        public ActionResult LDM()
        {
            string dateFrom = DateTime.Now.AddMonths(-1).ToString("dd-MMM-yyyy");
            string dateTo = DateTime.Now.ToString("dd-MMM-yyyy");
            string[] date = new string[2] { dateFrom, dateTo };
            ViewBag.date = date;
            return PartialView();
        }
        public ActionResult FightselectLDM(string[] dataValue)
        {
            string dtpFrom = dataValue[0];
            string dtpTo = dataValue[1];
            string Fight = dataValue[2];
            string sql = "SELECT distinct f1.FlightNumber FROM [MSGLDM].[FlightRecord] f1 join [MSGSLS].[FlightRecord] f3 on f1.FlightKey = f3.FlightKey where f3.LocalDate between'" + dtpFrom + "' and '" + dtpTo + "' order by 1 " + Environment.NewLine;
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

        public ActionResult SLSShowTableLDM(string[] dataValue)
        {
            string dtpFrom = dataValue[0];
            string dtpTo = dataValue[1];
            string Fight = dataValue[2];
            string fltno = "";

            if (Fight == "-All-")
            {
                fltno = "%";
            }
            else
            {
                fltno = Fight;
            }

            /* if (dbgSLS.Rows.Count < 1)
             {
                 MessageBox.Show("No data available for the selected criteria.", "Symphony",
                 MessageBoxButtons.OK, MessageBoxIcon.Information);
             }
             */

            string sql = "SELECT " + Environment.NewLine; ;
            sql += "format(cast(f3.LocalDate as date), 'dd-MMM-yyyy')as FlightDate" + Environment.NewLine;
            sql += ",f1.FlightNumber as FlightNo" + Environment.NewLine;
            sql += ",f2.DestinationCode" + Environment.NewLine;
            sql += ",f2.PAX3 as First" + Environment.NewLine;
            sql += ",f2.pax1 as Business" + Environment.NewLine;
            sql += ",f2.pax2 as Economy" + Environment.NewLine;
            sql += ",f2.Infants,f2.Children" + Environment.NewLine;
            sql += ", ISNULL(f2.pax1,0) + ISNULL(f2.pax2,0) + ISNULL(f2.pax3,0) as totPax" + Environment.NewLine;
            sql += ",f1.CockpitCrew,f1.CabinCrew" + Environment.NewLine;
            sql += "FROM [MSGLDM].[FlightRecord] f1" + Environment.NewLine;
            sql += "join [MSGLDM].[LDM] f2 on f1.FlightKey = f2.FlightKey" + Environment.NewLine;
            sql += "join [MSGSLS].[FlightRecord] f3 on f1.FlightKey = f3.FlightKey" + Environment.NewLine;
            sql += "join [MSGSLS].[SLS] f4 on f1.FlightKey = f4.FlightKey" + Environment.NewLine;
            sql += "where f3.LocalDate between'" + dtpFrom + "' and '" + dtpTo + "' and f1.FlightNumber like '" + fltno + "'" + Environment.NewLine;

            SqlConnection cs = new SqlConnection(pbConnectionString);
            if (cs.State == ConnectionState.Open)
            {
                cs.Close();
            }
            cs.Open();
            DataSet ds = new DataSet();
            SqlDataAdapter ada = new SqlDataAdapter(sql, cs);
            ada.Fill(ds);
            cs.Close();
            int lonAg = ds.Tables[0].Rows.Count;
            if (lonAg == 0)
            {
                ViewBag.message = " No data available for the selected criteria";
            }
            return PartialView(ds);
        }
        public ActionResult PNLPFS()
        {
            return PartialView();
        }
        public ActionResult OWN()
        {
            string dateFrom = DateTime.Now.AddMonths(-1).ToString("dd-MMM-yyyy");
            string dateTo = DateTime.Now.ToString("dd-MMM-yyyy");
            string[] date = new string[2] { dateFrom, dateTo };
            ViewBag.date = date;
            ViewBag.param = "own";
            return PartialView();
        }

        public ActionResult OAL()
        {
            string dateFrom = DateTime.Now.AddMonths(-1).ToString("dd-MMM-yyyy");
            string dateTo = DateTime.Now.ToString("dd-MMM-yyyy");
            string[] date = new string[2] { dateFrom, dateTo };
            ViewBag.date = date;
            ViewBag.param = "oal";
            return PartialView("OWN");
        }
        /*******************************end*************/
        /* Outward billing */
        public ActionResult OUTWARDBILLING(string type)
        {
            string airlineCode = "Select distinct BALC FROM[Pax].[OutwardBilling] order by BALC";
            DataSet ds = new DataSet();
            ds = dbconnect.RetObjDS(airlineCode);
            ViewBag.DateNow = DateTime.Now.ToString("dd-MMM-yyyy");
            return PartialView(ds);
        }

        public ActionResult getBillingPeriod(string dataValue)
        {
            string period = "Select distinct BILLINGPERIOD  FROM [Pax].[OutwardBilling] where balc like '" + dataValue + "' order by BILLINGPERIOD Desc ";
            DataSet ds = new DataSet();
            ds = dbconnect.RetObjDS(period);
            return PartialView(ds);
        }
        public ActionResult LoadPreviousBilling(string[] dataValue)
        {

            string sql = " with A as ( SELECT F1.BALC ,F2.[AirlineCode],F2.[AirlineName],PMC ,f1.BILLINGPERIOD, count(*) as CouponCount  " + Environment.NewLine;
            sql += " ,sum([AMOUNT]) as GrossAmount ,Sum([COMM]) As ICSAmount , sum([AMOUNT]) - Sum([COMM]) As NetAmount , INVOICENO As [Invoice Number]  " + Environment.NewLine;
            sql += " FROM [Pax].[OutwardBilling] F1  left join  [Ref].[Airlines] F2 on F2.[AirlineID] =F1.BALC   " + Environment.NewLine;
            sql += " where f1.BALC like '" + dataValue[0] + "' and f1.BILLINGPERIOD like '" + dataValue[1] + "'  " + Environment.NewLine;
            sql += " Group by F1.BALC, f1.BILLINGPERIOD ,F2.[AirlineCode]  ,[AirlineName],pmc,INVOICENO ) , B as  ( " + Environment.NewLine;
            sql += " select f1.BillingPeriod,f1.ALC,PMC,sum (Taxamt) as TaxAmt from pax.OutwardBillingTax  f1 join [Pax].[OutwardBilling] f3 on  " + Environment.NewLine;
            sql += " f1.alc = f3.alc and f1.billingperiod = f3.billingperiod and f1.doc = f3.doc and f1.cpn = f3.cpn   " + Environment.NewLine;
            sql += " where f1.ALC like '" + dataValue[0] + "' and f1.BILLINGPERIOD like '" + dataValue[1] + "'  group by f1.ALC,f1.BillingPeriod,pmc )  " + Environment.NewLine;
            sql += "  SELECT BALC ,f1.[AirlineCode],f1.[AirlineName],f1.PMC ,f1.BILLINGPERIOD,  CouponCount , GrossAmount    " + Environment.NewLine;
            sql += " , ICSAmount , NetAmount,isnull((f3.TAXAMT),0) as [Total Tax] , GrossAmount - ICSAmount+ isnull((f3.TAXAMT),0) as [Amount To Bill]  " + Environment.NewLine;
            sql += " ,  [Invoice Number] FROM A F1 left join  [Ref].[Airlines] F2 on F2.[AirlineID] =F1.BALC left join B f3 on f1.BALC= f3.ALC  and f1.BILLINGPERIOD = f3.BillingPeriod and f1.PMC = f3.PMC   " + Environment.NewLine;
            sql += " where f1.BALC like '" + dataValue[0] + "' and f1.BILLINGPERIOD like '" + dataValue[1] + "'  select   " + Environment.NewLine;
            sql += " f1.[BILLINGPERIOD],f1.[ALC] ,f1.[DOC] ,f1.[CPN],f1.[CHK]  ,f1.[PMC],(f1.[SECTORFROM]+' - '+f1.[SECTORTO]) as SECTOR,f1.[AMOUNT]  " + Environment.NewLine;
            sql += " ,f1.[ISC],f1.[COMM],f1.[FLIGHT],f1.[FLTDATE],f1.[ETKTSAC],f1.[Taxamt] as TotalTaxAmt  ,f1.[INVOICENO],f1.[NETAMT]  " + Environment.NewLine;
            sql += " ,[Pax].[fn_OutwardBilling_Tax_Combined](f1.alc+f1.DOC,f1.CPN) as TaxDetails from Pax.VW_SISCPN f1 where BILLINGPERIOD = '" + dataValue + "' order by alc,doc,cpn  " + Environment.NewLine;
            DataSet ds = new DataSet();
            ds = dbconnect.RetObjDS(sql);
            return PartialView(ds);

        }

        public ActionResult DetailsPreviousBilling(string dataValue, string dataValue1)
        {
            string sql = "";
            sql += " IF OBJECT_ID('tempdb..#base') IS NOT NULL DROP TABLE #base;  " + Environment.NewLine;
            sql += " SELECT row_number() over ( partition by OB.ALC,OB.DOC,OB.CPN,OB.BILLINGPERIOD order by OB.BILLINGPERIOD, OB.ALC,OB.DOC,OB.CPN ) as RowId,  " + Environment.NewLine;
            sql += " OB.[ALC],F2.AirlineName,OB.[DOC],OB.[CPN],OB.[CHK],[PMC],[SECTORFROM],[SECTORTO],[AMOUNT],[ISC],[COMM],[FLIGHT],FORMAT(FLTDATE,'dd-MMM-yyyy') as [FLTDATE],[CUR],[ETKTSAC]  " + Environment.NewLine;
            sql += " ,[INVOICENO],OB.[BILLINGPERIOD],CASE WHEN OB.[ManualFlag] = 'M' THEN 'MANUAL OB ENGINE' ELSE 'AUTOMATIC OB ENGINE' END  AS  [ManualFlag]  " + Environment.NewLine;
            sql += " ,OBTAX.TAX,OBTAX.TAXAMT INTO #base FROM [Pax].[OutwardBilling] OB left join [Pax].[OutwardBillingTax] OBTAX ON OB.DOC = OBTAX.DOC and OB.CPN= OBTAX.CPN AND OB.BILLINGPERIOD = OBTAX.BillingPeriod  " + Environment.NewLine;
            sql += " left join  [Ref].[Airlines] F2 on F2.[AirlineID] =OB.BALC ORDER BY BILLINGPERIOD,BALC  " + Environment.NewLine;

            sql += " SELECT TOP 100 PERCENT AirlineName,([ALC]+[DOC]) as DocumentNumber, [CPN],iif( RowId >1, NULL,[CHK]) as [CHK],iif( RowId >1, NULL,[PMC]) as [PMC]  " + Environment.NewLine;
            sql += " ,iif( RowId >1, NULL,[SECTORFROM]) as [SECTORFROM],iif( RowId >1, NULL,[SECTORTO]) as [SECTORTO],iif( RowId >1, NULL,[AMOUNT]) as [AMOUNT]  " + Environment.NewLine;
            sql += " ,iif( RowId >1, NULL,[ISC]) as [ISC],iif( RowId >1, NULL,[COMM]) as [COMM],iif( RowId >1, NULL,[FLIGHT]) as [FLIGHT],iif( RowId >1, NULL,[FLTDATE]) as [FLTDATE]  " + Environment.NewLine;
            sql += " ,iif( RowId >1, NULL,[CUR]) as [CUR],iif( RowId >1, NULL,[ETKTSAC]) as [ETKTSAC],iif( RowId >1, NULL,[INVOICENO]) as [INVOICENO], [BILLINGPERIOD]  " + Environment.NewLine;
            sql += " ,iif( RowId >1, NULL,[ManualFlag]) as OB_ENGINE,(SELECT [BillingPeriod] FROM [Ref].[IATACalendar] F2 WHERE   DATEADD(MM,iif(FLTDATE > '01-JUL-2015',3,4),FLTDATE) BETWEEN F2.[From] AND F2.[To]) AS BILLINGTIMELIMIT  " + Environment.NewLine;
            sql += " ,TAX TAX,TAXAMT FROM #base WHERE BILLINGPERIOD like '" + dataValue + "' and alc like '" + dataValue1 + "' ORDER BY BILLINGPERIOD,ALC,DOC,CPN  " + Environment.NewLine;
            DataSet ds = new DataSet();
            ds = dbconnect.RetObjDS(sql);
            return PartialView(ds);
        }



        public ActionResult GetValidatePrime(string[] dataValue)
        {
            int check = 0;
            string msg = "";
            string alc = dataValue[2] = dataValue[2] == "All" ? "%" : dataValue[2];
            string ErrorMsg = "";
            DataSet ds = new DataSet();

            if (dataValue[3] == "Both")
            {
                check = 1;
            }

            if (dataValue[3] == "Validated")
            {
                check = 2;
            }

            if (dataValue[3] == "NValidated")
            {
                check = 3;
            }

            try
            {
                ds = DisplayReslt(dataValue[0], dataValue[1], alc, check);


                if (ds.Tables[0].Rows.Count < 1)
                {
                    msg = "No data available for the selected criteria.";
                }

                /*try
                {
                    saving();
                }
                catch (Exception ex)
                {

                    msg = "Error while saving Record.";
                }*/
            }

            catch (Exception ex)
            {

                try
                {

                    string sql = "select String1 from ADM.GSP where Parameter ='ERR0001' " + Environment.NewLine;
                    DataSet ds1 = new DataSet();
                    ds1 = dbconnect.RetObjDS(sql);
                    foreach (DataRow dr in ds1.Tables[0].Rows)
                    {
                        msg = dr[ds.Tables[0].Columns[0].ColumnName].ToString();
                    }
                }
                catch { }

                if (ex.Message.Contains("Timeout expired."))
                {
                    ErrorMsg = "Timeout expired.";
                }

            }

            ViewBag.nbLigne = ds.Tables[0].Rows.Count;
            ViewBag.msg = msg;
            ViewBag.ErrorMsg = ErrorMsg;
            return PartialView(ds);
        }

        public JsonResult ValidatePrimeDetail(string dataValue)
        {

            string sql = "select distinct  FORMAT(sdh.DateofIssue,'dd-MMM-yyyy') as DateofIssue, BookingReference as PNR,EndosRestriction from Pax.SalesDocumentHeader sdh ";
            sql += "join pax.SalesRelatedDocumentInformation srd on srd.HdrGuid = sdh.HdrGuid ";
            sql += "join pax.SalesDocumentCoupon sdc on srd.RelatedDocumentGuid = sdc.RelatedDocumentGuid ";
            sql += "where sdc.RelatedDocumentNumber = '" + dataValue + "'";

            DataSet ds = new DataSet();
            ds = dbconnect.RetObjDS(sql);
            string[] response = new string[3];
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                response[0] = dr[ds.Tables[0].Columns[0].ColumnName].ToString();
                response[1] = dr[ds.Tables[0].Columns[1].ColumnName].ToString();
                response[2] = dr[ds.Tables[0].Columns[2].ColumnName].ToString();
            }
            return Json(response);
        }

        public ActionResult OalCarrier(string[] dataValue)
        {
            string sql = "declare @OwnAirline varchar(3) = (select OwnAirline from Adm.fn_OwnAirline()) " + Environment.NewLine;
            sql += " select distinct  f1.AirlineCode from FileLift.LiftHeader f1  " + Environment.NewLine;
            sql += " left join Pax.OutwardBilling f2 on f1.AirlineCode = f2.ALC and f1.TicketNumber = f2.DOC and f1.CouponNumber =f2.CPN " + Environment.NewLine;
            sql += " where f1.AirlineCode <> @OwnAirline and f2.BILLINGPERIOD is null  and f1.UsageAirline = @OwnAirline " + Environment.NewLine;
            sql += " and DATEFROMPARTS( right(f1.UsageDate,2) + 2000, substring(f1.UsageDate,3,2), left(f1.UsageDate,2) )  " + Environment.NewLine;
            sql += "  between '" + dataValue[0] + "' and '" + dataValue[1] + "' order by 1";
            DataSet ds = new DataSet();
            ds = dbconnect.RetObjDS(sql);
            return PartialView(ds);

        }

        public ActionResult ClickOBExclusion(string[] dataValue)
        {
            string msg = "";
            if (dataValue != null)
            {
                try
                {
                    saving(dataValue);
                }
                catch (Exception ex)
                {
                    msg = "Error while saving Record. '" + ex.Message + "' ";
                }
            }

            string sql = "SELECT distinct [AirlineID] FROM [Tmp].[OB_Exclude]" + Environment.NewLine;
            DataSet ds = new DataSet();
            ds = dbconnect.RetObjDS(sql);
            ViewBag.msg = msg;
            return PartialView("OalCarrier", ds);
        }

        public ActionResult LoadOBExclusion(string dataValue)
        {
            string sql = "SELECT AirlineID,SectorFrom,SectorTo,Format(StartDate,'dd-MMM-yyyy') as StartDate, Format(EndDate,'dd-MMM-yyyy') as EndDate, Validated FROM [Tmp].[OB_Exclude] where AirlineId like '" + dataValue + "' " + Environment.NewLine;
            DataSet ds = new DataSet();
            ds = dbconnect.RetObjDS(sql);
            return PartialView(ds);
        }

        public string UpdateValidatePrime(string[] dataValue)
        {

            try
            {
                saving(dataValue);
                return "Record save Successfully";
            }
            catch (Exception ex)
            {
                return "Error while saving Record. '" + ex.Message + "' ";
            }

        }

        public string BtnGeneratePrimeCoupon(string[] dataValue)
        {
            List<string> XDoc = new List<string>();
            List<string> XDocList = new List<string>();
            List<string> Doc = new List<string>();
            List<string> DocList = new List<string>();
            string response = "";
            string[] tmpCheck = new string[2];
            for (int i = 0; i < dataValue.Length; i++)
            {
                if ((dataValue[i] != ""))
                {
                    XDoc.Add(dataValue[i].ToString());
                }
            }

            var result = (from item in XDoc select item).Distinct();

            foreach (var ITE in result)
            {
                XDocList.Add((ITE.ToString()));
            }

            for (int i = 0; i < XDocList.Count(); i++)
            {
                tmpCheck = CheckAirlineInfo(XDocList);
            }

            if (tmpCheck[0] == "true")
            {
                response = "Airline Info missing in Ref.Airline.Please Input Airline Info for " + tmpCheck[1] + " . Do you wish to continue? ";

            }

            else
            {
                if (!Directory.Exists(@"c:\SISinv"))
                {
                    Directory.CreateDirectory(@"C:\SiSinv");
                    response = @"Folder C:\SISinv does not exist on your drive." + "\n" + "To produce SIS invoices, this folder is required." + "\n" + "Do you want Symphony to create this folder?";

                }
                /*#endregion

                #region date filter removal on choice
                DateTime fromFilter = dtpFltDateFrom.Value;

                DialogResult Reply2 = MessageBox.Show("Do you want to generate invoices as per the date filter?", "Symphony", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                if (Reply2 == DialogResult.No)
                {
                    fromFilter = Convert.ToDateTime("01-01-2013");
                }
                #endregion


                FrmSisBillingNew frm = new FrmSisBillingNew();
                frm.mainDock = mainDock;
                frm.LoadBillingData(fromFilter, dtpFltDateTo.Value, ACL);
                frm.ShowDialog();*/
            }
            return response;
        }

        public string yesConfimCouponBilling()
        {
            string response = "";
            if (!Directory.Exists(@"c:\SISinv"))
            {
                //response = @"Folder C:\SISinv does not exist on your drive." + "\n" + "To produce SIS invoices, this folder is required." + "\n" + "Do you want Symphony to create this folder?";
                Directory.CreateDirectory(@"C:\SiSinv");
            }
            //DateTime fromFilter = dtpFltDateFrom.Value;

            return "Do you want to generate invoices as per the date filter?";
            /*if (Reply2 == DialogResult.No)
            {
                fromFilter = Convert.ToDateTime("01-01-2013");
            }
            #endregion


            FrmSisBillingNew frm = new FrmSisBillingNew();
            frm.mainDock = mainDock;
            frm.LoadBillingData(fromFilter, dtpFltDateTo.Value, ACL);
            frm.ShowDialog();*/
        }

        public ActionResult yesGenerateCouponBilling(string[] dataValue)
        {
            var ds = LoadBillingData(Convert.ToDateTime(dataValue[0]), Convert.ToDateTime(dataValue[1]), dataValue[2]);
            return PartialView("LoadTaxVerification", ds);
        }

        private string[] CheckAirlineInfo(List<string> Docnum)
        {
            string[] MissingAirlineFlag = new string[2];
            foreach (string a in Docnum)
            {
                string sql = "  where AirlineID = '" + a + "' " + Environment.NewLine;

                DataSet ds = new DataSet();
                ds = dbconnect.RetObjDS(sql);

                if (ds.Tables[0].Rows.Count < 1)
                {
                    MissingAirlineFlag[0] = "true";
                    MissingAirlineFlag[1] = a;
                }
            }
            return MissingAirlineFlag;
        }
        private void saving(string[] dataVal)
        {
            string[] tmpArray = new string[30];
            for (int iter = 0; iter < dataVal.Length; iter++)
            {
                tmpArray = dataVal[iter].ToString().Split(',');
                string a = tmpArray[0];
                string b = tmpArray[1];
                string c = tmpArray[4];
                string d = tmpArray[5];
                string ee = tmpArray[6];
                string f = tmpArray[7];
                string g = tmpArray[8];
                string h = tmpArray[9];
                string i = tmpArray[10];
                string j = tmpArray[11];
                string k = tmpArray[12];
                string l = tmpArray[14];
                string m = tmpArray[15];
                string n = tmpArray[16];
                string o = tmpArray[17];
                string p = tmpArray[18];
                string q = tmpArray[19];
                string r = tmpArray[20];
                string s = tmpArray[21];
                int t = 0;
                string u = "MainForm.info.Username";
                string v = "MainForm.info.IpAddress";
                string w = tmpArray[2];
                string x = tmpArray[13];
                string y = tmpArray[3];
                string z = tmpArray[25];
                string Ca = tmpArray[26];
                string Cb = tmpArray[27];
                string Cc = tmpArray[28];
                string Cd = tmpArray[29];


                if (Convert.ToBoolean(tmpArray[22]) == true)
                {
                    t = 1;
                }
                string aa = tmpArray[0];
                string bb = tmpArray[4];
                if (a != "")
                {
                    if (checkifexist(aa, bb) == true)
                    {
                        delete(aa, bb);
                        Save2(a, b, c, d, ee, f, g, h, i, j, k, l, m, n, o, p, q, r, s, t, u, v, w, x, y, z, Ca, Cb, Cc, Cd);

                    }
                    else
                    {
                        Save2(a, b, c, d, ee, f, g, h, i, j, k, l, m, n, o, p, q, r, s, t, u, v, w, x, y, z, Ca, Cb, Cc, Cd);
                    }

                }


            }

        }
        private void delete(string a, string b)
        {
            string sql = " delete from [Tmp].[OB_ValidatePrime] where RelatedDocumentNumber = '" + a + "' and CouponNumber = '" + b + "'";
            DataSet ds = new DataSet();
            ds = dbconnect.RetObjDS(sql);
        }
        private bool checkifexist(string a, string b)
        {
            string sql = "SELECT * FROM [Tmp].[OB_ValidatePrime] where RelatedDocumentNumber = '" + a + "' and CouponNumber = '" + b + "' ";
            bool t = false;
            DataSet ds = new DataSet();
            ds = dbconnect.RetObjDS(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                t = true;
            }
            return t;
        }

        private void Save2(string a, string b, string c, string d, string e, string f, string g, string h, string i, string j, string k, string l, string m, string n, string o, string p, string q, string r, string s, int t, string u, string v, string w, string x, string y, string z, string Ca, string Cb, string Cc, string Cd)
        {
            string sql = "";
            sql += " INSERT INTO [Tmp].[OB_ValidatePrime] ( [RelatedDocumentNumber],[AirlineID],[CouponNumber],[ReservationBookingDesignator],[Origin],[Destination],[CouponStatus],[FlightDate]";
            sql += ",[PM],[SPA_Remarks],[SPA] ,[IscPercentage],[MOV_IscPercentage],[FinalShare],[MOV_FinalShare],[ProrationErrorMsg],[MinSectorValue],[MaxSectorValue],[Remarks],[Validate],[User]";
            sql += ",[IP],[MarketingFlightCode],[CODESHARE],[OperatingFlightCode],[SAC],[CodeShareValue],[CodeShareType],[CodeShareReason],[UserMethodology])";
            sql += "VALUES (";

            if (a == "")
            {
                sql += "NULL ";//RelatedDocumentNumber
            }
            else
            {
                sql += "'" + a + "' ";//RelatedDocumentNumber
            }

            if (b == "")
            {
                sql += ",NULL ";//AirlineID
            }
            else
            {
                sql += ",'" + b + "' ";//AirlineID
            }

            if (c == "")
            {
                sql += ",NULL ";//CouponNumber
            }
            else
            {
                sql += ",'" + c + "' ";//CouponNumber
            }

            if (d == "")
            {
                sql += ",NULL ";//ReservationBookingDesignator
            }
            else
            {
                sql += ",'" + d + "' ";//ReservationBookingDesignator
            }

            if (e == "")
            {
                sql += ",NULL ";//Origin
            }
            else
            {
                sql += ",'" + e + "' ";//Origin
            }

            if (f == "")
            {
                sql += ",NULL ";//Destination
            }
            else
            {
                sql += ",'" + f + "' ";//Destination
            }

            if (g == "")
            {
                sql += ",NULL ";//CouponStatus
            }
            else
            {
                sql += ",'" + g + "' ";//CouponStatus
            }

            if (h == "")
            {
                sql += ",NULL ";//FlightDate
            }
            else
            {
                sql += ",'" + Convert.ToDateTime(h).ToString("dd-MMM-yyyy") + "' ";//FlightDate
            }

            if (i == "")
            {
                sql += ",NULL ";//PM
            }
            else
            {
                sql += ",'" + i + "' ";//PM
            }

            if (j == "")
            {
                sql += ",NULL ";//SPA_Remarks
            }
            else
            {
                sql += ",'" + j + "' ";//SPA_Remarks
            }

            if (k == "")
            {
                sql += ",NULL ";//SPA
            }
            else
            {
                sql += ",'" + k + "' ";//SPA
            }

            if (l == "")
            {
                sql += ",NULL ";//IscPercentage
            }
            else
            {
                sql += ",'" + l + "' ";//IscPercentage
            }

            if (m == "")
            {
                sql += ",NULL ";//MOV_IscPercentage
            }
            else
            {
                sql += ",'" + m + "' ";//MOV_IscPercentage
            }

            if (n == "")
            {
                sql += ",NULL ";//FinalShare
            }
            else
            {
                sql += ",'" + n + "' ";//FinalShare
            }

            if (o == "")
            {
                sql += ",NULL ";//MOV_FinalShare
            }
            else
            {
                sql += ",'" + o + "' ";//MOV_FinalShare
            }

            if (p == "")
            {
                sql += ",NULL ";//ProrationErrorMsg
            }
            else
            {
                sql += ",'" + p + "' ";//ProrationErrorMsg
            }

            if (q == "")
            {
                sql += ",NULL ";//MinSectorValue
            }
            else
            {
                sql += ",'" + q + "' ";//MinSectorValue
            }

            if (r == "")
            {
                sql += ",NULL ";//MaxSectorValue}
            }

            else
            {
                sql += ",'" + r + "' ";//MaxSectorValue
            }

            if (s == "")
            {
                sql += ",NULL ";//Remarks
            }
            else
            {
                sql += ",'" + s + "' ";//Remarks
            }
            sql += ",'" + t + "' ";//

            if (u == "")
            {
                sql += ",NULL ";//user
            }
            else
            {
                sql += ",'" + u + "' ";//user
            }

            if (v == "")
            {
                sql += ",NULL ";//ip
            }
            else
            {
                sql += ",'" + v + "' ";//ip
            }

            if (w == "")
            {
                sql += ",NULL ";//MarketingFlightCode
            }
            else
            {
                sql += ",'" + w + "' ";//MarketingFlightCode
            }

            if (x == "")
            {
                sql += ",NULL ";//CodeShare
            }
            else
            {
                sql += ",'" + x + "' ";//CodeShare
            }

            if (y == "")
            {
                sql += ",NULL ";//OperatingFlightCode
            }
            else
            {
                sql += ",'" + y + "' ";//OperatingFlightCode
            }

            if (z == "")
            {
                sql += ",NULL ";//Sac
            }
            else
            {
                sql += ",'" + z + "' ";//Sac
            }

            if (Ca == "")
            {
                sql += ",NULL ";//CodeShareValue
            }
            else
            {
                sql += ",'" + Ca + "' ";//CodeShareValue
            }

            if (Cb == "")
            {
                sql += ",NULL ";//CodeShareType
            }
            else
            {
                sql += ",'" + Cb + "' ";//CodeShareType
            }

            if (Cc == "")
            {
                sql += ",NULL ";//CodeShareReason
            }
            else
            {
                sql += ",'" + Cc + "' ";//CodeShareReason
            }

            if (Cd == "")
            {
                sql += ",NULL ";//UserMethodology
            }
            else
            {
                sql += ",'" + Cd + "' ";//UserMethodology
            }
            sql += ")";

            DataSet ds = new DataSet();
            ds = dbconnect.RetObjDS(sql);

        }

        public string checkifexist(string[] dataValue)
        {
            string response = "";

            string sql1 = "SELECT * FROM [Tmp].[OB_Exclude] WHERE AirlineId = '" + dataValue[0] + "' and sectorfrom = '" + dataValue[1] + "' and sectorto = '" + dataValue[2] + "' and startdate =  '" + dataValue[3] + "' and enddate =  '" + dataValue[4] + "' ";
            string sql = "";
            DataSet ds = new DataSet();
            ds = dbconnect.RetObjDS(sql1);
            //update
            if (ds.Tables[0].Rows.Count > 1)
            {
                sql += "Update [Tmp].[OB_Exclude]  set AirlineId = '" + dataValue[0] + "' , sectorfrom = '" + dataValue[1] + "' , sectorto = '" + dataValue[2] + "' , startdate =  '" + dataValue[3] + "' , enddate =  '" + dataValue[4] + "' , validated = '" + dataValue[5] + "'  ";
                sql += "where AirlineId = '" + dataValue[0] + "' and sectorfrom = '" + dataValue[1] + "' and sectorto = '" + dataValue[2] + "' and startdate =  '" + dataValue[3] + "' and enddate =  '" + dataValue[4] + "'   ";
                response = "Record Save Sucessfully";
            }
            else  // add
            {
                sql = sql + "INSERT INTO [Tmp].[OB_Exclude] Values ( '" + dataValue[0] + "'";
                sql = sql + ",'" + dataValue[1] + "',";
                sql = sql + "'" + dataValue[2] + "',";
                sql = sql + "'" + dataValue[3] + "',";
                sql = sql + "'" + dataValue[4] + "',";
                sql = sql + "" + dataValue[5] + ")";
                response = "Record Save Sucessfully";
            }
            DataSet ds1 = new DataSet();
            ds1 = dbconnect.RetObjDS(sql);
            return response;
        }

        public ActionResult TaxCarrier()
        {
            string sql = "";
            sql += "SELECT  distinct f1.AirlineID FROM [Tmp].[OB_ValidatePrime] f1 " + Environment.NewLine;
            sql += "left join [Tmp].[OB_ValidatedTax] f2 on f1.RelatedDocumentNumber = f2.RelatedDocumentNumber and f1.CouponNumber = f2.CouponNumber " + Environment.NewLine;
            sql += "where f1.Validate =1 order by 1 asc " + Environment.NewLine;
            DataSet ds = new DataSet();
            ds = dbconnect.RetObjDS(sql);
            return PartialView("OalCarrier", ds);
        }

        public ActionResult TaxSector(string dataValue)
        {
            string sql = "";
            sql += " SELECT distinct Origin + ' - ' + Destination  FROM[Tmp].[OB_ValidatePrime] f1 ";
            sql += " left join[Tmp].[OB_ValidatedTax] f2 on f1.RelatedDocumentNumber = f2.RelatedDocumentNumber and f1.CouponNumber = f2.CouponNumber ";
            sql += " where f1.Validate = 1 and f1.AirlineID like '" + dataValue + "' order by 1 asc";
            DataSet ds = new DataSet();
            ds = dbconnect.RetObjDS(sql);
            return PartialView("OalCarrier", ds);
        }

        public ActionResult LoadTaxVerification(string dataValue, string dataValue1)
        {
            string sql = "";
            sql += "SELECT f1.RelatedDocumentNumber, f1.CouponNumber,AirlineID,Origin,Destination,[TaxCode],[TaxCurrency],[TaxAmount] ";
            sql += "FROM [Tmp].[OB_ValidatePrime] f1 left join [Tmp].[OB_ValidatedTax] f2 on f1.RelatedDocumentNumber = f2.RelatedDocumentNumber and f1.CouponNumber = f2.CouponNumber  ";
            sql += "where f1.Validate =1 and  Origin+' - '+Destination like '" + dataValue + "' and f1.AirlineID like '" + dataValue1 + "' ";
            DataSet ds = new DataSet();
            ds = dbconnect.RetObjDS(sql);
            return PartialView(ds);
        }

        public string BtnTransitCheckClick(string dataValue)
        {
            string Car = "";
            try
            {
                if (dataValue == "ALL")
                {
                    Car = "%";
                }
                else
                {
                    Car = dataValue;
                }
                TransitDocCheck();
                EXECTAX(Car);
            }
            catch { }
            return "Succesfuly";
        }

        private void TransitDocCheck()
        {

            string Sql = "select sdh.DocumentNumber,sdh.FareCalculationArea " + Environment.NewLine;
            Sql += "from Pax.SalesDocumentHeader sdh" + Environment.NewLine;
            Sql += "join pax.SalesRelatedDocumentInformation srd on sdh.HdrGuid = srd.HdrGuid" + Environment.NewLine;
            Sql += "join tmp.OB_ValidatePrime PR on PR.RelatedDocumentNumber = srd.RelatedDocumentNumber " + Environment.NewLine;
            Sql += "where PR.Validate = '1'" + Environment.NewLine;

            DataSet ds = new DataSet();
            ds = dbconnect.RetObjDS(Sql);
            int nbRow = ds.Tables[0].Rows.Count;
            string[] arr = new string[nbRow];
            int u = 0;
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                if (!string.IsNullOrEmpty(dr[ds.Tables[0].Columns[1].ColumnName].ToString()))
                {
                    arr[u] = dr[ds.Tables[0].Columns[0].ColumnName].ToString();
                    u++;
                }
            }

            string[] Xarr1 = arr.Where(s => !string.IsNullOrWhiteSpace(s)).Distinct().ToArray();

            for (int i = 0; i < Xarr1.Length; i++)
            {
                try
                {
                    StopOver(Xarr1[i].ToString());
                }
                catch
                {
                }
            }
        }

        private void EXECTAX(string Carrier)
        {
            try
            {
                string sql = "EXEC [Pax].[SP_ApplicableTaxes] @IssueDate_From=null, @IssueDate_To=null, 	@RelatedDocumentNumber =null , @BillingAirline = '" + Carrier + "'   " + Environment.NewLine;

                DataSet ds = new DataSet();
                ds = dbconnect.RetObjDS(sql);
            }
            catch
            {
            }

        }

        public void StopOver(string DocNum)
        {
            string FCA = Search(DocNum);

            string[] Sec = SearchDetails(DocNum);
            Sec = Sec.Where(x => !string.IsNullOrEmpty(x)).ToArray();

            string[] SecCount = new string[Sec.Length];

            string CleanFCA = ClearFCA(DocNum, FCA);

            int m = 0;
            for (int i = 0; i < Sec.Length; i++)
            {
                if (CleanFCA.Contains(Sec[i]))
                {
                    try
                    {
                        Update(Sec[i + 1], DocNum);
                    }
                    catch
                    {
                    }
                }
            }
        }

        public void Update(string Sector, string DocNum)
        {
            string Ori = Sector.Substring(0, 3);
            string Carr = Sector.Substring(4, 2);
            string Dest = Sector.Substring(9, 3);

            string sql = "UPDATE Pax.SalesDocumentCoupon SET StopOverCode = 'X'" + Environment.NewLine;
            sql += "FROM Pax.SalesDocumentCoupon SDC join pax.SalesRelatedDocumentInformation SRD on SDC.RelatedDocumentGuid = SRD.RelatedDocumentGuid " + Environment.NewLine;
            sql += "WHERE SRD.DocumentNumber = '" + DocNum + "' AND SDC.[OriginCity] = '" + Ori + "' AND SDC.[DestinationCity] = '" + Dest + "'";

            DataSet ds = new DataSet();
            ds = dbconnect.RetObjDS(sql);
        }
        private string[] SearchDetails(string DocNum)
        {
            string[] Sec = new string[10];

            string sql = "SELECT concat(sdc.OriginCity,' ',sdc.Carrier,' X/',sdc.DestinationCity) as Sector " + Environment.NewLine;
            sql += " FROM pax.SalesDocumentCoupon sdc  " + Environment.NewLine;
            sql += " join pax.SalesRelatedDocumentInformation srd on sdc.RelatedDocumentGuid = srd.RelatedDocumentGuid and  srd.DocumentNumber = '" + DocNum + "' " + Environment.NewLine;
            sql += " ORDER BY srd.RelatedDocumentNumber, sdc.CouponNumber " + Environment.NewLine;
            DataSet ds = new DataSet();
            ds = dbconnect.RetObjDS(sql);
            int j = 0;
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                if (!string.IsNullOrEmpty(dr[ds.Tables[0].Columns[0].ColumnName].ToString()))
                {
                    Sec[j] = dr[ds.Tables[0].Columns[0].ColumnName].ToString();
                    j++;
                }
            }
            return Sec;
        }

        private string Search(string DocNum)
        {
            string FCA = "";
            string sql = "SELECT FareCalculationArea" + Environment.NewLine;
            sql += " from pax.SalesDocumentHeader" + Environment.NewLine;
            sql += "where DocumentNumber = '" + DocNum + "'";
            DataSet ds = new DataSet();
            ds = dbconnect.RetObjDS(sql);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                if (!string.IsNullOrEmpty(dr[ds.Tables[0].Columns[0].ColumnName].ToString()))
                {
                    FCA = dr[ds.Tables[0].Columns[0].ColumnName].ToString();
                }
            }
            return FCA;
        }

        public string ClearFCA(string TicketNo, string FCA)
        {

            string Q = @"(((Q)(\s)([A-Z]{6})([0-9])+(\.)([0-9]{2}))|((Q)(\s)([A-Z]{6})(\s)([0-9])+(\.)([0-9]{2})))";
            string Q1 = @"(((Q)(\s)([A-Z]{6})(/IT))|((Q)(\s)([A-Z]{6})(\s)(/IT)))";
            string RemoveStartingDate = @"(^[0-9]{1,2}[A-Z]{3})";
            string Mrem = @"((?!([A-Z]{2}M([0-9])+((\.)+[0-9]{0,2})))((\.)([0-9]{0,2})(M)(([0-9])+((\.)+[0-9]{0,2})|([0-9]{2,8})))|((\s)([0-9]{0,2})(M)(([0-9])+((\.)+[0-9]{0,2})|([0-9]{2,8}))))";
            string CF = @"((/FC)|(FC/))";
            string FCdate = @"((FC )([0-9]{1,2})([A-Z]{3})(\s)([A-Z]{3}))";
            string MatchSpace = @"(((N)+((\s){1,100})+(UC))|((NU)+((\s){1,100})+(C)))";
            string MatchSpaceROE = @"(((R)((\s){1,50})(OE))|((RO)((\s){1,50})(E))|((ROE)(\s){1,100}))";
            string LESS = @"((([0-9])+((\.)+[0-9]{0,2})(LESS)([0-9])+((\.)+[0-9]{0,2}))|(([0-9]{2,8})(LESS)([0-9]{2,8})))";
            string RO = @"(((ROE)+([0-9])+(\.)+[0-9]{0,6})|(ROE1))";
            string RemoveBigM = @"(((\s)(M)(\s)([A-Z]{6}))|(([0-9]{1,2})(M)(\s)([A-Z]{6})))";

            string amt = @"((?!((A3)(A6)(A9)(B6)(B2)(B7)(B8)(C5)(C9)(D5)(D6)(D9)(E0)(E6)(E7)(F2)(F7)(F9)(G3)(G4)(G7)(H2)(I5)(J2)(J8)(K5)(K6)(L7)(M3)(M5)(M6)(M7)(N3)(N8)(O9)(P5)(Q2)(R2)(R4)(S2)(S3)(S4)(S5)(S7)(T0)(U6)(U7)(U8)(U9)(V0)(V3)(V4)(W5)(W8)(W9)(X3)(Y4)(Y8)(Z2)(Z4)(Z5)(Z6)))(([0-9])+(\.)+[0-9]{0,2})|([0-9]{2,7})|((\s)([Q0-9])+(\.)+[0-9]{0,2})|((\s)(Q)(\s)([A-Z]{6})(([0-9])+(\.)+[0-9]{0,2})))";

            string nucwithspace = @"((NUC)((\s){1,100}))";
            string GobalIndicator = @"(([*](WR)[*])|([*](EH)[*])|([*](AT)[*])|([*](SA)[*])|([*](PN)[*])|([*](PA)[*])|([*](AP)[*])|([*](TS)[*])|([*](RU)[*])|([*](FE)[*]))";

            Match matcstart = Regex.Match(FCA, RemoveStartingDate);
            Match man = Regex.Match(FCA, MatchSpace);
            Match maon = Regex.Match(FCA, MatchSpaceROE);
            Match less = Regex.Match(FCA, LESS);
            Match matchROE = Regex.Match(FCA, RO);
            Match matBigM = Regex.Match(FCA, RemoveBigM);
            Match nucmat = Regex.Match(FCA, nucwithspace);

            Match matc = Regex.Match(FCA, CF);

            if (matc.Success)
            {
                string cf = null;
                cf = matc.Value;
                string[] g = new string[] { cf };
                string[] g1 = FCA.Split(g, StringSplitOptions.None);
                string g3 = g1[1].ToString();
                FCA = g3;
            }

            foreach (Match Qf in Regex.Matches(FCA, Q))
            {
                string xx = Qf.Value;
                FCA = FCA.Replace(xx, " ");
            }

            foreach (Match Qf1 in Regex.Matches(FCA, Q1))
            {
                string xx = Qf1.Value;
                FCA = FCA.Replace(xx, " ");
            }

            foreach (Match Am in Regex.Matches(FCA, amt))
            {
                string xx = Am.Value;
                FCA = FCA.Replace(xx, " ");
            }

            if (FCA.StartsWith("FC "))
            {
                Match matFC = Regex.Match(FCA, FCdate);

                string cf = null;
                cf = matFC.Value;
                string[] g = new string[] { cf };
                string[] g1 = FCA.Split(g, StringSplitOptions.None);
                string g3 = g1[1].ToString();
                string GF = cf.Substring(9, cf.Length - 9);
                FCA = GF + g3;
            }


            foreach (Match matGlobal1 in Regex.Matches(FCA, GobalIndicator))
            {
                string xx = matGlobal1.Value;
                FCA = FCA.Replace(xx, " ");
            }

            if (matcstart.Success)
            {
                FCA = FCA.Substring(6, FCA.Length - 6);
            }

            if (nucmat.Success)
            {
                string j = nucmat.Value;
                string k = nucmat.Value.Trim();
                FCA = FCA.Replace(j, k);
            }

            foreach (Match matMM in Regex.Matches(FCA, Mrem))
            {
                string xx = matMM.Value;
                if (xx != "")
                {
                    string h = "M";
                    string[] a = new string[] { h };
                    string[] aa = xx.Split(a, StringSplitOptions.None);
                    string first = aa[0].ToString().Trim();
                    string second = aa[1].ToString().Trim();

                    if (first.Contains("."))
                    {
                        FCA = Regex.Replace(FCA, xx, first + " " + second.Trim());
                    }

                    FCA = Regex.Replace(FCA, xx, second.Trim());
                }
            }

            if (man.Success)
            {
                FCA = Regex.Replace(FCA, MatchSpace, "NUC");
            }

            if (maon.Success)
            {
                FCA = Regex.Replace(FCA, MatchSpaceROE, "ROE");
            }

            if (matBigM.Success)
            {
                FCA = Regex.Replace(FCA, RemoveBigM, "");
            }

            if (less.Success)
            {
                foreach (Match matless in Regex.Matches(FCA, LESS))
                {
                    string xx = matless.Value;
                    string yy = matless.Value;
                    string v = @"(LESS)";
                    Match l = Regex.Match(xx, v);
                    string h = l.Value;
                    string[] a = new string[] { h };
                    string[] aa = xx.Split(a, StringSplitOptions.None);
                    string first = aa[0].ToString().Trim();
                    string second = aa[1].ToString().Trim();
                    string newval = (Convert.ToDouble(first) - Convert.ToDouble(second)).ToString();
                    FCA = Regex.Replace(FCA, yy, newval);
                }
            }

            if (matchROE.Success)
            {
                string r = null;
                r = matchROE.Value;
                string[] g = new string[] { r };
                string[] g1 = FCA.Split(g, StringSplitOptions.None);
                string g3 = g1[0].ToString();

                FCA = g3 + r;
            }


            if ((FCA.StartsWith("I-")) || (FCA.StartsWith("I ")))
            {
                FCA = FCA.Substring(2, FCA.Length - 2);
            }

            FCA = FCA.TrimStart();

            if ((FCA.Substring(2, 3).ToUpper() == "JAN") || (FCA.Substring(2, 3).ToUpper() == "FEB") || (FCA.Substring(2, 3).ToUpper() == "MAR") || (FCA.Substring(2, 3).ToUpper().ToUpper() == "APR") || (FCA.Substring(2, 3).ToUpper() == "MAY") || (FCA.Substring(2, 3).ToUpper() == "JUN") || (FCA.Substring(2, 3).ToUpper() == "JUL") || (FCA.Substring(2, 3).ToUpper() == "AUG") || (FCA.Substring(2, 3).ToUpper() == "JAN") || (FCA.Substring(2, 3).ToUpper() == "SEP") || (FCA.Substring(2, 3).ToUpper() == "OCT") || (FCA.Substring(2, 3).ToUpper() == "NOV") || (FCA.Substring(2, 3).ToUpper() == "DEC"))
            {
                string _y = FCA.Substring(7, FCA.Length - 7);
                FCA = _y;
            }

            string Space = @"((\s){1,100})";
            foreach (Match Am in Regex.Matches(FCA, Space))
            {
                string xx = Am.Value;
                FCA = FCA.Replace(xx, " ");
            }

            string sql = "select sdh.InvoluntaryReroute from Pax.SalesRelatedDocumentInformation srd" + Environment.NewLine;
            sql += "join Pax.SalesDocumentHeader sdh on RelatedDocumentNumber = '" + TicketNo + "' and  IsConjunction=0 and srd.Transactioncode <> 'RFND'" + Environment.NewLine;
            sql += "and sdh.HdrGuid = srd.HdrGuid" + Environment.NewLine;





            string invo = InstanceSingleRecord(sql);

            if (invo.Trim() == "Y")
            {
                FCA = RemoveAllFareBasis(FCA);
            }

            bool Own = CheckifOwn(TicketNo);

            if (Own == false)
            {
                FCA = RemoveAllFareBasis(FCA);
            }

            FCA = FCA.Replace("/EEE", "");
            FCA = FCA.Replace(" E/XXX ", " ");
            FCA = FCA.Replace("E/XXX ", " ");
            FCA = FCA.Replace(" X/E/", " X/");
            FCA = FCA.Replace(" M ", " ");
            FCA = FCA.Replace(" /BT/ ", " ");
            FCA = FCA.Replace(" /IT/ ", " ");
            FCA = FCA.Replace(" USD ", " ");
            FCA = FCA.Replace(" PLUS/IT ", " ");
            return FCA;

        }

        public string InstanceSingleRecord(string sql)
        {
            string val = "";

            DataSet ds = new DataSet();
            ds = dbconnect.RetObjDS(sql);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                if (!string.IsNullOrEmpty(dr[ds.Tables[0].Columns[0].ColumnName].ToString()))
                {
                    val = dr[ds.Tables[0].Columns[0].ColumnName].ToString();
                }
            }
            return val;
        }

        public string RemoveAllFareBasis(string FCA)
        {
            string FC = FCA;
            string sql = "select farebasisticketDesignator from pax.Farebasis where len(farebasisticketDesignator) between 4 and 8 order by len(farebasisticketDesignator) desc";
            string frb = "";

            DataSet ds = new DataSet();
            ds = dbconnect.RetObjDS(sql);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                frb = dr[ds.Tables[0].Columns[0].ColumnName].ToString();

                if ((FC.Contains(frb) && (frb.Substring(0, 3) != "MOW")))
                {
                    FC = FC.Replace(frb, "");
                }
            }
            return FC;
        }

        public bool CheckifOwn(string ticketno)
        {
            bool a = false;

            string AirlineCode = ticketno.Trim().Substring(0, 3).ToString();

            string sql = "SELECT [String4] FROM [Adm].[GSP] WHERE [Parameter] = 'SYS0001'";
            string air = InstanceSingleRecord(sql);

            if (AirlineCode == air.ToString().Trim()) { a = true; }

            return a;
        }

        public ActionResult TaxEditingAirport()
        {
            string sql = "SELECT[FromAirport] + ' - ' + [ToAirport]  AS Sector  FROM [Ref].[Tax]  Group by FromAirport,ToAirport Order  by FromAirport, ToAirport";
            DataSet ds = new DataSet();
            ds = dbconnect.RetObjDS(sql);
            return PartialView(ds);
        }

        public ActionResult DetailTaxEditingAirport(string dataValue)
        {
            string[] Param = dataValue.Split('-');
            string Sql = "SELECT  [FromAirport] as [Sector From] ,[ToAirport] as [Sector To],[MappedPrimeCode] as [Prime Code],[PassengerType] as [Pax Type] " + Environment.NewLine;
            Sql += " ,[DomInt],[TransitDuration] as [Transit Duration],FORMAT([ValidFrom],'dd-MMM-yyyy') as [Valid From],FORMAT([ValidTo],'dd-MMM-yyyy') as [Valid To],[TaxCode] as [Tax Code],[TaxCurrency] as [Tax Currency] " + Environment.NewLine;
            Sql += " ,[TaxAmount] as [Tax Amount],[TaxPercentage],[TAXREFID]  FROM [Ref].[Tax] where [FromAirport]= '" + Param[0].ToString().Trim() + "' and [ToAirport] ='" + Param[1].ToString().Trim() + "'  " + Environment.NewLine;
            DataSet ds = new DataSet();
            ds = dbconnect.RetObjDS(Sql);
            return PartialView(ds);
        }

        public string DeleteDetailTaxEditing(string dataValue)
        {
            string SqlDel = "DELETE REF.TAX WHERE TAXREFID =" + dataValue;
            DataSet ds = new DataSet();
            ds = dbconnect.RetObjDS(SqlDel);
            return "Record deleted Successfully";
        }

        public string AddDetailTaxEditing(string[] dataValue)
        {
            string Sql = "DECLARE @RecId bigint set @RecId = (select iif(MAX(TAXREFID) is null,1, MAX(TAXREFID)+1) As MaxLineid from REF.TAX); INSERT INTO REF.TAX (";
            Sql = Sql + "[FromAirport],[ToAirport],[MappedPrimeCode],[PassengerType],[DomInt],[TransitDuration],[ValidFrom],[ValidTo],[TaxCode],[TaxCurrency],[TaxAmount],[TaxPercentage],TAXREFID)";
            Sql = Sql + "VALUES('";
            Sql = Sql + RetNullOrValue(dataValue[0]) + "','";
            Sql = Sql + RetNullOrValue(dataValue[1].Trim()) + "','";
            Sql = Sql + RetNullOrValue(dataValue[2].Trim()) + "','";
            Sql = Sql + RetNullOrValue(dataValue[3].Trim()) + "','";
            Sql = Sql + RetNullOrValue(dataValue[4].Trim()) + "',";
            Sql = Sql + RetNullOrValue(dataValue[5].Trim()) + ",'";
            if (dataValue[12] == "On")
            {
                Sql = Sql + RetNullOrValue(dataValue[6].Trim()) + "',";
            }
            else
            {
                Sql = Sql + "Null,";
            }
            if (dataValue[13] == "On")
            {
                Sql = Sql + "'" + RetNullOrValue(dataValue[7].Trim()) + "','";
            }
            else
            {
                Sql = Sql + "Null,";
            }

            Sql = Sql + RetNullOrValue(dataValue[8].Trim()) + "','";
            Sql = Sql + RetNullOrValue(dataValue[9].Trim()) + "',";
            Sql = Sql + RetNullOrValue(dataValue[10].Trim()) + ",";
            Sql = Sql + RetNullOrValue(dataValue[11]) + ", @RecId )";
            Sql = Sql.Replace("'Null'", "Null");

            DataSet ds = new DataSet();
            ds = dbconnect.RetObjDS(Sql);
            return "Record Save Sucessfully";
        }

        string RetNullOrValue(string data)
        {
            string RetVal = "Null";

            if (data.Trim().ToString().Length > 0) { RetVal = data.Trim().ToString(); }

            return RetVal;
        }

        public string UpdateRefTax(string[] dataValue)
        {
            string Sql = "UPDATE  REF.TAX SET";
            Sql += " [FromAirport]='" + RetNullOrValue(dataValue[0].Trim()) + "',";
            Sql += " [ToAirport]='" + RetNullOrValue(dataValue[1].Trim()) + "',";
            Sql += " [MappedPrimeCode]='" + RetNullOrValue(dataValue[2].Trim()) + "',";
            Sql += " [PassengerType]='" + RetNullOrValue(dataValue[3].Trim()) + "',";
            Sql += " [DomInt]='" + RetNullOrValue(dataValue[4].Trim()) + "',";
            Sql += " [TransitDuration]=" + RetNullOrValue(dataValue[5].Trim()) + ",";

            if (dataValue[12] == "On")
            {
                Sql += " [ValidFrom]='" + RetNullOrValue(dataValue[6].Trim()) + "',";
            }
            else
            {
                Sql += " [ValidFrom]= Null,";
            }
            if (dataValue[13] == "On")
            {
                Sql += " [ValidTo]='" + RetNullOrValue(dataValue[7].Trim()) + "',";
            }
            else
            {
                Sql += " [ValidTo]= Null,";
            }

            Sql += " [TaxCode]='" + RetNullOrValue(dataValue[8].Trim()) + "',";
            Sql += " [TaxCurrency]='" + RetNullOrValue(dataValue[9].Trim()) + "',";
            Sql += " [TaxAmount]=" + RetNullOrValue(dataValue[10].Trim()) + ",";
            Sql += " [TaxPercentage]=" + RetNullOrValue(dataValue[11].Trim());
            Sql += " WHERE TAXREFID=" + dataValue[14];
            Sql = Sql.Replace("'Null'", "Null");
            DataSet ds = new DataSet();
            ds = dbconnect.RetObjDS(Sql);
            return "Record Save Sucessfully";
        }

        public ActionResult btnGenerateInvoices(string[] dataValue)
        {
            int pgBarMaximum = 0;
            int pgBarMinimum = 0;
            int pgBarValue = 0;
            string lblProgress = "";
            string lblProgress2 = "";
            DataSet ds1 = new DataSet();
            try
            {
                GetSystemParameter();

                /*if (string.IsNullOrEmpty(dataValue[0].Trim()) || string.IsNullOrEmpty(dataValue[1].Trim()) || string.IsNullOrEmpty(dataValue[2].Trim()))
                {
                    dataValue[0].Focus();
                    MessageBox.Show("Invalid period.", "Symphony", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }*/

                //this.btnGenerateInvoices.Click -= new System.EventHandler(this.btnGenerateInvoices_Click);
                string mthclr = "20" + dataValue[0] + dataValue[1];
                //LoadBillingData(_fromDate, _toDate, _alcparam);
                var dbgListInv = LoadBillingData(Convert.ToDateTime(dataValue[3]), Convert.ToDateTime(dataValue[4]), dataValue[5]);

                pgBarMaximum = dbgListInv.Tables[0].Rows.Count + 1;
                pgBarMinimum = 0;
                pgBarValue = 1;
                lblProgress = string.Empty;
                lblProgress2 = string.Empty;

                string airlineCode = string.Empty;
                string airlineName = string.Empty;

                DateTime DateFrom;
                DateTime DateTo;
                string billingPeriod = dataValue[0] + dataValue[1] + dataValue[2];
                int iRow = 0;

                foreach (DataRow dr in dbgListInv.Tables[0].Rows)
                {

                    airlineCode = dr[dbgListInv.Tables[0].Columns[0]].ToString();
                    airlineName = dr[dbgListInv.Tables[0].Columns[1]].ToString();
                    lblProgress2 = string.Format("{0} of {1} Airlines", iRow + 1, pgBarMaximum - 1);
                    lblProgress = string.Format("Generating Billing Records for {0}", airlineName);

                    string fromConvert = Convert.ToDateTime(dataValue[3]).Year.ToString() + "-" +
                                         Convert.ToDateTime(dataValue[3]).Month.ToString() + "-" +
                                         Convert.ToDateTime(dataValue[3]).Day.ToString();

                    string toConvert = Convert.ToDateTime(dataValue[4]).Year.ToString() + "-" +
                                       Convert.ToDateTime(dataValue[4]).Month.ToString() + "-" +
                                       Convert.ToDateTime(dataValue[4]).Day.ToString();

                    DateFrom = Convert.ToDateTime(fromConvert);
                    DateTo = Convert.ToDateTime(toConvert);
                    iRow++;

                    try
                    {
                        string sql = "exec Pax.SP_GenerateBillingRecordsNew @AirlineCode ='" + airlineCode + "', @BillingPeriod = '" + billingPeriod + "', @fromDate = '" + DateFrom + "', @toDate = '" + DateTo + "'     " + Environment.NewLine;
                        DataSet ds = new DataSet();
                        ds = dbconnect.RetObjDS(sql);
                    }
                    catch (Exception ex)
                    {
                        /*using (StreamWriter writerTXT = new StreamWriter(@"C:\SISinv\OBerrorLog.txt", true))
                            writerTXT.WriteLine(ex.Message);
                        MessageBox.Show("Error While Processing Prime Biling.\n Please check C:\\SISinv\\OBerrorLog.txt ");*/

                    }

                    /*Application.DoEvents();
                    System.Threading.Thread.Sleep(1000);
                    pgBar.PerformStep();
                    pgBar.Refresh();

                    if (ErrorLog.fncCheckError() == 0)
                        dbgListInv.Rows[iRow].Cells[4].Value = statusImageList.Images[0];
                    dbgListInv.Refresh();
                    Application.DoEvents();*/
                    UpdateBillingPeriod(airlineCode, dataValue[0] + dataValue[1] + dataValue[2]);

                }

                /*lblProgress.Text = "Completing Process.";
                Application.DoEvents();
                System.Threading.Thread.Sleep(1000);
                lblProgress2.Text = "";
                lblProgress.Text = "Completed.";
                Application.DoEvents();

                pgBar.Value = pgBar.Maximum;
                btnViewRecord.Enabled = true;
                buttonSIS.Enabled = true;
                btnCancel.Enabled = true;*/

                ds1 = Query(billingPeriod);

            }
            catch
            {
            }
            return PartialView(ds1);
        }

        private DataSet Query(string perido)
        {
            string sql = "";

            sql += "  with base as   " + Environment.NewLine;
            sql += " (  " + Environment.NewLine;
            sql += " SELECT top 100 percent [BALC],F2.[AirlineCode],F2.[AirlineName],[BILLINGPERIOD], count(*) as CouponCount ,sum([AMOUNT]) as GrossAmount,sum([COMM]) as ISCAmount  " + Environment.NewLine;
            sql += " , INVOICENO As [Invoice Number]  FROM [Pax].[VW_SISCPN] f1  left join  [Ref].[Airlines] F2 on F2.[AirlineID] =F1.BALC   " + Environment.NewLine;
            sql += " where BILLINGPERIOD = '" + perido + "' Group by F1.BALC, BILLINGPERIOD ,F2.[AirlineCode],F2.[AirlineName] ,INVOICENO    " + Environment.NewLine;
            sql += " ORDER BY  [AirlineName] ) ,Tax as ( select ALC,BILLINGPERIOD,sum(f1.Taxamt) as TaxAmount from [Pax].[VW_SISCPN] f1 where BILLINGPERIOD  = '" + perido + "'  " + Environment.NewLine;
            sql += " group by ALC,BILLINGPERIOD ) select BALC,AirlineCode,AirlineName,f1.BILLINGPERIOD,CouponCount,GrossAmount,isnull(ISCAmount,'0.000') as ISCAmount  " + Environment.NewLine;
            sql += " ,isnull(TaxAmount,'0.000') as TaxAmount, (GrossAmount+isnull(TaxAmount,'0.00')-isnull(ISCAmount,'0.000') ) as NetAmount,[Invoice Number] ,null as XmlFile, null as Status " + Environment.NewLine;
            sql += " from Base f1 left join Tax f2 on f1.BALC = f2.ALC and f1.BILLINGPERIOD = f2.BILLINGPERIOD    " + Environment.NewLine;

            DataSet ds = new DataSet();
            ds = dbconnect.RetObjDS(sql);
            return ds;
        }

        public string DeleteSisBilling(string[] dataValue)
        {
            string sql = "DELETE FROM [Pax].[OutwardBillingTax] WHERE [ALC] like '" + dataValue[0] + "' and [BillingPeriod]  = '" + dataValue[1] + "';" + Environment.NewLine;
            sql = sql + "DELETE FROM [Pax].[OutwardBilling] WHERE [ALC] like '" + dataValue[0] + "' and [BillingPeriod]  = '" + dataValue[1] + "';" + Environment.NewLine;
            sql = sql + "DELETE FROM [OB].[ValidatePrime] WHERE [AirlineID] like '" + dataValue[0] + "' and [BillingPeriod]  = '" + dataValue[1] + "';" + Environment.NewLine;
            DataSet ds = new DataSet();
            ds = dbconnect.RetObjDS(sql);
            return "Records Deleted Successfully";
        }

        private string GetSystemParameter()
        {
            string Sql = "SELECT    [String1] as Airline ,[String2]as country ,[String3]  as Currency ,[String4]as AirlineNumericCode";
            Sql = Sql + " FROM [Adm].[GSP]";
            Sql = Sql + " where Parameter='SYS0001'";
            string xClient = "";
            try
            {
                DataSet ds = new DataSet();
                ds = dbconnect.RetObjDS(Sql);
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    xClient = dr[ds.Tables[0].Columns[0]].ToString();
                }

            }
            catch { }
            return xClient;
        }

        public DataSet LoadBillingData(DateTime FromDate, DateTime ToDate, string alc)
        {
            /*_fromDate = FromDate;
            _toDate = ToDate;
            _alcparam = alc;*/

            string[] row1;
            string codeAlc = null;
            string nameAl = null;
            decimal? amt = 0;
            int? numCpns = 0;

            string sqll = " select f2.AirlineID as Airline, A.AirlineName  ,sum(iif (f2.MOV_FinalShare is null, pr.FinalShare,f2.MOV_FinalShare)) as Finalshare   ,count(*)as CouponCount    " + Environment.NewLine;
            sqll += " from [Tmp].[OB_ValidatePrime] f2 join Ref.Airlines A on f2.AirlineID=A.AirlineID     " + Environment.NewLine;
            sqll += "  left join pax.OutwardBilling ob on ob.alc+ob.doc   = f2.RelatedDocumentNumber and ob.CPN = f2.CouponNumber     " + Environment.NewLine;
            sqll += " left join Pax.SalesRelatedDocumentInformation srd on srd.[RelatedDocumentNumber] = f2.[RelatedDocumentNumber]    " + Environment.NewLine;
            sqll += " left join pax.ProrationDetail pr on srd.RelatedDocumentGuid = pr.RelatedDocumentGuid and f2.CouponNumber = pr.CouponNumber and pr.ProrationFlag = f2.CouponStatus   " + Environment.NewLine;
            sqll += " where f2.[FlightDate]  between '" + FromDate + "' and '" + ToDate + "'  " + Environment.NewLine;
            sqll += "  and  f2.AirlineID is not null and f2.Validate = '1' and iif (f2.MOV_FinalShare is null, pr.FinalShare,f2.MOV_FinalShare) is not null    " + Environment.NewLine;
            sqll += " and f2.AirlineID like '" + alc + "' and ob.BILLINGPERIOD is null group by f2.AirlineID, A.AirlineName order by 1    " + Environment.NewLine;
            DataSet ds = new DataSet();
            try
            {
                ds = dbconnect.RetObjDS(sqll);
                int t = 0;
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    codeAlc = dr[ds.Tables[0].Columns[0]].ToString();
                    nameAl = dr[ds.Tables[0].Columns[1]].ToString();
                    amt = Convert.ToDecimal(dr[ds.Tables[0].Columns[2]].ToString());
                    numCpns = Convert.ToInt32(dr[ds.Tables[0].Columns[3]].ToString());
                    row1 = new string[] { codeAlc, nameAl, amt.Value.ToString(), numCpns.ToString() };
                    /*dbgListInv.Rows.Add(row1);
                    dbgListInv.Rows[dbgListInv.Rows.Count - 1].Cells[4].Value = statusImageList.Images[1];*/
                    t++;
                }
            }
            catch
            {
            }
            return ds;
        }

        private void UpdateBillingPeriod(string airlineid, string lblbillingPeriod)
        {
            try
            {
                string sql = "update [OB].[ValidatePrime] set [BillingPeriod] = '" + lblbillingPeriod + "'   " + Environment.NewLine;
                sql += "  from [OB].[ValidatePrime] where airlineid like '" + airlineid + "' and validate ='1'";
                DataSet ds = new DataSet();
                ds = dbconnect.RetObjDS(sql);
            }
            catch
            { }

        }

        public void btnProrateClick(string[] dataValue)
        {
            string mthclr = "20" + dataValue[0] + dataValue[1];
            int u1 = 0;
            var dbgListInv = LoadBillingData(Convert.ToDateTime(dataValue[2]), Convert.ToDateTime(dataValue[3]), dataValue[4]);

            foreach (DataRow dr in dbgListInv.Tables[0].Rows)
            {
                if (dr[dbgListInv.Tables[0].Columns[0]].ToString() != "")
                {
                    UpdateProrationFlag(dr[dbgListInv.Tables[0].Columns[0]].ToString());
                }
            }

            //DisplayReslt(dataValue[0], dataValue[1], dataValue[3]);
            /*int u = 0;

            for (int i = 0; i < dgv.RowCount; i++)
            {
                if ((dgv[0, i].Value != ""))
                {
                    arr[u] = dgv[0, i].Value.ToString();
                    u++;
                }

            }

            string[] arr2 = arr.Where(s => !string.IsNullOrWhiteSpace(s)).Distinct().ToArray();

            PRORATIONENGINE.ProrationBatchProratecs frm = new PRORATIONENGINE.ProrationBatchProratecs();

            try
            {
                frm.InvisibleOBProrate(arr2, mthclr);
                frm = null;
            }
            catch
            {
                frm = null;
            }*/

        }

        private void UpdateProrationFlag(string airlineid)
        {

            try
            {
                string sql = "update [OB].[ValidatePrime] set [ProratedFlag] = '1'   " + Environment.NewLine;
                sql += "  from [OB].[ValidatePrime] where airlineid like '" + airlineid + "' and validate ='1'";
                DataSet ds = new DataSet();
                ds = dbconnect.RetObjDS(sql);
            }
            catch
            {

            }

        }

        /*public DataSet DisplayReslt(string yy, string mm, string alcparam)
        {

            string mthclr = "20" + yy + mm;

            string sl = "ltrim((SELECT [String1] FROM [Adm].[GSP] where Parameter = 'SYS0013'))";
            int j = 4;
            DateTime x = DateTime.Now;
            DateTime y = DateTime.Now;

            string skl = "exec [Pax].[SP_OutwardBilling_OBValidated]'" + alcparam + "','" + x.ToString("dd-MMM-yyyy") + "','" + y.ToString("dd-MMM-yyyy") + "','" + mthclr + "'" + Environment.NewLine;
            skl += ", " + j + "," + sl + "";
            DataSet ds = new DataSet();
            ds = dbconnect.RetObjDS(skl);

            return ds;

        }*/
        public DataSet DisplayReslt(string dattFrom, string dateTo, string alc, int check)
        {

            string sl = "ltrim((SELECT [String1] FROM [Adm].[GSP] where Parameter = 'SYS0013'))";
            string skl = "exec [Pax].[SP_OutwardBilling_OBValidated]'" + alc + "','" + dattFrom + "','" + dateTo + "','" + dateTo + "'" + Environment.NewLine;
            skl += ", " + check + "," + sl + "";
            DataSet ds = new DataSet();
            ds = dbconnect.RetObjDS(skl);
            return ds;

        }

        public bool CheckBillingPeriod(string dataValue)
        {
            bool Billingperiodflag = false;

            string sql = "";
            sql += "IF OBJECT_ID('tempdb..#tmp_Date') IS NOT NULL DROP TABLE #tmp_Date select [From],[To] " + Environment.NewLine;
            sql += ",cast (DATEADD(day,-7,[From]) as Date) as StartDate,cast (DATEADD(day,7,[To]) as Date) as EndDate " + Environment.NewLine;
            sql += ", cast (GETDATE() as Date) as Today into #tmp_Date from Ref.IATACalendar where BILLINGPERIOD ='" + dataValue + "' " + Environment.NewLine;
            sql += "select * from #tmp_Date f1 where today between StartDate and EndDate " + Environment.NewLine;

            DataSet ds = new DataSet();
            ds = dbconnect.RetObjDS(sql);

            if (ds.Tables[0].Rows.Count >= 1)
            {
                Billingperiodflag = true;
            }
            return Billingperiodflag;
        }

        public ActionResult OBViewer(string dataValue)
        {
            string sql = "";
            sql += " select f1.[BILLINGPERIOD],f1.[ALC] ,f1.[DOC] ,f1.[CPN],f1.[CHK] ,f1.[PMC],(f1.[SECTORFROM]+' - '+f1.[SECTORTO]) as SECTOR,f1.[AMOUNT]  " + Environment.NewLine;
            sql += " ,f1.[ISC],f1.[COMM],f1.[FLIGHT],FORMAT(f1.[FLTDATE],'dd-MMM-yyyy') as FLTDATE,f1.[ETKTSAC],f1.[Taxamt] as TotalTaxAmt ,f1.[INVOICENO],f1.[NETAMT]  " + Environment.NewLine;
            sql += " ,[Pax].[fn_OutwardBilling_Tax_Combined](f1.alc+f1.DOC,f1.CPN) as TaxDetails from Pax.VW_SISCPN f1 where BILLINGPERIOD = '" + dataValue + "' order by alc,doc,cpn  " + Environment.NewLine;

            DataSet ds = new DataSet();
            ds = dbconnect.RetObjDS(sql);
            return PartialView(ds);
        }

        public string UpdateOBViewer(string[] dataValue)
        {

            string Sql = "";
            Sql = Sql += "IF NOT EXISTS(SELECT * FROM [Pax].[OutwardBillingTax] WHERE  [DOC] = '" + dataValue[2] + "' AND [CPN]  = '" + dataValue[3] + "' AND tax ='" + dataValue[5].Trim() + "')";
            Sql = Sql + " BEGIN" + Environment.NewLine;
            // Sql = Sql + "ELSE"; ;
            Sql = Sql + " INSERT INTO [Pax].[OutwardBillingTax]([ALC],[DOC],[CPN],[CHK],[TAX],[TAXAMT],[BillingPeriod],[ManualFlag]) ";
            Sql = Sql + " VALUES(";
            Sql = Sql + "'" + dataValue[1].Trim() + "',";
            Sql = Sql + "'" + dataValue[2].Trim() + "',";
            Sql = Sql + "'" + dataValue[3].Trim() + "',";
            Sql = Sql + "'" + dataValue[4].Trim() + "',";
            Sql = Sql + "'" + dataValue[5].Trim() + "',";
            Sql = Sql + "'" + dataValue[6].Trim() + "',";
            Sql = Sql + "'" + dataValue[0].Trim() + "',";
            Sql = Sql + "'M'";
            Sql = Sql + " )" + Environment.NewLine;
            //  Sql = Sql + "ELSE";
            Sql = Sql + " END ELSE  BEGIN" + Environment.NewLine;
            // Sql = Sql + "ELSE"; ;
            Sql = Sql + " UPDATE [Pax].[OutwardBillingTax]";
            Sql = Sql + " SET [TAX] ='" + dataValue[5].Trim() + "',";
            Sql = Sql + " [TAXAMT] ='" + dataValue[6].Trim() + "'";
            Sql = Sql + " WHERE  [DOC] = '" + dataValue[2].Trim().ToString() + "' AND [CPN]  = '" + dataValue[3].Trim().ToString() + "'  and ALC ='" + dataValue[1].ToString().Trim() + "' and [TAX] ='" + dataValue[5].Trim() + "'" + Environment.NewLine;
            Sql = Sql + " END";

            DataSet ds = new DataSet();
            ds = dbconnect.RetObjDS(Sql);
            return "Record Save Sucessfully";
        }

        public string DeleteOBViewer(string[] dataValue)
        {
            try
            {
                string sql = "DELETE FROM [Pax].[OutwardBillingTax] WHERE  [DOC] = '" + dataValue[0] + "' AND [CPN]  = '" + dataValue[1] + "' AND tax ='" + dataValue[2].Trim() + "'";
                DataSet ds = new DataSet();
                ds = dbconnect.RetObjDS(sql);

            }
            catch
            {
            }
            return "Record Save Sucessfully";
        }

        #region Re-generate
        public ActionResult ReGenerate(string dataValue, string dataValue1)
        {
            DataSet ds = new DataSet();
            string sql = "";
            string response = "";
            bool tmp = false;
            if (dataValue == "")
                sql = "Select distinct BILLINGPERIOD  FROM [Pax].[OutwardBilling]  order by BILLINGPERIOD Desc ";
            else
            {
                if (dataValue1 != "" && dataValue1 != "All")
                {
                    sql = "select AirlineName from Ref.Airlines where AirlineID = '" + dataValue1 + "' ";
                    tmp = true;
                }
                else
                    sql = "select distinct ALC from Pax.OutwardBilling where BILLINGPERIOD = '" + dataValue + "' order by ALC";
            }
            ds = dbconnect.RetObjDS(sql);
            if (tmp)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    response = dr[ds.Tables[0].Columns[0].ColumnName].ToString();
                }
                return Content(response);
            }
            else
                return PartialView("OalCarrier", ds);
        }

        public string DeleteOutWardBilling(string[] dataValue)
        {
            string sql = "DELETE FROM [Pax].[OutwardBillingTax] WHERE [ALC] like '" + dataValue[0] + "' and [BillingPeriod]  = '" + dataValue[1] + "';" + Environment.NewLine;
            sql = sql + "DELETE FROM [Pax].[OutwardBilling] WHERE [ALC] like '" + dataValue[0] + "' and [BillingPeriod]  = '" + dataValue[1] + "';" + Environment.NewLine;
            sql = sql + "DELETE FROM [OB].[ValidatePrime] WHERE [AirlineID] like '" + dataValue[0] + "' and [BillingPeriod]  = '" + dataValue[1] + "';" + Environment.NewLine;
            DataSet ds = new DataSet();
            ds = dbconnect.RetObjDS(sql);
            return "Records Deleted Successfully";
        }

        #endregion
    }
}