using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.Rendering;

public class AudioManager : MonoBehaviour
{
    public AudioMixer audioMixer;

    [SerializeField] AudioSource sESource;
    [SerializeField] AudioSource bGMSource;

    [SerializeField] List<SEData> sEDataList;
    [SerializeField] List<BGMData> bGMDataList;
    [SerializeField] Slider seSlider;
    [SerializeField] Slider bgmSlider;

    public static AudioManager Instance;

    public enum VolumeParam
    {
        SE,
        BGM
    }

    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // Volumeを初期化　初プレイ時は音量3
        foreach(var param in Enum.GetNames(typeof(VolumeParam)))
        {
            //SetVolume(param, PlayerPrefs.GetFloat(param.ToString() + "SliderValue", 0));
            SetVolume(param, 3);
        }
        seSlider.value = 3;
        bgmSlider.value = 3;
    }

    // --------------------------------------------------

    public void PlaySE(SEData.SEName sEName, AudioSource audioSource = null)
    {
        // 引数を指定しなかったら、sESourceから出力
        if(audioSource == null)
        {
            audioSource = sESource;
        }

        var data = sEDataList.Find(data => data.sEName == sEName);
        audioSource.PlayOneShot(data.sEClip);
    }

    public void PlayBGM(BGMData.BGMName bGMName)
    {
        var data = bGMDataList.Find(data => data.bGMName == bGMName);
        bGMSource.clip = data.bGMClip;
        bGMSource.Play();
    }

    public void StopBGM()
    {
        bGMSource.Stop();
    }

    public void SetVolume(string volumeParam, float sliderValue)
    {
        sliderValue /= 5;
        float volumeValue =  20 * Mathf.Log10(sliderValue) - 20; // 最大で0dBに調整
        volumeValue = Mathf.Max(-80, volumeValue); // 最低で-100dBに設定
        audioMixer.SetFloat(volumeParam, volumeValue);

        // sliderValueを保存　volumeValueじゃないよ
        //PlayerPrefs.SetFloat(volumeParam + "SliderValue", sliderValue);
    }

    public void SetSEVolume()
    {
        SetVolume("SE", seSlider.value*2);
    }

    public void SetBGMVolume()
    {
        SetVolume("BGM", bgmSlider.value*2);
    }
}

[System.Serializable]
public class SEData
{
    public enum SEName
    {
        Click,
        TileFlip,
        TileClick,
        Correct,
        Incorrect,
        Fall,
    }

    public SEName sEName;
    public AudioClip sEClip;
}

[System.Serializable]
public class BGMData
{
    public enum BGMName
    {
        InGame,
        Ending,
    }

    public BGMName bGMName;
    public AudioClip bGMClip;
}

