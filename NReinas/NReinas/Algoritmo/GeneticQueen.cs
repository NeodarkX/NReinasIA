using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NReinas.Algoritmo
{
    public class GeneticQueen
    {
        private Random rnd;
        private int[] mejorTablero = null;


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

        public GeneticQueen()
        {
            ProbabilidadDeMutar = 100;
            rnd = new Random();
        }

        public void resolver(int N, IGeneticQueen listener)
        {
            int np = 20;
            List<int[]> pob = new List<int[]>();
            for (int i = 0; i < np; ++i)
                pob.Add(aleatorio(N));
            //mejorTablero = geneticAlg(pob, 0);
            mejorTablero = algoritmoGenetico(pob, 0);
        }

        private int[] algoritmoGenetico(List<int[]> poblacion, int idoneo)
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
                    int[] hijo = reproducir(X, Y);

                    if (rnd.Next(100) < ProbabilidadDeMutar)
                        hijo = mutar(hijo);
                    nueva_poblacion.Add(hijo);
                    int fit = fitness(hijo);
                    if (fit == 0)
                    {
                        Console.WriteLine("C: " + c);
                        foreach (int val in hijo)
                            Console.Write(val + " ");
                        return hijo;
                    }
                    c++;
                }
            }
        }

        private int[] geneticAlg(List<int[]> poblacion, int idoneo)
        {
            int mejorH = idoneo + 100000;
            int[] anterior = null;
            int[] hijo = null;
            int c = 0;
            do
            {
                List<int[]> nuevaPoblacion = new List<int[]>();
                int i = 0;
                foreach (int[] board in poblacion)
                {
                    anterior = i++ > 0 ? board : poblacion.ElementAt(poblacion.Count - 1);
                    hijo = reproducir(anterior, board);
                    if (rnd.Next(10) < 1)
                    {
                        hijo = mutar(hijo);
                    }
                    nuevaPoblacion.Add(hijo);
                    mejorH = fitness(hijo);
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

        int fitness(int[] estado)
        {
            int c = 0;
            for (int i = 0; i < estado.Length - 1; i++)
                for (int j = i + 1; j < estado.Length; j++)
                    if (estado[i] == estado[j] || estado[i] - (j - i) == estado[j] || estado[i] + (j - i) == estado[j])
                        c++;
            return c;
        }

        private int[] reproducir(int[] X, int[] Y)
        {
            int[] hijo = new int[X.Length];
            for (int i = 0; i < X.Length; i++)
                hijo[i] = (i % 2 == 0) ? X[i] : Y[i];
            return hijo;
        }

        private int[] mutar(int[] estado)
        {
            int cantidadMutaciones = rnd.Next() % ( estado.Length / 2) + 1;
            for (int i = 0; i < cantidadMutaciones; i++)
                estado[rnd.Next(estado.Length)  ] = rnd.Next(estado.Length);
            return estado;
        }

        private int[] aleatorio(int n)
        {
            int[] board = new int[n];
            for (int i = 0; i < board.Length; ++i)
                board[i] = rnd.Next(0, n);
            return board;
        }


    }

    public interface IGeneticQueen
    {
    }

}

