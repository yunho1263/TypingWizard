using System;
using UnityEngine;
using UnityEditor;
using System.Linq;
using UnityEditor.Localization;
using System.Collections.Generic;
using UnityEngine.Localization.Tables;
using UnityEditor.Experimental.GraphView;

namespace DialogueSystem.Utilities
{
    using Data;
    using ScrObj;
    using Windows;
    using Elements;
    using Data.Save;

    public static class D_IO_Utility
    {
        private static D_GraphView graphView;
        private static string graphFileName;
        private static string containerFolderPath;

        // �׷��� �信�� ������ ������
        private static List<Dialogue_Group> groups;
        private static List<Dialogue_Node> nodes;

        // �׷��� �信�� ������ �����͸� ScriptableObject�� ����� ������ ������
        private static Dictionary<string, D_DialogueGroupSO> createdDialogueGroups;
        private static Dictionary<string, D_DialogueSO> createdDialogues;

        private static Dictionary<string, Dialogue_Group> loadedGroups;
        private static Dictionary<string, Dialogue_Node> loadedNodes;

        public static void Initialize(D_GraphView d_graphView, string graphName)
        {
            graphView = d_graphView;
            graphFileName = graphName;
            containerFolderPath = $"Assets/DialogueSystem/DialogueData/{graphFileName}";

            groups = new List<Dialogue_Group>();
            nodes = new List<Dialogue_Node>();

            createdDialogueGroups = new Dictionary<string, D_DialogueGroupSO>();
            createdDialogues = new Dictionary<string, D_DialogueSO>();

            loadedGroups = new Dictionary<string, Dialogue_Group>();
            loadedNodes = new Dictionary<string, Dialogue_Node>();
        }


        #region Save Methods / ���� �޼ҵ�
        public static void Save()
        {
            // ���� ����
            CreateStaticFolders();

            // �׷����信�� ������ ��������
            GetElementsFromGraphView();

            // ������ ������ ����
            D_GraphSaveDataSO graphData = CreateAsset<D_GraphSaveDataSO>("Assets/DialogueSystem/Editor/Graphs", $"{graphFileName}Graph");
            graphData.Initialize(graphFileName);

            D_DialogueContainerSO dialogueContainer  = CreateAsset<D_DialogueContainerSO>(containerFolderPath, graphFileName);
            dialogueContainer.Initialize(graphFileName);

            // ������ ����
            SaveGroups(graphData, dialogueContainer);
            SaveNodes(graphData, dialogueContainer);

            SaveAsset(graphData);
            SaveAsset(dialogueContainer);
        }

        #region Groups / �׷�
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
        #endregion

        #region Nodes / ���
        private static void SaveNodes(D_GraphSaveDataSO graphData, D_DialogueContainerSO dialogueContainer)
        {
            SerializableDictionary<string, List<string>> groupedNodeNames = new SerializableDictionary<string, List<string>>();
            List<string> ungroupedNodeNames = new List<string>();
            List<string> branchNames = new List<string>();

            foreach (Dialogue_Node node in nodes)
            {
                // ��� ����
                SaveNodeToGraph(node, graphData);
                SaveNodeToScriptableObject(node, dialogueContainer);

                if (node.DialogueType == Enums.DialogueNodeType.Branch)
                {
                    // �귻ġ �̸� ����
                    if (node.Group != null)
                    {
                        foreach (D_BranchSaveData branch in node.Branchs)
                        {
                            branchNames.Add(CreateBranchesLocalizedTextKey(dialogueContainer.FileName, node.Group.title, node.DialogueName, branch.branchName));
                        }
                    }
                    else
                    {
                        foreach (D_BranchSaveData branch in node.Branchs)
                        {
                            branchNames.Add(CreateBranchesLocalizedTextKey(dialogueContainer.FileName, node.DialogueName, branch.branchName));
                        }
                    }

                }

                // �׷� �̸� ����
                if (node.Group != null)
                {
                    groupedNodeNames.AddItem(node.Group.title, node.DialogueName);

                    continue;
                }

                // �׷��� ���� ��� �̸� ����
                ungroupedNodeNames.Add(node.DialogueName);
            }

            // �귻ġ ������Ʈ
            UpdateDialogueBranchConnections();
            UpdateOldBranchs(branchNames, graphData);

            // �׷� ������Ʈ
            UpdateOldGroupedNode(groupedNodeNames, graphData);

            // �׷��� ���� ��� ������Ʈ
            UpdateOldUngroupsNodes(ungroupedNodeNames, graphData);
        }

        private static void SaveNodeToGraph(Dialogue_Node node, D_GraphSaveDataSO graphData)
        {
            List<D_BranchSaveData> branches = CloneNodeBranchs(node.Branchs);
            string localizedTextKey;

            // �귻ġ����� �귻ġ�� ���ö���¡ Ű�� �����Ѵ�
            if (node.DialogueType == Enums.DialogueNodeType.Branch)
            {
                // �׷� ����϶��� �׷��� �̸��� �߰��Ѵ�
                if (node.Group != null)
                {
                    foreach (D_BranchSaveData branch in branches)
                    {
                        branch.LocalizedTextKey = CreateBranchesLocalizedTextKey(graphData.FileName, node.Group.title, node.DialogueName, branch.branchName);
                    }
                }
                // �׷� ��尡 �ƴҶ��� ����� �̸��� �߰��Ѵ�
                else
                {
                    foreach (D_BranchSaveData branch in branches)
                    {
                        branch.LocalizedTextKey = CreateBranchesLocalizedTextKey(graphData.FileName, node.DialogueName, branch.branchName);
                    }
                }
            }

            // ��ȭ�� ���ö���¡ Ű�� �����Ѵ�
            // �׷� ����϶��� �׷��� �̸��� �߰��Ѵ�
            if (node.Group != null)
            {
                localizedTextKey = CreateNodesLocalizedTextKey(graphData.FileName, node.Group.title, node.DialogueName);
            }
            // �׷� ��尡 �ƴҶ��� ����� �̸��� �߰��Ѵ�
            else
            {
                localizedTextKey = CreateNodesLocalizedTextKey(graphData.FileName, node.DialogueName);
            }

            // ������ ������ ��ü�� ����
            D_NodeSaveData nodeData = new D_NodeSaveData()
            {
                ID = node.ID,
                Name = node.DialogueName,
                Branchs = branches,
                Speaker = node.Speaker,
                Ko_Text = node.Ko_Text,
                Ja_Text = node.Ja_Text,
                En_Text = node.En_Text,
                LocalizedTextKey = localizedTextKey,
                GroupID = node.Group?.ID,
                DialogueType = node.DialogueType,
                Position = node.GetPosition().position
            };

            // �׷��������Ϳ� ��带 �߰�
            graphData.Nodes.Add(nodeData);
        }

        private static void SaveNodeToScriptableObject(Dialogue_Node node, D_DialogueContainerSO dialogueContainer)
        {
            D_DialogueSO dialogue;
            string groupName;
            string localizedTextKey;

            // ���ڿ� ���̺��� �����´�
            var table = LocalizationEditorSettings.GetStringTableCollection("DialogueText");

            if (node.Group != null) // �׷����� �׷��� �̸��� �����Ͽ� ����
            {
                groupName = node.Group.title;
                dialogue = CreateAsset<D_DialogueSO>($"{containerFolderPath}/Groups/{node.Group.title}/Dialogues", node.DialogueName);

                dialogueContainer.DialogueGroups.AddItem(createdDialogueGroups[node.Group.ID], dialogue);

                localizedTextKey = CreateNodesLocalizedTextKey(dialogueContainer.FileName, groupName, node.DialogueName);
            }
            else // �׷��尡 �ƴ϶�� �׷��� �̸��� �������� �ʰ� ����
            {
                groupName = "";
                dialogue = CreateAsset<D_DialogueSO>($"{containerFolderPath}/Global/Dialogues", node.DialogueName);

                dialogueContainer.UngroupedDialogues.Add(dialogue);

                localizedTextKey = CreateNodesLocalizedTextKey(dialogueContainer.FileName, node.DialogueName);
            }

            // ��ȭ�� ���ö���¡ Ű�� �����Ѵ�
            if (!table.SharedData.Contains(localizedTextKey))
            {
                AddLocalizationKey(table, localizedTextKey);
            }

            // �귻ġ�� �����͸� ��ȯ�Ͽ� �����´�
            List<D_DialoguebranchData> savedBranchDatas = ConvertNodeBranchsToDialogueBranchs(node.Branchs);

            // �귻ġ����� �귻ġ�� ���ö���¡ Ű�� �����Ѵ�
            if (node.DialogueType == Enums.DialogueNodeType.Branch)
            {
                if (node.Group != null) // �׷� ����϶��� �׷��� �̸��� �߰��Ѵ�
                {
                    foreach (D_DialoguebranchData branch in savedBranchDatas) // �귻ġ ����Ʈ�� ��ȸ
                    {
                        branch.LocalizedTextKey = CreateBranchesLocalizedTextKey(dialogueContainer.FileName, node.Group.title, node.DialogueName, branch.BranchName);
                        if (!table.SharedData.Contains(branch.LocalizedTextKey))
                        {
                            AddLocalizationKey(table, branch.LocalizedTextKey);
                        }

                        // ���ö���¡�� �ؽ�Ʈ�� �����Ѵ�
                        branch.SaveLocalizedText();
                    }
                }
                else // �׷� ��尡 �ƴҶ��� ����� �̸��� �߰��Ѵ�
                {
                    foreach (D_DialoguebranchData branch in savedBranchDatas)
                    {
                        branch.LocalizedTextKey = CreateBranchesLocalizedTextKey(dialogueContainer.FileName, node.DialogueName, branch.BranchName);
                        if (!table.SharedData.Contains(branch.LocalizedTextKey))
                        {
                            AddLocalizationKey(table, branch.LocalizedTextKey);
                        }
                        branch.SaveLocalizedText();
                    }
                }
            }
            else // �귻ġ��尡 �ƴ϶�� �귻ġ�� ���ö���¡ Ű�� ����
            {
                foreach (D_DialoguebranchData branch in savedBranchDatas)
                {
                    branch.LocalizedTextKey = "";
                }
            }

            // ��ȭ������ �ʱ�ȭ�Ѵ�
            dialogue.Initialize
            (
                node.DialogueName,
                groupName,
                node.Speaker,
                node.Ko_Text,
                node.Ja_Text,
                node.En_Text,
                savedBranchDatas,
                node.DialogueType,
                node.IsStartingNode(),
                localizedTextKey
            );

            // ���ö���¡�� �ؽ�Ʈ�� ����
            dialogue.SaveLocalizedText();

            // ������ ��ȭ ����Ʈ�� �߰��Ѵ�
            createdDialogues.Add(node.ID, dialogue);

            // ���� ��ȭ������ ������ ����
            SaveAsset(dialogue);
        }

        private static List<D_DialoguebranchData> ConvertNodeBranchsToDialogueBranchs(List<D_BranchSaveData> nodeBranchs)
        {
            List<D_DialoguebranchData> dialoguebranchs = new List<D_DialoguebranchData>();

            // ����� �귻ġ�� ��ȸ�ϸ� ��ȭ�귻ġ �����ͷ� ��ȯ�Ѵ�
            foreach (D_BranchSaveData nodeBranch in nodeBranchs)
            {
                D_DialoguebranchData brandata = new D_DialoguebranchData()
                {
                    BranchName = nodeBranch.branchName,
                    Ko_Text = nodeBranch.Ko_Text,
                    En_Text = nodeBranch.En_Text,
                    Ja_Text = nodeBranch.Ja_Text
                };

                // ���ö���¡�� �ؽ�Ʈ�� �����Ѵ�
                dialoguebranchs.Add(brandata);
            }

            // ��ȯ�� ��ȭ�귻ġ �����͸� ��ȯ�Ѵ�
            return dialoguebranchs;
        }

        private static void UpdateDialogueBranchConnections()
        {
            // ��帮��Ʈ�� ��ȸ�ϸ� �귻ġ�� ������ ������Ʈ�Ѵ�
            foreach (Dialogue_Node node in nodes)
            {
                D_DialogueSO dialogue = createdDialogues[node.ID];

                // �귻ġ�� ����� ��带 ã�� �����Ѵ�
                for (int branchIndex = 0; branchIndex < node.Branchs.Count; branchIndex++)
                {
                    D_BranchSaveData nodeBranch = node.Branchs[branchIndex];

                    // ��忡 ����� �귻ġ�� ���ٸ� ���� �귻ġ�� �Ѿ��
                    if (string.IsNullOrEmpty(nodeBranch.NodeId))
                    {
                        continue;
                    }

                    dialogue.Branchs[branchIndex].NextDialogue = createdDialogues[nodeBranch.NodeId];

                    // ��ȭ������ �����Ѵ�
                    SaveAsset(dialogue);
                }
            }
        }

        private static void UpdateOldGroups(List<string> currentGroupNames, D_GraphSaveDataSO graphData)
        {
            // �׷��� �����Ǿ��ٸ� ������ �����Ѵ�
            if (graphData.OldGroupNames != null && graphData.OldGroupNames.Count != 0)
            {
                List<string> groupsToRemove = graphData.OldGroupNames.Except(currentGroupNames).ToList();

                foreach (string groupToRemove in groupsToRemove)
                {
                    RemoveFolder($"{containerFolderPath}/Groups/{groupToRemove}");
                }
            }

            // ���� �׷��� �̸��� �����Ѵ�
            graphData.OldGroupNames = new List<string>(currentGroupNames);
        }

        private static void UpdateOldGroupedNode(SerializableDictionary<string, List<string>> currentGroupedNodeNames, D_GraphSaveDataSO graphData)
        {
            var table = LocalizationEditorSettings.GetStringTableCollection("DialogueText");

            // ��尡 �����Ǿ��ٸ� ������ �����Ѵ�
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

                        // ���ö���¡�� �ؽ�Ʈ�� �����Ѵ�
                        string localizedTextKey = CreateNodesLocalizedTextKey(graphData.FileName, oldgroupedNode.Key, nodeToRemove);
                        RemoveLocalizationKey(table, localizedTextKey);
                    }
                }
            }

            // ���� ����� �̸��� �����Ѵ�
            graphData.OldGroupedNodeNames = new SerializableDictionary<string, List<string>>(currentGroupedNodeNames);
        }

        private static void UpdateOldUngroupsNodes(List<string> currentUngroupedNodeNames, D_GraphSaveDataSO graphData)
        {
            var table = LocalizationEditorSettings.GetStringTableCollection("DialogueText");

            // ��尡 �����Ǿ��ٸ� ������ �����Ѵ�
            if (graphData.OldUngroupedNodeNames != null && graphData.OldUngroupedNodeNames.Count != 0)
            {
                List<string> nodesToRemove = graphData.OldUngroupedNodeNames.Except(currentUngroupedNodeNames).ToList();
                
                foreach (string nodeToRemove in nodesToRemove)
                {
                    RemoveAsset($"{containerFolderPath}/Global/Dialogues", nodeToRemove);

                    // ���ö���¡�� �ؽ�Ʈ�� �����Ѵ�
                    string localizedTextKey = CreateNodesLocalizedTextKey(graphData.FileName, nodeToRemove);
                    RemoveLocalizationKey(table, localizedTextKey);
                }
            }

            // ���� ����� �̸��� �����Ѵ�
            graphData.OldUngroupedNodeNames = new List<string>(currentUngroupedNodeNames);
        }

        private static void UpdateOldBranchs(List<string> currentBranchNames, D_GraphSaveDataSO graphData)
        {
            var table = LocalizationEditorSettings.GetStringTableCollection("DialogueText");

            // �귻ġ�� �����Ǿ��ٸ� ���ö���¡�� �ؽ�Ʈ�� �����Ѵ�
            if (graphData.OldBranchNames != null && graphData.OldBranchNames.Count != 0)
            {
                List<string> branchsToRemove = graphData.OldBranchNames.Except(currentBranchNames).ToList();

                foreach (string branchToRemove in branchsToRemove)
                {
                    RemoveLocalizationKey(table, branchToRemove);
                }
            }

            // ���� �귻ġ�� �̸��� �����Ѵ�
            graphData.OldBranchNames = new List<string>(currentBranchNames);
        }
        #endregion

        #endregion

        #region Load Methods / �ҷ����� �޼ҵ�
        public static void Load()
        {
            D_GraphSaveDataSO graphData = LoadAsset<D_GraphSaveDataSO>("Assets/DialogueSystem/Editor/Graphs", graphFileName);

            // �׷��� �����Ͱ� ���ٸ� ������ ǥ���ϰ� �����Ѵ�
            if (graphData == null)
            {
                EditorUtility.DisplayDialog("Error", "No graph data found", "OK");

                return;
            }

            // �׷��� �����͸� �ε��Ѵ�
            DialogueEditorWindow.UpdateFileName(graphData.FileName);

            // �׷����� �ʱ�ȭ�Ѵ�
            LoadGroups(graphData.Groups);
            LoadNodes(graphData.Nodes);
            LoadNodesConnections();
        }

        private static void LoadGroups(List<D_GroupSaveData> groups)
        {
            foreach (D_GroupSaveData groupDate in groups)
            {
                Dialogue_Group group = graphView.CreateGroup(groupDate.Name, groupDate.Position);

                group.ID = groupDate.ID;

                loadedGroups.Add(group.ID, group);
            }
        }

        private static void LoadNodes(List<D_NodeSaveData> nodes)
        {
            var table = LocalizationEditorSettings.GetStringTableCollection("DialogueText");

            foreach (D_NodeSaveData nodeData in nodes)
            {
                // �귻ġ ������ ��带 �����Ѵ�
                List<D_BranchSaveData> branches = CloneNodeBranchs(nodeData.Branchs);
                Dialogue_Node node = graphView.CreateNode(nodeData.Name, nodeData.DialogueType, nodeData.Position, false);

                // ��� ������ �ʱ�ȭ
                node.ID = nodeData.ID;
                node.Branchs = branches;
                node.Speaker = nodeData.Speaker;

                // ���ö���¡�� �ؽ�Ʈ�� �ҷ��´�
                if (table.SharedData.Contains(nodeData.LocalizedTextKey))
                {
                    LoadLocalizedTexts(table, nodeData.LocalizedTextKey, out string ko_s, out string ja_s, out string en_s);
                    node.Ko_Text = ko_s;
                    node.Ja_Text = ja_s;
                    node.En_Text = en_s;
                }
                else
                {
                    node.Ko_Text = nodeData.Ko_Text;
                    node.Ja_Text = nodeData.Ja_Text;
                    node.En_Text = nodeData.En_Text;
                }

                // �귻ġ ����� �귻ġ�� ���ö���¡ �ؽ�Ʈ�� �ҷ��´�
                if (node.DialogueType == Enums.DialogueNodeType.Branch)
                {
                    foreach (D_BranchSaveData branch in node.Branchs)
                    {
                        if (table.SharedData.Contains(branch.LocalizedTextKey))
                        {
                            LoadLocalizedTexts(table, branch.LocalizedTextKey, out string b_ko_s, out string b_ja_s, out string b_en_s);

                            branch.Ko_Text = b_ko_s;
                            branch.Ja_Text = b_ja_s;
                            branch.En_Text = b_en_s;
                        }
                    }
                }

                // ��带 �׷����� �߰��Ѵ�
                node.Draw();

                // ��带 �ε�� ��� ����Ʈ�� �߰��Ѵ�
                graphView.AddElement(node);
                loadedNodes.Add(node.ID, node);

                if (string.IsNullOrEmpty(nodeData.GroupID))
                {
                    continue;
                }

                // �׷쿡 ��带 �߰��Ѵ�
                Dialogue_Group group = loadedGroups[nodeData.GroupID];
                node.Group = group;
                group.AddElement(node);
            }
        }

        private static void LoadNodesConnections()
        {
            foreach (KeyValuePair<string, Dialogue_Node> loadedNode in loadedNodes)
            {
                foreach (Port branchPort in loadedNode.Value.outputContainer.Children())
                {
                    D_BranchSaveData branchData = branchPort.userData as D_BranchSaveData;

                    if (string.IsNullOrEmpty(branchData.NodeId))
                    {
                        continue;
                    }

                    Dialogue_Node nextNode = loadedNodes[branchData.NodeId];

                    Port nextNodeInputPort = nextNode.inputContainer.Children().First() as Port;

                    Edge edge = branchPort.ConnectTo(nextNodeInputPort);

                    graphView.AddElement(edge);

                    loadedNode.Value.RefreshPorts();
                }
            }
        }
        #endregion

        #region Creation Methods / ���� �޼ҵ�
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

        #region Fetch Methods / �������� �޼ҵ�
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

        #region Utility Methods / ��ƿ��Ƽ �޼ҵ�
        public static void CreateFolder(string path, string folderName)
        {
            if (AssetDatabase.IsValidFolder($"{path}/{folderName}"))
            {
                return;
            }

            AssetDatabase.CreateFolder(path, folderName);
        }

        public static void RemoveFolder(string fullPath)
        {
            FileUtil.DeleteFileOrDirectory($"{fullPath}.meta");
            FileUtil.DeleteFileOrDirectory($"{fullPath}/");
        }

        public static T CreateAsset<T>(string path, string assetName) where T : ScriptableObject
        {
            string fullPath = $"{path}/{assetName}.asset";
            T asset = LoadAsset<T>(path, assetName);

            if (asset == null)
            {
                asset = ScriptableObject.CreateInstance<T>();

                AssetDatabase.CreateAsset(asset, fullPath);
            }

            return asset;
        }

        public static T LoadAsset<T>(string path, string assetName) where T : ScriptableObject
        {
            string fullPath = $"{path}/{assetName}.asset";
            return AssetDatabase.LoadAssetAtPath<T>(fullPath);
        }

        public static void RemoveAsset(string path, string assetName)
        {
            AssetDatabase.DeleteAsset($"{path}/{assetName}.asset");
        }

        public static void SaveAsset(UnityEngine.Object asset)
        {
            EditorUtility.SetDirty(asset);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        private static List<D_BranchSaveData> CloneNodeBranchs(List<D_BranchSaveData> nodeBranchs)
        {
            List<D_BranchSaveData> branches = new List<D_BranchSaveData>();

            foreach (D_BranchSaveData branch in nodeBranchs)
            {
                D_BranchSaveData branchData = new D_BranchSaveData()
                {
                    branchName = branch.branchName,
                    Ko_Text = branch.Ko_Text,
                    Ja_Text = branch.Ja_Text,
                    En_Text = branch.En_Text,
                    LocalizedTextKey = branch.LocalizedTextKey,
                    NodeId = branch.NodeId
                };

                branches.Add(branchData);
            }

            return branches;
        }
        #endregion

        #region Localization Methods / ���ö������̼� �޼ҵ�
        private static void AddLocalizationKey(StringTableCollection tableCol, string key)
        {
            tableCol.SharedData.AddKey(key);

            EditorUtility.SetDirty(tableCol);
            EditorUtility.SetDirty(tableCol.SharedData);
        }

        private static void RemoveLocalizationKey(StringTableCollection tableCol, string key)
        {
            tableCol.SharedData.RemoveKey(key);

            EditorUtility.SetDirty(tableCol);
            EditorUtility.SetDirty(tableCol.SharedData);
        }

        private static void LoadLocalizedTexts(StringTableCollection tableCol, string key, out string koText, out string enText, out string jaText)
        {
            StringTable ko_Table = tableCol.GetTable("ko") as StringTable;
            koText = ko_Table.GetEntry(key).Value;

            StringTable en_Table = tableCol.GetTable("en") as StringTable;
            enText = en_Table.GetEntry(key).Value;

            StringTable ja_Table = tableCol.GetTable("ja") as StringTable;
            jaText = ja_Table.GetEntry(key).Value;

            EditorUtility.SetDirty(tableCol);
            EditorUtility.SetDirty(tableCol.SharedData);
        }

        private static string CreateNodesLocalizedTextKey(string fileName, string NodeName)
        {
            return $"st_D: {fileName} - {NodeName}";
        }
        private static string CreateNodesLocalizedTextKey(string fileName, string groupName, string NodeName)
        {
            return $"st_D: {fileName} - {groupName} - {NodeName}";
        }

        private static string CreateBranchesLocalizedTextKey(string fileName, string NodeName, string branchName)
        {
            return $"st_B: {fileName} - {NodeName} - {branchName}";
        }

        private static string CreateBranchesLocalizedTextKey(string fileName, string groupName, string NodeName, string branchName)
        {
            return $"st_B: {fileName} - {groupName} - {NodeName} - {branchName}";
        }

        #endregion
    }
}
