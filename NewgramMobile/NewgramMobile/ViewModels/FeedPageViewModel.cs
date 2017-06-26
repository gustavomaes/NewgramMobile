using NewgramMobile.Models;
using NewgramMobile.Util;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace NewgramMobile.ViewModels
{
    public class FeedPageViewModel : LoadingController, INavigationAware
    {
        private INavigationService _navigationService;

        //List info
        ObservableCollection<Post> _Posts;
        public ObservableCollection<Post> Posts
        {
            get { return _Posts; }
            set { SetProperty(ref _Posts, value); }
        }

        Post PostSelecionado { get; set; }

        private bool _isRefreshing;

        public bool IsRefreshing
        {
            get { return _isRefreshing; }
            set { SetProperty(ref _isRefreshing, value); }
        }

        public Command AtualizaCommand { get; }

        public DelegateCommand<Post> ItemTappedCommand { get; set; }
        public DelegateCommand<Post> ItemAppearingCommand { get; set; }


        public int Pagina { get; set; }
        public int QuantidadePagina { get; set; }
        public bool SemMaisDados { get; set; }

        public FeedPageViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;

            Pagina = 1;
            QuantidadePagina = 10;
            SemMaisDados = false;

            IsRefreshing = false;
            AtualizaCommand = new Command(ExecuteAtualizaCommand);

            ItemTappedCommand = new DelegateCommand<Post>(ExecuteItemTappedCommand);
            ItemAppearingCommand = new DelegateCommand<Post>(ExecuteItemAppearingCommand);

            BuscaPostsCache();
            BuscaPosts();

        }
        
        void ExecuteItemTappedCommand(Post post)
        {
            if (post != PostSelecionado)
                PostSelecionado = post;
        }

        async void ExecuteItemAppearingCommand(Post post)
        {
            if (post == Posts.Last() && !IsRefreshing)
            {
                if (!SemMaisDados)
                {
                    //Buscando mais
                    Pagina++;
                    await BuscaPosts();
                }
            }
        }

        async void ExecuteAtualizaCommand(object obj)
        {
            IsRefreshing = true;
            Pagina = 1;
            SemMaisDados = false;
            await BuscaPosts();
            IsRefreshing = false;
        }

        public async Task BuscaPostsCache()
        {
            using (DBHelper DB = new DBHelper())
            {
                Posts = new ObservableCollection<Post>(await DB.GetCahe());

            }
        }

        public async Task BuscaPosts()
        {
            try
            {

                using (APIHelper API = new APIHelper())
                {
                    API.HeadersRequest.Add("LarguraTela", App.LarguraTela.ToString());
                    var PostsRetorno = await API.GET<ObservableCollection<Post>>("api/posts/feed/?Pagina=" + Pagina + "&QuantidadePagina=" + QuantidadePagina);

                    if (Pagina == 1)//Renova
                    {
                        Posts = PostsRetorno;

                        using (DBHelper DB = new DBHelper())
                        {
                            DB.SalvaTudo(PostsRetorno.ToList());
                        }
                    }
                    else // Incrementa
                        foreach (var P in PostsRetorno)
                            Posts.Add(P);

                    if (PostsRetorno.Count < QuantidadePagina)
                        SemMaisDados = true;
                }
            }
            catch (HTTPException EX)
            {
            }
            catch (Exception EX)
            {
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
