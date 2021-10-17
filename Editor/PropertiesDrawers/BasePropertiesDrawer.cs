
using System.Collections.Generic;
using HandcraftedGames.AgentController.Abilities;
using HandcraftedGames.Utils;
using UnityEditor;
using UnityEngine;

namespace HandcraftedGames.AgentController.Properties
{
    public class BasePropertiesDrawer : PropertyDrawer
    {
        private static Dictionary<IProperties, bool> foldout = new Dictionary<IProperties, bool>();

        [UnityEditor.Callbacks.DidReloadScripts]
        private static void OnReload()
        {
            // foldout = new Dictionary<IAbility, bool>();
        }

        protected IProperties PropertiesFor(SerializedProperty property)
        {
            var retVal = property.GetValue() as IProperties;
            return retVal;
        }

        protected bool ShouldFoldout(IProperties properties)
        {
            if(properties == null)
                return false;
            if(!foldout.ContainsKey(properties))
                foldout[properties] = false;
            return foldout[properties];
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var height = EditorGUIUtility.singleLineHeight * 1;
            if(ShouldFoldout(PropertiesFor(property)))
            {
                if(Application.isPlaying)
                    height += EditorGUIUtility.singleLineHeight * 1;

                height += GetPropertiesHeight(property, label);
            }
            return height;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var ability = PropertiesFor(property);
            if(ability == null)
                return;

            var shouldFoldout = ShouldFoldout(ability);
            
            foldout[ability] = EditorGUI.Foldout(RectLayout.VerticalRect(position, 0), shouldFoldout, ability.Name);


            if(shouldFoldout)
            {
                var contentRect = position;
                contentRect.x += 12.0f;
                contentRect.width -= 12.0f;

                var additionalAbilityContent = contentRect;
                additionalAbilityContent.y += EditorGUIUtility.singleLineHeight * 1;
                additionalAbilityContent.height -= EditorGUIUtility.singleLineHeight * 1;

                if(Application.isPlaying)
                {
                    additionalAbilityContent.y += EditorGUIUtility.singleLineHeight * 1;
                    additionalAbilityContent.height -= EditorGUIUtility.singleLineHeight * 1;
                }

                DrawPropertiesGUI(additionalAbilityContent, property, label);
            }
        }

        protected virtual float GetPropertiesHeight(SerializedProperty property, GUIContent label)
        {
            return 0.0f;
            // return EditorGUI.GetPropertyHeight(property);
        }

        protected virtual Rect DrawPropertiesGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // EditorGUI.PropertyField(position, property);
            return position;
        }
    }

    [CustomPropertyDrawer(typeof(IProperties), true)]
    public class GenericPropertiesDrawer: BasePropertiesDrawer
    {
        protected override float GetPropertiesHeight(SerializedProperty property, GUIContent label)
        {
            var copy = property.Copy();
            var count = 0;
            foreach(var c in copy)
            {
                var p = c as SerializedProperty;
                if(p == null)
                    continue;
                count++;
            }

            return base.GetPropertiesHeight(property, label) + EditorGUIUtility.singleLineHeight * count;
        }

        protected override Rect DrawPropertiesGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var adjustedPosition = base.DrawPropertiesGUI(position, property, label);
            var index = 0;
            foreach(var subProp in property)
            {
                var p = subProp as SerializedProperty;
                if(p == null)
                    continue;
                EditorGUI.PropertyField(RectLayout.VerticalRect(adjustedPosition, index), p);
                index++;
            }

            var unusedArea = adjustedPosition;
            unusedArea.y = RectLayout.VerticalRect(adjustedPosition, index).yMax;
            unusedArea.height -= RectLayout.VerticalRect(adjustedPosition, index).height;
            return unusedArea;
        }
    }
}