using System;
using UnityEngine;

namespace UEx
{
    /// <summary>
    /// overrides the UnityEngine.Resources class
    /// </summary>
    public static class Resources
    {
        /// <summary>
        /// Load the object of the specified type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <returns></returns>
        public static T Load<T>(string path) where T : UnityEngine.Object
        {
            return (T)UnityEngine.Resources.Load(path, typeof(T));
        }

        /// <summary>
        /// Loads the specified map information (file named 'map', using same path as SceneInfo method)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T LoadMapInfo<T>() where T : UnityEngine.Object
        {
            return Load<T>("SceneInfo/" + Application.loadedLevelName + "/map");
        }

        /// <summary>
        /// Load the specified map information, from the specified level name
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="levelName"></param>
        /// <returns></returns>
        public static T LoadMapInfo<T>(string levelName) where T: UnityEngine.Object
        {
            return Load<T>("SceneInfo/" + levelName + "/map");
        }

        /// <summary>
        /// Load the specified scene info object (loads from Resources\SceneInfo\<seealso cref="UnityEngine.Application.loadedLevelName">levelName</seealso> folder)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filename"></param>
        /// <param name="allowDefault"></param>
        /// <returns></returns>
        public static T LoadSceneInfo<T>(string filename, bool allowDefault = false) where T : UnityEngine.Object
        {
            var resource = Load<T>("SceneInfo/" + Application.loadedLevelName + "/" + filename);
            if (allowDefault && resource == null)
            {
                resource = Load<T>("SceneInfo/Default Info/" + filename);
            }
            return resource;
        }

        //because overriding causes things to not use the UnityEngine class
        #region Redirection

        /// <summary>
        /// Load all of the specified type at the path
        /// </summary>
        /// <param name="path"></param>
        /// <param name="systemTypeInstance"></param>
        /// <returns></returns>
        public static UnityEngine.Object[] LoadAll(string path, Type systemTypeInstance)
        {
            return UnityEngine.Resources.LoadAll(path, systemTypeInstance);
        }

        /// <summary>
        /// Unload the asset. Any remaining references will cause the asset to be reloaded as soon as it is accessed.
        /// </summary>
        /// <param name="assetToUnload"></param>
        public static void UnloadAsset(UnityEngine.Object assetToUnload)
        {
            UnityEngine.Resources.UnloadAsset(assetToUnload);
        }

        /// <summary>
        /// Unload all unused assets. Might take a while to do.
        /// </summary>
        public static void UnloadUnusedAssets()
        {
            UnityEngine.Resources.UnloadUnusedAssets();
        }

        /// <summary>
        /// load the specified type at the path
        /// </summary>
        /// <param name="path"></param>
        /// <param name="systemTypeInstance"></param>
        /// <returns></returns>
        public static UnityEngine.Object Load(string path, Type systemTypeInstance)
        {
            return UnityEngine.Resources.Load(path, systemTypeInstance);
        }

        /// <summary>
        /// load the <seealso cref="UnityEngine.Object" /> at the path
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static UnityEngine.Object Load(string path)
        {
            return UnityEngine.Resources.Load(path);
        }
        #endregion
    }
}