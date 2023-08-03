using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TypingWizard.Spells
{
    using Enums;
    using Damages;

    public class TestSpell_3 : Arcana_Spell
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
    }
}
