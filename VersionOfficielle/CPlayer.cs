using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VersionOfficielle.Helpers;

namespace VersionOfficielle
{
    public enum PokerPosition
    {
        CO = 2,
        dealer = 3,
        SB = 0,
        BB = 1,

        PokerPositionCount =4
    }

    enum PlayerStates
    {
        playing = 0,
        sittingOut,
        openedSeat,
        waitingForBB,

        PlayerStatesCount
    }

    class CPlayer
    {
        public PokerPosition PPos;
        public double PStack;
        public double PBet;
        public PlayerStates PState;

        public CPlayer(PokerPosition _pos, double _stack, double _bet, PlayerStates _state)
        {
            PPos   = _pos;
            PStack = _stack;
            PBet = _bet;
            PState = _state;
        }

        public bool IsAllIn()
        {
            return PBet > CGGPokerConsantes.GG_LIMIT.getLimit();
        }

        public void Reinitialize()
        {
            PPos = PokerPosition.PokerPositionCount;
            PStack = 0;
            PBet = 0;
            PState = PlayerStates.openedSeat;
        }

    }
}
