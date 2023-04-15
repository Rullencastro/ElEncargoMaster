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
    public Image instructionsPanel;

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
        gameOverPanel.gameObject.GetComponent<PanelAnimation>().DisablePanel();
        bestScoresPanel.gameObject.GetComponent<PanelAnimation>().DisablePanel();
        levelCompletedPanel.gameObject.GetComponent<PanelAnimation>().DisablePanel();
        UpdateWallDestroyed(0);
    }

    public void OpenInstructions()
    {
        instructionsPanel.gameObject.GetComponent<PanelAnimation>().ShowPanel();
    }

    public void CloseInstructions()
    {
        instructionsPanel.gameObject.GetComponent<PanelAnimation>().DisablePanel();
    }

    public void GameOver()
    {
        gameOverPanel.gameObject.GetComponent<PanelAnimation>().ShowPanel();
        bestScoresPanel.gameObject.GetComponent<PanelAnimation>().ShowPanel();
    }

    public void LevelCompleted()
    {
        levelCompletedPanel.gameObject.GetComponent<PanelAnimation>().ShowPanel();
        bestScoresPanel.gameObject.GetComponent<PanelAnimation>().ShowPanel();
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
