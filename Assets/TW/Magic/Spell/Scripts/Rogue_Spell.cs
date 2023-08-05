using System.Collections;
using System.Collections.Generic;
using TypingWizard.SpellDictionary;
using UnityEngine;

namespace TypingWizard.Spells
{
    public class Rogue_Spell : Spell
    {
        public string aria;
        public List<string> modifiers;

        public virtual bool CanModify(List<string> modifiers, Rogue_SpellDictionary rogue_SpellDictionary)
        {
            return true;
        }

        public virtual void Modify(List<string> modifiers, Rogue_SpellDictionary rogue_SpellDictionary)
        {

        }
    }
}
