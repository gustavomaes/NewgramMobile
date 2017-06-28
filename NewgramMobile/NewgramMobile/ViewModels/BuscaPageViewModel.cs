using NewgramMobile.Models;
using Prism.Navigation;
using System;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using NewgramMobile.Util;
using System.Threading.Tasks;

namespace NewgramMobile.ViewModels
{
    public class BuscaPageViewModel : LoadingController, INavigationAware
    {
        private INavigationService _navigationService;

        private string _termo;

        public string Termo
        {
            get { return _termo; }
            set { SetProperty(ref _termo, value); }
        }

        private bool _buscaVisible;

        public bool BuscaVisible
        {
            get { return _buscaVisible; }
            set { SetProperty(ref _buscaVisible, value); }
        }

        private string _title;

        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }


        private string tipo { get; set; }
        private int UsuarioId { get; set; }

        ObservableCollection<Usuario> _usuarios;
        public ObservableCollection<Usuario> Usuarios
        {
            get { return _usuarios; }
            set { SetProperty(ref _usuarios, value); }
        }

        public Command PesquisaCommand { get; set; }

        public Command<Usuario> PerfilCommand { get; }
        public Command<Usuario> SeguirUsuarioCommand { get; }
        public Command<Usuario> DeixarSeguirUsuarioCommand { get; }

        public BuscaPageViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;

            PesquisaCommand = new Command(ExecutePesquisaCommand);
            PerfilCommand = new Command<Usuario>(ExecutePerfilCommand);
            SeguirUsuarioCommand = new Command<Usuario>(ExecuteSeguirUsuarioCommand);
            DeixarSeguirUsuarioCommand = new Command<Usuario>(ExecuteDeixarSeguirUsuarioCommand);
        }

        async void ExecutePerfilCommand(Usuario usuario)
        {
            var navigationParams = new NavigationParameters();
            navigationParams.Add("usuario", usuario);

            await _navigationService.NavigateAsync("PerfilPage", navigationParams, false);
        }

        async void ExecuteSeguirUsuarioCommand(Usuario usuario)
        {
            int usuarioId = usuario.Id;

            try
            {
                using (APIHelper API = new APIHelper())
                {
                    await API.PUT("api/usuario/seguir/" + usuarioId, null);
                }

                usuario.Sigo = true;
            }
            catch (HTTPException EX) { }
            catch (Exception EX) { }
        }

        async void ExecuteDeixarSeguirUsuarioCommand(Usuario usuario)
        {
            int usuarioId = usuario.Id;

            try
            {
                using (APIHelper API = new APIHelper())
                {
                    await API.PUT("api/usuario/deixar-seguir/" + usuarioId, null);
                }

                usuario.Sigo = false;
            }
            catch (HTTPException EX) { }
            catch (Exception EX) { }
        }

        async void ExecutePesquisaCommand()
        {
            try
            {
                using (APIHelper API = new APIHelper())
                {
                    API.HeadersRequest.Add("LarguraTela", App.LarguraTela.ToString());
                    Usuarios = await API.GET<ObservableCollection<Usuario>>("api/usuario/pesquisa/?termo=" + Termo);
                }
            }
            catch (HTTPException EX) { }
            catch (Exception EX) { }
        }

        public async Task BuscaSeguidores(int IDSeguido)
        {
            try
            {
                using (APIHelper API = new APIHelper())
                {
                    API.HeadersRequest.Add("LarguraTela", App.LarguraTela.ToString());
                    Usuarios = await API.GET<ObservableCollection<Usuario>>("api/usuario/" + IDSeguido + "/seguidores/");
                }
            }
            catch (HTTPException EX) { }
            catch (Exception EX) { }
        }

        public async Task BuscaSeguidos(int IDSeguidor)
        {
            try
            {
                using (APIHelper API = new APIHelper())
                {
                    API.HeadersRequest.Add("LarguraTela", App.LarguraTela.ToString());
                    Usuarios = await API.GET<ObservableCollection<Usuario>>("api/usuario/" + IDSeguidor + "/seguidos/");
                }
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

        public async void OnNavigatingTo(NavigationParameters parameters)
        {
            if ((bool)parameters["busca"])
            {
                BuscaVisible = true;
            }
            else
            {
                BuscaVisible = false;

                tipo = (string)parameters["tipo"];
                UsuarioId = (int)parameters["UsuarioId"];

                if (tipo == "seguidores")
                {
                    await BuscaSeguidores(UsuarioId);
                    Title = "Seguidores";
                }
                else if (tipo == "seguidos")
                {
                    await BuscaSeguidos(UsuarioId);
                    Title = "Seguidos";
                }
            }
        }
    }
}
