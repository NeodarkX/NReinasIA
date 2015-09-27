using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NReinas.Estructuras
{
    public class Board : List<int>
    {
        private static Random rnd = new Random();

        public Board(int n) : base(n)   {   }

        public Board(int[] arr) : base(arr) {   }

        public static Board GenerarAleatorio(int n)
        {
            Board newBoard = new Board(n);
            for (int i = 0; i < n; ++i)
                newBoard.Add(rnd.Next(n));
            return newBoard;
        }

    }
}
