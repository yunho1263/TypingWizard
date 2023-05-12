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
        if (inputField.text.EndsWith('\n')) // ����Ű �Է½�
        {
            spell = inputField.text;
            spell = spell.TrimEnd('\n'); // ����Ű ����
            spell = spell.TrimEnd(); // ���� ����
            spell = spell.TrimStart(); // ���� ����

            TypingFinish(); // �Է� ����
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
