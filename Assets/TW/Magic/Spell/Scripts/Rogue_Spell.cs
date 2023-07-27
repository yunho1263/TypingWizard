using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TypingWizard.Spells
{
    public class Rogue_Spell : Spell
    {
        public string aria;
        public List<string> modifiers;

        public virtual bool Modify(string modifier)
        {
            if (modifiers.Contains(modifier))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
