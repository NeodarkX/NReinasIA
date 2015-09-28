using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using NReinas.Estructuras;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;

namespace NReinas.UI
{
    public class BoardUI : Canvas
    {
        private Board mBoard;
        private List<Image> reinas;
        private double MARGIN = 2;

        public BoardUI(Board pBoard) : this(pBoard,100) { } 

        public BoardUI(Board pBoard,double size)
        {
            Width = Height = size; 
            if (pBoard == null)
                throw new ArgumentNullException();
            mBoard = pBoard;
            //Seteando background
            ImageBrush imageBrush = new ImageBrush();
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri("pack://application:,,,/Resources/chess.jpg");
            bitmap.EndInit();
            imageBrush.ImageSource = bitmap;
            Background = imageBrush;


            int n = mBoard.Count;
            double columnSize = size / n;
            MARGIN = columnSize / 10;
            double queenSize = columnSize - MARGIN * 2;
            reinas = new List<Image>(n);
            for(int i = 0; i < n; i++)
            {
                Image image = new Image();
                if (mBoard[i] % 2 == 0) // esta en casilla negra, reina de blanco
                    image.Source = new BitmapImage(new Uri(@"Resources\queen-white.png", UriKind.Relative));
                else // esta en casilla blanca, reina de negro
                    image.Source = new BitmapImage(new Uri(@"Resources\queen-black.png", UriKind.Relative));
                //setear tamanio
                image.Width = image.Height = queenSize;
                //setear coordenadas
                SetLeft(image, i * columnSize + MARGIN);
                SetTop(image, mBoard[i] * columnSize + MARGIN);
                //agregar al canvas
                Children.Add(image);
                //agregar a arreglo de reinas
                reinas.Add(image);
            }

        }

        public void Update(Board board)
        {
            if (board == null)
                throw new ArgumentNullException();
            if (board.Count != mBoard.Count)
                throw new ArgumentException("Los tamanios de los tableros deben concordar!");

            int i = 0;
            foreach(Image image in reinas)
            {
                if (mBoard[i] % 2 == 0) // esta en casilla negra, reina de blanco
                    image.Source = new BitmapImage(new Uri(@"Resource\queen-white.png"));
                else // esta en casilla blanca, reina de negro
                    image.Source = new BitmapImage(new Uri(@"Resource\queen-black.png"));
                //setear coordenadas
                SetLeft(image, i * image.Width + MARGIN / 2);
                SetTop(image, mBoard[i] * image.Height + MARGIN / 2);
                //agregar al canvas
                Children.Add(image);
                //agregar a arreglo de reinas
                reinas[i++] = image;
            }
        }

    }
}
