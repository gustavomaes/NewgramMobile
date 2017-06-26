using System;
using NewgramMobile.Custom;
using System.IO;

namespace NewgramMobile.iOS.Custom
{
    public class Arquivo : IArquivo
    {
        public string LocalBanco(string NomeArquivo)
        {
            string docFolder = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            string libFolder = Path.Combine(docFolder, "..", "Library", "Databases");

            if (!Directory.Exists(libFolder))
            {
                Directory.CreateDirectory(libFolder);
            }

            return Path.Combine(libFolder, NomeArquivo);
        }
    }
}