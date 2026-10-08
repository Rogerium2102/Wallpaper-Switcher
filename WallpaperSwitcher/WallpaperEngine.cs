using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace WallpaperSwitcher
{
    internal class WallpaperEngine
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern int SystemParametersInfo(
            uint uiAction,
            uint uiParam,
            string pvParam,
            uint fWinIni
        );

        private const uint SPI_SETDESKWALLPAPER = 0x14;
        private const uint SPIF_UPDATEINFILE = 0x01;
        private const uint SPIF_SENDWININICHANGE = 0x02;

        public static void SetWallpaper(string path)
        {
            SystemParametersInfo(SPI_SETDESKWALLPAPER, 0, path, SPIF_UPDATEINFILE | SPIF_SENDWININICHANGE);
        }
    }
}
