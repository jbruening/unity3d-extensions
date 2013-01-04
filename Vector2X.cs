using UnityEngine;

namespace UEx
{
    public static class Vector2X
    {
        public static Vector2 NearestPointStrict(Vector2 lineStart, Vector2 lineEnd, Vector2 point)
        {
            var fullDirection = lineEnd - lineStart;
            var lineDirection = fullDirection.normalized;
            var closestPoint = Vector2.Dot((point - lineStart), lineDirection) / Vector2.Dot(lineDirection, lineDirection);
            return lineStart + (Mathf.Clamp(closestPoint, 0, fullDirection.magnitude) * lineDirection);
        }

        /// <summary>
        /// Direct speedup of <seealso cref="Vector2.Lerp"/>
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Vector2 Lerp(Vector2 v1, Vector2 v2, float value)
        {
            if (value > 1.0f)
                return v2;
            if (value < 0.0f)
                return v1;
            return new Vector2(v1.x + (v2.x - v1.x) * value,
                                v1.y + (v2.y - v1.y) * value);
        }
    }
}
