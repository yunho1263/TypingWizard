using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TypingWizard
{
    public class Initializer : MonoBehaviour
    {
        private void Start()
        {
            GlobalSetting.Instance.Initialize();
        }
    }

}