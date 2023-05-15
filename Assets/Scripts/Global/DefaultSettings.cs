using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DefaultSettings", menuName = "ScriptableObject/DefaultSettings", order = int.MaxValue)]
public class DefaultSettings : ScriptableObject
{
    [Header("그래픽 설정")]
    [SerializeField]
    private int resolutionWidth;
    public int ResolutionWidth { get { return resolutionWidth; } }
    [SerializeField]
    private int resolutionHeight;
    public int ResolutionHeight { get { return resolutionHeight; } }
    [SerializeField]
    private bool isFullScreen;
    public bool IsFullScreen { get { return isFullScreen; } }
    [Space(10)]

    [SerializeField]
    private bool isVSync;
    public bool IsVSync { get { return isVSync; } }


    [Header("소리 설정")]
    [SerializeField]
    [Range(0,100)]
    private float masterVolume;
    public float MasterVolume { get { return masterVolume; } }
    [Space(10)]

    [SerializeField]
    [Range(0, 100)]
    private float backgroundMusicVolume;
    public float BackgroundMusicVolume { get { return backgroundMusicVolume; } }
    [SerializeField]
    [Range(0, 100)]
    private float effectSoundVolume;
    public float EffectSoundVolume { get { return effectSoundVolume; } }
    [SerializeField]
    [Range(0, 100)]
    private float voiceVolume;
    public float VoiceVolume { get { return voiceVolume; } }
    [SerializeField]
    [Range(0, 100)]
    private float uiVolume;
    public float UIVolume { get { return uiVolume; } }


    [Header("입력 설정")]
    [SerializeField]
    private KeyCode positive;
    public KeyCode Positive { get { return positive; } }
    [SerializeField]
    private KeyCode negative;
    public KeyCode Negative { get { return negative; } }
    [Space(10)]

    [SerializeField]
    private KeyCode pauseMenu;
    public KeyCode PauseMenu { get { return pauseMenu; } }

    [Space(10)]
    [SerializeField]
    private KeyCode up;
    public KeyCode Up { get { return up; } }
    [SerializeField]
    private KeyCode down;
    public KeyCode Down { get { return down; } }
    [SerializeField]
    private KeyCode left;
    public KeyCode Left { get { return left; } }
    [SerializeField]
    private KeyCode right;
    public KeyCode Right { get { return right; } }
    [Space(10)]

    [SerializeField]
    private KeyCode attack;
    public KeyCode Attack { get { return attack; } }
    [SerializeField]
    private KeyCode rolling;
    public KeyCode Rolling { get { return rolling; } }
    [SerializeField]
    private KeyCode interactions;
    public KeyCode Interactions { get { return interactions; } }
    [SerializeField]
    private KeyCode spell;
    public KeyCode Spell { get { return spell; } }
    [Space(10)]

    [SerializeField]
    private KeyCode inventory;
    public KeyCode Inventory { get { return inventory; } }
    [SerializeField]
    private KeyCode spellBook;
    public KeyCode SpellBook { get { return spellBook; } }


    [Header("게임 설정")]
    [SerializeField]
    private bool continuousTyping;
    public bool ContinuousTyping { get { return continuousTyping; } }
}
