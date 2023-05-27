using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.IO;
using System.Text;
using Unity.VisualScripting;
using System;
using UnityEngine.Localization.Settings;

[Serializable]
public class GraphicsSettings
{
    public int resolutionWidth;
    public int resolutionHeight;
    public bool isFullScreen;
    [Space(10)]
    public bool isVSync;
}

[Serializable]
public class SoundSettings
{
    [Range(0, 100)]
    public float masterVolume;
    [Range(0, 100)]
    public float backgroundMusicVolume;
    [Range(0, 100)]
    public float effectSoundVolume;
    [Range(0, 100)]
    public float voiceVolume;
    [Range(0, 100)]
    public float uiVolume;
}

[Serializable]
public class InputSettings
{
    public KeyCode positive;
    public KeyCode negative;
    [Space(10)]

    public KeyCode pauseMenu;
    [Space(10)]

    public KeyCode up;
    public KeyCode down;
    public KeyCode left;
    public KeyCode right;
    [Space(10)]

    public KeyCode attack;
    public KeyCode rolling;
    public KeyCode interactions;
    public KeyCode spell;
    [Space(10)]

    public KeyCode inventory;
    public KeyCode spellBook;
}

[Serializable]
public class GameSettings
{
    public bool continuousTyping;
    public bool autoSave;
    public bool autoAim;
}

public class GlobalSetting : MonoBehaviour
{
    //싱글톤 패턴 적용
    private static GlobalSetting instance;
    public static GlobalSetting Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/Global/GlobalSetting")).GetComponent<GlobalSetting>();
            }
            return instance;
        }
        set { instance = value; }
    }

    public DefaultSettings defaultSettings;
    public PlayerInput playerInput;

    public GraphicsSettings graphicsSettings;
    public SoundSettings soundSettings;
    public InputSettings inputSettings;
    public GameSettings gameSettings;

    public void Initialize()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);

        graphicsSettings = new GraphicsSettings();
        soundSettings = new SoundSettings();
        inputSettings = new InputSettings();
        gameSettings = new GameSettings();

        if (!ReadJson())
        {
            ResetAllSettings();
        }
    }

    public void SaveJson()
    {
        string json = JsonUtility.ToJson(graphicsSettings);
        // 줄바꿈
        json += "\n";
        json += JsonUtility.ToJson(soundSettings);
        json += "\n";
        json += JsonUtility.ToJson(inputSettings);
        json += "\n";
        json += JsonUtility.ToJson(gameSettings);

        string filename = "Settings.json";
        string path = Path.Combine(Application.persistentDataPath, filename);

        FileStream fileStream = new FileStream(path, FileMode.Create);
        byte[] bytes = Encoding.UTF8.GetBytes(json);
        fileStream.Write(bytes, 0, bytes.Length);
        fileStream.Close();
    }

    public bool ReadJson()
    {
        string filename = "Settings.json";
        string path = Path.Combine(Application.persistentDataPath, filename);

        FileStream fileStream;
        try
        {
            fileStream = new FileStream(path, FileMode.Open);
        }
        catch (FileNotFoundException)
        {
            return false;
        }

        byte[] bytes = new byte[fileStream.Length];
        fileStream.Read(bytes, 0, bytes.Length);
        fileStream.Close();

        string json = Encoding.UTF8.GetString(bytes);
        string graphicsJson;
        string soundJson;
        string inputJson;
        string gameJson;
        try
        {
            int index = 0;
            int nextIndex = json.IndexOf("\n", index);

            graphicsJson = json.Substring(index, nextIndex - index);
            index = nextIndex + 1;
            nextIndex = json.IndexOf("\n", index);

            soundJson = json.Substring(index, nextIndex - index);
            index = nextIndex + 1;
            nextIndex = json.IndexOf("\n", index);

            inputJson = json.Substring(index, nextIndex - index);
            index = nextIndex + 1;

            gameJson = json.Substring(index);
        }
        catch (ArgumentOutOfRangeException) // json이 잘못된 경우
        {
            return false;
        }

        graphicsSettings = JsonUtility.FromJson<GraphicsSettings>(graphicsJson);
        soundSettings = JsonUtility.FromJson<SoundSettings>(soundJson);
        inputSettings = JsonUtility.FromJson<InputSettings>(inputJson);
        gameSettings = JsonUtility.FromJson<GameSettings>(gameJson);

        return true;
    }

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

        gameSettings.continuousTyping = defaultSettings.ContinuousTyping;
        gameSettings.autoSave = defaultSettings.AutoSave;
        gameSettings.autoAim = defaultSettings.AutoAim;
    }

    public void ApplySettings()
    {
        
    }
}
