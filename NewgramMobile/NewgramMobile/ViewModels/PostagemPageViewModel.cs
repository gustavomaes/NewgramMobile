using NewgramMobile.Models;
using NewgramMobile.Util;
using System.Threading.Tasks;
using Xamarin.Forms;
using System;
using Prism.Services;

namespace NewgramMobile.ViewModels
{
    public class PostagemPageViewModel : LoadingController
    {
        private IPageDialogService _dialogService;

        public Post Post { get; set; }

        private ImageSource _imagem;
        public ImageSource Imagem
        {
            get { return _imagem; }
            set { SetProperty(ref _imagem, value); }
        }

        public Command TiraFotoCommand { get; set; }
        public Command BuscaFotoCommand { get; set; }
        public Command PostarCommand { get; set; }


        public PostagemPageViewModel(IPageDialogService dialogService)
        {
            _dialogService = dialogService;

            Post = new Post();

            Imagem = ImageSource.FromFile("photo.png");

            TiraFotoCommand = new Command(async () => await TirarFoto(), () => true);
            BuscaFotoCommand = new Command(async () => await BuscarFoto(), () => true);
            PostarCommand = new Command(async () => await ExecutePostarCommand(), () => true);
        }

        async Task ExecutePostarCommand()
        {

            //Valida Foto
            if (Post.Foto == null)
            {
                await _dialogService.DisplayAlertAsync("Erro", "Precisa de uma foto para postar", "OK");
                return;
            }

            //Valida Descrição
            if (String.IsNullOrEmpty(Post.Descricao))
            {
                await _dialogService.DisplayAlertAsync("Erro", "Informe uma descrição", "OK");
                return;
            }

            Inicia("Gravando Postagem...");

            using (APIHelper API = new APIHelper())
            {
                Post.UsuarioId = App.UsuarioLogado.Id;

                Post PostGravado = await API.POST<Post>("api/Posts", Post);

                //LIMPAR
                Post.Descricao = "";
                Imagem = ImageSource.FromFile("photo.png");
            }

            //Finaliza e mostra mensagem
            Post.Descricao = "";
            Finaliza();
            await _dialogService.DisplayAlertAsync("Postado!", "Postagem realizada com sucesso.", "OK");
        }

        private async Task TirarFoto()
        {
            using (FotoHelper Foto = new FotoHelper())
            {
                await Foto.TirarFoto(App.LarguraTela, App.LarguraTela);
                if (Foto.FotoColetada)
                {
                    Post.Foto = Foto.ImagemBytes;
                    Imagem = Foto.GetImageSource();
                }
            }
        }

        private async Task BuscarFoto()
        {
            using (FotoHelper Foto = new FotoHelper())
            {
                await Foto.BuscarFoto(App.LarguraTela, App.LarguraTela);
                if (Foto.FotoColetada)
                {
                    Post.Foto = Foto.ImagemBytes;
                    Imagem = Foto.GetImageSource();
                }
            }
        }

    }
}
