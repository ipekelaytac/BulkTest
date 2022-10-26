using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

namespace BulkTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string result = VeriGetir("https://query1.finance.yahoo.com/v7/finance/download/TRYUSD=X?period1=1104710400&period2=1587168000&interval=1d&events=history");
            DataTable dt = new DataTable();
            dt.Columns.Add("Date");
            dt.Columns.Add("Open");
            dt.Columns.Add("High");
            dt.Columns.Add("Low");
            dt.Columns.Add("Close");
            dt.Columns.Add("AdjClose");
            dt.Columns.Add("Volume");
            string[] satirlar = Regex.Split(result, "\n");
            string query = "";
            DateTime baslangic = DateTime.Now;
            for (int i = 1; i < satirlar.Length; i++)
            {
                string[] row = Regex.Split(satirlar[i], ",");
                if (row[3].ToString() == "null") continue;
                DataRow dr = dt.NewRow();
                dr["Date"] = row[0];
                dr["Open"] = row[1];
                dr["High"] = row[2];
                dr["Low"] = row[3];
                dr["Close"] = row[4];
                dr["AdjClose"] = row[5];
                dr["Volume"] = row[6];
                dt.Rows.Add(dr);
                //query = $"insert borsa(Date, [Open], High, Low, [Close], AdjClose, Volume) values('{row[0]}', '{row[1]}', '{row[2]}', '{row[3]}', '{row[4]}', '{row[5]}', '{row[5]}')";
                //Kaydet(query);
            }
            BKaydet(dt);
            DateTime bitis = DateTime.Now;

        }
        private string VeriGetir(string url)
        {
            WebClient wc = new WebClient();
            return wc.DownloadString(url);
        }
        private void Kaydet(string sql)
        {
            SqlConnection con = new SqlConnection("server =213.238.178.120; database = demirtest; uid = demir; pwd = 1690Dd*_;");
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = sql;
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
        }
        private void BKaydet(DataTable dt)
        {
            SqlConnection con = new SqlConnection("server =213.238.178.120; database = demirtest; uid = demir; pwd = 1690Dd*_;");
            SqlBulkCopy bulk = new SqlBulkCopy(con);
            bulk.DestinationTableName = "borsa";
            con.Open();
            bulk.WriteToServer(dt);
            con.Close();
        }
    }
}
