using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class SettingPanel : MonoBehaviour
{
    [Header("音量滑条")]
    public Slider volumeSlider;

    [Header("音乐组件")]
    public AudioSource musicAudioSource;

    void OnEnable()
    {
        volumeSlider.value = musicAudioSource.volume;
    }

    void Start()
    {
        volumeSlider.onValueChanged.AddListener(SetVolume);
    }

    public void SetVolume(float value)
    {
        musicAudioSource.volume = value;
    }
}
