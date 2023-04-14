using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI wallDestroyedText;
    public TextMeshProUGUI bestScoresText;

    public Image gameOverPanel;
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
        UpdateWallDestroyed(0);
    }

    public void GameOver()
    {
        gameOverPanel.gameObject.SetActive(true);
        bestScoresPanel.gameObject.SetActive(true);
    }

    public void UpdateBestScoresUI(WrapperHighScores _highScores)
    {
        string bestScoreTxt = "<color=#FF8000>BEST SCORES</color><br>";

        for (int i = 0; i < _highScores.scores.Count; i++)
        {
            bestScoreTxt += "<br><color=#000000>" + (i + 1) + ".-</color> <color=#A98307>" + _highScores.scores[i].wallsDestroyed + "</color>";
        }

        bestScoresText.GetComponent<TextMeshProUGUI>().text = bestScoreTxt;
    }
}
