using UnityEngine;

namespace Kutie.Extensions
{
    public static class ColorExtensions
    {
        public static string ToHex(this Color color, bool includeAlpha = true)
        {
            byte r = (byte) (255 * color.r);
            byte g = (byte) (255 * color.g);
            byte b = (byte) (255 * color.b);
            byte a = (byte) (255 * color.a);

            string rs = $"{r:X}".PadLeft(2, '0');
            string gs = $"{g:X}".PadLeft(2, '0');
            string bs = $"{b:X}".PadLeft(2, '0');
            string as_ = $"{a:X}".PadLeft(2, '0');

            return includeAlpha ? $"{rs}{gs}{bs}{as_}" : $"{rs}{gs}{bs}";
        }

        public static Color WithA(this Color color, float a) => new Color(color.r, color.g, color.b, a);
    }
}
