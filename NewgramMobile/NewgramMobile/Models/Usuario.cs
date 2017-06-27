using NewgramMobile.Util;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewgramMobile.Models
{
    public class Usuario : BindableBase
    {
        public int Id { get; set; }

        private string _nome;
        public string Nome
        {
            get { return _nome; }
            set { SetProperty(ref _nome, value); }
        }

        private string _email;

        public string Email
        {
            get { return _email; }
            set { SetProperty(ref _email, value); }
        }


        public string Senha { get; set; }

        private string _URLFoto;
        public string URLFoto
        {
            get { return _URLFoto; }
            set
            {
                _URLFoto = value;
                RaisePropertyChanged("URLFoto");
            }
        }


        public int QuantidadePosts { get; set; }

        private int _quantidadeSeguindo;
        public int QuantidadeSeguindo
        {
            get { return _quantidadeSeguindo; }
            set { SetProperty(ref _quantidadeSeguindo, value); }
        }

        private int _quantidadeSeguidores;
        public int QuantidadeSeguidores
        {
            get { return _quantidadeSeguidores; }
            set { SetProperty(ref _quantidadeSeguidores, value); }
        }

        public bool Meu { get; set; }
        public bool NaoMeu { get { return !Meu; } }

        bool _Sigo;
        public bool Sigo
        {
            get { return _Sigo; }
            set
            {
                _Sigo = value;
                RaisePropertyChanged("Sigo");
                RaisePropertyChanged("NaoSigo");
            }
        }

        public bool NaoSigo { get { return !Sigo; } }

    }
}
