using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TypingWizard.UI
{
    public class PauseUI : MonoBehaviour
    {
        public GameObject pauseUI;
        public GameObject settingUI;
        public GameObject saveUI;
        public GameObject loadUI;
        public GameObject quitUI;

        public void Pause()
        {
            pauseUI.SetActive(true);
            Time.timeScale = 0f;
        }

        public void Resume()
        {
            pauseUI.SetActive(false);
            Time.timeScale = 1f;
        }

        public void Setting()
        {
            settingUI.SetActive(true);
            pauseUI.SetActive(false);
        }

        public void Save()
        {
            saveUI.SetActive(true);
            pauseUI.SetActive(false);
        }

        public void Load()
        {
            loadUI.SetActive(true);
            pauseUI.SetActive(false);
        }

        public void Quit()
        {
            quitUI.SetActive(true);
            pauseUI.SetActive(false);
        }

        public void QuitGame()
        {
            Application.Quit();
        }
    }

}
