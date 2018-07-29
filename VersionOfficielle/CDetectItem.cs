using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using WindowScrape.Types;

namespace VersionOfficielle
{
    static class CDetectItem
    {

        /// <summary>
        /// Cherche une liste d'object dans une zone de recherche predefinie. 
        /// Ajoute la position de chaque object trouver a une liste. 
        /// Ne retourne que si le premier object trouve est a gauche ou a droite pour l'instant.
        /// </summary>
        /// <param name="_theWindow"></param>
        /// <param name="_theListItem"></param>
        /// <returns>Liste de position des items trouve</returns>
        static public unsafe List<CPosition> RetournerPosItemList(Bitmap theBmp, List<Bitmap> _theListItem)
        {
            // TODO: Retourner un liste associant indice de _theListItem et Pos
            Bitmap bmp2 = theBmp;
            BitmapData bmp = bmp2.LockBits(new Rectangle(0, 0, bmp2.Width, bmp2.Height), ImageLockMode.ReadOnly, bmp2.PixelFormat);

            List<BitmapData> bmpDataList = new List<BitmapData>();
            for (int i = 0; i < _theListItem.Count; i++)
            {
                bmpDataList.Add(_theListItem[i].LockBits(new Rectangle(0, 0, _theListItem[i].Width, _theListItem[i].Height), ImageLockMode.ReadOnly, _theListItem[i].PixelFormat));
            }
            

            int bytesPerPixel = System.Drawing.Bitmap.GetPixelFormatSize(bmp.PixelFormat) / 8;

            int windowHeightInPixels = bmp.Height;
            int windowWidthInBytes = bytesPerPixel * bmp.Width;

            List<int> imageHeightInPixelsList = new List<int>();
            List<int> imageWidthInBytesList = new List<int>();
            for (int i = 0; i < bmpDataList.Count; i++)
            {
                imageHeightInPixelsList.Add(bmpDataList[i].Height);
                imageWidthInBytesList.Add(bytesPerPixel * bmpDataList[i].Width);
            }            

            byte* ptrFirstPixel = (byte*)bmp.Scan0;
            byte*[] ptrImageArray = new byte*[bmpDataList.Count];
            for (int i = 0; i < bmpDataList.Count; i++)
            {
                ptrImageArray[i] = (byte*)bmpDataList[i].Scan0;
            }

            List<CPosition> decompte = new List<CPosition>();

            double Acc = 0;

            for (int i = 0; i < bmpDataList.Count; i++)
            {
                for (int l = 0; l <= windowHeightInPixels - imageHeightInPixelsList[i]; l++)
                {
                    for (int j = 0; j <= windowWidthInBytes - imageWidthInBytesList[i]; j += bytesPerPixel)
                    {
                        for (int y = 0; y < imageHeightInPixelsList[i]; y++)
                        {
                            byte* currentLineWindow = ptrFirstPixel + (y * bmp.Stride) + j + (bmp.Stride) * l;
                            byte* currentLineImage = ptrImageArray[i] + (y * bmpDataList[i].Stride);
                            for (int k = 0; k < imageWidthInBytesList[i]; k += bytesPerPixel)
                            {
                                Acc += Math.Pow(currentLineWindow[k] - currentLineImage[k], 2);
                                Acc += Math.Pow(currentLineWindow[k + 1] - currentLineImage[k + 1], 2);
                                Acc += Math.Pow(currentLineWindow[k + 2] - currentLineImage[k + 2], 2);

                                if (Acc >= 6000)
                                {
                                    goto NotFound;
                                }
                            }
                        }

                        CPosition temp1 = new CPosition(j/bytesPerPixel,l);
                        decompte.Add(temp1);
                    NotFound:
                        Acc = 0;
                    }
                }
            }

            bmp2.UnlockBits(bmp);
            for (int i = 0; i < _theListItem.Count; i++)
            {
                _theListItem[i].UnlockBits(bmpDataList[0]);
            }
            return decompte;
        }
    }
}
