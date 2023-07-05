using UnityEngine;

namespace DialogueSystem.Data.Error
{
    public class D_ErrorData
    {
        public Color Color { get; set; }

        public D_ErrorData()
        {
            GenerateRandomColor();
        }

        private void GenerateRandomColor()
        {
            Color = new Color32
            (
                (byte)Random.Range(65, 256),
                (byte)Random.Range(50, 176),
                (byte)Random.Range(50, 176),
                255
            );
        }
    }
}
