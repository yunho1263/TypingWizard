using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace DialogueSystem.Elements
{
    using Enums;
    using Windows;
    using Data.Save;
    using DialogueSystem.Utilities;

    public class Dialogue_Single_Node : Dialogue_Node
    {
        public override void initialize(D_GraphView d_GraphView, Vector2 pos)
        {
            base.initialize(d_GraphView, pos);

            DialogueType = DialogueNodeType.Single;

            D_BranchSaveData branchData = new D_BranchSaveData()
            {
                Text = "Next Dialogue"
            };

            Branchs.Add(branchData);
        }

        public override void Draw()
        {
            base.Draw();
            // 다음 대화
            foreach (D_BranchSaveData branch in Branchs)
            {
                Port outputPort = this.CreatePort(branch.Text);

                outputPort.userData = branch;

                outputContainer.Add(outputPort);
            }

            RefreshExpandedState();
        }
    }
}
