using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PeriodSound : MonoBehaviour
{
    // 公共数组，用于存储音频剪辑的名称
    public string[] AudioClipNames;

    // 公共变量，用于设置播放声音的最小和最大时间间隔
    public float PeriodMin = 7f;
	public float PeriodMax = 15f;

    // 在脚本启动时调用
    private void Start()
	{
        // 在随机时间间隔后调用PlayRandomSoundInInterval方法
        Invoke(nameof(PlayRandomSoundInInterval), Random.Range(PeriodMin, PeriodMax));
	}

    // 播放随机声音并设置下一次播放的时间间隔
    void PlayRandomSoundInInterval()
	{
        // 随机选择一个音频剪辑名称
        int idx = Random.Range(0, AudioClipNames.Length);
		var key = AudioClipNames[idx];
        // 使用Hub获取AudioControl实例并播放选定的音频剪辑
        Hub.Get<AudioControl>().PlaySound(key);

        // 在随机时间间隔后再次调用PlayRandomSoundInInterval方法
        Invoke(nameof(PlayRandomSoundInInterval), Random.Range(PeriodMin, PeriodMax));
	}
}
