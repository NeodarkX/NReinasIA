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
        }

        public async Task AgregandoANuevaPoblacion(Board hijo)
        {
        }

        public async Task MostrarPoblacion(List<Board> boards)
        {
            foreach(Board board in boards)
            {
                foreach (int v in board)
                    Console.Write(v + " ");
                Console.WriteLine();
            }
        }

        public async Task Mutando(Board hijo)
        {
        }

        public async Task Reproduciendo(Board X, Board Y)
        {
        }

        public async Task Solucion(Board solucion)
        {
        }

        public async Task TerminoNuevaPoblacion()
        {
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {

            GeneticQueen alg = new GeneticQueen(this);
            await alg.Resolver(8);
        }
    }
}
