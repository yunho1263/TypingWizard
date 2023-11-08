using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace TypingWizard
{
    using Spells;
    using Spells.Enums;
    using SpellDictionary;
    using SpellDictionary.Utility;
    using Dialogue;

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

            single_SpellDictionary.Initialize();
            multiple_SpellDictionary.Initialize();
            arcana_SpellDictionary.Initialize();
            rogue_SpellDictionary.Initialize();
            rune_SpellDictionary.Initialize();

            LoadAllLearnedSpells();
        }

        public PlayerInput playerInput;

        #region 주문 관련
        public GameObject[] single_Spell_Prefabs;
        public GameObject[] multiple_Spell_Prefabs;
        public GameObject[] arcana_Spell_Prefabs;
        public GameObject[] rogue_Spell_Prefabs;
        public GameObject[] rune_Spell_Prefabs;

        public List<int> learnedSpells; // 배운 주문 목록

        public Single_SpellDictionary single_SpellDictionary; // 단일 영창 주문 사전
        public Multiple_SpellDictionary multiple_SpellDictionary; // 다중 영창 주문 사전
        public Arcana_SpellDictionary arcana_SpellDictionary; // 아르카나 주문 사전
        public Rogue_SpellDictionary rogue_SpellDictionary; // 로그 주문 사전
        public Rune_SpellDictionary rune_SpellDictionary; // 룬 주문 사전

        public TMP_InputField spellInputField;

        public void OnInputModeChanges() // 주문 입력 필드 활성화
        {
            playerInput.SwitchCurrentActionMap("TextInput");
            spellInputField.gameObject.SetActive(true);
            spellInputField.Select();
            Input.imeCompositionMode = IMECompositionMode.On;
        }

        public void LoadAllLearnedSpells()
        {
            single_Spell_Prefabs = Resources.LoadAll<GameObject>("Prefebs/SingleAria_Spell");
            multiple_Spell_Prefabs = Resources.LoadAll<GameObject>("Prefebs/MultipleAria_Spell");
            arcana_Spell_Prefabs = Resources.LoadAll<GameObject>("Prefebs/Arcana_Spell");
            rogue_Spell_Prefabs = Resources.LoadAll<GameObject>("Prefebs/Rogue_Spell");
            rune_Spell_Prefabs = Resources.LoadAll<GameObject>("Prefebs/Rune_Spell");

            foreach (GameObject spellObj in single_Spell_Prefabs)
            {
                spellObj.TryGetComponent(out Spell spell);

                if (learnedSpells.Contains(spell.spellID))
                {
                    single_SpellDictionary.AddSpell(spell);
                }
            }
            foreach (GameObject spellObj in multiple_Spell_Prefabs)
            {
                spellObj.TryGetComponent(out Spell spell);

                if (learnedSpells.Contains(spell.spellID))
                {
                    multiple_SpellDictionary.AddSpell(spell);
                }
            }
            foreach (GameObject spellObj in arcana_Spell_Prefabs)
            {
                spellObj.TryGetComponent(out Spell spell);

                if (learnedSpells.Contains(spell.spellID))
                {
                    arcana_SpellDictionary.AddSpell(spell);
                }
            }
            foreach (GameObject spellObj in rogue_Spell_Prefabs)
            {
                spellObj.TryGetComponent(out Spell spell);

                if (learnedSpells.Contains(spell.spellID))
                {
                    rogue_SpellDictionary.AddSpell(spell);
                }
            }
            foreach (GameObject spellObj in rune_Spell_Prefabs)
            {
                spellObj.TryGetComponent(out Spell spell);

                if (learnedSpells.Contains(spell.spellID))
                {
                    rune_SpellDictionary.AddSpell(spell);
                }
            }
        }

        public void Search(string inputedStr)
        {
            List<string> words = SpellDictionary_Utility.GetWords(inputedStr);

            Spell castingSpell;

            // 단일 영창 주문 사전에서 주문을 찾아서 시전
            if (single_SpellDictionary.Search(inputedStr, out castingSpell))
            {
                CastSpell(castingSpell);
                return;
            }

            // 다중 영창 주문 사전에서 주문을 찾아서 시전
            if (multiple_SpellDictionary.Search(inputedStr, out castingSpell))
            {
                CastSpell(castingSpell);
                return;
            }

            // 아르카나 주문 사전에서 주문을 찾아서 시전
            if (arcana_SpellDictionary.Search(words, out castingSpell))
            {
                CastSpell(castingSpell);
                return;
            }

            // 로그 주문 사전에서 주문을 찾아서 시전
            if (rogue_SpellDictionary.Search(words, out castingSpell))
            {
                CastSpell(castingSpell);
                return;
            }

            // 룬 주문 사전에서 주문을 찾아서 시전
            if (rune_SpellDictionary.Search(words, out castingSpell))
            {
                CastSpell(castingSpell);
                return;
            }
        }

        public void CastSpell(Spell spell)
        {
            spell.Cast(this);
        }

        public void AddSpell(Spell spell, Spell_Type spell_Type)
        {
            switch (spell_Type)
            {
                case Spell_Type.Single_Aria:
                    single_SpellDictionary.AddSpell(spell);
                    break;
                case Spell_Type.Multiple_Aria:
                    multiple_SpellDictionary.AddSpell(spell);
                    break;
                case Spell_Type.Arcana:
                    arcana_SpellDictionary.AddSpell(spell);
                    break;
                case Spell_Type.Rogue_Spell:
                    rogue_SpellDictionary.AddSpell(spell);
                    break;
                case Spell_Type.Rune_Spell:
                    rune_SpellDictionary.AddSpell(spell);
                    break;
                case Spell_Type.Atri:
                    break;
                default:
                    break;
            }
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