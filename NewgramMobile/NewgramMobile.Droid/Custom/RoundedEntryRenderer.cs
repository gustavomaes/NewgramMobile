using Xamarin.Forms.Platform.Android;
using Xamarin.Forms;
using Android.Graphics;
using NewgramMobile.Custom;
using NewgramMobile.Droid.Custom;

[assembly: ExportRenderer(typeof(RoundedEntry), typeof(RoundedEntryRenderer))]
namespace NewgramMobile.Droid.Custom
{
    public class RoundedEntryRenderer : EntryRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);
            if (this.Control == null) return;

            this.Control.Background = this.Resources.GetDrawable(Resource.Drawable.RoundedCornerEntry);

        }
    }
}