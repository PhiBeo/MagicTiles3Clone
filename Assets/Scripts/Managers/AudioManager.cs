using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [SerializeField] private AudioSource _musicSource;

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
        _musicSource.clip = LevelManager.Instance.GetChart.musicClip;

        // Start playing music when the level starts with a delay
        LevelManager.Instance.OnStartLevel += () => 
        { 
            _musicSource.Play(); 
            StartCoroutine(SongTimer());
        };

        GameManager.Instance.OnGameOver += StopSong;
    }

    private IEnumerator SongTimer()
    {
        yield return new WaitForSeconds(_musicSource.clip.length + 2f);
        GameManager.Instance.CompleteLevel();
    }

    public void StopSong()
    {
        _musicSource.Stop();
    }

    public float SongTime => _musicSource.time;
    public float SongLength => _musicSource.clip.length;
    public bool IsPlaying => _musicSource.isPlaying;
}
