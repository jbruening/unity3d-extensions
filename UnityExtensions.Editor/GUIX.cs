using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace UEx.Editor
{
    public static class GUIX
    {
        /// <summary>
        /// display an array similar to how unity does by default
        /// </summary>
        /// <remarks>
        /// Supports drag and drop on the foldout area, just like unity's builtin version does
        /// </remarks>
        /// <typeparam name="T"></typeparam>
        /// <param name="objects"></param>
        /// <param name="expanded">similar to Folout's expanded</param>
        /// <param name="label"></param>
        /// <param name="indicesAdded"></param>
        /// <param name="selectedIndexGainedReference"></param>
        /// <param name="showRemoveButtons"></param>
        /// <param name="allowSceneObjects"> </param>
        /// <returns>if the array is expanded or not</returns>
        public static bool ObjectFieldArray<T>(ref T[] objects, bool expanded, string label, Action<int[]> indicesAdded = null, Action selectedIndexGainedReference = null, bool showRemoveButtons = false, bool allowSceneObjects = true)
        where T : UnityEngine.Object
        {
            int selectedIndex = -1;
            return ObjectFieldArray(ref objects, expanded, label, ref selectedIndex, indicesAdded, selectedIndexGainedReference, showRemoveButtons, allowSceneObjects);
        }

        /// <summary>
        /// Display an array similar to how unity does by default
        /// </summary>
        /// <remarks>
        /// supports drag and drop on the foldout area, just like unity's builtin version does
        /// </remarks>
        /// <typeparam name="T"></typeparam>
        /// <param name="objects"></param>
        /// <param name="expanded">similar to Foldout's expanded</param>
        /// <param name="label"></param>
        /// <param name="selectedIndex"></param>
        /// <param name="indicesAdded"></param>
        /// <param name="selectedIndexGainedReference"></param>
        /// <param name="showRemoveButtons"></param>
        /// <param name="allowSceneObjects"> </param>
        /// <returns>if the array is expanded or not</returns>
        public static bool ObjectFieldArray<T>(ref T[] objects, bool expanded, string label, ref int selectedIndex, Action<int[]> indicesAdded = null, Action selectedIndexGainedReference = null, bool showRemoveButtons = false, bool allowSceneObjects = true)
            where T : UnityEngine.Object
        {
            var retExpand = EditorGUILayout.Foldout(expanded, label);
            if (retExpand)
            {
                EditorGUIUtility.LookLikeInspector();
                if (Event.current.type == EventType.repaint || Event.current.type == EventType.DragPerform)
                {
                    if (GUILayoutUtility.GetLastRect().Contains(Event.current.mousePosition))
                    {
                        if (Event.current.type == EventType.repaint)
                        {
                            var didFind = DragAndDrop.objectReferences.OfType<GameObject>().Any();
                            DragAndDrop.visualMode = didFind ? DragAndDropVisualMode.Copy : DragAndDropVisualMode.Rejected;
                        }

                        if (Event.current.type == EventType.dragPerform)
                        {
                            Event.current.Use();
                            var addedObjects = new List<int>(DragAndDrop.objectReferences.Length);
                            foreach (var objectReference in DragAndDrop.objectReferences)
                            {
                                var casted = objectReference as T;
                                if (casted != null)
                                {
                                    addedObjects.Add(objects.Length);
                                    ArrayUtility.Add(ref objects, casted);
                                }
                            }

                            if (indicesAdded != null) indicesAdded(addedObjects.ToArray());
                        }
                    }
                }
                EditorGUI.indentLevel++;
                EditorGUI.BeginChangeCheck();
                var nCount = EditorGUILayout.IntField("Size", objects.Length);
                EditorGUI.EndChangeCheck();
                if (GUI.changed)
                {
                    if (nCount > objects.Length)
                    {
                        //add
                        ArrayUtility.AddRange(ref objects, new T[nCount - objects.Length]);
                    }
                    else if (nCount < objects.Length)
                    {
                        //truncate
                        objects = objects.RemoveRange(nCount, objects.Length - nCount);
                    }
                }

                var removeind = -1;
                for (int i = 0; i < objects.Length; i++)
                {
                    EditorGUILayout.BeginHorizontal();

                    if (selectedIndex == i)
                    {
                        objects[i] =
                            EditorGUILayout.ObjectField("☒ Element " + i, objects[i], typeof(GameObject), allowSceneObjects) as T;

                        if (objects[i] != null)
                        {
                            if (selectedIndexGainedReference != null) selectedIndexGainedReference();
                        }
                    }
                    else
                        objects[i] =
                            EditorGUILayout.ObjectField("Element " + i, objects[i], typeof(GameObject), allowSceneObjects) as T;

                    if (Event.current.type == EventType.mouseUp &&
                        GUILayoutUtility.GetLastRect().Contains(Event.current.mousePosition))
                    {
                        selectedIndex = i;
                    }

                    if (showRemoveButtons)
                        if (GUILayout.Button("-", GUILayout.MaxWidth(15), GUILayout.MaxHeight(16))) removeind = i;

                    GUILayout.EndHorizontal();
                }

                if (removeind > -1)
                    ArrayUtility.RemoveAt(ref objects, removeind);
                EditorGUI.indentLevel--;
                EditorGUIUtility.LookLikeControls();
            }

            return retExpand;
        }
    }
}
