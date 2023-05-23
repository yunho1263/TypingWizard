using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

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
            GameObject newSpell =  GameObject.Instantiate(pre);
            allSpellObjs.Add(newSpell);
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
        SpellNode current = root;
        Spell currSpell;

        while (true)
        {
            current.spellObj.TryGetComponent(out currSpell);

            if (aria.CompareTo(currSpell.arias[0]) < 0)
            {
                if (current.left == null) // ���� �ڽ��� ���ٸ�
                {
                    return null;
                }
                else // ���� �ڽ��� �ִٸ�
                {
                    current = current.left; // ���� ��带 ���� �ڽ����� �����ϰ�
                    continue;
                }
            }
            else if (aria.CompareTo(currSpell.arias[0]) > 0)
            {
                if (current.right == null) // ������ �ڽ��� ���ٸ�
                {
                    return null;
                }
                else // ������ �ڽ��� �ִٸ�
                {
                    current = current.right; // ���� ��带 ������ �ڽ����� �����ϰ�
                    continue;
                }
            }
            else // ã�Ҵٸ�
            {
                return current.spellObj; // ã�� ��带 ��ȯ�Ѵ�.
            }
        }
    }
}

[Serializable]
public class SpellNode
{
    public GameObject spellObj;
    public SpellNode left;
    public SpellNode right;
    public SpellNode parent;
    public SpellNode(GameObject spellObj)
    {
        this.spellObj = spellObj;
    }
}