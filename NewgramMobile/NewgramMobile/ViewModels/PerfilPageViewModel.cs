using NewgramMobile.Models;
using NewgramMobile.Util;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace NewgramMobile.ViewModels
{
    public class PerfilPageViewModel : LoadingController, INavigationAware
    {
        private INavigationService _navigationService;
        private IPageDialogService _dialogService;

        private Usuario _usuario;
        public Usuario Usuario
        {
            get { return _usuario; }
            set { SetProperty(ref _usuario, value); }
        }

        private int IdUsuario { get; set; }

        private byte[] ImagemBytes { get; set; }

        private int _heightDP;
        public int HeightDP
        {
            get { return _heightDP; }
            set { SetProperty(ref _heightDP, value); }
        }

        private List<Post> _posts;
        public List<Post> Posts
        {
            get { return _posts; }
            set { SetProperty(ref _posts, value); }
        }

        public Command<Post> ItemTappedCommand { get; set; }

        public Command SeguirUsuarioCommand { get; }
        public Command DeixarSeguirUsuarioCommand { get; }

        public Command CancelarCommand { get; }
        public Command LogoffCommand { get; }
        public Command FotoCommand { get; }
        public Command SeguindoCommand { get; }
        public Command SeguidoresCommand { get; }
        public Command AlterarCommand { get; }



        public PerfilPageViewModel(INavigationService navigationService, IPageDialogService dialogService)
        {
            _navigationService = navigationService;
            _dialogService = dialogService;

            HeightDP = App.LarguraDP / 3;

            Usuario = App.UsuarioLogado;

            IdUsuario = Usuario.Id;

            ItemTappedCommand = new Command<Post>(ExecuteItemTappedCommand);

            SeguirUsuarioCommand = new Command(ExecuteSeguirUsuarioCommand);
            DeixarSeguirUsuarioCommand = new Command(ExecuteDeixarSeguirUsuarioCommand);
            CancelarCommand = new Command(ExecuteCancelarCommand);
            LogoffCommand = new Command(ExecuteLogoffCommand);
            FotoCommand = new Command(ExecuteFotoCommand);
            SeguindoCommand = new Command(ExecuteSeguindoCommand);
            SeguidoresCommand = new Command(ExecuteSeguidoresCommand);
            AlterarCommand = new Command(ExecuteAlterarCommand);

            BuscaDadosUsuario();
            BuscaPostsUsuario();
        }

        async void ExecuteAlterarCommand(object obj)
        {
            var navigationParams = new NavigationParameters();
            navigationParams.Add("usuario", Usuario);

            await _navigationService.NavigateAsync("AlterarPage", navigationParams, false);
        }

        async void ExecuteSeguidoresCommand(object obj)
        {
            var navigationParams = new NavigationParameters();
            navigationParams.Add("tipo", "seguidores");
            navigationParams.Add("UsuarioId", IdUsuario);

            await _navigationService.NavigateAsync("BuscaPage", navigationParams, false);
        }

        async void ExecuteSeguindoCommand()
        {
            var navigationParams = new NavigationParameters();
            navigationParams.Add("tipo", "seguidos");
            navigationParams.Add("UsuarioId", IdUsuario);

            await _navigationService.NavigateAsync("BuscaPage", navigationParams, false);
        }

        async void ExecuteFotoCommand(object obj)
        {
            var FonteFoto = await _dialogService.DisplayActionSheetAsync("De onde gostaria de pegar a foto?", "Cancelar", null, "Camêra", "Biblioteca");

            switch (FonteFoto)
            {
                case "Camêra":
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        using (FotoHelper Foto = new FotoHelper())
                        {
                            await Foto.TirarFoto(200, 200);

                            if (Foto.FotoColetada)
                            {
                                ImagemBytes = Foto.ImagemBytes;
                            }
                        }

                        await TrocarFoto(ImagemBytes);
                    });
                    break;
                case "Biblioteca":
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        using (FotoHelper Foto = new FotoHelper())
                        {
                            await Foto.BuscarFoto(200, 200);

                            if (Foto.FotoColetada)
                            {
                                ImagemBytes = Foto.ImagemBytes;
                            }
                        }

                        await TrocarFoto(ImagemBytes);
                    });
                    break;
                default:
                    break;
            }
        }

        async void ExecuteLogoffCommand(object obj)
        {
            using (APIHelper API = new APIHelper())
            {
                try
                {
                    await API.POST("api/usuario/logoff", new { });

                    App.UsuarioLogado = null;
                    API.HeadersAllRequests = new Dictionary<string, string>();
                }
                catch (HTTPException EX) { }
                catch (Exception EX) { }

            }

            await _navigationService.NavigateAsync("app:///LoginPage");
        }

        async void ExecuteCancelarCommand()
        {
            var resposta = await _dialogService.DisplayAlertAsync("Atenção!", "Tem certeza que deseja cancelar sua conta ?", "Sim", "Não");

            if (resposta)
            {
                try
                {
                    using (APIHelper API = new APIHelper())
                    {
                        await API.DELETE("api/usuario/");

                        App.UsuarioLogado = null;
                        API.HeadersAllRequests = new Dictionary<string, string>();
                    }
                }
                catch (HTTPException EX) { }
                catch (Exception EX) { }

                await _navigationService.NavigateAsync("app:///LoginPage");
            }
        }

        async void ExecuteItemTappedCommand(Post post)
        {
            var navigationParams = new NavigationParameters();
            navigationParams.Add("post", post);

            await _navigationService.NavigateAsync("PostDetalhe", navigationParams, false);
        }

        async void ExecuteSeguirUsuarioCommand()
        {
            try
            {
                using (APIHelper API = new APIHelper())
                {
                    await API.PUT("api/usuario/seguir/" + IdUsuario, null);
                }

                Usuario.Sigo = true;
            }
            catch (HTTPException EX) { }
            catch (Exception EX) { }
        }

        async void ExecuteDeixarSeguirUsuarioCommand()
        {
            try
            {
                using (APIHelper API = new APIHelper())
                {
                    await API.PUT("api/usuario/deixar-seguir/" + IdUsuario, null);
                }

                Usuario.Sigo = false;
            }
            catch (HTTPException EX) { }
            catch (Exception EX) { }
        }

        public async Task BuscaDadosUsuario()
        {
            try
            {
                using (APIHelper API = new APIHelper())
                {
                    API.HeadersRequest.Add("LarguraTela", App.LarguraTela.ToString());
                    Usuario = await API.GET<Usuario>("api/usuario/" + IdUsuario);

                    MessagingCenter.Send<Object>(this, "DadosUsuarioAtualizados");
                }
            }
            catch (HTTPException EX) { }
            catch (Exception EX) { }
        }

        public async Task BuscaPostsUsuario()
        {
            try
            {
                using (APIHelper API = new APIHelper())
                {
                    API.HeadersRequest.Add("LarguraTela", App.LarguraTela.ToString());
                    Posts = await API.GET<List<Post>>("api/posts/feed-usuario/" + IdUsuario);

                    MessagingCenter.Send<Object>(this, "PostsUsuarioAtualizados");
                }
            }
            catch (HTTPException EX) { }
            catch (Exception EX) { }
        }

        //Trocar Foto
        public async Task TrocarFoto(byte[] FotoNova)
        {
            try
            {
                //Atualiza na API
                using (APIHelper API = new APIHelper())
                {
                    //Passar Dados 
                    Usuario.URLFoto = await API.PUT<String>("api/usuario/foto", FotoNova);
                    App.UsuarioLogado.URLFoto = Usuario.URLFoto;
                }

                ImagemBytes = null;
            }
            catch (HTTPException EX) { }
            catch (Exception EX) { }
        }

        public void OnNavigatedFrom(NavigationParameters parameters)
        {
        }

        public void OnNavigatedTo(NavigationParameters parameters)
        {
        }

        public void OnNavigatingTo(NavigationParameters parameters)
        {
        }
    }
}