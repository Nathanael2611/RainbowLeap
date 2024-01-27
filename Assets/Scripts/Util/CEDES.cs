using System;
using UnityEngine;

namespace Util
{
    public static class CEDES
    {
        
        
        private static double[] RGBtoLABColors(UnityEngine.Color c)
        {
            double r, g, b, X, Y, Z, xr, yr, zr;

            // D65/2°
            double Xr = 95.047;
            double Yr = 100.0;
            double Zr = 108.883;


            // --------- RGB to XYZ ---------//
            r = c.r;
            g = c.g;
            b = c.b;

            if (r > 0.04045)
                r = Math.Pow((r + 0.055) / 1.055, 2.4);
            else
                r /= 12.92;

            if (g > 0.04045)
                g = Math.Pow((g + 0.055) / 1.055, 2.4);
            else
                g /= 12.92;

            if (b > 0.04045)
                b = Math.Pow((b + 0.055) / 1.055, 2.4);
            else
                b /= 12.92;

            r *= 100;
            g *= 100;
            b *= 100;

            X = (0.4124 * r) + (0.3576 * g) + (0.1805 * b);
            Y = (0.2126 * r) + (0.7152 * g) + (0.0722 * b);
            Z = (0.0193 * r) + (0.1192 * g) + (0.9505 * b);


            // --------- XYZ to Lab --------- //
            xr = X / Xr;
            yr = Y / Yr;
            zr = Z / Zr;

            if (xr > 0.008856)
                xr = Math.Pow(xr, 0.3333333333);
            else
                xr = ((7.787 * xr) + (16 / 116.0));

            if (yr > 0.008856)
                yr = Math.Pow(yr, 0.3333333333);
            else
                yr = ((7.787 * yr) + (16 / 116.0));

            if (zr > 0.008856)
                zr = Math.Pow(zr, 0.3333333333);
            else
                zr = ((7.787 * zr) + (16 / 116.0));


            double[] lab = new double[3];

            lab[0] = (double)((116 * yr) - 16);
            lab[1] = (double)(500 * (xr - yr));
            lab[2] = (double)(200 * (yr - zr));

            return lab;
        }


        public static double CalculateDeltaE(UnityEngine.Color colorX, UnityEngine.Color colorY)
        {
            double[] labX = RGBtoLABColors(colorX);
            double[] labY = RGBtoLABColors(colorY);

            double l = (labX[0] - labY[0]) * (labX[0] - labY[0]);
            double a = (labX[1] - labY[1]) * (labX[1] - labY[1]);
            double b = (labX[2] - labY[2]) * (labX[2] - labY[2]);

            return (double)Math.Sqrt(l + a + b);
        }
        
        
        public static double CalculateCIEDE2000(Color color1, Color color2)
        {
            // Convertir les couleurs RGB en couleurs Lab
            Lab color1Lab = RGBtoLab(color1);
            Lab color2Lab = RGBtoLab(color2);

            // Calculer la différence CIEDE2000
            double deltaE = CIEDE2000(color1Lab, color2Lab);

            return deltaE;
        }

        public static Lab RGBtoLab(Color rgb)
        {
            double r = rgb.r /* 255.0*/;
            double g = rgb.g /* 255.0*/;
            double b = rgb.b /* 255.0*/;

            r = (r > 0.04045) ? Math.Pow((r + 0.055) / 1.055, 2.4) : r / 12.92;
            g = (g > 0.04045) ? Math.Pow((g + 0.055) / 1.055, 2.4) : g / 12.92;
            b = (b > 0.04045) ? Math.Pow((b + 0.055) / 1.055, 2.4) : b / 12.92;

            r *= 100.0;
            g *= 100.0;
            b *= 100.0;

            double x = r * 0.4124564 + g * 0.3575761 + b * 0.1804375;
            double y = r * 0.2126729 + g * 0.7151522 + b * 0.0721750;
            double z = r * 0.0193339 + g * 0.1191920 + b * 0.9503041;

            x /= 95.047;
            y /= 100.000;
            z /= 108.883;

            x = (x > 0.008856) ? Math.Pow(x, 1.0 / 3.0) : ((903.3 * x) + 16.0) / 116.0;
            y = (y > 0.008856) ? Math.Pow(y, 1.0 / 3.0) : ((903.3 * y) + 16.0) / 116.0;
            z = (z > 0.008856) ? Math.Pow(z, 1.0 / 3.0) : ((903.3 * z) + 16.0) / 116.0;

            return new Lab
            {
                L = Math.Max(0.0, (116.0 * y) - 16.0),
                a = (x - y) * 500.0,
                b = (y - z) * 200.0
            };
        }

        public static double CIEDE2000(Lab lab1, Lab lab2)
        {
            double L1 = lab1.L;
            double a1 = lab1.a;
            double b1 = lab1.b;

            double L2 = lab2.L;
            double a2 = lab2.a;
            double b2 = lab2.b;

            double kL = 1.0;
            double kC = 1.0;
            double kH = 1.0;

            double C1 = Math.Sqrt(a1 * a1 + b1 * b1);
            double C2 = Math.Sqrt(a2 * a2 + b2 * b2);

            double a_C1_C2 = (C1 + C2) / 2.0;

            double G = 0.5 * (1 - Math.Sqrt(Math.Pow(a_C1_C2, 7.0) / (Math.Pow(a_C1_C2, 7.0) + Math.Pow(25.0, 7.0))));

            double a1Prime = (1.0 + G) * a1;
            double a2Prime = (1.0 + G) * a2;

            double C1Prime = Math.Sqrt(a1Prime * a1Prime + b1 * b1);
            double C2Prime = Math.Sqrt(a2Prime * a2Prime + b2 * b2);

            double h1Prime = (b1 == 0.0 && a1Prime == 0.0) ? 0.0 : Math.Atan2(b1, a1Prime);
            double h2Prime = (b2 == 0.0 && a2Prime == 0.0) ? 0.0 : Math.Atan2(b2, a2Prime);

            h1Prime += (h1Prime < 0.0) ? 2.0 * Math.PI : 0.0;
            h2Prime += (h2Prime < 0.0) ? 2.0 * Math.PI : 0.0;

            double deltaLPrime = L2 - L1;
            double deltaCPrime = C2Prime - C1Prime;

            double deltahPrime;
            if (C1Prime * C2Prime == 0.0)
            {
                deltahPrime = 0.0;
            }
            else
            {
                deltahPrime = (Math.Abs(h2Prime - h1Prime) <= Math.PI) ? h2Prime - h1Prime : (h2Prime <= h1Prime) ? h2Prime - h1Prime + 2.0 * Math.PI : h2Prime - h1Prime - 2.0 * Math.PI;
            }

            double deltaHPrime = 2.0 * Math.Sqrt(C1Prime * C2Prime) * Math.Sin(deltahPrime / 2.0);

            double LPrime = (L1 + L2) / 2.0;
            double CPrime = (C1Prime + C2Prime) / 2.0;

            double hPrime;
            if (C1Prime * C2Prime == 0.0)
            {
                hPrime = h1Prime + h2Prime;
            }
            else
            {
                hPrime = (Math.Abs(h2Prime - h1Prime) <= Math.PI) ? (h1Prime + h2Prime) / 2.0 : (h1Prime + h2Prime + 2.0 * Math.PI) / 2.0;
            }

            double T = 1.0 - 0.17 * Math.Cos(hPrime - Math.PI / 6.0) + 0.24 * Math.Cos(2.0 * hPrime) + 0.32 * Math.Cos(3.0 * hPrime + Math.PI / 30.0) - 0.20 * Math.Cos(4.0 * hPrime - 63.0 * Math.PI / 180.0);

            double deltaTheta = (30.0 * Math.PI / 180.0) * Math.Exp(-Math.Pow((hPrime - 275.0 * Math.PI / 180.0) / (25.0 * Math.PI / 180.0), 2.0));

            double R_C = 2.0 * Math.Sqrt(Math.Pow(CPrime, 7.0) / (Math.Pow(CPrime, 7.0) + Math.Pow(25.0, 7.0)));

            double S_C = 1.0 + 0.045 * CPrime;

            double S_H = 1.0 + 0.015 * CPrime * T;

            double deltaThetaDiv = deltaTheta / (Math.PI / 6.0);

            double R_T = -Math.Sin(2.0 * deltaThetaDiv) * R_C;

            double deltaE2000 = Math.Sqrt(Math.Pow(deltaLPrime / (kL * 1.0), 2.0) + Math.Pow(deltaCPrime / (kC * S_C), 2.0) + Math.Pow(deltaHPrime / (kH * S_H), 2.0) + R_T * (deltaCPrime / (kC * S_C)) * (deltaHPrime / (kH * S_H)));

            return deltaE2000;
        }
    }
    
    
}