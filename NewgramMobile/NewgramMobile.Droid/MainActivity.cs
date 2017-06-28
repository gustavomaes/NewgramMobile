using Android.App;
using Android.Content.PM;
using Android.OS;
using Prism.Unity;
using Microsoft.Practices.Unity;
using XLabs.Ioc;
using XLabs.Platform.Device;
using ImageCircle.Forms.Plugin.Droid;

namespace NewgramMobile.Droid
{
    //[Activity(Label = "NewgramMobile", Icon = "@drawable/icon", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    [Activity(Label = "Newgram", Icon = "@drawable/icon", Theme = "@style/MyTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.tabs;
            ToolbarResource = Resource.Layout.toolbar;

            var resolverContainer = new SimpleContainer();
            
            resolverContainer.Register<IDevice>(t => AndroidDevice.CurrentDevice);

            Resolver.ResetResolver();
            Resolver.SetResolver(resolverContainer.GetResolver());

            base.OnCreate(bundle);

            Xamarin.Forms.DependencyService.Register<Custom.AjusteImagem>();
            Xamarin.Forms.DependencyService.Register<Custom.Arquivo>();

            global::Xamarin.Forms.Forms.Init(this, bundle);

            ImageCircleRenderer.Init();
            App._LarguraDP = (int)(Resources.DisplayMetrics.WidthPixels / Resources.DisplayMetrics.Density);

            //FormsPlugin.Iconize.Droid.IconControls.Init(Resource.Id.toolbar, Resource.Id.sliding_tabs);

            LoadApplication(new App(new AndroidInitializer()));
        }
    }

    public class AndroidInitializer : IPlatformInitializer
    {
        public void RegisterTypes(IUnityContainer container)
        {

        }
    }
}

