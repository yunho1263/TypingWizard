using UnityEditor;
using UnityEngine.UIElements;

namespace DialogueSystem.Utilities
{
    public static class D_StyleUtilitie
    {
        public static VisualElement AddClasses(this VisualElement element, params string[] classes)
        {
            foreach (string Class in classes)
            {
                element.AddToClassList(Class);
            }

            return element;
        }

        public static VisualElement AddStyleSheets(this VisualElement element, params string[] paths)
        {
            foreach (string sheet in paths)
            {
                StyleSheet newSheet = EditorGUIUtility.Load(sheet) as StyleSheet;

                element.styleSheets.Add(newSheet);
            }

            return element;
        }
    }
}
