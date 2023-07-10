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
                Text = "New Branch"
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
                    Text = "New Branch"
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

            TextField branchTextField = D_ElementUtilitie.CreateTextField(branchData.Text, null, callback =>
            {
                branchData.Text = callback.newValue;
            });

            branchTextField.AddClasses
            (
                "ds-node__textfield",
                "ds-node__Branch-textfield",
                "ds-node__textfield_hidden"
            );
            outputPort.contentContainer.Add(branchTextField);
            outputPort.contentContainer.Add(DeleteBranchButton);
            return outputPort;
        }
    }
}
