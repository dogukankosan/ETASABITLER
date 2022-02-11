using DevExpress.XtraEditors;
using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Windows.Forms;

namespace ETASABITLER
{
    public partial class Form1 : DevExpress.XtraEditors.XtraForm
    {
        private readonly SqlConnection myConnection = new SqlConnection(File.ReadAllText("database.txt"));
        public Form1()
        {
            InitializeComponent();
        }
        void DatabaseList(string text)
        {
            SqlDataAdapter da = new SqlDataAdapter(text, myConnection);
            DataSet ds = new DataSet();
            da.Fill(ds);
            gridControl1.DataSource = ds.Tables[0];
        }
        void Update()
        {
            if (!(string.IsNullOrEmpty(textEdit1.Text)) && !(string.IsNullOrEmpty(textEdit2.Text)))
            {
                for (byte i = 0; i < gridView1.RowCount; i++)
                {
                    try
                    {
                        myConnection.Open();
                        SqlCommand command = new SqlCommand(" use [" + gridView1.GetRowCellValue(i, "ISLEME KATILACAK VERITABANLARI").ToString() + "] update SABITLER SET SABITSTR='" + textEdit1.Text + "' where SABITKONU='1019'", myConnection);
                        command.ExecuteNonQuery();
                        myConnection.Close();

                        myConnection.Open();
                        SqlCommand command2 = new SqlCommand(" use [" + gridView1.GetRowCellValue(i, "ISLEME KATILACAK VERITABANLARI").ToString() + "] update SABITLER SET SABITSTR='" + textEdit2.Text + "' where SABITKONU='1020'", myConnection);
                        command2.ExecuteNonQuery();
                        myConnection.Close();
                    }
                    catch (Exception exception)
                    {
                        XtraMessageBox.Show(string.Concat(exception.Message, "  VERİ TABANINDA HATA BU VERİ TABANI İŞLEMİ ATLANIYOR !! ", gridView1.GetRowCellValue(i, "VERI TABANI ADI")), "HATA İLE KARŞILAŞILDI !!", MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                        i++;
                        continue;
                    }
                }
                XtraMessageBox.Show("GÜNCELLEME İŞLEMİ BİTTİ !!", "BAŞARILI", MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                textEdit1.Text = null;
                textEdit2.Text = null;
            }
            else
            {
                XtraMessageBox.Show("METİN KUTULARINI BOŞ GEÇMEYİNİZ !!", "BOŞ GEÇME", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                textEdit2.Focus();
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            DatabaseList("select name as 'ISLEME KATILACAK VERITABANLARI' from sys.databases where name like 'ETA%' and name <> 'ETA_MASTERV8'");
        }
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            Update();
        }
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F2)
                Update();
        }
    }
}