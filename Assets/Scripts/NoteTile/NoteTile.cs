using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteTile : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRenderer;

    public float NoteBeat = 0;
    public int LaneIndex = -1;
    public bool IsHold = false;

    public Vector2 SpawnLocation;

    private float _duration = 1f;
    private Color _originColor;

    private void Awake()
    {
        _originColor = _spriteRenderer.color;
    }

    //Handle case of hold tile
    public void SetLongTile(float duration)
    {
        Vector2 size = _spriteRenderer.size;
        size.y = size.y * duration;

        _duration = duration; 

        _spriteRenderer.size = size;
    }

    //Reset tile to default state after being returned to pool
    public void ResetTile()
    {
        Vector2 size = _spriteRenderer.size;
        size.y = size.y / _duration;
        _spriteRenderer.color = _originColor;

        _duration = 1f;
        LaneIndex = -1;
        IsHold = false;
    }

    public float GetDuration => _duration;
    public SpriteRenderer GetSpriteRenderer => _spriteRenderer;
}
