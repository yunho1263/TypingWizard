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

        private TextField fileNameTextField;
        private Button saveButton;

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

            toolbar.Add(fileNameTextField);
            toolbar.Add(saveButton);

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
        #endregion

        #region Utility Methods / 유틸리티 메소드
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