using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TypingWizard.SpellDictionary
{
    using Spells;
    using System;

    public class SpellDictionary : MonoBehaviour
    {
        protected Hashtable spellTable;

        public virtual void Initialize()
        {
            spellTable = new Hashtable();
        }
        public virtual bool Search(string input, out Spell spell)
        {
            spell = null;
            return false;
        }

        public virtual bool Search(List<string> words, out Spell spell)
        {
            spell = null;
            return false;
        }

        public virtual void AddSpell(Spell spell)
        {

        }
    }
}
