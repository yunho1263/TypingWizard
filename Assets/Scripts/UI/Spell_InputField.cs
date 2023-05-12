using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Spell_InputField : MonoBehaviour
{
    public TMP_InputField inputField;

    string spell;

    public void UpdateField()
    {
        if (inputField.text.EndsWith('\n')) // 엔터키 입력시
        {
            spell = inputField.text;
            spell = spell.TrimEnd('\n'); // 엔터키 제거
            spell = spell.TrimEnd(); // 공백 제거
            spell = spell.TrimStart(); // 공백 제거

            TypingFinish(); // 입력 종료
        }
    }

    public void TypingFinish()
    {
        Player.instance.CastSpell(Player.instance.spell_BinaryTree.Search(spell).GetComponent<Spell>());
        Player.instance.playerInput.currentActionMap = Player.instance.playerInput.actions.FindActionMap("Player");
        inputField.gameObject.SetActive(false);
        inputField.text = string.Empty;
    }
}
