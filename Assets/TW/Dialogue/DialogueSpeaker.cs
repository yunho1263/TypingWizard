using DialogueSystem;
using DialogueSystem.ScrObj;
using UnityEngine;

namespace TypingWizard.Dialogue
{
    public class DialogueSpeaker : InteractiveObject
    {
        public string speakerName;
        public AudioClip typingSoundEffect;
        public D_Dialogue dialogue;
        public D_DialogueSO dialogueSO;

        public override void Interact()
        {
            DialogueManager.Instance.SetDialogue(dialogueSO);
            DialogueManager.Instance.StartDialogue();
        }

        public void SetDialogue(string dialogueName)
        {
            dialogueSO = dialogue.GetStartingDialogue(dialogueName);
        }
    }
}
