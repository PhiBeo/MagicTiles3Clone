using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/*
This UI manager is a very simple implementation a proper UI management system should be more robust
- Individual UI panels should be their own components handling their own logic not all in one monolithic class
*/

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("Win Panel")]
    [SerializeField] private GameObject _winPanel;
    [SerializeField] private TextMeshProUGUI _finalScoreText;
    [SerializeField] private TextMeshProUGUI _maxComboText;
    [SerializeField] private TextMeshProUGUI _missCountText;

    [Header("Gameover Panel")]
    [SerializeField] private GameObject _gameoverPanel;

    [Header("Gameplay Panel")]
    [SerializeField] private GameObject _gameplayPanel;
    [SerializeField] private List<ClickDisplay> _clickDisplays = new List<ClickDisplay>(Enum.GetValues(typeof(HitQuality)).Length);
    [SerializeField] private TextMeshProUGUI _hitQualityText;
    [SerializeField] private TextPopup _hitQualityPopup;
    [SerializeField] private TextMeshProUGUI _comboText;
    [SerializeField] private Slider _progressBar;

    private GameManager _gameManager;
    private InputHandler _inputHandler;
    private AudioManager _audioManager;

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

        _winPanel?.SetActive(false);
        _gameoverPanel?.SetActive(false);
        _gameplayPanel?.SetActive(false);
    }

    private void Start()
    {
        _gameManager = GameManager.Instance;
        _audioManager = AudioManager.Instance;
        var state = _gameManager.CurrentState;

        _winPanel?.SetActive(state == GameState.LevelComplete);
        _gameoverPanel?.SetActive(state == GameState.GameOver);
        _gameplayPanel?.SetActive(state == GameState.StartingLevel);

        //Stack system is gonna better for bigger project
        // Here just use simple event subscription for demo purpose
        _gameManager.OnLevelComplete += () =>
        {
            _winPanel?.SetActive(true);
            _gameoverPanel?.SetActive(false);
            _gameplayPanel?.SetActive(false);
            _finalScoreText.text = $"Score: {ScoreManager.Instance.GetScore()}";
            _maxComboText.text = $"Max Combo: {ScoreManager.Instance.GetMaxCombo()}";
            _missCountText.text = $"Misses: {ScoreManager.Instance.GetMissCount()}";
        };

        _gameManager.OnGameOver += () =>
        {
            _winPanel?.SetActive(false);
            _gameoverPanel?.SetActive(true);
            _gameplayPanel?.SetActive(false);
        };

        _hitQualityPopup.OnComplete += () => _comboText.gameObject.SetActive(false);
        _comboText.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (_gameManager.CurrentState != GameState.Playing) return;

        var songProgress = Mathf.Clamp01(_audioManager.SongTime / _audioManager.SongLength);

        _progressBar.value = songProgress;
    }

    public void DisplayHitQuality(HitQuality quality)
    {
        var data = _clickDisplays[(int)quality];
        _comboText.gameObject.SetActive(false);

        _hitQualityText.text = data.displayText;
        _hitQualityText.color = data.textColor;

        _hitQualityPopup.PlayPopup();

        if (quality == HitQuality.Miss || quality == HitQuality.Good) return;

        _comboText.text = $"x{ScoreManager.Instance.GetCombo()}";
        _comboText.gameObject.SetActive(true);
    }

    [Serializable]
    private class ClickDisplay
    { 
        public string displayText;
        public Color textColor;
    }

}
