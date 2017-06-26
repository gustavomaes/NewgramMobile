using System;
using NewgramMobile.Custom;
using System.IO;

[assembly: Xamarin.Forms.Dependency(typeof(IArquivo))]
namespace NewgramMobile.Droid.Custom
{
    public class Arquivo : IArquivo
    {
        public string LocalBanco(string NomeArquivo)
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            return Path.Combine(path, NomeArquivo);
        }
    }
}