using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Spell : MonoBehaviour
{
    public SpellData spellData;
    public bool isAcquired;
    public void Cast(GameObject caster)
    {
        Debug.Log(spellData.SpellName.Search(GlobalSetting.Instance.gameSettings.language) +" "+ caster.gameObject.name);
    }
}
