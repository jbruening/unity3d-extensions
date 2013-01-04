using UnityEngine;

namespace UEx
{
    public static class Vector4X
    {
        /// <summary>
        /// Direct speedup of <seealso cref="Vector4.Lerp"/>
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Vector4 Lerp(Vector4 v1, Vector4 v2, float value)
        {
            if (value > 1.0f)
                return v2;
            if (value < 0.0f)
                return v1;
            return new Vector4(v1.x + (v2.x - v1.x) * value,
                                v1.y + (v2.y - v1.y) * value,
                                v1.z + (v2.z - v1.z) * value,
                                v1.w + (v2.w - v1.w) * value);
        }
    }
}
