using UnityEngine;

namespace UEx
{
// ReSharper disable InconsistentNaming
    /// <summary>
    /// Matrix4x4 extensions
    /// </summary>
    public static class Matrix4x4X
// ReSharper restore InconsistentNaming
    {
        public static Matrix4x4 RandomMatrix(float min = 0, float max = 1)
        {
            var ret = new Matrix4x4();
            for (int i = 0; i < 16; i++)

                ret[i] = UnityEngine.Random.Range(min, max);

            return ret;
        }

        public static Matrix4x4 Lerp(Matrix4x4 from, Matrix4x4 to, float time)
        {

            var ret = new Matrix4x4();

            for (int i = 0; i < 16; i++)

                ret[i] = Mathf.Lerp(from[i], to[i], time);

            return ret;

        }
    }
}
