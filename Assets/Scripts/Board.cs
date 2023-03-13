using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Board : MonoBehaviour, IEnumerable<Tile>
{
    private RectTransform _rectTransform;
    private List<Tile> _tiles; 
    public GameObject tilePrefab;
    public int columns = 15;
    public int rows = 10;
    
    public IEnumerable<Vector2Int> Positions
    {
        get
        {
            int x = 0, y = 0;

            for (int i = 0; i < rows; i++)
            {
                x = 0;
                for (int j = 0; j < columns; j++)
                {
                    yield return new Vector2Int(x, y);

                    x++;
                }

                y++;
            }
        }
    }

    public IEnumerable<Vector2Int> EmptyPositions
    {
        get
        {
            return Positions.Where((p) => { return this[p].Content == TileContent.Empty; });
        }
    }

    void Awake()
    {
        _rectTransform = transform as RectTransform;

        // Calculate tile size (assuming board always have to fit whole panel's width).
        var width = _rectTransform.rect.width;
        var tileSize = width / columns;
        var halfTileSize = tileSize / 2;

        // Change panel height to contain tiles.
        _rectTransform.sizeDelta = new Vector2(width, tileSize * rows);

        // Fill the board with rows * columns number of tiles
        _tiles = new List<Tile>();

        float y = 0;

        for (int i = 0; i < rows; i++)
        {
            float x = 0;
            for (int j = 0; j < columns; j++)
            {
                var tile = Instantiate(tilePrefab, new Vector3(x + halfTileSize, -y - halfTileSize, 0), Quaternion.identity).GetComponent<Tile>();
                tile.transform.SetParent(_rectTransform, false);
                tile.RectTransform.sizeDelta = new Vector2(tileSize, tileSize);
                tile.name = $"Tile -> ({i}, {j})";
                _tiles.Add(tile);

                x += tileSize;
            }

            y += tileSize;
        }

        this[5, 5].Content = TileContent.Apple;
    }

    public IEnumerator<Tile> GetEnumerator()
    {
        return ((IEnumerable<Tile>)_tiles).GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return ((IEnumerable<Tile>)_tiles).GetEnumerator();
    }

    /// <summary>
    /// Indexer for retrieving tiles at given position.
    /// </summary>
    /// <param name="x">x coordinate</param>
    /// <param name="y">y coordinate</param>
    /// <returns>tile at given position</returns>
    public Tile this[int x, int y]
    {
        get
        {
            if (!(x >= 0 && x < columns))
            {
                throw new System.ArgumentOutOfRangeException("x", "x coordinate must be between 0 and the number of columns.");
            }

            if (!(y >= 0 && y < rows))
            {
                throw new System.ArgumentOutOfRangeException("y", "y coordinate must be between 0 and the number of rows.");
            }

            return _tiles[y * columns + x];
        }
    }

    /// <summary>
    /// Indexer for retrieving tiles at given position.
    /// </summary>
    /// <param name="position">coordinates of wanted tile</param>
    /// <returns>tile at given position</returns>
    public Tile this[Vector2Int position] => this[position.x, position.y];
    
    
    public void Reset()
    {
        foreach (var tile in this)
        {
            tile.Reset();
        }
    }
}
