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
            //리소스에서 프리팹들을 불러와 노드로 만들어서 트리에 넣는다.
            GameObject[] allSpellPrefebs = Resources.LoadAll<GameObject>("Prefabs/Spells");
            allSpellObjs = new List<GameObject>();
            foreach (GameObject pre in allSpellPrefebs)
            {
                GameObject newSpell = GameObject.Instantiate(pre);
                allSpellObjs.Add(newSpell);
                newSpell.TryGetComponent(out Spell spell);
                spell.Initialize(Player.instance);
            }
            // 사용하지 않는 리소스 제거

            Resources.UnloadUnusedAssets();
            //트리 최적화
            Optimize();
        }

        public void Optimize()
        {
            //먼저 노드들을 배열로 만든다.
            SpellNode[] nodes = new SpellNode[allSpellObjs.Count];
            int i = 0;
            foreach (GameObject spellPrefeb in allSpellObjs)
            {
                nodes[i] = new SpellNode(spellPrefeb);
                i++;
            }
            //배열을 정렬한다.
            Array.Sort(nodes, delegate (SpellNode x, SpellNode y)
            {
                Spell xSpell;
                x.spellObj.TryGetComponent(out xSpell);

                Spell ySpell;
                y.spellObj.TryGetComponent(out ySpell);

                return xSpell.arias[0].CompareTo(ySpell.arias[0]);
            });
            //배열을 다시 트리로 만든다.
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
                    if (current.left == null) // 왼쪽 자식이 없다면
                    {
                        current.left = newNode; // 왼쪽 자식으로 넣는다.
                        newNode.parent = current; // 새 노드의 부모를 현재 노드로 설정한다.
                        break;
                    }
                    else // 왼쪽 자식이 있다면
                    {
                        current = current.left; // 현재 노드를 왼쪽 자식으로 설정하고
                        continue;
                    }
                }
                else
                {
                    if (current.right == null) // 오른쪽 자식이 없다면
                    {
                        current.right = newNode; // 오른쪽 자식으로 넣는다.
                        newNode.parent = current; // 새 노드의 부모를 현재 노드로 설정한다.
                        break;
                    }
                    else // 오른쪽 자식이 있다면
                    {
                        current = current.right; // 현재 노드를 오른쪽 자식으로 설정하고
                        continue;
                    }
                }
            }
        }

        public GameObject Search(string aria)
        {
            SpellNode current = root; // 현재 노드를 루트로 설정

            while (true)
            {
                if (aria.CompareTo(current.spell.arias[0]) < 0) // 현재 노드의 값보다 작다면
                {
                    if (current.left == null) // 왼쪽 자식이 없다면
                    {
                        return null; // null 반환
                    }
                    else
                    {
                        current = current.left; // 현재 노드를 왼쪽 자식으로 설정하고
                        continue; // 다시 반복
                    }
                }
                else if (aria.CompareTo(current.spell.arias[0]) > 0) // 현재 노드의 값보다 크다면
                {
                    if (current.right == null) // 오른쪽 자식이 없다면
                    {
                        return null; // null 반환
                    }
                    else
                    {
                        current = current.right; // 현재 노드를 오른쪽 자식으로 설정하고
                        continue; // 다시 반복
                    }
                }
                else
                {
                    return current.spellObj; // 일치하는 노드를 찾았다면 해당 노드 반환
                }
            }
        }
    }

}