using UEx.ColorSpace;
using UnityEngine;

namespace UEx
{
    public static class ColorX
    {
        public static HSV GetHSV(this Color color)
        {
            return HSV.FromColor(color);
        }

        public static HSL ToHsl(this Color color)
        {
            return HSL.FromColor(color);
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