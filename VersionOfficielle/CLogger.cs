﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;

namespace VersionOfficielle
{
    public static class CLogger
    {
        private const int DEFAULT_FONT_HEIGHT_PIXEL_SIZE = 20;

        private static Queue<CLogContainer> FFLstLogs = new Queue<CLogContainer>();

        /// <summary>
        /// Add a log to the lastest log container.
        /// </summary>
        /// <param name="_log">Consists of a message that we wants to add.</param>
        public static void AddLog(string _message)
        {
            //FFLstLogs.Peek().AddLog(_message);
            Debug.WriteLine(_message);
        }    

        public static void ChangePathOfLastestLogContainer(string _newPath)
        {
            FFLstLogs.Peek().PPath = _newPath;
        }

        public static void SetLogImage(Bitmap _screenshot)
        {
            FFLstLogs.Peek().PImage = _screenshot;
        }
            
        public static void ClearLogs()
        {
            FFLstLogs.Clear();
        }

        /// <summary>
        /// Create a object that contains a group of messages (logs) and one image linked to it.
        /// </summary>
        public static void CreateLogContainer(string _path)
        {
            FFLstLogs.Enqueue(new CLogContainer(_path));
        }

        /// <summary>
        /// Writes the logs in an image form. Will includes images.    
        /// WILL REMOVES THE LOGS AUTOMATICALLY.
        /// </summary>
        public static void WriteToFileAndDeleteLogs()
        {
            const string DEFAULT_FONT_NAME = "Arial";
            const int DEFAULT_FONT_SIZE = 12;
            const int DEFAULT_IMAGE_HEIGHT = 1000;
            const int DEFAULT_IMAGE_WIDTH = 1000;

            for (int currentLogIndex = 0; currentLogIndex < FFLstLogs.Count; ++currentLogIndex)
            {
                CLogContainer currentLogContainer = FFLstLogs.Dequeue();

                Bitmap wholeImage = null;

                if (currentLogContainer.PImage != null)
                {
                    wholeImage = ConvertTextToImage(currentLogContainer.GetLogs(true),
                                                    DEFAULT_FONT_NAME,
                                                    DEFAULT_FONT_SIZE,
                                                    Color.White,
                                                    Color.Black,
                                                    currentLogContainer.PImage.Width,
                                                    currentLogContainer.PImage.Height + (currentLogContainer.PLogsCount * DEFAULT_FONT_HEIGHT_PIXEL_SIZE));

                    Graphics drawer = Graphics.FromImage(wholeImage);

                    drawer.DrawImage(currentLogContainer.PImage,
                                     0,
                                     DEFAULT_FONT_HEIGHT_PIXEL_SIZE * 2,
                                     currentLogContainer.PImage.Width,
                                     currentLogContainer.PImage.Height);
                }
                else
                {
                    wholeImage = ConvertTextToImage(currentLogContainer.GetLogs(true),
                                                    DEFAULT_FONT_NAME,
                                                    DEFAULT_FONT_SIZE,
                                                    Color.White,
                                                    Color.Black,
                                                    DEFAULT_IMAGE_WIDTH,
                                                    DEFAULT_IMAGE_HEIGHT);
                }
                
                string pathWithFileName = currentLogContainer.PPath + " " + currentLogContainer.PDateCreated.ToString().Replace(":", "-").Replace("/", "-") + ".bmp";
                wholeImage.Save(pathWithFileName);
            }
        }

        // SOURCE: https://www.codeproject.com/Tips/184102/Convert-Text-to-Image
        private static Bitmap ConvertTextToImage(string txt, string fontname, int fontsize, Color bgcolor, Color fcolor, int width, int Height)
        {
            Bitmap bmp = new Bitmap(width, Height);
            using (Graphics graphics = Graphics.FromImage(bmp))
            {

                Font font = new Font(fontname, fontsize);
                graphics.FillRectangle(new SolidBrush(bgcolor), 0, 0, bmp.Width, bmp.Height);
                graphics.DrawString(txt, font, new SolidBrush(fcolor), 0, 0);
                graphics.Flush();
                font.Dispose();
                graphics.Dispose();
            }
            return bmp;
        }
    }
}
