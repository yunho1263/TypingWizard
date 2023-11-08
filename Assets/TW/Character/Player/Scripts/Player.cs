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

            single_SpellDictionary.Initialize();
            multiple_SpellDictionary.Initialize();
            arcana_SpellDictionary.Initialize();
            rogue_SpellDictionary.Initialize();
            rune_SpellDictionary.Initialize();

            LoadAllLearnedSpells();
        }

        public PlayerInput playerInput;

        #region �ֹ� ����
        public GameObject[] single_Spell_Prefabs;
        public GameObject[] multiple_Spell_Prefabs;
        public GameObject[] arcana_Spell_Prefabs;
        public GameObject[] rogue_Spell_Prefabs;
        public GameObject[] rune_Spell_Prefabs;

        public List<int> learnedSpells; // ��� �ֹ� ���

        public Single_SpellDictionary single_SpellDictionary; // ���� ��â �ֹ� ����
        public Multiple_SpellDictionary multiple_SpellDictionary; // ���� ��â �ֹ� ����
        public Arcana_SpellDictionary arcana_SpellDictionary; // �Ƹ�ī�� �ֹ� ����
        public Rogue_SpellDictionary rogue_SpellDictionary; // �α� �ֹ� ����
        public Rune_SpellDictionary rune_SpellDictionary; // �� �ֹ� ����

        public TMP_InputField spellInputField;

        public void OnInputModeChanges() // �ֹ� �Է� �ʵ� Ȱ��ȭ
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

            // ���� ��â �ֹ� �������� �ֹ��� ã�Ƽ� ����
            if (single_SpellDictionary.Search(inputedStr, out castingSpell))
            {
                CastSpell(castingSpell);
                return;
            }

            // ���� ��â �ֹ� �������� �ֹ��� ã�Ƽ� ����
            if (multiple_SpellDictionary.Search(inputedStr, out castingSpell))
            {
                CastSpell(castingSpell);
                return;
            }

            // �Ƹ�ī�� �ֹ� �������� �ֹ��� ã�Ƽ� ����
            if (arcana_SpellDictionary.Search(words, out castingSpell))
            {
                CastSpell(castingSpell);
                return;
            }

            // �α� �ֹ� �������� �ֹ��� ã�Ƽ� ����
            if (rogue_SpellDictionary.Search(words, out castingSpell))
            {
                CastSpell(castingSpell);
                return;
            }

            // �� �ֹ� �������� �ֹ��� ã�Ƽ� ����
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