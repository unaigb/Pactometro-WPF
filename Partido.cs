using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace El_Pactometro
{
    public class Partido : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged; //Evento para cuando se actualiza algún atributo

        private string nombre;
        private int escanios;
        private Color? color;

        public Partido()
        {
            this.nombre = "";
            this.color = null;
            this.escanios = 0;
        }

        public String Nombre
        {
            get { return nombre; }
            set { nombre = value; OnPropertyChanged("Nombre"); }
        }

        public Color? Color
        {
            get { return color; }
            set { color = value; OnPropertyChanged("Color"); }
        }

        public int Escanios
        {
            get { return escanios; }
            set { escanios = value; OnPropertyChanged("Escanios"); }
        }

        private void OnPropertyChanged(string propertyname)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyname));
        }
    }
}
