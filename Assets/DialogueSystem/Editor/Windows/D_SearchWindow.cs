using DialogueSystem.Elements;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace DialogueSystem.Windows
{
    using Elements;
    public class D_SearchWindow : ScriptableObject, ISearchWindowProvider
    {
        private D_GraphView graphView;
        private Texture2D icon;
        public void initialize(D_GraphView graphView)
        {
            this.graphView = graphView;

            icon = new Texture2D(1, 1);
            icon.SetPixel(0, 0, Color.clear);
            icon.Apply();
        }

        public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
        {
            List<SearchTreeEntry> searchTreeEntries = new List<SearchTreeEntry>()
            {
                new SearchTreeGroupEntry(new GUIContent("Create Element")),
                new SearchTreeGroupEntry(new GUIContent("Dialogue Node"), 1),
                new SearchTreeEntry(new GUIContent("Single Node", icon))
                {
                    userData = DialogueNodeType.Single,
                    level = 2
                },
                new SearchTreeEntry(new GUIContent("Branch Node", icon))
                {
                    userData = DialogueNodeType.Branch,
                    level = 2
                },
                new SearchTreeGroupEntry(new GUIContent("Dialogue Group"), 1),
                new SearchTreeEntry(new GUIContent("Single Group", icon))
                {
                    userData = new Group(),
                    level = 2
                },
            };

            return searchTreeEntries;
        }

        public bool OnSelectEntry(SearchTreeEntry SearchTreeEntry, SearchWindowContext context)
        {
            Vector2 localMousePosition = graphView.GetLocalMousePosition(context.screenMousePosition, true);
            switch (SearchTreeEntry.userData)
            {
                case DialogueNodeType.Single:
                {
                    Dialogue_Node single_Node = graphView.CreateNode(DialogueNodeType.Single, localMousePosition);
                    graphView.AddElement(single_Node);
                    return true;
                }

                case DialogueNodeType.Branch:
                {
                    Dialogue_Branch_Node branch_Node = graphView.CreateNode(DialogueNodeType.Branch, localMousePosition) as Dialogue_Branch_Node;
                    graphView.AddElement(branch_Node);
                    return true;
                }

                case Group _:
                {
                    Group group = graphView.CreateGroup("Dialogue Group", localMousePosition);
                    graphView.AddElement(group);
                    return true;
                }

                default: return false;
            }
        }
    }
}
