using DLP.Library;
using log4net;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace DLP.UI
{
    public partial class frmDLPEkrani : Form
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public frmDLPEkrani()
        {
            InitializeComponent();
            AdjustFormPosition();

        }

        private void AdjustFormPosition()
        {
            // Tüm ekranları al
            var screens = Screen.AllScreens;

            // Varsayılan olarak ikinci monitörü seç
            Screen targetScreen = screens.Length < 1 ? screens[1] : Screen.PrimaryScreen;

            this.MinimumSize = this.Size;
            this.MaximumSize = this.Size;

            // Formu sağ alt köşeye hizala
            int xPosition = targetScreen.WorkingArea.Right - this.Width;
            int yPosition = targetScreen.WorkingArea.Bottom - this.Height;

            // Formun başlangıç konumunu ayarla
            this.StartPosition = FormStartPosition.Manual;
            this.Location = new Point(xPosition, yPosition);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        bool dosyaAdiKontrolEt = true;
        bool dosyaIcerigiKontrolEt = true;

        /// <summary>
        /// Pano izleme ve yasaklı dosya kontrol döngüsünü başlatır.
        /// </summary>
        async Task PanoIzleVeKontrolEt()
        {
            Log.Info("Pano izleme ve kontrol döngüsü başlatıldı.");

            while (true)
            {
                try
                {
                    #region Dosya Okuma İşlemleri
                    // Yasaklı dosya uzantıları ve kelimeler listeleri
                    var yasakliUzantilar = new List<string>();
                    var yasakliKelimeler = new List<string>();

                    // Dosyadan yasaklı uzantıları oku
                    string uzantilarDosyaYolu = "yasakliUzantilar.txt";
                    if (File.Exists(uzantilarDosyaYolu))
                    {
                        yasakliUzantilar.AddRange(File.ReadAllLines(uzantilarDosyaYolu));
                    }
                    else
                    {
                        Log.Error($"Dosya bulunamadı: {uzantilarDosyaYolu}");
                    }

                    // Dosyadan yasaklı kelimeleri oku
                    string kelimelerDosyaYolu = "yasakliKelimeler.txt";
                    if (File.Exists(kelimelerDosyaYolu))
                    {
                        yasakliKelimeler.AddRange(File.ReadAllLines(kelimelerDosyaYolu));
                    }
                    else
                    {
                        Log.Error($"Dosya bulunamadı: {kelimelerDosyaYolu}");
                    }

                    // Okunan veriyi ekrana yazdır
                    Log.Info("Yasaklı Uzantılar:");
                    foreach (var uzanti in yasakliUzantilar)
                    {
                        Log.Info(uzanti);
                    }

                    Log.Info("\nYasaklı Kelimeler:");
                    foreach (var kelime in yasakliKelimeler)
                    {
                        Log.Info(kelime);
                    }

                    #endregion

                    // Pano boş mu kontrol et
                    if (Clipboard.ContainsFileDropList())
                    {
                        var dosyalar = Clipboard.GetFileDropList();

                        if (dosyalar.Count > 0)
                        {
                            Log.Info("Pano içeriği bulundu, dosyalar işleniyor.");

                            lstIzinVerilenler.Items.Clear();
                            lstIzinVerilmeyenler.Items.Clear();

                            // Pano içeriğini kontrol et
                            var (yasakliDosyalar, izinVerilenDosyalar) = await Task.Run(() =>
                                ClipboardHelper.Baslat(
                                    yasakliUzantilar,
                                    yasakliKelimeler,
                                    dosyaAdiKontrolEt,
                                    dosyaIcerigiKontrolEt));

                            // Yasaklı dosyaları listeye ekle
                            foreach (var yasakliDosya in yasakliDosyalar)
                            {
                                string mesaj = $"KOPYALAMAYA İZİN VERİLMEDİ - {yasakliDosya.FullName}";
                                lstIzinVerilmeyenler.Items.Add(yasakliDosya.Name);
                                Log.Error(mesaj);
                            }

                            // İzin verilen dosyaları listeye ekle
                            foreach (var izinVerilenDosya in izinVerilenDosyalar)
                            {
                                string mesaj = $"KOPYALAMAYA İZİN VERİLDİ - {izinVerilenDosya.FullName}";
                                lstIzinVerilenler.Items.Add(izinVerilenDosya.Name);
                                Log.Info(mesaj);
                            }
                        }
                    }

                    // Kısa bir süre bekle (örneğin 0.5 saniye)
                    await Task.Delay(500);
                }
                catch (Exception ex)
                {
                    Log.Error($"Pano izleme döngüsünde hata oluştu: {ex.Message}");
                }
            }
        }




        private void Form1_Load(object sender, EventArgs e)
        {
            btnClose.Select();
            PanoIzleVeKontrolEt();
        }

        bool _TakipModu = false;
        private void btnTakipModu_Click(object sender, EventArgs e)
        {
            if (_TakipModu)
            {
                btnTakipModu.Image = Properties.Resources.monitor_on;
                _TakipModu = false;
                pnlTakipModu.Enabled = true;
                btnDosyaAdiniKontrol.Image = Properties.Resources.OptiWheel_OptiPoints_Hover;
                btnDosyaIceriginiKontrolEt.Image = Properties.Resources.OptiWheel_OptiPoints_Hover;
                lblTakipModu.Text = "Takip Modunu Kapat";
                dosyaAdiKontrolEt = true;
                dosyaIcerigiKontrolEt = true;
            }
            else
            {
                btnTakipModu.Image = Properties.Resources.monitor_off;
                _TakipModu = true;
                pnlTakipModu.Enabled = false;
                btnDosyaAdiniKontrol.Image = Properties.Resources.OptiWheel_OptiPoints;
                btnDosyaIceriginiKontrolEt.Image = Properties.Resources.OptiWheel_OptiPoints;
                lblTakipModu.Text = "Takip Modunu Aç";
                dosyaAdiKontrolEt = false;
                dosyaIcerigiKontrolEt = false;
            }

        }

        private void btnDosyaAdiniKontrol_Click(object sender, EventArgs e)
        {
            if (dosyaAdiKontrolEt)
            {
                dosyaAdiKontrolEt = false;
                btnDosyaAdiniKontrol.Image = Properties.Resources.OptiWheel_OptiPoints;
            }
            else
            {
                dosyaAdiKontrolEt = true;
                btnDosyaAdiniKontrol.Image = Properties.Resources.OptiWheel_OptiPoints_Hover;
            }
        }

        private void btnDosyaIceriginiKontrolEt_Click(object sender, EventArgs e)
        {
            if (dosyaIcerigiKontrolEt)
            {
                dosyaIcerigiKontrolEt = false;
                btnDosyaIceriginiKontrolEt.Image = Properties.Resources.OptiWheel_OptiPoints;
            }
            else
            {
                dosyaIcerigiKontrolEt = true;
                btnDosyaIceriginiKontrolEt.Image = Properties.Resources.OptiWheel_OptiPoints_Hover;
            }
        }

        private void btnAyarlar_Click(object sender, EventArgs e)
        {
            frmAyarlar frmAyarlar = new frmAyarlar();
            frmAyarlar.TopMost = true;
            frmAyarlar.ShowDialog();
        }
    }
}
