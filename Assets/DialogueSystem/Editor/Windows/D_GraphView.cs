using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;

namespace DialogueSystem.Windows
{
    using Enums;
    using Data.Error;
    using Elements;
    using Utilities;
    using Data.Save;

    public class D_GraphView : GraphView
    {
        private D_SearchWindow searchWindow;
        private DialogueEditorWindow editorWindow;

        private MiniMap miniMap;

        private SerializableDictionary<string, D_NodeErrorData> ungroupedNodes;
        private SerializableDictionary<Group, SerializableDictionary<string, D_NodeErrorData>> groupedNodes;
        private SerializableDictionary<string, D_GroupErrorData> groups;

        private int nameErrorsAmount = 0;
        public int NameErrorsAmount
        {
            get 
            {
                return nameErrorsAmount;
            }
            set
            {
                nameErrorsAmount = value;
                if (nameErrorsAmount == 0)
                {
                    editorWindow.EnableSaveButton();
                }

                if (nameErrorsAmount >= 1)
                {
                    editorWindow.DisableSaveButton();
                }
            }
        }

        public D_GraphView(DialogueEditorWindow editorWindow)
        {
            this.editorWindow = editorWindow;

            ungroupedNodes = new SerializableDictionary<string, D_NodeErrorData>();
            groupedNodes = new SerializableDictionary<Group, SerializableDictionary<string, D_NodeErrorData>>();
            groups = new SerializableDictionary<string, D_GroupErrorData>();

            AddManipulators();
            AddSearchWindow();
            AddMiniMap();
            AddGridBackground();

            OnElementsDeleted();
            OnGroupElementsAdded();
            OnGroupElementsRemoved();
            OnGroupRenamed();
            OnGraphViewChanged();

            AddStyles();
            AddMiniMapStyles();
        }

        #region Override Methods / 오버라이드 메소드

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

        #region Manipulators / 조작기
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
                evt.menu.AppendAction("Add group", (action) => { AddElement(CreateGroup("DialogueGroup", GetLocalMousePosition(action.eventInfo.localMousePosition))); });
            });

            return menuManipulator;
        }

        private IManipulator CreateNodeContextualMenu(string actionTitle, DialogueNodeType NodeType)
        {
            ContextualMenuManipulator menuManipulator = new ContextualMenuManipulator((evt) =>
            {
                evt.menu.AppendAction(actionTitle, (action) => 
                { AddElement(CreateNode("DialogueName", NodeType, GetLocalMousePosition(action.eventInfo.localMousePosition))); });
            });

            return menuManipulator;
        }

        #endregion

        #region Create / 생성
        public Dialogue_Node CreateNode(string nodeName, DialogueNodeType newNodeType, Vector2 position, bool shouldDraw = true)
        {
            Type nodeType = Type.GetType($"DialogueSystem.Elements.Dialogue_{newNodeType}_Node");
            Dialogue_Node node = Activator.CreateInstance(nodeType) as Dialogue_Node;

            node.initialize(nodeName, this, position);

            if (shouldDraw)
            {
                node.Draw();
            }

            AddUngroupedNode(node);

            return node;
        }

        public Dialogue_Group CreateGroup(string title, Vector2 localMousePosition)
        {
            Dialogue_Group group = new Dialogue_Group(title, localMousePosition);

            AddGroup(group);
            AddElement(group);

            foreach (GraphElement selectedElement in selection)
            {
                if (!(selectedElement is Dialogue_Node))
                {
                    continue;
                }

                Dialogue_Node node = selectedElement as Dialogue_Node;
                group.AddElement(node);
            }

            return group;
        }
        #endregion

        #region Add / 추가
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

        private void AddMiniMap()
        {
            miniMap = new MiniMap()
            {
                anchored = true
            };

            miniMap.SetPosition(new Rect(15, 50, 200, 180));

            Add(miniMap);

            miniMap.visible = false;
        }


        private void AddMiniMapStyles()
        {
            StyleColor backgroundColor = new StyleColor(new Color32(29, 29, 30, 255));
            StyleColor borderColor = new StyleColor(new Color32(51, 51, 51, 255));

            miniMap.style.backgroundColor = backgroundColor;
            miniMap.style.borderTopColor = borderColor;
            miniMap.style.borderRightColor = borderColor;
            miniMap.style.borderBottomColor = borderColor;
            miniMap.style.borderLeftColor = borderColor;
        }
        #endregion

        #region Callbacks / 콜백
        private void OnElementsDeleted()
        {
            deleteSelection = (operationName, askUser) =>
            {
                Type groupType = typeof(Dialogue_Group);
                Type edgeType = typeof(Edge);

                List<Dialogue_Group> groupsToDelete = new List<Dialogue_Group>();
                List<Dialogue_Node> nodesToDelete = new List<Dialogue_Node>();
                List<Edge> edgesToDelete = new List<Edge>();

                foreach (GraphElement element in selection)
                {
                    if (element is Dialogue_Node node)
                    {
                        nodesToDelete.Add(node);
                        continue;
                    }

                    if (element.GetType() == edgeType)
                    {
                        Edge edge = element as Edge;
                        edgesToDelete.Add(edge);
                        continue;
                    }

                    if (element.GetType() != groupType)
                    {
                        continue;
                    }

                    Dialogue_Group group = element as Dialogue_Group;
                    groupsToDelete.Add(group);
                }

                foreach (Dialogue_Group group in groupsToDelete)
                {
                    List<Dialogue_Node> groupNodes = new List<Dialogue_Node>();

                    foreach (GraphElement groupElement in group.containedElements)
                    {
                        if (!(groupElement is Dialogue_Node))
                        {
                            continue;
                        }

                        Dialogue_Node groupNode = groupElement as Dialogue_Node;
                        groupNodes.Add(groupNode);
                    }

                    group.RemoveElements(groupNodes);

                    RemoveGroup(group);
                    RemoveElement(group);
                }

                DeleteElements(edgesToDelete);

                foreach (Dialogue_Node node in nodesToDelete)
                {
                    if (node.Group != null)
                    {
                        node.Group.RemoveElement(node);
                    }

                    RemoveUngroupedNode(node);
                    node.DisconnectAllPorts();
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

                    Dialogue_Group nodeGroup = group as Dialogue_Group;
                    Dialogue_Node node = element as Dialogue_Node;

                    RemoveUngroupedNode(node);
                    AddGroupedNode(node, nodeGroup);
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

        private void OnGroupRenamed()
        {
            groupTitleChanged = (group, newTitle) =>
            {
                Dialogue_Group dialogueGroup = group as Dialogue_Group;

                dialogueGroup.title = newTitle.RemoveWhitespaces().RemoveSpecialCharacters();

                if (string.IsNullOrEmpty(dialogueGroup.title))
                {
                    if (!string.IsNullOrEmpty(dialogueGroup.OldTitle))
                    {
                        NameErrorsAmount++;
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(dialogueGroup.OldTitle))
                    {
                        NameErrorsAmount--;
                    }
                }

                RemoveGroup(dialogueGroup);

                dialogueGroup.OldTitle = dialogueGroup.title;

                AddGroup(dialogueGroup);
            };
        }

        private void OnGraphViewChanged()
        {
            graphViewChanged = (changes) =>
            {
                if (changes.edgesToCreate != null)
                {
                    foreach (Edge edge in changes.edgesToCreate)
                    {
                        Dialogue_Node nextNode = edge.input.node as Dialogue_Node;

                        D_BranchSaveData branchData = edge.output.userData as D_BranchSaveData;

                        branchData.NodeId = nextNode.ID;
                    }
                }

                if (changes.elementsToRemove != null)
                {
                    Type edgeType = typeof(Edge);

                    foreach (GraphElement element in changes.elementsToRemove)
                    {
                        if (element.GetType() != edgeType)
                        {
                            continue;
                        }

                        Edge edge = element as Edge;

                        D_BranchSaveData branchData = edge.output.userData as D_BranchSaveData;

                        branchData.NodeId = "";
                    }
                }

                return changes;
            };
        }
        #endregion

        #region RepeatedElements / 반복요소
        public void AddUngroupedNode(Dialogue_Node node)
        {
            string nodeName = node.DialogueName.ToLower();

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
                NameErrorsAmount++;
                ungroupedNodesList[0].SetErrorStyle(errorColor);
            }
        }

        public void RemoveUngroupedNode(Dialogue_Node node)
        {
            string nodeName = node.DialogueName.ToLower();
            List<Dialogue_Node> ungroupedNodesList = ungroupedNodes[nodeName].Nodes;


            ungroupedNodesList.Remove(node);

            node.ResetStyle();

            if (ungroupedNodesList.Count == 1)
            {
                NameErrorsAmount--;
                ungroupedNodesList[0].ResetStyle();
                return;
            }

            if (ungroupedNodesList.Count == 0)
            {
                ungroupedNodes.Remove(nodeName);
            }

        }

        public void AddGroupedNode(Dialogue_Node node, Dialogue_Group group)
        {
            string nodeName = node.DialogueName.ToLower();

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
                NameErrorsAmount++;
                groupedNodesList[0].SetErrorStyle(errorColor);
            }
        }

        public void RemoveGroupedNode(Dialogue_Node node, Group group)
        {
            string nodeName = node.DialogueName.ToLower();

            node.Group = null;

            List<Dialogue_Node> groupedNodesList = groupedNodes[group][nodeName].Nodes;

            groupedNodesList.Remove(node);
            node.ResetStyle();

            if (groupedNodesList.Count == 1)
            {
                NameErrorsAmount--;
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

        private void AddGroup(Dialogue_Group group)
        {
            string groupName = group.title.ToLower();

            if (!groups.ContainsKey(groupName))
            {
                D_GroupErrorData errorData = new D_GroupErrorData();
                errorData.Groups.Add(group);
                groups.Add(groupName, errorData);

                return;
            }

            List<Dialogue_Group> groupList = groups[groupName].Groups;
            groupList.Add(group);

            Color errorColor = groups[groupName].ErrorData.Color;
            group.SetErrorStyle(errorColor);

            if (groupList.Count == 2)
            {
                NameErrorsAmount++;
                groupList[0].SetErrorStyle(errorColor);
            }
        }

        private void RemoveGroup(Dialogue_Group group)
        {
            string oldGroupName = group.OldTitle.ToLower();

            List<Dialogue_Group> groupsList = groups[oldGroupName].Groups;

            groupsList.Remove(group);

            group.ResetStyle();

            if (groupsList.Count == 1)
            {
                NameErrorsAmount--;
                groupsList[0].ResetStyle();

                return;
            }

            if (groupsList.Count == 0)
            {
                groups.Remove(oldGroupName);
            }
        }
        #endregion

        #region Utility / 유틸리티
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

        public void ClearGraph()
        {
            graphElements.ForEach((element) =>
            {
                RemoveElement(element);
            });

            groups.Clear();
            groupedNodes.Clear();
            ungroupedNodes.Clear();

            NameErrorsAmount = 0;
            
        }

        public void ToggleMinimap()
        {
            miniMap.visible = !miniMap.visible;
        }

        #endregion
    }
}
