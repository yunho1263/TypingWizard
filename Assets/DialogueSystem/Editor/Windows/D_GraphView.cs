using System;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;

namespace DialogueSystem.Windows
{
    using Codice.CM.Common;
    using Data.Error;
    using Elements;
    using Utilities;

    public class D_GraphView : GraphView
    {
        private D_SearchWindow searchWindow;
        private DialogueEditorWindow editorWindow;

        private SerializableDictionary<string, D_NodeErrorData> ungroupedNodes;
        private SerializableDictionary<Group, SerializableDictionary<string, D_NodeErrorData>> groupedNodes;

        public D_GraphView(DialogueEditorWindow editorWindow)
        {
            this.editorWindow = editorWindow;

            ungroupedNodes = new SerializableDictionary<string, D_NodeErrorData>();
            groupedNodes = new SerializableDictionary<Group, SerializableDictionary<string, D_NodeErrorData>>();

            AddManipulators();
            AddSearchWindow();
            AddGridBackground();

            OnElementsDeleted();
            OnGroupElementsAdded();
            OnGroupElementsRemoved();

            AddStyles();
        }

        #region Override Methods

        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {
            List<Port> compatiblePorts = new List<Port>();

            ports .ForEach((port) =>
            {
                if (startPort == port)
                {
                    return;
                }
                if (startPort.node == port.node)
                {
                    return;
                }

                if (startPort.direction == port.direction)
                {
                    return;
                }

                compatiblePorts.Add(port);
            });

            return compatiblePorts;
        }

        #endregion

        #region Manipulators
        private void AddManipulators()
        {
            SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);
            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());
            this.AddManipulator(CreateNodeContextualMenu("Add SingleNode", DialogueNodeType.Single));
            this.AddManipulator(CreateNodeContextualMenu("Add BranchNode", DialogueNodeType.Branch));
            this.AddManipulator(CreateGroupContextualMenu());
        }

        private IManipulator CreateGroupContextualMenu()
        {
            ContextualMenuManipulator menuManipulator = new ContextualMenuManipulator((evt) =>
            {
                evt.menu.AppendAction("Add group", (action) => { AddElement(CreateGroup("Dialogue Group", GetLocalMousePosition(action.eventInfo.localMousePosition))); });
            });

            return menuManipulator;
        }

        private IManipulator CreateNodeContextualMenu(string actionTitle, DialogueNodeType NodeType)
        {
            ContextualMenuManipulator menuManipulator = new ContextualMenuManipulator((evt) =>
            {
                evt.menu.AppendAction(actionTitle, (action) => { AddElement(CreateNode(NodeType, GetLocalMousePosition(action.eventInfo.localMousePosition))); });
            });

            return menuManipulator;
        }

        #endregion

        #region Create
        public Dialogue_Node CreateNode(DialogueNodeType newNodeType, Vector2 position)
        {
            Type nodeType = Type.GetType($"DialogueSystem.Elements.Dialogue_{newNodeType}_Node");
            Dialogue_Node node = Activator.CreateInstance(nodeType) as Dialogue_Node;

            node.initialize(this, position);
            node.Draw();

            AddUngroupedNode(node);

            return node;
        }

        public Group CreateGroup(string title, Vector2 localMousePosition)
        {
            Group group = new Group()
            {
                title = title
            };

            group.SetPosition(new Rect(localMousePosition, Vector2.zero));

            return group;
        }
        #endregion

        #region Add
        private void AddGridBackground()
        {
            var grid = new GridBackground();
            grid.StretchToParentSize();
            Insert(0, grid);
        }

        private void AddStyles()
        {
            this.AddStyleSheets
            (
                "DialogueSystem/D_GraphView_Style.uss",
                "DialogueSystem/D_Node_Style.uss"
            );
        }

        private void AddSearchWindow()
        {
            if (searchWindow == null)
            {
                searchWindow = ScriptableObject.CreateInstance<D_SearchWindow>();
                searchWindow.initialize(this);
            }

            nodeCreationRequest = (context) =>
            {
                SearchWindow.Open(new SearchWindowContext(context.screenMousePosition), searchWindow);
            };
        }
        #endregion

        #region Callbacks
        private void OnElementsDeleted()
        {
            deleteSelection = (operationName, askUser) =>
            {
                List<Dialogue_Node> nodesToDelete = new List<Dialogue_Node>();

                foreach (GraphElement element in selection)
                {
                    if (element is Dialogue_Node node)
                    {
                        nodesToDelete.Add(node);

                        continue;
                    }
                }

                foreach (Dialogue_Node node in nodesToDelete)
                {
                    if (node.Group != null)
                    {
                        node.Group.RemoveElement(node);
                    }

                    RemoveUngroupedNode(node);

                    RemoveElement(node);
                }
            };
        }

        private void OnGroupElementsAdded()
        {
            elementsAddedToGroup = (group, elements) =>
            {
                foreach (GraphElement element in elements)
                {
                    if (!(element is Dialogue_Node))
                    {
                        continue;
                    }

                    Dialogue_Node node = element as Dialogue_Node;

                    RemoveUngroupedNode(node);
                    AddGroupedNode(node, group);
                }
            };
        }

        private void OnGroupElementsRemoved()
        {
            elementsRemovedFromGroup = (group, elements) =>
            {
                foreach (GraphElement element in elements)
                {
                    if (!(element is Dialogue_Node))
                    {
                        continue;
                    }

                    Dialogue_Node node = element as Dialogue_Node;

                    RemoveGroupedNode(node, group);
                    AddUngroupedNode(node);
                }
            };
        }
        #endregion

        #region RepeatedElements
        public void AddUngroupedNode(Dialogue_Node node)
        {
            string nodeName = node.DialogueName;

            if (!ungroupedNodes.ContainsKey(nodeName))
            {
                D_NodeErrorData errorData = new D_NodeErrorData();
                errorData.Nodes.Add(node);

                ungroupedNodes.Add(nodeName, errorData);

                return;
            }

            List<Dialogue_Node> ungroupedNodesList = ungroupedNodes[nodeName].Nodes;

            ungroupedNodesList.Add(node);

            Color errorColor = ungroupedNodes[nodeName].ErrorData.Color;
            node.SetErrorStyle(errorColor);

            if (ungroupedNodesList.Count == 2)
            {
                ungroupedNodesList[0].SetErrorStyle(errorColor);
            }
        }

        public void RemoveUngroupedNode(Dialogue_Node node)
        {
            string nodeName = node.DialogueName;
            List<Dialogue_Node> ungroupedNodesList = ungroupedNodes[nodeName].Nodes;


            ungroupedNodesList.Remove(node);

            node.ResetStyle();

            if (ungroupedNodesList.Count == 1)
            {
                ungroupedNodesList[0].ResetStyle();
                return;
            }

            if (ungroupedNodesList.Count == 0)
            {
                ungroupedNodes.Remove(nodeName);
            }

        }

        public void AddGroupedNode(Dialogue_Node node, Group group)
        {
            string nodeName = node.DialogueName;

            node.Group = group;

            if (!groupedNodes.ContainsKey(group))
            {
                groupedNodes.Add(group, new SerializableDictionary<string, D_NodeErrorData>());
            }

            if (!groupedNodes[group].ContainsKey(nodeName))
            {
                D_NodeErrorData errorData = new D_NodeErrorData();
                errorData.Nodes.Add(node);
                groupedNodes[group].Add(nodeName, errorData);

                return;
            }

            List<Dialogue_Node> groupedNodesList = groupedNodes[group][nodeName].Nodes;

            groupedNodesList.Add(node);

            Color errorColor = groupedNodes[group][nodeName].ErrorData.Color;
            node.SetErrorStyle(errorColor);

            if (groupedNodesList.Count == 2)
            {
                groupedNodesList[0].SetErrorStyle(errorColor);
            }
        }

        public void RemoveGroupedNode(Dialogue_Node node, Group group)
        {
            string nodeName = node.DialogueName;

            node.Group = null;

            List<Dialogue_Node> groupedNodesList = groupedNodes[group][nodeName].Nodes;

            groupedNodesList.Remove(node);
            node.ResetStyle();

            if (groupedNodesList.Count == 1)
            {
                groupedNodesList[0].ResetStyle();
                return;
            }

            if (groupedNodesList.Count == 0)
            {
                groupedNodes[group].Remove(nodeName);

                if (groupedNodes[group].Count == 0)
                {
                    groupedNodes.Remove(group);
                }
            }
        }
        #endregion

        #region Utility
        public Vector2 GetLocalMousePosition(Vector2 m_pos, bool isSearchWindow = false)
        {
            Vector2 worldMousePosition = m_pos;

            if (isSearchWindow)
            {
                worldMousePosition -= editorWindow.position.position;
            }

            Vector2 localMousePosition = contentViewContainer.WorldToLocal(worldMousePosition);
            return localMousePosition;
        }

        #endregion
    }
}
