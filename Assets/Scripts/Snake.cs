using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public class Snake : MonoBehaviour, IEnumerable<Vector2Int>
{
    [HideInInspector] public InputController inputController;
    
    private Board _board;
    private LinkedList<Vector2Int> _body;
    private HashSet<Vector2Int> _bulges;

    private Vector2Int Head => _body.Last.Value;
    
    public IEnumerable<Vector2Int> WithoutTail => this.Where((p) => p != _body.First.Value);

    private void Start()
    {
        _board = TrackManager.Instance.board;
        _body = new LinkedList<Vector2Int>();
        _bulges = new HashSet<Vector2Int>();
        inputController.GetComponent<InputController>();
        Reset();
    }

    public void Reset()
    {
        foreach (var p in _body)
        {
            _board[p].Content = TileContent.Empty;
        }

        _body.Clear();

        var start = new Vector2Int(5, 5);
        for (int i = 0; i < 2; i++)
        {
            var position = new Vector2Int(start.x, start.y - i);
            _body.AddLast(position);
        }

        UpdateSnakeState();
    }

    public void Move(Vector2Int direction, bool extend)
    {
        var newHead = NextHeadPosition(direction);

        _body.AddLast(newHead);
        if (extend)
        {
            _bulges.Add(newHead);
        }
        else
        {
            _bulges.Remove(_body.First.Value);
            _board[_body.First.Value].Content = TileContent.Empty;
            _body.RemoveFirst();
        }

        UpdateSnakeState();
    }
    
    private void UpdateSnakeState()
    {
        // Handle head
        var headPosition = _body.Last.Value;
        var nextPosition = _body.Last.Previous.Value;

        var tile = _board[headPosition];
        tile.Content = TileContent.SnakeHead;

        AssignRotation(tile, nextPosition, headPosition);

        // Handle middle section
        var previous = _body.Last;
        var current = _body.Last.Previous;
        while (current != _body.First)
        {
            var next = current.Previous;
            tile = _board[current.Value];
            if (previous.Value.x == next.Value.x)
            {
                if (_bulges.Contains(current.Value))
                {
                    tile.Content = TileContent.SnakeBody;
                }
                else
                {
                    tile.Content = TileContent.SnakeBody;
                }
                tile.ZRotation = 0;
            }
            else if (previous.Value.y == next.Value.y)
            {
                if (_bulges.Contains(current.Value))
                {
                    tile.Content = TileContent.SnakeBody;
                }
                else
                {
                    tile.Content = TileContent.SnakeBody;
                }
                tile.ZRotation = 90;
            }
            else
            {
                if (_bulges.Contains(current.Value))
                {
                    tile.Content = TileContent.SnakeBody;
                }
                else
                {
                    tile.Content = TileContent.SnakeBody;
                }
                if ((previous.Value.x > current.Value.x && next.Value.y < current.Value.y) || (next.Value.x > current.Value.x && previous.Value.y < current.Value.y))
                {
                    tile.ZRotation = 0;
                }
                else if ((previous.Value.x < current.Value.x && next.Value.y < current.Value.y) || (next.Value.x < current.Value.x && previous.Value.y < current.Value.y))
                {
                    tile.ZRotation = 90;
                }
                else if ((previous.Value.x < current.Value.x && next.Value.y > current.Value.y) || (next.Value.x < current.Value.x && previous.Value.y > current.Value.y))
                {
                    tile.ZRotation = 180;
                }
                else if ((previous.Value.x > current.Value.x && next.Value.y > current.Value.y) || (next.Value.x > current.Value.x && previous.Value.y > current.Value.y))
                {
                    tile.ZRotation = 270;
                }
                else
                {
                    tile.Content = TileContent.SnakeHead;
                }
            }
            previous = current;
            current = current.Previous;
        }

        // Handle tail
        var tailPosition = _body.First.Value;
        var previousPosition = _body.First.Next.Value;

        tile = _board[tailPosition];
        tile.Content = TileContent.SnakeBody;

        AssignRotation(tile, previousPosition, tailPosition);
    }

    void AssignRotation(Tile tile, Vector2Int previousPosition, Vector2Int nextPosition)
    {
        if (previousPosition.y > nextPosition.y)
        {
            tile.ZRotation = 0;
        }
        else if (previousPosition.y < nextPosition.y)
        {
            tile.ZRotation = 180;
        }
        else if (previousPosition.x > nextPosition.x)
        {
            tile.ZRotation = 90;
        }
        else if (previousPosition.x < nextPosition.x)
        {
            tile.ZRotation = -90;
        }
    }
    
    public Vector2Int NextHeadPosition(Vector2Int direction)
    {
        return Head + new Vector2Int(direction.x, -direction.y);
    }

    public IEnumerator<Vector2Int> GetEnumerator()
    {
        return ((IEnumerable<Vector2Int>)_body).GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return ((IEnumerable<Vector2Int>)_body).GetEnumerator();
    }
}
