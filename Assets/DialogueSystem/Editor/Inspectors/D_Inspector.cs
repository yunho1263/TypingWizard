using UnityEditor;

namespace DialogueSystem.Inspectors
{
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
            serializedObject.Update();

            DrawDialogueContainerArea();

            if (dialogueContainerProperty.objectReferenceValue == null)
            {
                StopDrawing("Select a Dialogue Container to see the rest of the Inspector");

                return;
            }

            DrawFilterArea();

            if (groupedDialogueProperty.boolValue)
            {
                DrawDialogueGroupArea();
            }


            DrawDialogueArea();

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

        private void DrawDialogueGroupArea()
        {
            D_InspectorUtility.DrawHeader("Dialogue Group");

            selectedDialogueGroupIndexProperty.intValue = D_InspectorUtility.DrawPopup("Dialogue Group", selectedDialogueGroupIndexProperty, new string[] { });

            dialogueGroupProperty.DrawPropertyField();

            D_InspectorUtility.DrawSpace();
        }

        private void DrawDialogueArea()
        {
            D_InspectorUtility.DrawHeader("Dialogue");

            selectedDialogueIndexProperty.intValue = D_InspectorUtility.DrawPopup("Dialogue", selectedDialogueIndexProperty, new string[] { });

            dialogueProperty.DrawPropertyField();

            D_InspectorUtility.DrawSpace();
        }

        private void StopDrawing(string reason)
        {
            D_InspectorUtility.DrawHelpBox(reason);

            serializedObject.ApplyModifiedProperties();
        }
        #endregion
    }
}
