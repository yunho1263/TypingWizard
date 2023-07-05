using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace TypingWizard
{
    [Serializable]
    public class SpellNode
    {
        public GameObject spellObj;
        public SpellNode left;
        public SpellNode right;
        public SpellNode parent;
        public Spell spell;

        public SpellNode(GameObject spellObj)
        {
            this.spellObj = spellObj;
            this.spellObj.TryGetComponent(out spell);
        }

        public void UpdateSpell()
        {
            spellObj.TryGetComponent(out spell);
        }
    }

    [Serializable]
    public class Spell_BinaryTree
    {
        [SerializeField]
        List<GameObject> allSpellObjs;

        [SerializeField]
        public SpellNode root;

        public void Initialize()
        {
            //���ҽ����� �����յ��� �ҷ��� ���� ���� Ʈ���� �ִ´�.
            GameObject[] allSpellPrefebs = Resources.LoadAll<GameObject>("Prefabs/Spells");
            allSpellObjs = new List<GameObject>();
            foreach (GameObject pre in allSpellPrefebs)
            {
                GameObject newSpell = GameObject.Instantiate(pre);
                allSpellObjs.Add(newSpell);
                newSpell.TryGetComponent(out Spell spell);
                spell.Initialize(Player.instance);
            }
            // ������� �ʴ� ���ҽ� ����

            Resources.UnloadUnusedAssets();
            //Ʈ�� ����ȭ
            Optimize();
        }

        public void Optimize()
        {
            //���� ������ �迭�� �����.
            SpellNode[] nodes = new SpellNode[allSpellObjs.Count];
            int i = 0;
            foreach (GameObject spellPrefeb in allSpellObjs)
            {
                nodes[i] = new SpellNode(spellPrefeb);
                i++;
            }
            //�迭�� �����Ѵ�.
            Array.Sort(nodes, delegate (SpellNode x, SpellNode y)
            {
                Spell xSpell;
                x.spellObj.TryGetComponent(out xSpell);

                Spell ySpell;
                y.spellObj.TryGetComponent(out ySpell);

                return xSpell.arias[0].CompareTo(ySpell.arias[0]);
            });
            //�迭�� �ٽ� Ʈ���� �����.
            root = nodes[0];
            for (int j = 1; j < nodes.Length; j++)
            {
                Add(nodes[j]);
            }
        }

        public void Add(SpellNode newNode)
        {
            SpellNode current = root;

            Spell newSpell;
            newNode.spellObj.TryGetComponent(out newSpell);

            while (true)
            {
                Spell currSpell;
                current.spellObj.TryGetComponent(out currSpell);

                if (newSpell.arias[0].CompareTo(currSpell.arias[0]) < 0)
                {
                    if (current.left == null) // ���� �ڽ��� ���ٸ�
                    {
                        current.left = newNode; // ���� �ڽ����� �ִ´�.
                        newNode.parent = current; // �� ����� �θ� ���� ���� �����Ѵ�.
                        break;
                    }
                    else // ���� �ڽ��� �ִٸ�
                    {
                        current = current.left; // ���� ��带 ���� �ڽ����� �����ϰ�
                        continue;
                    }
                }
                else
                {
                    if (current.right == null) // ������ �ڽ��� ���ٸ�
                    {
                        current.right = newNode; // ������ �ڽ����� �ִ´�.
                        newNode.parent = current; // �� ����� �θ� ���� ���� �����Ѵ�.
                        break;
                    }
                    else // ������ �ڽ��� �ִٸ�
                    {
                        current = current.right; // ���� ��带 ������ �ڽ����� �����ϰ�
                        continue;
                    }
                }
            }
        }

        public GameObject Search(string aria)
        {
            SpellNode current = root; // ���� ��带 ��Ʈ�� ����

            while (true)
            {
                if (aria.CompareTo(current.spell.arias[0]) < 0) // ���� ����� ������ �۴ٸ�
                {
                    if (current.left == null) // ���� �ڽ��� ���ٸ�
                    {
                        return null; // null ��ȯ
                    }
                    else
                    {
                        current = current.left; // ���� ��带 ���� �ڽ����� �����ϰ�
                        continue; // �ٽ� �ݺ�
                    }
                }
                else if (aria.CompareTo(current.spell.arias[0]) > 0) // ���� ����� ������ ũ�ٸ�
                {
                    if (current.right == null) // ������ �ڽ��� ���ٸ�
                    {
                        return null; // null ��ȯ
                    }
                    else
                    {
                        current = current.right; // ���� ��带 ������ �ڽ����� �����ϰ�
                        continue; // �ٽ� �ݺ�
                    }
                }
                else
                {
                    return current.spellObj; // ��ġ�ϴ� ��带 ã�Ҵٸ� �ش� ��� ��ȯ
                }
            }
        }
    }

}