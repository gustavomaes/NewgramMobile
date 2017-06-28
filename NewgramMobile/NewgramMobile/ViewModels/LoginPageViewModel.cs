using NewgramMobile.Models;
using NewgramMobile.Util;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace NewgramMobile.ViewModels
{
    public class LoginPageViewModel : LoadingController, INavigationAware
    {
        private INavigationService _navigationService;
        private IPageDialogService _dialogService;

        public Usuario Usuario { get; set; }

        public Command EntrarCommand { get; }
        public Command CadastrarCommand { get; }

        public LoginPageViewModel(INavigationService navigationService, IPageDialogService dialogService)
        {
            _navigationService = navigationService;
            _dialogService = dialogService;

            Usuario = new Usuario();

            EntrarCommand = new Command(async () => await ExecuteEntrarCommand());
            CadastrarCommand = new Command(ExecuteCadastrarCommand);

        }

        async void ExecuteCadastrarCommand()
        {
            await _navigationService.NavigateAsync("CadastroPage");
        }

        async Task ExecuteEntrarCommand()
        {
            //Valida dados
            try
            {
                if (String.IsNullOrEmpty(Usuario.Email))
                {
                    await _dialogService.DisplayAlertAsync("Atenção", "Informe um e-mail.", "OK");
                    return;
                }
                if (String.IsNullOrEmpty(Usuario.Senha))
                {
                    await _dialogService.DisplayAlertAsync("Atenção", "Informe uma senha.", "OK");
                    return;
                }

                Inicia("Validando Usuário");

            }
            catch (HTTPException EX)
            {
                await _dialogService.DisplayAlertAsync("Atenção", EX.Message, "OK");
            }
            catch (Exception)
            {
                await _dialogService.DisplayAlertAsync("Erro", "Ocorreu um erro inesperado. Tente novamente mais tarde.", "OK");
            }

            try
            {
                using (APIHelper API = new APIHelper())
                {
                    App.UsuarioLogado = await API.POST<Usuario>("api/usuario/login", Usuario);

                    if (App.Logado)
                    {
                        Dictionary<String, String> Headers = API.HeadersAllRequests;
                        //Add Token
                        if (API.HeadersLastResponse.ContainsKey("token"))
                            Headers.Add("token", API.HeadersLastResponse["token"]);

                        API.HeadersAllRequests = Headers;//SET

                        await _navigationService.NavigateAsync("app:///NavigationPage/HomePage");

                    }
                }
            }
            catch (Exception EX)
            {
                await _dialogService.DisplayAlertAsync("Erro", "Ocorreu um erro ao entrar, verifique seus dados e tente novamente.", "OK");
                Finaliza();
            }
            
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
