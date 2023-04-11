using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private float _gameTimer = 30;

    public TextMeshProUGUI timeUI;

    public static GameManager Instance;

    protected virtual void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

       
    private void Start()
    {
        _gameTimer = 30;
        UpdateUI();
    }

    
    private void Update()
    {
        GameTime();
        UpdateUI();
    }

    private void GameTime()
    {
        if (_gameTimer > 0)
            _gameTimer -= Time.deltaTime;

        if (_gameTimer < 0)
        {
            _gameTimer = 30;
            GameOver();
        }
        
    }

    private void UpdateUI()
    {
        timeUI.text = ":" + (int)_gameTimer;
    }

    public void LevelCompleted()
    {
        Debug.Log("HAS GANADO");
    }

    private void GameOver()
    {
        Debug.Log("HAS PERDIDO");
    }
}
