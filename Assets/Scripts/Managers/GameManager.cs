using System;
using UnityEngine;
using UnityEngine.SceneManagement;

[Serializable]
public enum GameState
{
    StartingLevel,
    Playing,
    GameOver,
    LevelComplete
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private GameState _currentState = GameState.StartingLevel;

    public Action OnGameOver;
    public Action OnLevelComplete;

    [Header("Debug")]
    public bool AutoClear = false;

    TileManager _tileManager;
    InputHandler _inputHandler;
    LevelManager _levelManager;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Destroy duplicate instances
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        _tileManager = TileManager.Instance;
        _levelManager = LevelManager.Instance;
        _inputHandler = FindAnyObjectByType<InputHandler>();
    }

    private void Update()
    {
        //This can be remove later on
        //Not the most optimal but if you feel lazy just take it on=)
        if (!AutoClear) return;

        var activeTiles = _tileManager.GetActiveTiles;
        if (activeTiles.Count <= 0) return;

        for (int i = activeTiles.Count - 1; i >= 0; i--)
        {
            var tile = activeTiles[i];

            if (_levelManager.GetCurrentBeat() >= tile.NoteBeat)
            {
                _inputHandler.CheckLane(tile.LaneIndex, tile.transform.position + Vector3.up * GlobalValue.TileSize.y / 2f);
            }
        }
    }

    public void GameOver()
    {
        _currentState = GameState.GameOver;
        OnGameOver?.Invoke();
        Debug.Log("Game Over!");
    }

    public void CompleteLevel()
    {
        _currentState = GameState.LevelComplete;
        OnLevelComplete?.Invoke();
        Debug.Log("Level Complete!");
    }

    public void RestartLevel()
    {
        Debug.Log("Level Restarted!");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void StartLevel() => _currentState = GameState.Playing;

    public GameState CurrentState => _currentState;
}
