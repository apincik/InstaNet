using System;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace Bot.Browser.Extensions
{
    /// <summary>
    /// Proxy browser extension.
    /// </summary>
    /// <remarks>
    /// Load zip extension file, skipping http auth dialog on windows.
    /// </remarks>
    class ProxyExtension : Extensions
    {

        private string _ipAddress;
        private string _port;
        private string _username;
        private string _password;

        /*
         * Clean proxy folder from previously created files.
         */
        protected override void Init()
        {
            if(String.IsNullOrEmpty(_ipAddress) || String.IsNullOrEmpty(_port))
            {
                throw new ApplicationException("Undefined browser proxy extension connection details.");
            }

            var path = CURRENT_DIR + "\\driver-extensions\\proxy";
            System.IO.DirectoryInfo di = new DirectoryInfo(path);
            foreach (FileInfo file in di.GetFiles())
            {
                file.Delete();
            }
        }

        /*
         * Create zip chrome extension from manifest and background files. 
         */
        public override string Create()
        {
            Init();

            Random rand = new Random();
            string zipFileName = "proxy-config-" + DateTime.Now.ToString("h-mm-ss-tt") + rand.Next(1, 100) + ".zip";
            var extensionFilePath = CURRENT_DIR;
            extensionFilePath += "\\driver-extensions\\proxy\\" + zipFileName;

            //Create a zip archive - extension
            using (var fileStream = new FileStream(extensionFilePath, FileMode.CreateNew))
            {
                using (var archive = new ZipArchive(fileStream, ZipArchiveMode.Create, true))
                {

                    var backgroundJsFilePath = CURRENT_DIR + "\\driver-extensions\\background.js";
                    var backgroundJsFileString = File.ReadAllText(backgroundJsFilePath);
                    backgroundJsFileString = backgroundJsFileString.Replace("%IP_ADDRESS%", _ipAddress);
                    backgroundJsFileString = backgroundJsFileString.Replace("%PORT%", _port);
                    backgroundJsFileString = backgroundJsFileString.Replace("%USERNAME%", _username);
                    backgroundJsFileString = backgroundJsFileString.Replace("%PASSWORD%", _password);
                    byte[] backgroundJsBytes = Encoding.UTF8.GetBytes(backgroundJsFileString);

                    var manifestFilePath = CURRENT_DIR + "\\driver-extensions\\manifest.json";
                    var manifestFileString = File.ReadAllText(manifestFilePath);
                    byte[] manifestJsBytes = Encoding.UTF8.GetBytes(manifestFileString);

                    var zipArchiveEntry = archive.CreateEntry("background.js", CompressionLevel.Fastest);
                    using (var zipStream = zipArchiveEntry.Open())
                    {
                        zipStream.Write(backgroundJsBytes, 0, backgroundJsBytes.Length);
                    }

                    zipArchiveEntry = archive.CreateEntry("manifest.json", CompressionLevel.Fastest);
                    using (var zipStream = zipArchiveEntry.Open())
                    {
                        zipStream.Write(manifestJsBytes, 0, manifestJsBytes.Length);
                    }
                }
            }

            return extensionFilePath;
        }

        public void SetConnectionDetails(string ipAddress, string port, string username = "", string password = "")
        {
            _ipAddress = ipAddress;
            _port = port;
            _username = username;
            _password = password;
        }
    }
}
