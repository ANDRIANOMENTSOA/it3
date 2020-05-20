using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.Data;
using System.Drawing;

using System.Text;
using System.Data.SqlClient;
using System.IO;
using System.Configuration;

using System.Text.RegularExpressions;
using Microsoft.VisualBasic;
using System.Collections;


namespace WebApplication1.Models
{
    public class Proration
    {
       // public string pbConnectionString = "Server=DESKTOP-CGR76E3;Database=OnsiteBiatss_KK;User Id=sa; Password=1234";
        public string pbConnectionString = "Server=DESKTOP-54APPF8\\SYMPHONY2;Database=OnsiteBiatss_KK;User Id=sa; Password=1234";
        ConnexionSQLServer.DbConnection dbconnect = new ConnexionSQLServer.DbConnection();
        public string ConvertToUSD(string Period, string Currency, string Amt, SqlConnection cs)
        {
            string USD = "";

            string s = "SELECT [USDRate]" + Environment.NewLine;
            s = s + "FROM [Ref].[CurrencyRate]" + Environment.NewLine;
            s = s + "WHERE [Period] = '" + (Convert.ToDouble(Period) - 1).ToString() + "' AND [Currency] = '" + Currency + "'" + Environment.NewLine;

            if (cs.State == ConnectionState.Open)
            {
                cs.Close();
            }

            SqlCommand cmd = new SqlCommand(s, cs);
            cs.Open();
            SqlDataReader rd = cmd.ExecuteReader();

            while (rd.Read())
            {
                USD = rd.GetValue(0).ToString();

            }

            rd.Close();
            cs.Close();

            return USD;

        }

        public double valid(string a)
        {
            double x = string.IsNullOrEmpty(a)
                                          ? 0
                                          : Convert.ToDouble(a);
            return x;
        }

        public string Total(string[,] dg, int cel)
        {
            string sum = "0";

            for (int i = 1; i < 6; i++)
            {
                if (dg[cel, i] != null && !string.IsNullOrWhiteSpace(dg[cel, i].ToString()) && dg[cel, i] != "")
                {

                    sum = (Convert.ToDouble(sum) + Convert.ToDouble(dg[cel, i])).ToString();
                }


            }

            return sum;
        }

    }
}
