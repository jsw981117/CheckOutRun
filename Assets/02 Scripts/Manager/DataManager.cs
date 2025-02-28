using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance { get; private set; }
    GameUI gameUI;

    private int score;

    public int Score { get { return score; } }

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
    }

    //점수 데이터부분 초기화 작업
    public void Init()
    {
        score = 0;
        gameUI = FindObjectOfType<GameUI>();
    }

    //점수 더하기
    public void AddScore(int unitScore)
    {
        score += unitScore;
        gameUI.UpdateScoreUI();
    }

    //최고기록 갱신
    public void UpdateBestScore()
    {
        int bestScore = PlayerPrefs.GetInt("bestScore");
        if (score > bestScore)
        {
            PlayerPrefs.SetInt("bestScore", score);
        }
    }
}
