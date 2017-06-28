using NewgramMobile.Models;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace NewgramMobile.ViewModels
{
    public class MasterPageViewModel : BindableBase, INavigationAware
    {
        //Prism
        private INavigationService _navigationService;

        //Props
        public Usuario Usuario { get; set; }

        //Commands
        //public Command LogoffCommand { get; set; }
        public Command FeedCommand { get; set; }
        public Command PerfilCommand { get; set; }
        public Command BuscaCommand { get; set; }
        
        public MasterPageViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;

            Usuario = App.UsuarioLogado;


            //LogoffCommand = new Command(ExecuteLogoffCommand);
            FeedCommand = new Command(ExecuteFeedCommand);
            PerfilCommand = new Command(ExecutePerfilCommand);
            BuscaCommand = new Command(ExecuteBuscaCommand);
        }

        async void ExecuteFeedCommand()
        {
            await _navigationService.NavigateAsync("FeedPage");
        }

        async void ExecutePerfilCommand()
        {
            NavigationParameters navigationParams = new NavigationParameters();
            navigationParams.Add("usuario", App.UsuarioLogado);

            await _navigationService.NavigateAsync("PerfilPage", navigationParams);
        }

        async void ExecuteBuscaCommand()
        {
            NavigationParameters navigationParams = new NavigationParameters();
            navigationParams.Add("busca", true);

            await _navigationService.NavigateAsync("BuscaPage", navigationParams);
        }

        public void OnNavigatedFrom(NavigationParameters parameters) { }

        public void OnNavigatedTo(NavigationParameters parameters) { }

        public void OnNavigatingTo(NavigationParameters parameters) { }
    }
}
