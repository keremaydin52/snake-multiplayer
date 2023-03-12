using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Snake : IEnumerable<Vector2Int>
{
    private LinkedList<Vector2Int> _body;
    private HashSet<Vector2Int> _bulges;
    private Board _board;

    public Vector2Int Head => _body.Last.Value;

    public IEnumerable<Vector2Int> WithoutTail
    {
        get
        {
            return this.Where((p) => { return p != _body.First.Value; });
        }
    }

    public Snake(Board board)
    {
        _board = board;
        _body = new LinkedList<Vector2Int>();
        _bulges = new HashSet<Vector2Int>();
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

        if (nextPosition.y > headPosition.y)
        {
            tile.ZRotation = 0;
        }
        else if (nextPosition.y < headPosition.y)
        {
            tile.ZRotation = 180;
        }
        else if (nextPosition.x > headPosition.x)
        {
            tile.ZRotation = 90;
        }
        else if (nextPosition.x < headPosition.x)
        {
            tile.ZRotation = -90;
        }

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
                    //tile.Content = TileContent.SnakeBulge;
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
                    //tile.Content = TileContent.SnakeBulge;
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
                    //tile.Content = TileContent.SnakesLBulged;
                    tile.Content = TileContent.SnakeBody;
                }
                else
                {
                    //tile.Content = TileContent.SnakesL;
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
        //tile.Content = TileContent.SnakeTail;
        tile.Content = TileContent.SnakeBody;

        if (previousPosition.y > tailPosition.y)
        {
            tile.ZRotation = 0;
        }
        else if (previousPosition.y < tailPosition.y)
        {
            tile.ZRotation = 180;
        }
        else if (previousPosition.x > tailPosition.x)
        {
            tile.ZRotation = 90;
        }
        else if (previousPosition.x < tailPosition.x)
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
