using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Net;
using System.Net.Mail;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        SqlConnection baglanti = new SqlConnection(@"Data Source=FURKAN\SQLEXPRESS;Initial Catalog=C#loginekrani;Integrated Security=True");
        // sql bağlantısı için gerekli olan komut
        private void button1_Click_1(object sender, EventArgs e)
        {
            if (textBox1.Text == "" || textBox2.Text == "")
            {
                MessageBox.Show("Lütfen boş alan bırakmayınız...");
            }

            baglanti.Open();
            string user;
            string password;
            user = textBox1.Text;
            password = textBox2.Text;

            SqlCommand komut = new SqlCommand();
            komut.Connection = baglanti;
            komut.CommandText = "select * from Table_1 where kullanici='" + user + "' and sifre='" + password + "'";
            SqlDataReader oku = komut.ExecuteReader();
            if (oku.Read())
            {
                MessageBox.Show("Hoşgeldiniz " + user);
            }
            else
            {
                MessageBox.Show("Yanlış girdiniz");
            }
            baglanti.Close();
        }
        
        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            tabControl1.SelectedTab = tabPage2;
            textBox3.Text=textBox1.Text;

        }

        private void button2_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            SqlCommand komut = new SqlCommand("Select * From Table_1 Where kullanici='"+textBox3.Text.ToString()+"'and mail='"+textBox4.Text.ToString()+"'",baglanti);

            SqlDataReader oku = komut.ExecuteReader();
            while (oku.Read())
            {
                try
                {
                    if (baglanti.State == ConnectionState.Closed)
                    {
                        baglanti.Open();
                    }
                    SmtpClient smtp = new SmtpClient();
                    MailMessage message = new MailMessage();
                    string tarih = DateTime.Now.ToLongDateString();
                    string mailadresin = ("rapperx34@gmail.com");
                    string sifre = ("qeppmwx6");
                    string smtpsrvr = "smtp.gmail.com";
                    string kime = (oku["mail"].ToString());
                    string konu = ("Şifre hatırlatma talebi");
                    string yaz = ("Sayın " + oku["kullanici"].ToString() + "\nBizden" + tarih + " Tarihinde Şifre hatırlatma talebinde bulundunuz" + "\nParolanız:" + oku["sifre"].ToString() + "\nİyi günler");
                    smtp.Credentials = new NetworkCredential(mailadresin, sifre);
                    smtp.Port = 587;
                    smtp.Host = smtpsrvr;
                    smtp.EnableSsl = true;
                    smtp.UseDefaultCredentials = false;
                    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                    message.From = new MailAddress(mailadresin);
                    message.To.Add(kime);
                    message.IsBodyHtml = true;
                    message.Subject = konu;
                    message.Body = yaz;
                    smtp.Send(message);
                    DialogResult bilgi = new DialogResult();
                    bilgi = MessageBox.Show("Girmiş olduğunuz bilgiler uyuşuyor. Şifreniz mail adresinize gönderilmiştiri");
                    
                }
                catch(Exception Hata)
                {
                    MessageBox.Show("Mail gönderme hatası",Hata.Message);

                }
            }
            
            baglanti.Close();
        }
    }
}
