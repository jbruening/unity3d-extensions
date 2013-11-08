using System;
using System.Linq;
using UnityEngine;

namespace UEx
{
    public static class GameObjectX
    {
        //recursive calls
        private static void InternalMoveToLayer(Transform root, int layer)
        {
            root.gameObject.layer = layer;
            foreach (Transform child in root)
                InternalMoveToLayer(child, layer);
        }

        /// <summary>
        /// Move root and all children to the specified layer
        /// </summary>
        /// <param name="root"></param>
        /// <param name="layer"></param>
        public static void MoveToLayer(this GameObject root, int layer)
        {
            InternalMoveToLayer(root.transform, layer);
        }

        /// <summary>
        /// is the object's layer in the specified layermask
        /// </summary>
        /// <param name="gameObject"></param>
        /// <param name="mask"></param>
        /// <returns></returns>
        public static bool IsInLayerMask(this GameObject gameObject, LayerMask mask)
        {
            return ((mask.value & (1 << gameObject.layer)) > 0);
        }

        /// <summary>
        /// Returns all monobehaviours that are of type T, as T. Works for interfaces
        /// </summary>
        /// <typeparam name="T">class type</typeparam>
        /// <param name="gObj"></param>
        /// <returns></returns>
        public static T[] GetClasses<T>(this GameObject gObj) where T : class
        {
            var ts = gObj.GetComponents(typeof (T));

            var ret = new T[ts.Length];
            for (int i = 0; i < ts.Length; i++)
            {
                ret[i] = ts[i] as T;
            }
            return ret;
        }

        /// <summary>
        /// Returns all classes of type T (casted to T)
        /// works with interfaces
        /// </summary>
        /// <typeparam name="T">interface type</typeparam>
        /// <param name="gObj"></param>
        /// <returns></returns>
        public static T[] GetClasses<T>(this Transform gObj) where T : class
        {
            return gObj.gameObject.GetClasses<T>();
        }

        /// <summary>
        /// Returns the first monobehaviour that is of the class Type, as T
        /// works with interfaces
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="gObj"></param>
        /// <returns></returns>
        public static T GetClass<T>(this GameObject gObj) where T : class
        {
            return gObj.GetComponent(typeof (T)) as T;
        }

        /// <summary>
        /// Gets all monobehaviours in children that implement the class of type T (casted to T)
        /// works with interfaces
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="gObj"></param>
        /// <returns></returns>
        public static T[] GetClassesInChildren<T>(this GameObject gObj) where T : class
        {
            var ts = gObj.GetComponentsInChildren(typeof(T));

            var ret = new T[ts.Length];
            for (int i = 0; i < ts.Length; i++)
            {
                ret[i] = ts[i] as T;
            }
            return ret;
        }

        /// <summary>
        /// 
        /// Returns the first instance of the monobehaviour that is of the class type T (casted to T)
        /// works with interfaces
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="gObj"></param>
        /// <returns></returns>
        public static T GetClassInChildren<T>(this GameObject gObj) where T : class
        {
            return gObj.GetComponentInChildren(typeof (T)) as T;
        }
    }
}