using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class SettingUi : MonoBehaviour
{

    public Button apply;
    public Button cancel;

    public TMP_Dropdown language;

    private void Awake()
    {
        GlobalSetting.Instance.Initialize();
    }

    public void Apply()
    {
        GlobalSetting.Instance.gameSettings.language = (Language)language.value;
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
}
