using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TypingWizard.Spells.Utility
{
    using Enums;
    using System;

    [Serializable]
    public class ModifiyableObject
    {
        public Rune_ModifiyType modifiyType;

        public object target;

        public ModifiyableObject(Rune_ModifiyType modifiyType, object target)
        {
            this.modifiyType = modifiyType;
            this.target = target;
        }
    }
}
