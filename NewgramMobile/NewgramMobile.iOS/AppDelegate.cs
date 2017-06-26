using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using UIKit;
using Prism.Unity;
using Microsoft.Practices.Unity;
using XLabs.Ioc;
using XLabs.Platform.Device;
using XLabs.Platform.Services.Geolocation;
using ImageCircle.Forms.Plugin.iOS;

namespace NewgramMobile.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            var resolverContainer = new SimpleContainer();

            resolverContainer
                .Register<IDevice>(t => AppleDevice.CurrentDevice);

            Resolver.SetResolver(resolverContainer.GetResolver());

            Xamarin.Forms.DependencyService.Register<Custom.AjusteImagem>();
            Xamarin.Forms.DependencyService.Register<Custom.Arquivo>();

            Plugin.Iconize.Iconize.With(new Plugin.Iconize.Fonts.FontAwesomeModule());

            global::Xamarin.Forms.Forms.Init();
            Xamarin.FormsMaps.Init();
            ImageCircleRenderer.Init();
            App._LarguraDP = (int)UIScreen.MainScreen.Bounds.Width;

            FormsPlugin.Iconize.iOS.IconControls.Init();
            
            LoadApplication(new App(new iOSInitializer()));

            return base.FinishedLaunching(app, options);
        }
    }

    public class iOSInitializer : IPlatformInitializer
    {
        public void RegisterTypes(IUnityContainer container)
        {

        }
    }

}
