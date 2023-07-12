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

        #region 이동 관련
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

        #region 상호작용 관련
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
            // 범위 안에 있는 InteractiveObjects레이어인 오브젝트를 찾는다
            Collider[] colliders = Physics.OverlapSphere(transform.position, 3f, 1 << LayerMask.NameToLayer("InteractiveObjects"));
            if (colliders.Length == 0)
            {
                interactiveObject = null;
                return;
            }
            // 거리순으로 정렬한다
            colliders = colliders.OrderBy(x => Vector3.Distance(transform.position, x.transform.position)).ToArray();
            // 가장 가까운 오브젝트를 찾는다
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