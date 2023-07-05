using System;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace DialogueSystem.Elements
{
    using Windows;
    using Utilities;
    public enum DialogueNodeType
    {
        Single,
        Branch
    }

    [Serializable]
    public class Dialogue_Node : Node
    {
        public DialogueNodeType DialogueType { get; set; }

        //다이얼로그 이름
        public string DialogueName { get; set; }

        // 대화 내용
        public string Text { get; set; }
        
        // 분기
        public List<string> Branchs { get; set; }

        // 그룹
        public Group Group { get; set; }

        private D_GraphView graphView;
        private Color defalutBackgroundColor;

        public virtual void initialize(D_GraphView d_GraphView,  Vector2 pos)
        {
            DialogueName = "DialogueName";
            Branchs = new List<string>();
            Text = "sentences";

            graphView = d_GraphView;
            defalutBackgroundColor = new Color(29f/ 255f, 29f / 255f, 30f / 255f);

            SetPosition(new Rect(pos, Vector2.zero));

            mainContainer.AddToClassList("ds-node__main-container");
            extensionContainer.AddToClassList("ds-node__extension-container");
        }

        public virtual void Draw()
        {
            // 화자 이름
            TextField DialogueNameTextField = D_ElementUtilitie.CreateTextField(DialogueName, (callback) =>
            {
                if (Group == null)
                {
                    graphView.RemoveUngroupedNode(this);

                    DialogueName = callback.newValue;

                    graphView.AddUngroupedNode(this);

                    return;
                }

                Group currentGroup = Group;

                graphView.RemoveGroupedNode(this, Group);

                DialogueName = callback.newValue;

                graphView.AddGroupedNode(this, currentGroup);
            });
            DialogueNameTextField.AddClasses
            (
                "ds-node__textfield",
                "ds-node__filename-textfield",
                "ds-node__textfield_hidden"
            );
            titleContainer.Insert(0, DialogueNameTextField);

            // 이전 대화
            Port inputPort = this.CreatePort("Dialogue Conection", Orientation.Horizontal, Direction.Input, Port.Capacity.Multi);
            inputContainer.Add(inputPort);

            // 대화 내용
            VisualElement customDataContainer = new VisualElement();
            customDataContainer.AddToClassList("ds-node__custom-data-container");
            Foldout textFoldout = D_ElementUtilitie.CreateFoldout("Dialogue Text");
            TextField textTextField = D_ElementUtilitie.CreateTextArea(Text);
            textTextField.AddClasses
            (
                "ds-node__textfield",
                "ds-node__quote-textfield"
            );
            textFoldout.Add(textTextField);
            customDataContainer.Add(textFoldout);
            extensionContainer.Add(customDataContainer);

            RefreshExpandedState();
        }

        public void SetErrorStyle(Color color)
        {
            mainContainer.style.backgroundColor = color;
        }

        public void ResetStyle()
        {
            mainContainer.style.backgroundColor = defalutBackgroundColor;
        }
    }
}