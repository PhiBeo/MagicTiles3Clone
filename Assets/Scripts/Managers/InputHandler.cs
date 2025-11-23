using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    private Vector2 _halfDisplaySize = Vector2.zero;
    private ScoreManager _scoreManager;
    private TileManager _tileManager;
    private GameManager _gameManager;

    public Action OnTileClicked;

    private void Awake()
    {
        Vector2 displaySize = Vector2.zero;

        displaySize.y = Camera.main.orthographicSize * 2f;
        displaySize.x = displaySize.y * Camera.main.aspect;

        _halfDisplaySize = displaySize / 2f;
    }

    private void Start()
    {
        _tileManager = TileManager.Instance;
        _scoreManager = ScoreManager.Instance;
        _gameManager = GameManager.Instance;
    }

    private void Update()
    {
        if(_gameManager.CurrentState != GameState.Playing) return;

        MouseInputHandle();
    }

    private void MouseInputHandle()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

            //Check if the mouse is inside the display area
            if (Mathf.Abs(mousePos2D.x) > _halfDisplaySize.x || Mathf.Abs(mousePos2D.y) > _halfDisplaySize.y) return;

            //Go through each lane to see if the mouse is inside
            //Break the loop after find the mouse hovering lane
            for (int i = 0; i < GlobalValue.LaneXPosition.Length; i++)
            {
                float left = GlobalValue.LaneXPosition[i] - (GlobalValue.LaneSize.x / 2f);
                float right = GlobalValue.LaneXPosition[i] + (GlobalValue.LaneSize.x / 2f);

                if(mousePos2D.x >= left && mousePos2D.x <= right)
                {
                    CheckLane(i + 1, mousePos2D);
                    break;
                }
            }
        }
    }

    public void CheckLane(int lane, Vector2 inputPos)
    {
        List<NoteTile> activeTiles = _tileManager.GetActiveTiles;
        NoteTile hitTile = null;
        HitQuality hitQuality = HitQuality.Miss;

        foreach (var tile in activeTiles)
        {
            //Check if the current active tile is in the correct lane
            //Check if the closest tile reach the visible display area
            if (tile.LaneIndex != lane) continue;

            hitTile = tile;
            Vector2 tileCenterPos = tile.transform.position + (Vector3.up * (GlobalValue.TileSize.y / 2f));
            float distanceError = Mathf.Abs(tileCenterPos.y - LevelManager.Instance.HitZoneY);

            if (!GlobalValue.IsWithinRange(inputPos, tileCenterPos, GlobalValue.TileSize)) return;

            //Determine the hit quality based on the distance error
            //Note: Using beat to check might give more inconsistent result due to game time vs music time desync in the long run for this system
            if (distanceError <= GlobalValue.perfectDistance)
            {
                hitQuality = HitQuality.Perfect;
            }
            else if (distanceError <= GlobalValue.greatDistance)
            {
                hitQuality = HitQuality.Great;
            }
            else if (distanceError <= GlobalValue.goodDistance)
            {
                hitQuality = HitQuality.Good;
            }
            else
            {
                hitQuality = HitQuality.Miss;
            }

            break;
        }

        if (hitTile == null) return;

        //Handle case the the tile is a tap tile
        if (!hitTile.IsHold)
        {
            _scoreManager.RegisterHit(hitQuality);
            UIManager.Instance.DisplayHitQuality(hitQuality);
            _tileManager.UnregisterTile(hitTile);
        }

        OnTileClicked?.Invoke();

        //TODO: Handle case the tile is a hold tile
    }
}
