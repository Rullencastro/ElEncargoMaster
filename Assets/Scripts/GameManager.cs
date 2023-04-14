using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;


    private WrapperHighScores _highScores;
    private string filePath;
    private float _gameTimer = 30;
    private int wallsDestroyed;

    [SerializeField]
    private float _gameTime;

    private bool _gamePaused;

    protected virtual void Awake()
    {
        if (Instance == null)
            Instance = this;

        _highScores = new WrapperHighScores();
        filePath = "highScores.json";
        //arreglar lo de crear archivo si no existe
        _highScores.LoadScores(filePath);
    }

       
    private void Start()
    {
        _gameTimer = _gameTime;
        wallsDestroyed = 0;
        UIManager.Instance.UpdateWallDestroyed(wallsDestroyed);
        MazeGenerator.Instance.GenerateMaze();
    }

    
    private void Update()
    {
        GameTime();
    }

    private void GameTime()
    {
        if (_gameTimer > 0 && !_gamePaused)
            _gameTimer -= Time.deltaTime;

        if (_gameTimer < 0 && !_gamePaused)
        {
            GameOver();
        }

        UIManager.Instance.UpdateTimer(_gameTimer);
    }

    public void LevelCompleted()
    {
        _highScores.AddScore(new ScoreData(wallsDestroyed,(int)(_gameTime - _gameTimer)));
        _highScores.SaveScores(filePath);
        Pause();
        UIManager.Instance.UpdateBestScoresUI(_highScores);
        UIManager.Instance.LevelCompleted();
        //arreglar highscores
    }

    public void DestroyWall()
    {
        wallsDestroyed++;
        UIManager.Instance.UpdateWallDestroyed(wallsDestroyed);
    }

    private void UnPause()
    {
        _gamePaused = false;
    }


    private void Pause()
    {
        _gamePaused = true;
    }

    public bool GameStatus()
    {
        return _gamePaused;
    }

    public void ResetGame()
    {
        _gameTimer = _gameTime;
        UnPause();
        wallsDestroyed = 0;
        MazeGenerator.Instance.ResetMaze();
        UIManager.Instance.ResetUI();
    }

    private void GameOver()
    {
        Pause();
        UIManager.Instance.UpdateBestScoresUI(_highScores);
        UIManager.Instance.GameOver();
    }
}
