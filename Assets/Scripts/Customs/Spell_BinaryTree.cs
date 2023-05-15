using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Spell_BinaryTree
{
    List<GameObject> allSpellObjs;

    public SpellNode root;
    public SpellNode current;

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

            return xSpell.spellName.CompareTo(ySpell.spellName);
        });
        //�迭�� �ٽ� Ʈ���� �����.
        root = nodes[0];
        for (int j = 1; j < nodes.Length; j++)
        {
            current = root;
            Add(nodes[j]);
        }
    }

    public void Add(SpellNode newNode)
    {
        Spell currSpell;
        current.spellObj.TryGetComponent(out currSpell);

        Spell newSpell;
        newNode.spellObj.TryGetComponent(out newSpell);

        while (true)
        {
            
        }

        //// ��Ʈ ������ �����ؼ� �� ��带 �߰��� ��ġ�� ã�´�.
        //if (newSpell.spellName.CompareTo(currSpell.spellName) < 0)
        //{
        //    if (current.left == null) // ���� �ڽ��� ���ٸ�
        //    {
        //        current.left = newNode; // ���� �ڽ����� �ִ´�.
        //        newNode.parent = current; // �� ����� �θ� ���� ���� �����Ѵ�.
        //    }
        //    else // ���� �ڽ��� �ִٸ�
        //    {
        //        current = current.left; // ���� ��带 ���� �ڽ����� �����ϰ�
        //        Add(newNode); // ��� ȣ��
        //    }
        //}
        //else
        //{
        //    if (current.right == null) // ������ �ڽ��� ���ٸ�
        //    {
        //        current.right = newNode; // ������ �ڽ����� �ִ´�.
        //        newNode.parent = current; // �� ����� �θ� ���� ���� �����Ѵ�.
        //    }
        //    else // ������ �ڽ��� �ִٸ�
        //    {
        //        current = current.right; // ���� ��带 ������ �ڽ����� �����ϰ�
        //        Add(newNode); // ��� ȣ��
        //    }
        //}
    }

    public GameObject Search(string spellName)
    {
        Spell currSpell;
        current.spellObj.TryGetComponent(out currSpell);

        // ��Ʈ ������ �����ؼ� ã�� ��带 ã�´�.
        if (spellName.CompareTo(currSpell.spellName) < 0)
        {
            if (current.left == null) // ���� �ڽ��� ���ٸ�
            {
                return null;
            }
            else // ���� �ڽ��� �ִٸ�
            {
                current = current.left; // ���� ��带 ���� �ڽ����� �����ϰ�
                return Search(spellName); // ��� ȣ��
            }
        }
        else if (spellName.CompareTo(currSpell.spellName) > 0)
        {
            if (current.right == null) // ������ �ڽ��� ���ٸ�
            {
                return null;
            }
            else // ������ �ڽ��� �ִٸ�
            {
                current = current.right; // ���� ��带 ������ �ڽ����� �����ϰ�
                return Search(spellName); // ��� ȣ��
            }
        }
        else // ã�Ҵٸ�
        {
            return current.spellObj; // ã�� ��带 ��ȯ�Ѵ�.
        }
    }
}

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