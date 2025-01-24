using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace DLP.Library
{
    public static class ClipboardHelper
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Panoyu yasaklı uzantılar ve kelimeler için izler. 
        /// Dosya adı ve/veya içeriği kontrol edilir.
        /// </summary>
        public static (List<FileInfo> YasakliDosyalar, List<FileInfo> IzinVerilenDosyalar) Baslat(
            List<string> yasakliUzantilar,
            List<string> yasakliKelimeler,
            bool dosyaAdiKontrolEt,
            bool dosyaIcerigiKontrolEt)
        {
            Log.Info("Pano izleme işlemi başlatıldı.");
            (List<FileInfo> YasakliDosyalar, List<FileInfo> IzinVerilenDosyalar) sonuc = (null, null);

            Thread staThread = new Thread(() =>
            {
                sonuc = PanoyuOku(yasakliUzantilar, yasakliKelimeler, dosyaAdiKontrolEt, dosyaIcerigiKontrolEt);
            });

            staThread.SetApartmentState(ApartmentState.STA);
            staThread.Start();
            staThread.Join();
            Log.Info("Pano izleme işlemi tamamlandı.");
            return sonuc;
        }

        /// <summary>
        /// Panoyu temizler.
        /// </summary>
        public static void PanoyuTemizle()
        {
            Log.Info("Panoyu temizleme işlemi başlatıldı.");
            Thread staThread = new Thread(() =>
            {
                try
                {
                    Clipboard.Clear();
                    Log.Info("Pano başarıyla temizlendi.");
                }
                catch (Exception ex)
                {
                    Log.Error($"Panoyu temizlerken hata oluştu: {ex.Message}");
                }
            });

            staThread.SetApartmentState(ApartmentState.STA);
            staThread.Start();
            staThread.Join();
        }

        /// <summary>
        /// Panodan kopyalanan dosyaları yasaklı uzantı ve kelimelere göre kontrol eder.
        /// </summary>
        private static (List<FileInfo> YasakliDosyalar, List<FileInfo> IzinVerilenDosyalar) PanoyuOku(
            List<string> yasakliUzantilar,
            List<string> yasakliKelimeler,
            bool dosyaAdiKontrolEt,
            bool dosyaIcerigiKontrolEt)
        {
            Log.Info("Panoyu kontrol etme işlemi başlatıldı.");
            List<FileInfo> yasakliDosyalar = new List<FileInfo>();
            List<FileInfo> izinVerilenDosyalar = new List<FileInfo>();

            try
            {
                if (Clipboard.ContainsFileDropList())
                {
                    Log.Info("Pano dosya listesi içeriyor.");
                    var dosyalar = Clipboard.GetFileDropList()?.Cast<string>();

                    if (dosyalar != null)
                    {
                        foreach (var dosyaYolu in dosyalar)
                        {
                            var dosya = new FileInfo(dosyaYolu);
                            Log.Info($"Kontrol edilen dosya: {dosya.FullName}");

                            bool yasakli = DosyaYasakliMi(dosya, yasakliUzantilar, yasakliKelimeler, dosyaAdiKontrolEt, dosyaIcerigiKontrolEt);

                            if (yasakli)
                            {
                                Log.Warn($"Yasaklı dosya bulundu: {dosya.FullName}");
                                yasakliDosyalar.Add(dosya);
                            }
                            else
                            {
                                Log.Info($"İzin verilen dosya: {dosya.FullName}");
                                izinVerilenDosyalar.Add(dosya);
                            }
                        }

                        if (yasakliDosyalar.Count > 0)
                        {
                            Log.Warn("Yasaklı dosyalar bulundu, pano temizleniyor.");
                            PanoyuTemizle();
                        }
                    }
                }
                else
                {
                    Log.Info("Pano dosya listesi içermiyor.");
                }
            }
            catch (Exception ex)
            {
                Log.Fatal($"Panoya erişirken hata oluştu: {ex.Message}");
            }

            Log.Info("Pano kontrol işlemi tamamlandı.");
            return (yasakliDosyalar, izinVerilenDosyalar);
        }

        /// <summary>
        /// Bir dosyanın yasaklı olup olmadığını kontrol eder.
        /// </summary>
        private static bool DosyaYasakliMi(
            FileInfo dosya,
            List<string> yasakliUzantilar,
            List<string> yasakliKelimeler,
            bool dosyaAdiKontrolEt,
            bool dosyaIcerigiKontrolEt)
        {
            Log.Info($"Dosya yasaklı mı kontrol ediliyor: {dosya.FullName}");
            string dosyaUzantisi = dosya.Extension.ToLower();
            string dosyaAdi = dosya.Name.ToLower();

            if (!dosyaAdiKontrolEt && !dosyaIcerigiKontrolEt)
            {
                Log.Info("Dosya adı ve içerik kontrolü devre dışı, dosya izin verildi.");
                return false;
            }

            if (yasakliUzantilar.Contains(dosyaUzantisi))
            {
                Log.Warn($"Yasaklı uzantıya sahip dosya: {dosya.FullName}");
                return true;
            }

            if (dosyaAdiKontrolEt && yasakliKelimeler.Any(kelime => dosyaAdi.Contains(kelime.ToLower())))
            {
                Log.Warn($"Yasaklı kelime dosya adında bulundu: {dosya.FullName}");
                return true;
            }

            if (dosyaIcerigiKontrolEt && IcerikteYasakliKelimeVarMi(dosya.FullName, yasakliKelimeler))
            {
                Log.Warn($"Yasaklı kelime dosya içeriğinde bulundu: {dosya.FullName}");
                return true;
            }

            Log.Info($"Dosya yasaklı değil: {dosya.FullName}");
            return false;
        }

        /// <summary>
        /// Dosya içeriğinde yasaklı kelimelerin olup olmadığını kontrol eder.
        /// </summary>
        private static bool IcerikteYasakliKelimeVarMi(string dosyaYolu, List<string> yasakliKelimeler)
        {
            Log.Info($"Dosya içeriği kontrol ediliyor: {dosyaYolu}");
            try
            {
                string icerik = File.ReadAllText(dosyaYolu).ToLower();
                bool sonuc = yasakliKelimeler.Any(kelime => icerik.Contains(kelime.ToLower()));

                if (sonuc)
                {
                    Log.Warn($"Dosya içeriğinde yasaklı kelime bulundu: {dosyaYolu}");
                }
                else
                {
                    Log.Info($"Dosya içeriğinde yasaklı kelime bulunamadı: {dosyaYolu}");
                }

                return sonuc;
            }
            catch (Exception ex)
            {
                Log.Error($"Dosya okunurken hata oluştu ({dosyaYolu}): {ex.Message}");
                return false;
            }
        }
    }
}
