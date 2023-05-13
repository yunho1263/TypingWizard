using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DefaultSettings", menuName = "ScriptableObject/DefaultSettings", order = int.MaxValue)]
public class DefaultSettings : ScriptableObject
{
    [Header("언어 설정")]
    [SerializeField]
    private Language language;
    public Language Language { get { return language; } }

    [Header("소리 설정")]
    [SerializeField]
    [Range(0,100)]
    private float masterVolume;
    public float MasterVolume { get { return masterVolume; } }
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


}
