using UnityEngine;

namespace UEx
{
    /// <summary>
    /// Transform extensions
    /// </summary>
    public static class TransformX
    {
        /// <summary>
        /// Deep search the heirarchy of the specified transform for the name. Uses width-first search.
        /// </summary>
        /// <param name="t"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        static public Transform DeepSearch(this Transform t, string name)
        {
            Transform dt = t.Find(name);
            if (dt != null)
            {
                return dt;
            }
            
            foreach (Transform child in t)
            {
                dt = child.DeepSearch(name);
                if (dt != null)
                    return dt;
            }
            return null;
        }

        /// <summary>
        /// opposite of up
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static Vector3 down(this Transform t)
        {
            return -t.up;
        }

        /// <summary>
        /// opposite of right
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static Vector3 left(this Transform t)
        {
            return -t.right;
        }

        /// <summary>
        /// opposite of forward
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static Vector3 backward(this Transform t)
        {
            return -t.forward;
        }
    }
}
