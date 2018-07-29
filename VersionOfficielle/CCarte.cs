using System;
namespace VersionOfficielle
{
    public class CCarte
    {
        // Les énumérés sont basés sur les charactères ASCII pour les indices.
        public enum Valeur
        {
            Ace = 65,
            Two = 50, Three = 51, Four = 52, Five = 53, Six = 54, Seven = 55, Eight = 56, Nine = 57, Ten = 84,
            Jack = 74,
            Queen = 81,
            King = 75
        }

        public enum Type
        {
            Spades = 115,
            Hearts = 104,
            Diamonds = 100,
            Clubs = 99
        }

        private Valeur FValeur;
        private Type FType;

        public CCarte(Valeur _carteValeur, Type _carteType)
        {
            if (!Enum.IsDefined(typeof(Valeur), _carteValeur) || !Enum.IsDefined(typeof(Type), _carteType))
                throw new ArgumentException();

            FValeur = _carteValeur;
            FType = _carteType;
        }

        public override string ToString()
        {
            return String.Concat((char)(FValeur), (char)FType);
        }
    }
}