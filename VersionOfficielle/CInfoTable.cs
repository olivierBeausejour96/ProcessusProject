using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowScrape.Types;

namespace WindowsFormsApplication4
{
    class CInfoTable
    {
        CDetectItem.myPosition FFMyPosition;
        HwndObject FFWindowHandle;
        CBoardReaderHeadsUp FFBoardReader;
        CPlayerMoneyReaderHeadsUp FFMoneyReader;
        CHandReaderHeadsUp FFHandReader;
        CDetectItem FFPositionDectecter;
        double FFPot;

        public CInfoTable(HwndObject _windowHandle)
        {
            FFWindowHandle = _windowHandle;
            InitialiserObjects();
        }

        /// <summary>
        /// Cree les instances de classe necessaire a CInfotable et trouve automatiquement la position de self
        /// </summary>
        void InitialiserObjects()
        {
            FFBoardReader = new CBoardReaderHeadsUp(FFWindowHandle);
            FFMoneyReader = new CPlayerMoneyReaderHeadsUp(FFWindowHandle);
            FFPositionDectecter = new CDetectItem(FFWindowHandle);
            FFHandReader = new CHandReaderHeadsUp(FFWindowHandle);
            //FindMySeat();
            FFPot = 0;
        }

        /// <summary>
        /// Trouve la position du joueur et l'affecte a la donne membre MyPosition
        /// </summary>
        void FindMySeat()
        {
            FFMyPosition = FFPositionDectecter.RetournerMyPosition();
            if (FFMyPosition == CDetectItem.myPosition.Aucun)
            {
                MessageBox.Show("Vous n'etes pas assis a la table");
            }
            else
            {
                MessageBox.Show("Vous etes assis a la table\nVotre position est:\n" + (char)FFMyPosition);
            }
        }

        /// <summary>
        /// FindMySeat() public
        /// </summary>
        public void ActualiserSiege()
        {
            FindMySeat();
        }

        public CCarte[] RetournerFlop()
        {
            return FFBoardReader.RetournerFlop();
        }

        public CCarte RetournerTurn()
        {
            return FFBoardReader.RetournerTurn();
        }

        public CCarte RetournerRiver()
        {
            return RetournerRiver();
        }

        public CCarte[] RetournerMain()
        {
            return FFHandReader.RetournerMain(); // TODO: Il faut considerer la position
        }

        public double RetournerJoueurGStack()
        {
            return FFMoneyReader.RetournerJoueurGStack;
        }

        public double RetournerJoueurDStack()
        {
            return FFMoneyReader.RetournerJoueurDStack;
        }

        public CDetectItem.myPosition RetournerHeroPosition()
        {
            return FFMyPosition;
        }

        public CDetectItem.myPosition RetournerDealerPosition()
        {
            return FFPositionDectecter.RetournerDealerPosition();
        }

        public bool RetournerEstMonTour()
        {
            return FFPositionDectecter.RetournerEstMonTour();
        }

        public double RetournerMinCallValeur()
        {
            return FFMoneyReader.RetournerCallValue();
        }
    }
}
