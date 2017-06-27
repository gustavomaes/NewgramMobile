using NewgramMobile.Util;
using Newtonsoft.Json;
using Prism.Mvvm;
using SQLite;
using System;
using System.ComponentModel;
using Xamarin.Forms;
using Prism.Navigation;

namespace NewgramMobile.Models
{
    public class Post : BindableBase
    {
        [PrimaryKey]
        public int Id { get; set; }

        public DateTime Data { get; set; }

        public int UsuarioId { get; set; }

        [Ignore]
        public Usuario UsuarioDados { get; set; }
        public string _UsuarioDados { get; set; }
        public void SerializaUsuarioDados()
        {
            if (UsuarioDados != null)
                _UsuarioDados = JsonConvert.SerializeObject(UsuarioDados);
        }
        public void DesSerializaUsuarioDados()
        {
            if (!String.IsNullOrEmpty(_UsuarioDados))
                UsuarioDados = JsonConvert.DeserializeObject<Usuario>(_UsuarioDados);
        }

        [Ignore]
        public byte[] Foto { get; set; }

        public string FotoURL { get; set; }
        public string FotoURLPequena { get; set; }
        public string FotoURLMiniatura { get; set; }


        public string _Descricao { get; set; }
        public string Descricao
        {
            get { return _Descricao; }
            set
            {
                _Descricao = value;
                RaisePropertyChanged("Descricao");
            }
        }
        
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        private int _QuantidadeCurtidas { get; set; }
        public int QuantidadeCurtidas
        {
            get { return _QuantidadeCurtidas; }
            set
            {
                
                _QuantidadeCurtidas = value;
                RaisePropertyChanged("QuantidadeCurtidas");
                RaisePropertyChanged("IconeCurtida");
                RaisePropertyChanged("IconeCurtidaCor");
            }
        }


        private int _QuantidadeComentarios { get; set; }
        public int QuantidadeComentarios
        {
            get { return _QuantidadeComentarios; }
            set
            {
                _QuantidadeComentarios = value;
                RaisePropertyChanged("QuantidadeComentarios");
                RaisePropertyChanged("IconeComentario");
                RaisePropertyChanged("IconeComentarioCor");
            }
        }

        public int QuantidadeDenuncias { get; set; }

        public bool Denunciado { get; set; }

        public bool Bloqueado { get; set; }

        public bool Meu { get; set; }
        public bool NaoMeu { get { return !Meu; } }
        public bool EuCurti { get; set; }
        public bool EuComentei { get; set; }
        public bool EuDenunciei { get; set; }

        [Ignore]
        public string IconeCurtida { get { return (EuCurti ? "fa-heart" : "fa-heart-o"); } }
        [Ignore]
        public Color IconeCurtidaCor { get { return (EuCurti ? Color.Green : Color.Blue); } }

        [Ignore]
        public string IconeComentario { get { return (EuComentei ? "fa-comment" : "fa-comment-o"); } }
        [Ignore]
        public Color IconeComentarioCor { get { return (EuComentei ? Color.Green : Color.Blue); } }

        [Ignore]
        public int LarguraDP { get { return App.LarguraDP; } }

    }
}
