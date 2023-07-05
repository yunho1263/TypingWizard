using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.SmartFormat.Core.Parsing;

namespace TypingWizard.UI
{
    public class SettingUi : MonoBehaviour
    {
        public Button apply;
        public Button cancel;

        public TMP_Dropdown language;

        private void Awake()
        {

        }

        public void Apply()
        {
            GlobalSetting.Instance.SaveJson();
            gameObject.SetActive(false);
        }

        public void Cancel()
        {
            gameObject.SetActive(false);
        }

        public void ResetSettings()
        {
            GlobalSetting.Instance.ResetAllSettings();
        }

        //-------------------------------------------------------------------------

        public void SetScreenResolution()
        {
            //GlobalSetting.Instance.graphicsSettings.resolutionWidth =
            //GlobalSetting.Instance.graphicsSettings.resolutionHeight = 
        }
    }

}
