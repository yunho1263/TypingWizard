using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DialogueSystem
{
    public class Dialogue_System : MonoBehaviour
    {
        // 플레이어의 답
        public string answer;

        // 대화창
        public GameObject bubblePrefeb;
        public GameObject[] bubblePool;
        public Queue<DialogueSpeechBubble> activeBubbles;
        public Queue<DialogueSpeechBubble> inactiveBubbles;

        private void Awake()
        {
            // 말풍선 풀 생성
            bubblePool = new GameObject[10];
            activeBubbles = new Queue<DialogueSpeechBubble>();
            inactiveBubbles = new Queue<DialogueSpeechBubble>();

            // 말풍선 풀 초기화
            for (int i = 0; i < bubblePool.Length; i++)
            {
                bubblePool[i] = Instantiate(bubblePrefeb, transform);
                inactiveBubbles.Enqueue(bubblePool[i].GetComponent<DialogueSpeechBubble>());
            }
        }
    }

}