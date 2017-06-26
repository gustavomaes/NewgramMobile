using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NewgramMobile
{
    public partial class Loading : StackLayout
    {
        public Loading()
        {
            InitializeComponent();
        }

        public void Inicia()
        {
            Inicia("");
        }
        public void Inicia(String texto)
        {
            this.IsEnabled = true;
            this.IsVisible = true;
            actLoading.IsRunning = true;
            actLoading.IsVisible = true;
            txtLoading.IsVisible = true;
            txtLoading.IsEnabled = true;
            txtLoading.Text = texto;

        }

        public void Finaliza()
        {
            this.IsVisible = false;
            actLoading.IsRunning = false;
            actLoading.IsVisible = false;
            txtLoading.IsVisible = false;
            txtLoading.IsEnabled = false;
        }
    }
}