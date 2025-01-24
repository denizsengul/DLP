using DLP.Library;
using log4net;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;



namespace DLP.UI
{
    public partial class frmAyarlar : Form
    {
        string uzantilarDosyaYolu = "yasakliUzantilar.txt";
        string kelimelerDosyaYolu = "yasakliKelimeler.txt";
        public frmAyarlar()
        {
            InitializeComponent();


            txtKelimeler.Text = File.ReadAllText(kelimelerDosyaYolu);
            txtUzantilar.Text = File.ReadAllText(uzantilarDosyaYolu);

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnKaydet_Click(object sender, EventArgs e)
        {
            File.WriteAllText(kelimelerDosyaYolu, txtKelimeler.Text);
            File.WriteAllText(uzantilarDosyaYolu, txtUzantilar.Text);
            this.Close();
        }

        private void frmAyarlar_Load(object sender, EventArgs e)
        {
            btnKaydetKapat.Select();
        }
    }
}
