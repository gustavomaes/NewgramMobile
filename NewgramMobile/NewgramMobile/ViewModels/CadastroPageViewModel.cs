using NewgramMobile.Models;
using NewgramMobile.Util;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace NewgramMobile.ViewModels
{
    public class CadastroPageViewModel : LoadingController, INavigationAware
    {
        private INavigationService _navigationService;
        private IPageDialogService _dialogService;

        public Usuario Cadastro { get; set; }
        public Command VoltarCommand { get; }
        public Command CadastroCommand { get; }

        public CadastroPageViewModel(INavigationService navigationService, IPageDialogService dialogService)
        {
            _navigationService = navigationService;
            _dialogService = dialogService;
            
            Cadastro = new Usuario();

            CadastroCommand = new Command(async () => await ExecuteCadastroCommand());

            VoltarCommand = new Command(ExecuteVoltarCommand);

        }

        async Task ExecuteCadastroCommand()
        {
            try
            {
                if (String.IsNullOrEmpty(Cadastro.Nome))
                {
                    await _dialogService.DisplayAlertAsync("Atenção", "Informe um Nome.", "OK");
                    return;
                }
                if (String.IsNullOrEmpty(Cadastro.Email))
                {
                    await _dialogService.DisplayAlertAsync("Atenção", "Informe um E-mail.", "OK");
                    return;
                }
                if (String.IsNullOrEmpty(Cadastro.Senha))
                {
                    await _dialogService.DisplayAlertAsync("Atenção", "Informe uma Senha.", "OK");
                    return;
                }

                Inicia("Realizando Cadastro...");
            }
            catch (HTTPException EX)
            {
                await _dialogService.DisplayAlertAsync("Atenção", EX.Message, "OK");
            }
            catch
            {
                await _dialogService.DisplayAlertAsync("Erro", "Ocorreu um erro. Tente novamente mais tarde.", "OK");
            }

            using (APIHelper API = new APIHelper())
            {
                App.UsuarioLogado = await API.POST<Usuario>("api/usuario", Cadastro);

                if (App.Logado)
                {
                    Dictionary<String, String> Headers = API.HeadersAllRequests;

                    //Add token usuário
                    if (API.HeadersLastResponse.ContainsKey("token"))
                        Headers.Add("token", API.HeadersLastResponse["token"]);

                    API.HeadersAllRequests = Headers;

                    await _navigationService.NavigateAsync("NavigationPage/HomePage");
                }


            }
        }

        async void ExecuteVoltarCommand(object obj)
        {
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
        }
    }
}
