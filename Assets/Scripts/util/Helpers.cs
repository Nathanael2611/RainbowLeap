using System;
using UnityEngine;

namespace util
{
    public static class Helpers
    {
        public static Vector2 Rotate(Vector2 vec, float rotation)
        {
            var f = Mathf.Cos(rotation);
            var f1 = Mathf.Sin(rotation);
            var d0 = vec.x * (float)f + vec.y * (float)f1;
            var d2 = vec.y * (float)f - vec.x * (float)f1;
            return new Vector2(d0, d2);
        }
    
        public static float CompareLabs(Vector3 labA, Vector3 labB)
        {
            var deltaL = labA[0] - labB[0];
            var deltaA = labA[1] - labB[1];
            var deltaB = labA[2] - labB[2];
            var c1 = Mathf.Sqrt(labA[1] * labA[1] + labA[2] * labA[2]);
            var c2 = Mathf.Sqrt(labB[1] * labB[1] + labB[2] * labB[2]);
            var deltaC = c1 - c2;
            var deltaH = deltaA * deltaA + deltaB * deltaB - deltaC * deltaC;
            deltaH = deltaH < 0 ? 0 : Mathf.Sqrt(deltaH);
            var sc = 1.0 + 0.045 * c1;
            var sh = 1.0 + 0.015 * c1;
            var deltaLKlsl = deltaL / (1.0);
            var deltaCkcsc = deltaC / (sc);
            var deltaHkhsh = deltaH / (sh);
            var i = deltaLKlsl * deltaLKlsl + deltaCkcsc * deltaCkcsc + deltaHkhsh * deltaHkhsh;
            return i < 0 ? 0 : Mathf.Sqrt((float)i); 
        }
    
        public static Vector3 ConvertRGBToLab(Color color)
        {
            double red = color.r;
            double green = color.g;
            double blue  = color.b;

            //making the RGB values linear and in the nominal range b/t 0.0 and 1.0
            if (red > 0.04045)
                red = Math.Pow(((red + 0.055) / 1.055), 2.4);
            else
                red = red / 12.92;

            if (green > 0.04045)
                green = Math.Pow(((green + 0.055) / 1.055), 2.4);
            else
                green = green / 12.92;

            if (blue > 0.04045)
                blue = Math.Pow(((blue + 0.055) / 1.055), 2.4);
            else
                blue = blue / 12.92;

            red *= 100;
            green *= 100;
            blue *= 100;

            //converting to XYZ color space
            double x, y, z;
            x = red * 0.4124 + green * 0.3576 + blue * 0.1805;
            y = red * 0.2126 + green * 0.7152 + blue * 0.0722;
            z = red * 0.0193 + green * 0.1192 + blue * 0.9505;

            //finally, converting XYZ color space to CIE-L*ab color space
            x /= 95.047;
            y /= 100;
            z /= 108.883;

            if (x > 0.008856)
                x = Math.Pow(x, (.3333333333));
            else
                x = (7.787 * x) + (16 / 116);

            if (y > 0.008856)
                y = Math.Pow(y, (.3333333333));
            else
                y = (7.787 * y) + (16 / 116);

            if (z > 0.008856)
                z = Math.Pow(z, (.3333333333));
            else
                z = (7.787 * z) + (16 / 116);

            //last step
            double L, a, b;
            L = (116 * y) - 16;
            a = 500 * (x - y);
            b = 200 * (y - z);

            return new Vector3((float) L, (float) a, (float) b);
        }
    
    }
}