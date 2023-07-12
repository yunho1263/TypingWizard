using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace TypingWizard
{
    public class Player : Character
    {
        //�̱��� ���� ����
        public static Player instance;
        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else if (instance != this)
            {
                Destroy(this.gameObject);
            }

            DontDestroyOnLoad(this.gameObject);

            spell_BinaryTree = new Spell_BinaryTree();
            spell_BinaryTree.Initialize();
        }

        public PlayerInput playerInput;

        #region �ֹ� ����
        [SerializeField]
        public Spell_BinaryTree spell_BinaryTree;

        public TMP_InputField spellInputField;

        public void OnInputModeChanges() // �ֹ� �Է� �ʵ� Ȱ��ȭ
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
        #endregion

        #region �̵� ����
        public void OnMove(InputValue value)
        {
            moveDirNomormal = Vector3.zero;
            if (value.Get() == null)
            {
                return;
            }
            moveDirNomormal.x = value.Get<Vector2>().x;
            moveDirNomormal.z = value.Get<Vector2>().y;
        }
        #endregion

        #region ��ȣ�ۿ� ����
        public GameObject interactiveObject;

        public void OnInteractions()
        {
            if (interactiveObject == null)
            {
                return;
            }

            Debug.Log(interactiveObject.name);
        }

        public void FindInteractiveObjects()
        {
            // ���� �ȿ� �ִ� InteractiveObjects���̾��� ������Ʈ�� ã�´�
            Collider[] colliders = Physics.OverlapSphere(transform.position, 3f, 1 << LayerMask.NameToLayer("InteractiveObjects"));
            if (colliders.Length == 0)
            {
                interactiveObject = null;
                return;
            }
            // �Ÿ������� �����Ѵ�
            colliders = colliders.OrderBy(x => Vector3.Distance(transform.position, x.transform.position)).ToArray();
            // ���� ����� ������Ʈ�� ã�´�
            interactiveObject = colliders[0].gameObject.transform.parent.gameObject;
        }
        #endregion

        private void Update()
        {
            Move();
            FindInteractiveObjects();
        }
    }
}