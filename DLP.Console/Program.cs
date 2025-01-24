using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;
using DLP.Library;
using log4net;

class Program
{
    private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
    static bool checkFileName = true;
    static bool checkFileContent = true;
    
    static async Task Main(string[] args)
    {
       // Start();
    }

    //static async void Start()
    //{
    //    // Yasaklı dosya uzantıları belirle
    //    var forbiddenExtensions = new List<string> { ".cs", ".sql" };

    //    // Yasaklı dosya adları kelimeleri belirle
    //    var forbiddenWords = new List<string> { "script", "analiz", "api" };

    //    // Pano dinleniyor
    //    while (true)
    //    {
    //        if (Clipboard.ContainsFileDropList())
    //        {
    //            var files = Clipboard.GetFileDropList();
    //            if (files.Count > 0)
    //            {
    //                Console.Clear();
    //            }
    //        }



    //        // Pano içeriğini kontrol et
    //        var (bannedFiles, allowedFiles) = await Task.Run(() => ClipboardHelper.Start(forbiddenExtensions, forbiddenWords, checkFileName, checkFileContent));

    //        // Yasaklı dosyaları yazdır
    //        foreach (var bannedFile in bannedFiles)
    //        {
    //            string message;
    //            message = $"KOPYALAMAYA İZİN VERİLMEDİ - {bannedFile.Name}";
    //            Log.Error(message);
    //        }

    //        // Yasaklı olmayan dosyaları yazdır
    //        foreach (var allowedFile in allowedFiles)
    //        {
    //            string message;
    //            message = $"KOPYALAMAYA İZİN VERİLDİ - {allowedFile.Name}";
    //            Log.Info(message);
    //        }


    //        // Kısa bir süre bekle (örneğin 0.5 saniye)
    //        await Task.Delay(500);
    //    }
    //}
}
