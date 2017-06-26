using NewgramMobile.Models;
using NewgramMobile.Util;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace NewgramMobile.ViewModels
{
    public class AlterarPageViewModel : BindableBase, INavigationAware
    {
        private INavigationService _navigationService;
        private IPageDialogService _dialogService;

        private Usuario _usuario;
        public Usuario Usuario
        {
            get { return _usuario; }
            set { SetProperty(ref _usuario, value); }
        }

        private Usuario UsuarioAlterado { get; set;
        }
        public Command CancelarCommand { get; } 
        public Command AtualizarCommand { get; }


        public AlterarPageViewModel(INavigationService navigationService, IPageDialogService dialogService)
        {
            _navigationService = navigationService;
            _dialogService = dialogService;

            Usuario = new Usuario();
                 
            CancelarCommand = new Command(ExecuteCancelarCommand);
            AtualizarCommand = new Command(ExecuteAtualizarCommand);
        }

        async void ExecuteAtualizarCommand()
        {
            if (String.IsNullOrEmpty(Usuario.Nome))
            {
                await _dialogService.DisplayAlertAsync("Atenção!", "Você precisa informar um nome.", "OK");
                return;
            }
            if (String.IsNullOrEmpty(Usuario.Email))
            {
                await _dialogService.DisplayAlertAsync("Atenção!", "Você precisa informar um e-mail.", "OK");
                return;
            }

            try
            {
                using (APIHelper API = new APIHelper())
                {
                    await API.PUT("api/usuario/", Usuario);
                }

                UsuarioAlterado.Nome = Usuario.Nome;
            }
            catch (HTTPException EX) { }
            catch (Exception EX) { }

            await _navigationService.GoBackAsync();
        }

        async void ExecuteCancelarCommand()
        {
            //Usuario = UsuarioNaoAlterado;
            await _navigationService.GoBackAsync();
        }

        public void OnNavigatedFrom(NavigationParameters parameters)
        {
        }

        public void OnNavigatedTo(NavigationParameters parameters)
        {
        }

        public void OnNavigatingTo(NavigationParameters parameters)
        {
            Usuario.Nome = ((Usuario)parameters["usuario"]).Nome;
            Usuario.Email = ((Usuario)parameters["usuario"]).Email;

            UsuarioAlterado = (Usuario)parameters["usuario"];
        }
    }
}
