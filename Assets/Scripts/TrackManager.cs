using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class TrackManager : Singleton<TrackManager>
{
    // Variable used for game update delay calculations
    private float _time;
    private Snake _snake;
    private Vector2Int _applePosition;
    private Vector2Int _bonusPosition;
    private InputController _inputController;
    private bool _isMoving;

    [Range(0f, 3f)]
    public float gameSpeed;

    public Board board;

    void Update()
    {
        if(!_isMoving) return;
        
        _time += Time.deltaTime;
        while (_time > gameSpeed)
        {
            _time -= gameSpeed;
            UpdateGameState();
        }
    }

    public void Begin()
    {
        _inputController = GetComponent<InputController>();
        _inputController.Reset();
        
        _snake = new Snake(board);
        _snake.Reset();

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

    private void UpdateGameState()
    {
        if (_snake != null)
        {
            var dir = _inputController.NextDirection();

            // New head position
            var head = _snake.NextHeadPosition(dir);

            var x = head.x;
            var y = head.y;

            if (_snake.WithoutTail.Contains(head))
            {
                Stop();
                return;
            }

            if (x >= 0 && x < board.columns && y >= 0 && y < board.rows)
            {
                if (head == _applePosition)
                {
                    _snake.Move(dir, true);
                    PlantAnApple();
                }
                else
                {
                    _snake.Move(dir, false);
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
