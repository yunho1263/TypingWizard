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
        //���ڿ��� ������ ����
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
            Debug.Log("�ֹ��� �������� �ʽ��ϴ�.");
            
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
                Debug.Log("�ֹ��� ���� �رݵ��� �ʾҽ��ϴ�");
            }
            
        }
        Player.instance.playerInput.currentActionMap = Player.instance.playerInput.actions.FindActionMap("Player");
        inputField.gameObject.SetActive(false);
        inputField.text = string.Empty;
    }
}
