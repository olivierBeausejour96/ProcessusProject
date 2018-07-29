using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using VersionOfficielle.Interfaces;
using WindowScrape.Types;

namespace VersionOfficielle
{
    class CGGPokerSoftwareAction
    {
        private const string DEFAULT_ERROR_MESSAGE = "LE ROBOT NE FONCTIONNE PLUS!";
        private const ulong DEFAULT_PHONE_NUMBER = 18198613599;
        private const string DEFAULT_PROVIDER = "sms.rogers.com";
        private const string DEFAULT_EMAIL_ADDRESS = "jonathanclavetg@gmail.com";

        /// <summary>
        /// List of bots that are subscribed to the website.
        /// </summary>
        /// <param name="_table">See the method in ITableInformations</param>
        /// <returns>See the method in ITableInformations</returns>
        public delegate bool getDecision(ITableInformations _table);

        private object FFLockID;
        private List<string> FFProcessList;
        public Dictionary<int, HwndObject> FFWindowsDictionary;
        private int FFNbWindows;
        private getDecision FFBotMakeDecisionMethod;
        private List<CGGPokerTableInformations> FFLstTableInformations;
        private bool FFKeepWatching;
        private Form FFParentForm;

        private List<string> FFLstEmailAddresses;
        private List<CPhoneNumber> FFLstPhoneNumbers;

        private List<string> getWindowList()
        {
            IDictionary<IntPtr, string> lstOpenWindows = null;
            List<string> lstWindowList = new List<string>();

            lstOpenWindows = OpenWindowGetter.GetOpenWindows();

            for (int currentWindowIndex = 0; currentWindowIndex < lstOpenWindows.Count; ++currentWindowIndex)
            {
                string windowTitle = lstOpenWindows.ElementAt(currentWindowIndex).Value;

                if (windowTitle.Contains("$"))
                    lstWindowList.Add(windowTitle);
            }

            return lstWindowList;
        }

        private HwndObject getWindowHandleFromTitle(string _windowTitle)
        {
            IntPtr hwnd = IntPtr.Zero;
            IntPtr hwndChild = IntPtr.Zero;

            hwnd = CWindowsMethodes.FindWindow(null, _windowTitle);
            return new HwndObject(hwnd);
        }

        public CGGPokerSoftwareAction(getDecision _bot, Form _parentForm)
        {
            FFProcessList = getWindowList();
            FFNbWindows = 0;
            FFWindowsDictionary = new Dictionary<int, HwndObject>();

            for (int currentProcessIndex = 0; currentProcessIndex < FFProcessList.Count; ++currentProcessIndex)
                FFWindowsDictionary.Add(currentProcessIndex, getWindowHandleFromTitle(FFProcessList[currentProcessIndex]));

            ResizeWindowsToCorrectSize();
            ++FFNbWindows;
            FFBotMakeDecisionMethod = _bot;
            FFLstTableInformations = new List<CGGPokerTableInformations>();
            for (int currentWindowIndex = 0; currentWindowIndex < FFWindowsDictionary.Count; ++currentWindowIndex)
                FFLstTableInformations.Add(new CGGPokerTableInformations(FFWindowsDictionary.ElementAt(currentWindowIndex).Value));

            FFLockID = new object();
            FFParentForm = _parentForm;
            // For error report
            FFLstEmailAddresses = new List<string>();
            FFLstPhoneNumbers = new List<CPhoneNumber>();
            FFLstEmailAddresses.Add(DEFAULT_EMAIL_ADDRESS);
            FFLstPhoneNumbers.Add(new CPhoneNumber(DEFAULT_PHONE_NUMBER, DEFAULT_PROVIDER));
        }

        public void StartWatching()
        {
            try
            {
                List<Thread> lstTables = new List<Thread>();

                for (int currentWindowIndex = 0; currentWindowIndex < FFWindowsDictionary.Count; ++currentWindowIndex)
                {
                    int indexToUse = currentWindowIndex;
                    Thread newWatcherTable = new Thread(delegate () { WatchTable(indexToUse); });
                    newWatcherTable.Priority = ThreadPriority.Highest;

                    lstTables.Add(newWatcherTable);
                    newWatcherTable.Start();
                }
            }
            catch
            {
                CLogger.WriteToFileAndDeleteLogs();
                //createErrorReport(DEFAULT_ERROR_MESSAGE + " ---- EXCEPTION ---- " + e.Message + " ---- SOURCE ---- " + e.Source + " ---- STACK TRACE ----" + e.StackTrace);
            }
        }

        public void StopWatching()
        {
            FFKeepWatching = false;
            CLogger.WriteToFileAndDeleteLogs();
        }

        /// <summary>
        /// Watch a specific table on the screen.
        /// </summary>
        /// <param name="_tableIndex">Represents the table index from the FFDictionnary</param>
        private void WatchTable(int _tableIndex)
        {
            const string IMAGES_PATH = "C:\\Users\\olibb\\Desktop\\GGPokerImages\\";

            CGGPokerTableInformations tableInformation = FFLstTableInformations.ElementAt(_tableIndex);

            try
            {
                bool alreadyUpdated = false;
                bool hasPlayed = false;
                FFKeepWatching = true;
                CLogger.CreateLogContainer(IMAGES_PATH);

                while (FFKeepWatching)
                {
                    Thread.Sleep(100);
                    tableInformation.PReader.ActualizeCurrentImage();

                    if (tableInformation.PReader.hasCardsInHand())
                    {
                        if (!alreadyUpdated)
                        {
                            tableInformation.ActualizeHandNumber();
                            tableInformation.ActualizePlayingPlayersCount();
                            tableInformation.ActualizeHand();
                            tableInformation.ActualizePositions();
                            alreadyUpdated = true;

                            if (tableInformation.isOurHandInThisRange(CGGPokerConsantes.GG_SNAP_FOLD_RANGE))
                            {
                                CMouseController.AddClickToQueue(DoFold, _tableIndex);
                                hasPlayed = true;
                            }
                        }
                        if (tableInformation.PReader.isMyTurn())
                        {
                            if (alreadyUpdated && !hasPlayed)
                            {
                                tableInformation.ActualizeStackAndBets();

                                CLogger.SetLogImage(tableInformation.PReader.PCurrentImage);

                                if (FFBotMakeDecisionMethod(tableInformation))
                                {
                                    CMouseController.AddClickToQueue(DoAllIn, _tableIndex);
                                    
                                }
                                else
                                {
                                    CMouseController.AddClickToQueue(DoFold, _tableIndex);                                    
                                }

                                // Creates a new log container for the next hand.   
                                CLogger.CreateLogContainer(IMAGES_PATH);
                                hasPlayed = true;
                            }

                        }
                        else
                        {
                            hasPlayed = false;
                        }
                    }
                    else if (tableInformation.isNewHand())
                    {
                        alreadyUpdated = false;
                        hasPlayed = false;

                        if (tableInformation.PReader.IsDealMeInButtonPresent())
                        {
                            CMouseController.AddClickToQueue(DoDealMeIn, _tableIndex);                            
                        }
                    }
                }
            }
            catch (Exception e)
            {
                const int MAX_ATTEMPT = 60;
                const string EXCEPTION_PATH = "C:\\Users\\olibb\\Desktop\\GGPokerImages\\Exceptions\\";

                CLogger.ChangePathOfLastestLogContainer(EXCEPTION_PATH);
                CLogger.AddLog("EXCEPTION CAUGHT: " + e.Message);
                CLogger.AddLog("STACK TRACE: " + e.StackTrace);
                CLogger.SetLogImage(tableInformation.PReader.PCurrentImage);

                int currentAttempt = 0;

                while (!tableInformation.PReader.IsDealMeInButtonPresent() && !tableInformation.PReader.IsFoldButtonPresent() && currentAttempt <= MAX_ATTEMPT)
                {
                    tableInformation.PReader.ActualizeCurrentImage();
                    ++currentAttempt;
                    Thread.Sleep(1000);
                }

                // Implicitely we know that if the current attempt is below the max attempt,
                // it means that the Deal Me In button is there.
                if (currentAttempt <= MAX_ATTEMPT)
                {
                    // We refresh in case the fold button just disappeared and the deal me in button just appeared.
                    tableInformation.PReader.ActualizeCurrentImage();

                    if (tableInformation.PReader.IsDealMeInButtonPresent())
                    {
                        CMouseController.AddClickToQueue(DoDealMeIn, _tableIndex);
                    }
                       
                    else if (tableInformation.PReader.IsFoldButtonPresent())
                    {
                        CMouseController.AddClickToQueue(DoFold, _tableIndex);
                    }
                        

                    CMouseController.MoveMouseWithImprecision(750, 750);
                    // We need to take a screenshot again
                    tableInformation.PReader.ActualizeCurrentImage();

                    CLogger.CreateLogContainer(EXCEPTION_PATH);
                    CLogger.AddLog("Restarting the bot");
                    CLogger.SetLogImage(tableInformation.PReader.PCurrentImage);
                    WatchTable(_tableIndex);
                }
                else
                {
                    CLogger.CreateLogContainer(EXCEPTION_PATH);
                    CLogger.AddLog("Unable to find the fold or deal me in button. Ending the bot...");
                    CLogger.SetLogImage(tableInformation.PReader.PCurrentImage);
                    throw new Exception("Exceeded the maximum time for finding the Deal Me In button.");
                }
            }
        }

        private void DoAllIn(int _tableIndex)
        {
            CLogger.AddLog("Trying to click on All In button...");
            CGGPokerTableInformations tableInformations = FFLstTableInformations[_tableIndex];

            CPosition windowPos = new CPosition(FFWindowsDictionary[_tableIndex].Location.X, FFWindowsDictionary[_tableIndex].Location.Y);
            CPosition buttonCoordOnScreen = tableInformations.PReader.getAllInButtonCoords() + windowPos;

            CMouseController.MoveMouseWithImprecision(buttonCoordOnScreen.X, buttonCoordOnScreen.Y);
            tableInformations.PReader.SaveCurrentImageToFile(CGGPokerConsantes.GG_AUTO_SAVE_FOLDER_PATH + "HandNb.bmp");
            CMouseController.DoMouseClickWithRandomWaitingTime(42, 277);
            Thread.Sleep(50);
            tableInformations.PReader.ActualizeCurrentImage();
            if (tableInformations.PReader.IsAllInButtonPresent())
                DoAllIn(_tableIndex);

            CLogger.AddLog("Button clicked!");
            CMouseController.MoveMouseWithImprecision(750, 750);
        }

        public void DoFold(int _tableIndex)
        {
            const int MAX_CYCLE = 3;
            int nbCycle = 0;

            CLogger.AddLog("Trying to click on Fold button...");
            CGGPokerTableInformations tableInformations = FFLstTableInformations[_tableIndex];

            tableInformations.PReader.ActualizeCurrentImage();
            while (!tableInformations.PReader.IsFoldButtonPresent() && (nbCycle < MAX_CYCLE))
            {
                tableInformations.PReader.ActualizeCurrentImage();
                Thread.Sleep(500);
            }

            // Happens when the fold button disappears somehow
            if (!tableInformations.PReader.IsFoldButtonPresent())
                return;

            CPosition windowPos = new CPosition(FFWindowsDictionary[_tableIndex].Location.X, FFWindowsDictionary[_tableIndex].Location.Y);
            CPosition buttonCoordOnScreen = tableInformations.PReader.getFoldButtonCoords() + windowPos;

            CMouseController.MoveMouseWithImprecision(buttonCoordOnScreen.X, buttonCoordOnScreen.Y);
            tableInformations.PReader.SaveCurrentImageToFile(CGGPokerConsantes.GG_AUTO_SAVE_FOLDER_PATH + "HandNb0.bmp");
            CMouseController.DoMouseClickWithRandomWaitingTime(42, 277);

            CLogger.AddLog("Button clicked!");
            CMouseController.MoveMouseWithImprecision(750, 750);
        }

        public void DoDealMeIn(int _tableIndex)
        {
            CLogger.AddLog("Trying to click on Deal Me In button...");
            CGGPokerTableInformations tableInformations = FFLstTableInformations[_tableIndex];

            CPosition windowPos = new CPosition(FFWindowsDictionary[_tableIndex].Location.X, FFWindowsDictionary[_tableIndex].Location.Y);
            CPosition buttonCoordOnScreen = tableInformations.PReader.GetDealMeInButtonCoords() + windowPos;

            CMouseController.MoveMouseWithImprecision(buttonCoordOnScreen.X + CGGPokerConsantes.GG_DEAL_ME_IN_WIDTH_OFFSET, buttonCoordOnScreen.Y + CGGPokerConsantes.GG_DEAL_ME_IN_HEIGHT_OFFSET, 0, 0);
            CMouseController.DoMouseClickWithRandomWaitingTime(42, 277);
            CLogger.AddLog("Button clicked!");
        }

        public void DoRemoveChip()
        {
            throw new NotImplementedException("Pas implémenté");
            /*
            CPosition windowPos = new CPosition(FFwindowDictionary[0].Location.X, FFwindowDictionary[0].Location.Y);
            CPosition buttonCoordOnScreen = FFTableInformations.FFReader.getRemoveChipButtonCoords() + windowPos;

            CMouseController.MoveMouseWithImprecision(buttonCoordOnScreen.X, buttonCoordOnScreen.Y);
            Thread.Sleep(250);
            CMouseController.DoMouseClick();

            Thread.Sleep(300);
   
            HwndObject theHandle = getWindowHandleFromTitle("Remove Chips");
            windowPos = new CPosition(theHandle.Location.X, theHandle.Location.Y);
            if (windowPos.X == 0 && windowPos.Y == 0) return;

            buttonCoordOnScreen = FFTableInformations.FFReader.getOkButtonCoords(theHandle) + windowPos;

            CMouseController.MoveMouseWithImprecision(buttonCoordOnScreen.X, buttonCoordOnScreen.Y);
            CMouseController.DoMouseClick();*/
        }

        public void ResizeWindowsToCorrectSize()
        {
            int currentX = 0;
            int currentY = 0;

            for (int currentWindowIndex = 0; currentWindowIndex < FFWindowsDictionary.Count; ++currentWindowIndex)
            {
                Size windowSize = FFWindowsDictionary[currentWindowIndex].Size;

                windowSize.Width = CGGPokerConsantes.GG_WINDOW_WIDTH;
                windowSize.Height = CGGPokerConsantes.GG_WINDOW_HEIGHT;
                FFWindowsDictionary[currentWindowIndex].Size = windowSize;
                FFWindowsDictionary[currentWindowIndex].Location = new Point(currentX, currentY);

                if ((currentX + CGGPokerConsantes.GG_WINDOW_WIDTH * 2) <= Screen.PrimaryScreen.Bounds.Width)
                    currentX = currentX + CGGPokerConsantes.GG_WINDOW_WIDTH;
                else
                {
                    currentX = 0;
                    currentY = currentY + CGGPokerConsantes.GG_WINDOW_HEIGHT;
                }
            }
        }

        private string createErrorReport(string _errorMessage)
        {
            using (Bitmap bmp = new Bitmap(Screen.PrimaryScreen.Bounds.Width,
                               Screen.PrimaryScreen.Bounds.Height))
            {
                using (Graphics g = Graphics.FromImage(bmp))
                {
                    CAlert alertsManager = new CAlert(FFLstEmailAddresses, FFLstPhoneNumbers);
                    string filename = "AmigoCrash " + DateTime.Now.ToString("dd-MM-yyyy hh-mm-ss") + ".png";
                    string path = Path.Combine("C:\\AmigoScreenshots\\", filename);
                    List<string> imagesPath = new List<string>();

                    FFParentForm.Opacity = 1.0;
                    g.CopyFromScreen(Screen.PrimaryScreen.Bounds.X,
                                     Screen.PrimaryScreen.Bounds.Y, 0, 0, bmp.Size);
                    bmp.Save(path);
                    imagesPath.Add(path);

                    filename = "AmigoCrash " + DateTime.Now.ToString("dd-MM-yyyy hh-mm-ss") + "(HalfOpacity).png";
                    path = Path.Combine("C:\\AmigoScreenshots\\", filename);
                    FFParentForm.Opacity = 0.6;
                    g.CopyFromScreen(Screen.PrimaryScreen.Bounds.X,
                                     Screen.PrimaryScreen.Bounds.Y, 0, 0, bmp.Size);
                    bmp.Save(path);
                    imagesPath.Add(path);

                    filename = "AmigoCrash " + DateTime.Now.ToString("dd-MM-yyyy hh-mm-ss") + "(FullOpacity).png";
                    path = Path.Combine("C:\\AmigoScreenshots\\", filename);
                    FFParentForm.Opacity = 0.0;
                    g.CopyFromScreen(Screen.PrimaryScreen.Bounds.X,
                                     Screen.PrimaryScreen.Bounds.Y, 0, 0, bmp.Size);
                    bmp.Save(path);
                    imagesPath.Add(path);

                    FFParentForm.Opacity = 1.0; // Revert the opacity to the default settings

                    alertsManager.sendAlertToEmails("ALERTE - IMPORTANT", _errorMessage, imagesPath);
                    alertsManager.sendAlertToPhoneNumbers("ALERTE - IMPORTANT", _errorMessage);
                    alertsManager.sendAlertToPhoneNumbers("ALERTE - IMPORTANT", _errorMessage);
                    alertsManager.sendAlertToPhoneNumbers("ALERTE - IMPORTANT", _errorMessage);
                    return path;
                }
            }
        }
    }
}
