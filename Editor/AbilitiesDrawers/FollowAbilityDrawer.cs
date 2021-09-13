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
    [CustomPropertyDrawer(typeof(FollowAbility), true)]
    public class FollowAbilityPropertyDrawer : BaseAbilityDrawer
    {
        protected override float GetAbilityHeight(SerializedProperty property, GUIContent label)
        {
            return base.GetAbilityHeight(property, label) + EditorGUIUtility.singleLineHeight * 1;
        }

        protected override Rect DrawAbilityGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var adjustedPosition = base.DrawAbilityGUI(position, property, label);
            var ability = AbilityFor(property) as FollowAbility;

            var stopWhenReached = property.FindPropertyRelative("stopWhenReached");

            EditorGUI.PropertyField(RectLayout.VerticalRect(adjustedPosition, 0), stopWhenReached);

            // EditorGUI.LabelField(RectLayout.VerticalRect(adjustedPosition, 0), "T1213: " + property.type + ", " + ability.Name);

            var unusedArea = adjustedPosition;
            unusedArea.y = RectLayout.VerticalRect(adjustedPosition, 0).yMax;
            unusedArea.height -= RectLayout.VerticalRect(adjustedPosition, 0).height;
            return unusedArea;
        }
    }
}