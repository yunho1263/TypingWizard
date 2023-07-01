using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Dialog_Node
{
    // ȭ��
    public string name;

    // ��ȭ ����
    [TextArea(3, 10)]
    public string sentences;

    // branchs�ȿ� �ִ� ������ �Է��ؾ߸� ������ ����
    public bool isNeedCorrectAnswer;

    // �亯�� ���� �б�
    public List<string> branchs;
    public List<Dialog_Node> nextNodes;
}