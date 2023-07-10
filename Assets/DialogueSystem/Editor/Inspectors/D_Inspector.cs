using UnityEditor;
using System.Collections.Generic;

namespace DialogueSystem.Inspectors
{
    using ScrObj;
    using Utilities;

    [CustomEditor(typeof(D_Dialogue))]
    public class D_Inspector : Editor
    {
        // 스크립터블 오브젝트
        private SerializedProperty dialogueContainerProperty;
        private SerializedProperty dialogueGroupProperty;
        private SerializedProperty dialogueProperty;


        // 필터
        private SerializedProperty groupedDialogueProperty;
        private SerializedProperty startingDialogueOnlyProperty;

        // 인덱스
        private SerializedProperty selectedDialogueGroupIndexProperty;
        private SerializedProperty selectedDialogueIndexProperty;

        private void OnEnable()
        {
            // 프로퍼티를 찾아서 적용
            dialogueContainerProperty = serializedObject.FindProperty("dialogueContainer");
            dialogueGroupProperty = serializedObject.FindProperty("dialogueGroup");
            dialogueProperty = serializedObject.FindProperty("dialogue");

            groupedDialogueProperty = serializedObject.FindProperty("groupedDialogue");
            startingDialogueOnlyProperty = serializedObject.FindProperty("startingDialogueOnly");

            selectedDialogueGroupIndexProperty = serializedObject.FindProperty("selectedDialogueGroupIndex");
            selectedDialogueIndexProperty = serializedObject.FindProperty("selectedDialogueIndex");
        }

        public override void OnInspectorGUI()
        {
            // 업데이트
            serializedObject.Update();

            // 다이얼로그 컨테이너 그리기
            DrawDialogueContainerArea();

            D_DialogueContainerSO dialogueContainer = dialogueContainerProperty.objectReferenceValue as D_DialogueContainerSO;

            // 다이얼로그 그룹이 없으면 그리지 않음
            if (dialogueContainer == null)
            {
                StopDrawing("Select a Dialogue Container to see the rest of the Inspector");

                return;
            }

            // 필터 그리기
            DrawFilterArea();

            bool currentStrartingDialogueOnlyFilter = startingDialogueOnlyProperty.boolValue;

            List<string> dialogueNames;
            string dialogueFolderPath = $"Assets/DialogueSystem/DialogueData/{dialogueContainer.FileName}";
            string dialogueInfoMessage;

            // 그룹화된 다이얼로그 그리기
            if (groupedDialogueProperty.boolValue)
            {
                List<string> dialogueGroupNames = dialogueContainer.GetDialogueGroupNames();

                // 다이얼로그 그룹이 없으면 그리지 않음
                if (dialogueGroupNames.Count == 0)
                {
                    StopDrawing("There are no Dialogue Groups in the selected Dialogue Container");

                    return;
                }

                // 다이얼로그 그룹 그리기
                DrawDialogueGroupArea(dialogueContainer, dialogueGroupNames);

                D_DialogueGroupSO dialogueGroup = dialogueGroupProperty.objectReferenceValue as D_DialogueGroupSO;

                dialogueNames = dialogueContainer.GetGroupedDialogueNames(dialogueGroup, currentStrartingDialogueOnlyFilter);
                dialogueFolderPath += $"/Groups/{dialogueGroup.GroupName}/Dialogues";
                dialogueInfoMessage = "There are no " + (currentStrartingDialogueOnlyFilter ? "starting" : "") + " Dialogues in the selected Dialogue Group";
            }
            else
            {
                dialogueNames = dialogueContainer.GetUngroupedDialogueNames(currentStrartingDialogueOnlyFilter);
                dialogueFolderPath += $"/Global/Dialogues";
                dialogueInfoMessage = "There are no " + (currentStrartingDialogueOnlyFilter ? "starting" : "") + " Ungrouped Dialogues in the selected Dialogue Container";
            }

            // 다이얼로그가 없으면 그리지 않음
            if (dialogueNames.Count == 0)
            {
                StopDrawing(dialogueInfoMessage);

                return;
            }

            // 다이얼로그 그리기
            DrawDialogueArea(dialogueNames, dialogueFolderPath);

            // 모디파이 프로퍼티 적용
            serializedObject.ApplyModifiedProperties();
        }

        #region Draw Methods / 그리기 메소드
        private void DrawDialogueContainerArea()
        {
            D_InspectorUtility.DrawHeader("Dialogue Container");

            dialogueContainerProperty.DrawPropertyField();

            D_InspectorUtility.DrawSpace();
        }
        
        private void DrawFilterArea()
        {
            D_InspectorUtility.DrawHeader("Filters");

            groupedDialogueProperty.DrawPropertyField();
            startingDialogueOnlyProperty.DrawPropertyField();

            D_InspectorUtility.DrawSpace();
        }

        private void DrawDialogueGroupArea(D_DialogueContainerSO dialogueContainer, List<string> dialogueGroupNames)
        {
            D_InspectorUtility.DrawHeader("Dialogue Group");

            // 필요한 변수 초기화
            int oldSelectedDialogueGroupIndex = selectedDialogueGroupIndexProperty.intValue;
            D_DialogueGroupSO oldDialogueGroup = dialogueGroupProperty.objectReferenceValue as D_DialogueGroupSO;
            bool isDialogueGroupNull = oldDialogueGroup == null;
            string oldDialogueGroupName = isDialogueGroupNull ? "" : oldDialogueGroup.name;

            UpdateIndexOnNamesListUpdate(dialogueGroupNames, selectedDialogueGroupIndexProperty, oldSelectedDialogueGroupIndex, oldDialogueGroupName, isDialogueGroupNull);

            selectedDialogueGroupIndexProperty.intValue = D_InspectorUtility.DrawPopup("Dialogue Group", selectedDialogueGroupIndexProperty, dialogueGroupNames.ToArray());

            string selectedDialogueGroupName = dialogueGroupNames[selectedDialogueGroupIndexProperty.intValue];

            D_DialogueGroupSO selectedDialogueGroup = D_IO_Utility.LoadAsset<D_DialogueGroupSO>($"Assets/DialogueSystem/DialogueData/{dialogueContainer.FileName}/Groups/{selectedDialogueGroupName}", selectedDialogueGroupName);

            dialogueGroupProperty.objectReferenceValue = selectedDialogueGroup;

            D_InspectorUtility.DrawDisabledField(() => dialogueGroupProperty.DrawPropertyField());

            D_InspectorUtility.DrawSpace();
        }

        private void DrawDialogueArea(List<string> dialogueNames, string dialogueFolderPath)
        {
            D_InspectorUtility.DrawHeader("Dialogue");

            // 필요한 변수 초기화
            int oldSelectedDialogueIndex = selectedDialogueIndexProperty.intValue;
            D_DialogueSO oldDialogue = dialogueProperty.objectReferenceValue as D_DialogueSO;
            bool isDialogueNull = oldDialogue == null;
            string oldDialogueName = isDialogueNull ? "" : oldDialogue.DialogueName;

            UpdateIndexOnNamesListUpdate(dialogueNames, selectedDialogueIndexProperty, oldSelectedDialogueIndex, oldDialogueName, isDialogueNull);

            selectedDialogueIndexProperty.intValue = D_InspectorUtility.DrawPopup("Dialogue", selectedDialogueIndexProperty, dialogueNames.ToArray());

            string selectedDialogueName = dialogueNames[selectedDialogueIndexProperty.intValue];

            D_DialogueSO selectedDialogue = D_IO_Utility.LoadAsset<D_DialogueSO>(dialogueFolderPath, selectedDialogueName);
            
            dialogueProperty.objectReferenceValue = selectedDialogue;

            D_InspectorUtility.DrawDisabledField(() => dialogueProperty.DrawPropertyField());

            D_InspectorUtility.DrawSpace();
        }

        private void StopDrawing(string reason, MessageType messageType = MessageType.Info)
        {
            D_InspectorUtility.DrawHelpBox(reason, messageType);
            D_InspectorUtility.DrawSpace();
            D_InspectorUtility.DrawHelpBox("you need to select a Dialogue for this component to work properly at Runtime!", MessageType.Warning);

            serializedObject.ApplyModifiedProperties();
        }
        #endregion

        #region Index Methods / 인덱스 메소드
        private void UpdateIndexOnNamesListUpdate(List<string> optionNames, SerializedProperty indexProperty, int oldSelectedPropertyIndex, string oldPropertyName, bool isOldPropertyNull)
        {
            if (isOldPropertyNull)
            {
                indexProperty.intValue = 0;

                return;
            }

            // 옵션 이름이 바뀌었을 때 인덱스 업데이트
            bool oldIndexisOutOfBoundsOfNamesListCount = oldSelectedPropertyIndex > optionNames.Count - 1;
            bool oldNameisDifferentThanSelectedName = oldIndexisOutOfBoundsOfNamesListCount || oldPropertyName != optionNames[oldSelectedPropertyIndex];

            if (oldNameisDifferentThanSelectedName)
            {
                if (optionNames.Contains(oldPropertyName))
                {
                    indexProperty.intValue = optionNames.IndexOf(oldPropertyName);
                }
                else
                {
                    indexProperty.intValue = 0;
                }
            }
        }
        #endregion
    }
}
