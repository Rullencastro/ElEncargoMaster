using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI wallDestroyedText;
    public TextMeshProUGUI bestScoresPosText;
    public TextMeshProUGUI bestScoresWallsText;
    public TextMeshProUGUI bestScoresTimeText;

    public Image gameOverPanel;
    public Image levelCompletedPanel;
    public Image bestScoresPanel;

    public static UIManager Instance;

    protected virtual void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    public void UpdateTimer(float gameTimer)
    {
        timeText.text = ":" + (int)gameTimer;
    }

    public void UpdateWallDestroyed(int wallDestroyed)
    {
        wallDestroyedText.text = wallDestroyed.ToString();
    }

    public void ResetUI()
    {
        gameOverPanel.gameObject.SetActive(false);
        bestScoresPanel.gameObject.SetActive(false);
        levelCompletedPanel.gameObject.SetActive(false);
        UpdateWallDestroyed(0);
    }

    public void GameOver()
    {
        gameOverPanel.gameObject.SetActive(true);
        bestScoresPanel.gameObject.SetActive(true);
    }

    public void LevelCompleted()
    {
        levelCompletedPanel.gameObject.SetActive(true);
        bestScoresPanel.gameObject.SetActive(true);
    }

    public void UpdateBestScoresUI(WrapperHighScores _highScores)
    {
        string posTxt = "";
        string wallsTxt = "";
        string timeTxt = "";

        for (int i = 0; i < _highScores.scores.Count; i++)
        {
            posTxt += "<color=#000000>" + (i + 1) + ".-</color><br>";
            wallsTxt += "<color=#A98307>" + _highScores.scores[i].wallsDestroyed + "</color><br>";
            timeTxt += " <color=#A98307>" + _highScores.scores[i].time + "</color><br>";
        }

        bestScoresPosText.GetComponent<TextMeshProUGUI>().text = posTxt;
        bestScoresWallsText.GetComponent<TextMeshProUGUI>().text = wallsTxt;
        bestScoresTimeText.GetComponent<TextMeshProUGUI>().text = timeTxt;
    }
}
