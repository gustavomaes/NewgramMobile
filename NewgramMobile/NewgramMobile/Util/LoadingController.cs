using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewgramMobile.Util
{
    public class LoadingController : BindableBase
    {
        private bool _loadingEnable;

        public bool LoadingEnable
        {
            get { return _loadingEnable; }
            set { SetProperty(ref _loadingEnable, value); }
        }

        public bool _loadingVisible;
        public bool LoadingVisible
        {
            get { return _loadingVisible; }
            set { SetProperty(ref _loadingVisible, value); }
        }

        public bool _actionIsRunning;
        public bool ActionIsRunning
        {
            get { return _actionIsRunning; }
            set { SetProperty(ref _actionIsRunning, value); }
        }

        public bool _actionIsVisible;
        public bool ActionIsVisible
        {
            get { return _actionIsVisible; }
            set { SetProperty(ref _actionIsVisible, value); }
        }

        public bool _textIsVisible;
        public bool TextIsVisible
        {
            get { return _textIsVisible; }
            set { SetProperty(ref _textIsVisible, value); }
        }

        public bool _textIsEnabled;
        public bool TextIsEnabled
        {
            get { return _textIsEnabled; }
            set { SetProperty(ref _textIsEnabled, value); }
        }

        public string _text;
        public string Text
        {
            get { return _text; }
            set { SetProperty(ref _text, value); }
        }
        
        public LoadingController()
        {
            Finaliza();
        }

        public void Inicia()
        {
            Inicia("");
        }
        public void Inicia(String texto)
        {
            LoadingEnable = true;
            LoadingVisible = true;
            ActionIsRunning = true;
            ActionIsVisible = true;
            TextIsVisible = true;
            TextIsEnabled = true;
            Text = texto;

        }

        public void Finaliza()
        {
            LoadingVisible = false;
            ActionIsRunning = false;
            ActionIsVisible = false;
            TextIsVisible = false;
            TextIsEnabled = false;
        }
    }
}
