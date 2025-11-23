using System;
using UnityEngine;

public class TextPopup : MonoBehaviour
{
    // Animation scales
    public Vector3 startScale = Vector3.zero;
    public Vector3 peakScale = new Vector3(1.5f, 1.5f, 1);
    public Vector3 finalScale = Vector3.one;

    // Timing
    public float fastUpDuration = 0.08f;
    public float settleDuration = 0.12f;

    private float timer;
    private bool playing;
    private Transform t;

    public Action OnComplete;

    private enum Phase { Up, Down }
    private Phase phase;

    void Awake() => t = transform;

    private void Start()
    {
        PlayPopup();
        Update();
        gameObject.SetActive(false);
    }

    public void PlayPopup()
    {
        // Restart safely even if animation is playing
        playing = true;
        phase = Phase.Up;
        timer = 0;

        // Reset state
        gameObject.SetActive(true);
        t.localScale = startScale;
    }

    void Update()
    {
        if (!playing) return;

        timer += Time.deltaTime;

        if (phase == Phase.Up)
        {
            float tNorm = Mathf.Clamp01(timer / fastUpDuration);
            t.localScale = Vector3.Lerp(startScale, peakScale, EaseOutQuad(tNorm));

            if (tNorm >= 1f)
            {
                timer = 0;
                phase = Phase.Down;
            }
        }
        else
        { // Down phase
            float tNorm = Mathf.Clamp01(timer / settleDuration);
            t.localScale = Vector3.Lerp(peakScale, finalScale, EaseOutBack(tNorm));

            if (tNorm >= 1f)
            {
                // Animation completed
                OnComplete?.Invoke();
                playing = false;
                gameObject.SetActive(false);
            }
        }
    }

    // ==== EASING FUNCTIONS ====

    float EaseOutQuad(float t) => 1 - (1 - t) * (1 - t);

    float EaseOutBack(float t)
    {
        float c = 1.70158f;
        t -= 1;
        return t * t * ((c + 1) * t + c) + 1;
    }
}
