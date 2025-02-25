using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    public BGMManager bgmManager;
    public SFXManager sfxManager;

    [Range(0, 1)] public float bgmVolume = 1.0f;
    [Range(0, 1)] public float sfxVolume = 1.0f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // BGMManager와 SFXManager도 DontDestroyOnLoad 유지
        if (bgmManager == null)
        {
            bgmManager = gameObject.AddComponent<BGMManager>();
            DontDestroyOnLoad(bgmManager);
        }
        if (sfxManager == null)
        {
            sfxManager = gameObject.AddComponent<SFXManager>();
            DontDestroyOnLoad(sfxManager);
        }
    }
}
