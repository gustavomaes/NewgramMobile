using NewgramMobile.Custom;
using System;
using System.IO;
using System.Threading.Tasks;
using Xamarin.Forms;
using XLabs.Ioc;
using XLabs.Platform.Device;
using XLabs.Platform.Services.Media;

namespace NewgramMobile.Util
{
    public class FotoHelper : IDisposable
    {
        public byte[] ImagemBytes { get; set; }

        public bool FotoColetada { get; set; }

        public ImageSource GetImageSource()
        {
            return ImageSource.FromStream(() => new MemoryStream(ImagemBytes));
        }

        /// <summary>
        /// Tira foto com a câmera
        /// </summary>
        public async Task TirarFoto(int Largura, int Altura)
        {
            ConfigurarMedia();

            await _mediaPicker.TakePhotoAsync(new CameraMediaStorageOptions { DefaultCamera = CameraDevice.Rear })
            .ContinueWith(t =>
            {
                if (!t.IsFaulted && !t.IsCanceled)
                    ProcessaImagem(t.Result, Largura, Altura);
            });
        }

        /// <summary>
        /// Pega foto da biblioteca
        /// </summary>
        public async Task BuscarFoto(int Largura, int Altura)
        {
            ConfigurarMedia();

            try
            {
                await _mediaPicker.SelectPhotoAsync(new CameraMediaStorageOptions())
                .ContinueWith(t =>
                {
                    if (!t.IsFaulted && !t.IsCanceled)
                        ProcessaImagem(t.Result, Largura, Altura);
                });
            }
            catch (System.Exception ex) { }
        }


        IMediaPicker _mediaPicker;

        void ConfigurarMedia()
        {
            if (_mediaPicker != null)
                return;

            var device = Resolver.Resolve<IDevice>();
            _mediaPicker = DependencyService.Get<IMediaPicker>() ?? device.MediaPicker;
        }

        void ProcessaImagem(MediaFile mediaFile, int Largura, int Altura)
        {
            byte[] ImagemByteArrayProvisorio;

            using (MemoryStream ms = new MemoryStream())
            {
                mediaFile.Source.CopyTo(ms);

                ms.Position = 0;
                ImagemByteArrayProvisorio = ms.ToArray();
            }

            ImagemBytes = DependencyService.Get<IAjusteImagem>().Cortar(ImagemByteArrayProvisorio, Largura, Altura);

            FotoColetada = true;
        }

        public void Dispose()
        {
            _mediaPicker = null;
        }
    }
}
