using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using static System.Net.Mime.MediaTypeNames;

namespace ProyectoU1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DispatcherTimer dt1 = new DispatcherTimer();
        private DispatcherTimer dt2 = new DispatcherTimer();
        private DispatcherTimer dt3 = new DispatcherTimer();
        private DispatcherTimer SpriteTime = new DispatcherTimer();
        private Astar astar = new Astar();
        private Rectangle[,] cuadritos;
        private Nodo ene1;
        private Nodo ene2;
        private Nodo ene3;
        private Nodo final;
        private ImageBrush playerimage=new ImageBrush();
        private BitmapImage bimage1 = new BitmapImage();
        private BitmapImage bimage2 = new BitmapImage();
        bool sprite = false;
        public MainWindow()
        {
            InitializeComponent();
            InitializeTimers();
        }

        private void InitializeTimers()
        {
         
            bimage1.BeginInit();
            bimage1.UriSource = new Uri("pack://application:,,,/Assets/Player1.png", UriKind.RelativeOrAbsolute);
            bimage1.EndInit();
            playerimage.ImageSource = bimage1;
            
            bimage2.BeginInit();
            bimage2.UriSource = new Uri("pack://application:,,,/Assets/Player2.png", UriKind.RelativeOrAbsolute);
            bimage2.EndInit();
            playerimage.ImageSource = bimage2;


            dt1.Interval = TimeSpan.FromSeconds(0.5);
            dt1.Tick += Dp_Tick;
            dt1.Start();

            dt2.Interval = TimeSpan.FromSeconds(0.25);
            dt2.Tick += Dp2_Tick;
            dt2.Start();

            dt3.Interval = TimeSpan.FromSeconds(0.375);
            dt3.Tick += Dt3_Tick;
            dt3.Start();

            SpriteTime.Interval = TimeSpan.FromSeconds(0.5);
            SpriteTime.Tick += SpriteTime_Tick; ;
            SpriteTime.Start();
        }

        private void SpriteTime_Tick(object? sender, EventArgs e)
        {
            if(sprite==false)
            {
               
            
                playerimage.ImageSource = bimage1;
                sprite = true;
            }
            else
            {

           
                playerimage.ImageSource = bimage2;
                sprite = false;
            }

           
        }

        private void Dt3_Tick(object? sender, EventArgs e)
        {
            if (dt3.Interval.TotalSeconds > 0.2)
                dt3.Interval -= TimeSpan.FromSeconds(0.00001);
            MoverEnemigo(ene3);
        }

        private void Dp_Tick(object sender, EventArgs e)
        {
            if (dt1.Interval.TotalSeconds > 0.3)
                dt1.Interval -= TimeSpan.FromSeconds(0.00001);
            MoverEnemigo(ene1);
        }

        private void Dp2_Tick(object sender, EventArgs e)
        {
            if (dt2.Interval.TotalSeconds > 0.2)
                dt2.Interval -= TimeSpan.FromSeconds(0.00001);
            MoverEnemigo(ene2);
        }

        private void MoverEnemigo(Nodo enemigo)
        {
            if (final.Columna == enemigo.Columna && final.Renglon == enemigo.Renglon)
            {
                dt1.Stop();
                dt2.Stop();
                dt3.Stop();
                tablero.IsEnabled = false;
                return;
            }

            var s = astar.Buscar(enemigo, final);
            cuadritos[enemigo.Columna, enemigo.Renglon].Fill = Brushes.White;
            enemigo.Columna = s.Columna;
            enemigo.Renglon = s.Renglon;
            cuadritos[s.Columna, s.Renglon].Fill = Brushes.Red;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (JuegoHelper.Tablero != null)
            {
                tablero.Rows = JuegoHelper.Tablero.GetLength(0);
                tablero.Columns = JuegoHelper.Tablero.GetLength(1);
                cuadritos = new Rectangle[JuegoHelper.Tablero.GetLength(1), JuegoHelper.Tablero.GetLength(0)];

                Random r = new Random();
                ene1 = new Nodo() { Renglon = r.Next(0, 5), Columna = r.Next(0, 5) };
                ene2 = new Nodo() { Renglon = r.Next(25, 29), Columna = r.Next(25, 29) };
                ene3 = new Nodo() { Renglon = r.Next(15, 19), Columna = r.Next(15, 19) };
                final = new Nodo() { Renglon = 10, Columna = 10 };

                CrearTablero();
                CrearObstaculos();
            }
        }

        private void CrearTablero()
        {
            for (int i = 0; i < 30; i++)
            {
                for (int j = 0; j < 30; j++)
                {
                    tablero.Background = Brushes.White;
                    cuadritos[i, j] = new Rectangle() { Stroke = Brushes.Black, StrokeThickness = 0 };
                    tablero.Children.Add(cuadritos[i, j]);
                }
            }
            cuadritos[final.Columna, final.Renglon].Fill = Brushes.Yellow;
        }

        private void CrearObstaculos()
        {
            Random r = new Random();
            for (int i = 0; i < 80; i++)
            {
                int fila = r.Next(30);
                int columna = r.Next(30);
                cuadritos[columna, fila].Fill = Brushes.DarkSlateGray;
                JuegoHelper.Tablero[columna, fila] = true;
            }
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (final.Columna == ene1.Columna && final.Renglon == ene1.Renglon ||
                final.Columna == ene2.Columna && final.Renglon == ene2.Renglon ||
                final.Columna == ene3.Columna && final.Renglon == ene3.Renglon 
                )
            {
                dt1.Stop();
                dt2.Stop();
                dt3.Stop();
                tablero.IsEnabled = false;
                MessageBox.Show("Perdiste");
                return;
            }
          

            int col = final.Columna;
            int ren = final.Renglon;

            if (e.Key == Key.Up && final.Columna > 0 && !JuegoHelper.Tablero[final.Columna - 1, final.Renglon])
            {
                col = final.Columna - 1;
            }
            else if (e.Key == Key.Down && final.Columna < JuegoHelper.Tablero.GetLength(0) - 1 && !JuegoHelper.Tablero[final.Columna + 1, final.Renglon])
            {
                col = final.Columna + 1;
            }
            else if (e.Key == Key.Left && final.Renglon > 0 && !JuegoHelper.Tablero[final.Columna, final.Renglon - 1])
            {
                ren = final.Renglon - 1;
            }
            else if (e.Key == Key.Right && final.Renglon < JuegoHelper.Tablero.GetLength(1) - 1 && !JuegoHelper.Tablero[final.Columna, final.Renglon + 1])
            {
                ren = final.Renglon + 1;
            }

            cuadritos[final.Columna, final.Renglon].Fill = Brushes.White;
            cuadritos[col, ren].Fill = playerimage;
            final.Columna = col;
            final.Renglon = ren;

            JuegoHelper.ColumnaDestino = final.Columna;
            JuegoHelper.RenglonDestino = final.Renglon;
        }
    }

}

