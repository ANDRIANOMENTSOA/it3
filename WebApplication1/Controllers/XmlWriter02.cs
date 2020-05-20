using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using System.Data.SqlClient;
using System.IO.Compression;

namespace WebApplication1.Controllers
{
    public class XmlWriter02
    {

        #region Declaration
        public string SisFolder = "";
        public string fileNamePrefix = "";
        public string BillingAirlineCode = "";
        public string XmlError = "";
        public string XMLFileName = "";

        private string _Lino;
        private string _CPN;
        private string _ALCbilled;
        private string _ALC;
        private string _PMC;
        private string _Document;
        private string _chk;
        private string _sector;
        private string _from;
        private string _to;
        private string _amount;
        private string _ISCpercent;
        private string _commission;
        private string _net;
        private string _tax1Name;
        private string _tax2Name;
        private string _tax3Name;
        private string _tax4Name;
        private string _tax5Name;
        private string _tax6Name;
        private string _tax1Val;
        private string _tax2Val;
        private string _tax3Val;
        private string _tax4Val;
        private string _tax5Val;
        private string _tax6Val;
        private string _totalTax;
        private string _flight;
        private string _flightDate;
        private string _cur;
        private string _Etkt;
        private string _cpnTotalTax;
        private int LineDetailCounter = 0;
        private int RecordWithinBatch = 0;
        private int _lineNumber;

        private string _invoice;
        private string _billingAirlineCode;
        private string _BilledArl;
        private string _xmlError;
        private string PvBillingARL = "";
        private decimal _totAmount;

        const string ISXMLSCHEMA_INVOICE = "Symphony.XMLSchemas.IATA_IS_XML_Invoice_Standard_V3.2.1.xml";
        const string ISXMLSCHEMA_DATATYPES = "Symphony.XMLSchemas.IATA_IS_XML_Standard_Base_Datatypes_V3.2.1.xml";
        const string ISXMLSCHEMA_MAINDICTIONARY = "Symphony.XMLSchemas.IATA_IS_XML_Standard_Main_Dictionary_V3.2.1.xml";
        const string ISXMLSCHEMA_CUSTOMDICTIONARY = "Symphony.XMLSchemas.IATA_IS_XML_Standard_Custom_Dictionary_V3.2.1.xml";

        public string pbConnectionString = "Server=DESKTOP-Q821GFS;Database=OnsiteBiatss_KK;User Id=sa; Password=1234";

        #endregion
        private XmlWriter writer;
        //===================================================================================================


        public void xmlGenetator(string agBilledArl, string agBperiod, string agBillingArl, string InvoiceNumber, string OuputFolder)
        {
            PvBillingARL = agBillingArl;
            SisFolder = OuputFolder;

            if (agBilledArl.Trim() == string.Empty)
            {
                /*
                SqlDataReader AirlinesToBill = Readers.DataRead(
                                    "select distinct balc from OutwardBilling  order by balc",
                                    true);*/

                SqlConnection connection = new SqlConnection(pbConnectionString);

                SqlCommand commande = new SqlCommand(
                  "select distinct balc from OutwardBilling  order by balc;",
                 connection);

                connection.Open();

                SqlDataReader AirlinesToBill = commande.ExecuteReader();


                AirlinesToBill.Read();
                do
                {
                    WriteToXML(AirlinesToBill.GetValue(0).ToString(), agBperiod);
                    System.Threading.Thread.Sleep(1000);
                } while (AirlinesToBill.Read());

                AirlinesToBill.Close();


            }
            else
                WriteToXML(agBilledArl, agBperiod);


        }

        private void WriteToXML(string BilledArl, string BillingPeriod)
        {
            #region Header & doc creation

            XmlWriterSettings xmlWriterSettings = new XmlWriterSettings();
            xmlWriterSettings.Encoding = Encoding.ASCII;

            xmlWriterSettings.Indent = true;
            xmlWriterSettings.IndentChars = "  ";
            xmlWriterSettings.NewLineChars = "\r\n";
            xmlWriterSettings.NewLineHandling = NewLineHandling.Replace;

            _billingAirlineCode = PvBillingARL; //TODO for test only
            _BilledArl = BilledArl.Trim();

            //string fileNamePrefix = "CT-";
            string fileNamePrefix = string.Empty;

            string FilePath = SisFolder;
            //string fileName = @"C:\SIS Processing\OUTFOLDER\" + fileNamePrefix + "PXMLF-" +
            //                  _billingAirlineCode + "20" + BillingPeriod +
            //                  DateTime.Now.ToString("yyyyMMddHHmmss") + ".xml";
            //string ZipfileName = @"C:\SIS Processing\OUTFOLDER\" + fileNamePrefix + "PXMLF-" +
            //                     _billingAirlineCode + "20" + BillingPeriod +
            //                     DateTime.Now.ToString("yyyyMMddHHmmss") + ".zip";

            string fileName = FilePath + fileNamePrefix + "PXMLF-" + PvBillingARL + "20" + BillingPeriod + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xml";
            string ZipfileName = FilePath + fileNamePrefix + "PXMLF-" + PvBillingARL + "20" + BillingPeriod + DateTime.Now.ToString("yyyyMMddHHmmss") + ".zip";

            XMLFileName = fileName;



            string cdir = "";// CompletedFolder;


            string[] subFolder = FilePath.Split('\\');
            cdir = "";
            for (int i = 0; i < subFolder.Length; i++)
            {
                cdir = cdir + subFolder[i].ToUpper() + "\\";

                DirectoryInfo C_dir = new DirectoryInfo(cdir);
                if (!C_dir.Exists)
                {
                    C_dir.Create();
                }
            }

            writer = XmlWriter.Create(fileName, xmlWriterSettings);


            writer.WriteStartDocument();

            XML_header(); //Create transmission header
            string transDate = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fffffff%K");
            XML_transmissionHeader(transDate, _billingAirlineCode, "Passenger");
            //

            #endregion

            #region invoice header

            writer.WriteStartElement("Invoice");

            writer.WriteStartElement("InvoiceHeader"); //invoice header
            writer.WriteElementString("InvoiceNumber",
                                      GetInvoiceNumber(BilledArl, BillingPeriod));

            string newDate = DateTime.Now.ToString("yyyy-MM-dd").ToString();
            writer.WriteElementString("InvoiceDate", newDate);
            writer.WriteElementString("InvoiceType", "Invoice");
            writer.WriteElementString("ChargeCategory", "Pax Non Sample");

            #region Seller Organisation

            writer.WriteStartElement("SellerOrganization"); //seller
            writer.WriteElementString("OrganizationID", _billingAirlineCode);
            writer.WriteElementString("OrganizationDesignator",
                                      AirlineDesignator(_billingAirlineCode));
            string xair = AirlineName(_billingAirlineCode).Trim();
            if (xair.Length > 50)
            {

                writer.WriteElementString("OrganizationName1", xair.Substring(0, 50));
                writer.WriteElementString("OrganizationName2", xair.Substring(50, xair.Length - 50));

            }

            else { writer.WriteElementString("OrganizationName1", AirlineName(_billingAirlineCode)); }

            string sellerTaxReg = AirlineTaxRegistrationID(_billingAirlineCode);
            if (sellerTaxReg != null && sellerTaxReg != "")
            {
                writer.WriteElementString("TaxRegistrationID", AirlineTaxRegistrationID(_billingAirlineCode));
            }

            string sellerAddTaxReg = AirlineTaxAddTaxRegistrationID(_billingAirlineCode);
            if (sellerAddTaxReg != null && sellerAddTaxReg != "")
            {
                writer.WriteElementString("AdditionalTaxRegistrationID", AirlineTaxAddTaxRegistrationID(_billingAirlineCode));
            }


            string sellerCoReg = AirlineCompanyRegistrationID(_billingAirlineCode);
            if (sellerCoReg != null && sellerCoReg != "")
            {
                writer.WriteElementString("CompanyRegistrationID", AirlineCompanyRegistrationID(_billingAirlineCode));
            }



            writer.WriteStartElement("Address"); //address

            string addressline1 = string.Empty;
            string addressline2 = string.Empty;
            string addressline3 = string.Empty;

            addressline1 = AirlineAdd1(_billingAirlineCode);
            addressline2 = AirlineAdd2(_billingAirlineCode);
            addressline3 = AirlineAdd3(_billingAirlineCode);

            if (addressline1 != string.Empty && addressline2 == string.Empty && addressline3 != string.Empty)
            {
                addressline2 = addressline3;
                addressline3 = string.Empty;
            }

            if (addressline1 == string.Empty && addressline2 != string.Empty && addressline3 == string.Empty)
            {
                addressline1 = addressline2;
                addressline2 = string.Empty;
                addressline3 = string.Empty;
            }

            if (addressline1 == string.Empty && addressline2 == string.Empty && addressline3 != string.Empty)
            {
                addressline1 = addressline3;
                addressline2 = string.Empty;
                addressline3 = string.Empty;
            }

            string cityName = AirlineCity(_billingAirlineCode);

            if (addressline1 == string.Empty)
            {
                if (cityName != string.Empty)
                    addressline1 = cityName;
                else
                    addressline1 = "NA";
            }

            writer.WriteElementString("AddressLine1", addressline1);

            if (addressline2 != string.Empty)
                writer.WriteElementString("AddressLine2", addressline2);

            if (addressline3 != string.Empty)
                writer.WriteElementString("AddressLine3", addressline3);



            if (cityName != string.Empty)
            {
                writer.WriteElementString("CityName", AirlineCity(_billingAirlineCode));
            }
            else
            {
                writer.WriteElementString("CityName", "NA");
            }

            string countryCode = AirlineCountryCode(_billingAirlineCode);

            if (countryCode != string.Empty)
            {
                writer.WriteElementString("CountryCode", countryCode);
            }
            else
            {
                writer.WriteElementString("CountryCode", "NA");
            }

            string CountryName = AirlineCountryName(_billingAirlineCode);

            if (countryCode != string.Empty)
            {
                writer.WriteElementString("CountryName", CountryName);
            }
            else
            {
                writer.WriteElementString("CountryName", "NA");
            }



            string sellerPostal = AirlinePostalCode(_billingAirlineCode);
            if (sellerPostal != null && sellerPostal != "")
            {
                writer.WriteElementString("PostalCode", AirlinePostalCode(_billingAirlineCode));
            }


            writer.WriteEndElement(); //address


            writer.WriteEndElement(); //seller 

            #endregion

            #region Buyer  organisation

            writer.WriteStartElement("BuyerOrganization"); //buyer

            writer.WriteElementString("OrganizationID", _BilledArl);
            writer.WriteElementString("OrganizationDesignator",
                                      AirlineDesignator(_BilledArl));

            string bair = AirlineName(_BilledArl).Trim();
            if (bair.Length > 50)
            {

                writer.WriteElementString("OrganizationName1", bair.Substring(0, 50));
                writer.WriteElementString("OrganizationName2", bair.Substring(50, bair.Length - 50));

            }

            else { writer.WriteElementString("OrganizationName1", AirlineName(_BilledArl)); }



            string BuyerTaxReg = AirlineTaxRegistrationID(_BilledArl);
            if (BuyerTaxReg != null && BuyerTaxReg != "")
            {
                writer.WriteElementString("TaxRegistrationID", AirlineTaxRegistrationID(_BilledArl));
            }

            string BuyerAddTaxReg = AirlineTaxAddTaxRegistrationID(_BilledArl);
            if (BuyerAddTaxReg != null && BuyerAddTaxReg != "")
            {
                writer.WriteElementString("AdditionalTaxRegistrationID", AirlineTaxAddTaxRegistrationID(_BilledArl));
            }

            string BuyerCoReg = AirlineCompanyRegistrationID(_BilledArl);
            if (BuyerCoReg != null && BuyerCoReg != "")
            {
                writer.WriteElementString("CompanyRegistrationID", AirlineCompanyRegistrationID(_BilledArl));
            }

            writer.WriteStartElement("Address"); //address

            addressline1 = AirlineAdd1(_BilledArl);
            addressline2 = AirlineAdd2(_BilledArl);
            addressline3 = AirlineAdd3(_BilledArl);

            if (addressline1 != string.Empty && addressline2 == string.Empty && addressline3 != string.Empty)
            {
                addressline2 = addressline3;
                addressline3 = string.Empty;
            }

            if (addressline1 == string.Empty && addressline2 != string.Empty && addressline3 == string.Empty)
            {
                addressline1 = addressline2;
                addressline2 = string.Empty;
                addressline3 = string.Empty;
            }

            if (addressline1 == string.Empty && addressline2 == string.Empty && addressline3 != string.Empty)
            {
                addressline1 = addressline3;
                addressline2 = string.Empty;
                addressline3 = string.Empty;
            }

            cityName = AirlineCity(_BilledArl);

            if (addressline1 == string.Empty)
            {
                if (cityName != string.Empty)
                    addressline1 = cityName;
                else
                    addressline1 = "NA";
            }

            writer.WriteElementString("AddressLine1", addressline1);

            if (addressline2 != string.Empty)
                writer.WriteElementString("AddressLine2", addressline2);

            if (addressline3 != string.Empty)
                writer.WriteElementString("AddressLine3", addressline3);

            if (cityName != string.Empty)
            {
                writer.WriteElementString("CityName", AirlineCity(_BilledArl));
            }
            else { writer.WriteElementString("CityName", "NA"); }

            countryCode = AirlineCountryCode(_BilledArl);

            if (countryCode != string.Empty)
            {
                writer.WriteElementString("CountryCode", countryCode);
            }
            else { writer.WriteElementString("CountryCode", "NA"); }


            CountryName = AirlineCountryName(_BilledArl);

            if (countryCode != string.Empty)
            {
                writer.WriteElementString("CountryName", CountryName);
            }
            else
            {
                writer.WriteElementString("CountryName", "NA");
            }

            string BuyerPostal = AirlinePostalCode(_BilledArl);
            if (BuyerPostal != null && BuyerPostal != "")
            {
                writer.WriteElementString("PostalCode", AirlinePostalCode(_BilledArl));

            }



            writer.WriteEndElement(); //address


            writer.WriteEndElement(); //buyer 

            #endregion

            #region Payment terms

            writer.WriteStartElement("PaymentTerms"); //Payment terms

            writer.WriteElementString("CurrencyCode", "USD");
            //TODO check if this field neeeds to be dynamic
            writer.WriteElementString("ClearanceCurrencyCode", "USD");
            writer.WriteElementString("ExchangeRate", "1.000");

            writer.WriteElementString("SettlementMonthPeriod", BillingPeriod);
            //TODO to be obtained from header
            writer.WriteElementString("SettlementMethod", GetSettlementMethod(_BilledArl));

            writer.WriteEndElement(); //paymentTerms  

            #endregion

            #region ISdetails

            writer.WriteStartElement("ISDetails");
            writer.WriteElementString("DigitalSignatureFlag", "D");
            writer.WriteEndElement(); //Isdetail 

            #endregion

            writer.WriteEndElement(); //InvoiceHeader   

            #endregion

            #region LineItem
            // Sudhir 09012014
            //SqlDataReader lineItemsDR = 
            //           Readers.DataRead(
            //                     "select distinct balc,pmc from pax.OutwardBilling where bALC ='" +
            //                     _BilledArl.Trim() + "' order by pmc", true);


            SqlConnection connection = new SqlConnection(pbConnectionString);

            SqlCommand cmdLineItem = new SqlCommand(
               "select distinct balc,pmc from pax.OutwardBilling where bALC ='" +
                              _BilledArl.Trim() + "' and BillingPeriod='" + BillingPeriod + "' order by pmc",
             connection);

            connection.Open();

            SqlDataReader lineItemsDR = cmdLineItem.ExecuteReader();

            /*SqlDataReader lineItemsDR =
                    Readers.DataRead(
                              "select distinct balc,pmc from pax.OutwardBilling where bALC ='" +
                              _BilledArl.Trim() + "' and BillingPeriod='" + BillingPeriod + "' order by pmc", true);*/

            lineItemsDR.Read();



            int LineItemNumber = 0;

            string ChargeCode = string.Empty;

            do
            {
                ChargeCode = lineItemsDR.GetValue(1).ToString();

                writer.WriteStartElement("LineItem");

                LineItemNumber++;
                writer.WriteElementString("LineItemNumber", LineItemNumber.ToString());
                writer.WriteElementString("ChargeCode", ChargeCode);
                writer.WriteElementString("ChargeAmount",
                                          ChargeAmt(_BilledArl, ChargeCode,
                                                    BillingPeriod));
                //TODO sum(CouponGrossValue)

                if (TaxAmt(_BilledArl, ChargeCode, BillingPeriod) != string.Empty)
                {
                    writer.WriteStartElement("Tax");
                    writer.WriteElementString("TaxType", "Tax");

                    //writer.WriteStartElement("TaxAmount");
                    //writer.WriteAttributeString("Name", "Billed");
                    //writer.WriteString(TaxAmt(_BilledArl, ChargeCode));
                    ////TODO  ckeck what to feed  ,must compute total values of all taxes
                    //writer.WriteEndElement(); //tax amount

                    writer.WriteElementString("TaxAmount",
                                              TaxAmt(_BilledArl, ChargeCode,
                                                     BillingPeriod));

                    //SqlDataReader lineItemTax = Readers.DataRead(string.Format("select  T.TAX,SUM(T.TaxAmt) as TAXAmount from VW_SISCPN v inner join OutwardBillingTax T ON v.ALC=T.ALC and v.DOC=T.DOC and v.CPN=T.CPN and v.CHK=T.CHK where v.balc='{0}' and PMC={1} group by T.TAX",_BilledArl,ChargeCode).ToString(), true);
                    //lineItemTax.Read();

                    //do
                    //{
                    //writer.WriteStartElement("TaxBreakdown"); //tax bk
                    //writer.WriteElementString("TaxCode", lineItemTax.GetValue(0).ToString()); 
                    //writer.WriteElementString("TaxAmount", lineItemTax.GetValue(1).ToString()); 
                    //writer.WriteEndElement( ); //tax bk     
                    //} while (lineItemTax.Read());


                    writer.WriteEndElement(); //tax
                }

                //writer.WriteStartElement("Tax"); //Tax
                //writer.WriteElementString("TaxType", "VAT");

                //writer.WriteStartElement("TaxAmount");
                //writer.WriteAttributeString("Name", "Billed");
                //writer.WriteString("0.000");
                //writer.WriteEndElement( ); //tax amount


                //writer.WriteStartElement("TaxBreakdown"); //tax bk
                //writer.WriteElementString("TaxLabel", "VAT"); //TODO check
                //writer.WriteElementString("TaxIdentifier", "-"); //TODO check
                //writer.WriteElementString("TaxableAmount", "0.000"); //TODO check
                //writer.WriteElementString("TaxPercent", "0.00"); //TODO check
                //writer.WriteElementString("TaxAmount", "0.000"); //TODO check
                //writer.WriteElementString("TaxText", "NA"); //TODO check
                //writer.WriteEndElement(); //tax bk

                //writer.WriteEndElement(); //tax


                writer.WriteStartElement("AddOnCharges"); //AddOnCharges 1 
                writer.WriteElementString("AddOnChargeName", "ISCAllowed");
                writer.WriteElementString("AddOnChargeAmount",
                                          ISCamt(_BilledArl, ChargeCode, BillingPeriod));
                writer.WriteEndElement();

                /*
      writer.WriteStartElement("AddOnCharges"); //AddOnCharges 2 
      writer.WriteElementString("AddOnChargeName", "OtherCommissionAllowed");
      writer.WriteElementString("AddOnChargeAmount", "0.000");
      writer.WriteEndElement(); // 2 (OtherCommissionAllowed)

      writer.WriteStartElement("AddOnCharges"); //AddOnCharges 3  
      writer.WriteElementString("AddOnChargeName", "UATPAllowed");
      writer.WriteElementString("AddOnChargeAmount", "0.000");

      writer.WriteEndElement(); // 3 (UATPAllowed)


      writer.WriteStartElement("AddOnCharges"); //AddOnCharges 4  
      writer.WriteElementString("AddOnChargeName", "HandlingFeeAllowed");
      writer.WriteElementString("AddOnChargeAmount", "0.000");
      writer.WriteEndElement(); //4 (HandlingFeeAllowed)
       */

                writer.WriteElementString("TotalNetAmount",
                                          TotatnetAmt(_BilledArl, ChargeCode,
                                                      BillingPeriod));
                //TODO to compute
                writer.WriteElementString("DetailCount",
                                          LineItemCount(_BilledArl, ChargeCode,
                                                        BillingPeriod));
                //TODO to compute

                writer.WriteEndElement(); //invoice   

            } while (lineItemsDR.Read());

            lineItemsDR.Close();

            #endregion

            //=============================================================================================================================               



            /*lineItemsDR =
                      Readers.DataRead(
                                "select distinct balc,pmc from pax.OutwardBilling where bALC ='" +
                                _BilledArl.Trim() + "' and BillingPeriod='" + BillingPeriod + "' order by pmc", true);*/


            SqlCommand cmd = new SqlCommand(
                 "select distinct balc,pmc from pax.OutwardBilling where bALC ='" +
                                _BilledArl.Trim() + "' and BillingPeriod='" + BillingPeriod + "' order by pmc",
                connection);

            connection.Open();

             lineItemsDR = cmd.ExecuteReader();



            connection.Open();





            _lineNumber = 0;
            lineItemsDR.Read();

            do
            {
                ChargeCode = lineItemsDR.GetValue(1).ToString();

                /*  SqlDataReader xmlFeeder =
                            Readers.DataRead(
                                      "select * from pax.VW_SISCPN where BALC ='" +
                                      _BilledArl.Trim() +
                                      "' and PMC=" + ChargeCode + " and BillingPeriod='" +
                                      BillingPeriod + "'", true);*/

                SqlCommand cmdxmlFeeder = new SqlCommand(
               "select * from pax.VW_SISCPN where BALC ='" +_BilledArl.Trim() +"' and PMC=" + ChargeCode + " and BillingPeriod='" + BillingPeriod + "'", connection);

                connection.Open();

                SqlDataReader xmlFeeder = cmdxmlFeeder.ExecuteReader();


                xmlFeeder.Read();

                RecordWithinBatch = 0;
                LineDetailCounter = 0;
                _lineNumber++;


                do
                {
                    //_Lino = xmlFeeder.GetValue(0).ToString();
                    _ALCbilled = xmlFeeder.GetValue(0).ToString();
                    _ALC = xmlFeeder.GetValue(1).ToString();
                    _Document = xmlFeeder.GetValue(2).ToString();
                    _CPN = xmlFeeder.GetValue(3).ToString();
                    _chk = xmlFeeder.GetValue(4).ToString();
                    _PMC = xmlFeeder.GetValue(5).ToString();
                    _from = xmlFeeder.GetValue(6).ToString();
                    _to = xmlFeeder.GetValue(7).ToString();
                    _amount = xmlFeeder.GetValue(8).ToString();
                    _ISCpercent = xmlFeeder.GetValue(9).ToString();
                    _commission = xmlFeeder.GetValue(10).ToString();
                    _flight = xmlFeeder.GetValue(11).ToString();
                    _flightDate = ((DateTime)xmlFeeder.GetValue(12)).ToString("yyyy-MM-dd");
                    _Etkt = xmlFeeder.GetValue(13).ToString();

                    _cpnTotalTax = xmlFeeder.GetValue(14).ToString();
                    _net = xmlFeeder.GetValue(17).ToString();

                    //_totalTax = xmlFeeder.GetValue(16).ToString( );
                    //_flight = xmlFeeder.GetValue(17).ToString( );

                    //_cur = xmlFeeder.GetValue(19).ToString( );

                    LineDetailCounter++;
                    RecordWithinBatch++;
                    XML_linedetail(BillingPeriod);

                } while (xmlFeeder.Read());

                xmlFeeder.Close();

            } while (lineItemsDR.Read());

            lineItemsDR.Close();

            string SummAmount = string.Empty;
            string SummComm = string.Empty;
            string SummTax = string.Empty;
            string SummNet = string.Empty;

            writer.WriteStartElement("InvoiceSummary");
            writer.WriteElementString("LineItemCount", _lineNumber.ToString());
            writer.WriteElementString("TotalLineItemAmount",
                                      TotalGrossAmt(_BilledArl, BillingPeriod));

            //writer.WriteStartElement("Tax"); //Tax
            //writer.WriteElementString("TaxType", "VAT");

            //writer.WriteStartElement("TaxAmount");
            //writer.WriteAttributeString("Name", "Billed");
            //writer.WriteString("0.000");
            //writer.WriteEndElement( ); //tax amount

            //writer.WriteStartElement("TaxBreakdown"); //tax bk
            //writer.WriteElementString("TaxLabel", "VAT"); //TODO check
            //writer.WriteElementString("TaxIdentifier", "-"); //TODO check
            //writer.WriteElementString("TaxableAmount", "0.000"); //TODO check
            //writer.WriteElementString("TaxPercent", "0.00"); //TODO check
            //writer.WriteElementString("TaxAmount", "0.000"); //TODO check
            //writer.WriteElementString("TaxText", "NA"); //TODO check
            //writer.WriteEndElement( ); //tax bk

            //writer.WriteEndElement( ); //tax

            GetInvoiceSummary(_BilledArl, out SummAmount, out SummComm, out SummTax,
                              out SummNet, BillingPeriod);

            writer.WriteStartElement("AddOnCharges"); //AddOnCharges 1 
            writer.WriteElementString("AddOnChargeName", "ISCAllowed");
            writer.WriteElementString("AddOnChargeAmount", "-" + SummComm);
            writer.WriteEndElement();

            /*
  writer.WriteStartElement("AddOnCharges"); //AddOnCharges 2 
  writer.WriteElementString("AddOnChargeName", "OtherCommissionAllowed");
  writer.WriteElementString("AddOnChargeAmount", "0.000");
  writer.WriteEndElement( ); // 2 (OtherCommissionAllowed)

  writer.WriteStartElement("AddOnCharges"); //AddOnCharges 3  

  writer.WriteElementString("AddOnChargeName", "UATPAllowed");
  writer.WriteElementString("AddOnChargeAmount", "0.000");

  writer.WriteEndElement( ); // 3 (UATPAllowed)


  writer.WriteStartElement("AddOnCharges"); //AddOnCharges 4  
  writer.WriteElementString("AddOnChargeName", "HandlingFeeAllowed");
  writer.WriteElementString("AddOnChargeAmount", "0.000");
  writer.WriteEndElement( ); //4 (HandlingFeeAllowed)
   */

            writer.WriteElementString("TotalTaxAmount", SummTax);
            writer.WriteElementString("TotalAmountWithoutVAT", SummNet);
            writer.WriteElementString("TotalAmount", SummNet);
            writer.WriteElementString("TotalAmountInClearanceCurrency", SummNet);
            writer.WriteElementString("LegalText", "SIS Certification");

            #region transmission summarry

            writer.WriteEndElement();

            writer.WriteEndElement(); // Invoice

            writer.WriteStartElement("TransmissionSummary");
            writer.WriteElementString("InvoiceCount", "1"); // 

            writer.WriteStartElement("TotalAmount");
            writer.WriteAttributeString("CurrencyCode", "USD");
            writer.WriteString(TotatnetAmt(_BilledArl, BillingPeriod)); // 
            writer.WriteEndElement();

            #endregion

            writer.WriteEndDocument();
            writer.Close();


            // Check XML Schema

            _xmlError = string.Empty;

            //ValidateISXMLFile(fileName, ISXMLSCHEMA_INVOICE, ISXMLSCHEMA_DATATYPES, ISXMLSCHEMA_MAINDICTIONARY, ISXMLSCHEMA_CUSTOMDICTIONARY);

            if (_xmlError == string.Empty)
            {
                // Zip File

                ZipStorer zip;

                zip = ZipStorer.Create(ZipfileName, "Generated by Symphony");
                zip.AddFile(ZipStorer.Compression.Deflate, fileName,
                            System.IO.Path.GetFileName(fileName), "");
                zip.Close();

                System.IO.File.Delete(fileName);
            }
            else
            {
                TextWriter errFile = new StreamWriter(Path.GetDirectoryName(fileName) + "\\" + Path.GetFileNameWithoutExtension(fileName) + ".err");
                errFile.WriteLine(_xmlError);
                errFile.Close();

            }
        }

        private void FeedTable()
        {

            //x_invoiceLineTableAdapter.Connection.ConnectionString = DBconnect.connection;
            //x_invoiceLineTableAdapter.Fill(this.biatss_kkDataSet1.X_invoiceLine);

            //dbgXML.Refresh();



        }

        private void XML_linedetail(string BillingPeriod)
        {

            writer.WriteStartElement("LineItemDetail"); //LineItemDetail

            writer.WriteElementString("DetailNumber", LineDetailCounter.ToString());
            writer.WriteElementString("LineItemNumber", _lineNumber.ToString());
            writer.WriteElementString("BatchSequenceNumber", _lineNumber.ToString());
            writer.WriteElementString("RecordSequenceWithinBatch",
                                      RecordWithinBatch.ToString());

            writer.WriteStartElement("ChargeAmount");
            writer.WriteAttributeString("Name", "GrossBilled");
            writer.WriteString(LineChargeAmount(_Document, _CPN, _ALC, BillingPeriod));
            //TODO remove hardcode
            writer.WriteEndElement(); //LineItemDetail

            //TAX//

            if (_cpnTotalTax != string.Empty)
            {
                writer.WriteStartElement("Tax"); //Tax
                writer.WriteElementString("TaxType", "Tax");

                writer.WriteStartElement("TaxAmount"); //TaxAmount
                writer.WriteAttributeString("Name", "Billed");
                writer.WriteString(_cpnTotalTax); // X

                writer.WriteEndElement(); //TaxAmount



                //--------------------------------------------------------



                /*SqlDataReader xmlFeederTax =
                          Readers.DataRead(
                                    string.Format(
                                              "select tax,taxamt from Pax.OutwardBillingTax where ALC='{0}' and DOC='{1}' and CHK='{2}' and CPN='{3}'",
                                              _ALC, _Document, _chk, _CPN), true);*/

                SqlConnection connection = new SqlConnection(pbConnectionString);

                SqlCommand cmdxmlFeederTax = new SqlCommand(
               string.Format("select tax,taxamt from Pax.OutwardBillingTax where ALC='{0}' and DOC='{1}' and CHK='{2}' and CPN='{3}'", _ALC, _Document, _chk, _CPN),
             connection);

                connection.Open();

                SqlDataReader xmlFeederTax = cmdxmlFeederTax.ExecuteReader();



                xmlFeederTax.Read();
                do
                {

                    //#region Tax breakdown 1
                    writer.WriteStartElement("TaxBreakdown"); //TaxBreakdown 1

                    writer.WriteElementString("TaxCode",
                                              xmlFeederTax.GetValue(0).ToString());
                    //A

                    writer.WriteStartElement("TaxAmount"); //TaxAmount
                    writer.WriteAttributeString("Name", "Billed");
                    writer.WriteString(xmlFeederTax.GetValue(1).ToString());
                    //TODO remove hardcode
                    writer.WriteEndElement();

                    writer.WriteEndElement(); //TaxBreakdown 1 
                    //#endregion

                } while (xmlFeederTax.Read());

                xmlFeederTax.Close();

                ////#region tax breakdown2
                //writer.WriteStartElement("TaxBreakDown");//TaxBreakdown 2

                //writer.WriteElementString("TaxCode", _tax1Name);// B 
                ////TODO suum A + B = x Populate if more taxes involved
                //writer.WriteStartElement("TaxAmount");
                //writer.WriteAttributeString("Name", "Billed");
                //writer.WriteString(_tax1Val);//TODO remove hard code

                //writer.WriteEndElement( );//TaxBreakdown 2 
                ////#endregion


                //writer.WriteEndElement( );//Tax
                writer.WriteEndElement();
                //TAX 2//
            }

            //writer.WriteStartElement("Tax"); //Tax
            //writer.WriteElementString("TaxType", "VAT");

            //writer.WriteStartElement("TaxAmount");
            //writer.WriteAttributeString("Name", "Billed");
            //writer.WriteString("0.000");
            //writer.WriteEndElement( ); //tax amount

            //writer.WriteStartElement("TaxBreakdown"); //tax bk
            //writer.WriteElementString("TaxLabel", "VAT"); //TODO check
            //writer.WriteElementString("TaxIdentifier", "-"); //TODO check
            //writer.WriteElementString("TaxableAmount", "0.000"); //TODO check
            //writer.WriteElementString("TaxPercent", "0.00"); //TODO check
            //writer.WriteElementString("TaxAmount", "0.000"); //TODO check
            //writer.WriteElementString("TaxText", "NA"); //TODO check
            //writer.WriteEndElement( ); //tax bk

            //writer.WriteEndElement( ); //tax


            ////TODO check if condition must be inserted here
            //writer.WriteStartElement("Tax");//Tax 

            //          writer.WriteElementString("TaxType", "VAT");
            //          writer.WriteStartElement("TaxAmount");
            //          writer.WriteAttributeString("Name", "Billed");
            //          writer.WriteString("");//TODO remove hardcode

            //          writer.WriteEndElement();//Tax 


            //          //--------------------------------------------------------

            //          //#region TaxBreakdown
            //          writer.WriteStartElement("TaxBreakdown");//TaxBreakdown 
            //          writer.WriteElementString("TaxLabel", "VAT");
            //          writer.WriteElementString("TaxIdentifier", "GF");//TODO should be 1 of the following val: GF,TA,AS,OC,HF,OT ref to RAM A9 p98
            //          writer.WriteElementString("TaxableAmount", "100");//TODO remove hard code
            //          writer.WriteElementString("TaxPercent", "18.00");//TODO remove hard code

            //          writer.WriteStartElement("TaxAmount");//Tax.Amount
            //           writer.WriteAttributeString("Name", "Billed");
            //          writer.WriteString("18");//TODO remove hard code
            //          writer.WriteEndElement();//Tax.Amount
            //          writer.WriteElementString("TaxText", "VAT on GF");//TODO remove hard code
            //                                                            //TODO see SIS ram p98 Note V30 Vat text
            //          //#endregion
            //          //writer.WriteEndElement();// tax breakdown

            //          //writer.WriteEndElement();//Tax




            //          //ADDON CHARGES//  
            //          writer.WriteEndElement();
            //          writer.WriteEndElement();

            #region AddOnCharges

            writer.WriteStartElement("AddOnCharges"); //AddOnCharges 1 
            writer.WriteElementString("AddOnChargeName", "ISCAllowed");
            writer.WriteElementString("AddOnChargePercentage", "-" + _ISCpercent);
            writer.WriteElementString("AddOnChargeAmount", "-" + _commission);
            writer.WriteEndElement(); //AddOnCharges

            #endregion

            /*

                                        //#region AddOnCharges
                              writer.WriteStartElement("AddOnCharges");//AddOnCharges 2 ()
                                        writer.WriteElementString("AddOnChargeName", "OtherCommissionAllowed");
                                        writer.WriteElementString("AddOnChargePercentage", "0.00");
                                        writer.WriteElementString("AddOnChargeAmount", "0.000");
                              writer.WriteEndElement( );// 2
                              //#endregion

                              //--------------------------------------------------------

                              //#region AddOnCharges
                              writer.WriteStartElement("AddOnCharges");//AddOnCharges 3 ()
                              writer.WriteElementString("AddOnChargeName", "UATPAllowed");
                              writer.WriteElementString("AddOnChargePercentage", "0.00");
                              writer.WriteElementString("AddOnChargeAmount", "0.000");
                              writer.WriteEndElement( );
                              //#endregion

                           
                              //#region AddOnCharges
                              writer.WriteStartElement("AddOnCharges");//AddOnCharges 4 ()
                              writer.WriteElementString("AddOnChargeName", "HandlingFeeAllowed");
                              writer.WriteElementString("AddOnChargeAmount", "0.000");
                              writer.WriteEndElement( );
                              //#endregion

                               */

            writer.WriteElementString("TotalNetAmount", _net); //TODO computation here


            //COUPON DETAILS//

            #region CouponDetails

            writer.WriteStartElement("CouponDetails"); //CouponDetails
            writer.WriteElementString("TicketOrFIMIssuingAirline", _ALC);
            writer.WriteElementString("TicketOrFIMCouponNumber", _CPN);
            writer.WriteElementString("TicketDocOrFIMNumber", _Document);
            writer.WriteElementString("CheckDigit", _chk);
            writer.WriteElementString("CurrAdjustmentIndicator", "USD");
            writer.WriteElementString("ElectronicTicketIndicator", "E");
            writer.WriteElementString("AirlineFlightDesignator",
                                      AirlineDesignator(_billingAirlineCode));
            //TODO airline flight deg

            if (_flight != string.Empty)
                writer.WriteElementString("FlightNo", _flight);

            writer.WriteElementString("FlightDate", _flightDate.Substring(0, 10));
            writer.WriteElementString("FromAirportCode", _from);
            writer.WriteElementString("ToAirportCode", _to);
            //writer.WriteElementString("HandlingFeeType", "NA");//TODO possible values A,C,S,b Refer P92 RAM

            if (_Etkt != string.Empty)
                writer.WriteElementString("SettlementAuthorizationCode", _Etkt);
            //writer.WriteElementString("CabinClass", "Y");


            writer.WriteStartElement("Attachment"); //Attachment
            writer.WriteElementString("AttachmentIndicatorOriginal", "N");
            writer.WriteEndElement(); //Attachment
            writer.WriteEndElement();

            #endregion


            //--------------------------------------------------------


            writer.WriteEndElement();



        }

        private void XML_header()
        {
            //writer.WriteStartDocument();

            writer.WriteStartElement("InvoiceTransmission",
                                     "http://www.IATA.com/IATAAviationInvoiceStandard");
            writer.WriteAttributeString("xsi", "schemaLocation",
                                        "http://www.w3.org/2001/XMLSchema-instance",
                                        "http://www.IATA.com/IATAAviationInvoiceStandard http://www.iata.org/whatwedo/finance/clearing/sis/Documents/schemas/IATA_IS_XML_Invoice_Standard_V3.2.xsd");
            //writer.WriteAttributeString("xmlns", "http://www.IATA.com/IATAAviationInvoiceStandard");
            writer.WriteAttributeString("xmlns", "xsi", null,
                                        "http://www.w3.org/2001/XMLSchema-instance");





        }

        private void XML_transmissionHeader(string transmissionDateTime, string IssOrg,
                                            string BillCategory)
        {
            writer.WriteStartElement("TransmissionHeader");
            writer.WriteElementString("TransmissionDateTime", transmissionDateTime);
            writer.WriteElementString("Version", "IATA:ISXMLInvoiceV3.2");
            writer.WriteElementString("TransmissionID", Guid.NewGuid().ToString());
            writer.WriteElementString("IssuingOrganizationID", IssOrg);
            writer.WriteElementString("BillingCategory", "Passenger");
            writer.WriteEndElement();
        }

        private string GetBillingPeriod()
        {

            String billingPeriod = null;

            /*billingPeriod =
                      Readers.ScalarRead(
                                "select isnull(max(BillingPeriod),'') from OutwardBilling").
                                ToString();*/

            SqlConnection conn = new SqlConnection(pbConnectionString);
            string sql = "select isnull(max(BillingPeriod),'') from OutwardBilling";
            SqlCommand cmd = new SqlCommand(sql, conn);

            billingPeriod = cmd.ExecuteScalar().ToString();

            return billingPeriod;

        }

        private string GetInvoiceNumber(string _BilledArl, string BillingPeriod)
        {

            String invoiceNumber = null;
            /*invoiceNumber =
                      Readers.ScalarRead(
                                "select distinct invoiceno from pax.OutwardBilling where balc='" +
                                _BilledArl + "' and BillingPeriod='" + BillingPeriod + "'").
                                ToString();*/

            SqlConnection conn = new SqlConnection(pbConnectionString);
            string sql = "select distinct invoiceno from pax.OutwardBilling where balc='" +
                                _BilledArl + "' and BillingPeriod='" + BillingPeriod + "'";

            SqlCommand cmd = new SqlCommand(sql, conn);

            invoiceNumber = cmd.ExecuteScalar().ToString();


            return invoiceNumber;

        }

        private string AirlineDesignator(string airlineID)
        {

            String airlineDesig = null;

           /* airlineDesig =
                      Readers.ScalarRead(
                                "select AirlineCode from Ref.Airlines where AirlineID = '" +
                                airlineID + "'").ToString();*/

            SqlConnection conn = new SqlConnection(pbConnectionString);
            string sql = "select AirlineCode from Ref.Airlines where AirlineID = '" +
                                airlineID + "'";
            SqlCommand cmd = new SqlCommand(sql, conn);

            airlineDesig = cmd.ExecuteScalar().ToString();

            return airlineDesig;
        }

        private string AirlineName(string airlineID)
        {

            String airlineName = null;
            /* airlineName =
                       Readers.ScalarRead(
                                 "select AirlineName from Ref.Airlines where AirlineID = '" +
                                 airlineID + "'").ToString();*/

            SqlConnection conn = new SqlConnection(pbConnectionString);
            string sql = "select AirlineName from Ref.Airlines where AirlineID = '" +
                                 airlineID + "'";
            SqlCommand cmd = new SqlCommand(sql, conn);

            airlineName = cmd.ExecuteScalar().ToString();


            return airlineName;


        }
        private string AirlineTaxRegistrationID(string airlineID)
        {

            String airlineTaxRegistrationID = null;

            /*airlineTaxRegistrationID =
                      Readers.ScalarRead(
                                "select TaxRegistrationID from Ref.Airlines where AirlineID = '" +
                                airlineID + "'").ToString();*/

            SqlConnection conn = new SqlConnection(pbConnectionString);
            string sql = "select TaxRegistrationID from Ref.Airlines where AirlineID = '" +
                                airlineID + "'";
            SqlCommand cmd = new SqlCommand(sql, conn);

            airlineTaxRegistrationID = cmd.ExecuteScalar().ToString();

            return airlineTaxRegistrationID;


        }
        private string AirlineCompanyRegistrationID(string airlineID)
        {

            String airlineCompanyRegistrationID = null;

            /* airlineCompanyRegistrationID =
                       Readers.ScalarRead(
                                 "select CompanyRegistrationID from Ref.Airlines where AirlineID = '" +
                                 airlineID + "'").ToString();*/

            SqlConnection conn = new SqlConnection(pbConnectionString);
            string sql = "select CompanyRegistrationID from Ref.Airlines where AirlineID = '" +
                                 airlineID + "'";
            SqlCommand cmd = new SqlCommand(sql, conn);

            airlineCompanyRegistrationID = cmd.ExecuteScalar().ToString();


            return airlineCompanyRegistrationID;


        }

        private string AirlineTaxAddTaxRegistrationID(string airlineID)
        {

            String airlineTaxAdditionalRegistrationID = null;

            /*airlineTaxAdditionalRegistrationID =
                      Readers.ScalarRead(
                                "select AdditionalTaxRegistrationID from Ref.Airlines where AirlineID = '" +
                                airlineID + "'").ToString();*/

            SqlConnection conn = new SqlConnection(pbConnectionString);
            string sql = "select AdditionalTaxRegistrationID from Ref.Airlines where AirlineID = '" +
                                airlineID + "'";
            SqlCommand cmd = new SqlCommand(sql, conn);

            airlineTaxAdditionalRegistrationID = cmd.ExecuteScalar().ToString();

            return airlineTaxAdditionalRegistrationID;


        }

        private string AirlineAdd1(string airlineID)
        {

            String airlineAdd = null;

            /*airlineAdd =
                      Readers.ScalarRead(
                                "select AddressL1 from Ref.Airlines where AirlineID = '" +
                                airlineID + "'").ToString();*/

            SqlConnection conn = new SqlConnection(pbConnectionString);
            string sql = "select AddressL1 from Ref.Airlines where AirlineID = '" +
                                airlineID + "'";
            SqlCommand cmd = new SqlCommand(sql, conn);

            airlineAdd = cmd.ExecuteScalar().ToString();

            return airlineAdd;


        }

        private string AirlineAdd2(string airlineID)
        {

            String airlineAdd = null;
            /* airlineAdd =
                       Readers.ScalarRead(
                                 "select AddressL2 from Ref.Airlines where AirlineID = '" +
                                 airlineID + "'").ToString();*/

            SqlConnection conn = new SqlConnection(pbConnectionString);
            string sql = "select AddressL2 from Ref.Airlines where AirlineID = '" +
                                 airlineID + "'";
            SqlCommand cmd = new SqlCommand(sql, conn);

            airlineAdd = cmd.ExecuteScalar().ToString();

            return airlineAdd;


        }

        private string AirlineAdd3(string airlineID)
        {

            String airlineAdd = null;

            /*airlineAdd =
                      Readers.ScalarRead(
                                "select AddressL3 from Ref.Airlines where AirlineID = '" +
                                airlineID + "'").ToString();*/

            SqlConnection conn = new SqlConnection(pbConnectionString);

            string sql = "select AddressL3 from Ref.Airlines where AirlineID = '" +
                                airlineID + "'";
            SqlCommand cmd = new SqlCommand(sql, conn);

            airlineAdd = cmd.ExecuteScalar().ToString();


            return airlineAdd;


        }

        private string AirlineCity(string airlineID)
        {

            String airlineAdd = null;


           /* airlineAdd =
                      Readers.ScalarRead(
                                "select cityName from Ref.Airlines where AirlineID = '" +
                                airlineID + "'").ToString();*/

            SqlConnection conn = new SqlConnection(pbConnectionString);

            string sql = "select cityName from Ref.Airlines where AirlineID = '" +
                                airlineID + "'";
            SqlCommand cmd = new SqlCommand(sql, conn);

            airlineAdd = cmd.ExecuteScalar().ToString();



            return airlineAdd.Trim();

        }

        private string AirlineCountryCode(string airlineID)
        {

            String airlineAdd = null;

            /* airlineAdd =
                       Readers.ScalarRead(
                                 "select countryCode from Ref.Airlines where AirlineID = '" +
                                 airlineID + "'").ToString();*/

            SqlConnection conn = new SqlConnection(pbConnectionString);
            string sql = "select countryCode from Ref.Airlines where AirlineID = '" +
                                 airlineID + "'";
            SqlCommand cmd = new SqlCommand(sql, conn);

            airlineAdd = cmd.ExecuteScalar().ToString();

            return airlineAdd;

        }

        private string AirlineCountryName(string airlineID)
        {

            string airlineAdd = null;

            /* airlineAdd =
                       Readers.ScalarRead("select CountryName from Ref.Airlines where AirlineID = '" + airlineID + "'").ToString();*/

            SqlConnection conn = new SqlConnection(pbConnectionString);
            string sql = "select CountryName from Ref.Airlines where AirlineID = '" + airlineID + "'";

            SqlCommand cmd = new SqlCommand(sql, conn);

            airlineAdd = cmd.ExecuteScalar().ToString();

            if (airlineAdd == "")
            {
                airlineAdd = "NA";
            }

            return airlineAdd;

        }

        private string AirlinePostalCode(string airlineID)
        {

            string airlineAdd = null;

            /* airlineAdd =
                       Readers.ScalarRead("select PostalCode from Ref.Airlines where AirlineID = '" + airlineID + "'").ToString();*/

            SqlConnection conn = new SqlConnection(pbConnectionString);

            string sql = "select PostalCode from Ref.Airlines where AirlineID = '" + airlineID + "'";

            SqlCommand cmd = new SqlCommand(sql, conn);

            airlineAdd = cmd.ExecuteScalar().ToString();

            //if (airlineAdd == "")
            //{
            //    airlineAdd = "NA";
            //}

            return airlineAdd;

        }

        private string ChargeAmt(string _BilledArl, string ChargeCode, string BillingPeriod)
        {
            string amt;

            /*amt =
                      Readers.ScalarRead(
                                "select SUM( AMOUNT)    from pax.VW_SISCPN where BALC = '" +
                                _BilledArl + "' and PMC=" + ChargeCode +
                                " and BillingPeriod='" + BillingPeriod + "'").ToString();*/

            SqlConnection conn = new SqlConnection(pbConnectionString);
            string sql = "select SUM( AMOUNT)    from pax.VW_SISCPN where BALC = '" +
                                _BilledArl + "' and PMC=" + ChargeCode +
                                " and BillingPeriod='" + BillingPeriod + "'";
            SqlCommand cmd = new SqlCommand(sql, conn);

            amt = cmd.ExecuteScalar().ToString();

            return amt;

        }

        private string TaxAmt(string _BilledArl, string ChargeCode, string BillingPeriod)
        {
            string amt;

            /* amt =
                       Readers.ScalarRead(
                                 "select SUM( Taxamt)  from pax.VW_SISCPN where BALC = '" +
                                 _BilledArl + "' and PMC=" + ChargeCode +
                                 " and BillingPeriod='" + BillingPeriod + "'").ToString();*/

            SqlConnection conn = new SqlConnection(pbConnectionString);
            string sql = "select SUM( Taxamt)  from pax.VW_SISCPN where BALC = '" +
                                 _BilledArl + "' and PMC=" + ChargeCode +
                                 " and BillingPeriod='" + BillingPeriod + "'";
            SqlCommand cmd = new SqlCommand(sql, conn);

            amt = cmd.ExecuteScalar().ToString();

            return amt;

        }

        private string ISCamt(string _BilledArl, string ChargeCode, string BillingPeriod)
        {
            string amt;

            /* amt =
                       Readers.ScalarRead(
                                 "select  0-SUM(COMM)   from pax.VW_SISCPN where BALC = '" +
                                 _BilledArl + "' and PMC=" + ChargeCode +
                                 " and BillingPeriod='" + BillingPeriod + "'").ToString();*/

            SqlConnection conn = new SqlConnection(pbConnectionString);
            string sql = "select  0-SUM(COMM)   from pax.VW_SISCPN where BALC = '" +
                                 _BilledArl + "' and PMC=" + ChargeCode +
                                 " and BillingPeriod='" + BillingPeriod + "'";
            SqlCommand cmd = new SqlCommand(sql, conn);

            amt = cmd.ExecuteScalar().ToString();

            return amt;

        }

        private string TotatnetAmt(string _BilledArl, string ChargeCode, string BillingPeriod)
        {
            string amt;

            /* amt =
                       Readers.ScalarRead(
                                 "select   ( SUM(AMOUNT) -SUM(comm) ) + SUM(isnull(Taxamt,0))  from pax.VW_SISCPN where BALC = '" +
                                 _BilledArl + "' and PMC=" + ChargeCode +
                                 " and BillingPeriod='" + BillingPeriod + "'").ToString();*/

            SqlConnection conn = new SqlConnection(pbConnectionString);
            string sql = "select   ( SUM(AMOUNT) -SUM(comm) ) + SUM(isnull(Taxamt,0))  from pax.VW_SISCPN where BALC = '" +
                                 _BilledArl + "' and PMC=" + ChargeCode +
                                 " and BillingPeriod='" + BillingPeriod + "'";
            SqlCommand cmd = new SqlCommand(sql, conn);

            amt = cmd.ExecuteScalar().ToString();

            return amt;

        }

        private string TotatnetAmt(string _BilledArl, string BillingPeriod)
        {
            string amt;

            /*amt =
                      Readers.ScalarRead(
                                "select   ( SUM(AMOUNT) -SUM(comm) ) + SUM(isnull(Taxamt,0))  from pax.VW_SISCPN where BALC = '" +
                                _BilledArl + "'" + " and BillingPeriod='" + BillingPeriod +
                                "'").ToString();*/

            SqlConnection conn = new SqlConnection(pbConnectionString);
            string sql = "select   ( SUM(AMOUNT) -SUM(comm) ) + SUM(isnull(Taxamt,0))  from pax.VW_SISCPN where BALC = '" +
                                _BilledArl + "'" + " and BillingPeriod='" + BillingPeriod +
                                "'";
            SqlCommand cmd = new SqlCommand(sql, conn);

            amt = cmd.ExecuteScalar().ToString();

            return amt;

        }

        private string TotalGrossAmt(string _BilledArl, string BillingPeriod)
        {
            string amt;

            /* amt =
                       Readers.ScalarRead(
                                 "select   SUM(AMOUNT)  from pax.VW_SISCPN where BALC = '" +
                                 _BilledArl + "'" + " and BillingPeriod='" + BillingPeriod +
                                 "'").ToString();*/

            SqlConnection conn = new SqlConnection(pbConnectionString);
            string sql = "select   SUM(AMOUNT)  from pax.VW_SISCPN where BALC = '" +
                                 _BilledArl + "'" + " and BillingPeriod='" + BillingPeriod +
                                 "'";
            SqlCommand cmd = new SqlCommand(sql, conn);

            amt = cmd.ExecuteScalar().ToString();


            return amt;

        }

        private string GetSettlementMethod(string _BilledArl)
        {
            string SettlementMethod;

            /* SettlementMethod =
                       Readers.ScalarRead(
                                 "select   SettlementMethod  from Ref.Airlines where AirlineID = '" +
                                 _BilledArl + "'").ToString();*/

            SqlConnection conn = new SqlConnection(pbConnectionString);
            string sql = "select   SettlementMethod  from Ref.Airlines where AirlineID = '" +
                                 _BilledArl + "'";
            SqlCommand cmd = new SqlCommand(sql, conn);

            SettlementMethod = cmd.ExecuteScalar().ToString();

            return SettlementMethod;

        }

        private void GetInvoiceSummary(string _BilledArl, out string Amount, out string Comm,
                                       out string Tax, out string Net, string BillingPeriod)
        {
           /* SqlDataReader xmlFeeder =
                      Readers.DataRead(
                                "SELECT SUM(AMOUNT) as AMOUNT,SUM(COMM) as COMM, SUM(isnull(TAXAMT,0)) as TAX,SUM(AMOUNT)+SUM(isnull(TAXAMT,0))-SUM(COMM) as NET from pax.VW_SISCPN where BALC='" +
                                _BilledArl.Trim() + "'" + " and BillingPeriod='" +
                                BillingPeriod + "'", true);*/

            SqlConnection connection = new SqlConnection(pbConnectionString);
            SqlCommand cmd = new SqlCommand(
              "SELECT SUM(AMOUNT) as AMOUNT,SUM(COMM) as COMM, SUM(isnull(TAXAMT,0)) as TAX,SUM(AMOUNT)+SUM(isnull(TAXAMT,0))-SUM(COMM) as NET from pax.VW_SISCPN where BALC='" +
                                _BilledArl.Trim() + "'" + " and BillingPeriod='" +
                                BillingPeriod + "'",
             connection);

            connection.Open();

            SqlDataReader xmlFeeder = cmd.ExecuteReader();

            xmlFeeder.Read();
            Amount = xmlFeeder.GetValue(0).ToString();
            Comm = xmlFeeder.GetValue(1).ToString();
            Tax = xmlFeeder.GetValue(2).ToString();
            Net = xmlFeeder.GetValue(3).ToString();

            xmlFeeder.Close();
        }

        private string LineChargeAmount(string Document, string Coupon, string _Arl,
                                        string BillingPeriod)
        {
            string amt;

            /* amt =
                       Readers.ScalarRead("select AMOUNT  from pax.VW_SISCPN  where DOC = '" +
                                          Document + "'  and CPN = '" + Coupon + "'" +
                                          " and ALC='" + _Arl + "'" + " and BillingPeriod='" +
                                          BillingPeriod + "'").ToString();*/
            SqlConnection conn = new SqlConnection(pbConnectionString);
            string sql = "select AMOUNT  from pax.VW_SISCPN  where DOC = '" +
                                          Document + "'  and CPN = '" + Coupon + "'" +
                                          " and ALC='" + _Arl + "'" + " and BillingPeriod='" +
                                          BillingPeriod + "'";
            SqlCommand cmd = new SqlCommand(sql, conn);

            amt = cmd.ExecuteScalar().ToString();

            return amt;
        }

        private string LineItemCount(string _BilledArl, string ChargeCode, string BillingPeriod)
        {
            string amt;

            /*amt =
                      Readers.ScalarRead("select count(*)  from pax.VW_SISCPN  where BALC='" +
                                         _BilledArl + "' and PMC=" + ChargeCode +
                                         " and BillingPeriod='" + BillingPeriod + "'").
                                ToString();*/

            SqlConnection conn = new SqlConnection(pbConnectionString);
            string sql = "select count(*)  from pax.VW_SISCPN  where BALC='" + _BilledArl + "' and PMC=" + ChargeCode +" and BillingPeriod='" + BillingPeriod + "'";
            SqlCommand cmd = new SqlCommand(sql, conn);

            amt = cmd.ExecuteScalar().ToString();

            return amt;
        }

        private void ValidateISXMLFile(string ISXMLFileName, string ISXMLSCHEMA_INVOICE,
                                       string ISXMLSCHEMA_DATATYPES,
                                       string ISXMLSCHEMA_MAINDICTIONARY,
                                       string ISXMLSCHEMA_CUSTOMDICTIONARY)
        {
            try
            {
                FileStream xmlFileStream = File.OpenRead(ISXMLFileName);

                XmlTextReader isxmlInvoiceSchemaReader =
                          new XmlTextReader(
                                    System.Reflection.Assembly.GetExecutingAssembly().
                                              GetManifestResourceStream(
                                                        ISXMLSCHEMA_INVOICE));
                XmlTextReader isxmlDataTypesSchemaReader =
                          new XmlTextReader(
                                    System.Reflection.Assembly.GetExecutingAssembly().
                                              GetManifestResourceStream(
                                                        ISXMLSCHEMA_DATATYPES));
                XmlTextReader isxmlMainDictionarySchemaReader =
                          new XmlTextReader(
                                    System.Reflection.Assembly.GetExecutingAssembly().
                                              GetManifestResourceStream(
                                                        ISXMLSCHEMA_MAINDICTIONARY));
                XmlTextReader isxmlCustomDictionarySchemaReader =
                          new XmlTextReader(
                                    System.Reflection.Assembly.GetExecutingAssembly().
                                              GetManifestResourceStream(
                                                        ISXMLSCHEMA_CUSTOMDICTIONARY));

                XmlReaderSettings xmlSettings = new XmlReaderSettings();
                xmlSettings.Schemas = new System.Xml.Schema.XmlSchemaSet();

                xmlSettings.Schemas.Add(
                          "http://www.IATA.com/IATAAviationInvoiceStandard",
                          isxmlInvoiceSchemaReader);
                xmlSettings.Schemas.Add(
                          "http://www.IATA.com/IATAAviationStandardDataTypes",
                          isxmlDataTypesSchemaReader);
                xmlSettings.Schemas.Add(
                          "http://www.IATA.com/IATAAviationStandardMainDictionary",
                          isxmlMainDictionarySchemaReader);
                xmlSettings.Schemas.Add(
                          "http://www.IATA.com/IATAAviationStandardCustomDictionary",
                          isxmlCustomDictionarySchemaReader);

                xmlSettings.ValidationType = ValidationType.Schema;
                xmlSettings.ValidationEventHandler +=
                          new ValidationEventHandler(ValidationCallBack);

                XmlReader reader = XmlReader.Create(xmlFileStream, xmlSettings);
                // Parse the file.
                while (reader.Read()) ;

                reader.Close();
                xmlFileStream.Close();
            }
            catch (Exception ex)
            {
                if (_xmlError == string.Empty)
                    _xmlError = ex.Message;
                else
                {
                    _xmlError = _xmlError +
                                "\n\n" + ex.Message;
                }
            }
        }


        // Display any warnings or errors.
        private void ValidationCallBack(object sender, ValidationEventArgs args)
        {
            if (args.Severity == XmlSeverityType.Warning)
            {
                if (_xmlError == string.Empty)
                    _xmlError =
                              "Warning: Matching schema not found.  No validation occurred." +
                              args.Message;
                else
                {
                    _xmlError = _xmlError +
                                "\n\nWarning: Matching schema not found.  No validation occurred." +
                                args.Message;
                }
            }

            else
            {
                if (_xmlError == string.Empty)
                    _xmlError =
                              "Validation error: " + args.Message;
                else
                {
                    _xmlError = _xmlError +
                                "\n\nValidation error: " + args.Message;
                }
            }


        }

        //===================================================================================================
    }
}