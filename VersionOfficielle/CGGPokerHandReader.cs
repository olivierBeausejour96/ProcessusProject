using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading.Tasks;
using WindowScrape.Types;
using VersionOfficielle;

namespace VersionOfficielle
{
    class CGGPokerHandReader
    {
        HwndObject FFWindowHandle;

        Bitmap FFCurrentImage;

        public CCarte[] FFActualHand;

        private Rectangle getTheZone()
        {
            // zone 6max 4eme joueur
            Rectangle zoneTypeCarte1 = new Rectangle(CGGPokerConsantes.GG_HAND_OFFSET1_WIDTH, CGGPokerConsantes.GG_HAND_OFFSET1_HEIGHT, CGGPokerConsantes.GG_HAND_SIGN_RECTANGLE_WIDTH, CGGPokerConsantes.GG_HAND_SIGN_RECTANGLE_HEIGHT);
            Rectangle zoneTypeCarte2 = new Rectangle(CGGPokerConsantes.GG_HAND_OFFSET2_WIDTH, CGGPokerConsantes.GG_HAND_OFFSET1_HEIGHT, CGGPokerConsantes.GG_HAND_SIGN_RECTANGLE_WIDTH, CGGPokerConsantes.GG_HAND_SIGN_RECTANGLE_HEIGHT);
            Rectangle zoneValueCarte1 = new Rectangle(CGGPokerConsantes.GG_HAND_OFFSET1_WIDTH, CGGPokerConsantes.GG_HAND_OFFSET2_HEIGHT, CGGPokerConsantes.GG_HAND_VALUE_RECTANGLE_WIDTH, CGGPokerConsantes.GG_HAND_VALUE_RECTANGLE_HEIGHT);
            Rectangle zoneValueCarte2 = new Rectangle(CGGPokerConsantes.GG_HAND_OFFSET2_WIDTH, CGGPokerConsantes.GG_HAND_OFFSET2_HEIGHT, CGGPokerConsantes.GG_HAND_VALUE_RECTANGLE_WIDTH, CGGPokerConsantes.GG_HAND_VALUE_RECTANGLE_HEIGHT);

            return zoneTypeCarte1;
        }

        public CGGPokerHandReader(HwndObject _handle)
        {
            FFWindowHandle = _handle;
        }

        private void ActualizeCurrentImage()
        {
            FFCurrentImage = CScreenshot.getBmpFromScreen(CGGPokerConsantes.GG_WINDOW_WIDTH, CGGPokerConsantes.GG_WINDOW_HEIGHT, FFWindowHandle.Location.X, FFWindowHandle.Location.Y);
        }

        public void ActualizeHand()
        {
            ActualizeCurrentImage();
            FFActualHand = RetournerMain();
        }

        /// <summary>
        /// Retourne un Bitmap du Signe de la premiere carte du board par logique de jeux.
        /// </summary>
        /// <returns>Bitmap du Signe de la premiere carte du board par logique de jeux.</returns>
        public Bitmap RetournerBmpTypeCarte1()
        {
            Bitmap bmp = FFCurrentImage;
            Rectangle zoneTypeCarte1 = new Rectangle(CGGPokerConsantes.GG_HAND_OFFSET1_WIDTH, CGGPokerConsantes.GG_HAND_OFFSET2_HEIGHT, CGGPokerConsantes.GG_HAND_SIGN_RECTANGLE_WIDTH, CGGPokerConsantes.GG_HAND_SIGN_RECTANGLE_HEIGHT);
            bmp = (Bitmap)bmp.Clone(zoneTypeCarte1, bmp.PixelFormat);
            return bmp;
        }

        public Bitmap RetournerBmpTypeCarte1(int x)
        {
            Bitmap bmp = FFCurrentImage;
            Rectangle zoneTypeCarte1 = new Rectangle(CGGPokerConsantes.GG_HAND_OFFSET1_WIDTH + x, CGGPokerConsantes.GG_HAND_OFFSET2_HEIGHT, CGGPokerConsantes.GG_HAND_SIGN_RECTANGLE_WIDTH, CGGPokerConsantes.GG_HAND_SIGN_RECTANGLE_HEIGHT);
            bmp = (Bitmap)bmp.Clone(zoneTypeCarte1, bmp.PixelFormat);
            return bmp;
        }

        public Bitmap RetournerBmpTypeCarte1(int x, int y)
        {
            Bitmap bmp = FFCurrentImage;
            Rectangle zoneTypeCarte1 = new Rectangle(CGGPokerConsantes.GG_HAND_OFFSET1_WIDTH + x, CGGPokerConsantes.GG_HAND_OFFSET2_HEIGHT + y, CGGPokerConsantes.GG_HAND_SIGN_RECTANGLE_WIDTH, CGGPokerConsantes.GG_HAND_SIGN_RECTANGLE_HEIGHT);
            bmp = (Bitmap)bmp.Clone(zoneTypeCarte1, bmp.PixelFormat);
            return bmp;
        }

        public Bitmap RetournerBmpTypeCarte2()
        {
            Bitmap bmp = FFCurrentImage;
            Rectangle zoneTypeCarte2 = new Rectangle(CGGPokerConsantes.GG_HAND_OFFSET2_WIDTH, CGGPokerConsantes.GG_HAND_OFFSET2_HEIGHT, CGGPokerConsantes.GG_HAND_SIGN_RECTANGLE_WIDTH, CGGPokerConsantes.GG_HAND_SIGN_RECTANGLE_HEIGHT);
            bmp = (Bitmap)bmp.Clone(zoneTypeCarte2, bmp.PixelFormat);

            return bmp;
        }

        public Bitmap RetournerBmpValeurCarte1()
        {
            Bitmap bmp = FFCurrentImage;
            Rectangle zoneTypeCarte1 = new Rectangle(CGGPokerConsantes.GG_HAND_OFFSET1_WIDTH, CGGPokerConsantes.GG_HAND_OFFSET1_HEIGHT, CGGPokerConsantes.GG_HAND_SIGN_RECTANGLE_WIDTH, CGGPokerConsantes.GG_HAND_SIGN_RECTANGLE_HEIGHT);
            bmp = (Bitmap)bmp.Clone(zoneTypeCarte1, bmp.PixelFormat);
            return bmp;
        }

        public Bitmap RetournerBmpValeurCarte2()
        {
            Bitmap bmp = FFCurrentImage;
            Rectangle zoneValueCarte2 = new Rectangle(CGGPokerConsantes.GG_HAND_OFFSET2_WIDTH, CGGPokerConsantes.GG_HAND_OFFSET1_HEIGHT, CGGPokerConsantes.GG_HAND_VALUE_RECTANGLE_WIDTH, CGGPokerConsantes.GG_HAND_VALUE_RECTANGLE_HEIGHT);
            bmp = (Bitmap)bmp.Clone(zoneValueCarte2, bmp.PixelFormat);

            return bmp;
        }

        public List<Bitmap> RetournerListeBmpValeur(CCarte.Type _type)
        {
            List<Bitmap> bmpList = new List<Bitmap>();

            if ((_type == CCarte.Type.Diamonds) || (_type == CCarte.Type.Hearts))
            {
                bmpList.Add(Properties.Resources.GGPokerR2);
                bmpList.Add(Properties.Resources.GGPokerR3);
                bmpList.Add(Properties.Resources.GGPokerR4);
                bmpList.Add(Properties.Resources.GGPokerR5);
                bmpList.Add(Properties.Resources.GGPokerR6);
                bmpList.Add(Properties.Resources.GGPokerR7);
                bmpList.Add(Properties.Resources.GGPokerR8);
                bmpList.Add(Properties.Resources.GGPokerR9);
                bmpList.Add(Properties.Resources.GGPokerR10);
                bmpList.Add(Properties.Resources.GGPokerRJ);
                bmpList.Add(Properties.Resources.GGPokerRQ);
                bmpList.Add(Properties.Resources.GGPokerRK);
                bmpList.Add(Properties.Resources.GGPokerRA);
            }
            else
            {
                bmpList.Add(Properties.Resources.GGPokerB2);
                bmpList.Add(Properties.Resources.GGPokerB3);
                bmpList.Add(Properties.Resources.GGPokerB4);
                bmpList.Add(Properties.Resources.GGPokerB5);
                bmpList.Add(Properties.Resources.GGPokerB6);
                bmpList.Add(Properties.Resources.GGPokerB7);
                bmpList.Add(Properties.Resources.GGPokerB8);
                bmpList.Add(Properties.Resources.GGPokerB9);
                bmpList.Add(Properties.Resources.GGPokerB10);
                bmpList.Add(Properties.Resources.GGPokerBJ);
                bmpList.Add(Properties.Resources.GGPokerBQ);
                bmpList.Add(Properties.Resources.GGPokerBK);
                bmpList.Add(Properties.Resources.GGPokerBA);
            }

            return bmpList;
        }

        public unsafe Dictionary<CCarte.Type, int> RetournerDecompteType(Bitmap bmp2)
        {
            BitmapData bmp = bmp2.LockBits(new Rectangle(0, 0, bmp2.Width, bmp2.Height), ImageLockMode.ReadOnly, bmp2.PixelFormat);

            List<Bitmap> bmpList = new List<Bitmap>();

            bmpList.Add(Properties.Resources.GGPokerHeart);
            bmpList.Add(Properties.Resources.GGPokerSpade);
            bmpList.Add(Properties.Resources.GGPokerDiamond);
            bmpList.Add(Properties.Resources.GGPokerClove);

            List<BitmapData> bmpReferences = new List<BitmapData>();
            for (int i = 0; i < bmpList.Count; i++)
            {
                bmpReferences.Add(bmpList[i].LockBits(new Rectangle(0, 0, bmpList[i].Width, bmpList[i].Height), ImageLockMode.ReadOnly, bmpList[i].PixelFormat));
            }



            int bytesPerPixel = System.Drawing.Bitmap.GetPixelFormatSize(bmp2.PixelFormat) / 8;
            int heightInPixels = bmp.Height;
            int widthInBytes = bytesPerPixel * bmp.Width;

            byte* ptrFirstPixel = (byte*)bmp.Scan0;
            byte*[] ptrSignArray = new byte*[4];
            for (int i = 0; i < bmpReferences.Count; i++)
            {
                ptrSignArray[i] = (byte*)bmpReferences[i].Scan0;
            }



            var decompte = new Dictionary<int, int>();
            decompte[0] = 0;
            decompte[1] = 0;
            decompte[2] = 0;
            decompte[3] = 0;

            Parallel.For(0, heightInPixels, y =>
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
            });

            bmp2.UnlockBits(bmp);

            for (int i = 0; i < bmpReferences.Count; i++)
            {
                bmpList[i].UnlockBits(bmpReferences[i]);
            }

            var decompte2 = new Dictionary<CCarte.Type, int>();
            decompte2[CCarte.Type.Hearts] = decompte[0];
            decompte2[CCarte.Type.Spades] = decompte[1];
            decompte2[CCarte.Type.Diamonds] = decompte[2];
            decompte2[CCarte.Type.Clubs] = decompte[3];

            return decompte2;
        }

        public unsafe Dictionary<int, int> RetournerDecompteValeur(Bitmap bmp2, CCarte.Type theSign)
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

            Parallel.For(0, heightInPixels, y =>
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
            });

            bmp2.UnlockBits(bmp);

            for (int i = 0; i < bmpReferences.Count; i++)
            {
                bmpValueList[i].UnlockBits(bmpReferences[i]);
            }

            return decompte;
        }

        public CCarte.Type RetournerType(Dictionary<CCarte.Type, int> _decompte)
        {
            CCarte.Type type;

            int eh = Math.Min(_decompte[CCarte.Type.Hearts], _decompte[CCarte.Type.Spades]);
            eh = Math.Min(eh, _decompte[CCarte.Type.Diamonds]);
            eh = Math.Min(eh, _decompte[CCarte.Type.Clubs]);

            if (_decompte[CCarte.Type.Hearts] == eh)
                type = CCarte.Type.Hearts;
            else if (_decompte[CCarte.Type.Spades] == eh)
                type = CCarte.Type.Spades;
            else if (_decompte[CCarte.Type.Clubs] == eh)
                type = CCarte.Type.Clubs;
            else
                type = CCarte.Type.Diamonds;
            return type;
        }

        public CCarte.Valeur RetournerValeur(Dictionary<int, int> decompte)
        {
            int min = 0;
            for (int i = 0; i < decompte.Count; ++i)
                if (decompte[min] > decompte[i])
                    min = i;



            switch (min)
            {
                case 0:
                    return CCarte.Valeur.Two;
                case 1:
                    return CCarte.Valeur.Three;
                case 2:
                    return CCarte.Valeur.Four;
                case 3:
                    return CCarte.Valeur.Five;
                case 4:
                    return CCarte.Valeur.Six;
                case 5:
                    return CCarte.Valeur.Seven;
                case 6:
                    return CCarte.Valeur.Eight;
                case 7:
                    return CCarte.Valeur.Nine;
                case 8:
                    return CCarte.Valeur.Ten;
                case 9:
                    return CCarte.Valeur.Jack;
                case 10:
                    return CCarte.Valeur.Queen;
                case 11:
                    return CCarte.Valeur.King;
                case 12:
                    return CCarte.Valeur.Ace;
                default:
                    throw new InvalidOperationException("La valeur de la carte n'est pas valide");
            }
        }

        private CCarte[] RetournerMain()
        {
            Bitmap bmp = RetournerBmpTypeCarte1();
            Bitmap bmp2 = RetournerBmpValeurCarte1();
            Dictionary<CCarte.Type, int> decompte = RetournerDecompteType(bmp);
            CCarte.Type type = RetournerType(decompte);
            Dictionary<int, int> decompte2 = RetournerDecompteValeur(bmp2, type);
            CCarte.Valeur theValue = RetournerValeur(decompte2);
            CCarte carte1 = new CCarte(theValue, type);

            bmp = RetournerBmpTypeCarte2();
            bmp2 = RetournerBmpValeurCarte2();
            decompte = RetournerDecompteType(bmp);
            type = RetournerType(decompte);
            decompte2 = RetournerDecompteValeur(bmp2, type);
            theValue = RetournerValeur(decompte2);
            CCarte carte2 = new CCarte(theValue, type);

            CLogger.AddLog(carte1.ToString() + " " + carte2.ToString());
            CCarte[] lol = { carte1, carte2 };
            return lol;
        }

        public string statement(Dictionary<CCarte.Type, int> decompte)
        {
            string statement = "Club Accuracy: " + decompte[CCarte.Type.Clubs] / 100000 + "\n";
            statement += "Spade Accuracy: " + decompte[CCarte.Type.Spades] / 100000 + "\n";
            statement += "Diamond Accuracy: " + decompte[CCarte.Type.Diamonds] / 100000 + "\n";
            statement += "Heart Accuracy: " + decompte[CCarte.Type.Hearts] / 100000 + "\n";


            statement += RetournerType(decompte).ToString();
            return statement;
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

            switch (min)
            {
                case 0:
                    statement += min.ToString() + " Two it is";
                    break;
                case 1:
                    statement += min.ToString() + "Three it is";
                    break;
                case 2:
                    statement += min.ToString() + "Four it is";
                    break;
                case 3:
                    statement += min.ToString() + "Five it is";
                    break;
                case 4:
                    statement += min.ToString() + "Six it is";
                    break;
                case 5:
                    statement += min.ToString() + "Seven it is";
                    break;
                case 6:
                    statement += min.ToString() + "Eight it is";
                    break;
                case 7:
                    statement += min.ToString() + "Nine it is";
                    break;
                case 8:
                    statement += min.ToString() + "Ten it is";
                    break;
                case 9:
                    statement += min.ToString() + "Jack it is";
                    break;
                case 10:
                    statement += min.ToString() + "Queen it is";
                    break;
                case 11:
                    statement += min.ToString() + "King it is";
                    break;
                case 12:
                    statement += min.ToString() + "As it is";
                    break;
                default:
                    statement += "Something went wrong...";
                    break;
            }

            return statement;
        }

        public string valueStatement1()
        {
            return valueStatement(RetournerDecompteValeur(RetournerBmpValeurCarte1(), RetournerType(RetournerDecompteType(RetournerBmpTypeCarte1())))).ToString();
        }

        public string valueStatement2()
        {
            return valueStatement(RetournerDecompteValeur(RetournerBmpValeurCarte2(), RetournerType(RetournerDecompteType(RetournerBmpTypeCarte2())))).ToString();
        }

        public string statement1()
        {
            string lol = "";
            lol += statement(RetournerDecompteType(RetournerBmpTypeCarte1(-1, -1))) + "\n";
            lol += statement(RetournerDecompteType(RetournerBmpTypeCarte1(-1, 0))) + "\n";
            lol += statement(RetournerDecompteType(RetournerBmpTypeCarte1(-1, 1))) + "\n";
            lol += statement(RetournerDecompteType(RetournerBmpTypeCarte1(0, -1))) + "\n";
            lol += statement(RetournerDecompteType(RetournerBmpTypeCarte1(0, 0))) + "\n";
            lol += statement(RetournerDecompteType(RetournerBmpTypeCarte1(0, 1))) + "\n";
            lol += statement(RetournerDecompteType(RetournerBmpTypeCarte1(1, -1))) + "\n";
            lol += statement(RetournerDecompteType(RetournerBmpTypeCarte1(1, 0))) + "\n";
            lol += statement(RetournerDecompteType(RetournerBmpTypeCarte1(1, 1))) + "\n";
            return lol;
        }

        public string statement2()
        {
            return statement(RetournerDecompteType(RetournerBmpTypeCarte2()));
        }
    }
}
