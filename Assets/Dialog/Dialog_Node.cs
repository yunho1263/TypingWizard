using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Dialog_Node
{
    // 화자
    public string name;

    // 대화 내용
    [TextArea(3, 10)]
    public string sentences;

    // branchs안에 있는 문장을 입력해야만 진행이 가능
    public bool isNeedCorrectAnswer;

    // 답변에 따른 분기
    public List<string> branchs;
    public List<Dialog_Node> nextNodes;
}