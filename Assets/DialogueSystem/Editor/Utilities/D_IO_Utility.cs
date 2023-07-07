using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.Linq;

namespace DialogueSystem.Utilities
{
    using Data;
    using Data.Save;
    using ScrObj;
    using Elements;
    using Windows;

    public static class D_IO_Utility
    {
        private static D_GraphView graphView;
        private static string graphFileName;
        private static string containerFolderPath;

        private static List<Dialogue_Group> groups;
        private static List<Dialogue_Node> nodes;

        private static Dictionary<string, D_DialogueGroupSO> createdDialogueGroups;
        private static Dictionary<string, D_DialogueSO> createdDialogues;

        public static void Initialize(D_GraphView d_graphView, string graphName)
        {
            graphView = d_graphView;
            graphFileName = graphName;
            containerFolderPath = $"Assets/DialogueSystem/DialogueData/{graphFileName}";

            groups = new List<Dialogue_Group>();
            nodes = new List<Dialogue_Node>();

            createdDialogueGroups = new Dictionary<string, D_DialogueGroupSO>();
            createdDialogues = new Dictionary<string, D_DialogueSO>();
        }


        #region Save Methods / 저장 메소드
        public static void Save()
        {
            CreateStaticFolders();

            GetElementsFromGraphView();

            D_GraphSaveDataSO graphData = CreateAsset<D_GraphSaveDataSO>("Assets/DialogueSystem/Editor/Graphs", $"{graphFileName}Graph");
            graphData.Initialize(graphFileName);

            D_DialogueContainerSO dialogueContainer  = CreateAsset<D_DialogueContainerSO>(containerFolderPath, graphFileName);
            dialogueContainer.Initialize(graphFileName);

            SaveGroups(graphData, dialogueContainer);
            SaveNodes(graphData, dialogueContainer);

            SaveAsset(graphData);
            SaveAsset(dialogueContainer);
        }

        #region Groups / 그룹
        private static void SaveGroups(D_GraphSaveDataSO graphData, D_DialogueContainerSO dialogueContainer)
        {
            List<string> groupNames = new List<string>();
            foreach (Dialogue_Group group in groups)
            {
                SaveGroupToGraph(group, graphData);
                SaveGroupToScriptableObject(group, dialogueContainer);

                groupNames.Add(group.title);
            }

            UpdateOldGroups(groupNames, graphData);
        }

        private static void SaveGroupToGraph(Dialogue_Group group, D_GraphSaveDataSO graphData)
        {
            D_GroupSaveData groupData = new D_GroupSaveData
            {
                ID = group.ID,
                Name = group.title,
                Position = group.GetPosition().position,
            };

            graphData.Groups.Add(groupData);
        }

        private static void SaveGroupToScriptableObject(Dialogue_Group group, D_DialogueContainerSO dialogueContainer)
        {
            string groupName = group.title;

            CreateFolder($"{containerFolderPath}/Groups", groupName);
            CreateFolder($"{containerFolderPath}/Groups/{groupName}", "Dialogues");

            D_DialogueGroupSO dialogueGroup = CreateAsset<D_DialogueGroupSO>($"{containerFolderPath}/Groups/{groupName}", groupName);

            dialogueGroup.Initialize(groupName);

            createdDialogueGroups.Add(group.ID, dialogueGroup);

            dialogueContainer.DialogueGroups.Add(dialogueGroup, new List<D_DialogueSO>());

            SaveAsset(dialogueGroup);
        }

        private static void UpdateOldGroups(List<string> currentGroupNames, D_GraphSaveDataSO graphData)
        {
            if (graphData.OldGroupNames != null && graphData.OldGroupNames.Count != 0)
            {
                List<string> groupsToRemove = graphData.OldGroupNames.Except(currentGroupNames).ToList();

                foreach (string groupToRemove in groupsToRemove)
                {
                    RemoveFolder($"{containerFolderPath}/Groups/{groupToRemove}");
                }
            }

            graphData.OldGroupNames = new List<string>(currentGroupNames);
        }
        #endregion

        #region Nodes / 노드
        private static void SaveNodes(D_GraphSaveDataSO graphData, D_DialogueContainerSO dialogueContainer)
        {
            SerializableDictionary<string, List<string>> groupedNodeNames = new SerializableDictionary<string, List<string>>();
            List<string> ungroupedNodeNames = new List<string>();

            foreach (Dialogue_Node node in nodes)
            {
                SaveNodeToGraph(node, graphData);
                SaveNodeToScriptableObject(node, dialogueContainer);

                if (node.Group != null)
                {
                    groupedNodeNames.AddItem(node.Group.title, node.DialogueName);

                    continue;
                }

                ungroupedNodeNames.Add(node.DialogueName);
            }

            UpdateDialogueBranchConnections();
            UpdateOldGroupedNode(groupedNodeNames, graphData);
            UpdateOldUngroupsNodes(ungroupedNodeNames, graphData);
        }

        

        private static void SaveNodeToGraph(Dialogue_Node node, D_GraphSaveDataSO graphData)
        {
            List<D_BranchSaveData> branches = new List<D_BranchSaveData>();

            foreach (D_BranchSaveData branch in node.Branchs)
            {
                D_BranchSaveData branchData = new D_BranchSaveData()
                {
                    Text = branch.Text,
                    NodeId = branch.NodeId
                };

                branches.Add(branchData);
            }

            D_NodeSaveData nodeData = new D_NodeSaveData()
            {
                ID = node.ID,
                Name = node.DialogueName,
                Branchs = branches,
                Text = node.Text,
                GroupID = node.Group?.ID,
                DialogueType = node.DialogueType,
                Position = node.GetPosition().position
            };

            graphData.Nodes.Add(nodeData);
        }

        private static void SaveNodeToScriptableObject(Dialogue_Node node, D_DialogueContainerSO dialogueContainer)
        {
            D_DialogueSO dialogue;

            if (node.Group != null)
            {
                dialogue = CreateAsset<D_DialogueSO>($"{containerFolderPath}/Groups/{node.Group.title}/Dialogues", node.DialogueName);

                dialogueContainer.DialogueGroups.AddItem(createdDialogueGroups[node.Group.ID], dialogue);
            }
            else
            {
                dialogue = CreateAsset<D_DialogueSO>($"{containerFolderPath}/Global/Dialogues", node.DialogueName);

                dialogueContainer.ungroupedDialogues.Add(dialogue);
            }

            dialogue.Initialize
            (
                node.DialogueName,
                node.Text,
                ConvertNodeBranchsToDialogueBranchs(node.Branchs),
                node.DialogueType,
                node.IsStartingNode()
            );

            createdDialogues.Add(node.ID, dialogue);

            SaveAsset(dialogue);
        }

        private static List<D_DialoguebranchData> ConvertNodeBranchsToDialogueBranchs(List<D_BranchSaveData> nodeBranchs)
        {
            List<D_DialoguebranchData> dialoguebranchs = new List<D_DialoguebranchData>();

            foreach (D_BranchSaveData nodeBranch in nodeBranchs)
            {
                D_DialoguebranchData brandata = new D_DialoguebranchData()
                {
                    Text = nodeBranch.Text
                };

                dialoguebranchs.Add(brandata);
            }

            return dialoguebranchs;
        }

        private static void UpdateDialogueBranchConnections()
        {
            foreach (Dialogue_Node node in nodes)
            {
                D_DialogueSO dialogue = createdDialogues[node.ID];

                for (int branchIndex = 0; branchIndex < node.Branchs.Count; branchIndex++)
                {
                    D_BranchSaveData nodeBranch = node.Branchs[branchIndex];

                    if (string.IsNullOrEmpty(nodeBranch.NodeId))
                    {
                        continue;
                    }

                    dialogue.Branchs[branchIndex].NextDialogue = createdDialogues[nodeBranch.NodeId];

                    SaveAsset(dialogue);
                }
            }
        }

        private static void UpdateOldGroupedNode(SerializableDictionary<string, List<string>> currentGroupedNodeNames, D_GraphSaveDataSO graphData)
        {
            if (graphData.OldGroupedNodeNames != null && graphData.OldGroupedNodeNames.Count != 0)
            {
                foreach (KeyValuePair<string, List<string>> oldgroupedNode in graphData.OldGroupedNodeNames)
                {
                    List<string> nodesToRemove = new List<string>();

                    if (currentGroupedNodeNames.ContainsKey(oldgroupedNode.Key))
                    {
                        nodesToRemove = oldgroupedNode.Value.Except(currentGroupedNodeNames[oldgroupedNode.Key]).ToList();
                    }

                    foreach (string nodeToRemove in nodesToRemove)
                    {
                        RemoveAsset($"{containerFolderPath}/Groups/{oldgroupedNode.Key}/Dialogues", nodeToRemove);
                    }
                }
            }

            graphData.OldGroupedNodeNames = new SerializableDictionary<string, List<string>>(currentGroupedNodeNames);
        }

        private static void UpdateOldUngroupsNodes(List<string> currentUngroupedNodeNames, D_GraphSaveDataSO graphData)
        {
            if (graphData.OldUngroupedNodeNames != null && graphData.OldUngroupedNodeNames.Count != 0)
            {
                List<string> nodesToRemove = graphData.OldUngroupedNodeNames.Except(currentUngroupedNodeNames).ToList();
                
                foreach (string nodeToRemove in nodesToRemove)
                {
                    RemoveAsset($"{containerFolderPath}/Global/Dialogues", nodeToRemove);
                }
            }

            graphData.OldUngroupedNodeNames = new List<string>(currentUngroupedNodeNames);
        }
        #endregion

        #endregion

        #region Fetch Methods / 가져오기 메소드
        private static void GetElementsFromGraphView()
        {
            Type groupType = typeof(Dialogue_Group);

            graphView.graphElements.ForEach(graphElement =>
            {
                if (graphElement is Dialogue_Node node)
                {
                    nodes.Add(node);

                    return;
                }

                if (graphElement.GetType() == groupType)
                {
                    Dialogue_Group group = graphElement as Dialogue_Group;

                    groups.Add(group);

                    return;
                }
            });
        }
        #endregion

        #region Creation Methods / 생성 메소드
        private static void CreateStaticFolders()
        {
            CreateFolder("Assets/DialogueSystem", "DialogueData");
            CreateFolder("Assets/DialogueSystem/Editor", "Graphs");

            CreateFolder("Assets/DialogueSystem/DialogueData", graphFileName);
            CreateFolder(containerFolderPath, "Global");
            CreateFolder(containerFolderPath, "Groups");
            CreateFolder($"{containerFolderPath}/Global", "Dialogues");
        }
        #endregion

        #region Utility Methods / 유틸리티 메소드
        private static void CreateFolder(string path, string folderName)
        {
            if (AssetDatabase.IsValidFolder($"{path}/{folderName}"))
            {
                return;
            }

            AssetDatabase.CreateFolder(path, folderName);
        }

        private static void RemoveFolder(string fullPath)
        {
            FileUtil.DeleteFileOrDirectory($"{fullPath}.meta");
            FileUtil.DeleteFileOrDirectory($"{fullPath}/");
        }

        private static T CreateAsset<T>(string path, string assetName) where T : ScriptableObject
        {
            string fullPath = $"{path}/{assetName}.asset";

            T asset = AssetDatabase.LoadAssetAtPath<T>(fullPath);

            if (asset == null)
            {
                asset = ScriptableObject.CreateInstance<T>();

                AssetDatabase.CreateAsset(asset, fullPath);
            }


            return asset;
        }

        private static void RemoveAsset(string path, string assetName)
        {
            AssetDatabase.DeleteAsset($"{path}/{assetName}.asset");
        }

        private static void SaveAsset(UnityEngine.Object asset)
        {
            EditorUtility.SetDirty(asset);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
        #endregion
    }
}
