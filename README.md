# 🛡️ DLP (Data Loss Prevention) - Veri Sızıntısı Önleme Sistemi

## 📋 Proje Hakkında

Bu proje, kamu kurumlarında kritik verilerin gizliliği, ulusal güvenlik ve kurumsal sorumluluk açısından büyük önem taşıyan **Veri Sızıntısı Önleme (DLP)** çözümüdür. Hassas bilgileri izinsiz erişim ve sızıntılara karşı korur.

## 🎯 Temel Özellikler

- **📋 Pano İzleme**: Sistem panosunu sürekli izler ve kopyalanan dosyaları kontrol eder
- **🚫 Dosya Uzantı Kontrolü**: Yasaklı dosya uzantılarını (.cs, .sql vb.) engeller
- **🔍 İçerik Analizi**: Dosya içeriğinde yasaklı kelimeleri tarar
- **📝 Dosya Adı Kontrolü**: Dosya adlarında hassas kelimeler arar
- **📊 Gerçek Zamanlı İzleme**: Anlık olarak dosya transferlerini takip eder
- **🎨 Kullanıcı Dostu Arayüz**: Modern Windows Forms tabanlı GUI
- **📋 Detaylı Loglama**: log4net ile kapsamlı kayıt tutma

## 🏗️ Proje Yapısı

```
DLP/
├── DLP.Console/          # Konsol uygulaması
│   ├── Program.cs        # Ana konsol programı
│   └── App.config        # Konfigürasyon dosyası
├── DLP.Library/          # Temel kütüphane
│   └── ClipboardHelper.cs # Pano işlemleri sınıfı
├── DLP.UI/              # Windows Forms arayüzü
│   ├── frmDLPEkrani.cs  # Ana form
│   ├── frmAyarlar.cs    # Ayarlar formu
│   └── Resources/       # Görsel kaynaklar
└── Files/               # Ek dosyalar ve kaynaklar
```

## 🔧 Teknolojiler

- **Framework**: .NET Framework 4.7.2
- **Dil**: C#
- **UI**: Windows Forms
- **Loglama**: log4net 3.0.3
- **IDE**: Visual Studio

## 🚀 Kurulum

1. **Projeyi klonlayın:**
   ```bash
   git clone https://github.com/denizsengul/DLP.git
   ```

2. **Visual Studio'da açın:**
   - `DLP.sln` dosyasını Visual Studio ile açın

3. **NuGet paketlerini yükleyin:**
   - Solution'ı sağ tıklayın → "Restore NuGet Packages"

4. **Projeyi derleyin:**
   - Build → Build Solution (Ctrl+Shift+B)

## 📖 Kullanım

### GUI Uygulaması (DLP.UI)

1. **Uygulamayı başlatın:**
   - `DLP.UI.exe` dosyasını çalıştırın

2. **Temel kontroller:**
   - **Takip Modu**: Sistemi aktif/pasif yapar
   - **Dosya Adı Kontrolü**: Dosya adlarında yasaklı kelime araması
   - **Dosya İçeriği Kontrolü**: Dosya içeriğinde yasaklı kelime araması

3. **Konfigürasyon dosyaları:**
   - `yasakliUzantilar.txt`: Yasaklı dosya uzantıları
   - `yasakliKelimeler.txt`: Yasaklı kelimeler listesi

### Konsol Uygulaması (DLP.Console)

```csharp
// Yasaklı dosya uzantıları
var forbiddenExtensions = new List<string> { ".cs", ".sql" };

// Yasaklı kelimeler
var forbiddenWords = new List<string> { "script", "analiz", "api" };

// Pano kontrolü
var (bannedFiles, allowedFiles) = ClipboardHelper.Baslat(
    forbiddenExtensions, 
    forbiddenWords, 
    checkFileName: true, 
    checkFileContent: true
);
```

## 🔍 Ana Sınıflar ve Metodlar

### ClipboardHelper Sınıfı

```csharp
// Pano izleme başlatma
public static (List<FileInfo> YasakliDosyalar, List<FileInfo> IzinVerilenDosyalar) 
    Baslat(List<string> yasakliUzantilar, List<string> yasakliKelimeler, 
           bool dosyaAdiKontrolEt, bool dosyaIcerigiKontrolEt)

// Pano temizleme
public static void PanoyuTemizle()

// Dosya yasaklı mı kontrolü
private static bool DosyaYasakliMi(FileInfo dosya, ...)

// İçerik kontrolü
private static bool IcerikteYasakliKelimeVarMi(string dosyaYolu, ...)
```

## 📊 Özellik Detayları

### 1. Pano İzleme Sistemi
- Sürekli pano aktivitesini izler
- Dosya kopyalama işlemlerini yakalar
- Yasaklı dosyaları otomatik olarak panodan temizler

### 2. Çoklu Kontrol Mekanizması
- **Uzantı Kontrolü**: Dosya uzantısına göre filtreleme
- **Ad Kontrolü**: Dosya adında yasaklı kelime arama
- **İçerik Kontrolü**: Dosya içeriğinde hassas veri tarama

### 3. Loglama Sistemi
- Tüm aktiviteler log4net ile kaydedilir
- XML formatında detaylı loglar
- Hata ve uyarı seviyelerinde kategorize edilmiş kayıtlar

### 4. Kullanıcı Arayüzü
- Modern ve kullanıcı dostu tasarım
- Gerçek zamanlı durum gösterimi
- Kolay konfigürasyon yönetimi

## 🔒 Güvenlik Özellikleri

- **Gerçek Zamanlı Koruma**: Anlık dosya transfer kontrolü
- **Çoklu Katman Güvenlik**: Uzantı, ad ve içerik kontrolü
- **Otomatik Müdahale**: Yasaklı dosyaları otomatik engelleme
- **Kapsamlı Loglama**: Tüm aktivitelerin izlenebilirliği

## 📈 Performans

- **Düşük Kaynak Kullanımı**: Minimal sistem kaynağı tüketimi
- **Hızlı Tarama**: Etkili dosya analiz algoritmaları
- **Asenkron İşlemler**: UI donmaması için async/await kullanımı

## 🛠️ Geliştirme

### Katkıda Bulunma
1. Fork yapın
2. Feature branch oluşturun (`git checkout -b feature/YeniOzellik`)
3. Değişikliklerinizi commit edin (`git commit -am 'Yeni özellik eklendi'`)
4. Branch'inizi push edin (`git push origin feature/YeniOzellik`)
5. Pull Request oluşturun

### Kod Standartları
- C# coding conventions'ları takip edin
- XML documentation kullanın
- Unit testler yazın
- Log4net ile loglama yapın

## 📝 Lisans

Bu proje açık kaynak kodludur. Kullanım koşulları için repository sahibi ile iletişime geçin.

## 👨‍💻 Geliştirici

**Deniz Şengül**
- GitHub: [@denizsengul](https://github.com/denizsengul)

## 🆘 Destek

Herhangi bir sorun yaşarsanız:
1. GitHub Issues bölümünü kullanın
2. Detaylı hata açıklaması yapın
3. Log dosyalarını ekleyin

## 📚 Ek Kaynaklar

- [.NET Framework Dokümantasyonu](https://docs.microsoft.com/en-us/dotnet/framework/)
- [log4net Dokümantasyonu](https://logging.apache.org/log4net/)
- [Windows Forms Rehberi](https://docs.microsoft.com/en-us/dotnet/desktop/winforms/)

---

⭐ Bu projeyi beğendiyseniz yıldız vermeyi unutmayın!