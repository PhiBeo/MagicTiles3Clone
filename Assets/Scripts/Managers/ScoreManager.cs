using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum HitQuality
{
    Miss,
    Good,
    Great,
    Perfect
}

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    private int combo = 0;
    private int score = 0;
    private int maxCombo = 0;
    private int missCount = 0;

    public Action<int> OnUpdateScore;

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

    public void RegisterHit(HitQuality quality, float receiveMul = 1f)
    {

        if (quality == HitQuality.Miss)
        {
            combo = 0;
            missCount++;
            return;
        }

        combo++;
        if (quality == HitQuality.Good) combo = 0;
        maxCombo = Mathf.Max(maxCombo, combo);

        int baseScore = quality switch
        {
            HitQuality.Good => Mathf.RoundToInt(100 * receiveMul),
            HitQuality.Great => Mathf.RoundToInt(200 * receiveMul),
            HitQuality.Perfect => Mathf.RoundToInt(300 * receiveMul),
            _ => 0
        };
        score += baseScore + (combo * 10);

        // Notify score update for UI
        OnUpdateScore?.Invoke(score);
    }

    public int GetScore() => score;
    public int GetMaxCombo() => maxCombo;
    public int GetMissCount() => missCount;
    public int GetCombo() => combo;
}
