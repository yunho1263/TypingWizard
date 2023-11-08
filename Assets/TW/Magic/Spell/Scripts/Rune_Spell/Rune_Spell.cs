using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TypingWizard.Spells
{
    using Enums;
    using TypingWizard.Spells.Utility;

    public class Rune_Spell : Spell
    {
        public virtual RuneSpell_Type runeSpell_Type { get; }
        public string aria;

        public Rune_Spell previousRune;
        public Rune_Spell nextRune;

        public List<Modifier_Rune> modifiys;
        public List<ModifiyableObject> modifiyableObjects;

        public override void Initialize(Character caster)
        {
            base.Initialize(caster);
            modifiys = new List<Modifier_Rune>();
            modifiyableObjects = new List<ModifiyableObject>();
        }

        public void ResetSpell()
        {
            nextRune.ResetSpell();
            nextRune = null;
            previousRune = null;
            Destroy(gameObject);
        }

        public virtual List<ModifiyableObject> ModifiyRequest(List<Rune_ModifiyType> modifiyTypes)
        {
            List<ModifiyableObject> modifiyableObjects = new List<ModifiyableObject>();

            foreach (Rune_ModifiyType modifiyType in modifiyTypes)
            {
                ModifiyableObject obj = this.modifiyableObjects.Find(x => x.modifiyType == modifiyType);
                if (obj != null)
                {
                    modifiyableObjects.Add(obj);
                }
                else
                {
                    return null;
                }
            }

            return modifiyableObjects;
        }
    }
}
