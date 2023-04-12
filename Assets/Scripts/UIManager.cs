using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI timeUI;
    public Image gameOverPanel;

    public static UIManager Instance;

    protected virtual void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    public void UpdateTimer(float gameTimer)
    {
        timeUI.text = ":" + (int)gameTimer;
    }

    public void ResetUI()
    {
        gameOverPanel.gameObject.SetActive(false);
    }

    public void GameOver()
    {
        Debug.Log("HAS GANADO2");
        gameOverPanel.gameObject.SetActive(true);
    }
}
