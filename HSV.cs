using UnityEngine;

namespace UEx
{
    // ReSharper disable InconsistentNaming
    public struct HSV
    // ReSharper restore InconsistentNaming
    {
        /// <summary>
        ///this is actually a 'degree'. ranges from 0 - 360 
        /// </summary>
        private float _hue;


        public float Hue
        {
            get
            {
                return this._hue;
            }
            set
            {
                _hue = Mathf.Clamp(value, 0, 360f);
            }
        }

        /// <summary>
        ///0-1. if 0, then h = -1 (undefined) 
        /// </summary>
        private float _saturation;


        public float Saturation
        {
            get
            {
                return this._saturation;
            }
            set
            {
                _saturation = Mathf.Clamp01(value);
            }
        }

        private float _value;

        public float Value
        {
            get
            {
                return this._value;
            }
            set
            {
                _value = Mathf.Clamp01(value);
            }
        }

        public Color GetColor()
        {
            float f, p, q, t, r, g, b;
            float h = _hue;
            float s = _saturation;
            float v = _value;

            if (s == 0)
            {
                // achromatic (grey)
                r = g = b = v;
                return new Color(r, g, b, 1f);
            }

            h /= 60f;			// sector 0 to 5
            int i = (int)h;
            f = h - i;			// factorial part of h
            p = v * (1f - s);
            q = v * (1f - s * f);
            t = v * (1f - s * (1f - f));

            switch (i)
            {
                case 0:
                    r = v;
                    g = t;
                    b = p;
                    break;
                case 1:
                    r = q;
                    g = v;
                    b = p;
                    break;
                case 2:
                    r = p;
                    g = v;
                    b = t;
                    break;
                case 3:
                    r = p;
                    g = q;
                    b = v;
                    break;
                case 4:
                    r = t;
                    g = p;
                    b = v;
                    break;
                default:		// case 5:
                    r = v;
                    g = p;
                    b = q;
                    break;
            }

            return new Color(r, g, b, 1f);
        }
    }
}