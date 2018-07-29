using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading.Tasks;
using WindowScrape.Types;
using VersionOfficielle;
using System.Linq;

namespace VersionOfficielle
{
    class Analyseur
    {
        Bitmap FFCurrentImage;

        static int sharedVariableY;

        public Analyseur(Bitmap image)
        {
            sharedVariableY = -1;
            FFCurrentImage = image;
        }

        public Bitmap RetournerBmpValeurCarte1()
        {
            Bitmap bmp = FFCurrentImage;
            Rectangle zoneTypeCarte1 = new Rectangle(Consantes.HAND_OFFSET1_WIDTH, Consantes.HAND_OFFSET1_HEIGHT, Consantes.HAND_SIGN_RECTANGLE_WIDTH, Consantes.HAND_SIGN_RECTANGLE_HEIGHT);
            bmp = (Bitmap)bmp.Clone(zoneTypeCarte1, bmp.PixelFormat);
            return bmp;
        }

        public Bitmap RetournerBmpValeurCarte2()
        {
            Bitmap bmp = FFCurrentImage;
            Rectangle zoneValueCarte2 = new Rectangle(Consantes.HAND_OFFSET2_WIDTH, Consantes.HAND_OFFSET1_HEIGHT, Consantes.HAND_VALUE_RECTANGLE_WIDTH, Consantes.HAND_VALUE_RECTANGLE_HEIGHT);
            bmp = (Bitmap)bmp.Clone(zoneValueCarte2, bmp.PixelFormat);

            return bmp;
        }

        public List<Bitmap> RetournerListeBmpValeur(Carte.Type _type)
        {
            List<Bitmap> bmpList = new List<Bitmap>();

            if ((_type == Carte.Type.Diamonds) || (_type == Carte.Type.Hearts))
            {
                bmpList.Add(Properties.Resources.R2);
                bmpList.Add(Properties.Resources.R3);
                bmpList.Add(Properties.Resources.R4);
                bmpList.Add(Properties.Resources.R5);
                bmpList.Add(Properties.Resources.R6);
                bmpList.Add(Properties.Resources.R7);
                bmpList.Add(Properties.Resources.R8);
                bmpList.Add(Properties.Resources.R9);
                bmpList.Add(Properties.Resources.R10);
                bmpList.Add(Properties.Resources.RJ);
                bmpList.Add(Properties.Resources.RQ);
                bmpList.Add(Properties.Resources.RK);
                bmpList.Add(Properties.Resources.RA);
            }
            else
            {
                bmpList.Add(Properties.Resources.B2);
                bmpList.Add(Properties.Resources.B3);
                bmpList.Add(Properties.Resources.B4);
                bmpList.Add(Properties.Resources.B5);
                bmpList.Add(Properties.Resources.B6);
                bmpList.Add(Properties.Resources.B7);
                bmpList.Add(Properties.Resources.B8);
                bmpList.Add(Properties.Resources.B9);
                bmpList.Add(Properties.Resources.B10);
                bmpList.Add(Properties.Resources.BJ);
                bmpList.Add(Properties.Resources.BQ);
                bmpList.Add(Properties.Resources.BK);
                bmpList.Add(Properties.Resources.BA);
            }

            return bmpList;
        }

        public unsafe Dictionary<int, int> STRetournerDecompteValeur(Bitmap bmp2, Carte.Type theSign)
        {
            BitmapData bmp = bmp2.LockBits(new Rectangle(0, 0, bmp2.Width, bmp2.Height), ImageLockMode.ReadOnly, bmp2.PixelFormat);
            List<Bitmap> bmpValueList = RetournerListeBmpValeur(theSign);

            List<BitmapData> bmpReferences = new List<BitmapData>();
            for (int i = 0; i < bmpValueList.Count; i++)
            {
                bmpReferences.Add(bmpValueList[i].LockBits(new Rectangle(0, 0, bmpValueList[i].Width, bmpValueList[i].Height), ImageLockMode.ReadOnly, bmpValueList[i].PixelFormat));
            }


            int bytesPerPixel = System.Drawing.Bitmap.GetPixelFormatSize(bmp2.PixelFormat) / 8;
            int heightInPixels = bmp.Height;
            int widthInBytes = bytesPerPixel * bmp.Width;

            byte* ptrFirstPixel = (byte*)bmp.Scan0;
            byte*[] ptrSignArray = new byte*[bmpReferences.Count];
            for (int i = 0; i < bmpReferences.Count; i++) // 13 == bmpValueList.count
            {
                ptrSignArray[i] = (byte*)bmpReferences[i].Scan0;
            }

            Dictionary<int, int> decompte = new Dictionary<int, int>();
            for (int i = 0; i < bmpReferences.Count; i++)
            {
                decompte[i] = 0;
            }

            for(int y = 0; y < heightInPixels; y++)
            {
                for (int i = 0; i < bmpReferences.Count; i++)
                {
                    byte* currentLine = ptrFirstPixel + (y * bmp.Stride);
                    byte* currentLineSign = ptrSignArray[i] + (y * bmpReferences[i].Stride);
                    for (int j = 0; j < widthInBytes; j += bytesPerPixel)
                    {
                        int difB = (int)Math.Pow(currentLine[j] - currentLineSign[j], 2);
                        int difG = (int)Math.Pow(currentLine[j + 1] - currentLineSign[j + 1], 2);
                        int difR = (int)Math.Pow(currentLine[j + 2] - currentLineSign[j + 2], 2);

                        decompte[i] += difB + difG + difR;
                    }
                }
            }

            bmp2.UnlockBits(bmp);

            for (int i = 0; i < bmpReferences.Count; i++)
            {
                bmpValueList[i].UnlockBits(bmpReferences[i]);
            }

            return decompte;
        }
        static readonly object o = new object();
        private unsafe void MethodWithParameter(ref List<BitmapData> bmpReferences, ref byte* ptrFirstPixel, ref BitmapData bmp, ref byte*[] ptrSignArray, ref int widthInBytes, ref int bytesPerPixel, ref Dictionary<int, int> decompte)
        {
            int y;
            lock (o)
            {
                y = ++sharedVariableY;
            }
            for (int i = 0; i < bmpReferences.Count; i++)
            {
                byte* currentLine = ptrFirstPixel + (y * bmp.Stride);
                byte* currentLineSign = ptrSignArray[i] + (y * bmpReferences[i].Stride);
                for (int j = 0; j < widthInBytes; j += bytesPerPixel)
                {
                    int difB = (int)Math.Pow(currentLine[j] - currentLineSign[j], 2);
                    int difG = (int)Math.Pow(currentLine[j + 1] - currentLineSign[j + 1], 2);
                    int difR = (int)Math.Pow(currentLine[j + 2] - currentLineSign[j + 2], 2);
                    lock (decompte)
                    {
                        decompte[i] += difB + difG + difR;
                    }
                }
            }
        }
        public unsafe Dictionary<int, int> MTRetournerDecompteValeur(Bitmap bmp2, Carte.Type theSign)
        {
            BitmapData bmp = bmp2.LockBits(new Rectangle(0, 0, bmp2.Width, bmp2.Height), ImageLockMode.ReadOnly, bmp2.PixelFormat);
            List<Bitmap> bmpValueList = RetournerListeBmpValeur(theSign);

            List<BitmapData> bmpReferences = new List<BitmapData>();
            for (int i = 0; i < bmpValueList.Count; i++)
            {
                bmpReferences.Add(bmpValueList[i].LockBits(new Rectangle(0, 0, bmpValueList[i].Width, bmpValueList[i].Height), ImageLockMode.ReadOnly, bmpValueList[i].PixelFormat));
            }

            int bytesPerPixel = System.Drawing.Bitmap.GetPixelFormatSize(bmp2.PixelFormat) / 8;
            int heightInPixels = bmp.Height;
            int widthInBytes = bytesPerPixel * bmp.Width;

            byte* ptrFirstPixel = (byte*)bmp.Scan0;
            byte*[] ptrSignArray = new byte*[bmpReferences.Count];
            for (int i = 0; i < bmpReferences.Count; i++) // 13 == bmpValueList.count
            {
                ptrSignArray[i] = (byte*)bmpReferences[i].Scan0;
            }

            Dictionary<int, int> decompte = new Dictionary<int, int>();
            for (int i = 0; i < bmpReferences.Count; i++)
            {
                decompte[i] = 0;
            }
            List<Task> qwe = new List<Task>();
            for (int y = 0; y < heightInPixels; y++)
            {
                qwe.Add(
                Task.Run(() =>
                {
                    MethodWithParameter(ref bmpReferences, ref ptrFirstPixel, ref bmp, ref ptrSignArray, ref widthInBytes, ref bytesPerPixel, ref decompte);
                }));
            }

            foreach (var item in qwe)
            {
                item.Wait();
            }
            sharedVariableY = -1;

            bmp2.UnlockBits(bmp);

            for (int i = 0; i < bmpReferences.Count; i++)
            {
                bmpValueList[i].UnlockBits(bmpReferences[i]);
            }

            return decompte;
        }

        public Carte.Valeur RetournerValeur(Dictionary<int, int> decompte)
        {
            int min = 0;
            for (int i = 0; i < decompte.Count; ++i)
                if (decompte[min] > decompte[i])
                    min = i;

            switch (min)
            {
                case 0:
                    return Carte.Valeur.Two;
                case 1:
                    return Carte.Valeur.Three;
                case 2:
                    return Carte.Valeur.Four;
                case 3:
                    return Carte.Valeur.Five;
                case 4:
                    return Carte.Valeur.Six;
                case 5:
                    return Carte.Valeur.Seven;
                case 6:
                    return Carte.Valeur.Eight;
                case 7:
                    return Carte.Valeur.Nine;
                case 8:
                    return Carte.Valeur.Ten;
                case 9:
                    return Carte.Valeur.Jack;
                case 10:
                    return Carte.Valeur.Queen;
                case 11:
                    return Carte.Valeur.King;
                case 12:
                    return Carte.Valeur.Ace;
                default:
                    throw new InvalidOperationException("La valeur de la carte n'est pas valide");
            }
        }

        public int[] getOrderedList(Dictionary<int, int> decompte)
        {
            int[] myList = new int[decompte.Count];

            for (int i = 0; i < decompte.Count; i++)
            {
                myList[i] = decompte[i] / 100000;
            }
            int min;
            for (int i = 0; i < decompte.Count - 1; i++)
            {
                min = i;
                for (int j = i + 1; j < decompte.Count; j++)
                {
                    if (myList[min] > myList[j])
                        min = j;
                }
                int lol = myList[i];
                myList[i] = myList[min];
                myList[min] = lol;
            }

            return myList;
        }

        public string valueStatement(Dictionary<int, int> decompte)
        {
            string statement = "Two Accuracy: " + decompte[0] / 100000 + "\n";
            statement += "Three Accuracy: " + decompte[1] / 100000 + "\n";
            statement += "Four Accuracy: " + decompte[2] / 100000 + "\n";
            statement += "Five Accuracy: " + decompte[3] / 100000 + "\n";
            statement += "Six Accuracy: " + decompte[4] / 100000 + "\n";
            statement += "Seven Accuracy: " + decompte[5] / 100000 + "\n";
            statement += "Eight Accuracy: " + decompte[6] / 100000 + "\n";
            statement += "Nine Accuracy: " + decompte[7] / 100000 + "\n";
            statement += "Ten Accuracy: " + decompte[8] / 100000 + "\n";
            statement += "Jack Accuracy: " + decompte[9] / 100000 + "\n";
            statement += "Queen Accuracy: " + decompte[10] / 100000 + "\n";
            statement += "King Accuracy: " + decompte[11] / 100000 + "\n";
            statement += "As Accuracy: " + decompte[12] / 100000 + "\n";

            int min = 0;
            for (int i = 0; i < decompte.Count; i++)
            {
                if (decompte[min] > decompte[i])
                {
                    min = i;
                }
            }
            statement += "\n*******\n";

            switch (min)
            {
                case 0:
                    statement +=  " Two it is";
                    break;
                case 1:
                    statement +=  "Three it is";
                    break;
                case 2:
                    statement +=  "Four it is";
                    break;
                case 3:
                    statement +=  "Five it is";
                    break;
                case 4:
                    statement +=  "Six it is";
                    break;
                case 5:
                    statement +=  "Seven it is";
                    break;
                case 6:
                    statement +=  "Eight it is";
                    break;
                case 7:
                    statement +=  "Nine it is";
                    break;
                case 8:
                    statement +=  "Ten it is";
                    break;
                case 9:
                    statement +=  "Jack it is";
                    break;
                case 10:
                    statement +=  "Queen it is";
                    break;
                case 11:
                    statement +=  "King it is";
                    break;
                case 12:
                    statement +=  "As it is";
                    break;
                default:
                    statement += "Something went wrong...";
                    break;
            }

            statement += "\n*******\n";

            return statement;
        }

        public string STvalueStatement1()
        {
            var redDecompte = STRetournerDecompteValeur(RetournerBmpValeurCarte1(), Carte.Type.Hearts);
            var redMin = redDecompte.Aggregate((l, r) => l.Value < r.Value ? l : r).Value;
            var blackDecompte = STRetournerDecompteValeur(RetournerBmpValeurCarte1(), Carte.Type.Spades);
            var blackMin = blackDecompte.Aggregate((l, r) => l.Value < r.Value ? l : r).Value;


            if (redMin < blackMin)
            {
                return valueStatement(redDecompte);
            }
            else
            {
                return valueStatement(blackDecompte);
            }

        }

        public string MTvalueStatement1()
        {
            var redDecompte = MTRetournerDecompteValeur(RetournerBmpValeurCarte1(), Carte.Type.Hearts);
            var redMin = redDecompte.Aggregate((l, r) => l.Value < r.Value ? l : r).Value;
            var blackDecompte = MTRetournerDecompteValeur(RetournerBmpValeurCarte1(), Carte.Type.Spades);
            var blackMin = blackDecompte.Aggregate((l, r) => l.Value < r.Value ? l : r).Value;


            if (redMin < blackMin)
            {
                return valueStatement(redDecompte);
            }
            else
            {
                return valueStatement(blackDecompte);
            }

        }

        public string STvalueStatement2()
        {
            var redDecompte = STRetournerDecompteValeur(RetournerBmpValeurCarte2(), Carte.Type.Hearts);
            var redMin = redDecompte.Aggregate((l, r) => l.Value < r.Value ? l : r).Value;
            var blackDecompte = STRetournerDecompteValeur(RetournerBmpValeurCarte2(), Carte.Type.Spades);
            var blackMin = blackDecompte.Aggregate((l, r) => l.Value < r.Value ? l : r).Value;

            if (redMin < blackMin)
            {
                return valueStatement(redDecompte);
            }
            else
            {
                return valueStatement(blackDecompte);
            }
        }

        public string MTvalueStatement2()
        {
            var redDecompte = MTRetournerDecompteValeur(RetournerBmpValeurCarte2(), Carte.Type.Hearts);
            var redMin = redDecompte.Aggregate((l, r) => l.Value < r.Value ? l : r).Value;
            var blackDecompte = MTRetournerDecompteValeur(RetournerBmpValeurCarte2(), Carte.Type.Spades);
            var blackMin = blackDecompte.Aggregate((l, r) => l.Value < r.Value ? l : r).Value;

            if (redMin < blackMin)
            {
                return valueStatement(redDecompte);
            }
            else
            {
                return valueStatement(blackDecompte);
            }
        }
    }
}
