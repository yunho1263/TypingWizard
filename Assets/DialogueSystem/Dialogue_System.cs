using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DialogueSystem
{
    public class Dialogue_System : MonoBehaviour
    {
        // �÷��̾��� ��
        public string answer;

        // ��ȭâ
        public GameObject bubblePrefeb;
        public GameObject[] bubblePool;
        public Queue<DialogueSpeechBubble> activeBubbles;
        public Queue<DialogueSpeechBubble> inactiveBubbles;

        private void Awake()
        {
            // ��ǳ�� Ǯ ����
            bubblePool = new GameObject[10];
            activeBubbles = new Queue<DialogueSpeechBubble>();
            inactiveBubbles = new Queue<DialogueSpeechBubble>();

            // ��ǳ�� Ǯ �ʱ�ȭ
            for (int i = 0; i < bubblePool.Length; i++)
            {
                bubblePool[i] = Instantiate(bubblePrefeb, transform);
                inactiveBubbles.Enqueue(bubblePool[i].GetComponent<DialogueSpeechBubble>());
            }
        }
    }

}