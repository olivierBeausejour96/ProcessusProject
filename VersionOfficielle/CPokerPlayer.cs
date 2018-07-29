using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VersionOfficielle
{

    class CPokerPlayer
    {
        private double stack;
        public ScreenPosition FFPos;
        public double FFStack
        {
            get
            {
                return Math.Max(stack, FFBet);
            }
            set
            {
                stack = value;
            }
        }
        public double FFBet;
        public PlayerStates FFState;

        public CPokerPlayer(ScreenPosition _pos = ScreenPosition.North, double _stack = 0, double _bet = 0, PlayerStates _state = PlayerStates.openedSeat)
        {
            FFPos = _pos;
            FFStack = _stack;
            FFBet = _bet;
            FFState = _state;
        }


        public bool isAllin()
        {
            return FFStack == 0 && FFBet != 0;
        }

        public void reinitialize()
        {
            FFPos = ScreenPosition.ScreenPositionCount;
            FFStack = 0;
            FFBet = 0;
            FFState = PlayerStates.openedSeat;
        }
    }
}