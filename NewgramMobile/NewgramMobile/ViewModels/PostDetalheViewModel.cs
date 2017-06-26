using NewgramMobile.Models;
using NewgramMobile.Util;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace NewgramMobile.ViewModels
{
    public class PostDetalheViewModel : BindableBase, INavigationAware
    {
        private INavigationService _navigationService;
        private IPageDialogService _dialogService;

        Post _Post;
        public Post Post
        {
            get { return _Post; }
            set { SetProperty(ref _Post, value); }
        }

        List<PostInteracao> _comentarios;
        public List<PostInteracao> Comentarios
        {
            get { return _comentarios; }
            set { SetProperty(ref _comentarios, value); }
        }

        private PostInteracao _novoComentario;
        public PostInteracao NovoComentario
        {
            get { return _novoComentario; }
            set { SetProperty(ref _novoComentario, value); }
        }

        public Command DenunciarCommand { get; set; }
        public Command EnviarComentarioCommand { get; set; }
        public Command ExcluirCommand { get; set; }

        public PostDetalheViewModel(INavigationService navigationService, IPageDialogService dialogService)
        {
            _navigationService = navigationService;
            _dialogService = dialogService;

            NovoComentario = new PostInteracao();

            DenunciarCommand = new Command(async () => await ExecuteDenunciarCommand());
            EnviarComentarioCommand = new Command(async () => await ExecuteEnviarComentarioCommand());
            ExcluirCommand = new Command(async () => await ExecuteExcluirCommand());
        }

        public async void BuscaComentarios()
        {
            try
            {
                using (APIHelper API = new APIHelper())
                {
                    Comentarios = await API.GET<List<PostInteracao>>("api/posts/comentarios/" + Post.Id);
                    MessagingCenter.Send<Object>(this, "ComentariosRecebidos");
                }
            }
            catch (HTTPException EX) { }
            catch (Exception EX) { }
        }

        public async Task ExecuteExcluirCommand()
        {
            var resposta = await _dialogService.DisplayAlertAsync("Certeza ?", "Quer mesmo exluir essa foto ?", "Sim", "Não");

            if (resposta)
            {
                try
                {
                    using (APIHelper API = new APIHelper())
                    {
                        await API.DELETE("api/posts/" + Post.Id);
                    }
                }
                catch (HTTPException EX) { }
                catch (Exception EX) { }
            }
        }

        public async Task ExecuteEnviarComentarioCommand()
        {
            //Novo comentario
            NovoComentario.UsuarioId = App.UsuarioLogado.Id;
            NovoComentario.UsuarioDados = App.UsuarioLogado;
            NovoComentario.Data = DateTime.Now;

            try
            {
                using (APIHelper API = new APIHelper())
                {
                    await API.PUT("api/posts/comentar/" + Post.Id + "?Texto=" + NovoComentario.Texto, null);
                }
            }
            catch (HTTPException EX) { }
            catch (Exception EX) { }

            Comentarios.Add(NovoComentario);

            Post.EuComentei = true;
            NovoComentario = new PostInteracao();
        }

        public async Task ExecuteDenunciarCommand()
        {

            if (Post.EuDenunciei)
                return;

            try
            {
                Post.EuDenunciei = true;

                using (APIHelper API = new APIHelper())
                {
                    await API.PUT("api/posts/denunciar/" + Post.Id, null);
                }
            }
            catch (HTTPException EX) { }
            catch (Exception EX) { }
        }

        public async void DenunciarComentario(PostInteracao Comentario)
        {
            if (Comentario.EuDenunciei)
                return;

            try
            {
                Comentario.EuDenunciei = true;

                using (APIHelper API = new APIHelper())
                {
                    await API.PUT("api/posts/denunciar-comentario/" + Comentario.Id, null);
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
            Post = (Post)parameters["post"];

            BuscaComentarios();
        }
    }
}
