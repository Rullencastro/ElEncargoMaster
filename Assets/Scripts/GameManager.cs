using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private float _gameTimer = 30;

    [SerializeField]
    private float _gameTime;

    protected virtual void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

       
    private void Start()
    {
        _gameTimer = _gameTime;
        MazeGenerator.Instance.GenerateMaze();
    }

    
    private void Update()
    {
        GameTime();
    }

    private void GameTime()
    {
        if (_gameTimer > 0)
            _gameTimer -= Time.deltaTime;

        if (_gameTimer < 0)
        {
            GameOver();
        }

        UIManager.Instance.UpdateTimer(_gameTimer);
    }

    public void LevelCompleted()
    {
        Debug.Log("HAS GANADO");
    }

    public void ResetGame()
    {
        _gameTimer = _gameTime;
        MazeGenerator.Instance.ResetMaze();
        UIManager.Instance.ResetUI();
    }

    private void GameOver()
    {
        Debug.Log("HAS GANADO");
        UIManager.Instance.GameOver();
    }
}
