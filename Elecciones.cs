using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace El_Pactometro
{
    public class Elecciones : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged; //Evento para cuando se actualiza algún atributo

        private string tipo;
        private string lugar;
        private DateTime? fecha;
        private int escanios;
        private int mayAbsoluta;
        public List<Partido> _partidos;

        public Elecciones() 
        {
            this.tipo = "";
            this.lugar = "";
            this.fecha = new DateTime();
            this.escanios = 0;
            this.mayAbsoluta = 0;
            this.Partidos = new List<Partido>();
        }

        public String Tipo
        {
            get { return tipo; }
            set { tipo = value; OnPropertyChanged("Tipo"); }
        }

        public String Lugar
        {
            get { return lugar; }
            set { lugar = value; OnPropertyChanged("Lugar"); }
        }

        public int Escanios
        {
            get { return escanios; }
            set { escanios = value; OnPropertyChanged("Escanios"); }
        }

        public int MayAbsoluta
        {
            get { return mayAbsoluta; }
            set { mayAbsoluta = value; OnPropertyChanged("MayAbsoluta"); }
        }

        public DateTime? Fecha
        {
            get { return fecha; }
            set { fecha = value; OnPropertyChanged("Fecha"); }
        }

        public List<Partido> Partidos
        {
            get { return _partidos; }
            set { _partidos = value; OnPropertyChanged(nameof(Partidos)); }
        }

        private void OnPropertyChanged(string propertyname)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyname));
        }
    }
}
