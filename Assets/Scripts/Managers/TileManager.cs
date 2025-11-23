using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    public static TileManager Instance { get; private set; }

    private List<NoteTile> _activeTiles= new List<NoteTile>();
    private float _scrollSpeed = 0f;

    private LevelManager _levelManager;
    private TilePool _tilePool;
    private GameManager _gameManager;
    private TileAnimation _tileAnimation;
    [SerializeField] private ParticlePool _particlePool;

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
        _levelManager = LevelManager.Instance;
        _tilePool = TilePool.Instance;
        _gameManager = GameManager.Instance;
        _tileAnimation = TileAnimation.Instance;

        // Calculate scroll speed based on BPM
        _scrollSpeed = GlobalValue.UnitPerBeat * (_levelManager.GetChart.bpm/ 60f);
    }

    private void LateUpdate()
    {
        if(_gameManager.CurrentState != GameState.Playing)
        {
            return;
        }

        // Move all active tiles downwards
        // Iterate backwards to safely remove tiles if needed
        // If any tile goes beyond the despawn zone, trigger game over
        // Note: Currently move the tile using const speed (cal using BPM with unit move per beat) not beat synced speed
        for (int i = _activeTiles.Count - 1; i >= 0; i--)
        {
            NoteTile tile = _activeTiles[i];

            tile.transform.position += Vector3.down * _scrollSpeed * Time.deltaTime;

            if (tile.transform.position.y < _levelManager.DespawnZoneY * tile.GetDuration)
            {
                GameManager.Instance?.GameOver();
            }
        }

    }

    public void RegisterTile(NoteTile tile) => _activeTiles.Add(tile);

    //Return the tile to the pool and remove it from active tiles list
    public void UnregisterTile(NoteTile tile)
    {
        _activeTiles.Remove(tile);
        _particlePool.Spawn(tile.transform.position + Vector3.up * GlobalValue.TileSize.y / 2f);

        _tileAnimation.StartFadeOut(tile.GetSpriteRenderer, GlobalValue.tileFadeTime, () =>
        {
            _tilePool.ReturnTile(tile);
        });
    }

    public List<NoteTile> GetActiveTiles => _activeTiles;
}
