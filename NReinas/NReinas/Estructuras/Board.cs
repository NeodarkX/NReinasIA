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

        public Board(int[] arr,Board padre, Board madre) : base(arr)
        {
            Madre = madre;
            Padre = padre;
        }

        public Board Padre { get; }
        public Board Madre { get; }
        public Board AntesDeMutar { get; set; }

    }
}
