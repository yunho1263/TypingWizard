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

        // 그래프 뷰에서 가져온 데이터
        private static List<Dialogue_Group> groups;
        private static List<Dialogue_Node> nodes;

        // 그래프 뷰에서 가져온 데이터를 ScriptableObject로 만들어 저장할 데이터
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


        #region Save Methods / 저장 메소드
        public static void Save()
        {
            // 폴더 생성
            CreateStaticFolders();

            // 그래프뷰에서 데이터 가져오기
            GetElementsFromGraphView();

            // 저장할 데이터 생성
            D_GraphSaveDataSO graphData = CreateAsset<D_GraphSaveDataSO>("Assets/DialogueSystem/Editor/Graphs", $"{graphFileName}Graph");
            graphData.Initialize(graphFileName);

            D_DialogueContainerSO dialogueContainer  = CreateAsset<D_DialogueContainerSO>(containerFolderPath, graphFileName);
            dialogueContainer.Initialize(graphFileName);

            // 데이터 저장
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
        #endregion

        #region Nodes / 노드
        private static void SaveNodes(D_GraphSaveDataSO graphData, D_DialogueContainerSO dialogueContainer)
        {
            SerializableDictionary<string, List<string>> groupedNodeNames = new SerializableDictionary<string, List<string>>();
            List<string> ungroupedNodeNames = new List<string>();
            List<string> branchNames = new List<string>();

            foreach (Dialogue_Node node in nodes)
            {
                // 노드 저장
                SaveNodeToGraph(node, graphData);
                SaveNodeToScriptableObject(node, dialogueContainer);

                if (node.DialogueType == Enums.DialogueNodeType.Branch)
                {
                    // 브렌치 이름 저장
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

                // 그룹 이름 저장
                if (node.Group != null)
                {
                    groupedNodeNames.AddItem(node.Group.title, node.DialogueName);

                    continue;
                }

                // 그룹이 없는 노드 이름 저장
                ungroupedNodeNames.Add(node.DialogueName);
            }

            // 브렌치 업데이트
            UpdateDialogueBranchConnections();
            UpdateOldBranchs(branchNames, graphData);

            // 그룹 업데이트
            UpdateOldGroupedNode(groupedNodeNames, graphData);

            // 그룹이 없는 노드 업데이트
            UpdateOldUngroupsNodes(ungroupedNodeNames, graphData);
        }

        private static void SaveNodeToGraph(Dialogue_Node node, D_GraphSaveDataSO graphData)
        {
            List<D_BranchSaveData> branches = CloneNodeBranchs(node.Branchs);
            string localizedTextKey;

            // 브렌치노드라면 브렌치의 로컬라이징 키를 저장한다
            if (node.DialogueType == Enums.DialogueNodeType.Branch)
            {
                // 그룹 노드일때는 그룹의 이름을 추가한다
                if (node.Group != null)
                {
                    foreach (D_BranchSaveData branch in branches)
                    {
                        branch.LocalizedTextKey = CreateBranchesLocalizedTextKey(graphData.FileName, node.Group.title, node.DialogueName, branch.branchName);
                    }
                }
                // 그룹 노드가 아닐때는 노드의 이름만 추가한다
                else
                {
                    foreach (D_BranchSaveData branch in branches)
                    {
                        branch.LocalizedTextKey = CreateBranchesLocalizedTextKey(graphData.FileName, node.DialogueName, branch.branchName);
                    }
                }
            }

            // 대화의 로컬라이징 키를 저장한다
            // 그룹 노드일때는 그룹의 이름을 추가한다
            if (node.Group != null)
            {
                localizedTextKey = CreateNodesLocalizedTextKey(graphData.FileName, node.Group.title, node.DialogueName);
            }
            // 그룹 노드가 아닐때는 노드의 이름만 추가한다
            else
            {
                localizedTextKey = CreateNodesLocalizedTextKey(graphData.FileName, node.DialogueName);
            }

            // 저장할 데이터 객체를 생성
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

            // 그래프데이터에 노드를 추가
            graphData.Nodes.Add(nodeData);
        }

        private static void SaveNodeToScriptableObject(Dialogue_Node node, D_DialogueContainerSO dialogueContainer)
        {
            D_DialogueSO dialogue;
            string groupName;
            string localizedTextKey;

            // 문자열 테이블을 가져온다
            var table = LocalizationEditorSettings.GetStringTableCollection("DialogueText");

            if (node.Group != null) // 그룹노드라면 그룹의 이름을 포함하여 저장
            {
                groupName = node.Group.title;
                dialogue = CreateAsset<D_DialogueSO>($"{containerFolderPath}/Groups/{node.Group.title}/Dialogues", node.DialogueName);

                dialogueContainer.DialogueGroups.AddItem(createdDialogueGroups[node.Group.ID], dialogue);

                localizedTextKey = CreateNodesLocalizedTextKey(dialogueContainer.FileName, groupName, node.DialogueName);
            }
            else // 그룹노드가 아니라면 그룹의 이름을 포함하지 않고 저장
            {
                groupName = "";
                dialogue = CreateAsset<D_DialogueSO>($"{containerFolderPath}/Global/Dialogues", node.DialogueName);

                dialogueContainer.UngroupedDialogues.Add(dialogue);

                localizedTextKey = CreateNodesLocalizedTextKey(dialogueContainer.FileName, node.DialogueName);
            }

            // 대화의 로컬라이징 키를 저장한다
            if (!table.SharedData.Contains(localizedTextKey))
            {
                AddLocalizationKey(table, localizedTextKey);
            }

            // 브렌치의 데이터를 변환하여 가져온다
            List<D_DialoguebranchData> savedBranchDatas = ConvertNodeBranchsToDialogueBranchs(node.Branchs);

            // 브렌치노드라면 브렌치의 로컬라이징 키를 저장한다
            if (node.DialogueType == Enums.DialogueNodeType.Branch)
            {
                if (node.Group != null) // 그룹 노드일때는 그룹의 이름을 추가한다
                {
                    foreach (D_DialoguebranchData branch in savedBranchDatas) // 브렌치 리스트를 순회
                    {
                        branch.LocalizedTextKey = CreateBranchesLocalizedTextKey(dialogueContainer.FileName, node.Group.title, node.DialogueName, branch.BranchName);
                        if (!table.SharedData.Contains(branch.LocalizedTextKey))
                        {
                            AddLocalizationKey(table, branch.LocalizedTextKey);
                        }

                        // 로컬라이징된 텍스트를 저장한다
                        branch.SaveLocalizedText();
                    }
                }
                else // 그룹 노드가 아닐때는 노드의 이름만 추가한다
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
            else // 브렌치노드가 아니라면 브렌치의 로컬라이징 키를 비운다
            {
                foreach (D_DialoguebranchData branch in savedBranchDatas)
                {
                    branch.LocalizedTextKey = "";
                }
            }

            // 대화정보를 초기화한다
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

            // 로컬라이징된 텍스트를 저장
            dialogue.SaveLocalizedText();

            // 생성된 대화 리스트에 추가한다
            createdDialogues.Add(node.ID, dialogue);

            // 만든 대화정보의 에셋을 저장
            SaveAsset(dialogue);
        }

        private static List<D_DialoguebranchData> ConvertNodeBranchsToDialogueBranchs(List<D_BranchSaveData> nodeBranchs)
        {
            List<D_DialoguebranchData> dialoguebranchs = new List<D_DialoguebranchData>();

            // 노드의 브렌치를 순회하며 대화브렌치 데이터로 변환한다
            foreach (D_BranchSaveData nodeBranch in nodeBranchs)
            {
                D_DialoguebranchData brandata = new D_DialoguebranchData()
                {
                    BranchName = nodeBranch.branchName,
                    Ko_Text = nodeBranch.Ko_Text,
                    En_Text = nodeBranch.En_Text,
                    Ja_Text = nodeBranch.Ja_Text
                };

                // 로컬라이징된 텍스트를 저장한다
                dialoguebranchs.Add(brandata);
            }

            // 변환된 대화브렌치 데이터를 반환한다
            return dialoguebranchs;
        }

        private static void UpdateDialogueBranchConnections()
        {
            // 노드리스트를 순회하며 브렌치의 연결을 업데이트한다
            foreach (Dialogue_Node node in nodes)
            {
                D_DialogueSO dialogue = createdDialogues[node.ID];

                // 브렌치에 연결된 노드를 찾아 연결한다
                for (int branchIndex = 0; branchIndex < node.Branchs.Count; branchIndex++)
                {
                    D_BranchSaveData nodeBranch = node.Branchs[branchIndex];

                    // 노드에 연결된 브렌치가 없다면 다음 브렌치로 넘어간다
                    if (string.IsNullOrEmpty(nodeBranch.NodeId))
                    {
                        continue;
                    }

                    dialogue.Branchs[branchIndex].NextDialogue = createdDialogues[nodeBranch.NodeId];

                    // 대화정보를 저장한다
                    SaveAsset(dialogue);
                }
            }
        }

        private static void UpdateOldGroups(List<string> currentGroupNames, D_GraphSaveDataSO graphData)
        {
            // 그룹이 삭제되었다면 폴더를 삭제한다
            if (graphData.OldGroupNames != null && graphData.OldGroupNames.Count != 0)
            {
                List<string> groupsToRemove = graphData.OldGroupNames.Except(currentGroupNames).ToList();

                foreach (string groupToRemove in groupsToRemove)
                {
                    RemoveFolder($"{containerFolderPath}/Groups/{groupToRemove}");
                }
            }

            // 현재 그룹의 이름을 저장한다
            graphData.OldGroupNames = new List<string>(currentGroupNames);
        }

        private static void UpdateOldGroupedNode(SerializableDictionary<string, List<string>> currentGroupedNodeNames, D_GraphSaveDataSO graphData)
        {
            var table = LocalizationEditorSettings.GetStringTableCollection("DialogueText");

            // 노드가 삭제되었다면 에셋을 삭제한다
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

                        // 로컬라이징된 텍스트를 삭제한다
                        string localizedTextKey = CreateNodesLocalizedTextKey(graphData.FileName, oldgroupedNode.Key, nodeToRemove);
                        RemoveLocalizationKey(table, localizedTextKey);
                    }
                }
            }

            // 현재 노드의 이름을 저장한다
            graphData.OldGroupedNodeNames = new SerializableDictionary<string, List<string>>(currentGroupedNodeNames);
        }

        private static void UpdateOldUngroupsNodes(List<string> currentUngroupedNodeNames, D_GraphSaveDataSO graphData)
        {
            var table = LocalizationEditorSettings.GetStringTableCollection("DialogueText");

            // 노드가 삭제되었다면 에셋을 삭제한다
            if (graphData.OldUngroupedNodeNames != null && graphData.OldUngroupedNodeNames.Count != 0)
            {
                List<string> nodesToRemove = graphData.OldUngroupedNodeNames.Except(currentUngroupedNodeNames).ToList();
                
                foreach (string nodeToRemove in nodesToRemove)
                {
                    RemoveAsset($"{containerFolderPath}/Global/Dialogues", nodeToRemove);

                    // 로컬라이징된 텍스트를 삭제한다
                    string localizedTextKey = CreateNodesLocalizedTextKey(graphData.FileName, nodeToRemove);
                    RemoveLocalizationKey(table, localizedTextKey);
                }
            }

            // 현재 노드의 이름을 저장한다
            graphData.OldUngroupedNodeNames = new List<string>(currentUngroupedNodeNames);
        }

        private static void UpdateOldBranchs(List<string> currentBranchNames, D_GraphSaveDataSO graphData)
        {
            var table = LocalizationEditorSettings.GetStringTableCollection("DialogueText");

            // 브렌치가 삭제되었다면 로컬라이징된 텍스트를 삭제한다
            if (graphData.OldBranchNames != null && graphData.OldBranchNames.Count != 0)
            {
                List<string> branchsToRemove = graphData.OldBranchNames.Except(currentBranchNames).ToList();

                foreach (string branchToRemove in branchsToRemove)
                {
                    RemoveLocalizationKey(table, branchToRemove);
                }
            }

            // 현재 브렌치의 이름을 저장한다
            graphData.OldBranchNames = new List<string>(currentBranchNames);
        }
        #endregion

        #endregion

        #region Load Methods / 불러오기 메소드
        public static void Load()
        {
            D_GraphSaveDataSO graphData = LoadAsset<D_GraphSaveDataSO>("Assets/DialogueSystem/Editor/Graphs", graphFileName);

            // 그래프 데이터가 없다면 에러를 표시하고 리턴한다
            if (graphData == null)
            {
                EditorUtility.DisplayDialog("Error", "No graph data found", "OK");

                return;
            }

            // 그래프 데이터를 로드한다
            DialogueEditorWindow.UpdateFileName(graphData.FileName);

            // 그래프를 초기화한다
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
                // 브렌치 정보와 노드를 생성한다
                List<D_BranchSaveData> branches = CloneNodeBranchs(nodeData.Branchs);
                Dialogue_Node node = graphView.CreateNode(nodeData.Name, nodeData.DialogueType, nodeData.Position, false);

                // 노드 정보를 초기화
                node.ID = nodeData.ID;
                node.Branchs = branches;
                node.Speaker = nodeData.Speaker;

                // 로컬라이징된 텍스트를 불러온다
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

                // 브렌치 노드라면 브렌치의 로컬라이징 텍스트를 불러온다
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

                // 노드를 그래프에 추가한다
                node.Draw();

                // 노드를 로드된 노드 리스트에 추가한다
                graphView.AddElement(node);
                loadedNodes.Add(node.ID, node);

                if (string.IsNullOrEmpty(nodeData.GroupID))
                {
                    continue;
                }

                // 그룹에 노드를 추가한다
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

        #region Utility Methods / 유틸리티 메소드
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

        #region Localization Methods / 로컬라이제이션 메소드
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
