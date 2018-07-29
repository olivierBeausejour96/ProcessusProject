using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VersionOfficielle
{
    public class CScreenshot
    {
        static public Bitmap getBmpFromScreen(int _width, int _height, int _X, int _Y) {
            Bitmap bmpScreenshot = new Bitmap(_width, _height, PixelFormat.Format24bppRgb);
            using (var g = Graphics.FromImage(bmpScreenshot)) {
                g.CopyFromScreen(_X, _Y, 0, 0, Screen.PrimaryScreen.Bounds.Size);
                return bmpScreenshot;
            }
        }
    }
}
