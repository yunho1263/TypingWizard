using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using TypingWizard.Dialogue;
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
            playerInput.SwitchCurrentActionMap("TextInput");
            spellInputField.gameObject.SetActive(true);
            spellInputField.Select();
            Input.imeCompositionMode = IMECompositionMode.On;
        }

        public void CastSpell(Spell spell)
        {
            spell.Cast(this);
        }
        #endregion

        #region �̵�
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

        #region ��ȣ�ۿ�
        public GameObject interactiveObject;

        public void OnInteractions()
        {
            InteractiveObject Object;
            if (interactiveObject == null || !interactiveObject.TryGetComponent(out Object))
            {
                return;
            }

            Object.Interact();
        }

        public void FindInteractiveObjects()
        {
            LayerMask hitLayers = (1 << LayerMask.NameToLayer("InteractiveObjects")) | (1 << LayerMask.NameToLayer("NPC"));
            // ���� �ȿ� �ִ� InteractiveObjects���̾��� ������Ʈ�� ã�´�
            Collider[] colliders = Physics.OverlapSphere(transform.position, 3f, hitLayers);
            if (colliders.Length == 0)
            {
                interactiveObject = null;
                return;
            }
            // �Ÿ������� �����Ѵ�
            colliders = colliders.OrderBy(x => Vector3.Distance(transform.position, x.transform.position)).ToArray();
            // ���� ����� ������Ʈ�� ã�´�
            interactiveObject = colliders[0].gameObject;
        }
        #endregion

        #region ��ȭ

        public TMP_InputField dialogueInputField;

        public void OpenDialogueInputField()
        {
            if (dialogueInputField == null)
            {
                dialogueInputField = DialogueManager.Instance.dialogueInputField;
            }

            dialogueInputField.gameObject.SetActive(true);
            spellInputField.Select();
            Input.imeCompositionMode = IMECompositionMode.On;
        }

        public void OnNextDialogue()
        {
            DialogueManager.Instance.NextDialogue();
        }
        #endregion

        private void Update()
        {
            Move();
            FindInteractiveObjects();
        }
    }
}