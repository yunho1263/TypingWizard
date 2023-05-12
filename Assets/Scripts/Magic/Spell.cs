using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell : MonoBehaviour
{
    public SpellData spellData;
    public bool isAcquired;
    public void Cast(GameObject caster)
    {
        Debug.Log(spellData.SpellName + caster.gameObject.name);
    }
}
