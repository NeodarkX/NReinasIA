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
        public MainWindow()
        {
            InitializeComponent();

            for(int i = 0; i < 3; i++)
            {
                RowDefinition rd = new RowDefinition();
                rd.Height = new GridLength(BOARD_SIZE);
                Poblacion.RowDefinitions.Add(rd);
            }
            Poblacion.Height = BOARD_SIZE*3;

        }

        int contNueva = 0;

        private static TimeSpan delay = new TimeSpan(10);

        private List<int[]> nuevaGeneracion;

        public async Task AgregandoANuevaPoblacion(int[] hijo)
        {
            nuevaGeneracion.Add(hijo);

            /*BoardUI boardUI = new BoardUI(hijo, BOARD_SIZE - BOARD_MARGIN * 2);
            Grid.SetColumn(boardUI, contNueva++);
            Grid.SetRow(boardUI, 1);
            boardUI.Margin = new Thickness(BOARD_MARGIN);
            Poblacion.Children.Add(boardUI);
            await Task.Delay(1);*/
            
        }

        private int BOARD_SIZE = 70;
        private int BOARD_MARGIN = 4;
        public async Task MostrarPoblacion(List<int[]> boards)
        {
            
            int n = boards.Count;
            Poblacion.Width = BOARD_SIZE * n;
            int i = 0;
            foreach (int[] board in boards)
            {
                ColumnDefinition cd = new ColumnDefinition();
                cd.Width = new GridLength(BOARD_SIZE);
                Poblacion.ColumnDefinitions.Add(cd);
                BoardUI boardUI = new BoardUI(board, BOARD_SIZE - BOARD_MARGIN * 2);
                Grid.SetColumn(boardUI, i++);
                boardUI.Margin = new Thickness(BOARD_MARGIN);
                Poblacion.Children.Add(boardUI);
            }
            await Task.Delay(1);
            nuevaGeneracion = new List<int[]>(boards.Count);
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
            foreach (int[] board in nuevaGeneracion)
            {
                BoardUI boardUI = new BoardUI(board, BOARD_SIZE - BOARD_MARGIN * 2);
                Grid.SetColumn(boardUI, i++);
                Grid.SetRow(boardUI, 1);
                boardUI.Margin = new Thickness(BOARD_MARGIN);
                Poblacion.Children.Add(boardUI);
            }
            await Task.Delay(1);
        }

        int aux = 0;

        public async Task TerminoNuevaPoblacion()
        {
            int i = 0;
            foreach (int[] board in nuevaGeneracion)
            {
                BoardUI boardUI = new BoardUI(board, BOARD_SIZE - BOARD_MARGIN * 2);
                Grid.SetColumn(boardUI, i++);
                Grid.SetRow(boardUI, 1);
                boardUI.Margin = new Thickness(BOARD_MARGIN);
                Poblacion.Children.Add(boardUI);
            }
            await Task.Delay(1);

            contNueva = 0;
            int n = Poblacion.ColumnDefinitions.Count;
            int temp = n * 2;
            for (i = temp-1; i >= n; i--)
            {
                UIElement element = Poblacion.Children[i];
                Poblacion.Children.Remove(element);
            }
            nuevaGeneracion.Clear();
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {

            GeneticQueen alg = new GeneticQueen(this);

            await alg.Resolver(8);
        }
    }
}
