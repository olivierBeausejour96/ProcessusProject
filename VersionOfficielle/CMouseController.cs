using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static VersionOfficielle.CWindowsMethodes;

namespace VersionOfficielle
{
    static public class CMouseController
    {
        public delegate void ClickMethod(int _tableIndex);
        private static object FFLockID = "123";
        private static Queue<ClickMethod> FFClicksToDo = new Queue<ClickMethod>();
        private static List<int> FFLstTableIndex = new List<int>();
        private static bool FFBusy = false;
        /// <summary>
        /// Bouge la souris aavec une certaine incertitude
        /// </summary>
        /// <param name="posX">Coordonee X de la positon de l'objectif</param>
        /// <param name="posY">Coordonee Y de la positon de l'objectif</param>
        /// <param name="MaxErrorX">L'erreur Maximale en X acceptable(positive et negative)</param>
        /// <param name="MaxErrorY">L'erreur Maximale en Y acceptable(positive et negative)</param>
        /// <param name="speed">La vitesse a laquelle l'objectif sera atteint; Plus c'est haut plus c'est vite; TEST BEFORE USING</param>
        static public void MoveMouseWithImprecision(int posX, int posY, int _maxErrorX = 25, int _maxErrorY = 25, int _speed = 4)
        {
            Random rnd = new Random();

            double ErrorX = rnd.Next(-_maxErrorX, _maxErrorX); // incertitude initiale du mouvement
            double ErrorY = rnd.Next(-_maxErrorY, _maxErrorY);

            Point initPos = Cursor.Position;
            double difX = (posX + ErrorX) - initPos.X;
            double difY = (posY + ErrorY) - initPos.Y;

            double DX = difX / (Math.Abs(difX) + Math.Abs(difY)) * _speed;
            double DY = difY / (Math.Abs(difX) + Math.Abs(difY)) * _speed / 8; // 7/8 du deplacement est constant; 1/8 forme un arc



            double X = Cursor.Position.X;
            double Y = Cursor.Position.Y;

            for (int i = 0; i < (difX / DX); i++)
            {
                Thread.Sleep(1);
                X += DX * Math.PI / 2 * Math.Sin(((difX / DX) - i) / (difX / DX) * Math.PI / 2); // DX = deplacement; Formule etrange = mouvement non lineaire => 
                Y += 7 * DY + DY * Math.PI / 2 * Math.Sin((1 - ((difX / DX) - i) / (difX / DX)) * Math.PI / 2); // mouvement plus rapide au debut et plus lent a la fin

                Point location = new Point((int)Math.Round(X, 0), (int)Math.Round(Y, 0));
                Cursor.Position = location;
            }
            Thread.Sleep(rnd.Next(50, 150));

            MoveMouseWithPrecision(posX, posY, _speed);

        }

        static public void DoMouseClick()
        {
            mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, (uint)Cursor.Position.X, (uint)Cursor.Position.Y, 0, 0);
        }

        /// <summary>
        /// Executes a mouse click after a certain time.
        /// </summary>
        /// <param name="_minimumWaitingTime">In milliseconds</param>
        /// <param name="_maximumWaitingTime">In milliseconds</param>
        static public void DoMouseClickWithRandomWaitingTime(int _minimumWaitingTime, int _maximumWaitingTime)
        {
            Random rnd = new Random();
            int randomTimeWait = rnd.Next(_minimumWaitingTime, _maximumWaitingTime);

            Thread.Sleep(randomTimeWait);
            mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, (uint)Cursor.Position.X, (uint)Cursor.Position.Y, 0, 0);
        }

        static private void MoveMouseWithPrecision(int posX, int posY, int speed)
        {
            double difX = posX - Cursor.Position.X;
            double difY = posY - Cursor.Position.Y;

            double DX = difX / (Math.Abs(difX) + Math.Abs(difY)) * speed / (3 * 2);
            double DY = difY / (Math.Abs(difX) + Math.Abs(difY)) * speed / (3 * 2);
            double DY1 = DY / 8;

            double X = Cursor.Position.X;
            double Y = Cursor.Position.Y;

            for (int i = 0; i < (difX / DX); i++)
            {
                Thread.Sleep(1);
                X += DX * Math.PI / 2 * Math.Sin(((difX / DX) - i) / (difX / DX) * Math.PI / 2);
                Y += 7 * DY1 + DY1 * Math.PI / 2 * Math.Sin((1 - ((difX / DX) - i) / (difX / DX)) * Math.PI / 2);
                Point location = new Point((int)Math.Round(X, 0), (int)Math.Round(Y, 0));
                Cursor.Position = location;
            }
        }

        static public void MoveMouseToRandomLocation()
        {
            Random rnd = new Random();



        }


        public static void AddClickToQueue(ClickMethod _method, int _tableIndex)
        {
            lock (FFLockID)
            {
                FFClicksToDo.Enqueue(_method);
                FFLstTableIndex.Add(_tableIndex);
                
            }
            DoClicks();
        }

        private static void DoClicks()
        {
            lock (FFLockID)
            {
                if (!FFBusy)
                {
                    FFBusy = true;

                    while (FFClicksToDo.Count != 0)
                    {
                        ClickMethod methodToCall = FFClicksToDo.Dequeue();
                        methodToCall(FFLstTableIndex.First());
                        FFLstTableIndex.RemoveAt(0);
                    }

                    FFBusy = false;
                }
            }
        }
    }
}
