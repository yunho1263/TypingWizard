using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TypingWizard.SpellDictionary
{
    public abstract class SpellDictionary : MonoBehaviour
    {
        public abstract void Initialize();
        public abstract bool Search(string input, out Spell spell);
        public abstract void AddSpell(Spell spell);
        public abstract void Optimize();
        public abstract void Clear();
    }
}
