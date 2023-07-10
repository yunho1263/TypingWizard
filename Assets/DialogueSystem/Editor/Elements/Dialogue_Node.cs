using System;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using System.Linq;
using UnityEngine.Localization;
using UnityEditor.UIElements;
using UnityEditor;

namespace DialogueSystem.Elements
{
    using Windows;
    using Utilities;
    using Enums;
    using Data.Save;

    [Serializable]
    public class Dialogue_Node : Node
    {
        public string ID { get; set; }

        public DialogueNodeType DialogueType { get; set; }

        //다이얼로그 이름
        public string DialogueName { get; set; }

        // 화자
        public string Speaker { get; set; }

        // 대화 내용
        public string Text { get; set; }
        
        // 분기
        public List<D_BranchSaveData> Branchs { get; set; }

        // 그룹
        public Dialogue_Group Group { get; set; }

        protected D_GraphView graphView;
        private Color defalutBackgroundColor;

        public virtual void initialize(string nodeName, D_GraphView d_GraphView,  Vector2 pos)
        {
            ID = Guid.NewGuid().ToString();
            DialogueName = nodeName;
            Branchs = new List<D_BranchSaveData>();
            Speaker = "SpeakerName";
            Text = "Sentence";

            graphView = d_GraphView;
            defalutBackgroundColor = new Color(29f/ 255f, 29f / 255f, 30f / 255f);

            SetPosition(new Rect(pos, Vector2.zero));

            mainContainer.AddToClassList("ds-node__main-container");
            extensionContainer.AddToClassList("ds-node__extension-container");
        }

        public virtual void Draw()
        {
            // 대화 이름
            TextField DialogueNameTextField = D_ElementUtilitie.CreateTextField(DialogueName, null, (callback) =>
            {
                TextField target = callback.target as TextField;
                target.value = callback.newValue.RemoveWhitespaces().RemoveSpecialCharacters();

                if (string.IsNullOrEmpty(target.value))
                {
                    if (!string.IsNullOrEmpty(DialogueName))
                    {
                        graphView.NameErrorsAmount++;
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(DialogueName))
                    {
                        graphView.NameErrorsAmount--;
                    }
                }

                if (Group == null)
                {
                    graphView.RemoveUngroupedNode(this);

                    DialogueName = target.value;

                    graphView.AddUngroupedNode(this);

                    return;
                }

                Dialogue_Group currentGroup = Group;

                graphView.RemoveGroupedNode(this, Group);

                DialogueName = target.value;

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

            // 화자
            VisualElement customDataContainer1 = new VisualElement();
            customDataContainer1.AddToClassList("ds-node__custom-data-container");
            Foldout textFoldout1 = D_ElementUtilitie.CreateFoldout("Dialogue Speaker");

            TextField speakerTextField = D_ElementUtilitie.CreateTextField(Speaker, null, callback =>
            {
                Speaker = callback.newValue;
            });
            speakerTextField.AddClasses
            (
                "ds-node__textfield",
                "ds-node__quote-textfield"
            );
            textFoldout1.Add(speakerTextField);
            customDataContainer1.Add(textFoldout1);
            extensionContainer.Add(customDataContainer1);

            // 대화 내용
            VisualElement customDataContainer2 = new VisualElement();
            customDataContainer2.AddToClassList("ds-node__custom-data-container");
            Foldout textFoldout2 = D_ElementUtilitie.CreateFoldout("Dialogue Text");

            TextField textTextField = D_ElementUtilitie.CreateTextArea(Text, null, callback =>
            {
                Text = callback.newValue;
            });
            textTextField.AddClasses
            (
                "ds-node__textfield",
                "ds-node__quote-textfield"
            );
            textFoldout2.Add(textTextField);
            customDataContainer2.Add(textFoldout2);
            extensionContainer.Add(customDataContainer2);

            RefreshExpandedState();
        }

        #region Override Methods / 오버라이드 메소드
        public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
        {
            evt.menu.AppendAction("Disconnect Input Ports", (action) => DisconnectInputPorts());
            evt.menu.AppendAction("Disconnect Output Ports", (action) => DisconnectOutputPorts());

            base.BuildContextualMenu(evt);
        }
        #endregion

        #region Utility Methods / 유틸리티 메소드
        public void DisconnectAllPorts()
        {
            DisconnectInputPorts();
            DisconnectOutputPorts();
        }

        private void DisconnectInputPorts()
        {
            Disconnectports(inputContainer);
        }

        private void DisconnectOutputPorts()
        {
            Disconnectports(outputContainer);
        }

        private void Disconnectports(VisualElement container)
        {
            foreach (Port port in container.Children())
            {
                if (!port.connected)
                {
                    continue;
                }

                graphView.DeleteElements(port.connections);
            }
        }

        public bool IsStartingNode()
        {
            Port inputPort = inputContainer.Children().First() as Port;

            return !inputPort.connected;
        }

        public void SetErrorStyle(Color color)
        {
            mainContainer.style.backgroundColor = color;
        }

        public void ResetStyle()
        {
            mainContainer.style.backgroundColor = defalutBackgroundColor;
        }
        #endregion
    }
}