using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VersionOfficielle.Interfaces;
using static VersionOfficielle.CAllInPokerGame;

namespace VersionOfficielle
{
    class CBot
    {
        private int FFHandNumber;

        public CBot()
        {
            FFHandNumber = 0;
        }

        /// <summary>
        /// Ask to the bot if we should go all in or not.
        /// </summary>
        /// <param name="_table">The informations of the table</param>
        /// <exception cref="Exception">Will occur if an unknown error occured. In that case, no decision will be made.</exception>
        /// <returns>Returns true if we should go all in or false if we shouldn't.</returns>
        public bool getDecision(ITableInformations _table)
        {            
            List<string> lstMessages = new List<string>();
            List<string> lstDetectionLogs = _table.GetDetectionLogsForCurrentHand();

            ++FFHandNumber;
            CLogger.AddLog("--- NEW HAND --- HAND NUMBER " + FFHandNumber.ToString());

            for (int currentLogIndex = 0; currentLogIndex < lstDetectionLogs.Count; ++currentLogIndex)
                CLogger.AddLog(lstDetectionLogs[currentLogIndex]);           

            Position ourPosition = _table.getMyPosition();          

            switch (ourPosition)
            {
                case Position.CO:
                    #region We're first to act. We're from CO.   
                    CLogger.AddLog("We're first to act. We're from CO.");
                    byte ourBBStack = _table.getOurBBEquivalentShove();

                    // Open shove range here.
                    switch (ourBBStack)
                    {
                        case 8:
                            return _table.isOurHandInThisRange("22+ A2o+ A2s+ K8s+ KTo+ Q9s+ QJo J9s+ T8s+ 98s 78s");
                        case 9:
                            return _table.isOurHandInThisRange("22+ A2s+ A3o+ K9s+ KTo+ Q9s+ QJo J8s+ JTo T8s+ 98s 78s");                            
                        case 10:
                            return _table.isOurHandInThisRange("22+ A2s+ A7o+ A5o K7s+ KTo+ Q8s+ QTo+ J8s+ JTo T8s+ 98s 78s");                            
                        case 11:
                            return _table.isOurHandInThisRange("22+ A2s+ A8o+ K8s+ KTo+ Q8s+ QJo J8s+ JTo T8s+ 98s 78s");
                        case 12:
                            return _table.isOurHandInThisRange("22+ A2s+ A9o+ K9s+ KTo+ Q9s+ QJo J8s+ JTo T8s+ 98s");
                        case 13:
                            return _table.isOurHandInThisRange("22+ A7s+ A5s A4s A3s A9o+ K8s+ KJo+ Q9s+ QJo J9s+ T8s+ 98s");
                        case 14:
                            return _table.isOurHandInThisRange("22+ A7s+ A5s A4s A3s ATo+ K8s+ KJo+ Q8s+ QJo J8s+ T8s+ 98s");
                        case 15:
                            return _table.isOurHandInThisRange("22+ A7s+ A5s A4s ATo+ K8s+ KJo+ Q9s+ QJo J9s+ T9s 98s");
                        case 16:
                            return _table.isOurHandInThisRange("22+ A8s+ A5s A4s ATo+ K9s+ KJo+ Q9s+ QJo J9s+ T9s");
                        case 17:
                            return _table.isOurHandInThisRange("22+ A8s+ A5s A4s ATo+ K9s+ KJo+ Q9s+ J9s+ T9s");
                        case 18:
                            return _table.isOurHandInThisRange("22+ A8s+ A5s ATo+ K9s+ KQo Q9s+ J9s+ T9s");
                        default:
                            if (ourBBStack > 18)
                            {
                                return _table.isOurHandInThisRange("QQ+ AKo AKs+");
                            }
                            else
                                throw new Exception("The stack is below 8BB!");                            
                    }
                    #endregion                    
                case Position.DEALER:
                    bool someoneOpenShoved = _table.someoneOpenShoved();

                    if (someoneOpenShoved)
                    {
                        #region CO shoves. We're from BTN
                        CLogger.AddLog("CO shoves. We're from BTN.");
                        byte BBStackShover = _table.getBBEquivalentShove();

                        switch (BBStackShover)
                        {
                            case 8:
                                return _table.isOurHandInThisRange("55+ ATo+ A8s+");
                            case 9:
                                return _table.isOurHandInThisRange("55+ ATo+ A8s+");
                            case 10:
                                return _table.isOurHandInThisRange("77+ ATo+ A9s+");
                            case 11:
                                return _table.isOurHandInThisRange("77+ AJo+ ATs+");
                            case 12:
                                return _table.isOurHandInThisRange("77+ AJo+ ATs+");
                            case 13:
                                return _table.isOurHandInThisRange("77+ AJo+ ATs+");
                            case 14:
                                return _table.isOurHandInThisRange("77+ AQo+ AJs+");
                            case 15:
                                return _table.isOurHandInThisRange("77+ AQo+ AJs+");
                            case 16:
                                return _table.isOurHandInThisRange("77+ AQo+ AJs+");
                            case 17:
                                return _table.isOurHandInThisRange("88+ AQo+ AJs+");
                            case 18:
                                return _table.isOurHandInThisRange("88+ AQo+ AQs+");
                            default:
                                if (BBStackShover > 18)
                                {
                                    // call a shove that is > 18bb
                                    return _table.isOurHandInThisRange("QQ+ AKs+ AKo+");
                                }
                                else
                                {
                                    // call a shove that is < 8BB
                                    return _table.isOurHandInThisRange("44+ A8o+ A5s+");                                    
                                }                                
                        }
                        #endregion
                    }
                    else
                    {
                        #region Everyone folds. We're from BTN.
                        CLogger.AddLog("Everyone folds. We're from BTN");
                        byte ourBBEquivalentStack = _table.getOurBBEquivalentShove();

                        switch (ourBBEquivalentStack)
                        {
                            case 8:
                                return _table.isOurHandInThisRange("22+ A2o+ A2s+ K3s+ K9o+ Q8s+ QTo+ J7s+ JTo T7s+ T9o 97s+ 86s+ 76s 65s");
                            case 9:
                                return _table.isOurHandInThisRange("22+ A2o+ A2s+ K5s+ K9o+ Q8s+ QTo+ J8s+ JTo T7s+ 97s+ 86s+ 76s");
                            case 10:
                                return _table.isOurHandInThisRange("22+ A2o+ A2s+ K6s+ KTo+ Q8s+ QTo+ J8s+ JTo T8s+ 97s+ 87s 76s");
                            case 11:
                                return _table.isOurHandInThisRange("22+ A2o+ A2s+ K6s+ KTo+ Q8s+ QTo+ J8s+ JTo T8s+ 97s+ 87s");
                            case 12:
                                return _table.isOurHandInThisRange("22+ A2o+ A2s+ K6s+ KTo+ Q8s+ QTo+ J8s+ JTo T8s+ 97s+ 87s 76s");
                            case 13:
                                return _table.isOurHandInThisRange("22+ A2o+ A2s+ K7s+ KTo+ Q8s+ QTo+ J8s+ JTo T8s+ 97s+ 87s 76s");
                            case 14:
                                return _table.isOurHandInThisRange("22+ A2s+ A3o+ K6s+ KTo+ Q8s+ QTo+ J8s+ JTo T8s+ 97s+ 87s");
                            case 15:
                                return _table.isOurHandInThisRange("22+ A2s+ A5o+ K6s+ KTo+ Q8s+ QTo+ J8s+ JTo T7s+ 97s+ 87s 76s");
                            case 16:
                                return _table.isOurHandInThisRange("22+ A2s+ A7o+ A5o A4o K7s+ KTo+ Q8s+ QTo+ J8s+ JTo T8s+ 98s 87s");
                            case 17:
                                return _table.isOurHandInThisRange("22+ A2s+ A8o+ K7s+ KTo+ Q8s+ QTo+ J8s+ JTo T8s+ 98s");
                            case 18:
                                return _table.isOurHandInThisRange("22+ A2s+ A8o+ K7s+ KTo+ Q8s+ QTo+ J8s+ JTo T8s+ 98s");
                            default:
                                if (ourBBEquivalentStack > 18)
                                {
                                    return _table.isOurHandInThisRange("QQ+ AKs AKo+");                                    
                                }
                                else
                                    throw new Exception("The stack cannot be below 8BB!");                                
                        }
                        #endregion
                    }                    
                case Position.SB:
                    bool someoneOpenShoved2 = _table.someoneOpenShoved();

                    if (someoneOpenShoved2)
                    {
                        #region Someone shoved. We're from SB.
                        CLogger.AddLog("Someone shoved. We're from SB.");
                        bool someoneCalledAnOpenShoved = _table.someoneCalledAnOpenShoved();
                        // If someone call, it means implicitely that the CO shoved and the BTN called.
                        if (someoneCalledAnOpenShoved)
                        {
                            #region CO shoves. BTN called. We're from SB.
                            CLogger.AddLog("CO shoves. BTN called. We're from SB.");
                            byte BBStackShover = _table.getBBEquivalentShove();
                            Dictionary<Position, byte> callersBBStack = _table.getBBEquivalentOfCallers();

                            if (callersBBStack[Position.DEALER] > BBStackShover)
                            {
                                #region If the BTN has a higher stack than the open-shover and ours is also higher than the open-shover.
                                CLogger.AddLog("The BTN has a higher stack than the open-shover. Our stack is also higher than the open-shover.");

                                switch (callersBBStack[Position.DEALER])
                                {
                                    case 8:
                                        return _table.isOurHandInThisRange("77+ ATs+ AJo+");
                                    case 9:
                                        return _table.isOurHandInThisRange("77+ ATs+ AJo+");
                                    case 10:
                                        return _table.isOurHandInThisRange("88+ ATs+ AJo+");
                                    case 11:
                                        return _table.isOurHandInThisRange("88+ ATs+ AJo+");
                                    case 12:
                                        return _table.isOurHandInThisRange("88+ ATs+ AJo+");
                                    case 13:
                                        return _table.isOurHandInThisRange("88+ ATs+ AJo+");
                                    case 14:
                                        return _table.isOurHandInThisRange("TT+ AJs+ AQo+");
                                    case 15:
                                        return _table.isOurHandInThisRange("TT+ AJs+ AQo+");
                                    case 16:
                                        return _table.isOurHandInThisRange("TT+ AJs+ AQo+");
                                    case 17:
                                        return _table.isOurHandInThisRange("JJ+ AQs+ AQo+");
                                    case 18:
                                        return _table.isOurHandInThisRange("JJ+ AQs+ AQo+");
                                    default:
                                        if (callersBBStack[Position.DEALER] > 18)
                                        {
                                            // Call two shoves that is > 18BB
                                            return _table.isOurHandInThisRange("KK+ AKs+ AKo+");                                            
                                        }
                                        else
                                        {
                                            // Call two shoves that is < 8BB
                                            return _table.isOurHandInThisRange("66+ A8s+ A9o+");                                            
                                        }                                        
                                }
                                #endregion
                            }
                            else
                            {
                                #region If the CO has the higher stack or equal to everyone else.
                                CLogger.AddLog("The open-shover has the higher stack or equal to everyone else.");

                                switch (BBStackShover)
                                {
                                    case 8:
                                        return _table.isOurHandInThisRange("77+ ATs+ AJo+");
                                    case 9:
                                        return _table.isOurHandInThisRange("77+ ATs+ AJo+");
                                    case 10:
                                        return _table.isOurHandInThisRange("88+ ATs+ AJo+");
                                    case 11:
                                        return _table.isOurHandInThisRange("88+ ATs+ AJo+");
                                    case 12:
                                        return _table.isOurHandInThisRange("88+ ATs+ AJo+");
                                    case 13:
                                        return _table.isOurHandInThisRange("88+ ATs+ AJo+");
                                    case 14:
                                        return _table.isOurHandInThisRange("TT+ AJs+ AQo+");
                                    case 15:
                                        return _table.isOurHandInThisRange("TT+ AJs+ AQo+");
                                    case 16:
                                        return _table.isOurHandInThisRange("TT+ AJs+ AQo+");
                                    case 17:
                                        return _table.isOurHandInThisRange("JJ+ AQs+ AQo+");
                                    case 18:
                                        return _table.isOurHandInThisRange("JJ+ AQs+ AQo+");
                                    default:
                                        if (callersBBStack[Position.DEALER] > 18)
                                            // Call two shoves that is > 18BB
                                            return _table.isOurHandInThisRange("KK+ AKs+ AKo+");
                                        
                                        else
                                            // Call two shoves that is < 8BB
                                            return _table.isOurHandInThisRange("66+ A8s+ A9o+");
                                        
                                }
                                #endregion
                            }
                            #endregion
                        }
                        else
                        {
                            Position positionOpenShover = _table.getPositionOfOpenShover();

                            if (positionOpenShover == Position.CO)
                            {
                                #region CO shoves. BTN folds. We're from SB.
                                CLogger.AddLog("CO shoves. BTN folds. We're from SB.");
                                byte COBBStackShover = _table.getBBEquivalentShove();

                                switch (COBBStackShover)
                                {
                                    case 8:
                                        return _table.isOurHandInThisRange("66+ KQs+ A7o+ A3s+");
                                    case 9:
                                        return _table.isOurHandInThisRange("66+ KQs+ A8o+ A5s+");
                                    case 10:
                                        return _table.isOurHandInThisRange("66+ KQs+ A8o+ A7s+");
                                    case 11:
                                        return _table.isOurHandInThisRange("66+ A9o+ A8s+");
                                    case 12:
                                        return _table.isOurHandInThisRange("66+ A9o+ A8s+");
                                    case 13:
                                        return _table.isOurHandInThisRange("66+ ATo+ A8s+");
                                    case 14:
                                        return _table.isOurHandInThisRange("66+ ATo+ A9s+");
                                    case 15:
                                        return _table.isOurHandInThisRange("77+ ATo+ A9s+");
                                    case 16:
                                        return _table.isOurHandInThisRange("77+ AJo+ ATs+");
                                    case 17:
                                        return _table.isOurHandInThisRange("77+ AJo+ ATs+");
                                    case 18:
                                        return _table.isOurHandInThisRange("88+ AJo+ ATs+");
                                    default:
                                        if (COBBStackShover > 18)
                                        {
                                            // call a shove that is > 18B
                                            return _table.isOurHandInThisRange("QQ+ AKs+ AKo+");                                            
                                        }
                                        else
                                        {
                                            // call a shove that is < 8BB
                                            return _table.isOurHandInThisRange("44+ KTs+ A5o+ A2s+");                                            
                                        }                                        
                                }
                                #endregion
                            }
                            else if (positionOpenShover == Position.DEALER)
                            {
                                #region CO folds. BTN shoves. We're from SB.
                                CLogger.AddLog("CO folds. BTN shoves. We're from SB.");
                                byte BTNBBStackShover = _table.getBBEquivalentShove();

                                switch (BTNBBStackShover)
                                {
                                    case 8:
                                        return _table.isOurHandInThisRange("44+ KQo+ KJs+ A5o+ A2s+ QJs");
                                    case 9:
                                        return _table.isOurHandInThisRange("44+ KJs+ A7o+ A2s+");
                                    case 10:
                                        return _table.isOurHandInThisRange("44+ KQs+ A7o+ A3s+");
                                    case 11:
                                        return _table.isOurHandInThisRange("44+ KQs+ A7o+ A4s+");
                                    case 12:
                                        return _table.isOurHandInThisRange("55+ KQs+ A8o+ A5s+");
                                    case 13:
                                        return _table.isOurHandInThisRange("55+ KQs+ A8o+ A5s+");
                                    case 14:
                                        return _table.isOurHandInThisRange("55+ A8o+ A7s+");
                                    case 15:
                                        return _table.isOurHandInThisRange("55+ A9o+ A7s+");
                                    case 16:
                                        return _table.isOurHandInThisRange("66+ A9o+ A8s+");
                                    case 17:
                                        return _table.isOurHandInThisRange("66+ A9o+ A8s+");
                                    case 18:
                                        return _table.isOurHandInThisRange("66+ ATo+ A8s+");
                                    default:
                                        if (BTNBBStackShover > 18)
                                        {
                                            // call a shove that is > 18BB
                                            return _table.isOurHandInThisRange("QQ+ AKs+ AKo+");                                            
                                        }
                                        else
                                        {
                                            // call a shove that is < 8BB
                                            return _table.isOurHandInThisRange("22+ KTo+ K9s+ A2o+ A2s+ QJs");                                            
                                        }                                        
                                }
                                #endregion
                            }
                            else
                                throw new Exception("This situation is impossible! Something wrong happened!");
                        }
                        #endregion
                    }
                    else
                    {
                        #region Everyone folds. We're from SB.
                        CLogger.AddLog("Everyone folds. We're from SB.");
                        byte ourBBEquivalentStack = _table.getOurBBEquivalentShove();

                        switch (ourBBEquivalentStack)
                        {
                            case 8:
                                return _table.isOurHandInThisRange("22+ A2o+ A2s+ K2o+ K2s+ Q2s+ Q3o+ J2s+ J7o+ T4s+ T7o+ 95s+ 97o+ 85s+ 87o 74s+ 64s+ 53s+");
                            case 9:
                                return _table.isOurHandInThisRange("22+ A2o+ A2s+ K2o+ K2s+ Q2s+ Q5o+ J2s+ J8o+ T5s+ T8o+ 95s+ 97o+ 85s+ 87o 74s+ 64s+ 53s+");
                            case 10:
                                return _table.isOurHandInThisRange("22+ A2o+ A2s+ K2o+ K2s+ Q2s+ Q7o+ J3s+ J8o+ T5s+ T8o+ 95s+ 97o+ 85s+ 87o 74s+ 64s+ 53s+");
                            case 11:
                                return _table.isOurHandInThisRange("22+ A2o+ A2s+ K2o+ K2s+ Q2s+ Q8o+ J4s+ J8o+ T5s+ T8o+ 95s+ 98o 85s+ 87o 74s+ 64s+ 53s+");
                            case 12:
                                return _table.isOurHandInThisRange("22+ A2o+ A2s+ K2s+ K3o+ Q2s+ Q8o+ J5s+ J9o+ T6s+ T8o+ 95s+ 98o 85s+ 87o 75s+ 64s+ 54s");
                            case 13:
                                return _table.isOurHandInThisRange("22+ A2o+ A2s+ K2s+ K4o+ Q3s+ Q8o+ J5s+ J9o+ T6s+ T8o+ 96s+ 98o 85s+ 75s+ 64s+ 54s");
                            case 14:
                                return _table.isOurHandInThisRange("22+ A2o+ A2s+ K2s+ K5o+ Q4s+ Q9o+ J5s+ J8o+ T6s+ T8o+ 95s+ 98o 85s+ 87o 75s+ 64s+ 54s");
                            case 15:
                                return _table.isOurHandInThisRange("22+ A2o+ A2s+ K2s+ K6o+ Q4s+ Q9o+ J6s+ J9o+ T6s+ T8o+ 96s+ 98o 85s+ 75s+ 64s+ 54s");
                            case 16:
                                return _table.isOurHandInThisRange("22+ A2o+ A2s+ K2s+ K7o+ Q5s+ Q9o+ J6s+ J9o+ T6s+ T9o 96s+ 98o 85s+ 75s+ 65s 54s");
                            case 17:
                                return _table.isOurHandInThisRange("22+ A2o+ A2s+ K2s+ K8o+ Q5s+ Q9o+ J6s+ J9o+ T6s+ T8o+ 96s+ 98o 85s+ 75s+ 65s 54s");
                            case 18:
                                return _table.isOurHandInThisRange("22+ A2o+ A2s+ K2s+ K8o+ Q5s+ Q9o+ J6s+ J9o+ T6s+ T9o 96s+ 98o 85s+ 75s+ 65s 54s");
                            default:
                                if (ourBBEquivalentStack > 18)
                                {
                                    // Shove with > 18BB
                                    return _table.isOurHandInThisRange("QQ+ AKo+ AKs+");                                    
                                }
                                else
                                    throw new Exception("The stack cannot be below 8BB!");                                
                        }
                        #endregion
                    }                    
                case Position.BB:
                    #region Someone shoved. We're from BB.  
                    CLogger.AddLog("Someone shoved. We're from BB.");
                    Position openShoverPosition = _table.getPositionOfOpenShover();

                    switch (openShoverPosition)
                    {
                        #region Calling range
                        case Position.CO:
                            bool someoneCalledAnOpenShoved = _table.someoneCalledAnOpenShoved();

                            if (someoneCalledAnOpenShoved)
                            {
                                byte COBBStackShover = _table.getBBEquivalentShove();
                                byte numberOfCallers = _table.getNumberOfCallers();

                                if (numberOfCallers == 1)
                                {
                                    #region CO shoves. BTN OR SB called. We're from BB.  
                                    CLogger.AddLog("CO shoves. BTN OR SB called. We're from BB.");
                                    Dictionary<Position, byte> callersBBStack = _table.getBBEquivalentOfCallers();

                                    if (callersBBStack.First().Value > COBBStackShover)
                                    {
                                        #region If the BTN OR SB that called has a higher stack and we(us) have a higher stack than the open-shover.
                                        CLogger.AddLog("The BTN OR SB that called has a higher stack and we(us) have a higher stack than the open-shover.");

                                        switch (callersBBStack.First().Value)
                                        {
                                            case 8:
                                                return _table.isOurHandInThisRange("55+ A6s+ A8o+");
                                            case 9:
                                                return _table.isOurHandInThisRange("55+ A6s+ A8o+");
                                            case 10:
                                                return _table.isOurHandInThisRange("66+ A8s+ ATo+");
                                            case 11:
                                                return _table.isOurHandInThisRange("66+ A8s+ ATo+");
                                            case 12:
                                                return _table.isOurHandInThisRange("66+ A8s+ ATo+");
                                            case 13:
                                                return _table.isOurHandInThisRange("66+ A8s+ ATo+");
                                            case 14:
                                                return _table.isOurHandInThisRange("88+ ATs+ AJo+");
                                            case 15:
                                                return _table.isOurHandInThisRange("88+ ATs+ AJo+");
                                            case 16:
                                                return _table.isOurHandInThisRange("88+ ATs+ AJo+");
                                            case 17:
                                                return _table.isOurHandInThisRange("TT+ AJs+ AQo+");
                                            case 18:
                                                return _table.isOurHandInThisRange("TT+ AJs+ AQo+");
                                            default:
                                                if (callersBBStack.First().Value > 18)
                                                {
                                                    // Call two shoves that is > 18BB
                                                    return _table.isOurHandInThisRange("KK+ AKs+ AKo+");
                                                }
                                                else
                                                {
                                                    // Call two shoves that is < 8BB
                                                    return _table.isOurHandInThisRange("44+ A5s+ A7o+");
                                                }
                                        }
                                        #endregion
                                    }
                                    else
                                    {
                                        #region If the BTN OR SB that called has the same or lower stack than the open shover.
                                        CLogger.AddLog("The BTN OR SB that called has the same or lower stack than the open-shover.");

                                        switch (COBBStackShover)
                                        {
                                            case 8:
                                                return _table.isOurHandInThisRange("55+ A6s+ A8o+");
                                            case 9:
                                                return _table.isOurHandInThisRange("55+ A6s+ A8o+");
                                            case 10:
                                                return _table.isOurHandInThisRange("66+ A8s+ ATo+");
                                            case 11:
                                                return _table.isOurHandInThisRange("66+ A8s+ ATo+");
                                            case 12:
                                                return _table.isOurHandInThisRange("66+ A8s+ ATo+");
                                            case 13:
                                                return _table.isOurHandInThisRange("66+ A8s+ ATo+");
                                            case 14:
                                                return _table.isOurHandInThisRange("88+ ATs+ AJo+");
                                            case 15:
                                                return _table.isOurHandInThisRange("88+ ATs+ AJo+");
                                            case 16:
                                                return _table.isOurHandInThisRange("88+ ATs+ AJo+");
                                            case 17:
                                                return _table.isOurHandInThisRange("TT+ AJs+ AQo+");
                                            case 18:
                                                return _table.isOurHandInThisRange("TT+ AJs+ AQo+");
                                            default:
                                                if (callersBBStack[Position.DEALER] > 18)
                                                {
                                                    // Call two shoves that is > 18BB
                                                    return _table.isOurHandInThisRange("KK+ AKs+ AKo+");
                                                }
                                                else
                                                {
                                                    // Call two shoves that is < 8BB
                                                    return _table.isOurHandInThisRange("44+ A5s+ A7o+");
                                                }
                                        }
                                        #endregion
                                    }
                                    #endregion
                                }
                                else if (numberOfCallers == 2)
                                {
                                    #region CO shoves. BTN AND SB called. We're from BB.  
                                    CLogger.AddLog("CO shoves. BTN AND SB called. We're from BB.");
                                    Dictionary<Position, byte> callersBBStack = _table.getBBEquivalentOfCallers();
                                    byte highestBBStackFromCallers = Math.Max(callersBBStack[Position.SB], callersBBStack[Position.DEALER]);

                                    if (highestBBStackFromCallers > COBBStackShover)
                                    {
                                        #region If BTN or SB has a higher stack than the open-shover and we have a higher stack (or the other position that called) than the open-shover too.
                                        CLogger.AddLog("The BTN or SB has a higher stack than the open-shover and we have a higher stack (or the other position that called) than the open-shover too.");

                                        switch (highestBBStackFromCallers)
                                        {
                                            case 8:
                                                return _table.isOurHandInThisRange("JJ+ AKs+ AKo+");
                                            case 9:
                                                return _table.isOurHandInThisRange("JJ+ AKs+ AKo+");
                                            case 10:
                                                return _table.isOurHandInThisRange("JJ+ AKs+ AKo+");
                                            case 11:
                                                return _table.isOurHandInThisRange("JJ+ AKs+ AKo+");
                                            case 12:
                                                return _table.isOurHandInThisRange("QQ+ AKs+ AKo+");
                                            case 13:
                                                return _table.isOurHandInThisRange("QQ+ AKs+ AKo+");
                                            case 14:
                                                return _table.isOurHandInThisRange("QQ+ AKs+ AKo+");
                                            case 15:
                                                return _table.isOurHandInThisRange("KK+");
                                            case 16:
                                                return _table.isOurHandInThisRange("KK+");
                                            case 17:
                                                return _table.isOurHandInThisRange("KK+");
                                            case 18:
                                                return _table.isOurHandInThisRange("KK+");
                                            default:
                                                if (COBBStackShover > 18)
                                                {
                                                    // Call three shoves that is > 18BB
                                                    return _table.isOurHandInThisRange("KK+");
                                                }
                                                else
                                                {
                                                    // Call three shoves that is < 8BB
                                                    return _table.isOurHandInThisRange("TT+ AQs+ AQo+");
                                                }
                                        }
                                        #endregion
                                    }
                                    else
                                    {
                                        #region If BTN and SB has the same or lower stack than the open-shover.
                                        CLogger.AddLog("The BTN and SB has the same or lower stack than the open-shover.");

                                        switch (COBBStackShover)
                                        {
                                            case 8:
                                                return _table.isOurHandInThisRange("JJ+ AKs+ AKo+");
                                            case 9:
                                                return _table.isOurHandInThisRange("JJ+ AKs+ AKo+");
                                            case 10:
                                                return _table.isOurHandInThisRange("JJ+ AKs+ AKo+");
                                            case 11:
                                                return _table.isOurHandInThisRange("JJ+ AKs+ AKo+");
                                            case 12:
                                                return _table.isOurHandInThisRange("QQ+ AKs+ AKo+");
                                            case 13:
                                                return _table.isOurHandInThisRange("QQ+ AKs+ AKo+");
                                            case 14:
                                                return _table.isOurHandInThisRange("QQ+ AKs+ AKo+");
                                            case 15:
                                                return _table.isOurHandInThisRange("KK+");
                                            case 16:
                                                return _table.isOurHandInThisRange("KK+");
                                            case 17:
                                                return _table.isOurHandInThisRange("KK+");
                                            case 18:
                                                return _table.isOurHandInThisRange("KK+");
                                            default:
                                                if (COBBStackShover > 18)
                                                {
                                                    // Call three shoves that is > 18BB
                                                    return _table.isOurHandInThisRange("KK+");
                                                }
                                                else
                                                {
                                                    // Call three shoves that is < 8BB
                                                    return _table.isOurHandInThisRange("TT+ AQs+ AQo+");
                                                }                                                
                                        }
                                        #endregion
                                    }
                                    #endregion
                                }
                                else
                                    throw new Exception("The number of callers can only be 1 or 2!");
                            }
                            else
                            {
                                #region CO shoves. BTN folds. SB folds. We're from BB.
                                CLogger.AddLog("CO shoves. BTN folds. SB folds. We're from BB.");
                                byte COBBStackShover = _table.getBBEquivalentShove();

                                switch (COBBStackShover)
                                {
                                    case 8:
                                        return _table.isOurHandInThisRange("33+ KQo+ KJs+ A4o+ A2s+ QJs");
                                    case 9:
                                        return _table.isOurHandInThisRange("33+ KJs+ A6o+ A2s+ QJs");
                                    case 10:
                                        return _table.isOurHandInThisRange("44+ KQs+ A7o+ A2s+");
                                    case 11:
                                        return _table.isOurHandInThisRange("44+ KQs+ A8o+ A4s+");
                                    case 12:
                                        return _table.isOurHandInThisRange("55+ KQs+ A5s A7s+");
                                    case 13:
                                        return _table.isOurHandInThisRange("55+ KQs+ A5s A7s+");
                                    case 14:
                                        return _table.isOurHandInThisRange("55+ ATo+ A8s+");
                                    case 15:
                                        return _table.isOurHandInThisRange("66+ ATo+ A9s+");
                                    case 16:
                                        return _table.isOurHandInThisRange("77+ ATo+ A9s+");
                                    case 17:
                                        return _table.isOurHandInThisRange("77+ ATo+ ATs+");
                                    case 18:
                                        return _table.isOurHandInThisRange("77+ AJo+ ATs+");
                                    default:
                                        if (COBBStackShover > 18)
                                        {
                                            // Call a shove that is > 18BB
                                            return _table.isOurHandInThisRange("QQ+ AKs+ AKo+");                                            
                                        }
                                        else
                                        {
                                            // Call a shove that is < 8BB
                                            return _table.isOurHandInThisRange("22+ KTo+ K9s+ A2o+ A2s+ QJs");                                            
                                        }                                        
                                }
                                #endregion
                            }                            
                        case Position.DEALER:
                            bool someoneCalledAnOpenShoved2 = _table.someoneCalledAnOpenShoved();

                            if (someoneCalledAnOpenShoved2)
                            {
                                #region CO folds. BTN shoves. SB called. We're from BB.      
                                CLogger.AddLog("CO folds. BTN shoves. SB called. We're from BB");
                                byte BTNBBStackShover = _table.getBBEquivalentShove();
                                Dictionary<Position, byte> callersBBStack = _table.getBBEquivalentOfCallers();
                                byte highestBBStackFromCaller = callersBBStack[Position.SB];

                                if (highestBBStackFromCaller > BTNBBStackShover)
                                {
                                    #region If SB and we(us) has a higher stack.
                                    CLogger.AddLog("SB and we(us) has a higher stack.");

                                    switch (highestBBStackFromCaller)
                                    {
                                        case 8:
                                            return _table.isOurHandInThisRange("55+ A6s+ A8o+");
                                        case 9:
                                            return _table.isOurHandInThisRange("55+ A6s+ A8o+");
                                        case 10:
                                            return _table.isOurHandInThisRange("66+ A6s+ A8o+");
                                        case 11:
                                            return _table.isOurHandInThisRange("66+ A7s+ A9o+");
                                        case 12:
                                            return _table.isOurHandInThisRange("66+ A7s+ A9o+");
                                        case 13:
                                            return _table.isOurHandInThisRange("77+ A8s+ ATo+");
                                        case 14:
                                            return _table.isOurHandInThisRange("88+ A8s+ ATo+");
                                        case 15:
                                            return _table.isOurHandInThisRange("99+ ATs+ AJo+");
                                        case 16:
                                            return _table.isOurHandInThisRange("99+ ATs+ AJo+");
                                        case 17:
                                            return _table.isOurHandInThisRange("TT+ AJs+ AQo+");
                                        case 18:
                                            return _table.isOurHandInThisRange("TT+ AQs+ AQo+");
                                        default:
                                            if (highestBBStackFromCaller > 18)
                                            {
                                                // Call a shove that is > 18BB
                                                return _table.isOurHandInThisRange("KK+ AKs+ AKo+");                                                
                                            }
                                            else
                                            {
                                                // Call a shove that is < 8BB
                                                return _table.isOurHandInThisRange("55+ A6s+ A8o+");                                                
                                            }                                            
                                    }
                                    #endregion
                                }
                                else
                                {
                                    #region If SB has the same stack of the open-shover.
                                    CLogger.AddLog("SB has the same stack of the open-shover.");

                                    switch (BTNBBStackShover)
                                    {
                                        case 8:
                                            return _table.isOurHandInThisRange("55+ A8s+ ATo+");
                                        case 9:
                                            return _table.isOurHandInThisRange("55+ A8s+ ATo+");
                                        case 10:
                                            return _table.isOurHandInThisRange("66+ A8s+ ATo+");
                                        case 11:
                                            return _table.isOurHandInThisRange("66+ A9s+ AJo+");
                                        case 12:
                                            return _table.isOurHandInThisRange("66+ A9s+ AJo+");
                                        case 13:
                                            return _table.isOurHandInThisRange("77+ ATs+ AQo+");
                                        case 14:
                                            return _table.isOurHandInThisRange("88+ ATs+ AQo+");
                                        case 15:
                                            return _table.isOurHandInThisRange("99+ AQs+ AQo+");
                                        case 16:
                                            return _table.isOurHandInThisRange("99+ AQs+ AQo+");
                                        case 17:
                                            return _table.isOurHandInThisRange("JJ+ AKs+ AKo+");
                                        case 18:
                                            return _table.isOurHandInThisRange("JJ+ AKs+ AKo+");
                                        default:
                                            if (highestBBStackFromCaller > 18)
                                            {
                                                // Call a shove that is > 18BB
                                                return _table.isOurHandInThisRange("KK+ AKs+ AKo+");
                                            }
                                            else
                                            {
                                                // Call a shove that is < 8BB
                                                return _table.isOurHandInThisRange("55+ A8s+ ATo+");
                                            }
                                    }
                                    #endregion
                                }
                                #endregion
                            }
                            else
                            {
                                #region CO folds. BTN shoves. SB folds. We're from BB.
                                CLogger.AddLog("CO folds. BTN shoves. SB folds. We're from BB.");
                                byte BTNBBStackShover = _table.getBBEquivalentShove();

                                switch (BTNBBStackShover)
                                {
                                    case 8:
                                        return _table.isOurHandInThisRange("22+ KJo+ KTs+ A2o+ A2s+ QJs");
                                    case 9:
                                        return _table.isOurHandInThisRange("22+ KJo+ KTs+ A2o+ A2s+");
                                    case 10:
                                        return _table.isOurHandInThisRange("22+ KQo+ KQs+ A5o+ A2s+");
                                    case 11:
                                        return _table.isOurHandInThisRange("22+ KQo+ KQs+ A6o+ A2s+");
                                    case 12:
                                        return _table.isOurHandInThisRange("33+ KQs+ A6o+ A2s+");
                                    case 13:
                                        return _table.isOurHandInThisRange("44+ KQs+ A8o+ A3s+");
                                    case 14:
                                        return _table.isOurHandInThisRange("44+ KQs+ A9o+ A5s+");
                                    case 15:
                                        return _table.isOurHandInThisRange("55+ KQs+ A9o+ A7s+");
                                    case 16:
                                        return _table.isOurHandInThisRange("55+ KQs+ A9o+ A8s+");
                                    case 17:
                                        return _table.isOurHandInThisRange("55+ KQs+ ATo+ A8s+");
                                    case 18:
                                        return _table.isOurHandInThisRange("55+ ATo+ A9s+");
                                    default:
                                        if (BTNBBStackShover > 18)
                                        {
                                            // Call a shove that is > 18BB
                                            return _table.isOurHandInThisRange("QQ+ AKs+ AKo+");                                            
                                        }
                                        else
                                        {
                                            // Call shove that is < 8BB
                                            return _table.isOurHandInThisRange("22+ K9o+ K8s+ A2o+ A2s+ QJs");                                            
                                        }                                        
                                }
                                #endregion
                            }                            
                        case Position.SB:
                            #region CO folds. BTN folds. SB shoves. We're from BB
                            CLogger.AddLog("CO folds. BTN folds. SB shoves. We're from BB.");
                            byte SBBBStackShover = _table.getBBEquivalentShove();

                            switch (SBBBStackShover)
                            {
                                case 8:
                                    return _table.isOurHandInThisRange("22+ K6o+ K2s+ A2o+ A2s+ QJo Q6s+");
                                case 9:
                                    return _table.isOurHandInThisRange("22+ K8o+ K2s+ A2o+ A2s+ QJo Q8s+");
                                case 10:
                                    return _table.isOurHandInThisRange("22+ KTo+ K3s+ A2o+ A2s+ Q9s+");
                                case 11:
                                    return _table.isOurHandInThisRange("22+ KTo+ K5s+ A2o+ A2s+ QTs+");
                                case 12:
                                    return _table.isOurHandInThisRange("22+ KJo+ K7s+ A2o+ A2s+ QJs");
                                case 13:
                                    return _table.isOurHandInThisRange("22+ KJo+ K8s+ A2o+ A2s+");
                                case 14:
                                    return _table.isOurHandInThisRange("33+ KQo K9s+ A2o+ A2s+");
                                case 15:
                                    return _table.isOurHandInThisRange("44+ KQo KTs+ A3o+ A2s+");
                                case 16:
                                    return _table.isOurHandInThisRange("44+ KQo KJs+ A4o+ A2s+");
                                case 17:
                                    return _table.isOurHandInThisRange("44+ KJs+ A4o+ A2s+");
                                case 18:
                                    return _table.isOurHandInThisRange("44+ KQs A5o+ A2s+");
                                default:
                                    if (SBBBStackShover > 18)
                                    {
                                        // Call a shove that is > 18BB
                                        return _table.isOurHandInThisRange("QQ+ AKs+ AKo+");
                                    }
                                    else
                                    {
                                        // Call a shove that is < 8BB
                                        return _table.isOurHandInThisRange("22+ K4o+ K2s+ A2o+ A2s+ Q9o+ Q4s+");                                        
                                    }                                    
                            }
                            #endregion                            
                        default:
                            throw new Exception("The open-shover position cannot be from the BB!");
                            #endregion
                    }
                    #endregion
            }

            throw new Exception("This line of code is never supposed to be executed! There's missing atleast one user-case scenario!");
        }
    }
}
