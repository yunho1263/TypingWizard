using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class Player : Character
{
    //싱글톤 패턴 적용
    public static Player instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if(instance != this)
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);

        spell_BinaryTree = new Spell_BinaryTree();
        spell_BinaryTree.Initialize();
    }

    public PlayerInput playerInput;

    [SerializeField]
    public Spell_BinaryTree spell_BinaryTree;

    public TMP_InputField spellInputField;

    public void OnMove(InputValue value)
    {
        if (value.Get() == null)
        {
            moveDirNomormal = Vector2.zero;
            return;
        }
        moveDirNomormal = value.Get<Vector2>();
    }

    public void OnInputModeChanges() // 주문 입력 필드 활성화
    {
        playerInput.SwitchCurrentActionMap("MagicSpell");
        spellInputField.gameObject.SetActive(true);
        spellInputField.Select();
        Input.imeCompositionMode = IMECompositionMode.On;
    }

    public void CastSpell(Spell spell)
    {
        spell.Cast(this);
    }

    private void Update()
    {
        Move();
    }
}
