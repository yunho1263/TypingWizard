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
        public string Ko_Text { get; set; }
        public string Ja_Text { get; set; }
        public string En_Text { get; set; }
        
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
            Ko_Text = "ko_Sentence";
            Ja_Text = "ja_Sentence";
            En_Text = "en_Sentence";

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
            VisualElement speakerDataContainer = new VisualElement();
            speakerDataContainer.AddToClassList("ds-node__custom-data-container");
            Foldout speakerFoldout = D_ElementUtilitie.CreateFoldout("Dialogue Speaker");

            TextField speakerTextField = D_ElementUtilitie.CreateTextField(Speaker, null, callback =>
            {
                Speaker = callback.newValue;
            });
            speakerTextField.AddClasses
            (
                "ds-node__textfield",
                "ds-node__quote-textfield"
            );
            speakerFoldout.Add(speakerTextField);
            speakerDataContainer.Add(speakerFoldout);
            extensionContainer.Add(speakerDataContainer);

            // 대화 내용
            VisualElement textDataContainer = new VisualElement();
            textDataContainer.AddToClassList("ds-node__custom-data-container");
            Foldout textFoldout = D_ElementUtilitie.CreateFoldout("Dialogue Text");

            // ----------------------------------------------------------

            TextField ko_textTextField = D_ElementUtilitie.CreateTextArea(Ko_Text, null, callback =>
            {
                Ko_Text = callback.newValue;
            });
            ko_textTextField.AddClasses
            (
                "ds-node__textfield",
                "ds-node__quote-textfield_ko"
            );
            textFoldout.Add(ko_textTextField);
            textDataContainer.Add(textFoldout);

            TextField ja_textTextField = D_ElementUtilitie.CreateTextArea(Ja_Text, null, callback =>
            {
                Ja_Text = callback.newValue;
            });
            ja_textTextField.AddClasses
            (
                "ds-node__textfield",
                "ds-node__quote-textfield_ja"
            );
            textFoldout.Add(ja_textTextField);
            textDataContainer.Add(textFoldout);

            TextField en_textTextField = D_ElementUtilitie.CreateTextArea(En_Text, null, callback =>
            {
                En_Text = callback.newValue;
            });
            en_textTextField.AddClasses
            (
                "ds-node__textfield",
                "ds-node__quote-textfield_en"
            );
            textFoldout.Add(en_textTextField);
            textDataContainer.Add(textFoldout);

            //----------------------------------------------------------


            extensionContainer.Add(textDataContainer);

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