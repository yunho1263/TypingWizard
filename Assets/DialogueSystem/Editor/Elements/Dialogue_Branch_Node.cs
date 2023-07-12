using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace DialogueSystem.Elements
{
    using Enums;
    using Windows;
    using Utilities;
    using Data.Save;

    public class Dialogue_Branch_Node : Dialogue_Node
    {
        public override void initialize(string nodeName, D_GraphView d_GraphView, Vector2 pos)
        {
            // ���̽� �ʱ�ȭ ����
            base.initialize(nodeName, d_GraphView, pos);

            // ��� Ÿ�� ����
            DialogueType = DialogueNodeType.Branch;

            // ��� Ÿ��Ʋ ����
            D_BranchSaveData branchData = new D_BranchSaveData()
            {
                branchName = "NewBranchName",
                Ko_Text = "koText",
                Ja_Text = "jaText",
                En_Text = "enText"
            };

            Branchs.Add(branchData);
        }

        public override void Draw()
        {
            // ���̽� ��ο� ����
            base.Draw();

            // �귻ġ �߰� ��ư �׸���
            Button addBranchButton = D_ElementUtilitie.CreateButton("Add Branch", () =>
            {
                D_BranchSaveData branchData = new D_BranchSaveData()
                {
                    branchName = "NewBranchName",
                    Ko_Text = "koText",
                    Ja_Text = "jaText",
                    En_Text = "enText"
                };

                Branchs.Add(branchData);

                Port outputPort = CreateBranchPort(branchData);
                outputContainer.Add(outputPort);
            });

            addBranchButton.AddToClassList("ds-node__button");
            mainContainer.Insert(1, addBranchButton);

            foreach (D_BranchSaveData branch in Branchs)
            {
                Port outputPort = CreateBranchPort(branch);
                outputContainer.Add(outputPort);
            }

            RefreshExpandedState();
        }

        private Port CreateBranchPort(object userData) // �귻ġ ��Ʈ ����
        {
            Port outputPort = this.CreatePort();

            outputPort.userData = userData;

            D_BranchSaveData branchData = userData as D_BranchSaveData;

            Button DeleteBranchButton = D_ElementUtilitie.CreateButton("X", () =>
            {
                if (Branchs.Count == 1)
                {
                    return;
                }

                if (outputPort.connected)
                {
                    graphView.DeleteElements(outputPort.connections);
                }

                Branchs.Remove(branchData);
                graphView.RemoveElement(outputPort);
            });
            DeleteBranchButton.AddToClassList("ds-node__button");

            TextField branchNameField = D_ElementUtilitie.CreateTextField(branchData.branchName, null, callback =>
            {
                branchData.branchName = callback.newValue;
            });

            branchNameField.AddClasses
            (
                "ds-node__textfield",
                "ds-node__Branch-textfield",
                "ds-node__textfield_hidden"
            );
            outputPort.contentContainer.Add(branchNameField);

            //----------------------------------------------

            TextField branch_Ko_Text_Field = D_ElementUtilitie.CreateTextField(branchData.Ko_Text, null, callback =>
            {
                branchData.Ko_Text = callback.newValue;
            });

            branch_Ko_Text_Field.AddClasses
            (
                "ds-node__textfield",
                "ds-node__Branch-textfield_ko",
                "ds-node__textfield_hidden"
            );
            outputPort.contentContainer.Add(branch_Ko_Text_Field);

            //----------------------------------------------

            TextField branch_Ja_Text_Field = D_ElementUtilitie.CreateTextField(branchData.Ja_Text, null, callback =>
            {
                branchData.Ja_Text = callback.newValue;
            });

            branch_Ja_Text_Field.AddClasses
            (
                "ds-node__textfield",
                "ds-node__Branch-textfield_ja",
                "ds-node__textfield_hidden"
            );
            outputPort.contentContainer.Add(branch_Ja_Text_Field);

            //----------------------------------------------

            TextField branch_En_Text_Field = D_ElementUtilitie.CreateTextField(branchData.En_Text, null, callback =>
            {
                branchData.En_Text = callback.newValue;
            });

            branch_En_Text_Field.AddClasses
            (
                "ds-node__textfield",
                "ds-node__Branch-textfield_en",
                "ds-node__textfield_hidden"
            );
            outputPort.contentContainer.Add(branch_En_Text_Field);

            //----------------------------------------------

            outputPort.contentContainer.Add(DeleteBranchButton);
            return outputPort;
        }
    }
}
