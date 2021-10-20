using UnityEditor;
using UnityEngine;

namespace HandcraftedGames.Utils
{
    public static class RectLayout
    {
        public static Rect VerticalRect(Rect overallRect, float rowHeight, int index)
        {
            var retVal = overallRect;
            retVal.height = rowHeight;
            retVal.y = overallRect.y + (rowHeight * index);
            return retVal;
        }
        public static Rect VerticalRect(Rect overallRect, int index)
        {
            return VerticalRect(overallRect, EditorGUI.GetPropertyHeight(SerializedPropertyType.String, new GUIContent("line")), index);
        }
    }
}