using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using HandcraftedGames.AgentController.Abilities;
using HandcraftedGames.AgentController.Abilities.Animator;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using HandcraftedGames.AgentController.Properties;

namespace HandcraftedGames.AgentController
{
    [CustomEditor(typeof(MonoAgent))]
    public class MonoAgentEditor: Editor
    {
        MonoAgent targetAgent;
        SerializedProperty abilities;
        List<IAbility> abilitiesList;
        SerializedProperty properties;
        List<IProperties> propertiesList;

        ReorderableList ReorderableAbilitiesList;
        ReorderableList ReorderablePropertiesList;

        IEnumerable<Type> typesWithAbilityAttribute;
        IEnumerable<Type> typesWithPropertiesAttribute;
        
        void OnEnable()
        {
            targetAgent = target as MonoAgent;

            abilities = serializedObject.FindProperty("abilities");
            abilitiesList = abilities.GetValue() as List<IAbility>;

            properties = serializedObject.FindProperty("properties");
            propertiesList = properties.GetValue() as List<IProperties>;

            typesWithAbilityAttribute = GetTypesWithHelpAttribute<AbilityAttribute>(Assembly.GetAssembly(typeof(IAbility)));
            typesWithPropertiesAttribute = GetTypesWithHelpAttribute<PropertiesAttribute>(Assembly.GetAssembly(typeof(IProperties)));

            ReorderableAbilitiesList = new ReorderableList(serializedObject, abilities, true, true, true, true);
            ReorderableAbilitiesList.drawElementCallback = AbilitiesDrawListItems;
            ReorderableAbilitiesList.drawHeaderCallback = AbilitiesDrawHeader;
            ReorderableAbilitiesList.elementHeightCallback = AbilitiesElementHeight;
            ReorderableAbilitiesList.onAddDropdownCallback = AbilitiesOnAddDropDownCallback;

            ReorderablePropertiesList = new ReorderableList(serializedObject, properties, true, true, true, true);
            ReorderablePropertiesList.drawElementCallback = PropertiesDrawListItems;
            ReorderablePropertiesList.drawHeaderCallback = PropertiesDrawHeader;
            ReorderablePropertiesList.elementHeightCallback = PropertiesElementHeight;
            ReorderablePropertiesList.onAddDropdownCallback = PropertiesOnAddDropDownCallback;
        }

        static IEnumerable<Type> GetTypesWithHelpAttribute<AttributeType>(Assembly assembly) where AttributeType: System.Attribute
        {
            foreach(Type type in assembly.GetTypes())
            {
                if (type.GetCustomAttributes(typeof(AttributeType), true).Length > 0)
                {
                    yield return type;
                }
            }
        }

        #region Abilities Reordable List

        float AbilitiesElementHeight(int index)
        {
            return EditorGUI.GetPropertyHeight(abilities.GetArrayElementAtIndex(index));
        }

        void AbilitiesOnAddDropDownCallback(Rect buttonRect, ReorderableList list)
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
        void AbilitiesDrawListItems(Rect rect, int index, bool isActive, bool isFocused)
        {
            var indent = 15.0f;
            rect.x += indent;
            rect.width -= indent;

            EditorGUI.PropertyField(rect, abilities.GetArrayElementAtIndex(index));
        }

        //Draws the header
        void AbilitiesDrawHeader(Rect rect)
        {
            string name = "Abilities";
            EditorGUI.LabelField(rect, name);
        }
            
        #endregion

        #region Properties Reordable List
            

        float PropertiesElementHeight(int index)
        {
            return EditorGUI.GetPropertyHeight(properties.GetArrayElementAtIndex(index));
        }

        void PropertiesOnAddDropDownCallback(Rect buttonRect, ReorderableList list)
        {
            var menu = new GenericMenu();

            foreach(var t in typesWithPropertiesAttribute)
            {
                var attribute = t.GetCustomAttribute(typeof(PropertiesAttribute)) as PropertiesAttribute;

                if(attribute == null)
                    continue;

                if(propertiesList.Any(i => i.GetType() == t))
                    continue;

                menu.AddItem(new GUIContent(attribute.EditorItemName), false, () => {
                    
                    var instance = Activator.CreateInstance(t) as IProperties;
                    var l = propertiesList;
                    l.Add(instance);
                    list.serializedProperty.SetValue(l);
                });
            }

            menu.ShowAsContext ();
        }

        // Draws the elements on the list
        void PropertiesDrawListItems(Rect rect, int index, bool isActive, bool isFocused)
        {
            var indent = 15.0f;
            rect.x += indent;
            rect.width -= indent;

            EditorGUI.PropertyField(rect, properties.GetArrayElementAtIndex(index));
        }

        //Draws the header
        void PropertiesDrawHeader(Rect rect)
        {
            string name = "Properties";
            EditorGUI.LabelField(rect, name);
        }
        #endregion
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            // EditorGUILayout.PropertyField(properties);

            ReorderableAbilitiesList.DoLayoutList();
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

            ReorderablePropertiesList.DoLayoutList();
            typesCount = new Dictionary<Type, int>();
            foreach(var property in propertiesList)
            {
                if(!typesCount.ContainsKey(property.GetType()))
                    typesCount[property.GetType()] = 0;
                typesCount[property.GetType()] += 1;
            }
            foreach(var property in typesCount)
            {
                if(property.Value > 1)
                {
                    var foundInstance = propertiesList.Find(a => a.GetType() == property.Key);
                    EditorGUILayout.HelpBox("There are duplicates of " + foundInstance.Name, MessageType.Error);
                }
            }
            serializedObject.ApplyModifiedProperties();
        }
    }
}