using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace El_Pactometro
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        IntroducirDatos intDatos = null;

        // Colecciones Observables
        ObservableCollection<Elecciones> listaElecciones = new ObservableCollection<Elecciones>();
        ObservableCollection<Partido> listaPartidos = new ObservableCollection<Partido>();

        //Listas complementarias para el pactómetro
        List<Partido> listaIzquierda = new List<Partido>();
        List<Partido> listaDerecha = new List<Partido>();

        Elecciones eleccionSeleccionada;

        //Color para el eje
        Color colorEje = Color.FromRgb(0, 0, 0);

        public MainWindow()
        {
            InitializeComponent();
            Loaded += Cargado;

            //Manejadores de cambios en las listas observables
            listaElecciones.CollectionChanged += ListaElecciones_CollectionChanged;
            listaPartidos.CollectionChanged += ListaPartidos_CollectionChanged;
        }

        private void ListaElecciones_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.OldItems != null)
            {
                if (listaElecciones.Count() <= 0)
                {
                    miCanvas.Children.Clear();
                    tipoGrafico.Content = "";
                    elecSeleccionada.Content = "";
                }
                else
                {
                    if ((String)tipoGrafico.Content == "GRÁFICO DE BARRAS")
                    {
                        DibujarBarras();
                    }
                    else if ((String)tipoGrafico.Content == "COMPARACIÓN DE ELECCIONES")
                    {
                        DibujarComparacion();
                    }
                    else if ((String)tipoGrafico.Content == "PACTÓMETRO")
                    {
                        DibujarPactos();
                    }
                    else
                    {
                        DibujarBarras();
                    }
                }
            }
            else
            {
                foreach (Elecciones elec in listaElecciones)
                {
                    elec.PropertyChanged += Elecciones_PropertyChanged;
                }
            }
        }

        private void Elecciones_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if ((String)tipoGrafico.Content == "GRÁFICO DE BARRAS")
            {
                DibujarBarras();
            }
            else if ((String)tipoGrafico.Content == "COMPARACIÓN DE ELECCIONES")
            {
                DibujarComparacion();
            }
            else if ((String)tipoGrafico.Content == "PACTÓMETRO")
            {
                DibujarPactos();
            }
            else
            {
                DibujarBarras();
            }
        }

        private void ListaPartidos_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.OldItems != null)
            {
                if (listaPartidos.Count() <= 0)
                {
                    if ((String)tipoGrafico.Content == "GRÁFICO DE BARRAS")
                    {
                        DibujarBarras();
                    }
                    else if ((String)tipoGrafico.Content == "COMPARACIÓN DE ELECCIONES")
                    {
                        DibujarComparacion();
                    }
                    else if ((String)tipoGrafico.Content == "PACTÓMETRO")
                    {
                        DibujarPactos();
                    }
                    else
                    {
                        DibujarBarras();
                    }
                }
                else
                {
                    if ((String)tipoGrafico.Content == "GRÁFICO DE BARRAS")
                    {
                        DibujarBarras();
                    }
                    else if ((String)tipoGrafico.Content == "COMPARACIÓN DE ELECCIONES")
                    {
                        DibujarComparacion();
                    }
                    else if ((String)tipoGrafico.Content == "PACTÓMETRO")
                    {
                        DibujarPactos();
                    }
                    else
                    {
                        DibujarBarras();
                    }
                }
            }
            else
            {
                if (listaPartidos.Count() >= 0)
                {
                    if ((String)tipoGrafico.Content == "GRÁFICO DE BARRAS")
                    {
                        DibujarBarras();
                    }
                    else if ((String)tipoGrafico.Content == "COMPARACIÓN DE ELECCIONES")
                    {
                        DibujarComparacion();
                    }
                    else if ((String)tipoGrafico.Content == "PACTÓMETRO")
                    {
                        DibujarPactos();
                    }
                    else
                    {
                        DibujarBarras();
                    }
                }
                foreach (Partido part in listaPartidos)
                {
                    part.PropertyChanged += Partido_PropertyChanged;
                }
            }
        }

        private void Partido_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if ((String)tipoGrafico.Content == "GRÁFICO DE BARRAS")
            {
                DibujarBarras();
            }
            else if ((String)tipoGrafico.Content == "COMPARACIÓN DE ELECCIONES")
            {
                DibujarComparacion();
            }
            else if ((String)tipoGrafico.Content == "PACTÓMETRO")
            {
                DibujarPactos();
            }
            else
            {
                DibujarBarras();
            }
        }

        private void IntDatos_seleccionEleccionesCambiada(object sender, SeleccionEleccionCambiaEventArgs e)
        {
            if (e.eleccionSeleccionada == null)
            {
                miCanvas.Children.Clear();
                miCanvas.Width = 797;
                tipoGrafico.Content = "NO HAY ELECCIONES SELECCIONADAS";
                elecSeleccionada.Content = "";
            }
            else
            {
                eleccionSeleccionada = e.eleccionSeleccionada;

                if ((String)tipoGrafico.Content == "GRÁFICO DE BARRAS")
                {
                    DibujarBarras();
                }
                else if ((String)tipoGrafico.Content == "COMPARACIÓN DE ELECCIONES")
                {
                    DibujarComparacion();
                }
                else if ((String)tipoGrafico.Content == "PACTÓMETRO")
                {
                    DibujarPactos();
                }
                else
                {
                    DibujarBarras();
                }
            }
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if ((String)tipoGrafico.Content == "GRÁFICO DE BARRAS")
            {
                miCanvas.Children.Clear();

                DibujarBarras();
            }
            else if ((String)tipoGrafico.Content == "COMPARACIÓN DE ELECCIONES")
            {
                miCanvas.Children.Clear();

                DibujarComparacion();
            }
            else if ((String)tipoGrafico.Content == "PACTÓMETRO")
            {
                miCanvas.Children.Clear();

                if (eleccionSeleccionada.Tipo == "Elecciones Generales")
                {
                    DibujarPactosEG();
                }
                else
                {
                    DibujarPactos();
                }
            }
        }

        private void Window_StateChanged(object sender, EventArgs e)
        {
            if (WindowState == WindowState.Maximized)
            {
                if ((String)tipoGrafico.Content == "GRÁFICO DE BARRAS")
                {
                    miCanvas.Children.Clear();

                    DibujarBarras();
                }
                else if ((String)tipoGrafico.Content == "COMPARACIÓN DE ELECCIONES")
                {
                    miCanvas.Children.Clear();

                    DibujarComparacion();
                }
                else if ((String)tipoGrafico.Content == "PACTÓMETRO")
                {
                    miCanvas.Children.Clear();

                    if (eleccionSeleccionada.Tipo == "Elecciones Generales")
                    {
                        DibujarPactosEG();
                    }
                    else
                    {
                        DibujarPactos();
                    }
                }
            }
            else if (WindowState == WindowState.Normal)
            {
                if ((String)tipoGrafico.Content == "GRÁFICO DE BARRAS")
                {
                    miCanvas.Children.Clear();

                    DibujarBarras();
                }
                else if ((String)tipoGrafico.Content == "COMPARACIÓN DE ELECCIONES")
                {
                    miCanvas.Children.Clear();

                    DibujarComparacion();
                }
                else if ((String)tipoGrafico.Content == "PACTÓMETRO")
                {
                    miCanvas.Children.Clear();

                    if (eleccionSeleccionada.Tipo == "Elecciones Generales")
                    {
                        DibujarPactosEG();
                    }
                    else
                    {
                        DibujarPactos();
                    }
                }
            }
        }

        private void dibujarEjes(Color colorEje)
        {
            miCanvas.Children.Clear();

            tipoGrafico.Content = "GRÁFICO DE BARRAS";
            botonIzq.Content = "COMPARACIÓN";
            botonDer.Content = "PACTÓMETRO";

            if (eleccionSeleccionada.Tipo == "Elecciones Generales")
            {
                elecSeleccionada.Content = string.Format("{0}, {1:dd/MM/yyyy}", eleccionSeleccionada.Tipo, eleccionSeleccionada.Fecha);
            }
            else if (eleccionSeleccionada.Tipo == "Elecciones Autonómicas")
            {
                elecSeleccionada.Content = string.Format("{0} de {1}, {2:dd/MM/yyyy}", eleccionSeleccionada.Tipo, eleccionSeleccionada.Lugar, eleccionSeleccionada.Fecha);
            }


            double ypantmin = 15;
            double ypantmax = miCanvas.ActualHeight;
            double ypantmaxmarcas = miCanvas.ActualHeight - 40;
            double yrealmin = 0;

            double escaniosDisp = eleccionSeleccionada.Escanios;

            Brush brochaEje = new SolidColorBrush(colorEje);

            for (double i = 0; i <= escaniosDisp; i += (Math.Round(escaniosDisp / 10)))
            {
                Line marca = new Line();
                marca.Stroke = brochaEje;
                marca.StrokeThickness = 2;
                marca.X1 = 0;
                marca.X2 = 5;
                marca.Y1 = (ypantmin - ypantmaxmarcas) * ((i - yrealmin) / (escaniosDisp - yrealmin)) + ypantmaxmarcas;
                marca.Y2 = (ypantmin - ypantmaxmarcas) * ((i - yrealmin) / (escaniosDisp - yrealmin)) + ypantmaxmarcas;

                Label etiqueta = new Label();
                etiqueta.Content = i;
                Point punto = new Point();
                punto.X = 8;
                punto.Y = (ypantmin - ypantmaxmarcas) * ((i - yrealmin) / (escaniosDisp - yrealmin)) + ypantmaxmarcas - 12;
                etiqueta.Foreground = brochaEje;
                etiqueta.Margin = new Thickness(punto.X, punto.Y, 0, 0);


                miCanvas.Children.Add(marca);
                miCanvas.Children.Add(etiqueta);
            }
        }

        private void dibujarEjesComparacion(Color colorEje)
        {
            miCanvas.Children.Clear();

            tipoGrafico.Content = "COMPARACIÓN DE ELECCIONES";
            botonIzq.Content = "GRÁFICO DE BARRAS";
            botonDer.Content = "PACTÓMETRO";

            if (eleccionSeleccionada.Tipo == "Elecciones Generales")
            {
                elecSeleccionada.Content = string.Format("{0}", eleccionSeleccionada.Tipo);
            }
            else if (eleccionSeleccionada.Tipo == "Elecciones Autonómicas")
            {
                elecSeleccionada.Content = string.Format("{0} de {1}", eleccionSeleccionada.Tipo, eleccionSeleccionada.Lugar);
            }

            double ypantmin = 15;
            double ypantmax = miCanvas.ActualHeight;
            double ypantmaxmarcas = miCanvas.ActualHeight - 40;
            double yrealmin = 0;

            double escaniosDisp = eleccionSeleccionada.Escanios;

            Brush brochaEje = new SolidColorBrush(colorEje);

            for (double i = 0; i <= escaniosDisp; i += (Math.Round(escaniosDisp / 10)))
            {
                Line marca = new Line();
                marca.Stroke = brochaEje;
                marca.StrokeThickness = 2;
                marca.X1 = 0;
                marca.X2 = 5;
                marca.Y1 = (ypantmin - ypantmaxmarcas) * ((i - yrealmin) / (escaniosDisp - yrealmin)) + ypantmaxmarcas;
                marca.Y2 = (ypantmin - ypantmaxmarcas) * ((i - yrealmin) / (escaniosDisp - yrealmin)) + ypantmaxmarcas;

                Label etiqueta = new Label();
                etiqueta.Content = i;
                Point punto = new Point();
                punto.X = 8;
                punto.Y = (ypantmin - ypantmaxmarcas) * ((i - yrealmin) / (escaniosDisp - yrealmin)) + ypantmaxmarcas - 12;
                etiqueta.Foreground = brochaEje;
                etiqueta.Margin = new Thickness(punto.X, punto.Y, 0, 0);


                miCanvas.Children.Add(marca);
                miCanvas.Children.Add(etiqueta);
            }
        }

        private void dibujarLeyenda(double xLeyenda, double yLeyenda, List<Elecciones> listaComparacion)
        {
            int numElecciones = listaComparacion.Count();

            double decrementoColor;

            Color color = Color.FromRgb(105, 105, 105);

            if (numElecciones < 7)
            {
                decrementoColor = 0.25;
            }
            else
            {
                decrementoColor = 1.0 / numElecciones;
            }

            for (int i = 0; i < numElecciones; i++)
            {
                Elecciones elec = listaComparacion[i];
                SolidColorBrush miBrocha = new SolidColorBrush(color);

                double aclararColor = 1.0 - i * decrementoColor;

                miBrocha.Color = Color.FromArgb(miBrocha.Color.A,
                        (byte)(color.R + (255 - color.R) * (1.0 - aclararColor)),
                        (byte)(color.G + (255 - color.G) * (1.0 - aclararColor)),
                        (byte)(color.B + (255 - color.B) * (1.0 - aclararColor)));

                Line marca = new Line();
                marca.Stroke = miBrocha;
                marca.StrokeThickness = 15;
                marca.X1 = xLeyenda;
                marca.X2 = marca.X1 + 35;
                marca.Y1 = yLeyenda;
                marca.Y2 = marca.Y1;

                Label etiqueta = new Label();
                etiqueta.Content = string.Format("{0:dd/MM/yyyy}", elec.Fecha);
                Point punto = new Point();
                punto.X = marca.X2 + 15;
                punto.Y = yLeyenda - 15;
                etiqueta.Foreground = Brushes.Black;
                etiqueta.Margin = new Thickness(punto.X, punto.Y, 0, 0);

                yLeyenda += 20;

                miCanvas.Children.Add(marca);
                miCanvas.Children.Add(etiqueta);
            }
        }

        private void dibujarEjePactos(Color colorEje)
        {
            tipoGrafico.Content = "PACTÓMETRO";
            botonIzq.Content = "COMPARACIÓN";
            botonDer.Content = "GRÁFICO DE BARRAS";

            if (eleccionSeleccionada.Tipo == "Elecciones Generales")
            {
                elecSeleccionada.Content = string.Format("{0}, {1:dd/MM/yyyy}", eleccionSeleccionada.Tipo, eleccionSeleccionada.Fecha);
            }
            else if (eleccionSeleccionada.Tipo == "Elecciones Autonómicas")
            {
                elecSeleccionada.Content = string.Format("{0} de {1}, {2:dd/MM/yyyy}", eleccionSeleccionada.Tipo, eleccionSeleccionada.Lugar, eleccionSeleccionada.Fecha);
            }


            double ypantmin = 15;
            double ypantmax = miCanvas.ActualHeight;
            double ypantmaxmarcas = miCanvas.ActualHeight - 8;
            double yrealmin = 0;

            double escaniosDisp = eleccionSeleccionada.Escanios;

            Brush brochaEje = new SolidColorBrush(colorEje);

            Line marca = new Line();
            marca.Stroke = brochaEje;
            marca.StrokeThickness = 1;
            marca.X1 = 0;
            marca.X2 = miCanvas.ActualWidth;
            marca.Y1 = (ypantmin - ypantmaxmarcas) * ((eleccionSeleccionada.MayAbsoluta - yrealmin) / (escaniosDisp - yrealmin)) + ypantmaxmarcas;
            marca.Y2 = marca.Y1;

            Label etiqueta = new Label();
            etiqueta.Content = string.Format("Mayoría Absoluta = {0}", eleccionSeleccionada.MayAbsoluta);
            Point punto = new Point();
            punto.X = 5;
            punto.Y = (ypantmin - ypantmaxmarcas) * ((eleccionSeleccionada.MayAbsoluta - yrealmin) / (escaniosDisp - yrealmin)) + ypantmaxmarcas - 5;
            etiqueta.Foreground = brochaEje;
            etiqueta.Margin = new Thickness(punto.X, punto.Y, 0, 0);

            miCanvas.Children.Add(marca);
            miCanvas.Children.Add(etiqueta);
        }

        private void DibujarBarras()
        {
            if (eleccionSeleccionada.Partidos.Count >= 0)
            {
                dibujarEjes(colorEje);

                double aumento;
                int numPartidos;

                double escDisponibles = eleccionSeleccionada.Escanios;

                numPartidos = eleccionSeleccionada.Partidos.Count();
                aumento = 75 * numPartidos;

                if (aumento < 800)
                {
                    miCanvas.Width = contenedor.ActualWidth;
                    scrollBarra.HorizontalScrollBarVisibility = ScrollBarVisibility.Hidden;
                }
                else if (miCanvas.Width < contenedor.ActualWidth)
                {
                    miCanvas.Width = contenedor.ActualWidth;
                    scrollBarra.HorizontalScrollBarVisibility = ScrollBarVisibility.Hidden;
                }
                else
                {
                    miCanvas.Width = 200 + aumento;
                    scrollBarra.HorizontalScrollBarVisibility = ScrollBarVisibility.Visible;
                }

                double posInicio;
                double posColocar = 0;

                double yDeInicio = miCanvas.ActualHeight - 38;
                double escPartido;

                double ypantmin = 15;
                double ypantmax = miCanvas.ActualHeight;
                double yrealmin = 0;

                foreach (Partido part in eleccionSeleccionada.Partidos)
                {
                    Color? colorPartido = part.Color;
                    Color color = colorPartido.Value;
                    SolidColorBrush miBrocha = new SolidColorBrush(color);

                    posInicio = 75 + posColocar;

                    escPartido = part.Escanios;
                    Line barra = new Line();
                    barra.Stroke = miBrocha;
                    barra.StrokeThickness = 40;

                    barra.X1 = posInicio;
                    barra.X2 = barra.X1;

                    barra.Y1 = yDeInicio;
                    barra.Y2 = (ypantmin - (yDeInicio)) * ((escPartido - yrealmin) / (escDisponibles - yrealmin)) + yDeInicio;

                    ToolTipService.SetToolTip(barra, $"{part.Nombre}, {part.Escanios} escaños");

                    barra.MouseEnter += Barra_MouseEnter;
                    barra.MouseLeave += Barra_MouseLeave;

                    miCanvas.Children.Add(barra);

                    posColocar = barra.X1;

                    Label nombre = new Label();

                    nombre.Content = part.Nombre;
                    Point punto = new Point();
                    punto.X = barra.X1 - 25;
                    punto.Y = barra.Y1;

                    nombre.Foreground = Brushes.Black;
                    nombre.Margin = new Thickness(punto.X, punto.Y, 0, 0);
                    miCanvas.Children.Add(nombre);
                }
            }
        }

        private void DibujarComparacion()
        {
            if (eleccionSeleccionada.Partidos.Count > 0)
            {
                dibujarEjesComparacion(colorEje);

                double aumento;
                int numPartidos, numElecciones, partidosTotales;

                double escDisponibles = eleccionSeleccionada.Escanios;

                //Listas para la comparación
                List<Elecciones> listaComparacion = new List<Elecciones>();
                List<Partido> partAgrupados = new List<Partido>();

                foreach (Elecciones elec in listaElecciones)
                {
                    if (eleccionSeleccionada.Tipo == "Elecciones Generales")
                    {
                        if (elec.Tipo == eleccionSeleccionada.Tipo)
                        {
                            listaComparacion.Add(elec);
                        }
                    }
                    if (eleccionSeleccionada.Tipo == "Elecciones Autonómicas")
                    {
                        if (elec.Tipo == eleccionSeleccionada.Tipo && elec.Lugar == eleccionSeleccionada.Lugar)
                        {
                            listaComparacion.Add(elec);
                        }
                    }
                }

                partAgrupados = AgruparPartidos(listaComparacion);

                numElecciones = listaComparacion.Count();
                numPartidos = partAgrupados.Count() / numElecciones;
                partidosTotales = partAgrupados.Count();
                aumento = (25 * numElecciones + 35) * numPartidos;

                if (aumento < 800)
                {
                    miCanvas.Width = contenedor.ActualWidth;
                    scrollBarra.HorizontalScrollBarVisibility = ScrollBarVisibility.Hidden;
                }
                else if (miCanvas.Width < contenedor.ActualWidth)
                {
                    miCanvas.Width = contenedor.ActualWidth;
                    scrollBarra.HorizontalScrollBarVisibility = ScrollBarVisibility.Hidden;
                }
                else
                {
                    miCanvas.Width = 200 + aumento;
                    scrollBarra.HorizontalScrollBarVisibility = ScrollBarVisibility.Visible;
                }

                double posInicio = 75;
                double posLabel = posInicio;

                double yDeInicio = miCanvas.ActualHeight - 38;
                double escPartido;

                double ypantmin = 15;
                double ypantmax = miCanvas.ActualHeight;
                double yrealmin = 0;

                double xLeyenda = contenedor.ActualWidth - 150;
                double yLeyenda = 30;

                dibujarLeyenda(xLeyenda, yLeyenda, listaComparacion);

                for (int i = 0; i < partidosTotales; i += numElecciones)
                {
                    Partido partido = partAgrupados[i];
                    Color? colorPartido = partido.Color;
                    Color color = colorPartido.Value;

                    double decrementoColor;

                    if (numElecciones < 7)
                    {
                        decrementoColor = 0.25;
                    }
                    else
                    {
                        decrementoColor = 1.0 / numElecciones;
                    }

                    for (int j = 0; j < numElecciones; j++)
                    {
                        Partido part = partAgrupados[i+j];
                        SolidColorBrush miBrocha = new SolidColorBrush(color);

                        double aclararColor = 1.0 - j * decrementoColor;

                        miBrocha.Color = Color.FromArgb(miBrocha.Color.A,
                                (byte)(color.R + (255 - color.R) * (1.0 - aclararColor)),
                                (byte)(color.G + (255 - color.G) * (1.0 - aclararColor)),
                                (byte)(color.B + (255 - color.B) * (1.0 - aclararColor)));

                        //Barra por cada información que se tiene de un partido
                        escPartido = part.Escanios;
                        Line barra = new Line();
                        barra.Stroke = miBrocha;
                        barra.StrokeThickness = 20;

                        barra.X1 = posInicio;
                        barra.X2 = barra.X1;

                        barra.Y1 = yDeInicio;
                        barra.Y2 = (ypantmin - (yDeInicio)) * ((escPartido - yrealmin) / (escDisponibles - yrealmin)) + yDeInicio;

                        ToolTipService.SetToolTip(barra, $"{part.Nombre}, {part.Escanios} escaños");

                        barra.MouseEnter += Barra_MouseEnter;
                        barra.MouseLeave += Barra_MouseLeave;

                        miCanvas.Children.Add(barra);

                        posInicio = barra.X1 + 25;
                    }
                    Label nombre = new Label();

                    nombre.Content = partido.Nombre;
                    Point punto = new Point();
                    punto.X = posLabel - 15;
                    punto.Y = yDeInicio;

                    nombre.Foreground = Brushes.Black;
                    nombre.Margin = new Thickness(punto.X, punto.Y, 0, 0);
                    miCanvas.Children.Add(nombre);

                    posInicio += 40;
                    posLabel = posInicio;
                }
            }
            else
            {
                DibujarBarras();
                System.Windows.MessageBox.Show("No hay partidos suficientes para comparar en las Elecciones seleccionadas.", "ERROR", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        private void DibujarPactosEG()
        {
            if (eleccionSeleccionada.Partidos.Count > 0)
            {
                miCanvas.Width = contenedor.ActualWidth;

                listaIzquierda.Clear();
                listaDerecha.Clear();

                double posInicioIzq = miCanvas.ActualWidth * 0.230;
                double posInicioDer = miCanvas.ActualWidth * 0.533;

                double yDeInicioIzquierda = miCanvas.ActualHeight - 8;
                double yDeInicioDerecha = miCanvas.ActualHeight - 8;

                if (eleccionSeleccionada.Partidos[0].Escanios > eleccionSeleccionada.MayAbsoluta) // Se comprueba si el primer partido supera la Mayoría Absoluta, 
                {                                                                                 // y si es así, se dibujará solo en la izquierda
                    listaIzquierda.Add(eleccionSeleccionada.Partidos[0]);

                    for (int i = 1; i < eleccionSeleccionada.Partidos.Count; i++)
                    {
                        listaDerecha.Add(eleccionSeleccionada.Partidos[i]);
                    }
                }
                else                                                                              // En caso de que no ocurra, se añadirán solo los dos primeros
                {
                    for (int i = 0; i < eleccionSeleccionada.Partidos.Count; i++)
                    {
                        Partido part = eleccionSeleccionada.Partidos[i];

                        if (i < 2) // Los dos primeros partidos van a la izquierda, pues al estar ordenados por escaños, superarán la mayoría absoluta casi seguro
                        {
                            listaIzquierda.Add(part);
                        }
                        else // El resto va a la derecha
                        {
                            listaDerecha.Add(part);
                        }
                    }
                }

                DibujarBarraIzq(listaIzquierda, posInicioIzq, ref yDeInicioIzquierda);
                DibujarBarraDer(listaDerecha, posInicioDer, ref yDeInicioDerecha);
                dibujarEjePactos(colorEje);
            }
        }

        private void DibujarPactos()
        {
            if (eleccionSeleccionada.Partidos.Count > 0)
            {
                miCanvas.Width = contenedor.ActualWidth;

                listaIzquierda.Clear();
                listaDerecha.Clear();

                double posInicioIzq = miCanvas.ActualWidth * 0.305;
                double posInicioDer = miCanvas.ActualWidth * 0.693;
                
                double yDeInicioIzquierda = miCanvas.ActualHeight - 8;
                double yDeInicioDerecha = miCanvas.ActualHeight - 8;

                if (eleccionSeleccionada.Partidos[0].Escanios > eleccionSeleccionada.MayAbsoluta) // Se comprueba si el primer partido supera la Mayoría Absoluta, 
                {                                                                                 // y si es así, se dibujará solo en la izquierda
                    listaIzquierda.Add(eleccionSeleccionada.Partidos[0]);

                    for (int i = 1; i < eleccionSeleccionada.Partidos.Count; i++)
                    {
                        listaDerecha.Add(eleccionSeleccionada.Partidos[i]);
                    }
                }
                else                                                                              // En caso de que no ocurra, se añadirán solo los dos primeros
                {
                    for (int i = 0; i < eleccionSeleccionada.Partidos.Count; i++)
                    {
                        Partido part = eleccionSeleccionada.Partidos[i];

                        if (i < 2) // Los dos primeros partidos van a la izquierda, pues al estar ordenados por escaños, superarán la mayoría absoluta casi seguro
                        {
                            listaIzquierda.Add(part);
                        }
                        else // El resto va a la derecha
                        {
                            listaDerecha.Add(part);
                        }
                    }
                }

                DibujarBarraIzq(listaIzquierda, posInicioIzq, ref yDeInicioIzquierda);
                DibujarBarraDer(listaDerecha, posInicioDer, ref yDeInicioDerecha);
                dibujarEjePactos(colorEje);
            }
        }

        private void DibujarBarraIzq(List<Partido> listaPactos, double posInicio, ref double yDeInicio)
        {
            miCanvas.Children.Clear();

            double ypantmin = 15;
            double ypantmax = miCanvas.ActualHeight - 8;
            double posEtiqueta = 0;

            double escDisponibles = eleccionSeleccionada.Escanios;
            double sumaIzq = 0;

            foreach (Partido part in listaPactos)
            {
                Color? colorPartido = part.Color;
                Color color = colorPartido.Value;
                SolidColorBrush miBrocha = new SolidColorBrush(color);

                int escPartido = part.Escanios;
                sumaIzq += part.Escanios;
                Line barra = new Line();
                barra.Stroke = miBrocha;
                barra.StrokeThickness = 150;

                barra.X1 = posInicio;
                barra.X2 = barra.X1;
                posEtiqueta = barra.X1;

                barra.Y1 = yDeInicio;
                barra.Y2 = Math.Max(yDeInicio - (escPartido / escDisponibles) * (ypantmax - ypantmin), ypantmin);

                ToolTipService.SetToolTip(barra, $"{part.Nombre}, {part.Escanios} escaños");

                miCanvas.Children.Add(barra);

                barra.MouseEnter += Barra_MouseEnter;
                barra.MouseLeave += Barra_MouseLeave;

                barra.MouseDown += (sender, e) => MoverPartidoAOtraBarra(part, 0);

                yDeInicio = barra.Y2;

                if (eleccionSeleccionada.Tipo == "Elecciones Generales")
                {
                    if (part.Escanios > 20)
                    {
                        Label nombre = new Label();

                        nombre.Content = part.Nombre;
                        Point punto = new Point();
                        punto.X = barra.X1 + 75;
                        punto.Y = barra.Y2;

                        nombre.Foreground = Brushes.Black;
                        nombre.Margin = new Thickness(punto.X, punto.Y, 0, 0);
                        miCanvas.Children.Add(nombre);
                    }
                }
                else if (eleccionSeleccionada.Tipo == "Elecciones Autonómicas")
                {
                    if (part.Escanios > 5)
                    {
                        Label nombre = new Label();

                        nombre.Content = part.Nombre;
                        Point punto = new Point();
                        punto.X = barra.X1 + 70;
                        punto.Y = barra.Y2 + 20;

                        nombre.Foreground = Brushes.Black;
                        nombre.Margin = new Thickness(punto.X, punto.Y, 0, 0);
                        miCanvas.Children.Add(nombre);
                    }
                }
            }

            Label etiqueta2 = new Label();
            etiqueta2.Content = string.Format("Coalición\nEscaños = {0}", sumaIzq);
            Point punto2 = new Point();
            punto2.X = posEtiqueta - 170;
            punto2.Y = miCanvas.ActualHeight - 50;
            etiqueta2.Foreground = Brushes.Black;
            etiqueta2.Margin = new Thickness(punto2.X, punto2.Y, 0, 0);

            miCanvas.Children.Add(etiqueta2);
        }

        private void DibujarBarraDer(List<Partido> listaPactos, double posInicio, ref double yDeInicio)
        {
            double ypantmin = 15;
            double ypantmax = miCanvas.ActualHeight - 8;
            double posEtiqueta = 0;

            double escDisponibles = eleccionSeleccionada.Escanios;
            double sumaDer = 0;

            foreach (Partido part in listaPactos)
            {
                Color? colorPartido = part.Color;
                Color color = colorPartido.Value;
                SolidColorBrush miBrocha = new SolidColorBrush(color);

                int escPartido = part.Escanios;
                sumaDer += part.Escanios;
                Line barra = new Line();
                barra.Stroke = miBrocha;
                barra.StrokeThickness = 150;

                barra.X1 = posInicio;
                barra.X2 = barra.X1;
                posEtiqueta = barra.X1;

                barra.Y1 = yDeInicio;
                barra.Y2 = Math.Max(yDeInicio - (escPartido / escDisponibles) * (ypantmax - ypantmin), ypantmin);

                ToolTipService.SetToolTip(barra, $"{part.Nombre}, {part.Escanios} escaños");

                miCanvas.Children.Add(barra);

                barra.MouseEnter += Barra_MouseEnter;
                barra.MouseLeave += Barra_MouseLeave;

                barra.MouseDown += (sender, e) => MoverPartidoAOtraBarra(part, 1);

                yDeInicio = barra.Y2;

                if (eleccionSeleccionada.Tipo == "Elecciones Generales")
                {
                    if (part.Escanios > 20)
                    {
                        Label nombre = new Label();

                        nombre.Content = part.Nombre;
                        Point punto = new Point();
                        punto.X = barra.X1 + 75;
                        punto.Y = barra.Y2;

                        nombre.Foreground = Brushes.Black;
                        nombre.Margin = new Thickness(punto.X, punto.Y, 0, 0);
                        miCanvas.Children.Add(nombre);
                    }
                }
                else if (eleccionSeleccionada.Tipo == "Elecciones Autonómicas")
                {
                    if (part.Escanios > 5)
                    {
                        Label nombre = new Label();

                        nombre.Content = part.Nombre;
                        Point punto = new Point();
                        punto.X = barra.X1 + 70;
                        punto.Y = barra.Y2 + 20;

                        nombre.Foreground = Brushes.Black;
                        nombre.Margin = new Thickness(punto.X, punto.Y, 0, 0);
                        miCanvas.Children.Add(nombre);
                    }
                }
            }

            Label etiqueta3 = new Label();
            etiqueta3.Content = string.Format("Resto de partidos\nEscaños = {0}", sumaDer);
            Point punto3 = new Point();
            punto3.X = posEtiqueta + 125;
            punto3.Y = miCanvas.ActualHeight - 50;
            etiqueta3.Foreground = Brushes.Black;
            etiqueta3.Margin = new Thickness(punto3.X, punto3.Y, 0, 0);

            miCanvas.Children.Add(etiqueta3);
        }

        private void MoverPartidoAOtraBarra(Partido partido, int destino)
        {
            // Lógica para mover el partido de una lista a otra
            if (destino == 1)
            {
                // Mover a la lista izquierda
                listaDerecha.Remove(partido);
                listaIzquierda.Add(partido);
            }
            else if (destino == 0)
            {
                // Mover a la lista derecha
                listaIzquierda.Remove(partido);
                listaDerecha.Add(partido);
            }

            // Redibujar las barras
            double posInicioIzq = miCanvas.ActualWidth * 0.317;
            double posInicioDer = miCanvas.ActualWidth * 0.693;
            double yDeInicioIzquierda = miCanvas.ActualHeight - 8;
            double yDeInicioDerecha = miCanvas.ActualHeight - 8;

            DibujarBarraIzq(listaIzquierda, posInicioIzq, ref yDeInicioIzquierda);
            DibujarBarraDer(listaDerecha, posInicioDer, ref yDeInicioDerecha);
            dibujarEjePactos(colorEje);
        }


        static List<Partido> AgruparPartidos(List<Elecciones> elecciones)
        {
            // Diccionario para realizar el seguimiento de los partidos, sus escaños y color para cada elección
            Dictionary<string, List<(int Escanios, Color? Color)>> partidosDiccionario = new Dictionary<string, List<(int, Color?)>>();

            // Llenar el diccionario
            foreach (Elecciones eleccion in elecciones)
            {
                foreach (Partido partido in eleccion.Partidos)
                {
                    if (!partidosDiccionario.ContainsKey(partido.Nombre))
                    {
                        partidosDiccionario[partido.Nombre] = new List<(int, Color?)>();
                    }
                    partidosDiccionario[partido.Nombre].Add((partido.Escanios, partido.Color));
                }
            }

            // Crear la lista final de partidos
            List<Partido> partidosAgrupados = new List<Partido>();

            foreach (var kvp in partidosDiccionario)
            {
                var nombrePartido = kvp.Key;
                var escañosYColoresPorEleccion = kvp.Value;

                // Agregar una entrada para cada elección, con 0 escaños y color correspondiente si el partido no estaba en esa elección
                for (int i = 0; i < elecciones.Count; i++)
                {
                    var (escaños, color) = i < escañosYColoresPorEleccion.Count ? escañosYColoresPorEleccion[i] : (0, null);
                    partidosAgrupados.Add(new Partido { Nombre = nombrePartido, Escanios = escaños, Color = color });
                }
            }

            return partidosAgrupados;
        }


        private void Barra_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (sender is Line barra)
            {
                ToolTipService.SetIsEnabled(barra, true);
            }
        }

        private void Barra_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (sender is Line barra)
            {
                ToolTipService.SetIsEnabled(barra, false);
            }
        }

        private async void Cargado(object sender, RoutedEventArgs e)
        {
            //Ventana secundaria cerrada
            intDatos = null;

            //Mensaje de bienvenida
            tipoGrafico.Content = "BIENVENID@ AL PACTÓMETRO";

            // Simula un proceso de carga
            await Task.Delay(3000);

            // Inicia la animación de desvanecimiento
            FadeOutLoadingScreen();
        }
        
        private void FadeOutLoadingScreen()
        {
            DoubleAnimation fadeOutAnimation = new DoubleAnimation
            {
                From = 1.0,
                To = 0.0,
                Duration = TimeSpan.FromSeconds(1),
                FillBehavior = FillBehavior.Stop
            };

            fadeOutAnimation.Completed += (s, e) =>
            {
                // Oculta la pantalla de carga cuando la animación ha terminado
                loadingGrid.Visibility = Visibility.Hidden;
                Menu.Visibility = Visibility.Visible;
                miGrid.Visibility = Visibility.Visible;
            };

            loadingGrid.BeginAnimation(Grid.OpacityProperty, fadeOutAnimation);
        }

        private void IntDatos_Click(object sender, RoutedEventArgs e)
        {
            if (intDatos == null)
            {
                intDatos = new IntroducirDatos(listaElecciones, listaPartidos);
                intDatos.Closed += IntDatos_Closed;
                intDatos.seleccionEleccionesCambiada += IntDatos_seleccionEleccionesCambiada;
                intDatos.Owner = this;
            }
            if (listaElecciones.Count != 0)
            {
                if ((String)tipoGrafico.Content == "GRÁFICO DE BARRAS")
                {
                    miCanvas.Children.Clear();

                    DibujarBarras();
                }
                else if ((String)tipoGrafico.Content == "COMPARACIÓN DE ELECCIONES")
                {
                    miCanvas.Children.Clear();

                    DibujarComparacion();
                }
                else if ((String)tipoGrafico.Content == "PACTÓMETRO")
                {
                    miCanvas.Children.Clear();

                    if (eleccionSeleccionada.Tipo == "Elecciones Generales")
                    {
                        DibujarPactosEG();
                    }
                    else
                    {
                        DibujarPactos();
                    }
                }
            }
            intDatos.Show();
        }

        private void botonIzquierdo_Click(object sender, RoutedEventArgs e)
        {
            if ((String)tipoGrafico.Content == "GRÁFICO DE BARRAS")
            {
                int contador = 0;

                foreach (Elecciones elec in listaElecciones)
                {
                    if (eleccionSeleccionada.Tipo == "Elecciones Generales")
                    {
                        if (elec.Tipo == eleccionSeleccionada.Tipo)
                        {
                            contador++;
                        }
                    }
                    if (eleccionSeleccionada.Tipo == "Elecciones Autonómicas")
                    {
                        if (elec.Tipo == eleccionSeleccionada.Tipo && elec.Lugar == eleccionSeleccionada.Lugar)
                        {
                            contador++;
                        }
                    }
                }
                if (contador > 1)
                {
                    miCanvas.Children.Clear();

                    tipoGrafico.Content = "COMPARACIÓN DE ELECCIONES";

                    DibujarComparacion();
                }
                else
                {
                    System.Windows.MessageBox.Show("No hay suficientes Elecciones del mismo tipo para comparar.", "ERROR", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
            }
            else if ((String)tipoGrafico.Content == "COMPARACIÓN DE ELECCIONES")
            {
                miCanvas.Children.Clear();

                tipoGrafico.Content = "GRÁFICO DE BARRAS";

                DibujarBarras();
            }
            if ((String)tipoGrafico.Content == "PACTÓMETRO")
            {
                int contador = 0;

                foreach (Elecciones elec in listaElecciones)
                {
                    if (eleccionSeleccionada.Tipo == "Elecciones Generales")
                    {
                        if (elec.Tipo == eleccionSeleccionada.Tipo)
                        {
                            contador++;
                        }
                    }
                    if (eleccionSeleccionada.Tipo == "Elecciones Autonómicas")
                    {
                        if (elec.Tipo == eleccionSeleccionada.Tipo && elec.Lugar == eleccionSeleccionada.Lugar)
                        {
                            contador++;
                        }
                    }
                }
                if (contador > 1)
                {
                    miCanvas.Children.Clear();

                    tipoGrafico.Content = "COMPARACIÓN DE ELECCIONES";

                    DibujarComparacion();
                }
                else
                {
                    System.Windows.MessageBox.Show("No hay suficientes Elecciones del mismo tipo para comparar.", "ERROR", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
            }
        }

        private void botonDerecho_Click(object sender, RoutedEventArgs e)
        {
            if ((String)tipoGrafico.Content == "GRÁFICO DE BARRAS")
            {
                miCanvas.Children.Clear();
                miCanvas.Width = contenedor.ActualWidth;

                tipoGrafico.Content = "PACTÓMETRO";

                if (eleccionSeleccionada.Tipo == "Elecciones Generales")
                {
                    DibujarPactosEG();
                }
                else
                {
                    DibujarPactos();
                }
                
            }
            else if ((String)tipoGrafico.Content == "PACTÓMETRO")
            {
                miCanvas.Children.Clear();

                tipoGrafico.Content = "GRÁFICO DE BARRAS";

                DibujarBarras();
            }
            else if ((String)tipoGrafico.Content == "COMPARACIÓN DE ELECCIONES")
            {
                miCanvas.Children.Clear();
                miCanvas.Width = contenedor.ActualWidth;

                tipoGrafico.Content = "PACTÓMETRO";

                if (eleccionSeleccionada.Tipo == "Elecciones Generales")
                {
                    DibujarPactosEG();
                }
                else
                {
                    DibujarPactos();
                }
            }
        }

        private void IntDatos_Closed(object sender, EventArgs e)
        {
            intDatos = null;
        }

        private void GuardarImagen_Click(object sender, RoutedEventArgs e)
        {
            miCanvas.Background = Brushes.Beige;
            Visual pantalla = miCanvas;
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.FileName = "Grafica"; // Default file name
            dlg.DefaultExt = ".png"; // Default file extension
            dlg.Filter = "PNG File (.png)|*.png"; // Filter files by extension

            // Show save file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            // Process save file dialog box results
            if (result == true)
            {
                // Save document
                string filename = dlg.FileName;

                double dpiX = 96.0;
                double dpiY = 96.0;

                var rtb = new RenderTargetBitmap(
                    (int)miCanvas.ActualWidth, // width
                    (int)miCanvas.ActualHeight, // height
                    dpiX, // dpi x
                    dpiY, // dpi y
                    PixelFormats.Pbgra32 // pixelformat
                );

                // Render the canvas onto the RenderTargetBitmap
                rtb.Render(pantalla);

                var enc = new System.Windows.Media.Imaging.PngBitmapEncoder();
                enc.Frames.Add(System.Windows.Media.Imaging.BitmapFrame.Create(rtb));

                using (var stm = System.IO.File.Create(filename))
                    enc.Save(stm);
            }
            miCanvas.Background = null;
        }

        private void Ayuda_Click(object sender, RoutedEventArgs e)
        {
            String ayuda = "¡Hola!\n\n" +
                "Esta aplicación permite consultar a través de gráficos los resultados de las diferentes elecciones del territorio español, ya sean elecciones generales o autonómicas.\n\n" +
                "Para empezar a introducir estos datos de las elecciones, haga \"Click\" en el botón de la parte superior izquierda \"Introducir Datos\".\n\n" +
                "Se le abrirá una ventana donde encontrará distintas opciones para añadir, modificar o eliminar elecciones, así como los respectivos partidos participantes en ellas.\n\n" +
                "Además, encontrará la opción de \"Pre-Cargados\" que le permitirá cargar algunos datos de manera rápida para probar la aplicación.\n" +
                "Los nuevos datos introducidos, si los quiere conservar, podrá a través del botón \"Guardar\", así, la próxima vez que utilice la precarga, aparecerán los nuevos datos que había añadido.\n\n" +
                "Podrá visualizar los datos en un gráfico de barras, compararlos con elecciones de otro años y hacer coaliciones gracias al PACTÓMETRO.\n\n" +
                "¡Espero que la aplicación le sea útil y sea de su agrado!\n\n" +
                "Unai García Blanco, para Manipulaciones y Bulos S.A.";
            MessageBox.Show(ayuda, "Ayuda", MessageBoxButton.OK, MessageBoxImage.Question);
        }
    }
}
