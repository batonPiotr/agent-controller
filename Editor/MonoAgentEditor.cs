using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using HandcraftedGames.AgentController.Abilities;
using HandcraftedGames.AgentController.Abilities.Animator;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace HandcraftedGames.AgentController
{
    [CustomEditor(typeof(MonoAgent))]
    public class MonoAgentEditor: Editor
    {
        MonoAgent targetAgent;
        SerializedProperty abilities;
        List<IAbility> abilitiesList;

        ReorderableList List;

        IEnumerable<Type> typesWithAbilityAttribute;
        
        void OnEnable()
        {
            targetAgent = target as MonoAgent;
            abilities = serializedObject.FindProperty("abilities");
            abilitiesList = abilities.GetValue() as List<IAbility>;
            typesWithAbilityAttribute = GetTypesWithHelpAttribute(Assembly.GetAssembly(typeof(IAbility)));

            List = new ReorderableList(serializedObject, abilities, true, true, true, true);
            List.drawElementCallback = DrawListItems;
            List.drawHeaderCallback = DrawHeader;
            List.elementHeightCallback = ElementHeight;
            List.onAddDropdownCallback = onAddDropDownCallback;
        }

        static IEnumerable<Type> GetTypesWithHelpAttribute(Assembly assembly)
        {
            foreach(Type type in assembly.GetTypes())
            {
                if (type.GetCustomAttributes(typeof(AbilityAttribute), true).Length > 0)
                {
                    yield return type;
                }
            }
        }

        float ElementHeight(int index)
        {
            return EditorGUI.GetPropertyHeight(abilities.GetArrayElementAtIndex(index));
        }

        void onAddDropDownCallback(Rect buttonRect, ReorderableList list)
        {
            var menu = new GenericMenu();

            foreach(var t in typesWithAbilityAttribute)
            {
                var attribute = t.GetCustomAttribute(typeof(AbilityAttribute)) as AbilityAttribute;

                if(attribute == null)
                    continue;

                if(abilitiesList.Any(i => i.GetType() == t))
                    continue;

                menu.AddItem(new GUIContent(attribute.EditorItemName), false, () => {
                    
                    var instance = Activator.CreateInstance(t) as IAbility;
                    var l = abilitiesList;
                    l.Add(instance);
                    list.serializedProperty.SetValue(l);
                });
            }

            menu.ShowAsContext ();
        }

        // Draws the elements on the list
        void DrawListItems(Rect rect, int index, bool isActive, bool isFocused)
        {
            var indent = 15.0f;
            rect.x += indent;
            rect.width -= indent;

            EditorGUI.PropertyField(rect, abilities.GetArrayElementAtIndex(index));
        }

        //Draws the header
        void DrawHeader(Rect rect)
        {
            string name = "Abilities";
            EditorGUI.LabelField(rect, name);
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            List.DoLayoutList();
            var typesCount = new Dictionary<Type, int>();
            foreach(var ability in abilitiesList)
            {
                if(!typesCount.ContainsKey(ability.GetType()))
                    typesCount[ability.GetType()] = 0;
                typesCount[ability.GetType()] += 1;
            }
            foreach(var ability in typesCount)
            {
                if(ability.Value > 1)
                {
                    var foundInstance = abilitiesList.Find(a => a.GetType() == ability.Key);
                    EditorGUILayout.HelpBox("There are duplicates of " + foundInstance.Name, MessageType.Error);
                }
            }
            serializedObject.ApplyModifiedProperties();
        }
    }
}