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
        private static Random rnd = new Random();
        private int[] mejorTablero = null;
        List<List<int[]>> Generaciones;
        private List<int[]> poblacionActual = null;


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
        public int GeneracionesCount()
        {
            return Generaciones.Count;
        }
        public GeneticQueen(IGeneticQueen pListener)
        {
            //Inicio la lista de generaciones aqui
            Generaciones = new List<List<int[]>>();
            ProbabilidadDeMutar = 80;
            Listener = pListener;
        }
        public async Task CreateFirstGen(int N,int size)
        {
            List<int[]> poblacion = new List<int[]>();
            for (int i = 0; i < size; ++i)
                poblacion.Add(GenerarAleatorio(N));
            poblacionActual = poblacion;
            Generaciones.Add(poblacion);
            await Listener.MostrarPoblacion(poblacion);
        }
        public async Task<int[]> Resolver()
        {           
            mejorTablero = await AlgoritmoGenetico(poblacionActual, 0);
            return mejorTablero;
        }

        private async Task<int[]> AlgoritmoGenetico(List<int[]> poblacion, int idoneo)
        {
            int c = 0;
            while (true)
            {
                List<int[]> nueva_poblacion = new List<int[]>(poblacion.Count);
                for (int i = 0; i < poblacion.Count; i++)
                {
                    //Escoger dos elementos de la poblacion al azar
                    int x, y;
                    x = rnd.Next(poblacion.Count);
                    do
                    {
                        y = rnd.Next(poblacion.Count);
                    } while (y == x);
                    int[] X = poblacion[x], Y = poblacion[y];

                    //Reproducir los dos elementos escogidos
                    int[] hijo = Reproducir(X, Y);
                    //await Listener.Reproduciendo(X, Y);
                    nueva_poblacion.Add(hijo);
                    //await Listener.AgregandoANuevaPoblacion(hijo);

                    int fit = Fitness(hijo);
                    if (fit == 0)
                    {
                        Console.WriteLine("C: " + c);
                        foreach (int val in hijo)
                            Console.Write(val + " ");
                        //await Listener.Solucion(hijo);
                        return hijo;
                    }

                    //Es posible que se de una mutacion...
                    if (rnd.Next(100) < ProbabilidadDeMutar)
                    {
                        Mutar(hijo);
                        //await Listener.Mutando(hijo);
                    }
                    fit = Fitness(hijo);
                    if (fit == 0)
                    {
                        Console.WriteLine("C: " + c);
                        foreach (int val in hijo)
                            Console.Write(val + " ");
                        //await Listener.Solucion(hijo);
                        return hijo;
                    }
                }
                c++;
                //Aqui agrego la nueva generacion
                Generaciones.Add(nueva_poblacion);
                //await Listener.TerminoNuevaPoblacion();
            }
        }

        private int[] GeneticAlg(List<int[]> poblacion, int idoneo)
        {
            int mejorH = idoneo + 100000;
            int[] anterior = null, hijo = null;
            int c = 0;
            do
            {
                List<int[]> nuevaPoblacion = new List<int[]>();
                int i = 0;
                foreach (int[] board in poblacion)
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

        private static int[] GenerarAleatorio(int n)
        {
            int[] newBoard = new int[n];
            for (int i = 0; i < n; ++i)
                newBoard[i] = rnd.Next(n);
            return newBoard;
        }

        int Fitness(int[] estado)
        {
            int c = 0;
            for (int i = 0; i < estado.Length - 1; i++)
                for (int j = i + 1; j < estado.Length; j++)
                    if (estado[i] == estado[j] || estado[i] - (j - i) == estado[j] || estado[i] + (j - i) == estado[j])
                        c++;
            return c;
        }

        private int[] Reproducir(int[] X, int[] Y)
        {
            int[] hijo = new int[X.Length];
            for (int i = 0; i < X.Length; i++)
                hijo[i] = (i % 2 == 0) ? X[i] : Y[i];
            return hijo;
        }

        private void Mutar(int[] estado)
        {
            int cantidadMutaciones = rnd.Next() % ( estado.Length / 2) + 1;
            for (int i = 0; i < cantidadMutaciones; i++)
                estado[rnd.Next(estado.Length)  ] = rnd.Next(estado.Length);
            return;
        }
        public List<int[]> ReturnGenByIndex(int ind)
        {
            //Retorno la lista de generacion pedida durante el slider
            return Generaciones[ind];
        }


    }

    public interface IGeneticQueen
    {
        Task MostrarPoblacion(List<int[]> boards);
        Task Reproduciendo(int[] X, int[] Y);
        Task AgregandoANuevaPoblacion(int[] hijo);
        Task Mutando(int[] hijo);
        Task Solucion(int[] solucion);
        Task TerminoNuevaPoblacion(List<int[]> nuevaGeneracion);
    }

}

