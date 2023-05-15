using System.Collections;
using System.Collections.Generic;
using UnityEditor.UI;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;

public class TitleUiControl : MonoBehaviour
{
    public Button newGame;
    public Button loadGame;
    public Button setting;
    public Button exit;

    public GameObject settingPanel;

    public void NewGame()
    {
        SceneManager.LoadScene("main");
    }

    public void LoadGame()
    {
        Debug.Log("LoadGame");
    }

    public void Setting()
    {
        settingPanel.SetActive(true);
    }

    public void Exit()
    {
        Application.Quit();
    }
}
