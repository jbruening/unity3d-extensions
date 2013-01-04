using UnityEngine;

namespace UEx
{
    public static class Vector3X
    {
        /// <summary>
        /// gets the square distance between two vector3 positions. this is much faster that Vector3.distance.
        /// </summary>
        /// <param name="first">first point</param>
        /// <param name="second">second point</param>
        /// <returns>squared distance</returns>
        public static float SqrDistance(this Vector3 first, Vector3 second)
        {
            return Vector3.SqrMagnitude(first - second);
        }

        public static Vector3 MidPoint(this Vector3 first, Vector3 second)
        {
            return new Vector3((first.x + second.x)*0.5f, (first.y + second.y)*0.5f, (first.z + second.z)*0.5f);
        }

        /// <summary>
        /// get the square distance from a point to a line segment.
        /// </summary>
        /// <param name="point">point to get distance to</param>
        /// <param name="lineP1">line segment start point</param>
        /// <param name="lineP2">line segment end point</param>
        /// <param name="closestPoint">set to either 1, 2, or 4, determining which end the point is closest to (p1, p2, or the middle)</param>
        /// <returns></returns>
        public static float SqrLineDistance(this Vector3 point, Vector3 lineP1, Vector3 lineP2, out int closestPoint)
        {

            Vector3 v = lineP2 - lineP1;
            Vector3 w = point - lineP1;

            float c1 = Vector3.Dot(w, v);

            if (c1 <= 0) //closest point is p1
            {
                closestPoint = 1;
                return SqrDistance(point, lineP1);
            }

            float c2 = Vector3.Dot(v, v);
            if (c2 <= c1) //closest point is p2
            {
                closestPoint = 2;
                return SqrDistance(point, lineP2);
            }


            float b = c1/c2;

            Vector3 pb = lineP1 + b*v;
            {
                closestPoint = 4;
                return SqrDistance(point, pb);
            }
        }

        /// <summary>
        /// Absolute value of components
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public static Vector3 Abs(this Vector3 v)
        {
            return new Vector3(Mathf.Abs(v.x), Mathf.Abs(v.y), Mathf.Abs(v.z));
        }

        /// <summary>
        /// Vector3.Project, onto a plane
        /// </summary>
        /// <param name="v"></param>
        /// <param name="planeNormal"></param>
        /// <returns></returns>
        public static Vector3 ProjectOntoPlane(this Vector3 v, Vector3 planeNormal)
        {
            return v - Vector3.Project(v, planeNormal);
        }

        /// <summary>
        /// Gets the normal of the triangle formed by the 3 vectors
        /// </summary>
        /// <param name="vec1"></param>
        /// <param name="vec2"></param>
        /// <param name="vec3"></param>
        /// <returns></returns>
        public static Vector3 Vector3Normal(Vector3 vec1, Vector3 vec2, Vector3 vec3)
        {
            return Vector3.Cross((vec3 - vec1), (vec2 - vec1));
        }

        /// <summary>
        /// Using the magic of 0x5f3759df
        /// </summary>
        /// <param name="vec1"></param>
        /// <returns></returns>
        public static Vector3 FastNormalized(this Vector3 vec1)
        {
            var componentMult = MathX.FastInvSqrt(vec1.sqrMagnitude);
            return new Vector3(vec1.x*componentMult, vec1.y*componentMult, vec1.z*componentMult);
        }

        /// <summary>
        /// Gets the center of two points
        /// </summary>
        /// <param name="vec1"></param>
        /// <param name="vec2"></param>
        /// <returns></returns>
        public static Vector3 Center(Vector3 vec1, Vector3 vec2)
        {
            return new Vector3((vec1.x + vec2.x)/2, (vec1.y + vec2.y)/2, (vec1.z + vec2.z)/2);
        }

        public static bool IsNaN(this Vector3 vec)
        {
            return float.IsNaN(vec.x*vec.y*vec.z);
        }

        public static Vector3 Center(this Vector3[] points)
        {
            Vector3 ret = Vector3.zero;
            foreach (var p in points)
            {
                ret += p;
            }
            ret /= points.Length;
            return ret;
        }

        public static float AngleAroundAxis(Vector3 dir1, Vector3 dir2, Vector3 axis)
        {
            dir1 = dir1 - Vector3.Project(dir1, axis);
            dir2 = dir2 - Vector3.Project(dir2, axis);

            float angle = Vector3.Angle(dir1, dir2);
            return angle*(Vector3.Dot(axis, Vector3.Cross(dir1, dir2)) < 0 ? -1 : 1);
        }

        /// <summary>
        /// Returns a random direction in a cone. a spread of 0 is straight, 0.5 is 180*
        /// </summary>
        /// <param name="spread"></param>
        /// <param name="forward"></param>
        /// <returns></returns>
        public static Vector3 RandomDirection(float spread, Vector3 forward)
        {
            return Vector3.Slerp(forward, UnityEngine.Random.onUnitSphere, spread);
        }
        // test if a Vector3 is close to another Vector3 (due to floating point inprecision)
        // compares the square of the distance to the square of the range as this
        // avoids calculating a square root which is much slower than squaring the range
        public static bool Approx(Vector3 val, Vector3 about, float range)
        {
            return ((val - about).sqrMagnitude < range * range);
        }

        public static Vector3 NearestPoint(Vector3 lineStart, Vector3 lineEnd, Vector3 point)
        {
            Vector3 lineDirection = Vector3.Normalize(lineEnd - lineStart);
            float closestPoint = Vector3.Dot((point - lineStart), lineDirection) / Vector3.Dot(lineDirection, lineDirection);
            return lineStart + (closestPoint * lineDirection);
        }

        public static Vector3 NearestPointStrict(Vector3 lineStart, Vector3 lineEnd, Vector3 point)
        {
            Vector3 fullDirection = lineEnd - lineStart;
            Vector3 lineDirection = Vector3.Normalize(fullDirection);
            float closestPoint = Vector3.Dot((point - lineStart), lineDirection) / Vector3.Dot(lineDirection, lineDirection);
            return lineStart + (Mathf.Clamp(closestPoint, 0.0f, Vector3.Magnitude(fullDirection)) * lineDirection);
        }

        /// <summary>
        /// Direct speedup of <seealso cref="Vector3.Lerp"/>
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Vector3 Lerp(Vector3 v1, Vector3 v2, float value)
        {
            if (value > 1.0f)
                return v2;
            if (value < 0.0f)
                return v1;
            return new Vector3(v1.x + (v2.x - v1.x) * value,
                               v1.y + (v2.y - v1.y) * value,
                               v1.z + (v2.z - v1.z) * value);
        }
    }
}