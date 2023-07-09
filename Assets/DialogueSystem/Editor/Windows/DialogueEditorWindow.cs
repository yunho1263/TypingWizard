using System.IO;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace DialogueSystem.Windows
{
    using System;
    using Utilities;

    public class DialogueEditorWindow : EditorWindow
    {
        private D_GraphView graphView;

        private readonly string defaultFileName = "NewDialogue";

        private static TextField fileNameTextField;
        private Button saveButton;
        private Button minimapButton;

        [MenuItem("Window/DialogueSystem/Dialogue Graph")]
        public static void Open()
        {
            GetWindow<DialogueEditorWindow>("Dialogue Graph");
        }

        private void OnEnable()
        {
            AddGraphView();
            AddToolbar();
            AddStyle();

        }

        #region Adding Elements / 엘리먼트 추가
        private void AddGraphView()
        {
            graphView = new D_GraphView(this);
            graphView.StretchToParentSize();
            rootVisualElement.Add(graphView);
        }

        private void AddToolbar()
        {
            Toolbar toolbar = new Toolbar();

            fileNameTextField = D_ElementUtilitie.CreateTextField(defaultFileName, "File Name : ", callback =>
            {
                fileNameTextField.value = callback.newValue.RemoveWhitespaces().RemoveSpecialCharacters();
            });

            saveButton = D_ElementUtilitie.CreateButton("Save", () => Save());

            Button loadButton = D_ElementUtilitie.CreateButton("Load", () => Load());
            Button clearButton = D_ElementUtilitie.CreateButton("Clear", () => Clear());
            Button resetButton = D_ElementUtilitie.CreateButton("Reset", () => ResetGraph());
            minimapButton = D_ElementUtilitie.CreateButton("Minimap", () => ToggleMinimap());

            toolbar.Add(fileNameTextField);
            toolbar.Add(saveButton);
            toolbar.Add(loadButton);
            toolbar.Add(clearButton);
            toolbar.Add(resetButton);
            toolbar.Add(minimapButton);

            toolbar.AddStyleSheets("DialogueSystem/D_Toolbar_Style.uss");

            rootVisualElement.Add(toolbar);
        }

        private void AddStyle()
        {
            rootVisualElement.AddStyleSheets("DialogueSystem/D_Variables.uss");
        }
        #endregion

        #region Toolbar Actions / 툴바 작업
        private void Save()
        {
            if (string.IsNullOrEmpty(fileNameTextField.value))
            {
                EditorUtility.DisplayDialog(
                    "Invalid file name.",
                    "Please enter a valid file name.",
                    "OK"
                    );

                return;
            }

            D_IO_Utility.Initialize(graphView, fileNameTextField.value);
            D_IO_Utility.Save();
        }

        private void Load()
        {
            string filePath = EditorUtility.OpenFilePanel("Load Dialogue Graph", "Assets/DialogueSystem/Editor/Graphs", "asset");

            if (string.IsNullOrEmpty(filePath))
            {
                return;
            }

            Clear();

            D_IO_Utility.Initialize(graphView, Path.GetFileNameWithoutExtension(filePath));
            D_IO_Utility.Load();
        }

        private void Clear()
        {
            graphView.ClearGraph();
        }

        private void ResetGraph()
        {
            Clear();
            UpdateFileName(defaultFileName);
        }

        private void ToggleMinimap()
        {
            graphView.ToggleMinimap();

            minimapButton.ToggleInClassList("ds-Toolbar__button__selected");
        }
        #endregion

        #region Utility Methods / 유틸리티 메소드
        public static void UpdateFileName(string newFileName)
        {
            fileNameTextField.value = newFileName;
        }

        public void EnableSaveButton()
        {
            saveButton.SetEnabled(true);
        }

        public void DisableSaveButton()
        {
            saveButton.SetEnabled(false);
        }
        #endregion
    }
}