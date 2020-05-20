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
    public class InadClass
    {
        //public string pbConnectionString = "Server=DESKTOP-CGR76E3;Database=OnsiteBiatss_KK;User Id=sa; Password=1234";
        public string pbConnectionString = "DESKTOP-54APPF8\\SYMPHONY2;Database=OnsiteBiatss_KK;User Id=sa; Password=1234";
        ConnexionSQLServer.DbConnection dbconnect = new ConnexionSQLServer.DbConnection();
        public string[] BreakINboundCarriage(string FCA, string a, string b, int choose)
        {

            string[] sp = new string[10];
            string a1 = b.Trim() + " " + a.Trim();
            string a2 = b.Trim() + " X/" + a.Trim();

            if (FCA.Contains(a1))
            {
                for (int o = 0; o < 10; o++)
                {
                    sp[o] = Split(FCA, a1, choose)[o];
                }
            }
            else
            {
                for (int o = 0; o < 10; o++)
                {
                    sp[o] = Split(FCA, a2, choose)[o];
                }
            }
            return sp;

        }

        public string[] Split(string FCA, string v1, int choose)
        {
            string[] sp = new string[10];
            string[] g = new string[] { v1 };
            string[] Fc = FCA.Split(g, StringSplitOptions.None);
            string c = Fc[0].ToString();

            string Secq = @"(((?!([A-Z]{6}))([A-Z]{2,3}))|((X/)([A-Z]{3}))|(1A )|(1B )|(1G )|(1S )|(2I )|(2J )|(2M )|(2V )|(3A )|(3E )|(3L )|(3M )|(3S )|(3U )|(3V )|(3X )|(4C )|(4H )|(4M )|(4Q )|(4S )|(4X )|(5C )|(5D )|(5J )|(5K )|(5L )|(5N )|(5T )|(5X )|(6A )|(6H )|(6R )|(7D )|(7F )|(7H )|(7I )|(7U )|(7W )|(8D )|(8M )|(8U )|(8V )|(9E )|(9H )|(9K )|(9U )|(9W )|(A3 )|(A6 )|(A9 )|(B6 )|(B7 )|(B8 )|(C5 )|(C9 )|(D5 )|(D6 )|(D9 )|(E0 )|(E6 )|(E7 )|(F2 )|(F7 )|(F9 )|(G3 )|(G4 )|(G7 )|(H2 )|(I5 )|(J2 )|(J8 )|(K5 )|(K6 )|(L7 )|(M3 )|(M5 )|(M6 )|(M7 )|(N3 )|(N8 )|(O9 )|(P5 )|(Q2 )|(R2 )|(R4 )|(S2 )|(S3 )|(S4 )|(S5 )|(S7 )|(T0 )|(U6 )|(U7 )|(U8 )|(U9 )|(V0 )|(V3 )|(V4 )|(W5 )|(W8 )|(W9 )|(X3 )|(Y4 )|(Y8 )|(Z2 )|(Z4 )|(Z5 )|(Z6 )|((//))|((/-)))";

            string xq = null;

            int i = 0;
            string[] y = new string[100];

            try
            {

                foreach (Match matSecq in Regex.Matches(c, Secq))
                {

                    xq = matSecq.Value.ToString();

                    y[i] = xq;
                    i++;

                }


            }
            catch
            {
            }

            int ll = i;

            string[] F = new string[ll + 2];
            for (int l = 0; l < i; l++)
            {
                if (y[l] != null)
                {
                    F[l] = y[l];
                }
            }

            F[i] = v1.Trim().Substring(0, 2);
            F[i + 1] = v1.Trim().Substring(v1.Length - 3);

            int arrlengh = F.Length;
            int exc = 0;

            for (int k = arrlengh - 1; k >= 0; k -= 2)
            {
                string sec = F[k - 2].Trim().ToString();

                if (!(sec.Contains("X/")))
                {
                    exc = k - 2;
                    break;
                }

            }

            int val = ((arrlengh - 1) - exc) / 2;

            if (choose == 0)
            {

                if (val == 1)
                {
                    sp[0] = F[exc].ToString().Substring(F[exc].Length - 3);
                    sp[1] = F[exc + 2].ToString().Substring(F[exc + 2].Length - 3);

                }
                else
                    if (val == 2)
                {
                    sp[0] = F[exc].ToString().Substring(F[exc].Length - 3); ;
                    sp[1] = F[exc + 2].ToString().Substring(F[exc + 2].Length - 3);
                    sp[2] = F[exc + 4].ToString().Substring(F[exc + 4].Length - 3);
                }
                else
                        if (val == 3)
                {
                    sp[0] = F[exc].ToString().Substring(F[exc].Length - 3); ;
                    sp[1] = F[exc + 2].ToString().Substring(F[exc + 2].Length - 3);
                    sp[2] = F[exc + 4].ToString().Substring(F[exc + 4].Length - 3);
                    sp[3] = F[exc + 6].ToString().Substring(F[exc + 6].Length - 3);
                }
                else
                            if (val == 4)
                {
                    sp[0] = F[exc].ToString().Substring(F[exc].Length - 3); ;
                    sp[1] = F[exc + 2].ToString().Substring(F[exc + 2].Length - 3);
                    sp[2] = F[exc + 4].ToString().Substring(F[exc + 4].Length - 3);
                    sp[3] = F[exc + 6].ToString().Substring(F[exc + 6].Length - 3);
                    sp[4] = F[exc + 8].ToString().Substring(F[exc + 8].Length - 3);
                }
            }
            else
                if (choose == 1)
            {
                if (val == 1)
                {

                    sp[0] = F[exc].ToString().Substring(F[exc].Length - 3);

                    sp[1] = F[exc + 1].ToString();

                    sp[2] = F[exc + 2].ToString().Substring(F[exc + 2].Length - 3);

                }
                else
                    if (val == 2)
                {
                    sp[0] = F[exc].ToString().Substring(F[exc].Length - 3);

                    sp[1] = F[exc + 1].ToString();

                    sp[2] = F[exc + 2].ToString().Substring(F[exc + 2].Length - 3);

                    sp[3] = F[exc + 3].ToString();

                    sp[4] = F[exc + 4].ToString().Substring(F[exc + 4].Length - 3);
                }
                else
                        if (val == 3)
                {
                    sp[0] = F[exc].ToString().Substring(F[exc].Length - 3);

                    sp[1] = F[exc + 1].ToString();

                    sp[2] = F[exc + 2].ToString().Substring(F[exc + 2].Length - 3);

                    sp[3] = F[exc + 3].ToString();

                    sp[4] = F[exc + 4].ToString().Substring(F[exc + 4].Length - 3);

                    sp[5] = F[exc + 5].ToString();

                    sp[5] = F[exc + 6].ToString().Substring(F[exc + 6].Length - 3);
                }
                else
                            if (val == 4)
                {
                    sp[0] = F[exc].ToString().Substring(F[exc].Length - 3);

                    sp[1] = F[exc + 1].ToString();

                    sp[2] = F[exc + 2].ToString().Substring(F[exc + 2].Length - 3);

                    sp[3] = F[exc + 3].ToString();

                    sp[4] = F[exc + 4].ToString().Substring(F[exc + 4].Length - 3);

                    sp[5] = F[exc + 5].ToString();

                    sp[6] = F[exc + 6].ToString().Substring(F[exc + 6].Length - 3);

                    sp[7] = F[exc + 7].ToString();

                    sp[8] = F[exc + 8].ToString().Substring(F[exc + 8].Length - 3);
                }
            }



            return sp;



        }

        public string[,] PopulateGrid(string[] arr, string[,] dg)
        {
            dg = new string[10, 10];
            int k = 0;
            int kk = 1;

            for (int i = 0; i < arr.Length; i++)
            {
                if (arr[i] != null)
                {
                    if (i % 2 == 0)
                    {

                        dg[0, k] = arr[i];
                        k++;
                    }
                    else
                    {

                        dg[1, kk] = arr[i];
                        kk++;

                    }
                }
            }
            return dg;
        }

        public string prorateFactor(string SecOrg, string SecDest, string DOI, SqlConnection cs)
        {
            string val = "0";

            string s = "SELECT [Factor]" + Environment.NewLine;
            s = s + "FROM [Ref].[PMPFactor]" + Environment.NewLine;
            s = s + "WHERE [OrgCity] = '" + SecOrg + "' AND [DestCity] = '" + SecDest + "' AND " + Environment.NewLine;
            s = s + "cast('" + DOI + "' as date) BETWEEN [ValidFrom] AND [ValidTo]" + Environment.NewLine;


            if (cs.State == ConnectionState.Open)
            {
                cs.Close();
            }

            SqlCommand cmd = new SqlCommand(s, cs);
            cs.Open();
            SqlDataReader rd = cmd.ExecuteReader();

            while (rd.Read())
            {
                val = rd.GetValue(0).ToString();

            }

            rd.Close();
            cs.Close();

            return val;

        }

        public string[,] PopulateProrateFactor(string[,] dg, string DOI, SqlConnection cs)
        {

            for (int i = 0; i < 8; i++)
            {
                if (dg[0, i] != null && !string.IsNullOrWhiteSpace(dg[0, i].ToString()) && dg[0, i] != "")
                {
                    if (dg[0, i + 1] != null && !string.IsNullOrWhiteSpace(dg[0, i + 1].ToString()) && dg[0, i + 1] != "")
                    {
                        string a = dg[0, i].ToString();
                        string b = dg[0, i + 1].ToString();


                        dg[2, i + 1] = prorateFactor(a, b, DOI, cs);

                    }

                }
            }
            return dg;

        }

        public void InsertStatement(string Sql, SqlConnection cs)
        {
            if (cs.State == ConnectionState.Open)
            {
                cs.Close();
            }

            cs.Open();

            SqlDataAdapter da = new SqlDataAdapter();
            da.InsertCommand = new SqlCommand(Sql, cs);

            da.InsertCommand.ExecuteNonQuery();

            cs.Close();
        }

    } 
}