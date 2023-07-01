using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialog_System : MonoBehaviour
{
    

    // ���� ��ȭ ����
    public Dialog_Node currentNode;
    public GameObject currentSpeaker;

    // �÷��̾��� ��
    public string answer;

    // ��ȭâ
    public GameObject bubblePrefeb;
    public GameObject[] bubblePool;
    public Queue<DialogSpeechBubble> activeBubbles;
    public Queue<DialogSpeechBubble> inactiveBubbles;

    private void Awake()
    {
        // ��ǳ�� Ǯ ����
        bubblePool = new GameObject[10];
        activeBubbles = new Queue<DialogSpeechBubble>();
        inactiveBubbles = new Queue<DialogSpeechBubble>();

        // ��ǳ�� Ǯ �ʱ�ȭ
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
            Debug.LogError("��ȭ ��븦 ã�� �� �����ϴ�");
        }
    }

    public void NextDialog()
    {
        // ���� ��ȭ�� ������ ����
        if (currentNode.nextNodes.Count == 0)
        {
            EndDialog();
            return;
        }

        // ���� ��ȭ�� ������ ��ȭ�� ����
        currentNode = currentNode.nextNodes[0];
        //speechBubble.SetDialog(currentSpeaker, currentNode.sentences);
    }

    public void EndDialog()
    {

    }

    public bool FindSpeaker()
    {
        // Character Ŭ������ ���� ������Ʈ�� ��� ã�´�
        var characters = GameObject.FindObjectsOfType<Character>();

        // Character Ŭ������ ���� ������Ʈ �߿��� characterName�� currentNode.name�� ���� ������Ʈ�� ã�´�
        foreach (var ch in characters)
        {
            if (ch.characterName == currentNode.name)
            {
                currentSpeaker = ch.gameObject;
                return true;
            }
        }
        // �� ã���� false�� ��ȯ
        return false;
    }
}
