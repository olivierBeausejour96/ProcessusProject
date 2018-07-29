using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading.Tasks;
using WindowScrape.Types;
using VersionOfficielle;



namespace VersionOfficielle
{
    public class CGGPokerBoardReader
    {
        private HwndObject FFWindowHandle;

        public Bitmap PCurrentImage { private set; get; }
       // private Bitmap FFCurrentHandBmp;

        public CGGPokerBoardReader(HwndObject _handle)
        {
            FFWindowHandle = _handle;
        }

        /// <summary>
        /// Take a screenshot of the window linked to the HwndObject and save it. Must be done every time the board state has changed.
        /// </summary>
        public void ActualizeCurrentImage()
        {
            PCurrentImage = CScreenshot.getBmpFromScreen(CGGPokerConsantes.GG_WINDOW_WIDTH, CGGPokerConsantes.GG_WINDOW_HEIGHT, getWindowHandle().Location.X, getWindowHandle().Location.Y);
        }

        //find and return the coordonates of the dealer jeton on screen, if not found, return coords (-1, -1)
        public CPosition GetDealerPositionOnScreen()
        {
            List<Bitmap> dealerJetonContainer = new List<Bitmap>();
            dealerJetonContainer.Add(Properties.Resources.dealerJeton);
            List<CPosition> cxz = CDetectItem.RetournerPosItemList(PCurrentImage, dealerJetonContainer);

            if (cxz.Count != 0)
            {
                return cxz[0];
            }
            else
            {
                return new CPosition(-1, -1);
            }
        }

        public bool isMyTurn()
        {
            List<Bitmap> bmpContainer = new List<Bitmap>();
            bmpContainer.Add(Properties.Resources.TurnBmp);
            List<CPosition> cxz = CDetectItem.RetournerPosItemList(PCurrentImage, bmpContainer);

            return cxz.Count != 0;
        }

        public bool hasCardsInHand()
        {
            List<Bitmap> bmpContainer = new List<Bitmap>();
            bmpContainer.Add(Properties.Resources.GGPokerCardInHandBmp);
            List<CPosition> cxz = CDetectItem.RetournerPosItemList(PCurrentImage, bmpContainer);

            return cxz.Count != 0;
        }

        private HwndObject getWindowHandle()
        {
            return FFWindowHandle;
        }

        private unsafe List<int[]> getNumbers(Bitmap bmp2, double alpha, List<Bitmap> bmpList)
        {
            BitmapData bmp = bmp2.LockBits(new Rectangle(0, 0, bmp2.Width, bmp2.Height), ImageLockMode.ReadOnly, bmp2.PixelFormat);

            List<BitmapData> bmpDataList = new List<BitmapData>();
            for (int i = 0; i < bmpList.Count; i++)
            {
                bmpDataList.Add(bmpList[i].LockBits(new Rectangle(0, 0, bmpList[i].Width, bmpList[i].Height), ImageLockMode.ReadOnly, bmpList[0].PixelFormat));
            }

            int bytesPerPixel = System.Drawing.Bitmap.GetPixelFormatSize(bmp.PixelFormat) / 8;
            int heightInPixels = bmp.Height;
            int totalWidthInBytes = bytesPerPixel * bmp.Width;
            int signWidthInBytes;

            byte* ptrFirstPixel = (byte*)bmp.Scan0;
            byte*[] ptrSignArray = new byte*[bmpDataList.Count];
            for (int i = 0; i < bmpDataList.Count; i++)
            {
                ptrSignArray[i] = (byte*)bmpDataList[i].Scan0;
            }

            List<int[]> decompte = new List<int[]>();

            double Acc = 0;

            for (int i = 0; i < bmpDataList.Count; i++)
            {
                signWidthInBytes = bytesPerPixel * bmpDataList[i].Width;
                for (int j = 0; j <= totalWidthInBytes - signWidthInBytes; j += bytesPerPixel)
                {
                    for (int y = 0; y < heightInPixels; y++)
                    {
                        byte* currentLine = ptrFirstPixel + (y * bmp.Stride) + j;
                        byte* currentLineSign = ptrSignArray[i] + (y * bmpDataList[i].Stride);
                        for (int k = 0; k < signWidthInBytes; k += bytesPerPixel)
                        {
                            Acc += Math.Pow(currentLine[k] - currentLineSign[k], 2);
                            Acc += Math.Pow(currentLine[k + 1] - currentLineSign[k + 1], 2);
                            Acc += Math.Pow(currentLine[k + 2] - currentLineSign[k + 2], 2);

                            if (Acc >= alpha)
                            {
                                goto NotFound;
                            }
                        }
                    }
                    int[] temp = { i, j / bytesPerPixel };
                    decompte.Add(temp);
                NotFound:
                    Acc = 0;
                }
            }

            bmp2.UnlockBits(bmp);
            for (int i = 0; i < bmpDataList.Count; i++)
            {
                bmpList[i].UnlockBits(bmpDataList[i]);
            }

            return decompte;
        }

        public Bitmap GetWestPlayerStackBmp()
        {
            Bitmap bmp2 = PCurrentImage;
            Rectangle zoneTypeCarte2 = new Rectangle(CGGPokerConsantes.GG_STACK_WEST_POSITION_OFFSET_WIDTH, CGGPokerConsantes.GG_STACK_WEST_POSITION_OFFSET_HEIGHT, CGGPokerConsantes.GG_STACK_RECTANGLE_WIDTH, CGGPokerConsantes.GG_STACK_RECTANGLE_HEIGHT);
            bmp2 = (Bitmap)bmp2.Clone(zoneTypeCarte2, bmp2.PixelFormat);
            return bmp2;
        }

        public Bitmap GetWestPlayerBetBmp()
        {
            Bitmap bmp2 = PCurrentImage;
            Rectangle zoneTypeCarte2 = new Rectangle(CGGPokerConsantes.GG_BET_WEST_POSITION_OFFSET_WIDTH, CGGPokerConsantes.GG_BET_WEST_POSITION_OFFSET_HEIGHT, CGGPokerConsantes.GG_BET_RECTANGLE_WIDTH, CGGPokerConsantes.GG_BET_RECTANGLE_HEIGHT);
            bmp2 = (Bitmap)bmp2.Clone(zoneTypeCarte2, bmp2.PixelFormat);
            return bmp2;
        }

        public Bitmap GetNorthPlayerStackBmp()
        {
            Bitmap bmp2 = PCurrentImage;
            Rectangle zoneTypeCarte2 = new Rectangle(CGGPokerConsantes.GG_STACK_NORTH_POSITION_OFFSET_WIDTH, CGGPokerConsantes.GG_STACK_NORTH_POSITION_OFFSET_HEIGHT, CGGPokerConsantes.GG_STACK_RECTANGLE_WIDTH, CGGPokerConsantes.GG_STACK_RECTANGLE_HEIGHT);
            bmp2 = (Bitmap)bmp2.Clone(zoneTypeCarte2, bmp2.PixelFormat);
            return bmp2;
        }

        public Bitmap GetNorthPlayerBetBmp()
        {
            Bitmap bmp2 = PCurrentImage;
            Rectangle zoneTypeCarte2 = new Rectangle(CGGPokerConsantes.GG_BET_NORTH_POSITION_OFFSET_WIDTH, CGGPokerConsantes.GG_BET_NORTH_POSITION_OFFSET_HEIGHT, CGGPokerConsantes.GG_BET_RECTANGLE_WIDTH, CGGPokerConsantes.GG_BET_RECTANGLE_HEIGHT);
            bmp2 = (Bitmap)bmp2.Clone(zoneTypeCarte2, bmp2.PixelFormat);
            return bmp2;
        }

        public Bitmap GetEstPlayerStackBmp()
        {
            Bitmap bmp2 = PCurrentImage;
            Rectangle zoneTypeCarte2 = new Rectangle(CGGPokerConsantes.GG_STACK_EST_POSITION_OFFSET_WIDTH, CGGPokerConsantes.GG_STACK_EST_POSITION_OFFSET_HEIGHT, CGGPokerConsantes.GG_STACK_RECTANGLE_WIDTH, CGGPokerConsantes.GG_STACK_RECTANGLE_HEIGHT);
            bmp2 = (Bitmap)bmp2.Clone(zoneTypeCarte2, bmp2.PixelFormat);
            return bmp2;
        }

        public Bitmap GetEstPlayerBetBmp()
        {
            Bitmap bmp2 = PCurrentImage;
            Rectangle zoneTypeCarte2 = new Rectangle(CGGPokerConsantes.GG_BET_EST_POSITION_OFFSET_WIDTH, CGGPokerConsantes.GG_BET_EST_POSITION_OFFSET_HEIGHT, CGGPokerConsantes.GG_BET_RECTANGLE_WIDTH, CGGPokerConsantes.GG_BET_RECTANGLE_HEIGHT);
            bmp2 = (Bitmap)bmp2.Clone(zoneTypeCarte2, bmp2.PixelFormat);
            return bmp2;
        }

        public Bitmap GetSouthPlayerStackBmp()
        {
            Bitmap bmp2 = PCurrentImage;
            Rectangle zoneTypeCarte2 = new Rectangle(CGGPokerConsantes.GG_STACK_SOUTH_POSITION_OFFSET_WIDTH, CGGPokerConsantes.GG_STACK_SOUTH_POSITION_OFFSET_HEIGHT, CGGPokerConsantes.GG_OUR_STACK_RECTANGLE_WIDTH, CGGPokerConsantes.GG_OUR_STACK_RECTANGLE_HEIGHT);
            bmp2 = (Bitmap)bmp2.Clone(zoneTypeCarte2, bmp2.PixelFormat);
            return bmp2;
        }

        public Bitmap GetSouthPlayerBetBmp()
        {
            Bitmap bmp2 = PCurrentImage;
            Rectangle zoneTypeCarte2 = new Rectangle(CGGPokerConsantes.GG_BET_SOUTH_POSITION_OFFSET_WIDTH, CGGPokerConsantes.GG_BET_SOUTH_POSITION_OFFSET_HEIGHT, CGGPokerConsantes.GG_BET_RECTANGLE_WIDTH, CGGPokerConsantes.GG_BET_RECTANGLE_HEIGHT);
            bmp2 = (Bitmap)bmp2.Clone(zoneTypeCarte2, bmp2.PixelFormat);
            return bmp2;
        }

        /// <summary>
        /// Retourne une chaine representant le nombre(stack of vilain) a partir de theNumberList
        /// </summary>
        /// <param name="theNumberList">Liste d'indice associe a un chiffre(3,4,7,0)</param>
        /// <param name="bmpWidth"></param>
        /// <returns></returns>
        private string getTheStackNumber(List<int[]> theNumberList)
        {
            string theNumber = "";

            if (theNumberList.Count == 0)
            {
                return "0";
            }

            theNumber += theNumberList[0][0].ToString();
            for (int i = 1; i < theNumberList.Count; i++)
            {
                if ((theNumberList[i][1] - theNumberList[i - 1][1]) > getBmpListOurMoneySign()[theNumberList[i-1][0]].Width)
                {
                    if (theNumberList[i][0] == 10)
                    {
                        theNumber += ",";
                    }
                    else
                    {
                        theNumber += theNumberList[i][0].ToString();
                    }
                }
            }
            return theNumber;
        }

        private string getTheBetNumber(List<int[]> theNumberList)
        {

            bool alreadyHasAComma = false;
            string theNumber = "";

            if (theNumberList.Count == 0)
            {
                return "0";
            }

            theNumber += theNumberList[0][0].ToString();
            for (int i = 1; i < theNumberList.Count; i++)
            {
                if ((theNumberList[i][1] - theNumberList[i - 1][1]) > (Properties.Resources.GGPokerBetNumber9.Width + 4) && !alreadyHasAComma)
                {
                    alreadyHasAComma = true;
                    theNumber += ",";
                }
                if ((theNumberList[i][1] - theNumberList[i - 1][1]) > (Properties.Resources.GGPokerBetNumber9.Width - 2))
                    theNumber += (theNumberList[i][0] % 10).ToString();
            }
            return theNumber;
        }

        /// <summary>
        /// Retourne une liste contenant les bitmaps des chiffres de l'argent des joueeurss sur espace Jeux
        /// </summary>
        /// <returns>une liste contenant les bitmaps des chiffres de l'argent des joueeurss sur espace Jeux</returns>
        private List<Bitmap> getBmpListPlayerMoneySign()
        {
            List<Bitmap> theBmpList = new List<Bitmap>();


            theBmpList.Add(Properties.Resources.GGPokerOurStackSign0);
            theBmpList.Add(Properties.Resources.GGPokerOurStackSign1);
            theBmpList.Add(Properties.Resources.GGPokerOurStackSign2);
            theBmpList.Add(Properties.Resources.GGPokerOurStackSign3);
            theBmpList.Add(Properties.Resources.GGPokerOurStackSign4);
            theBmpList.Add(Properties.Resources.GGPokerOurStackSign5);
            theBmpList.Add(Properties.Resources.GGPokerOurStackSign6);
            theBmpList.Add(Properties.Resources.GGPokerOurStackSign7);
            theBmpList.Add(Properties.Resources.GGPokerOurStackSign8);
            theBmpList.Add(Properties.Resources.GGPokerOurStackSign9);
            theBmpList.Add(Properties.Resources.GGPokerOurStackSignPoint);

            return theBmpList;
        }

        private List<Bitmap> getBmpListOurMoneySign()
        {
            List<Bitmap> theBmpList = new List<Bitmap>();


            theBmpList.Add(Properties.Resources.GGPokerOurStackSign0);
            theBmpList.Add(Properties.Resources.GGPokerOurStackSign1);
            theBmpList.Add(Properties.Resources.GGPokerOurStackSign2);
            theBmpList.Add(Properties.Resources.GGPokerOurStackSign3);
            theBmpList.Add(Properties.Resources.GGPokerOurStackSign4);
            theBmpList.Add(Properties.Resources.GGPokerOurStackSign5);
            theBmpList.Add(Properties.Resources.GGPokerOurStackSign6);
            theBmpList.Add(Properties.Resources.GGPokerOurStackSign7);
            theBmpList.Add(Properties.Resources.GGPokerOurStackSign8);
            theBmpList.Add(Properties.Resources.GGPokerOurStackSign9);
            theBmpList.Add(Properties.Resources.GGPokerOurStackSignPoint);

            return theBmpList;
        }

        private List<Bitmap> getBmpListPlayerBetNumberSign()
        {
            List<Bitmap> theBmpList = new List<Bitmap>();


            theBmpList.Add(Properties.Resources.GGPokerBetNumber0);
            theBmpList.Add(Properties.Resources.GGPokerBetNumber1);
            theBmpList.Add(Properties.Resources.GGPokerBetNumber2);
            theBmpList.Add(Properties.Resources.GGPokerBetNumber3);
            theBmpList.Add(Properties.Resources.GGPokerBetNumber4);
            theBmpList.Add(Properties.Resources.GGPokerBetNumber5);
            theBmpList.Add(Properties.Resources.GGPokerBetNumber6);
            theBmpList.Add(Properties.Resources.GGPokerBetNumber7);
            theBmpList.Add(Properties.Resources.GGPokerBetNumber8);
            theBmpList.Add(Properties.Resources.GGPokerBetNumber9);

            return theBmpList;
        }

        private List<int[]> sort(List<int[]> theList)
        {
            int min;
            for (int i = 0; i < theList.Count - 1; i++)
            {
                min = i;
                for (int j = i + 1; j < theList.Count; j++) // bubble sort improved
                {
                    if (theList[min][1] > theList[j][1])
                        min = j;
                }
                int[] temp = theList[i];
                theList[i] = theList[min];
                theList[min] = temp;
            }
            return theList;
        }

        public double WestPlayerStack
        {
            get
            {
                List<int[]> theList = getNumbers(GetWestPlayerStackBmp(), 10000, getBmpListPlayerMoneySign()); // cst pour alpha values
                theList = sort(theList);
                string theNumber = getTheStackNumber(theList);
                return Double.Parse(theNumber);
            }
        }

        public double WestPlayerBet
        {
            get
            {
                List<int[]> theList = getNumbers(GetWestPlayerBetBmp(), 300000, getBmpListPlayerBetNumberSign()); // cst pour alpha values
                theList = sort(theList);
                string theNumber = getTheBetNumber(theList);
                return Double.Parse(theNumber);
            }
        }

        public double NorthPlayerStack
        {
            get
            {
                List<int[]> theList = getNumbers(GetNorthPlayerStackBmp(), 10000, getBmpListPlayerMoneySign()); // cst pour alpha values
                theList = sort(theList);
                string theNumber = getTheStackNumber(theList);
                return Double.Parse(theNumber);
            }
        }

        public double NorthPlayerBet
        {
            get
            {
                List<int[]> theList = getNumbers(GetNorthPlayerBetBmp(), 300000, getBmpListPlayerBetNumberSign()); // cst pour alpha values
                theList = sort(theList);
                string theNumber = getTheBetNumber(theList);
                return Double.Parse(theNumber);
            }
        }

        public double EstPlayerStack
        {
            get
            {
                List<int[]> theList = getNumbers(GetEstPlayerStackBmp(), 50000, getBmpListPlayerMoneySign()); // cst pour alpha values
                theList = sort(theList);
                string theNumber = getTheStackNumber(theList);
                return Double.Parse(theNumber);
            }
        }

        public double EstPlayerBet
        {
            get
            {
                List<int[]> theList = getNumbers(GetEstPlayerBetBmp(), 300000, getBmpListPlayerBetNumberSign()); // cst pour alpha values
                theList = sort(theList);
                string theNumber = getTheBetNumber(theList);
                return Double.Parse(theNumber);
            }
        }

        public double SouthPlayerStack
        {
            get
            {
                List<int[]> theList = getNumbers(GetSouthPlayerStackBmp(), 100000, getBmpListPlayerMoneySign()); // cst pour alpha values
                theList = sort(theList);
                string theNumber = getTheStackNumber(theList);
                return Double.Parse(theNumber);
            }
        }

        public double SouthPlayerBet
        {
            get
            { 
                List<int[]> theList = getNumbers(GetSouthPlayerBetBmp(), 300000, getBmpListPlayerBetNumberSign()); // cst pour alpha values
                theList = sort(theList);
                string theNumber = getTheBetNumber(theList);
                return Double.Parse(theNumber);
            }
        }

        public bool isWestPlayerAllIn
        {
            get
            {
                List<Bitmap> temp = new List<Bitmap>();
                temp.Add(Properties.Resources.AllIn);
                List<int[]> theList = getNumbers(GetWestPlayerStackBmp(), 700000, temp); // cst pour alpha values
                return theList.Count != 0;
            }
        }

        public bool isNorthPlayerAllIn
        {
            get
            {
                List<Bitmap> temp = new List<Bitmap>();
                temp.Add(Properties.Resources.AllIn);
                List<int[]> theList = getNumbers(GetNorthPlayerStackBmp(), 700000, temp); // cst pour alpha values
                return theList.Count != 0;
            }
        }

        public bool isEstPlayerAllIn
        {
            get
            {
                List<Bitmap> temp = new List<Bitmap>();
                temp.Add(Properties.Resources.AllIn);
                List<int[]> theList = getNumbers(GetEstPlayerStackBmp(), 700000, temp); // cst pour alpha values
                return theList.Count != 0;
            }
        }

        public bool isSouthPlayerAllIn
        {
            get
            {
                List<Bitmap> temp = new List<Bitmap>();
                temp.Add(Properties.Resources.AllIn);
                List<int[]> theList = getNumbers(GetSouthPlayerStackBmp(), 700000, temp); // cst pour alpha values
                return theList.Count != 0;
            }
        }

        public CPosition getAllInButtonCoords()
        {
            List<Bitmap> theItem = new List<Bitmap>();
            theItem.Add(Properties.Resources.GGPokerAllInButton);
            List<CPosition> thePosition;
            thePosition = CDetectItem.RetournerPosItemList(PCurrentImage, theItem);
            if (thePosition.Count != 0)
            {
                return thePosition[0];
            }
            else
            {
                return new CPosition(500, 500);
                throw new Exception("All in button not found");
            }
        }

        public CPosition getFoldButtonCoords()
        {
            List<Bitmap> theItem = new List<Bitmap>();
            theItem.Add(Properties.Resources.GGPokerFoldButton);
            theItem.Add(Properties.Resources.GGPokerFoldButton2);
            List<CPosition> thePosition;
            thePosition = CDetectItem.RetournerPosItemList(PCurrentImage, theItem);
            if (thePosition.Count != 0)
            {
                return thePosition[0];
            }
            else
            {
                throw new Exception("Fold button not found");
            }
        }

        public bool IsDealMeInButtonPresent()
        {
            if (PCurrentImage == null)
                throw new Exception("There is no image of the table!");

            List<Bitmap> theItem = new List<Bitmap>();
            theItem.Add(Properties.Resources.GGDealMeInBmp);
            List<CPosition> thePosition;
            thePosition = CDetectItem.RetournerPosItemList(PCurrentImage, theItem);

            return thePosition.Count != 0;
        }

        public bool IsAllInButtonPresent()
        {
            if (PCurrentImage == null)
                throw new Exception("There is no image of the table!");

            List<Bitmap> theItem = new List<Bitmap>();
            theItem.Add(Properties.Resources.GGPokerAllInButton);
            List<CPosition> thePosition;
            thePosition = CDetectItem.RetournerPosItemList(PCurrentImage, theItem);

            return thePosition.Count != 0;
        }

        public bool IsFoldButtonPresent()
        {
            if (PCurrentImage == null)
                throw new Exception("There is no image of the table!");

            List<Bitmap> theItem = new List<Bitmap>();
            theItem.Add(Properties.Resources.GGPokerFoldButton);
            theItem.Add(Properties.Resources.GGPokerFoldButton2);
            List<CPosition> thePosition;
            thePosition = CDetectItem.RetournerPosItemList(PCurrentImage, theItem);

            return thePosition.Count != 0;
        }

        public CPosition GetDealMeInButtonCoords()
        {
            if (PCurrentImage == null)
                throw new Exception("There is no image of the table!");

            List<Bitmap> theItem = new List<Bitmap>();
            theItem.Add(Properties.Resources.GGDealMeInBmp);
            List<CPosition> thePosition;
            thePosition = CDetectItem.RetournerPosItemList(PCurrentImage, theItem);

            if (thePosition.Count != 0)
                return thePosition[0];            
            else
                throw new Exception("DealMeIn button not found");
        }

        public bool IsRemoveChipButtonAvailable()
        {
            if (PCurrentImage == null)
                throw new Exception("There is no image of the table!");

            List<Bitmap> theItem = new List<Bitmap>();
            theItem.Add(Properties.Resources.GGPokerRemoveChipLabel);
            List<CPosition> thePos = CDetectItem.RetournerPosItemList(PCurrentImage, theItem);
            return thePos.Count != 0;
        }

        public CPosition GetRemoveChipButtonCoords()
        {
            //TODO: Fixer cette methode la
            if (PCurrentImage == null)
                throw new Exception("There is no image of the table!");

            List<Bitmap> theItem = new List<Bitmap>();
            theItem.Add(Properties.Resources.GGPokerRemoveChipLabel);
            List<CPosition> thePos = CDetectItem.RetournerPosItemList(PCurrentImage, theItem);
            if (thePos.Count != 0)
            {
                return thePos[0];
            }
            else
            {
                return new CPosition(750, 750);
                //throw new Exception("RemoveChip button not found");
            }
        }

        public CPosition GetOKButtonCoords(HwndObject _window)
        {
            if (PCurrentImage == null)
                throw new Exception("There is no image of the table!");
            else if (_window == null)
                throw new Exception("The window received is null!");

            List<Bitmap> theItem = new List<Bitmap>();
            theItem.Add(Properties.Resources.GGPokerOkButton);
            List<CPosition> thePos = CDetectItem.RetournerPosItemList(PCurrentImage, theItem);

            if (thePos.Count != 0)
                return thePos[0];
            else
                throw new Exception("Ok button not found");            
        }

        public List<CPosition> GetSittingOutPlayersPosition()
        {
            if (PCurrentImage == null)
                throw new Exception("There is no image of the table!");

            List<Bitmap> theItem = new List<Bitmap>();
            theItem.Add(Properties.Resources.GGPokerSittingOut);
            theItem.Add(Properties.Resources.GGPokerSittingOut2);
            List<CPosition> thePos = CDetectItem.RetournerPosItemList(PCurrentImage, theItem);
            return thePos;
        }

        public List<CPosition> GetPlayersPosition()
        {
            if (PCurrentImage == null)
                throw new Exception("There is no image of the table!");

            List<Bitmap> theItem = new List<Bitmap>();
            theItem.Add(Properties.Resources.GGPokerPlayersBorder);
            theItem.Add(Properties.Resources.GGPokerPlayersBorder2);
            List<CPosition> thePos = CDetectItem.RetournerPosItemList(PCurrentImage, theItem);
            return thePos;
        }

        public Bitmap GetHandNumberBmp()
        {
            Bitmap bmp2 = PCurrentImage;
            Rectangle zoneTypeCarte2 = new Rectangle(CGGPokerConsantes.GG_HAND_NUMBER_OFFSET_WIDTH, 
                                                     CGGPokerConsantes.GG_HAND_NUMBER_OFFSET_HEIGHT, 
                                                     CGGPokerConsantes.GG_HAND_NUMBER_RECTANGLE_WIDTH, 
                                                     CGGPokerConsantes.GG_HAND_NUMBER_RECTANGLE_HEIGHT);
            bmp2 = (Bitmap)bmp2.Clone(zoneTypeCarte2, bmp2.PixelFormat);
            return bmp2;
        }

        public bool IsNewHand(Bitmap _hand)
        {
            if (PCurrentImage == null)
                throw new Exception("There is no image of the table!");
            else if (_hand == null)
                throw new Exception("There is no image of the hand received!");

            List<Bitmap> handContainer = new List<Bitmap>();
            handContainer.Add(_hand);
            List<CPosition> dsa = CDetectItem.RetournerPosItemList(PCurrentImage, handContainer);
            return dsa.Count == 0;
        }

        public void SaveCurrentImageToFile(string filename)
        {
            PCurrentImage.Save(filename);
        }

    }
}
