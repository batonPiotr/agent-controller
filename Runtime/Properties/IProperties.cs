using System;

namespace HandcraftedGames.AgentController.Properties
{
    [System.AttributeUsage(System.AttributeTargets.Class)]
    public class PropertiesAttribute: Attribute
    {
        public string EditorItemName;
        public PropertiesAttribute(string editorItemName)
        {
            EditorItemName = editorItemName;
        }
    }
    public interface IProperties
    {
        string Name { get; }
    }
}