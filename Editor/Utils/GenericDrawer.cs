
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace HandcraftedGames.Utils
{
    public class GenericDrawer
    {
        public static GenericDrawer Shared = new GenericDrawer();
        public List<GenericElementDrawer> drawers = new List<GenericElementDrawer>();

        public void Register<T>(GenericElementDrawer<T> drawer)
        {
            drawers.Add(drawer);
        }

        public float GetPropertyHeight<T>(T item, GUIContent label)
        {
            var drawer = GetDrawer<T>();
            if(drawer == null)
            {
                throw new System.Exception("There is no drawer registered for item type: " + typeof(T));
            }
            return drawer.GetPropertyHeight(item, label);
        }

        /// <summary>
        /// Used for property drawers, where the area is limited.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="rect"></param>
        public void Draw<T>(T item, Rect rect)
        {
            var drawer = GetDrawer<T>();
            if(drawer == null)
            {
                throw new System.Exception("There is no drawer registered for item type: " + typeof(T));
            }
            drawer.Draw(item, rect);
        }

        /// <summary>
        /// Used for custom editor where it's possible to use EditorGUILayout.
        /// </summary>
        /// <param name="item"></param>
        public void Draw<T>(T item)
        {
            var drawer = GetDrawer<T>();
            if(drawer == null)
            {
                throw new System.Exception("There is no drawer registered for item type: " + typeof(T));
            }
            drawer.Draw(item);
        }

        private GenericElementDrawer<T> GetDrawer<T>()
        {
            foreach(var i in drawers)
            {
                var d = i as GenericElementDrawer<T>;
                if(d != null)
                    return d;
            }
            return null;
        }
    }

    public interface GenericElementDrawer {}

    public interface GenericElementDrawer<T>: GenericElementDrawer
    {
        float GetPropertyHeight(T item, GUIContent label);

        /// <summary>
        /// Used for property drawers, where the area is limited.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="rect"></param>
        void Draw(T item, Rect rect);

        /// <summary>
        /// Used for custom editor where it's possible to use EditorGUILayout.
        /// </summary>
        /// <param name="item"></param>
        void Draw(T item);
    }
}