using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilePool : MonoBehaviour
{
    public static TilePool Instance { get; private set; }

    [SerializeField] private GameObject _tilePrefab;
    [SerializeField] private Transform _poolParent;
    [SerializeField] private int _initialPoolSize = 15;

    private Queue<NoteTile> _pool = new Queue<NoteTile>();

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

        for (int i = 0; i < _initialPoolSize; i++)
        {
            CreateTile();
        }
    }

    // Create a new tile and add it to the pool
    // Tile is deactivated and reset before being added
    private void CreateTile()
    {
        GameObject tileObj = Instantiate(_tilePrefab, _poolParent);
        NoteTile tile = tileObj.GetComponent<NoteTile>();
        tile.ResetTile();
        tileObj.SetActive(false);
        _pool.Enqueue(tile);
    }

    // Get a tile from the pool
    // If pool is empty, create a new tile
    public NoteTile GetTile()
    {
        if(_pool.Count == 0)
        {
            CreateTile();
        }
        NoteTile tile = _pool.Dequeue();
        tile.gameObject.SetActive(true);
        return tile;
    }

    public void ReturnTile(NoteTile tile)
    {
        tile.ResetTile();
        tile.gameObject.SetActive(false);
        _pool.Enqueue(tile);
    }
}
