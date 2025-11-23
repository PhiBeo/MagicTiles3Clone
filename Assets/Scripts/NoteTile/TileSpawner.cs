using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSpawner : MonoBehaviour
{
    private Chart _chart;
    private int _nextTileIndex = 0;

    private TileManager _tileManager;
    private LevelManager _levelManager;
    private TilePool _tilePool;

    private void Start()
    {
        _tileManager = TileManager.Instance;
        _levelManager = LevelManager.Instance;
        _tilePool = TilePool.Instance;

        _chart = _levelManager.GetChart;
    }

    private void Update()
    {
        double currentBeat = _levelManager.GetCurrentBeat();

        //Keep track of the next tile to spawn
        //After iterate through all the tiles that need to be spawned within the beat spawn lead, break the loop
        while (_nextTileIndex < _chart.notes.Count)
        {
            var note = _chart.notes[_nextTileIndex];
            float noteBeat = note.beat;

            if (currentBeat >= noteBeat - _levelManager.BeatToApproach)
            {
                //SpawnTile(note, Mathf.Abs(currentBeat - noteBeat));
                SpawnTile(note);
                _nextTileIndex++;
            }
            else break;
        }
    }

    private void SpawnTile(Note note)
    {
        Vector2 lane = Vector2.zero;
        lane.x = GlobalValue.LaneXPosition[note.lane - 1];
        Vector2 spawnPos = lane + Vector2.up * (GlobalValue.SpawnY - (GlobalValue.TileSize.y / 2f));

        //Get tile from pool and set its properties
        var tile = _tilePool.GetTile();
        tile.transform.position = spawnPos;
        
        tile.NoteBeat = note.beat;
        tile.LaneIndex = note.lane;
        tile.SpawnLocation = spawnPos;

        if (note.isLongNote)
        {
            tile.SetLongTile(note.duration);
            tile.IsHold = true;
        }

        _tileManager.RegisterTile(tile);
    }
}
