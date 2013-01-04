using UnityEngine;

namespace UEx
{
    public static class ColorX
    {
        public static HSV GetHSV(this Color color)
        {
            float min, max, delta, v, s, h;

            min = Mathf.Min(color.r, color.g, color.b);
            max = Mathf.Max(color.r, color.g, color.b);
            v = max;				// v

            delta = max - min;

            if (max != 0)
                s = delta / max;		// s
            else
            {
                // r = g = b = 0		// s = 0, v is undefined
                s = 0;
                h = -1f;
                return new HSV() { Hue = h, Saturation = s, Value = v };
            }

            if (color.r == max)
                h = (color.g - color.b) / delta;		// between yellow & magenta
            else if (color.g == max)
                h = 2f + (color.b - color.r) / delta;	// between cyan & yellow
            else
                h = 4f + (color.r - color.g) / delta;	// between magenta & cyan

            h *= 60f;				// degrees
            if (h < 0)
                h += 360f;

            return new HSV() { Hue = h, Saturation = s, Value = v };
        }

        public static Color MakeRandomColor(this Color color, float minClamp = 0.5f)
        {
            var randCol = UnityEngine.Random.onUnitSphere * 3;
            randCol.x = Mathf.Clamp(randCol.x, minClamp, 1f);
            randCol.y = Mathf.Clamp(randCol.y, minClamp, 1f);
            randCol.z = Mathf.Clamp(randCol.z, minClamp, 1f);

            return new Color(randCol.x, randCol.y, randCol.z, 1f);
        }

        /// <summary>
        /// Direct speedup of <seealso cref="Color.Lerp"/>
        /// </summary>
        /// <param name="c1"></param>
        /// <param name="c2"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Color Lerp(Color c1, Color c2, float value)
        {
            if (value > 1.0f)
                return c2;
            if (value < 0.0f)
                return c1;
            return new Color(c1.r + (c2.r - c1.r) * value,
                             c1.g + (c2.g - c1.g) * value,
                             c1.b + (c2.b - c1.b) * value,
                             c1.a + (c2.a - c1.a) * value);
        }
    }
}