using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileAnimation : MonoBehaviour
{
    public static TileAnimation Instance { get; private set; }
    private List<FadeData> fadeDatas = new List<FadeData>();

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

    private void Update()
    {
        for (int i = fadeDatas.Count - 1; i >= 0; i--)
        {
            FadeData data = fadeDatas[i];

            data.elapsedTime += Time.deltaTime;

            float t = Mathf.Clamp01(data.elapsedTime / data.duration);

            data.renderer.color = new Color(
                data.startColor.r,
                data.startColor.g,
                data.startColor.b,
                Mathf.Lerp(data.startColor.a, 0f, t)
            );

            if (t >= 1f)
            {
                data.OnComplete?.Invoke();
                fadeDatas.RemoveAt(i);
            }
        }
    }

    public void StartFadeOut(SpriteRenderer sr, float duration, Action onComplete = null)
    {
        FadeData data = new FadeData(sr, duration, onComplete);
        fadeDatas.Add(data);
    }

    private class FadeData
    {
        public SpriteRenderer renderer;
        public Color startColor;
        public float duration;
        public float elapsedTime;
        public Action OnComplete;
        public FadeData(SpriteRenderer sr, float dur, Action onC)
        {
            renderer = sr;
            duration = dur;
            elapsedTime = 0f;
            OnComplete = onC;
            startColor = sr.color;
        }
    }
}
