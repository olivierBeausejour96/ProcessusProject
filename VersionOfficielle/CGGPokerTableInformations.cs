using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using VersionOfficielle.Helpers;
using VersionOfficielle.Interfaces;
using WindowScrape.Types;

namespace VersionOfficielle
{
    public enum ScreenPosition
    {
        North = 0,
        Est,
        South,
        West,

        ScreenPositionCount
    }

    public class CGGPokerTableInformations : ITableInformations
    {
        private CGGPokerHandReader FFHandReader;
        private Dictionary<ScreenPosition, CPlayer> FFTable;
        private Dictionary<PokerPosition, CPokerPlayer> FFTablePoker;
        private ScreenPosition FFcardPosBB;
        private ScreenPosition FFcardPosSB;
        private ScreenPosition FFcardPosDE;
        private ScreenPosition FFcardPosCO;

        private CPokerPlayer FFDealer;
        private CPokerPlayer FFSB;
        private CPokerPlayer FFBB;
        private CPokerPlayer FFCO;

        public CGGPokerBoardReader PReader;

        public HwndObject FFWindowHandle;
        private Bitmap FFCurrentHandNumber;

        private List<string> FFLstLogs;
        private int FFPlayingPlayerCount;

        public CGGPokerTableInformations(HwndObject _windowHandle)
        {
            FFWindowHandle = _windowHandle;
            FFHandReader = new CGGPokerHandReader(_windowHandle);
            PReader = new CGGPokerBoardReader(_windowHandle);
            FFTable = new Dictionary<ScreenPosition, CPlayer>();
            FFTablePoker = new Dictionary<PokerPosition, CPokerPlayer>();
            FFCurrentHandNumber = null;
            FFLstLogs = new List<string>();

            FFTable.Add(ScreenPosition.West, new CPlayer(PokerPosition.PokerPositionCount, 0, 0, PlayerStates.openedSeat));
            FFTable.Add(ScreenPosition.North, new CPlayer(PokerPosition.PokerPositionCount, 0, 0, PlayerStates.openedSeat));
            FFTable.Add(ScreenPosition.Est, new CPlayer(PokerPosition.PokerPositionCount, 0, 0, PlayerStates.openedSeat));
            FFTable.Add(ScreenPosition.South, new CPlayer(PokerPosition.PokerPositionCount, 0, 0, PlayerStates.openedSeat));

            FFTablePoker.Add(PokerPosition.SB, new CPokerPlayer(ScreenPosition.ScreenPositionCount, 0, 0, PlayerStates.openedSeat));
            FFTablePoker.Add(PokerPosition.BB, new CPokerPlayer(ScreenPosition.ScreenPositionCount, 0, 0, PlayerStates.openedSeat));
            FFTablePoker.Add(PokerPosition.dealer, new CPokerPlayer(ScreenPosition.ScreenPositionCount, 0, 0, PlayerStates.openedSeat));
            FFTablePoker.Add(PokerPosition.CO, new CPokerPlayer(ScreenPosition.ScreenPositionCount, 0, 0, PlayerStates.openedSeat));
        }

        public void ActualizePositions()
        {
            FFLstLogs.Add("------ UPDATING Positions ------");
            FFBB.FFPos = FFcardPosBB;
            FFSB.FFPos = FFcardPosSB;
            FFDealer.FFPos = FFcardPosDE;

            FFTable[FFcardPosBB].PPos = PokerPosition.BB;
            FFTable[FFcardPosDE].PPos = PokerPosition.dealer;
            FFTable[FFcardPosSB].PPos = PokerPosition.SB;
            if (FFPlayingPlayerCount == 4)
                FFTable[FFcardPosCO].PPos = PokerPosition.CO;
            FFLstLogs.Add("------ UPDATING done ------");

            FFLstLogs.Add("------ POSITIONS OF PLAYERS ------");
            FFLstLogs.Add("NORTH: " + getPokerPosByCardPos(ScreenPosition.North));
            FFLstLogs.Add("SOUTH: " + getPokerPosByCardPos(ScreenPosition.South));
            FFLstLogs.Add("EAST: " + getPokerPosByCardPos(ScreenPosition.Est));
            FFLstLogs.Add("WEST: " + getPokerPosByCardPos(ScreenPosition.West));
        }

        public void ActualizeHand()
        {
            FFLstLogs.Add("Actualizing hand");
            FFHandReader.ActualizeHand();
            FFLstLogs.Add("Actualizing done");
        }

        public void ActualizeStackAndBets()
        {

            FFTable[FFcardPosBB].PBet = FFBB.FFBet = getBetFromCardPos(FFcardPosBB);
            FFTable[FFcardPosDE].PBet = FFDealer.FFBet = getBetFromCardPos(FFcardPosDE);
            FFTable[FFcardPosSB].PBet = FFSB.FFBet = getBetFromCardPos(FFcardPosSB);
            if (FFPlayingPlayerCount == 4)
            {
                FFTable[FFcardPosCO].PBet = FFCO.FFBet = getBetFromCardPos(FFcardPosCO);
            }

            FFTable[FFcardPosBB].PStack = FFBB.FFStack = getStackFromCardPos(FFcardPosBB);
            FFTable[FFcardPosDE].PStack = FFDealer.FFStack = getStackFromCardPos(FFcardPosDE);
            FFTable[FFcardPosSB].PStack = FFSB.FFStack = getStackFromCardPos(FFcardPosSB);
            if (FFPlayingPlayerCount == 4)
            {
                FFTable[FFcardPosCO].PStack = FFCO.FFStack = getStackFromCardPos(FFcardPosCO);
            }

            FFTablePoker[PokerPosition.BB] = FFBB;
            FFTablePoker[PokerPosition.SB] = FFSB;
            FFTablePoker[PokerPosition.dealer] = FFDealer;
            FFTablePoker[PokerPosition.CO] = FFCO;

            FFLstLogs.Add("------ STACKS ------");
            FFLstLogs.Add("NORTH: " + FFTable[ScreenPosition.North].PStack);
            FFLstLogs.Add("SOUTH: " + FFTable[ScreenPosition.South].PStack);
            FFLstLogs.Add("EAST: " + FFTable[ScreenPosition.Est].PStack);
            FFLstLogs.Add("WEST: " + FFTable[ScreenPosition.West].PStack);
            FFLstLogs.Add("------ BETS ------");
            FFLstLogs.Add("NORTH: " + FFTable[ScreenPosition.North].PBet);
            FFLstLogs.Add("SOUTH: " + FFTable[ScreenPosition.South].PBet);
            FFLstLogs.Add("EAST: " + FFTable[ScreenPosition.Est].PBet);
            FFLstLogs.Add("WEST: " + FFTable[ScreenPosition.West].PBet);
        }

        public double getOurStack()
        {
            return FFTable[ScreenPosition.South].PBet + FFTable[ScreenPosition.South].PStack;
        }

        private bool isBetPresentAtThisCardPos(ScreenPosition _pos)
        {
            switch (_pos)
            {
                case ScreenPosition.North:
                    return PReader.NorthPlayerBet != 0;
                case ScreenPosition.West:
                    return PReader.WestPlayerBet != 0;
                case ScreenPosition.South:
                    return PReader.SouthPlayerBet != 0;
                case ScreenPosition.Est:
                    return PReader.EstPlayerBet != 0;
                default:
                    throw new Exception("Card pos is invalid");
            }
        }

        private double getBetFromCardPos(ScreenPosition _pos)
        {
            switch (_pos)
            {
                case ScreenPosition.North:
                    return PReader.NorthPlayerBet;
                case ScreenPosition.West:
                    return PReader.WestPlayerBet;
                case ScreenPosition.South:
                    return PReader.SouthPlayerBet;
                case ScreenPosition.Est:
                    return PReader.EstPlayerBet;
                default:
                    throw new Exception("Card pos is invalid");
            }
        }

        private ScreenPosition getPreviousCardinalPos(ScreenPosition _pos)
        {
            int thePos = (int)_pos;
            --thePos;
            if (thePos == -1)
                thePos = 3;
            return (ScreenPosition)thePos;
        }

        private ScreenPosition getNextCardinalPos(ScreenPosition _pos)
        {
            int thePos = (int)_pos;
            ++thePos;
            if (thePos == (int)ScreenPosition.ScreenPositionCount)
                thePos = 0;
            return (ScreenPosition)thePos;
        }

        private ScreenPosition getSBCardPos()
        {
            ScreenPosition posBB = FFcardPosBB;
            ScreenPosition posSB = getPreviousCardinalPos(posBB);

            if (!isBetPresentAtThisCardPos(posSB))
            {
                posSB = getPreviousCardinalPos(posSB);
                if (!isBetPresentAtThisCardPos(posSB))
                {
                    posSB = getPreviousCardinalPos(posSB);
                    if (!isBetPresentAtThisCardPos(posSB))
                    {
                        throw new Exception("SB not found");
                    }
                }
            }
            return posSB;
        }

        private ScreenPosition getBBCardPos()
        {
            ScreenPosition posBB;

            if (PReader.EstPlayerBet == CGGPokerConsantes.GG_LIMIT.getLimit())
            {
                posBB = ScreenPosition.Est;
            }
            else if (PReader.NorthPlayerBet == CGGPokerConsantes.GG_LIMIT.getLimit())
            {
                posBB = ScreenPosition.North;
            }
            else if (PReader.WestPlayerBet == CGGPokerConsantes.GG_LIMIT.getLimit())
            {
                posBB = ScreenPosition.West;
            }
            else if (PReader.SouthPlayerBet == CGGPokerConsantes.GG_LIMIT.getLimit())
            {
                posBB = ScreenPosition.South;
            }
            else
            {
                throw new Exception("BB not found");
            }
            ScreenPosition insurance = getPreviousCardinalPos(posBB);
            if (getBetFromCardPos(insurance) == CGGPokerConsantes.GG_LIMIT.getLimit())
                posBB = insurance;
            return posBB;
        }

        private ScreenPosition getDealerCardPos()
        {
            CPosition dealerScreenPos = PReader.GetDealerPositionOnScreen();
            ScreenPosition dealerCardPos = getPlayerScreenPosition(dealerScreenPos);
            return dealerCardPos;
        }

        private ScreenPosition getCOCardPos()
        {
            return getPreviousCardinalPos(FFcardPosDE);
        }

        private ScreenPosition getPlayerScreenPosition(CPosition _pos)
        {
            if (_pos.Y < CGGPokerConsantes.GG_SCREEN_NORTH_LOWER_BOUND)
            {
                return ScreenPosition.North;
            }
            else if (_pos.X > CGGPokerConsantes.GG_SCREEN_EST_LEFT_BOUND)
            {
                return ScreenPosition.Est;
            }
            else if (_pos.Y > CGGPokerConsantes.GG_SCREEN_SOUTH_UPPER_BOUND)
            {
                return ScreenPosition.South;
            }
            else if (_pos.X < CGGPokerConsantes.GG_SCREEN_WEST_RIGHT_BOUND)
            {
                return ScreenPosition.West;
            }
            else
            {
                throw new Exception("The position specified is not a player position");
            }
        }

        private ScreenPosition getCardPosByPokerPos(PokerPosition _pos)
        {
            return FFTablePoker[_pos].FFPos;
        }

        private PokerPosition getPokerPosByCardPos(ScreenPosition _pos)
        {
            return FFTable[_pos].PPos;
        }

        private double getStackFromCardPos(ScreenPosition _pos)
        {
            switch (_pos)
            {
                case ScreenPosition.North:
                    return PReader.NorthPlayerStack;
                case ScreenPosition.West:
                    return PReader.WestPlayerStack;
                case ScreenPosition.South:
                    return PReader.SouthPlayerStack;
                case ScreenPosition.Est:
                    return PReader.EstPlayerStack;
                default:
                    throw new Exception("Card pos is invalid");
            }
        }

        private double getStackFromPokerPos(PokerPosition _pos)
        {
            return FFTablePoker[_pos].FFStack;
        }

        private double getBiggestStackNextPlayers(PokerPosition _pos)
        {
            double biggestStackNextPlayers = FFBB.FFStack + FFBB.FFBet;
            ScreenPosition nextPos = getNextCardinalPos(getCardPosByPokerPos(_pos));
            ScreenPosition cardPosBB = FFcardPosBB;

            if (nextPos != cardPosBB)
            {
                biggestStackNextPlayers = Math.Max(biggestStackNextPlayers, FFTable[nextPos].PStack + FFTable[nextPos].PBet);
                nextPos = getNextCardinalPos(nextPos);
                if (nextPos != cardPosBB)
                {
                    biggestStackNextPlayers = Math.Max(biggestStackNextPlayers, FFTable[nextPos].PStack + FFTable[nextPos].PBet);
                    nextPos = getNextCardinalPos(nextPos);
                    if (nextPos != cardPosBB)
                    {
                        throw new Exception("No BB detected, PROBLEM!!!!!");
                    }
                }
            }
            return biggestStackNextPlayers;
        }

        public int getNbDetectedPlayers()
        {
            //TODO: À vérifier cette méthode
            // not working properly
            List<CPosition> theList = PReader.GetPlayersPosition();

            bool westPresent = false;
            bool northPresent = false;
            bool estPresent = false;

            foreach (CPosition pos in theList)
            {
                if (!westPresent && pos.X < CGGPokerConsantes.GG_SCREEN_WEST_RIGHT_BOUND)
                    westPresent = true;
                if (!northPresent && pos.Y < CGGPokerConsantes.GG_SCREEN_NORTH_LOWER_BOUND)
                    northPresent = true;
                if (!estPresent && pos.X > CGGPokerConsantes.GG_SCREEN_EST_LEFT_BOUND)
                    estPresent = true;

            }

            int playerCount = 1;
            if (westPresent) playerCount++;
            if (northPresent) playerCount++;
            if (estPresent) playerCount++;

            return playerCount;
        }

        public void ActualizePlayingPlayersCount()
        {
            #region Updating the cardinal positions of players since we use these to update the number of playing players.

            FFDealer = new CPokerPlayer();
            FFSB = new CPokerPlayer();
            FFBB = new CPokerPlayer();
            FFCO = new CPokerPlayer();

            for (int i = 0; i < (int)ScreenPosition.ScreenPositionCount; i++)            
                FFTable[(ScreenPosition)i].Reinitialize();
            
            FFLstLogs.Add("------ UPDATING FFPOS ------");
            FFcardPosBB = getBBCardPos();
            FFcardPosSB = getSBCardPos();
            FFcardPosDE = getDealerCardPos();

            if (getPreviousCardinalPos(FFcardPosDE) != FFcardPosBB)
            {
                FFcardPosCO = getCOCardPos();
                FFCO.FFPos = FFcardPosCO;
            }
            #endregion
            #region Getting the number of players
            if (FFcardPosDE == FFcardPosSB)            
                FFPlayingPlayerCount = 2;            
            else if (FFcardPosSB != getPreviousCardinalPos(FFcardPosBB) || FFcardPosDE != getPreviousCardinalPos(FFcardPosSB))            
                FFPlayingPlayerCount = 3;            
            else          
                FFPlayingPlayerCount = 4;            
            #endregion
        }

        public string PlayerListToText()
        {
            ActualizePlayingPlayersCount();
            string theString = "";
            theString += "Nb Player: " + FFPlayingPlayerCount.ToString() + "\n";

            theString += "Player States: " + "\n";
            theString += "      | ";
            theString += getPlayerPokerPosString(ScreenPosition.North);
            theString += " |\n ";
            theString += getPlayerPokerPosString(ScreenPosition.West);
            theString += " |      | ";
            theString += getPlayerPokerPosString(ScreenPosition.Est);
            theString += "\n      | ";
            theString += getPlayerPokerPosString(ScreenPosition.South);
            theString += " |\n\n";

            theString += "Player stacks: " + "\n";
            theString += "      | ";
            theString += GetPlayerStackToString(ScreenPosition.North);
            theString += " |\n ";
            theString += GetPlayerStackToString(ScreenPosition.West);
            theString += " |      | ";
            theString += GetPlayerStackToString(ScreenPosition.Est);
            theString += "\n      | ";
            theString += GetPlayerStackToString(ScreenPosition.South);
            theString += " |\n\n";

            return theString;
        }

        public string getPlayerPokerPosString(ScreenPosition _pos)
        {
            PokerPosition pokerPos = getPokerPosByCardPos(_pos);

            switch (pokerPos)
            {
                case PokerPosition.BB:
                    return "BB";
                case PokerPosition.SB:
                    return "SB";
                case PokerPosition.dealer:
                    return "De";
                case PokerPosition.CO:
                    return "CO";
                default:
                    return "";             
            }
        }

        private string GetPlayerStackToString(ScreenPosition _pos)
        {
            return getStackFromCardPos(_pos).ToString();
        }

        private PokerPosition myPokerPosition()
        {
            return FFTable[ScreenPosition.South].PPos;
        }

        private bool isPlayerAllIn(PokerPosition _pos)
        {
            return FFTablePoker[_pos].isAllin();
        }

        private bool isPlayerAllIn(ScreenPosition _pos)
        {
            return FFTable[_pos].IsAllIn();
        }

        public void ActualizeHandNumber()
        {
            FFLstLogs.Add("Updating Hand Number");
            FFCurrentHandNumber = PReader.GetHandNumberBmp();
            FFLstLogs.Add("Updating Done");
        }

        public bool isNewHand()
        {
            if (FFCurrentHandNumber == null)
                return true;
            return PReader.IsNewHand(FFCurrentHandNumber);
        }

        public Dictionary<CAllInPokerGame.Position, byte> getBBEquivalentOfCallers()
        {
            if (!someoneOpenShoved() || getNumberOfCallers() == 0)
                throw new Exception("there is no callers");

            PokerPosition openShoverPokerPos = (PokerPosition)getPositionOfOpenShover();
            ScreenPosition screenPosOfOpenShover = getCardPosByPokerPos(openShoverPokerPos);
            Dictionary<CAllInPokerGame.Position, byte> theDictionnary = new Dictionary<CAllInPokerGame.Position, byte>();

            if (getNumberOfCallers() == 2)
            {
                double stackCaller1 = FFTablePoker[PokerPosition.dealer].FFBet;
                double stackCaller2 = FFTablePoker[PokerPosition.SB].FFBet;
                double stackOpener = FFTablePoker[PokerPosition.CO].FFBet;
                double myStack = FFTablePoker[PokerPosition.BB].FFBet + FFTablePoker[PokerPosition.BB].FFStack;

                double equivalentBBCall1 = Math.Max(stackOpener, myStack);
                equivalentBBCall1 = Math.Max(equivalentBBCall1, stackCaller2);
                byte finalEquivalentBBCall1 = CPokerGame.ConvertToBB(CGGPokerConsantes.GG_LIMIT, equivalentBBCall1);

                double equivalentBBCall2 = Math.Max(stackOpener, myStack);
                equivalentBBCall2 = Math.Max(equivalentBBCall2, stackCaller1);
                byte finalEquivalentBBCall2 = CPokerGame.ConvertToBB(CGGPokerConsantes.GG_LIMIT, equivalentBBCall2);

                theDictionnary.Add(FromMyPokerPosToCAllInPokerPos(getPokerPosByCardPos(FFcardPosDE)), finalEquivalentBBCall1);
                theDictionnary.Add(FromMyPokerPosToCAllInPokerPos(getPokerPosByCardPos(FFcardPosSB)), finalEquivalentBBCall2);
            }
            else
            {
                ScreenPosition callerCardPos;
                if (isPlayerAllIn(FFcardPosSB))
                {
                    callerCardPos = FFcardPosSB;
                }
                else
                {
                    callerCardPos = FFcardPosDE;
                }
                double callerStack = FFTable[callerCardPos].PBet;
                double stackOpener = FFTablePoker[openShoverPokerPos].FFBet;
                double biggestNextStack = getBiggestStackNextPlayers(getPokerPosByCardPos(callerCardPos));
                biggestNextStack = Math.Max(biggestNextStack, stackOpener);

                double equivalentBBStack = Math.Min(biggestNextStack, callerStack);
                byte bEquivalentBBStack = CPokerGame.ConvertToBB(CGGPokerConsantes.GG_LIMIT, equivalentBBStack);

                theDictionnary.Add(FromMyPokerPosToCAllInPokerPos(getPokerPosByCardPos(callerCardPos)), bEquivalentBBStack);

            }

            CLogger.AddLog("-- BB Equivalent Stack of callers --");
            if (theDictionnary.ContainsKey(CAllInPokerGame.Position.SB))
                CLogger.AddLog("SMALL BLIND: " + theDictionnary[CAllInPokerGame.Position.SB]);
            if (theDictionnary.ContainsKey(CAllInPokerGame.Position.DEALER))
                CLogger.AddLog("DEALER: " + theDictionnary[CAllInPokerGame.Position.DEALER]);

            return theDictionnary;
        }

        public byte getBBEquivalentShove()
        {
            if (!someoneOpenShoved())
                throw new Exception("there is no open shovers");

            PokerPosition thePokerPosOfOpener = FromCAllPokerPosToMyPokerPos(getPositionOfOpenShover());

            double stack = getBiggestStackNextPlayers(thePokerPosOfOpener);

            stack = Math.Min(stack, getStackFromPokerPos(thePokerPosOfOpener));

            byte BBEquivalentShove = CPokerGame.ConvertToBB(CGGPokerConsantes.GG_LIMIT, stack);
            CLogger.AddLog("The BB Equivalent Shove: " + Convert.ToString(BBEquivalentShove));

            return BBEquivalentShove;
        }

        public CAllInPokerGame.Position getMyPosition()
        {
            switch (myPokerPosition())
            {
                case PokerPosition.CO:
                    CLogger.AddLog("Our position is: CO");
                    return CAllInPokerGame.Position.CO;
                case PokerPosition.dealer:
                    CLogger.AddLog("Our position is: DEALER");
                    return CAllInPokerGame.Position.DEALER;
                case PokerPosition.SB:
                    CLogger.AddLog("Our position is: SMALL BLIND");
                    return CAllInPokerGame.Position.SB;
                case PokerPosition.BB:
                    CLogger.AddLog("Our position is: BIG BLIND");
                    return CAllInPokerGame.Position.BB;
                default:
                    throw new Exception("This situation should not happen!");
            }
        }

        public CAllInPokerGame.Position getPositionOfOpenShover()
        {
            if (!someoneOpenShoved())
                throw new Exception("No one did open Shove");
            if (FFPlayingPlayerCount == 4)
            {
                if (isPlayerAllIn(FFcardPosCO))
                {
                    CLogger.AddLog("Position of open-shover: CO");
                    return CAllInPokerGame.Position.CO;
                }                    
            }
            if (FFPlayingPlayerCount >= 3)
            {
                if (isPlayerAllIn(FFcardPosDE))
                {
                    CLogger.AddLog("Position of open-shover: DEALER");
                    return CAllInPokerGame.Position.DEALER;
                }                    
            }
            if (FFPlayingPlayerCount >= 2)
            {
                if (isPlayerAllIn(FFcardPosSB))
                {
                    CLogger.AddLog("Position of open-shover: SMALL BLIND");
                    return CAllInPokerGame.Position.SB;
                }                    
            }
            throw new Exception("there is a problem with Players Detection");
        }

        public byte getNumberOfCallers()
        {
            int count = 0;

            if (FFTable[ScreenPosition.Est].IsAllIn())
                ++count;
            if (FFTable[ScreenPosition.North].IsAllIn())
                ++count;
            if (FFTable[ScreenPosition.West].IsAllIn())
                ++count;

            byte numberOfCallers = (byte)(count - 1);
            CLogger.AddLog("The number of callers is: " + Convert.ToString(numberOfCallers));

            return numberOfCallers;
        }

        public byte getOurBBEquivalentShove()
        {
            //TODO: change converToBB025.
            double ourStack = FFTable[ScreenPosition.South].PBet + FFTable[ScreenPosition.South].PStack;
            double theStackToUse = 0;
            ScreenPosition ourPos = ScreenPosition.South;

            theStackToUse = getBiggestStackNextPlayers(getPokerPosByCardPos(ourPos));
            theStackToUse = Math.Min(ourStack, theStackToUse);

            byte ourBBEquivalentShove = CPokerGame.ConvertToBB(CGGPokerConsantes.GG_LIMIT, theStackToUse);
            CLogger.AddLog("OUR BB Equivalent shove: " + Convert.ToString(ourBBEquivalentShove));

            return ourBBEquivalentShove;
        }

        public bool isOurHandInThisRange(string _range)
        {
            CLogger.AddLog("Is our hand in this range? " + _range);
            string[] theTable = CPokerRangeConverter.ConvertRange(_range);
            string hand1 = FFHandReader.FFActualHand[0].ToString() + FFHandReader.FFActualHand[1];
            string hand2 = FFHandReader.FFActualHand[1].ToString() + FFHandReader.FFActualHand[0];

            foreach(string theComboInTheTableFromTheRangeConverter in theTable)
            {
                if (theComboInTheTableFromTheRangeConverter == hand1 || theComboInTheTableFromTheRangeConverter == hand2)
                {
                    CLogger.AddLog("True");
                    return true;
                }                    
            }

            CLogger.AddLog("False");
            return false;
        }

        public bool someoneCalledAnOpenShoved()
        {
            int count = 0;

            if (FFTable[ScreenPosition.Est].IsAllIn())
                ++count;
            if (FFTable[ScreenPosition.North].IsAllIn())
                ++count;
            if (FFTable[ScreenPosition.West].IsAllIn())
                ++count;

            bool someoneCalledAnOpenShoved = (count > 1);
            CLogger.AddLog("Someone called an open shoved? " + someoneCalledAnOpenShoved.ToString());

            return someoneCalledAnOpenShoved;
        }

        public bool someoneOpenShoved()
        {
            bool someoneOpenShoved = FFTable[ScreenPosition.West].IsAllIn() || FFTable[ScreenPosition.North].IsAllIn() || FFTable[ScreenPosition.Est].IsAllIn();

            CLogger.AddLog("Someone open shoved? " + someoneOpenShoved.ToString());
            return someoneOpenShoved;
        }

        /// <summary>
        /// Returns the logs of the table information. Useful because before a bot has to make a decision,
        /// the table has to know when to call the bot to make a decision. Because of this, it will
        /// happen often that we will need to debug detection methods and therefore, we need the
        /// logs from the table.
        /// </summary>
        /// <returns>Returns the logs of what happened during a hand.</returns>
        public List<string> GetDetectionLogsForCurrentHand()
        {
            string[] arrayLogs = new string[FFLstLogs.Count];

            FFLstLogs.CopyTo(arrayLogs);
            FFLstLogs.Clear();

            return arrayLogs.ToList();
        }

        public PokerPosition FromCAllPokerPosToMyPokerPos(CAllInPokerGame.Position _pos)
        {
            switch (_pos) {
                case CAllInPokerGame.Position.BB:
                    return PokerPosition.BB;
                case CAllInPokerGame.Position.SB:
                    return PokerPosition.SB;
                case CAllInPokerGame.Position.DEALER:
                    return PokerPosition.dealer;
                case CAllInPokerGame.Position.CO:
                    return PokerPosition.CO;
                default:
                    throw new Exception("Bad position");
            }
        }

        public CAllInPokerGame.Position FromMyPokerPosToCAllInPokerPos(PokerPosition _pos)
        {
            switch (_pos)
            {
                case PokerPosition.BB:
                    return CAllInPokerGame.Position.BB;
                case PokerPosition.SB:
                    return CAllInPokerGame.Position.SB;
                case PokerPosition.dealer:
                    return CAllInPokerGame.Position.DEALER;
                case PokerPosition.CO:
                    return CAllInPokerGame.Position.CO;
                default:
                    throw new Exception("Bad position");
            }
        }
    }
}
