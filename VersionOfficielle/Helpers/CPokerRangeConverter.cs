﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace VersionOfficielle.Helpers
{
    /// <summary>
    /// Convert a range into all combinaisons possible
    /// </summary>
    public static class CPokerRangeConverter
    {
        private enum CardType {Heart = 'h', Diamond = 'd', Spade = 's', Clubs = 'c' };

        /// <summary>
        /// Convert a range given (range must be in this format: "22+ AK+ JTs") into all combinaison possible.
        /// </summary>
        /// <param name="_range">The range separated by white space.</param>
        /// <returns>Returns all the combinations without duplicate.</returns>
        public static string[] ConvertRange(string _range)
        {
            string[] handsRange = _range.Split(' ');
            string combos = "";

            for (int currentHandRange = 0; currentHandRange < handsRange.Length; ++currentHandRange)
            {
                // If the last character of the current handrange (example: JTo+) has a +
                if (handsRange[currentHandRange].Last() == '+')
                {
                    string currentHand = handsRange[currentHandRange].Substring(0, handsRange[currentHandRange].Length - 1);

                    while (currentHand != null)
                    {
                        if (string.IsNullOrEmpty(combos))
                            combos = GetAllCombinations(currentHand);
                        else
                            combos += " " + GetAllCombinations(currentHand);

                        if (IsPocketPair(currentHand))
                        {
                            switch (currentHand[1])
                            {
                                case 'A':
                                    currentHand = null;
                                    break;
                                case '2':
                                    currentHand = "33";
                                    break;
                                case '3':
                                    currentHand = "44";
                                    break;
                                case '4':
                                    currentHand = "55";
                                    break;
                                case '5':
                                    currentHand = "66";
                                    break;
                                case '6':
                                    currentHand = "77";
                                    break;
                                case '7':
                                    currentHand = "88";
                                    break;
                                case '8':
                                    currentHand = "99";
                                    break;
                                case '9':
                                    currentHand = "TT";
                                    break;
                                case 'T':
                                    currentHand = "JJ";
                                    break;
                                case 'J':
                                    currentHand = "QQ";
                                    break;
                                case 'Q':
                                    currentHand = "KK";
                                    break;
                                case 'K':
                                    currentHand = "AA";
                                    break;
                                default:
                                    throw new Exception("Cannot read the hand!");
                            }
                        }
                        else
                        {
                            switch (currentHand[1])
                            {
                                case 'A':
                                    currentHand = null;
                                    break;
                                case '2':
                                    currentHand = currentHand[0] + "3" + currentHand.Last();
                                    break;
                                case '3':
                                    currentHand = currentHand[0] + "4" + currentHand.Last();
                                    break;
                                case '4':
                                    currentHand = currentHand[0] + "5" + currentHand.Last();
                                    break;
                                case '5':
                                    currentHand = currentHand[0] + "6" + currentHand.Last();
                                    break;
                                case '6':
                                    currentHand = currentHand[0] + "7" + currentHand.Last();
                                    break;
                                case '7':
                                    currentHand = currentHand[0] + "8" + currentHand.Last();
                                    break;
                                case '8':
                                    currentHand = currentHand[0] + "9" + currentHand.Last();
                                    break;
                                case '9':
                                    currentHand = currentHand[0] + "T" + currentHand.Last();
                                    break;
                                case 'T':
                                    currentHand = currentHand[0] + "J" + currentHand.Last();
                                    break;
                                case 'J':
                                    currentHand = currentHand[0] + "Q" + currentHand.Last();
                                    break;
                                case 'Q':
                                    currentHand = currentHand[0] + "K" + currentHand.Last();
                                    break;
                                case 'K':
                                    currentHand = currentHand[0] + "A" + currentHand.Last();
                                    break;
                                default:
                                    throw new Exception("Cannot read the hand!");
                            }
                        }
                    }                    
                }
                else
                {
                    if (string.IsNullOrEmpty(combos))
                        combos = GetAllCombinations(handsRange[currentHandRange]);
                    else
                        combos += " " + GetAllCombinations(handsRange[currentHandRange]);
                }                    
            }

            return combos.Split(' ').Distinct().OrderBy(x => x).ToArray();
        }

        private static string GetAllCombinations(string _hand)
        {
            if (_hand.Length != 2 && _hand.Length != 3)
                throw new Exception("The string given is not a poker hand!");

            string combinaisons = "";

            if (IsOffSuited(_hand))
                combinaisons = GetOffSuitedCombinaison(_hand);
            else if (IsPocketPair(_hand))
                combinaisons = GetPocketPairCombinaison(_hand);
            else if (IsSuited(_hand))
                combinaisons = GetSuitedCombinaison(_hand);
            else
                throw new Exception("Impossible to convert the range given!");

            return combinaisons;
        }

        private static string SortCombination(string _combo)
        {
            if (_combo.Length != 4)
                throw new Exception("The combination given is not valid!");

            // If it is a pocket pair.
            if (_combo[0] == _combo[2])
            {
                switch (_combo[1])
                {
                    case 'h':
                        return _combo;
                    case 'd':
                        if (_combo[3] == 'h')
                            return RevertPokerCombination(_combo);
                        else
                            return _combo;
                    case 's':
                        if ((_combo[3] == 'd') || (_combo[3] == 'h'))
                            return RevertPokerCombination(_combo);
                        else
                            return _combo;
                    case 'c':
                        return RevertPokerCombination(_combo);
                    default:
                        throw new Exception("The combination given is not valid!");                                              
                }
            }

            // Otherwise it's something else, either a suited or a offsuited card.
            switch (_combo[2])
            {
                case 'A':
                    return RevertPokerCombination(_combo);
                case 'K':
                    if (_combo[0] != 'A')
                        return RevertPokerCombination(_combo);
                    else
                        return _combo;
                case 'Q':
                    if ((_combo[0] != 'A') && (_combo[0] != 'K'))
                        return RevertPokerCombination(_combo);
                    else
                        return _combo;
                case 'J':
                    if ((_combo[0] != 'A') && (_combo[0] != 'K') && (_combo[0] != 'Q'))
                        return RevertPokerCombination(_combo);
                    else
                        return _combo;
                case 'T':
                    if ((_combo[0] != 'A') && (_combo[0] != 'K') && (_combo[0] != 'Q') && (_combo[0] != 'J'))
                        return RevertPokerCombination(_combo);
                    else
                        return _combo;
                case '9':
                    if ((_combo[0] != 'A') && (_combo[0] != 'K') && (_combo[0] != 'Q') && (_combo[0] != 'J') && (_combo[0] != 'T'))
                        return RevertPokerCombination(_combo);
                    else
                        return _combo;
                case '8':
                    if ((_combo[0] != 'A') && (_combo[0] != 'K') && (_combo[0] != 'Q') && (_combo[0] != 'J') && (_combo[0] != 'T') && (_combo[0] != '9'))
                        return RevertPokerCombination(_combo);
                    else
                        return _combo;
                case '7':
                    if ((_combo[0] != 'A') && (_combo[0] != 'K') && (_combo[0] != 'Q') && (_combo[0] != 'J') && (_combo[0] != 'T') && (_combo[0] != '9') && (_combo[0] != '8'))
                        return RevertPokerCombination(_combo);
                    else
                        return _combo;
                case '6':
                    if ((_combo[0] != 'A') && (_combo[0] != 'K') && (_combo[0] != 'Q') && (_combo[0] != 'J') && (_combo[0] != 'T') && (_combo[0] != '9') && (_combo[0] != '8') && (_combo[0] != '7'))
                        return RevertPokerCombination(_combo);
                    else
                        return _combo;
                case '5':
                    if ((_combo[0] != 'A') && (_combo[0] != 'K') && (_combo[0] != 'Q') && (_combo[0] != 'J') && (_combo[0] != 'T') && (_combo[0] != '9') && (_combo[0] != '8') && (_combo[0] != '7') && (_combo[0] != '6'))
                        return RevertPokerCombination(_combo);
                    else
                        return _combo;
                case '4':
                    if ((_combo[0] != 'A') && (_combo[0] != 'K') && (_combo[0] != 'Q') && (_combo[0] != 'J') && (_combo[0] != 'T') && (_combo[0] != '9') && (_combo[0] != '8') && (_combo[0] != '7') && (_combo[0] != '6') && (_combo[0] != '5'))
                        return RevertPokerCombination(_combo);
                    else
                        return _combo;
                case '3':
                    if ((_combo[0] != 'A') && (_combo[0] != 'K') && (_combo[0] != 'Q') && (_combo[0] != 'J') && (_combo[0] != 'T') && (_combo[0] != '9') && (_combo[0] != '8') && (_combo[0] != '7') && (_combo[0] != '6') && (_combo[0] != '5') && (_combo[0] != '4'))
                        return RevertPokerCombination(_combo);
                    else
                        return _combo;
                case '2':
                    return _combo;
                default:
                    throw new Exception("The combination given is not valid!");
            }
        }

        private static string RevertPokerCombination(string _combo)
        {
            if (_combo.Length != 4)
                throw new Exception("The combination given is not valid!");

            return string.Concat(_combo[2], _combo[3], _combo[0], _combo[1]);
        }

        private static bool IsPocketPair(string _hand)
        {
            return _hand[0] == _hand[1];
        }

        private static bool IsSuited(string _hand)
        {
            if (_hand.Length == 3)
                return _hand[2] == 's';
            else
                return false;
        }

        private static bool IsOffSuited(string _hand)
        {
            if (_hand.Length == 3)
                return _hand[2] == 'o';
            else
                return false;
        }

        private static string GetPocketPairCombinaison(string _hand)
        {
            if (!IsPocketPair(_hand))
                throw new Exception("The hand is not a pocket pair!");

            string combo1 = string.Concat(_hand[0], (char)CardType.Heart, _hand[1], (char)CardType.Diamond);
            string combo2 = string.Concat(_hand[0], (char)CardType.Heart, _hand[1], (char)CardType.Spade);
            string combo3 = string.Concat(_hand[0], (char)CardType.Heart, _hand[1], (char)CardType.Clubs);
            string combo4 = string.Concat(_hand[0], (char)CardType.Diamond, _hand[1], (char)CardType.Spade);
            string combo5 = string.Concat(_hand[0], (char)CardType.Diamond, _hand[1], (char)CardType.Clubs);
            string combo6 = string.Concat(_hand[0], (char)CardType.Spade, _hand[1], (char)CardType.Clubs);

            List<string> combos = new List<string>();

            combos.Add(combo1);
            combos.Add(combo2);
            combos.Add(combo3);
            combos.Add(combo4);
            combos.Add(combo5);
            combos.Add(combo6);

            for (int currentCombinationIndex = 0; currentCombinationIndex < combos.Count; currentCombinationIndex++)
                combos[currentCombinationIndex] = SortCombination(combos[currentCombinationIndex]);

            return string.Join(" ", combos);            
        }

        private static string GetSuitedCombinaison(string _hand)
        {
            if (!IsSuited(_hand))
                throw new Exception("The hand is not suited!");

            string combo1 = string.Concat(_hand[0], (char)CardType.Heart, _hand[1], (char)CardType.Heart);
            string combo2 = string.Concat(_hand[0], (char)CardType.Diamond, _hand[1], (char)CardType.Diamond);
            string combo3 = string.Concat(_hand[0], (char)CardType.Spade, _hand[1], (char)CardType.Spade);
            string combo4 = string.Concat(_hand[0], (char)CardType.Clubs, _hand[1], (char)CardType.Clubs);

            List<string> combos = new List<string>();

            combos.Add(combo1);
            combos.Add(combo2);
            combos.Add(combo3);
            combos.Add(combo4);

            for (int currentCombinationIndex = 0; currentCombinationIndex < combos.Count; currentCombinationIndex++)
                combos[currentCombinationIndex] = SortCombination(combos[currentCombinationIndex]);

            return string.Join(" ", combos);
        }

        private static string GetOffSuitedCombinaison(string _hand)
        {
            if (!IsOffSuited(_hand))
                throw new Exception("The hand is not offsuited!");

            string combo1 = string.Concat(_hand[0], (char)CardType.Heart, _hand[1], (char)CardType.Diamond);
            string combo2 = string.Concat(_hand[0], (char)CardType.Heart, _hand[1], (char)CardType.Spade);
            string combo3 = string.Concat(_hand[0], (char)CardType.Heart, _hand[1], (char)CardType.Clubs);
            string combo4 = string.Concat(_hand[0], (char)CardType.Diamond, _hand[1], (char)CardType.Heart);
            string combo5 = string.Concat(_hand[0], (char)CardType.Diamond, _hand[1], (char)CardType.Spade);
            string combo6 = string.Concat(_hand[0], (char)CardType.Diamond, _hand[1], (char)CardType.Clubs);
            string combo7 = string.Concat(_hand[0], (char)CardType.Spade, _hand[1], (char)CardType.Heart);
            string combo8 = string.Concat(_hand[0], (char)CardType.Spade, _hand[1], (char)CardType.Diamond);
            string combo9 = string.Concat(_hand[0], (char)CardType.Spade, _hand[1], (char)CardType.Clubs);
            string combo10 = string.Concat(_hand[0], (char)CardType.Clubs, _hand[1], (char)CardType.Heart);
            string combo11 = string.Concat(_hand[0], (char)CardType.Clubs, _hand[1], (char)CardType.Diamond);
            string combo12 = string.Concat(_hand[0], (char)CardType.Clubs, _hand[1], (char)CardType.Spade);

            List<string> combos = new List<string>();

            combos.Add(combo1);
            combos.Add(combo2);
            combos.Add(combo3);
            combos.Add(combo4);
            combos.Add(combo5);
            combos.Add(combo6);
            combos.Add(combo7);
            combos.Add(combo8);
            combos.Add(combo9);
            combos.Add(combo10);
            combos.Add(combo11);
            combos.Add(combo12);

            for (int currentCombinationIndex = 0; currentCombinationIndex < combos.Count; currentCombinationIndex++)            
                combos[currentCombinationIndex] = SortCombination(combos[currentCombinationIndex]);

            return string.Join(" ", combos);
        }
    }
}
