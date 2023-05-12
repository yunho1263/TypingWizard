using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Spell_BinaryTree
{
    List<GameObject> allSpellObjs;

    public SpellNode root;
    public SpellNode current;

    public void Initialize()
    {
        //리소스에서 프리팹들을 불러와 노드로 만들어서 트리에 넣는다.
        GameObject[] allSpellPrefebs = Resources.LoadAll<GameObject>("Assets/Prefabs/Spells");
        foreach (GameObject pre in allSpellPrefebs)
        {
            allSpellObjs.Add(GameObject.Instantiate(pre));
        }
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
            return x.spellObj.GetComponent<Spell>().spellData.SpellName.Search(Player.instance.language).CompareTo(y.spellObj.GetComponent<Spell>().spellData.SpellName.Search(Player.instance.language));
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
        // 루트 노드부터 시작해서 새 노드를 추가할 위치를 찾는다.
        if (newNode.spellObj.GetComponent<Spell>().spellData.SpellName.Search(Player.instance.language).CompareTo(current.spellObj.GetComponent<Spell>().spellData.SpellName.Search(Player.instance.language)) < 0)
        {
            if (current.left == null) // 왼쪽 자식이 없다면
            {
                current.left = newNode; // 왼쪽 자식으로 넣는다.
                newNode.parent = current; // 새 노드의 부모를 현재 노드로 설정한다.
            }
            else // 왼쪽 자식이 있다면
            {
                current = current.left; // 현재 노드를 왼쪽 자식으로 설정하고
                Add(newNode); // 재귀 호출
            }
        }
        else
        {
            if (current.right == null) // 오른쪽 자식이 없다면
            {
                current.right = newNode; // 오른쪽 자식으로 넣는다.
                newNode.parent = current; // 새 노드의 부모를 현재 노드로 설정한다.
            }
            else // 오른쪽 자식이 있다면
            {
                current = current.right; // 현재 노드를 오른쪽 자식으로 설정하고
                Add(newNode); // 재귀 호출
            }
        }
    }

    public GameObject Search(string spellName)
    {
        // 루트 노드부터 시작해서 찾는 노드를 찾는다.
        if (spellName.CompareTo(current.spellObj.GetComponent<Spell>().spellData.SpellName.Search(Player.instance.language)) < 0)
        {
            if (current.left == null) // 왼쪽 자식이 없다면
            {
                return null;
            }
            else // 왼쪽 자식이 있다면
            {
                current = current.left; // 현재 노드를 왼쪽 자식으로 설정하고
                Search(spellName); // 재귀 호출
            }
        }
        else if (spellName.CompareTo(current.spellObj.GetComponent<Spell>().spellData.SpellName.Search(Player.instance.language)) > 0)
        {
            if (current.right == null) // 오른쪽 자식이 없다면
            {
                return null;
            }
            else // 오른쪽 자식이 있다면
            {
                current = current.right; // 현재 노드를 오른쪽 자식으로 설정하고
                Search(spellName); // 재귀 호출
            }
        }
        else
        {
            return current.spellObj;
        }
        return null;
    }
}

public class SpellNode
{
    public GameObject spellObj;
    public SpellNode left;
    public SpellNode right;
    public SpellNode parent;
    public SpellNode(GameObject spellPrefeb)
    {
        this.spellObj = spellPrefeb;
    }
}