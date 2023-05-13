using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum Language
{
    English,
    Korean
}

public class GlobalSetting : MonoBehaviour
{
    //싱글톤 패턴 적용
    public static GlobalSetting instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if(instance != this)
        {
            Destroy(this.gameObject);
        }
    }

    public DefaultSettings defaultSettings;
    public PlayerInput playerInput;

    public struct GraphicsSettings
    {
        public int resolutionWidth;
        public int resolutionHeight;
        public bool isFullScreen;
        public bool isVSync;
    }
    public GraphicsSettings graphicsSettings;

    public struct SoundSettings
    {
        public float masterVolume;
        public float backgroundMusicVolume;
        public float effectSoundVolume;
        public float voiceVolume;
        public float uiVolume;
    }
    public SoundSettings soundSettings;

    public struct InputSettings
    {
        public KeyCode positive;
        public KeyCode negative;
        public KeyCode pauseMenu;

        public KeyCode up;
        public KeyCode down;
        public KeyCode left;
        public KeyCode right;

        public KeyCode attack;
        public KeyCode rolling;
        public KeyCode interactions;
        public KeyCode spell;

        public KeyCode inventory;
        public KeyCode spellBook;
    }
    public InputSettings inputSettings;

    public struct GameSettings
    {
        public Language language;
        public bool continuousTyping;
    }
    public GameSettings gameSettings;

    public void ResetAllSettings()
    {
        graphicsSettings.resolutionWidth = defaultSettings.ResolutionWidth;
        graphicsSettings.resolutionHeight = defaultSettings.ResolutionHeight;
        graphicsSettings.isFullScreen = defaultSettings.IsFullScreen;
        graphicsSettings.isVSync = defaultSettings.IsVSync;

        soundSettings.masterVolume = defaultSettings.MasterVolume;
        soundSettings.backgroundMusicVolume = defaultSettings.BackgroundMusicVolume;
        soundSettings.effectSoundVolume = defaultSettings.EffectSoundVolume;
        soundSettings.voiceVolume = defaultSettings.VoiceVolume;
        soundSettings.uiVolume = defaultSettings.UIVolume;

        inputSettings.positive = defaultSettings.Positive;
        inputSettings.negative = defaultSettings.Negative;
        inputSettings.pauseMenu = defaultSettings.PauseMenu;
        inputSettings.up = defaultSettings.Up;
        inputSettings.down = defaultSettings.Down;
        inputSettings.left = defaultSettings.Left;
        inputSettings.right = defaultSettings.Right;
        inputSettings.attack = defaultSettings.Attack;
        inputSettings.rolling = defaultSettings.Rolling;
        inputSettings.interactions = defaultSettings.Interactions;
        inputSettings.spell = defaultSettings.Spell;
        inputSettings.inventory = defaultSettings.Inventory;
        inputSettings.spellBook = defaultSettings.SpellBook;

        gameSettings.language = defaultSettings.Language;
        gameSettings.continuousTyping = defaultSettings.ContinuousTyping;
    }

    public void ApplyAllSettings(GraphicsSettings graphicsSettings)
    {
        Screen.SetResolution(graphicsSettings.resolutionWidth, graphicsSettings.resolutionHeight, graphicsSettings.isFullScreen);
        QualitySettings.vSyncCount = graphicsSettings.isVSync ? 1 : 0;
        if (graphicsSettings.isFullScreen)
        {
            // 전체화면으로 전환
        }
        else
        {
            // 윈도우화면으로 전환
        }
    }

    public void ApplyAllSettings(SoundSettings soundSettings)
    {
        AudioListener.volume = soundSettings.masterVolume;
        // 나중에 사운드 매니저 만들어서 적용
    }

    public void ApplyAllSettings(InputSettings inputSettings)
    {
        // 인풋시스템으로 바인딩 변경
    }
}
