using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace DialogueSystem.Elements
{
    using Windows;
    using Utilities;
    using static Codice.CM.Common.BranchExplorerData;

    public class Dialogue_Branch_Node : Dialogue_Node
    {
        public override void initialize(D_GraphView d_GraphView, Vector2 pos)
        {
            base.initialize(d_GraphView, pos);

            DialogueType = DialogueNodeType.Branch;
            Branchs.Add("New Branch");
        }

        public override void Draw()
        {
            base.Draw();

            Button addBranchButton = D_ElementUtilitie.CreateButton("Add Branch", () =>
            {
                Port outputPort = CreateBranchPort("New Branch");
                Branchs.Add("New Branch");
                outputContainer.Add(outputPort);
            });

            addBranchButton.AddToClassList("ds-node__button");
            mainContainer.Insert(1, addBranchButton);

            // 다음 대화
            foreach (string branch in Branchs)
            {
                Port outputPort = CreateBranchPort(branch);
                outputContainer.Add(outputPort);
            }

            RefreshExpandedState();
        }

        private Port CreateBranchPort(string branch)
        {
            Port outputPort = this.CreatePort();
            Button DeleteBranchButton = D_ElementUtilitie.CreateButton("X");
            DeleteBranchButton.AddToClassList("ds-node__button");

            TextField branchTextField = D_ElementUtilitie.CreateTextField(branch);

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
