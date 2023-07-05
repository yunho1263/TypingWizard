using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DialogueSystem
{
    public class Dialogue
    {
        public enum DialogMode
        {
            Default,
            Cinematic,
            Auto
        }
        public DialogMode DialogueMode;

        public string DialogueName;
    }
}