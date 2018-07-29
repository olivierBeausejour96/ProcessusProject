using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VersionOfficielle.Helpers;

namespace VersionOfficielle
{
    static class CGGPokerConsantes
    {
        public const CPokerGame.GGLimits GG_LIMIT = CPokerGame.GGLimits.BBNL25;

        public const int GG_WINDOW_WIDTH  = 800;
        public const int GG_WINDOW_HEIGHT   = 600;

        // consts for hands coords and screen shots
        public const int GG_HAND_SIGN_RECTANGLE_HEIGHT = 20;
        public const int GG_HAND_SIGN_RECTANGLE_WIDTH = 20;
        public const int GG_HAND_VALUE_RECTANGLE_HEIGHT = 20;
        public const int GG_HAND_VALUE_RECTANGLE_WIDTH = 20;
        public const int GG_HAND_OFFSET2_WIDTH = 401;
        public const int GG_HAND_OFFSET1_HEIGHT = 465;
        public const int GG_HAND_OFFSET1_WIDTH = 342;
        public const int GG_HAND_OFFSET2_HEIGHT = 485;

        // consts for stacks coords and screen shots
        public const int GG_STACK_WEST_POSITION_OFFSET_WIDTH = 50;
        public const int GG_STACK_WEST_POSITION_OFFSET_HEIGHT = 317;
        public const int GG_STACK_NORTH_POSITION_OFFSET_WIDTH = 380;
        public const int GG_STACK_NORTH_POSITION_OFFSET_HEIGHT = 149;
        public const int GG_STACK_EST_POSITION_OFFSET_WIDTH = 700;
        public const int GG_STACK_EST_POSITION_OFFSET_HEIGHT = 317;
        //public const int GG_STACK_SOUTH_POSITION_OFFSET_WIDTH = 380;
        //public const int GG_STACK_SOUTH_POSITION_OFFSET_HEIGHT = 579;
        public const int GG_STACK_RECTANGLE_WIDTH = 70;
        public const int GG_STACK_RECTANGLE_HEIGHT = 11;


        // const for our stack
        public const int GG_STACK_SOUTH_POSITION_OFFSET_WIDTH = 380;
        public const int GG_STACK_SOUTH_POSITION_OFFSET_HEIGHT = 571;
        public const int GG_OUR_STACK_RECTANGLE_WIDTH = 70;
        public const int GG_OUR_STACK_RECTANGLE_HEIGHT = 11;

        // consts for bets coords and screen shots
        public const int GG_BET_WEST_POSITION_OFFSET_WIDTH =  110;
        public const int GG_BET_WEST_POSITION_OFFSET_HEIGHT = 317;
        public const int GG_BET_NORTH_POSITION_OFFSET_WIDTH = 315;
        public const int GG_BET_NORTH_POSITION_OFFSET_HEIGHT = 201;
        public const int GG_BET_EST_POSITION_OFFSET_WIDTH = 590;
        public const int GG_BET_EST_POSITION_OFFSET_HEIGHT = 317;
        public const int GG_BET_SOUTH_POSITION_OFFSET_WIDTH = 345;
        public const int GG_BET_SOUTH_POSITION_OFFSET_HEIGHT = 437;
        public const int GG_BET_RECTANGLE_WIDTH = 100;
        public const int GG_BET_RECTANGLE_HEIGHT = 11;

        // delimiters to determine wether the position on Screen of a player is N, E, S or W
        public const int GG_SCREEN_NORTH_LOWER_BOUND = 190;
        public const int GG_SCREEN_SOUTH_UPPER_BOUND = 400;
        public const int GG_SCREEN_WEST_RIGHT_BOUND = 200;
        public const int GG_SCREEN_EST_LEFT_BOUND = 600;

        //Offsets and rectangle's dimensions to get the Bitmap of the HandNumber. Used to determine end of hands.
        public const int GG_HAND_NUMBER_OFFSET_WIDTH = 10;
        public const int GG_HAND_NUMBER_OFFSET_HEIGHT = 83;
        public const int GG_HAND_NUMBER_RECTANGLE_WIDTH = 110;
        public const int GG_HAND_NUMBER_RECTANGLE_HEIGHT = 20;

        public const int GG_DEAL_ME_IN_WIDTH_OFFSET = 50;
        public const int GG_DEAL_ME_IN_HEIGHT_OFFSET = 20;


        public const string GG_AUTO_SAVE_FOLDER_PATH = "C:\\Users\\olibb\\Desktop\\Hands\\";

        public const string GG_SNAP_FOLD_RANGE = "32o 42o 43o 52o 53o 54o 62o 63o 64o 72o 73o 74o 82o 83o 84o 92o 93o 94o 95o T2o T3o T4o T5o J2o J3o J4o J5o";

    }
}
