using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TypingWizard.Spells
{
    using Enums;
    using Damages;
    using TypingWizard.SpellDictionary;
    using UnityEditor.Experimental.GraphView;
    using static TypingWizard.Damages.Damage;

    public class TestSpell_4 : Rogue_Spell
    {
        public float radius;
        public Damage damage;
        public DamageScrObj d_ScrObj;


        public override void Initialize(Character caster)
        {
            base.Initialize(caster);
            damage = new Damage(d_ScrObj);
        }

        public override void Cast(Character caster)
        {
            base.Cast(caster);
            Debug.Log(damage.damageElement.ToString());
            LayerMask hitLayers = 1 << LayerMask.NameToLayer("Enemy");
            Collider[] colliders = Physics.OverlapSphere(caster.transform.position, radius, hitLayers);
            foreach (Collider collider in colliders)
            {
                Character character = collider.GetComponent<Character>();
                if (character != null)
                {
                    character.TakeDamage(damage);
                }
            }
        }

        public override bool CanModify(List<string> modifiers, Rogue_SpellDictionary rogue_SpellDictionary)
        {
            return rogue_SpellDictionary.elementalType.Contains(modifiers[0]);
        }

        public override void Modify(List<string> modifiers, Rogue_SpellDictionary rogue_SpellDictionary)
        {
            Rogue_Elemental elementalType;
            rogue_SpellDictionary.elementalType.GetValue(modifiers[0], out elementalType);

            switch (elementalType)
            {
                case Rogue_Elemental.None:
                    damage.damageElement = DamageElement.None;
                    break;
                case Rogue_Elemental.Fire:
                    damage.damageElement = DamageElement.Fire;
                    break;
                case Rogue_Elemental.Water:
                    damage.damageElement = DamageElement.Water;
                    break;
                case Rogue_Elemental.Cold:
                    damage.damageElement = DamageElement.Cold;
                    break;
                case Rogue_Elemental.Thunder:
                    damage.damageElement = DamageElement.Thunder;
                    break;
                case Rogue_Elemental.Earth:
                    damage.damageElement = DamageElement.Earth;
                    break;
                case Rogue_Elemental.Wind:
                    damage.damageElement = DamageElement.Wind;
                    break;
                case Rogue_Elemental.Light:
                    damage.damageElement = DamageElement.Light;
                    break;
                case Rogue_Elemental.Dark:
                    damage.damageElement = DamageElement.Dark;
                    break;
            }
        }
    }
}
