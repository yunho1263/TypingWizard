using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace DialogueSystem.Windows
{
    using Utilities;

    public class DialogueEditorWindow : EditorWindow
    {
        [MenuItem("Window/DialogueSystem/Dialogue Graph")]
        public static void Open()
        {
            GetWindow<DialogueEditorWindow>("Dialogue Graph");
        }

        private void OnEnable()
        {
            ConstructGraphView();
            AddStyle();

            GenerateToolbar();
        }

        private void ConstructGraphView()
        {
            var graphView = new D_GraphView(this);
            graphView.StretchToParentSize();
            rootVisualElement.Add(graphView);
        }

        private void AddStyle()
        {
            rootVisualElement.AddStyleSheets("DialogueSystem/D_Variables.uss");
        }

        private void GenerateToolbar()
        {

        }
    }
}