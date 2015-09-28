using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NReinas.Estructuras;

namespace NReinas.Algoritmo
{
    public class GeneticQueen

    {
        private Random rnd;
        private Board mejorTablero = null;


        private int _probabilidadDeMutar;
        public int ProbabilidadDeMutar
        {
            get { return _probabilidadDeMutar; }
            set
            {
                if (value < 0 || value > 100)
                    throw new ArgumentOutOfRangeException("Probabilidad", "La probabilidad debe estar entre 0 y 100");
                _probabilidadDeMutar = value;
            }
        }
        public IGeneticQueen Listener;

        public GeneticQueen(IGeneticQueen pListener)
        {
            ProbabilidadDeMutar = 100;
            rnd = new Random();
            Listener = pListener;
        }

        public async Task<Board> Resolver(int N)
        {
            int np = 30;
            List<Board> poblacion = new List<Board>();
            for (int i = 0; i < np; ++i)
                poblacion.Add(Board.GenerarAleatorio(N));
            await Listener.MostrarPoblacion(poblacion);
            mejorTablero = await AlgoritmoGenetico(poblacion, 0);
            return mejorTablero;
        }

        private async Task<Board> AlgoritmoGenetico(List<Board> poblacion, int idoneo)
        {
            int c = 0;
            while (true)
            {
                List<Board> nueva_poblacion = new List<Board>(poblacion.Count);
                for (int i = 0; i < poblacion.Count; i++)
                {
                    //Escoger dos elementos de la poblacion al azar
                    int x, y;
                    x = rnd.Next(poblacion.Count);
                    do
                    {
                        y = rnd.Next(poblacion.Count);
                    } while (y == x);
                    Board X = poblacion[x], Y = poblacion[y];

                    //Reproducir los dos elementos escogidos
                    Board hijo = Reproducir(X, Y);
                    await Listener.Reproduciendo(X, Y);
                    nueva_poblacion.Add(hijo);
                    await Listener.AgregandoANuevaPoblacion(hijo);

                    int fit = Fitness(hijo);
                    if (fit == 0)
                    {
                        Console.WriteLine("C: " + c);
                        foreach (int val in hijo)
                            Console.Write(val + " ");
                        await Listener.Solucion(hijo);
                        return hijo;
                    }

                    //Es posible que se de una mutacion...
                    if (rnd.Next(100) < ProbabilidadDeMutar)
                    {
                        Mutar(hijo);
                        await Listener.Mutando(hijo);
                    }

                    fit = Fitness(hijo);
                    if (fit == 0)
                    {
                        Console.WriteLine("C: " + c);
                        foreach (int val in hijo)
                            Console.Write(val + " ");
                        await Listener.Solucion(hijo);
                        return hijo;
                    }
                }
                c++;
                await Listener.TerminoNuevaPoblacion();
            }
        }

        private Board GeneticAlg(List<Board> poblacion, int idoneo)
        {
            int mejorH = idoneo + 100000;
            Board anterior = null, hijo = null;
            int c = 0;
            do
            {
                List<Board> nuevaPoblacion = new List<Board>();
                int i = 0;
                foreach (Board board in poblacion)
                {
                    anterior = i++ > 0 ? board : poblacion.ElementAt(poblacion.Count - 1);
                    hijo = Reproducir(anterior, board);
                    if (rnd.Next(10) < 1)
                        Mutar(hijo);
                    nuevaPoblacion.Add(hijo);
                    mejorH = Fitness(hijo);
                    if (mejorH == idoneo)
                    {
                        Console.WriteLine("C:" + c);
                        mejorTablero = hijo;
                        break;
                    }
                    c++;
                }
                poblacion = nuevaPoblacion;
            } while (mejorH > idoneo);
            return mejorTablero;
        }

        int Fitness(Board estado)
        {
            int c = 0;
            for (int i = 0; i < estado.Count - 1; i++)
                for (int j = i + 1; j < estado.Count; j++)
                    if (estado[i] == estado[j] || estado[i] - (j - i) == estado[j] || estado[i] + (j - i) == estado[j])
                        c++;
            return c;
        }

        private Board Reproducir(Board X, Board Y)
        {
            int[] hijo = new int[X.Count];
            for (int i = 0; i < X.Count; i++)
                hijo[i] = (i % 2 == 0) ? X[i] : Y[i];
            return new Board(hijo);
        }

        private void Mutar(Board estado)
        {
            int cantidadMutaciones = rnd.Next() % ( estado.Count / 2) + 1;
            for (int i = 0; i < cantidadMutaciones; i++)
                estado[rnd.Next(estado.Count)  ] = rnd.Next(estado.Count);
            return;
        }

    }

    public interface IGeneticQueen
    {
        Task MostrarPoblacion(List<Board> boards);
        Task Reproduciendo(Board X, Board Y);
        Task AgregandoANuevaPoblacion(Board hijo);
        Task Mutando(Board hijo);
        Task Solucion(Board solucion);
        Task TerminoNuevaPoblacion();
    }

}

