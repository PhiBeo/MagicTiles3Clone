using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    [SerializeField] private Chart _chart;
    [SerializeField] private float _hitZoneY = -3.5f;
    [SerializeField] private float _despawnZoneY = -6f;
    [SerializeField] private Transform _hitZoneTransform;

    private double _songStartTime;
    private float _beatToApproach;
    public float _currentBeat;

    private AudioManager _audioManager;

    public Action OnStartLevel;

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

        _hitZoneTransform.position = new Vector3(0f, _hitZoneY, 0f);
        _beatToApproach = (GlobalValue.SpawnY - _hitZoneY) / GlobalValue.UnitPerBeat;

        Debug.Log($"Sound Length: {_chart.musicClip.length}");
        Debug.Log($"BPM: {_chart.bpm}");
        Debug.Log($"BPS: {_chart.bpm / 60f}");
    }

    private void Start()
    {
        _audioManager = AudioManager.Instance;

        // Start the level after a short delay
        StartCoroutine(StartLevel());
    }

    private IEnumerator StartLevel()
    {
        yield return new WaitForSeconds(2f);
        GameManager.Instance.StartLevel();
        OnStartLevel?.Invoke();
        _songStartTime = AudioSettings.dspTime;
    }

    private void Update()
    {
        _currentBeat = (float)GetCurrentBeat();
    }

    public double GetCurrentBeat()
    {
        if (!_audioManager.IsPlaying) return 0f;

        double songTime = AudioSettings.dspTime - _songStartTime;
        return (songTime * (_chart.bpm / 60f));
    }

    public Chart GetChart => _chart;
    public float HitZoneY => _hitZoneY;
    public float DespawnZoneY => _despawnZoneY;
    public float BeatToApproach => _beatToApproach;
}
