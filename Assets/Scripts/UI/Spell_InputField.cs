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
        //문자열이 없으면 종료
        if (spell == string.Empty)
        {
            Player.instance.playerInput.currentActionMap = Player.instance.playerInput.actions.FindActionMap("Player");
            inputField.gameObject.SetActive(false);
            inputField.text = string.Empty;
            return;
        }

        Player.instance.spell_BinaryTree.current = Player.instance.spell_BinaryTree.root;
        GameObject castingSpellObj = Player.instance.spell_BinaryTree.Search(spell);
        if (castingSpellObj == null)
        {
            Debug.Log("주문이 존재하지 않습니다.");
            
        }
        else
        {
            Spell castSpell = castingSpellObj.GetComponent<Spell>();
            if (castSpell.isAcquired)
            {
                Player.instance.CastSpell(castingSpellObj.GetComponent<Spell>());
            }
            else
            {
                Debug.Log("주문이 아직 해금되지 않았습니다");
            }
            
        }
        Player.instance.playerInput.currentActionMap = Player.instance.playerInput.actions.FindActionMap("Player");
        inputField.gameObject.SetActive(false);
        inputField.text = string.Empty;
    }
}
