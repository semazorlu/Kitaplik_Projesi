using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;


namespace Kitapli_Proje
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        OleDbConnection baglanti = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=""C:\Users\Sema Nur Zorlu\Desktop\VP - Access\Kitaplik.mdb""");

        void listele()
        {
            DataTable dt = new DataTable();
            OleDbDataAdapter da = new OleDbDataAdapter("Select * From Kitaplar", baglanti); // baglanti daki adreste olan kitaplar tablosundaki verileri çekmek için
            da.Fill(dt);  // Hafızaya almış olduğumuz o tablonun içini sorgudan gelecek olan değer ile doldurduk
            dataGridView1.DataSource = dt; // DataGrid e veri kaynağı olarak dt den gelen nesneyi verdik
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {

        }
        

        private void Form1_Load(object sender, EventArgs e)
        {
            listele();
        }

        private void BtnListele_Click(object sender, EventArgs e)
        {
            listele();
        }

        string durum = "";

        private void BtnKaydet_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            OleDbCommand komut1 = new OleDbCommand("insert into Kitaplar (kitapAd, Yazar, Tur, Sayfa, Durum) " +
                "values (@p1,@p2,@p3,@p4,@p5)", baglanti);
            komut1.Parameters.AddWithValue("@p1", TxtKitapAd.Text);
            komut1.Parameters.AddWithValue("@p2", TxtYazar.Text);
            komut1.Parameters.AddWithValue("@p3", CmbTur.Text);
            komut1.Parameters.AddWithValue("@p4", TxtSayfa.Text);
            komut1.Parameters.AddWithValue("@p5", durum);
            komut1.ExecuteNonQuery();
            baglanti.Close();
            MessageBox.Show("Kitap Sisteme Kaydedildi", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            listele();
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            durum = "0";
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            durum = "1";
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int secilen = dataGridView1.SelectedCells[0].RowIndex; // Seçtiğimiz alanı sıfırıncı değer olarak hafızada tut ve bunun satır indexini al

            TxtKitapId.Text = dataGridView1.Rows[secilen].Cells[0].Value.ToString();
            TxtKitapAd.Text = dataGridView1.Rows[secilen].Cells[1].Value.ToString();
            TxtYazar.Text = dataGridView1.Rows[secilen].Cells[2].Value.ToString();
            CmbTur.Text = dataGridView1.Rows[secilen].Cells[3].Value.ToString();
            TxtSayfa.Text = dataGridView1.Rows[secilen].Cells[4].Value.ToString();

            if (dataGridView1.Rows[secilen].Cells[5].Value.ToString() == "True")
            {
                radioButton2.Checked = true;
            }
            else
            {
                radioButton1.Checked = true;
            }
        }

        private void BtnSil_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            OleDbCommand komut = new OleDbCommand("Delete From Kitaplar where KitapId=@p1", baglanti);
            komut.Parameters.AddWithValue("@p1", TxtKitapId.Text);
            komut.ExecuteNonQuery();
            baglanti.Close();
            MessageBox.Show("Kitap Listeden Silindi","Bigi",MessageBoxButtons.OK, MessageBoxIcon.Warning);

            listele();
        }

        private void BtnGuncelle_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            OleDbCommand komut = new OleDbCommand("update Kitaplar set KitapAd=@p1, Yazar=@p2, Tur=@p3, Sayfa=@p4, Durum=@p5 where KitapId=@p6", baglanti);  //Şartı yazmazsak bütün kaydı günceller.
            komut.Parameters.AddWithValue("@p1", TxtKitapAd.Text);
            komut.Parameters.AddWithValue("@p2", TxtYazar.Text);
            komut.Parameters.AddWithValue("@p3", CmbTur.Text);
            komut.Parameters.AddWithValue("@p4", TxtSayfa.Text);

            if (radioButton1.Checked == true)
            {
                komut.Parameters.AddWithValue("@p5", durum);  //İkinci El anlamına gelir
            }
            if (radioButton2.Checked == true)
            {
                komut.Parameters.AddWithValue("@p5", durum);  //Pakette anlamına gelir
            }

            komut.Parameters.AddWithValue("@p6", TxtKitapId.Text);
            komut.ExecuteNonQuery();
            baglanti.Close();
            MessageBox.Show("Kayıt Güncellendi", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Information);

            listele();
        }

        private void BtnAra_Click(object sender, EventArgs e)
        {
            OleDbCommand komut = new OleDbCommand("Select * From Kitaplar Where KitapAd=@p1", baglanti);
            komut.Parameters.AddWithValue("@p1", TxtKitapAra.Text);
            
            DataTable dt = new DataTable();
            OleDbDataAdapter da = new OleDbDataAdapter(komut);
            da.Fill(dt);
            dataGridView1.DataSource = dt;
            
            // HARF ARAMASINA GÖRE KİTAP BULMA KODLARI
            /* 
                OleDbCommand komut = new OleDbCommand("Select * From Kitaplar Where KitapAd like '%" + TxtKitapAra.Text + "%'", baglanti);
            
                DataTable dt = new DataTable();
                OleDbDataAdapter da = new OleDbDataAdapter(komut);
                da.Fill(dt);
                dataGridView1.DataSource = dt;
            */ 
            
        }
    }
}
