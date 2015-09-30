using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
using NReinas.Algoritmo;
using NReinas.Estructuras;
using NReinas.UI;

namespace NReinas
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window,IGeneticQueen
    {
        GeneticQueen alg;
        int n = 0;
        //Reajuste este tamaño para que se vean mas generaciones
        private double BOARD_SIZE = 150;
        private double BOARD_MARGIN = 4;
        private int MaxColumns,Rows;

        public MainWindow()
        {
            InitializeComponent();
        }
        
        public async Task AgregandoANuevaPoblacion(int[] hijo)
        {
        }
        
        public async Task MostrarPoblacion(List<int[]> boards)
        {            
            int n = boards.Count;
            int i = 0;
            foreach (int[] board in boards)
            {
                BoardUI boardUI = new BoardUI(board, BOARD_SIZE - BOARD_MARGIN * 2);
                Grid.SetColumn(boardUI, (i % MaxColumns));
                Grid.SetRow(boardUI, i++ / MaxColumns);
                boardUI.Margin = new Thickness(BOARD_MARGIN);
                Poblacion.Children.Add(boardUI);
            }
            await Task.Delay(1);
        }

        public async Task Mutando(int[] hijo)
        {
        }

        public async Task Reproduciendo(int[] X, int[] Y)
        {
        }

        public async Task Solucion(int[] solucion)
        {
            int i = 0;
            BoardUI boardUI = new BoardUI(solucion, BOARD_SIZE - BOARD_MARGIN * 2);
            boardUI.Margin = new Thickness(BOARD_MARGIN);
            SolucionArea.Children.Clear();
            SolucionArea.Children.Add(boardUI);
            await Task.Delay(1);
        }

        int aux = 0;

        public async Task TerminoNuevaPoblacion(List<int[]> nuevaGeneracion)
        {
            int i = 0;
            //int n = Poblacion.ColumnDefinitions.Count;
            //Poblacion.Children.RemoveRange(0, n);
            Poblacion.Children.Clear();
            foreach (int[] board in nuevaGeneracion)
            {
                BoardUI boardUI = new BoardUI(board, BOARD_SIZE - BOARD_MARGIN * 2);
                Grid.SetColumn(boardUI, (i % MaxColumns));
                Grid.SetRow(boardUI, i++ / MaxColumns);
                boardUI.Margin = new Thickness(BOARD_MARGIN);
                Poblacion.Children.Add(boardUI);
            }
            await Task.Delay(1);
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            // Es es el boton de nuevo escondo y muestro el mensaje
            TextBPop.Visibility = Visibility.Visible;
            GridV.Visibility = Visibility.Visible;
            
        }

        private async void TextBPop_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Enter)
                {
                    n = Int32.Parse(TextBPop.Text);
                    if (n >= 0)//No permite negativos
                    {
                        //Setea los valores iniciales de la grilla
                        initGrid();
                        // Escondo el anuncio
                        GridV.Visibility = Visibility.Hidden;
                        alg = new GeneticQueen(this);
                        //Creo la primera generacion de 8x8 y tamaño de muestra n
                        await alg.CreateFirstGen(8,n);
                        //Escondo y muestro herramientas
                        TitleExecution.Visibility = Visibility.Visible;
                        GenShow.Visibility = Visibility.Visible;
                        Poblacion.Visibility = Visibility.Visible;
                    }
                    else
                        MessageBox.Show("Valores ingresados incorrectos", "Error");
                }
            }
            catch (FormatException fe)
            {
                MessageBox.Show("Valores ingresados incorrectos", "Error");
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //Para cerrar la ventana
            this.Close();
        }

        private async void GenShow_Click(object sender, RoutedEventArgs e)
        {
            //Al mostrar la genereacion limpio la poblacion 
            Poblacion.Children.Clear();
            //Muestro la barra lateral
            BarraLateral.Visibility = Visibility.Visible;
            initGrid();
            //Resolver...
            await alg.Resolver();
            //Inicio el slider en su minimum and maximum
            SliderGen.Minimum = 0;
            SliderGen.Maximum = alg.GeneracionesCount()-1;
            //Reinicio y escondo
            GenShow.Visibility = Visibility.Hidden;
            SliderGen.Visibility = Visibility.Visible;
        }



        private async void SliderGen_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            // al obtener el valor cambio durande el movimiento del slider lo convierto a entero
            int nvalue = Convert.ToInt32(e.NewValue);
            //lo busco dentro de la lista Generacion que contiene todas las generaciones
            await TerminoNuevaPoblacion(alg.ReturnGenByIndex(nvalue));
            //Cambio el titulo de cada generacion con el valor a string
            TitleExecution.Content = "Generacion " + Convert.ToString(nvalue);
        }

        private void Reset_Click(object sender, RoutedEventArgs e)
        {
            //limpio el objeto alg aunque creo que se necesita algo mas para limpiar :v
            alg = null;
            //El titulo lo reinicio al gen padre
            TitleExecution.Content = "Generacion Padre";
            // limpio la poblacion que se quedo en la anterior muestra
            Poblacion.Children.Clear();
            //Este codigo es de otra funcion es para reajustar el tamaño del grif o algo asi :v
            Poblacion.RowDefinitions.Clear();
            for (int i = 0; i < Rows; i++)
            {
                RowDefinition rd = new RowDefinition();
                rd.Height = new GridLength(BOARD_SIZE);
                Poblacion.RowDefinitions.Add(rd);
            }
            //oli boli :v
            Poblacion.Height = BOARD_SIZE;
            //Al reiniciar esconde los textos, botones, slider etc...
            TitleExecution.Visibility = Visibility.Hidden;
            GenShow.Visibility = Visibility.Hidden;
            SliderGen.Visibility = Visibility.Hidden;
            Poblacion.Visibility = Visibility.Hidden;
            BarraLateral.Visibility = Visibility.Collapsed;
        }

        private void initGrid()
        {
            //Calculo max columnas
            MaxColumns = (int)((Scroll.ActualWidth - BarraLateral.ActualWidth) / BOARD_SIZE);
            Poblacion.ColumnDefinitions.Clear();
            for (int i = 0; i < MaxColumns; i++)
            {
                ColumnDefinition cd = new ColumnDefinition();
                cd.Width = new GridLength(BOARD_SIZE);
                Poblacion.ColumnDefinitions.Add(cd);
            }
                Poblacion.Width = Math.Min(n,MaxColumns) * BOARD_SIZE;
            //Calculo numero de filas
            Rows = (n / MaxColumns) + 1;
            Poblacion.RowDefinitions.Clear();
            for (int i = 0; i < Rows; i++)
            {
                RowDefinition rd = new RowDefinition();
                rd.Height = new GridLength(BOARD_SIZE);
                Poblacion.RowDefinitions.Add(rd);
            }
            Poblacion.Height = Rows * BOARD_SIZE;
        }
    }
}
