using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundManager : MonoBehaviour
{
    [Header("Decor")]
    [SerializeField] private SpriteRenderer _decorRenderer;
    [SerializeField] private float _easeUpDuration = 0.08f;
    [SerializeField] private float _easeDownDuration = 0.2f;
    private float _decorFadeTimer = 0f;
    private bool _isPlaying = false;
    private Color _decorOriginColor;

    private enum Phase { Up, Down};
    private Phase _phase;


    private void Awake()
    {
        _decorOriginColor = _decorRenderer.color;
    }

    private void Start()
    {
        FindAnyObjectByType<InputHandler>().OnTileClicked += DecorateReact;
    }

    private void Update()
    {
        if (!_isPlaying) return;

        _decorFadeTimer += Time.deltaTime;

        if (_phase == Phase.Up)
        {
            float tNorm = Mathf.Clamp01(_decorFadeTimer / _easeUpDuration);

            _decorRenderer.color = new Color
                (_decorOriginColor.r,
                _decorOriginColor.g,
                _decorOriginColor.b,
                Mathf.Lerp(_decorOriginColor.a, 255, tNorm));

            if (tNorm >= 1f)
            {
                _decorFadeTimer = 0;
                _phase = Phase.Down;
            }
        }
        else if (_phase == Phase.Down)
        {
            float tNorm = Mathf.Clamp01(_decorFadeTimer / _easeDownDuration);

            _decorRenderer.color = new Color
                (_decorOriginColor.r,
                _decorOriginColor.g,
                _decorOriginColor.b,
                Mathf.Lerp(255, _decorOriginColor.a, tNorm));

            if (tNorm >= 1f)
            {
                _isPlaying = false;
            }
        }
    }

    private void DecorateReact()
    {

        _isPlaying = true;
        _phase = Phase.Up;
        _decorFadeTimer = 0f;

        //Reset Phase
        _decorRenderer.color = _decorOriginColor;
    }

}
