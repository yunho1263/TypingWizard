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
    //싱글톤 패턴 적용
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

    public void OnInputModeChanges() // 주문 입력 필드 활성화
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
        // 입력받은 노말 값으로 Translate
        transform.Translate(moveNormal * moveSpeed * Time.deltaTime);
    }
}
