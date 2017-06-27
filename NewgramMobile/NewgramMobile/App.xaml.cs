using NewgramMobile.Models;
using NewgramMobile.Views;
using Newtonsoft.Json;
using Prism.Unity;
using System;
using Xamarin.Forms;
using XLabs.Ioc;
using XLabs.Platform.Device;

namespace NewgramMobile
{
    public partial class App : PrismApplication
    {

        #region Preferences Data

        public const string URL = "http://192.168.25.135:4000";

        public static App Current;

        public string PushToken
        {
            get { return App.PreferenceGet<String>("PushToken"); }
            set { App.PreferenceAdd("PushToken", value); }
        }

        public static Usuario UsuarioLogado
        {
            get { return App.PreferenceGet<Usuario>("UsuarioLogado"); }
            set { App.PreferenceAdd("UsuarioLogado", value); }
        }

        public static bool Logado
        {
            get { return UsuarioLogado != null; }
        }

        public static int LarguraTela
        {
            get { return App.PreferenceGet<int>("LarguraTela"); }
            set { App.PreferenceAdd("LarguraTela", value); }
        }
        public static int AlturaTela
        {
            get { return App.PreferenceGet<int>("AlturaTela"); }
            set { App.PreferenceAdd("AlturaTela", value); }
        }
        public static int _LarguraDP;
        public static int LarguraDP
        {
            get { return App.PreferenceGet<int>("LarguraDP"); }
            set { App.PreferenceAdd("LarguraDP", value); }
        }

        #endregion

        public App(IPlatformInitializer initializer = null) : base(initializer) { }

        protected override void OnInitialized()
        {
            InitializeComponent();

            Current = this;

            LarguraTela = Resolver.Resolve<IDevice>().Display.Width;
            AlturaTela = Resolver.Resolve<IDevice>().Display.Height;
            LarguraDP = _LarguraDP;
            if (Logado)
                NavigationService.NavigateAsync("NavigationPage/HomePage");
            else
                NavigationService.NavigateAsync("LoginPage");
            
        }

        protected override void RegisterTypes()
        {
            Container.RegisterTypeForNavigation<NavigationPage>();
            Container.RegisterTypeForNavigation<LoginPage>();
            Container.RegisterTypeForNavigation<CadastroPage>();
            Container.RegisterTypeForNavigation<HomePage>();
            Container.RegisterTypeForNavigation<FeedPage>();
            Container.RegisterTypeForNavigation<PostagemPage>();
            Container.RegisterTypeForNavigation<BuscaPage>();
            Container.RegisterTypeForNavigation<PerfilPage>();
            Container.RegisterTypeForNavigation<AlterarPage>();
            Container.RegisterTypeForNavigation<PostDetalhe>();
        }

        #region Preferences

        public static void PreferenceAdd(string Name, object Data)
        {
            string Serializable = JsonConvert.SerializeObject(Data);

            if (PreferenceExist(Name))
                PreferenceRemove(Name);

            Current.Properties[Name] = Serializable;
            Current.SavePropertiesAsync();

        }

        public static bool PreferenceExist(string Name)
        {
            return Current.Properties.ContainsKey("Name");
        }

        public static void PreferenceRemove(string Name)
        {
            Current.Properties.Remove(Name);
            Current.SavePropertiesAsync();
        }

        public static T PreferenceGet<T>(string Name)
        {
            if (Current.Properties.ContainsKey(Name))
            {
                string Dado = Current.Properties[Name].ToString();

                return JsonConvert.DeserializeObject<T>(Dado);

            }

            return default(T);
        }

        #endregion
    }
}
