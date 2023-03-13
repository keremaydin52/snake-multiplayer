using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class TrackManager : Singleton<TrackManager>
{
    [Range(0f, 3f)]
    public float gameSpeed;
    public Board board;

    [SerializeField] private List<Snake> snakes;

    private float _time;
    private Vector2Int _applePosition;
    private Vector2Int _bonusPosition;
    private bool _isMoving;

    void Update()
    {
        if(!_isMoving) return;
        
        _time += Time.deltaTime;
        while (_time > gameSpeed)
        {
            _time -= gameSpeed;
            foreach (var snake in snakes)
            {
                UpdateGameState(snake);
            }
        }
    }

    public void Begin()
    {
        foreach (var snake in snakes)
        {
            snake.inputController.Reset();
            snake.Reset();
        }

        board.Reset();
        PlantAnApple();
        
        _time = 0;
        _isMoving = true;
    }

    void Stop()
    {
        _isMoving = false;
        GameManager.Instance.SwitchState("GameOver");
    }

    private void UpdateGameState(Snake snake)
    {
        if (snake != null)
        {
            var dir = snake.inputController.NextDirection();

            // New head position
            var head = snake.NextHeadPosition(dir);

            var x = head.x;
            var y = head.y;

            if (snake.WithoutTail.Contains(head))
            {
                Stop();
                return;
            }

            if (x >= 0 && x < board.columns && y >= 0 && y < board.rows)
            {
                if (head == _applePosition)
                {
                    snake.Move(dir, true);
                    PlantAnApple();
                }
                else
                {
                    snake.Move(dir, false);
                }
            }
            else
            {
                Stop();
            }
        }
    }

    private void PlantAnApple()
    {
        if (board[_applePosition].Content == TileContent.Apple)
        {
            board[_applePosition].Content = TileContent.Empty;
        }

        var emptyPositions = board.EmptyPositions.ToList();
        if (emptyPositions.Count == 0)
        {
            return;
        }
        _applePosition = emptyPositions.RandomElement();
        board[_applePosition].Content = TileContent.Apple;
    }
}
