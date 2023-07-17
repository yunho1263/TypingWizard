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
        //싱글톤 패턴 적용
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

        #region 주문 관련
        [SerializeField]
        public Spell_BinaryTree spell_BinaryTree;

        public TMP_InputField spellInputField;

        public void OnInputModeChanges() // 주문 입력 필드 활성화
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

        #region 이동
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

        #region 상호작용
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
            // 범위 안에 있는 InteractiveObjects레이어인 오브젝트를 찾는다
            Collider[] colliders = Physics.OverlapSphere(transform.position, 3f, hitLayers);
            if (colliders.Length == 0)
            {
                interactiveObject = null;
                return;
            }
            // 거리순으로 정렬한다
            colliders = colliders.OrderBy(x => Vector3.Distance(transform.position, x.transform.position)).ToArray();
            // 가장 가까운 오브젝트를 찾는다
            interactiveObject = colliders[0].gameObject;
        }
        #endregion

        #region 대화

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