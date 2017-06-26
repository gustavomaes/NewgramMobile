using NewgramMobile.Models;
using Xamarin.Forms;

namespace NewgramMobile.Views
{
    public partial class PostDetalhe : ContentPage
    {
        //PostDetalheViewModel PDVM { get; set; }

        public PostDetalhe(Post P)
        {
            InitializeComponent();

            //Title = "Foto de " + P.UsuarioDados.Nome;

            //PDVM = new PostDetalheViewModel(P, this);

            //BindingContext = PDVM;

            //MessagingCenter.Subscribe<Object>(this, "ComentariosRecebidos", (sender) => {

            //    Comentarios.Children.Clear();

            //    foreach (var C in PDVM.Comentarios)
            //    {
            //        ComentarioParcial CP = new ComentarioParcial(C);
            //        CP.OnDenunciaComentario += CP_OnDenunciaComentario;

            //        Comentarios.Children.Add(CP);
            //    }
            //});
        }

        //private void CP_OnDenunciaComentario(PostInteracao comentario)
        //{
        //    PDVM.DenunciarComentario(comentario);
        //}

        //private void ClickUsuario(object sender, EventArgs e)
        //{
        //    //Navigation.PushAsync(new PerfilPage(PDVM.Post.UsuarioId));
        //}

        //private async void Excluir(object sender, EventArgs e)
        //{
        //    await PDVM._Excluir();
        //    await Navigation.PopAsync();
        //}
    }
}
