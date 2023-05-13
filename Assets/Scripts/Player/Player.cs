using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    //�̱��� ���� ����
    public static Player instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }


        spell_BinaryTree = new Spell_BinaryTree();
        spell_BinaryTree.Initialize();
    }

    public Language language;

    public PlayerInput playerInput;

    public float moveSpeed;
    public Vector2 moveNormal;

    [SerializeField]
    public Spell_BinaryTree spell_BinaryTree;
    public TMP_InputField spellInputField;

    public void OnMove(InputValue value)
    {
        if (value.Get() == null)
        {
            moveNormal = Vector2.zero;
            return;
        }
        moveNormal = value.Get<Vector2>();
    }

    public void OnInputModeChanges() // �ֹ� �Է� �ʵ� Ȱ��ȭ
    {
        playerInput.SwitchCurrentActionMap("MagicSpell");
        spellInputField.gameObject.SetActive(true);
        spellInputField.Select();
        Input.imeCompositionMode = IMECompositionMode.On;
    }

    public void CastSpell(Spell spell)
    {
        spell.Cast(gameObject);
    }

    private void Update()
    {
        // �Է¹��� �븻 ������ Translate
        transform.Translate(moveNormal * moveSpeed * Time.deltaTime);
    }
}
