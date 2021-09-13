using System;
using System.Collections.Generic;
using System.Reflection;
using HandcraftedGames.AgentController;
using HandcraftedGames.AgentController.Abilities;
using HandcraftedGames.AgentController.Abilities.Animator;
using HandcraftedGames.Utils;
using UnityEditor;
using UnityEngine;

namespace HandcraftedGames.AgentController.Abilities
{

    [CustomPropertyDrawer(typeof(Ability), true)]
    public class BaseAbilityDrawer : PropertyDrawer
    {
        private static Dictionary<IAbility, bool> foldout = new Dictionary<IAbility, bool>();

        [UnityEditor.Callbacks.DidReloadScripts]
        private static void OnReload()
        {
            // foldout = new Dictionary<IAbility, bool>();
        }

        protected IAbility AbilityFor(SerializedProperty property)
        {
            return property.GetValue() as IAbility;
        }

        protected bool ShouldFoldout(IAbility ability)
        {
            if(!foldout.ContainsKey(ability))
                foldout[ability] = false;
            return foldout[ability];
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var height = EditorGUIUtility.singleLineHeight * 1;
            if(ShouldFoldout(AbilityFor(property)))
            {
                if(Application.isPlaying)
                    height += EditorGUIUtility.singleLineHeight * 1;

                height += GetAbilityHeight(property, label);
            }
            return height;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var ability = AbilityFor(property);

            var shouldFoldout = ShouldFoldout(ability);

            var enabledProperty = property.FindPropertyRelative("_Enabled");

            if(enabledProperty != null)
            {
                var togglePosition = RectLayout.VerticalRect(position, 0);
                togglePosition.x += position.width - 22.0f;
                var value = ability.Enabled;
                var newValue = EditorGUI.Toggle(togglePosition, enabledProperty.boolValue);
                if(newValue != enabledProperty.boolValue)
                {
                    if(newValue)
                        ability.Enable();
                    else
                        ability.Disable();
                    enabledProperty.boolValue = ability.Enabled;
                }
            }
            
            foldout[ability] = EditorGUI.Foldout(RectLayout.VerticalRect(position, 0), shouldFoldout, ability.Name);


            if(shouldFoldout)
            {
                var contentRect = position;
                contentRect.x += 12.0f;
                contentRect.width -= 12.0f;

                if(Application.isPlaying)
                {
                    EditorGUI.BeginDisabledGroup(true);
                    EditorGUI.Toggle(RectLayout.VerticalRect(contentRect, 1), new GUIContent("Active: "), ability.IsActive);
                    EditorGUI.EndDisabledGroup();
                }

                var additionalAbilityContent = contentRect;
                additionalAbilityContent.y += EditorGUIUtility.singleLineHeight * 1;
                additionalAbilityContent.height -= EditorGUIUtility.singleLineHeight * 1;

                if(Application.isPlaying)
                {
                    additionalAbilityContent.y += EditorGUIUtility.singleLineHeight * 1;
                    additionalAbilityContent.height -= EditorGUIUtility.singleLineHeight * 1;
                }

                DrawAbilityGUI(additionalAbilityContent, property, label);
            }
        }

        protected virtual float GetAbilityHeight(SerializedProperty property, GUIContent label)
        {
            return 0.0f;
        }

        protected virtual Rect DrawAbilityGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            return position;
        }
    }

    [CustomPropertyDrawer(typeof(IChangeSpeedAbility), true)]
    public class ChangeSpeedAbilityPropertyDrawer: BaseAbilityDrawer
    {
        protected override float GetAbilityHeight(SerializedProperty property, GUIContent label)
        {
            return base.GetAbilityHeight(property, label) + EditorGUIUtility.singleLineHeight * 1;
        }

        protected override Rect DrawAbilityGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var adjustedPosition = base.DrawAbilityGUI(position, property, label);
            var ability = AbilityFor(property) as ChangeSpeed;

            EditorGUI.LabelField(RectLayout.VerticalRect(adjustedPosition, 0), "T11: " + property.type + ", " + ability.Name);

            var unusedArea = adjustedPosition;
            unusedArea.y = RectLayout.VerticalRect(adjustedPosition, 0).yMax;
            unusedArea.height -= RectLayout.VerticalRect(adjustedPosition, 0).height;
            return unusedArea;
        }
    }


    [CustomPropertyDrawer(typeof(ChangeSpeed), true)]
    public class ChangeSpeedAbilityDrawer2 : ChangeSpeedAbilityPropertyDrawer
    {
        protected override float GetAbilityHeight(SerializedProperty property, GUIContent label)
        {
            return base.GetAbilityHeight(property, label) + EditorGUIUtility.singleLineHeight * 1;
        }

        protected override Rect DrawAbilityGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var adjustedPosition = base.DrawAbilityGUI(position, property, label);
            var ability = AbilityFor(property) as ChangeSpeed;


            EditorGUI.LabelField(RectLayout.VerticalRect(adjustedPosition, 0), "T1213: " + property.type + ", " + ability.Name);

            var unusedArea = adjustedPosition;
            unusedArea.y = RectLayout.VerticalRect(adjustedPosition, 0).yMax;
            unusedArea.height -= RectLayout.VerticalRect(adjustedPosition, 0).height;
            return unusedArea;
        }
    }
}