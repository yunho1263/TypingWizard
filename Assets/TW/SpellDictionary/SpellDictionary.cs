using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TypingWizard.SpellDictionary
{
    using Spells;
    public class SpellDictionary
    {
        protected Hashtable spellTable;

        public SpellDictionary()
        {
            Initialize();
        }

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
