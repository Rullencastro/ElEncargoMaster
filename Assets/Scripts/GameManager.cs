using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private float _gameTimer = 30;

    public TextMeshProUGUI timeUI;
    // Start is called before the first frame update
    void Start()
    {
        _gameTimer = 30;
        UpdateUI();
    }

    // Update is called once per frame
    void Update()
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

    private void GameOver()
    {
        Debug.Log("HAS PERDIDO");
    }
}
