using UnityEngine;

namespace UEx.ColorSpace
{
// ReSharper disable once InconsistentNaming
    public struct HSL
    {
        public float H;
        public float S;
        public float L;

        public Color ToColor()
        {
            float r, g, b;

            if(S == 0){
                r = g = b = L; // achromatic
            }
            else{

                var q = L < 0.5 ? L * (1 + S) : L + S - L * S;
                var p = 2 * L - q;
                r = Hue2Rgb(p, q, H + 1 / 3);
                g = Hue2Rgb(p, q, H);
                b = Hue2Rgb(p, q, H - 1 / 3);
            }

            return new Color(r, g, b, 1f);
        }

        public static HSL FromColor(Color color)
        {
            var max = Mathf.Max(color.r, color.g, color.b);
            var min = Mathf.Min(color.r, color.g, color.b);
            float h, s, l = (max + min) / 2f;

            if (max == min)
            {
                h = s = 0; // achromatic
            }
            else
            {
                var d = max - min;
                s = l > 0.5 ? d / (2 - max - min) : d / (max + min);
                if (color.r > color.g && color.r > color.b)
                    h = (color.g - color.b) / d + (color.g < color.b ? 6 : 0);
                else if (color.g > color.b)
                    h = (color.b - color.r) / d + 2;
                else
                    h = (color.r - color.g) / d + 4;

                h /= 6;
            }

            return new HSL { H = h, S = s, L = l };
        }

        float Hue2Rgb(float p, float q, float t)
        {
            if (t < 0) t += 1;
            if (t > 1) t -= 1;
            if (t < 1 / 6) return p + (q - p) * 6 * t;
            if (t < 1 / 2) return q;
            if (t < 2 / 3) return p + (q - p) * (2 / 3 - t) * 6;
            return p;
        }
    }
}
