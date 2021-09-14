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
            return base.GetAbilityHeight(property, label) + EditorGUIUtility.singleLineHeight * 2;
        }

        protected override Rect DrawAbilityGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var adjustedPosition = base.DrawAbilityGUI(position, property, label);
            var ability = AbilityFor(property) as FollowAbility;

            int index = 0;

            var stopWhenReached = property.FindPropertyRelative("stopWhenReached");
            if(stopWhenReached != null)
                EditorGUI.PropertyField(RectLayout.VerticalRect(adjustedPosition, index++), stopWhenReached);
            var updateTargetPositionInterval = property.FindPropertyRelative("updateTargetPositionInterval");
            if(stopWhenReached != null)
                EditorGUI.PropertyField(RectLayout.VerticalRect(adjustedPosition, index++), updateTargetPositionInterval);

            var unusedArea = adjustedPosition;
            unusedArea.y = RectLayout.VerticalRect(adjustedPosition, index).yMax;
            unusedArea.height -= RectLayout.VerticalRect(adjustedPosition, index).height;
            return unusedArea;
        }
    }
}