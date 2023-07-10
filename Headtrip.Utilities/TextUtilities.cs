using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Headtrip.Utilities
{
    public static class TextUtilities
    {
        public static int RGBToColorCode(byte r, byte g, byte b)=>
            (int)(((uint)r << 16) | ((uint)g << 8) | (uint)b);

        public static void ColorCodeToRGB(int c, out byte r, out byte g, out byte b)
        {
            r = (byte)(((uint)c & 0x00ff0000) >> 16);
            g = (byte)(((uint)c & 0x0000ff00) >> 8);
            b = (byte)(((uint)c & 0x000000ff));
        }

        public static string CreateColorCodeEmbed(int ColorCode)
            => $"\x11[{ColorCode}]";

        public static string CreateLinkEmbed(string Text, string Binding)
            => $"\x12{Text}\x13{Binding}\x14";

    }
}
