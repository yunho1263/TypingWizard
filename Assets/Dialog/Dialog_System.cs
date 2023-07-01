using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialog_System : MonoBehaviour
{
    

    // 현재 대화 정보
    public Dialog_Node currentNode;
    public GameObject currentSpeaker;

    // 플레이어의 답
    public string answer;

    // 대화창
    public GameObject bubblePrefeb;
    public GameObject[] bubblePool;
    public Queue<DialogSpeechBubble> activeBubbles;
    public Queue<DialogSpeechBubble> inactiveBubbles;

    private void Awake()
    {
        // 말풍선 풀 생성
        bubblePool = new GameObject[10];
        activeBubbles = new Queue<DialogSpeechBubble>();
        inactiveBubbles = new Queue<DialogSpeechBubble>();

        // 말풍선 풀 초기화
        for (int i = 0; i < bubblePool.Length; i++)
        {
            bubblePool[i] = Instantiate(bubblePrefeb, transform);
            inactiveBubbles.Enqueue(bubblePool[i].GetComponent<DialogSpeechBubble>());
        }
    }

    public void StartDialog(Dialog_Node dialog)
    {
        currentNode = dialog;
        if (FindSpeaker())
        {
            //speechBubble.SetDialog(currentSpeaker, currentNode.sentences);
        }
        else
        {
            Debug.LogError("대화 상대를 찾을 수 없습니다");
        }
    }

    public void NextDialog()
    {
        // 다음 대화가 없으면 종료
        if (currentNode.nextNodes.Count == 0)
        {
            EndDialog();
            return;
        }

        // 다음 대화가 있으면 대화를 진행
        currentNode = currentNode.nextNodes[0];
        //speechBubble.SetDialog(currentSpeaker, currentNode.sentences);
    }

    public void EndDialog()
    {

    }

    public bool FindSpeaker()
    {
        // Character 클래스를 가진 오브젝트를 모두 찾는다
        var characters = GameObject.FindObjectsOfType<Character>();

        // Character 클래스를 가진 오브젝트 중에서 characterName이 currentNode.name과 같은 오브젝트를 찾는다
        foreach (var ch in characters)
        {
            if (ch.characterName == currentNode.name)
            {
                currentSpeaker = ch.gameObject;
                return true;
            }
        }
        // 못 찾으면 false를 반환
        return false;
    }
}
