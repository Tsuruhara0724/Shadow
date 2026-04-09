﻿using System.Collections;
using System.Collections.Generic;
using EazyTools.SoundManager;
using UnityEngine;
 
public class AudioControl : MonoBehaviour
{
    // 公共变量，用于存储声音和音乐剪辑的字典
    public ObjectDictionary SoundClips;
    public ObjectDictionary MusicClips;

    // 默认音乐的键值
    public string DefaultMusic;

    // 布尔值，控制是否忽略重复音乐播放
    public bool IgnoreDuplicateMusic = true;

    // 静态变量，存储当前播放的音乐ID和音乐键值
    private static int _currentMusicId;
    private static string _currentMusic;

    // 当脚本启用时调用
    void OnEnable()
    {
        // 此方法目前没有实现任何功能
    }

    void Start()
    {
        // 将当前对象注册到Hub（Hub是一个统一管理脚本的类）
        Hub.Register(this);
    }

    // 播放默认音乐
    public void PlayDefaultMusic()
    {
        PlayDefaultMusic(SoundManager.globalMusicVolume);
    }
    // 通过键值播放音乐
    public void PlayMusic(string key)
    {
        PlayMusic(key, SoundManager.globalMusicVolume);
    }
    // 通过键值播放声音
    public void PlaySound(string key)
    {
        PlaySound(key, SoundManager.globalSoundsVolume);
    }
    // 通过音频剪辑播放音乐
    public void PlayMusic(AudioClip clip)
    {
        PlayMusic(clip, SoundManager.globalMusicVolume);
    }
    // 通过音频剪辑播放声音
    public void PlaySound(AudioClip clip)
    {
        PlaySound(clip, SoundManager.globalSoundsVolume);
    }
    // 播放默认音乐，带有可选参数
    public void PlayDefaultMusic(float volume = 0, bool loop = true, bool persist = true)
    {
        PlayMusic(DefaultMusic, volume, loop, persist);
    }
    // 通过键值播放音乐，带有可选参数
    public int PlayMusic(string key, float volume = 0, bool loop = true, bool persist = true)
    {
        // 如果忽略重复音乐且当前音乐与新播放音乐相同，返回当前音乐ID
        if (IgnoreDuplicateMusic && _currentMusic == key)
            return _currentMusicId;
 
        Object clip;
        // 从音乐字典中获取音频剪辑
        var found = MusicClips.TryGetValue(key, out clip);
        if (found)
        {
            // 设置当前音乐并播放
            _currentMusic = key;
            _currentMusicId = PlayMusic((AudioClip)clip, volume, loop, persist);
            return _currentMusicId;
        }
 
        return -1; // 如果未找到音乐，返回-1
    }
    // 通过键值播放音乐，带有可选参数
    public int PlayMusic(AudioClip clip, float volume = 0, bool loop = true, bool persist = true)
    {
        return SoundManager.PlayMusic(clip, volume, loop, persist);
    }
    // 通过键值播放声音，带有可选参数
    public int PlaySound(string key, float volume = 0, Transform sourceTransform = null)
    {
        Object clip;
        // 从声音字典中获取音频剪辑
        var found = SoundClips.TryGetValue(key, out clip);
        if (found)
        {
            return PlaySound((AudioClip)clip, volume, sourceTransform);
        }
 
        return -1;
    }
    // 通过音频剪辑播放声音，带有可选参数
    public int PlaySound(AudioClip clip, float volume = 0, Transform sourceTransform = null)
    {
        return SoundManager.PlaySound(clip, volume, false, sourceTransform);
    }
}