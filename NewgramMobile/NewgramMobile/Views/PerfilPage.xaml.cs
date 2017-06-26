using NewgramMobile.Models;
using NewgramMobile.Util;
using NewgramMobile.ViewModels;
using Rg.Plugins.Popup.Extensions;
using System;
using Xamarin.Forms;

namespace NewgramMobile.Views
{
    public partial class PerfilPage : ContentPage
    {
        public PerfilPage()
        {
            InitializeComponent();
        }
    }
    

        //Seleciona Post
        //void PMP_OnClicaPost(Post post)
        //{
        //    Navigation.PushAsync(new PostDetalhe(post));
        //}


        //private void AlteraDados_Clicked(object sender, EventArgs e)
        //{
        //    AlteraDados AD = new AlteraDados(PVM.Usuario);
        //    AD.OnUsuarioAlterado += AD_OnUsuarioAlterado;
        //    Navigation.PushPopupAsync(AD);
        //}
        
        //private void ListaSeguidos(object sender, EventArgs e)
        //{
        //    ListaUsuario LU = new ListaUsuario();
        //    LU.Config(ListaUsuario.TipoLista.Seguidos, PVM.IdUsuario);

        //    Navigation.PushAsync(LU);
        //}

        //private void ListaSeguidores(object sender, EventArgs e)
        //{
        //    ListaUsuario LU = new ListaUsuario();
        //    LU.Config(ListaUsuario.TipoLista.Seguidores, PVM.IdUsuario);

        //    Navigation.PushAsync(LU);
        //}
    
}
