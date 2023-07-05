using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

namespace TypingWizard
{
    [CreateAssetMenu(fileName = "DamegaScrObj", menuName = "Scriptable Object/Damage")]
    public class DamageScrObj : ScriptableObject
    {
        [SerializeField]
        Damage.DamageType damageType;
        public Damage.DamageType D_Type { get { return damageType; } }

        [SerializeField]
        Damage.DamageCategori damageCategori;
        public Damage.DamageCategori D_Categori { get { return damageCategori; } }

        [SerializeField]
        Damage.DamageElement damageElement;
        public Damage.DamageElement D_Element { get { return damageElement; } }

        [SerializeField]
        public LocalizedString damageName;
        public string D_Name { get { return damageName.GetLocalizedString(); } }

        [SerializeField]
        public LocalizedString description;
        public string D_Description { get { return description.GetLocalizedString(); } }
    }

}
