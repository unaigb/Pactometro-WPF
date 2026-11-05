using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Xceed.Wpf.Toolkit;

namespace El_Pactometro
{
    /// <summary>
    /// Lógica de interacción para IntroducirDatos.xaml
    /// </summary>

    //Evento para cuando cambie la selección del ListView
    public class SeleccionEleccionCambiaEventArgs : EventArgs
    {

        public Elecciones eleccionSeleccionada { get; set; }

        public SeleccionEleccionCambiaEventArgs(Elecciones ele)
        {
            eleccionSeleccionada = ele;
        }
    }

    public partial class IntroducirDatos : Window
    {
        //Manejadores de selección cambiada
        public event EventHandler<SeleccionEleccionCambiaEventArgs> seleccionEleccionesCambiada;

        //Listas observables
        ObservableCollection<Elecciones> listaElecciones = new ObservableCollection<Elecciones>();
        ObservableCollection<Partido> listaPartidos = new ObservableCollection<Partido>();

        public IntroducirDatos(ObservableCollection<Elecciones> elec, ObservableCollection<Partido> part)
        {
            InitializeComponent();

            Loaded += IntroducirDatos_Loaded;
            Closing += VentanaClosing;

            listaElecciones = elec;
            tablaElecciones.ItemsSource = listaElecciones;

            listaPartidos = part;
            tablaPartidos.ItemsSource = listaPartidos;
        }

        void OnhaySeleccionCocheCambiada(SeleccionEleccionCambiaEventArgs e)
        {
            if (seleccionEleccionesCambiada != null)
                seleccionEleccionesCambiada(this, e);
        }

        public ObservableCollection<Elecciones> ListaElecciones
        {
            get { return this.listaElecciones; }
            set { this.listaElecciones = value; tablaElecciones.ItemsSource = this.listaElecciones; }
        }

        public ObservableCollection<Partido> ListaPartidos
        {
            get { return this.listaPartidos; }
            set { this.listaPartidos = value; tablaPartidos.ItemsSource = this.listaPartidos; }
        }

        private void lista_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender == tablaElecciones)
            {
                int num = listaPartidos.Count();
                for (int i = 0; i < num; i++)
                {
                    listaPartidos.RemoveAt(0);
                }

                if (tablaElecciones.SelectedIndex >= 0)
                {
                    Elecciones el = (Elecciones)tablaElecciones.Items[tablaElecciones.SelectedIndex];
                    SeleccionEleccionCambiaEventArgs misarg = new SeleccionEleccionCambiaEventArgs(el);
                    OnhaySeleccionCocheCambiada(misarg);

                    if (tablaElecciones.Items.Count > 1) //Comprobación visibilidad botones subir/bajar
                    {
                        int indiceActual = tablaElecciones.SelectedIndex;
                        if (indiceActual != 0 && indiceActual != (tablaElecciones.Items.Count - 1))
                        {
                            Boton_Subir.Visibility = Visibility.Visible;
                            Boton_Bajar.Visibility = Visibility.Visible;
                        }
                        else if (indiceActual == 0)
                        {
                            Boton_Subir.Visibility = Visibility.Hidden;
                            Boton_Bajar.Visibility = Visibility.Visible;
                        }
                        else if (indiceActual == (tablaElecciones.Items.Count - 1))
                        {
                            Boton_Subir.Visibility = Visibility.Visible;
                            Boton_Bajar.Visibility = Visibility.Hidden;
                        }
                    }

                    if (tablaElecciones.SelectedItems.Count == 1)
                    {
                        Caja1.Text = el.Tipo;
                        Caja2.Text = el.Lugar;
                        Caja3.SelectedDate = el.Fecha;
                        Boton_Modificar.IsEnabled = true;
                    }
                    Boton_Eliminar.IsEnabled = true;

                    foreach (Partido p in el.Partidos)
                    {
                        AgregarPartidoOrdenado(p);
                    }
                }
                else
                {
                    Elecciones el = null;
                    SeleccionEleccionCambiaEventArgs misarg = new SeleccionEleccionCambiaEventArgs(el);
                    OnhaySeleccionCocheCambiada(misarg);

                    Boton_Eliminar.IsEnabled = false;
                    Boton_Modificar.IsEnabled = false;
                    Caja1.Text = "";
                    Caja2.Text = "";
                    Caja3.Text = "";
                }
            }
            else if (sender == tablaPartidos)
            {
                if (tablaElecciones.SelectedIndex >= 0 && tablaPartidos.SelectedIndex >= 0)
                {
                    Partido part = (Partido)tablaPartidos.Items[tablaPartidos.SelectedIndex];

                    if (tablaElecciones.SelectedItems.Count == 1 && tablaPartidos.SelectedItems.Count == 1)
                    {
                        Caja5.Text = part.Nombre;
                        Caja6.Text = part.Escanios.ToString();
                        colorPicker.SelectedColor = part.Color;
                        Boton_Modificar2.IsEnabled = true;
                    }
                    Boton_Eliminar2.IsEnabled = true;
                }
                else
                {
                    Boton_Eliminar2.IsEnabled = false;
                    Boton_Modificar2.IsEnabled = false;
                    Caja5.Text = "";
                    Caja6.Text = "";
                    colorPicker.SelectedColor = null;
                }
            }
        }

        private void Caja1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Caja1.SelectedItem != null)
            {
                string opcionSeleccionada = ((ComboBoxItem)Caja1.SelectedItem).Content.ToString();
                if (opcionSeleccionada == "Elecciones Autonómicas")
                {
                    Caja2.IsEnabled = true;
                }
                else if (opcionSeleccionada == "Elecciones Generales")
                {
                    Caja2.IsEnabled = false;
                    Caja2.SelectedItem = null;
                }
            }
        }

        private void Anadir_Click(object sender, RoutedEventArgs e)
        {
            if (sender == Boton_Anadir)
            {
                Elecciones eleccion = new Elecciones();
                string tipo = "", lugar = "";
                DateTime? fecha = null;
                int escanios = 0;
                bool error = false;

                if (Caja1.Text == "")
                {
                    System.Windows.MessageBox.Show("Debe seleccionar el tipo en el desplegable \"Tipo\".", "ERROR", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    error = true;
                }
                else
                {
                    tipo = Caja1.Text;
                }

                if (tipo == "Elecciones Autonómicas")
                {
                    if (Caja2.Text == "")
                    {
                        System.Windows.MessageBox.Show("Debe seleccionar la comunidad autónoma en la casilla \"Lugar\".", "ERROR", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                        error = true;
                    }
                    else
                        lugar = Caja2.Text;

                    if (lugar == "Andalucía")
                        escanios = 109;
                    else if (lugar == "Aragón")
                        escanios = 67;
                    else if (lugar == "Asturias")
                        escanios = 45;
                    else if (lugar == "Islas Baleares")
                        escanios = 59;
                    else if (lugar == "Canarias")
                        escanios = 70;
                    else if (lugar == "Cantabria")
                        escanios = 35;
                    else if (lugar == "Castilla-La Mancha")
                        escanios = 33;
                    else if (lugar == "Castilla y León")
                        escanios = 81;
                    else if (lugar == "Cataluña")
                        escanios = 135;
                    else if (lugar == "Ceuta")
                        escanios = 25;
                    else if (lugar == "Comunidad Valenciana")
                        escanios = 99;
                    else if (lugar == "Extremadura")
                        escanios = 65;
                    else if (lugar == "Galicia")
                        escanios = 75;
                    else if (lugar == "La Rioja")
                        escanios = 33;
                    else if (lugar == "Comunidad de Madrid")
                        escanios = 136;
                    else if (lugar == "Melilla")
                        escanios = 25;
                    else if (lugar == "Región de Murcia")
                        escanios = 45;
                    else if (lugar == "Navarra")
                        escanios = 50;
                    else if (lugar == "País Vasco")
                        escanios = 75;
                }
                else
                {
                    lugar = "----------";
                    escanios = 350;
                }

                if (Caja3.SelectedDate != null)
                {
                    if (Caja3.SelectedDate > DateTime.Now)
                    {
                        System.Windows.MessageBox.Show("El valor de la casilla \"Fecha\" es inválido.\nLa fecha introducida es posterior a la actual, y por tanto no se tienen datos de esas elecciones.\nCompruebe los datos e inténtelo de nuevo.", "ERROR", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                        error = true;
                    }
                    else
                    {
                        fecha = Caja3.SelectedDate;
                    }
                }
                else
                {
                    System.Windows.MessageBox.Show("Debe introducir el valor de la casilla \"Fecha\".", "ERROR", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    error = true;
                }

                foreach (Elecciones el in listaElecciones)
                {
                    if (el.Tipo == tipo && el.Lugar == lugar && el.Fecha == fecha)
                    {
                        System.Windows.MessageBox.Show("Las elecciones que intenta introducir ya han sido añadidas.", "ERROR", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                        error = true;
                    }
                }

                if (error == false)
                {
                    eleccion.Tipo = tipo;
                    eleccion.Lugar = lugar;
                    eleccion.Fecha = fecha;
                    eleccion.Escanios = escanios;
                    eleccion.MayAbsoluta = (escanios / 2) + 1;

                    listaElecciones.Add(eleccion);

                    Caja1.Text = "";
                    Caja2.Text = "";
                    Caja2.IsEnabled = false;
                    Caja3.SelectedDate = null;
                }
            }
            else if (sender == Boton_Anadir2)
            {
                if (tablaElecciones.SelectedIndex >= 0)
                {
                    Elecciones el = new Elecciones();
                    el = (Elecciones)tablaElecciones.Items[tablaElecciones.SelectedIndex];
                    Partido partido = new Partido();
                    int escDisponibles = el.Escanios;
                    int sumaEscanios = 0;

                    string nombre = "";
                    int escanios = 0;
                    Color? color = null;
                    bool error = false;

                    if (Caja5.Text == "")
                    {
                        System.Windows.MessageBox.Show("Debe especficar el nombre del partido en la casilla \"Partido\".", "ERROR", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                        error = true;
                    }
                    else
                    {
                        if (Caja5.Text == Caja5.Text.ToUpper())
                        {
                            nombre = Caja5.Text;
                        }
                        else
                        {
                            System.Windows.MessageBox.Show("Las siglas o nombre del partido deben ir en mayúsculas.", "ERROR", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                            error = true;
                        }
                    }
                    if (Caja6.Text != "")
                    {
                        if (!int.TryParse(Caja6.Text, out escanios))
                        {
                            System.Windows.MessageBox.Show("El valor de la casilla \"Escaños\" no es válido.", "ERROR", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                            error = true;
                        }
                        if (escanios <= 0)
                        {
                            System.Windows.MessageBox.Show("El valor de la casilla \"Escaños\" no es válido.", "ERROR", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                            error = true;
                        }
                    }
                    if (escanios <= 0)
                    {
                        System.Windows.MessageBox.Show("El valor de la casilla \"Escaños\" no es válido.", "ERROR", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                        error = true;
                    }
                    if (colorPicker.SelectedColor != null)
                    {
                        color = colorPicker.SelectedColor;
                    }
                    else
                    {
                        System.Windows.MessageBox.Show("Debe elegir un color en la casilla \"Color\".", "ERROR", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                        error = true;
                    }
                    foreach (Partido part in el.Partidos)
                    {
                        if (part.Nombre == nombre)
                        {
                            System.Windows.MessageBox.Show("El nombre de partido que intenta introducir ya pertenece a un partido añadido.", "ERROR", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                            error = true;
                        }
                    }
                    foreach (Partido part in el.Partidos)
                    {
                        sumaEscanios = sumaEscanios + part.Escanios;
                    }
                    if ((sumaEscanios + escanios) > escDisponibles)
                    {
                        System.Windows.MessageBox.Show("Si se añade este partido, se sobrepasarán los escaños disponibles para el parlamento seleccionado.\nPor favor compruebe los datos y vuelva a intentarlo.", "ERROR", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                        error = true;
                    }
                    if (error == false)
                    {
                        partido.Nombre = nombre;
                        partido.Escanios = escanios;
                        partido.Color = color;

                        
                        AgregarOrdenado(partido);
                        AgregarPartidoOrdenado(partido);

                        Caja5.Text = "";
                        Caja6.Text = "";
                        colorPicker.SelectedColor = null;
                    }
                }
            }
        }

        private void PreCargar_Click(object sender, RoutedEventArgs e)
        {
            Elecciones res;
            String rutaArchivo = "datos\\Partidos.txt";

            try
            {
                string[] lineas = File.ReadAllLines(rutaArchivo);

                foreach (string linea in lineas)
                {
                    res = new Elecciones();
                    string[] datos = linea.Split('*');

                    res.Tipo = datos[0];

                    if (res.Tipo == "Elecciones Generales")
                    {
                        if (DateTime.TryParse(datos[1], out DateTime fecha))
                        {
                            res.Fecha = fecha;
                        }
                        if (int.TryParse(datos[2], out int escanios))
                        {
                            res.Escanios = escanios;
                            res.MayAbsoluta = (escanios / 2) + 1;
                        }
                        for (int i = 3; i < datos.Length; i += 3)
                        {
                            Partido pCargado = new Partido();
                            pCargado.Nombre = datos[i];
                            if (int.TryParse(datos[i + 1], out int escPart))
                            {
                                pCargado.Escanios = escPart;
                            }
                            string color = datos[i + 2];
                            pCargado.Color = (Color)ColorConverter.ConvertFromString(color);
                            res.Partidos.Add(pCargado);
                        }
                        res.Lugar = "----------";
                        listaElecciones.Add(res);
                    }
                    else if (res.Tipo == "Elecciones Autonómicas")
                    {
                        res.Lugar = datos[1];
                        if (DateTime.TryParse(datos[2], out DateTime fecha))
                        {
                            res.Fecha = fecha;
                        }
                        if (int.TryParse(datos[3], out int escanios))
                        {
                            res.Escanios = escanios;
                            res.MayAbsoluta = (escanios / 2) + 1;
                        }
                        for (int i = 4; i < datos.Length; i += 3)
                        {
                            Partido pCargado = new Partido();
                            pCargado.Nombre = datos[i];
                            if (int.TryParse(datos[i + 1], out int escPart))
                            {
                                pCargado.Escanios = escPart;
                            }
                            string color = datos[i + 2];
                            pCargado.Color = (Color)ColorConverter.ConvertFromString(color);
                            res.Partidos.Add(pCargado);
                        }
                        listaElecciones.Add(res);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Error al leer el archivo: Partidos.txt {ex.Message}"); ;
            }

            Boton_AnadirPrecargados.IsEnabled = false;
        }

        private void Modificar_Click(object sender, RoutedEventArgs e)
        {
            if (sender == Boton_Modificar)
            {
                if (tablaElecciones.SelectedItems.Count == 1)
                {
                    DateTime fecha;

                    if (Caja1.Text != "")
                        this.listaElecciones[tablaElecciones.SelectedIndex].Tipo = Caja1.Text;

                    if (Caja2.Text != "")
                        this.listaElecciones[tablaElecciones.SelectedIndex].Lugar = Caja2.Text;

                    if (DateTime.TryParse(Caja3.Text, out fecha))
                        this.listaElecciones[tablaElecciones.SelectedIndex].Fecha = fecha;
                    else
                        System.Windows.MessageBox.Show("El valor de la casilla \"Fecha\" no es válido.", "ERROR", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
                tablaElecciones.Items.Refresh();
            }
            if (sender == Boton_Modificar2)
            {
                Elecciones el = new Elecciones();
                el = (Elecciones)tablaElecciones.Items[tablaElecciones.SelectedIndex];

                if (tablaPartidos.SelectedItems.Count == 1)
                {
                    int escanios;

                    if (Caja5.Text != "")
                    {
                        if (Caja5.Text == Caja5.Text.ToUpper())
                        {
                            this.listaPartidos[tablaPartidos.SelectedIndex].Nombre = Caja5.Text;
                        }
                        else
                        {
                            System.Windows.MessageBox.Show("Las siglas o nombre del partido deben ir en mayúsculas.", "ERROR", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                        }
                    }
                    if (int.TryParse(Caja6.Text, out escanios))
                        this.listaPartidos[tablaPartidos.SelectedIndex].Escanios = escanios;
                    else
                        System.Windows.MessageBox.Show("El valor de la casilla \"Escaños\" no es válido.", "ERROR", MessageBoxButton.OK, MessageBoxImage.Exclamation);

                    if (colorPicker.SelectedColor != null)
                    {
                        this.listaPartidos[tablaPartidos.SelectedIndex].Color = colorPicker.SelectedColor;
                    }
                    else
                    {
                        System.Windows.MessageBox.Show("Debe elegir un color en la casilla \"Color\".", "ERROR", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    }

                    tablaPartidos.SelectedItem = null;

                    OrdenarPorEscaños();
                    listaPartidos.Clear();
                    foreach (Partido p in el.Partidos)
                    {
                        AgregarPartidoOrdenado(p);
                    }
                }
                tablaPartidos.Items.Refresh();
            }
        }

        private void Eliminar_Click(object sender, RoutedEventArgs e)
        {
            //https://stackoverflow.com/questions/21220467/remove-selected-items-from-listbox-observablecollection
            if (sender == Boton_Eliminar)
            {
                var selectedFiles = tablaElecciones.SelectedItems.Cast<Elecciones>().ToList();
                foreach (Elecciones item in selectedFiles)
                {
                    this.listaElecciones.Remove(item);
                }
                Boton_Eliminar.IsEnabled = false;
            }
            else if (sender == Boton_Eliminar2)
            {
                var selectedFiles = tablaPartidos.SelectedItems.Cast<Partido>().ToList();
                Elecciones elec = (Elecciones)tablaElecciones.Items[tablaElecciones.SelectedIndex];

                foreach (Partido item in selectedFiles)
                {
                    elec.Partidos.Remove(item);
                }
                listaPartidos.Clear();
                foreach (Partido item in elec.Partidos)
                {
                    AgregarPartidoOrdenado(item);
                }
                Boton_Eliminar.IsEnabled = false;
            }
        }

        private void GuardarDatos_Click(object sender, RoutedEventArgs e)
        {
            String rutaArchivo = "datos\\Partidos.txt";

            try
            {
                if (listaElecciones.Count >= 1)
                {
                    List<string> lineasGuardar = new List<string>();

                    // Comprobar si Boton_AnadirPrecargados está habilitado
                    if (Boton_AnadirPrecargados.IsEnabled)
                    {
                        // Si está habilitado, cargar datos existentes desde el archivo
                        string[] lineasExist = File.ReadAllLines(rutaArchivo);
                        lineasGuardar.AddRange(lineasExist);
                    }

                    foreach (Elecciones eleccion in listaElecciones)
                    {
                        StringBuilder linea = new StringBuilder();
                        linea.Append(eleccion.Tipo);
                        linea.Append("*");

                        if (eleccion.Tipo == "Elecciones Generales")
                        {
                            linea.Append(String.Format("{0:dd/MM/yyyy}", eleccion.Fecha));
                            linea.Append("*");
                            linea.Append(eleccion.Escanios.ToString());
                            linea.Append("*");

                            foreach (Partido partido in eleccion.Partidos)
                            {
                                linea.Append(partido.Nombre);
                                linea.Append("*");
                                linea.Append(partido.Escanios.ToString());
                                linea.Append("*");
                                linea.Append(partido.Color.ToString());
                                linea.Append("*");
                            }
                        }
                        else if (eleccion.Tipo == "Elecciones Autonómicas")
                        {
                            linea.Append(eleccion.Lugar);
                            linea.Append("*");
                            linea.Append(String.Format("{0:dd/MM/yyyy}", eleccion.Fecha));
                            linea.Append("*");
                            linea.Append(eleccion.Escanios.ToString());
                            linea.Append("*");

                            foreach (Partido partido in eleccion.Partidos)
                            {
                                linea.Append(partido.Nombre);
                                linea.Append("*");
                                linea.Append(partido.Escanios.ToString());
                                linea.Append("*");
                                linea.Append(partido.Color.ToString());
                                linea.Append("*");
                            }
                        }

                        // Eliminar el último asterisco
                        linea.Length--;
                        lineasGuardar.Add(linea.ToString());
                    }

                    File.WriteAllLines(rutaArchivo, lineasGuardar);
                    System.Windows.MessageBox.Show("Datos guardados correctamente.");
                }
                else
                {
                    System.Windows.MessageBox.Show("No hay Elecciones suficientes para guardar.", "ERROR", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Error al guardar el archivo: Partidos.txt {ex.Message}");
            }
        }


        public void AgregarOrdenado(Partido nuevoPartido)
        {
            Elecciones el = (Elecciones)tablaElecciones.Items[tablaElecciones.SelectedIndex];

            int index = 0;

            // Encontrar la posición correcta según el número de escaños
            while (index < el.Partidos.Count && nuevoPartido.Escanios < el.Partidos[index].Escanios)
            {
                index++;
            }

            // Insertar el nuevo partido en la posición correcta
            el.Partidos.Insert(index, nuevoPartido);
        }

        public void AgregarPartidoOrdenado(Partido nuevoPartido)
        {
            int index = 0;

            // Encontrar la posición correcta según el número de escaños
            while (index < listaPartidos.Count && nuevoPartido.Escanios < listaPartidos[index].Escanios)
            {
                index++;
            }

            // Insertar el nuevo partido en la posición correcta
            listaPartidos.Insert(index, nuevoPartido);
        }

        public void OrdenarPorEscaños()
        {
            Elecciones el = (Elecciones)tablaElecciones.Items[tablaElecciones.SelectedIndex];

            if (el.Partidos != null)
            {
                el.Partidos = el.Partidos.OrderByDescending(partido => partido.Escanios).ToList();
            }
        }

        private void Subir_Click(object sender, RoutedEventArgs e)
        {
            if (tablaElecciones.SelectedItems.Count == 1)
            {
                tablaElecciones.SelectedIndex = tablaElecciones.SelectedIndex - 1;
            }
        }

        private void Bajar_Click(object sender, RoutedEventArgs e)
        {
            if (tablaElecciones.SelectedItems.Count == 1)
            {
                tablaElecciones.SelectedIndex = tablaElecciones.SelectedIndex + 1;
            }
        }

        private void lista_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //https://stackoverflow.com/questions/30737097/wpf-how-deselect-all-my-selected-items-when-click-on-empty-space-in-my-listview

            if (sender == tablaElecciones)
            {
                HitTestResult r = VisualTreeHelper.HitTest(this, e.GetPosition(this));
                if (r.VisualHit.GetType() != typeof(ListBoxItem))
                    tablaElecciones.UnselectAll();
            }
            else if (sender == tablaPartidos)
            {
                HitTestResult r = VisualTreeHelper.HitTest(this, e.GetPosition(this));
                if (r.VisualHit.GetType() != typeof(ListBoxItem))
                    tablaPartidos.UnselectAll();
            }
        }

        private void IntroducirDatos_Loaded(object sender, RoutedEventArgs e)
        {
            int contErrores = 0;
            DateTime fecha1 = new DateTime(2023, 07, 23);
            DateTime fecha2 = new DateTime(2019, 11, 10);
            DateTime fecha3 = new DateTime(2022, 02, 14);
            DateTime fecha4 = new DateTime(2019, 05, 26);

            if (listaElecciones.Count > 0)
            {
                foreach (Elecciones item in listaElecciones)
                {
                    if (item.Tipo == "Elecciones Generales" && item.Fecha == fecha1)
                    {
                        contErrores += 1;
                    }
                    if (item.Tipo == "Elecciones Generales" && item.Fecha == fecha2)
                    {
                        contErrores += 1;
                    }
                    if (item.Tipo == "Elecciones Autonómicas" && item.Lugar == "Castilla y León" && item.Fecha == fecha3)
                    {
                        contErrores += 1;
                    }
                    if (item.Tipo == "Elecciones Autonómicas" && item.Lugar == "Castilla y León" && item.Fecha == fecha4)
                    {
                        contErrores += 1;
                    }
                }
                if (contErrores == 4)
                {
                    Boton_AnadirPrecargados.IsEnabled = false;
                }
            }
        }

        private void VentanaClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Boton_AnadirPrecargados.IsEnabled = false;
            if (listaPartidos.Count() > 0)
            {
                listaPartidos.Clear();
            }
        }

        private void Cerrar_Click(object sender, RoutedEventArgs e)
        {
            Boton_AnadirPrecargados.IsEnabled = false;
            if (listaPartidos.Count() > 0)
            {
                listaPartidos.Clear();
            }
            Close();
        }
    }
}
