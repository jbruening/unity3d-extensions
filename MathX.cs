using System.Runtime.InteropServices;
using UnityEngine;

namespace UEx
{
    /// <summary>
    /// Most of these are speedups of builtin Lerps, as well as a bunch of extra lerp options
    /// </summary>
    public static class MathX
    {
        public static float Lerp(float from, float to, float value)
        {
            if (value < 0.0f)
                return from;
            if (value > 1.0f)
                return to;
            return (to - from) * value + from;
        }

        public static float LerpUnclamped(float from, float to, float value)
        {
            return (1.0f - value) * from + value * to;
        }

        public static float InverseLerp(float from, float to, float value)
        {
            if (from < to)
            {
                if (value < from)
                    return 0.0f;
                if (value > to)
                    return 1.0f;
            }
            else
            {
                if (value < to)
                    return 1.0f;
                if (value > @from)
                    return 0.0f;
            }
            return (value - from) / (to - from);
        }

        public static float InverseLerpUnclamped(float from, float to, float value)
        {
            return (value - from) / (to - from);
        }

        public static float SmoothStep(float from, float to, float value)
        {
            if (value < 0.0f)
                return from;
            if (value > 1.0f)
                return to;
            value = value * value * (3.0f - 2.0f * value);
            return (1.0f - value) * from + value * to;
        }

        public static float SmoothStepUnclamped(float from, float to, float value)
        {
            value = value * value * (3.0f - 2.0f * value);
            return (1.0f - value) * from + value * to;
        }

        public static float SuperLerp(float from, float to, float from2, float to2, float value)
        {
            if (from2 < to2)
            {
                if (value < from2)
                    value = from2;
                else if (value > to2)
                    value = to2;
            }
            else
            {
                if (value < to2)
                    value = to2;
                else if (value > from2)
                    value = from2;
            }
            return (to - from) * ((value - from2) / (to2 - from2)) + from;
        }

        public static float SuperLerpUnclamped(float from, float to, float from2, float to2, float value)
        {
            return (to - from) * ((value - from2) / (to2 - from2)) + from;
        }

        public static float Hermite(float start, float end, float value)
        {
            return Mathf.Lerp(start, end, value * value * (3.0f - 2.0f * value));
        }

        public static float Sinerp(float start, float end, float value)
        {
            return Mathf.Lerp(start, end, Mathf.Sin(value * Mathf.PI * 0.5f));
        }

        public static float Coserp(float start, float end, float value)
        {
            return Mathf.Lerp(start, end, 1.0f - Mathf.Cos(value * Mathf.PI * 0.5f));
        }

        public static float Berp(float start, float end, float value)
        {
            value = Mathf.Clamp01(value);
            value = (Mathf.Sin(value * Mathf.PI * (0.2f + 2.5f * value * value * value)) * Mathf.Pow(1f - value, 2.2f) + value) * (1f + (1.2f * (1f - value)));
            return start + (end - start) * value;
        }

        public static float Bounce(float x)
        {
            return Mathf.Abs(Mathf.Sin(6.28f * (x + 1f) * (x + 1f)) * (1f - x));
        }

        // test for value that is near specified float (due to floating point inprecision)
        // all thanks to Opless for this!
        public static bool Approx(float val, float about, float range)
        {
            return ((Mathf.Abs(val - about) < range));
        }

        /*
          * CLerp - Circular Lerp - is like lerp but handles the wraparound from 0 to 360.
          * This is useful when interpolating eulerAngles and the object
          * crosses the 0/360 boundary.  The standard Lerp function causes the object
          * to rotate in the wrong direction and looks stupid. Clerp fixes that.
          */
        public static float Clerp(float start, float end, float value)
        {
            const float min = 0.0f;
            const float max = 360.0f;
            float half = Mathf.Abs((max - min) / 2.0f);//half the distance between min and max
            float retval;
            float diff;

            if ((end - start) < -half)
            {
                diff = ((max - start) + end) * value;
                retval = start + diff;
            }
            else if ((end - start) > half)
            {
                diff = -((max - end) + start) * value;
                retval = start + diff;
            }
            else retval = start + (end - start) * value;

            // Debug.Log("Start: "  + start + "   End: " + end + "  Value: " + value + "  Half: " + half + "  Diff: " + diff + "  Retval: " + retval);
            return retval;
        }

        public static float GaussFalloff(float distance, float inRadius)
        {
            return Mathf.Clamp01(Mathf.Pow(360f, -Mathf.Pow(distance / inRadius, 2.5f) - 0.01f));
        }

        public static float FastInvSqrt(float x)
        {
            float xhalf = 0.5f * x;
            var bitHack = new FourBytes { Float = x };
            int i = bitHack.Int;
            i = 0x5f3759df - (i >> 1);  // da magicks
            bitHack.Int = i;
            x = bitHack.Float;
            x = x * (1.5f - (xhalf * x * x)); //newtons method to improve approximation
            return x;
        }

        public static float FastSqrt(float x)
        {
            float xhalf = 0.5f * x;
            var bitHack = new FourBytes { Float = x };
            int i = bitHack.Int;
            i = 0x1fbd1df5 + (i >> 1);  // da magicks
            bitHack.Int = i;
            x = bitHack.Float;
            x = x * (1.5f - (xhalf * x * x)); //newtons method to improve approximation
            return x;
        }

        [StructLayout(LayoutKind.Explicit)]
        private struct FourBytes
        {
            [FieldOffset(0)]
            public float Float;
            [FieldOffset(0)]
            public int Int;
        }

        public static string ConvertSeconds(int time)
        {
            var _minutes = (int)(time / 60);
            string minutes = "";
            int _seconds = (time % 60);
            string seconds = "";
            if (_minutes > 0) minutes = _minutes + " minute" + (_minutes > 1 ? "s " : " ");
            if (_seconds > 0) seconds = _seconds + " second" + (_seconds > 1 ? "s " : " ");
            return (minutes + seconds).Substring(0, (minutes + seconds).Length - 1);
        }
    }
}
