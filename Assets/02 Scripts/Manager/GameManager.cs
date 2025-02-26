using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public bool isWin = false;

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
        }
        Application.targetFrameRate = 30;
        QualitySettings.vSyncCount = 0;
    }

    public void GameOver()
    {
        IsGameClear isGameClear = TimeManager.Instance.GetNowState();
        if (isGameClear == IsGameClear.Clear) isWin = true;
        DataManager.Instance.UpdateBestScore();
        TimeManager.Instance.UpdateBestTime();
        SceneManager.LoadScene("ResultScene");
        SoundManager.Instance.bgmManager.StopBGM();
    }
}
