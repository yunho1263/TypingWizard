using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace DialogueSystem.Elements
{
    using Windows;
    public class Dialogue_Single_Node : Dialogue_Node
    {
        public override void initialize(D_GraphView d_GraphView, Vector2 pos)
        {
            base.initialize(d_GraphView, pos);

            DialogueType = DialogueNodeType.Single;
            Branchs.Add("Next Dialogue");
        }

        public override void Draw()
        {
            base.Draw();
            // 다음 대화
            foreach (string branch in Branchs)
            {
                Port outputPort = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(float));
                outputPort.portName = branch;
                outputContainer.Add(outputPort);
            }

            RefreshExpandedState();
        }
    }
}
